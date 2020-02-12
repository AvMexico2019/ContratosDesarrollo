<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DireccionInmueble.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.DireccionInmueble" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/InmuebleArrto/DireccionRiuf.ascx" TagPrefix="uc1" TagName="DireccionRiuf" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="uc1" TagName="UsuarioInfo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Direcciones de Inmuebles</title>

    <!-- CSS -->
    <link href="../css/EstilosEspecificos.css" rel="stylesheet" />
    <link href="https://framework-gb.cdn.gob.mx/assets/styles/main.css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300' rel='stylesheet' type='text/css' />
    <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon' />
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>

    <script type="text/javascript">
        function closeWindow() {
            var response = new Array("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            window.returnValue = response;
            window.opener.asignaValores(response);
            window.close();
        }

        function validaSeleccion() {
            var response = new Array("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            response[0] = document.getElementById('hRiuf').value;
            response[1] = document.getElementById('hPais').value;
            response[2] = document.getElementById('hTipoInmueble').value;
            response[3] = document.getElementById('hEdo').value;
            response[4] = document.getElementById('hMun').value;
            response[5] = document.getElementById('hColonia').value;
            response[6] = document.getElementById('hOtraColonia').value;
            response[7] = document.getElementById('hNombreVialidad').value;
            response[8] = document.getElementById('hDenominacionDireccion').value;
            response[9] = document.getElementById('hCP').value;
            response[10] = document.getElementById('hTipoVialidad').value;
            response[11] = document.getElementById('hNumExterior').value;
            response[12] = document.getElementById('hNumInterior').value;
            response[13] = document.getElementById('hGeoRefLatitud').value;
            response[14] = document.getElementById('hGeoRefLongitud').value;
            response[15] = document.getElementById('hIdInmueble').value;
            response[16] = document.getElementById('hCodigoPostalExtranjero').value;
            response[17] = document.getElementById('hEstadoExtranjero').value;
            response[18] = document.getElementById('hCiudadExtranjero').value;
            response[19] = document.getElementById('hMunicipioExtranjero').value;

            // Llamado a la funcion asignaValores, de la forma CapturaSolicitud.aspx
            window.opener.asignaValores(response);
            window.close();
        }
    </script>
</head>
<body>

    <script type="text/javascript">
        function WebForm_OnSubmit() {
            //alert('WebForm_OnSubmit');
            var validationGroupName = 'Direcciones';
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
    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" EnableScriptGlobalization="true" EnablePartialRendering="true" AsyncPostBackTimeout="360000" />
        <asp:UpdatePanel ID="upCaptura" runat="server">
            <ContentTemplate>
                <input type="hidden" id="lblIdInmuebleArrendamiento" runat="server" />
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Domicilio del inmueble generado en la emisión</strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">País:</span>
                                    <asp:DropDownList ID="DropDownListPais" Enabled="false" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DropDownListPais_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Estado:</span>
                                    <asp:DropDownList ID="DropDownListEdo" Enabled="false" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEdo_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Municipio:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListMpo" Enabled="false" runat="server" CssClass="form-control" ToolTip="Debe seleccionar primero un Estado, para exponer sus Municipios">
                                        <asp:ListItem Value="0">--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <span class="control-label">Dirección:</span>
                                    <br />
                                    <asp:TextBox ID="TextBoxDireccionActual" TextMode="MultiLine" Enabled="false" CssClass="form-control" placeholder="Dirección actual seleccionada" runat="server" MaxLength="10" ToolTip="Dirección actual seleccionada"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="pnlBusqueda" runat="server">
                    <div class="panel panel-default">
                        <div class="panel-heading"><strong>Panel de búsqueda de direcciones registradas con RIUF</strong></div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <span class="control-label">Dirección:</span>
                                        <br />
                                        <asp:TextBox ID="TextBoxDireccion" CssClass="form-control" placeholder="Cualquier dato de la dirección a buscar" runat="server" MaxLength="250" ToolTip="Escribir completa o parcialmente la direccion a buscar"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <span class="control-label">RIUF:</span>
                                        <br />
                                        <asp:TextBox ID="TextBoxRIUF" onkeypress="validaRiuf(this);" CssClass="form-control" placeholder="##-#####-#" runat="server" MaxLength="10" ToolTip="Registro de Inmueble en Uso de la Federación"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderRIUF"
                                            runat="server" TargetControlID="TextBoxRIUF" ValidChars="0123456789-"></cc1:FilteredTextBoxExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxRIUF" Enabled="false"
                                            ValidationExpression="^\d{2}-\d{5}-\d{1}$" Display="Dynamic" SetFocusOnError="true" EnableClientScript="true" ErrorMessage="Formato de RIUF inválido" CssClass="error text-danger" ValidationGroup="CamposBusqueda"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <span class="control-label">Consultar: </span>
                                        <br />
                                        <asp:DropDownList ID="DropDownListTipoConsulta" runat="server" CssClass="form-control" ToolTip="Seleccionar el timpo de consulta">
                                            <asp:ListItem Value="0">Domicilios en coincidencia al País/Estado/Municipio de la emisión seleccionada</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">Domicilios en coincidencia parcial al domicilio de la emisión seleccionada</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <button class="btn btn-primary" runat="server" onserverclick="ButtonConsultar_Click" tooltip="Da clic para consultar los domicilios de acuerdo a los criterios de búsqueda" type="button"><span class="glyphicon glyphicon-search"></span>Consultar</button>
                                        <asp:Button ID="ButtonCancelarConsulta" runat="server" CssClass="btn btn-default" Text="Cancelar" ToolTip="Salir de la consulta de direcciones" OnClientClick="closeWindow();" />
                                        <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" Visible="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlGrivView" runat="server" Visible="false">
                    <div class="panel panel-default">
                        <div class="panel-heading"><strong>Direcciones de Inmuebles con RIUF</strong></div>
                        <div class="panel-body">
                            <div class="row">
                                <asp:GridView ID="GridViewResult" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" Font-Size="Small" PageSize="5" AllowPaging="True" OnPageIndexChanging="GridViewResult_PageIndexChanging" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="GridViewResult_SelectedIndexChanged">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>

                                        <asp:CommandField ShowSelectButton="True" SelectText='Seleccionar' ButtonType="Link">
                                            <ControlStyle Font-Bold="True" />
                                        </asp:CommandField>

                                        
                                        
                                        <asp:TemplateField HeaderText="Clave del Inmueble" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdInmueble" runat="server" Text='<%# Bind("IdInmueble") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RIUF.RIUF1" HeaderText="RIUF">
                                            <ItemStyle Font-Bold="True" HorizontalAlign="Left" Width="90px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Denominación Inmueble" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDenominacionInmueble" runat="server" Text='<%# Bind("NombreInmueble") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="True" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección Inmueble" />
                                        <asp:TemplateField HeaderText="IdPais" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdPais" runat="server" Text='<%# Bind("IdPais") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IdTipoInmueble" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdTipoInmueble" runat="server" Text='<%# Bind("IdTipoInmueble") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IdEstado" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdEstado" runat="server" Text='<%# Bind("IdEstado") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IdMunicipio" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdMunicipio" runat="server" Text='<%# Bind("IdMunicipio") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IdLocalidad" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdLocalidad" runat="server" Text='<%# Bind("IdLocalidad") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OtraColonia" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOtraColonia" runat="server" Text='<%# Bind("OtraColonia") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IdTipoVialidad" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdTipoVialidad" runat="server" Text='<%# Bind("IdTipoVialidad") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NombreVialidad" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNombreVialidad" runat="server" Text='<%# Bind("NombreVialidad") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NumExterior" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNumExterior" runat="server" Text='<%# Bind("NumExterior") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NumInterior" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNumInterior" runat="server" Text='<%# Bind("NumInterior") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CodigoPostal" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCodigoPostal" runat="server" Text='<%# Bind("CodigoPostal") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GeoRefLatitud" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGeoRefLatitud" runat="server" Text='<%# Bind("GeoRefLatitud") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GeoRefLongitud" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGeoRefLongitud" runat="server" Text='<%# Bind("GeoRefLongitud") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CodigoPostalExtranjero" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCodigoPostalExtranjero" runat="server" Text='<%# Bind("CodigoPostalExtranjero") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EstadoExtranjero" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstadoExtranjero" runat="server" Text='<%# Bind("EstadoExtranjero") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CiudadExtranjero" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCiudadExtranjero" runat="server" Text='<%# Bind("CiudadExtranjero") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MunicipioExtranjero" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMunicipioExtranjero" runat="server" Text='<%# Bind("MunicipioExtranjero") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" />
                                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings Position="TopAndBottom" />
                                    <PagerStyle CssClass="pagination-ys" />
                                    <RowStyle BackColor="#F7F7DE" />
                                    <SelectedRowStyle BackColor="#428bca" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                    <SortedAscendingHeaderStyle BackColor="#848384" />
                                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                    <SortedDescendingHeaderStyle BackColor="#575357" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblGenerarRIUF" runat="server" Visible="False"></asp:Label>
                <input type="hidden" id="hRiuf" runat="server" />
                <input type="hidden" id="hPais" runat="server" />
                <input type="hidden" id="hEdo" runat="server" />
                <input type="hidden" id="hMun" runat="server" />
                <input type="hidden" id="hTipoInmueble" runat="server" />
                <input type="hidden" id="hNombreVialidad" runat="server" />
                <input type="hidden" id="hDenominacionDireccion" runat="server" />
                <input type="hidden" id="hCP" runat="server" />
                <input type="hidden" id="hColonia" runat="server" />
                <input type="hidden" id="hOtraColonia" runat="server" />
                <input type="hidden" id="hTipoVialidad" runat="server" />
                <input type="hidden" id="hNumExterior" runat="server" />
                <input type="hidden" id="hNumInterior" runat="server" />
                <input type="hidden" id="hGeoRefLatitud" runat="server" />
                <input type="hidden" id="hGeoRefLongitud" runat="server" />
                <input type="hidden" id="hIdInmueble" runat="server" />
                <input type="hidden" id="hCodigoPostalExtranjero" runat="server" />
                <input type="hidden" id="hEstadoExtranjero" runat="server" />
                <input type="hidden" id="hCiudadExtranjero" runat="server" />
                <input type="hidden" id="hMunicipioExtranjero" runat="server" />
                <asp:Panel ID="pnlBusquedaBotones" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" style="text-align: center;">
                                <asp:Label ID="LabelInfoGridResult" runat="server"></asp:Label>
                                <br />
                                <asp:Button ID="ButtonSeleccionarDomicilio" runat="server" CssClass="btn btn-primary" OnClientClick="validaSeleccion();" Text="Seleccionar" Visible="False" />
                                <asp:Button ID="ButtonSeleccionarDomicilioConsulta" runat="server" CssClass="btn btn-primary" OnClientClick="validaSeleccion();" Text="Seleccionar" Visible="False" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="upCaptura">
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
    </form>
</body>
</html>
