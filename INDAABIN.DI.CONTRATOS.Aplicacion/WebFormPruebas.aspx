<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormPruebas.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.WebFormPruebas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    abreGenexus();

    <script type="text/javascript">
        function abreGenexus() {
            window.open('http://200.76.24.107/AvaluosGx/arptcalificacion2.aspx?41783,68', '_blank');
            //alert(document.getElementById("Button1"));
            document.getElementById("Button1").click();
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
         <div>
            <asp:Table ID="tblData" runat="server"></asp:Table>
         </div>

    </div>
        <asp:Label ID="LabelInfo" runat="server"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <br />
        <br />
        <br />
        <asp:TextBox ID="TextBoxAcentos" runat="server" Height="18px" Width="238px">Mañana será otro día</asp:TextBox>
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Quitar acentos" />
        <br />
        <br />
        <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged">0.00</asp:TextBox>
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Convertir" />
        <br />
        <br />
        <br />
        email para:
        <asp:TextBox ID="TextBoxEmail" runat="server" Height="21px" Width="309px">desa10@funcionpublica.gob.mx</asp:TextBox>
        <br />
        <asp:Button ID="ButtonEnviarEmail" runat="server" OnClick="ButtonEnviarEmail_Click" Text="Enviar" />
        <br />
        <asp:Label ID="LabelInfoEmail" runat="server"></asp:Label>
        <br />
        <br />
        Secuencial<br />
        <asp:TextBox ID="TextBoxSecuencial" runat="server">4372</asp:TextBox>
        <br />
        <asp:Button ID="ButtonSecuencial" runat="server" OnClick="ButtonSecuencial_Click" Text="obtener" />
        <br />
        <asp:Label ID="LabelInfoSec" runat="server" style="font-weight: 700"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        --------Digito verificador --------------------------<br />
        Por Ejemplo de <strong>53-000028-1</strong> fue uno:<br />
        <br />
        IdEstado: <asp:TextBox ID="TextBoxIdEdo" runat="server"></asp:TextBox>
&nbsp;<br />
        Ultimo Contador de Consecutivo de Inmueble Arrandado para el Edo:
        <asp:TextBox ID="TextBoxUltConsecutivo" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="DV" runat="server" OnClick="DV_Click" Text="Digito verificador" />
        <br />
        <asp:Label ID="LabelDV" runat="server" style="font-weight: 700"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="ButtonGeneraExcepcion" runat="server" OnClick="ButtonGeneraExcepcion_Click" Text="Generea Excepcion" />
        <br />
        <asp:Label ID="LabelInfoEx" runat="server" style="font-weight: 700"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <input id="btnGenexus" type="button" value="Genexus" onclick="abreGenexus();" /><br />
        <br />
        <br />
        <br />



    </form>
</body>
</html>
