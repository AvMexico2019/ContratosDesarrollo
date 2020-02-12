var MapComponent = null;
var tDeactivate = null;
var tAddPoint = null;
var tAddLine = null;
var tAddPolygon = null;
var tDeleteDraw = null;
var GeoComponent = null;
var geoSearch = null;
var collectionEstadosPolygons = [];
var polygonsMuns = {};
var wktWGS84 = '';
var componentOptions = null;
var GEOCOMPONENT_IMAGES_URL = '';
var GEOCOMPONENT_RESOURCES_URL = '';
var ggsSingleton = null;
var tipoMapaActual = null;
var poligonoGoogle = null;
var JSONLocation = {
    IdEdo: null,
    IdMun: null,
    IdLoc: null,
    IdCol: null,
    IdCP: null
};

function setupComponent(options) {

    componentOptions = options;
    
    $('#' + options.idContainer).empty();
    $('#' + options.idContainer).append('<div id="mainToolBar" style="position: absolute; z-index: 1;"></div>'); // div mainToolBar
    
	
	if (componentOptions.google){
		$('#' + options.idContainer).append('<div id="_map_"  class="fill"></div>'); // div container for Map
		$('#' + options.idContainer).append('<div id="_gmap_" class="fill" ></div>'); // div container for Google Map
	}else{
		$('#' + options.idContainer).append('<div id="_map_"  style="width:99%"></div>'); // div container for Map
	}
		
		
	
    $('#' + options.idContainer).append('<div id="messages" style="margin-left:10px; margin-top:-30px; position:relative;"></div>'); // div container for Map

    // Add elements to mainToolBar
    $('#mainToolBar').append('<div id="baseMap" style="position:  absolute; z-index: 1; margin-left: 50px; top: 1px;"></div>'); // div baseMap
    $('#mainToolBar').append('<div id="containerCurrentZoom" style="position: static; width: 119px; top:10px"></div>'); // div containerCurrentZoom

    if (componentOptions.search)
        $('#mainToolBar').append('<div id="containerGeoSearch" style="position:absolute; top:65px;"></div>'); // div containerGeoSearch

    $('#mainToolBar').append('<div id="toolBar" style="position: relative; width:150px; z-index: 1; top: 18px;"></div>'); // div toolBar

    //Add elements for each toolBar

    //Add elements to baseMap
    $('#baseMap').append('<select id="toolBaseMap" title="Seleccione para cambiar el mapa base" onchange="changeBaseMap(this.value);"></select>'); // select toolBaseMap
    $('#toolBaseMap').append('<option value="OpenStreetMap">OpenStreetMap</option>');
    //$('#toolBaseMap').append('<option value="MapQuest">MapQuest</option>');
    $('#toolBaseMap').append('<option value="Bing Maps Road">Bing Maps Road</option>');
	$('#toolBaseMap').append('<option selected="selected" value="Bing Maps Satelital">Bing Maps Satelital</option>');
	////
	if (componentOptions.google)
		$('#toolBaseMap').append('<option value="Google Map">Google Map</option>');

    //Add elements to containerCurrentZoom    
    $('#containerCurrentZoom').append('<div><table id="tblCZ"><tr><td><label>Zoom actual: </label></td><td><input id="currentZoom" type="text" style="width: 20px; height: 10px;" readonly /></td></tr></table></div>');

    //Add elements to containerGeoSearch
    $('#containerGeoSearch').append('<input id="txtGeoSearch" value="" placeholder="Escriba una palabra" />');

    //Add elements to toolBar
	if (GEOCOMPONENT_IMAGES_URL) {
		$('#toolBar').append('<a id="tPaneo" href="javascript:void(0);"><img id="toolDeactivate" title="Desactivar herramientas" src="' + GEOCOMPONENT_IMAGES_URL + 'images/paneo_i.png" /></a><a id="tPoint" href="javascript:void(0);"><img id="toolAddPoint" title="Agregar punto" src="' + GEOCOMPONENT_IMAGES_URL + 'images/agregar_punto_i.png" /></a><a id="tLine" href="javascript:void(0);"><img id="toolAddLine" title="Agregar línea" src="' + GEOCOMPONENT_IMAGES_URL + 'images/linea_buffer_i.png" /></a><a id="tPolygon" href="javascript:void(0);"><img id="toolAddPolygon" title="Agregar polígono" src="' + GEOCOMPONENT_IMAGES_URL + 'images/poligono_i.png" /></a><a id="tErase" href="javascript:void(0);"><img id="toolDeleteDraw" title="Borrar trazo" src="' + GEOCOMPONENT_IMAGES_URL + 'images/borrar_i.png" /></a>');
	}

    var pixels = 0;
    var containerWidth = $('#' + options.idContainer).width();

    pixels = 150;
    $("#containerCurrentZoom").css("margin-left", pixels + "px");

    pixels = ($('#' + options.idContainer).width() - 150) - 5;
    $("#toolBar").css("margin-left", pixels + "px");

    pixels = ($('#' + options.idContainer).width() - 173) - 5;
    $("#containerGeoSearch").css("margin-left", pixels + "px");

	if (componentOptions.google){
		MapComponent = new GeoGoogleMapComponent('_map_', 19.546195, -99.035104, 5, '_gmap_');		
	}else {
		MapComponent = new GeoMapComponent('_map_', -99.035104, 19.546195, 5);
	}	
    
    var map = MapComponent.getMap();

    if (componentOptions.search) {
        geoSearch = new GeoSearch('txtGeoSearch', MapComponent);
        geoSearch.Init();
    }

     $('#_map_').css({
			height: $('#' + options.idContainer).height(),
			width: $('#' + options.idContainer).width(),
		}); 
		
	if (componentOptions.google){
		$('#_gmap_').css({
			height: $('#' + options.idContainer).height(),
			width: $('#' + options.idContainer).width(),
		});				
	}else{		 
		map.updateSize();
	}    

    map.on('moveend', onMoveEnd);
    
    MapComponent.changeBaseMap('Bing Maps Satelital');

    tipoMapaActual = 'Bing Maps Satelital';
	
    GeoComponent = new GenericGeometryActions(MapComponent, 'GeometryEdit');

    var valueEditable = $('#' + options.serverInputsId.idInputEditable).val();    

    if (valueEditable == 'true' || valueEditable == 'false') {

        var JSONGeometry = {
            IdEdo: $('#' + options.serverInputsId.idInputEdo).val(),
            IdMun: $('#' + options.serverInputsId.idInputMun).val(),
            TipoGeom: $('#' + options.serverInputsId.idInputGeometryType).val(),
            WKT: $('#' + options.serverInputsId.idInputWKT).val(),
            X: $('#' + options.serverInputsId.idInputX).val(),
            Y: $('#' + options.serverInputsId.idInputY).val(),
            Edit: valueEditable == 'true' ? true : false
        };


        var statusComponent = setGeometry(JSONGeometry, callbackJSONRequest);	

    } else {

        toolSettings(false, '');

        statusComponent = {
            Code: '200',
            Msg: 'El valor editar no contiene un valor boleano.',
            WKT: '',
            X: '',
            Y: ''
        };

        $('#' + componentOptions.messagesInputsId.idInputCode).val(statusComponent.Code);
        $('#' + componentOptions.messagesInputsId.idInputMessage).val(statusComponent.Msg);
    }

    return statusComponent;
}

function setGeometry(JSONGeometry, callbackJSONRequest) {

    var JSONResult = GeoComponent.setGeometry(JSONGeometry);
    if (MapComponent.clearStreetView) {
        MapComponent.clearStreetView();
    }
    $('#' + componentOptions.messagesInputsId.idInputCode).val(JSONResult.Code);
    $('#' + componentOptions.messagesInputsId.idInputMessage).val(JSONResult.Msg);

    return JSONResult;
}

function getGeometry() {    

    var JSONResult = GeoComponent.getGeometryData();

    $('#' + componentOptions.serverInputsId.idInputGeometryType).val(JSONResult.GeometryType).trigger('change');
    $('#' + componentOptions.serverInputsId.idInputWKT).val(JSONResult.WKT).trigger('change');
    $('#' + componentOptions.serverInputsId.idInputX).val(JSONResult.X).trigger('change');
    $('#' + componentOptions.serverInputsId.idInputY).val(JSONResult.Y).trigger('change');

    $('#' + componentOptions.messagesInputsId.idInputCode).val(JSONResult.Code);
    $('#' + componentOptions.messagesInputsId.idInputMessage).val(JSONResult.Msg);
   

}

function setLocationCenter(scope, ParamsLocation, closeup) {
	
    JSONLocation = ParamsLocation;

    var idScope = "", x = '', y = '', zoom = 0;

    switch (scope) {

        case "CP":
            idScope = JSONLocation.IdCP;
            zoom = 14;
            break;
        case "Localidad":
            idScope = JSONLocation.IdLoc;
            zoom = 13;
            break;
        case "Municipio":
            idScope = JSONLocation.IdMun;
            zoom = 12;
            break;
        case "Estado":
            idScope = JSONLocation.IdEdo;
            zoom = 8;
            break;
    }

    var params = {
        'method': 'GET_SCOPE_GEOMETRY',
        'scope': scope,
        'idScope': idScope
    };
    var founded = false;

	$.post('../ashx/UtilsGeo.ashx?', params, function (data) {

	    if (data.Code == 100) {

	        if (data.Obj != null) {

	            var geometry = data.Obj;
	            var feature = ggsSingleton.olWKTParser.readFeature(geometry.WKT);

	            feature.getGeometry().transform('EPSG:4326', 'EPSG:3857');
	            ggsSingleton.sourceScope.clear(true);
	            ggsSingleton.sourceScope.addFeature(feature);

	            ggsSingleton.vectorLayerScope.setStyle(ggsSingleton.styleLayerScope);
	            ggsSingleton.geMap.getMap().render();
	            ggsSingleton.geMap.getMap().renderSync();

	            if(closeup)
					MapComponent.setCenterZoom(geometry.X, geometry.Y, zoom, 'EPSG:4326');

	            if (scope == "Estado") {
	                GeoComponent.JSONGeometryObject.IdEdo = idScope;
	                $('#' + componentOptions.serverInputsId.idInputEdo).val(idScope);
	            } else if (scope == "Municipio") {
	                GeoComponent.JSONGeometryObject.IdEdo = idScope.substring(0, 2);
	                GeoComponent.JSONGeometryObject.IdMun = idScope;
	                $('#' + componentOptions.serverInputsId.idInputEdo).val(idScope.substring(0, 2));
	                $('#' + componentOptions.serverInputsId.idInputMun).val(idScope);
	            }
	            else if (scope != "Municipio" && scope != "Estado") {
	                GeoComponent.JSONGeometryObject.IdEdo = geometry.IdEdo;
	                GeoComponent.JSONGeometryObject.IdMun = geometry.IdMun;
	                $('#' + componentOptions.serverInputsId.idInputEdo).val(geometry.IdEdo);
	                $('#' + componentOptions.serverInputsId.idInputMun).val(geometry.IdMun);
	            }
	        }
	        else {
	            if (scope == "CP")
	                setLocationCenter("Localidad", JSONLocation, closeup);
                else if(scope == "Localidad")
                    setLocationCenter("Municipio", JSONLocation, closeup);
                else if(scope == "Municipio")
                    setLocationCenter("Estado", JSONLocation, closeup);
	        }


	    }
	    else {
	        //Notifier.warning("messages", data.Msg);
	    }
	}, 'json');   
}


function changeBaseMap(baseMap) {
    MapComponent.changeBaseMap(baseMap);
    tipoMapaActual = baseMap;
}

function onMoveEnd() {

    var map = MapComponent.getMap();
    var currentZoom = map.getView().getZoom();

    $('#currentZoom').val(currentZoom);

    if (currentZoom < 14) {

        if (GeoComponent.hasGeometry()) {

            if (tAddPoint != null) {
                if (tAddPoint.getStatus() == 'on') {
                    GeoComponent.clear();
                    GeoComponent.deactivate();
                    tAddPoint.off();
                }
            }


            if (tAddLine != null) {
                if (tAddLine.getStatus() == 'on') {
                    GeoComponent.clear();
                    GeoComponent.deactivate();
                    tAddLine.off();
                }
            }

            if (tAddPolygon != null) {
                if (tAddPolygon.getStatus() == 'on') {
                    GeoComponent.clear();
                    GeoComponent.deactivate();
                    tAddPolygon.off();
                }
            }
        } else {

            if (tAddPoint != null) {
                if (tAddPoint.getStatus() == 'on') {                    
                    GeoComponent.deactivate();
                    tAddPoint.off();
                }
            }


            if (tAddLine != null) {
                if (tAddLine.getStatus() == 'on') {                    
                    GeoComponent.deactivate();
                    tAddLine.off();
                }
            }

            if (tAddPolygon != null) {
                if (tAddPolygon.getStatus() == 'on') {                    
                    GeoComponent.deactivate();
                    tAddPolygon.off();
                }
            }
        }
    }
}

function toolDeactivate_click() {    

    if (tDeactivate.getStatus() == 'off') {
        tDeactivate.on();        

        if (tAddPoint != null) {
            GeoComponent.deactivate();
            tAddPoint.off();
        }
            

        if (tAddLine != null) {
            GeoComponent.deactivate();
            tAddLine.off();
        }            

        if (tAddPolygon != null) {
            GeoComponent.deactivate();
            tAddPolygon.off();
        }
            

        if (tDeleteDraw != null)
            tDeleteDraw.off();        

    }
    else {
        tDeactivate.off();
    }    
}

function toolAddPoint_click() {

    if (tAddPoint.getStatus() == 'off') {

        var map = MapComponent.getMap();
        var currentZoom = map.getView().getZoom();

        if (currentZoom >= 14) {

            tAddPoint.on();

            if (tAddLine != null) {
                GeoComponent.deactivate();                
                tAddLine.off();
            }
                

            if (tAddPolygon != null) {
                GeoComponent.deactivate();                
                tAddPolygon.off();
            }                

            GeoComponent.activate('Point');

            if (tDeleteDraw != null)
                tDeleteDraw.off();
        } else {
            Messages.info('messages', 'El nivel de Zoom para Editar/Trazar debe ser 14.', 5500);
        }
    }
    else {
        GeoComponent.deactivate();
        tAddPoint.off();
    }
}

function toolAddLine_click() {

    if (tAddLine.getStatus() == 'off') {

        //var map = MapComponent.getMap();
        //var currentZoom = map.getView().getZoom();

        //if (currentZoom >= 14) {

            tAddLine.on();            

            if (tAddPoint != null) {
                GeoComponent.deactivate();                
                tAddPoint.off();
            }                

            if (tAddPolygon != null) {
                GeoComponent.deactivate();                
                tAddPolygon.off();
            }

            GeoComponent.activate('LineString');

            if (tDeleteDraw != null)
                tDeleteDraw.off();
        /*} else {
            Messages.info('messages', 'El nivel de Zoom requerido es 14 o mayor.', 5500);
        }*/
    }
    else {
        GeoComponent.deactivate();
        tAddLine.off();
    }
}

function toolAddPolygon_click() {

    if (tAddPolygon.getStatus() == 'off') {

        var map = MapComponent.getMap();
        var currentZoom = map.getView().getZoom();

        if (currentZoom >= 14) {
            tAddPolygon.on();            

            if (tAddPoint != null) {				
                GeoComponent.deactivate();                
                tAddPoint.off();
            }                

            if (tAddLine != null) {
                GeoComponent.deactivate();                
                tAddLine.off();
            }                
			
			

            GeoComponent.activate('Polygon');
			

            if (tDeleteDraw != null)
                tDeleteDraw.off();

        } else {
            Messages.info('messages', 'El nivel de Zoom para Editar/Trazar debe ser 14.', 5500);
        }

    }
    else {
        GeoComponent.deactivate();
        tAddPolygon.off();
    }
}

function toolDeleteDraw_click() {

    if (tDeleteDraw.getStatus() == 'off') {
        tDeleteDraw.on();
        GeoComponent.clear();

        if (tAddPoint != null) {
            GeoComponent.deactivate();
            tAddPoint.off();
        }

        if (tAddLine != null) {
            GeoComponent.deactivate();
            tAddLine.off();
        }

        if (tAddPolygon != null) {
            GeoComponent.deactivate();
            tAddPolygon.off();
        }
    }
    else {
        tDeleteDraw.off();
    }
}

function callbackJSONRequest(JSONResult) {

    alert('Code: ' + JSONResult.Code);
    alert('Msg: ' + JSONResult.Msg);
    alert('WKT: ' + JSONResult.WKT);
    alert('X: ' + JSONResult.X);
    alert('Y: ' + JSONResult.Y);
}

/*
Codigos de Respuesta:
Codigo: 100 Mensaje: El componente se cargó correctamente
Codigo: 101 Mensaje: Se obtuvo la geometria Correctamente

Codigo: 200 Mensaje: Es necesario la Clave del Estado y opcionalmente la Clave del Municipio
Codigo: 201 Mensaje: Es necesario indicar los permisos de Edición. 
Codigo: 202 Mensaje: Es necesario indicar el tipo de Geometria (Polygon, LineString, Point).
Codigo: 203 Mensaje: Se trazó la geometría fuera del Ambito geográfico permitido
Codigo: 203 Mensaje: Se trazó la geometría fuera del Ambito geográfico permitido
Codigo: 203 Mensaje: Se trazó la geometría fuera del Ambito geográfico permitido
Codigo: 203 Mensaje: Se trazó la geometría fuera del Ambito geográfico permitido

Codigo: 300 Mensajes de error al convertir WKT o X Y

*/



var type = '';
var wkt = '';
var featurePoint = null;

function GeoMapComponent(idContainer, latitude, longitude, zoom) {

    if (!(this instanceof GeoMapComponent)) {
        throw new TypeError("GeoComponentMap constructor cannot be called as a function.");
    }

    this.map = null;
    this.initialView = null;
    this.actualProjection = 'EPSG:3857';
    this.currentBaseMap = null;
    this.baseMaps = null;


    this.initialView = new ol.View({ center: ol.proj.transform([latitude, longitude], 'EPSG:4326', 'EPSG:3857'), projection: this.actualProjection, zoom: zoom });
    this.currentBaseMap = new ol.layer.Tile({ visible: true, source: null });
    this.baseMaps = [];

    var mapquest = {
        title: 'MapQuest',
        type: 'otherSource',
        source: new ol.source.MapQuest({ layer: 'sat' })
    };

    var bing = {
        title: 'Bing Maps Road',
        type: 'otherSource',
        source: new ol.source.BingMaps({
            key: 'AlgNo85CUM4wUdugkITU3QgkWl-6enJb-NQ8UHhTrgKl6-JLlG1TrlX2vqkb8NgX',
            imagerySet: 'Road'
        })
    };

    var osm = {
        title: 'OpenStreetMap',
        type: 'otherSource',
        source: new ol.source.OSM()
    };
	
	var bing2 = {
        title: 'Bing Maps Satelital',
        type: 'otherSource',
        source: new ol.source.BingMaps({
            key: 'AlgNo85CUM4wUdugkITU3QgkWl-6enJb-NQ8UHhTrgKl6-JLlG1TrlX2vqkb8NgX',
            imagerySet: 'AerialWithLabels'
        })
    };	
	
    var attr = ol.control.defaults({ attribution: false });

    this.baseMaps.push(mapquest);
    this.baseMaps.push(bing);
    this.baseMaps.push(osm);
	this.baseMaps.push(bing2);

    this.map = new ol.Map({
        target: idContainer,
        layers: [this.currentBaseMap],
        view: this.initialView,
        controls: attr
    });

    this.setCenterZoom = function (x, y, pzoom, proj) {
        var xy = ol.proj.transform([Number(x), Number(y)], proj, this.actualProjection);
        this.initialView.setCenter(xy);
        this.initialView.setZoom(pzoom);
    };

    this.changeBaseMap = function (basemapTitle) {
        var basemap = null;

        if (basemapTitle == null) {
            this.currentBaseMap.setSource(null);
        }
        else {
            for (var i = 0; i < this.baseMaps.length; i++) {
                if (this.baseMaps[i].title == basemapTitle) {
                    basemap = this.baseMaps[i];
                    break;
                }
            }
            this.currentBaseMap.setSource(basemap.source);
        }
    };

    this.getMap = function () {
        return this.map;
    };

    this.getCurrentProjection = function () {
        return this.actualProjection;
    };

}

function GeoGoogleMapComponent(idContainer, latitude, longitude, zoom, idContainerGM) {

    if (!(this instanceof GeoGoogleMapComponent)) {
        throw new TypeError("GeoGoogleMapComponent constructor cannot be called as a function.");
    }

    this.map = null;
    this.initialView = null;
    this.actualProjection = 'EPSG:3857';
    this.currentBaseMap = null;
    this.baseMaps = null;
	this.baseMapsLayers= null;
	this.gmap_ = null;
    //this.

	var gmap = new google.maps.Map(document.getElementById(idContainerGM), {
	  disableDefaultUI: true,
	  keyboardShortcuts: false,
	  draggable: false,
	  disableDoubleClickZoom: true,
	  scrollwheel: false,
	  mapTypeId: google.maps.MapTypeId.HYBRID,
	  streetViewControl: false,
      tilt:0

	});
	
	this.gmap_ = gmap;

	var viewNow = new ol.View({ center: ol.proj.transform([0, 0], 'EPSG:4326', 'EPSG:3857'), projection: this.actualProjection, zoom: 1, maxZoom:20 });

	//Son importantes para actualizar la ubicación de openlayers a googlemaps
	viewNow.on('change:center', function() {	
	  var center = ol.proj.transform(viewNow.getCenter(), 'EPSG:3857', 'EPSG:4326');
	  gmap.setCenter(new google.maps.LatLng(center[1], center[0]));
	});
	viewNow.on('change:resolution', function() {	
	  gmap.setZoom(viewNow.getZoom());
	});

	this.initialView= viewNow;
	this.baseMaps = [];
	
	this.baseMaps.push(new ol.layer.Tile({
		   title: 'OpenStreetMap',
		   type: 'otherSource',
		   source: new ol.source.OSM(),
		   visible:true
		}));	  
	this.baseMaps.push(new ol.layer.Tile({		  
		title: 'MapQuest',
        type: 'otherSource',
        source: new ol.source.MapQuest({ layer: 'sat' }),
          visible: false,          
        }));
		
	this.baseMaps.push(new ol.layer.Tile({	
		title: 'Bing Maps Road',
        type: 'otherSource',
        source: new ol.source.BingMaps({
            key: 'AlgNo85CUM4wUdugkITU3QgkWl-6enJb-NQ8UHhTrgKl6-JLlG1TrlX2vqkb8NgX',
            imagerySet: 'Road'
        }),
          visible: false
          
        }));
	this.baseMaps.push(new ol.layer.Tile({	
		title: 'Bing Maps Satelital',
        type: 'otherSource',
        source: new ol.source.BingMaps({
            key: 'AlgNo85CUM4wUdugkITU3QgkWl-6enJb-NQ8UHhTrgKl6-JLlG1TrlX2vqkb8NgX',
            imagerySet: 'AerialWithLabels'
        }),
		visible:false
        }));

	//Control de mapa de openlayers
	var olMapDiv = document.getElementById(idContainer);
	
	this.map = new ol.Map({
	  layers: this.baseMaps,
	  interactions: ol.interaction.defaults({
		altShiftDragRotate: false,
		dragPan: false,
		rotate: false
	  }).extend([new ol.interaction.DragPan({kinetic: null})]),
	  target: olMapDiv,
	  view: this.initialView
	});
	
	var centera = ol.proj.transform([longitude, latitude], 'EPSG:4326', 'EPSG:3857')
	this.initialView.setCenter(centera);
	this.initialView.setZoom(zoom);

	//agregar el div de openlayers a googlemaps
	olMapDiv.parentNode.removeChild(olMapDiv);
	gmap.controls[google.maps.ControlPosition.TOP_LEFT].push(olMapDiv);

	 this.changeBaseMap = function (basemapTitle) {
	 
		if (basemapTitle =='Google Map'){
			//Apagar todas las capas para dejar visible google maps
			for (var i = 0, ii = this.baseMaps.length; i < ii; ++i) {
			  this.baseMaps[i].setVisible(false);
			}
			//activar boton de streetView
			gmap.setOptions({ streetViewControl: true });
		    //Activar poligono si existe
			if (poligonoGoogle) {
			    poligonoGoogle.setMap(this.gmap_);
			}
		    //Se oculta el poligono para el mapa de openlayers
			if (GeoComponent) {
			    if (GeoComponent.vectorLayer) {
			        GeoComponent.vectorLayer.setVisible(false);
			    }
			}
		}else {
			//Desactivar boton streetView
			gmap.setOptions({streetViewControl:false});
			//Apagar vista de streetview
			gmap.getStreetView().setVisible(false)
		    //Se oculta el poligono de google si existe.
			if (poligonoGoogle) {
			    poligonoGoogle.setMap(null);
			}
		    //Se activa el poligono para estas capas
			if (GeoComponent) {
			    if (GeoComponent.vectorLayer) {
			        GeoComponent.vectorLayer.setVisible(true);
			    }
			}
			//Activar capa seleccionada
			for (var i = 0, ii = this.baseMaps.length; i < ii; ++i) {
				this.baseMaps[i].setVisible(this.baseMaps[i].get('title') === basemapTitle)
			}
		    
		}
	 }

    this.getMap = function () {
        return this.map;
    };

    this.getCurrentProjection = function () {
        return this.actualProjection;
    };

    this.clearStreetView = function () {
        gmap.getStreetView().setVisible(false);
    }

}


function ActionButton(options) {

    if (!(this instanceof ActionButton)) {
        throw new TypeError("ActionButton constructor cannot be called as a function.");
    }

    var me = this;
    this.id = options.id;
    this.imgNormal = options.imgNormal;
    this.imgActive = options.imgActive;
    this.imgInactive = options.imgInactive;
    this.imgOver = options.imgOver;
    this.status = 'regular';
    this.click = options.onClick != undefined ? options.onClick : null;

    $("#" + this.id).mouseover(function () {
        $("#" + me.id).attr("src", me.imgOver);
    });

    $("#" + this.id).mouseout(function () {
        if (me.status == 'regular') {
            $("#" + me.id).attr("src", me.imgNormal);
        } else if (me.status == 'on') {
            $("#" + me.id).attr("src", me.imgActive);
        } else if (me.status == 'off') {
            $("#" + me.id).attr("src", me.imgInactive);
        }
    });

    $("#" + this.id).click(function () {
        if (me.click != null)
            me.click(me);
    });

    this.on = function () {
        $("#" + this.id).hide().attr("src", this.imgActive).show();
        this.status = 'on';
    };

    this.off = function () {
        $("#" + this.id).attr("src", this.imgInactive).show();
        this.status = 'off';
    };

    this.regular = function () {
        $("#" + this.id).attr("src", this.imgNormal).show();
        this.status = 'regular';
    };

    this.getStatus = function () {
        return this.status;
    };


    this.getId = function () {
        return this.id;
    };

}


// inicia clase EditGenericGeometry



function GenericGeometryActions(_geMap, vectorLayerName) {

    if (!(this instanceof GenericGeometryActions)) {
        throw new TypeError("GenericGeometryActions constructor cannot be called as a function.");
    }

    this.active = false;
    this.addFeatureActive = true;
    this.olWKTParser = new ol.format.WKT();
    this.ftrGCCurrent = null;
    this.ftrGCTemp = null;
    this.source = null;
    this.vectorLayer = null;
    this.sourcePoint = null;
    this.vectorLayerPoint = null;
	this.sourceScope = null;
	this.vectorLayerScope = null;
    this.styleLayer = null;
    this.styleLayerEdit = null;
    this.draw = null;
    this.typeEdit = 'Point';
    this.geMap = _geMap;
    this.evtFeature = null;
    this.JSONGeometryObject = null;
    this.JSONResult = null;
    this.select = null;
    this.modify = null;
    ggsSingleton = this;
    this.ftrGCCurrentGoogle = null;

    var mapy = this.geMap.getMap();

    this.styleLayer = new ol.style.Style({
        fill: new ol.style.Fill({
            color: 'rgba(248, 246, 243, 0.5)'
        }),
        stroke: new ol.style.Stroke({
            color: '#f20c1f',
            width: 3
        }),
        image: new ol.style.Circle({
            radius: 7,
            fill: new ol.style.Fill({
                color: '#ffcc33'
            })
        })
    });

    this.styleMarker = new ol.style.Style({

        image: new ol.style.Icon(({
            anchor: [0.5, 46],
            anchorXUnits: 'fraction',
            anchorYUnits: 'pixels',
            opacity: 0.75,
            src: GEOCOMPONENT_IMAGES_URL + 'images/map-pin.png'
        }))
    });

    this.styleLayerEdit = new ol.style.Style({
        fill: new ol.style.Fill({
            color: 'rgba(255,255,255,0.4)'
        }),
        stroke: new ol.style.Stroke({
            color: '#3399CC',
            width: 1.5
        }),
        image: new ol.style.Circle({
            radius: 5,
            stroke: new ol.style.Stroke({
                color: '#3399CC',
                width: 1.5
            }),
            fill: new ol.style.Fill({
                color: '#rgba(255,255,255,0.4)'
            })
        })
    });
	
	this.styleLayerScope = new ol.style.Style({
		fill: new ol.style.Fill({
			color: 'rgba(255, 255, 255, 0)'
		}),
		stroke: new ol.style.Stroke({
			color: '#5ce552',
			width: 2
		}),
		image: new ol.style.Circle({
			radius: 7,
			fill: new ol.style.Fill({
				color: '#16930d'
			})
		})
    });

    this.source = new ol.source.Vector({ wrapX: false });
    this.vectorLayer = new ol.layer.Vector({
        source: this.source,
        style: this.styleLayer
    });

    this.vectorLayer.nameLayer = this.vectorLayerName;

    mapy.addLayer(this.vectorLayer);

    this.sourcePoint = new ol.source.Vector({ wrapX: false });

    this.vectorLayerPoint = new ol.layer.Vector({
        source: this.sourcePoint,
        style: this.styleLayer
    });

    this.vectorLayerPoint.nameLayer = 'LayerMarkerCentroid';
    mapy.addLayer(this.vectorLayerPoint);
	
	this.sourceScope = new ol.source.Vector({ wrapX: false });
	
	this.vectorLayerScope = new ol.layer.Vector({
        source: this.sourceScope,
        style: this.styleLayer
    });
	
	this.vectorLayerScope.nameLayer = 'LayerGeometryScope';
    mapy.addLayer(this.vectorLayerScope);

    this.source.on('addFeature', function (evt) {

        if (ggsSingleton.addFeatureActive == false) {
            return;
        }

        ggsSingleton.evtFeature = evt.feature;

        if (ggsSingleton.ftrGCTemp == null) {
            ggsSingleton.ftrGCTemp = ggsSingleton.evtFeature;
            ggsSingleton.ftrGCCurrent = ggsSingleton.evtFeature.clone();
        } else {
            ggsSingleton.source.removeFeature(ggsSingleton.ftrGCTemp);
            ggsSingleton.ftrGCCurrent = null;
            ggsSingleton.ftrGCTemp = ggsSingleton.evtFeature;
            ggsSingleton.ftrGCCurrent = ggsSingleton.evtFeature.clone();
        }
    }, this);

    this.getVectorLayer = function () {
        return this.vectorLayer;
    };

    this.setStyle = function (st) {

        this.styleLayer = st;
        this.vectorLayer.setStyle(this.styleLayer);
        this.geMap.getMap().render();
        this.geMap.getMap().renderSync();
    };

    this.setStyleEdit = function (st) {

        this.styleLayerEdit = st;
        this.vectorLayer.setStyle(this.styleLayerEdit);
        this.geMap.getMap().render();
        this.geMap.getMap().renderSync();
    };

    this.setGeometryInitType = function (_type, active) {
        var mapy = this.geMap.getMap();

        if (this.draw != null) {
            mapy.removeInteraction(this.draw);
        }

        this.source.clear(true);

        this.ftrGCCurrent = null;
        this.ftrGCTemp = null;

        this.draw = null;
        this.typeEdit = _type;


        if (active == true) {
            this.draw = new ol.interaction.Draw({
                source: this.source,
                type: this.typeEdit
            });

            mapy.addInteraction(this.draw);
            this.active = true;

        } else
            this.active = false;
    };


    this.setGeometryEditType = function (_type) {
        var mapy = this.geMap.getMap();
		var idEdo = ggsSingleton.JSONGeometryObject.IdEdo;
		var idMun = ggsSingleton.JSONGeometryObject.IdMun;
		var claveEdo = idEdo.length == 2 ? ggsSingleton.JSONGeometryObject.IdEdo : '0' + ggsSingleton.JSONGeometryObject.IdEdo


        if (this.draw != null) {
            mapy.removeInteraction(this.draw);
        }

        this.draw = null;
        this.typeEdit = _type;		

        if ((ggsSingleton.JSONGeometryObject.WKT == '' || ggsSingleton.JSONGeometryObject.WKT != '') && ggsSingleton.ftrGCTemp == null) {

            this.draw = new ol.interaction.Draw({
                source: this.source,
                type: this.typeEdit
            });

            mapy.addInteraction(this.draw);

            this.draw.on('drawend',
                function (evt) {

                    var feature = evt.feature;
                    var featureClone = feature.clone();
                    featureClone.getGeometry().transform('EPSG:3857', 'EPSG:4326');
                    var tmpGeometryCoordinates = featureClone.getGeometry().getCoordinates();

                    var ftr = null, tmpGeom;

                    if (ggsSingleton.typeEdit == 'Polygon') {
                        tmpGeom = new ol.geom.Polygon(tmpGeometryCoordinates);
                    } else if (ggsSingleton.typeEdit == 'LineString') {
                        tmpGeom = new ol.geom.LineString(tmpGeometryCoordinates);
                    } else if (ggsSingleton.typeEdit == 'Point') {
                        tmpGeom = new ol.geom.Point(tmpGeometryCoordinates);
                    }

                    ftr = new ol.Feature(tmpGeom);
                    var ftrWKT = ggsSingleton.olWKTParser.writeFeature(ftr);

                    ggsSingleton.JSONGeometryObject.WKT = ftrWKT;
                    ggsSingleton.JSONGeometryObject.WKTDraw = ftrWKT;

                    ftrtmp = ggsSingleton.olWKTParser.readFeature(ftrWKT);
                    var g = ftrtmp.getGeometry();
                    g.transform('EPSG:4326', 'EPSG:3857');

                    var ftrnew = new ol.Feature({
                        geometry: g
                    });

                    ggsSingleton.source.clear(true);
                    ggsSingleton.ftrGCCurrent = null;
                    ggsSingleton.ftrGCTemp = ftrnew;
                    ggsSingleton.ftrGCCurrent = ftrnew.clone();
                    ggsSingleton.addFeatureActive = false;
                    ggsSingleton.source.addFeature(ggsSingleton.ftrGCTemp);
                    ggsSingleton.addFeatureActive = true;

                    var xy = this.getCentroid_WGS84();

                    var iconFeature = new ol.Feature({
                        geometry: new ol.geom.Point(ol.proj.transform([Number(xy[0]), Number(xy[1])], 'EPSG:4326',
                        'EPSG:3857'))
                    });

                    iconFeature.setStyle(ggsSingleton.styleMarker);
                    ggsSingleton.sourcePoint.addFeature(iconFeature);
                    featurePoint = iconFeature;

                    var mappy = this.geMap.getMap();

                    mappy.getView().fit(ftrnew.getGeometry().getExtent(), mappy.getSize());

                    var zoom = mappy.getView().getZoom();
                    if (zoom >= 20)
                        mappy.getView().setZoom(19);

                    ggsSingleton.deactivate();

                    if (ggsSingleton.typeEdit == 'Polygon') {
                        tAddPolygon.off();
                    } else if (ggsSingleton.typeEdit == 'LineString') {
                        tAddLine.off();
                    } else if (ggsSingleton.typeEdit == 'Point') {
                        tAddPoint.off();
                    }

                    this.geMap.getMap().render();
                    this.geMap.getMap().renderSync();

                    var JSONResult = ggsSingleton.getGeometryData();

                    $('#' + componentOptions.serverInputsId.idInputGeometryType).val(JSONResult.GeometryType).trigger('change');
                    $('#' + componentOptions.serverInputsId.idInputWKT).val(JSONResult.WKT).trigger('change');
                    $('#' + componentOptions.serverInputsId.idInputX).val(JSONResult.X).trigger('change');
                    $('#' + componentOptions.serverInputsId.idInputY).val(JSONResult.Y).trigger('change');
                    $('#' + componentOptions.messagesInputsId.idInputCode).val(JSONResult.Code);
                    $('#' + componentOptions.messagesInputsId.idInputMessage).val(JSONResult.Msg);

                }, this);

        } else if (this.JSONGeometryObject.WKT != '' && ggsSingleton.ftrGCTemp != null) {

            this.setStyleEdit(this.styleLayerEdit);

            ggsSingleton.source.clear(true);
            ggsSingleton.sourcePoint.clear(true);
            ggsSingleton.source.addFeature(ggsSingleton.ftrGCTemp);

            var xy = this.getCentroid_WGS84();

            var iconFeature = new ol.Feature({
                geometry: new ol.geom.Point(ol.proj.transform([Number(xy[0]), Number(xy[1])], 'EPSG:4326',
                'EPSG:3857'))
            });

            iconFeature.setStyle(ggsSingleton.styleMarker);
            ggsSingleton.sourcePoint.addFeature(iconFeature);

            featurePoint = iconFeature;


            this.select = new ol.interaction.Select();
            mapy.addInteraction(this.select);

            var collection = this.select.getFeatures();
            collection.push(ggsSingleton.ftrGCTemp);

            this.modify = new ol.interaction.Modify({
                features: collection
            });

            mapy.addInteraction(this.modify);

            this.modify.on('modifyend',
                function (evt) {

                    ggsSingleton.sourcePoint.removeFeature(featurePoint);

                    var xy = this.getCentroid_WGS84();

                    var iconFeature = new ol.Feature({
                        geometry: new ol.geom.Point(ol.proj.transform([Number(xy[0]), Number(xy[1])], 'EPSG:4326',
                        'EPSG:3857'))
                    });

                    iconFeature.setStyle(ggsSingleton.styleMarker);
                    ggsSingleton.sourcePoint.addFeature(iconFeature);

                    featurePoint = iconFeature;

                    var JSONResult = ggsSingleton.getGeometryData();

                    $('#' + componentOptions.serverInputsId.idInputGeometryType).val(JSONResult.GeometryType).trigger('change');
                    $('#' + componentOptions.serverInputsId.idInputWKT).val(JSONResult.WKT).trigger('change');
                    $('#' + componentOptions.serverInputsId.idInputX).val(JSONResult.X).trigger('change');
                    $('#' + componentOptions.serverInputsId.idInputY).val(JSONResult.Y).trigger('change');

                    $('#' + componentOptions.messagesInputsId.idInputCode).val(JSONResult.Code);
                    $('#' + componentOptions.messagesInputsId.idInputMessage).val(JSONResult.Msg);

                }, this);
        }
    };

    this.activate = function (type) {
        if (this.active == false) {
			
			if(this.typeEdit != type)
				this.clear();
				
            this.typeEdit = type;
            this.setGeometryEditType(this.typeEdit);
            this.active = true;
        }
    }

    this.clear = function () {
        this.source.clear(true);
        this.sourcePoint.clear(true);
        this.ftrGCCurrent = null;
        this.ftrGCTemp = null;
        this.evtFeature = null;

        $('#' + componentOptions.serverInputsId.idInputGeometryType).val('').trigger('change');
        $('#' + componentOptions.serverInputsId.idInputWKT).val('').trigger('change');
        $('#' + componentOptions.serverInputsId.idInputX).val('').trigger('change');
        $('#' + componentOptions.serverInputsId.idInputY).val('').trigger('change');
        $('#' + componentOptions.messagesInputsId.idInputCode).val('100');
        $('#' + componentOptions.messagesInputsId.idInputMessage).val('El componente se cargó correctamente');        
    };

    this.deactivate = function () {

        if (this.hasGeometry())
            this.setStyle(this.styleLayer);

        if (this.active == true) {
            var mapy = this.geMap.getMap();
            mapy.removeInteraction(this.draw);
            mapy.removeInteraction(this.modify);
            mapy.removeInteraction(this.select);
            this.draw = null;
            this.active = false;
        }
    };

    this.isActive = function () {
        return this.active;
    };

    this.hasGeometry = function () {
        var has = false;

        if (this.ftrGCCurrent == null)
            has = false;
        else
            has = true;

        return has;
    };

    this.getGeometry = function () {
        if (this.hasGeometry() == true)
            return this.ftrGCTemp.getGeometry();
        else
            return null;
    };

    this.getExtendGeometry = function () {
        if (this.hasGeometry() == true)
            return this.ftrGCCurrent.getGeometry().getExtent();
        else
            return null;
    };

    this.getWKT = function () {
        var wkt = '';

        if (this.hasGeometry())
            wkt = this.olWKTParser.writeFeature(this.ftrGCCurrent);

        return wkt;
    };

    this.getGeometryData = function () {
		
        var wkt = '';
        var encoded_line = '';
        var type = this.typeEdit;
        var geom = this.getGeometry();

        if (type != "LineString") {
            if (ggsSingleton.JSONGeometryObject.IdEdo != '') {
                if (ggsSingleton.JSONGeometryObject.IdMun != '') {

                    var idEdo = ggsSingleton.JSONGeometryObject.IdEdo;
                    var idMun = ggsSingleton.JSONGeometryObject.IdMun;
                    var claveEdo = idEdo.length == 2 ? ggsSingleton.JSONGeometryObject.IdEdo : '0' + ggsSingleton.JSONGeometryObject.IdEdo;

                    geom = this.getGeometry();

                    var geomClone = geom.clone();
                    var drawGeom = geomClone.transform('EPSG:3857', 'EPSG:4326');
                    var format = new ol.format.WKT();
                    var wktToValidate = format.writeGeometry(drawGeom);

                    var params = {
                        'method': 'IS_INSIDE',
                        'wkt': wktToValidate,
                        'idScope': idMun
                    };

                    $.post('../ashx/UtilsGeo.ashx?', params, function (data) {

                        if (data.Code == 100) {

                            if (data.Obj == "True") {
                                var ftr = new ol.Feature();
                                ftr.setGeometry(drawGeom);
                                wkt = ggsSingleton.olWKTParser.writeFeature(ftr);

                                var xy = ggsSingleton.getCentroid_WGS84();
                                Messages.clean('messages');

                                ggsSingleton.JSONResult = {
                                    Code: '101',
                                    Msg: 'Se obtuvo la geometria Correctamente',
                                    WKT: wkt,
                                    GeometryType: geom.getType(),
                                    X: xy[0],
                                    Y: xy[1]
                                };

                                $('#' + componentOptions.serverInputsId.idInputGeometryType).val(ggsSingleton.JSONResult.GeometryType).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputWKT).val(ggsSingleton.JSONResult.WKT).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputX).val(ggsSingleton.JSONResult.X).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputY).val(ggsSingleton.JSONResult.Y).trigger('change');

                                $('#' + componentOptions.messagesInputsId.idInputCode).val(ggsSingleton.JSONResult.Code);
                                $('#' + componentOptions.messagesInputsId.idInputMessage).val(ggsSingleton.JSONResult.Msg);

                            }
                            else {

                                Messages.error('messages', 'La geometría trazada esta fuera del ambito permitido.', 19500);

                                ggsSingleton.JSONResult = {
                                    Code: '203',
                                    Msg: 'Se trazó la geometría fuera del Ambito geográfico permitido',
                                    WKT: '',
                                    GeometryType: '',
                                    X: '',
                                    Y: ''
                                };

                                $('#' + componentOptions.serverInputsId.idInputGeometryType).val(ggsSingleton.JSONResult.GeometryType).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputWKT).val(ggsSingleton.JSONResult.WKT).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputX).val(ggsSingleton.JSONResult.X).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputY).val(ggsSingleton.JSONResult.Y).trigger('change');
                                $('#' + componentOptions.messagesInputsId.idInputCode).val(ggsSingleton.JSONResult.Code);
                                $('#' + componentOptions.messagesInputsId.idInputMessage).val(ggsSingleton.JSONResult.Msg);
                            }

                        }
                        else {
                            Notifier.warning("messages", data.Msg);
                        }
                    }, 'json');
                }// fin de if mun
                else {

                    var idEdo = ggsSingleton.JSONGeometryObject.IdEdo;
                    var idMun = ggsSingleton.JSONGeometryObject.IdMun;
                    var claveEdo = idEdo.length == 2 ? ggsSingleton.JSONGeometryObject.IdEdo : '0' + ggsSingleton.JSONGeometryObject.IdEdo;

                    geom = this.getGeometry();

                    var geomClone = geom.clone();
                    var drawGeom = geomClone.transform('EPSG:3857', 'EPSG:4326');
                    var format = new ol.format.WKT();
                    var wktToValidate = format.writeGeometry(drawGeom);

                    var params = {
                        'method': 'IS_INSIDE',
                        'wkt': wktToValidate,
                        'idScope': idEdo
                    };

                    $.post('../ashx/UtilsGeo.ashx?', params, function (data) {

                        if (data.Code == 100) {

                            if (data.Obj == "True") {
                                var ftr = new ol.Feature();
                                ftr.setGeometry(drawGeom);
                                wkt = ggsSingleton.olWKTParser.writeFeature(ftr);

                                var xy = ggsSingleton.getCentroid_WGS84();
                                Messages.clean('messages');

                                ggsSingleton.JSONResult = {
                                    Code: '101',
                                    Msg: 'Se obtuvo la geometria Correctamente',
                                    WKT: wkt,
                                    GeometryType: geom.getType(),
                                    X: xy[0],
                                    Y: xy[1]
                                };

                                $('#' + componentOptions.serverInputsId.idInputGeometryType).val(ggsSingleton.JSONResult.GeometryType).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputWKT).val(ggsSingleton.JSONResult.WKT).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputX).val(ggsSingleton.JSONResult.X).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputY).val(ggsSingleton.JSONResult.Y).trigger('change');
                                $('#' + componentOptions.messagesInputsId.idInputCode).val(ggsSingleton.JSONResult.Code);
                                $('#' + componentOptions.messagesInputsId.idInputMessage).val(ggsSingleton.JSONResult.Msg);

                            }
                            else {

                                Messages.error('messages', 'La geometría trazada esta fuera del ambito permitido.', 19500);

                                ggsSingleton.JSONResult = {
                                    Code: '203',
                                    Msg: 'Se trazó la geometría fuera del Ambito geográfico permitido',
                                    WKT: '',
                                    GeometryType: '',
                                    X: '',
                                    Y: ''
                                };

                                $('#' + componentOptions.serverInputsId.idInputGeometryType).val(ggsSingleton.JSONResult.GeometryType).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputWKT).val(ggsSingleton.JSONResult.WKT).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputX).val(ggsSingleton.JSONResult.X).trigger('change');
                                $('#' + componentOptions.serverInputsId.idInputY).val(ggsSingleton.JSONResult.Y).trigger('change');
                                $('#' + componentOptions.messagesInputsId.idInputCode).val(ggsSingleton.JSONResult.Code);
                                $('#' + componentOptions.messagesInputsId.idInputMessage).val(ggsSingleton.JSONResult.Msg);
                            }

                        }
                        else {
                            Notifier.warning("messages", data.Msg);
                        }
                    }, 'json');

                }
            }//fin de if edo
            else {

                ggsSingleton.JSONResult = {
                    Code: '204',
                    Msg: 'No se ha especificado el Estado.',
                    WKT: '',
                    GeometryType: '',
                    X: '',
                    Y: ''
                };

                $('#' + componentOptions.serverInputsId.idInputGeometryType).val(ggsSingleton.JSONResult.GeometryType).trigger('change');
                $('#' + componentOptions.serverInputsId.idInputWKT).val(ggsSingleton.JSONResult.WKT).trigger('change');
                $('#' + componentOptions.serverInputsId.idInputX).val(ggsSingleton.JSONResult.X).trigger('change');
                $('#' + componentOptions.serverInputsId.idInputY).val(ggsSingleton.JSONResult.Y).trigger('change');
                $('#' + componentOptions.messagesInputsId.idInputCode).val(ggsSingleton.JSONResult.Code);
                $('#' + componentOptions.messagesInputsId.idInputMessage).val(ggsSingleton.JSONResult.Msg);
            }
        } else {
            var geomClone = geom.clone();
            var drawGeom = geomClone.transform('EPSG:3857', 'EPSG:4326');
            var ftr = new ol.Feature();
            ftr.setGeometry(drawGeom);
            wkt = ggsSingleton.olWKTParser.writeFeature(ftr);

            var xy = ggsSingleton.getCentroid_WGS84();

            ggsSingleton.JSONResult = {
                Code: '101',
                Msg: 'Se obtuvo la geometria Correctamente',
                WKT: wkt,
                GeometryType: geom.getType(),
                X: xy[0],
                Y: xy[1]
            };

            $('#' + componentOptions.serverInputsId.idInputGeometryType).val(ggsSingleton.JSONResult.GeometryType).trigger('change');
            $('#' + componentOptions.serverInputsId.idInputWKT).val(ggsSingleton.JSONResult.WKT).trigger('change');
            $('#' + componentOptions.serverInputsId.idInputX).val(ggsSingleton.JSONResult.X).trigger('change');
            $('#' + componentOptions.serverInputsId.idInputY).val(ggsSingleton.JSONResult.Y).trigger('change');

            $('#' + componentOptions.messagesInputsId.idInputCode).val(ggsSingleton.JSONResult.Code);
            $('#' + componentOptions.messagesInputsId.idInputMessage).val(ggsSingleton.JSONResult.Msg);
        }
		
		return this.JSONResult;
		
    }//fin de metodo

    this.getCentroid_WGS84 = function () {
        var geom = this.getGeometry();
        var Extent = geom.getExtent();
        var xy = null;

        var X = Extent[0] + (Extent[2] - Extent[0]) / 2;
        var Y = Extent[1] + (Extent[3] - Extent[1]) / 2;
        xy = [X, Y];

        xy = ol.proj.transform(xy, this.geMap.getCurrentProjection(), 'EPSG:4326');

        return xy;
    }

    this.setGeometry = function (JSONGeometry) {

        this.JSONGeometryObject = JSONGeometry;
		
		var lineColor=null;
		
		if(typeof JSONGeometry.lineColor != 'undefined'){
			lineColor=JSONGeometry.lineColor;
		}

        if (JSONGeometry.Edit != '' || JSONGeometry.Edit != null || JSONGeometry.Edit != undefined) {

            if ((JSONGeometry.IdEdo == '' || JSONGeometry.IdEdo == null || JSONGeometry.IdEdo == undefined) && (JSONGeometry.TipoGeom == '' || JSONGeometry.TipoGeom == null || JSONGeometry.TipoGeom == undefined) && (JSONGeometry.WKT == '' || JSONGeometry.WKT == null || JSONGeometry.WKT == undefined)) {

                toolSettings(JSONGeometry.Edit, JSONGeometry.TipoGeom);				

                this.JSONResult = {
                    Code: '100',
                    Msg: 'El componente se cargó correctamente. Los atributos Edo, tipoGeom y WKT no tienen ningun valor.',
                    WKT: '',
                    X: '',
                    Y: ''
                };

            } else {

                if (JSONGeometry.IdEdo != '' || JSONGeometry.IdEdo != null || JSONGeometry.IdEdo != undefined) {

                    if ((JSONGeometry.TipoGeom == '' || JSONGeometry.TipoGeom == null || JSONGeometry.TipoGeom == undefined) && (JSONGeometry.WKT == '' || JSONGeometry.WKT == null || JSONGeometry.WKT == undefined)) {

                        toolSettings(JSONGeometry.Edit, JSONGeometry.TipoGeom);

                        this.JSONResult = {
                            Code: '100',
                            Msg: 'El componente se cargó correctamente. Solo hay valor en Edo',
                            WKT: '',
                            X: '',
                            Y: ''
                        };

                    } else {

                        if ((JSONGeometry.TipoGeom == '' || JSONGeometry.TipoGeom == null || JSONGeometry.TipoGeom == undefined) && (JSONGeometry.WKT != '' || JSONGeometry.WKT != null || JSONGeometry.WKT != undefined)) {

                            this.JSONResult = {
                                Code: '202',
                                Msg: 'Es necesario indicar el tipo de Geometria (Polygon, LineString, Point).',
                                WKT: '',
                                X: '',
                                Y: ''
                            };

                            toolSettings(false, '');

                        } else {


                            if (JSONGeometry.TipoGeom != 'Polygon' && JSONGeometry.TipoGeom != 'Point' && JSONGeometry.TipoGeom != 'LineString') {

                                this.JSONResult = {
                                    Code: '202',
                                    Msg: 'Es necesario indicar un tipo de Geometria valido (Polygon, LineString, Point).',
                                    WKT: '',
                                    X: '',
                                    Y: ''
                                };

                                toolSettings(false, '');
                            } else {

                                try {

                                    var idEdo = JSONGeometry.IdEdo;

                                    if (isNaN(Number(JSONGeometry.IdEdo)))
                                        throw new Error("El valor del Estado debe ser un entero.");
                                    else {

                                        var idEdoNumber = Number(JSONGeometry.IdEdo);

                                        if (idEdoNumber > 32 || idEdoNumber < 1)
                                            throw new Error("El valor del Estado no es valido. Los valores van desde 1 a 32.");

                                    }


                                    toolSettings(JSONGeometry.Edit, '');
									//renderTools();
                                    ggsSingleton.setGeometryInitType(JSONGeometry.TipoGeom, false);

                                    if (JSONGeometry.WKT != '') {

                                        if (JSONGeometry.TipoGeom == 'LineString') {
											

                                            var styleLineString = new ol.style.Style({
                                                stroke: new ol.style.Stroke({
                                                    //color: '#00FF00',
													color: lineColor == null? '00FF00': lineColor,
                                                    width: 5
                                                }),
                                                image: new ol.style.Circle({
                                                    radius: 7,
                                                    fill: new ol.style.Fill({
                                                        color: '#ffcc33'
                                                    })
                                                })
                                            });

                                            this.setStyle(styleLineString);
                                        }
										
										if(JSONGeometry.TipoGeom == 'Polygon' && lineColor != null){
											var stylePolygon= new ol.style.Style({
												stroke: new ol.style.Stroke({
													color: lineColor,
													width: 5
												}),
												fill: new ol.style.Fill({
													color:[255,255,255,0.4]
												})
											});
											
											this.setStyle(stylePolygon);
										}
                                        ftrtmp = this.olWKTParser.readFeature(JSONGeometry.WKT);
                                        var g = ftrtmp.getGeometry();
                                        g.transform('EPSG:4326', 'EPSG:3857');

                                        var ftrnew = new ol.Feature({
                                            geometry: g
                                        });

                                        if (isNaN(Number(JSONGeometry.X)))
                                            throw new Error("El valor X debe ser un entero");

                                        if (isNaN(Number(JSONGeometry.Y)))
                                            throw new Error("El valor Y debe ser un entero");

                                        var iconFeature = new ol.Feature({
                                            geometry: new ol.geom.Point(ol.proj.transform([Number(JSONGeometry.X), Number(JSONGeometry.Y)], 'EPSG:4326',
                                            'EPSG:3857'))
                                        });

                                        /********************Se genera poligono para mapa google********************************/
                                        if (typeof (google) != 'undefined') {
                                            var puntosPoligono = ConvertirGeometriaAJson(JSONGeometry.WKT);
                                            var puntosGoogle = new Array();

                                            for (var i = 0; i < puntosPoligono.length; i++) {
                                                for (var j = 0; j < puntosPoligono[i].length; j++) {
                                                    puntosGoogle.push(new google.maps.LatLng(puntosPoligono[i][j].y, puntosPoligono[i][j].x));
                                                }
                                                var opciones = {
                                                    clickable: false,
													strokeColor: lineColor == null ? '#F20C1F' : lineColor,
                                                    //strokeColor: '#F20C1F',
                                                    strokeOpacity: 1,
                                                    strokeWeight: 2,
                                                    fillColor: '#F8F6F3',
                                                    fillOpacity: 0.4,
                                                    paths: puntosGoogle,
                                                    //map: MapComponent.gmap_,
                                                    visible: true
                                                }

                                                if (poligonoGoogle) {
                                                    poligonoGoogle.setMap(null);
                                                }
                                                poligonoGoogle = new google.maps.Polygon(opciones);
                                            }
                                        }

                                        //si el mapa actual es google maps se oculta la geometria de openlayers
                                        if (tipoMapaActual == 'Google Map') {
                                            ggsSingleton.vectorLayer.setVisible(false);
                                            if (poligonoGoogle) {
                                                poligonoGoogle.setMap(MapComponent.gmap_);
                                            }
                                        }
                                        else {
                                            ggsSingleton.vectorLayer.setVisible(true);
                                            if (poligonoGoogle) {
                                                poligonoGoogle.setMap(null);
                                            }
                                        }

                                        iconFeature.setStyle(ggsSingleton.styleMarker);
                                        featurePoint = iconFeature;
                                        
                                        ggsSingleton.source.clear(true);
                                        ggsSingleton.ftrGCCurrent = null;
                                        ggsSingleton.ftrGCTemp = ftrnew;
                                        ggsSingleton.ftrGCCurrent = ftrnew.clone();
                                        ggsSingleton.addFeatureActive = false;
                                        ggsSingleton.source.addFeature(ggsSingleton.ftrGCTemp);
                                        ggsSingleton.sourcePoint.addFeature(iconFeature);
                                        ggsSingleton.addFeatureActive = true;

                                        var mappy = ggsSingleton.geMap.getMap();

                                        mappy.getView().fit(ftrnew.getGeometry().getExtent(), mappy.getSize());

                                        var zoom = mappy.getView().getZoom();
                                        if (zoom >= 20)
                                            mappy.getView().setZoom(19);

                                        this.JSONResult = {
                                            Code: '100',
                                            Msg: 'El componente se cargó correctamente',
                                            WKT: '',
                                            X: '',
                                            Y: ''
                                        };
                                    }
                                }
                                catch (e) {

                                    this.JSONResult = {
                                        Code: '300',
                                        Msg: e.message,
                                        WKT: '',
                                        X: '',
                                        Y: ''
                                    };

                                    toolSettings(false, '');
                                }
                            }
                        }
                    }
                }
            }
        } else {

            this.JSONResult = {
                Code: '200',
                Msg: 'Es necesario la Clave del Estado y opcionalmente la Clave del Municipio para validar donde se trazó la geometriía.',
                WKT: '',
                X: '',
                Y: ''
            };

            toolSettings(false, '');
        }

        return this.JSONResult;
    }
}



var gsSingleton = null;

function GeoSearch(idInputText, mapComponent) {
    this.mapComponent = mapComponent;
    this.idInputText = idInputText;
    gsSingleton = this;

    this.Init = function () {

        gsSingleton.SearchOnServer();
    }

    this.SearchOnServer = function () {

        $("#" + this.idInputText).autocomplete({
            source: function (request, response) {

                var textSearch = request.term;
                var regex = new RegExp(request.term, 'i');
                $.ajax({
                    url: GEOCOMPONENT_RESOURCES_URL + 'data/ambitos.json',
                    data: { term: textSearch },
                    dataType: 'json',
                    success: function (result) {

                        response($.map(result.data, function (item) {
                            if (regex.test(item.nombre)) {
                                return {
                                    label: item.nombre,
                                    value: item.nombre,
                                    x: item.X,
                                    y: item.Y,
                                    clave: item.clave
                                }
                            }
                        }));
                    }
                });

            },
            minLength: 2,
            select: function (event, ui) {
                gsSingleton.showResultOnMap(ui.item.x, ui.item.y, ui.item.clave);
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    }

    this.showResultOnMap = function (x, y, cve) {

        if (cve.length == 2)
            gsSingleton.mapComponent.setCenterZoom(x, y, 9, 'EPSG:4326');
        else if (cve.length == 5)
            gsSingleton.mapComponent.setCenterZoom(x, y, 12, 'EPSG:4326');
    }
}


function toolSettings(edit, geometryType) {

    if (edit) {

        var opt = null;

        opt = {
            id: "toolDeactivate",
            imgNormal: GEOCOMPONENT_IMAGES_URL + 'images/paneo_i.png',
            imgOver: GEOCOMPONENT_IMAGES_URL + 'images/paneo_a.png',
            imgActive: GEOCOMPONENT_IMAGES_URL + 'images/paneo_a.png',
            imgInactive: GEOCOMPONENT_IMAGES_URL + 'images/paneo_i.png',
            onClick: toolDeactivate_click
        };

        tDeactivate = new ActionButton(opt);
        tDeactivate.off();

        opt = {
            id: "toolDeleteDraw",
            imgNormal: GEOCOMPONENT_IMAGES_URL + 'images/borrar_i.png',
            imgOver: GEOCOMPONENT_IMAGES_URL + 'images/borrar_a.png',
            imgActive: GEOCOMPONENT_IMAGES_URL + 'images/borrar_a.png',
            imgInactive: GEOCOMPONENT_IMAGES_URL + 'images/borrar_i.png',
            onClick: toolDeleteDraw_click
        };

        tDeleteDraw = new ActionButton(opt);
        tDeleteDraw.off();       

        opt = {
            id: "toolAddPolygon",
            imgNormal: GEOCOMPONENT_IMAGES_URL + 'images/poligono_i.png',
            imgOver: GEOCOMPONENT_IMAGES_URL + 'images/poligono_a.png',
            imgActive: GEOCOMPONENT_IMAGES_URL + 'images/poligono_a.png',
            imgInactive: GEOCOMPONENT_IMAGES_URL + 'images/poligono_i.png',
            onClick: toolAddPolygon_click
        };

        tAddPolygon = new ActionButton(opt);
        tAddPolygon.off();
        

        opt = {
            id: "toolAddLine",
            imgNormal: GEOCOMPONENT_IMAGES_URL + 'images/linea_buffer_i.png',
            imgOver: GEOCOMPONENT_IMAGES_URL + 'images/linea_buffer_a.png',
            imgActive: GEOCOMPONENT_IMAGES_URL + 'images/linea_buffer_a.png',
            imgInactive: GEOCOMPONENT_IMAGES_URL + 'images/linea_buffer_i.png',
            onClick: toolAddLine_click
        };

        tAddLine = new ActionButton(opt);
        tAddLine.off();
    

        opt = {
            id: "toolAddPoint",
            imgNormal: GEOCOMPONENT_IMAGES_URL + 'images/agregar_punto_i.png',
            imgOver: GEOCOMPONENT_IMAGES_URL + 'images/agregar_punto_a.png',
            imgActive: GEOCOMPONENT_IMAGES_URL + 'images/agregar_punto_a.png',
            imgInactive: GEOCOMPONENT_IMAGES_URL + 'images/agregar_punto_i.png',
            onClick: toolAddPoint_click
        };

        tAddPoint = new ActionButton(opt);
        tAddPoint.off();       
        

    } else {

        $('#toolAddLine').parent().remove();
        $('#toolAddPolygon').parent().remove();
        $('#toolAddPoint').parent().remove();
        $('#toolDeactivate').parent().remove();
        $('#toolDeleteDraw').parent().remove();
        $('#containerCurrentZoom').remove();
        $('#containerGeoSearch').remove();
    }
}

var Messages = {

    clean: function (id) {
        $('#' + id).html('');
    },
    error: function (id, msg, fadetime) {

        var nId = 'notify-' + new Date().getTime();

        $('#' + id).html('');
        $('#' + id).append('<span id="' + nId + '" class="notify notify-error"><img src="' + GEOCOMPONENT_IMAGES_URL + 'images/icon-error.png"><label>' + msg + '</label></span>');

        if (fadetime) {
            setTimeout(function () {
                $('#' + nId).fadeOut();
                $('#' + id).html('');
            }, fadetime);
        }

        return nId;
    },
    warning: function (id, msg, fadetime) {

        var nId = 'notify-' + new Date().getTime();

        $('#' + id).html('');
        $('#' + id).append('<div id="' + nId + '" class="notify notify-warning"><img src="' + GEOCOMPONENT_IMAGES_URL + 'images/icon-warning.png">' + msg + '</div>');

        if (fadetime) {
            setTimeout(function () {
                $('#' + nId).fadeOut();
                $('#' + id).html('');
            }, fadetime);
        }

        return nId;
    },
    success: function (id, msg, fadetime) {

        var nId = 'notify-' + new Date().getTime();

        $('#' + id).html('');
        $('#' + id).append('<div id="' + nId + '" class="notify notify-success"><img src="' + GEOCOMPONENT_IMAGES_URL + 'images/icon-success.png">' + msg + '</div>');

        if (fadetime) {
            setTimeout(function () {
                $('#' + nId).fadeOut();
                $('#' + id).html('');
            }, fadetime);
        }

        return nId;
    },
    info: function (id, msg, fadetime) {

        var nId = 'notify-' + new Date().getTime();

        $('#' + id).html('');
        $('#' + id).append('<div id="' + nId + '" class="notify notify-info"><img src="' + GEOCOMPONENT_IMAGES_URL + 'images/icon-info.png">' + msg + '</div>');

        if (fadetime) {
            setTimeout(function () {
                $('#' + nId).fadeOut();
                $('#' + id).html('');
            }, fadetime);
        }

        return nId;
    }
}

/*Funcion para convertir geometria SQL a json con propiedades x,y*/
function ConvertirGeometriaAJson(CadenaGeometria) {
    var ArregloPuntosGeometrias = [];
    if (CadenaGeometria.indexOf('GEOMETRYCOLLECTION') != -1) {
        CadenaGeometria = CadenaGeometria.replace("GEOMETRYCOLLECTION (", '');
        CadenaGeometria = CadenaGeometria.split(')))').join('))');
        var poligonos = CadenaGeometria.split('),');

        CadenaGeometria = '';
        for (var i = 0; i < poligonos.length; i++) {
            var CadenaResultado = poligonos[i] + ")";
            if (poligonos[i].indexOf("MULTIPOLYGON") != -1) {
                CadenaResultado = CadenaResultado.replace(" MULTIPOLYGON ((", 'MULTIPOLYGON ((');
                CadenaResultado = CadenaResultado.replace("MULTIPOLYGON ((", '');
                CadenaResultado = CadenaResultado.split(", ").join(',');
                CadenaResultado = CadenaResultado.split("), ").join("@");
                CadenaResultado = CadenaResultado.split("(").join('');
                CadenaResultado = CadenaResultado.split(")").join('');
                CadenaResultado = CadenaResultado.split(' ').join('');
                if (i < (poligonos.length - 1)) {
                    CadenaGeometria += CadenaResultado + "@";
                }
                else {
                    CadenaGeometria += CadenaResultado;
                }
            }
            else if (poligonos[i].indexOf("POLYGON") != -1) {
                CadenaResultado = CadenaResultado.replace(' POLYGON', 'POLYGON');
                CadenaResultado = CadenaResultado.replace("POLYGON (", '');
                CadenaResultado = CadenaResultado.split(", ").join(',');
                CadenaResultado = CadenaResultado.split("(").join('');
                CadenaResultado = CadenaResultado.split(")").join('');
                if (i < (poligonos.length - 1)) {
                    CadenaGeometria += CadenaResultado + "@";
                }
                else {
                    CadenaGeometria += CadenaResultado;
                }
            }
        }
    }
    else if (CadenaGeometria.indexOf('MULTIPOLYGON') != -1) {
        CadenaGeometria = CadenaGeometria.replace(" MULTIPOLYGON ((", 'MULTIPOLYGON ((');
        CadenaGeometria = CadenaGeometria.replace("MULTIPOLYGON ((", '');
        CadenaGeometria = CadenaGeometria.split(", ").join(',');
        CadenaGeometria = CadenaGeometria.split("), ").join("@");
        CadenaGeometria = CadenaGeometria.split("(").join('');
        CadenaGeometria = CadenaGeometria.split(")").join('');
        CadenaGeometria = CadenaGeometria.split(' ').join('');
    }
    else if (CadenaGeometria.indexOf('POLYGON') != -1) {
        CadenaGeometria = CadenaGeometria.replace(' POLYGON', 'POLYGON');
        CadenaGeometria = CadenaGeometria.replace("POLYGON (", '');
        CadenaGeometria = CadenaGeometria.split(", ").join(',');
        CadenaGeometria = CadenaGeometria.split("(").join('');
        CadenaGeometria = CadenaGeometria.split(")").join('');
    }
    else if (CadenaGeometria.indexOf('POINT') != -1) {

        CadenaGeometria = CadenaGeometria.split("POINT (").join('');
        CadenaGeometria = CadenaGeometria.split(")").join('');
    }
    else if (CadenaGeometria.indexOf('LINESTRING') != -1) {
        CadenaGeometria = CadenaGeometria.split("LINESTRING (").join('');
        CadenaGeometria = CadenaGeometria.split(", ").join(',');
        CadenaGeometria = CadenaGeometria.split(")").join('');
    }
    var Geometrias = CadenaGeometria.split('@');
    for (var i = 0; i < Geometrias.length; i++) {
        var arregloPuntos = [];
        var puntos = Geometrias[i].split(',');
        for (var j = 0; j < puntos.length; j++) {
            var cadenaPunto = puntos[j].split(' ');
            arregloPuntos.push({ x: cadenaPunto[0], y: cadenaPunto[1] });
        }
        ArregloPuntosGeometrias.push(arregloPuntos);
    }

    return ArregloPuntosGeometrias;

}