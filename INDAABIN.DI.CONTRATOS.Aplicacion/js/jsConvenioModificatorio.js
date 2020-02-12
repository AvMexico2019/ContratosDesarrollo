var MONTO_MINIMO_SECUENCIAL = 7130;

function CargaPaginaRegistroConvenio() {
    MuestraSpinner();
    document.getElementById('txtInstPromovente').value = document.getElementById('cphBody_hdnInstitucionPromovente').value;

    var botonDireccion = document.getElementById('cphBody_ctrlDireccionLectura_ButtonRegistrarInmueble');

    if (botonDireccion != null)
        botonDireccion.setAttribute('style', 'display:none');

    $('.selectorCalendario').datepicker();
    OcultaSpinner();
}

function ValidarCampo(control) {

    if (control.type == 'select-one') {
        if (control.value == 0)
            indicaCampoObligatorio(control)
        else
            restauraCampoObligatorio(control);
    }

    else {
        if (control.value == "")
            indicaCampoObligatorio(control)
        else
            restauraCampoObligatorio(control);
    }
}

function indicaCampoObligatorio(elemento) {

    if (elemento.parentNode.getElementsByTagName('span').length > 0) {
        elemento.parentNode.getElementsByTagName('span')[0].classList.add("form-text-error");
    } else {

        if (elemento.parentNode.parentNode.getElementsByTagName('span').length > 0)
            elemento.parentNode.parentNode.getElementsByTagName('span')[0].classList.add("form-text-error");
    }

    if (elemento.parentNode.getElementsByTagName('small').length > 0) {
        elemento.parentNode.getElementsByTagName('small')[0].setAttribute("style", "display:inline");
    }
    else {
        if (elemento.parentNode.parentNode.getElementsByTagName('small')[0] != null)
            elemento.parentNode.parentNode.getElementsByTagName('small')[0].setAttribute("style", "display:inline");
    }
    elemento.classList.add("form-control-error");
}

function restauraCampoObligatorio(elemento) {

    if (elemento.parentNode.getElementsByTagName('span').length > 0) {
        elemento.parentNode.getElementsByTagName('span')[0].classList.remove("form-text-error");
    } else {

        if (elemento.parentNode.parentNode.getElementsByTagName('span').length > 0)
            elemento.parentNode.parentNode.getElementsByTagName('span')[0].classList.remove("form-text-error");
    }

    if (elemento.parentNode.getElementsByTagName('small').length > 0) {
        elemento.parentNode.getElementsByTagName('small')[0].setAttribute("style", "display:none");
    }
    else {
        if (elemento.parentNode.parentNode.getElementsByTagName('small')[0] != null)
            elemento.parentNode.parentNode.getElementsByTagName('small')[0].setAttribute("style", "display:none");
    }
    elemento.classList.remove("form-control-error");
}

function MostrarMensajeDiv(clase, Mensaje, NombreDiv, tiempo) {
    try {
        if (tiempo == undefined)
            tiempo = 0;

        var dvmsj = document.getElementById(NombreDiv);
        dvmsj.setAttribute('style', 'text-align:center');

        if (dvmsj != null) {

            if (clase == '')
                dvmsj.hidden = true;
            else {
                dvmsj.hidden = false;
                if (tiempo > 0) {
                    setTimeout(function () {
                        dvmsj.hidden = true;
                    }, tiempo);
                }

            }

            dvmsj.className = clase;
            dvmsj.innerHTML = Mensaje;
            dvmsj.focus();
        }
    } catch (e) {

    }
}

function BuscarPosicionDiv(NombrePosicion) {
    var target = $('#' + NombrePosicion);
    if (target != null) {
        target = target.length && target || $('[name=' + this.hash.slice(1) + ']');
        if (target.length) {
            var targetOffset = target.offset().top;
            targetOffset = targetOffset - 70;
            $('html,body').animate({ scrollTop: targetOffset }, 1000);
            return false;
        }
    }
}

function ValidaElementos(divSeleccion) {

    var div = document.getElementById(divSeleccion);

    var elementos = div.getElementsByClassName('obligatorio');
    var primerElementoInvalido = "";
    var validar = true;

    if (elementos.length > 0) {

        for (var i = 0; i <= elementos.length - 1; i++) {

            if (elementos[i].type == "select-one" && (elementos[i].value == "0" || elementos[i].value == "" || elementos[i].childElementCount == 0)) {
                indicaCampoObligatorio(elementos[i]);
                validar = false;
                primerElementoInvalido = (primerElementoInvalido == "") ? elementos[i] : primerElementoInvalido;
            }

            else if ((!(elementos[i].value) || /^\s+$/.test(elementos[i].value)) && (elementos[i].type == "textarea" || elementos[i].type == "text" || elementos[i].type == "password" || elementos[i].type == 'time')) {
                indicaCampoObligatorio(elementos[i]);
                validar = false;
                primerElementoInvalido = (primerElementoInvalido == "") ? elementos[i] : primerElementoInvalido;
            }

            else if (elementos[i].type == 'file' && elementos[i].files.length == 0) {
                indicaCampoObligatorio(elementos[i]);
                validar = false;
                primerElementoInvalido = (primerElementoInvalido == "") ? elementos[i] : primerElementoInvalido;
            }

            else if (elementos[i].nodeName == "DIV" && elementos[i].classList.contains('radio')) {

                var validaRadio = false;
                var Lradios = elementos[i].getElementsByTagName('input');

                if (Lradios.length > 0) {
                    for (var radio = 0; radio <= Lradios.length - 1; radio++) {
                        if (Lradios[radio].checked == true) {
                            validaRadio = true;
                            break;
                        }
                    }
                }

                if (!validaRadio) {

                    indicaCampoObligatorio(elementos[i]);
                    validar = false;
                    primerElementoInvalido = (primerElementoInvalido == "") ? elementos[i] : primerElementoInvalido;
                }

                else {
                    restauraCampoObligatorio(elementos[i]);
                }

            }

            else {
                restauraCampoObligatorio(elementos[i]);
            }
        }

        return validar;
    }
}

function ValidarProrrogaConvenio(control, IdFechaTermino) {

    var fecha = document.getElementById(IdFechaTermino);
    var IdSeleccion = parseInt(control.value);

    fecha.value = '';
    fecha.setAttribute('disabled', 'disabled');
    fecha.removeAttribute('onblur');
    fecha.classList.remove('obligatorio');

    restauraCampoObligatorio(fecha);

    if (IdSeleccion == 1) {
        fecha.removeAttribute('disabled');
        fecha.classList.add('obligatorio');
        fecha.setAttribute('onblur', 'ValidarCampo(this);');
    }
}

function ValidarFecha(control, div) {

    if (control.value == '')
        return;

    var fecha = control.value;

    var RegExPattern = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/;
    if (!(fecha.match(RegExPattern))) {
        control.value = '';
        MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> El formato de la fecha es incorrecto. Favor de validar tus datos', div, 10000);
        BuscarPosicionDiv(div);
    } else {
        MostrarMensajeDiv('', '', div, 10000);
        restauraCampoObligatorio(control);
    }
}

function ValidarTieneSúperficie(control, IdSuperficie) {

    var superficie = document.getElementById(IdSuperficie);
    var IdSeleccion = parseInt(control.value);

    superficie.value = '';
    superficie.setAttribute('disabled', 'disabled');
    superficie.removeAttribute('onblur');
    superficie.classList.remove('obligatorio');
    restauraCampoObligatorio(superficie);

    if (IdSeleccion == 1) {
        superficie.removeAttribute('disabled');
        superficie.classList.add('obligatorio');
        superficie.setAttribute('onblur', 'ValidarCampo(this); VerificarValorDecimal(this, "divMensaje");');
    }
}

function VerificarValorDecimal(control, div) {

    if (control.value == '')
        return;

    var esDecimal = /^[0-9]+(\.[0-9]+)?$/.test(control.value.replace(/,/g, ''));

    MostrarMensajeDiv('', '', div, 10000);

    if (!esDecimal) {
        control.value = '';
        MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> El valor debe ser una cantidad. Favor de validar tus datos', div, 10000);
        BuscarPosicionDiv(div);
        return;
    }

    control.value = control.value.replace(',', '');    
}

function GenerarFormatoDecimal(valor) {

    if (valor.length == 0)
        return '';

    var caracteres = ['.'];

    if (caracteres.indexOf(valor) == -1)
        valor = valor + '.00';

    var entero = valor.split('.')[0];
    var decimal = 0;

    if (valor.split('.')[1] == undefined)
        decimal = ".00";
    else
        decimal = valor.split('.')[1];

    decimal = parseFloat('0.' + decimal).toFixed(2);
    var valorComas = conComas(entero) + "." + decimal.split('.')[1];

    return valorComas;
}

function conComas(valor) {
    var nums = new Array();
    var simb = ","; //Éste es el separador
    valor = valor.toString();
    valor = valor.replace(/\D/g, "");   //Ésta expresión regular solo permitira ingresar números
    nums = valor.split(""); //Se vacia el valor en un arreglo
    var long = nums.length - 1; // Se saca la longitud del arreglo
    var patron = 3; //Indica cada cuanto se ponen las comas
    var prox = 2; // Indica en que lugar se debe insertar la siguiente coma
    var res = "";

    while (long > prox) {
        nums.splice((long - prox), 0, simb); //Se agrega la coma
        prox += patron; //Se incrementa la posición próxima para colocar la coma
    }

    for (var i = 0; i <= nums.length - 1; i++) {
        res += nums[i]; //Se crea la nueva cadena para devolver el valor formateado
    }

    return res;
}

function ValidarNuevoMonto(control, IdImporte) {

    var importe = document.getElementById(IdImporte);
    //var fecha = document.getElementById(IdFecha);
    var IdSeleccion = parseInt(control.value);

    importe.value = '';
    importe.setAttribute('disabled', 'disabled');
    importe.removeAttribute('onblur');
    importe.classList.remove('obligatorio');

    //fecha.value = '';
    //fecha.setAttribute('disabled', 'disabled');
    //fecha.removeAttribute('onblur');
    //fecha.classList.remove('obligatorio');

    restauraCampoObligatorio(importe);
    //restauraCampoObligatorio(fecha);

    if (IdSeleccion == 1) {
        importe.removeAttribute('disabled');
        importe.classList.add('obligatorio');
        importe.setAttribute('onblur', 'ValidarCampo(this); VerificarValorDecimal(this, "divMensaje"); ValidarSecuencial("ddlTieneNuevaSuperficie", "ddlTieneNuevoMonto", "txtSecuencial", "btnSecuencial", "txtNvoImporte");');

        //fecha.removeAttribute('disabled');
        //fecha.classList.add('obligatorio');
        //fecha.setAttribute('onblur', 'ValidarCampo(this);');
    }
}

function ValidarSecuencial(IdNvaSup, IdNvoMonto, IdSecuencial, IdBoton, IdImporte) {

    var NvaSup = document.getElementById(IdNvaSup);
    var NvaMonto = document.getElementById(IdNvoMonto);
    var boton = document.getElementById(IdBoton);
    var secuencial = document.getElementById(IdSecuencial);
    var importe = document.getElementById(IdImporte);

    secuencial.value = '';
    secuencial.setAttribute('disabled', 'disabled');
    secuencial.removeAttribute('onblur');
    secuencial.classList.remove('obligatorio');

    secuencial.removeAttribute('data-toggle');
    secuencial.removeAttribute('data-placement');
    secuencial.removeAttribute('title');

    boton.setAttribute('disabled', 'disabled');

    var selNvaSpu = parseInt(NvaSup.value);
    var selNvoMonto = parseInt(NvaMonto.value);

    VaciarCamposDiv('divFormularioSecuencial');
    restauraCampoObligatorio(secuencial);
    document.getElementById('txtInstPromovente').value = document.getElementById('cphBody_hdnInstitucionPromovente').value;

    //if (selNvaSpu == 1 && selNvoMonto == 1 && importe.value.length > 0) {
    if (selNvoMonto == 1 && importe.value.length > 0) {

        if (parseFloat(importe.value) > MONTO_MINIMO_SECUENCIAL) {

            secuencial.removeAttribute('disabled');
            secuencial.classList.add('obligatorio');
            secuencial.setAttribute('onblur', 'ValidarCampo(this); ObtenerInformacionSecuencial();');

            secuencial.setAttribute('data-toggle', 'tooltip');
            secuencial.setAttribute('data-placement', 'top');
            secuencial.setAttribute('title', 'La estructura del secuencial es 00-00-0000');

            boton.removeAttribute('disabled');

            $('[data-toggle="tooltip"]').tooltip();
        }
    }
}

function VaciarCamposDiv(IdDiv) {

    var div = document.getElementById(IdDiv);

    if (div != null) {

        var lcampos = div.getElementsByClassName('form-control');

        if (lcampos.length == 0)
            return;

        for (var c = 0; c <= lcampos.length - 1; c++) {

            if (lcampos[c].type == 'select-one')
                lcampos[c].value = 0;
            else
                lcampos[c].value = '';
        }
    }
}

function ValidarEstructuraSecuencial(control) {

    var secuencial = control.value;
    var lnumeros = secuencial.split('-');

    if (secuencial.length == 0)
        return false;

    if (lnumeros.length != 3) {
        MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> La estructura del secuencial no es correcta. Favor de validar tus datos', 'divMensajeJustipreciacion', 10000);
        BuscarPosicionDiv('divMensajeJustipreciacion');
        control.value = '';
        indicaCampoObligatorio(control);
        return false;
    }

    for (var num = 0; num <= lnumeros.length - 1; num++) {

        var valido = ValidarCadenaNumeros(lnumeros[num]);

        if (valido == false) {
            MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> La estructura del secuencial no es correcta. Favor de validar tus datos', 'divMensajeJustipreciacion', 10000);
            BuscarPosicionDiv('divMensajeJustipreciacion');
            control.value = '';
            indicaCampoObligatorio(control);
            return false;
        }
    }

    return true;
}

function ValidarCadenaNumeros(cadNumeros) {

    var numeros = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

    if (cadNumeros.length == 0)
        return false;

    for (var cad = 0; cad <= cadNumeros.length - 1; cad++) {

        if (numeros.indexOf(cadNumeros[cad]) == -1) {
            return false;
        }
    }

    return true;
}


function ValidaCorreo(control, div) {

    MostrarMensajeDiv('', '', div, 10000);

    if (control.value.length == 0)
        return;

    if (!ValidarCorreoElectronico(control.value)) {
        MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> La estructura de tu correo electrónico no es valida. Favor de validar tus datos', div, 10000);
        BuscarPosicionDiv(div);
        control.value = '';
        indicaCampoObligatorio(control);
    }
}

function ValidarCorreoElectronico(valor) {
    //return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/.test(valor);
    return /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(valor);
}

function ObtenerInformacionSecuencial() {

    try {

        MuestraSpinner();

        var txtSecuencial = document.getElementById('txtSecuencial');
        MostrarMensajeDiv('', '', 'divMensajeJustipreciacion', 10000);

        VaciarCamposDiv('divFormularioSecuencial');

        if (txtSecuencial.value.length == 0) {
            MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> Favor de ingresar el secuencial para realizar la búsqueda', 'divMensajeJustipreciacion', 10000);
            indicaCampoObligatorio(txtSecuencial);
            OcultaSpinner();
            return;
        }

        if (!ValidarEstructuraSecuencial(txtSecuencial)) {
            OcultaSpinner();
            return;
        }

        var json = {};
        json.secuencial = txtSecuencial.value;
        json.IdPais = parseInt(document.getElementById('cphBody_hdnIdPais').value);
        json.IdEstado = parseInt(document.getElementById('cphBody_hdnIdEstado').value);
        json.IdMunicipio = parseInt(document.getElementById('cphBody_hdnIdMunicipio').value);
        json.IdInmueble = parseInt(document.getElementById('cphBody_hdnIdInmueble').value);

        var Json = JSON.stringify(json);

        $.ajax({
            url: "../Ajax/A_Convenio.aspx/ObtenerJustipreciacionSecuencial",
            data: Json,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (resp) {

                var Respuesta = resp.d;

                if (Respuesta.respuesta == true) {

                    LlenarInformacionJustipreciacion(Respuesta.Justipreciacion);
                    OcultaSpinner();

                } else {

                    var Mensaje = Respuesta.Mensaje;

                    if (Mensaje.length == 0)
                        Mensaje = 'Hubo un problema al realizar la búsqueda del secuencial. Favor de contactar a tu administrador';

                    MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMensajeJustipreciacion', 10000);
                    BuscarPosicionDiv('divMensajeJustipreciacion');
                    txtSecuencial.value = '';
                    OcultaSpinner();
                }

            }, error: function (err) {

                var Mensaje = '';

                if (err.responseJSON == undefined)
                    Mensaje = 'Hubo un problema al realizar la búsqueda del secuencial. Favor de contactar a tu administrador';
                else
                    Mensaje = err.responseJSON.Message;

                txtSecuencial.value = '';
                MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMensajeJustipreciacion', 10000);
                BuscarPosicionDiv('divMensajeJustipreciacion');
                OcultaSpinner();
            }
        });
    }

    catch (error) {
        MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + error, 'divMensajeJustipreciacion', 10000);
        BuscarPosicionDiv('divMensajeJustipreciacion');
        OcultaSpinner();
    }
}

function LlenarInformacionJustipreciacion(Justipreciacion) {

    document.getElementById('txtInstJustipreciacion').value = Justipreciacion.InstitucionJustipreciacion;
    document.getElementById('txtInstPromovente').value = document.getElementById('cphBody_hdnInstitucionPromovente').value;
    document.getElementById('txtGenerico').value = Justipreciacion.NoGenerico;
    document.getElementById('txtEstatusAtencion').value = Justipreciacion.EstatusAtencion;
    document.getElementById('txtFechaDictamen').value = Justipreciacion.descFechaDictamen;
    document.getElementById('txtSupDictaminada').value = Justipreciacion.SuperficieDictaminada;
    document.getElementById('txtUnidadSup').value = Justipreciacion.UnidadMedidaSupRentableDictaminada;
    document.getElementById('txtMontoDictaminado').value = Justipreciacion.MontoDictaminado;
}

function GenerarAcuse() {

    try {

        MuestraSpinner();

        if (!ValidaElementos('divFormulario')) {
            MostrarMensajeDiv('alert alert-warning', '<strong>¡Atención!</strong> Te hacen falta campos por capturar', 'divMensaje', 10000)
            BuscarPosicionDiv('divMensaje');
            OcultaSpinner();
            return;
        }

        var json = {};
        json.IdUsuario = parseInt(document.getElementById('cphBody_hdnIdUsuario').value);
        json.Convenio = {};
        json.Convenio.FolioContrato = parseInt(document.getElementById('cphBody_hdnFolio').value);
        json.Convenio.descFechaConvenio = document.getElementById('txtFechaInicio').value;
        json.Convenio.TieneProrroga = parseInt(document.getElementById('ddlTieneProrroga').value);
        json.Convenio.descFechaTermino = document.getElementById('txtFechaTermino').value;
        json.Convenio.TieneNvaSuperfice = parseInt(document.getElementById('ddlTieneNuevaSuperficie').value);
        json.Convenio.SupM2 = document.getElementById('txtSupM2').value == '' ? 0 : document.getElementById('txtSupM2').value;
        json.Convenio.TieneNvoMonto = parseInt(document.getElementById('ddlTieneNuevoMonto').value);
        json.Convenio.ImporteRenta = document.getElementById('txtNvoImporte').value == '' ? 0 : document.getElementById('txtNvoImporte').value;
        json.Convenio.DescFechaEfectoConvenio = document.getElementById('txtFechaEfectoConvenio').value;
        json.Convenio.Secuencial = document.getElementById('txtSecuencial').value;
        json.Convenio.NombreOIC = document.getElementById('txtNombre').value;
        json.Convenio.PApellidoOIC = document.getElementById('txtPApellido').value;
        json.Convenio.SApellidoOIC = document.getElementById('txtSApellido').value;
        json.Convenio.CargoOIC = document.getElementById('txtCargo').value;
        json.Convenio.CorreoOIC = document.getElementById('txtCorreo').value;

        json.JustripreciacionContrato = {};
        json.JustripreciacionContrato.MontoDictaminado = document.getElementById('txtMontoDictaminado').value;
        json.JustripreciacionContrato.EstatusAtencion = document.getElementById('txtEstatusAtencion').value;
        json.JustripreciacionContrato.NoGenerico = document.getElementById('txtGenerico').value;
        json.JustripreciacionContrato.UnidadMedidaSupRentableDictaminada = document.getElementById('txtUnidadSup').value;
        json.JustripreciacionContrato.SuperficieDictaminada = document.getElementById('txtSupDictaminada').value;
        json.JustripreciacionContrato.descFechaDictamen = document.getElementById('txtFechaDictamen').value;

        json.Institucion = document.getElementById('cphBody_hdnInstitucionPromovente').value;
        json.IdInmueble = parseInt(document.getElementById('cphBody_hdnIdInmueble').value);

        var Json = JSON.stringify(json);

        $.ajax({
            url: "../Ajax/A_Convenio.aspx/GenerarRegistroConvenio",
            data: Json,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (resp) {

                var Respuesta = resp.d;

                if (Respuesta.respuesta == true) {

                    LimpiarPantallaCaptura();
                    window.open(Respuesta.Url, '_blank');
                    MostrarMensajeDiv('alert alert-success', '<strong>¡Felicidades!</strong> El convenio modificatorio se registró correctamente', 'divMensaje', 10000);
                    BuscarPosicionDiv('divMensaje');
                    OcultaSpinner();

                } else {

                    var Mensaje = Respuesta.Mensaje;

                    if (Mensaje.length == 0)
                        Mensaje = 'Hubo un problema al generar el registro del convenio modificatorio. Favor de contactar a tu administrador';

                    MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMensaje', 10000);
                    BuscarPosicionDiv('divMensaje');
                    OcultaSpinner();
                }

            }, error: function (err) {

                var Mensaje = '';

                if (err.responseJSON == undefined)
                    Mensaje = 'Hubo un problema al generar el registro del convenio modificatorio. Favor de contactar a tu administrador';
                else
                    Mensaje = err.responseJSON.Message;

                MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMensaje', 10000);
                BuscarPosicionDiv('divMensaje');
                OcultaSpinner();
            }
        });

    }

    catch (error) {
        MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + error, 'divMensaje', 10000);
        BuscarPosicionDiv('divMensaje');
        OcultaSpinner();
    }
}

function MuestraSpinner() {
    var Spinner = document.getElementById("Contenedor-spinner");
    Spinner.innerHTML = "<div id='Spinner' class = 'spinner'><img src='../Imagenes/ajax-loader.gif' /></div>";
    Spinner.classList.add("in");
}

function OcultaSpinner() {
    var Spinner = document.getElementById("Contenedor-spinner");
    Spinner.innerHTML = "";
    Spinner.classList.remove("in");
}

function RedireccionarPagina(url) {

    MuestraSpinner();

    if (url.length == 0) {
        OcultaSpinner();
        return;
    }

    window.location.href = url;
}

function LimpiarPantallaCaptura() {

    var div = document.getElementById('divFormulario');
    var lcampos = div.getElementsByClassName('form-control');

    for (var campo = 0; campo <= lcampos.length - 1; campo++) {

        if (lcampos[campo].type == 'select-one')
            lcampos[campo].value = 0;
        else
            lcampos[campo].value = '';
    }

    ValidarProrrogaConvenio(document.getElementById('ddlTieneProrroga'), 'txtFechaTermino');
    ValidarTieneSúperficie(document.getElementById('ddlTieneNuevaSuperficie'), 'txtSupM2');
    ValidarNuevoMonto(document.getElementById('ddlTieneNuevoMonto'), 'txtNvoImporte');
    ValidarSecuencial('ddlTieneNuevaSuperficie', 'ddlTieneNuevoMonto', 'txtSecuencial', 'btnSecuencial', 'txtNvoImporte');
}

function ObtenerConveniosModificatorios(folioContrato) {

    try {

        MuestraSpinner();

        if (folioContrato == null) {
            MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> No se encuentra el registro del contrato. Favor de contactar a tu administrador', 'divMensaje', 10000);
            BuscarPosicionDiv('divMensaje');
            OcultaSpinner();
            return;
        }

        if (folioContrato == 0) {
            MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> No se encuentra el registro del contrato. Favor de contactar a tu administrador', 'divMensaje', 10000);
            BuscarPosicionDiv('divMensaje');
            OcultaSpinner();
            return;
        }

        var json = {};
        json.FolioContrato = folioContrato;

        var Json = JSON.stringify(json);

        $.ajax({
            url: "../Ajax/A_Convenio.aspx/ObtenerConveniosModificatorios",
            data: Json,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (resp) {

                var Respuesta = resp.d;

                if (Respuesta.respuesta == true) {

                    LLenarTablaConvenios('tblConvenios', Respuesta.Lconvenio);

                    MostrarModal('mdlConvenios');
                    OcultaSpinner();

                } else {

                    var Mensaje = Respuesta.Mensaje;

                    if (Mensaje.length == 0)
                        Mensaje = 'Hubo un problema al obtener la lista de convenios modificatorios. Favor de contactar a tu administrador';

                    MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMensaje', 10000);
                    BuscarPosicionDiv('divMensaje');
                    OcultaSpinner();
                }

            }, error: function (err) {

                var Mensaje = '';

                if (err.responseJSON == undefined)
                    Mensaje = 'Hubo un problema al obtener la lista de convenios modificatorios. Favor de contactar a tu administrador';
                else
                    Mensaje = err.responseJSON.Message;

                MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMensaje', 10000);
                BuscarPosicionDiv('divMensaje');
                OcultaSpinner();
            }
        });

    }

    catch (error) {
        MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + error, 'divMensaje', 10000);
        BuscarPosicionDiv('divMensaje');
        OcultaSpinner();
    }


}

function MostrarModal(modal) {
    $('#' + modal).modal('show');
}

function OcultaModal(modal) {
    $('#' + modal).modal('hide');
}

function LLenarTablaConvenios(IdTabla, Lconvenio) {

    var tabla = document.getElementById(IdTabla);

    for (var i = 0; i <= tabla.childNodes.length - 1; i++) {

        if (tabla.childNodes[i].nodeName.toUpperCase() == "TBODY") {
            tabla.childNodes[i].innerHTML = '';

            if (Lconvenio.length == 0) {

                var trSRegistro = document.createElement('tr');
                var tdSRegistro = document.createElement('td');

                tdSRegistro.innerHTML = 'No se encontraron registros';
                tdSRegistro.setAttribute('colspan', '2');

                trSRegistro.appendChild(tdSRegistro);
                tabla.childNodes[i].appendChild(trSRegistro);

                return;
            }

            for (var c = 0; c <= Lconvenio.length - 1; c++) {

                var tr = document.createElement('tr');

                var tdFolio = document.createElement('td');
                var tdLnk = document.createElement('td');

                tdFolio.setAttribute('style', 'text-align:center');
                tdLnk.setAttribute('style', 'text-align:center');

                tdFolio.innerText = Lconvenio[c].FolioConvenio;

                var lnk = document.createElement('a');
                lnk.innerText = 'Abrir acuse';
                lnk.setAttribute('onclick', 'ObtenerAcuseConvenio("' + Lconvenio[c].FolioConvenio + '");')
                lnk.setAttribute('class', 'link.email');
                lnk.setAttribute('style', 'cursor:pointer;');

                tdLnk.appendChild(lnk);

                tr.appendChild(tdFolio);
                tr.appendChild(tdLnk);

                tabla.childNodes[i].appendChild(tr);
            }
        }
    }
}

function ObtenerAcuseConvenio(folioConvenio) {

    try {

        MuestraSpinner();

        if (folioConvenio.length == 0) {
            MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> No se encuentra el registro del convenio. Favor de contactar a tu administrador', 'divMsjconvenios', 10000);
            OcultaSpinner();
            return;
        }

        var json = {};
        json.folioConvenio = folioConvenio;

        var Json = JSON.stringify(json);

        $.ajax({
            url: "../Ajax/A_Convenio.aspx/ObtenerAcuseConvenio",
            data: Json,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (resp) {

                var Respuesta = resp.d;

                if (Respuesta.respuesta == true) {

                    window.open(Respuesta.Url, '_blank');
                    OcultaSpinner();

                } else {

                    var Mensaje = Respuesta.Mensaje;

                    if (Mensaje.length == 0)
                        Mensaje = 'Hubo un problema al obtener el convenio modificatorio. Favor de contactar a tu administrador';

                    MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMsjconvenios', 10000);                    
                    OcultaSpinner();
                }

            }, error: function (err) {

                var Mensaje = '';

                if (err.responseJSON == undefined)
                    Mensaje = 'Hubo un problema al obtener el convenio modificatorio. Favor de contactar a tu administrador';
                else
                    Mensaje = err.responseJSON.Message;

                MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + Mensaje, 'divMsjconvenios', 10000);                
                OcultaSpinner();
            }
        });
    }

    catch (error) {
        MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + error, 'divMsjconvenios', 10000);        
        OcultaSpinner();
    }
}

function MarcarErrorPagina(TipoError) {

    var mensaje = '';

    switch (TipoError) {

        case 'Aplicacion':
            mensaje = 'No se encuentra el registro de la aplicación. Favor de contactar a tu administrador';
            break;
        case 'Roles':
            mensaje = 'No se encontraron roles configurados para tu usuario. Favor de contactar a tu administrador';
            break;
        case 'Permiso':
            mensaje = 'Tu usuario no cuenta con los permisos necesarios para generar un convenio modificatorio. Favor de contactar a tu administrador';
            break;
    }

    document.getElementById('btnGenerar').setAttribute('style', 'display:none');
    MostrarMensajeDiv('alert alert-danger', '<strong>¡Problemas!</strong> ' + mensaje, 'divMensaje', 10000);
    BuscarPosicionDiv('divMensaje');
}