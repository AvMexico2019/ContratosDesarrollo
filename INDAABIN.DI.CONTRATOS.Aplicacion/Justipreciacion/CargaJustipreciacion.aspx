<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CargaJustipreciacion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Justipreciacion.CargaJustipreciacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function openCustomWindow(folio, tipo) {
            window.open('../EmisionOpinion/AcuseEmisionOpinion.aspx?IdFolio=' + folio + '&TipoArrto=' + tipo, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');
        }

        //window.onbeforeunload = function () { return 'Es posible que los cambios no se guarden, ¿Confirmas que deseas continuar con ésta acción? '; }

        //RCA 21/08/2017
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $("#<%= TextBoxFechaDictamen.ClientID %>").datepicker({ changeYear: true });
            }
            $("#<%= TextBoxFechaDictamen.ClientID %>").datepicker({ changeYear: true });
        });

        function WebForm_OnSubmit12() {
            var validationGroupName = 'Justipreciaciones';
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

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>

            <div id="Contenedor-spinner" class="collapse"></div>

            <div class="row">
                <div class="col-md-8">
                    <div class="form-group" style="text-align: center;">
                        <br />
                        <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="txtFolio">Justipreciación externa:</label>

                    </div>
                </div>
                <div class="col-md-8"></div>
            </div>

            <%-- priemro--%>
            <asp:Panel ID="pnlControles" runat="server">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Secuencial:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtFolioBPM" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredtxtFolioBPM" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="txtFolioBPM" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Generico:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBox1" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBox1" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                </div>

                <%--segundo--%>
                <div class="row">

                    <div class="col-md-8">
                        <div class="form-group-required-as">
                            <span class="control-label">Institución:</span>
                        </div>
                        <div class="form-group">

                            <asp:DropDownList runat="server" ID="DropDownListInstitucion" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropDownListInstitucion_SelectedIndexChanged" />
                        </div>
                    </div>

                    <div class="col-md-8">
                        <div class="form-group-required-as">
                            <span class="control-label">Sector</span>
                        </div>
                        <div class="form-group">
                            <%--RCA 21/08/2017--%>
                            <%--AutoPostBack="true" OnSelectedIndexChanged="DropDownSector_SelectedIndexChanged"--%>
                            <asp:DropDownList runat="server" ID="DropDownSector" CssClass="form-control" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredDropDownListSector" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListInstitucion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                </div>

                <%--tercero--%>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Superficie terreno dictaminado:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxSupDic" onblur="backFromErrorClass(this);" onchange="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxSUperDic" runat="server" TargetControlID="TextBoxSupDic" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvTextBoxSupDic" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxSupDic" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Unidad de medida:</span>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="DropDownListUniTer" CssClass="form-control" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredDropDownListUniTer" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListUniTer" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                </div>

                <%-- cuarto--%>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Superficie construida dictaminado:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxConsDic" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxConsDic" runat="server" TargetControlID="TextBoxConsDic" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBoxConsDic" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxConsDic" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Unidad de medida:</span>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="DropDownListUniCons" CssClass="form-control" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredDropDownListUniCons" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListUniCons" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                </div>

                <%--quinto--%>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Superficie rentable dictanimado:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxRenDic" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxRenDic" runat="server" TargetControlID="TextBoxRenDic" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBoxRenDic" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxRenDic" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Unidad de medida:</span>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="DropDownListRenDic" CssClass="form-control" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredDropDownListRenDic" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListRenDic" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                </div>

                <%--sexto--%>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Monto dictaminado:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxMOntoDic" onblur="backFromErrorClass(this);" runat="server" placeholder="0.00" CssClass="form-control"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxMOntoDic" runat="server" TargetControlID="TextBoxMOntoDic" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBoxMOntoDic" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxMOntoDic" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="control-label">Unidad responsable:</span>
                            <asp:TextBox ID="TextBoxUnidadrespon" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <%--RCA 21/08/2017--%>
                        <div class="form-group-required-as">
                            <span class="control-label">Fecha Dictamen:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxFechaDictamen" onblur="backFromErrorClass(this);" onchange="backFromErrorClass(this);" placeholder="dd/mm/aa" runat="server" CssClass="form-control" MaxLength="11"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaDictamen" runat="server" TargetControlID="TextBoxFechaDictamen" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvTextBoxFechaDictamen" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxFechaDictamen" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                </div>

                <%--octavo--%>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Calle:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxCalle" runat="server" CssClass="form-control" MaxLength="500"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBoxCalle" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCalle" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">No ext:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxNumExt" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBoxNumExt" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNumExt" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="control-label">No int:</span>
                            <asp:TextBox ID="TextBoxNumInt" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%--noveno --%>
                <div class="row">

                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Estado:</span>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="DropDownListEdo" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEdo_SelectedIndexChanged" />
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Municipio:</span>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="DropDownListMpo" CssClass="form-control" />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredDropDownListMpo" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListMpo" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>

                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Codigo postal:</span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="TextBoxCP" runat="server" CssClass="form-control" MaxLength="5" placeholder="#####" OnTextChanged="TextBoxCP_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxCP" runat="server" TargetControlID="TextBoxCP" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredTextBoxCP" ValidationGroup="Justipreciaciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxCP" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />
                    </div>

                </div>

                <div class="row">

                    <div class="col-md-4">
                        <div class="form-group-required-as">
                            <span class="control-label">Colonia:</span>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList ID="DropDownListColonia" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropDownListColonia_SelectedIndexChanged">
                                <asp:ListItem Value="0">Selecciona una localidad</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvDropDownListColonia" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="DropDownListColonia" InitialValue="0" Display="Dynamic" SetFocusOnError="true" CssClass="error text-danger"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group">
                            <span class="control-label">Otra colonia:</span>
                            <asp:TextBox ID="TextBoxOtraColonia" runat="server" MaxLength="150" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>

                    </div>

                </div>
            </asp:Panel>

           
             <%--boton y cuadro de texto para guardar archivos--%>
            <div class="row bottom-buffer">
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="<%=FUJustipreciacion.ClientID%>" class="control-label">
                            Subir un archivo<asp:RequiredFieldValidator ValidationGroup="Justipreciaciones" ID="RequiredFieldValidator6" SetFocusOnError="true" CssClass="form-text form-text-error" ControlToValidate="FUJustipreciacion" Display="Dynamic" Text="*" ErrorMessage="Este campo es obligatorio o estas introduciendo un formato incorrecto" runat="server" />
                        </label>
                        <label class="control-label" aria-controls="<%=FUJustipreciacion.ClientID%>">*</label>
                        <label class="control-label">:</label>
                        <asp:FileUpload runat="server" accept=".pdf,.PDF" ID="FUJustipreciacion" CssClass="form-control" ValidationGroup="Justipreciaciones"  />
                        <small class="form-text form-text-error" aria-controls="<%=FUJustipreciacion.ClientID%>"></small>
                        <asp:Label runat="server" CssClass="control-label small" Text="Solo se permiten los formatos pdf"></asp:Label><br />
                    </div>
                </div>
            </div>
   

            <asp:Panel runat="server" ID="PnlDireccion"></asp:Panel>

            <div class="row">
                <div class="col-md-8">
                    <div style="text-align: center">
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#myModal" style="display: none">Cancelar</button>
                        <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClick="ButtonCancelar_Click" Text="Cancelar" />
                        <asp:Button ID="ButtonEnviar" runat="server" Text="Guardar" CssClass="btn btn-primary" ValidationGroup="Contratos" CausesValidation="true" OnClientClick="return WebForm_OnSubmit12();" OnClick="ButtonEnviar_Click" Height="43px" ToolTip="Aceptar el envio de la información para su guardado" />
                        <asp:Label ID="LabelInfoEnviar" runat="server" BackColor="#FFFF99"></asp:Label>
                    </div>
                </div>
            </div>

            <asp:Panel runat="server" ID="PnlAvaluo"></asp:Panel>

            <asp:Panel runat="server" ID="Panel2"></asp:Panel>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonEnviar" />
        </Triggers>

    </asp:UpdatePanel>

    

   <%--  <%--GIF PARA QUE SE MUESTRE CADA QUE SE ESTA RECARGANDO LA PAGINA--%>
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
