using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
using System.Reflection;
using System.Data;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio; //para uso del BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public partial class UsuarioInfo : System.Web.UI.UserControl
    {
        public string NombreUsuario 
        {
            get
            {
                SSO oSSO = ((SSO)Session["Contexto"]);
                return oSSO.Nombre;
            }
        }

        public string APaternoUsuario
        {
            get
            {
                SSO oSSO = ((SSO)Session["Contexto"]);
                return oSSO.ApellidoP;
            }
        }

        public string AMaternoUsuario
        {
            get
            {
                SSO oSSO = ((SSO)Session["Contexto"]);
                return oSSO.ApellidoM;
            }
        }

        public string CargoUsuario
        {
            get
            {
                SSO oSSO = ((SSO)Session["Contexto"]);
                return oSSO.Cargo;
            }
        }

        public string CorreoUsuario
        {
            get
            {
                SSO oSSO = ((SSO)Session["Contexto"]);
                return oSSO.Email;
            }
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                    this.ColocarURLCierreSession();
                
                    //validar informacion de la cuenta del usuario, si existe excepcion redireccionar a Msj.aspx
                    this.ValidaInstitucionUsr();
                   
                    //SI LLEGA  AQUI ENTONCES PASO LA VALIDACION PORQUE NO HUBO REDIRECCIONAMIENTO

                    //USERNAME
                    this.LabelUsr.Text = ((SSO)Session["Contexto"]).UserName.ToUpper();

                    //NOMBRE
                    this.LabelNombre.Text = ((SSO)Session["Contexto"]).Nombre.ToUpper() + " " + ((SSO)Session["Contexto"]).ApellidoP.ToUpper() + " " + ((SSO)Session["Contexto"]).ApellidoM.ToUpper();


                    //CARGO
                    if (!String.IsNullOrEmpty(((SSO)Session["Contexto"]).Cargo))
                        
                        this.LabelCargo.Text = ((SSO)Session["Contexto"]).Cargo.ToUpper();
                    else
                        this.LabelCargo.Text = "--";

                    //INSTITUCION
                    if (!String.IsNullOrEmpty(((SSO)Session["Contexto"]).NombreInstitucion))
                        this.LabelInstitucion.Text = ((SSO)Session["Contexto"]).NombreInstitucion.ToUpper();
                    else
                        this.LabelInstitucion.Text = "--";

                    //SECTOR
                    if (!String.IsNullOrEmpty(((SSO)Session["Contexto"]).NombreSector))
                        this.LabelSector.Text = ((SSO)Session["Contexto"]).NombreSector.ToUpper();
                    else
                        this.LabelSector.Text = "--";

                    //ROL
                    String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                    this.LabelRol.Text = RolUsr.ToUpper();
               
            }
        }//load


        //asigna al control HyperLink de cerrar session;  la URL del SSO que esta en el WebConfig
        private void ColocarURLCierreSession()
        {
          
            try
            {
                string valor = ConfigurationManager.AppSettings.Get("URL_SSO");
                HyperLinkCerrarSession.NavigateUrl = valor;
               
            }

            catch (Exception ex)
            {
                string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Session["Msj"] = msj;
                Response.Redirect("Msj.aspx", false);
            }
           
        }

        //validaciones de datos de usuario; despues de cargar a: Session["Contexto"]
        //es importante que la Institución del SSO, exista en el BUS, porque este aplicativo implementa DB_CATNueva
        private void ValidaInstitucionUsr()
        {

            //Institucion
            if (((SSO)Session["Contexto"]).IdInstitucion.HasValue == false || Util.IsEnteroNatural(((SSO)Session["Contexto"]).IdInstitucion.Value) == false)
            {
                Session["Msj"] = "Es necesario que tu usuario tenga definida una institución para poder consultar información de este aplicativo, por favor consulta a Soporte Indaaabin para su ajuste";
                Response.Redirect("~/Msj.aspx", false);
            }
            else
            {

                try
                {
                    string NombreInstitucion = AdministradorCatalogos.ObtenerNombreInstitucion(((SSO)Session["Contexto"]).IdInstitucion.Value);

                    if (NombreInstitucion == null)
                    {
                        Session["Msj"] = "La Institución que se asocia tu cuenta de acceso no existe, por favor consulta a Soporte Indaaabin para su ajuste";
                        Response.Redirect("~/Msj.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    Session["Msj"] = ex.Message;
                    Response.Redirect("~/Msj.aspx", false);
                }
            }
        }




    }//clase
}