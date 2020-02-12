using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Reflection;

using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//capa BO
using INDAABIN.DI.ModeloNegocio; //bus
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;
using System.Web.UI.HtmlControls;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto
{
    public partial class BusqMvtosConvenioModificatorio : System.Web.UI.Page
    {
        private String Msj;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = String.Empty;
            //this.LabelInfoGridResult.Text = string.Empty;

            if (!IsPostBack)
            {
                try
                {
                    if (Session["Contexto"] == null)
                        Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                    this.LimpiarSessiones();

                    this.lblTableName.Text = Session.SessionID.ToString() + "BusqMvtosConvenioModificatorio";

                    String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                    if (this.PoblarDropDownListaPais())
                    {
                        //si pasa a esta linea es que no es promovente, o si lo es si tiene inmuebles registrados, a los cuales registrar contratos.
                        if (this.PoblarDropDownListEntidadFederativa())
                        {

                            if (this.PoblarDropDownListInstitucion())
                            {
                                //poblar la rejilla, pues ya se conoce la institucion para ejecutar la busqueda
                                //this.PoblarRejillaMvtosInmueblesVsContratos();
                                this.PoblarRejillaMvtosInmueblesVsContratos();

                                if (Session["Msj"] != null)
                                {
                                    Msj = Session["Msj"].ToString();
                                    this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                                    MostrarMensajeJavaScript(Msj);
                                }
                            }
                        }
                    }

                    if (RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        //   if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                        this.ButtonRegistrarInmueble.Visible = false; //no puede registrar inmuebles

                    this.GridViewResult.Focus();

                }

                catch (Exception ex)
                {
                    Msj = string.Format("Ha ocurrido un error durante la carga del modulo: {0}", ex.Message);
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
            }
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        private Boolean PoblarDropDownListInstitucion()
        {
            Boolean Ok = false;
            DropDownListInstitucion.DataTextField = "Descripcion";
            DropDownListInstitucion.DataValueField = "IdValue";
            int? IdInstitucion;

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListInstitucion.DataSource = AdministradorCatalogos.ObtenerCatalogoInstituciones();
                DropDownListInstitucion.DataBind();

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
                        // this.DropDownListPais.Enabled = false; //no deshabilitar, porque para institucion es posible registrar contratos: Nac y Extranjeros
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
                            //this.ButtonConsultar.Enabled = false;
                            //this.ButtonRegistrarInmueble.Enabled = false;
                            Msj = "No se encontró una institución asociada a tu usuario, por favor solicita a Indaabin que se asigne a su cuenta la Institución a la que estás adscrito";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                        }
                        else
                        {
                            Msj = "Selecciona una institución y da clic en Consultar, para visualizar los inmubles de arrendamiento y sus movimientos asociados a la Institución.";
                            this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de instituciones. Contacta al área de sistemas.";
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

        private Boolean PoblarDropDownListaPais()
        {
            Boolean Ok = false;
            this.DropDownListPais.Items.Clear();
            this.DropDownListPais.DataTextField = "Descripcion";
            this.DropDownListPais.DataValueField = "IdValue";

            try
            {
                //obtener informacion de paises del BUS, si la lista ya existe solo obtenerla, sino cargarla del BUS 
                List<Catalogo> oListaPais = AdministradorCatalogos.ObtenerCatalogoPais();

                if (!oListaPais.Contains(new Catalogo { IdValue = 0, Descripcion = "Todos" }))
                    oListaPais.Add(new Catalogo { IdValue = 0, Descripcion = "Todos" });

                this.DropDownListPais.DataSource = oListaPais;
                this.DropDownListPais.DataBind();

                //agregar un elemento para representar a todos
                this.DropDownListPais.SelectedValue = "0";
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de paises. Contacta al área de sistemas.";
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

        //Obtener catalogo no depediente de: Entidad Federativa
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
                //msj al usuario
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Entidades Federativas. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        private void LimpiarSessiones()
        {
            //limpieza de sessiones, de otras paginas
            Session["intFolioConceptoResp"] = null;
            Session["ListAplicacionConcepto"] = null;
            Session["Msj"] = null;
            Session["ListInmuebleArrtoRegistadosXInstitucion"] = null;
            Session["URLQueLllama"] = null;
            Session["IdInmuebleArrto"] = null;
            //generadas en: EmisionOpinion.aspx
            Session["ListCptosOpinionNuevo"] = null;
            Session["ListCptosOpinionSustitucion"] = null;
            Session["ListCptosOpinionContinuacion"] = null;
            //
            Session["NumContratoHist"] = null;
            Session["TipoArrto"] = null;
            Session["FolioContrato"] = null;
        }

        protected void DropDownListPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListPais.SelectedItem.Text.ToUpper() == "MÉXICO")
            {
                this.DropDownListEdo.Enabled = true;
                this.DropDownListMpo.Enabled = true;
            }
            else
            {
                this.DropDownListEdo.Enabled = false;
                this.DropDownListMpo.Enabled = false;
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

        private void PoblarDropDownListMposXEntFed()
        {

            this.DropDownListMpo.DataTextField = "Descripcion";
            this.DropDownListMpo.DataValueField = "IdValue";
            this.DropDownListMpo.DataSource = AdministradorCatalogos.ObtenerMunicipios(Convert.ToInt32(this.DropDownListEdo.SelectedValue));
            this.DropDownListMpo.DataBind();

            //agregar un elemento para representar a todos
            this.DropDownListMpo.Items.Add("--");
            this.DropDownListMpo.Items.FindByText("--").Selected = true;

        }

        protected void ButtonRegistrarInmueble_Click(object sender, EventArgs e)
        {
            Session["URLQueLllama"] = "~/InmuebleArrto/BusqMvtosConvenioModificatorio.aspx";
            Response.Redirect("~/InmuebleArrto/InmuebleArrto.aspx"); //redireccionar al registro de nueva solicitud

        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
            if (this.ValidarEntradaDatos())
            {
                this.PoblarRejillaMvtosInmueblesVsContratos(true);
            }
        }

        private Boolean PoblarRejillaMvtosInmueblesVsContratos(bool forceUpdate = false)
        {
            Boolean Ok = false;
            this.ButtonExportarExcel.Visible = false;
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = null;
            int? IdInstitucion = 0;

            GridViewResult.DataSource = null;
            GridViewResult.DataBind();

            if (forceUpdate)
                Session[this.lblTableName.Text] = null;


            //if (this.ValidarEntradaDatos())
            //{

            IdInstitucion = Convert.ToInt32(this.DropDownListInstitucion.SelectedValue);

            int intFolioContrato = 0;
            //verificar si se ha proporcionado un # de folio de contrato, para filtrar la obtencion de datos, sino se pasa 0
            if (this.TextBoxFolioContrato.Text.Length > 0)
                intFolioContrato = Convert.ToInt32(this.TextBoxFolioContrato.Text);

            int IdPais_Busq = 0;
            if (this.DropDownListPais.SelectedItem.Text != "--")
                IdPais_Busq = Convert.ToInt16(this.DropDownListPais.SelectedValue);

            //el edo depende del pais
            int IdEstado_Busq = 0;
            if (this.DropDownListPais.SelectedItem.Text != "--")
            {
                if (this.DropDownListEdo.SelectedItem.Text != "--")
                    IdEstado_Busq = Convert.ToInt16(this.DropDownListEdo.SelectedValue);
            }

            //el mpo depende del edo

            int IdMpo_Busq = 0;
            if (this.DropDownListEdo.SelectedItem.Text != "--")
            {
                if (this.DropDownListMpo.SelectedItem.Text != "--")
                    IdMpo_Busq = Convert.ToInt16(this.DropDownListMpo.SelectedValue);
            }

            try
            {
                if (Session[this.lblTableName.Text] != null)
                    ListInmuebleArrtoRegistadosXInstitucion = new NG_ContratoArrto().ObtenerContratosConvenioModificatorio(IdInstitucion.Value, intFolioContrato, this.TextBoxRIUF.Text.Trim(), IdPais_Busq, IdEstado_Busq, IdMpo_Busq);
                else
                    //ir a la BD por los datos
                    ListInmuebleArrtoRegistadosXInstitucion = new NG_ContratoArrto().ObtenerContratosConvenioModificatorio(IdInstitucion.Value, intFolioContrato, this.TextBoxRIUF.Text.Trim(), IdPais_Busq, IdEstado_Busq, IdMpo_Busq);
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de inmuebles. Contacta al área de sistemas.";
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


            //si existe el objeto y tiene contenido
            if (ListInmuebleArrtoRegistadosXInstitucion != null)
            {
                if (ListInmuebleArrtoRegistadosXInstitucion.Count > 0)
                {
                    //poblar la rejilla
                    GridViewResult.DataSource = ListInmuebleArrtoRegistadosXInstitucion;
                    GridViewResult.DataBind();
                    Session[this.lblTableName.Text] = ListInmuebleArrtoRegistadosXInstitucion;

                    this.ButtonExportarExcel.Visible = true;

                    if (GridViewResult.Rows.Count > 0)
                    {
                        Msj = "Se encontraron: [" + this.ConteoIdInmueblesEnRejilla() + "] dirección(es) registrada(s) a la institución en la que estás adscrito, con [" + this.ConteoMvtosXInmueblesEnRejilla() + "] registro(s) de convenio modificatorio. Identifica el inmueble en la tabla para el que deseas realizar alguna operación de: Nuevo, Sustitución, Continuación ó exponer su Acuse de registro, ó da clic en Registrar direccion si no lo identifica en la rejilla.";
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript("Se encontraron: [" + this.ConteoIdInmueblesEnRejilla() + "] dirección(es) registrada(s) a la institución en la que estás adscrito, con [" + this.ConteoMvtosXInmueblesEnRejilla() + "] registro(s) de convenio modificatorio.");
                        this.LabelInfoGridResult.Text = this.LabelInfo.Text;
                        Ok = true;
                    }
                }
                else
                {
                    Msj = "No se encontraron direcciones de inmuebles de arrendamiento registrados con los parámetros especificados.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.LabelInfoGridResult.Text = Msj;
                    Ok = true;
                }
            }
            else
            {
                Msj = "No se encontraron direcciones de inmuebles de arrendamiento registrados con los parámetros especificados.";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                this.LabelInfoGridResult.Text = Msj;
                Ok = true;
            }
            //}
            return Ok;
        }

        private int ConteoMvtosXInmueblesEnRejilla()
        {
            //List<int> ListIdInmueblesEnRejilla = new List<int>();
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = (List<ModeloNegocios.InmuebleArrto>)this.GridViewResult.DataSource;
            int totalMvtos = 0;

            foreach (ModeloNegocios.InmuebleArrto item in ListInmuebleArrtoRegistadosXInstitucion)
            {
                if (item.ContratoArrtoInmueble.IdConvenio > 0)
                    totalMvtos += 1;
                else
                    if (item.ContratoArrtoInmueble.FolioContratoArrto.ToString().Trim().Length > 0)
                        totalMvtos += 1;
            }

            return totalMvtos;
        }

        private int ConteoIdInmueblesEnRejilla()
        {

            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = (List<ModeloNegocios.InmuebleArrto>)this.GridViewResult.DataSource;
            List<int> ListIdInmueblesEnRejilla = new List<int>();

            foreach (ModeloNegocios.InmuebleArrto item in ListInmuebleArrtoRegistadosXInstitucion)
            {
                ListIdInmueblesEnRejilla.Add(item.IdInmuebleArrendamiento);
            }

            return ListIdInmueblesEnRejilla.Distinct().ToList().Count;
        }

        private bool ValidarEntradaDatos()
        {

            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacion


            if (this.TextBoxFolioContrato.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioContrato.Text) == false)
                {

                    Msj = "El folio del contrato deber ser un número entero, verifica.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.TextBoxFolioContrato.Focus();
                    return false;
                }
            }


            if (this.TextBoxRIUF.Text.Length > 0)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(this.TextBoxRIUF.Text, "^[-0-9]*$ "))
                {
                    Msj = "la clave RIUF del inmueble deben ser números separados por un guión con la siguiente nomenclatura #-#####-# ";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.TextBoxRIUF.Focus();
                    return false;
                }

            }

            Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
            return true;
        }

        protected void GridViewResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandName))
                return;

            switch (e.CommandName)
            {
                case "ConvenioModificatorio":

                    var BotonNuevo = e.CommandSource as LinkButton;
                    GridViewRow fila = BotonNuevo.NamingContainer as GridViewRow;

                    if (fila != null)
                    {
                        string folioContrato = fila.Cells[4].Text;
                        string IdInmuebleArrendamiento = fila.Cells[0].Text;
                        Response.Redirect("~/Contrato/ConvenioModificatorioRegistro.aspx?IdContrato=" + folioContrato + "&IdInmueble=" + IdInmuebleArrendamiento);
                    }

                    break;

                case "AcuseConvenio":
                    break;
            }            
        }

        protected void GridViewResult_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Pager)
                e.Row.Cells[0].Visible = false; //IdInmueble
        }

        protected void GridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                INDAABIN.DI.CONTRATOS.ModeloNegocios.InmuebleArrto oInmueble = (INDAABIN.DI.CONTRATOS.ModeloNegocios.InmuebleArrto)e.Row.DataItem;
                ContratoArrto oConcepto = oInmueble.ContratoArrtoInmueble;

                LinkButton linkNuevo = e.Row.FindControl("LinkNuevoContrato") as LinkButton;
                //LinkButton linkAcuse = e.Row.FindControl("LinkButtonAcuseContrato") as LinkButton;
                HtmlAnchor linkAcuse = (HtmlAnchor)e.Row.FindControl("LinkButtonAcuseContrato");

                if (oConcepto.IdConvenio == 0)
                {
                    linkNuevo.Visible = true;
                    linkAcuse.Visible = false;

                    if (oConcepto.DescripcionTipoArrendamiento != "Nuevo" && oConcepto.DescripcionTipoArrendamiento != "Continuación")
                        linkNuevo.Visible = false;
                }

                else                
                    linkAcuse.Attributes.Add("onclick", "ObtenerConveniosModificatorios(" + oInmueble.FolioContratoArrto + ");");
                
            }
        }

        protected void GridViewResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewResult.DataSource = Session[this.lblTableName.Text];
            this.GridViewResult.PageIndex = e.NewPageIndex;
            this.GridViewResult.DataBind();
        }

        protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        {
            if (this.GridViewResult.Rows.Count > 0)
                ExportarXLS();
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
                gvExport.Columns[6].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, "ContratosInmuebles");
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
    }
}