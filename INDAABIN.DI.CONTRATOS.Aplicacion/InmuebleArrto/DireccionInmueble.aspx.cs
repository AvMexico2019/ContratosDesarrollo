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
using System.Data.SqlClient;
//
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Data.Sql;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto
{
    public partial class DireccionInmueble : System.Web.UI.Page
    {
        private String Msj;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.LabelInfo.Text = String.Empty;
            this.LabelInfoGridResult.Text = string.Empty;

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                //this.LimpiarSessiones();
                this.lblTableName.Text = Session.SessionID.ToString() + "DireccionInmueble";

                //obtener el rol del usuario autenticado
                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                //Obtener el IdInmueble de la emisión seleccionada
                this.lblIdInmuebleArrendamiento.Value = Request.QueryString["IdInmuebleArrendamiento"].ToString();
                this.PoblarRejillaDireccionesPorIdInmuebleArrenadmiento();


                ////el usuario autentificado es Promovente, entonces hacer conteo de Solicitudes de emisión de opinión:
                //if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                //    || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                //{
                //    //verificar si existen inmuebles registrados a la institucion del promovente autenticado
                //    if (this.CountInmueblesRegistradosAlaInstitucionUsr() == 0)
                //    {
                //        //no existen registros de inmuebles para poder asociar una solicitud de opinion
                //        Session["Msj"] = "Registra primero una dirección de un inmueble para arreandamiento para poder registrar una solicitud";
                //        Response.Redirect("~/InmuebleArrto/InmuebleArrto.aspx", false); //redireccionar al registro de nueva captrua de un inmueble de arrendamiento (direccion)
                //    }
                //}

                //si pasa a esta linea es que no es promovente, o si lo es si tiene inmuebles registrados, a los cuales registrar mvtos de emisión de opinión.
                //this.PoblarDropDownListPais();
                //this.PoblarDropDownListEntidadFederativa();                                  

                //String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                ////determinar el tipo de usuario autenticado
                //if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                //if (RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                //{
                //    this.ButtonRegistrarExitente.Visible = false; //no puede registrar inmuebles
                //    this.ButtonRegistrarNuevo.Visible = false; //no puede registrar inmuebles
                //}

                this.GridViewResult.Focus();
            }
        }

        // Lista de Inmuebles
        private void PoblarRejillaDireccionesPorIdInmuebleArrenadmiento()
        {
            try
            {
                if (this.lblIdInmuebleArrendamiento.Value != "0")
                {
                    int IdInmuebleArrto = System.Convert.ToInt32(this.lblIdInmuebleArrendamiento.Value);
                    ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrto);
                    this.TextBoxDireccionActual.Text = objInmuebleArrto.DireccionCompleta;
                    this.TextBoxDireccionActual.Visible = true;
                    this.PoblarDropDownListPais();
                    this.DropDownListPais.SelectedValue = objInmuebleArrto.IdPais.ToString();

                    if (objInmuebleArrto.IdPais == 165)
                    {
                        this.PoblarDropDownListEntidadFederativa();
                        this.DropDownListEdo.SelectedValue = objInmuebleArrto.IdEstado.ToString();
                        this.PoblarDropDownListMposXEntFed();
                        this.DropDownListMpo.SelectedValue = objInmuebleArrto.IdMunicipio.ToString();
                    }
                    else
                    {
                        this.PoblarDropDownListEntidadFederativa();
                        this.DropDownListEdo.Enabled = false;
                        this.DropDownListMpo.Enabled = false;
                    }
                    this.PoblarRejillaDirecciones(objInmuebleArrto);
                    this.ButtonCancelarConsulta.Text = "Generar";
                }
                else
                {
                    this.TextBoxDireccionActual.Text = "";
                    this.TextBoxDireccion.Text = "";
                    this.TextBoxRIUF.Text = "";
                    this.TextBoxDireccionActual.Visible = false;

                    this.DropDownListPais.Enabled = true;
                    this.DropDownListEdo.Enabled = true;
                    this.DropDownListMpo.Enabled = true;
                    this.DropDownListTipoConsulta.Enabled = false;

                    this.PoblarDropDownListPais();
                    this.PoblarDropDownListEntidadFederativa();
                    this.DropDownListTipoConsulta.SelectedValue = "0";
                    this.ButtonCancelarConsulta.Text = "Cancelar";
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el valor del concepto. Contacta al área de sistemas.";
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
            }
        }

        // Lista de Inmuebles
        private Boolean PoblarRejillaDirecciones(ModeloNegocios.InmuebleArrto oInmuebleArrendamiento = null)
        {
            Boolean Ok = false;
            List<ModeloNegocios.Inmueble> ListInmuebles = null;
            int IdPais_Busq = 0;
            int IdEstado_Busq = 0;
            int IdMpo_Busq = 0;
            string RIUF_Busq = "";
            string Direccion_Busq = "";

            try
            {
                this.GridViewResult.DataSource = null;
                this.GridViewResult.DataBind();

                if (this.DropDownListPais.SelectedItem.Text != "--")
                    IdPais_Busq = Convert.ToInt16(this.DropDownListPais.SelectedValue);

                if (IdPais_Busq > 0 && IdPais_Busq == 165)
                {
                    if (this.DropDownListEdo.SelectedItem.Text != "--")
                        IdEstado_Busq = Convert.ToInt16(this.DropDownListEdo.SelectedValue);

                    if (this.DropDownListEdo.SelectedItem.Text != "--")
                        if (this.DropDownListMpo.SelectedItem.Text != "--")
                            IdMpo_Busq = Convert.ToInt16(this.DropDownListMpo.SelectedValue);
                }

                if (this.TextBoxRIUF.Text.Trim() != "")
                    RIUF_Busq = this.TextBoxRIUF.Text.Trim();

                if (this.TextBoxDireccion.Text.Trim() != "")
                    Direccion_Busq = this.TextBoxDireccion.Text.Trim();

                //Se recupera la lista de inmuebles por los filtros seleccionados
                ListInmuebles = new NG_Inmueble().ObtenerInmuebles(IdPais_Busq, IdEstado_Busq, IdMpo_Busq, RIUF_Busq, Direccion_Busq, oInmuebleArrendamiento);

                //si existe el objeto y tiene contenido
                if (ListInmuebles != null)
                {
                    this.pnlGrivView.Visible = true;
                    if (ListInmuebles.Count > 0)
                    {
                        //poblar la rejilla
                        this.GridViewResult.DataSource = ListInmuebles;
                        this.GridViewResult.DataBind();
                        Session[this.lblTableName.Text] = ListInmuebles;

                        //if (GridViewResult.Rows.Count > 0)
                        if (ListInmuebles.Count > 0)
                        {
                            //this.ButtonExportarExcel.Visible = true;
                            Msj = "Se encontraron: [" + ListInmuebles.Count.ToString() + "] dirección(es) registrada(s) al Estado/Municpio seleccionado";
                            this.LabelInfoGridResult.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                            this.MostrarMensajeJavaScript(Msj);
                            Ok = true;
                        }
                    }
                    else
                    {
                        Msj = "No se encontraron direcciones de arrendamiento";
                        //this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoGridResult.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        this.LabelInfoGridResult.Text = Msj;
                        this.MostrarMensajeJavaScript(Msj);
                        Ok = true;
                    }
                }
                return Ok;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error y no fue posible exponer las direcciones de los inmuebles. Contacta al área de sistemas.";
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

                return false;
            }
        }

        // Lista de Paises
        private Boolean PoblarDropDownListPais()
        {
            Boolean Ok = false;
            this.DropDownListPais.DataTextField = "Descripcion";
            this.DropDownListPais.DataValueField = "IdValue";

            try
            {
                //obtener informacion de paises del BUS, si la lista ya existe solo obtenerla, sino cargarla del BUS                        
                this.DropDownListPais.DataSource = AdministradorCatalogos.ObtenerCatalogoPais();
                this.DropDownListPais.DataBind();
                //agregar un elemento para representar a todos

                this.DropDownListPais.Items.FindByText("México").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de Paises. Contacta al área de sistemas.";
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
            }
            return Ok;
        }

        // Lista de Entidad Federativa
        private Boolean PoblarDropDownListEntidadFederativa()
        {
            Boolean Ok = false;
            DropDownListEdo.DataTextField = "Descripcion";
            DropDownListEdo.DataValueField = "IdValue";

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListEdo.DataSource = AdministradorCatalogos.ObtenerCatalogoEstados();
                DropDownListEdo.DataBind();
                //agregar un elemento para representar a todos
                DropDownListEdo.Items.Add("--");
                this.DropDownListEdo.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de Entidades Federativa. Contacta al área de sistemas.";
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
            }
            return Ok;
        }

        // Lista de Municipios por Entidad federativa
        private void PoblarDropDownListMposXEntFed()
        {
            try
            {
                this.DropDownListMpo.DataTextField = "Descripcion";
                this.DropDownListMpo.DataValueField = "IdValue";
                this.DropDownListMpo.DataSource = AdministradorCatalogos.ObtenerMunicipios(Convert.ToInt32(this.DropDownListEdo.SelectedValue));
                this.DropDownListMpo.DataBind();

                //agregar un elemento para representar a todos
                this.DropDownListMpo.Items.Add("--");
                this.DropDownListMpo.Items.FindByText("--").Selected = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de Municipios. Contacta al área de sistemas.";
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
            }
        }

        protected void DropDownListPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListPais.SelectedItem.Text == "México")
                this.PoblarDropDownListEntidadFederativa();
            else
            {
                //limpiar mpos porque se ha seleccionado que no se buscara por estado
                this.DropDownListEdo.DataSource = null;
                this.DropDownListEdo.DataBind();
                this.DropDownListEdo.Items.Clear();

                this.DropDownListMpo.DataSource = null;
                this.DropDownListMpo.DataBind();
                this.DropDownListMpo.Items.Clear();
            }
        }

        protected void DropDownListEdo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListEdo.SelectedItem.Text != "--")
                this.PoblarDropDownListMposXEntFed();
            else
            {
                //limpiar mpos porque se ha seleccionado que no se buscara por estado
                this.DropDownListMpo.DataSource = null;
                this.DropDownListMpo.DataBind();
                this.DropDownListMpo.Items.Clear();
            }
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
            if (this.ValidarEntradaDatos())
            {
                if (this.DropDownListTipoConsulta.SelectedValue == "0")
                    this.PoblarRejillaDirecciones();
                else
                    this.PoblarRejillaDireccionesPorIdInmuebleArrenadmiento();
            }
        }

        private bool ValidarEntradaDatos()
        {
            if (this.DropDownListPais.SelectedItem.Text == "--"
                && this.DropDownListEdo.SelectedItem.Text == "--"
                && this.DropDownListMpo.SelectedItem.Text == "--"
                && this.TextBoxRIUF.Text.Trim() == ""
                && this.TextBoxDireccion.Text.Trim() == "")
            {
                Msj = "Debe seleccionar al menos un criterio de Búsqueda";
                this.LabelInfoGridResult.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                return false;
            }
            return true;
        }

        protected void GridViewResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewResult.SelectedIndex = -1;
            this.GridViewResult.DataSource = Session[this.lblTableName.Text];
            this.GridViewResult.PageIndex = e.NewPageIndex;
            this.GridViewResult.DataBind();
            this.DefineSeleccionActual();
        }

        protected void GridViewResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DefineSeleccionActual();
        }

        private void DefineSeleccionActual()
        {
            if (this.GridViewResult.SelectedIndex >= 0)
            {
                this.hRiuf.Value = Server.HtmlEncode(this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].Cells[2].Text);
                this.hPais.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdPais")).Text;
                this.hTipoInmueble.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdTipoInmueble")).Text;
                this.hEdo.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdEstado")).Text;
                this.hMun.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdMunicipio")).Text;
                this.hColonia.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdLocalidad")).Text;
                this.hOtraColonia.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblOtraColonia")).Text;
                this.hNombreVialidad.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblNombreVialidad")).Text;
                this.hDenominacionDireccion.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblDenominacionInmueble")).Text;
                this.hCP.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblCodigoPostal")).Text;
                this.hTipoVialidad.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdTipoVialidad")).Text;
                this.hNumExterior.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblNumExterior")).Text;
                this.hNumInterior.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblNumInterior")).Text;
                this.hGeoRefLatitud.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblGeoRefLatitud")).Text;
                this.hGeoRefLongitud.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblGeoRefLongitud")).Text;
                this.hIdInmueble.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblIdInmueble")).Text;
                this.hCodigoPostalExtranjero.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblCodigoPostalExtranjero")).Text;
                this.hEstadoExtranjero.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblEstadoExtranjero")).Text;
                this.hCiudadExtranjero.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblCiudadExtranjero")).Text;
                this.hMunicipioExtranjero.Value = ((Label)this.GridViewResult.Rows[this.GridViewResult.SelectedIndex].FindControl("lblMunicipioExtranjero")).Text;

                Msj = "Has seleccionado la direccion correspondiente al RIUF: " + this.hRiuf.Value;
                //this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                this.LabelInfoGridResult.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                //this.ButtonRegistrarExitente.Enabled = true;

                if (this.lblIdInmuebleArrendamiento.Value != "0")
                {
                    this.ButtonSeleccionarDomicilio.Visible = true;
                    this.ButtonSeleccionarDomicilioConsulta.Visible = false;
                }
                else
                {
                    this.ButtonSeleccionarDomicilio.Visible = false;
                    this.ButtonSeleccionarDomicilioConsulta.Visible = true;
                }
            }
            else
            {
                Msj = "No se ha seleccionado direccion";
                this.LabelInfoGridResult.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                this.ButtonSeleccionarDomicilio.Visible = false;
            }
        }

        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewResult.Columns.CloneFields();
                foreach (DataControlField col in gvdcfCollection)
                {
                    if (col.Visible)
                        gvExport.Columns.Add(col);
                }
                //gvExport.Columns[11].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, "DireccionInmueble");
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al exportar a Excel. Contacta al área de sistemas.";
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
            }
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }


        //protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        //{
        //    if (this.GridViewResult.Rows.Count > 0)
        //        ExportarXLS();
        //}

        //protected void ButtonRegistrarNuevo_Click(object sender, EventArgs e)
        //{
        //    this.pnlBusqueda.Visible = false;
        //    this.pnlBusquedaBotones.Visible = false; 
        //    this.pnlGrivView.Visible = false;
        //    this.pnlDireccion.Visible = true;
        //    this.DireccionRiuf.CargaInicial();
        //    this.DireccionRiuf.CargaInicialCampos(false);
        //    this.lblGenerarRIUF.Text = "1";
        //}

        //protected void ButtonRegistrarExitente_Click(object sender, EventArgs e)
        //{
        //    this.pnlBusqueda.Visible = false;
        //    this.pnlBusquedaBotones.Visible = false;
        //    this.pnlGrivView.Visible = false;
        //    this.pnlDireccion.Visible = true;
        //    this.DireccionRiuf.CargaInicial();
        //    this.DireccionRiuf.CargaInicialCampos(true);
        //    this.DireccionRiuf.RIUF = this.hRiuf.Value;
        //    this.DireccionRiuf.ClavePais = this.hPais.Value;
        //    this.DireccionRiuf.ClaveTipoInmueble = this.hTipoInmueble.Value;
        //    this.DireccionRiuf.ClaveEstado = this.hEdo.Value;
        //    this.DireccionRiuf.ClaveMunicipio = this.hMun.Value;
        //    this.DireccionRiuf.ClaveColonia = this.hColonia.Value;
        //    this.DireccionRiuf.OtraColonia = this.hOtraColonia.Value;
        //    this.DireccionRiuf.NombreVialidad = this.hNombreVialidad.Value;
        //    this.DireccionRiuf.DenominacionDeLaDireccion = this.hDenominacionDireccion.Value;
        //    this.DireccionRiuf.CodigoPostal = this.hCP.Value;
        //    this.DireccionRiuf.ClaveTipoVialidad = this.hTipoVialidad.Value;
        //    this.DireccionRiuf.NumeroExterior = this.hNumExterior.Value;
        //    this.DireccionRiuf.NumeroInterior = this.hNumInterior.Value;
        //    this.DireccionRiuf.GeoReferenciaLatitud = this.hGeoRefLatitud.Value;
        //    this.DireccionRiuf.GeoReferenciaLongitud = this.hGeoRefLongitud.Value;
        //    this.lblGenerarRIUF.Text = "0";
        //}

        //protected void ButtonCancelar_Click(object sender, EventArgs e)
        //{
        //    this.lblGenerarRIUF.Text = "";
        //    this.pnlBusqueda.Visible = !this.pnlBusqueda.Visible;
        //    this.pnlBusquedaBotones.Visible = !this.pnlBusquedaBotones.Visible;  
        //    this.pnlGrivView.Visible = !this.pnlGrivView.Visible;
        //    this.pnlDireccion.Visible = !this.pnlDireccion.Visible;
        //    this.DireccionRiuf.CargaInicial();
        //}

        //protected void ButtonGuardar_Click(object sender, EventArgs e)
        //{
        //    if (this.DireccionRiuf.ValidarEntradaDatos())
        //    {
        //        if (this.GuardarInmueble())
        //        {
        //            //MostrarMensajeJavaScript("Se registro el inmueble de arrendamiento, ahora puede utilizarlo para asociar con una solicitud de emisión de opinión o el registro de un contrato de arrandamiento");
        //            Session["Msj"] = "Se registró el inmueble de arrendamiento, ahora puedes utilizarlo para asociar con una solicitud de emisión de opinión ó el registro de un contrato de arrandamiento";

        //            // VALIDAR DESDE DONDE MAS SE LLAMA
        //            ////redirigir a la vista que lo invoco, despues de realizar el registro OK.
        //            //if (Session["URLQueLllama"] != null)
        //            //    Response.Redirect(Session["URLQueLllama"].ToString());
        //            //else
        //            //    //llevar al webform de emisión de opinión si no existe quien lo llamo, ya que en teoria es lo 1ro que se debe registrar para un nuevo inmueble
        //            //    Response.Redirect("~/InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx");
        //        }
        //    }
        //}

        //private bool GuardarInmueble()
        //{
        //    Boolean Ok = false;
        //    string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;

        //    //Integrar objeto de negocio: [InmuebleArrto], para pasar a la capa de Negocio y DAL y realizar la operacion DML-SQL
        //    //recolectar datos de los controles y colocar en objeto de negocio

        //    //obtener controles del UserControl
        //    Control DropDownListPais = this.DireccionRiuf.FindControl("DropDownListPais");

        //    Control DropDownListTipoInmuble = this.DireccionRiuf.FindControl("DropDownListTipoInmueble");

        //    //de direccion nacional
        //    Control DropDownListEntFed = this.DireccionRiuf.FindControl("DropDownListEdo");
        //    Control DropDownListMpo = this.DireccionRiuf.FindControl("DropDownListMpo");
        //    Control DropDownListColonia = this.DireccionRiuf.FindControl("DropDownListColonia");
        //    Control TextBoxOtraColonia = this.DireccionRiuf.FindControl("TextBoxOtraColonia");
        //    Control TextBoxCP = this.DireccionRiuf.FindControl("TextBoxCP");

        //    //Comunes: aplicables a cualquier direccion: nacional o en el extranjero
        //    Control DropDownListTipoVialidad = this.DireccionRiuf.FindControl("DropDownListTipoVialidad");
        //    Control TextBoxNombreVialidad = this.DireccionRiuf.FindControl("TextBoxNombreVialidad");
        //    Control TextBoxNumExt = this.DireccionRiuf.FindControl("TextBoxNumExt");
        //    Control TextBoxNumInt = this.DireccionRiuf.FindControl("TextBoxNumInt");
        //    Control TextBoxLatitud = this.DireccionRiuf.FindControl("TextBoxLatitud");
        //    Control TextBoxLongitud = this.DireccionRiuf.FindControl("TextBoxLongitud");
        //    Control TextBoxNombreDireccion = this.DireccionRiuf.FindControl("TextBoxNombreDireccion");

        //    //de direccion en el extranjero              
        //    Control TextBoxEdoExtranjero = this.DireccionRiuf.FindControl("TextBoxEdoExtranjero");
        //    Control TextBoxMpoExtranjero = this.DireccionRiuf.FindControl("TextBoxMpoExtranjero");
        //    Control TextBoxCiudadExtranjero = this.DireccionRiuf.FindControl("TextBoxCiudadExtranjero");
        //    Control TextBoxCPExtranjero = this.DireccionRiuf.FindControl("TextBoxCPExtranjero");


        //    //Numero RIUF seleccionado
        //    Control TextBoxRIUF = this.DireccionRiuf.FindControl("TextBoxRIUF");           


        //    //creacion de objeto de direccion de inmueble arto.
        //    ModeloNegocios.Inmueble objDireccionInmueble = new ModeloNegocios.Inmueble();

        //    //poblado de datos al objeto
        //    objDireccionInmueble.CargoUsuarioRegistro = ((SSO)Session["Contexto"]).Cargo;
        //    objDireccionInmueble.IdUsuarioRegistro = Convert.ToInt32(((SSO)Session["Contexto"]).IdUsuario);
        //    objDireccionInmueble.IdPais = Convert.ToInt32(((DropDownList)DropDownListPais).SelectedValue);
        //    objDireccionInmueble.PaisDescripcion = ((DropDownList)DropDownListPais).SelectedItem.Text;
        //    objDireccionInmueble.IdTipoInmueble = Convert.ToInt32(((DropDownList)DropDownListTipoInmuble).SelectedValue);

        //    if (((DropDownList)DropDownListPais).SelectedItem.Text == "México")
        //    {
        //        //aplicables a direccion Nacional
        //        objDireccionInmueble.IdEstado = Convert.ToInt32(((DropDownList)DropDownListEntFed).SelectedValue);
        //        objDireccionInmueble.EstadoDescripcion = ((DropDownList)DropDownListEntFed).SelectedItem.Text;
        //        objDireccionInmueble.IdMunicipio = Convert.ToInt32(((DropDownList)DropDownListMpo).SelectedValue);
        //        objDireccionInmueble.MunicipioDescripcion = ((DropDownList)DropDownListMpo).SelectedItem.Text;

        //        if (((DropDownList)DropDownListColonia).SelectedItem.Text != "-Otra Colonia-")
        //            objDireccionInmueble.IdLocalidad = Convert.ToInt32(((DropDownList)DropDownListColonia).SelectedValue);
        //        else
        //            objDireccionInmueble.OtraColonia = ((TextBox)TextBoxOtraColonia).Text;

        //        objDireccionInmueble.LocalidadDescripcion = ((DropDownList)DropDownListColonia).SelectedItem.Text;
        //        objDireccionInmueble.CodigoPostal = ((TextBox)TextBoxCP).Text;
        //    }

        //    //comunes a inmueble con direccion: Nacional y Extranjero
        //    objDireccionInmueble.IdTipoVialidad = Convert.ToInt32(((DropDownList)DropDownListTipoVialidad).SelectedValue);
        //    objDireccionInmueble.TipoVialidadDescripcion = ((DropDownList)DropDownListTipoVialidad).SelectedItem.Text;
        //    objDireccionInmueble.NombreVialidad = ((TextBox)TextBoxNombreVialidad).Text.Trim();
        //    objDireccionInmueble.NumExterior = ((TextBox)TextBoxNumExt).Text.Trim();

        //    //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
        //    if (!String.IsNullOrWhiteSpace(((TextBox)TextBoxNumInt).Text))
        //        objDireccionInmueble.NumInterior = ((TextBox)TextBoxNumInt).Text.Trim();

        //    //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
        //    if (!String.IsNullOrWhiteSpace(((TextBox)TextBoxLatitud).Text))
        //        //    objDireccionInmueble.GeoRefLatitud = null;
        //        //else
        //        objDireccionInmueble.GeoRefLatitud = Convert.ToDecimal(((TextBox)TextBoxLatitud).Text);

        //    //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
        //    if (!String.IsNullOrWhiteSpace(((TextBox)TextBoxLongitud).Text))
        //        //    objDireccionInmueble.GeoRefLongitud = null;
        //        //else
        //        objDireccionInmueble.GeoRefLongitud = Convert.ToDecimal(((TextBox)TextBoxLongitud).Text);

        //    //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
        //    objDireccionInmueble.NombreInmueble = ((TextBox)TextBoxNombreDireccion).Text.Trim();

        //    //se valida la generacion del RIUF
        //    objDireccionInmueble.RIUF = new RIUF();
        //    if (this.lblGenerarRIUF.Text == "1")
        //    {
        //        objDireccionInmueble.GeneraRIUF = 1;
        //    }                
        //    else
        //    {
        //        objDireccionInmueble.GeneraRIUF = 0;
        //        objDireccionInmueble.RIUF.RIUF1 = (((TextBox)TextBoxRIUF).Text.Trim()) == "" ? "0" : ((TextBox)TextBoxRIUF).Text.Trim();
        //    }           

        //    //aplicables a direccion en el Extranjero
        //    if (((DropDownList)DropDownListPais).SelectedItem.Text != "México")
        //    {
        //        objDireccionInmueble.CodigoPostalExtranjero = ((TextBox)TextBoxCPExtranjero).Text.Trim();
        //        objDireccionInmueble.EstadoExtranjero = ((TextBox)TextBoxEdoExtranjero).Text.Trim();
        //        objDireccionInmueble.CiudadExtranjero = ((TextBox)TextBoxCiudadExtranjero).Text.Trim();
        //        objDireccionInmueble.MunicipioExtranjero = ((TextBox)TextBoxMpoExtranjero).Text.Trim();
        //        //objDireccionInmueble.GeneraRIUF = 0;
        //        //objDireccionInmueble.RIUF.RIUF1 = "0";
        //    }

        //    int iAffect = 0;
        //    try
        //    {
        //        iAffect = new Negocio.NG_Inmueble().InsertInmueble(strConnectionString,objDireccionInmueble);
        //        if (iAffect > 0)
        //        {
        //            Ok = true;                   
        //            this.DireccionRiuf.IdInmueble = iAffect.ToString();
        //            this.hIdInmueble.Value = iAffect.ToString();
        //            this.DireccionRiuf.RIUF = objDireccionInmueble.RIUF.RIUF1;
        //            this.hRiuf.Value = objDireccionInmueble.RIUF.RIUF1;
        //            this.hPais.Value = this.DireccionRiuf.ClavePais;
        //            this.hTipoInmueble.Value = this.DireccionRiuf.ClaveTipoInmueble;
        //            this.hEdo.Value = this.DireccionRiuf.ClaveEstado;
        //            this.hMun.Value = this.DireccionRiuf.ClaveMunicipio;
        //            this.hColonia.Value = this.DireccionRiuf.ClaveColonia;
        //            this.hOtraColonia.Value = this.DireccionRiuf.OtraColonia;
        //            this.hNombreVialidad.Value = this.DireccionRiuf.NombreVialidad;
        //            this.hDenominacionDireccion.Value = this.DireccionRiuf.DenominacionDeLaDireccion;
        //            this.hCP.Value = this.DireccionRiuf.CodigoPostal;
        //            this.hTipoVialidad.Value = this.DireccionRiuf.ClaveTipoVialidad;
        //            this.hNumExterior.Value = this.DireccionRiuf.NumeroExterior;
        //            this.hNumInterior.Value = this.DireccionRiuf.NumeroInterior;
        //            this.hGeoRefLatitud.Value = this.DireccionRiuf.GeoReferenciaLatitud;
        //            this.hGeoRefLongitud.Value = this.DireccionRiuf.GeoReferenciaLongitud;
        //            this.lblGenerarRIUF.Text = "";
        //            this.TextBoxRIUF.Text = objDireccionInmueble.RIUF.RIUF1;
        //            this.DropDownListPais.SelectedValue = this.DireccionRiuf.ClavePais;
        //            this.pnlBusqueda.Visible = true;
        //            this.pnlBusquedaBotones.Visible = true;  
        //            this.pnlGrivView.Visible = true;
        //            this.pnlDireccion.Visible = false;
        //            this.DireccionRiuf.CargaInicial();
        //            //this.PoblarRejillaDirecciones();
        //        }                    
        //    }
        //    catch (SqlException ex)
        //    {
        //        //execpcion
        //        Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        //        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
        //        MostrarMensajeJavaScript(Msj);
        //    }
        //    catch (Exception ex)
        //    {
        //        //execpcion
        //        Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        //        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
        //        MostrarMensajeJavaScript(Msj);
        //    }

        //    return Ok;
        //}     
    }
}