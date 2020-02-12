<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Principal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Validar() {
        }
        

        //Constantes de Datos Generales
        const msjVersionExplorer = "<div class='alert alert-warning'><strong>¡Precaución! </strong>Estás utilizando un navegador web con una version no compatible con este sitio. <br />Esto significa que algunas características no funcionarán como se pretende. Esto puede resultar en comportamientos extraños al navegar en el sitio. Actualiza o instala uno de los siguientes navegadores para aprovechar al máximo este sitio web. <br>* Internet Explorer 11 o superior. <br />* Google Chrome 55 o superio.r</div>";
        const msjBrowserInvalido = "<div class='alert alert-danger'><strong>¡Error! </strong>Estás utilizando un navegador web no compatible con este sitio. <br />Esto significa que algunas características no funcionarán como se pretende. Esto puede resultar en comportamientos extraños al navegar en el sitio. Actualiza o instala uno de los siguientes navegadores para aprovechar al máximo este sitio web. <br>* Internet Explorer 11 o superior. <br />* Google Chrome 55 o superior.</div>";
        const msjsesiónCorrecta = "<div class='alert alert-success'><strong>¡Enhorabuena! </strong> Has iniciado sesión correctamente</div>";

        function ValidaExplorador() {
            document.getElementById('Panel1').style.display = 'block'; 
            var nav = navigator.userAgent.toLowerCase();            
            
            if (nav.indexOf("msie") != -1) {
                alert(document.documentMode);
                if (document.documentMode) {
                    if (document.documentMode < 11) {
                        document.getElementById('<%=LabelInfo.ClientID%>').innerHTML = msjVersionExplorer;
                        return;
                    }
                }
            }
            else if (nav.indexOf("firefox") != -1) {
                //document.getElementById('<%=LabelInfo.ClientID%>').innerHTML = msjBrowserInvalido;
            } else if (nav.indexOf("opera") != -1) {
                //document.getElementById('<%=LabelInfo.ClientID%>').innerHTML = msjBrowserInvalido;
            } else if (nav.indexOf("chrome") != -1) {
                //document.getElementById('<%=LabelInfo.ClientID%>').innerHTML = msjBrowserInvalido;
            } else {
                if (document.documentMode) {
                    if (document.documentMode < 11) {
                        document.getElementById('<%=LabelInfo.ClientID%>').innerHTML = msjVersionExplorer;
                        return;
                    }
                }
            }
        }

        function sesiónCorrecta() {
            document.getElementById('Panel2').style.display = 'block'; // to show
            document.getElementById('<%=LabelInfoSession.ClientID%>').innerHTML = msjsesiónCorrecta;            
        }
        
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="row" id="Panel2" style="display:none">
        <div class="col-md-12">
            <div class="form-group" style="text-align: center;">
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="LabelInfoSession" runat="server"></asp:Label>
                <br />
            </div>
        </div>
    </div>
    <div class="row" id="Panel1" style="display:none">
        <div class="col-md-12">
            <div class="form-group" style="text-align: center;">
                <br />
                <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                <br />
            </div>
        </div>
    </div>
     <div  style="text-align:right">
         <asp:HyperLink ID="HyperLinkCerrarSession" runat="server"> Cerrar sesión</asp:HyperLink>        
     </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
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
