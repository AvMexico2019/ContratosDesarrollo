<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusqMvtosConvenioModificatorio.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.BusqMvtosConvenioModificatorio" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../css/EstilosEspecificos.css" rel="stylesheet" />
    <script src="../js/jsConvenioModificatorio.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div id="Contenedor-spinner" class="collapse"></div>

    <asp:UpdatePanel ID="UpdatePanelBusqMvtosConvenioModificatorio" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />

            <div class="panel panel-primary">
                <div class="panel-heading">Convenios modificatorios de inmuebles </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda</strong></div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucion" runat="server" controlWidth="70%" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Folio de contrato:</span>
                                    <asp:TextBox ID="TextBoxFolioContrato" runat="server" placeholder="Sólo Números" MaxLength="10" CssClass="form-control" ToolTip="Proporciona el número de convenio modificatorio"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioContrato"
                                        runat="server" TargetControlID="TextBoxFolioContrato" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">RIUF en contrato:</span>

                                    <asp:TextBox ID="TextBoxRIUF" onkeypress="validaRiuf(this);" runat="server" placeholder="##-#####-#" MaxLength="10" CssClass="form-control" ToolTip="Proporciona la clave RIUF del Inmueble de Arrendamiento"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" TargetControlID="TextBoxRIUF" ValidChars="0123456789-"></cc1:FilteredTextBoxExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxRIUF" Enabled="false"
                                        ValidationExpression="^\d{2}-\d{5}-\d{1}$" Display="Dynamic" SetFocusOnError="true" EnableClientScript="true" ErrorMessage="Formato de RIUF inválido" CssClass="error text-danger" ValidationGroup="CamposBusqueda"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">País:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListPais" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DropDownListPais_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Estado:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListEdo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEdo_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Municipio:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListMpo" runat="server" CssClass="form-control" ToolTip="Debe seleccionar primero un Estado, para exponer sus Municipios">
                                        <asp:ListItem Value="0">--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" ValidationGroup="CamposBusqueda" Visible="true" />

                                    <!-- Linea eliminada para ocultar el boton a solicitud el usuario 12 de nov 2019 Raymundo Peralta -->
                                    
                                    <!-- <asp:Button ID="ButtonRegistrarInmueble" runat="server" CssClass="btn btn-default" OnClick="ButtonRegistrarInmueble_Click" Text="Registrar dirección " ToolTip="Ir al formulario para registrar una dirección de un inmueble para arrendamiento" Visible="true" /> -->

                                   
                                </div>
                            </div>
                        </div>
                        <p>
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Direcciones de arrendamiento registrados y sus movimiento(s) de contratos</strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView ID="GridViewResult" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="GridViewResult_RowCommand" Font-Size="Small" OnRowCreated="GridViewResult_RowCreated" OnRowDataBound="GridViewResult_RowDataBound" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewResult_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="IdInmuebleArrendamiento" HeaderText="IdInmueble" />
                                    <%--  <asp:BoundField DataField="NombreInmueble" HeaderText="Denominación inmueble">
                                        <ItemStyle Font-Bold="True" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección inmueble" />
                                    <asp:BoundField Visible="false" DataField="FechaAltaMvtoAInmueble" HeaderText="Movimiento y fecha de registro" />
                                    <asp:BoundField Visible="false" DataField="PromoventeConCargo" HeaderText="Promovente que registró" />
                                    <asp:BoundField DataField="FolioContratoArrto" HeaderText="Folio contrato arrendamiento">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ContratoArrtoInmueble.DescripcionTipoArrendamiento" HeaderText="Tipo de arrendamiento" />
                                    <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución que registró" />
                                    <asp:BoundField DataField="RIUFInmueble" HeaderText="RIUF" ItemStyle-Width="150px" />
                                    <asp:TemplateField HeaderText="Operaciones de convenio modificatorio:">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkNuevoContrato" runat="server" CommandName="ConvenioModificatorio" Text="►Convenio modificatorio" />
                                            <%--<asp:LinkButton ID="LinkButtonAcuseContrato" runat="server" Text="►Acuse" CommandName="AcuseConvenio"/>--%>
                                            <a style="cursor: pointer" id="LinkButtonAcuseContrato" runat="server">►Acuse</a>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="True" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                            <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group" style="text-align: center;">
                                    <button id="ButtonExportarExcel" class="btn btn-default" tooltip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcel_Click"><span class="glyphicon glyphicon-export"></span>Exportar tabla a excel</button><br />
                                    <br />
                                    <asp:Label ID="LabelInfoGridResult" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div class="modal fade" id="mdlConvenios">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <h4 class="modal-title">Lista de convenios modificatorios</h4>
                        </div>

                        <div class="modal-body">

                            <div id="divMsjconvenios"></div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <table id="tblConvenios" style="width:100%" class="table table-bordered table-striped">
                                            <thead>
                                                <tr>                                                    
                                                    <th style="text-align:center">Folio del convenio</th>
                                                    <th style="text-align:center">Acción</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>


                        </div>

                        <div class="modal-footer pad-footer-mdl">
                            <input type="button" value="Cancelar" class="btn btn-default" data-dismiss="modal" />
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcel" />
        </Triggers>

    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgressBusqMvtosConvenioModificatorio" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelBusqMvtosConvenioModificatorio">
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
