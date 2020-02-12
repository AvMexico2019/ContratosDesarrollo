<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DireccionLectura.ascx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto.DireccionLectura" %>    
 <div class="panel panel-default">
    <div class="panel-heading"><strong>Dirección del inmueble propuesto para el arrendamiento</strong></div>
        <div class="panel-body">            
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="LabelIdInmuebleArrto" runat="server" Visible="False"></asp:Label>
                        <span class="control-label">Denominación:</span>
                        <asp:Label ID="LabelNombreInmueble" runat="server" Text="" style="font-weight: 700"></asp:Label>
                    </div>
                </div>
                 <div class="col-md-6">
                    <div class="form-group">
                    
                        <span class="control-label">País:</span>
                        <asp:Label ID="LabelPais" runat="server" Text="" style="font-weight: 700"></asp:Label>
                    </div>
                </div>       
              </div>
              <div class="row">              
                 <div class="col-md-12">
                    <div class="form-group">
                        <span class="control-label">Dirección:</span>
                        <asp:Label ID="LabelDireccion" runat="server" Text="" style="font-weight: 700"></asp:Label>
                    </div>
                </div>
            </div> 
            <div class="row">              
                 <div class="col-md-12">
                    <div class="form-group">
                        <span class="control-label">RIUF:</span>
                        <asp:Label ID="LabelRIUF" runat="server" Text="" style="font-weight: 700"></asp:Label>
                    </div>
                </div>
            </div>        
            <div style="text-align:center">
                 <asp:Button ID="ButtonRegistrarInmueble" runat="server" CssClass="btn btn-default" OnClick="ButtonRegistrarInmueble_Click" Text="Registrar dirección " ToolTip="Ir al formulario para registrar una dirección de un inmueble para arrendamiento" />                         
                <asp:Label ID="LabelInfo" runat="server" Text="" style="font-weight: 700"></asp:Label>
            </div>
        </div>
    </div>

