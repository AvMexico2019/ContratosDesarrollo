<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="BusqMvtosSAEF.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.SAEF.BusqMvtosSAEF" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="ContentBusqMvtosSAEF1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="ContentBusqMvtosSAEF2" ContentPlaceHolderID="cphBody" runat="server">


    <asp:UpdatePanel ID="UpdatePanelBusqMvtosSAEF" runat="server">
        <ContentTemplate>

            <%--CONTROLADOR PARA LA INFORMACION DEL USUARIO--%>
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />

            <%--PANEL PRINCIPAL--%>
            <div class="panel panel-primary">
                <div class="panel-heading" >Programa nacional de accesibilidad a inmuebles en uso de la federación.</div>


                <%--PANEL DE BUSQUEDA--%>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda</strong></div>
                    <div class="panel-body">

                       <%-- PRIMERA FILA QUE CONTIENE  LA INSTITUCION Y EL FOLIODE EMISION DE OPINION --%>
                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucionSAEF" runat="server" controlWidth="70%" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Folio de emisión de opinión:</span>
                                    <asp:TextBox ID="TextBoxFolioOpinionSAEF" runat="server" MaxLength="10" placeholder="Sólo Números" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioOpinion"
                                        runat="server" TargetControlID="TextBoxFolioOpinionSAEF" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Folio de accesibilidad:</span>
                                    <asp:TextBox ID="TextBoxFolioSAEF" runat="server" MaxLength="10" placeholder="Sólo Números" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" TargetControlID="TextBoxFolioSAEF" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>

                        </div>

                        <%--FILA QUE CONTIENE EL ESTADO Y MUNICIPIO--%>
                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Estado:</span>
                                    <asp:DropDownList ID="DropDownListEdoSAEF" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEdoSAEF_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-8">
                                <div class="form-group">
                                    <span class="control-label">Municipio:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListMpoSAEF" runat="server" CssClass="form-control" ToolTip="Debe seleccionar primero un Estado, para exponer sus Municipios">
                                        <asp:ListItem Value="0">--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>

                         <%--BOTON DE BUSQUEDA DE LA EMISION DE OPINION--%>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:Button ID="ButtonConsultarSAEF" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultarSAEF_Click"  Visible="false" />
                                        <button class="btn btn-primary" tooltip="Da clic para consultar los registros de emision de opinión" type="button" runat="server"  onserverclick="ButtonConsultarSAEF_Click"><span class="glyphicon glyphicon-search"></span>Consultar</button>
                                    </div>
                                </div>
                            </div>

                       <%-- MOSTRAMOS LA  INFORMACION QUE SE LE MUESTRA AL USUARIO A ENCONTRAR REGISTROS--%>
                        <p>
                            <asp:Label ID="LabelInfoSAEF" runat="server"></asp:Label>
                        </p>



                    </div>
                </div>


                <%--PANEL QUE MUESTRA LA INFORMACION  DEL GRID PARA SELECCIONAR EL FOLIO DE EMISION DE OPINION--%>
                <div class="panel-default">
                    <div class="panel-heading"><strong>Direcciones de arrendamiento registrados y sus movimiento(s) de emisión de opinión</strong></div>
                    <div class="panel-body">

                        <%--FILA QUE CONTIENE ELM GRID CON LA INFORMACION E LAS EMISIONES DE OPNION--%>
                        <div class="row">
                            
                            <%--CONTIENE EL GRID--%>
                            <asp:GridView ID="GridViewResultadoSAEF" runat="server" AutoGenerateColumns="false" OnRowDataBound="GridViewResultadoSAEF_RowDataBound"
                                CssClass="table table-striped" OnRowCommand="GridViewResultadoSAEF_RowCommand" Font-Size="Small"
                                OnRowCreated="GridViewResultadoSAEF_RowCreated" PageSize="5" AllowPaging="true" 
                                OnPageIndexChanging="GridViewResultadoSAEF_PageIndexChanging" DataKeyNames="FolioSAEF">

                                <Columns>

                                    <asp:BoundField DataField="IdInmuebleArrendamiento" HeaderText="IdInmueble" />
                                    <asp:BoundField DataField="NombreInmueble" HeaderText="Denominación Inmueble">
                                        <ItemStyle Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección Inmueble" />
                                    <asp:BoundField DataField="FechaAltaMvtoAInmueble" HeaderText="Mvto. y fecha de registro" />
                                    <asp:BoundField DataField="PromoventeConCargo" HeaderText="Promovente que registró" />
                                    <asp:BoundField DataField="EmisionOpinion.FolioSMOI_AplicadoOpinion" HeaderText="Folio SMOI">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmisionOpinion.FolioAplicacionConcepto" HeaderText="Folio emisión de opinión">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmisionOpinion.TemaAplicacionConcepto" HeaderText="Tipo emisión opinión">
                                        <ItemStyle Font-Bold="False" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="FolioSAEF" HeaderText="Folio de SAEF">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FolioContratoArrtoVsInmuebleArrendado" HeaderText="Folio contrato arrendamiento">
                                        <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución que registró" />
                                    <asp:TemplateField HeaderText="Operación:">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkNuevoOpinionSAEF"  runat="server"  CommandName="NuevaOpinionSAEF" Text="►Nueva" />
                                            <asp:LinkButton ID="LinkButtonAcuseOpinionSAEF" runat="server" Text="►Acuse" OnClick="LinkButtonAcuseOpinionSAEF_Click"/>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="True" />
                                    </asp:TemplateField>

                                </Columns>

                                <PagerSettings Position="TopAndBottom" />
                                <PagerStyle CssClass="pagination-ys" />

                            </asp:GridView>
                             <asp:Label ID="lblTableNameSAEF" runat="server" Visible="False"></asp:Label>
                        </div>

                        <%--BOTON PARA EXPORTAR A EXCEL--%>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group" style="text-align: center;">
                                    <button id="ButtonExportarExcelSAEF" class="btn btn-default" tooltip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcelSAEF_ServerClick"><span class="glyphicon glyphicon-export"></span>Exportar tabla a excel</button><br />
                                    <br />
                                    <asp:Label ID="LabelInfoGridResultSAEF" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>

                    </div>
                </div> 

            </div>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcelSAEF" />
             <asp:PostBackTrigger ControlID="GridViewResultadoSAEF" />
        </Triggers>

    </asp:UpdatePanel>

    <%--MOSTRAMOS EL GIF PARA VER QUE ESTA LOADING--%>
    <asp:UpdateProgress ID="UpdateProgressSAEF2" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelBusqMvtosSAEF">
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

