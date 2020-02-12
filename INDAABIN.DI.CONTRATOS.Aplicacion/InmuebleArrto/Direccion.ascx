<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Direccion.ascx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.Direccion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div class="panel panel-default">
    <div class="panel-heading">
        <strong class="text-right">Dirección del inmueble a ocupar</strong>
    </div>
    <div class="panel-body">
        <div style="align-content: center" class="text-center">
            <asp:Label ID="LabelInfoEnc" runat="server"></asp:Label>
            <asp:Label ID="LabelIdInmueble" runat="server" Style="visibility: hidden"></asp:Label>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <span class="control-label">País*:</span>
                    <asp:DropDownList ID="DropDownListPais" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DropDownListPais_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                </div>
            </div>
        </div>
        <asp:Panel ID="PanelNacional" runat="server">
            <div class="row">
               <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Estado:</span>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="DropDownListEdo" runat="server" onchange="backFromErrorClass(this);"  ValidationGroup="Direcciones" OnSelectedIndexChanged="DropDownListEdo_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvDropDownListEdo" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListEdo" InitialValue="--" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Municipio:</span>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="DropDownListMpo" runat="server" onchange="backFromErrorClass(this);"  ValidationGroup="Direcciones" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DropDownListMpo_SelectedIndexChanged">
                            <asp:ListItem Value="0">Seleccione un Municipio</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvDropDownListMpo" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListMpo" InitialValue="--" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Código postal:</span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TextBoxCP"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="5" CssClass="form-control" placeholder="#####" OnTextChanged="TextBoxCP_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_TextBoxCP" runat="server" TargetControlID="TextBoxCP" ValidChars="0123456789" />
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxCP" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCP" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" /> 
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Colonia:</span>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="DropDownListColonia" runat="server" onchange="backFromErrorClass(this);"  ValidationGroup="Direcciones" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropDownListColonia_SelectedIndexChanged">
                            <asp:ListItem Value="0">Seleccione una Localidad</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvDropDownListColonia" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListColonia" InitialValue="0" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />  
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Otra colonia:</span>
                        <asp:TextBox ID="TextBoxOtraColonia" runat="server" MaxLength="150" CssClass="form-control" Enabled="False"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="PanelExtranjero" runat="server" Visible="False">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Estado ó equivalente:</span>
                    </div>   
                    <div class="form-group">
                        <asp:TextBox ID="TextBoxEdoExtranjero"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxEdoExtranjero" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxEdoExtranjero" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Municipio ó equivalente:</span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TextBoxMpoExtranjero"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxMpoExtranjero" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxMpoExtranjero" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Código postal ó equivalente:</span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TextBoxCPExtranjero"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxCPExtranjero" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCPExtranjero" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Ciudad ó equivalente:</span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TextBoxCiudadExtranjero"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxCiudadExtranjero" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCiudadExtranjero" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group-required-as">
                    <span class="control-label">Tipo de vialidad:</span>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="DropDownListTipoVialidad" CssClass="form-control" runat="server" onchange="backFromErrorClass(this);"  ValidationGroup="Direcciones">
                    </asp:DropDownList>
                </div>
                <asp:RequiredFieldValidator ID="rfvDropDownListTipoVialidad" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoVialidad" InitialValue="--" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
            </div>
             <div class="col-md-4">
                <div class="form-group-required-as">
                    <span class="control-label">Tipo inmueble:</span>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="DropDownListTipoInmueble" runat="server" onchange="backFromErrorClass(this);"  ValidationGroup="Direcciones" AutoPostBack="False" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <asp:RequiredFieldValidator ID="rfvDropDownListTipoInmueble" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListTipoInmueble" InitialValue="--" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
            </div>
            <div class="col-md-4">
                <div class="form-group-required-as">
                    <span class="control-label">Denominación de la dirección:</span> 
                </div> 
                <div class="form-group">
                    <asp:TextBox ID="TextBoxNombreDireccion"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="150" CssClass="form-control" placeholder="Registrar como: BODEGA-1, EDICIFIO OFICINAS, etc..."></asp:TextBox>
                </div>                  
                <asp:RequiredFieldValidator ID="rfvTextBoxNombreDireccion" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreDireccion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />                    
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group-required-as">
                    <span class="control-label">Nombre de vialidad:</span>
                </div> 
	            <div class="form-group">
                    <asp:TextBox ID="TextBoxNombreVialidad"  onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="55" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvTextBoxNombreVialidad" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreVialidad" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
            </div>
            <div class="col-md-4">
                <div class="form-group-required-as">
                    <span class="control-label">Número exterior:</span>
                </div> 
	            <div class="form-group">
                    <asp:TextBox ID="TextBoxNumExt" onblur="backFromErrorClass(this);" ValidationGroup="Direcciones" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="rfvTextBoxNumExt" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNumExt" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <span class="control-label">Número interior:</span>
                    <asp:TextBox ID="TextBoxNumInt" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <asp:Panel ID="pnlControlesGeo" runat="server" Visible="true" Style="text-align: center">                        
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Georeferencia latitud:</span>
                    </div>
                    <div class="form-group">                    
                        <asp:TextBox ID="TextBoxLatitud" onchange="backFromErrorClass(this);" runat="server" MaxLength="16" CssClass="form-control" Enabled="False"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderLatitud" runat="server" TargetControlID="TextBoxLatitud" ValidChars="-.0123456789"></cc1:FilteredTextBoxExtender>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxLatitud" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxLatitud" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Georeferencia longitud:</span>
                    </div>
                    <div class="form-group">                    
                        <asp:TextBox ID="TextBoxLongitud" onchange="backFromErrorClass(this);" runat="server" MaxLength="16" CssClass="form-control" Enabled="False"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderLongitud" runat="server" TargetControlID="TextBoxLongitud" ValidChars="-.0123456789"></cc1:FilteredTextBoxExtender>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxLongitud" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxLongitud" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Mapa de ubicación:</span>
                            <button id="btnMuestraMapa" type="button" onclick="openIDEForm();" class="btn btn-default"><span class="glyphicon glyphicon-globe"></span>  Abrir mapa</button>
                            <asp:RequiredFieldValidator ID="rfvMapaWrt" runat="server" ControlToValidate="txtDMapaWrt" CssClass="fuenteEtiqueta" Display="Dynamic" ErrorMessage="Punto, linea o poligono en el mapa requerido." ForeColor="Red" ValidationGroup="vgrDatosRequeridos" InitialValue="" Enabled="true"></asp:RequiredFieldValidator>
                            <br />
                            <table align="left">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDMapaWrt" Width="100px" runat="server" CausesValidation="true" ValidationGroup="vgrDatosRequeridos" Text="" Style="visibility: hidden"></asp:TextBox>
                                        <input type="hidden" id="wkt" runat="server" />
                                        <input type="hidden" id="Edo" runat="server" />
                                        <input type="hidden" id="Mun" runat="server" />
                                        <input type="hidden" id="tipoGeometria" runat="server" />
                                        <input type="hidden" id="x" runat="server" />
                                        <input type="hidden" id="y" runat="server" />
                                        <input type="hidden" id="Editar" runat="server" />
                                        <input type="hidden" id="code" runat="server" />
                                        <input type="hidden" id="message" runat="server" />
                                    </td>
                                </tr>
                            </table>                        
                    </div>
                </div>
            </asp:Panel>
        </div>
        <span class="control-label">* Campos obligatorios</span>       
        <div style="align-content: center" class="text-center">
            <asp:Label ID="LabelInfoInmuebleDir" runat="server">
            </asp:Label>
        </div>
    </div>
</div>
