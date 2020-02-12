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
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio; //para uso del BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto
{
   

    //es un user control que se expone como encabezado en vistas para las que se utiliza una direccion de un inmueble
    public partial class DireccionLectura : System.Web.UI.UserControl
    {
        public string DireccionInmuebleActual
        {
            get
            {
                return this.LabelDireccion.Text;
            }
        }

        private String Msj;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = String.Empty;

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                int IdInmuebleArrto = System.Convert.ToInt32(Request.QueryString["IdInmueble"].ToString() != null ? Request.QueryString["IdInmueble"].ToString() : "0");
                if (IdInmuebleArrto > 0)
                {
                    try
                    {
                        CargarObjetoDireccionEnCtrlsVista(IdInmuebleArrto);
                    }
                    catch (Exception ex)
                    {
                        Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                    }
                }
                else
                {
                    Msj = "Debes registrar ó seleccionar la dirección de un inmueble, desde la opción del menú principal: Emisión de Opinión ó Contratos y la opción Registrar...(ó reporte a Sistemas)";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    //Session["Msj"] = "Debes registrar ó seleccionar la dirección de un inmueble, desde la opción del menú principal: Emisión de Opinión ó Contratos y la opción Registrar...(ó reporte a Sistemas)";
                    //Response.Redirect("~/Msj.aspx"); 
                }                
            }
        }

        private void CargarObjetoDireccionEnCtrlsVista(int IdInmuebleArrendamiento)
        {
            ModeloNegocios.InmuebleArrto objInmuebleArrtoSeleccionado = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrendamiento);
            if (objInmuebleArrtoSeleccionado != null)
            {
                this.LabelIdInmuebleArrto.Text = objInmuebleArrtoSeleccionado.IdInmuebleArrendamiento.ToString();
                ////obtener nombres de Cpto de los IdFk
                //objInmuebleArrtoSeleccionado.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(objInmuebleArrtoSeleccionado.IdPais);

                //if (Util.QuitarAcentosTexto(objInmuebleArrtoSeleccionado.NombrePais.ToUpper()) == "MEXICO")
                //{
                //    //obtener nombre de la ent. fed
                //    objInmuebleArrtoSeleccionado.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(objInmuebleArrtoSeleccionado.IdEstado.Value);

                //    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                //    objInmuebleArrtoSeleccionado.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(objInmuebleArrtoSeleccionado.IdEstado.Value, objInmuebleArrtoSeleccionado.IdMunicipio.Value);

                //    objInmuebleArrtoSeleccionado.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(objInmuebleArrtoSeleccionado.IdTipoVialidad);

                //    //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                //    if (objInmuebleArrtoSeleccionado.IdLocalidadColonia != null)
                //        objInmuebleArrtoSeleccionado.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidadColonia(objInmuebleArrtoSeleccionado.IdMunicipio.Value, objInmuebleArrtoSeleccionado.IdLocalidadColonia.Value);
                //    else
                //        objInmuebleArrtoSeleccionado.NombreLocalidadColonia = objInmuebleArrtoSeleccionado.OtraColonia;
                //}
                //bajar a propiedades                
                LabelNombreInmueble.Text = objInmuebleArrtoSeleccionado.NombreInmueble;
                this.LabelPais.Text = objInmuebleArrtoSeleccionado.NombrePais;
                this.LabelDireccion.Text = objInmuebleArrtoSeleccionado.DireccionCompletaSinPais;
                this.LabelDireccion.Text = objInmuebleArrtoSeleccionado.DireccionCompleta;
                if (String.IsNullOrEmpty(objInmuebleArrtoSeleccionado.RIUFInmueble))
                    this.LabelRIUF.Text = "No disponible para el domicilio seleccionado";
                else
                    this.LabelRIUF.Text = objInmuebleArrtoSeleccionado.RIUFInmueble;
                objInmuebleArrtoSeleccionado = null;
            }
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void ButtonRegistrarInmueble_Click(object sender, EventArgs e)
        {
            Session["URLQueLllama"] = Request.RawUrl;
            Response.Redirect("~/InmuebleArrto/InmuebleArrto.aspx"); //redireccionar al registro de nueva solicitud
        }

    }
}