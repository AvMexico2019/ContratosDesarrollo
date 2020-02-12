<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Opinion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.Opinion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
  
    

         
        <div class="panel panel-default">
            <div class="panel-heading"><strong> EMISIÓN DE OPINIÓN PARA LA CONTRATACIÓN DE ARRENDAMIENTO </strong>
            </div>
            <div class="panel-body">
                <table style="text-align: center; margin: auto; width:100%">
                    <tr>
                         <td>Tipo de Contrato para el que se solicita Emisión de Opinión de Arrendamiento<br />
                             <asp:DropDownList ID="DropDownListTipoArrto" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListTipoArrto_SelectedIndexChanged">
                             </asp:DropDownList>
                             <br />
                             <br />
                             <asp:Button ID="ButtonAgregar" runat="server" CssClass="btn btn-primary" OnClick="ButtonAgregar_Click" Text="Capturar" />
                             <br />
                             <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                         </td>
                    </tr>
                </table>
      
                <br />
       
                <asp:Table ID="TableEmisionOpinion" class="table table-bordered" runat="server">
                </asp:Table>

                   <asp:Table ID="TablePiePagina" class="table table-bordered" runat="server">
                </asp:Table>

                <div>
                    <center>
                         <asp:Button ID="ButtonEnviarOpinion" runat="server" Text="Enviar" CssClass="btn btn-primary" Visible="False" />
                    </center>
                </div>
             </div> 
       </div>

</asp:Content>
