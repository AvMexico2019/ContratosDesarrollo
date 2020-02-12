<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="BusqMvtosOtrasFigurasOcupacion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.BusqMvtosOtrasFigurasOcupacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="UsrSSO" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <script type="text/javascript">
        function openCustomWindow(folio) {
            window.open('../Contrato/AcuseOtraFigura.aspx?IdFolio=' + folio, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');
        }
        
        function validaRiuf(control) {            
            var val = control.value.replace(/\D/g, '');
            var newVal = '';

            //alert('val: .' + val + '.');
            //alert('newVal: ' + newVal + '.');

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
                val = val.substr(7,1);
            }
            newVal += val;
            control.value = newVal;
        }


    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <UsrSSO:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />
            <div class="panel panel-primary">
                <div class="panel-heading">Contratos de otras figuras de ocupación</div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Panel de búsqueda</strong></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Institución:</span>
                                    <asp:DropDownList ID="DropDownListInstitucion" runat="server" controlWidth="70%" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Folio de contrato:</span>
                                    <asp:TextBox ID="TextBoxFolioContrato" runat="server" placeholder="Sólo Números" MaxLength="10" CssClass="form-control" ToolTip="proporciona el número de contrato  de arrendamiento o de otras figuras de ocupación"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFolioContrato"
                                        runat="server" TargetControlID="TextBoxFolioContrato" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">RIUF en contrato:</span>
                                    <asp:TextBox ID="TextBoxRIUF" onkeypress="validaRiuf(this);" runat="server" placeholder="##-#####-#" MaxLength="10" CssClass="form-control" ToolTip="proporciona la clave RIUF del Inmueble de Arrendamiento"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" TargetControlID="TextBoxRIUF" ValidChars="0123456789-"></cc1:FilteredTextBoxExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxRIUF" Enabled="false"  
                                        ValidationExpression="^\d{2}-\d{5}-\d{1}$" Display="Dynamic" SetFocusOnError="true" EnableClientScript="true" ErrorMessage="Formato de RIUF inválido" CssClass="error text-danger" ValidationGroup="CamposBusqueda"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">País:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListPais" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DropDownListPais_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Estado:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListEdo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListEdo_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span class="control-label">Municipio:</span>
                                    <br />
                                    <asp:DropDownList ID="DropDownListMpo" runat="server" CssClass="form-control" ToolTip="Debe seleccionar primero un Estado, para exponer sus Municipios">
                                        <asp:ListItem Value="0">--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8">
                                <div class="form-group">
                                    <asp:Button ID="ButtonConsultar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="ButtonConsultar_Click" ValidationGroup="CamposBusqueda" Visible="false" />
                                    <asp:Button ID="ButtonRegistrarInmueble" runat="server" CssClass="btn btn-default" OnClick="ButtonRegistrarInmueble_Click" Text="Registrar dirección " ToolTip="Ir al formulario para registrar una dirección de un inmueble para arrendamiento" Visible="true" />
                                    <button class="btn btn-default" ToolTip="Ir al formulario para registrar una dirección de un inmueble para arrendamiento" type="button" runat="server" onserverclick="ButtonRegistrarInmueble_Click" style="display:none"><span class="glyphicon glyphicon-pencil"></span>  Registrar dirección</button>
                                    <button class="btn btn-primary" ToolTip="Da clic para consultar los registros de otras figuras de ocupacion" type="button" runat="server" validationgroup="CamposBusqueda" onserverclick="ButtonConsultar_Click" ><span class="glyphicon glyphicon-search"></span>  Consultar</button>
                                </div>
                            </div>
                        </div>
                        <p>
                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading"><strong>Direcciones registradas y sus movimiento(s) de contratos</strong></div>
                    <div class="panel-body">
                            <div class="row">
                                <asp:GridView ID="GridViewResult" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="GridViewResult_RowCommand" Font-Size="Small" OnRowCreated="GridViewResult_RowCreated" OnRowDataBound="GridViewResult_RowDataBound" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewResult_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="IdInmuebleArrendamiento" HeaderText="IdInmueble" />
                                        <asp:BoundField DataField="NombreInmueble" HeaderText="Denominación Inmueble">
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección Inmueble" />
                                        <asp:BoundField DataField="FechaAltaMvtoAInmueble" HeaderText="Movimiento y fecha de registro" />
                                        <asp:BoundField DataField="PromoventeConCargo" HeaderText="Promovente que registró" />
                                        <asp:BoundField DataField="EmisionOpinion.FolioAplicacionConcepto" HeaderText="Folio emisión de opinión">
                                            <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContratoArrtoInmueble.FolioContratoArrto" HeaderText="Folio contrato">
                                            <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContratoArrtoInmueble.DescripcionTipoContrato" HeaderText="Tipo contrato" />
                                        <asp:BoundField DataField="ContratoArrtoInmueble.DescripcionTipoArrendamiento" HeaderText="Tipo de arrendamiento" />
                                        <asp:BoundField DataField="NombreInstitucion" HeaderText="Institución que registró" />
                                        <asp:BoundField DataField="ContratoArrtoInmueble.RIUF" HeaderText="RIUF" />
                                        <asp:TemplateField HeaderText="Operaciones de contrato:">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkNuevoContrato" runat="server" CommandName="NuevoContrato" Text="►Nuevo" />
                                                <asp:LinkButton ID="LinkSustitucionContrato" runat="server" CommandName="SustitucionContrato" Text="►Sustitución" />
                                                <asp:LinkButton ID="LinkContinuacionContrato" runat="server" CommandName="ContinuacionContrato" Text="►Continuación" />
                                                <asp:LinkButton ID="LinkButtonOtrasFigOcupacion" runat="server" CommandName="OtrasFigOcupacion" Text="►Otras figuras ocupación" />
                                                <asp:LinkButton ID="LinkButtonAcuseContrato" runat="server" Text="►Acuse" />
                                            </ItemTemplate>
                                            <ItemStyle Font-Bold="True" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" />
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>
                                <asp:Label ID="lblTableName" runat="server" Visible="False"></asp:Label>
                            </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group" style="text-align: center;">
                                    <button id="ButtonExportarExcel" class="btn btn-default" ToolTip="Da clic para descargar los registros en un archivo excel." type="button" runat="server" onserverclick="ButtonExportarExcel_Click" ><span class="glyphicon glyphicon-export"></span>  Exportar tabla a excel</button><br />
                                    <br />
                                    <asp:Label ID="LabelInfoGridResult" runat="server"></asp:Label>
                                </div>
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
