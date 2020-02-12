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

//conexion con las demas capas
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//capa BO
using INDAABIN.DI.ModeloNegocio; //bus
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;
using INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.SAEF
{
    public partial class BusqMvtosSAEF : System.Web.UI.Page
    {

        private String Msj;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfoSAEF.Text = string.Empty;
            this.LabelInfoGridResultSAEF.Text = string.Empty;

            if(!IsPostBack)
            {
                try
                {
                    //valida que la sesion no sea nula, si no se regresa al sso
                    if(Session["Contexto"] == null)
                    {
                        Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));
                    }

                    //this.LimpiarSessiones();
                    //this.lblTableNameSAEF.Text = Session.SessionID.ToString() + "BusqMvtosSAEF";

                    //obtener el rol del usuario autenticado
                    String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);


                    //si pasa a esta linea es que no es promovente, o si lo es si tiene inmuebles registrados, a los cuales registrar mvtos de SAEF.
                    if(this.PoblarDropDownListEntidadFederativa())
                    {
                        if(PoblarDropDownListInstitucion())
                        {
                            //poblar la rejilla, pues ya se conoce la institucion del usuario autentificado, para ejecutar la busqueda
                            if (this.PoblarRejillaMvtosInmueblesVsEmisionOpinionSAEF())
                            {
                                if (Session["Msj"] != null)
                                {
                                    this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Session["Msj"].ToString() + "</div>";
  
                                }
                            }
                        }
                    }

                    //si el usuario es un OIC
                    if (RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                    {

                    }

                }
                catch(Exception ex)
                {
                    Msj = "Ha ocurrido un error al cargar. Contacta al área de sistemas.";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                   

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
           
        }

        //Obtener catalogo no depediente de: Entidad Federativa
        private Boolean PoblarDropDownListEntidadFederativa()
        {
            Boolean Ok = false;
            DropDownListEdoSAEF.DataTextField = "Descripcion";
            DropDownListEdoSAEF.DataValueField = "IdValue";

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListEdoSAEF.DataSource = AdministradorCatalogos.ObtenerCatalogoEstados();
                DropDownListEdoSAEF.DataBind();
                //agregar un elemento para representar a todos
                DropDownListEdoSAEF.Items.Add("--");
                this.DropDownListEdoSAEF.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de entidades federativas. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";

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

        private Boolean PoblarDropDownListInstitucion()
        {
            Boolean Ok = false;
            DropDownListInstitucionSAEF.DataTextField = "Descripcion";
            DropDownListInstitucionSAEF.DataValueField = "IdValue";
            int? IdInstitucion;

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListInstitucionSAEF.DataSource = AdministradorCatalogos.ObtenerCatalogoInstituciones();
                DropDownListInstitucionSAEF.DataBind();

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
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.DropDownListInstitucionSAEF.Enabled = false;

                    //autoseleccionar, si existe en la lista la institucion del usuario
                    if (this.DropDownListInstitucionSAEF.Items.Contains(this.DropDownListInstitucionSAEF.Items.FindByValue(IdInstitucion.ToString())) == true)
                    {
                        this.DropDownListInstitucionSAEF.Items.FindByValue(IdInstitucion.ToString()).Selected = true;

                        Ok = true;
                    }
                    else
                    {
                        //si el usuario  es un promovente bloquear funcionalidad, a otro rol, permitirle hacer una seleccion de institucion
                        if (RolUsr.ToUpper().Replace(" ", "") == UtilContratosArrto.Roles.Promovente.ToString().ToUpper().Replace(" ", ""))
                        {
                            //bloquear al usuario realizar operacion, si no tiene una institucion asociada
                            this.ButtonConsultarSAEF.Enabled = false;
                            Msj = "No se encontró una institución asociada a tu usuario, por favor solicita a Indaabin que se asigne a su cuenta la institución a la que estás adscrito"; ;
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                        }
                        else
                        {
                            Msj = "Selecciona una institución y da clic en Consultar, para visualizar las direcciones de inmuebles  de arrendamiento y sus movimientos asociados a la Institución.";
                            this.LabelInfoSAEF.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de instituciones. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
           

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


        protected void DropDownListEdoSAEF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListEdoSAEF.SelectedItem.Text != "--")
            {
                this.PoblarDropDownListMposXEntFed();
            }
                

            else
            {
                //limpiar mpos porque se ha seleccionado que no se buscara por estado
                this.DropDownListMpoSAEF.DataSource = null;
                this.DropDownListMpoSAEF.DataBind();
                this.DropDownListMpoSAEF.Items.Clear();
            }
        }

        private void PoblarDropDownListMposXEntFed()
        {

            this.DropDownListMpoSAEF.DataTextField = "Descripcion";
            this.DropDownListMpoSAEF.DataValueField = "IdValue";
            this.DropDownListMpoSAEF.DataSource = AdministradorCatalogos.ObtenerMunicipios(Convert.ToInt32(this.DropDownListEdoSAEF.SelectedValue));
            this.DropDownListMpoSAEF.DataBind();

            //agregar un elemento para representar a todos
            this.DropDownListMpoSAEF.Items.Add("--");
            this.DropDownListMpoSAEF.Items.FindByText("--").Selected = true;

        }

         private Boolean PoblarRejillaMvtosInmueblesVsEmisionOpinionSAEF(bool forceUpdate = false)
        {
            Boolean Ok = false;
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosSAEF = null;
            int? IdInstitucion = 0;
            this.ButtonExportarExcelSAEF.Visible = false;
            GridViewResultadoSAEF.DataSource = null;
            GridViewResultadoSAEF.DataBind();

            if (forceUpdate)
                Session[this.lblTableNameSAEF.Text] = null;


            IdInstitucion = Convert.ToInt32(this.DropDownListInstitucionSAEF.SelectedValue);

            int intFolioOpinion = 0;
            //verificar si se ha proporcionado un # de folio de opinion, para filtrar la obtencion de datos, sino se pasa 0
            if (this.TextBoxFolioOpinionSAEF.Text.Length > 0)
                intFolioOpinion = Convert.ToInt32(this.TextBoxFolioOpinionSAEF.Text);
            
            int IdEstado_Busq = 0;
            if (this.DropDownListEdoSAEF.SelectedItem.Text != "--")
                IdEstado_Busq = Convert.ToInt16(this.DropDownListEdoSAEF.SelectedValue);
        

            //el mpo depende del edo, para refrenciarlo
            int IdMpo_Busq = 0;
            if (this.DropDownListEdoSAEF.SelectedItem.Text != "--")
            {
                if (this.DropDownListMpoSAEF.SelectedItem.Text != "--")
                    IdMpo_Busq = Convert.ToInt16(this.DropDownListMpoSAEF.SelectedValue);
            }

             //obtenemos el folioSAEF
            int? FolioSAEF = null;
            if (this.TextBoxFolioSAEF.Text.Length > 0)
                FolioSAEF = Convert.ToInt32(this.TextBoxFolioSAEF.Text);


            try
            {
                if (Session[this.lblTableNameSAEF.Text] != null)
                {
                    //ListInmuebleArrtoRegistadosSAEF = (List<ModeloNegocios.InmuebleArrto>)Session[this.lblTableNameSAEF.Text];
                    ListInmuebleArrtoRegistadosSAEF = new NG_InmuebleArrto().ObtenerMvtosSAEFInmueblesRegistrados(IdInstitucion, intFolioOpinion, IdEstado_Busq, IdMpo_Busq, 0, FolioSAEF);
                }
                    
                else
                {
                    //ir a la BD por la informacion de mvtos de emisión de opinión por inmueble
                    ListInmuebleArrtoRegistadosSAEF = new NG_InmuebleArrto().ObtenerMvtosSAEFInmueblesRegistrados(IdInstitucion, intFolioOpinion, IdEstado_Busq, IdMpo_Busq, 0,FolioSAEF);
                }
                   
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de inmuebles. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                

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
            if (ListInmuebleArrtoRegistadosSAEF != null)
            {
                if (ListInmuebleArrtoRegistadosSAEF.Count > 0)
                {
                    //poblar la rejilla
                    GridViewResultadoSAEF.DataSource = ListInmuebleArrtoRegistadosSAEF;
                    GridViewResultadoSAEF.DataBind();
                    Session[this.lblTableNameSAEF.Text] = ListInmuebleArrtoRegistadosSAEF;

                    //if (GridViewResult.Rows.Count > 0)
                    if (ListInmuebleArrtoRegistadosSAEF.Count > 0)
                    {
                        this.ButtonExportarExcelSAEF.Visible = true;
                        Msj = "Se encontraron [" + this.ConteoMvtosXInmueblesEnRejilla() + "]  solicitudes de emisión de opinión. Identifica la dirección del inmueble en la tabla para la que deseas hacer realizar alguna operación  de: Nuevo, Sustitución, Continuación ó exponer su Acuse de registro, ó haz clic en Registrar direccion si no lo identifica en la rejilla.";
                        this.LabelInfoSAEF.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        this.LabelInfoGridResultSAEF.Text = Msj;
                        Ok = true;
                    }

                }
                else
                {
                    Msj = "No se encontraron inmuebles de arrendamiento registrados con los parámetros especificados.";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoGridResultSAEF.Text = Msj;
                    Ok = true;
                }
            }            
            return Ok;
        }


         private int ConteoMvtosXInmueblesEnRejilla()
         {

             List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoRegistadosXInstitucionSAEF = (List<ModeloNegocios.InmuebleArrto>)this.GridViewResultadoSAEF.DataSource;
             int totalMvtos = 0;

             foreach (ModeloNegocios.InmuebleArrto item in ListInmuebleArrtoRegistadosXInstitucionSAEF)
             {
                 if (item.EmisionOpinion.FolioSMOI_AplicadoOpinion.ToString().Trim().Length > 0)
                     totalMvtos += 1;
                 else
                     if (item.FolioContratoArrtoVsInmuebleArrendado.ToString().Trim().Length > 0)
                         totalMvtos += 1;
             }

             return ListInmuebleArrtoRegistadosXInstitucionSAEF.Distinct().GroupBy(i => i.EmisionOpinion.FolioAplicacionConcepto).Count();
         }

        protected void ButtonConsultarSAEF_Click(object sender, EventArgs e)
        {
            if (this.ValidarEntradaDatos())
            {
                this.PoblarRejillaMvtosInmueblesVsEmisionOpinionSAEF(true);
            }
        }

        private bool ValidarEntradaDatos()
        {
            if (this.TextBoxFolioOpinionSAEF.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioOpinionSAEF.Text) == false)
                {

                    Msj = "El folio de la solicitud de opinión deber ser un número entero, verifica.";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";


                    this.TextBoxFolioOpinionSAEF.Focus();
                    return false;
                }
            }

            return true;
        }

        //metodo para mostar la operacion correspon diente de acuerdo a la regla de negocios
        protected void GridViewResultadoSAEF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.DataItem != null)
            {
                ModeloNegocios.InmuebleArrto ObjSAEF = (ModeloNegocios.InmuebleArrto)e.Row.DataItem;

               
 
                LinkButton LinkNuevoSAEF = e.Row.FindControl("LinkNuevoOpinionSAEF") as LinkButton;

                LinkButton LinkAcuseSAEF = e.Row.FindControl("LinkButtonAcuseOpinionSAEF") as LinkButton;

                int? FolioContrato = ObjSAEF.FolioContratoArrtoVsInmuebleArrendado;

                int? FolioSAEF = Convert.ToInt32(ObjSAEF.FolioSAEF);

                

              if( FolioContrato != null)
              {
                  if(FolioSAEF != null)
                  {
                      if(FolioSAEF == 0)
                      {
                          LinkNuevoSAEF.Visible = false;
                          LinkAcuseSAEF.Visible = false;
                      }
                      else
                      {
                          LinkNuevoSAEF.Visible = false;
                          LinkAcuseSAEF.Visible = true;
                      }
                     
                  }
                  else
                  {
                      
                      LinkNuevoSAEF.Visible = false;
                      LinkAcuseSAEF.Visible = true; 

                  }
              }
              else
              {
                  if(FolioSAEF != null)
                  {
                      if (FolioSAEF > 0)
                      {
                          LinkNuevoSAEF.Visible = false;
                          LinkAcuseSAEF.Visible = true;
                      }
                      else
                      {
                          LinkNuevoSAEF.Visible = true;
                          LinkAcuseSAEF.Visible = false;
                      }
                  }
                  else
                  {
                      LinkNuevoSAEF.Visible = true;
                      LinkAcuseSAEF.Visible = false;
                  }
                  
              }
                    
            }
        }

        //este metodo contiene las acciones que hara cada boton
        protected void GridViewResultadoSAEF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string sFolioContratoSeleccionado;
            string sEmisionOpinionSeleccionada;
            string sIdInmuebleArrendamientoSeleccionado;

            switch(e.CommandName)
            {
                case "NuevaOpinionSAEF":
                    var clickedButtonNuevaOp = e.CommandSource as LinkButton;
                    var clickedRowNuevaOp = clickedButtonNuevaOp.NamingContainer as GridViewRow;

                    sIdInmuebleArrendamientoSeleccionado = clickedRowNuevaOp.Cells[0].Text;
                    sEmisionOpinionSeleccionada = Server.HtmlDecode(clickedRowNuevaOp.Cells[6].Text).Trim() == "" ? "0" : Server.HtmlDecode(clickedRowNuevaOp.Cells[6].Text).Trim();
                    sFolioContratoSeleccionado = Server.HtmlDecode(clickedRowNuevaOp.Cells[9].Text);

                    //des-ocupar objetos
                    clickedButtonNuevaOp = null;
                    clickedRowNuevaOp = null;

                    //ir al url correspondiente, se pasa QueryString porque se utiliza la misma vista para los 3 tipos de Emisio de Opinion
                    Response.Redirect("~/SAEF/SAEFRegistro.aspx?TemaOpinion=1&IdInmueble=" + sIdInmuebleArrendamientoSeleccionado + "&FolioEmision=" + sEmisionOpinionSeleccionada);
                    break;
            }

        }

        //meetodo para hacer invisible la primera fila 
        protected void GridViewResultadoSAEF_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                e.Row.Cells[0].Visible = false; //IdInmueble
            }
                
        }

        protected void GridViewResultadoSAEF_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewResultadoSAEF.DataSource = Session[this.lblTableNameSAEF.Text];
            this.GridViewResultadoSAEF.PageIndex = e.NewPageIndex;
            this.GridViewResultadoSAEF.DataBind();
        }

        protected void ButtonExportarExcelSAEF_ServerClick(object sender, EventArgs e)
        {
            if (this.GridViewResultadoSAEF.Rows.Count > 0)
            {
                ExportarXLS();
            }
                
        }

        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewResultadoSAEF.Columns.CloneFields();
                foreach (DataControlField col in gvdcfCollection)
                {
                    if (col.Visible)
                    {
                        gvExport.Columns.Add(col);
                    }
                       
                }
                gvExport.Columns[0].Visible = false;
                gvExport.Columns[11].Visible = false;
                gvExport.DataSource = Session[this.lblTableNameSAEF.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
               
                ExportExcel.ExportarExcel(gvExport, "EmisionDeOpinion");

                
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al exportar a Excel. Contacta al área de sistemas.";
                this.LabelInfoGridResultSAEF.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
               

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


        //metodon para cuando se presione el boton de acuse del SAEF.
        protected void LinkButtonAcuseOpinionSAEF_Click(object sender, EventArgs e)
        {
            INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias.ExportHTML exportar = new INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias.ExportHTML();

            //obtenemos el idaplicacion o el folio de aplicacion
            int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;

            int FolioSAEF = Convert.ToInt32(GridViewResultadoSAEF.DataKeys[rowIndex].Values["FolioSAEF"]);

            //buscamos el idaplicacionconcepto
            int IdAplicacionConcept = new NG_SAEF().ObtenerAplpicacionConcepto(FolioSAEF);


            //mostramos eoll acuse
            exportar.CuerpoCompletoPlantillaSAEF(null, IdAplicacionConcept);


        }

       
       
    }
}