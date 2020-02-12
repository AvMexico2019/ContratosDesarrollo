<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"  MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Opinion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.Opinion" %>
<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="Direccion" TagName="UsuarioInfo" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">


           
<div class="panel panel-primary">

    <div class="panel-heading"><strong> EMISIÓN DE OPINIÓN PARA LA CONTRATACIÓN DE UN NUEVO ARRENDAMIENTO </strong>
    </div>
    <div class="panel-body">
        
            <%--controles de usuario--%>
            <Direccion:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <Direccion:DireccionLectura runat="server" id="ctrlDireccionLectura" />

            <span style="text-align:center" > 
                 <asp:Label ID="LabelInfo" runat="server" BackColor="#FFFFCC" class="control-label"></asp:Label>
            </span>
            <br /><br />  

                    
                       
        <div class="panel-heading"><strong> CONCEPTOS DE OPINIÓN PARA LA EMISION DE OPINION SOBRE LA CONTRATACIÓN DE UN NUEVO ARRENDAMIENTO </strong>
        </div>
            <div class="panel-body">

                    <asp:Table ID="TableEmisionOpinion" class="table table-bordered" runat="server">
                    </asp:Table>

                        <asp:Table ID="TablePiePagina" class="table table-bordered" runat="server">
                    </asp:Table>

                </div>
            </div>

        <div style="text-align:center">                       
              <asp:Button ID="ButtonEnviarOpinion" runat="server" Text="Enviar" CssClass="btn btn-primary" Visible="False" OnClick="ButtonEnviarOpinion_Click" />
              <br />
              <asp:Label ID="LabelInfoEnviar" runat="server"></asp:Label>
        </div>
    </div> 
      

</asp:Content>
