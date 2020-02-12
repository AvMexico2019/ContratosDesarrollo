<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="VistaQR.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.QR.VistaQR" %>


<asp:Content ID="ContentVistaQR1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="ContentVistaQR2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">

    </script>

    <asp:UpdatePanel ID="UpdatePanelVistaQR1" runat="server">
        <ContentTemplate>

            <%--PANEL PRINCIPAL--%>
            <div class="panel panel-primary">

                <div class="panel-heading">Solicitudes de accesibilidad a la institución</div>

                <br />
                <br />
                <asp:Label ID="LabelInfoBusqSAEF" runat="server"></asp:Label>

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

            </div>


             

        </ContentTemplate>

           <Triggers>
            <asp:PostBackTrigger ControlID="GridViewSolicitudesOpinionEmitidasSAEF" />
        </Triggers>

    </asp:UpdatePanel>

     <%--MUESTRA EL CARGADO DE LA VENTANA--%>
     <asp:UpdateProgress ID="UpdateProgressVistaQRSAEF" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelVistaQR1">
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