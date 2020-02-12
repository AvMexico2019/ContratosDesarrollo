using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Reflection;

using INDAABIN.DI.ModeloNegocio;

using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.AdministradorRIUF
{
    public partial class AdminRIUF : System.Web.UI.Page
    {

        private string Msj = string.Empty;
        string URL = string.Empty;
        string RolUsr;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = string.Empty;
            
            if(!IsPostBack)
            {

                if (Session["Contexto"] == null)
                {
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));
                }

                SSO usuario = (SSO)Session["Contexto"];
                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(usuario.LRol);

                if (!RolUsr.Equals("ADMINISTRADOR DE CONTRATOS", StringComparison.InvariantCultureIgnoreCase))
                //if (!RolUsr.Equals("ADMINISTRADOR", StringComparison.InvariantCultureIgnoreCase))
                {
                    //si no eres administrador de contratos te va a regresar a la pagina principal 
                    Response.Redirect("~/Principal.aspx", true);
                }
                else
                {
                    try
                    {
                        this.PoblarDropDownListInstitucion();
                        this.PoblarDropDownListPais();
                        this.PoblarDropDownListEstado();
                        this.PoblarDropDownTipoRegistro();
                        this.PoblarDropDownEstatusRUSP();
                    }
                    catch (Exception ex)
                    {
                        Msj = "Ha ocurrido un error iniciar. Contacta al área de sistemas.";
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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


            }

        }

        //poblar el catalogo del tipo de registro
        private Boolean PoblarDropDownTipoRegistro()
        {
            bool ok = false;

            try
            {
                //agregamos los elementos

                DropDownTipoRegistro.Items.Add(new ListItem("TODAS","0"));
                DropDownTipoRegistro.Items.Add(new ListItem("Arrendamientos nacionales", "1"));
                DropDownTipoRegistro.Items.Add(new ListItem("Arrendamientos en el extranjero", "2"));
                DropDownTipoRegistro.Items.Add(new ListItem("Otras Figuras de ocupación nacionales", "3"));
                DropDownTipoRegistro.Items.Add(new ListItem("Otras Figuras de ocupación en el extranjero", "4"));
                

                ok = true;
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de tipo de registros. Contacta al área de sistemas";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

                //poner el llamado a una bitacora donde se guardara el registro de todas estas incidencias
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "RUSP",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }

            return ok;
        }

        //poblar el catalogo del estatus en RUSP
        private Boolean PoblarDropDownEstatusRUSP()
        {
            bool ok = false;

            try
            {
                //agregamos los elementos
                DropDownRUSP.Items.Add(new ListItem("TODAS", "2"));
                DropDownRUSP.Items.Add(new ListItem("Habilitado","1"));
                DropDownRUSP.Items.Add(new ListItem("Deshabilitado","0"));


                ok = true;
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de estatus en el RUSP. Contacta al área de sistemas";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

                //poner el llamado a una bitacora donde se guardara el registro de todas estas incidencias
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "RUSP",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }

            return ok;
        }

        //poblar la lista de instituciones
        private Boolean PoblarDropDownListInstitucion()
        {
            bool ok = false;

            this.DropDownListInstitucion.DataTextField = "Descripcion";
            this.DropDownListInstitucion.DataValueField = "IdValue";

            try
            {
                DropDownListInstitucion.DataSource = AdministradorCatalogos.ObtenerCatalogoInstituciones();
                DropDownListInstitucion.DataBind();

                this.DropDownListInstitucion.Items.FindByText("TODAS").Selected = true;

                ok = true;
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de instituciones. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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

            return ok;
        }

        //obtenemos el catalogo del pais
        private Boolean PoblarDropDownListPais()
        {
            bool ok = false;

            this.DropDownListPais.DataTextField = "Descripcion";
            this.DropDownListPais.DataValueField = "IdValue";

            try
            {
                this.DropDownListPais.DataSource = AdministradorCatalogos.ObtenerCatalogoPais();
                this.DropDownListPais.DataBind();

                DropDownListPais.Items.Add("TODAS");
                this.DropDownListPais.Items.FindByText("TODAS").Selected = true;


                DropDownListMunicipio.Items.Add("TODAS");
                this.DropDownListMunicipio.Items.FindByText("TODAS").Selected = true;

                ok = true;
            }
            catch(Exception ex)
            {
                Msj = "Ocurrió una excepción al cargar la lista de Paises. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'> " + Msj + "</div>";

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

            return ok;
        }

        //obtenemos el catalogo del estado
        private Boolean PoblarDropDownListEstado()
        {
            bool ok = false;

            DropDownListEstado.DataTextField = "Descripcion";
            DropDownListEstado.DataValueField = "IdValue";

            try
            {
                DropDownListEstado.DataSource = AdministradorCatalogos.ObtenerCatalogoEstados();
                DropDownListEstado.DataBind();

                DropDownListEstado.Items.Add("TODAS");
                this.DropDownListEstado.Items.FindByText("TODAS").Selected = true;

                ok = true;
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de estados. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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


            return ok;
        }

        //obtener catalogo dependiente de municipios con entidad federativa
        private void PoblarDropDownListMposXEntFed()
        {
            this.DropDownListMunicipio.Items.Clear();
            this.DropDownListMunicipio.DataTextField = "Descripcion";
            this.DropDownListMunicipio.DataValueField = "IdValue";
            this.DropDownListMunicipio.DataSource = AdministradorCatalogos.ObtenerMunicipios(Convert.ToInt32(this.DropDownListEstado.SelectedValue));
            this.DropDownListMunicipio.DataBind();

            this.DropDownListMunicipio.Items.Add("TODAS");
            this.DropDownListMunicipio.Items.FindByText("TODAS").Selected = true;
        }

        protected void DropDownListEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListEstado.Text != "TODAS")
            {
                this.PoblarDropDownListMposXEntFed();
            }
            else
            {
                this.DropDownListMunicipio.DataSource = null;
                this.DropDownListMunicipio.DataBind();
                this.DropDownListMunicipio.Items.Clear();
            }
        }

        protected void txtCP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtCP.Text))
                {
                    int i;

                    if(int.TryParse(this.txtCP.Text.Trim(),out i))
                    {
                        if(i > 0)
                        {
                            FiltroXCP oLocalidad = AdministradorCatalogos.ObtenerDetalleLocalidadPorCodigoPostal(this.txtCP.Text.Trim().PadLeft(2, '0'));
                            this.DropDownListPais.SelectedValue = oLocalidad.IdPais.Value.ToString();
                            this.DropDownListEstado.SelectedValue = oLocalidad.IdEstado.Value.ToString();
                            this.PoblarDropDownListMposXEntFed();
                            this.DropDownListMunicipio.SelectedValue = oLocalidad.IdMunicipio.Value.ToString();
                        }
                        else
                        {
                            this.txtCP.Text = "";
                            Msj = "El código postal es inválido, verifica.";
                            this.LabelInfo.Text = "<div class='alert alert-warning'> " + Msj + "</div>";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                this.txtCP.Text = "";
                Msj = "No se ha podido recuperar la información del código postal. <br />Valida que el código postal exista o de lo contrario contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'> " + Msj + "</div>";

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

        protected void btnConsultar_ServerClick(object sender, EventArgs e)
        {
            //validamos que si introduce ina fecha que la otra este igual
            if(ValidarEntradaFechas())
            {
                this.PoblarRejillaContratosVsRIUF();

               
            }
            else
            {
                this.calendarInicio.Value = "";
                this.calendarFin.Value = "";
            }
            
        }

        protected void btnLimpiar_ServerClick(object sender, EventArgs e)
        {
            this.LimpiarCampos();
        }

        //metodo para limpiar los campos
        public void LimpiarCampos()
        {
            this.DropDownListInstitucion.SelectedValue = "0";
            this.txtRIUF.Text = "";
            this.DropDownListPais.SelectedValue = "TODAS";
            this.DropDownListEstado.SelectedValue = "TODAS";
            this.DropDownListMunicipio.SelectedValue = "TODAS";
            this.txtCP.Text = "";
            this.DropDownTipoRegistro.SelectedValue = "0";
            this.DropDownRUSP.SelectedValue = "2";
            this.calendarFin.Value = "";
            this.calendarInicio.Value = "";
        }

        //metodo para poblar el grid
        private Boolean PoblarRejillaContratosVsRIUF()
        {
            bool ok = false;

            this.ButtonExportarExcel.Visible = false;
            this.btnHabilitar.Visible = false;
            this.btnDeshabilitar.Visible = false;

            List<ModeloNegocios.EstatusRUSPvsRIUF> ListInmuebleArrtoRegistrados = null;
            Session[this.lblTableName.Text] = null;

            this.GridViewRIUF.DataSource = null;
            this.GridViewRIUF.DataBind();

            //obtenemos los valores de los campos de busqueda
            DateTime? FechaInicio = null;
            if (this.calendarInicio.Value.Trim().Length > 0)
                FechaInicio = Convert.ToDateTime(this.calendarInicio.Value.Trim());

            DateTime? FechaFin = null;
            if (this.calendarFin.Value.Trim().Length > 0)
                FechaFin = Convert.ToDateTime(this.calendarFin.Value.Trim());

            int? IdInstitucion = null;
            if (this.DropDownListInstitucion.SelectedItem.Text != "TODAS")
                IdInstitucion = Convert.ToInt32(this.DropDownListInstitucion.SelectedValue);

            string RIUF = null;
            if (this.txtRIUF.Text.Length > 0)
                RIUF = this.txtRIUF.Text;

            int? IdPais = null;
            if (this.DropDownListPais.SelectedItem.Text != "TODAS")
                IdPais = Convert.ToInt16(this.DropDownListPais.SelectedValue);

            int? IdEstado = null;
            if (this.DropDownListEstado.SelectedItem.Text != "TODAS")
                IdEstado = Convert.ToInt16(this.DropDownListEstado.SelectedValue);

            int? IdMunicipio = null;
            if (this.DropDownListMunicipio.SelectedItem.Text != "TODAS")
                IdMunicipio = Convert.ToInt16(this.DropDownListMunicipio.SelectedValue);

            string CP = null;
            if (this.txtCP.Text.Length > 0)
                CP = this.txtCP.Text;

            int? TipoRegistro = null;
            if (this.DropDownTipoRegistro.SelectedItem.Text != "TODAS")
                TipoRegistro = Convert.ToInt16(this.DropDownTipoRegistro.SelectedValue);

            int? EstatusRUSP = null;
            if (this.DropDownRUSP.SelectedItem.Text != "TODAS")
                EstatusRUSP = Convert.ToInt16(this.DropDownRUSP.SelectedValue);

            int? FolioContrato = null;
            if (this.txtFolioContrato.Text.Length > 0)
                FolioContrato = Convert.ToInt32(this.txtFolioContrato.Text);


            //obtenemos la informacion de DB
            try
            {
                if(Session[this.lblTableName.Text] != null)
                {
                    ListInmuebleArrtoRegistrados = (List<ModeloNegocios.EstatusRUSPvsRIUF>)Session[this.lblTableName.Text];
                }
                else
                {
                    ListInmuebleArrtoRegistrados = new NG_InmuebleArrto().ObtenerEstatusRUSPvsRIUF(FechaInicio, FechaFin, IdInstitucion, RIUF, IdPais, IdEstado, IdMunicipio, CP, TipoRegistro, EstatusRUSP, FolioContrato);

                }
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de inmuebles. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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



            //si eoll objeto contiene informacion 
            if(ListInmuebleArrtoRegistrados != null)
            {
                if(ListInmuebleArrtoRegistrados.Count > 0)
                {
                  //poblar la rejilla
                    GridViewRIUF.DataSource = ListInmuebleArrtoRegistrados;
                    GridViewRIUF.DataBind();
                    Session[this.lblTableName.Text] = ListInmuebleArrtoRegistrados;

                    this.ButtonExportarExcel.Visible = true;
                    this.btnHabilitar.Visible = true;
                    this.btnDeshabilitar.Visible = true;

                    this.calendarInicio.Value = "";
                    this.calendarFin.Value = "";

                    if(GridViewRIUF.Rows.Count > 0)
                    {
                        Msj = "Se encontraron: [" + ListInmuebleArrtoRegistrados.Count + "] registro(s) de contrato.";
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                       
                        ok = true;
                    }
                }
                else
                {
                    Msj = "No se encontraron registros.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                    this.calendarInicio.Value = "";
                    this.calendarFin.Value = "";

                    ok = true;
                }
            }
            else
            {
                Msj = "No se encontraron registros con los parámetros especificados.";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
               
                ok = true;
            }


            return ok;
        }

        protected void GridViewRIUF_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GridViewRIUF_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                //no hace visible el IdPlaza en el grid
                e.Row.Cells[14].Visible = false;
            }
        }

        protected void GridViewRIUF_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ButtonExportarExcel_ServerClick(object sender, EventArgs e)
        {
            if(this.GridViewRIUF.Rows.Count > 0)
            {
                ExportarXLS();
            }
        }

        //validar la entrada de las fechas
        private Boolean ValidarEntradaFechas()
        {
            bool ok = true;

            

            if(calendarInicio.Value.Length > 0)
            {
                if(calendarFin.Value.Length == 0)
                {
                    Msj = "Selecciona ambas fechas para continuar con la consulta.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.calendarFin.Focus();

                    ok = false;
                }
            }

            if(calendarFin.Value.Length > 0)
            {
                if(calendarInicio.Value.Length == 0)
                {
                    Msj = "Selecciona ambas fechas para continuar con la consulta.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.calendarInicio.Focus();

                    ok = false;
                }
            }

            return ok;
        }

        //metodo para exportar a excel
        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewRIUF.Columns.CloneFields();

                foreach (DataControlField col in gvdcfCollection)
                {
                    if (col.Visible)
                    {
                        gvExport.Columns.Add(col);
                    }
                }

                gvExport.Columns[14].Visible = false;
                gvExport.Columns[0].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, "ReporteRIUFArrendamiento");
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al expoprtar a Excel.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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

        //metodo para que con el header se seleccione todo
        protected void CheckTodo_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)GridViewRIUF.HeaderRow.FindControl("CheckTodo");
            foreach (GridViewRow row in GridViewRIUF.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("CheckRIUF");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

        protected void btnHabilitar_ServerClick(object sender, EventArgs e)
        {
            bool ok = false;

            try
            {
                foreach(GridViewRow row in this.GridViewRIUF.Rows)
                {
                    //validamos que por lo menos uno este chequeado
                    bool isChecked = ((CheckBox)row.FindControl("CheckRIUF")).Checked;

                    if(isChecked)
                    {
                        int FolioContrato = Convert.ToInt32(row.Cells[7].Text);
                        int TipoCaso = 2;

                        //mandamos a DB la eleccion 
                        ok = new NG_InmuebleArrto().HabilitarDeshabilitarRIUF(FolioContrato, TipoCaso);

                       
                    }
                    else
                    {
                        //mensaje cuando no se selecciona ningun check 
                        Msj = "No se ha seleccionado ningun registro, favor de seleccionar por lo menos uno.";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    }
                }

                if(ok == true)
                {
                    //URL = Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?"), Request.Url.ToString().Length - Request.Url.ToString().IndexOf("?"));

                    //mandamos a abrir otra ventana que contrende los inmuebles seleccionados 
                    Response.Redirect(string.Concat(string.Format("~/AdministradorRIUF/AdminRIUF.aspx{0}",URL)));
                }

            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error actualizar los registros.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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

        protected void btnDeshabilitar_ServerClick(object sender, EventArgs e)
        {
            bool ok = false;

            try
            {
                foreach (GridViewRow row in this.GridViewRIUF.Rows)
                {
                    //validamos que por lo menos uno este chequeado
                    bool isChecked = ((CheckBox)row.FindControl("CheckRIUF")).Checked;

                    if (isChecked)
                    {
                        int FolioContrato = Convert.ToInt32(row.Cells[7].Text);
                        int TipoCaso = 1;

                        //mandamos a DB la eleccion 
                        ok = new NG_InmuebleArrto().HabilitarDeshabilitarRIUF(FolioContrato, TipoCaso);


                    }
                    else
                    {
                        //mensaje cuando no se selecciona ningun check 
                        Msj = "No se ha seleccionado ningun registro, favor de seleccionar por lo menos uno.";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    }
                }

                if (ok == true)
                {
                    //URL = Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?"), Request.Url.ToString().Length - Request.Url.ToString().IndexOf("?"));

                    //mandamos a abrir otra ventana que contrende los inmuebles seleccionados 
                    Response.Redirect(string.Concat(string.Format("~/AdministradorRIUF/AdminRIUF.aspx{0}",URL)));
                }
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error actualizar los registros.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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


    }
}