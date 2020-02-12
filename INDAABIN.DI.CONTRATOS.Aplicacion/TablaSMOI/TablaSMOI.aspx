<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="TablaSMOI.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI.TablaSMOI" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function actualizaPantalla() {
            window.location = 'BusqTablaSMOIEmitidas.aspx';
        }

        //window.onbeforeunload = function () { return; }
        //window.onbeforeunload = function () { return 'Es posible que los cambios no se guarden si no ho se dupliquen'; }
        window.onbeforeunload = function () { return 'Es posible que los cambios no se guarden, ¿Confirmas que deseas continuar con ésta acción? '; }

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

        function validateValue(control, maxValue) {
            //alert('backFromErrorClass');
            if (maxValue <= 0.0) {
                if (control != null) {
                    if (control.value != ' ' && control.value != '' && control.value != '--' && control.value != '0' && control.value != '-99') {
                        //control.parentElement.className = "form-group-required-as";
                        control.parentElement.className = "form-group";
                        return true;
                    }
                    else {
                        //control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                        control.parentElement.className = "form-group has-error";
                        return false;
                    }
                }
            }
            else {
                if (control != null) {
                    if (control.value != ' ' && control.value != '' && control.value != '--' && control.value != '0' && control.value != '-99') {
                        currentValue = control.value;
                        if (currentValue > maxValue) {
                            control.parentElement.className = "form-group has-error";
                            alert('El valor maximo ingresado debe ser ' + maxValue);
                            control.focus();
                            return false;
                        }
                        else {
                            control.parentElement.className = "form-group";
                            return true;
                        }
                    }
                    else {
                        //control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                        control.parentElement.className = "form-group has-error";
                        return false;
                    }
                }
            }            
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">       
        <ContentTemplate>
            <input type="hidden" id="lblNombreSession" runat="server" />
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Tabla de Superficie Máxima a Ocupar por Institución (SMOI)       
                </div>
                <div class="panel-body">
                    <br />
                    <br />
                    <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
                    <span style="text-align: center">
                        <asp:Label ID="LabelInfo" runat="server" BackColor="#FFFFCC" class="control-label"></asp:Label>
                    </span>
                    <div class="panel-body">
                        <asp:Panel ID="pnlTablaSmoi" runat="server">
                        <asp:Table ID="TableSMOI" class="table table-bordered" runat="server">
                        </asp:Table>
                        <asp:Table ID="TableTotalResultados" class="table table-bordered" runat="server">
                        </asp:Table>
                        <asp:Table ID="TablePiePagina" class="table table-bordered" runat="server">
                        </asp:Table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div style="text-align: center">
                <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClientClick="window.onbeforeunload = null;" OnClick="ButtonCancelar_Click" Text="Cancelar" />
                <asp:Button ID="ButtonEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="ButtonEnviar_Click" />
                <br />
                <br />
                <asp:Label ID="LabelInfoEnviar" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanel1">
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
