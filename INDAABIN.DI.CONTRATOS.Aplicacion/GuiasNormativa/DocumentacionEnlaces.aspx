<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DocumentacionEnlaces.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.GuiasNormativa.DocumentacionEnlaces" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Documentación </div>
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                            <asp:GridView ID="GridViewResult" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" Font-Size="Small" AllowPaging="True" PageSize="5">
                                <Columns>
                                    <asp:BoundField DataField="NombreDocumento" HeaderText="Nombre" />
                                    <%--<asp:BoundField DataField="DescripcionDocumento" HeaderText="Descripción" />--%>
                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlDocumento" ToolTip="Ver" Target="_blank" runat="server" NavigateUrl='<%# Eval("URLDocumento") %>' Text='<span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span>&nbsp;&nbsp;<span>Ver</span>'></asp:HyperLink>
                                        </ItemTemplate>
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
                                    <asp:Label ID="LabelInfoGridResult" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
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

