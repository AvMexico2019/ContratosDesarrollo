function closeWindow() {
    window.open('', '_parent', '');
    window.close();
}


function ValidacionDatos() {

    if (document.getElementById('code').value == "203") {
        alert("La Geometría esta fuera del Estado/Municipio elegido, favor de verificar.");
        return false;
    } else {
        if (document.getElementById('wkt').value == "" || document.getElementById('wkt').value == null) {
            alert("Debe trazar la Geometría.");
            return false;
        }
    }
    exportMap();
}

//function exportMap(IdSolicitud, IdUser, Oficio, ruta) {
function exportMap(IdDocumento) {

    //var exportPNGElement = document.getElementById('export-png');
    var canvas = $('canvas')[0];
    //exportPNGElement.href = canvas.toDataURL('image/png');

    //var canvas2 = document.createElement('CANVAS');
    var ctx = canvas.getContext('2d');


    //canvas2.height = canvas.height;
    //canvas2.width = canvas.width;
    ctx.drawImage(canvas, 0, 0, canvas.width, canvas.height);


    var img64 = null;

    try {
        img64 = canvas.toDataURL('image/png');
    }
    catch (e) {
        img64 = null;
    }

    var wkt = $("#wkt").val();
    var x = $("#x").val();
    var y = $("#y").val();
    var tipoGeometria = $("#tipoGeometria").val();
    var solicitudId = $("#SolicitudId").val();
    var foficio = $("#hoficio").val();
    var idUser = $("#UserId").val();


    var params = {
        idUser: idUser,
        img64: img64,
        idsolicitud: solicitudId,
        oficio: foficio,
        x: x,
        y: y,
        wkt: wkt,
        tipogeometria: tipoGeometria,
        iddocumento: IdDocumento
    };

    $.post('../Controles/Image.ashx', params,
        function (data) {
            if (data.d == "True") {
                alert('Se ha guardado correctamente la imagen')
                location.reload(true);

            }
            else {
                alert('No se guardo correctamente la imagen.');
            }
        }, 'json');
}
