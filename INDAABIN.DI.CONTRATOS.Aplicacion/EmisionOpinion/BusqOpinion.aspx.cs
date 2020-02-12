using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
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


//modulo de consulta solo para el personal de Contratos
namespace INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion
{
    public partial class BusqOpinion : System.Web.UI.Page
    {
        //obj de negocio
        List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_Opinion;
        String Msj;

        /// <summary>
        /// El WebForm: [ControladorEmisponOpinion.aspx] es el que consulta
        /// si la institucion del promovente firmado, cuenta con Solicitudes de emisión de Opinión y 
        /// de ser asi redirecciona a esta pagina.
        /// 
        /// Esta vista tiene por objetivo:
        /// - permitir visualizar un Acuse de emisión de opinión
        /// - Permitir crear una solicitud de opinion de tipo: RENOVACION, solo si existe inmueble que asocie un Folio de Contrato de Arrto.
        /// - Permitir crear una solicitud de opinion de tipo: SUSTITUCION, solo si existe inmueble que asocie un Folio de Contrato de Arrto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = String.Empty;

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
                        this.ButtonNueva.Visible = false;
                }

                this.lblTableName.Text = Session.SessionID.ToString() + "BusqTablaOpiniones";
                this.PoblarDropDownListInstitucion();
            }
        }


        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
            this.PoblarRejillaSolicitudesEmitidas(true);
        }

        private Boolean PoblarRejillaSolicitudesEmitidas(bool forceUpdate = false)
        {
            Boolean Ok = false;
            ListAplicacionConcepto_Opinion = null;
            this.ButtonExportarExcel.Visible = false;
            this.GridViewSolicitudesOpinionEmitidas.DataSource = null;
            this.GridViewSolicitudesOpinionEmitidas.DataBind();

            if (forceUpdate)
                Session[this.lblTableName.Text] = null;

            //obtener informacion de la BD
            if (this.ObtenerEmisionOpinion())
            {

                //si existe el objeto y tiene contenido
                if (ListAplicacionConcepto_Opinion != null && ListAplicacionConcepto_Opinion.Count > 0)
                {
                    this.GridViewSolicitudesOpinionEmitidas.DataSource = ListAplicacionConcepto_Opinion;
                    this.GridViewSolicitudesOpinionEmitidas.DataBind();
                    Session[this.lblTableName.Text] = ListAplicacionConcepto_Opinion;

                    //if (this.GridViewSolicitudesOpinionEmitidas.Rows.Count > 0)
                    if (ListAplicacionConcepto_Opinion.Count > 0)
                    {
                        if (this.TextBoxFolioSolicitud.Text.Length > 0)
                            //Msj = "Se encontraron: [" + GridViewSolicitudesOpinionEmitidas.Rows.Count.ToString() + "] solicitudes de emisión de opinión con el Folio: [" + this.TextBoxFolioSolicitud.Text + "].";
                            Msj = "Se encontraron: [" + ListAplicacionConcepto_Opinion.Count.ToString() + "] solicitud(es) de emisión de opinión con el Folio: [" + this.TextBoxFolioSolicitud.Text + "].";
                        else
                        {
                            if (this.DropDownListTipoOpinion.SelectedItem.Text == "--")
                                Msj = "Se encontraron: [" + ListAplicacionConcepto_Opinion.Count.ToString() + "] solicitud(es) de emisión de opinión emitidas a la institución en la que estás adscrito.";
                            else
                                Msj = "Se encontraron: [" + ListAplicacionConcepto_Opinion.Count.ToString() + "] solicitud(es) de emisión de opinión emitidas, con los parametros espcificados.";
                        }

                        this.ButtonExportarExcel.Visible = true;
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        Ok = true;
                    }

                }
                else //este else, en teoria no aplicaria porque el redireccionamiento lo hace el webform:  [ControladorEmisponOpinion.aspx]
                {
                    if (this.TextBoxFolioSolicitud.Text.Length > 0)
                        Msj = "No existen solicitudes de emisión de opinión de arrendamiento registrados con el Folio: [" + this.TextBoxFolioSolicitud.Text + "].";
                    else
                    {

                        if (this.DropDownListTipoOpinion.SelectedItem.Text == "--")
                            Msj = "No existen solicitudes de emisión de opinión de arrendamiento registradas a la institución a la que estás adscrito, da clic en Nueva, para registrar una";
                        else
                            Msj = "No existen solicitudes de emisión de opinión de arrendamiento registradas con los parámetros proporcionados";
                    }
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    Ok = true;
                }
            }

            return Ok;
        }


        //obtener Solicitudes de Opinion desde la BD
        private bool ObtenerEmisionOpinion()
        {
            Boolean Ok = false;
            if (this.ValidarEntradaDatos())
            {
                try
                {
                    int intFolioOpinion = 0;
                    if (this.TextBoxFolioSolicitud.Text.Length > 0)
                        intFolioOpinion = Convert.ToInt32(this.TextBoxFolioSolicitud.Text);

                    if (Session[this.lblTableName.Text] != null)
                        ListAplicacionConcepto_Opinion = (List<ModeloNegocios.AplicacionConcepto>)Session[this.lblTableName.Text];
                    else
                        ListAplicacionConcepto_Opinion = new NGConceptoRespValor().ObtenerSolicitudesEmisionOpinionEmitidas(Convert.ToInt32(this.DropDownListInstitucion.SelectedValue), intFolioOpinion, Convert.ToByte(this.DropDownListTipoOpinion.SelectedValue),null);
                    Ok = true;
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al recuperar la lista de emisiones de opinión. Contacta al área de sistemas.";
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
                    Ok = false;
                }
            }
            return Ok;
        }

        private bool ValidarEntradaDatos()
        {
            if (this.TextBoxFolioSolicitud.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioSolicitud.Text) == false)
                {
                    Msj = "El folio de opinión deber ser un número entero, verifica.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.TextBoxFolioSolicitud.Focus();
                    return false;
                }
            }
            return true;
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }


        //poblar llenado de dropdownlist de Institucion, y si existe una institucion del usuario en la lista, desplegar las solicitudes asociadas
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

                    int IdInstitucionUsr = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
                    //el usuario autentificado es Promovente ó OIC, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.DropDownListInstitucion.Enabled = false; //usuarios propietarios del proceo no pueden registrar nuevas solicitude de opinión.

                    //autoseleccionar, si existe en la lista la institucion del usuario
                    if (this.DropDownListInstitucion.Items.Contains(this.DropDownListInstitucion.Items.FindByValue(IdInstitucionUsr.ToString())) == true)
                    {
                        this.DropDownListInstitucion.Items.FindByValue(IdInstitucionUsr.ToString()).Selected = true;
                        this.PoblarRejillaSolicitudesEmitidas(); //poblar la rejilla, pues ya se conoce la institucion para ejecutar la busqueda
                        Ok = true;
                    }
                    else
                    {
                        //si el usuario  es un promovente bloquear funcionalidad, a otro rol, permitirle hacer una seleccion de institucion
                        if (RolUsr.ToUpper().Replace(" ", "") == UtilContratosArrto.Roles.Promovente.ToString().ToUpper().Replace(" ", ""))
                        {
                            //bloquear al usuario realizar operacion, si no tiene una institucion asociada
                            this.ButtonConsultar.Enabled = false;
                            this.ButtonNueva.Enabled = false;

                            Msj = "No se encontró una institución asociada a su usuario, por favor solicita a Indaabin que se asigne a tu cuenta la institución a la que estás adscrito";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(this.LabelInfo.Text);
                        }
                        else
                        {
                            Msj = "Selecciona una institución y da clic en Consultar, para visualizar sus solicitudes de opinión emitidas";
                            this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
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
                Ok = false;
            }
            return Ok;
        }

        //llenado de Combo Catalogo de BD Local
        private Boolean PoblarDropDownTemasCptoValorResp()
        {
            Boolean Ok = false;
            List<TemaConcepto> ListTemaCptos;
            try
            {

                ListTemaCptos = new NG_Catalogos().ObtenerTemaCptos();
                this.DropDownListTipoOpinion.DataSource = ListTemaCptos;
                this.DropDownListTipoOpinion.DataValueField = "IdTema";
                this.DropDownListTipoOpinion.DataTextField = "DescripcionTema";
                this.DropDownListTipoOpinion.DataBind();
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de temas-concepto. Contacta al área de sistemas.";
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
                Ok = false;
            }
            return Ok;
        }

        //protected void GridViewSolicitudesOpinionEmitidas_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Page")
        //        return;

        //    // get the row index stored in the CommandArgument property                     
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    // get the GridViewRow where the command is raised
        //    GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];

        //    switch (e.CommandName)
        //    {
        //        case "Acuse":
        //            //re-mapear descricpion  de tema a tipo
        //            string strTipoArrendamiento = Server.HtmlDecode(selectedRow.Cells[1].Text);
        //            switch (strTipoArrendamiento)
        //            {
        //                case "Opinión Nuevo Arrendamiento":
        //                    strTipoArrendamiento = "Nuevo";
        //                    break;
        //                case "Opinión Sustitución Arrendamiento":
        //                    strTipoArrendamiento = "Sustitución";
        //                    break;
        //                case "Opinión Continuación Arrendamiento":
        //                    strTipoArrendamiento = "Continuación";
        //                    break;
        //            }
        //            Session["intFolioConceptoResp"] = selectedRow.Cells[0].Text;
        //            selectedRow = null;
        //            Session["URLQueLllama"] = "~/EmisionOpinion/BusqOpinion.aspx";
        //           //Response.Redirect("~/EmisionOpinion/AcuseEmisionOpinion.aspx");
        //            Response.Redirect("~/EmisionOpinion/AcuseEmisionOpinion.aspx?TipoArrto=" + strTipoArrendamiento);
        //            break;
        //    }            
        //}
        protected void GridViewSolicitudesOpinionEmitidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                AplicacionConcepto oConcepto = (AplicacionConcepto)e.Row.DataItem;
                string strTipoArrendamiento = "";
                switch (oConcepto.TemaAplicacionConcepto)
                {
                    case "Opinión Nuevo Arrendamiento":
                        strTipoArrendamiento = "Nuevo";
                        break;
                    case "Opinión Sustitución Arrendamiento":
                        strTipoArrendamiento = "Sustitución";
                        break;
                    case "Opinión Continuación Arrendamiento":
                        strTipoArrendamiento = "Continuación";
                        break;
                    case "Opinión Seguridad Arrendamiento":
                        strTipoArrendamiento = "Seguridad";
                        break;
                }
                LinkButton link = e.Row.FindControl("lnkAcuseSMOI") as LinkButton;
                if (link != null)
                {
                    link.Attributes["onclick"] = "openCustomWindow('" + oConcepto.FolioAplicacionConcepto + "','" + strTipoArrendamiento + "');";
                }
            }
        }

        protected void ButtonRegistrarInmueble_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InmuebleArrto/InmuebleArrto.aspx"); //redireccionar al registro de nueva solicitud
        }

        protected void ButtonNueva_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx");
        }

        //exporta a Excel con todo y formato, como se ve la rejilla
        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewSolicitudesOpinionEmitidas.Columns.CloneFields();
                foreach (DataControlField col in gvdcfCollection)
                {
                    if (col.Visible)
                        gvExport.Columns.Add(col);
                }
                gvExport.Columns[10].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, this.lblTableName.Text);
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
            //GridViewSolicitudesOpinionEmitidas.AllowSorting = false;
            //GridViewSolicitudesOpinionEmitidas.AllowPaging = false;
            ////this.GridViewResult.DataSource = Session["lstResultBusqInmueblesPort"] as List<Portafolio>;
            ////this.GridViewResult.DataBind();

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Page page = new Page();
            //HtmlForm form = new HtmlForm();
            //GridViewSolicitudesOpinionEmitidas.EnableViewState = false;
            //page.EnableEventValidation = false;
            ////Page que requieran los diseñadores RAD.
            //page.DesignerInitialize();
            //page.Controls.Add(form);
            //form.Controls.Add(GridViewSolicitudesOpinionEmitidas);
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


        protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        {
            if (this.GridViewSolicitudesOpinionEmitidas.Rows.Count > 0)
                ExportarXLS();
        }

        protected void GridViewSolicitudesOpinionEmitidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewSolicitudesOpinionEmitidas.DataSource = Session[this.lblTableName.Text];
            this.GridViewSolicitudesOpinionEmitidas.PageIndex = e.NewPageIndex;
            this.GridViewSolicitudesOpinionEmitidas.DataBind();
        }
    }
}