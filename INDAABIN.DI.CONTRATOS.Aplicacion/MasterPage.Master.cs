using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using INDAABIN.DI.ModeloNegocio; //bus
using INDAABIN.DI.CONTRATOS.Negocio;

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        string UserName;
        string Token;

        protected void Page_Load(object sender, EventArgs e)
        {

            string color = ConfigurationManager.AppSettings["Color"];

            string version = ConfigurationManager.AppSettings["Version"];

            if(!string.IsNullOrEmpty(color) && !string.IsNullOrWhiteSpace(version))
            {
                //poner version y color
                this.DivVersion.Style.Add("background-color", color);
                this.DivVersion.InnerText = version;
            }

           

            if (!IsPostBack)
            {
                try
                {
                    if (Session["Contexto"] != null )
                    {
                        SSO usuario = (SSO)Session["Contexto"];
                        String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(usuario.LRol);
                        var oic = UtilContratosArrto.Roles.OIC.ToString();
                        
                        if (RolUsr == oic)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "soloLectura", "ValidarSoloLectura('none');", true);
                        } //MZT presentar u ocultar menu item
                        //ADMINISTRADOR DE CONTRATOS
                        else if (RolUsr.Equals("ADMINISTRADOR DE CONTRATOS", StringComparison.InvariantCultureIgnoreCase) || RolUsr.Equals("ADMINISTRADOR", StringComparison.InvariantCultureIgnoreCase))
                        {
                            itemJust.Visible = true;
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "soloLectura", "MuestraJustipreciaciones();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "soloLectura", "ValidarSoloLectura('none');", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "alertUsuario", "alert('No se ha podido recuperar el objeto Contexto del usuario firmado, contacte al administrador del sistema.');", true);
                    }
                }
                catch (Exception ex)
                {
                    string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    Session["Msj"] = msj;
                    Response.Redirect("Msj.aspx", false);
                }
            }
        }
    }
}