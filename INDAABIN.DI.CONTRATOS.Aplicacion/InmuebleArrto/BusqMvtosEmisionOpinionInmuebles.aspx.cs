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
    public partial class BusqMvtosEmisionOpinionInmuebles : System.Web.UI.Page
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
                    this.lblTableName.Text = Session.SessionID.ToString() + "BusqMvtosEmisionOpInmuebles";

                    //obtener el rol del usuario autenticado
                    String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);


                   

                    //si pasa a esta linea es que no es promovente, o si lo es si tiene inmuebles registrados, a los cuales registrar mvtos de emisión de opinión.
                    if (this.PoblarDropDownListEntidadFederativa())
                    {
                        if (PoblarDropDownListInstitucion())
                        {
                            //poblar la rejilla, pues ya se conoce la institucion del usuario autentificado, para ejecutar la busqueda
                            if (this.PoblarRejillaMvtosInmueblesVsEmisionOpinion())
                            {
                                if (Session["Msj"] != null)
                                {
                                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Session["Msj"].ToString() + "</div>";
                                    MostrarMensajeJavaScript(Msj);

                                }
                            }
                        }
                    }

                   
                    ////determinar el tipo de usuario autenticado
                    //if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                    if (RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.ButtonRegistrarInmueble.Visible = false; //no puede registrar inmuebles

                    this.GridViewResult.Focus();
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al cargar forma Contacta al área de sistemas.";
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
        
        //private int CountInmueblesRegistradosAlaInstitucionUsr()
        //{
        //    int InstitucionUserAutenticado = ((SSO)Session["Contexto"]).IdInstitucion.Value;
        //    int ConteoInmueblesRegistradosAlaInstitucion = 0;

        //    if (InstitucionUserAutenticado > 0)
        //    {
        //        try
        //        {
        //            ConteoInmueblesRegistradosAlaInstitucion = new NG_InmuebleArrto().ObtenerConteoInmueblesArrtoXInstitucion(InstitucionUserAutenticado);
        //        }
        //        catch (Exception ex)
        //        {
        //            Msj = "Ha ocurrido un error al recuperar el total de inmuebles. Contacta al área de sistemas.";
        //            this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
        //            MostrarMensajeJavaScript(Msj);

        //            BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
        //            {
        //                CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
        //                Aplicacion = "ContratosArrto",
        //                Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
        //                Funcion = MethodBase.GetCurrentMethod().Name + "()",
        //                DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
        //                Usr = ((SSO)Session["Contexto"]).UserName.ToString()
        //            };
        //            BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
        //            BitacoraExcepcionAplictivo = null;
        //        }
        //    }
        //    return ConteoInmueblesRegistradosAlaInstitucion;
        //}
              
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
                        
                       // MostrarMensajeJavaScript("Su usuario no esta asociado a ninguna institución, por favor solicite a Sistemas que asocie a su cuenta la institución");
                        Session["Msj"] = "Tú usuario no está asociado a ninguna institución, por favor solicita a Sistemas que asocie a tu cuenta la institución a la que estás adscrito";
                        Response.Redirect("~/Msj.aspx", false);
                    }


                    //el usuario autentificado es Promovente, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString() )
                        this.DropDownListInstitucion.Enabled = false;

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
                            Msj = "No se encontró una institución asociada a tu usuario, por favor solicita a Indaabin que se asigne a su cuenta la institución a la que estás adscrito"; ;
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                        }
                        else
                        {
                         Msj = "Selecciona una institución y da clic en Consultar, para visualizar las direcciones de inmuebles  de arrendamiento y sus movimientos asociados a la Institución.";
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
                Msj = "Ha ocurrido un error al recuperar la lista de entidades federativas. Contacta al área de sistemas.";
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

        private Boolean PoblarRejillaMvtosInmueblesVsEmisionOpinion(bool forceUpdate = false)
        {
            Boolean Ok = false;
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = null;
            int? IdInstitucion = 0;
            this.ButtonExportarExcel.Visible = false;
            GridViewResult.DataSource = null;
            GridViewResult.DataBind();

            if (forceUpdate)
                Session[this.lblTableName.Text] = null;


            IdInstitucion = Convert.ToInt32(this.DropDownListInstitucion.SelectedValue);

            int intFolioOpinion = 0;
            //verificar si se ha proporcionado un # de folio de opinion, para filtrar la obtencion de datos, sino se pasa 0
            if (this.TextBoxFolioOpinion.Text.Length > 0)
                intFolioOpinion = Convert.ToInt32(this.TextBoxFolioOpinion.Text);
            
            int IdEstado_Busq = 0;
            if (this.DropDownListEdo.SelectedItem.Text != "--")
                IdEstado_Busq = Convert.ToInt16(this.DropDownListEdo.SelectedValue);
        

            //el mpo depende del edo, para refrenciarlo
            int IdMpo_Busq = 0;
            if (this.DropDownListEdo.SelectedItem.Text != "--")
            {
                if (this.DropDownListMpo.SelectedItem.Text != "--")
                    IdMpo_Busq = Convert.ToInt16(this.DropDownListMpo.SelectedValue);
            }

            int intFolioSMOI = 0;
            //verificar si se ha proporcionado un # de folio de contrato, para filtrar la obtencion de datos, sino se pasa 0
            if (this.TextBoxFolioSMOI.Text.Length > 0)
                intFolioSMOI = Convert.ToInt32(this.TextBoxFolioSMOI.Text);

            try
            {
                if (Session[this.lblTableName.Text] != null)
                    ListInmuebleArrtoRegistadosXInstitucion = (List<ModeloNegocios.InmuebleArrto>)Session[this.lblTableName.Text];
                else
                    //ir a la BD por la informacion de mvtos de emisión de opinión por inmueble
                    ListInmuebleArrtoRegistadosXInstitucion = new NG_InmuebleArrto().ObtenerMvtosEmisionOpinionInmueblesRegistrados(IdInstitucion, intFolioOpinion, IdEstado_Busq, IdMpo_Busq, intFolioSMOI,null);
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

                        //if (GridViewResult.Rows.Count > 0)
                        if (ListInmuebleArrtoRegistadosXInstitucion.Count > 0)
                        {
                            this.ButtonExportarExcel.Visible = true;
                            Msj = "Se encontraron: [" + this.ConteoIdInmueblesEnRejilla().ToString() + "] dirección(es) registrada(s) a la institución en la que estás adscrito, con [" + this.ConteoMvtosXInmueblesEnRejilla() + "]  solicitudes de emisión de opinión. Identifica la dirección del inmueble en la tabla para la que deseas hacer realizar alguna operación  de: Nuevo, Sustitución, Continuación ó exponer su Acuse de registro, ó haz clic en Registrar dirección si no lo identifica en la rejilla.";
                            this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                            MostrarMensajeJavaScript("Se encontraron: [" + this.ConteoIdInmueblesEnRejilla().ToString() + "] dirección(es) registrada(s) a la institución en la que estás adscrito, con [" + this.ConteoMvtosXInmueblesEnRejilla() + "] solicitudes de emisión de opinión.");
                            this.LabelInfoGridResult.Text = Msj;
                            Ok = true;
                        }

                    }
                    else
                    {                       
                        Msj = "No se encontraron inmuebles de arrendamiento registrados con los parámetros especificados.";                     
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoGridResult.Text = Msj;
                        MostrarMensajeJavaScript(Msj);
                        Ok = true;
                    }
            }            
            return Ok;
        }

        private bool ValidarEntradaDatos()
        {
            if (this.TextBoxFolioOpinion.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioOpinion.Text) == false)
                {

                    Msj = "El folio de la solicitud de opinión deber ser un número entero, verifica.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                                        
                    this.TextBoxFolioOpinion.Focus();
                    return false;
                }
            }

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

        //obtener cuantos IdInmueble estan en la rejilla
        private int ConteoIdInmueblesEnRejilla()
        {
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = (List<ModeloNegocios.InmuebleArrto>)this.GridViewResult.DataSource;
            
            List<int> ListIdInmueblesEnRejilla = new List<int>();

            foreach (ModeloNegocios.InmuebleArrto item in ListInmuebleArrtoRegistadosXInstitucion)
            {
                ListIdInmueblesEnRejilla.Add(item.IdInmuebleArrendamiento);
            }
            //return ListIdInmueblesEnRejilla.Distinct().ToList().Count;
            return ListInmuebleArrtoRegistadosXInstitucion.Distinct().GroupBy(i => i.IdInmuebleArrendamiento).Count();
        }

        private int ConteoMvtosXInmueblesEnRejilla()
        {

            //List<int> ListIdInmueblesEnRejilla = new List<int>();
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucion = (List <ModeloNegocios.InmuebleArrto>)this.GridViewResult.DataSource;
            int totalMvtos = 0;

            foreach (ModeloNegocios.InmuebleArrto item in ListInmuebleArrtoRegistadosXInstitucion)
            {
                if (item.EmisionOpinion.FolioSMOI_AplicadoOpinion.ToString().Trim().Length > 0)
                    totalMvtos += 1;
                else
                    if (item.FolioContratoArrtoVsInmuebleArrendado.ToString().Trim().Length > 0)
                        totalMvtos += 1;
            }

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
            
            
            //return totalMvtos;

            return ListInmuebleArrtoRegistadosXInstitucion.Distinct().GroupBy(i => i.EmisionOpinion.FolioAplicacionConcepto).Count();
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
              if (this.ValidarEntradaDatos())
              {
                this.PoblarRejillaMvtosInmueblesVsEmisionOpinion(true);
              }
        }
        
        protected void ButtonRegistrarInmueble_Click(object sender, EventArgs e)
        {
            Session["URLQueLllama"] = "~/InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx";
            Response.Redirect("~/InmuebleArrto/InmuebleArrto.aspx"); //redireccionar al registro de nueva solicitud
        }
        
        protected void GridViewResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sFolioContratoSeleccionado;
            string sEmisionOpinionSeleccionada;
            string sIdInmuebleArrendamientoSeleccionado;

            switch (e.CommandName)
            {
                case "NuevaOpinion":
                    var clickedButtonNuevaOp = e.CommandSource as LinkButton;
                    var clickedRowNuevaOp = clickedButtonNuevaOp.NamingContainer as GridViewRow;

                    sIdInmuebleArrendamientoSeleccionado = clickedRowNuevaOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowNuevaOp.Cells[6].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowNuevaOp.Cells[6].Text).Trim();
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowNuevaOp.Cells[8].Text);

                    //des-ocupar objetos
                    clickedButtonNuevaOp = null;
                    clickedRowNuevaOp = null;

                    //ir al url correspondiente, se pasa QueryString porque se utiliza la misma vista para los 3 tipos de Emisio de Opinion
                    Response.Redirect("~/EmisionOpinion/EmisionOpinionSMOIOpcional.aspx?TemaOpinion=1&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);            
                    break;
                case "SustitucionOpinion":
                    var clickedButtonSustOp = e.CommandSource as LinkButton;
                    var clickedRowSustOp = clickedButtonSustOp.NamingContainer as GridViewRow;

                    sIdInmuebleArrendamientoSeleccionado = clickedRowSustOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowSustOp.Cells[6].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowSustOp.Cells[6].Text).Trim();
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowSustOp.Cells[8].Text);

                    
                    
                    //Desocupar objetos
                    clickedButtonSustOp = null;
                    clickedRowSustOp = null;
                    //ir al url correspondiente, se pasa QueryString porque se utiliza la misma vista para los 3 tipos de Emisio de Opinion
                    // Response.Redirect("~/EmisionOpinion/EmisionOpinion.aspx?TemaOpinion=2"); // Comantado por EESA
                    Response.Redirect("~/EmisionOpinion/EmisionOpinionSMOIOpcional.aspx?TemaOpinion=2&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado); 
                    //Response.Redirect("~/EmisionOpinion/EmisionOpinionSMOIOpcional.aspx?TemaOpinion=2&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);
                    //}
                    break;

                case "ContinuacionOpinion":
                    var clickedButtonContOp = e.CommandSource as LinkButton;
                    var clickedRowContOp = clickedButtonContOp.NamingContainer as GridViewRow;

                    sIdInmuebleArrendamientoSeleccionado = clickedRowContOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowContOp.Cells[6].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowContOp.Cells[6].Text).Trim();
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowContOp.Cells[8].Text);

                 

                    //Desocupar objetos
                    clickedButtonSustOp = null;
                    clickedRowSustOp = null;
                    //ir al url correspondiente, se pasa QueryString porque se utiliza la misma vista para los 3 tipos de Emisio de Opinion
                    //Response.Redirect("~/EmisionOpinion/EmisionOpinion.aspx?TemaOpinion=3");  // comentado por EESA
                    Response.Redirect("~/EmisionOpinion/EmisionOpinionSMOIOpcional.aspx?TemaOpinion=3&IdContrato=" + sFolioContratoSeleccionado + "&IdEmision=" + sEmisionOpinionSeleccionada + "&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);
                    //}
                    break;

                case "SeguridadOpinion":
                    var clickedButtonSegOp = e.CommandSource as LinkButton;
                    var clickedRowSegOp = clickedButtonSegOp.NamingContainer as GridViewRow;

                    sIdInmuebleArrendamientoSeleccionado = clickedRowSegOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowSegOp.Cells[6].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowSegOp.Cells[6].Text).Trim();
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowSegOp.Cells[8].Text);

                    //Desocupar objetos
                    clickedButtonSustOp = null;
                    clickedRowSustOp = null;

                    Response.Redirect("~/EmisionOpinion/EmisionOpinionSMOIOpcional.aspx?TemaOpinion=4&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado);
                    break;



            }//swicht
        }

        protected void GridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                INDAABIN.DI.CONTRATOS.ModeloNegocios.InmuebleArrto oInmueble = (INDAABIN.DI.CONTRATOS.ModeloNegocios.InmuebleArrto)e.Row.DataItem;
                
                // MZT se cambio a usarse aqui para optimizar el tiempo de carga
                if (string.IsNullOrEmpty(oInmueble.NombreUsuario))
                {
                    oInmueble.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(oInmueble.IdUsuarioRegistro);
                }                
                // MZT se cambio a usarse aqui para optimizar el tiempo de carga

                AplicacionConcepto oConcepto = oInmueble.EmisionOpinion;
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
                LinkButton linkNuevoOpinion = e.Row.FindControl("LinkNuevoOpinion") as LinkButton;
                LinkButton linkSustitucionOpinion = e.Row.FindControl("LinkSustitucionOpinion") as LinkButton;
                LinkButton linkContinuacionOpinion = e.Row.FindControl("LinkContinuacionOpinion") as LinkButton;
                LinkButton linkButtonAcuseOpinion = e.Row.FindControl("LinkButtonAcuseOpinion") as LinkButton;
                LinkButton linkSeguridad = e.Row.FindControl("LinkSeguridad") as LinkButton;

                //RCA 15/11/2018
                //ocultamos el link para seguridad
                if(oInmueble.IdInstitucion != 259)
                {
                    linkSeguridad.Visible = false;
                }

                if (linkButtonAcuseOpinion != null)
                {
                    if (oInmueble.FolioContratoArrtoVsInmuebleArrendado != null)
                    {
                        linkNuevoOpinion.Visible = false;
                        linkSustitucionOpinion.Visible = false;
                        linkSeguridad.Visible = false;

                        if (oConcepto.IsNotReusable == 0)
                        {
                            linkContinuacionOpinion.Visible = true;                            
                        }
                        else
                        {
                            linkContinuacionOpinion.Visible = false;
                        }         
                   
                        //RCA 16/08/2018
                        if (oConcepto.IsNotReusable > 0)
                        {
                            linkContinuacionOpinion.Visible = true; 
                        }

                        //RCA 21/11/2018
                        if(oInmueble.IdInstitucion == 259)
                        {
                            linkNuevoOpinion.Visible = false;
                            linkSustitucionOpinion.Visible = false;
                            linkContinuacionOpinion.Visible = false;
                            linkSeguridad.Visible = true;
                        }

                        linkButtonAcuseOpinion.Visible = true;
                    }
                    else
                    {
                        if (oInmueble.EmisionOpinion.FolioAplicacionConcepto == null)
                        {
                            linkNuevoOpinion.Visible = true;
                            linkSustitucionOpinion.Visible = false;
                            linkContinuacionOpinion.Visible = false;
                            linkButtonAcuseOpinion.Visible = false;

                            if (oInmueble.IdInstitucion == 259)
                            {
                                linkSeguridad.Visible = true;
                                linkNuevoOpinion.Visible = false;
                            }
                        }
                        else
                        {
                            //linkNuevoOpinion.Visible = true;

                            //RCA 16/08/2018
                            linkNuevoOpinion.Visible = false;

                            linkSustitucionOpinion.Visible = false;
                            linkContinuacionOpinion.Visible = false;

                            if (oInmueble.IdInstitucion == 259)
                            {
                                linkSeguridad.Visible = false;
                            }

                            linkButtonAcuseOpinion.Visible = true;
                        }
                    }
                    linkButtonAcuseOpinion.Attributes["onclick"] = "openCustomWindow('" + oConcepto.FolioAplicacionConcepto + "','" + strTipoArrendamiento + "');";
                }
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

                    int IdInstitucion = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
                    byte IdEntidadFederativa = Convert.ToByte(objInmuebleArrto.IdEstado);
                    
                    //no existe folio en la tabla de Contratos, buscar si existen para la institucion del promovente y mpo del inmueble seleccionado
                    //ListContratoArrtoHistorico = new NG_ContratoArrto().ObtenerContratosArrtoHistorico("COMISION FEDERAL DE TELECOMUNICACIONES", "Ciudad de México", "CUAJIMALPA");
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
                Msj = "Ha ocurrido un error al recuperar la lista de inmuebles historicos. Contacta al área de sistemas.";
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
                gvExport.Columns[10].Visible = false;
                gvExport.DataSource = Session[this.lblTableName.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                //ExportExcel.ExportarExcel(gvExport, this.lblTableName.Text);
                ExportExcel.ExportarExcel(gvExport, "EmisionDeOpinion");

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
            catch(Exception ex)
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