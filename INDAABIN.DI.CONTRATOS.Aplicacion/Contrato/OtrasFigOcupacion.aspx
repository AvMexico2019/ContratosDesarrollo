<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="OtrasFigOcupacion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.OtrasFigOcupacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function actualizaPantalla() {
            window.location = '../InmuebleArrto/BusqMvtosOtrasFigurasOcupacion.aspx';
        }

        window.onbeforeunload = function () { return 'Es posible que los cambios no se guarden, ¿Confirmas que deseas continuar con ésta acción? '; }

        function abreDomicilios() {
            var valorInmuebleArrendamiento = $('#cphBody_lblIdInmuebleArrendamiento').val();
            var response = new Array('', '', '', '', '', '');
            window.open('../InmuebleArrto/DireccionInmueble.aspx?IdInmuebleArrendamiento=' + valorInmuebleArrendamiento, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1080, height = 650', 'true');
        }

        function asignaValores(responseCatched) {
            var response = new Array("", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            response = responseCatched;
            if (response != null) {
                document.getElementById('cphBody_TextBoxRIUF').value = response[0];
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
            //alert('WebForm_OnSubmit');
            var validationGroupName = 'Contratos';
            if (!Page_ClientValidate(validationGroupName)) {
                for (var i in Page_Validators) {
                    try {
                        if (Page_Validators[i].validationGroup == validationGroupName) {
                            var control = document.getElementById(Page_Validators[i].controltovalidate);
                            if (!Page_Validators[i].isvalid) {
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

        function backFromErrorClass(control) {
            //alert('backFromErrorClass');
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

        function backFromErrorClassOtro(control) {
            var controlOtros = document.getElementById('<%= DropDownListTipoUsoInmueble.ClientID %>');
            if (controlOtros != null) {
                if (controlOtros.value == '-1') {
                    if (control.value != ' ' && control.value != '' && control.value != '-' && control.value != '.') {
                        control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                        control.parentElement.className = "form-group";
                    }
                    else {
                        control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                        control.parentElement.className = "form-group has-error";
                    }
                }
                else {
                    control.parentElement.parentElement.childNodes[1].className = "form-group";
                    control.parentElement.className = "form-group";
                }
            }
        }

        <%--function validateOtroUsoInmueble(control) {
            var controlOtros = document.getElementById('<%= TextBoxOtrosUsoInmuebleArrto.ClientID %>');
            if (control != null) {
                if (control.value == '-1') {
                    document.getElementById("<%= rfvTextBoxOtrosUsoInmuebleArrto.ClientID%>").style.visibility = '';
                    document.getElementById("<%= rfvTextBoxOtrosUsoInmuebleArrto.ClientID%>").enabled = true;
                    controlOtros.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                    controlOtros.parentElement.className = "form-group";
                }
                else {
                    document.getElementById("<%= rfvTextBoxOtrosUsoInmuebleArrto.ClientID%>").style.display = 'none';
                    document.getElementById("<%= rfvTextBoxOtrosUsoInmuebleArrto.ClientID%>").style.visibility = 'none';
                    document.getElementById("<%= rfvTextBoxOtrosUsoInmuebleArrto.ClientID%>").enabled = false;
                    controlOtros.parentElement.parentElement.childNodes[1].className = "form-group";
                    controlOtros.parentElement.className = "form-group";
                }
            }
        }--%>
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" id="lblIdEmisionOpinion" runat="server" />
            <input type="hidden" id="lblIdContrato" runat="server" />
            <input type="hidden" id="lblIdInmuebleArrendamiento" runat="server" />
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
                                Tipificación de otra figura de ocupación:
                        <asp:Label ID="LabelTipificacionArrto" runat="server"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Tipo de ocupación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList ID="DropDownListTipoOcupacion" CausesValidation="false" runat="server" onchange="backFromErrorClass(this);" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropDownListTipoOcupacion_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvDropDownListTipoOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoOcupacion" Display="Dynamic" InitialValue="--" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <span class="control-label">Otro tipo de ocupación, especifique:</span>
                                            <asp:TextBox ID="TextBoxOtrosTipoOcupacion" CssClass="form-control" ReadOnly="true" runat="server" ToolTip="proporciona otro tipo de ocupación"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading"><strong>Responsable de la ocupación</strong></div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre(s):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxNombreRespOcupacion" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNombreRespOcupacion"
                                                runat="server" TargetControlID="TextBoxNombreRespOcupacion" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxNombreRespOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreRespOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Primer apellido:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxApPatRespOcupacion" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderApPatRespOcupacion"
                                                runat="server" TargetControlID="TextBoxApPatRespOcupacion" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApPatRespOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxApPatRespOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Segundo apellido:</span>
                                            <asp:TextBox ID="TextBoxApMatRespOcupacion" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderApMatRespOcupacion"
                                                runat="server" TargetControlID="TextBoxApMatRespOcupacion" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApMatRespOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxApMatRespOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre del cargo:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxCargoRespOcupacion" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" ToolTip="Puesto del Titular de OIC que se registra" MaxLength="200"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderCargoRespOcupacion"
                                                runat="server" TargetControlID="TextBoxCargoRespOcupacion" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, "></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxCargoRespOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCargoRespOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Correo electrónico:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxEmailRespOcupacion" onblur="backFromErrorClass(this);" runat="server" placeholder="Correo electrónico institucional" CssClass="form-control" MaxLength="100" ToolTip="usuario@servidor.com"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderEmailRespOcupacion"
                                                runat="server" TargetControlID="TextBoxEmailRespOcupacion" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.,-@"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxEmailRespOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxEmailRespOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
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
                            <div class="panel-heading"><strong>Ubicación y uso del inmueble </strong></div>
                            <div class="panel-body">
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
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label ID="LabelInfoInmuebleArrendamiento" runat="server" Font-Bold="False"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Uso generico del inmueble:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList ID="DropDownListTipoUsoInmueble" onchange="backFromErrorClass(this);" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropDownListTipoUsoInmueble_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvDropDownListTipoUsoInmueble" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoUsoInmueble" Display="Dynamic" InitialValue="--" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="form-group-required-as">
                                                <span class="control-label">Uso especifico del inmueble:</span>
                                            </div>
                                            <div class="form-group">
                                                <asp:DropDownList ID="DropDownListTipoUsoEspecificoInmueble" onchange="backFromErrorClass(this);" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoUsoEspecificoInmueble" Display="Dynamic" InitialValue="--" SetFocusOnError="True" CssClass="error text-danger" />
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <span class="control-label">Otros uso de inmueble:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxOtrosUsoInmuebleArrto" CssClass="form-control" runat="server" TextMode="MultiLine" MaxLength="200" onblur="backFromErrorClassOtro(this);"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxOtrosUsoInmuebleArrto" Enabled="false " ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxOtrosUsoInmuebleArrto" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading"><strong>Datos de contratación </strong></div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Fecha inicio ocupación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxFechaIOcupacion" onblur="backFromErrorClass(this);" onchange="backFromErrorClass(this);" runat="server" placeholder="dd/mm/aa" CssClass="form-control" ToolTip="Fecha Fin de la contratación" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaIOcupacion"
                                                runat="server" TargetControlID="TextBoxFechaIOcupacion" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxFechaIOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFechaIOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Fecha fin de ocupación:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxFechaFOcupacion" onblur="backFromErrorClass(this);" onchange="backFromErrorClass(this);" placeholder="dd/mm/aa" runat="server" CssClass="form-control" ToolTip="Fecha de Inicio de la contratación" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaFOcupacion"
                                                runat="server" TargetControlID="TextBoxFechaFOcupacion" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxFechaFOcupacion" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFechaFOcupacion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Área ocupada M2:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxAreaOcupadaM2" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control" ToolTip="Número de metros cuadrados " MaxLength="30"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderAreaOcupadaM2"
                                                runat="server" TargetControlID="TextBoxAreaOcupadaM2" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
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
                                            <asp:TextBox ID="TextBoxMontoPagoMes" onblur="backFromErrorClassOtro(this);" runat="server" placeholder="0.00" CssClass="form-control" AutoPostBack="True" OnTextChanged="TextBoxMontoPagoMes_TextChanged" ToolTip="importe de pago mensual de la renta sin impuesto" MaxLength="24"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_MontoPagoMes"
                                                runat="server" TargetControlID="TextBoxMontoPagoMes" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxMontoPagoMes" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxMontoPagoMes" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Cuota de mantenimiento (sin impuesto):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxCuotaMantenimiento" onblur="backFromErrorClass(this);" runat="server" Text="0.00" placeholder="0.00" CssClass="form-control" AutoPostBack="True" OnTextChanged="TextBoxCuotaMantenimiento_TextChanged" ToolTip="Importe del pago de renta por concepto de mantenimiento" MaxLength="24"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_CuotaMantenimiento"
                                                runat="server" TargetControlID="TextBoxCuotaMantenimiento" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxCuotaMantenimiento" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCuotaMantenimiento" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Monto pago por cajones de estacionamiento (sin impuesto):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxMontoPagoEstacionamiento" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control" Text="0.00" AutoPostBack="True" OnTextChanged="TextBoxMontoPagoEstacionamiento_TextChanged" ToolTip="Importe de pago por estacionamiento" MaxLength="20"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMontoPagoEstacionamiento"
                                                runat="server" TargetControlID="TextBoxMontoPagoEstacionamiento" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
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
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPtjeImpuesto"
                                                runat="server" TargetControlID="TextBoxPtjeImpuesto" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
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
                                            <span class="control-label">Monto total renta unitaria (sin Impuesto):</span>
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
                            </div>
                        </div>
                        <div class="panel panel-default">
                            <div class="panel-heading"><strong>Órgano Interno de Control (OIC)</strong></div>
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre(s):</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxNombreTitularOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTitularOIC"
                                                runat="server" TargetControlID="TextBoxNombreTitularOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxNombreTitularOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreTitularOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Primer apellido:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxApPatOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderApPatOIC"
                                                runat="server" TargetControlID="TextBoxApPatOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApPatOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreTitularOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Segundo apellido:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxApMatOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderApMatOIC"
                                                runat="server" TargetControlID="TextBoxApMatOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApMatOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxApMatOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Nombre del cargo:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxNombreCargoOIC" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" ToolTip="Puesto del Titular de OIC que se registra" MaxLength="200"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderNombreCargoOIC"
                                                runat="server" TargetControlID="TextBoxNombreCargoOIC" ValidChars="ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789.-, ÁÉÍÓÚáéíóú"></cc1:FilteredTextBoxExtender>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxNombreCargoOIC" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreCargoOIC" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group-required-as">
                                            <span class="control-label">Correo electrónico:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxEmailOIC" onblur="backFromErrorClass(this);" runat="server" placeholder="Correo electrónico institucional" CssClass="form-control" MaxLength="100" ToolTip="usuario@servidor.com"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderEmailOIC"
                                                runat="server" TargetControlID="TextBoxEmailOIC" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._-@"></cc1:FilteredTextBoxExtender>
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
                                        <div class="form-group-required-as">
                                            <span class="control-label">Segundo apellido:</span>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="TextBoxApMatCapturista" onblur="backFromErrorClass(this);" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvTextBoxApMatCapturista" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxApMatCapturista" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
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
                                        <asp:RequiredFieldValidator ID="rfvTextBoxEmailCapturista" ValidationGroup="Contratos" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxEmailCapturista" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <div style="text-align: center">
                    <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClientClick="window.onbeforeunload = null;" OnClick="ButtonCancelar_Click" Text="Cancelar" ToolTip="Salir de esta vista" />
                    <asp:Button ID="ButtonEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" ValidationGroup="Contratos" CausesValidation="true" OnClientClick="return WebForm_OnSubmit12();" OnClick="ButtonEnviar_Click" Height="43px" ToolTip="Aceptar el envio de la información para su registro y generación del acuse" />
                    <br />
                    <asp:Label ID="LabelInfoEnviar" runat="server" BackColor="#FFFF99"></asp:Label>
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
