var locationValues = {
    IdEdo: null,
    IdMun: null,
    IdLoc: null,
    IdCol: null,
    IdCP: null
};
GEOCOMPONENT_IMAGES_URL = '../';
GEOCOMPONENT_RESOURCES_URL = 'https://sistemas.indaabin.gob.mx/GeoPruebas/';

function initCapturaGeoposicionComponenteGeo() {
    var options = {

        idContainer: 'mapaAvaluos',
        serverInputsId: {
            idInputEdo: 'Edo',
            idInputMun: 'Mun',
            idInputGeometryType: 'tipoGeometria',
            idInputWKT: 'wkt',
            idInputX: 'x',
            idInputY: 'y',
            idInputEditable: 'Editar'
        },
        messagesInputsId: {
            idInputCode: 'code',
            idInputMessage: 'message'
        },
        search: false,
        google: false
    };

    setupComponent(options);

    var idEdo = $('#Edo').val();

    if (idEdo != '0' && idEdo != undefined)
        if (idEdo.length == 1)
            idEdo = '0' + idEdo;

    var idMun = $('#Mun').val();

    if (idMun != '0' && idMun != undefined) {
        if (idMun.length == 1)
            idMun = idEdo + '00' + idMun;
        else if (idMun.length == 2)
            idMun = idEdo + '0' + idMun;
        else if (idMun.length == 3)
            idMun = idEdo + idMun;
    }
    else if (idMun == '0')
        idMun = '';

    var idCP = $('#CP').val();
    if (idCP == '0')
        idCP = '';

    locationValues.IdEdo = idEdo;
    locationValues.IdMun = idMun;
    locationValues.IdCP = idCP;

    var hasWKT = $('#' + options.serverInputsId.idInputWKT).val();

    if (hasWKT == '') {
        if (locationValues.IdCP != null && locationValues.IdCP != '')
            setLocationCenter("CP", locationValues, true);
        else {
            if (locationValues.IdLoc != null && locationValues.IdLoc != '' && locationValues.IdLoc != '0')
                setLocationCenter("Localidad", locationValues, true);
            else if (locationValues.IdMun != null && locationValues.IdMun != '' && locationValues.IdMun != '0')
                setLocationCenter("Municipio", locationValues, true);
            else if (locationValues.IdEdo != null && locationValues.IdEdo != '' && locationValues.IdEdo != '0')
                setLocationCenter("Estado", locationValues, true);
        }
    } else {
        if (locationValues.IdCP != null && locationValues.IdCP != '')
            setLocationCenter("CP", locationValues, false);
        else {
            if (locationValues.IdLoc != null && locationValues.IdLoc != '' && locationValues.IdLoc != '0')
                setLocationCenter("Localidad", locationValues, false);
            else if (locationValues.IdMun != null && locationValues.IdMun != '' && locationValues.IdMun != '0')
                setLocationCenter("Municipio", locationValues, false);
            else if (locationValues.IdEdo != null && locationValues.IdEdo != '' && locationValues.IdEdo != '0')
                setLocationCenter("Estado", locationValues, false);
        }
    }
}