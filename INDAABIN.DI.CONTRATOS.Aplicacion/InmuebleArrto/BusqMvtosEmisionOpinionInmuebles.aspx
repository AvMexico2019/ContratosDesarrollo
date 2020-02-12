<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="BusqMvtosEmisionOpinionInmuebles.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.BusqMvtosEmisionOpinionInmuebles" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function openCustomWindow(folio,tipo) {
            window.open('../EmisionOpinion/AcuseEmisionOpinion.aspx?IdFolio=' + folio + '&TipoArrto=' + tipo, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');
        }
    </script>
    <asp:UpdatePanel ID="upCaptura" runat="server">
        <ContentTemplate>

            <%--RCA 03/07/2018--%>
            <br />
            <br />
            <br />
            <br />

            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Solicitudes de emisión de opinión a direcciones de arrendamiento </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda de direcciones registradas a la institución</strong></div>
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
                                    <span class="control-label">Folio de solicitud de opinión:</span>
                                    <asp:TextBox ID="TextBoxFolioOpinion" runat="server" MaxLength="10" placeholder="Sólo Números" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioOpinion"
                                        runat="server" TargetControlID="TextBoxFolioOpinion" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Folio de SMOI:</span>
                                    <asp:TextBox ID="TextBoxFolioSMOI" runat="server" MaxLength="10" CssClass="form-control" placeholder="Sólo Números" ToolTip="proporciona el número de  Folio SMOI que se aplicó a una Emisión de Opinión"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioContrato"
                                        runat="server" TargetControlID="TextBoxFolioSMOI" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Estado:</span>
                                    <asp:DropDownList ID="DropDownListEdo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEdo_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-8">
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
                                    <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" ValidationGroup="CamposBusqueda" Visible="false" />
                                    <asp:Button ID="ButtonRegistrarInmueble" runat="server" CssClass="btn btn-default" OnClick="ButtonRegistrarInmueble_Click" Text="Registrar dirección " ToolTip="Ir al formulario para registrar una dirección de un inmueble para arrendamiento" Visible="false" />
                                    <button class="btn btn-default" ToolTip="Ir al formulario para registrar una dirección de un inmueble para arrendamiento" type="button" runat="server" onserverclick="ButtonRegistrarInmueble_Click" ><span class="glyphicon glyphicon-pencil"></span>  Registrar dirección</button>
                                    <button class="btn btn-primary" ToolTip="Da clic para consultar los registros de emision de opinión" type="button" runat="server" validationgroup="CamposBusqueda" onserverclick="ButtonConsultar_Click" ><span class="glyphicon glyphicon-search"></span>  Consultar</button>
                                </div>
                            </div>
                        </div>
                        <p>
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>
                        <p>
                            <div class='alert alert-warning'><strong> ¡Importante! </strong><br /> Estimado usuario, en caso de requerir una Emisión de Opinión para una <strong>Sustitución de arrendamiento</strong>, deberá capturar la nueva dirección y elegir la opción <strong>Nuevo</strong> y cuando registre su contrato deberá elegir la opción de <strong>Sustitución</strong></div>
                            <p>
                            </p>
                        </p>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Direcciones de arrendamiento registrados y sus movimiento(s) de emisión de opinión </strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView ID="GridViewResult" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridViewResult_RowDataBound" 
                                CssClass="table table-striped" OnRowCommand="GridViewResult_RowCommand" Font-Size="Small" OnRowCreated="GridViewResult_RowCreated" PageSize="5" 
                                AllowPaging="True" OnPageIndexChanging="GridViewResult_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="IdInmuebleArrendamiento" HeaderText="IdInmueble" />
                                    <asp:BoundField DataField="NombreInmueble" HeaderText="Denominación Inmueble">
                                        <ItemStyle Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección Inmueble" />
                                    <asp:BoundField DataField="FechaAltaMvtoAInmueble" HeaderText="Mvto. y fecha de registro" />
                                    <asp:BoundField DataField="PromoventeConCargo" HeaderText="Promovente que registró" />
                                    <asp:BoundField DataField="EmisionOpinion.FolioSMOI_AplicadoOpinion" HeaderText="Folio SMOI">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmisionOpinion.FolioAplicacionConcepto" HeaderText="Folio emisión de opinión">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmisionOpinion.TemaAplicacionConcepto" HeaderText="Tipo emisión opinión">
                                        <ItemStyle Font-Bold="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FolioContratoArrtoVsInmuebleArrendado" HeaderText="Folio contrato arrendamiento">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución que registró" />
                                    <asp:TemplateField HeaderText="Operaciones de emisión de opinión:">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkNuevoOpinion" runat="server" CommandName="NuevaOpinion" Text="►Nueva" />
                                            <asp:LinkButton ID="LinkSustitucionOpinion" runat="server" CommandName="SustitucionOpinion" Text="►Sustitución" />
                                            <asp:LinkButton ID="LinkContinuacionOpinion" runat="server" CommandName="ContinuacionOpinion" Text="► Continuación" />
                                            <asp:LinkButton ID="LinkSeguridad" runat="server" CommandName="SeguridadOpinion" Text="►Seguridad" />
                                            <asp:LinkButton ID="LinkButtonAcuseOpinion" runat="server" Text="►Acuse" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="True" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Position="TopAndBottom" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                            <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group" style="text-align: center;">
                                    <button id="ButtonExportarExcel" class="btn btn-default" ToolTip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcel_Click" ><span class="glyphicon glyphicon-export"></span>  Exportar tabla a excel</button><br />
                                    <br />
                                    <asp:Label ID="LabelInfoGridResult" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="upCaptura">
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
