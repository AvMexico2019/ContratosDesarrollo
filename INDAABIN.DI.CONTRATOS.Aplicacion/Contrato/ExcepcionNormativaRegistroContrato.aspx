<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"  MaintainScrollPositionOnPostback="true"  AutoEventWireup="true" CodeBehind="ExcepcionNormativaRegistroContrato.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.ExcepcionNormativaRegistroContrato" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Excepción de normatividad en contratos registrados</div>
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
                                    <button class="btn btn-primary" tooltip="Da clic para consultar los registros con excepción" type="button" runat="server" onserverclick="ButtonConsultar_Click"><span class="glyphicon glyphicon-search"></span>Consultar</button>
                                    <br />
                                    <br />
                                    <asp:Label ID="LabelInfo" runat="server"></asp:Label>

                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Contratos de arrendamiento registrados con excepción en la normativa </strong></div>
                    <div class="panel-body">
                            <div class="row">
                                <asp:GridView ID="GridViewBusqContratos" runat="server" AutoGenerateColumns="False" Width="100%" Font-Size="Small" CssClass="table table-striped" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewBusqContratos_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="FolioContratoArrto" HeaderText="Folio Contrato">
                                            <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución" />
                                        <asp:BoundField DataField="strFecharegistro" HeaderText="Fecha Registro"></asp:BoundField>
                                        <asp:BoundField DataField="InmuebleArrto.DireccionCompleta" HeaderText="Dirección Inmueble" />
                                        <asp:BoundField DataField="DescripcionTipoContrato" HeaderText="Tipo de Contrato">
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PeriodoOcupacion" HeaderText="Periodo de Ocupación" />
                                        <asp:BoundField DataField="DescripcionExcepcionTipoNormativa" HeaderText="Excepción Normativa" />
                                        <asp:BoundField DataField="ObservacionesExcepcionNormativa" HeaderText="Observaciones Excepción" />
                                        <asp:ButtonField CommandName="Acuse" HeaderText="Ver" Text="Acuse" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Font-Bold="True" />
                                        </asp:ButtonField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                            </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group" style="text-align: center;">
                            <button id="ButtonExportarExcel" class="btn btn-default" tooltip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcel_Click"><span class="glyphicon glyphicon-export"></span>Exportar tabla a excel</button><br />

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

