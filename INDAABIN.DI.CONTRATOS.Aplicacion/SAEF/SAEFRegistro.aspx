<%@ Page Title=""  Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="SAEFRegistro.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.SAEF.SAEFRegistro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>
<%@ Register Src="~/InmuebleArrto/DireccionLectura.ascx" TagPrefix="Direccion" TagName="DireccionLectura" %>


<asp:Content ID="ContentSAEF1" ContentPlaceHolderID="head" runat="server">

    <script src="../js/SAEF.js"></script>

    <style>

        table{
            width: 100%;
            height: 100%;
            font-size:12px;
           
        }

        
        td{
            /*text-align:center;*/
            padding:10px;
        }

        

    </style>

</asp:Content>

<asp:Content ID="ContentSAEF2" ContentPlaceHolderID="cphBody" runat="server">



    <asp:UpdatePanel ID="UpdatePanelSAEF" runat="server">
        <ContentTemplate>

            <%--OBTENEMOS LOS HIDEN DE LOS PARAMETROS DE ENTRADA QUE SE NECESITAN--%>
            <input type="hidden" id="lblIdEmisionOpinion" runat="server" />
            <input type="hidden" id="lblIdContrato" runat="server" />
            <input type="hidden" id="lblIdInmuebleArrendamiento" runat="server" />
            <input type="hidden" id="lblIdInmuebleRiuf" runat="server" />
            <input type="hidden" id="lblIdInstitucion" runat="server" />
            <input type="hidden" id="lblNombreInstitucion" runat="server" />
            <input type="hidden" id="lblNombreInstitucionActual" runat="server" />
            <input type="hidden" id="lblDireccionInmuebleArrendamiento" runat="server" />
            <input type="hidden" id="lblEsSustitucion" runat="server" />
            <input type="hidden" id="lblEsContinuacion" runat="server" />

            <input type="hidden" id="lblEstadoEmision" runat="server" />
            <input type="hidden" id="lblMunicpioEmision" runat="server" />
            <input type="hidden" id="lblCPEmision" runat="server" />

            <input type="hidden" id="lblEstadoJustipreciacion" runat="server" />
            <input type="hidden" id="lblMunicpioJustipreciacion" runat="server" />
            <input type="hidden" id="lblCPJustipreciacion" runat="server" />

            <%--MOSTRAMOS EL CONTROLADOR PARA VISUALIZAR LA INFORMACION DEL USUARIO--%>
            <br /> 
            <br />
            <br /> 
            <br />
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />

            <%--MOSTRAR LA INFORMACION DEL INMUEBLE--%>
            <Direccion:DireccionLectura runat="server" ID="ctrlDireccionLectura" />

            <%--PANEL PRINCIPAL DEL REGISTRO--%>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:Label runat="server">Registro de un nuevo SAEF</asp:Label>
                </div>

                <%--PANEL DEL PARA MOSTRAR TODO EL CONTENIDO PARA REGISTRAR--%>
                 <asp:Panel ID="pnlSAEF" runat="server">
                     <div class="panel-body">

                         <%--MOSTRAMOS LA SUGERENCIA DE DE LO QUE SE DEBE DE REALIZAR--%>
                         <div class="row">
                             <div class="col-md-12">
                                 <div class="form-group" style="text-align:center;">
                                     <br />
                                     <asp:Label runat="server">
                                         <div class='alert alert-info'>
                                             <strong>Leyenda: </strong> 
                                             Protesta de decir verdad. La información capturada es responsabilidad del servidor 
                                             público y la institución que la envía; el INDAABIN se reserva el derecho a solicitar 
                                             información adicional y/o probatoria” y conservando la validación actual que no permite 
                                             avanzar al paso siguiente a menos que se encuentre terminado el anterior en su totalidad.
                                         </div>
                                     </asp:Label>
                                 </div>
                             </div>
                         </div>

                         <%--MOSTRAMOS LA ALERTA POS SI SE OCACIONO ALGUNA INCIDENCIA--%>
                          <br />
                          <asp:Label ID="LabelInfoSAEF" runat="server" BackColor="#FFFFCC" class="control-label" Style="font-weight: 700"></asp:Label>

                         <%--MOSTRAMOS EL PANEL QUE MUESTRA LA INFORMACION DEL INMUEBLE--%>
                         <div class="panel panel-default">
                             <div class="panel-heading">
                                 Información del contrato
                             </div>
                             <div class="panel-body">

                                 <%--CONTIENE LA FILA DEL NUMERO DE FOLIO DE EMISION, USUARIO UE REGISTRO, FECHA DE REGISTRO--%>
                                 <div class="row">
                                     

                                     <div class="col-md-4">
                                         <div class="form-group">
                                             <label>Folio de emisión:</label>
                                             <asp:TextBox ID="txtFolioContratoSAEF" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                         </div>
                                     </div>

                                      <div class="col-md-4">
                                         <div class="form-group">
                                              <label>Usuario que registro la emisión:</label>
                                             <asp:TextBox ID="txtUsuarioRegistroSAEF" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                         </div>
                                     </div>

                                     <div class="col-md-4">
                                         <div class="form-group">
                                              <label>Fecha de registro de la emisión:</label>
                                             <asp:TextBox ID="txtFechaRegistroSAEF" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                         </div>
                                     </div>


                                    
                                 </div>

                             

                             </div>
                         </div>

                         <%--MOSTRAMOS EL PANEL COLAPSABLE QUE MUESTRA EL SERVICIO DEL INMUEBLE--%> 
                        <div class="panel-group ficha-collapse" id="accordionServiciosInmueble">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordionServiciosInmueble"  data-toggle="collapse"  href="#panel-ServicionInmueble"  aria-expanded="true" aria-controls="panel-ServicionInmueble"> 
                                                 Servicios del inmueble
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordionServiciosInmueble" data-toggle="collapse" href="#panel-ServicionInmueble"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-ServicionInmueble">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE A TODOS LOS CONCEPTOS DE SERVICIOS DEL INMUEBLE--%>
                                             <asp:Table ID="TableServicioInmueble" class="table" runat="server">
                                                 
                                             </asp:Table>

                                             
                                         </div>
                                     </div>
                                 </div>
                             </div>

                         <%--MOSTRAMOS EL PANEL DE LOS 7 INDICADORES--%>
                         <div class="panel panel-default">
                             <div class="panel-heading">
                                 <strong>Indicadores</strong>
                             </div>

                             <div class="panel-body">

                             <%--PANEL COLAPSABLE DEL INDICADOR ACCESO AL INMUEBLE--%>
                             <div class="panel-group ficha-collapse" id="accordion">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion"  data-toggle="collapse"  href="#panel-01"  aria-expanded="true" aria-controls="panel-01"> 
                                                 Acceso al inmueble
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion" data-toggle="collapse" href="#panel-01"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-01">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE LOS CONCEPTOS DE ACCESO AL INMUEBLE--%>
                                             <asp:Table ID="TableAccesoInmueble" class="table" runat="server">
                                             </asp:Table>

                                             <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="btnGuardarAccesoInmueble" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="btnGuardarAccesoInmueble_Click"/>
                                                     </div>
                                                 </div>
                                             </div>

                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <%--PANEL COLAPSABLE DEL INDICADOR VESTIBULO--%>
                             <div class="panel-group ficha-collapse" id="accordion02">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion02"  data-toggle="collapse"  href="#panel-02"  aria-expanded="true" aria-controls="panel-02"> 
                                                 Vestíbulo
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion02" data-toggle="collapse" href="#panel-02"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-02">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE LOS CONCEPTOS DE VESTIBULO--%>
                                              <asp:Table ID="TableVestibulo" class="table" runat="server">
                                             </asp:Table>
                                              <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="btnGuardarVestibulo" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="btnGuardarVestibulo_Click"/>
                                                     </div>
                                                 </div>
                                             </div>
                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <%--PANEL COLAPSABLE DEL INDICADOR CIRCULACIONES--%>
                             <div class="panel-group ficha-collapse" id="accordion03">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion03"  data-toggle="collapse"  href="#panel-03"  aria-expanded="true" aria-controls="panel-03"> 
                                                 Circulaciones
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion03" data-toggle="collapse" href="#panel-03"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-03">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE  CONTIENE LOS CONCEPTOS DE CIRCULACIONES--%>
                                             <asp:Table ID="TableCirculaciones" class="table" runat="server">
                                             </asp:Table>
                                             
                                              <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="btnGuardarCirculaciones" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="btnGuardarCirculaciones_Click"/>
                                                     </div>
                                                 </div>
                                             </div>

                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <%--PANEL COLAPSABLE DEL INDICADOR SEÑALIZACION--%>
                             <div class="panel-group ficha-collapse" id="accordion04">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion04"  data-toggle="collapse"  href="#panel-04"  aria-expanded="true" aria-controls="panel-04"> 
                                                 Señalización
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion04" data-toggle="collapse" href="#panel-04"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-04">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE LOS CONCEPTOS DE SEÑALIZACION--%>
                                             <asp:Table ID="TableSenalizacion" class="table" runat="server">
                                             </asp:Table>
                                             
                                              <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="btnGuardarSenalizacion" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="btnGuardarSenalizacion_Click"/>
                                                     </div>
                                                 </div>
                                             </div>

                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <%--PANEL COLAPSABLE DEL INDICADOR USO DE EDIFICIO Y SERVICIOS--%>
                             <div class="panel-group ficha-collapse" id="accordion05">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion05"  data-toggle="collapse"  href="#panel-05"  aria-expanded="true" aria-controls="panel-05"> 
                                                 Uso de edificio y servicio
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion05" data-toggle="collapse" href="#panel-05"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-05">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE LOS CONCEPTOS DE USO DE EDIFICACION Y SERVICIO--%>
                                             <asp:Table ID="TableUsoEdificioServicio" class="table" runat="server">
                                             </asp:Table>

                                              <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="BtnUsoEdificio" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="BtnUsoEdificio_Click"/>
                                                     </div>
                                                 </div>
                                             </div>

                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <%--PANEL COLAPSABLE DEL INDICADOR SANITARIOS PARA USO EXCLUSIVO--%>
                             <div class="panel-group ficha-collapse" id="accordion06">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion06"  data-toggle="collapse"  href="#panel-06"  aria-expanded="true" aria-controls="panel-06"> 
                                                 Sanitarios para uso exclusivo
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion06" data-toggle="collapse" href="#panel-06"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-06">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE LOS CONCEPTOS DE SANITARIO PARA USO EXCLUSIVO--%>
                                             <asp:Table ID="TableSanitariosUsoExclusivo" class="table" runat="server">
                                             </asp:Table>

                                              <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="BtnSanitarios" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="BtnSanitarios_Click"/>
                                                     </div>
                                                 </div>
                                             </div>

                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <%--PANEL COLAPSABLE DEL INDICADOR RUTA DE EVACUACION EMERGENTE--%>
                             <div class="panel-group ficha-collapse" id="accordion07">
                                 <div class="panel panel-default">
                                     <div class="panel-heading">
                                         <h4 class="panel-title">
                                             <a data-parent="#accordion07"  data-toggle="collapse"  href="#panel-07"  aria-expanded="true" aria-controls="panel-07"> 
                                                Ruta de evacuación emergente
                                             </a>
                                         </h4>
                                         <button type="button" class="collpase-button collapsed" data-parent="#accordion07" data-toggle="collapse" href="#panel-07"></button>
                                     </div>
                                     <div class="panel-collapse collapse" id="panel-07">
                                         <div class="panel-body">
                                              
                                            <%--LA TABLA QUE CONTIENE LOS CONCEPTOS DE RUTA DE EVACUACION EMERGENTE--%>
                                             <asp:Table ID="TableRutaEvacuacionEmergente" class="table" runat="server">
                                             </asp:Table>
                                             
                                              <div class="row">
                                                 <div class="col-md-12">
                                                     <div class="form-group">
                                                         <asp:Button runat="server" ID="btnRutaEvacuacion" CssClass="btn btn-primary btn-sm pull-right" Text="Guardar" OnClick="btnRutaEvacuacion_Click"/>
                                                     </div>
                                                 </div>
                                             </div>

                                         </div>
                                     </div>
                                 </div>
                             </div>

                         </div>

                         </div>

                     </div>
                 </asp:Panel>

                 <%--MOSTRAMOS LA ALERTA POS SI SE OCACIONO ALGUNA INCIDENCIA--%>
                          <br />
                          <asp:Label ID="LabelInfoSAEF2" runat="server" BackColor="#FFFFCC" class="control-label" Style="font-weight: 700"></asp:Label>


                 <%--MOSTRAMOS EL BOTON DE ENVIAR Y EL BOTON DE CANCELAR--%>
                <br />
                <div style="text-align:center">
                    <asp:Button ID="ButtonCancelarSAEF" runat="server" CssClass="btn btn-default" OnClientClick="window.onbeforeunload = null;" OnClick="ButtonCancelarSAEF_Click" Text="Cancelar" />
                   
                    <asp:Button ID="ButtonEnviarSAEF" runat="server" Text="Enviar" CssClass="btn btn-primary" ValidationGroup="Contratos" CausesValidation="true"  OnClick="ButtonEnviarSAEF_Click" Height="43px" ToolTip="Aceptar el envio de la información para su registro y generación del acuse" />

                    <asp:Button ID="ButtonDescargarSAEF" runat="server" Text="Descargar" CssClass="btn btn-primary" ValidationGroup="Contratos" CausesValidation="true"  OnClick="ButtonDescargarSAEF_Click" Height="43px" Visible="false" />
                   
                    <br /> 
                    <br />
                </div>

            </div>


          

        </ContentTemplate>

        <Triggers>
             <asp:PostBackTrigger ControlID="ButtonDescargarSAEF" />

        </Triggers>

    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgressEmisionSAEF" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelSAEF">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">
                <asp:Label ID="LblWaitEmisionSAEF" runat="server" Text="Espere un momento por favor..." Width="200px"
                    Style="font-size: 9pt; font-family: Arial; font-weight: bold; background-color: #003355; color: #FFFFFF;"> 
                </asp:Label><br />
                <br />
                <img src="../Imagenes/ajax-loader.gif" alt="Loading" style="border-style: none; border-width: 0px; border-color: Red;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>