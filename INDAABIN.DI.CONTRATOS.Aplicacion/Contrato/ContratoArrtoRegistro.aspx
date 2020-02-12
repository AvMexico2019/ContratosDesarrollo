<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ContratoArrtoRegistro.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.ContratoArrtoRegistro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <script type="text/javascript">
        function actualizaPantalla() {
            window.location = '../InmuebleArrto/BusqMvtosContratosInmuebles.aspx';
        }

        window.onbeforeunload = function () { return 'Es posible que los cambios no se guarden, ¿Confirmas que deseas continuar con ésta acción? '; }

        function abreDomicilios() {
            var valorInmuebleArrendamiento = $('#cphBody_lblIdInmuebleArrendamiento').val();
            var response = new Array('', '', '', '', '', '');
            window.open('../InmuebleArrto/DireccionInmueble.aspx?IdInmuebleArrendamiento=' + valorInmuebleArrendamiento, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1080, height = 650', 'true');
        }

        function DeshabilitaControl(IdControl) {

            var control = document.getElementById(IdControl);

            if (control != null) {
                control.setAttribute('style', 'display:none');
            }
        }

        function asignaValores(responseCatched) {
            var response = new Array("", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            response = responseCatched;
            if (response != null) {
                document.getElementById('cphBody_TextBoxRIUF').value = response[0];
                document.getElementById('cphBody_lblIdInmuebleRiuf').value = response[15];
                document.getElementById("cphBody_btnValidaRIUF").click();
            }
        }

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $("#<%= TextBoxFechaIOcupacion.ClientID %>").datepicker();
                $("#<%= TextBoxFechaFOcupacion.ClientID %>").datepicker();
                $("#<%= txtFechaDictamen.ClientID %>").datepicker();
            }
            $("#<%= TextBoxFechaIOcupacion.ClientID %>").datepicker();
            $("#<%= TextBoxFechaFOcupacion.ClientID %>").datepicker();
            $("#<%= txtFechaDictamen.ClientID %>").datepicker();
        });

        function WebForm_OnSubmit12() {
            debugger;
            var validationGroupName = 'Contratos';

            ValidarTipoContratacion(document.getElementById('cphBody_DropDownListTipoContratacion'));

            if (!Page_ClientValidate(validationGroupName)) {
                for (var i in Page_Validators) {
                    try {
                        if (Page_Validators[i].validationGroup == validationGroupName) {
                            var control = document.getElementById(Page_Validators[i].controltovalidate);
                            if (!Page_Validators[i].isvalid && !Page_Validators[i].disabled) {
                                control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                                control.parentElement.className = "form-group has-error";
                            } else {
                                control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                                control.parentElement.className = "form-group";
                            }
                        }
                    } catch (e) { }
                }
                return false;
            }
            return true;
        }

        function defineValidacionInstitucion() {
            controlValidateName = 'cphBody_TextBoxInstitucionJustipreciacion';
            controlCheckName = 'cphBody_CheckBoxInstitucion';
            var validationGroupName = 'Contratos';
            for (var i in Page_Validators) {
                try {
                    if (Page_Validators[i].controltovalidate == controlValidateName) {
                        Page_Validators[i].disabled = document.getElementById(controlCheckName).checked;
                        Page_Validators[i].isvalid = document.getElementById(controlCheckName).checked;
                        Page_Validators[i].hidden = document.getElementById(controlCheckName).checked;
                        var control = document.getElementById(controlValidateName);
                        if (document.getElementById(controlCheckName).checked) {
                            control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                            control.parentElement.className = "form-group";
                            Page_Validators[i].errormessage = '';
                            Page_Validators[i].innerHTML = '';
                            Page_Validators[i].innerText = '';
                            Page_Validators[i].outerText = '';
                            Page_Validators[i].textContent = '';

                        }
                        else {
                            control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                            control.parentElement.className = "form-group has-error";
                            Page_Validators[i].errormessage = 'Este campo debe coincidir con la institución del promovente';
                            Page_Validators[i].errormessage = 'Este campo debe coincidir con la institución del promovente';
                            Page_Validators[i].innerHTML = 'Este campo debe coincidir con la institución del promovente';
                            Page_Validators[i].innerText = 'Este campo debe coincidir con la institución del promovente';
                            Page_Validators[i].outerText = 'Este campo debe coincidir con la institución del promovente';
                            Page_Validators[i].textContent = 'Este campo debe coincidir con la institución del promovente';
                        }
                    }
                } catch (e) { }
            }
        }


        function backFromErrorClass(control) {
            if (control != null) {
                if (control.value != ' ' && control.value != '' && control.value != '--' && control.value != '0') {
                    control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                    control.parentElement.className = "form-group";
                }
                else {
                    control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                    control.parentElement.className = "form-group has-error";
                }
            }
        }  


        function ValidarTipoContratacion(control) {
            //rfv.setAttribute('validationGroup', 'Contratos');

            //if (seleccion == '5' || seleccion == '8' || seleccion == '11') {
            //    rfv.setAttribute('validationGroup', 'ContratosNV');
            //    return;
            //}
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" id="lblIdEmisionOpinion" runat="server" />
            <input type="hidden" id="lblIdContrato" runat="server" />
            <input type="hidden" id="lblIdInmuebleArrendamiento" runat="server" />
            <input type="hidden" id="lblIdInmuebleRiuf" runat="server" />
            <input type="hidden" id="lblIdInstitucion" runat="server" />
            <input type="hidden" id="lblNombreInstitucion" runat="server" />
            <input type="hidden" id="lblNombreInstitucionActual" runat="server" />
            <input type="hidden" id="lblDireccionInmuebleArrendamiento" runat="server" />
            <input type="hidden" id="lblEsSustitucion" runat="server" />
            <input type="hidden" id="lblEsContinuacion" runat="server" />

            <input type="hidden" id="lblEstadoEmision" runat="server" />
            <input type="hidden" id="lblMunicpioEmision" runat="server" />
            <input type="hidden" id="lblCPEmision" runat="server" />

            <input type="hidden" id="lblEstadoJustipreciacion" runat="server" />
            <input type="hidden" id="lblMunicpioJustipreciacion" runat="server" />
            <input type="hidden" id="lblCPJustipreciacion" runat="server" />

            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <Direccion:DireccionLectura runat="server" ID="ctrlDireccionLectura" />
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:Label ID="LabelInfoEncabezadoPanelPrincipal" runat="server"></asp:Label>
                </div>
                <asp:Panel ID="pnlControles" runat="server">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group" style="text-align: center;">
                                    <br />
                                    <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Tipificación del arrendamiento:
                        <asp:Label ID="LabelTipificacionArrto" runat="server" Style="font-weight: 700"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="control-label">Tipo de arrendamiento*:</span>
                                            <asp:DropDownList ID="DropDownListTipoArrandamiento" runat="server" CssClass="form-control" Enabled="False">
                                                <asp:ListItem Value="1">Nuevo</asp:ListItem>
                                                <asp:ListItem Value="2">Continuación</asp:ListItem>
                                                <asp:ListItem Value="3">Sustitución</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Tipo de contratación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList ID="DropDownListTipoContratacion" CausesValidation="true" AutoPostBack="True" OnSelectedIndexChanged="DropDownListTipoContratacion_SelectedIndexChanged" onchange="backFromErrorClass(this); ValidarTipoContratacion(this);" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvDropDownListTipoContratacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoContratacion" Display="Dynamic" InitialValue="0" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <!-- Eliminado a petición del control de cambios del 19 de noviembre del 2019 Desa21
                            <div class="panel-heading">
                                <strong>Justipreciación de rentas</strong>
                                <br />
                                Proporciona el secuencial correspondiente al avalúo de justipreciación de rentas y haz clic en consultar
                                <br />
                                Nota: El Importe mínimo a partir del que deberá contar con un secuencial de justipreciación de renta es $ 
                                <asp:Label ID="LabelMontoMinimoRentaParaJustipreciacion" Text="--" runat="server"></asp:Label>
                            </div>
                            -->
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Secuencial:</span>
                                            <asp:TextBox ID="TextBoxSecuencialJust" runat="server" AutoPostBack="True" CssClass="form-control" MaxLength="20" OnTextChanged="TextBoxSecuencialJust_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxSecuencialJust" ValidationGroup="Contratos" Enabled="True" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxSecuencialJust" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="ButtonObtenerJustipreciacion" runat="server" Text="Consultar" class="btn btn-primary" OnClick="ButtonObtenerJustipreciacion_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Institución de justipreciación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxInstitucionJustipreciacion" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                            <%--RCA 2/08/2017 inconsistencia en la validacion de compraciondel nombre actual con el nombre de justipreciacion--%>
                                            <%--<asp:CompareValidator ID="cvTextBoxInstitucionJustipreciacion" runat="server" ControlToCompare="TextBoxInstitucionActual" ValidationGroup="Contratos" ErrorMessage="Este campo debe coincidir con la institución del promovente" ControlToValidate="TextBoxInstitucionJustipreciacion" Display="Dynamic" CssClass="error text-danger" ></asp:CompareValidator>--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <span class="control-label">Institución de promovente:</span>
                                            <asp:TextBox ID="TextBoxInstitucionActual" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlInstitucion" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="alert alert-warning">
                                                <span class="control-label">Las instituciones no conciden en su denominación:</span><br />
                                                <asp:CheckBox ID="CheckBoxInstitucion" runat="server" AutoPostBack="false" onchange="defineValidacionInstitucion();" Text="&nbsp;&nbsp;Confirmar el uso de esta justipreciación" ToolTip="Selecciona para confirmar el uso de ésta justipreciación" CausesValidation="false" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">No. Genérico:</span>
                                            <asp:TextBox ID="TextBoxGenericoJust" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Estatus de atención:</span>
                                            <asp:TextBox ID="TextBoxEstatusAttJust" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Fecha de dictamen:</span>
                                            <asp:TextBox ID="TextBoxFechaDictamenJust" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Superficie dictaminada:</span>
                                            <asp:TextBox ID="TextBoxSupDictaminadaJust" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Unidad de medida de la superficie:</span>
                                            <asp:TextBox ID="TextBoxUnidadMedidaSup" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>

                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Monto dictaminado:</span>
                                            <asp:TextBox ID="TextBoxMontoDictaminadoJust" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="LabelInfoSecuencialJust" runat="server" Font-Bold="False"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <strong>Seguridad estructural </strong>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="control-label">Cuenta con dictamen de seguridad estructural</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList runat="server" ID="drpCuentaConDictamenSeguridad" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpCuentaConDictamenSeguridad_SelectedIndexChanged">
                                                <asp:ListItem Text="No" Value="0" Selected="True" />
                                                <asp:ListItem Text="Si" Value="1" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="pnlSeguridadEstructural" visible="false">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Fecha emisión del dictamen:</span>
                                        </div>
                                        <div class="form-group datepicker-group">

                                            <asp:TextBox CssClass="form-control" type="text" runat="server" ID="txtFechaDictamen" placeholder="dd/mm/aa" />
                                            <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6"></div>
                                </div>
                            </div>
                        </div>

                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <strong>Conceptos de cumplimiento normativo</strong>
                                <br />
                                Proporciona el folio de emisión de opinión y haz clic en consultar, para verificar su validez y aplicarlo, si no cuentas con él, puedes registrarlo ahora en: Solicitar
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Folio de emisión de opinión:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxFolioOpinion" runat="server" onblur="backFromErrorClass(this);" placeholder="Sólo números" CssClass="form-control" MaxLength="10" OnTextChanged="TextBoxFolioOpinion_TextChanged" ToolTip="proporciona el  folio de emisión de opinión ó da clic en Consultar si no recuerda cual es" AutoPostBack="True"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFolioOpinion" runat="server" TargetControlID="TextBoxFolioOpinion" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxFolioOpinion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFolioOpinion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="ButtonConsultarFolioOpinion" runat="server" Text="Consultar" class="btn btn-primary" OnClick="ButtonConsultarFolioOpinion_Click" Visible="false" />
                                            <asp:Button ID="ButtonIrRegistrarOpinion" runat="server" Text="Solicitar" class="btn btn-default" OnClick="ButtonIrRegistrarOpinion_Click" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="LabelDireccionSustitucion" runat="server" Font-Bold="False"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="LabelInfoFolioOpinion" runat="server" Font-Bold="False"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="text-align: center">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Button ID="ButtonSinFolioOpinion" runat="server" class="btn btn-default" Text="Continuar sin folio" OnClick="ButtonSinFolioOpinion_Click" Visible="False" ToolTip="Cancelar la opción de proporcionar un Folio de Emisión de Opinión"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DatosTablaSMOI" class="row">

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="control-label">Superficie Máxima a Ocupar por Institución (SMOI):</span>
                                            <asp:Label ID="LabelTotalSMOI" runat="server" CssClass="form-control" Text="00.00" ReadOnly="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="control-label">Número de dictamen excepción si no se cuenta con SMOI:</span>
                                            <asp:TextBox ID="TextBoxNumDictamenExcepcionSMOI" runat="server" CssClass="form-control" Text="" MaxLength="30" ToolTip="proporciona este # de dictamen proporcionado por el INDAABIN, cuando no cuenta con un folio de SMOI "></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading"><strong>El Inmueble en arrendamiento </strong></div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Propietario del inmueble:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxPropietarioInmueble" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTextBoxPropietarioInmueble" runat="server" TargetControlID="TextBoxPropietarioInmueble" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz123456789.;-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxPropietarioInmueble" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxPropietarioInmueble" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre del funcionario responsable:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxFuncionarioResp" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFuncionarioResp" runat="server" TargetControlID="TextBoxFuncionarioResp" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxFuncionarioResp" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFuncionarioResp" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">RIUF:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxRIUF" CssClass="form-control" onchange="backFromErrorClass(this);" placeholder="##-#####-#" runat="server" MaxLength="10" ToolTip="Registro de Inmueble en Uso de la Federación" Enabled="False"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderRIUF" runat="server" TargetControlID="TextBoxRIUF" ValidChars="0123456789-"></cc1:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rfvTextBoxRIUF" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxRIUF" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <br />
                                            <asp:Panel ID="PanelBuscaDomicilios" runat="server" Visible="false">
                                                <input id="btnLanzaDomicilios" runat="server" type="button" onclick="abreDomicilios();" value="Consultar" class="btn btn-primary" tooltip="Da clic para consultar el RIUF del Inmueble" />
                                                &nbsp;
                                            <asp:CheckBox ID="CheckBoxGenerarRIUF" Visible="false" runat="server" Text="&nbsp;&nbsp;Generar nuevo RIUF" ToolTip="Seleccionar para genera RIUF al domicilio del Inmueble" AutoPostBack="True" OnCheckedChanged="CheckBoxGenerarRIUF_CheckedChanged" />
                                            </asp:Panel>
                                            <asp:Button ID="btnValidaRIUF" runat="server" Text="Button" OnClick="btnValidaRIUF_Click" Style="visibility: hidden" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="LabelInfoInmuebleArrendamiento" runat="server" Font-Bold="False"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Uso genérico del inmueble:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList ID="DropDownListTipoUsoInmueble" runat="server" CssClass="form-control" AutoPostBack ="true"  OnSelectedIndexChanged="DropDownListTipoUsoInmueble_SelectedIndexChanged"></asp:DropDownList>         
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvDropDownListTipoUsoInmueble" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoUsoInmueble" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger"   />
                                    </div>

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-group">
                                                <div class="form-group-required-as">
                                                    <span class="control-label">Uso específico del inmueble:</span>
                                                </div>
                                                <div class="form-group">
                                                    <asp:DropDownList ID="DropDownListTipoUsoEspecificoInmueble"  runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <asp:RequiredFieldValidator ID="rfvDropDownListTipoUsoEspecificoInmueble" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoUsoEspecificoInmueble" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger"  />
                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>    
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <strong>Datos de contratación </strong>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Fecha inicio ocupación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxFechaIOcupacion" onblur="backFromErrorClass(this);" onchange="backFromErrorClass(this);" runat="server" placeholder="dd/mm/aa" CssClass="form-control" ToolTip="Fecha Inicio de la contratación" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaIOcupacion" runat="server" TargetControlID="TextBoxFechaIOcupacion" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxFechaIOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFechaIOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Fecha fin de ocupación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxFechaFOcupacion" onblur="backFromErrorClass(this);" onchange="backFromErrorClass(this);" placeholder="dd/mm/aa" runat="server" CssClass="form-control" ToolTip="Fecha de Fin de la contratación" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaFOcupacion" runat="server" TargetControlID="TextBoxFechaFOcupacion" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxFechaFOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFechaFOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Área ocupada M2:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxAreaOcupadaM2" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control" ToolTip="Número de metros cuadrados " MaxLength="11"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderAreaOcupadaM2" runat="server" TargetControlID="TextBoxAreaOcupadaM2" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxAreaOcupadaM2" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxAreaOcupadaM2" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Monto pago mensual (sin impuesto):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxMontoPagoMes" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control" AutoPostBack="True" OnTextChanged="TextBoxMontoPagoMes_TextChanged" ToolTip="importe de pago mensual de la renta sin impuesto" MaxLength="11"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_MontoPagoMes" runat="server" TargetControlID="TextBoxMontoPagoMes" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxMontoPagoMes" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxMontoPagoMes" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Cuota de mantenimiento (sin impuesto):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxCuotaMantenimiento" onblur="backFromErrorClass(this);" runat="server" Text="0.00" placeholder="0.00" CssClass="form-control" AutoPostBack="True" OnTextChanged="TextBoxCuotaMantenimiento_TextChanged" ToolTip="Importe del pago de renta por concepto de mantenimiento" MaxLength="9"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_CuotaMantenimiento" runat="server" TargetControlID="TextBoxCuotaMantenimiento" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxCuotaMantenimiento" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCuotaMantenimiento" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Monto pago por cajones estacionamiento (sin impuesto):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxMontoPagoEstacionamiento" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control" Text="0.00" AutoPostBack="True" OnTextChanged="TextBoxMontoPagoEstacionamiento_TextChanged" ToolTip="Importe de pago por estacionamiento" MaxLength="9"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMontoPagoEstacionamiento" runat="server" TargetControlID="TextBoxMontoPagoEstacionamiento" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxMontoPagoEstacionamiento" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxMontoPagoEstacionamiento" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Porcentaje de impuesto:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxPtjeImpuesto" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" ToolTip="Impuesto que se paga por el arrendamiento" MaxLength="2"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPtjeImpuesto" runat="server" TargetControlID="TextBoxPtjeImpuesto" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxPtjeImpuesto" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxPtjeImpuesto" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Tipo moneda para pago:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList ID="DropDownListTipoMonedaPago" onchange="backFromErrorClass(this);" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvDropDownListTipoMonedaPago" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoMonedaPago" Display="Dynamic" InitialValue="--" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Monto total renta unitaria (sin impuesto):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxTotalRenta" runat="server" ReadOnly="true" placeholder="0.00" CssClass="form-control" Font-Bold="True" ToolTip="Es la suma de Monto de:  (Renta Mensual) + (Cuota de Manteniento) + (Monto por Cajones de Estacionamiento)">0.00</asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <span class="control-label">Observaciones:</span>
                                            <asp:TextBox ID="TextBoxObs" runat="server" TextMode="MultiLine" placeholder="Comentarios.." CssClass="form-control" ToolTip="Nota adicional" MaxLength="400"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <asp:Label ID="LabelInfoDatosContratacion" runat="server" Font-Bold="False"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading"><strong>Órgano Interno de Control (OIC) </strong></div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre(s):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxNombreTitularOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTitularOIC" runat="server" TargetControlID="TextBoxNombreTitularOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxNombreTitularOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreTitularOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Primer apellido:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxApPatOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderApPatOIC" runat="server" TargetControlID="TextBoxApPatOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApPatOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxApPatOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Segundo apellido:</span>
                                            <asp:TextBox ID="TextBoxApMatOIC" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderApMatOIC"
                                                runat="server" TargetControlID="TextBoxApMatOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre del cargo:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxNombreCargoOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" ToolTip="Puesto del Titular de OIC que se registra" MaxLength="200"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNombreCargoOIC" runat="server" TargetControlID="TextBoxNombreCargoOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxNombreCargoOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreCargoOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Correo electrónico:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxEmailOIC" onblur="backFromErrorClass(this);" runat="server" placeholder="Correo electrónico institucional" CssClass="form-control" MaxLength="100" ToolTip="usuario@servidor.com"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderEmailOIC" runat="server" TargetControlID="TextBoxEmailOIC" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-_@"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxEmailOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxEmailOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading"><strong>Capturista </strong></div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre(s):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxNombreCapturista" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxNombreCapturista" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreCapturista" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Primer apellido:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxApPatCapturista" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApPatCapturista" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxApPatCapturista" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <span class="control-label">Segundo apellido:</span>
                                            <asp:TextBox ID="TextBoxApMatCapturista" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre del cargo:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxCargoCapturista" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" ToolTip="Puesto que ocupa la persona que registra el contrato de arrendamiento" MaxLength="200"></asp:TextBox>

                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxCargoCapturista" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCargoCapturista" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Correo electrónico:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxEmailCapturista" onblur="backFromErrorClass(this);" runat="server" placeholder="Correo electrónico institucional" CssClass="form-control" MaxLength="100" ToolTip="usuario@servidor.com"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator   ID="rfvTextBoxEmailCapturista" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxEmailCapturista" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <div style="text-align: center">
                    <button type="button" class="btn btn-default" data-toggle="modal" data-target="#myModal" style="display: none">Cancelar</button>
                    <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClientClick="window.onbeforeunload = null;" OnClick="ButtonCancelar_Click" Text="Cancelar" />
                    <asp:Button ID="ButtonverificarNormatividad" runat="server" CssClass="btn btn-default" OnClick="ButtonverificarNormatividad_Click" CausesValidation="false" Text="Verificar normatividad" ToolTip="Expone que conceptos presentan excepción en el cumplimiento de la Normatividad, sin guardar la información" />
                    <asp:Button ID="ButtonEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary"   ValidationGroup="Contratos" CausesValidation="true" OnClientClick="return WebForm_OnSubmit12();" OnClick="ButtonEnviar_Click" Height="43px" ToolTip="Aceptar el envio de la información para su registro y generación del acuse" />
                    <br />
                    <asp:Label ID="LabelInfoEnviar" runat="server" BackColor="#FFFF99"></asp:Label>
                </div>
                <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title" id="myModalLabel">Contratos de Arrendamiento</h4>
                            </div>
                            <div class="modal-body">
                                ¿Confirma que desea salir de la captura de información?
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="ButtonCancelarModal" runat="server" CssClass="btn btn-default" Text="Aceptar" OnClick="ButtonCancelarModal_Click" />
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">
                <asp:Label ID="LblWait" runat="server" Text="Espere un momento por favor..." Width="200px"
                    Style="font-size: 9pt; font-family: Arial; font-weight: bold; background-color: #003355; color: #FFFFFF;"> 
                </asp:Label><br />
                <br />
                <img src="../Imagenes/ajax-loader.gif" alt="Loading" style="border-style: none; border-width: 0px; border-color: Red;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
