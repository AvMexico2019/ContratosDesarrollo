<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ConvenioModificatorioRegistro.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.ConvenioModificatorioRegistro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../css/EstilosEspecificos.css" rel="stylesheet" />
    <script src="../js/jsConvenioModificatorio.js?v=<%= new Random().Next(0,100000) %>"></script>
    <script src="../js/filtro.js"></script>
    <script>
        window.addEventListener("keypress", function (event) {
            if (event.keyCode == 13) {
                event.preventDefault();
            }
        }, false);
    </script>
    <style>        errorTag {
            background-color: yellow!important; border-radius:0.25em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div id="Contenedor-spinner" class="collapse"></div>

    <asp:HiddenField runat="server" ID="hdnIdPais" />
    <asp:HiddenField runat="server" ID="hdnIdEstado" />
    <asp:HiddenField runat="server" ID="hdnIdMunicipio" />
    <asp:HiddenField runat="server" ID="hdnFolio" />
    <asp:HiddenField runat="server" ID="hdnIdInmueble" />
    <asp:HiddenField runat="server" ID="hdnInstitucionPromovente" />
    <asp:HiddenField runat="server" ID="hdnIdUsuario" />

    <asp:UpdatePanel ID="UpdatePanelBusqMvtosConvenioModificatorio" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <Direccion:DireccionLectura runat="server" ID="ctrlDireccionLectura" />

            <div class="panel panel-primary">
                <div class="panel-heading">
                    Registro de un convenio modificatorio:
                    <asp:Label runat="server" ID="lblFolioContrato"></asp:Label>
                </div>

                <div class="panel-body" id="divFormulario">

                    <div id="divMensaje"></div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group datepicker-group">
                                <label class="control-label">Fecha firma de convenio<span class="form-text">*</span>:</label>
                                <input type="text" id="txtFechaInicio" placeholder="Ingresa la nueva fecha" maxlength="10" class="form-control obligatorio selectorCalendario" onblur="ValidarCampo(this);" onchange="ValidarFecha(this, 'divMensaje');" />
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">¿Tiene prorroga de vigencia?<span class="form-text">*</span>:</label>
                                <select id="ddlTieneProrroga" class="form-control obligatorio" onblur="ValidarCampo(this);" onchange="ValidarProrrogaConvenio(this, 'txtFechaTermino');">
                                    <option value="0">Selecciona</option>
                                    <option value="1">Si</option>
                                    <option value="2">No</option>
                                </select>
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group datepicker-group">
                                <label class="control-label">Fecha efecto del convenio<span class="form-text">*</span>:</label>
                                <input type="text" id="txtFechaEfectoConvenio" placeholder="Ingresa la fecha que surge efecto el convenio" maxlength="10" class="form-control obligatorio selectorCalendario" onchange="ValidarFecha(this, 'divMensaje');" />
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                         <div class="col-md-4">
                            <div class="form-group datepicker-group">
                                <label class="control-label">Fecha fin de ocupación<span class="form-text">*</span>:</label>
                                <input type="text" id="txtFechaTermino" placeholder="Ingresa la fecha fin de ocupación" maxlength="10" class="form-control selectorCalendario" disabled onchange="ValidarFecha(this, 'divMensaje');" />
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">¿Tiene nueva superfice de renta?<span class="form-text">*</span>:</label>
                                <select id="ddlTieneNuevaSuperficie" class="form-control obligatorio" onblur="ValidarCampo(this);" onchange="ValidarTieneSúperficie(this, 'txtSupM2'); ValidarSecuencial('ddlTieneNuevaSuperficie', 'ddlTieneNuevoMonto', 'txtSecuencial', 'btnSecuencial', 'txtNvoImporte');">
                                    <option value="0">Selecciona</option>
                                    <option value="1">Si</option>
                                    <option value="2">No</option>
                                </select>
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Superficie en m&#178<span class="form-text">*</span>:</label>
                                <input class="form-control" type="number" id="txtSupM2" placeholder="Ingresa la superficie en m&#178" disabled maxlength="12" />
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                            </div>
                        </div>

                        
                    </div>

                    <div class="row bottom-buffer">

                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">¿Hay incremento en el monto de pago mensual?<span class="form-text">*</span>:</label>
                                <select id="ddlTieneNuevoMonto" class="form-control obligatorio" onblur="ValidarCampo(this);" onchange="ValidarNuevoMonto(this, 'txtNvoImporte'); ValidarSecuencial('ddlTieneNuevaSuperficie', 'ddlTieneNuevoMonto', 'txtSecuencial', 'btnSecuencial', 'txtNvoImporte');">
                                    <option value="0">Selecciona</option>
                                    <option value="1">Si</option>
                                    <option value="2">No</option>
                                </select>
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Importe de renta (Sin impuestos)<span class="form-text">*</span>:</label>
                                <input class="form-control" type="number" id="txtNvoImporte" placeholder="Ingresa el importe de renta" disabled maxlength="12" />
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                            </div>
                        </div>

                        <%--<div class="col-md-4">
                            <div class="form-group datepicker-group">
                                <label class="control-label">Fecha de inicio del nuevo importe<span class="form-text">*</span>:</label>
                                <input type="text" id="txtFechaInicioNvoImporte" placeholder="Ingresa la fecha de inicio" maxlength="10" class="form-control selectorCalendario" disabled onchange="ValidarFecha(this, 'divMensaje');" />
                                <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                            </div>
                        </div>--%>

                    </div>

                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <strong>Justipreciación de rentas</strong>
                        </div>
                        <div class="panel-body">

                            <div id="divMensajeJustipreciacion"></div>

                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Secuencial<span class="form-text">*</span>:</label>
                                        <input type="text" id="txtSecuencial" class="form-control" disabled placeholder="Ingresa el secuencial" maxlength="15" onblur="ObtenerInformacionSecuencial();" />
                                        <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                    </div>
                                </div>

                                <div class="col-md-8">
                                    <div class="form-group">
                                        <br />
                                        <input class="btn btn-default" type="button" id="btnSecuencial" value="Consultar" disabled onclick="ObtenerInformacionSecuencial();" />
                                    </div>
                                </div>
                            </div>

                            <div id="divFormularioSecuencial">

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label">Institución de justipreciación:</label>
                                            <input type="text" class="form-control" id="txtInstJustipreciacion" placeholder="Ingresa la institución de justipreciación" disabled />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label">Institución del promovente:</label>
                                            <input type="text" class="form-control" id="txtInstPromovente" placeholder="Ingresa la institución del promovente" disabled />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">No. Genérico:</label>
                                            <input type="text" class="form-control" id="txtGenerico" placeholder="Ingresa el No. Genérico" disabled />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">Estatus de atención:</label>
                                            <input type="text" class="form-control" id="txtEstatusAtencion" placeholder="Ingresa el estatus de atención" disabled />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group datepicker-group">
                                            <label class="control-label">Fecha de dictamen:</label>
                                            <input type="text" class="form-control" id="txtFechaDictamen" placeholder="Ingresa la fecha de dictamen" disabled />
                                            <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">Superficie dictaminada:</label>
                                            <input type="text" class="form-control" id="txtSupDictaminada" placeholder="Ingresala superficie dictaminada" disabled />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">Unidad de la superficie:</label>
                                            <input type="text" class="form-control" id="txtUnidadSup" placeholder="Ingresa la unidad de superficie" disabled />
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">Monto dictaminado:</label>
                                            <input type="text" class="form-control" id="txtMontoDictaminado" placeholder="Ingresa el monto dictaminado" disabled />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                
                    
                    <div class="panel panel-default">

                        <script type="text/javascript">
                            //debugger;
                            document.addEventListener('ready', addFilterHandlerInput);
                        </script>

                        <div class="panel-heading">
                            <strong>Órgano Interno de Control (OIC)</strong>
                        </div>

                        <div class="panel-body">

                            <div id="divMsjOIC"></div>

                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Nombre(s)<span class="form-text">*</span>:</label>
                                        <input class="form-control obligatorio" type="text"
                                            data-filter = "yes" 
                                            data-errorMsg = "No se aceptan ni números ni carácteres especiales"
                                            data-INDAABINREGEX ="([A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+(?: [A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+)*)"
                                            id="txtNombre" placeholder="Ingresa el(los) nombre(s)"
                                            maxlength="100" onblur="ValidarCampo(this);" />
                                        <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                        <span class="form-text form-text-error"
                                            id="spanNombre" style="display: yes"></span>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Primer apellido<span class="form-text">*</span>:</label>
                                        <input class="form-control obligatorio" type="text"
                                            data-filter = "yes"
                                            data-errorMsg = "No se aceptan ni números ni carácteres especiales"
                                            data-INDAABINREGEX="([A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+(?: [A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+)*)"
                                            id="txtPApellido" placeholder="Ingresa el primer apellido"
                                            maxlength="100" onblur="ValidarCampo(this);" />
                                        <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                        <span id="spanPApellido" class="form-text form-text-error"></span>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Segundo apellido:</label>
                                        <input class="form-control" type="text"
                                            data-filter = "yes"
                                            data-errorMsg = "No se aceptan ni números ni carácteres especiales"
                                            data-INDAABINREGEX="([A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+(?: [A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+)*)"
                                            id="txtSApellido" placeholder="Ingresa el segundo apellido" maxlength="100" />
                                       <span id="spanSApellido" class="form-text form-text-error"></span>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">Nombre del cargo<span class="form-text">*</span>:</label>
                                        <input class="form-control obligatorio" type="text"
                                            data-filter = "yes"
                                            data-errorMsg = "No se aceptan ni números ni carácteres especiales"
                                            data-INDAABINREGEX="([A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+(?: [A-Za-zÁÉÍÓÚáéíóúÜüÑñ\´']+)*)"
                                            id="txtCargo" placeholder="Ingresa el nombre del cargo"
                                            maxlength="100" onblur="ValidarCampo(this);" />
                                        <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                        <span id="spanCargo" class="form-text form-text-error"></span>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">Correo electrónico<span class="form-text"></label>
                                        <input class="form-control obligatorio" type="text" id="txtCorreo" placeholder="Ingresa el correo electrónico" maxlength="100" onblur="ValidarCampo(this); ValidaCorreo(this, 'divMsjOIC');" />
                                        <small class="form-text form-text-error" style="display: none">Este campo es obligatorio</small>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group pull-right">
                                <input class="btn btn-default" value="Cancelar" type="button" onclick="RedireccionarPagina('../InmuebleArrto/BusqMvtosConvenioModificatorio.aspx');" />
                                <input class="btn btn-primary" value="Generar acuse" id="btnGenerar" type="button" onclick="GenerarAcuse();" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
