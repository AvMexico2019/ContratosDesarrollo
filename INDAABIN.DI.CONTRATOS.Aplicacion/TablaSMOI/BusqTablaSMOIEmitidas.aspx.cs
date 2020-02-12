using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
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
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;


namespace INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI
{
    public partial class BusqTablaSMOIEmitidas : System.Web.UI.Page
    {

        //obj de negocio
        List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_SMOI;
        string Msj;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                if (!String.IsNullOrEmpty(RolUsr))
                {
                    //autoseleccionar la institucion del usuario
                    int IdInstitucionUsr = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);

                    //el usuario autentificado es Promovente, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.ButtonNuevaSMOI.Visible = false;
                }                    

                //poblar lista de institucioines y autoselecconar al institucion del promovente autentificado
                this.lblTableName.Text = Session.SessionID.ToString() + "BusqTablaSMOIEmitidas";
                this.PoblarDropDownListInstitucion();
            }
        }

        //poblar llenado de dropdownlist de Institucion
        private Boolean PoblarDropDownListInstitucion()
        {
            Boolean Ok = false;
            DropDownListInstitucion.DataTextField = "Descripcion";
            DropDownListInstitucion.DataValueField = "IdValue";

            try 
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListInstitucion.DataSource = AdministradorCatalogos.ObtenerCatalogoInstituciones();
                DropDownListInstitucion.DataBind();

                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                if (!String.IsNullOrEmpty(RolUsr))
                {
                    //autoseleccionar la institucion del usuario
                    int IdInstitucionUsr = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);

                    //el usuario autentificado es Promovente, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.DropDownListInstitucion.Enabled = false;


                    //autoseleccionar, si existe en la lista la institucion del usuario
                    if (this.DropDownListInstitucion.Items.Contains(this.DropDownListInstitucion.Items.FindByValue(IdInstitucionUsr.ToString())) == true)
                    {
                        this.DropDownListInstitucion.Items.FindByValue(IdInstitucionUsr.ToString()).Selected = true;
                        this.PoblarRejillaSolicitudesSMOIEmitas(); //poblar la rejilla, pues ya se conoce la institucion para ejecutar la busqueda
                        Ok = true;
                    }
                    else
                    {
                        //si el usuario  es un promovente u OIC, bloquear funcionalidad, a otro rol, permitirle hacer una seleccion de institucion
                        if (RolUsr.ToUpper().Replace(" ", "") == UtilContratosArrto.Roles.Promovente.ToString().ToUpper().Replace(" ", "")
                            || (RolUsr.ToUpper().Replace(" ", "") == UtilContratosArrto.Roles.OIC.ToString().ToUpper().Replace(" ", ""))
                            )
                        {
                            //bloquear al usuario realizar operacion, si no tiene una institucion asociada
                            this.ButtonConsultar.Enabled = false;
                            this.ButtonNuevaSMOI.Enabled = false;

                            Msj = "No se encontró una institución asociada a tu usuario, por favor solicita a Indaabin que se asigne a tu cuenta la Institución a la que estas adscrito";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                        }
                        else
                        {
                            Msj = "Selecciona una Institución y da clic en Consultar, para visualizar las solicitudes de SMOI emitidas";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de instituciones. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
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
            }
            return Ok;
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
            this.PoblarRejillaSolicitudesSMOIEmitas(true);
        }


        //al poblar el combo de instituciones, se pobla la rejilla
        private Boolean PoblarRejillaSolicitudesSMOIEmitas(bool forceUpdate = false)
        {
            Boolean Ok = false;
            ListAplicacionConcepto_SMOI = null;
            this.ButtonExportarExcel.Visible = false;

            this.GridViewSolicitudesSMOIEmitidas.DataSource = null;
            this.GridViewSolicitudesSMOIEmitidas.DataBind();

            if (forceUpdate)
                Session[this.lblTableName.Text] = null;

            //conexion con la BD para la recuperacion de datos y colocar en List<T>
            if (this.ObtenerSMOIEmitidas())
            {
                //si existe el objeto y tiene contenido
                if (ListAplicacionConcepto_SMOI != null)
                {
                    if (ListAplicacionConcepto_SMOI.Count > 0)
                    {
                        this.GridViewSolicitudesSMOIEmitidas.DataSource = ListAplicacionConcepto_SMOI;
                        this.GridViewSolicitudesSMOIEmitidas.DataBind();
                        Session[this.lblTableName.Text] = ListAplicacionConcepto_SMOI;

                        if (ListAplicacionConcepto_SMOI.Count > 0)
                        {
                            this.ButtonExportarExcel.Visible = true;
                            Msj = "Se encontraron: [" + ListAplicacionConcepto_SMOI.Count.ToString() + "] solicitud(es) de Tabla-SMOI emitida(s) a la institución en la que estás adscrito.";
                            this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                            Ok = true;
                        }
                    }
                    else
                    {
                        Msj = "No existen solicitudes SMOI registradas a la institución a la que estás adscrito, da clic en Nueva, para registrar una";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        Ok = true;
                        //Response.Redirect("TablaSMOI.aspx"); //redireccionar al registro de nueva solicitud
                    }
                }
            }
            return Ok;
        }


        //obtener informacio de la BD
        private bool ObtenerSMOIEmitidas()
        {
            if (this.ValidarEntradaDatos())
            {
                try
                {
                    int intFolioSMOI = 0;

                    //verificar si se ha proporcionado un # de folio de SMOI, para filtrar la obtencion de datos, sino se pasa 0
                    if (this.TextBoxFolioSMOI.Text.Length > 0)
                        intFolioSMOI = Convert.ToInt32(this.TextBoxFolioSMOI.Text);

                    if (Session[this.lblTableName.Text] != null)
                    {
                        ListAplicacionConcepto_SMOI = (List<AplicacionConcepto>)Session[this.lblTableName.Text];
                        return true;
                    }
                        
                    ListAplicacionConcepto_SMOI = new NGConceptoRespValor().ObtenerSolicitudesSMOIEmitidas(Convert.ToInt32(this.DropDownListInstitucion.SelectedValue), intFolioSMOI);
                    return true;
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al recuperar la lista de tablas SMOI. Contacta al área de sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
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
                    return false;
                }
            }
            return false;
        }

        private bool ValidarEntradaDatos()
        {
            if (this.TextBoxFolioSMOI.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioSMOI.Text) == false)
                {
                    Msj = "El folio de SMOI deber ser un número entero, verifica.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.TextBoxFolioSMOI.Focus();
                    return false;
                }
            }
            return true;
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void GridViewSolicitudesSMOIEmitidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                AplicacionConcepto oConcepto = (AplicacionConcepto)e.Row.DataItem;
                LinkButton link = e.Row.FindControl("lnkAcuseSMOI") as LinkButton;
                if (link != null)
                {
                    link.Attributes["onclick"] = "openCustomWindow('" + oConcepto.FolioAplicacionConcepto + "');";
                }
            }
        }

        protected void ButtonNuevaSMOI_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TablaSMOI/TablaSMOI.aspx");
        }
        protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        {
            if (this.GridViewSolicitudesSMOIEmitidas.Rows.Count > 0)
                ExportarXLS();
        }

        //exporta a Excel con todo y formato, como se ve la rejilla
        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewSolicitudesSMOIEmitidas.Columns.CloneFields();
                foreach (DataControlField col in gvdcfCollection)
                {
                    if (col.Visible)
                        gvExport.Columns.Add(col);
                }
                //gvExport.Columns[11].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, "TablaSMOI");
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al exportar a Excel. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
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
            }

            ////quitar paginacion para mandar todo a excel.
            //this.GridViewSolicitudesSMOIEmitidas.AllowSorting = false;
            //GridViewSolicitudesSMOIEmitidas.AllowPaging = false;
            ////this.GridViewResult.DataSource = Session["lstResultBusqInmueblesPort"] as List<Portafolio>;
            ////this.GridViewResult.DataBind();

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Page page = new Page();
            //HtmlForm form = new HtmlForm();
            //GridViewSolicitudesSMOIEmitidas.EnableViewState = false;
            //page.EnableEventValidation = false;
            ////Page que requieran los diseñadores RAD.
            //page.DesignerInitialize();
            //page.Controls.Add(form);
            //form.Controls.Add(GridViewSolicitudesSMOIEmitidas);
            //page.RenderControl(htw);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=data.xls");
            //Response.Charset = "UTF-8";
            //Response.ContentEncoding = Encoding.Default;
            //Response.Write(sb.ToString());
            //Response.End();
        }

        protected void GridViewSolicitudesSMOIEmitidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewSolicitudesSMOIEmitidas.DataSource = Session[this.lblTableName.Text];
            this.GridViewSolicitudesSMOIEmitidas.PageIndex = e.NewPageIndex;
            this.GridViewSolicitudesSMOIEmitidas.DataBind();
        }

    }//clase
}