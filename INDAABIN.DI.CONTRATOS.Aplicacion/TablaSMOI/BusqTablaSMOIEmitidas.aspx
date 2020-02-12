<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusqTablaSMOIEmitidas.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI.BusqTablaSMOIEmitidas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function openCustomWindow(folioSmoi) {
            //alert(folioSmoi);
            window.open('AcuseSMOI.aspx?IdFolio=' + folioSmoi, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Solicitudes emitidas de Tabla de Superficie Máxima a Ocupar por Institución (SMOI) </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de Búsqueda </strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucion" runat="server" CssClass="form-control" controlWidth="70%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Folio de SMOI:</span>
                                    <asp:TextBox ID="TextBoxFolioSMOI" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioSMOI"
                                        runat="server" TargetControlID="TextBoxFolioSMOI" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" Visible="false" />
                                    <asp:Button ID="ButtonNuevaSMOI" runat="server" CssClass="btn btn-default" OnClick="ButtonNuevaSMOI_Click" Text="Nueva" />
                                    <button class="btn btn-primary" ToolTip="Da clic para consultar los registros SMOI" type="button" runat="server" onserverclick="ButtonConsultar_Click" ><span class="glyphicon glyphicon-search"></span>  Consultar</button>
                                </div>
                            </div>
                        </div>
                        <p>
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Solicitudes de tabla SMOI emitidas a la institución</strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView ID="GridViewSolicitudesSMOIEmitidas" runat="server" Width="100%" OnRowDataBound="GridViewSolicitudesSMOIEmitidas_RowDataBound"  Font-Size="Medium" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="True" OnPageIndexChanging="GridViewSolicitudesSMOIEmitidas_PageIndexChanging" PageSize="5">
                                <Columns>
                                    <asp:BoundField DataField="FolioAplicacionConcepto" HeaderText="Folio SMOI">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SupM2XSMOI" DataFormatString="{0:N}" HeaderText="M2 de superficie SMOI" />
                                    <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución" />
                                    <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha de registro" />
                                    <asp:BoundField DataField="NombreUsuario" HeaderText="Promovente que registró" />
                                    <asp:BoundField DataField="NombreCargo" HeaderText="Cargo promovente" />
                                    <asp:BoundField DataField="FolioEmisionOpinion_DondeSeAplicoFolioSMOI" HeaderText="Folio Emisión Opinión en que se aplicó">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAcuseSMOI" runat="server" CausesValidation="false" Text="Acuse"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Position="TopAndBottom" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                            <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                        </div>                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group" style="text-align: center;">
                            <button id="ButtonExportarExcel" class="btn btn-default" ToolTip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcel_Click" ><span class="glyphicon glyphicon-export"></span>  Exportar tabla a excel</button><br />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcel" />
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
