using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
using System.Reflection;
//
//para excel
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//capa BO
using INDAABIN.DI.ModeloNegocio; //bus

namespace INDAABIN.DI.CONTRATOS.Aplicacion.GuiasNormativa
{
    public partial class DocumentacionEnlaces : System.Web.UI.Page
    {
        private String Msj;
        private int IdTipoDocumento = 0;
        private static readonly string downloadDirectory = HttpContext.Current.Server.MapPath("~/GuiasNormativa/Guias");

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfoGridResult.Text = string.Empty;

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                this.lblTableName.Text = Session.SessionID.ToString() + "DocumentacionEnlaces";

                //obtener el rol del usuario autenticado
                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                this.PoblarRejillaDocumentos();
                this.GridViewResult.Focus();
            }
        }

        private Boolean PoblarRejillaDocumentos()
        {
            Boolean Ok = false;
            List<ModeloNegocios.Documento> ListDocumento = null;
            string sTipoDocumento = Request.QueryString["IdTipoDocumento"] == null ? "0" : Request.QueryString["IdTipoDocumento"].ToString();
            IdTipoDocumento = System.Convert.ToInt32(sTipoDocumento);

            GridViewResult.DataSource = null;
            GridViewResult.DataBind();
            try
            {
                //ir a la BD por los datos
                ListDocumento = new NG_Catalogos().ObtenerDocumentos(IdTipoDocumento);

                if (ListDocumento != null)
                {
                    if (IdTipoDocumento == 1)
                    {
                        String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                        string filePath = Path.Combine(downloadDirectory, string.Format("002-Guia_Usuario_Contratos_arrendamientos.pdf"));
                        string filePath1 = Path.Combine(downloadDirectory, string.Format("002-Guia_Usuario_Contratos_arrendamientos.pdf"));

                        switch (RolUsr.ToUpper())
                        {
                            case "PROMOVENTE":


                                if (File.Exists(filePath))
                                {
                                    ListDocumento.Add(new Documento
                                    {
                                        IdDocumento = 0,
                                        IdTipoDocumento = 1,
                                        NombreDocumento = "Guía rapida promovente",
                                        DescripcionDocumento = "Sistema de Registro de Contratos de Arrendamiento y Otras Figuras de Ocupación",
                                        URLDocumento = "/Contratos/GuiasNormativa/Guias/002-Guia_Usuario_Contratos_arrendamientos.pdf"
                                    });
                                }
                                break;
                            case "OIC":
                                if (File.Exists(filePath1))
                                {
                                    ListDocumento.Add(new Documento
                                    {
                                        IdDocumento = 0,
                                        IdTipoDocumento = 1,
                                        NombreDocumento = "Guía rapida organo interno de control",
                                        DescripcionDocumento = "Sistema de Registro de Contratos de Arrendamiento y Otras Figuras de Ocupación",
                                        URLDocumento = "/Contratos/GuiasNormativa/Guias/002 -Guia_Usuario_Contratos _arrendamientos_organo_interno.pdf"
                                    });
                                }
                                break;
                            default:
                                if (File.Exists(filePath1))
                                {
                                    ListDocumento.Add(new Documento
                                    {
                                        IdDocumento = 0,
                                        IdTipoDocumento = 1,
                                        NombreDocumento = "Guía rapida organo interno de control",
                                        DescripcionDocumento = "Sistema de Registro de Contratos de Arrendamiento y Otras Figuras de Ocupación",
                                        URLDocumento = "/Contratos/GuiasNormativa/Guias/002 -Guia_Usuario_Contratos _arrendamientos_organo_interno.pdf"
                                    });
                                }
                                if (File.Exists(filePath))
                                {
                                    ListDocumento.Add(new Documento
                                    {
                                        IdDocumento = 0,
                                        IdTipoDocumento = 1,
                                        NombreDocumento = "Guía rapida promovente",
                                        DescripcionDocumento = "Sistema de Registro de Contratos de Arrendamiento y Otras Figuras de Ocupación",
                                        URLDocumento = "/Contratos/GuiasNormativa/Guias/002-Guia_Usuario_Contratos_arrendamientos.pdf"
                                    });
                                }
                                break;
                        }
                    }

                    if (ListDocumento.Count > 0)
                    {
                        //poblar la rejilla
                        GridViewResult.DataSource = ListDocumento;
                        GridViewResult.DataBind();
                        Session[this.lblTableName.Text] = ListDocumento;
                    }
                    else
                    {
                        Msj = "No se encontraron documentos para mostrar";
                        this.LabelInfoGridResult.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        this.LabelInfoGridResult.Text = Msj;
                        Ok = true;
                    }
                }
                else
                {
                    Msj = "No se encontraron documentos para mostrar";
                    this.LabelInfoGridResult.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.LabelInfoGridResult.Text = Msj;
                    Ok = true;
                }
                return Ok;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de documentos. Contacta al área de sistemas.";
                this.LabelInfoGridResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);

                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
                Ok = false;
            }
            return Ok;

        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }
    }
}