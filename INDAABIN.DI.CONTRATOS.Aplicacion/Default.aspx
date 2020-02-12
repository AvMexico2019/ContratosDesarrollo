<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Direcciones de Inmuebles</title>
    <link href="../css/EstilosEspecificos.css" rel="stylesheet" />
    <link href="https://framework-gb.cdn.gob.mx/assets/styles/main.css" rel="stylesheet" />
    <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon' />
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group-required-as">
                        <span class="control-label">Denominación de la dirección:</span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TextBoxNombreDireccion" CausesValidation="true" ValidationGroup="Direcciones"  runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTextBoxNombreDireccion" ValidationGroup="Direcciones" runat="server" ErrorMessage="Este campo es obligatorio" ControlToValidate="TextBoxNombreDireccion" Display="Dynamic" SetFocusOnError="True" CssClass="error text-danger" />                    
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="Direcciones" />
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function WebForm_OnSubmit() {
            if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) {
                for (var i in Page_Validators) {
                    try {
                        var control = document.getElementById(Page_Validators[i].controltovalidate);
                        if (!Page_Validators[i].isvalid) {
                            control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                            control.parentElement.className = "form-group has-error";
                        } else {
                            control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                            control.parentElement.className = "form-group";
                        }
                    } catch (e) { }
                }
                return false;
            }
            return true;
        }
    </script>
</body>
</html>

