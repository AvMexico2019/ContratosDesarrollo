using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;//acceder al webconfig
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;
using System.Text;
//using INDAABIN.DI.CONTRATOS.ModeloNegocios.ContratoArrto;//interconexion con el BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Justipreciacion
{
    public partial class ConsultaJustipreciacion : System.Web.UI.Page
    {
        String Msj;
        String RolUsr;
        List<ModeloNegocios.SolicitudAvaluosExt> ListJustipreciacionesRegistrados;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    //poblar la rejilla, ya que eres administrador de contratos
                    this.PoblarRejillaJustipreciaciones();
                }
            }

        }

        protected void ButtonRegistrarSecuencial_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Justipreciacion/CargaJustipreciacion.aspx");
        }

        protected void ButtonConsultar_Click(object sender, EventArgs e)
        {
            this.PoblarRejillaJustipreciaciones(true);
        }

        //se va a poblar la rejilla.
        private Boolean PoblarRejillaJustipreciaciones(bool forceUpdate = true)
        {
            Boolean Ok = false;
            ListJustipreciacionesRegistrados = null;
            this.GridViewBusqJustipreciacion.DataSource = null;
            this.GridViewBusqJustipreciacion.DataBind();

            //obtener informacion de la DB
            if (forceUpdate)
                Session[this.lblTableName.Text] = null;

            if (this.ObtenerInfoJustipreciacion())
            {
                //si existe el objeto y tiene contenido
                if (ListJustipreciacionesRegistrados != null && ListJustipreciacionesRegistrados.Count > 0)
                {
                    this.GridViewBusqJustipreciacion.DataSource = ListJustipreciacionesRegistrados;
                    this.GridViewBusqJustipreciacion.DataBind();
                    Session[this.lblTableName.Text] = ListJustipreciacionesRegistrados;

                    if (this.GridViewBusqJustipreciacion.Rows.Count > 0)
                    {
                        Msj = "Se encontraron: [" + ListJustipreciacionesRegistrados.Count.ToString() + "] Justipreciacion(s) registrado(s).";
                        //this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        Ok = true;
                    }
                }
                else
                {
                    if (this.TextBoxSecuencial.Text.Length > 0)
                    {
                        Msj = "No existen justipreciaciones registrados con el secuencial: [" + this.TextBoxSecuencial.Text + "].";
      
                    }

                    //RCA 09/02/2018
                    //modificacion para que muestre el mensaje por si no encuentra ningun secuencial
                    if(this.TextBoxGenerico.Text.Length > 0)
                    {
                        Msj = "No existen justipreciaciones registrados con el generico: ["+ this.TextBoxGenerico.Text +"].";
                    }

                    MostrarMensajeJavaScript(Msj);

                    Ok = true;

                }
            }

            return Ok;
        }

        //obtener informacion de la DB
        private Boolean ObtenerInfoJustipreciacion()
        {
            Boolean Ok = false;
           
                //recoge parametros de entrada de controles (pueden ser opcionales)
                string Secuencial = null;
                if (this.TextBoxSecuencial.Text.Trim().Length > 0)
                    Secuencial = this.TextBoxSecuencial.Text.Trim();

                string Generico = null;
                if (this.TextBoxGenerico.Text.Trim().Length > 0)
                    Generico = this.TextBoxGenerico.Text.Trim();

                try
                {
                    if (Session[this.lblTableName.Text] != null)
                        ListJustipreciacionesRegistrados = (List<ModeloNegocios.SolicitudAvaluosExt>)Session[this.lblTableName.Text];
                    else
                    {
                        Filtro filtro = new Filtro
                        {
                            NoGenerico = string.Empty,
                            NoSecuencial = string.Empty
                        };

                        if (!string.IsNullOrEmpty(TextBoxSecuencial.Text))
                        {
                            filtro.NoSecuencial = TextBoxSecuencial.Text.Trim();
                        }
                        else if (!string.IsNullOrEmpty(TextBoxGenerico.Text))
                        {
                            filtro.NoGenerico = TextBoxGenerico.Text.Trim();
                        }

                        ListJustipreciacionesRegistrados = new NegocioJustipreciacionExt()
                            .ObtenerJustipreciacionesRegistradas(filtro);
                    }

                    Ok = true;


                }
                catch (Exception ex)
                {
                    Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    MostrarMensajeJavaScript(Msj);
                }
            
            return Ok;
        }

        //validar si se introdujo el secuencial o generico
       

        private void MostrarMensajeJavaScript(string mensaje)
        {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAlerta", "alert(\"" + mensaje + "\");", true);
        }

        protected void GridViewBusqJustipreciacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                INDAABIN.DI.CONTRATOS.ModeloNegocios.SolicitudAvaluosExt oJustipreciacion = (INDAABIN.DI.CONTRATOS.ModeloNegocios.SolicitudAvaluosExt)e.Row.DataItem;
                LinkButton link = e.Row.FindControl("linkPdfJustipreciacion") as LinkButton;

                if (link != null)
                {
                    link.Attributes["onclick"] = "openCustomWindow('" + oJustipreciacion.RutaDocumento + "');";
                }

            }
        }

        protected void GridViewBusqJustipreciacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridViewBusqJustipreciacion.DataSource = Session[this.lblTableName.Text];
            this.GridViewBusqJustipreciacion.PageIndex = e.NewPageIndex;
            this.GridViewBusqJustipreciacion.DataBind();
        }
    }
}