<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ContratoHistoricoXInstitucion.aspx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.Contrato.ContratoHistoricoXInstitucion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Src="~/UsuarioInfo.ascx" TagPrefix="Direccion" TagName="UsuarioInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>

    <%--controles de usuario--%>
    <Direccion:UsuarioInfo runat="server" ID="ctrlUsuarioInfo" />


    
    <div class="panel panel-primary">
    <div class="panel-heading">Contratos de arrendamiento en histórico </div>

          <div class="panel panel-default">
            <div class="panel-heading"><strong>Contratos de arrendamiento registrados a la institución y ubicación con el inmueble </strong></div>
             <div class="panel-body">
                 <asp:Label ID="LabelInfo" runat="server" Text="" ></asp:Label>   
                 
                 <br />
                 
                 <asp:GridView ID="GridViewContratosHistor" runat="server" Width="100%" Font-Size="Medium" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="GridViewContratosHistor_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="NumContratoHistorico" HeaderText="Num. contrato">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DireccionCompleta" HeaderText="Dirección Inmueble" />
                            <asp:BoundField DataField="FechaInicioContrato" HeaderText="Fecha inicio contrato" />
                            <asp:BoundField DataField="FechaFinContrato" HeaderText="Fecha fin contrato" />
                            <asp:BoundField DataField="FechaContrato" HeaderText="Fecha contrato" />
                            <asp:BoundField DataField="Propietario" HeaderText="Propietario" />
                            <asp:ButtonField CommandName="Seleccionar" HeaderText="Ver" Text="Seleccionar" >
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>

                  <div class="row">
                    <div class="col-md-12">
                        <div class="form-group" style="text-align: center;"> 
                            <asp:ImageButton ID="ImageButtonExcel" runat="server" 
                            ImageUrl="~/Imagenes/excel.png" 
                            ToolTip="Exportar a Excel" 
                            onclick="ImageButtonExcel_Click" Visible="False" Height="38px" Width="47px" />
                                                                          
                        </div>
                    </div>
             </div>

                 </div>   
               </div>  
      </div>   

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="ImageButtonExcel" />                              
    </Triggers>  
  </asp:UpdatePanel>  
</asp:Content>
