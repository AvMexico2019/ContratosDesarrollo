using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//capa BO
using INDAABIN.DI.ModeloNegocio; //bus


namespace INDAABIN.DI.CONTRATOS.Aplicacion.Contrato
{


    //esta es una vista de paso que le permite al usuario seleccionar un contrato de una rejilla y redirigir a la vista correspondiente
    public partial class ContratoHistoricoXInstitucion : System.Web.UI.Page
    {
        string Msj;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                if (Session["ListContratoArrtoHistorico"] != null)
                    this.PoblarRejillaContratosHistorico();
                else
                    Response.Redirect("~/InmuebleArro/BusqMvtosXInmueble.aspx");
               
            }
        }


        private void PoblarRejillaContratosHistorico()
        {
            //bajar los datos de session
            List<ModeloNegocios.ContratoArrtoHistorico> ListContratoArrtoHistorico = (List<ModeloNegocios.ContratoArrtoHistorico>)Session["ListContratoArrtoHistorico"];
            
            this.GridViewContratosHistor.DataSource = ListContratoArrtoHistorico;
            this.GridViewContratosHistor.DataBind();

            if (this.GridViewContratosHistor.Rows.Count > 0)
            {
                Msj = "Se encontraron: [" + ListContratoArrtoHistorico.Count.ToString() + "] contrato(s) asociado(s) al Estado Municipio del inmueble seleccionado y a la institución en la que estás adscrito. Identifica el contrato para el que deseas realizar la Sustitución o Continuación de Arrendamiento"; ;
                this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                MostrarMensajeJavaScript("Se encontraron: [" + ListContratoArrtoHistorico.Count.ToString() + "] contrato(s) asociado(s) al Estado Municipio del inmueble seleccionado y a la institución en la que estás adscrito.");
               
            }
                    
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }


        //al selecciar el promovente un contrato
        protected void GridViewContratosHistor_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            switch (e.CommandName)
            {

                case "Seleccionar":

                    // get the row index stored in the CommandArgument property                     
                    int index = Convert.ToInt32(e.CommandArgument);
                    // get the GridViewRow where the command is raised
                    GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];
                    //poner en una session el ContratoHisto, seleccionado
                    Session["NumContratoHist"] = Server.HtmlDecode(selectedRow.Cells[0].Text);
                    selectedRow = null;

                    //redireccionar a la vista correspondiente, una vez seleccionado el contrato padre de la sustitucion o Continuacion
                    switch (Session["TipoArrto"].ToString())
                    {

                      //casos para emisión de opinión
                        case "Sustitución-Opinion":
                             Response.Redirect("~/EmisionOpinion/EmisionOpinion.aspx?TemaOpinion=2");
                            break;

                        case "Continuación-Opinion":
                            Response.Redirect("~/EmisionOpinion/EmisionOpinion.aspx?TemaOpinion=3");
                            break;

                      //casos para contrato de arrto: Continuacion o Sustitucion
                        case "Sustitución-ContratoArrto":
                            Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?TipoArrto=2");
                            break;

                        case "Continuación-ContratoArrto":
                            Response.Redirect("~/Contrato/ContratoArrtoRegistro.aspx?TipoArrto=3");
                            break;

                    }//switch
                    break;
            }
        }

        protected void ImageButtonExcel_Click(object sender, ImageClickEventArgs e)
        {

        }


    }//clase
}