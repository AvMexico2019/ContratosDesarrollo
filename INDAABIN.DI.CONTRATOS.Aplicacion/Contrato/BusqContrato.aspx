<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="BusqContrato.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.BusqContrato" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function openCustomWindow(folio) {
            window.open('../Contrato/AcuseContrato.aspx?IdFolio=' + folio, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Contratos registrados a la institución</div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda </strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucion" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Tipo de contrato:</span>
                                    <asp:DropDownList ID="DropDownListTipoContrato" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Folio de contrato:</span>
                                    <asp:TextBox ID="TextBoxFolioContrato" CssClass="form-control" placeholder="Sólo números" runat="server"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioContrato"
                                        runat="server" TargetControlID="TextBoxFolioContrato" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group" style="text-align: left;">
                                    <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" Visible="false" />
                                    <asp:Button ID="ButtonNueva" runat="server" CssClass="btn btn-default" OnClick="ButtonNueva_Click" Text="Nuevo" />
                                    <asp:Button ID="ButtonRegistradosConExcepcion" runat="server" CssClass="btn btn-default" OnClick="ButtonRegistradosConExcepcion_Click" Text="Registrados con excepción" />
                                    <button class="btn btn-primary" ToolTip="Da clic para consultar los registros de contrato de arrendamiento" type="button" runat="server" onserverclick="ButtonConsultar_Click" ><span class="glyphicon glyphicon-search"></span>  Consultar</button>
                                    <br />
                                    <br />
                                    <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Contratos de arrendamiento registrados </strong></div>
                    <div class="panel-body">
                            <div class="row">   
                                <asp:GridView ID="GridViewBusqContratos" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="GridViewBusqContratos_RowDataBound" Font-Size="Small" CssClass="table table-striped" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewBusqContratos_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="FolioContratoArrto" HeaderText="Folio contrato">
                                            <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InmuebleArrto.NombreInstitucion" HeaderText="Institución" />
                                        <asp:BoundField DataField="strFecharegistro" HeaderText="Fecha registro"></asp:BoundField>
                                        <asp:BoundField DataField="InmuebleArrto.DireccionCompleta" HeaderText="Dirección Inmueble" />
                                        <asp:BoundField DataField="DescripcionTipoContrato" HeaderText="Tipo de contrato">
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DescripcionTipoArrendamiento" HeaderText="Tipo arrendamiento" />
                                        <asp:BoundField DataField="DescripcionTipoContratacion" HeaderText="Tipo de contratación" NullDisplayText="N/A" />
                                        <asp:BoundField DataField="DescripcionTipoOcupacion" HeaderText="Tipo ocupación (Otras Fig. Ocupación)" NullDisplayText="N/A" />
                                        <asp:BoundField DataField="PeriodoOcupacion" HeaderText="Periodo de ocupación" />
                                        <asp:BoundField DataField="InmuebleArrto.RIUFInmueble" HeaderText="RIUF" />
                                        <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButtonAcuseContrato" runat="server" CausesValidation="false" Text="Acuse"></asp:LinkButton>
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
            <br />
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
