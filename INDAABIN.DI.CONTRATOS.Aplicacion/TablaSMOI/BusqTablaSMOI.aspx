<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BusqTablaSMOI.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI.BusqTablaSMOI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <p>
        <br />
        Folio SMOI:
    </p>
    <p>
        <asp:TextBox ID="TextBoxFolioSMOI" runat="server">1</asp:TextBox>
    </p>
    <p>
        <asp:Button ID="ButtonBuscar" runat="server" OnClick="ButtonBuscar_Click" Text="Buscar" />
    </p>
    <p>
    </p>
</asp:Content>
