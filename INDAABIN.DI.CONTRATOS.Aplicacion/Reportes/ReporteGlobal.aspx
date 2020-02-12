<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteGlobal.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Reportes.ReporteGlobal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
         $(document).ready(function () {
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
             function EndRequestHandler(sender, args) {
                 $("#<%= TextBoxFechaRegistroInicio.ClientID %>").datepicker();
                 $("#<%= TextBoxFechaRegistroFinal.ClientID %>").datepicker();
                 $("#<%= TextBoxFechaIOcupacionInicio.ClientID %>").datepicker();
                 $("#<%= TextBoxFechaIOcupacionFinal.ClientID %>").datepicker();
                 $("#<%= TextBoxFechaFOcupacionInicio.ClientID %>").datepicker();
                 $("#<%= TextBoxFechaFOcupacionFinal.ClientID %>").datepicker();
             }
             $("#<%= TextBoxFechaRegistroInicio.ClientID %>").datepicker();
             $("#<%= TextBoxFechaRegistroFinal.ClientID %>").datepicker();
             $("#<%= TextBoxFechaIOcupacionInicio.ClientID %>").datepicker();
             $("#<%= TextBoxFechaIOcupacionFinal.ClientID %>").datepicker();
             $("#<%= TextBoxFechaFOcupacionInicio.ClientID %>").datepicker();
             $("#<%= TextBoxFechaFOcupacionFinal.ClientID %>").datepicker();
         });
    </script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--controles de usuario--%>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />

            <div class="panel panel-primary">
                <div class="panel-heading">Reporte global de Contratos </div>
                    <div class="panel panel-default">
                    <div class="panel-heading"><strong>Seleccione los campos del reporte </strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:CheckBox ID="chkTodosCampos" runat="server" Text="&nbsp; Todos" AutoPostBack="true" OnCheckedChanged="chkTodosCampos_CheckedChanged"/>
                                    <asp:GridView ID="GridViewFields" ShowHeader="false" ShowFooter="false" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" Font-Size="Small" PageSize="10">
                                        <Columns>
                                            <asp:TemplateField HeaderText="NombreCampoCol1" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNombreCampoCol1" runat="server" Text='<%# Bind("NombreCampoCol1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DescripcionCampoCol1">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCampoCol1" runat="server" Text='<%# Bind("DescripcionCampoCol1") %>' Checked='<%# Bind("CheckCampoCol1") %>' Visible='<%# Bind("VisibleCampoCol1") %>'/>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="True" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="NombreCampoCol2" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNombreCampoCol2" runat="server" Text='<%# Bind("NombreCampoCol2") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DescripcionCampoCol2">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCampoCol2" runat="server" Text='<%# Bind("DescripcionCampoCol2") %>' Checked='<%# Bind("CheckCampoCol2") %>' Visible='<%# Bind("VisibleCampoCol2") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="True" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="NombreCampoCol3" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNombreCampoCol3" runat="server" Text='<%# Bind("NombreCampoCol3") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DescripcionCampoCol3">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCampoCol3" runat="server" Text='<%# Bind("DescripcionCampoCol3") %>' Checked='<%# Bind("CheckCampoCol3") %>' Visible='<%# Bind("VisibleCampoCol3") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="True" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="NombreCampoCol4" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNombreCampoCol4" runat="server" Text='<%# Bind("NombreCampoCol4") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DescripcionCampoCol4">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCampoCol4" runat="server" Text='<%# Bind("DescripcionCampoCol4") %>' Checked='<%# Bind("CheckCampoCol4") %>' Visible='<%# Bind("VisibleCampoCol4") %>'  />
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="True" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lblFieldList" runat="server" Visible="False"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Filtros de búsqueda</strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucion" runat="server" controlWidth="70%" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Tipo de contratación:</span>
                                    <asp:DropDownList ID="DropDownListTipoContrato" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Tipo de ocupación:</span>
                                    <asp:DropDownList ID="DropDownListTipoOcupacion" runat="server" CssClass="form-control">
                                    </asp:DropDownList>                                    
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Fecha de registro (inicial):</span>                                    
                                    <asp:TextBox ID="TextBoxFechaRegistroInicio" runat="server" placeholder="dd/mm/aaaa" CssClass="form-control" ToolTip="Rango inicial de la fecha de registro del contrato" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaRegistroInicio" runat="server" TargetControlID="TextBoxFechaFOcupacionInicio" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                             <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Fecha de registro (final):</span>
                                    <asp:TextBox ID="TextBoxFechaRegistroFinal" runat="server" placeholder="dd/mm/aaaa" CssClass="form-control" ToolTip="Rango final de la fecha de registro del contrato" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaRegistroFinal" runat="server" TargetControlID="TextBoxFechaFOcupacionFinal" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Fecha de inicio de ocupación (inicial):</span>
                                    <asp:TextBox ID="TextBoxFechaIOcupacionInicio" aria-hidden="true" runat="server" placeholder="dd/mm/aaaa" CssClass="form-control" ToolTip="Rango inicial de fecha de inicio de contratación" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaIOcupacionInicio" runat="server" TargetControlID="TextBoxFechaIOcupacionInicio" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                             <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Fecha de inicio de ocupación (final):</span>
                                    <asp:TextBox ID="TextBoxFechaIOcupacionFinal" runat="server" placeholder="dd/mm/aaaa" CssClass="form-control" ToolTip="Rango final de fecha de inicio de contratación" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaIOcupacionFinal" runat="server" TargetControlID="TextBoxFechaIOcupacionFinal" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Fecha de fin de ocupación (inicial):</span>                                    
                                    <asp:TextBox ID="TextBoxFechaFOcupacionInicio" runat="server" placeholder="dd/mm/aaaa" CssClass="form-control" ToolTip="Rango inicial de fecha de termino de contratación" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaFOcupacionInicio" runat="server" TargetControlID="TextBoxFechaFOcupacionInicio" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                             <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Fecha de fin de ocupación (final):</span>
                                    <asp:TextBox ID="TextBoxFechaFOcupacionFinal" runat="server" placeholder="dd/mm/aaaa" CssClass="form-control" ToolTip="Rango final de fecha de termino de contratación" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderFechaFOcupacionFinal" runat="server" TargetControlID="TextBoxFechaFOcupacionFinal" ValidChars="0123456789/"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <br />
                                    <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" OnClientClick="this.disabled = true; this.value = 'Consultando...';" UseSubmitBehavior="false" OnClick="ButtonConsultar_Click" Text="Consultar" />
                                </div>
                            </div>
                        </div>
                        <p>
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                    <asp:Panel ID="pnlReporte" runat="server" Visible="false">
                        <div class="panel panel-default">
                        <div class="panel-heading"><strong>Resultados de la búsqueda</strong></div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div style="overflow-x: scroll; height: 100%"> 
                                        <asp:GridView ID="GridViewReporte" ShowHeader="True" ShowFooter="false" runat="server" AutoGenerateColumns="True" CssClass="table table-striped" Font-Size="Small" PageSize="10" AllowPaging="True" OnPageIndexChanging="GridViewReporte_PageIndexChanging">
                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>                       
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group" style="align-content:center">   
                                        <br />                                    
                                        <asp:Button ID="ButtonExportar" runat="server" Text="Exportar reporte a excel" class="btn btn-default" OnClick="ButtonExportar_Click" Visible="false" />
                                        <asp:Label ID="lblTableNameReport" runat="server" Visible="False"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <p>
                                <asp:Label ID="LabelInfoResult" runat="server"></asp:Label>
                            </p>
                        </div>
                            </div>
                    </asp:Panel>                    
                
            </div>
         </ContentTemplate>
        <Triggers>
                  <asp:PostBackTrigger ControlID="ButtonExportar"  />    
        </Triggers>
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
