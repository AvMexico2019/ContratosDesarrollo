<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="AdminRIUF.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.AdministradorRIUF.AdminRIUF" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%-- RCA 15/05/2018--%>
    <%--STILO PARA EL SCROLL HORIZONTAL Y VERTICAL DE UN GRIDVIEW--%>
    <style type="text/css">

        .scrolling-table-container {
            height: 380px;
            overflow-y: scroll;
            overflow-x: scroll;
        }



    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

   <%--ZONA DE SCRIPTS--%>
    <script src="../js/AdminRIUF.js"></script>

    <asp:UpdatePanel ID="UpdatePanelAdminRIUF" runat="server">
        <ContentTemplate>

            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />

            <%--PANEL DE BUSQUEDA--%>
                <div class="panel panel-default">
                    <div class="panel panel-heading"><strong>Panel de búsqueda</strong></div>
                    <div class="panel-body">

                        <%--CHECK PARA MOSTRAR U OCULTAR LAS FECHAS--%>
                        <div class="row">

                            <div class="col-md-4">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" class="Fechas" value="RangoFechas" />Mostrar fechas.
                                    </label>
                                </div>
                            </div>

                        </div>

                        <%--FILA QUE CONTIENE LOS CAMPOS DE FECHAS--%>
                        <div id="CampoFechas" class="row">

                            <div class="col-md-4">
                                <div class="form-group datepicker-group">
                                    <label class="control-label" for="calendarInicio">Fecha inicio:</label>
                                    <input class="form-control" id="calendarInicio" type="text" runat="server" />
                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group datepicker-group">
                                    <label class="control-label" for="calendarFin">Fecha fin:</label>
                                    <input class="form-control" id="calendarFin" type="text" runat="server" />
                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                </div>
                            </div>
                        </div>

                        <%--FILA PARA INSTITUCION, RIUF, PAIS--%>
                        <div class="row">

                            <div class="col-md-4">
                                <label>Institución:</label>
                                <div class="form-group">
                                    <asp:DropDownList ID="DropDownListInstitucion" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>RIUF:</label>
                                    <asp:TextBox ID="txtRIUF" onkeypress="validaRiuf(this);" runat="server" placeholder="##-#####-#" MaxLength="10" CssClass="form-control"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRIUF" ValidChars="0123456789-" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtRIUF" Enabled="false" ValidationExpression="^\d{2}-\d{5}-\d{1}$" Display="Dynamic" SetFocusOnError="true" EnableClientScript="true" ErrorMessage="Formato de RIUF inválido" CssClass="error text-danger" ValidationGroup="CamposBusqueda"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>País:</label>
                                    <asp:DropDownList ID="DropDownListPais" runat="server"  CssClass="form-control" onchange="HabilitarCasillas(this.value);" ></asp:DropDownList> 
                                </div>
                            </div>

                        </div>

                        <%--FILA QUE CONTIENE ESTADO,MUNICIPIO,CP--%>
                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Estado:</label>
                                    <asp:DropDownList ID="DropDownListEstado" runat="server" OnSelectedIndexChanged="DropDownListEstado_SelectedIndexChanged" CssClass="form-control" AutoPostBack="true"></asp:DropDownList> <%--AutoPostBack="true"--%>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Municipio:</label>
                                    <asp:DropDownList ID="DropDownListMunicipio" runat="server"  CssClass="form-control" ></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Código Postal:</label>
                                    <asp:TextBox ID="txtCP" runat="server" MaxLength="5" CssClass="form-control" placeholder="#####" OnTextChanged="txtCP_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_TextBoxCP" runat="server" TargetControlID="txtCP" ValidChars="0123456789" />
                                    <asp:RegularExpressionValidator ID="rfvTextBoxCP" ValidationGroup="Direcciones" runat="server" ErrorMessage="El formato del CP no es válido" ControlToValidate="txtCP" Display="Dynamic" SetFocusOnError="true" CssClass="error text-danger"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                        </div>

                        <%--FILA QUE CONTIENE EL TIPO DE REGISTRO , EL ESTATUS DE RUSP Y FOLIO DE CONTRATO--%>
                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Tipo de registro</label>
                                    <asp:DropDownList ID="DropDownTipoRegistro" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Estatus en RUSP</label>
                                    <asp:DropDownList ID="DropDownRUSP" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Folio de contrato:</label>
                                    <asp:TextBox ID="txtFolioContrato" runat="server" MaxLength="10" CssClass="form-control" ></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioContrato" runat="server"  TargetControlID="txtFolioContrato" ValidChars="0123456789" />
                                </div>
                            </div>

                        </div>


                       <%-- FILA QUE CONTIENE LOS BOTONES DE C0OONUSLTAR Y DE  LIMPIAR--%>
                        <div class="row">

                            <div class="col-md-8">
                                <div class="form-group">
                                    <button class="btn btn-primary" type="button" runat="server" id="btnConsultar" onserverclick="btnConsultar_ServerClick"><span class="glyphicon glyphicon-search"></span>Consultar</button>
                                    <button class="btn btn-default" type="button" runat="server" id="btnLimpiar" onserverclick="btnLimpiar_ServerClick">Limpiar</button>
                                </div>
                            </div>

                        </div>


                        <%--NOTIFICACIONES PARA EL USUARIO--%>
                        <p>
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>

                    </div>
                </div>


           <%--PANEL DEL GRID--%>
            <div class="panel panel-default">
                <div class="panel-heading"><strong>Registros de contratos</strong></div>
                <div class="panel-body">

                    <div class="scrolling-table-container">
                    <%--<div class="row">--%>
                        <asp:GridView ID="GridViewRIUF" ClientIDMode="Static" runat="server" CssClass="table table-striped" 
                            AutoGenerateColumns="false" OnRowCommand="GridViewRIUF_RowCommand" Font-Size="Small"  
                            OnRowCreated="GridViewRIUF_RowCreated" OnRowDataBound="GridViewRIUF_RowDataBound" >
                            <Columns>
                                <asp:TemplateField> 
                                    <HeaderTemplate>
                                         <asp:CheckBox ID="CheckTodo" runat="server" Text="Todos" TextAlign="Right" OnCheckedChanged="CheckTodo_CheckedChanged"  AutoPostBack="true" />
                                    
                                    </HeaderTemplate>
                                    
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckRIUF" runat="server" />
                                    </ItemTemplate>

                                    

                                    <HeaderStyle Width="25px" HorizontalAlign="Center" Wrap="false"  />
                                </asp:TemplateField>

                                <asp:BoundField DataField="NombreInmueble" HeaderText="Denominación Inmueble" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección Inmueble" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ></asp:BoundField>
                                <asp:BoundField DataField="FechaAltaMvtoAInmueble" HeaderText="Movimiento y fecha de registro" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ></asp:BoundField>
                                <asp:BoundField DataField="FechaFinOcupacion" HeaderText="Fecha de finalización del contrato" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ></asp:BoundField>
                                <asp:BoundField DataField="PromoventeConCargo" HeaderText="Promovente que registró" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ></asp:BoundField>
                                <asp:BoundField DataField="FolioAplicacionConcepto" HeaderText="Folio emisión de opinión" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="FolioContratoArrto" HeaderText="Folio contrato arrendamiento" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="DescripcionTipoContrato" HeaderText="Tipo contrato" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="DescripcionTipoArrendamiento" HeaderText="Tipo de arrendamiento" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución que registró" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="RIUF" HeaderText="RIUF" ItemStyle-Width="20px" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="ObservacionesContratosReferencia" HeaderText="Observaciones"  HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="EstatusRUSP" HeaderText="Estatus RUSP" HeaderStyle-Width="20px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>
                                <asp:BoundField DataField="IdInmuebleArrendamiento" HeaderText="IdInmueble" />
                            </Columns>
                        </asp:GridView>

                        <asp:Label ID="lblTableName" runat="server" Visible="false"></asp:Label>
                    </div>

                    <%--BOTON PARA EXPORTAR A EXCEL--%>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group" > <%--style="text-align:center;"--%>
                                <button class="btn btn-primary" type="button" runat="server" id="btnHabilitar" onserverclick="btnHabilitar_ServerClick" visible="false">Habilitar</button>
                                <button class="btn btn-primary" type="button" runat="server" id="btnDeshabilitar" onserverclick="btnDeshabilitar_ServerClick" visible="false">Deshabilitar</button>
                                <button id="ButtonExportarExcel" class="btn btn-default" type="button" runat="server" onserverclick="ButtonExportarExcel_ServerClick" visible="false"><span class="glyphicon glyphicon-export"></span>Exportar tabla a Excel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonExportarExcel" />
        </Triggers>

    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgressAdminRIUF" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdatePanelAdminRIUF">
        <ProgressTemplate>
            <div class="overlay"/>
            <div class="overlayContent">
                <asp:Label ID="LblWait" runat="server" Text="Espere un momento por favor..." Width="200px"  Style="font-size: 9pt; font-family: Arial; font-weight: bold; background-color: #003355; color: #FFFFFF;"></asp:Label>
                <br />
                <br />
                <img src="../Imagenes/ajax-loader.gif" alt="Loading" style="border-style: none; border-width: 0px; border-color: Red;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>