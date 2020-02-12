<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="FundamentoLegalCpto.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.FundamentoLegalCpto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
     

    <table style="text-align:left ; width:100%">
        <tr>
            <td>
                <span class="control-label"> <strong>Aplicable al tipo de solicitud de emisión:</strong></span><br />
                <asp:Label ID="LabelTema" runat="server" Text=""></asp:Label> 
                 <br />
            </td>
        </tr>
        <tr>
             <td>
                <span class="control-label"> <strong>Número de concepto:</strong></span><br />
                <asp:Label ID="LabelNumCpto" runat="server" Text=""></asp:Label> 
                 <br />
            </td>
        </tr>
        <tr>
            <td>
                  <span class="control-label"><strong>Concepto:</strong></span><br />
                  <asp:Label ID="LabelDescCpto" runat="server" Text=""></asp:Label>
                  <br />
            </td>
            
        </tr>
        <tr>
            <td style="word-break:break-all;">
                 <span class="control-label"><strong>Fundamento legal:</strong></span><br />
                <asp:Label ID="LabelFundamentoLegalCpto" runat="server" Text=""></asp:Label>
                 <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelInfo" runat="server" Text="" style="font-weight: 700"></asp:Label>
            </td>
        </tr>
    </table>





</asp:Content>
