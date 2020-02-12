<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsuarioInfo.ascx.cs" Inherits="INDAABIN.DI.CONTRATOS.Aplicacion.UsuarioInfo" %>
      
 <div class="panel panel-default">
    <div class="panel-heading">
         <div class="row">
             <div class="col-md-6" style="text-align:left">
                <strong>Usuario </strong> 
             </div>
             <div class="col-md-6" style="text-align:right">
                <asp:HyperLink ID="HyperLinkCerrarSession" runat="server" > Cerrar sesión</asp:HyperLink>    
            </div>

         </div>
    </div>
    <div class="panel-body">

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Usuario:</span>
                        <br />
                        <asp:Label ID="LabelUsr" runat="server" Text=""  class="control-label" style="font-size:14px; font-weight: 700"></asp:Label>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Nombre:</span>
                        <br />
                        <asp:Label ID="LabelNombre" runat="server" Text=""  class="control-label" style="font-size:14px; font-weight: 700"></asp:Label>
                    </div>
                </div>
                  <div class="col-md-4">
                    <div class="form-group" style="text-align:left">
                        <span class="control-label">Rol:</span>
                        <br />
                        <asp:Label ID="LabelRol" runat="server" Text=""  class="control-label" style="font-size:14px; font-weight: 700"></asp:Label>
                    </div>
                </div>

            </div>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Institución:</span>
                        <br />
                        <asp:Label ID="LabelInstitucion" runat="server" Text=""  class="control-label" style="font-size:14px; font-weight: 700"></asp:Label>
                    </div>
                </div>
            
                <div class="col-md-4">
                    <div class="form-group">
                        <span class="control-label">Sector:</span>
                        <br />
                        <asp:Label ID="LabelSector" runat="server" Text=""  class="control-label" style="font-size:14px; font-weight: 700"></asp:Label>
                    </div>
                </div>

                 <div class="col-md-4">
                    <div class="form-group" style="text-align:left">
                        <span class="control-label">Cargo:</span>
                        <br />
                        <asp:Label ID="LabelCargo" runat="server" Text=""  class="control-label" style="font-size:14px; font-weight: 700"></asp:Label>
                    </div>
                </div>

            </div>
    </div>
</div>