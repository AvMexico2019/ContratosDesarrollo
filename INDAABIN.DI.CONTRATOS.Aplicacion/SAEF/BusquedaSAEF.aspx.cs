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



namespace INDAABIN.DI.CONTRATOS.Aplicacion.SAEF
{
    public partial class BusquedaSAEF : System.Web.UI.Page
    {
        //obj de negocio
        List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_SAEF;
        String Msj;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfoBusqSAEF.Text = String.Empty;

            if(!IsPostBack)
            {
                 if (Session["Contexto"] == null)
                 {
                     Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));
                 }

                 String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                 if (!String.IsNullOrEmpty(RolUsr))
                 {
                     //autoseleccionar la institucion del usuario
                     int IdInstitucionUsr = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);

                 }

                 this.lblTableNameSAEF.Text = Session.SessionID.ToString() + "BusqTablaOpiniones";
                 this.PoblarDropDownListInstitucion();

            }
        }

        //poblar llenado de dropdownlist de Institucion, y si existe una institucion del usuario en la lista, desplegar las solicitudes asociadas
        private Boolean PoblarDropDownListInstitucion()
        {
            Boolean Ok = false;
            DropDownListInstitucionSAEF.DataTextField = "Descripcion";
            DropDownListInstitucionSAEF.DataValueField = "IdValue";

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListInstitucionSAEF.DataSource = AdministradorCatalogos.ObtenerCatalogoInstituciones();
                DropDownListInstitucionSAEF.DataBind();

                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                if (!String.IsNullOrEmpty(RolUsr))
                {

                    int IdInstitucionUsr = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
                    //el usuario autentificado es Promovente ó OIC, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.DropDownListInstitucionSAEF.Enabled = false; //usuarios propietarios del proceo no pueden registrar nuevas solicitude de opinión.

                    //autoseleccionar, si existe en la lista la institucion del usuario
                    if (this.DropDownListInstitucionSAEF.Items.Contains(this.DropDownListInstitucionSAEF.Items.FindByValue(IdInstitucionUsr.ToString())) == true)
                    {
                        this.DropDownListInstitucionSAEF.Items.FindByValue(IdInstitucionUsr.ToString()).Selected = true;
                        this.PoblarRejillaSolicitudesEmitidas(); //poblar la rejilla, pues ya se conoce la institucion para ejecutar la busqueda
                        Ok = true;
                    }
                    else
                    {
                        //si el usuario  es un promovente bloquear funcionalidad, a otro rol, permitirle hacer una seleccion de institucion
                        if (RolUsr.ToUpper().Replace(" ", "") == UtilContratosArrto.Roles.Promovente.ToString().ToUpper().Replace(" ", ""))
                        {
                            //bloquear al usuario realizar operacion, si no tiene una institucion asociada
                            this.ButtonConsultarSAEF.Enabled = false;
                            

                            Msj = "No se encontró una institución asociada a su usuario, por favor solicita a Indaabin que se asigne a tu cuenta la institución a la que estás adscrito";
                            this.LabelInfoBusqSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    
                        }
                        else
                        {
                            Msj = "Selecciona una institución y da clic en Consultar, para visualizar sus solicitudes de accesibilidad";
                            this.LabelInfoBusqSAEF.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de instituciones. Contacta al área de sistemas.";
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

        private Boolean PoblarRejillaSolicitudesEmitidas(bool forceUpdate = false)
        {
            Boolean Ok = false;
            ListAplicacionConcepto_SAEF = null;
            this.ButtonExportarExcelSAEF.Visible = false;
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
                        if (this.TextBoxFolioSAEF.Text.Length > 0 )
                        {
                            Msj = "Se encontraron: [" + ListAplicacionConcepto_SAEF.Count.ToString() + "] solicitud(es) de accesibilidad  con el Folio: [" + this.TextBoxFolioSolicitudSAEF.Text + "].";
                        } 

                        if (this.TextBoxFolioSolicitudSAEF.Text.Length > 0 )
                        {
                            Msj = "Se encontraron: [" + ListAplicacionConcepto_SAEF.Count.ToString() + "] solicitud(es) de accesibilidad  con el Folio: [" + this.TextBoxFolioSolicitudSAEF.Text + "].";
                        } 
                        else
                        {
                            
                                Msj = "Se encontraron: [" + ListAplicacionConcepto_SAEF.Count.ToString() + "] solicitud(es) de accesibilidad emitidas a la institución en la que estás adscrito.";
                           
                        }

                        this.ButtonExportarExcelSAEF.Visible = true;
                        this.LabelInfoBusqSAEF.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        
                        Ok = true;
                    }

                }
                else 
                {

                    if (this.TextBoxFolioSolicitudSAEF.Text.Length > 0)
                    {
                        Msj = "No existen solicitudes de accesibilidad de arrendamiento registrados con el Folio: [" + this.TextBoxFolioSolicitudSAEF.Text + "].";
                    }
                    else if (this.TextBoxFolioSAEF.Text.Length > 0)
                    {
                        Msj = "No existen solicitudes de accesibilidad de arrendamiento registrados con el Folio: [" + this.TextBoxFolioSolicitudSAEF.Text + "].";
                    }
                    else
                    {

                            Msj = "No existen solicitudes de accesibilidad de arrendamiento registradas a la institución a la que estás adscrito.";
                    }
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
            if (this.ValidarEntradaDatos())
            {
                try
                {
                    int intFolioOpinion = 0;
                    if (this.TextBoxFolioSolicitudSAEF.Text.Length > 0)
                        intFolioOpinion = Convert.ToInt32(this.TextBoxFolioSolicitudSAEF.Text);

                    int? FolioSAEF = null;
                    if (this.TextBoxFolioSAEF.Text.Length > 0)
                        FolioSAEF = Convert.ToInt32(this.TextBoxFolioSAEF.Text);

                    if (Session[this.lblTableNameSAEF.Text] != null)
                    {
                        ListAplicacionConcepto_SAEF = (List<ModeloNegocios.AplicacionConcepto>)Session[this.lblTableNameSAEF.Text];
                    }  
                    else
                    {
                        ListAplicacionConcepto_SAEF = new NG_SAEF().ObtenerSolicitudesEmisionOpinionEmitidasSAEF(Convert.ToInt32(this.DropDownListInstitucionSAEF.SelectedValue), intFolioOpinion, null,FolioSAEF);
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
            }
            return Ok;
        }

        //metodo para validar la entrada de datos
        private bool ValidarEntradaDatos()
        {
            //validamos el folio de emision de opinion
            if (this.TextBoxFolioSolicitudSAEF.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioSolicitudSAEF.Text) == false)
                {
                    Msj = "El folio de opinión deber ser un número entero, verifica.";
                    this.LabelInfoBusqSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    
                    this.TextBoxFolioSolicitudSAEF.Focus();
                    return false;
                }
            }

            //validamos el folio de SAEF
            if (this.TextBoxFolioSAEF.Text.Length > 0)
            {
                if (Util.IsEnteroNatural(this.TextBoxFolioSAEF.Text) == false)
                {
                    Msj = "El folio de accesibilidad deber ser un número entero, verifica.";
                    this.LabelInfoBusqSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                    this.TextBoxFolioSAEF.Focus();
                    return false;
                }
            }


            return true;
        }

        protected void ButtonConsultarSAEF_Click(object sender, EventArgs e)
        {
            this.PoblarRejillaSolicitudesEmitidas(true);
        }

        protected void GridViewSolicitudesOpinionEmitidasSAEF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ModeloNegocios.AplicacionConcepto ObjSAEF = (ModeloNegocios.AplicacionConcepto)e.Row.DataItem;

                
                LinkButton LinkAcuseSAEF = e.Row.FindControl("lnkAcuseSAEF") as LinkButton;

                int? FolioSAEF = Convert.ToInt32(ObjSAEF.FolioSAEF);

               if(FolioSAEF != null)
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

        protected void ButtonExportarExcelSAEF_ServerClick(object sender, EventArgs e)
        {
            if (this.GridViewSolicitudesOpinionEmitidasSAEF.Rows.Count > 0)
            {
                ExportarXLS();
            }
               
        }

        //exporta a Excel con todo y formato, como se ve la rejilla
        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewSolicitudesOpinionEmitidasSAEF.Columns.CloneFields();
                foreach (DataControlField col in gvdcfCollection)
                {
                    if (col.Visible)
                        gvExport.Columns.Add(col);
                }

                //gvExport.Columns[0].Visible = false;
                gvExport.Columns[8].Visible = false;
                gvExport.DataSource = Session[this.lblTableNameSAEF.Text];
                gvExport.DataBind();
                PaginaBase ExportExcel = new PaginaBase();
                ExportExcel.ExportarExcel(gvExport, this.lblTableNameSAEF.Text);
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al exportar a Excel. Contacta al área de sistemas.";
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
            }

          
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