<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="BusquedaSAEF.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.SAEF.BusquedaSAEF" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="ContentBusqSAEF1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="ContentBusqSAEF2" ContentPlaceHolderID="cphBody" runat="server">

    <%--LUGAR EN DONDE VAN LOS SCRIPTS--%>


    <asp:UpdatePanel ID="UpdatePanelBuqSAEF1" runat="server">

        <ContentTemplate>

            <%--CONTROLADOR PARA LA INFORMACION DEL USUARIO--%>
           
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />


            <%--PANEL PRINCIPAL--%>
            <div class="panel panel-primary">
                <div class="panel-heading">Solicitudes de accesibilidad a la institución</div>

                <%--PANEL DE BUSQUEDA--%>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda </strong></div>
                    <div class="panel-body">

                      <%--  FILA DE LA INSTITUCION --%>
                        <div class="row">

                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucionSAEF" CssClass="form-control" runat="server" Width="70%">
                                    </asp:DropDownList>
                                </div>
                            </div>

                           

                            
                            </div>

                        <%--  FILA DE  FOLIO DE EMISION DE OPINION Y DE FOLIO SAEF--%>
                        <div class="row">

                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Folio de solicitud de opinión:</span>
                                    <asp:TextBox ID="TextBoxFolioSolicitudSAEF" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioSolicitudSAEF"
                                        runat="server" TargetControlID="TextBoxFolioSolicitudSAEF" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <span class="control-label">Folio de solicitud de accesibilidad:</span>
                                    <asp:TextBox ID="TextBoxFolioSAEF" runat="server" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioSAEF"
                                        runat="server" TargetControlID="TextBoxFolioSAEF" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>

                        </div>

                        <%--AQUI SE ENCUENTRA EL BOTON DE CONSULTAR--%>
                        <p>
                            <asp:Button ID="ButtonConsultarSAEF" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultarSAEF_Click" Visible="false" />
                            <button class="btn btn-primary" ToolTip="Da clic para consultar los registros de accesibilidad" type="button" runat="server" onserverclick="ButtonConsultarSAEF_Click" ><span class="glyphicon glyphicon-search"></span>  Consultar</button>
                            <br />
                            <br />
                            <asp:Label ID="LabelInfoBusqSAEF" runat="server"></asp:Label>
                        </p>
                    </div>

                </div>


                <%--PANEL PARA LAS SOLICITUD DE ACCESIBILIDAD EMITIDAS--%>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Solicitudes de accesibilidad de arrendamiento emitidas </strong></div>
                    <div class="panel-body">

                        <%--FILA QUE CONTIENE EL GRID QUE MUESTRA LA INFORMACION--%>
                        <div class="row">
                            <asp:GridView ID="GridViewSolicitudesOpinionEmitidasSAEF" runat="server" AutoGenerateColumns="False"
                                 Width="100%" OnRowDataBound="GridViewSolicitudesOpinionEmitidasSAEF_RowDataBound" 
                                Font-Size="Small" CssClass="table table-striped" AllowPaging="True" PageSize="5" 
                                OnPageIndexChanging="GridViewSolicitudesOpinionEmitidasSAEF_PageIndexChanging" DataKeyNames="FolioSAEF">

                                <Columns>
                                    <asp:BoundField DataField="FolioSAEF" HeaderText="Folio accesibilidad">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha registro" />
                                   <%-- <asp:BoundField DataField="PorcentajeAccesibiliadad" HeaderText="Porcentaje de accesibilidad" />--%>
                                    <asp:BoundField DataField="FolioAplicacionConcepto" HeaderText="Folio emisión aplicado">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución" />
                                    <asp:BoundField DataField="InmuebleArrto.DireccionCompleta" HeaderText="Dirección Inmueble" />
                                    <asp:BoundField DataField="NombreUsuario" HeaderText="Promovente que registra" />
                                    <asp:BoundField DataField="NombreCargo" HeaderText="Cargo promovente" />
                                    <asp:BoundField DataField="InmuebleArrto.FolioContratoArrtoVsInmuebleArrendado" HeaderText="Folio contrato en que se aplico">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAcuseSAEF" runat="server" CausesValidation="false" Text="Acuse" OnClick="lnkAcuseSAEF_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>

                                <PagerSettings Position="TopAndBottom" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>

                            <asp:Label ID="lblTableNameSAEF" runat="server" Visible="False"></asp:Label>
                        </div>
                    </div>
                </div>

                <%--POSICION DEL BOTON PARA EXPORTAR A EXCEL--%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group" style="text-align: center;">
                            <button id="ButtonExportarExcelSAEF" class="btn btn-default" ToolTip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcelSAEF_ServerClick" ><span class="glyphicon glyphicon-export"></span>  Exportar tabla a excel</button><br />

                        </div>
                    </div>
                </div>

            </div>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcelSAEF" />
            <asp:PostBackTrigger ControlID="GridViewSolicitudesOpinionEmitidasSAEF" />
        </Triggers>

    </asp:UpdatePanel>

    <%--MUESTRA EL CARGADO DE LA VENTANA--%>
     <asp:UpdateProgress ID="UpdateProgressBusqSAEF1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelBuqSAEF1">
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