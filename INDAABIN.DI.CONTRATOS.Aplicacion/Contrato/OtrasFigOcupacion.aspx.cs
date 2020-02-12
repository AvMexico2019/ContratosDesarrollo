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
using System.Text.RegularExpressions;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;//interconexion con el BUS


namespace INDAABIN.DI.CONTRATOS.Aplicacion.Contrato
{
    public partial class OtrasFigOcupacion : System.Web.UI.Page
    {

        private String Msj;
        private string strFolioContrato;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.LabelInfo.Text = String.Empty;
                this.LabelInfoEnviar.Text = String.Empty;

                if (!IsPostBack)
                {
                    if (Session["Contexto"] == null)
                        Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                    String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                    //determinar el tipo de usuario autenticado
                    if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                        this.ButtonEnviar.Visible = false; //no puede registrar Solicitudes

                    // Se recuperan valores del registro seleccionado en la consulta de BusqMvtosContratosInmuebles
                    this.lblIdEmisionOpinion.Value = Request.QueryString["IdEmision"].ToString();
                    this.lblIdContrato.Value = Request.QueryString["IdContrato"].ToString();
                    this.lblIdInmuebleArrendamiento.Value = Request.QueryString["IdInmueble"].ToString();

                    this.LimpiarSessionesGeneradas();

                    if (this.IdentificarTipoContratoVsQuerySting())
                    {
                        //cargar catgalogos y autoseleccionar
                        if (this.PoblarDropDownTipoOcupacion())
                        {
                            if (this.PoblarDropDownTipoUsoInmueble())
                            {
                                if (this.PoblarDropDownTipoMoneda())
                                {
                                    if (this.ObtenerValorImpuesto())
                                    {
                                        //Recupera RIUF, si existe
                                        this.RecuperaRIUFInmueble();

                                        //Se llena la informacion del Capturista   
                                        this.LLenaCamposCapturista();

                                        Msj = "Proporciona los valores para cada concepto y al final de la captura haz clic en enviar para que el sistema registre la información y te proporcione un acuse con un número de Folio del Contrato de Arrendamiento de Otras Figuras de Ocupación";
                                        this.LabelInfo.Text = "<div class='alert alert-info'><strong>Sugerencia: </strong> " + Msj + "</div>";
                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario    
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //el valor del query string no es valido, devolver a la pagina origen de seleccion de tipo de emision
                        //Response.Redirect("ControladorEmisionOpinion.aspx");
                        Response.Redirect("~/InmuebleArrto/BusqMvtosContratosInmuebles.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Captura de error al cargar otras figuras de ocupación.";
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

        private void RecuperaRIUFInmueble()
        {
            try
            {
                int IdInmuebleArrto = System.Convert.ToInt32(this.lblIdInmuebleArrendamiento.Value);
                ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrto);

                if (objInmuebleArrto.RIUFInmueble.Trim() == "")
                {
                    this.PanelBuscaDomicilios.Visible = true;
                    this.TextBoxRIUF.Text = "";
                    Msj = "No se ha encontrado un RIUF asociado al domicilio para el inmueble actual. Debera seleccionar un RIUF existente o generar uno nuevo.";
                    this.LabelInfoInmuebleArrendamiento.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                    //MostrarMensajeJavaScript(Msj);
                }
                else
                {
                    this.LabelInfoInmuebleArrendamiento.Text = "";
                    this.TextBoxRIUF.Text = objInmuebleArrto.RIUFInmueble;
                    this.PanelBuscaDomicilios.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el RIUF asociado el domicilio. Contacta al área de sistemas.";
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
        private void LLenaCamposCapturista()
        {
            this.TextBoxNombreCapturista.Text = this.ctrlUsuarioInfo.NombreUsuario;
            this.TextBoxApPatCapturista.Text = this.ctrlUsuarioInfo.APaternoUsuario;
            this.TextBoxApMatCapturista.Text = this.ctrlUsuarioInfo.AMaternoUsuario;
            this.TextBoxCargoCapturista.Text = this.ctrlUsuarioInfo.CargoUsuario;
            this.TextBoxEmailCapturista.Text = this.ctrlUsuarioInfo.CorreoUsuario;

            this.TextBoxNombreCapturista.Enabled = false;
            this.TextBoxApPatCapturista.Enabled = false;
            this.TextBoxApMatCapturista.Enabled = false;
            this.TextBoxCargoCapturista.Enabled = false;
            this.TextBoxEmailCapturista.Enabled = false;
        }

        private Boolean IdentificarTipoContratoVsQuerySting()
        {
            Boolean Ok = false;
            //recepcion del queryString en otro pagina, con el nombre de la variable
            string sName = Request.QueryString["TipoArrto"];
            if (sName != null)
            {
                switch (sName)
                {
                    case "1":
                        this.LabelTipificacionArrto.Text = "Nacional";
                        //strTipoArrendamiento = "Nuevo";
                        //etiquetas de titulos
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "REGISTRO DE UN NUEVO CONTRATO DE OTRAS FIGURAS DE CONTRATACIÓN NACIONAL";
                        Ok = true;
                        break;

                    case "2": //nuevo- extranjero
                        this.LabelTipificacionArrto.Text = "Extranjero";
                        //strTipoArrendamiento = "Nuevo";
                        //etiquetas de titulos
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "REGISTRO DE UN NUEVO CONTRATO DE OTRAS FIGURAS DE CONTRATACIÓN EN EL EXTRANJERO";
                        Ok = true;
                        break;
                }
            }
            return Ok;
        }

        private Boolean ObtenerValorImpuesto()
        {
            try
            {
                this.TextBoxPtjeImpuesto.Text = new NG_Catalogos().ObtenerValorCatParametro("Impuesto IVA Nacional");
                return true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el valor del IVA. Contacta al área de sistemas.";
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
            return false;
        }

        //llenado de Combo Catalogo de BD Local. 
        private Boolean PoblarDropDownTipoOcupacion()
        {
            Boolean Ok = false;
            List<TipoOcupacion> ListTipoOcupacion;
            try
            {
                ListTipoOcupacion = new NG_Catalogos().ObtenerTipoOcupacion();
                this.DropDownListTipoOcupacion.DataSource = ListTipoOcupacion;
                this.DropDownListTipoOcupacion.DataValueField = "IdTipoOcupacion";
                this.DropDownListTipoOcupacion.DataTextField = "DescripcionTipoOcupacion";
                this.DropDownListTipoOcupacion.DataBind();
                //agregar un elemento para reprsentar que no se ha seleccionado un valor
                this.DropDownListTipoOcupacion.Items.Add("--");
                this.DropDownListTipoOcupacion.Items.FindByText("--").Selected = true;

                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de tipos de ocupación. Contacta al área de sistemas.";
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

        //poblar lista del Bus
        //DEportivo, educacion, infraestucutura.....
        private Boolean PoblarDropDownTipoUsoInmueble()
        {
            Boolean Ok = false;
            this.DropDownListTipoUsoInmueble.DataTextField = "Descripcion";
            DropDownListTipoUsoInmueble.DataValueField = "IdValue";

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                // MZT cambio por uso generico
                //DropDownListTipoUsoInmueble.DataSource = AdministradorCatalogos.ObtenerCatalogoUsoInmueble();
                DropDownListTipoUsoInmueble.DataSource = new NG().LlenaCombo("ObtenerUsogenerico")
                    .OrderBy(x => x.Descripcion)
                    .ToList();
                // MZT cambio por uso generico

                DropDownListTipoUsoInmueble.DataBind();
                //agregar un elemento para representar a todos
                DropDownListTipoUsoInmueble.Items.Insert(0, "--");
                this.DropDownListTipoUsoInmueble.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de tipos de uso del inmueble. Contacta al área de sistemas.";
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

        //poblar lista del Bus
        private Boolean PoblarDropDownTipoMoneda()
        {
            Boolean Ok = false;
            this.DropDownListTipoMonedaPago.DataTextField = "Descripcion";
            DropDownListTipoMonedaPago.DataValueField = "IdCatalogo";

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListTipoMonedaPago.DataSource = AdministradorCatalogos.ObtenerCatalogoMoneda();
                DropDownListTipoMonedaPago.DataBind();
                //auteseleccionar pesos mx
                if (this.DropDownListTipoMonedaPago.Items.Contains(this.DropDownListTipoMonedaPago.Items.FindByText("Peso mexicano")) == true)
                {
                    if (LabelTipificacionArrto.Text == "Nacional")
                    {
                        this.DropDownListTipoMonedaPago.Items.FindByText("Peso mexicano").Selected = true;
                        //this.DropDownListTipoMonedaPago.Enabled = false; //deshabilitar la seleccion
                    }
                    else //es otro pais en el extranjero
                    {
                        //this.DropDownListTipoMonedaPago.Enabled = true; //habilitar la seleccion
                        //agregar un elemento para representar a todos
                        DropDownListTipoMonedaPago.Items.Add("--");
                        this.DropDownListTipoMonedaPago.Items.FindByText("--").Selected = true;
                    }

                }
                Ok = true;

            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de tipos de moneda. Contacta al área de sistemas.";
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

        private void LimpiarSessionesGeneradas()
        {
            //limpieza de sessiones, de otras paginas
            Session["intFolioConceptoResp"] = null;
            Session["ListAplicacionConcepto"] = null;
            Session["Msj"] = null;
            // Session["ListInmuebleArrtoRegistadosXInstitucion"] = null;
            Session["URLQueLllama"] = null;
            //Session["IdInmuebleArrto"] = null;
        }

        private Boolean ObtenerInformacionInmueble()
        {
            Boolean Ok = false;
            int intFolio;
            bool ConversionOK; //esta nos dice si es un número válido

            ConversionOK = int.TryParse(Session["intFolioConceptoResp"].ToString(), out intFolio);

            if (ConversionOK)
            {
                this.LabelInfo.Text = "";
                Ok = true;
            }
            return Ok;
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {
            if (this.ValidaEntradaDatos()) //validar entrada de datos requerida
            {
                if (ValidarCtrlEntradaImportes())
                    if (this.InsertContratoOtrasFigOcupacion())
                    {
                        string mensaje = "Otra figura de ocupación ha sido registrada con éxito.";
                        this.LabelInfo.Text += "<div class='alert alert-success'><strong> ¡Felicidades! </strong></br>" + mensaje + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                        this.pnlControles.Enabled = false;
                        this.ButtonEnviar.Enabled = false;
                        this.ButtonCancelar.Text = "Regresar";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAlerta", "alert(\"" + mensaje + "\");", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAcuse", "window.open('AcuseOtraFigura.aspx?IdFolio=" + this.strFolioContrato + "&isInsert=true', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, titlebar = no, width = 1024, height = 650', 'true');", true);
                    }
            }
        }

        private Boolean ValidaEntradaDatos()
        {
            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacion
            string Msj;

            if (DropDownListTipoOcupacion.SelectedItem.Text == "--") //
            {
                Msj = "Debes seleccionar un tipo de ocupación, o selecciona Otros y a continuación capturala.";
                this.MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListTipoOcupacion.Focus();
                return false;
            }

          

            if (DropDownListTipoUsoInmueble.SelectedItem.Text == "--") //
            {
              
            }

            //validacion de seccion de datos de contratacion
            if (TextBoxFechaIOcupacion.Text.Length > 0)
            {
                if (Util.IsDate(TextBoxFechaIOcupacion.Text) == false)
                {
                    Msj = "Debes proporcionar una fecha inicio de ocupación, valida en el formato: (dd/mm/aaaa)";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.TextBoxFechaIOcupacion.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar una fecha inicio de ocupación, valida en el formato: (dd/mm/aaaa)";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxFechaIOcupacion.Focus();
                return false;
            }

            //validacion de seccion de datos de contratacion
            if (TextBoxFechaFOcupacion.Text.Length > 0)
            {
                if (Util.IsDate(TextBoxFechaFOcupacion.Text) == false)
                {
                    Msj = "Debes proporcionar una fecha fin de ocupación, valida en el formato: (dd/mm/aaaa)";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.TextBoxFechaFOcupacion.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar una fecha fin de ocupación, valida en el formato: (dd/mm/aaaa)";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxFechaFOcupacion.Focus();
                return false;
            }


            //si ambas fechas de busq se proporcionaron, entonces validar rango valido
            if ((TextBoxFechaIOcupacion.Text.Length > 0) && (TextBoxFechaFOcupacion.Text.Length > 0))
            {

                //la fecha inicial menor que la final y la final mayor que la inicial
                if (Convert.ToDateTime(this.TextBoxFechaIOcupacion.Text) >= Convert.ToDateTime(this.TextBoxFechaFOcupacion.Text))
                {
                    Msj = "Rango de fecha inválido, la fecha inicio debe ser menor a la fecha fin en periodo de ocupación del arrendamiento";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.TextBoxFechaIOcupacion.Focus();
                    return false;
                }
            }

            //area ocupada de arrto.
            if (this.TextBoxAreaOcupadaM2.Text.Trim().Length > 0)
            {
                if (Util.IsNumeric(this.TextBoxAreaOcupadaM2.Text) == false)
                {
                    Msj = "Debes proporcionar un area de ocupación del arrendamiento correspondiente a los  metros cuadrados como un número decimal.";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxAreaOcupadaM2.Focus();
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(this.TextBoxAreaOcupadaM2.Text) < 5)
                    {
                        Msj = "Debes proporcionar la superficie ocupada en m2 del área de arrendamiento";
                        MostrarMensajeJavaScript(Msj);

                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                        this.TextBoxAreaOcupadaM2.Focus();
                        return false;
                    }
                }
            }
            else
            {
                Msj = "Debes proporcionar un área de ocupación del arrendamiento correspondiente a los metros cuadrados.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxAreaOcupadaM2.Focus();
                return false;
            }

            // Nota: las validaciones de importes de renta, se validan  en la funcion ValidarCrlEntradaImportes()


            // Ptje de Impuesto
            if (this.TextBoxPtjeImpuesto.Text.Trim().Length > 0)
            {
                if (Util.IsNumeric(this.TextBoxPtjeImpuesto.Text) == false)
                {
                    Msj = "Debes proporcionar el porcentaje de impuesto como un número decimal.";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.TextBoxPtjeImpuesto.Focus();
                    return false;
                }

            }
            else
            {
                Msj = "Debes proporcionar el porcentaje de impuesto";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxPtjeImpuesto.Focus();
                return false;
            }

            //para el casos de contratos en el extranjero se habilita la lista para seleccionar una moneda
            if (this.DropDownListTipoMonedaPago.SelectedItem.Text == "--")
            {

                Msj = "Selecciona la moneda con el que se realiza el pago de la renta";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListTipoMonedaPago.Focus();
                return false;


            }


            //datos de referencias de personas

            if (this.TextBoxNombreTitularOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el nombre del titular del OIC para tu Institución.";
                MostrarMensajeJavaScript(Msj);


                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxNombreTitularOIC.Focus();
                return false;
            }

            if (this.TextBoxApPatOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el 1er apellido  titular del OIC para tu Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxApPatOIC.Focus();
                return false;
            }

            if (this.TextBoxNombreCargoOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el cargo del titular del OIC para tu Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxNombreCargoOIC.Focus();
                return false;
            }

            if (this.TextBoxEmailOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el correo electrónico del titular del OIC para tu Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailOIC.Focus();
                return false;
            }

            if (Regex.IsMatch(this.TextBoxEmailOIC.Text, @"[\w-]+@([\w-]+\.)+[\w-]+") == false)
            {
                Msj = "Debes proporcionar un correo electrónico válido del titular del OIC para tu Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailOIC.Focus();
                return false;
            }


            //Datos del Capturista
            if (this.TextBoxNombreCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu nombre";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxNombreCapturista.Focus();
                return false;
            }

            if (this.TextBoxApPatCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el primero de tus apellidos";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxApPatCapturista.Focus();
                return false;
            }

            if (this.TextBoxCargoCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu cargo";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxCargoCapturista.Focus();
                return false;
            }

            if (this.TextBoxEmailCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu correo electrónico institucional";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailCapturista.Focus();
                return false;
            }



            if (Regex.IsMatch(this.TextBoxEmailCapturista.Text, @"[\w-]+@([\w-]+\.)+[\w-]+") == false)
            {
                Msj = "Debes proporcionar tu correo electrónico, como uno válido.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailCapturista.Focus();
                return false;
            }

            Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
            return true;
        }

        private Boolean ValidarCtrlEntradaImportes()
        {
            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacion

            string Msj;

            //monto pago mensual de renta
            if (this.TextBoxMontoPagoMes.Text.Trim().Length > 0)
            {
                if (Util.IsNumeric(this.TextBoxMontoPagoMes.Text) == false)
                {
                    Msj = "Debes proporcionar el importe de pago mensual del arrendamiento como un número decimal.";
                    this.MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxMontoPagoMes.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar el importe de pago mensual del arrendamiento";
                this.MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxMontoPagoMes.Focus();
                return false;
            }

            //monto pago mensual por mantemiento
            if (this.TextBoxCuotaMantenimiento.Text.Trim().Length > 0)
            {
                if (Util.IsNumeric(this.TextBoxCuotaMantenimiento.Text) == false)
                {
                    Msj = "Debes proporcionar el importe de pago por mantenimiento del arrendamiento como un número decimal.";
                    this.MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxCuotaMantenimiento.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar el importe de pago mensual por mantenimiento del arrendamiento";
                this.MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxCuotaMantenimiento.Focus();
                return false;
            }

            //monto pago mensual por estacionamiento
            if (this.TextBoxMontoPagoEstacionamiento.Text.Trim().Length > 0)
            {
                if (Util.IsNumeric(this.TextBoxMontoPagoEstacionamiento.Text) == false)
                {

                    Msj = "Debes proporcionar el importe de pago por estacionamiento del arrendamiento como un número decimal.";
                    this.MostrarMensajeJavaScript(this.LabelInfo.Text);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxMontoPagoEstacionamiento.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar el importe de pago por estacionamiento del arrendamiento, si no aplica especifique como: 0.00";
                this.MostrarMensajeJavaScript(this.LabelInfo.Text);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxMontoPagoEstacionamiento.Focus();
                return false;
            }
            //Page.MaintainScrollPositionOnPostBack = false; // mentener la posicion del scroll del navegador, porque si paso la valiadacion
            return true;
        }

        private Boolean InsertContratoOtrasFigOcupacion()
        {
            Boolean Ok = false;
            try
            {
                //Determinar tipo de Contrato: Nac, Extranjero ó Otras Fig. Ocupacion
                Byte IdTipoContrato = 0;
                Control LabelPais = this.ctrlDireccionLectura.FindControl("LabelPais");
                string NombrePais = ((Label)LabelPais).Text;
                if (NombrePais == "MÉXICO")
                    IdTipoContrato = 3; //otras figuras Nacional
                else
                    IdTipoContrato = 4; //otras figuras en el Extranjero

                string strOtroTipoOcupacion = null;
                if (this.TextBoxOtrosTipoOcupacion.Text.Length > 0)
                    strOtroTipoOcupacion = this.TextBoxOtrosTipoOcupacion.Text.Trim();

                int? iTipoUsoInmueble = null;
                if (this.DropDownListTipoUsoInmueble.SelectedItem.Text != "")
                    iTipoUsoInmueble = Convert.ToInt32(this.DropDownListTipoUsoInmueble.SelectedValue);

                string strOtroUsoInmueble = null;

                if (this.DropDownListTipoUsoEspecificoInmueble.SelectedValue == "--")
                {
                    strOtroUsoInmueble = " SIN INFORMACIÓN";
                }
                else
                {
                    strOtroUsoInmueble = DropDownListTipoUsoEspecificoInmueble.SelectedItem.Text;
                }

                //obtener controles del UserControl, la direccion completa: se usa para generar el Sello digital
                Control LabelDireccion = this.ctrlDireccionLectura.FindControl("LabelDireccion");
                string DireccionInmueble = ((Label)LabelDireccion).Text;

                //idInmueble para el que se realiza la emisión de opinión
                Control LabelIdInmuebleArrto = this.ctrlDireccionLectura.FindControl("LabelIdInmuebleArrto");
                int IdInmuebleArrendamiento = Convert.ToInt32(((Label)LabelIdInmuebleArrto).Text);

                Control LabelInstitucion = this.ctrlUsuarioInfo.FindControl("LabelInstitucion");
                string InstitucionUsr = ((Label)LabelInstitucion).Text;

                // estructura de seguridad
                bool CuentaConDictamenSeguridad = false;
                DateTime? fechaDictamen = null;

                if (drpCuentaConDictamenSeguridad.SelectedValue == "1")
                {
                    CuentaConDictamenSeguridad = true;
                    fechaDictamen = DateTime.Parse(txtFechaDictamen.Text);
                }

                //crear sello digital
                string CadenaOriginal = "||Invocante:[" + InstitucionUsr + "] || Inmueble:[" + DireccionInmueble + "]||Fecha:[" + DateTime.Today.ToLongDateString() + "]||" + Guid.NewGuid().ToString();
                //generar el sello diigital, con la llave de ciframiento
                string SelloDigital = UtilContratosArrto.Encrypt(CadenaOriginal, true, "ContratoArrtoNacional");

                ModeloNegocios.ContratoArrto objContratoArrto = new ModeloNegocios.ContratoArrto();
                //poblar propiedades
                objContratoArrto.Fk_IdTipoContrato = 3; // Otras Fig. de Ocupacion
                objContratoArrto.Fk_IdTipoOcupacion = Convert.ToInt32(this.DropDownListTipoOcupacion.SelectedValue);
                objContratoArrto.OtroTipoOcupacion = strOtroTipoOcupacion; //este cpto solo aplica a otras Fig. de ocupacion

                objContratoArrto.Fk_IdInmuebleArrendamiento = IdInmuebleArrendamiento; // Convert.ToInt32(Session["IdInmuebleArrto"].ToString());
                objContratoArrto.Fk_IdTipoUsoInm = iTipoUsoInmueble.Value;
                objContratoArrto.OtroUsoInmueble = strOtroUsoInmueble;

                objContratoArrto.Fk_IdTipoMoneda = Convert.ToInt32(this.DropDownListTipoMonedaPago.SelectedValue);
                objContratoArrto.Fk_IdInstitucion = ((SSO)Session["Contexto"]).IdInstitucion.Value;
                objContratoArrto.NombreInstitucion = ((SSO)Session["Contexto"]).NombreInstitucion;
                objContratoArrto.FechaInicioOcupacion = Convert.ToDateTime(this.TextBoxFechaIOcupacion.Text.Trim());
                objContratoArrto.FechaFinOcupacion = Convert.ToDateTime(this.TextBoxFechaFOcupacion.Text.Trim());
                objContratoArrto.AreaOcupadaM2 = Convert.ToDecimal(this.TextBoxAreaOcupadaM2.Text);
                objContratoArrto.MontoPagoMensual = Convert.ToDecimal(this.TextBoxMontoPagoMes.Text);
                objContratoArrto.MontoPagoPorCajonesEstacionamiento = Convert.ToDecimal(this.TextBoxMontoPagoEstacionamiento.Text);
                objContratoArrto.CuotaMantenimiento = Convert.ToDecimal(this.TextBoxCuotaMantenimiento.Text);
                objContratoArrto.PtjeImpuesto = Convert.ToDecimal(this.TextBoxPtjeImpuesto.Text);

                // estructura de seguridad
                objContratoArrto.CuentaConDictamen = CuentaConDictamenSeguridad;
                objContratoArrto.FechaDictamen = fechaDictamen;

                //nulificar si no hay valores
                if (string.IsNullOrEmpty(this.TextBoxRIUF.Text))
                    objContratoArrto.RIUF = null;
                else
                    objContratoArrto.RIUF = this.TextBoxRIUF.Text.Trim();

                //nulificar si no hay valores
                if (string.IsNullOrEmpty(this.TextBoxObs.Text))
                    objContratoArrto.Observaciones = null;
                else
                    objContratoArrto.Observaciones = this.TextBoxObs.Text.Trim();

                objContratoArrto.PropietarioInmueble = "";
                objContratoArrto.FuncionarioResponsable = "";

                objContratoArrto.Fk_IdUsuarioRegistro = Convert.ToInt32(((SSO)Session["Contexto"]).IdUsuario); //el usuario del SSO;
                objContratoArrto.CargoUsuarioRegistro = ((SSO)Session["Contexto"]).Cargo; //SSO (nombre del cargo)


               
                //objetos de Persona Referencia

                //Responsable de Ocupacion
                objContratoArrto.PersonaReferenciaResponsableOcupacion = new PersonaReferencia
                {
                    NombreCargo = this.TextBoxNombreRespOcupacion.Text.Trim(),
                    Nombre = this.TextBoxNombreRespOcupacion.Text.Trim(),
                    ApellidoPaterno = this.TextBoxApPatRespOcupacion.Text.Trim(),
                    ApellidoMaterno = this.TextBoxApMatRespOcupacion.Text.Trim(),
                    Email = this.TextBoxEmailRespOcupacion.Text.Trim()
                };

                //titular del OIC
                objContratoArrto.PersonaReferenciaTitularOIC = new PersonaReferencia
                {
                    NombreCargo = this.TextBoxNombreCargoOIC.Text.Trim(),
                    Nombre = this.TextBoxNombreTitularOIC.Text.Trim(),
                    ApellidoPaterno = this.TextBoxApPatOIC.Text.Trim(),
                    ApellidoMaterno = this.TextBoxApMatOIC.Text.Trim(),
                    Email = this.TextBoxEmailOIC.Text.Trim()
                };

                //Capturista
                objContratoArrto.PersonaReferenciaCapturista = new PersonaReferencia
                {
                    NombreCargo = this.TextBoxCargoCapturista.Text,
                    Nombre = this.TextBoxNombreCapturista.Text,
                    ApellidoPaterno = this.TextBoxApPatCapturista.Text,
                    ApellidoMaterno = this.TextBoxApMatCapturista.Text,
                    Email = this.TextBoxEmailCapturista.Text
                };

                objContratoArrto.CadenaOriginal = CadenaOriginal;
                objContratoArrto.SelloDigital = SelloDigital;

                //Se inserta el Contrato en el Sistema
                int iFolioContrato = new NG_ContratoArrto().InsertContratoArrtoOtrasFigOcupacion(objContratoArrto);
                if (iFolioContrato > 0)
                {
                    // Si se genera el contrato, se genera el RIUF de ser necesario
                    if (this.CheckBoxGenerarRIUF.Checked)
                    {
                        string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;
                        int iIdInmuebleArrto = System.Convert.ToInt32(this.lblIdInmuebleArrendamiento.Value);
                        ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(iIdInmuebleArrto);
                        ModeloNegocios.Inmueble objInmueble = new ModeloNegocios.Inmueble();

                        objInmueble.PaisDescripcion = objInmuebleArrto.NombrePais;
                        objInmueble.IdPais = objInmuebleArrto.IdPais;
                        objInmueble.IdTipoInmueble = objInmuebleArrto.IdTipoInmueble;
                        objInmueble.IdEstado = objInmuebleArrto.IdEstado;
                        objInmueble.IdMunicipio = objInmuebleArrto.IdMunicipio;
                        objInmueble.IdLocalidad = objInmuebleArrto.IdLocalidadColonia;
                        objInmueble.OtraColonia = objInmuebleArrto.OtraColonia;
                        objInmueble.IdTipoVialidad = objInmuebleArrto.IdTipoVialidad;
                        objInmueble.NombreVialidad = objInmuebleArrto.NombreVialidad;
                        objInmueble.NumExterior = objInmuebleArrto.NumExterior;
                        objInmueble.NumInterior = objInmuebleArrto.NumInterior;
                        objInmueble.CodigoPostal = objInmuebleArrto.CodigoPostal;
                        objInmueble.GeoRefLatitud = objInmuebleArrto.GeoRefLatitud;
                        objInmueble.GeoRefLongitud = objInmuebleArrto.GeoRefLongitud;
                        objInmueble.NombreInmueble = objInmuebleArrto.NombreInmueble;
                        objInmueble.CodigoPostalExtranjero = objInmuebleArrto.CodigoPostalExtranjero;
                        objInmueble.EstadoExtranjero = objInmuebleArrto.EstadoExtranjero;
                        objInmueble.CiudadExtranjero = objInmuebleArrto.CiudadExtranjero;
                        objInmueble.MunicipioExtranjero = objInmuebleArrto.MunicipioExtranjero;
                        objInmueble.GeneraRIUF = 1;
                        objInmueble.IdUsuarioRegistro = Convert.ToInt32(((SSO)Session["Contexto"]).IdUsuario);
                        objInmueble.CargoUsuarioRegistro = ((SSO)Session["Contexto"]).Cargo;
                        objInmueble.RIUF = new RIUF();
                        objInmueble.RIUF.RIUF1 = "0";
                        int EsOtraFiguraOcupacion = 1; //0 Se trata de Otra Figura de Ocupacion 1 Es un contrato de arrendamiento normal
                        int iIdInmueble = new Negocio.NG_Inmueble().InsertInmueble(strConnectionString, objInmueble, EsOtraFiguraOcupacion);
                        if (iIdInmueble > 0)
                        {
                            string sRIUF = objInmueble.RIUF.RIUF1;
                            int iInstitucion = ((SSO)Session["Contexto"]).IdInstitucion.Value;

                            int InmmuebleActualizado;
                            InmmuebleActualizado = new Negocio.NG_InmuebleArrto().UpdateIdInmuebleByIdInmuebleArrendamiento(iIdInmueble, iIdInmuebleArrto);
                            int ContratoActualizado;
                            ContratoActualizado = new Negocio.NG_ContratoArrto().UpdateRIUFByFolioContrato(sRIUF, iFolioContrato, iInstitucion);

                            this.TextBoxRIUF.Text = sRIUF;
                        }
                    }
                    this.strFolioContrato = iFolioContrato.ToString();
                    objContratoArrto = null;

                    //RCA 13/08/2018
                    string UrlAbrirQRContrato = "AcuseOtraFigura.aspx?IdFolio=" + this.strFolioContrato + "&isInsert=true";
                    Control LabelNombreContrato = this.ctrlUsuarioInfo.FindControl("LabelUsr");
                    string NombreUsuarioContrato = ((Label)LabelNombreContrato).Text;


                    //RCA 13/08/201
                    //obtenemos el QR el tipo 5 es para contratos de otras figuras de ocupacion
                    string QR = UtilContratosArrto.GenerarCodigoQR(iFolioContrato.ToString(), 5, NombreUsuarioContrato, UrlAbrirQRContrato);

                    //obtenemos el idaplicacion dl contrato 
                    int IdAplicacionContrato = new NG_ContratoArrto().IdContrato(iFolioContrato.ToString());

                    //RCA 13/08/2018
                    if (!string.IsNullOrEmpty(QR) && IdAplicacionContrato > 0)
                    {
                        //actualizamos el campo de la QR
                        Ok = new NG_ContratoArrto().ActualizarQRContrato(QR, IdAplicacionContrato);
                    }


                    Ok = true;
                }
                else
                {
                    Msj = "No fue posible registrar el Contrato de Otras Figuras de Ocupación, por favor vuelve a intentar el envío ó reporte al área de Sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                    MostrarMensajeJavaScript(Msj);
                    objContratoArrto = null; //desocupar
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("o es posible") || ex.InnerException.Message.Contains("ya fue aplicado"))
                    {
                        Msj = ex.InnerException.Message;
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        return false;
                    }
                }

                Msj = "Ocurrió una excepción al procesar el registro de la información del contrato, por favor vuelva a intentar ó reporte a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                MostrarMensajeJavaScript(Msj);
                this.LabelInfoEnviar.Focus();

                //registra en una tabla o archivo informacion acerca de una excepcion
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    //valores de contexto  de Excepcion a guardar
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString() //el usuario del SSO
                };
                //persistir la informacion de la Excepcion
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null; //desocupar
            }
            return Ok;
        }

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InmuebleArrto/BusqMvtosOtrasFigurasOcupacion.aspx", false);
        }

        protected void TextBoxMontoPagoMes_TextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxMontoPagoMes.Text.Length == 0)
                this.TextBoxMontoPagoMes.Text = "0.00";

            this.ObtenerTotalRenta();
            this.TextBoxCuotaMantenimiento.Focus();
        }

        protected void TextBoxMontoPagoEstacionamiento_TextChanged(object sender, EventArgs e)
        {

            if (this.TextBoxMontoPagoEstacionamiento.Text.Length == 0)
                this.TextBoxMontoPagoEstacionamiento.Text = "0.00";

            this.ObtenerTotalRenta();
            this.TextBoxPtjeImpuesto.Focus();
        }

        protected void TextBoxCuotaMantenimiento_TextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxCuotaMantenimiento.Text.Length == 0)
                this.TextBoxCuotaMantenimiento.Text = "0.00";

            this.ObtenerTotalRenta();
            this.TextBoxMontoPagoEstacionamiento.Focus();
        }

        //suma los tres valores de cuotas de arrendamiento para obtener un gran total
        private void ObtenerTotalRenta()
        {
            //validar que sean numeros y obtener el totalAcumlado
            if (ValidarCtrlEntradaImportes())
            {
                decimal TotalMontoPago = 0;

                TotalMontoPago = Convert.ToDecimal(this.TextBoxMontoPagoMes.Text);
                TotalMontoPago += Convert.ToDecimal(this.TextBoxCuotaMantenimiento.Text);
                TotalMontoPago += Convert.ToDecimal(this.TextBoxMontoPagoEstacionamiento.Text);

                this.TextBoxTotalRenta.Text = String.Format("{0:C}", TotalMontoPago);
            }
        }

        protected void ButtonIrRegistrarOpinion_Click(object sender, EventArgs e)
        {
            //lleva el IdInmueble seleccionado, por lo que es viable ir a esta vista
            Response.Redirect("~/EmisionOpinion/BusqOpinion.aspx");
        }


        //habiliar o deshabilitar opcion de captura de otros, si selecciono el item: Otros
        protected void DropDownListTipoOcupacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListTipoOcupacion.SelectedItem.Text == "Otros")
            {
                this.TextBoxOtrosTipoOcupacion.ReadOnly = false;
                this.TextBoxOtrosTipoOcupacion.Focus();
            }
            else
            {
                this.TextBoxOtrosTipoOcupacion.ReadOnly = true;
                this.TextBoxOtrosTipoOcupacion.Text = String.Empty;
                DropDownListTipoOcupacion.Focus();
            }
        }

        protected void btnValidaRIUF_Click(object sender, EventArgs e)
        {
            if (this.TextBoxRIUF.Text.Trim() != "")
            {
                this.LabelInfoInmuebleArrendamiento.Text = "";
                this.CheckBoxGenerarRIUF.Checked = false;
                this.CheckBoxGenerarRIUF.Visible = false;
            }
            else
            {
                this.CheckBoxGenerarRIUF.Checked = false;
                this.CheckBoxGenerarRIUF.Visible = true;
            }
        }
        protected void CheckBoxGenerarRIUF_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxGenerarRIUF.Checked)
            {
                this.rfvTextBoxRIUF.Enabled = false;
                this.TextBoxRIUF.Text = "";
                Msj = "Has seleccionado generar un nuevo RIUF, éste será generado al registrar el nuevo Contrato";
                this.LabelInfoInmuebleArrendamiento.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
            }
            else
            {
                this.rfvTextBoxRIUF.Enabled = true;
                if (this.TextBoxRIUF.Text == "")
                {
                    Msj = "No se ha encontrado un RIUF asociado al domicilio para el inmueble actual. Debera seleccionar un RIUF existente o generar uno nuevo.";
                    this.LabelInfoInmuebleArrendamiento.Text = "<div class='alert alert-warning'> " + Msj + "</div>";
                }
            }
        }

        protected void drpCuentaConDictamenSeguridad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCuentaConDictamenSeguridad.SelectedValue == "1")
            {
                pnlSeguridadEstructural.Visible = true;
            }
            else
            {
                pnlSeguridadEstructural.Visible = false;
            }
        }

        protected void DropDownListTipoUsoInmueble_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IdUsoGenerica = 0;

            DropDownListTipoUsoEspecificoInmueble.Items.Clear();
            this.DropDownListTipoUsoEspecificoInmueble.DataTextField = "Descripcion";
            DropDownListTipoUsoEspecificoInmueble.DataValueField = "IdValue";

            if (int.TryParse(DropDownListTipoUsoInmueble.SelectedValue, out IdUsoGenerica))
            {
                List<Catalogo> catalogos = new NG()
                    .LlenaComboUsoEspecifico(IdUsoGenerica)
                    .Select(x => new Catalogo { IdValue = x.IdCatalogo, Descripcion = x.Nombre })
                    .ToList();

                if (catalogos != null)
                {
                    DropDownListTipoUsoEspecificoInmueble.DataSource = catalogos
                        .OrderBy(x => x.Descripcion)
                        .ToList();
                    DropDownListTipoUsoEspecificoInmueble.DataBind();
                }
            }

            DropDownListTipoUsoEspecificoInmueble.Items.Insert(0, "--");
            this.DropDownListTipoUsoEspecificoInmueble.Items.FindByText("--").Selected = true;
        }
    }
}