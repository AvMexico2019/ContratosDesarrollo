
$(document).ready(function () {

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    //funcion que devuelve la instancia a la pagina, esto es para cuando se hace un postback que no se pierda la fecha
    function EndRequestHandler(sender, args)
    {
        $('#cphBody_calendarInicio').datepicker();
        $('#cphBody_calendarFin').datepicker();

        //ocultamos las fechas desde el inicio
        $('#CampoFechas').hide();


        //funcion para que de acuerdo a la seleccion se muestre o se oculte la pantalla de fechas
        $('.Fechas').change(function () {

            if (!$(this).prop('checked')) {
                $('#CampoFechas').hide();
                $('#cphBody_calendarInicio').val('');
                $('#cphBody_calendarFin').val('');

            } else {

                $('#CampoFechas').show();
            }
        })

    }

    $('#cphBody_calendarInicio').datepicker();
    $('#cphBody_calendarFin').datepicker();

    //ocultamos las fechas desde el inicio
    $('#CampoFechas').hide();


    //funcion para que de acuerdo a la seleccion se muestre o se oculte la pantalla de fechas
    $('.Fechas').change(function () {

        if (!$(this).prop('checked')) {
            $('#CampoFechas').hide();
            $('#cphBody_calendarInicio').val('');
            $('#cphBody_calendarFin').val('');

        } else {

            $('#CampoFechas').show();
        }
    })
});


//funcion para la validacion de RIUF
function validaRiuf(control) {
    var val = control.value.replace(/\D/g, '');
    var newVal = '';

    if (val.length > 3) {
        control.value = val;
    }
    if ((val.length > 2) && (val.length < 8)) {
        newVal += val.substr(0, 2) + '-';
        val = val.substr(2);
    }
    if (val.length > 7) {
        newVal += val.substr(0, 2) + '-';
        newVal += val.substr(2, 5) + '-';
        val = val.substr(7, 1);
    }
    newVal += val;
    control.value = newVal;
}

//funcion para habilitar y deshabilitar estado, municipio y CP
function HabilitarCasillas(selectedvalue)
{
    if(selectedvalue != "165")
    {
        document.getElementById("cphBody_DropDownListEstado").setAttribute("disabled", "false");
        document.getElementById("cphBody_DropDownListMunicipio").setAttribute("disabled", "false");
        document.getElementById("cphBody_txtCP").setAttribute("disabled", "false");

        //limpiamos las casillas de la seleccion
        document.getElementById("cphBody_DropDownListEstado").value = "--";
        document.getElementById("cphBody_DropDownListMunicipio").value = "--";
        document.getElementById("cphBody_txtCP").value = "";
    }
    else
    {
        document.getElementById("cphBody_DropDownListEstado").removeAttribute("disabled");
        document.getElementById("cphBody_DropDownListMunicipio").removeAttribute("disabled");
        document.getElementById("cphBody_txtCP").removeAttribute("disabled");
    }
}


