<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="WebFormPruebasConMasterPage.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.WebFormPruebasConMasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


        <script>
            $(function () {

                var x;
                x = $(document);
                x.ready(inicializarEventos);

                function inicializarEventos() {
                    var btn;
                    btn = $('#ButtonEnviarOpinionX');
                    btn.click(ValidaDatos);
                    $('#TextBoxFolioSMOI').focus();
                }

                function ValidaDatos() {
                    var folioSMOI = document.getElementById('<%=TextBoxFolioSMOI.ClientID%>').value;
                    var existeDictamen = document.getElementById('<%=DropDownListDictamen.ClientID%>').options[document.getElementById("<%=DropDownListDictamen.ClientID%>").selectedIndex].text;
                    //validar dato requerido
                    if (folioSMOI == '' && existeDictamen == 'No')
                    {
                        document.getElementById('dvBodyMdl').innerHTML = "<strong>Excepción de Normatividad</strong>";
                       
                    }
                    else {
                        document.getElementById('dvBodyMdl').innerHTML = "<strong>Ok, enviar</strong>";
                    }
                    $('#myModal').modal('show');
                }

            }
             )
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            alert('aqui');

                $("#<%= txtDate.ClientID %>").datepicker({
            //showOn: "button",
            //buttonImage: generateURL("/Images/Calendar.jpg"),
            buttonImageOnly: true

        });

        });
        </script>

        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                Mostrar ejemplo de ventana Modal

                Folio SMOI: <asp:TextBox ID="TextBoxFolioSMOI" runat="server"></asp:TextBox>   
                <br />
                Cuenta con Dictamen Excepcion: 
                <asp:DropDownList ID="DropDownListDictamen" runat="server">
                    <asp:ListItem>--</asp:ListItem>
                    <asp:ListItem Value="1">Si</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList> 

                <br />

        <br />
       <!-- Button trigger modal -->
          <button type="button" id="ButtonEnviarOpinionX" class="btn btn-primary btn-lg">Guardar</button>


        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel">Modal title</h4>
              </div>
              <div class="modal-body" id="dvBodyMdl">
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-primary">Enviar</button>
              </div>
            </div>
          </div>
        </div>



</asp:Content>
