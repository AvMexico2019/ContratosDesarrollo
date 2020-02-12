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

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Contrato
{
    public partial class BusqContrato : System.Web.UI.Page
    {

        //obj de negocio
        String Msj;
        String RolUsr;
        List<ModeloNegocios.ContratoArrto> ListContratosRegistrados;

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

                this.lblTableName.Text = Session.SessionID.ToString() + "BusqTablaContratos";
                if (this.PoblarDropDownTipoContrato())
                    this.PoblarDropDownListInstitucion();
            }


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

                RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                if (!String.IsNullOrEmpty(RolUsr))
                {

                    int IdInstitucionUsr = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
                    //el usuario autentificado es Promovente, entonces no permitir busq por institucion
                    if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString()
                        || RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        this.DropDownListInstitucion.Enabled = false; //usuarios propietarios del proceo no pueden registrar nuevas solicitude de opinión.

                    //autoseleccionar, si existe en la lista la institucion del usuario
                    if (this.DropDownListInstitucion.Items.Contains(this.DropDownListInstitucion.Items.FindByValue(IdInstitucionUsr.ToString())) == true)
                    {
                        this.DropDownListInstitucion.Items.FindByValue(IdInstitucionUsr.ToString()).Selected = true;
                        this.PoblarRejillaBusqContratos(); //poblar la rejilla, pues ya se conoce la institucion para ejecutar la busqueda
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


                            Msj = "No se encontró una institución asociada a su usuario, por favor solicita a Indaabin que se asigne a tu cuenta la Institución a la que estás adscrito";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(this.LabelInfo.Text);
                        }
                        else
                        {
                            Msj = "Selecciona una Institución y da clic en Buscar, para visualizar las solicitudes de opinión emitidas";
                            this.LabelInfo.Text = "<div class='alert alert-info'><strong>Sugerencia: </strong> " + Msj + "</div>";
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                //msj al usuario
                //Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de instituciones. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfo.Text = Msj;
                MostrarMensajeJavaScript(this.LabelInfo.Text);

            }
            return Ok;
        }

        private Boolean PoblarRejillaBusqContratos(bool forceUpdate = false)
        {

            Boolean Ok = false;
            ListContratosRegistrados = null;
            this.ButtonExportarExcel.Visible = false;
            this.GridViewBusqContratos.DataSource = null;
            this.GridViewBusqContratos.DataBind();
            //obtener informacion de la BD

            if (forceUpdate)
                Session[this.lblTableName.Text] = null;

            if (this.ObtenerInfoContratos())
            {
                //si existe el objeto y tiene contenido
                if (ListContratosRegistrados != null && ListContratosRegistrados.Count > 0)
                {

                    this.GridViewBusqContratos.DataSource = ListContratosRegistrados;
                    this.GridViewBusqContratos.DataBind();
                    Session[this.lblTableName.Text] = ListContratosRegistrados;

                    if (this.GridViewBusqContratos.Rows.Count > 0)
                    {
                        Msj = "Se encontraron: [" + ListContratosRegistrados.Count.ToString() + "] contrato(s) de arrendamiento registrado(s) a la institución en la que estás adscrito.";
                        this.ButtonExportarExcel.Visible = true;
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        Ok = true;
                    }

                }
                else //este else, en teoria no aplicaria porque el redireccionamiento lo hace el webform:  [ControladorEmisponOpinion.aspx]
                {

                    //si solo esta el paramentro de busqueda
                    if (this.DropDownListTipoContrato.SelectedItem.Text == "--" && this.TextBoxFolioContrato.Text.Length == 0)
                    {

                        if (RolUsr == UtilContratosArrto.Roles.Promovente.ToString())
                            Msj = "No existen contratos de arrendamiento registrados a la institución a la que estás adscrito, da clic en Nuevo, para registrar uno.";
                        else //es otro que puede hacer busq por institucion
                            Msj = "No existen contratos de arrendamiento registrados a la institución a la que estás adscrito, proporciona algun parámetro de búsqueda y da clic en Consultar.";
                    }

                    else
                    {
                        if (this.TextBoxFolioContrato.Text.Length > 0)
                            Msj = "No existen contratos de arrendamiento registrados con el Folio: [" + this.TextBoxFolioContrato.Text + "].";
                        else
                        {
                            if (this.DropDownListTipoContrato.SelectedItem.Text == "--")
                                Msj = "No existen contratos de arrendamiento registrados a la institución a la que estás adscrito, da clic en Nueva, para registrar una.";
                            else
                                Msj = "No existen contratos de arrendamiento registrados con los parámetros proporcionados.";
                        }
                    }

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);

                    Ok = true;
                    //Response.Redirect("Opinion.aspx"); //redireccionar al registro de nueva solicitud
                }

            }


            return Ok;
        }

        //obtener Solicitudes de Opinion desde la BD
        private Boolean ObtenerInfoContratos()
        {
            Boolean Ok = false;
            if (this.ValidarEntradaDatos())
            {

                //recoger parametros de entrada de controles (pueden ser opcionales)
                int? intFolioOpinion = null;
                if (this.TextBoxFolioContrato.Text.Trim().Length > 0)
                    intFolioOpinion = Convert.ToInt32(this.TextBoxFolioContrato.Text);

                byte? intTipoContato = null;
                if (this.DropDownListTipoContrato.SelectedValue != "--")
                    intTipoContato = Convert.ToByte(this.DropDownListTipoContrato.SelectedValue);

                try
                {
                    if (Session[this.lblTableName.Text] != null)
                        ListContratosRegistrados = (List<ModeloNegocios.ContratoArrto>)Session[this.lblTableName.Text];
                    else
                        ListContratosRegistrados = new NG_ContratoArrto().ObtenerContratosArrtoRegistrados(Convert.ToInt32(this.DropDownListInstitucion.SelectedValue), intFolioOpinion, intTipoContato);
                    Ok = true;
                }
                catch (Exception ex)
                {

                    Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                }
            }
            return Ok;
        }

        //llenado de Combo Catalogo de BD Local. (nac, extr u otras fig)
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
                //msj al usuario

                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);

            }
            return Ok;
        }

        private bool ValidarEntradaDatos()
        {
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

            return true;
        }

        ////click en link columna, para selección de mostrar Acuse de registro de Contrato
        //protected void GridViewBusqContratos_RowCommand(object sender, GridViewCommandEventArgs e)
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

        //          //datos necesarios para exponer en AcuseContrato.aspx
        //           Session["DireccionInmueble"] = Server.HtmlDecode(selectedRow.Cells[3].Text);
        //           Session["FolioContrato"] =  Server.HtmlDecode(selectedRow.Cells[0].Text);; //Folio de Contrato
        //           //redireccionar
        //           Session["URLQueLllama"] = "~/Contrato/BusqContrato.aspx";
        //           Response.Redirect("~/Contrato/AcuseContrato.aspx");

        //           break;
        //    }
        //}

        protected void GridViewBusqContratos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                INDAABIN.DI.CONTRATOS.ModeloNegocios.ContratoArrto oInmueble = (INDAABIN.DI.CONTRATOS.ModeloNegocios.ContratoArrto)e.Row.DataItem;
                LinkButton linkAcuse = e.Row.FindControl("LinkButtonAcuseContrato") as LinkButton;

                if (linkAcuse != null)
                {
                    if (oInmueble.FolioContratoArrto != null)
                    {
                        linkAcuse.Attributes["onclick"] = "openCustomWindow('" + oInmueble.FolioContratoArrto.ToString() + "');";
                    }
                    else
                    {
                        if (linkAcuse != null) { linkAcuse.Visible = false; }
                    }
                }
            }
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {

            this.GridViewBusqContratos.DataSource = null;
            this.GridViewBusqContratos.DataBind();
            this.ButtonExportarExcel.Visible = false;

            if (PoblarRejillaBusqContratos(true))
            {
                if (ListContratosRegistrados != null && ListContratosRegistrados.Count > 0)
                {
                    this.GridViewBusqContratos.DataSource = ListContratosRegistrados;
                    this.GridViewBusqContratos.DataBind();
                    Session[this.lblTableName.Text] = ListContratosRegistrados;

                    if (this.GridViewBusqContratos.Rows.Count > 0)
                    {
                        if (this.TextBoxFolioContrato.Text.Length > 0)
                        {
                            //limpiar otros parametros que no se aplicaron

                            this.DropDownListTipoContrato.Items.FindByText("--").Selected = true;
                            Msj = "Se encontraron: [" + ListContratosRegistrados.Count.ToString() + "] contrato(s) de arrendamiento con el folio:  [" + this.TextBoxFolioContrato.Text + "].";
                        }
                        else
                        {
                            if (this.DropDownListTipoContrato.SelectedItem.Text == "--")//solo esta activo institucion
                                Msj = "Se encontraron: [" + ListContratosRegistrados.Count.ToString() + "] contrato(s) de arrendamiento registrados a la institución la que estas adscrito.";
                            else
                                Msj = "Se encontraron: [" + ListContratosRegistrados.Count.ToString() + "] contrato(s) de arrrendamiento, con los parámetros especificados.";
                        }
                        this.ButtonExportarExcel.Visible = true;
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);

                    }

                }
                //else //este else, en teoria no aplicaria porque el redireccionamiento lo hace el webform:  [ControladorEmisponOpinion.aspx]
                //{
                //    if (this.TextBoxFolioContrato.Text.Length > 0)
                //        Msj = "No existen contratos de arrendamiento registrados con el Folio: [" + this.TextBoxFolioContrato.Text + "].";
                //    else
                //    {
                //        if (this.DropDownListTipoContrato.SelectedItem.Text == "--")
                //            Msj = "No existen contratos de arrendamiento registrados a la institución a la que estás adscrito, da clic en Nueva, para registrar una";
                //        else
                //            Msj = "No existen contratos de arrendamiento registrados con los parámetros proporcionados";
                //    }

                //    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                //    MostrarMensajeJavaScript(Msj);

                //}
            }

        }

        protected void ButtonNueva_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InmuebleArrto/BusqMvtosContratosInmuebles.aspx");
        }

        //exporta a Excel con todo y formato, como se ve la rejilla
        private void ExportarXLS()
        {
            try
            {
                GridView gvExport = new GridView();
                gvExport.AutoGenerateColumns = false;
                DataControlFieldCollection gvdcfCollection = GridViewBusqContratos.Columns.CloneFields();
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
        }

        protected void ButtonRegistradosConExcepcion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Contrato/ExcepcionNormativaRegistroContrato.aspx");
        }

        protected void ButtonExportarExcel_Click(object sender, EventArgs e)
        {
            if (this.GridViewBusqContratos.Rows.Count > 0)
                ExportarXLS();
        }

        protected void GridViewBusqContratos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewBusqContratos.DataSource = Session[this.lblTableName.Text];
            this.GridViewBusqContratos.PageIndex = e.NewPageIndex;
            this.GridViewBusqContratos.DataBind();
        }

    }//clase
}