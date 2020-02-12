using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;

using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Contrato
{
    public partial class ConvenioModificatorioRegistro : Page
    {
        private ModeloNegocios.InmuebleArrto objInmuebleArrto
        {
            get
            {
                ModeloNegocios.InmuebleArrto valor = ViewState["vsobjInmuebleArrto"] == null ?
                    new ModeloNegocios.InmuebleArrto() : (ViewState["vsobjInmuebleArrto"] as ModeloNegocios.InmuebleArrto);
                return valor;
            }

            set { ViewState["vsobjInmuebleArrto"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int IdInmuebleArrendamiento = 0;
            SSO SSO = new SSO();
            bool rolGeneracion = true;            

            try
            {
                string IdInmueble = Request.QueryString["IdInmueble"];
                string folio = Request.QueryString["IdContrato"];
                int IdUsuario = 0;
                int.TryParse(IdInmueble, out IdInmuebleArrendamiento);                

                if (!IsPostBack)
                {
                    if (Session["Contexto"] == null)
                        Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                    IdUsuario = ((SSO)Session["Contexto"]).IdUsuario == null ? 0 : ((SSO)Session["Contexto"]).IdUsuario.Value;

                    if (IdUsuario == 0)
                        Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));                    

                    if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                        rolGeneracion = false;

                    if (!rolGeneracion)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "PostBack", "MarcarErrorPagina('Permiso');", true);
                        return;
                    }

                    objInmuebleArrto = new NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrendamiento);

                    hdnFolio.Value = folio;
                    hdnIdPais.Value = objInmuebleArrto.IdPais.ToString();
                    hdnIdEstado.Value = objInmuebleArrto.IdEstado.ToString();
                    hdnIdMunicipio.Value = objInmuebleArrto.IdMunicipio.ToString();
                    hdnIdInmueble.Value = IdInmueble;
                    hdnInstitucionPromovente.Value = QuitarAcentosTexto(((SSO)Session["Contexto"]).NombreInstitucion);
                    hdnIdUsuario.Value = IdUsuario.ToString();
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "PostBack", "CargaPaginaRegistroConvenio();", true);
            }

            catch (Exception ex)
            {
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };

                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }
        }

        private string QuitarAcentosTexto(string Texto)
        {            
            if (!string.IsNullOrEmpty(Texto))
            {
                string textoNormalizado = Texto.Trim().Normalize(System.Text.NormalizationForm.FormD);
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
                string textoSinAcentos = reg.Replace(textoNormalizado, "");
                return textoSinAcentos;
            }

            return string.Empty;
        }
    }    
}