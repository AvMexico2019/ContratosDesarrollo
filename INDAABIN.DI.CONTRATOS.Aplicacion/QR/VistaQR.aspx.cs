using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
//
using System.Configuration;
using System.Reflection;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//capa BO
using INDAABIN.DI.ModeloNegocio; //bus
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;



namespace INDAABIN.DI.CONTRATOS.Aplicacion.QR
{
    public partial class VistaQR : System.Web.UI.Page
    {

        #region Atributos

        ParametrosURL vwParam
        {
            get
            {
                ParametrosURL obj = ViewState["vwParam"] == null ? new ParametrosURL() : (ParametrosURL)ViewState["vwParam"];
                return obj;
            }
            set
            {
                ViewState["vwParam"] = value;
            }
        }

        private string vwUrlOri
        {
            get
            {
                string dato = ViewState["vwUrlOri"] == null ? string.Empty : ViewState["vwUrlOri"].ToString();
                return dato;
            }
            set
            {
                ViewState["vwUrlOri"] = value;
            }
        }

        private string Msj;

        List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_SAEF;

        SSO usuario = null;
        string url = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                {
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));
                }
                //String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                if (this.ObtenerParam() == false)
                {
                    return;
                }
                else
                {

                    //usuario = new NG().ObtenerUsuario(vwParam.UserName);


                    Session["Contexto"] = usuario;

                    ////llamar al metodo para llenar la tabla
                    this.PoblarRejillaSolicitudesEmitidas();
                }

                ParametrosURL oParam = vwParam;
            }

        }

        private bool ObtenerParam()
        {
            bool resp = false;
            ProcesadorParametrosURL pParam = new ProcesadorParametrosURL();
            Dictionary<string, object> param = new Dictionary<string, object>();
            ParametrosURL obj = new ParametrosURL();
            Cifrado.Cifrado procesadorCifrado = new Cifrado.Cifrado();

            if (Request.QueryString.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < Request.QueryString.Count; i++)
            {
                string nombre = Request.QueryString.Keys[i];
                string val = Request.QueryString[nombre];

                param.Add(nombre, val);
            }

            try
            {
                obj = pParam.ObtenerParametros(obj, param);

                if (obj == null)
                {
                    obj = new ParametrosURL();
                }

                vwParam = obj;

                if (param.ContainsKey(INDAABIN.DI.CONTRATOS.ModeloNegocios.Constantes.IndenParametroEncrip) == true)
                {
                    vwUrlOri = procesadorCifrado.Decifrar(param[INDAABIN.DI.CONTRATOS.ModeloNegocios.Constantes.IndenParametroEncrip].ToString());
                }
                else
                {
                    vwUrlOri = Request.Url.Query;
                }

                resp = true;
            }
            catch (Exception ex)
            {
                //mensaje de error para el usuario
                Msj = "No se ha podido desencriptar la Url para obtener los parametros, favor de contactar al área de sistemas.";
                this.LabelInfoBusqSAEF.Text = "<div class='alert alert-danger'><strong> ¡ERROR! </strong> " + Msj + "</div>";

                //ingreso de la incidencia a bitacora de errores
            }

            param = null;
            pParam = null;
            procesadorCifrado = null;

            return resp;

        }

        private Boolean PoblarRejillaSolicitudesEmitidas(bool forceUpdate = false)
        {
            Boolean Ok = false;
            ListAplicacionConcepto_SAEF = null;
            this.GridViewSolicitudesOpinionEmitidasSAEF.DataSource = null;
            this.GridViewSolicitudesOpinionEmitidasSAEF.DataBind();

            if (forceUpdate)
                Session[this.lblTableNameSAEF.Text] = null;

            //obtener informacion de la BD
            if (this.ObtenerEmisionOpinion())
            {

                //si existe el objeto y tiene contenido
                if (ListAplicacionConcepto_SAEF != null && ListAplicacionConcepto_SAEF.Count > 0)
                {
                    this.GridViewSolicitudesOpinionEmitidasSAEF.DataSource = ListAplicacionConcepto_SAEF;
                    this.GridViewSolicitudesOpinionEmitidasSAEF.DataBind();
                    Session[this.lblTableNameSAEF.Text] = ListAplicacionConcepto_SAEF;


                    if (ListAplicacionConcepto_SAEF.Count > 0)
                    {
                        
                        
                        Msj = "Se encontraron: [" + ListAplicacionConcepto_SAEF.Count.ToString() + "] solicitud(es) de accesibilidad emitidas a la institución en la que estás adscrito.";
                        this.LabelInfoBusqSAEF.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";

                        Ok = true;
                    }

                }
                else
                {

                    Msj = "No se ha encontrado ningun registro";
                    this.LabelInfoBusqSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                    Ok = true;
                }
            }

            return Ok;
        }

        //obtener Solicitudes de Opinion desde la BD
        private bool ObtenerEmisionOpinion()
        {
            Boolean Ok = false;
           
                try
                {
                    int intFolioOpinion = Convert.ToInt32(vwParam.Folio);
                   

                    int? FolioSAEF = null;
                    

                    if (Session[this.lblTableNameSAEF.Text] != null)
                    {
                        ListAplicacionConcepto_SAEF = (List<ModeloNegocios.AplicacionConcepto>)Session[this.lblTableNameSAEF.Text];
                    }
                    else
                    {
                        ListAplicacionConcepto_SAEF = new NG_SAEF().ObtenerSolicitudesEmisionOpinionEmitidasSAEF(null, intFolioOpinion, null, FolioSAEF);
                    }

                    Ok = true;
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al recuperar la lista de accesibilidad. Contacta al área de sistemas.";
                    this.LabelInfoBusqSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";


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

        protected void GridViewSolicitudesOpinionEmitidasSAEF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ModeloNegocios.AplicacionConcepto ObjSAEF = (ModeloNegocios.AplicacionConcepto)e.Row.DataItem;


                LinkButton LinkAcuseSAEF = e.Row.FindControl("lnkAcuseSAEF") as LinkButton;

                int? FolioSAEF = Convert.ToInt32(ObjSAEF.FolioSAEF);

                if (FolioSAEF != null)
                {
                    LinkAcuseSAEF.Visible = true;
                }
                else
                {
                    LinkAcuseSAEF.Visible = false;
                }

            }
        }

        protected void GridViewSolicitudesOpinionEmitidasSAEF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewSolicitudesOpinionEmitidasSAEF.DataSource = Session[this.lblTableNameSAEF.Text];
            this.GridViewSolicitudesOpinionEmitidasSAEF.PageIndex = e.NewPageIndex;
            this.GridViewSolicitudesOpinionEmitidasSAEF.DataBind();
        }


        protected void lnkAcuseSAEF_Click(object sender, EventArgs e)
        {
            INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias.ExportHTML exportar = new INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias.ExportHTML();

            //obtenemos el idaplicacion o el folio de aplicacion
            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;

            int FolioSAEF = Convert.ToInt32(GridViewSolicitudesOpinionEmitidasSAEF.DataKeys[rowIndex].Values["FolioSAEF"]);

            //buscamos el idaplicacionconcepto
            int IdAplicacionConcept = new NG_SAEF().ObtenerAplpicacionConcepto(FolioSAEF);


            //mostramos eoll acuse
            exportar.CuerpoCompletoPlantillaSAEF(null, IdAplicacionConcept);
        }
    }
}