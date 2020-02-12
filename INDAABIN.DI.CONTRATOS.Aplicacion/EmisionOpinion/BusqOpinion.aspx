﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="BusqOpinion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.BusqOpinion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function openCustomWindow(folio, tipo) {
            window.open('AcuseEmisionOpinion.aspx?IdFolio=' + folio + '&TipoArrto=' + tipo, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
             <%--CONTROLADOR PARA LA INFORMACION DEL USUARIO--%>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Solicitudes de opinión emitidas a la institución</div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda </strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-8">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucion" CssClass="form-control" runat="server" Width="70%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Tipo de solicitud:</span>
                                    <asp:DropDownList ID="DropDownListTipoOpinion" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">--</asp:ListItem>
                                        <asp:ListItem Value="2">Opinión Nuevo Arrendamiento</asp:ListItem>
                                        <asp:ListItem Value="3">Opinión Continuación Arrendamiento</asp:ListItem>
                                        <asp:ListItem Value="4">Opinión Sustitución Arrendamiento</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Folio de solicitud de opinión:</span>
                                    <asp:TextBox ID="TextBoxFolioSolicitud" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioSolicitud"
                                        runat="server" TargetControlID="TextBoxFolioSolicitud" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <p>
                            <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" Visible="false" />
                            <asp:Button ID="ButtonNueva" runat="server" CssClass="btn btn-default" OnClick="ButtonNueva_Click" Text="Nueva" />
                            <button class="btn btn-primary" ToolTip="Da clic para consultar los registros de emisión de opinión" type="button" runat="server" onserverclick="ButtonConsultar_Click" ><span class="glyphicon glyphicon-search"></span>  Consultar</button>
                            <br />
                            <br />
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Solicitudes de emisión de opinión de arrendamiento emitidas </strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView ID="GridViewSolicitudesOpinionEmitidas" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="GridViewSolicitudesOpinionEmitidas_RowDataBound" Font-Size="Small" CssClass="table table-striped" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewSolicitudesOpinionEmitidas_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="FolioAplicacionConcepto" HeaderText="Folio emisión opinión">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TemaAplicacionConcepto" HeaderText="Tipo opinión">
                                        <ItemStyle Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha registro" />
                                    <asp:BoundField DataField="ResultadoEmisionOpinion" HeaderText="Resultado opinión" />
                                    <asp:BoundField DataField="FolioSMOI_AplicadoOpinion" HeaderText="Folio SMOI aplicado">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución" />
                                    <asp:BoundField DataField="InmuebleArrto.DireccionCompleta" HeaderText="Dirección Inmueble" />
                                    <asp:BoundField DataField="NombreUsuario" HeaderText="Promovente que registra" />
                                    <asp:BoundField DataField="NombreCargo" HeaderText="Cargo promovente" />
                                    <asp:BoundField DataField="InmuebleArrto.FolioContratoArrtoVsInmuebleArrendado" HeaderText="Folio contrato en que se aplico">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
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
