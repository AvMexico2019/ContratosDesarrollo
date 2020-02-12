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

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Reportes
{
    public partial class ReporteGlobal : System.Web.UI.Page
    {
        private String Msj;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = String.Empty;
            this.LabelInfoResult.Text = String.Empty;

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                this.lblTableName.Text = Session.SessionID.ToString() + "CamposReporte";
                this.lblTableNameReport.Text = Session.SessionID.ToString() + "ReporteGlobal";

                this.lblFieldList.Text = "";
                this.PoblarDropDownListInstitucion();
                this.PoblarDropDownTipoOcupacion();
                this.PoblarDropDownTipoContrato();
                this.PoblarRejillaCampos();
            }
        }

        private Boolean PoblarRejillaCampos()
        {
            Boolean Ok = false;
            try
            {
                DataTable oTable;
                oTable = this.CreaEstructuraCampos();

                List<ModeloNegocios.CampoReporte> ListCamposReporte = null;
                int Fk_IdReporte = 1;

                GridViewFields.DataSource = null;
                GridViewFields.DataBind();

                //ir a la BD por los datos
                ListCamposReporte = new NG_Catalogos().ObtenerCamposReporte(Fk_IdReporte);

                //si existe el objeto y tiene contenido
                if (ListCamposReporte != null)
                {
                    if (ListCamposReporte.Count > 0)
                    {
                        //poblar la tabla
                        int columnas = this.GridViewFields.Columns.Count / 2;
                        string colPrefixNombreCampo = "NombreCampoCol";
                        string colPrefixDescripcionCampo = "DescripcionCampoCol";
                        string colPrefixCheckCampo = "CheckCampoCol";
                        string colPrefixVisibleCampo = "VisibleCampoCol";

                        bool dataRetrieved = false;
                        int currentColumnNumber = 1;
                        DataRow dr = oTable.NewRow();
                        foreach (CampoReporte item in ListCamposReporte)
                        {
                            dr[colPrefixNombreCampo + currentColumnNumber] = item.NombreCampoReporte;
                            dr[colPrefixDescripcionCampo + currentColumnNumber] = item.DescripcionCampoReporte;
                            dr[colPrefixCheckCampo + currentColumnNumber] = false;
                            dr[colPrefixVisibleCampo + currentColumnNumber] = true;
                            currentColumnNumber += 1;
                            dataRetrieved = true;
                            if (currentColumnNumber > columnas)
                            {
                                oTable.Rows.Add(dr);
                                currentColumnNumber = 1;
                                dr = oTable.NewRow();
                            }
                        }
                        if (dataRetrieved && dr.RowState == DataRowState.Detached)
                        {
                            for (int i = currentColumnNumber; i <= columnas; i++)
                            {
                                dr[colPrefixNombreCampo + i] = "";
                                dr[colPrefixDescripcionCampo + i] = "";
                                dr[colPrefixCheckCampo + i] = false;
                                dr[colPrefixVisibleCampo + i] = false;
                            }
                            oTable.Rows.Add(dr);
                        }
                        GridViewFields.DataSource = oTable;
                        GridViewFields.DataBind();
                        Session[this.lblTableName.Text] = oTable;
                        Ok = true;
                    }
                    else
                    {
                        Msj = "No se encontraron campos para el reporte seleccionado";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoResult.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        Ok = true;
                    }
                }
                else
                {
                    Msj = "No se encontraron campos para el reporte seleccionado";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoResult.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    Ok = true;
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al mostrar la lista de campos para el reporte. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = this.LabelInfo.Text;
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

        private DataTable CreaEstructuraCampos()
        {
            DataTable oTable = new DataTable();
            int columnas = this.GridViewFields.Columns.Count / 2;
            string colPrefixNombreCampo = "NombreCampoCol";
            string colPrefixDescripcionCampo = "DescripcionCampoCol";
            string colPrefixCheckCampo = "CheckCampoCol";
            string colPrefixVisibleCampo = "VisibleCampoCol";

            for (int i = 1; i <= columnas; i++)
            {
                DataColumn dcNombre = new DataColumn(colPrefixNombreCampo + i, System.Type.GetType("System.String"));
                DataColumn dcDescripcion = new DataColumn(colPrefixDescripcionCampo + i, System.Type.GetType("System.String"));
                DataColumn dcCheck = new DataColumn(colPrefixCheckCampo + i, System.Type.GetType("System.Boolean"));
                DataColumn dcVisible = new DataColumn(colPrefixVisibleCampo + i, System.Type.GetType("System.Boolean"));
                oTable.Columns.Add(dcNombre);
                oTable.Columns.Add(dcDescripcion);
                oTable.Columns.Add(dcCheck);
                oTable.Columns.Add(dcVisible);
            }
            return oTable;
        }

        //poblar llenado de dropdownlist de Institucion
        private Boolean PoblarDropDownListInstitucion()
        {
            Boolean Ok = false;
            DropDownListInstitucion.DataTextField = "Descripcion";
            DropDownListInstitucion.DataValueField = "IdValue";
            int? IdInstitucion;

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                this.DropDownListInstitucion.DataSource = AdministradorCatalogos.ObtenerCatalogoInstituciones();
                this.DropDownListInstitucion.DataBind();
                this.DropDownListInstitucion.Items.Add("--");
                //this.DropDownListInstitucion.Items.FindByText("--").Selected = true;

                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                if (!String.IsNullOrEmpty(RolUsr))
                {
                    //autoseleccionar la institucion del usuario
                    IdInstitucion = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
                    if (IdInstitucion.HasValue == false)
                    {
                        Session["Msj"] = "Tu usuario no está asociado a ninguna Institución, por favor solicita a Sistemas que asocie a tu cuenta la Institución.";
                        Response.Redirect("~/Msj.aspx", false);
                    }

                    //el usuario autentificado es Promovente, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                    {
                        this.DropDownListInstitucion.Enabled = false;
                    }

                    //autoseleccionar, si existe en la lista la institucion del usuario
                    if (this.DropDownListInstitucion.Items.Contains(this.DropDownListInstitucion.Items.FindByValue(IdInstitucion.ToString())) == true)
                    {
                        this.DropDownListInstitucion.Items.FindByValue(IdInstitucion.ToString()).Selected = true;

                        Ok = true;
                    }
                    else
                    {
                        //si el usuario  es un promovente bloquear funcionalidad, a otro rol, permitirle hacer una seleccion de institucion
                        if (RolUsr.ToUpper().Replace(" ", "") == UtilContratosArrto.Roles.Promovente.ToString().ToUpper().Replace(" ", ""))
                        {
                            //bloquear al usuario realizar operacion, si no tiene una institucion asociada
                            this.ButtonConsultar.Enabled = false;
                            Msj = "No se encontró una institución asociada a su usuario, por favor solicita a Indaabin que se asigne a su cuenta la Institución a la que estás adscrito";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoResult.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                        }
                        else
                        {
                            Msj = "Selecciona una institución y da clic en Consultar, para visualizar los inmubles de arrendamiento y sus movimientos asociados a la Institución.";
                            this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                            this.LabelInfoResult.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de instituciones. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = this.LabelInfo.Text;
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
        //poblar llenado de dropdownlist de tipo de ocupacion
        private Boolean PoblarDropDownTipoOcupacion()
        {
            Boolean Ok = false;
            List<TipoOcupacion> ListTipoOcupacion;
            try
            {
                ListTipoOcupacion = new NG_Catalogos().ObtenerTipoOcupacion();
                this.DropDownListTipoOcupacion.DataSource = ListTipoOcupacion;
                this.DropDownListTipoOcupacion.DataValueField = "IdTipoOcupacion";
                this.DropDownListTipoOcupacion.DataTextField = "DescripcionTipoOcupacion";
                this.DropDownListTipoOcupacion.DataBind();
                //agregar un elemento para reprsentar que no se ha seleccionado un valor
                this.DropDownListTipoOcupacion.Items.Add("--");
                this.DropDownListTipoOcupacion.Items.FindByText("--").Selected = true;

                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de tipo de ocupación. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = this.LabelInfo.Text;
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

        private Boolean PoblarDropDownTipoContrato()
        {
            Boolean Ok = false;
            List<TipoContrato> ListTipoContrato;
            try
            {

                ListTipoContrato = new NG_Catalogos().ObtenerTipoContrato();
                this.DropDownListTipoContrato.DataSource = ListTipoContrato;
                this.DropDownListTipoContrato.DataValueField = "IdTipoContrato";
                this.DropDownListTipoContrato.DataTextField = "DescripcionTipoContrato";
                this.DropDownListTipoContrato.DataBind();
                //agregar un elemento para reprsentar que no se ha seleccionado un valor
                this.DropDownListTipoContrato.Items.Add("--");
                this.DropDownListTipoContrato.Items.FindByText("--").Selected = true;

                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de tipo de contrato. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = this.LabelInfo.Text;
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

        private bool ValidaParametros(ParametroReporte oParametros)
        {
            //MZT 14/jun/2017
            if (oParametros.ListaCampos == null || oParametros.ListaCampos.Count == 0)
            {
                Msj = "Debe seleccionar al menos un campo para el reporte.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                return false;
            }

            if (oParametros.RangoFechaRegistroInicial != "0" && oParametros.RangoFechaRegistroFinal == "0")
            {
                Msj = "Debe seleccionar un rango valido para la fecha de registro del Contrato";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                return false;
            }

            if (oParametros.RangoFechaRegistroInicial != "0" && oParametros.RangoFechaRegistroFinal != "0")
            {
                if (Convert.ToDateTime(oParametros.RangoFechaRegistroInicial) > Convert.ToDateTime(oParametros.RangoFechaRegistroFinal))
                {
                    Msj = "La fecha final del rango de registro de contrato debe ser mayor a la fecha inicial.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    return false;
                }
            }

            if (oParametros.RangoFechaInicioOcupacionInicial != "0" && oParametros.RangoFechaInicioOcupacionFinal == "0")
            {
                Msj = "Debe seleccionar un rango final valido para la fecha de inicio de ocupación.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                return false;
            }

            if (oParametros.RangoFechaTerminoOcupacionInicial != "0" && oParametros.RangoFechaTerminoOcupacionFinal == "0")
            {
                Msj = "Debe seleccionar un rango valido para la fecha de término de ocupación.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                return false;
            }

            if (oParametros.RangoFechaInicioOcupacionInicial != "0" && oParametros.RangoFechaInicioOcupacionFinal != "0")
            {
                if (Convert.ToDateTime(oParametros.RangoFechaInicioOcupacionInicial) > Convert.ToDateTime(oParametros.RangoFechaInicioOcupacionFinal))
                {
                    Msj = "La fecha final del rango de inicio de ocupación debe ser mayor a la fecha inicial.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    return false;
                }
            }

            if (oParametros.RangoFechaTerminoOcupacionInicial != "0" && oParametros.RangoFechaTerminoOcupacionFinal != "0")
            {
                if (Convert.ToDateTime(oParametros.RangoFechaTerminoOcupacionInicial) > Convert.ToDateTime(oParametros.RangoFechaTerminoOcupacionFinal))
                {
                    Msj = "La fecha final del rango de termino de ocupación debe ser mayor a la fecha inicial.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    return false;
                }
            }

            this.LabelInfo.Text = "";
            this.LabelInfoResult.Text = "";
            return true;
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Threading.Thread.Sleep(5000);
                string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;
                ParametroReporte oParametrosReporte = new ParametroReporte();
                oParametrosReporte.IdInstitucion = this.DropDownListInstitucion.SelectedValue == "--" ? (int?)null : System.Convert.ToInt32(this.DropDownListInstitucion.SelectedValue.ToString());
                oParametrosReporte.ListaCampos = this.RecuperaSeleccionCampos();

                oParametrosReporte.IdTipoContrato = this.DropDownListTipoContrato.SelectedValue.Trim() == "--" ? (int?)null : System.Convert.ToInt32(this.DropDownListTipoContrato.SelectedValue.ToString());
                oParametrosReporte.IdTipoOcupacion = this.DropDownListTipoOcupacion.SelectedValue.Trim() == "--" ? (int?)null : System.Convert.ToInt32(this.DropDownListTipoOcupacion.SelectedValue.ToString());
                oParametrosReporte.RangoFechaRegistroInicial = this.TextBoxFechaRegistroInicio.Text.Trim() == "" ? "0" : this.TextBoxFechaRegistroInicio.Text.Trim();
                oParametrosReporte.RangoFechaRegistroFinal = this.TextBoxFechaRegistroFinal.Text.Trim() == "" ? "0" : this.TextBoxFechaRegistroFinal.Text.Trim();
                oParametrosReporte.RangoFechaInicioOcupacionInicial = this.TextBoxFechaIOcupacionInicio.Text.Trim() == "" ? "0" : this.TextBoxFechaIOcupacionInicio.Text.Trim();
                oParametrosReporte.RangoFechaInicioOcupacionFinal = this.TextBoxFechaIOcupacionFinal.Text.Trim() == "" ? "0" : this.TextBoxFechaIOcupacionFinal.Text.Trim();
                oParametrosReporte.RangoFechaTerminoOcupacionInicial = this.TextBoxFechaFOcupacionInicio.Text.Trim() == "" ? "0" : this.TextBoxFechaFOcupacionInicio.Text.Trim();
                oParametrosReporte.RangoFechaTerminoOcupacionFinal = this.TextBoxFechaFOcupacionFinal.Text.Trim() == "" ? "0" : this.TextBoxFechaFOcupacionFinal.Text.Trim();

                if (!ValidaParametros(oParametrosReporte))
                    return;

                DataTable oReportTable = new NG_Reportes().SelectReporteInmuebles(strConnectionString, oParametrosReporte);

                if (oReportTable == null)
                {
                    this.pnlReporte.Visible = false;
                    this.ButtonExportar.Visible = false;
                    Msj = "La consulta no ha devuelto resultados, verifica los filtros de busqueda.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    return;
                }
                else
                {
                    this.pnlReporte.Visible = true;
                    this.ButtonExportar.Visible = true;
                    this.GridViewReporte.DataSource = oReportTable;
                    this.GridViewReporte.DataBind();
                    Session[this.lblTableNameReport.Text] = oReportTable;
                    Msj = "La consulta ha devuelto " + oReportTable.Rows.Count.ToString() + " registro(s)";
                    this.LabelInfo.Text = "<div class='alert alert-info'>" + Msj + "</div>";
                    this.LabelInfoResult.Text = "<div class='alert alert-info'>" + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    return;
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el reporte. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoResult.Text = this.LabelInfo.Text;
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

        }

        protected void ButtonExportar_Click(object sender, EventArgs e)
        {
            if (Session[this.lblTableNameReport.Text] != null)
            {
                GridView gvExport = new GridView();
                gvExport.DataSource = (DataTable)(Session[this.lblTableNameReport.Text]);
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, "reporteGlobal");
            }
        }

        protected void GridViewReporte_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewReporte.DataSource = Session[this.lblTableNameReport.Text];
            this.GridViewReporte.PageIndex = e.NewPageIndex;
            this.GridViewReporte.DataBind();
        }

        private List<string> RecuperaSeleccionCampos()
        {
            List<string> oListaCampos = new List<string>();
            int columnas = this.GridViewFields.Columns.Count / 2;
            string colPrefixNombreCampo = "lblNombreCampoCol";
            string colPrefixCheckCampo = "chkCampoCol";
            foreach (GridViewRow item in GridViewFields.Rows)
            {
                for (int i = 1; i <= columnas; i++)
                {
                    Label oLabel = (Label)item.FindControl(colPrefixNombreCampo + i);
                    CheckBox oCheck = (CheckBox)item.FindControl(colPrefixCheckCampo + i);
                    if (oCheck.Checked && oLabel.Text.Trim() != "")
                    {
                        oListaCampos.Add(oLabel.Text);
                    }
                }
            }
            return oListaCampos;
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void chkTodosCampos_CheckedChanged(object sender, EventArgs e)
        {
            if (Session[this.lblTableName.Text] != null)
            {
                DataTable oTable = (DataTable)Session[this.lblTableName.Text];
                int columnas = this.GridViewFields.Columns.Count / 2;
                string colPrefixCheckCampo = "CheckCampoCol";
                foreach (DataRow item in oTable.Rows)
                {
                    for (int i = 1; i <= columnas; i++)
                    {
                        item[colPrefixCheckCampo + i] = this.chkTodosCampos.Checked;
                    }
                }
                GridViewFields.DataSource = oTable;
                GridViewFields.DataBind();
                Session[this.lblTableName.Text] = oTable;
            }
        }
    }
}