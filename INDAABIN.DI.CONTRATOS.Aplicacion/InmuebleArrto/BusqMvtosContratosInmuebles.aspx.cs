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

namespace INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto
{
    public partial class BusqMvtosContratosInmuebles : System.Web.UI.Page
    {
        private String Msj;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = String.Empty;
            this.LabelInfoGridResult.Text = string.Empty;

            if (!IsPostBack)
            {
                try
                {
                    if (Session["Contexto"] == null)
                        Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                    this.LimpiarSessiones();

                    this.lblTableName.Text = Session.SessionID.ToString() + "BusqMvtosContratosInmuebles";

                    //obtener el rol del usuario autenticado
                    String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                    //Es promovente y cuenta con inmuebles registrados
                    //No es promovente, es administrador de contratos

                    if (this.PoblarDropDownListaPais())
                    {
                        //si pasa a esta linea es que no es promovente, o si lo es si tiene inmuebles registrados, a los cuales registrar contratos.
                        if (this.PoblarDropDownListEntidadFederativa())
                        {

                            if (this.PoblarDropDownListInstitucion())
                            {
                                //poblar la rejilla, pues ya se conoce la institucion para ejecutar la busqueda
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

                    //String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                    //determinar el tipo de usuario autenticado
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

        //Obtener si existen  inmuebles asociados a la instirucion del usuario autenticado
        private int ObtenerTotalInmueblesRegistradosAlaInstitucionUsr()
        {
            int InstitucionUserAutenticado = ((SSO)Session["Contexto"]).IdInstitucion.Value;
            int ConteoInmueblesRegistradosAlaInstitucion = 0;

            if (InstitucionUserAutenticado > 0)
            {
                try
                {
                    ConteoInmueblesRegistradosAlaInstitucion = new NG_InmuebleArrto().ObtenerConteoInmueblesArrtoXInstitucion(InstitucionUserAutenticado);
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al recuperar el total de la lista de inmuebles. Contacta al área de sistemas.";
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
            return ConteoInmueblesRegistradosAlaInstitucion;
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
                            this.ButtonConsultar.Enabled = false;
                            this.ButtonRegistrarInmueble.Enabled = false;
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

        //Obtener catalogo no depediente de : PAIS
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
                    ListInmuebleArrtoRegistadosXInstitucion = (List<ModeloNegocios.InmuebleArrto>)Session[this.lblTableName.Text];
                else
                    //ir a la BD por los datos
                    ListInmuebleArrtoRegistadosXInstitucion = new NG_InmuebleArrto().ObtenerMvtosContratosInmueblesRegistrados(0, IdInstitucion, intFolioContrato, IdPais_Busq, IdEstado_Busq, IdMpo_Busq, this.TextBoxRIUF.Text.Trim());
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
                        Msj = "Se encontraron: [" + this.ConteoIdInmueblesEnRejilla() + "] dirección(es) registrada(s) a la institución en la que estás adscrito, con [" + this.ConteoMvtosXInmueblesEnRejilla() + "] registro(s) de contrato de arrendamiento. Identifica el inmueble en la tabla para el que deseas realizar alguna operación de: Nuevo, Sustitución, Continuación ó exponer su Acuse de registro, ó da clic en Registrar dirección si no lo identifica en la rejilla.";
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript("Se encontraron: [" + this.ConteoIdInmueblesEnRejilla() + "] dirección(es) registrada(s) a la institución en la que estás adscrito, con [" + this.ConteoMvtosXInmueblesEnRejilla() + "] registro(s) de contrato de arrendamiento.");
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

        //obtener cuantos IdInmueble estan en la rejilla
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

        private int ConteoMvtosXInmueblesEnRejilla()
        {
            //List<int> ListIdInmueblesEnRejilla = new List<int>();
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = (List<ModeloNegocios.InmuebleArrto>)this.GridViewResult.DataSource;
            int totalMvtos = 0;

            foreach (ModeloNegocios.InmuebleArrto item in ListInmuebleArrtoRegistadosXInstitucion)
            {
                if (item.EmisionOpinion.FolioSMOI_AplicadoOpinion.ToString().Trim().Length > 0)
                    totalMvtos += 1;
                else
                    if (item.ContratoArrtoInmueble.FolioContratoArrto.ToString().Trim().Length > 0)
                        totalMvtos += 1;
            }

            ////List<int> ListIdInmueblesEnRejilla = new List<int>();
            //int totalMvtos = 0;

            //for (int i = 0; i < this.GridViewResult.Rows.Count; i++)
            //{
            //    //contabilizar mvtos, si el inmueble en la tabla tiene  folio de emisión de opinión
            //    if (Server.HtmlDecode(this.GridViewResult.Rows[i].Cells[6].Text).ToString().Trim().Length > 0)
            //        //ListIdInmueblesEnRejilla.Add(Convert.ToInt16(this.GridViewResult.Rows[i].Cells[0].Text));
            //        totalMvtos += 1;
            //    else
            //    {
            //        //no tiene, emisión de opinión, ver si el inmueble tiene folio de contrato de arrto, para contabilizar
            //        if (Server.HtmlDecode(this.GridViewResult.Rows[i].Cells[9].Text).ToString().Trim().Length > 0)
            //            //ListIdInmueblesEnRejilla.Add(Convert.ToInt16(this.GridViewResult.Rows[i].Cells[0].Text));
            //            totalMvtos += 1;
            //    }
            //}

            //List<int> distinct = ListIdInmueblesEnRejilla.Distinct().ToList();
            //return distinct.Count();
            return totalMvtos;
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {

            if (this.ValidarEntradaDatos())
            {
                this.PoblarRejillaMvtosInmueblesVsContratos(true);
            }
        }

        protected void ButtonRegistrarInmueble_Click(object sender, EventArgs e)
        {
            Session["URLQueLllama"] = "~/InmuebleArrto/BusqMvtosContratosInmuebles.aspx";
            Response.Redirect("~/InmuebleArrto/InmuebleArrto.aspx"); //redireccionar al registro de nueva solicitud
        }

        protected void GridViewResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sFolioContratoSeleccionado;
            string sDireccionInmuebleSeleccionado;
            string sEmisionOpinionSeleccionada;
            string sIdInmuebleArrendamientoSeleccionado;
            switch (e.CommandName)
            {
                case "NuevoContrato":
                    var clickedButtonNuevo = e.CommandSource as LinkButton;
                    var clickedRowNuevo = clickedButtonNuevo.NamingContainer as GridViewRow;
                    sIdInmuebleArrendamientoSeleccionado = clickedRowNuevo.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowNuevo.Cells[5].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowNuevo.Cells[5].Text).Trim();
                    sDireccionInmuebleSeleccionado = Server.HtmlDecode(clickedRowNuevo.Cells[2].Text);

                    //Desocupar objetos
                    clickedButtonNuevo = null;
                    clickedRowNuevo = null;

                    if (sDireccionInmuebleSeleccionado.Split(',').First() == "MÉXICO")
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?IdContrato=0&TipoArrto=1&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//1=Nuevo-Nacional
                    else
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?IdContrato=0&TipoArrto=4&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//4=Nuevo- extranjero
                    break;
                case "SustitucionContrato":
                    var clickedButtonSustOp = e.CommandSource as LinkButton;
                    var clickedRowSustOp = clickedButtonSustOp.NamingContainer as GridViewRow;
                    sIdInmuebleArrendamientoSeleccionado = clickedRowSustOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowSustOp.Cells[5].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowSustOp.Cells[5].Text).Trim();
                    sDireccionInmuebleSeleccionado = Server.HtmlDecode(clickedRowSustOp.Cells[2].Text);
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowSustOp.Cells[6].Text);
                    ////si existe folio de contrato
                    //if (Server.HtmlDecode(clickedRowSustOp.Cells[7].Text).Trim().Length == 0)
                    //{
                    //    int IdInmuebleArrto = Convert.ToInt32(Server.HtmlDecode(clickedRowSustOp.Cells[0].Text));
                    //    //identifica el contrato Padre, en la tabla de ContratoArrto o en ContratoArrto_Historico y obtiene una list de objetos: ListContratoArrtoHistorico
                    //    if (this.ObtenerInmueblesHistoricoCorrespondientesAlaInstitucionYMpoInmuebleSeleccionado(IdInmuebleArrto) > 0)
                    //    {
                    //        //des-ocupar objetos
                    //        clickedButtonSustOp = null;
                    //        clickedRowSustOp = null;
                    //        Session["TipoArrto"] = "Sustitución-ContratoArrto";
                    //        //redireccionar a la vista que expone los registros de contratos historicos 
                    //        Response.Redirect("~/Contrato/ContratoHistoricoXInstitucion.aspx", false);
                    //    }
                    //    else //No existe(n) contratos correspondientes con el inmueble, informar que debera solicitar nuevo contrato
                    //    {
                    //        //des-ocupar objetos
                    //        clickedButtonSustOp = null;
                    //        clickedRowSustOp = null;
                    //        Msj = "No existe registro de un contrato previo, para hacer la sustitución de arrendamiento, por favor registre el contrato como Nuevo";
                    //        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    //        MostrarMensajeJavaScript(Msj);
                    //    }
                    //}
                    //else //si existe un Folio de Contrato En la Tabla: Contratos
                    //{

                    //Desocupar objetos
                    clickedButtonSustOp = null;
                    clickedRowSustOp = null;

                    if (sDireccionInmuebleSeleccionado.Split(',').First() == "MÉXICO")
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?TipoArrto=2&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//2=Sustitucion Nacional
                    else
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?TipoArrto=5&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//5=Sustitucion extrj
                    //}
                    break;

                case "ContinuacionContrato":
                    var clickedButtonContOp = e.CommandSource as LinkButton;
                    var clickedRowContOp = clickedButtonContOp.NamingContainer as GridViewRow;
                    sIdInmuebleArrendamientoSeleccionado = clickedRowContOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowContOp.Cells[5].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowContOp.Cells[5].Text).Trim();
                    sDireccionInmuebleSeleccionado = Server.HtmlDecode(clickedRowContOp.Cells[2].Text);
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowContOp.Cells[6].Text);

                    //des-ocupar objetos
                    clickedButtonSustOp = null;
                    clickedRowSustOp = null;
                    if (sDireccionInmuebleSeleccionado.Split(',').First() == "MÉXICO")
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?TipoArrto=3&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//3=Continuacion nac.
                    else
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?TipoArrto=6&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//6=Continuacion ext.
                    //}
                    break;
                case "OtrasFigOcupacion":
                    var clickedButtonOtrasFigOcupacion = e.CommandSource as LinkButton;
                    var clickedRowOtrasFigOcupacion = clickedButtonOtrasFigOcupacion.NamingContainer as GridViewRow;

                    sIdInmuebleArrendamientoSeleccionado = clickedRowOtrasFigOcupacion.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowOtrasFigOcupacion.Cells[5].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowOtrasFigOcupacion.Cells[5].Text).Trim();
                    sDireccionInmuebleSeleccionado = Server.HtmlDecode(clickedRowOtrasFigOcupacion.Cells[2].Text);
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowOtrasFigOcupacion.Cells[6].Text) == "" ? "0" : Server.HtmlDecode(clickedRowOtrasFigOcupacion.Cells[6].Text).Trim();

                    //Desocupar objetos
                    clickedButtonOtrasFigOcupacion = null;
                    clickedRowOtrasFigOcupacion = null;

                    if (sDireccionInmuebleSeleccionado.Split(',').First() == "MÉXICO")
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/OtrasFigOcupacion.aspx?TipoArrto=1&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//1=Nacional
                    else
                        //se redirecciona y se pasa el QueryString, porque se utiliza la misma vista para los 3 tipos de registro.
                        Response.Redirect("~/Contrato/OtrasFigOcupacion.aspx?TipoArrto=2&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);//2=Extranjero
                    break;
            }
        }

        protected void GridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                INDAABIN.DI.CONTRATOS.ModeloNegocios.InmuebleArrto oInmueble = (INDAABIN.DI.CONTRATOS.ModeloNegocios.InmuebleArrto)e.Row.DataItem;
                ContratoArrto oConcepto = oInmueble.ContratoArrtoInmueble;

                //RCA 16/08/2017
                //usamos una bandera para ver si ya fue utilizado y si ya fue utilizado este solo mostrara el acuse
                int FolioContrato = Convert.ToInt32(oConcepto.FolioContratoArrto);
                var a = new INDAABIN.DI.CONTRATOS.Negocio.NG_InmuebleArrto();

                
                if (oInmueble.Bandera == null || oInmueble.Bandera == false)
                {
                    a.UpdateBandera(FolioContrato);
                }

                //RCA 15/08/2017
                DateTime FechaActual_T1 = new DateTime();
                DateTime FechaFinOcu_T2 = new DateTime();

                FechaActual_T1 = DateTime.Now;
                FechaFinOcu_T2 = oInmueble.FechaFinOcupacion;
                int Result = DateTime.Compare(FechaActual_T1, FechaFinOcu_T2);

                LinkButton linkNuevo = e.Row.FindControl("LinkNuevoContrato") as LinkButton;
                LinkButton linkAcuse = e.Row.FindControl("LinkButtonAcuseContrato") as LinkButton;
                LinkButton linkSustitucion = e.Row.FindControl("LinkSustitucionContrato") as LinkButton;
                LinkButton linkContinuacion = e.Row.FindControl("LinkContinuacionContrato") as LinkButton;
                LinkButton linkOtras = e.Row.FindControl("LinkButtonOtrasFigOcupacion") as LinkButton;

                if (linkAcuse != null)
                {
                    if (oInmueble.ContratoArrtoInmueble.FolioContratoArrto != null)
                    {
                        if (linkNuevo != null) { linkNuevo.Visible = false; }
                        if (linkOtras != null) { linkOtras.Visible = false; }
                        if (linkAcuse != null) { linkAcuse.Visible = true; }

                        if (oInmueble.ContratoArrtoInmueble.DescripcionTipoContrato.ToString().Contains("Otras"))
                        {
                            if (linkSustitucion != null) { linkSustitucion.Visible = false; }
                            if (linkContinuacion != null) { linkContinuacion.Visible = false; }
                        }
                        else
                        {

                            if (oInmueble.Bandera == true)
                            {

                                if (linkNuevo != null) { linkNuevo.Visible = false; }
                                if (linkSustitucion != null) { linkSustitucion.Visible = false; }
                                if (linkContinuacion != null) { linkContinuacion.Visible = false; }
                            }

                            //RCA 05/12/2017
                            else
                            {

                                //else if (oConcepto.IsNotReusable == 1 && Result > 0)
                                if ((oInmueble.Bandera == null && oInmueble.Bandera == false) && Result == 1)
                                {

                                    if (linkSustitucion != null) { linkSustitucion.Visible = true; }
                                    if (linkContinuacion != null) { linkContinuacion.Visible = true; }
                                }


                                if (oInmueble.Bandera == null && oConcepto.DescripcionTipoArrendamiento == "Nuevo" && Result > 0)
                                {
                                    if (linkSustitucion != null) { linkSustitucion.Visible = true; }
                                    if (linkContinuacion != null) { linkContinuacion.Visible = true; }
                                }

                                if(oInmueble.Bandera == null && oConcepto.DescripcionTipoArrendamiento == "Nuevo" && Result == -1)
                                {
                                    if (linkNuevo != null) { linkNuevo.Visible = false; }
                                    if (linkOtras != null) { linkOtras.Visible = false; }
                                }

                                if (oInmueble.Bandera == false && Result == -1)
                                {
                                    if (linkSustitucion != null) { linkSustitucion.Visible = false; }
                                    if (linkContinuacion != null) { linkContinuacion.Visible = false; }
                                }

                                if (oInmueble.Bandera == null && Result == -1)
                                {
                                    if (linkSustitucion != null) { linkSustitucion.Visible = false; }
                                    if (linkContinuacion != null) { linkContinuacion.Visible = false; }
                                }

                            }
                           
                        }
                        linkAcuse.Attributes["onclick"] = "openCustomWindow('" + oInmueble.ContratoArrtoInmueble.FolioContratoArrto.ToString() + "');";
                    }
                    else
                    {
                        if (oConcepto.IsNotReusable == 1)
                            if (linkNuevo != null) { linkNuevo.Visible = false; }
                            else
                                if (linkNuevo != null) { linkNuevo.Visible = true; }
                        if (linkOtras != null) { linkOtras.Visible = false; }
                        if (linkAcuse != null) { linkAcuse.Visible = false; }
                        if (linkSustitucion != null) { linkSustitucion.Visible = false; }
                        if (linkContinuacion != null) { linkContinuacion.Visible = false; }
                    }
                }//fin if principal
            }
        }

        private int ObtenerInmueblesHistoricoCorrespondientesAlaInstitucionYMpoInmuebleSeleccionado(int IdInmuebleArrto)
        {
            int TotalContratosEnHistorico = 0;

            try
            {

                //obtener a que IdEstado y IdMpo pertenecen el inmueble Arrto. seleccionado, para obtener sus descripciones y pasarlas a el SP que obtiene los contratosHistoricos
                ModeloNegocios.InmuebleArrto objInmuebleArrto = new NG_InmuebleArrto().ObtenerEstadoMpoXIdInmuebleArrto(IdInmuebleArrto);

                if (objInmuebleArrto != null)
                {
                    //obj de negocio
                    List<ModeloNegocios.ContratoArrtoHistorico> ListContratoArrtoHistorico;

                    //no existe folio en la tabla de Contratos, buscar si existen para la institucion del promovente y mpo del inmueble seleccionado
                    //ListContratoArrtoHistorico = new NG_ContratoArrto().ObtenerContratosArrtoHistorico("COMISION FEDERAL DE TELECOMUNICACIONES", "Ciudad de México", "CUAJIMALPA");
                    int IdInstitucion = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
                    byte IdEntidadFederativa = Convert.ToByte(objInmuebleArrto.IdEstado);

                    ListContratoArrtoHistorico = new NG_ContratoArrto().ObtenerContratosArrtoHistorico(IdInstitucion, IdEntidadFederativa, objInmuebleArrto.NombreMunicipio);

                    //devolver # de registros encontrados
                    TotalContratosEnHistorico = ListContratoArrtoHistorico.Count();
                    if (TotalContratosEnHistorico > 0)
                        //persistir la lista de contratos en historico para redireccionar a una vista que los expone en una rejilla
                        Session["ListContratoArrtoHistorico"] = ListContratoArrtoHistorico;

                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de inmuebles historica. Contacta al área de sistemas.";
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

            return TotalContratosEnHistorico;
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void GridViewResult_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Pager)
                e.Row.Cells[0].Visible = false; //IdInmueble
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

        //Obtener catalogo Depediente de: Municipios de Entidad federativa
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

        //exporta a Excel con todo y formato, como se ve la rejilla
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
                gvExport.Columns[11].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, "ContratosInmuebles");

                ////quitar paginacion para mandar todo a excel.
                //GridViewResult.AllowSorting = false;
                //GridViewResult.AllowPaging = false;
                ////this.GridViewResult.DataSource = Session["lstResultBusqInmueblesPort"] as List<Portafolio>;
                ////this.GridViewResult.DataBind();

                //StringBuilder sb = new StringBuilder();
                //StringWriter sw = new StringWriter(sb);
                //HtmlTextWriter htw = new HtmlTextWriter(sw);

                //Page page = new Page();
                //HtmlForm form = new HtmlForm();
                //GridViewResult.EnableViewState = false;
                //page.EnableEventValidation = false;
                ////Page que requieran los diseñadores RAD.
                //page.DesignerInitialize();
                //page.Controls.Add(form);
                //form.Controls.Add(GridViewResult);
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

        protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        {
            if (this.GridViewResult.Rows.Count > 0)
                ExportarXLS();
        }

        protected void GridViewResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewResult.DataSource = Session[this.lblTableName.Text];
            this.GridViewResult.PageIndex = e.NewPageIndex;
            this.GridViewResult.DataBind();
        }
    }
}