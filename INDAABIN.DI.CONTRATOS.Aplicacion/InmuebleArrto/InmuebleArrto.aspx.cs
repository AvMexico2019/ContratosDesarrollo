using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration; //para accear al WebConfig
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;//para uso del BUS
using INDAABIN.DI.ModeloNegocio;



namespace INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto
{
    public partial class InmuebleArrto : System.Web.UI.Page
    {
        private String Msj;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = string.Empty;
            this.LabelInfoEnc.Text = string.Empty;
            this.LabelInfo.ForeColor = System.Drawing.Color.Black;

            if (!IsPostBack)
            {
                //si ya caduco la session, redireccionar a la autenticacion
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                {
                    this.ButtonGuardar.Visible = false; //solo lectura
                }
                else //es otro rol: promovente o administrador de contratos
                {
                    if (Session["Msj"] != null)
                    {
                        Msj = Session["Msj"].ToString();
                        this.LabelInfoEnc.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                    }
                    else
                    {
                        Msj = "Consulta la dirección del inmueble de arrendamiento y da clic en Enviar";
                        this.LabelInfoEnc.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                    }
                }
            }
        }

        protected void ButtonGuardar_Click(object sender, EventArgs e)
        {
            if (Direccion.ValidarEntradaDatos())
            {
                if (this.GuardarInmueble())
                {
                    //MostrarMensajeJavaScript("Se registro el inmueble de arrendamiento, ahora puede utilizarlo para asociar con una solicitud de emisión de opinión o el registro de un contrato de arrandamiento");
                    Session["Msj"] = "Se registró el inmueble de arrendamiento, ahora puedes utilizarlo para asociar con una solicitud de emisión de opinión ó el registro de un contrato de arrandamiento";

                    //redirigir a la vista que lo invoco, despues de realizar el registro OK.
                    if (Session["URLQueLllama"] != null)
                        Response.Redirect(Session["URLQueLllama"].ToString());
                    else
                        //llevar al webform de emisión de opinión si no existe quien lo llamo, ya que en teoria es lo 1ro que se debe registrar para un nuevo inmueble
                        Response.Redirect("~/InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx");
                }
            }
        }

        protected void btnCargaDireccion_Click(object sender, EventArgs e)
        {
            //this.pnlDireccion.Visible = true;
            this.Direccion.CargaInicial();
            this.Direccion.ClavePais = this.hPais.Value;

            if (this.Direccion.ClavePais == "165")
            {
                this.Direccion.ClaveEstado = this.hEdo.Value;
                this.Direccion.ClaveMunicipio = this.hMun.Value;
                this.Direccion.ClaveColonia = this.hColonia.Value;
                this.Direccion.OtraColonia = this.hOtraColonia.Value;
                this.Direccion.CodigoPostal = this.hCP.Value;
            }
            else
            {
                this.Direccion.CodigoPostalExtranjero = this.hCodigoPostalExtranjero.Value;
                this.Direccion.EstadoExtranjero = this.hEstadoExtranjero.Value;
                this.Direccion.CiudadExtranjero = this.hCiudadExtranjero.Value;
                this.Direccion.MunicipioExtranjero = this.hMunicipioExtranjero.Value;
            }

            this.Direccion.ClaveTipoInmueble = this.hTipoInmueble.Value;
            this.Direccion.NombreVialidad = this.hNombreVialidad.Value;
            this.Direccion.DenominacionDeLaDireccion = this.hDenominacionDireccion.Value;
            this.Direccion.ClaveTipoVialidad = this.hTipoVialidad.Value;
            this.Direccion.NumeroExterior = this.hNumExterior.Value;
            this.Direccion.NumeroInterior = this.hNumInterior.Value;
            this.Direccion.GeoReferenciaLatitud = this.hGeoRefLatitud.Value;
            this.Direccion.GeoReferenciaLongitud = this.hGeoRefLongitud.Value;
            this.Direccion.IdInmueble = this.hIdInmueble.Value;
            this.Direccion.HabilitarCampos(true);
            this.Direccion.DefineVisibilidadPanelesPorPais();
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        private bool GuardarInmueble()
        {
            Boolean Ok = false;

            //Integrar objeto de negocio: [InmuebleArrto], para pasar a la capa de Negocio y DAL y realizar la operacion DML-SQL
            //recolectar datos de los controles y colocar en objeto de negocio

            //obtener controles del UserControl
            Control DropDownListPais = Direccion.FindControl("DropDownListPais");

            Control DropDownListTipoInmuble = Direccion.FindControl("DropDownListTipoInmueble");

            //de direccion nacional
            Control DropDownListEntFed = Direccion.FindControl("DropDownListEdo");
            Control DropDownListMpo = Direccion.FindControl("DropDownListMpo");
            Control DropDownListColonia = Direccion.FindControl("DropDownListColonia");
            Control TextBoxOtraColonia = Direccion.FindControl("TextBoxOtraColonia");
            Control TextBoxCP = Direccion.FindControl("TextBoxCP");

            //Comunes: aplicables a cualquier direccion: nacional o en el extranjero
            Control DropDownListTipoVialidad = Direccion.FindControl("DropDownListTipoVialidad");
            Control TextBoxNombreVialidad = Direccion.FindControl("TextBoxNombreVialidad");
            Control TextBoxNumExt = Direccion.FindControl("TextBoxNumExt");
            Control TextBoxNumInt = Direccion.FindControl("TextBoxNumInt");
            Control TextBoxLatitud = Direccion.FindControl("TextBoxLatitud");
            Control TextBoxLongitud = Direccion.FindControl("TextBoxLongitud");
            Control TextBoxNombreDireccion = Direccion.FindControl("TextBoxNombreDireccion");

            //de direccion en el extranjero              
            Control TextBoxEdoExtranjero = Direccion.FindControl("TextBoxEdoExtranjero");
            Control TextBoxMpoExtranjero = Direccion.FindControl("TextBoxMpoExtranjero");
            Control TextBoxCiudadExtranjero = Direccion.FindControl("TextBoxCiudadExtranjero");
            Control TextBoxCPExtranjero = Direccion.FindControl("TextBoxCPExtranjero");

            //creacion de objeto de direccion de inmueble arto.
            ModeloNegocios.InmuebleArrto objDireccionInmuebleArrto = new ModeloNegocios.InmuebleArrto();

            objDireccionInmuebleArrto.IdInmueble = 0; // System.Convert.ToInt32(Direccion.IdInmueble); // Se obliga a que el campo sea Cero, por el cambio de generacion de RIUF

            //poblado de datos al objeto
            objDireccionInmuebleArrto.IdInstitucion = Convert.ToInt32(((SSO)Session["Contexto"]).IdInstitucion);
            //int IdCargo = AdministradorCatalogos.ObtenerIdCargo(((SSO)Session["Contexto"]).Cargo);
            //objDireccionInmuebleArrto.IdCargo = IdCargo;
            objDireccionInmuebleArrto.NombreCargo = ((SSO)Session["Contexto"]).Cargo;
            objDireccionInmuebleArrto.IdUsuarioRegistro = Convert.ToInt32(((SSO)Session["Contexto"]).IdUsuario);
            objDireccionInmuebleArrto.IdPais = Convert.ToInt32(((DropDownList)DropDownListPais).SelectedValue);
            objDireccionInmuebleArrto.NombrePais = ((DropDownList)DropDownListPais).SelectedItem.Text;
            objDireccionInmuebleArrto.IdTipoInmueble = Convert.ToInt32(((DropDownList)DropDownListTipoInmuble).SelectedValue);

            if (((DropDownList)DropDownListPais).SelectedItem.Text == "México")
            {
                //aplicables a direccion Nacional
                objDireccionInmuebleArrto.IdEstado = Convert.ToInt32(((DropDownList)DropDownListEntFed).SelectedValue);
                objDireccionInmuebleArrto.NombreEstado = ((DropDownList)DropDownListEntFed).SelectedItem.Text;
                objDireccionInmuebleArrto.IdMunicipio = Convert.ToInt32(((DropDownList)DropDownListMpo).SelectedValue);
                objDireccionInmuebleArrto.NombreMunicipio = ((DropDownList)DropDownListMpo).SelectedItem.Text;

                if (((DropDownList)DropDownListColonia).SelectedItem.Text != "-Otra Colonia-")
                    objDireccionInmuebleArrto.IdLocalidadColonia = Convert.ToInt32(((DropDownList)DropDownListColonia).SelectedValue);
                else
                    objDireccionInmuebleArrto.OtraColonia = ((TextBox)TextBoxOtraColonia).Text.Trim().ToUpper();

                objDireccionInmuebleArrto.NombreLocalidadColonia = ((DropDownList)DropDownListColonia).SelectedItem.Text;
                objDireccionInmuebleArrto.CodigoPostal = ((TextBox)TextBoxCP).Text.Trim().ToUpper();
            }
            else
            {
                //aplicables a direccion en el Extranjero
                objDireccionInmuebleArrto.CodigoPostalExtranjero = ((TextBox)TextBoxCPExtranjero).Text.Trim().ToUpper();
                objDireccionInmuebleArrto.EstadoExtranjero = ((TextBox)TextBoxEdoExtranjero).Text.Trim().ToUpper();
                objDireccionInmuebleArrto.CiudadExtranjero = ((TextBox)TextBoxCiudadExtranjero).Text.Trim().ToUpper();
                objDireccionInmuebleArrto.MunicipioExtranjero = ((TextBox)TextBoxMpoExtranjero).Text.Trim().ToUpper();
            }

            //comunes a inmueble con direccion: Nacional y Extranjero
            objDireccionInmuebleArrto.IdTipoVialidad = Convert.ToInt32(((DropDownList)DropDownListTipoVialidad).SelectedValue);
            objDireccionInmuebleArrto.NombreTipoVialidad = ((DropDownList)DropDownListTipoVialidad).SelectedItem.Text.Trim().ToUpper();
            objDireccionInmuebleArrto.NombreVialidad = ((TextBox)TextBoxNombreVialidad).Text.Trim().ToUpper();
            objDireccionInmuebleArrto.NumExterior = ((TextBox)TextBoxNumExt).Text.Trim().ToUpper();

            //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
            if (!String.IsNullOrWhiteSpace(((TextBox)TextBoxNumInt).Text))
                objDireccionInmuebleArrto.NumInterior = ((TextBox)TextBoxNumInt).Text.Trim().ToUpper();

            //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
            if (!String.IsNullOrWhiteSpace(((TextBox)TextBoxLatitud).Text))
                //    objDireccionInmuebleArrto.GeoRefLatitud = null;
                //else
                objDireccionInmuebleArrto.GeoRefLatitud = Convert.ToDecimal(((TextBox)TextBoxLatitud).Text.Trim().ToUpper());

            //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
            if (!String.IsNullOrWhiteSpace(((TextBox)TextBoxLongitud).Text))
                //    objDireccionInmuebleArrto.GeoRefLongitud = null;
                //else
                objDireccionInmuebleArrto.GeoRefLongitud = Convert.ToDecimal(((TextBox)TextBoxLongitud).Text.Trim().ToUpper());

            //si hay valor en el ctrl, asignar a propiedad de objeto de negocio
            objDireccionInmuebleArrto.NombreInmueble = ((TextBox)TextBoxNombreDireccion).Text.Trim().ToUpper();

            int iAffect = 0;
            try
            {
                iAffect = new Negocio.NG_InmuebleArrto().InsertInmuebleArrto(objDireccionInmuebleArrto);
                if (iAffect > 0)
                    Ok = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("o es posible"))
                    {
                        Msj = ex.InnerException.Message;
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        return false;
                    }
                }

                Msj = "Ha ocurrido un error al guardar el inmueble. Contacta al área de sistemas.";
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

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            if (Session["URLQueLllama"] != null)
                Response.Redirect(Session["URLQueLllama"].ToString());
            else
                //si la session no existe, entonces redirigir a prinicpal, porque teoricamente no se sabe quien llamo a esta vista.
                Response.Redirect("~/Principal.aspx");
        }       
    }
}