<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfSeleccionMapa.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Geoposicion.wfSeleccionMapa" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">

   

   <%-- RCA 11/08/2017
    SE AGREGO METAETIQUETA PARA CONVERTIR DE HTM A HTMLS--%>
    <meta name="google-site-verification" content="IARv6LJ_6S7oUH1ME7QLlphc9xjC1Srhx-swSD6MzVA" />

    <%-- Referencias GeoComponent --%>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../css/smoothness/jquery-ui-1.10.4.custom.css " />
    <%--<link href="http://ide.indaabin.gob.mx/GeoPruebas/css/styles.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="http://ide.indaabin.gob.mx/GeoPruebas/ol/ol.js" type="text/javascript"></script>--%>
   <%-- <script src="http://ide.indaabin.gob.mx/GeoPruebas/GeoComponent.js" type="text/javascript"></script>--%>
    <%--<script src="http://ide.indaabin.gob.mx/GeoPruebas/jquery/jquery-2.1.1.min.js" type="text/javascript"></script>--%>
   <%-- <script src="http://ide.indaabin.gob.mx/GeoPruebas/jquery/jquery-ui.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.21&key=AIzaSyDyw0tuwkWlKq6HVP5QLj2PLHwbH17QFVY"></script>
    <link href="../css/styles.css" rel="stylesheet" type="text/css" />
    <script src="../ol/ol.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/GeoComponent.js" type="text/javascript"></script>
    <script src="../js/componentegeo.js"></script>
    <script src="../js/commons.js"></script>   

    <script type="text/javascript">
        function validaSeleccion2() {
            // Se omite esta funcion porque Chrome no adminte el llamado a la funcion showModalDialog
            if (document.getElementById('code').value == "203") {
                alert("La Geometría esta fuera del Estado/Municipio elegido, favor de verificar.");
                return false;
            } else {
                if (document.getElementById('wkt').value == "" || document.getElementById('wkt').value == null) {
                    alert("Debe trazar la Geometría.");
                    return false;
                }
            }
            var response = new Array("", "", "", "", "", "");
            response[0] = document.getElementById('Edo').value;
            response[1] = document.getElementById('Mun').value;
            response[2] = document.getElementById('tipoGeometria').value;
            response[3] = document.getElementById('x').value;
            response[4] = document.getElementById('y').value;
            response[5] = document.getElementById('wkt').value;
            window.returnValue = response;
            window.close();
        }

        function closeWindow() {
            var response = new Array("", "", "", "", "", "");
            window.returnValue = response;
            window.close();
        }

        function validaSeleccion() {
            if (document.getElementById('code').value == "203") {
                alert("La Geometría esta fuera del Estado/Municipio elegido, favor de verificar.");
                return false;
            } else {
                if (document.getElementById('wkt').value == "" || document.getElementById('wkt').value == null) {
                    alert("Debe trazar la Geometría.");
                    return false;
                }
            }
            var response = new Array("", "", "", "", "", "");
            response[0] = document.getElementById('Edo').value;
            response[1] = document.getElementById('Mun').value;
            response[2] = document.getElementById('tipoGeometria').value;
            response[3] = document.getElementById('x').value;
            response[4] = document.getElementById('y').value;
            response[5] = document.getElementById('wkt').value;
            // Llamado a la funcion asignaValores, de la forma CapturaSolicitud.aspx
            window.opener.asignaValoresRetornoMapa(response);
            window.close();
        }
    </script>
    <%-- FIN - Referencias GeoComponent --%>
    
    <style>
        body {
            font-family: "Open Sans","Helvetica Neue",Helvetica,Arial,sans-serif;
            font-size: 18px;
            line-height: 1.428571429;
            color: #545454;
        }

        /*PARTE PARA QUITAR label OPCION defsL CUADRO QUE MUESTRA OS DIFERENTES MAPAS*/
        /*select#toolBaseMap {
            visibility:hidden;
        }*/

        /*MUESTRA LAS OPCIONES defs LINEA PONER PUNTO QUITAR PUNTO, POLIGONO*/
        /*select#toolBaseMap,*/ img#toolDeactivate, img#toolAddLine, img#toolAddPolygon {
            visibility:hidden;
        }
        
        .fill {
				width:100%;
				height:100%;
			}


        input#currentZoom {
            background-color: transparent;
            color:#FF732C;
            text-shadow: -1px 0 black, 0 1px black, 1px 0 black, 0 -1px black;
            border: none; 
            font-size: 18px;
            vertical-align: middle;
            user-select: none;            
            padding: 10px 25px;
        }

        label
        {
           width:120px;
           color:#FF732C;
           font-size:18px;
           padding-left:60px;
           text-shadow: -1px 0 black, 0 1px black, 1px 0 black, 0 -1px black;
        }

        .labelsMap
        {
           color:#FF5500;
           font-size:18px;
           text-shadow: -1px 0 black, 0 1px black, 1px 0 black, 0 -1px black;
           font-family:Verdana; 
        }

        .btn-primary {
            box-shadow: 0 2px 0 0 #1b5dab;
            color: #fff;
            background-color: #4A90E2;
            -webkit-appearance: button;
            cursor: pointer;
            border: 1px solid #777;
            font-size: 18px;
            border-radius: 3px;
            padding: 10px 25px;
            text-decoration: none;
            display: inline-block;
            margin-bottom: 0;
            font-weight: 300;
            text-align: center;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            white-space: nowrap;
            line-height: 1.428571429;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            vertical-align: middle;
            font: inherit;
            margin: 0;
            align-items: flex-start;	
            -webkit-rtl-ordering: logical;
        }

        .btn-default {
            box-shadow: 0 2px 0 0 #444;
            color: #333;
            background-color: #fff;
            -webkit-appearance: button;
            cursor: pointer;
            border: 1px solid #777;
            font-size: 18px;
            border-radius: 3px;
            padding: 10px 25px;
            text-decoration: none;
            display: inline-block;
            margin-bottom: 0;
            font-weight: 300;
            text-align: center;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            white-space: nowrap;
            line-height: 1.428571429;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            vertical-align: middle;
            font: inherit;
            margin: 0;
            align-items: flex-start;	
            -webkit-rtl-ordering: logical;
        }

        table#tblCZ {
            width:280px;
        }        
    </style>
    <title>Sistema de contratos y arrendamientos: Captura de punto en mapa</title>
</head>


<body>
    <form id="form1" runat="server">    
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    // <![CDATA[
                    Sys.Application.add_load(WireEvents); // fix wiring for .NET ajax updatepanel
                    WireEvents(); // handle page load wiring using jquery. This will fire on page load

                    function WireEvents() {
                        //initEditGeoposicionComponenteGeo();
                        initCapturaGeoposicionComponenteGeo();
                    };
                    // ]]>
                </script>
                <div id="mapaAvaluos" class="labelsMap" style="width: 675px; height: 500px;"></div> <%--style="width: 675px; height: 500px;"--%>
                <input type="hidden" id="wkt" runat="server" />
                <input type="hidden" id="Edo" runat="server" />
                <input type="hidden" id="Mun" runat="server" />
                <input type="hidden" id="tipoGeometria" runat="server" />
                <input type="hidden" id="x" runat="server" />
                <input type="hidden" id="y" runat="server" />
                <input type="hidden" id="Editar" runat="server" />
                <input type="hidden" id="code" runat="server" />
                <input type="hidden" id="message" runat="server" />
                <input type="hidden" id="SolicitudId" runat="server" />
                <input type="hidden" id="UserId" runat="server" />
                <input type="hidden" id="hoficio" runat="server" />
                <input type="hidden" id="CP" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div style="align-content:center; align-items:center; text-align:center">
            <input type="button" class="btn-default" onclick="closeWindow();" value="Cancelar" />            
            &nbsp;&nbsp;
            <input type="button" class="btn-primary" onclick="validaSeleccion();" value="Guardar" />
        </div>
                           
    </form>
</body>
</html>
