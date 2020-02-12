<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="InmuebleArrto.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.InmuebleArrto" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register src="Direccion.ascx" tagname="Direccion" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
         function WebForm_OnSubmit12() {
             //alert('WebForm_OnSubmit');
             var validationGroupName = 'Direcciones';
             if (!Page_ClientValidate(validationGroupName)) {
                 for (var i in Page_Validators) {
                     try {
                         if (Page_Validators[i].validationGroup == validationGroupName) {
                             var control = document.getElementById(Page_Validators[i].controltovalidate);
                             if (!Page_Validators[i].isvalid) {
                                 control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                                 control.parentElement.className = "form-group has-error";
                             } else {
                                 control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                                 control.parentElement.className = "form-group";
                             }
                         }
                     } catch (e) { }
                 }
                 return false;
             }
             return true;
         }

         function backFromErrorClass(control)
         {
             //alert('backFromErrorClass');
             if (control != null)
             {
                 if (control.value != ' ' && control.value != '' && control.value != '--' && control.value != '0' && control.value != '00000') {
                     control.parentElement.parentElement.childNodes[1].className = "form-group-required-as";
                     control.parentElement.className = "form-group";
                 }
                 else {
                     control.parentElement.parentElement.childNodes[1].className = "form-group-required-red";
                     control.parentElement.className = "form-group has-error";
                 }
             }
         }

         function validaRiuf(control) {
             var val = control.value.replace(/\D/g, '');
             var newVal = '';

             if (val.length > 3) {
                 control.value = val;
             }
             if ((val.length > 2) && (val.length < 8)) {
                 newVal += val.substr(0, 2) + '-';
                 val = val.substr(2);
             }
             if (val.length > 7) {
                 newVal += val.substr(0, 2) + '-';
                 newVal += val.substr(2, 5) + '-';
                 val = val.substr(7, 1);
             }
             newVal += val;
             control.value = newVal;
         }

         function abreDomicilios() {
             window.open('DireccionInmueble.aspx?IdInmuebleArrendamiento=0', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1080, height = 630', 'true');
         }

         function openIDEForm() {
             var response = new Array('', '', '', '', '', '');
             var vEstadoId, vMunicipioId, vCodigoPostal, vTipoGeometria, vWkt, vX, vY, vEditar;
             vEstadoId = $('#cphBody_Direccion_DropDownListEdo').val();
             vMunicipioId='';
             vCodigoPostal = $('#cphBody_Direccion_TextBoxCP').val();
             vTipoGeometria = $('#cphBody_Direccion_tipoGeometria').val();
             vWkt = $('#cphBody_Direccion_wkt').val();
             vX = $('#cphBody_Direccion_x').val();
             vY = $('#cphBody_Direccion_y').val();
             vEditar = 'true';
             if (vEstadoId == '--' || vCodigoPostal == '')
             {
                 alert('Debe seleccionar un estado y código postal');
                 return;
             }
             // Se omite esta funcion porque Chrome no adminte el llamado a la funcion showModalDialog
             //response = window.showModalDialog('wfSeleccionMapa.aspx?EstadoId=' + vEstadoId + '&MunicipioId=' + vMunicipioId + '&CP=' + vCodigoPostal + '&TipoGeometria=' + vTipoGeometria + '&Wkt=' + vWkt + '&vX=' + vX + '&vY=' + vY + '&Editar=true', '', 'dialogHeight:600 px;dialogWidth:650 px; ;scroll:off; resizable:off'); , width = 690, height = 650
             window.open('../Geoposicion/wfSeleccionMapa.aspx?EstadoId=' + vEstadoId + '&MunicipioId=' + vMunicipioId + '&CP=' + vCodigoPostal + '&TipoGeometria=' + vTipoGeometria + '&Wkt=' + vWkt + '&vX=' + vX + '&vY=' + vY + '&Editar=true', '_blank', 'top = 10, toolbar = no, scrollbars = no, resizable = no, width = 690, height = 650', 'true');
             //window.open('../Geoposicion/wfSeleccionMapa.aspx?EstadoId=9&MunicipioId=7&CP=09310&TipoGeometria=' + vTipoGeometria + '&Wkt=' + vWkt + '&vX=' + vX + '&vY=' + vY + '&Editar=true', '_blank', 'top = 10, toolbar = no, scrollbars = no, resizable = no, width = 690, height = 670', 'true');
         }

         // Se implementa esta funcion para ser llamada desde el PopUp wfSeleccionMapa y recuperar los valores seleccionados en el mapa
         function asignaValoresRetornoMapa(responseCatched) {
             var response = new Array('', '', '', '', '', '');
             response = responseCatched;
             //alert(response);
             if (response != null) {
                 if (response[5] != '') {
                     document.getElementById('cphBody_Direccion_Edo').value = response[0];
                     document.getElementById('cphBody_Direccion_Mun').value = response[1];
                     document.getElementById('cphBody_Direccion_tipoGeometria').value = response[2];
                     document.getElementById('cphBody_Direccion_x').value = response[3];
                     document.getElementById('cphBody_Direccion_TextBoxLatitud').value = response[3];
                     document.getElementById('cphBody_Direccion_y').value = response[4];
                     document.getElementById('cphBody_Direccion_TextBoxLongitud').value = response[4];
                     document.getElementById('cphBody_Direccion_wkt').value = response[5];
                     // Se asigna wl valor wkt al textbox que valida la entrada de datos en el mapa
                     elemento = document.getElementById('cphBody_txtDMapaWrt');
                     if (elemento != null) {
                         elemento.setAttribute("value", response[5]);
                     }
                 }
             }
         }

         function asignaValores(responseCatched) {
             var response = new Array("", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
             response = responseCatched;
             if (response != null) {
                 document.getElementById('cphBody_hRiuf').value = response[0];
                 document.getElementById('cphBody_hPais').value = response[1];
                 document.getElementById('cphBody_hTipoInmueble').value = response[2];
                 document.getElementById('cphBody_hEdo').value = response[3];
                 document.getElementById('cphBody_hMun').value = response[4];
                 document.getElementById('cphBody_hColonia').value = response[5];
                 document.getElementById('cphBody_hOtraColonia').value = response[6];
                 document.getElementById('cphBody_hNombreVialidad').value = response[7];
                 document.getElementById('cphBody_hDenominacionDireccion').value = response[8];
                 document.getElementById('cphBody_hCP').value = response[9];
                 document.getElementById('cphBody_hTipoVialidad').value = response[10];
                 document.getElementById('cphBody_hNumExterior').value = response[11];
                 document.getElementById('cphBody_hNumInterior').value = response[12];
                 document.getElementById('cphBody_hGeoRefLatitud').value = response[13];
                 document.getElementById('cphBody_hGeoRefLongitud').value = response[14];
                 document.getElementById('cphBody_hIdInmueble').value = response[15];
                 document.getElementById('cphBody_hCodigoPostalExtranjero').value = response[16];
                 document.getElementById('cphBody_hEstadoExtranjero').value = response[17];
                 document.getElementById('cphBody_hCiudadExtranjero').value = response[18];
                 document.getElementById('cphBody_hMunicipioExtranjero').value = response[19];
                 document.getElementById("cphBody_btnCargaDireccion").click();
             }
         }
     </script>

    <asp:UpdatePanel ID="upCaptura" runat="server">
        <ContentTemplate>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <asp:Label ID="LabelInfoEnc" runat="server" Text=""></asp:Label>
            <input id="btnLanzaDomicilios" type="button" onclick="abreDomicilios();" value="Consultar domicilios existentes" class="btn btn-primary" ToolTip="Da clic para consultar la dirección del Inmueble" style="display:none" />
            <button class="btn btn-primary" ToolTip="Da clic para consultar la dirección del Inmueble" type="button" onclick="abreDomicilios();"><span class="glyphicon glyphicon-search"></span>  Consultar domicilios existentes</button>
            <br />
            <input type="hidden" id="hRiuf" runat="server" />
            <input type="hidden" id="hPais" runat="server" />
            <input type="hidden" id="hEdo" runat="server" />
            <input type="hidden" id="hMun" runat="server" />
            <input type="hidden" id="hTipoInmueble" runat="server" />
            <input type="hidden" id="hNombreVialidad" runat="server" />
            <input type="hidden" id="hDenominacionDireccion" runat="server" />
            <input type="hidden" id="hCP" runat="server" />
            <input type="hidden" id="hColonia" runat="server" />
            <input type="hidden" id="hOtraColonia" runat="server" />
            <input type="hidden" id="hTipoVialidad" runat="server" />
            <input type="hidden" id="hNumExterior" runat="server" />
            <input type="hidden" id="hNumInterior" runat="server" />
            <input type="hidden" id="hGeoRefLatitud" runat="server" />
            <input type="hidden" id="hGeoRefLongitud" runat="server" />
            <input type="hidden" id="hIdInmueble" runat="server" />                     
            <input type="hidden" id="hCodigoPostalExtranjero" runat="server" />
            <input type="hidden" id="hEstadoExtranjero" runat="server" />
            <input type="hidden" id="hCiudadExtranjero" runat="server" />
            <input type="hidden" id="hMunicipioExtranjero" runat="server" />

            <asp:Button ID="btnCargaDireccion" runat="server" Text="" OnClick="btnCargaDireccion_Click" CssClass="invisibleButton" style="visibility:hidden" />
            <uc1:Direccion ID="Direccion" runat="server" />
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group" style="text-align: center;">
                        <asp:Button ID="ButtonCancelar" runat="server" CssClass="btn btn-default" OnClick="ButtonCancelar_Click" Text="Cancelar" ToolTip="Cancelar e ir a Movimientos de Inmuebles" />
                        <asp:Button ID="ButtonGuardar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClientClick="return WebForm_OnSubmit12();" OnClick="ButtonGuardar_Click" ToolTip="Da clic para guardar la dirección del Inmueble " />
                        <br />
                        <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        <br />
                    </div>
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




