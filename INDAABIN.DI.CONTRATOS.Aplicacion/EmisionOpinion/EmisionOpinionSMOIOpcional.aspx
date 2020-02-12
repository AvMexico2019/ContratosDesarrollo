<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="EmisionOpinionSMOIOpcional.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion.EmisionOpinionSMOIOpcional" %>

<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function actualizaPantalla() {
            window.location = '../InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx';
        }

        window.onbeforeunload = function () { return 'Es posible que los cambios no se guarden, ¿Confirmas que deseas continuar con ésta acción? '; }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function backFromErrorClass(control) {
            //alert('backFromErrorClass');
            if (control != null) {
                if (control.value != ' ' && control.value != '' && control.value != '--' && control.value != '0' && control.value != '-99') {
                    //control.parentElement.className = "form-group-required-as";
                    control.parentElement.className = "form-group";
                }
                else {
                    //control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                    control.parentElement.className = "form-group has-error";
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="upCaptura" runat="server">
        <ContentTemplate>
            <asp:Label ID="LabelIdContrato" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="LabelIdInmuebleArrendamiento" runat="server" Visible="False"></asp:Label>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:Label ID="LabelInfoEncabezadoPanelPrincipal" runat="server"></asp:Label>
                </div>
                <br />
                <br />
                <br />
                <asp:Panel ID="pnlControles" runat="server">
                    <div class="panel-body">
                        <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
                        <Direccion:DireccionLectura runat="server" ID="ctrlDireccionLectura" />
                        <table>
                            <tr>
                                <td style="text-align: justify">
                                    <strong>Manual de Recursos Materiales y Servicios Generales  
                        166.</strong>
                                    <br />
                                    Las Instituciones Públicas deberán justificar ante el INDAABIN a efecto de economizar, racionalizar, lograr la eficiencia y transparentar el gasto público, previo a la celebración del contrato correspondiente, los nuevos arrendamientos, continuación de los mismos, o en su caso, sustitución de alguno anterior, sujetándose a las medidas de ahorro, austeridad y eficiencia establecidas en el Presupuesto de Egresos de la Federación para el ejercicio fiscal correspondiente y al formato contenido en la dirección de Internet www.indaabin.gob.mx, a fin de que dicho Instituto emita la opinión correspondiente.                      
                                </td>

                            </tr>
                            <tr>
                                <td style="text-align: justify">
                                    <br />
                                    <asp:Label ID="LabelInfo" runat="server" BackColor="#FFFFCC" class="control-label" Style="font-weight: 700"></asp:Label>
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
                </asp:Panel>
                <br />
                <div style="text-align: center">
                    <div style="text-align: center">
                        <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClientClick="window.onbeforeunload = null;" OnClick="ButtonCancelar_Click" Text="Cancelar" />
                        <asp:Button ID="ButtonverificarNormatividad" runat="server" CssClass="btn btn-default" OnClick="ButtonverificarNormatividad_Click" Text="Verificar normatividad" ToolTip="Expone que conceptos presentan excepción en el cumplimiento de la Normatividad, sin guardar la información" />
                        <asp:Button ID="ButtonEnviarOpinion" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="ButtonEnviarOpinion_Click" />
                        <br />
                    </div>
                    <br />
                    <asp:Label ID="LabelInfoEnviar" runat="server" Style="font-weight: 700"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="upCaptura">
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
