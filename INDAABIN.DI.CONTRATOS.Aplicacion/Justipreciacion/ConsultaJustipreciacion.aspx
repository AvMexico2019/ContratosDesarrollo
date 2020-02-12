<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ConsultaJustipreciacion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Justipreciacion.ConsultaJustipreciacion" %>

<%@ Register Assembly="AjaxControlToolKit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">

        //function openCustomWindow(ruta) {
        //    window.open('../Contrato/AcuseContrato.aspx?IdFolio=' + folio, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true')
        //}

    </script>

    <%--PANEL DONDE MOSTRARA LAS OPCIONES DE BUSQUEDA --%>
    <div class="panel panel-default">
        <div class="panel-heading"><strong>Panel de Búsqueda</strong></div>
        <div class="panel-body">
            <div class="row">

                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Secuencial:</span>
                        <asp:TextBox ID="TextBoxSecuencial" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Genérico:</span>
                        <asp:TextBox ID="TextBoxGenerico" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="col-md-8">
                    <div class="form-group">
                        <asp:Button ID="ButtonRegistrarSecuencial" runat="server" OnClick="ButtonRegistrarSecuencial_Click" CssClass="btn btn-default" Text="Registrar Secuencial " Visible="false" />
                        <button class="btn btn-default" ToolTip="Ir al formulario para registrar un secuencial" type="button" runat="server" onserverclick="ButtonRegistrarSecuencial_Click" ><span class="glyphicon glyphicon-pencil"></span>  Registrar Secuencial</button>

                        <%--ValidationGroup="CamposBusqueda"--%>
                        <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" OnClick="ButtonConsultar_Click" Visible="false" />
                        <button class="btn btn-primary" tooltip="Da clic para consultar los registros" type="button" runat="server" onserverclick="ButtonConsultar_Click"><span class="glyphicon glyphicon-search"></span>Consultar</button>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--PANEL PARA MOSTRAR LA LISTA DE DATOS--%>
    <div class="panel panel-default">
        <div class="panel-heading"><strong>Consulta de secuenciales</strong></div>
        <div class="panel-body">
            <div class="row">

                <asp:GridView ID="GridViewBusqJustipreciacion" runat="server" Width="100%" OnRowDataBound ="GridViewBusqJustipreciacion_RowDataBound" Font-Size="Medium" AutoGenerateColumns="False" CssClass="table table-striped" AllowPaging="True" OnPageIndexChanging = "GridViewBusqJustipreciacion_PageIndexChanging" PageSize="5">
                    <Columns>
                        <asp:BoundField DataField="NoSecuencial" HeaderText="Secuencial">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Font-Bold="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NoGenerico" HeaderText="Genérico" />
                        <asp:BoundField DataField="SuperficieRentableDictaminado" HeaderText="Superficie Dictaminada" />
                        <asp:BoundField DataField="MontoDictaminado" HeaderText="Monto Dictaminado" />
                       <%-- <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkPdfJustipreciacion" runat="server" CausesValidation="false" Text="PDF"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                    </Columns>
                    <PagerSettings Position="TopAndBottom" />
                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>

                <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group" style="text-align: center;">

                        <br />
                        <asp:Label ID="LabelInfoGridResult" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

