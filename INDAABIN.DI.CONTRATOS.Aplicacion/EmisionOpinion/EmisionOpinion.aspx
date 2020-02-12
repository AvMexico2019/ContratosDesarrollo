<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"  MaintainScrollPositionOnPostback="true" CodeBehind="EmisionOpinion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.EmisionOpinion" %>
<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
           

<div class="panel panel-primary">
    <div class="panel-heading">
        <asp:Label ID="LabelInfoEncabezadoPanelPrincipal" runat="server"></asp:Label>
    </div>
    <div class="panel-body">
        
            <%--controles de usuario--%>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <Direccion:DireccionLectura runat="server" id="ctrlDireccionLectura" />
           
            <table>
                <tr>
                     <td  style="text-align:justify">
                        <strong>Manual de Recursos Materiales y Servcios Generales  
                        166.</strong>
                        <br />
                        Las Instituciones Públicas deberán justificar ante el INDAABIN a efecto de economizar, racionalizar, lograr la eficiencia y transparentar el gasto público, previo a la celebración del contrato correspondiente, los nuevos arrendamientos, continuación de los mismos, o en su caso, sustitución de alguno anterior, sujetándose a las medidas de ahorro, austeridad y eficiencia establecidas en el Presupuesto de Egresos de la Federación para el ejercicio fiscal correspondiente y al formato contenido en la dirección de Internet www.indaabin.gob.mx, a fin de que dicho Instituto emita la opinión correspondiente.                      
                     </td>

                </tr>
                <tr>
                     <td style="text-align:justify">
                         <br />
                        <asp:Label ID="LabelInfo" runat="server" BackColor="#FFFFCC" class="control-label"></asp:Label>
                    </td>
                </tr>
                
            </table>
                
            <div class="panel-body">

                    <asp:Table ID="TableEmisionOpinion" class="table table-bordered" runat="server">
                    </asp:Table>

                    <asp:Table ID="TablePiePagina" class="table table-bordered" runat="server">
                    </asp:Table>

                </div>
            </div>
    <br />
        <div style="text-align:center">                       
              <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClick="ButtonCancelar_Click" Text="Cancelar" />

                    <asp:Button ID="ButtonverificarNormatividad" runat="server" CssClass="btn btn-default" OnClick="ButtonverificarNormatividad_Click" Text="Verificar normatividad" ToolTip="Expone que conceptos presentan excepción en el cumplimiento de la Normatividad, sin guardar la información" />

              <asp:Button ID="ButtonEnviarOpinion" runat="server" Text="Enviar" CssClass="btn btn-primary"  OnClick="ButtonEnviarOpinion_Click" />
              <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                                <img alt="" src="../Imagenes/ajax-loader.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>  
            <br />
           <asp:Label ID="LabelInfoEnviar" runat="server"></asp:Label>
        </div>
    </div> 
      

</asp:Content>