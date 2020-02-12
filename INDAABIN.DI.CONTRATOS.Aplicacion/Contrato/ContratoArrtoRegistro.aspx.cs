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
using INDAABIN.DI.ModeloNegocio;
using System.Text;//interconexion con el BUS


namespace INDAABIN.DI.CONTRATOS.Aplicacion.Contrato
{
    public partial class ContratoArrtoRegistro : System.Web.UI.Page
    {
        //private static string strTipoArrendamiento;
        private String Msj;
        private string strFolioContrato;
        private int IdTema;
        string jefedepto = ConfigurationManager.AppSettings["jefeDepot"] == null ? string.Empty : ConfigurationManager.AppSettings["jefeDepot"];

        decimal MontoMinimo
        {
            get
            {
                string dato = ViewState["IdMontoMinimo"] == null ? string.Empty : ViewState["IdMontoMinimo"].ToString();
                decimal valor = 0;
                decimal.TryParse(dato, out valor);
                return valor;
            }
            set { ViewState["IdMontoMinimo"] = value; }
        }

        private ContratoArrto ContratoArrtoAnterior
        {
            get
            {
                ContratoArrto valor = ViewState["vsContratoArrtoAnterior"] == null ?
                    new ContratoArrto() : (ViewState["vsContratoArrtoAnterior"] as ContratoArrto);
                return valor;
            }

            set { ViewState["vsContratoArrtoAnterior"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string strTrace = "";
            //try
            //{
            strTrace += "Inicia Load -> ";
            this.LabelInfo.Text = String.Empty;
            this.LabelInfoEnviar.Text = String.Empty;
            this.LabelInfoDatosContratacion.Text = String.Empty;
            this.LabelInfoSecuencialJust.Text = string.Empty;
            this.LabelInfoFolioOpinion.Text = String.Empty;

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

                strTrace += "Valida contexto -> ";

                String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
                //determinar el tipo de usuario autenticado
                if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                    this.ButtonEnviar.Visible = false; //no puede registrar Solicitudes

                strTrace += "Valida OIC -> ";

                this.lblIdInstitucion.Value = ((SSO)Session["Contexto"]).IdInstitucion.Value.ToString();
                this.lblNombreInstitucionActual.Value = QuitarAcentosTexto(((SSO)Session["Contexto"]).NombreInstitucion);
                this.TextBoxInstitucionActual.Text = ((SSO)Session["Contexto"]).NombreInstitucion;

                strTrace += "Recupera Institucion -> ";

                // Se recuperan valores del registro seleccionado en la consulta de BusqMvtosContratosInmuebles
                this.lblIdEmisionOpinion.Value = Request.QueryString["IdEmision"].ToString();
                this.lblIdContrato.Value = Request.QueryString["IdContrato"].ToString();
                this.lblIdInmuebleArrendamiento.Value = Request.QueryString["IdInmueble"].ToString();

                strTrace += "Recupera QueryString -> ";

                this.LimpiarSessionesGeneradas();

                strTrace += "Limpia sessiones -> ";

                if (this.IdentificarTipoContratoVsQuerySting())
                {
                    if (this.PoblarDropDownTipoContratacion())
                    {
                        if (this.PoblarDropDownTipoMoneda())
                        {
                            if (this.PoblarDropDownUsoGenerico())
                            {
                                if (this.ObtenerValorParametroImpuesto())
                                {
                                    if (this.ObtenerValorParametro_MontoMinimoRentaParaJustipreciacion())
                                    {
                                        //Recupera RIUF, si existe
                                        this.RecuperaRIUFInmueble(this.lblIdInmuebleArrendamiento.Value);
                                        strTrace += "Recupera RIUF -> ";
                                        //Recupera Contrato de Arrendamiento
                                        this.RecuperaContratoArrendamiento(this.lblIdInstitucion.Value, this.lblIdContrato.Value);
                                        strTrace += "Recupera CONTRATO -> ";
                                        //Recupera Folio Opinion
                                        this.RecuperaFolioOpinion(this.lblIdInstitucion.Value, this.lblIdEmisionOpinion.Value, false);
                                        strTrace += "Recupera emisión -> ";
                                        //Se llena la informacion del Capturista   
                                        this.LLenaCamposCapturista();
                                        strTrace += "Recupera CAPTURISTA -> ";
                                        this.DropDownListTipoContratacion.Focus();




                                        //Determinar tipo de Contrato: Nac, Extranjero, en funcion de la direccion del inmueble seleccionada
                                        Control LabelPais = this.ctrlDireccionLectura.FindControl("LabelPais");
                                        string NombrePais = ((Label)LabelPais).Text;
                                        if (NombrePais == "MÉXICO") //solo para contratos nacionales aplica la validacion
                                            this.ButtonverificarNormatividad.Visible = true;

                                        Msj = "Proporciona los valores para cada concepto y al final de la captura da clic en Enviar para que el sistema registre la información y te proporcione un acuse con un número de Folio del Contrato de Arrendamiento";
                                        this.LabelInfo.Text = "<div class='alert alert-info'><strong>Sugerencia: </strong> " + Msj + "</div>";
                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario 

                                        strTrace += "Fin Not isPostBack -> ";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/InmuebleArrto/BusqMvtosContratosInmuebles.aspx");
                }

            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "PostBack", "DeshabilitaControl('cphBody_ctrlDireccionLectura_ButtonRegistrarInmueble');", true);

            strTrace += "Termina Load -> ";
        }

        private void RecuperaRIUFInmueble(string sIdInmueble)
        {
            int IdInmuebleArrto;
            IdInmuebleArrto = System.Convert.ToInt32(sIdInmueble);
            ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrto);

            if (objInmuebleArrto.RIUFInmueble.Trim() == "")
            {
                this.PanelBuscaDomicilios.Visible = true;
                this.TextBoxRIUF.Text = "";
                if (this.lblEsSustitucion.Value == "1")
                    Msj = "No se ha encontrado un RIUF asociado al domicilio el inmueble sustituto. Consulta un RIUF existente o genera uno nuevo.";
                else
                    Msj = "No se ha encontrado un RIUF asociado al domicilio para el inmueble actual. Consulta un RIUF existente o genera uno nuevo.";
                this.LabelInfoInmuebleArrendamiento.Text = "<div class='alert alert-warning'> <strong> ¡Precaución! </strong>" + Msj + "</div>";
            }
            else
            {
                this.LabelInfoInmuebleArrendamiento.Text = "";
                this.TextBoxRIUF.Text = objInmuebleArrto.RIUFInmueble;
                this.lblIdInmuebleRiuf.Value = sIdInmueble;
                this.PanelBuscaDomicilios.Visible = false;
            }
        }

        private void RecuperaContratoArrendamiento(string sIdInstitucion, string sIdContrato)
        {
            if (this.lblEsContinuacion.Value == "1")
            {
                int IdContratoArrto, IdInstitucion;
                IdContratoArrto = System.Convert.ToInt32(sIdContrato);
                IdInstitucion = System.Convert.ToInt32(sIdInstitucion);

                ModeloNegocios.ContratoArrto objContratoArrto = new Negocio.NG_ContratoArrto().ObtenerContratoArrto(IdInstitucion, IdContratoArrto);

                if (objContratoArrto != null)
                {
                    ContratoArrtoAnterior = objContratoArrto;

                    //Datos de Contrato
                    this.TextBoxPropietarioInmueble.Text = objContratoArrto.PropietarioInmueble;
                    this.TextBoxFuncionarioResp.Text = objContratoArrto.FuncionarioResponsable;
                    this.TextBoxRIUF.Text = objContratoArrto.RIUF;
                    // this.DropDownListTipoUsoInmueble.SelectedValue = objContratoArrto.Fk_IdTipoUsoInm.ToString();
                    this.DropDownListTipoMonedaPago.Text = objContratoArrto.Fk_IdTipoMoneda.ToString();
                    this.DropDownListTipoContratacion.Text = objContratoArrto.Fk_IdTipoContratacion.ToString();
                    this.DefineTipoAmbiente();
                    //this.TextBoxFechaIOcupacion.Text = objContratoArrto.strFechaInicioOcupacion;
                    //this.TextBoxFechaFOcupacion.Text = objContratoArrto.strFechaFinOcupacion;
                    this.TextBoxFechaIOcupacion.Text = objContratoArrto.strFechaFinOcupacion;
                    this.TextBoxFechaFOcupacion.Text = "";
                    this.TextBoxAreaOcupadaM2.Text = objContratoArrto.AreaOcupadaM2.ToString("0.00");
                    this.TextBoxMontoPagoMes.Text = objContratoArrto.MontoPagoMensual.ToString("0.00");
                    this.TextBoxCuotaMantenimiento.Text = objContratoArrto.CuotaMantenimiento.ToString("0.00");
                    this.TextBoxMontoPagoEstacionamiento.Text = objContratoArrto.MontoPagoPorCajonesEstacionamiento.ToString("0.00");
                    this.TextBoxPtjeImpuesto.Text = objContratoArrto.PtjeImpuesto.ToString("0.00");
                    this.TextBoxTotalRenta.Text = (objContratoArrto.MontoPagoMensual + objContratoArrto.CuotaMantenimiento + objContratoArrto.MontoPagoPorCajonesEstacionamiento).ToString("0.00");
                    this.TextBoxObs.Text = objContratoArrto.Observaciones;

                    //Datos OIC
                    this.TextBoxNombreTitularOIC.Text = objContratoArrto.PersonaReferenciaTitularOIC.Nombre;
                    this.TextBoxApPatOIC.Text = objContratoArrto.PersonaReferenciaTitularOIC.ApellidoPaterno;
                    this.TextBoxApMatOIC.Text = objContratoArrto.PersonaReferenciaTitularOIC.ApellidoMaterno;
                    this.TextBoxNombreCargoOIC.Text = objContratoArrto.PersonaReferenciaTitularOIC.NombreCargo;
                    this.TextBoxEmailOIC.Text = objContratoArrto.PersonaReferenciaTitularOIC.Email;

                    ////Datos Otra Figura de Ocupacion
                    //this.TextBoxNombreTitularOIC.Text = objContratoArrto.PersonaReferenciaResponsableOcupacion.Nombre;
                    //this.TextBoxApPatOIC.Text = objContratoArrto.PersonaReferenciaResponsableOcupacion.ApellidoPaterno;
                    //this.TextBoxApMatOIC.Text = objContratoArrto.PersonaReferenciaResponsableOcupacion.ApellidoMaterno;
                    //this.TextBoxNombreCargoOIC.Text = objContratoArrto.PersonaReferenciaResponsableOcupacion.NombreCargo;
                    //this.TextBoxEmailOIC.Text = objContratoArrto.PersonaReferenciaResponsableOcupacion.Email;

                    //Datos de Justipreciacion
                    if (objContratoArrto.JustripreciacionContrato != null)
                    {
                        if (objContratoArrto.JustripreciacionContrato.Secuencial != null)
                        {
                            this.TextBoxSecuencialJust.Text = objContratoArrto.JustripreciacionContrato.Secuencial;
                            this.TextBoxGenericoJust.Text = objContratoArrto.JustripreciacionContrato.NoGenerico;
                            this.TextBoxEstatusAttJust.Text = objContratoArrto.JustripreciacionContrato.EstatusAtencion;
                            this.TextBoxFechaDictamenJust.Text = objContratoArrto.JustripreciacionContrato.strFechaDictamen;
                            this.TextBoxSupDictaminadaJust.Text = objContratoArrto.JustripreciacionContrato.SuperficieDictaminada;
                            this.TextBoxUnidadMedidaSup.Text = objContratoArrto.JustripreciacionContrato.UnidadMedidaSupRentableDictaminada;
                            this.TextBoxMontoDictaminadoJust.Text = objContratoArrto.JustripreciacionContrato.MontoDictaminado.Value.ToString("0.00");
                        }
                    }

                }
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
            this.lblEsSustitucion.Value = "0";
            this.lblEsContinuacion.Value = "0";
            this.TextBoxFolioOpinion.Enabled = false;
            this.TextBoxFolioOpinion.Text = "";
            if (sName != null)
            {
                switch (sName)
                {
                    case "1":
                        this.LabelTipificacionArrto.Text = "Nacional";
                        //strTipoArrendamiento = "Nuevo";
                        //etiquetas de titulos
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "Registro de un nuevo contrato de arrendamiento de inmueble nacional";
                        this.TextBoxFolioOpinion.Text = this.lblIdEmisionOpinion.Value;
                        //autoseleccion de: [tipo de Contrato] en la lista desplegable
                        if (this.DropDownListTipoArrandamiento.Items.Contains(this.DropDownListTipoArrandamiento.Items.FindByText("Nuevo")) == true)
                        {
                            this.DropDownListTipoArrandamiento.Items.FindByText("Nuevo").Selected = true;
                            Ok = true;
                        }
                        break;
                    case "4": //nuevo- extranjero
                        this.LabelTipificacionArrto.Text = "Extranjero";
                        //strTipoArrendamiento = "Nuevo";
                        //etiquetas de titulos
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "Registro de un nuevo contrato de arrendamiento de inmueble en el extranjero";
                        this.TextBoxFolioOpinion.Text = this.lblIdEmisionOpinion.Value;
                        //autoseleccion de: [tipo de Contrato] en la lista desplegable
                        if (this.DropDownListTipoArrandamiento.Items.Contains(this.DropDownListTipoArrandamiento.Items.FindByText("Nuevo")) == true)
                        {
                            this.DropDownListTipoArrandamiento.Items.FindByText("Nuevo").Selected = true;
                            Ok = true;
                        }
                        break;
                    case "2": //NACIONAL-SUST
                        this.LabelTipificacionArrto.Text = "Nacional";
                        //strTipoArrendamiento = "Sustitución";
                        //etiquetas de titulos
                        this.lblEsSustitucion.Value = "1";
                        this.TextBoxRIUF.Text = "";
                        this.TextBoxFolioOpinion.Enabled = true;
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "Registro de una de sustitución al contrato de arrendamiento nacional: [" + this.lblIdContrato.Value + "].";

                        ////validar si se pide sustitucion basada en contrato local o de historico
                        //if (Session["FolioContrato"] != null)
                        //    this.LabelInfoEncabezadoPanelPrincipal.Text = "REGISTRO DE UNA DE SUSTITUCIÓN AL CONTRATO DE ARRENDAMIENTO NACIONAL: [" + Session["FolioContrato"].ToString() + "].";
                        //else
                        //{
                        //    if (Session["NumContratoHist"] != null)
                        //        this.LabelInfoEncabezadoPanelPrincipal.Text = "REGISTRO DE UNA SUSTITUCIÓN AL CONTRATO DE ARRENDAMIENTO NACIONAL: [" + Session["NumContratoHist"].ToString() + "].";
                        //}

                        //autoseleccion de: [tipo de Contrato] en la lista desplegable
                        if (this.DropDownListTipoArrandamiento.Items.Contains(this.DropDownListTipoArrandamiento.Items.FindByText("Sustitución")) == true)
                        {
                            this.DropDownListTipoArrandamiento.Items.FindByText("Sustitución").Selected = true;
                            Ok = true;
                        }
                        break;

                    case "5": //EXTRJ-SUST
                        this.LabelTipificacionArrto.Text = "Extranjero";
                        //strTipoArrendamiento = "Sustitución";
                        //etiquetas de titulos
                        this.lblEsSustitucion.Value = "1";
                        this.TextBoxRIUF.Text = "";
                        this.TextBoxFolioOpinion.Enabled = true;
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "Registro de una de sustitución al contrato de arrendamiento extranjero: [" + this.lblIdContrato.Value + "].";

                        //autoseleccion de: [tipo de Contrato] en la lista desplegable
                        if (this.DropDownListTipoArrandamiento.Items.Contains(this.DropDownListTipoArrandamiento.Items.FindByText("Sustitución")) == true)
                        {
                            this.DropDownListTipoArrandamiento.Items.FindByText("Sustitución").Selected = true;
                            Ok = true;
                        }
                        break;

                    case "3": //CONTI-NACIONAL
                        this.LabelTipificacionArrto.Text = "Nacional";
                        //strTipoArrendamiento = "Continuación";
                        //etiquetas de titulos
                        this.lblEsContinuacion.Value = "1";
                        this.TextBoxFolioOpinion.Enabled = true;
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "Registro de una continuación al contrato de arrendamiento nacional: [" + this.lblIdContrato.Value + "].";

                        //autoseleccion de: [tipo de Contrato] en la lista desplegable
                        if (this.DropDownListTipoArrandamiento.Items.Contains(this.DropDownListTipoArrandamiento.Items.FindByText("Continuación")) == true)
                        {

                            this.DropDownListTipoArrandamiento.Items.FindByText("Continuación").Selected = true;
                            Ok = true;
                        }
                        break;

                    case "6": //CONT- EXTRANJERO
                        this.LabelTipificacionArrto.Text = "Extranjero";
                        //strTipoArrendamiento = "Continuación";
                        //etiquetas de titulos
                        this.lblEsContinuacion.Value = "1";
                        this.TextBoxFolioOpinion.Enabled = true;
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "Registro de una continuación al contrato de arrendamiento en el extranjero: [" + this.lblIdContrato.Value + "].";

                        //autoseleccion de: [tipo de Contrato] en la lista desplegable
                        if (this.DropDownListTipoArrandamiento.Items.Contains(this.DropDownListTipoArrandamiento.Items.FindByText("Continuación")) == true)
                        {
                            this.DropDownListTipoArrandamiento.Items.FindByText("Continuación").Selected = true;
                            Ok = true;
                        }
                        break;
                }
            }
            return Ok;
        }

        private Boolean ObtenerValorParametroImpuesto()
        {
            try
            {
                this.TextBoxPtjeImpuesto.Text = new NG_Catalogos().ObtenerValorCatParametro("Impuesto IVA Nacional");
                return true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el valor del porcentaje de IVA. Contacta al área de sistemas.";
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

        //monto minimo para exigir Justipreciacion de Renta
        private Boolean ObtenerValorParametro_MontoMinimoRentaParaJustipreciacion()
        {
            try
            {
                this.LabelMontoMinimoRentaParaJustipreciacion.Text = new NG_Catalogos().ObtenerValorCatParametro("Monto Minimo de Renta para Justipreciacion");

                //Veririficar que sea un decimal el valor recuperado, porque en la BD se guarda como un varchar
                decimal Valor; //variable para leer el número
                bool OkConversion; //esta nos dice si es un número válido

                OkConversion = Decimal.TryParse(this.LabelMontoMinimoRentaParaJustipreciacion.Text, out Valor); //almacenamos true si la conversión se realizó
                if (OkConversion)
                {
                    MontoMinimo = Valor;
                    return true;
                }

                else
                {
                    Msj = "El valor definido para la justipreciación minima de renta no es un valor decimal. Contacta al área de sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el monto minimo para la renta de justipreciacion. Contacta al área de sistemas.";
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

        //llenado de Combo Catalogo de BD Local. (Automtico, Dictaminado...)
        private Boolean PoblarDropDownTipoContratacion()
        {
            Boolean Ok = false;
            List<TipoContratacion> ListTipoContratacion = new List<TipoContratacion>();
            try
            {

                TipoContratacion tContratacion = new TipoContratacion
                {
                    IdTipoContratacion = 0,
                    DescripcionTipoContratacion = "--",
                };

                ListTipoContratacion.Add(tContratacion);
                ListTipoContratacion.AddRange(new NG_Catalogos().ObtenerTipoContratacion());

                this.DropDownListTipoContratacion.DataSource = ListTipoContratacion;
                this.DropDownListTipoContratacion.DataValueField = "IdTipoContratacion";
                this.DropDownListTipoContratacion.DataTextField = "DescripcionTipoContratacion";
                this.DropDownListTipoContratacion.DataBind();
                //agregar un elemento para reprsentar que no se ha seleccionado un valor
                //this.DropDownListTipoContratacion.Items.Add("--");
                this.DropDownListTipoContratacion.Items.FindByText("--").Selected = true;

                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al poblar la lista Tipo de Contratacion. Contacta al área de sistemas.";
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

        private Boolean PoblarDropDownUsoGenerico()
        {


            bool ok = false;

            DropDownListTipoUsoInmueble.DataTextField = "Descripcion";
            DropDownListTipoUsoInmueble.DataValueField = "IdValue";

            try
            {
                //cargar la lista de usus genericos del inmueble
                DropDownListTipoUsoInmueble.DataSource = AdministradorCatalogos.ObtenerCatUsoGenerico();
                DropDownListTipoUsoInmueble.DataBind();

                //agregar un elemento que represente a todos
                DropDownListTipoUsoEspecificoInmueble.Items.Add("--");
                DropDownListTipoUsoInmueble.Items.Add("--");
                DropDownListTipoUsoInmueble.Items.FindByText("--").Selected = true;

                ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ocurrió una excepción al cargar la lista de tipos de uso de inmueble. Contacta al área de Sistemas.";
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
            return ok;





            //Boolean Ok = false;

            ////this.DropDownListTipoUsoInmueble.Items.Clear();
            //this.DropDownListTipoUsoInmueble.DataTextField = "Descripcion";
            //this.DropDownListTipoUsoInmueble.DataValueField = "IdValue";

            //try
            //{
            //    //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
            //    // RCA cambio por uso generico
            //    this.DropDownListTipoUsoInmueble.DataSource = new NG().LlenaCombo("ObtenerUsogenerico")
            //        .OrderBy(x => x.Descripcion)
            //        .ToList();



            //    // RCA cambio por uso generico
            //    this.DropDownListTipoUsoInmueble.DataBind();



            //    //this.DropDownListTipoUsoInmueble.Items.Add(new ListItem("TODOS","0"));
            //    //this.DropDownListTipoUsoInmueble.Items.Insert(0, "--");
            //    //this.DropDownListTipoUsoInmueble.Items.FindByText("--").Selected = true;
            //    //this.DropDownListTipoUsoInmueble.Items.FindByText("TODOS").Selected = true;
            //    Ok = true;
            //}
            //catch (Exception ex)
            //{
            //    Msj = "Ocurrió una excepción al cargar la lista de tipos de uso de inmueble. Contacta al área de Sistemas.";
            //    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
            //    MostrarMensajeJavaScript(Msj);

            //    BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
            //    {
            //        CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
            //        Aplicacion = "ContratosArrto",
            //        Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
            //        Funcion = MethodBase.GetCurrentMethod().Name + "()",
            //        DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
            //        Usr = ((SSO)Session["Contexto"]).UserName.ToString()
            //    };
            //    BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
            //    BitacoraExcepcionAplictivo = null;
            //}
            //return Ok;
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
                Msj = "Ha ocurrido un error al cargar la lista de tipos de moneda. Contacta al área de sistemas.";
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

        protected void ButtonObtenerJustipreciacion_Click(object sender, EventArgs e)
        {
            if (this.TextBoxSecuencialJust.Text.Trim().Length == 0)
            {
                //limpieaza de ctrls
                this.TextBoxPropietarioInmueble.Text = String.Empty;
                this.TextBoxFuncionarioResp.Text = String.Empty;
                this.TextBoxFechaDictamenJust.Text = String.Empty;
                this.TextBoxSupDictaminadaJust.Text = String.Empty;
                this.TextBoxMontoDictaminadoJust.Text = String.Empty;
                this.TextBoxGenericoJust.Text = String.Empty;
                this.TextBoxUnidadMedidaSup.Text = string.Empty;
                this.TextBoxEstatusAttJust.Text = string.Empty;
                this.LabelInfo.Text = string.Empty;
                this.LabelInfoSecuencialJust.Text = string.Empty;
                this.TextBoxInstitucionJustipreciacion.Text = string.Empty;
            }
            else
            {
                this.ObtenerJustipreciacion();
            }
        }

        private void ObtenerJustipreciacion()
        {
            //limpieaza de ctrls
            this.TextBoxPropietarioInmueble.Text = String.Empty;
            this.TextBoxFuncionarioResp.Text = String.Empty;
            this.TextBoxFechaDictamenJust.Text = String.Empty;
            this.TextBoxSupDictaminadaJust.Text = String.Empty;
            this.TextBoxMontoDictaminadoJust.Text = String.Empty;
            this.TextBoxGenericoJust.Text = String.Empty;
            this.TextBoxUnidadMedidaSup.Text = string.Empty;
            this.TextBoxEstatusAttJust.Text = string.Empty;
            this.LabelInfo.Text = string.Empty;
            this.LabelInfoSecuencialJust.Text = string.Empty;
            this.TextBoxInstitucionJustipreciacion.Text = string.Empty;
            this.pnlInstitucion.Visible = false;

            if (TextBoxSecuencialJust.Text.Length == 0)
            {
                Msj = "Proporciona el secuencial de justipreciación";
                this.LabelInfo.Text = "<div class='alert alert-warning'> <span>¡Precaución!</span>" + Msj + "</div>";
                this.LabelInfoSecuencialJust.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.TextBoxSecuencialJust.Focus();
            }
            else
            {
                //Interconectar con el BUS para obtener la informacion de Avaluos
                if (this.lblEsSustitucion.Value == "1")
                {

                    if (this.ObtenerInformacionSecuenciaJustipreciacionParaSustitucion())
                    {
                        Msj = "El secuencial de justipreciación proporcionado es válido para a la institución, estado, municipio y número exterior de la dirección del inmueble para el que se registra el Contrato";
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong> " + Msj + "</div>";
                        this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;

                        //si se encontro la informacion, desbloquear los ctrls
                        this.TextBoxPropietarioInmueble.ReadOnly = false;
                        this.TextBoxFuncionarioResp.ReadOnly = false;
                    }
                }
                else
                {
                    if (this.ObtenerInformacionSecuenciaJustipreciacion())
                    {
                        Msj = "El secuencial de justipreciación proporcionado es válido para a la institución, estado, municipio y número exterior de la dirección del inmueble para el que se registra el Contrato";
                        this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong> " + Msj + "</div>";
                        this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;

                        //si se encontro la informacion, desbloquear los ctrls
                        this.TextBoxPropietarioInmueble.ReadOnly = false;
                        this.TextBoxFuncionarioResp.ReadOnly = false;
                    }
                }
            }
        }

        private bool ComparaInstitucion(string InstitucionActual, string InstitucionJustipreciacion)
        {
            //RCA 08/02/2018
            //convertir todo a minuscula tanto la institucionactual como la institucion de la justipreciacion esto bos permite que aunque
            //sea la misma institucion no importa si tiene acentos o esta una en mayusculas o una en minusculas, si son el mismo nombre pasara

            if (InstitucionActual.ToLower() != InstitucionJustipreciacion.ToLower())
            {
                if (InstitucionActual.Contains(InstitucionJustipreciacion) || InstitucionJustipreciacion.Contains(InstitucionActual))
                    Msj = "La institución de la justipreciación corresponde parcialmente con la institución a la que estas adscrito, sin embargo, puedes confirmar el uso de la justipreciación.";
                else
                    Msj = "La institución de la justipreciación no corresponde con la institución a la que estas adscrito, sin embargo, puedes confirmar el uso de la justipreciación.";

                this.LabelInfo.Text = "<div class='alert alert-warning'> <span>¡Precaución!</span>" + Msj + "</div>";
                this.LabelInfoSecuencialJust.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.pnlInstitucion.Visible = true;
                return false;
            }
            else
            {
                this.LabelInfo.Text = "";
                this.LabelInfoSecuencialJust.Text = this.LabelInfo.Text;
                this.pnlInstitucion.Visible = false;
                return true;
            }

        }

        //interconectar con el sistema de avaluos, a traves del BUS, para obtener el secuencial proporcionado
        private Boolean ObtenerInformacionSecuenciaJustipreciacion()
        {
            Boolean Ok = false;

            SolicitudAvaluos objSolicitudAvaluos = null;


            string secuencial = string.Empty;
            try
            {
                secuencial = this.TextBoxSecuencialJust.Text.Trim();

                //interconexion con el sistema de Avaluos, a través del BUS
                objSolicitudAvaluos = new NG().ObtenerJustipreciacionAvaluos(this.TextBoxSecuencialJust.Text.Trim());

                if (objSolicitudAvaluos != null)
                {
                    //Validacion1: que el status de att de la justipreciacion no este: cancelada
                    if (objSolicitudAvaluos.Estatus.ToUpper() != "CANCELADO")
                    {
                        //Validacion2: que la institucion de la justipreciacion sea igual a la institucion del promovente
                        //verificar coincidencias de cadenas, esto por que no son iguales los nombres de las instituciones de Avaluos vs SSO.DbCat_Nueva
                        //if (objSolicitudAvaluos.InstitucionDescripcion.Replace(" ", "").IndexOf(((SSO)Session["Contexto"]).NombreInstitucion.Replace(" ", "")) > -1)
                        //TODO comparar cuando SSO y BUS se correspondan en las instituciones
                        if (1 == 1) //mientras las instiucion del bus (de DBCat_Nueva) se corresponden las del SSO

                        //que la justipreciacion corresponda a la institucion (comparativa por descripcion porque son fuentes de catalogos distintas)
                        //  if (objSolicitudAvaluos.InstitucionDescripcion.Replace(" ", "") == ((SSO)Session["Contexto"]).NombreInstitucion.Replace(" ", ""))
                        {
                            ////session poblada al seleccionar un objeto en: DireccionLectura.ascx
                            //if (Session["objInmuebleArrtoSeleccionado"] != null)
                            //{
                            ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(System.Convert.ToInt32(this.lblIdInmuebleArrendamiento.Value));
                            if (QuitarAcentosTexto(objInmuebleArrto.NombrePais.ToUpper()) == "MEXICO")
                            {
                                //obtener nombre de la ent. fed
                                objInmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(objInmuebleArrto.IdEstado.Value);
                                //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                                objInmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(objInmuebleArrto.IdEstado.Value, objInmuebleArrto.IdMunicipio.Value);

                                //Validacion3: que la justipreciacion corresponda con estado y municipio de la direccion del inmueble que se registra el contrato
                                //comparar cadenas sin espacios, sin acentos y en mayusculas
                                if (QuitarAcentosTexto(objSolicitudAvaluos.EstadoDescripcion.Replace(" ", "").ToUpper()) == QuitarAcentosTexto(objInmuebleArrto.NombreEstado.Replace(" ", "").ToUpper())
                                        && QuitarAcentosTexto(objSolicitudAvaluos.MunicipioDescripcion.Replace(" ", "").ToUpper()) == QuitarAcentosTexto(objInmuebleArrto.NombreMunicipio.Replace(" ", "").ToUpper())
                                    //no se aplico validacion vs #Ext porque como su entrada es escrita, puede ser diferente por un punto o un caracter o una abreviacion  
                                    //  && objSolicitudAvaluos.NoExterior.Replace(" ", "").ToUpper() == objInmuebleArrto.NumExterior.Replace(" ", "").ToUpper()

                                    )
                                {
                                    Boolean CP_Ok = false;  //Descomentar para validar CP
                                    //Boolean CP_Ok = true;

                                    //existen casos en que ciertos justipreciaciones de renta, concluidos no tienen CP, entonces solo se vlaidara cuando exista
                                    if (String.IsNullOrEmpty(objSolicitudAvaluos.CP) == false)
                                    {
                                        //si existe el CP, comparar que sean iguales, sin espacios
                                        var res = objSolicitudAvaluos.CP.Trim().Replace(" ", "").PadLeft(5, '0');
                                        var res1 = objInmuebleArrto.CodigoPostal.Trim().Replace(" ", "").PadLeft(5, '0');

                                        //if (objSolicitudAvaluos.CP.Trim().Replace(" ", "").PadLeft(5, '0') == objInmuebleArrto.CodigoPostal.Trim().Replace(" ", "").PadLeft(5, '0'))
                                        if (res == res1)
                                        {
                                            CP_Ok = true;
                                        }
                                        else
                                        {
                                            Msj = "El secuencial de justipreciación proporcionado no corresponde con el código postal  de la dirección del inmueble ¡verifica! <br/> Entidad Federativa de la justipreciación: " + objSolicitudAvaluos.EstadoDescripcion
                                                    + " <br/> Muninicipio de la justipreciación: " + objSolicitudAvaluos.MunicipioDescripcion
                                                    + " <br/> Código postal de la justipreciación: " + objSolicitudAvaluos.CP;

                                            this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            MostrarMensajeJavaScript("El secuencial de justipreciación proporcionado no corresponde con el código postal de la dirección del inmueble, verifica.");//sin html en la cadena
                                            this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                                            this.LimpiarJustupreciacion();
                                            this.LabelInfoSecuencialJust.Focus();

                                        }
                                    }
                                    else //no hay CP, no comparar y dejar pasar
                                        CP_Ok = true;

                                    //solo si existe el CP en la justipreciacion, se debio comparar y validar su igualdad
                                    if (CP_Ok)
                                    {
                                        //Validacion 4: que Fecha de Dictamen, Monto Dictaminado y  Sup. Dictaminada existan,
                                        //son los datos basicos para validar el cumplimiento de la normatividad
                                        //puede ser que el secuencial este en proceso, y aun no cuente con esta informcion, por lo que no sera una
                                        //justipreciacion valida para registrar vs. el contrato.
                                        string SupRentable = null;
                                        //existen justipreciaciones de renta que no tienen SuperficieRentableDictaminado, entonces se tomara la capturada
                                        if (objSolicitudAvaluos.SuperficieRentableDictaminado != null || objSolicitudAvaluos.SuperficieRentable != null)
                                        {
                                            if ((objSolicitudAvaluos.SuperficieRentableDictaminado != null) && (objSolicitudAvaluos.SuperficieRentableDictaminado > 0))
                                                SupRentable = objSolicitudAvaluos.SuperficieRentableDictaminado.Value.ToString("0.00");
                                            if ((objSolicitudAvaluos.SuperficieRentable != null) && (objSolicitudAvaluos.SuperficieRentable > 0))
                                                SupRentable = objSolicitudAvaluos.SuperficieRentable.Value.ToString("0.00");
                                        }

                                        string MontoDictaminado = null;
                                        if (objSolicitudAvaluos.MontoDictaminado != null)
                                            MontoDictaminado = objSolicitudAvaluos.MontoDictaminado.Value.ToString("0.00");

                                        //if (objSolicitudAvaluos.SuperficieRentableDictaminado.Value > 0 && String.IsNullOrEmpty(objSolicitudAvaluos.SuperficieRentableDictaminado.ToString()) == false)
                                        //    //se toma como 1ra opcion, siempre la Sup. rentable dictaminada
                                        //    SupRentable = objSolicitudAvaluos.SuperficieRentableDictaminado.ToString();
                                        //else//la sup. rentable que se toma, es la captura por el promovente en la solicitud
                                        //    SupRentable = objSolicitudAvaluos.SuperficieRentable.ToString();

                                        //Es necesario que existan estos dos valores de la justipreciacion para poder aplicar la validacion de la normatividad
                                        if ((SupRentable != null && MontoDictaminado != null) && (Convert.ToDecimal(SupRentable) > 0 && Convert.ToDecimal(MontoDictaminado) > 0))
                                        {
                                            //si existen los valores numericos para validar normatividad; exponer los valores en la vista
                                            this.TextBoxGenericoJust.Text = objSolicitudAvaluos.NoGenerico;

                                            if (String.IsNullOrEmpty(objSolicitudAvaluos.FechaDictamen.ToString()) == false)
                                                this.TextBoxFechaDictamenJust.Text = objSolicitudAvaluos.FechaDictamen.Substring(0, 10);

                                            this.TextBoxMontoDictaminadoJust.Text = MontoDictaminado;
                                            this.TextBoxSupDictaminadaJust.Text = SupRentable;
                                            this.TextBoxUnidadMedidaSup.Text = objSolicitudAvaluos.UnidadMedidaRentable;
                                            this.TextBoxEstatusAttJust.Text = objSolicitudAvaluos.Estatus;
                                            this.TextBoxPropietarioInmueble.Text = objSolicitudAvaluos.Propietario;
                                            this.TextBoxFuncionarioResp.Text = objSolicitudAvaluos.Responsable;

                                            //Valida la Institucion
                                            this.TextBoxInstitucionJustipreciacion.Text = objSolicitudAvaluos.InstitucionDescripcion;
                                            //this.lblNombreInstitucion.Value = QuitarAcentosTexto(objSolicitudAvaluos.InstitucionDescripcion.Replace(" ", ""));

                                            //RCA
                                            //agregue un split para cuando encuentre un nombre con un prefijo, solo tome los nombres sin los prefijos, para que entren en el metodo de comparar 
                                            //abajo dejo la linea de codigo original, solo agregue tres lineas de codigo y modifique la llamada al metodo.
                                            //se modifico y se quito el split, reemplazandolo por un content que contiene los espacios en blanco, los - y las comas
                                            //this.ComparaInstitucion(this.lblNombreInstitucionActual.Value, this.lblNombreInstitucion.Value); 

                                            //RCA 11/08/2017
                                            //quitamos el replace
                                            String content = @"\s-[,]?\s?";
                                            string[] InstitucionJustipreciacion = Regex.Split(objSolicitudAvaluos.InstitucionDescripcion, content);
                                            string[] NombreInstitucionActual = Regex.Split(this.TextBoxInstitucionActual.Text, content);
                                            //String NombreInstitucionJustipreciacion = QuitarAcentosTexto(InstitucionJustipreciacion[0].Replace(" ", ""));
                                            //String NombreInstitoActual = QuitarAcentosTexto(NombreInstitucionActual[0].Replace(" ", ""));
                                            String NombreInstitucionJustipreciacion = QuitarAcentosTexto(InstitucionJustipreciacion[0]);
                                            String NombreInstitoActual = QuitarAcentosTexto(NombreInstitucionActual[0]);

                                            this.ComparaInstitucion(NombreInstitoActual, NombreInstitucionJustipreciacion);



                                        }
                                        else //no existen los valores numericos de Superficie y monto para validar la normatividad
                                        {
                                            if (objSolicitudAvaluos.Estatus.ToUpper() != "CONCLUIDO")
                                                Msj = "El secuencial de justipreciación proporcionado con estatus de atención: " + objSolicitudAvaluos.Estatus.ToUpper() + ", aun no cuenta con: <br/> * Monto dictaminado <br/> * Superficie rentable dictaminado ó capturada por el promovente en la solicitud de avalúo <br/> Es necesario que se cuente con esta información para poder registrarlo al contrato, por favor espere a que se concluya el avalúo para poder registrar el contrato de arrendamiento.";
                                            else //es concluido, pero no tiene los datos basicos para validar normatividad, este caso seria muy extraño que ocurriera
                                                Msj = "El secuencial de justipreciación proporcionado con estatus de atención: " + objSolicitudAvaluos.Estatus.ToUpper() + ", aun no cuenta con: <br/> * Monto dictaminado <br/> * Superficie rentable dictaminado ó capturada por el promovente en la solicitud de avalúo <br/> Es necesario que se cuente con esta información para poder registrarlo al contrato, por favor contacte al Indaabin.";
                                            this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            //msj sin uso de HTML
                                            Msj = Msj = "El secuencial de justipreciación proporcionado con estatus de atención: " + objSolicitudAvaluos.Estatus.ToUpper() + ", aun no cuenta con el Monto Dictaminado y/o la  Superficie Rentable Dictaminado ó capturada por el promovente en la solicitud de avalúo. Es necesario que se cuente con esta información para poder registrarlo.";
                                            MostrarMensajeJavaScript(Msj);//sin html en la cadena
                                            this.LimpiarJustupreciacion();
                                            this.LabelInfoSecuencialJust.Focus();
                                        }
                                    }
                                    Ok = true;
                                }
                                else
                                {
                                    Msj = "El secuencial de justipreciación proporcionado no corresponde con la entidad federativa y municipio en la dirección del inmueble, verifica. <br/> Entidad Federativa de la justipreciación: " + objSolicitudAvaluos.EstadoDescripcion
                                                    + " <br/> Muninicipio de la justipreciación: " + objSolicitudAvaluos.MunicipioDescripcion
                                                        + " <br/> Código postal de la justipreciación: " + objSolicitudAvaluos.CP
                                                    + " <br/> No. Exterior: " + objSolicitudAvaluos.NoExterior;
                                    this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                    this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                                    MostrarMensajeJavaScript("El secuencial de justipreciación proporcionado no corresponde con la entidad federativa y municipio en la dirección del inmueble, verifica.");//sin html en la cadena
                                    this.LimpiarJustupreciacion();
                                    this.LabelInfoSecuencialJust.Focus();
                                }
                            }
                            else
                            {
                                Msj = "El secuencial de justipreciación debe ser para un inmueble nacional, verifica.";
                                this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                                MostrarMensajeJavaScript(Msj);
                                this.LimpiarJustupreciacion();
                                this.LabelInfoSecuencialJust.Focus();
                            }
                            objInmuebleArrto = null;
                        }
                        else
                        {
                            Msj = "El secuencial de justipreciación proporcionado no pertenece a la institución a la que estás adscrito";
                            this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                            MostrarMensajeJavaScript(Msj);
                            this.LimpiarJustupreciacion();
                            this.TextBoxSecuencialJust.Focus();
                        }
                    }
                    else
                    {
                        Msj = "El secuencial de justipreciación proporcionado tiene un estatus de atención: Cancelado, por lo que es inválido para utilizar, verifica.";
                        this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                        MostrarMensajeJavaScript(Msj);//sin html en la cadena
                        this.LimpiarJustupreciacion();
                        this.LabelInfoSecuencialJust.Focus();
                    }
                }
                else
                {
                    Msj = "<h4>Estimado Promovente, por favor envía la copia de tu justipreciación de renta al siguiente correo electrónico: <u> " + jefedepto + "</u>, para continuar con el registro de tu contrato.</h4>";


                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Notificar! </strong> " + Msj + "</div>";
                    this.LabelInfoSecuencialJust.Text = this.LabelInfo.Text;


                    // MZT 15/08/2017
                    this.LimpiarJustupreciacion();
                    this.TextBoxSecuencialJust.Focus();

                    // MZT 15/08/2017
                    this.NotificarNoExisteJustipreciacion(secuencial);

                }
                objSolicitudAvaluos = null;
            }
            catch (Exception ex)
            {
                Msj = "No se ha podido recuperar la Justipreciación, verifica el número de secuencial proporcionado.";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> Error </strong>" + Msj + "<br/> Detalle: " + ex.Message + "</div>";
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

        private Boolean ObtenerInformacionSecuenciaJustipreciacionParaSustitucion()
        {
            Boolean Ok = false;
            SolicitudAvaluos objSolicitudAvaluos = null;
            string secuencial = string.Empty;


            try
            {
                // MZT 15/08/2017
                secuencial = this.TextBoxSecuencialJust.Text.Trim();
                //interconexion con el sistema de Avaluos, a través del BUS
                objSolicitudAvaluos = new NG().ObtenerJustipreciacionAvaluos(this.TextBoxSecuencialJust.Text.Trim());

                if (objSolicitudAvaluos != null)
                {
                    //Validacion1: que el status de att de la justipreciacion no este: cancelada
                    if (objSolicitudAvaluos.Estatus.ToUpper() != "CANCELADO")
                    {
                        //Validacion2: que la institucion de la justipreciacion sea igual a la institucion del promovente
                        //verificar coincidencias de cadenas, esto por que no son iguales los nombres de las instituciones de Avaluos vs SSO.DbCat_Nueva
                        //if (objSolicitudAvaluos.InstitucionDescripcion.Replace(" ", "").IndexOf(((SSO)Session["Contexto"]).NombreInstitucion.Replace(" ", "")) > -1)
                        //TODO comparar cuando SSO y BUS se correspondan en las instituciones
                        if (1 == 1) //mientras las instiucion del bus (de DBCat_Nueva) se corresponden las del SSO
                        //que la justipreciacion corresponda a la institucion (comparativa por descripcion porque son fuentes de catalogos distintas)
                        //  if (objSolicitudAvaluos.InstitucionDescripcion.Replace(" ", "") == ((SSO)Session["Contexto"]).NombreInstitucion.Replace(" ", ""))
                        {
                            //Es necesario que existan estos dos valores de la justipreciacion para poder aplicar la validacion de la normatividad
                            string SupRentable = null;
                            if (objSolicitudAvaluos.SuperficieRentableDictaminado != null || objSolicitudAvaluos.SuperficieRentable != null)
                            {
                                if ((objSolicitudAvaluos.SuperficieRentableDictaminado != null) && (objSolicitudAvaluos.SuperficieRentableDictaminado > 0))
                                    SupRentable = objSolicitudAvaluos.SuperficieRentableDictaminado.Value.ToString("0.00");
                                if ((objSolicitudAvaluos.SuperficieRentable != null) && (objSolicitudAvaluos.SuperficieRentable > 0))
                                    SupRentable = objSolicitudAvaluos.SuperficieRentable.Value.ToString("0.00");
                            }

                            string MontoDictaminado = null;
                            if (objSolicitudAvaluos.MontoDictaminado != null)
                                MontoDictaminado = objSolicitudAvaluos.MontoDictaminado.Value.ToString("0.00");

                            if ((SupRentable != null && MontoDictaminado != null) && (Convert.ToDecimal(SupRentable) > 0 && Convert.ToDecimal(MontoDictaminado) > 0))
                            {
                                //si existen los valores numericos para validar normatividad; exponer los valores en la vista
                                this.TextBoxGenericoJust.Text = objSolicitudAvaluos.NoGenerico;

                                if (String.IsNullOrEmpty(objSolicitudAvaluos.FechaDictamen.ToString()) == false)
                                    this.TextBoxFechaDictamenJust.Text = objSolicitudAvaluos.FechaDictamen.Substring(0, 10);

                                this.TextBoxMontoDictaminadoJust.Text = MontoDictaminado;
                                this.TextBoxSupDictaminadaJust.Text = SupRentable;
                                this.TextBoxUnidadMedidaSup.Text = objSolicitudAvaluos.UnidadMedidaRentable;
                                this.TextBoxEstatusAttJust.Text = objSolicitudAvaluos.Estatus;
                                this.TextBoxPropietarioInmueble.Text = objSolicitudAvaluos.Propietario;
                                this.TextBoxFuncionarioResp.Text = objSolicitudAvaluos.Responsable;

                                //Valida la Institucion
                                this.TextBoxInstitucionJustipreciacion.Text = objSolicitudAvaluos.InstitucionDescripcion;
                                //this.lblNombreInstitucion.Value = QuitarAcentosTexto(objSolicitudAvaluos.InstitucionDescripcion.Replace(" ", ""));

                                //RCA
                                //agregue un split para cuando encuentre un nombre con un prefijo, solo tome los nombres sin los prefijos, para que entren en el metodo de comparar 
                                //abajo dejo la linea de codigo original, solo agregue tres lineas de codigo y modifique la llamada al metodo.
                                //se modifico y se quito el split, reemplazandolo por un content que contiene los espacios en blanco, los - y las comas
                                //this.ComparaInstitucion(this.lblNombreInstitucionActual.Value, this.lblNombreInstitucion.Value); 

                                String content = @"\s-[,]?\s?";
                                string[] InstitucionJustipreciacion = Regex.Split(objSolicitudAvaluos.InstitucionDescripcion, content);
                                string[] NombreInstitucionActual = Regex.Split(this.TextBoxInstitucionActual.Text, content);
                                //RCA
                                //quitamos el replace 
                                //String NombreInstitucionJustipreciacion = QuitarAcentosTexto(InstitucionJustipreciacion[0].Replace(" ", ""));
                                //String NombreInstitoActual = QuitarAcentosTexto(NombreInstitucionActual[0].Replace(" ", ""));
                                String NombreInstitucionJustipreciacion = QuitarAcentosTexto(InstitucionJustipreciacion[0]);
                                String NombreInstitoActual = QuitarAcentosTexto(NombreInstitucionActual[0]);

                                this.ComparaInstitucion(NombreInstitoActual, NombreInstitucionJustipreciacion);

                            }
                            else //no existen los valores numericos de Superficie y monto para validar la normatividad
                            {
                                if (objSolicitudAvaluos.Estatus.ToUpper() != "CONCLUIDO")
                                    Msj = "El secuencial de justipreciación proporcionado con estatus de atención: " + objSolicitudAvaluos.Estatus.ToUpper() + ", aun no cuenta con: <br/> * Monto dictaminado <br/> * Superficie rentable dictaminado ó capturada por el promovente en la solicitud de avalúo <br/> Es necesario que se cuente con esta información para poder registrarlo al contrato, por favor espere a que se concluya el avalúo para poder registrar el contrato de arrendamiento";
                                else //es concluido, pero no tiene los datos basicos para validar normatividad, este caso seria muy extraño que ocurriera
                                    Msj = "El secuencial de justipreciación proporcionado con estatus de atención: " + objSolicitudAvaluos.Estatus.ToUpper() + ", aun no cuenta con: <br/> * Monto dictaminado <br/> * Superficie rentable dictaminado ó capturada por el promovente en la solicitud de avalúo <br/> Es necesario que se cuente con esta información para poder registrarlo al contrato, por favor contacte al Indaabin";
                                this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                //msj sin uso de HTML
                                Msj = Msj = "El secuencial de justipreciación proporcionado con estatus de atención: " + objSolicitudAvaluos.Estatus.ToUpper() + ", aun no cuenta con el Monto Dictaminado y/o la  Superficie Rentable Dictaminado ó capturada por el promovente en la solicitud de avalúo. Es necesario que se cuente con esta información para poder registrarlo";
                                MostrarMensajeJavaScript(Msj);//sin html en la cadena
                                this.LimpiarJustupreciacion();
                                this.LabelInfoSecuencialJust.Focus();
                            }

                            //RCA 11/08/2017
                            //quitamos replace.
                            //this.lblEstadoJustipreciacion.Value = QuitarAcentosTexto(objSolicitudAvaluos.EstadoDescripcion.Replace(" ", "").ToUpper());
                            //this.lblMunicpioJustipreciacion.Value = QuitarAcentosTexto(objSolicitudAvaluos.MunicipioDescripcion.Replace(" ", "").ToUpper());
                            //this.lblCPJustipreciacion.Value = QuitarAcentosTexto(objSolicitudAvaluos.CP.Replace(" ", "").ToUpper());
                            this.lblEstadoJustipreciacion.Value = QuitarAcentosTexto(objSolicitudAvaluos.EstadoDescripcion.ToUpper());
                            this.lblMunicpioJustipreciacion.Value = QuitarAcentosTexto(objSolicitudAvaluos.MunicipioDescripcion.ToUpper());
                            this.lblCPJustipreciacion.Value = QuitarAcentosTexto(objSolicitudAvaluos.CP.ToUpper());

                            if (this.TextBoxFolioOpinion.Text.Trim() != "")
                            {
                                if (this.lblEstadoEmision.Value != this.lblEstadoJustipreciacion.Value || this.lblMunicpioEmision.Value != this.lblMunicpioJustipreciacion.Value || this.lblCPEmision.Value != this.lblCPJustipreciacion.Value)
                                {
                                    Msj = "El secuencial de justipreciación proporcionado no corresponde con el estado, municipio o código postal de la dirección del inmueble de la emisión de opinión proporcionada ¡verifica! "
                                        + " <br/> Entidad Federativa de la justipreciación: " + this.lblEstadoEmision.Value
                                        + " <br/> Muninicipio de la justipreciación: " + this.lblMunicpioEmision.Value
                                        + " <br/> Código postal de la justipreciación: " + this.lblCPEmision.Value;

                                    this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                    MostrarMensajeJavaScript("El secuencial de justipreciación proporcionado no corresponde con el estado, municipio o código postal de la dirección del inmueble de la emisión de opinión proporcionada, verifica.");//sin html en la cadena
                                    this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                                    this.LimpiarJustupreciacion();
                                    this.LabelInfoSecuencialJust.Focus();

                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Msj = "El secuencial de justipreciación proporcionado no pertenece a la institución a la que estás adscrito";
                            this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                            MostrarMensajeJavaScript(Msj);
                            this.LimpiarJustupreciacion();
                            this.TextBoxSecuencialJust.Focus();
                        }
                    }
                    else
                    {
                        Msj = "El secuencial de justipreciación proporcionado tiene un estatus de atención: Cancelado, por lo que es inválido para utilizar, verifica.";
                        this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfo.Text = this.LabelInfoSecuencialJust.Text;
                        MostrarMensajeJavaScript(Msj);//sin html en la cadena
                        this.LimpiarJustupreciacion();
                        this.LabelInfoSecuencialJust.Focus();
                    }
                }
                else
                {
                    // MZT 15/08/2017
                    Msj = "<h4>Estimado Promovente, por favor envía la copia de tu justipreciación de renta al siguiente correo electrónico: <u>" + jefedepto + "</u>, para continuar con el registro de tu contrato.</h4>";

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Notificar! </strong> " + Msj + "</div>";
                    this.LabelInfoSecuencialJust.Text = this.LabelInfo.Text;
                    // MZT 15/08/2017
                    //MostrarMensajeJavaScript(Msj);
                    // MZT 15/08/2017
                    this.LimpiarJustupreciacion();
                    this.TextBoxSecuencialJust.Focus();

                    // MZT 15/08/2017
                    this.NotificarNoExisteJustipreciacion(secuencial);
                }
                //Desocupar objetos
                objSolicitudAvaluos = null;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la Justipreciación. Contacta al área de sistemas.";
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

        private void NotificarNoExisteJustipreciacion(string secuencial)
        {
            string msjPromovente = string.Empty;
            string msgJefeDepto = string.Empty;
            string encabezado = string.Empty;

            try
            {
                SSO sso = ((SSO)Session["Contexto"]);

                string jefedepto = ConfigurationManager.AppSettings["jefeDepot"] == null ? string.Empty : ConfigurationManager.AppSettings["jefeDepot"];



                if (!string.IsNullOrEmpty(jefedepto))
                {
                    StringBuilder sb = new StringBuilder();

                    encabezado = "Sistema de contratos de arrendamiento incidencia de justipreciación no encontrada";

                    msgJefeDepto = string.Concat(msgJefeDepto, string.Format(@"No se encontró la justipreciación: {0} del promovente {1} correspondiente a la institución {2}.",
                       secuencial, sso.Nombre, sso.NombreInstitucion));


                    // notificar a jefe de departamente
                    NG.NotificarPorcorreo(encabezado, msgJefeDepto, jefedepto);


                    sb.Clear();
                    sb.Append(string.Format("<p>Estimando promovente, por favor envia la copia de tu justipreciación de renta al siguiente correo electrónico {0} ", jefedepto));
                    sb.Append(", para continuar con el registro de tu contrato. </p>");
                    msjPromovente = sb.ToString();

                    // notificar a promovente
                    NG.NotificarPorcorreo(encabezado, msjPromovente, sso.Email);
                }
            }
            catch (Exception ex)
            {
                string Msj = "Ha ocurrido un error al recuperar la Justipreciación. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);

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

        private void LimpiarJustupreciacion()
        {
            this.TextBoxPropietarioInmueble.Text = String.Empty;
            this.TextBoxFuncionarioResp.Text = String.Empty;
            this.TextBoxFechaDictamenJust.Text = String.Empty;
            this.TextBoxSupDictaminadaJust.Text = String.Empty;
            this.TextBoxMontoDictaminadoJust.Text = String.Empty;
            this.TextBoxGenericoJust.Text = String.Empty;
            this.TextBoxUnidadMedidaSup.Text = string.Empty;
            this.TextBoxEstatusAttJust.Text = string.Empty;
            this.TextBoxSecuencialJust.Text = string.Empty;
        }

        private void LimpiarEmision()
        {
            this.TextBoxFolioOpinion.Text = String.Empty;
            this.LabelDireccionSustitucion.Text = String.Empty;
            this.LabelTotalSMOI.Text = "00.00";
            this.TextBoxNumDictamenExcepcionSMOI.Text = String.Empty;
        }

        //RCA 11/08/2017
        //modificamos el metodo de quitar acentos para que acepte nullos
        private string QuitarAcentosTexto(string Texto)
        {
            /*string textoNormalizado = Texto.Trim().Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;*/
            if (!string.IsNullOrEmpty(Texto))
            {
                string textoNormalizado = Texto.Trim().Normalize(System.Text.NormalizationForm.FormD);
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
                string textoSinAcentos = reg.Replace(textoNormalizado, "");
                return textoSinAcentos;
            }
            return string.Empty;


        }

        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {
            if (this.ValidaEntradaDatos())
            {
                if (ValidarCtrlEntradaImportes())
                {
                    if (DropDownListTipoContratacion.SelectedValue == "11")
                    {
                        decimal montoMensualCapt = 0;
                        decimal.TryParse(TextBoxMontoPagoMes.Text, out montoMensualCapt);

                        if (montoMensualCapt > MontoMinimo)
                        {
                            Msj = "El monto mensual debe ser menor a $" + MontoMinimo;
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> Atención </strong>" + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                            return;
                        }
                    }
                    // A solicitud del control de cambios del 19 de noviembbre del2019 se incluye
                    // la regla siguiente:
                    // los numeros magicos del segundo switch están definidos en:
                    // [ArrendamientoInmueble].[dbo].[Cat_TipoContratacion]
                    // los numeros magicos del primer switch estan en:
                    // [ArrendamientoInmueble].[dbo].[Cat_TipoArrendamiento].[IdTipoArrendamiento]

                    /*string mensajeNuevo2020 = "El monto registrado rebasa lo establecido en el ACUERDO por el que " +
                                            "se fija el importe máximo de rentas por zonas y tipos de inmuebles, a que " +
                                            "se refiere el artículo 146 de la Ley General de Bienes Nacionales, " +
                                            "publicado en el Diario Oficial de la Federación el 23 de octubre de 2019, " +
                                            "verifique el Artículo que le corresponda.";*/
                    //Cambio 18/12/2019 Mensaje definido por el usuario
                    string mensajeNuevo2020 = @"El monto registrado rebasa lo establecido en el ACUERDO por el que se fija el importe máximo de rentas por zonas y tipos de inmuebles, a que se refiere el artículo 146 de la Ley General de Bienes Nacionales, publicado en el Diario Oficial de la Federación.";

                    switch (DropDownListTipoArrandamiento.SelectedValue)
                    {
                        case "1":
                        case "2":
                        case "3":
                            {
                                decimal montoAnterior2020 = ContratoArrtoAnterior.MontoPagoMensual;
                                decimal montoActual2020 = 0;
                                decimal.TryParse(TextBoxMontoPagoMes.Text, out montoActual2020);
                                decimal montoDictaminado = 0;
                                switch (DropDownListTipoContratacion.SelectedValue)
                                {
                                    case "13":  // Excepción Artículo 3 (Acuerdo de montos, aplicable 2020)
                                        if (montoActual2020 > montoAnterior2020)
                                        {
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> Atención </strong>" + mensajeNuevo2020 + "</div>";
                                            MostrarMensajeJavaScript(mensajeNuevo2020);
                                            return;
                                        }
                                        break;
                                    case "14":  // Excepción Artículo 4 (Acuerdo de montos, aplicable 2020)
                                        decimal.TryParse(TextBoxMontoDictaminadoJust.Text, out montoDictaminado);
                                        if (montoActual2020 > montoAnterior2020 * Convert.ToDecimal(1.0393) ||
                                            montoActual2020 > montoDictaminado)
                                        {
                                            /*Msj = "El monto registrado rebasa el porcentaje del 3.93% del monto de " +     //Cambio 18/12/2019 DGO Cambio de usuario
                                                "la renta mensual antes de impuestos del contrato anterior, por lo que se " +
                                                "deberá solicitar una actualización de justipreciación tradicional o electrónica " +
                                                "y para el registro del contrato se deberá seleccionar, en la sección tipo de contratación, " +
                                                "la opción: Dictaminado.";*/
                                            Msj = @"El importe registrado rebasa el porcentaje del aumento de renta, con respecto al contrato vigente, o inmediato anterior y/o lo señalado en el dictamen valuatorio, por lo que se deberá solicitar la actualización de justipreciación tradicional o electrónica y, para el registro del contrato se deberá seleccionar en la sección tipo de contratación, la opción: Dictaminado.";


                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> Atención </strong>" + Msj + "</div>";
                                            MostrarMensajeJavaScript(Msj);
                                            return;
                                        }
                                        break;
                                    case "15":  // Excepción Artículo 5 (Acuerdo de montos, aplicable 2020)
                                                // bloquear la casilla secuencial
                                        if (montoActual2020 > Convert.ToDecimal(7410.21))
                                        {
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> Atención </strong>" + mensajeNuevo2020 + "</div>";
                                            MostrarMensajeJavaScript(mensajeNuevo2020);
                                            return;
                                        }
                                        break;
                                    default:
                                        break;

                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if (DropDownListTipoArrandamiento.SelectedValue == "2" || DropDownListTipoArrandamiento.SelectedValue == "3")
                    {
                        if (DropDownListTipoContratacion.SelectedValue == "10")
                        {
                            decimal montoAnterior = ContratoArrtoAnterior.MontoPagoMensual;
                            decimal montoActual = 0;

                            decimal.TryParse(TextBoxMontoPagoMes.Text, out montoActual);

                            if (montoAnterior < montoActual)
                            {
                                Msj = "El monto mensual debe ser menor o igual a  $" + montoAnterior + " que es el monto del contrato anterior";
                                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> Atención </strong>" + Msj + "</div>";
                                MostrarMensajeJavaScript(Msj);
                                return;
                            }
                        }
                    }

                    if (this.InsertContratoArrto())
                    {
                        string mensaje = "El contrato ha sido registrado con éxito.";
                        this.LabelInfo.Text += "<div class='alert alert-success'><strong> ¡Felicidades! </strong></br>" + mensaje + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                        this.pnlControles.Enabled = false;
                        this.ButtonverificarNormatividad.Enabled = false;
                        this.ButtonEnviar.Enabled = false;
                        this.ButtonCancelar.Text = "Regresar";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAlerta", "alert(\"" + mensaje + "\");", true);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAcuse", "window.open('AcuseContrato.aspx?IdFolio=" + this.strFolioContrato + "&isInsert=true', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, titlebar = no, width = 1024, height = 650', 'true');", true);
                        this.ButtonEnviar.Enabled = false;
                        this.ButtonverificarNormatividad.Enabled = false;
                    }
                }
            }
        }

        //validar datos de entrada de captura
        private Boolean ValidaEntradaDatos()
        {
            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacion
            string Msj;

            if (DropDownListTipoContratacion.SelectedItem.Text == "--") //
            {

                Msj = "Debes seleccionar un tipo de contratación";
                this.MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListTipoContratacion.Focus();
                return false;
            }

            //si hay contenido de secuencial, entonces validar que ya se obtuvieron los datos de la justipreciacion
            if (this.TextBoxSecuencialJust.Text.Length > 0)
            {
                //si esta vacia la fecha, pero hay algo en la caja del secuencial, significa que el promovente no ha hecho clic en el boton Consultar
                if ((this.TextBoxFechaDictamenJust.Text.Trim().Length == 0) && (this.TextBoxEstatusAttJust.Text.Trim().Length == 0) && (this.TextBoxSupDictaminadaJust.Text.Trim().Length == 0) && (this.TextBoxMontoDictaminadoJust.Text.Trim().Length == 0))
                {
                    Msj = "Si cuentas con el secuencial de justipreciación, entonces da clic en Consultar, para obtener la información de éste.";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfoSecuencialJust.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfoSecuencialJust.Text;

                    this.TextBoxSecuencialJust.Focus();
                    return false;
                }
            }

            if (TextBoxFolioOpinion.Text.Length > 0)
            {
                //si esta vacia la etiqueta, pero hay algo en la caja del FolioSMOI, significa que el promovente no ha hecho clic en el boton Consultar
                if (this.lblEsContinuacion.Value != "1")
                {

                    IdTema = new NGConceptoRespValor().ObtenerIdTema(Convert.ToInt32(TextBoxFolioOpinion.Text));


                    if (IdTema != 8)
                    {
                        if (LabelTotalSMOI.Text.Trim().Length == 0 && TextBoxNumDictamenExcepcionSMOI.Text.Trim().Length == 0)
                        {
                            Msj = "El folio de emisión de opinión ingresado debe contar con una tabla SMOI o un número de dictamen de excepción. Valida que el folio de emisión o dictamen de excepción aplique para un movimiento de arrendamiento Nuevo o Sustitución.";
                            MostrarMensajeJavaScript(Msj);
                            this.LabelDireccionSustitucion.Text = "";
                            this.LabelInfoFolioOpinion.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoEnviar.Text = this.LabelInfoFolioOpinion.Text;
                            this.TextBoxFolioOpinion.Focus();
                            return false;
                        }
                    }


                }

                //verificar si el FOlio de emisión de opinión, no fue aplicado a un Contrato, si ya fue utilizado se lanzara la excepcion
                try
                {
                    AplicacionConcepto oEmision;
                    oEmision = new NGConceptoRespValor().ObtenerEmisionOpinionPorFolio(Convert.ToInt32(TextBoxFolioOpinion.Text), Convert.ToInt32(this.lblIdInstitucion.Value));
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al obtener la emisión de opinión. Contacta al área de sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                    this.TextBoxFolioOpinion.Focus();

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

                    return false;
                }
            }

            if (this.TextBoxPropietarioInmueble.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el propietario del inmueble";
                MostrarMensajeJavaScript(Msj);


                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxPropietarioInmueble.Focus();
                return false;
            }

            if (this.TextBoxFuncionarioResp.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el nombre completo del funcionario responsable.";
                MostrarMensajeJavaScript(Msj);


                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxFuncionarioResp.Focus();
                return false;
            }

            //RCA
            if (this.DropDownListTipoUsoInmueble.SelectedItem.Text == "--")
            {
                Msj = "Debes seleccionar un tipo de uso genérico";
                this.MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListTipoUsoInmueble.Focus();
                return false;
            }

            if (this.DropDownListTipoUsoEspecificoInmueble.SelectedItem.Text == "--" || this.DropDownListTipoUsoEspecificoInmueble.Text == "")
            {
                Msj = "Debes seleccionar un tipo de uso especifíco";
                this.MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListTipoUsoEspecificoInmueble.Focus();
                return false;
            }


            //validacion de seccion de datos de contratacion
            if (TextBoxFechaIOcupacion.Text.Length > 0)
            {
                if (Util.IsDate(TextBoxFechaIOcupacion.Text) == false)
                {
                    Msj = "Debes proporcionar una fecha inicio de ocupación, valida en el formato (dd/mm/aaaa)";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.TextBoxFechaIOcupacion.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar una fecha inicio de ocupación, valida en el formato (dd/mm/aaaa)";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxFechaIOcupacion.Focus();
                return false;
            }

            //validacion de seccion de datos de contratacion
            if (TextBoxFechaFOcupacion.Text.Length > 0)
            {
                //si hay texto en la caja, ver que sea una fecha
                if (Util.IsDate(TextBoxFechaFOcupacion.Text) == false)
                {
                    Msj = "Debes proporcionar una fecha fin de ocupación, valida en el formato (dd/mm/aaaa)";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.TextBoxFechaFOcupacion.Focus();
                    return false;
                }
            }
            else
            {
                Msj = "Debes proporcionar una fecha fin de ocupación, valida en el formato (dd/mm/aaaa)";
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
                //si hay texto en la caja, ver que sea un numero
                if (Util.IsNumeric(this.TextBoxAreaOcupadaM2.Text) == false)
                {
                    Msj = "Debes proporcionar un area de ocupación del arrendamiento correspondiente a los metros cuadrados como un número decimal.";
                    MostrarMensajeJavaScript(Msj);

                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxAreaOcupadaM2.Focus();
                    return false;
                }
                else
                {
                    if (Convert.ToDecimal(this.TextBoxAreaOcupadaM2.Text) <= 0)
                    {
                        Msj = "Debes proporcionar la superficie ocupada en m2 del área de arrendamiento.";
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
                Msj = "Debes proporcionar el porcentaje de impuesto.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxPtjeImpuesto.Focus();
                return false;
            }

            //para el casos de contratos en el extranjero se habilita la lista para seleccionar una moneda
            if (this.DropDownListTipoMonedaPago.SelectedItem.Text == "--")
            {

                Msj = "Seleccione la moneda con el que se realiza el pago de la renta.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListTipoMonedaPago.Focus();
                return false;


            }


            //datos de referencias de personas

            if (this.TextBoxNombreTitularOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el nombre del titular del OIC para su Institución.";
                MostrarMensajeJavaScript(Msj);


                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxNombreTitularOIC.Focus();
                return false;
            }

            if (this.TextBoxApPatOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el primer apellido del titular del OIC para su Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxApPatOIC.Focus();
                return false;
            }

            if (this.TextBoxNombreCargoOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el cargo del titular del OIC para su Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxNombreCargoOIC.Focus();
                return false;
            }

            if (this.TextBoxEmailOIC.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el correo electrónico del titular del OIC para su Institución.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailOIC.Focus();
                return false;
            }

            if (Regex.IsMatch(this.TextBoxEmailOIC.Text, @"[\w-]+@([\w-]+\.)+[\w-]+") == false)
            {
                Msj = "Debes proporcionar su correo electrónico válido.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailOIC.Focus();
                return false;
            }


            //Datos del Capturista
            if (this.TextBoxNombreCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu nombre.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxNombreCapturista.Focus();
                return false;
            }

            if (this.TextBoxApPatCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu primer apellido.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxApPatCapturista.Focus();
                return false;
            }

            if (this.TextBoxCargoCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu cargo.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxCargoCapturista.Focus();
                return false;
            }

            if (this.TextBoxEmailCapturista.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar tu correo electrónico institucional.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailCapturista.Focus();
                return false;
            }



            if (Regex.IsMatch(this.TextBoxEmailCapturista.Text, @"[\w-]+@([\w-]+\.)+[\w-]+") == false)
            {
                Msj = "Debes proporcionar tu correo electrónico válido.";
                MostrarMensajeJavaScript(Msj);

                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                this.TextBoxEmailCapturista.Focus();
                return false;
            }

            //Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
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
                Msj = "Debes proporcionar el importe de pago mensual del arrendamiento.";
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
                Msj = "Debes proporcionar el importe de pago mensual por mantenimiento del arrendamiento.";
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

        private Boolean InsertContratoArrto()
        {
            Boolean Ok = false;
            try
            {
                //Determinar tipo de Contrato: Nac, Extranjero ó Otras Fig. Ocupacion
                Byte IdTipoContrato = 0;
                Control LabelPais = this.ctrlDireccionLectura.FindControl("LabelPais");
                string NombrePais = ((Label)LabelPais).Text;
                if (NombrePais == "MÉXICO")
                    IdTipoContrato = 1; //Nacional
                else
                    IdTipoContrato = 2; //Extranjero

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
                string DireccionInmueble;
                int IdInmuebleArrendamiento;
                if (this.lblEsSustitucion.Value == "1")
                {
                    IdInmuebleArrendamiento = Convert.ToInt32(this.lblIdInmuebleArrendamiento.Value);
                    DireccionInmueble = this.lblDireccionInmuebleArrendamiento.Value;
                }
                else
                {
                    Control LabelDireccion = this.ctrlDireccionLectura.FindControl("LabelDireccion");
                    Control LabelIdInmuebleArrto = this.ctrlDireccionLectura.FindControl("LabelIdInmuebleArrto");
                    IdInmuebleArrendamiento = Convert.ToInt32(((Label)LabelIdInmuebleArrto).Text);
                    DireccionInmueble = ((Label)LabelDireccion).Text;
                }

                Control LabelInstitucion = this.ctrlUsuarioInfo.FindControl("LabelInstitucion");
                string InstitucionUsr = ((Label)LabelInstitucion).Text;

                int? FolioEmisionOpinion = null;
                if (this.TextBoxFolioOpinion.Text.Length > 0)
                    FolioEmisionOpinion = Convert.ToInt32(this.TextBoxFolioOpinion.Text);

                //Id de Contrato, para los casos de contrato del tipo: Renovacion o Continuacion
                int? NumContratoHist_Padre = null;
                int? FolioContratoPadre = null;

                if (this.DropDownListTipoArrandamiento.SelectedItem.Text != "Nuevo")
                {

                    FolioContratoPadre = Convert.ToInt32(this.lblIdContrato.Value);
                }

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
                objContratoArrto.Fk_IdTipoContrato = IdTipoContrato; //Nacional = 1 , extranjero = 2 , (solo arrendamientos)
                objContratoArrto.Fk_IdTipoArrendamiento = Convert.ToByte(this.DropDownListTipoArrandamiento.SelectedValue);
                objContratoArrto.Fk_IdTipoContratacion = Convert.ToByte(this.DropDownListTipoContratacion.SelectedValue);
                objContratoArrto.Fk_IdInmuebleArrendamiento = IdInmuebleArrendamiento; // Convert.ToInt32(Session["IdInmuebleArrto"].ToString());
                objContratoArrto.Fk_NumContratoHistorico = NumContratoHist_Padre;
                objContratoArrto.Fk_IdContratoArrtoPadre = FolioContratoPadre;
                objContratoArrto.Fk_IdTipoUsoInm = iTipoUsoInmueble.Value;
                objContratoArrto.OtroUsoInmueble = strOtroUsoInmueble;
                objContratoArrto.Fk_IdTipoOcupacion = null; //este cpto solo aplica a otras Fig. de ocupacion
                objContratoArrto.OtroTipoOcupacion = null; //este cpto solo aplica a otras Fig. de ocupacion
                objContratoArrto.Fk_IdTipoMoneda = Convert.ToInt32(this.DropDownListTipoMonedaPago.SelectedValue);
                objContratoArrto.Fk_IdInstitucion = Convert.ToInt32(this.lblIdInstitucion.Value); // ((SSO)Session["Contexto"]).IdInstitucion.Value;
                objContratoArrto.NombreInstitucion = ((SSO)Session["Contexto"]).NombreInstitucion;
                objContratoArrto.FechaInicioOcupacion = Convert.ToDateTime(this.TextBoxFechaIOcupacion.Text.Trim());
                objContratoArrto.FechaFinOcupacion = Convert.ToDateTime(this.TextBoxFechaFOcupacion.Text.Trim());
                objContratoArrto.AreaOcupadaM2 = Convert.ToDecimal(this.TextBoxAreaOcupadaM2.Text);
                objContratoArrto.MontoPagoMensual = Convert.ToDecimal(this.TextBoxMontoPagoMes.Text);
                objContratoArrto.MontoPagoPorCajonesEstacionamiento = Convert.ToDecimal(this.TextBoxMontoPagoEstacionamiento.Text);
                objContratoArrto.CuotaMantenimiento = Convert.ToDecimal(this.TextBoxCuotaMantenimiento.Text);
                objContratoArrto.PtjeImpuesto = Convert.ToDecimal(this.TextBoxPtjeImpuesto.Text);
                objContratoArrto.FolioEmisionOpinion = FolioEmisionOpinion; //opcional

                // estructura de seguridad
                objContratoArrto.CuentaConDictamen = CuentaConDictamenSeguridad;
                objContratoArrto.FechaDictamen = fechaDictamen;

                //nulificar si no hay valores, porque los textbox vacios, no contienen nulo
                if (string.IsNullOrEmpty(this.TextBoxNumDictamenExcepcionSMOI.Text))
                    objContratoArrto.NumeroDictamenExcepcionFolioSMOI = null;
                else
                    objContratoArrto.NumeroDictamenExcepcionFolioSMOI = this.TextBoxNumDictamenExcepcionSMOI.Text.Trim();

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

                objContratoArrto.PropietarioInmueble = this.TextBoxPropietarioInmueble.Text.Trim();
                objContratoArrto.FuncionarioResponsable = this.TextBoxFuncionarioResp.Text.Trim();

                objContratoArrto.Fk_IdUsuarioRegistro = Convert.ToInt32(((SSO)Session["Contexto"]).IdUsuario); //el usuario del SSO;
                objContratoArrto.CargoUsuarioRegistro = ((SSO)Session["Contexto"]).Cargo; //SSO (nombre del cargo)



                //objetos de Persona Referencia

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
                    NombreCargo = this.TextBoxCargoCapturista.Text.Trim(),
                    Nombre = this.TextBoxNombreCapturista.Text.Trim(),
                    ApellidoPaterno = this.TextBoxApPatCapturista.Text.Trim(),
                    ApellidoMaterno = this.TextBoxApMatCapturista.Text.Trim(),
                    Email = this.TextBoxEmailCapturista.Text.Trim()
                };

                //Objeto de Justipreciacion, si existen los datos crear el objeto, si no, dejar en nulo
                if (this.TextBoxSecuencialJust.Text.Length > 0)
                {
                    objContratoArrto.JustripreciacionContrato = new JustripreciacionContrato();
                    objContratoArrto.JustripreciacionContrato.Secuencial = this.TextBoxSecuencialJust.Text.Trim();
                    objContratoArrto.JustripreciacionContrato.SuperficieDictaminada = this.TextBoxSupDictaminadaJust.Text.Trim();
                    if (this.TextBoxFechaDictamenJust.Text.Trim() == "")
                        objContratoArrto.JustripreciacionContrato.FechaDictamen = null;
                    else
                        objContratoArrto.JustripreciacionContrato.FechaDictamen = Convert.ToDateTime(this.TextBoxFechaDictamenJust.Text.Trim());
                    objContratoArrto.JustripreciacionContrato.MontoDictaminado = Convert.ToDecimal(this.TextBoxMontoDictaminadoJust.Text);
                    objContratoArrto.JustripreciacionContrato.EstatusAtencion = this.TextBoxEstatusAttJust.Text.Trim();
                    objContratoArrto.JustripreciacionContrato.NoGenerico = this.TextBoxGenericoJust.Text.Trim();
                    objContratoArrto.JustripreciacionContrato.UnidadMedidaSupRentableDictaminada = this.TextBoxUnidadMedidaSup.Text.Trim();
                }
                else
                    //solo instanciar, para que sus propiedades esten en nulo
                    objContratoArrto.JustripreciacionContrato = new JustripreciacionContrato();

                objContratoArrto.CadenaOriginal = CadenaOriginal;
                objContratoArrto.SelloDigital = SelloDigital;



                //Se inserta el Contrato en el Sistema
                int iFolioContrato = new NG_ContratoArrto().InsertContratoArrto(objContratoArrto);
                if (iFolioContrato > 0)
                {
                    string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;
                    int iIdInmuebleArrto = System.Convert.ToInt32(this.lblIdInmuebleArrendamiento.Value);

                    //RCA 13/08/2018
                    string UrlAbrirQRContrato = "AcuseContrato.aspx?IdFolio=" + iFolioContrato + "&isInsert=true";
                    Control LabelNombreContrato = this.ctrlUsuarioInfo.FindControl("LabelUsr");
                    string NombreUsuarioContrato = ((Label)LabelNombreContrato).Text;

                    //RCA 13/08/2018
                    //obtenemos el QR el tipo 4 es para contratos
                    string QR = UtilContratosArrto.GenerarCodigoQR(iFolioContrato.ToString(), 4, NombreUsuarioContrato, UrlAbrirQRContrato);

                    //obtenemos el idaplicacion dl contrato 
                    int IdAplicacionContrato = new NG_ContratoArrto().IdContrato(iFolioContrato.ToString());



                    // Si se genera el contrato, se genera el RIUF de ser necesario
                    if (this.CheckBoxGenerarRIUF.Checked)
                    {
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
                        int EsOtraFiguraOcupacion = 0; //0 Se trata de Otra Figura de Ocupacion 1 Es un contrato de arrendamiento normal
                        int iIdInmueble = new Negocio.NG_Inmueble().InsertInmueble(strConnectionString, objInmueble, EsOtraFiguraOcupacion);
                        if (iIdInmueble > 0)
                        {
                            string sRIUF = objInmueble.RIUF.RIUF1;
                            int iInstitucion = Convert.ToInt32(this.lblIdInstitucion.Value);

                            int InmmuebleActualizado;
                            InmmuebleActualizado = new Negocio.NG_InmuebleArrto().UpdateIdInmuebleByIdInmuebleArrendamiento(iIdInmueble, iIdInmuebleArrto);
                            int ContratoActualizado;
                            ContratoActualizado = new Negocio.NG_ContratoArrto().UpdateRIUFByFolioContrato(sRIUF, iFolioContrato, iInstitucion);

                            this.TextBoxRIUF.Text = sRIUF;
                        }
                    }
                    else
                    {
                        if (this.lblEsContinuacion.Value != "1")
                        {
                            int iIdInmueble = Convert.ToInt32(this.lblIdInmuebleRiuf.Value);
                            if (iIdInmueble > 0)
                            {
                                string sRIUF = this.TextBoxRIUF.Text.Trim();
                                int iInstitucion = Convert.ToInt32(this.lblIdInstitucion.Value);

                                int InmmuebleActualizado;
                                InmmuebleActualizado = new Negocio.NG_InmuebleArrto().UpdateIdInmuebleByIdInmuebleArrendamiento(iIdInmueble, iIdInmuebleArrto);
                                int ContratoActualizado;
                                ContratoActualizado = new Negocio.NG_ContratoArrto().UpdateRIUFByFolioContrato(sRIUF, iFolioContrato, iInstitucion);

                                this.TextBoxRIUF.Text = sRIUF;
                            }
                        }
                    }
                    this.strFolioContrato = iFolioContrato.ToString();
                    objContratoArrto = null;


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
                    Msj = "No fue posible registrar el Contrato, por favor vuelva a intentar ó reporte a Sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    MostrarMensajeJavaScript(Msj);
                    this.LabelInfoEnviar.Focus();
                    objContratoArrto = null;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                //TODO: implementar registro de Bitacora de Excepciones
                Msj = "Ocurrió una excepción al procesar el registro de la información del contrato, por favor vuelva a intentar ó reporte a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.LabelInfoEnviar.Focus();

                //Registra informacion acerca de una excepcion
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

                //msj al usuario
                Msj = "Ocurrió una excepción al procesar el registro de la información del contrato, por favor vuelva a intentar ó reporte a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.LabelInfoEnviar.Focus();

                //Registra informacion acerca de una excepcion
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
            Response.Redirect("~/InmuebleArrto/BusqMvtosContratosInmuebles.aspx");
        }

        protected void ButtonCancelarModal_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/InmuebleArrto/BusqMvtosContratosInmuebles.aspx");
        }

        protected void TextBoxMontoPagoMes_TextChanged(object sender, EventArgs e)
        {

            if (this.TextBoxMontoPagoMes.Text.Trim().Length == 0)
                this.TextBoxMontoPagoMes.Text = "0.00";

            this.ObtenerTotalRenta();
            //this.TextBoxCuotaMantenimiento.Focus();
        }

        protected void TextBoxMontoPagoEstacionamiento_TextChanged(object sender, EventArgs e)
        {

            if (this.TextBoxMontoPagoEstacionamiento.Text.Trim().Length == 0)
                this.TextBoxMontoPagoEstacionamiento.Text = "0.00";

            this.ObtenerTotalRenta();
            //this.TextBoxPtjeImpuesto.Focus();
        }

        protected void TextBoxCuotaMantenimiento_TextChanged(object sender, EventArgs e)
        {

            if (this.TextBoxCuotaMantenimiento.Text.Trim().Length == 0)
                this.TextBoxCuotaMantenimiento.Text = "0.00";

            this.ObtenerTotalRenta();
            //this.TextBoxMontoPagoEstacionamiento.Focus();
        }

        //suma los tres valores de cuotas de arrendamiento para obtener un gran total
        private void ObtenerTotalRenta()
        {
            //validar que sean numeros y obtener el totalAcumlado
            if (ValidarCtrlEntradaImportes())
            {
                decimal TotalMontoPago = 0, TotalMontoPagoMes = 0;

                TotalMontoPagoMes = Convert.ToDecimal(this.TextBoxMontoPagoMes.Text);
                TotalMontoPago = Convert.ToDecimal(this.TextBoxMontoPagoMes.Text);
                TotalMontoPago += Convert.ToDecimal(this.TextBoxCuotaMantenimiento.Text);
                TotalMontoPago += Convert.ToDecimal(this.TextBoxMontoPagoEstacionamiento.Text);
                this.TextBoxTotalRenta.Text = String.Format("{0:C}", TotalMontoPago); //exponer en la vista            

                MostrarAvisosExcepcionNormatividad();
            }
        }

        //en función de ciertos valores ingresados en la vista, se llama a este metodo, que expone mensajes de avisos de excepcion de la normatividad al usuario
        private void MostrarAvisosExcepcionNormatividad()
        {
            //**  Exposicion de Avisos de Normatividad  ******

            bool OkConversion; //esta nos dice si es un número válido


            Msj = string.Empty;
            decimal TotalMontoPagoRentaMes = Util.ToDecimal(this.TextBoxTotalRenta.Text);

            //RCA 29/05/2018
            //monto de pagomensual sin impuesto
            decimal TotalMontoPagoMesSinImpuesto = Util.ToDecimal(this.TextBoxMontoPagoMes.Text);

            //1. Warning al usuario: se ha rebasado el monto minimo para tener secuencial de justipreciacion

            if (TotalMontoPagoRentaMes > 0)
            {
                decimal ValorMinRentaJustipreciacion; //variable para leer el número (cargado al load de la vista)  
                OkConversion = Decimal.TryParse(this.LabelMontoMinimoRentaParaJustipreciacion.Text, out ValorMinRentaJustipreciacion); //almacenamos true si la conversión se realizó

                if (OkConversion) //este if es solo para mostrar un mensaje de error si no se realizó la conversión
                {

                    //el importe de renta se rebasa el parametro de valor minimo de justipreciacion y  no se ha proporcionaod un secuencial de justipreacion; alertar 
                    if ((TotalMontoPagoRentaMes > ValorMinRentaJustipreciacion) && this.TextBoxSupDictaminadaJust.Text.Length == 0)
                    {

                        Msj = "Recomendación de la normatividad: De acuerdo al monto de pago renta mensual (sin impuestos) proporcionado, debe contar con un secuencial de la justipreciación de renta, por favor si cuentas con él, proporciónalo arriba.";
                        //this.LabelInfoImporteRenta.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        //MostrarMensajeJavaScript(Msj);

                    }

                }
            }//excepcion normatividad #1


            //2. Warning al usuario: Existe Secuencial de Justipreciación, pero el pago de Monto Total Renta Unitaria, rebasa el Monto Dictaminado  de la Justipreciación  
            if (this.TextBoxMontoDictaminadoJust.Text.Length > 0)
            {
                //si hay monto dictaminado de justipreciacion, aplicar la validacion contra el importe de renta que se registra
                //if (TotalMontoPagoRentaMes > Convert.ToDecimal(this.TextBoxMontoDictaminadoJust.Text))

                //RCA 29/05/2018
                //modificacion en la validacion del total de la renta con la justipreciacion
                if (TotalMontoPagoMesSinImpuesto > Convert.ToDecimal(this.TextBoxMontoDictaminadoJust.Text))
                {

                    //acumular Msj al warning
                    Msj += "<br/> Recomendación de la normatividad: El monto de pago renta mensual ingresado (sin impuestos) es mayor al monto dictaminado de la justipreciación de renta";

                }
            }//excepcion normatividad #2


            //3. Warning al usuario:: Existe Secuencial de Justipreciación, pero el AreaOcupadaM2 proporcionado es mayor que la Superficie de m2 Dictaminada de la Justipreciación 

            //se ha proporcionado Justripreciacion y el area ocupada del contrato
            if ((this.TextBoxAreaOcupadaM2.Text.Trim().Length > 0) && (this.TextBoxSupDictaminadaJust.Text.Length > 0))
            {
                decimal AreaOcuapadaM2, SupM2DictaminadaJustipreciacion;

                //convertir valor ingresado a decimal
                OkConversion = Decimal.TryParse(this.TextBoxAreaOcupadaM2.Text, out AreaOcuapadaM2); //almacenamos true si la conversión se realizó
                if (OkConversion)
                {

                    //convertir el area m2 dictaminada a decimal
                    OkConversion = Decimal.TryParse(this.TextBoxSupDictaminadaJust.Text, out SupM2DictaminadaJustipreciacion);
                    if (OkConversion)
                    {
                        //el monto de area proporcionado es > la supM2 dictaminada en la justipreciacion
                        if (AreaOcuapadaM2 > SupM2DictaminadaJustipreciacion)
                        {

                            //acumular Msj al warning
                            Msj += "<br/> Recomendación de la normatividad: el área ocuapada de M2, proporcionada es mayor a la superficie de M2 dictaminado en la justipreciación de renta";

                        }
                    }
                }

            }//excepcion normatividad #3
            //exponer msj al usuario en la vista
            if (Msj.Length > 0)
                this.LabelInfoDatosContratacion.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
        }

        protected void ButtonIrRegistrarOpinion_Click(object sender, EventArgs e)
        {
            //lleva el IdInmueble seleccionado, por lo que es viable ir a esta vista
            Response.Redirect("~/EmisionOpinion/BusqOpinion.aspx");
        }

        protected void ButtonConsultarFolioOpinion_Click(object sender, EventArgs e)
        {
            if (this.TextBoxFolioOpinion.Text.Trim() != "")
            {
                this.RecuperaFolioOpinion(this.lblIdInstitucion.Value, this.TextBoxFolioOpinion.Text);
            }
            else
            {
                Msj = "si cuentas con el folio de emisión de opinión, proporcionalo y vuelve a hacer clic en Consultar, para obtener la información de éste"; ;
                this.LabelInfoEnviar.Text = "<div class='alert alert-warning'><strong>¡Precaución!</strong> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
                LabelInfoFolioOpinion.Text = this.LabelInfoEnviar.Text;
                this.TextBoxFolioOpinion.Focus();
            }
        }

        private void RecuperaFolioOpinion(string IdInstitucion, string IdEmisionOpinion, bool validaEmisionOcupada = true)
        {
            this.ButtonSinFolioOpinion.Visible = false;
            this.LabelTotalSMOI.Text = String.Empty;
            this.LabelInfo.Text = "";
            this.LabelDireccionSustitucion.Text = "";
            this.LabelInfoFolioOpinion.Text = "";
            int FolioOpinion = Convert.ToInt32(IdEmisionOpinion);
            int Institucion = Convert.ToInt32(IdInstitucion);

            try
            {
                if (FolioOpinion <= 0)
                {
                    return;
                }

                //RCA 16/04/2018
                //ocultamos los datos de la tabla smoi cuando es una continuacion
                if (this.lblEsContinuacion.Value == "1")
                {
                    DatosTablaSMOI.Visible = false;
                }

                // Se recupera la emisión de opinión y su Inmueble relacionado
                AplicacionConcepto oEmision = new NGConceptoRespValor().ObtenerEmisionOpinionPorFolio(FolioOpinion, Institucion);

                IdTema = new NGConceptoRespValor().ObtenerIdTema(FolioOpinion);

                //RCA 09/08/2018
                if (!string.IsNullOrEmpty(oEmision.FolioSAEF))
                {
                    // Caso SUSTITUCION: El inmueble debera ser diferente y la bandera de Sustitucion = 1
                    if (this.lblEsSustitucion.Value == "1")
                    {

                        int IdInmuebleArrto = System.Convert.ToInt32(oEmision.InmuebleArrto.IdInmuebleArrendamiento.ToString().Trim());
                        ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrto);

                        // Se guardan los valores del Inmueble sustituto
                        this.lblIdInmuebleArrendamiento.Value = IdInmuebleArrto.ToString();
                        if (QuitarAcentosTexto(objInmuebleArrto.NombrePais).ToUpper() == "MEXICO")
                        {
                            //RCA 11/08/2017
                            //quitamos replace
                            //this.lblEstadoEmision.Value = QuitarAcentosTexto(objInmuebleArrto.NombreEstado).Replace(" ", "").ToUpper();
                            //this.lblMunicpioEmision.Value = QuitarAcentosTexto(objInmuebleArrto.NombreMunicipio).Replace(" ", "").ToUpper();
                            //this.lblCPEmision.Value = QuitarAcentosTexto(objInmuebleArrto.CodigoPostal).Replace(" ", "").ToUpper();
                            this.lblEstadoEmision.Value = QuitarAcentosTexto(objInmuebleArrto.NombreEstado).ToUpper();
                            this.lblMunicpioEmision.Value = QuitarAcentosTexto(objInmuebleArrto.NombreMunicipio).ToUpper();
                            this.lblCPEmision.Value = QuitarAcentosTexto(objInmuebleArrto.CodigoPostal).ToUpper();
                        }
                        else
                        {
                            //RCA 11/08/2017
                            //quitamos replace
                            //this.lblEstadoEmision.Value = QuitarAcentosTexto(objInmuebleArrto.EstadoExtranjero).Replace(" ", "").ToUpper();
                            //this.lblMunicpioEmision.Value = QuitarAcentosTexto(objInmuebleArrto.MunicipioExtranjero).Replace(" ", "").ToUpper();
                            //this.lblCPEmision.Value = QuitarAcentosTexto(objInmuebleArrto.CodigoPostalExtranjero).Replace(" ", "").ToUpper();
                            this.lblEstadoEmision.Value = QuitarAcentosTexto(objInmuebleArrto.EstadoExtranjero).Replace(" ", "").ToUpper();
                            this.lblMunicpioEmision.Value = QuitarAcentosTexto(objInmuebleArrto.MunicipioExtranjero).ToUpper();
                            this.lblCPEmision.Value = QuitarAcentosTexto(objInmuebleArrto.CodigoPostalExtranjero).ToUpper();
                        }

                        if (this.TextBoxSecuencialJust.Text.Trim() != "")
                        {
                            if (this.lblEstadoEmision.Value != this.lblEstadoJustipreciacion.Value || this.lblMunicpioEmision.Value != this.lblMunicpioJustipreciacion.Value || this.lblCPEmision.Value != this.lblCPJustipreciacion.Value)
                            {
                                Msj = "El folio de emisión proporcionado no corresponde con el estado, municipio o código postal de la dirección del inmueble de la justipreciación proporcionada ¡verifica! "
                                    + " <br/> Entidad Federativa de la emisión: " + this.lblEstadoEmision.Value
                                    + " <br/> Muninicipio de la emisión: " + this.lblMunicpioEmision.Value
                                    + " <br/> Código postal de la emisión: " + this.lblCPEmision.Value;

                                this.LabelInfoFolioOpinion.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                MostrarMensajeJavaScript("El folio de emisión proporcionado no corresponde con el estado, municipio o código postal de la dirección del inmueble de la justipreciación proporcionada, verifica.");//sin html en la cadena
                                this.LabelInfo.Text = this.LabelInfoFolioOpinion.Text;
                                this.LimpiarEmision();
                                this.LabelInfoFolioOpinion.Focus();
                                return;
                            }
                        }

                        this.lblDireccionInmuebleArrendamiento.Value = objInmuebleArrto.DireccionCompleta;
                        this.RecuperaRIUFInmueble(this.lblIdInmuebleArrendamiento.Value);
                        this.LabelDireccionSustitucion.Text = "<div class='alert alert-warning' style='font-size:small'><p>Dirección actual del inmueble:  </p> " + this.ctrlDireccionLectura.DireccionInmuebleActual;
                        this.LabelDireccionSustitucion.Text += "<p>Dirección de sustitución del Inmueble:  </p> " + objInmuebleArrto.DireccionCompleta + "</div>";
                        this.LabelInfoFolioOpinion.Text = "";

                        if (IdTema != 8)
                        {
                            if (oEmision.SupM2XSMOI > 0)
                            {
                                this.LabelTotalSMOI.Text = oEmision.SupM2XSMOI.ToString() + " m2";
                                //Msj = "Se identificó el folio de opinión con su respectivo folio SMOI (se cumple con la Normatividad)";
                                Msj = "Se identificó el folio de opinión con su respectivo folio SMOI";
                                this.LabelInfoFolioOpinion.Text = "<div class='alert alert-info'> <strong> ¡Felicidades! </strong>" + Msj + "</div>";
                            }
                            else
                            {
                                Msj = "Se identificó el folio de opinión, pero sin SMOI, si cuentas con el número de dictamen de excepción, proporciónalo a continuación, de otra manera omita ";
                                this.LabelInfoFolioOpinion.Text = "<div class='alert alert-warning'> <strong>¡Precaución!</strong>" + Msj + "</div>";
                            }
                        }




                        return;
                    }//fin del label de sustitucion

                    // Caso CONTINUACION y NUEVO: El inmueble debera ser el mismo y la bandera de Continuacion = 1
                    //if (this.lblEsContinuacion.Value == "1")
                    //{
                    if ((oEmision.InmuebleArrto.IdInmuebleArrendamiento.ToString().Trim() == this.lblIdInmuebleArrendamiento.Value.Trim()))
                    {
                        int IdInmuebleArrto = System.Convert.ToInt32(oEmision.InmuebleArrto.IdInmuebleArrendamiento.ToString().Trim());
                        ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmuebleArrto);



                        // Se guardan los valores del Inmueble sustituto
                        this.lblIdInmuebleArrendamiento.Value = IdInmuebleArrto.ToString();
                        this.lblDireccionInmuebleArrendamiento.Value = objInmuebleArrto.DireccionCompleta;



                        this.RecuperaRIUFInmueble(this.lblIdInmuebleArrendamiento.Value);
                        this.LabelDireccionSustitucion.Text = "";
                        this.LabelInfoFolioOpinion.Text = "";
                        if (oEmision.SupM2XSMOI > 0)
                        {
                            this.LabelTotalSMOI.Text = oEmision.SupM2XSMOI.ToString() + " m2";
                            //Msj = "Se identificó el folio de opinión con su respectivo folio SMOI (se cumple con la Normatividad)";
                            Msj = "Se identificó el folio de opinión con su respectivo folio SMOI";
                            this.LabelInfoFolioOpinion.Text = "<div class='alert alert-info'> <strong> ¡Felicidades! </strong>" + Msj + "</div>";
                        }
                        else
                        {
                            if (this.lblEsContinuacion.Value != "1" && IdTema == 2)
                            {
                                Msj = "Se identificó el folio de opinión, pero sin SMOI, si cuentas con el número de dictamen de excepción, proporciónalo a continuación, de otra manera omita ";
                                this.LabelInfoFolioOpinion.Text = "<div class='alert alert-warning'> <strong>¡Precaución!</strong>" + Msj + "</div>";
                            }
                        }
                        return;
                    }
                    else
                    {
                        this.LimpiarEmision();
                        this.LabelDireccionSustitucion.Text = "";
                        Msj = "El folio de opinión " + IdEmisionOpinion + ", no pertenece al inmueble seleccionado para el Contrato";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong>¡Precaución!</strong> " + Msj + "</div>";
                        this.LabelInfoFolioOpinion.Text = this.LabelInfo.Text;
                        MostrarMensajeJavaScript(Msj);
                    }

                }
                else
                {
                    this.LimpiarEmision();
                    this.LabelDireccionSustitucion.Text = "";
                    Msj = "El folio de opinión " + IdEmisionOpinion + ", no cuenta con un folio de accesibilidad";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong>¡Precaución!</strong> " + Msj + "</div>";
                    this.LabelInfoFolioOpinion.Text = this.LabelInfo.Text;
                    MostrarMensajeJavaScript(Msj);
                }


                return;

            }
            catch (Exception ex)
            {
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

                if (Msj.IndexOf("No existe registro para este folio de emisión asociado a la institución a la que está adscrito, verifica.") > -1)
                {
                    ButtonSinFolioOpinion.Visible = true;
                }
                else if (Msj.IndexOf("ya no es vigente", 0) > -1)
                    ButtonSinFolioOpinion.Visible = true;
                else
                    if (Msj.IndexOf("No existe registro", 0) > -1)
                    ButtonSinFolioOpinion.Visible = true;
                else
                        if (Msj.IndexOf("ya fue aplicado", 0) > -1)
                {
                    //this.TextBoxFolioOpinion.Text = "";
                    //this.LimpiarEmision();
                    if ((this.lblEsSustitucion.Value == "1" || this.lblEsContinuacion.Value == "1") && validaEmisionOcupada == false)
                        return;
                    else
                        this.LimpiarEmision();
                }
                else
                {
                    Msj = "Ha ocurrido un error al recuperar los datos del folio de emisión de opinión. Contacta al área de sistemas.";
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
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong>¡Precaución!</strong> " + Msj + "</div>";
                this.LabelInfoFolioOpinion.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
            }
        }

        protected void ButtonSinFolioOpinion_Click(object sender, EventArgs e)
        {
            //limieza
            ButtonSinFolioOpinion.Visible = false;
            TextBoxFolioOpinion.Text = string.Empty;
            this.LabelInfoEnviar.Text = String.Empty;
            this.LabelInfo.Text = String.Empty;
            Msj = "Si no cuentas con el folio de emisión de opinión, proporciona el dictamen de excepción o continua si no cuentas con él. O puedes solicitarlo y continuar después con la captura del contrato de arrendamiento.";
            this.LabelInfoFolioOpinion.Text = "<div class='alert alert-warning'><strong>¡Precaución!</strong> " + Msj + "</div>";
            this.TextBoxNumDictamenExcepcionSMOI.Focus();
        }

        //Obtiene las Excepciones a la normativa, de lo capturado, previo al envio
        protected void ButtonverificarNormatividad_Click(object sender, EventArgs e)
        {
            //bajar valores capturados a varibles locales
            if (this.ValidarCtrlEntradaImportes())
            {
                //area ocupada de arrto.
                if (this.TextBoxAreaOcupadaM2.Text.Trim().Length > 0)
                {
                    //si hay texto en la caja, ver que sea un numero
                    if (Util.IsNumeric(this.TextBoxAreaOcupadaM2.Text) == false)
                    {
                        Msj = "Debes proporcionar un área de ocupación del arrendamiento correspondiente a los  metros cuadrados como un número decimal.";
                        MostrarMensajeJavaScript(Msj);

                        this.LabelInfo.Text = "<div class='alert alert-warning'>" + "<p>¡Información requerida!</p> " + Msj + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                        this.TextBoxAreaOcupadaM2.Focus();
                        return; //salir
                    }
                    else
                    {
                        if (Convert.ToDecimal(this.TextBoxAreaOcupadaM2.Text) < 5)
                        {
                            Msj = "Debes proporcionar la superficie ocupada en m2 del área de arrendamiento";
                            MostrarMensajeJavaScript(Msj);

                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong>¡Precaución!</strong> " + Msj + "</div>";
                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                            this.TextBoxAreaOcupadaM2.Focus();
                            return; //salir
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
                    return; //salir
                }



                string MsjExcepcionNormativa = null;
                decimal AreaOcupadaM2 = 0, RentaMensualUnitaria = 0;
                decimal? MontoDictaminado_Justipreciacion = null;
                int? FolioEmisionOpinion = null;
                String SuperficieDictaminada_Justipreciacion = null, NumeroDictamenExcepcionFolioSMOI = null;

                RentaMensualUnitaria = Util.ToDecimal(this.TextBoxTotalRenta.Text);

                AreaOcupadaM2 = Convert.ToDecimal(this.TextBoxAreaOcupadaM2.Text);


                if (this.TextBoxNumDictamenExcepcionSMOI.Text.Length > 0)
                    NumeroDictamenExcepcionFolioSMOI = this.TextBoxNumDictamenExcepcionSMOI.Text;
                if (this.TextBoxFolioOpinion.Text.Length > 0)
                    FolioEmisionOpinion = Convert.ToInt32(this.TextBoxFolioOpinion.Text);
                if (this.TextBoxMontoDictaminadoJust.Text.Length > 0)
                    MontoDictaminado_Justipreciacion = Convert.ToDecimal(this.TextBoxMontoDictaminadoJust.Text);
                if (this.TextBoxSupDictaminadaJust.Text.Length > 0)
                    SuperficieDictaminada_Justipreciacion = this.TextBoxSupDictaminadaJust.Text;

                try
                {


                    //ejecutar la operacion a la capa de Negocio y de ahi a la DAL
                    //1=Contrato Nacional, esta validacion de excepcion a la normatividad solo aplica a contratos Nacionales
                    MsjExcepcionNormativa = new NG_ContratoArrto().ObteneExcepcionNormatividadPreviaContrato(
                                                1, AreaOcupadaM2,
                                                RentaMensualUnitaria, FolioEmisionOpinion,
                                                SuperficieDictaminada_Justipreciacion, MontoDictaminado_Justipreciacion,
                                                NumeroDictamenExcepcionFolioSMOI);


                }
                catch (Exception ex)
                {
                    Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    this.LabelInfo.Text = "<div class='alert alert-danger'> <span>¡Error de registro!</span> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                    MostrarMensajeJavaScript(Msj);
                    this.LabelInfoEnviar.Focus();
                }

                //exponer mensaje al usuario
                if (MsjExcepcionNormativa != null && MsjExcepcionNormativa.Length > 0)
                {
                    if (this.lblEsContinuacion.Value == "1" && MsjExcepcionNormativa.Contains("abla SMOI"))
                    {
                        //Manejo de esta funcionalidad
                    }
                    else
                    {
                        this.LabelInfoEnviar.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> "; //
                        this.LabelInfoEnviar.Text += "<b>Validación de la normatividad en la información ingresada </b></br>";
                        this.LabelInfoEnviar.Text += "<b>¡Precaución! </b>";

                        //RCA 16/04/2018
                        //Mensaje para cuando no se inserte el folio de emision en continuaciones.
                        if (this.lblEsContinuacion.Value == "1")
                        {
                            if (string.IsNullOrEmpty(this.TextBoxFolioOpinion.Text))
                            {
                                this.LabelInfoEnviar.Text += "</br>No se proporcionó el folio de emisión. <br/>" + "</div>";
                            }

                        }
                        else
                        {
                            this.LabelInfoEnviar.Text += MsjExcepcionNormativa + "</div>";
                        }


                        this.LabelInfo.Text = this.LabelInfoEnviar.Text;
                        this.LabelInfoEnviar.Focus();
                    }
                }
                else
                {
                    this.LabelInfoEnviar.Text = "<div class='alert alert-success'>";
                    this.LabelInfoEnviar.Text += "<b>¡Felicidades! </b>";
                    this.LabelInfoEnviar.Text += "La información capturada cumple con la normatividad";

                    this.LabelInfo.Text = this.LabelInfoEnviar.Text;
                    this.LabelInfoEnviar.Focus();
                }
            }
        }

        //limpieza de controles si se quita el foco, y no hay secuencial de justipreciacion
        protected void TextBoxSecuencialJust_TextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxSecuencialJust.Text.Trim().Length == 0)
            {
                //limpieaza de ctrls
                this.TextBoxPropietarioInmueble.Text = String.Empty;
                this.TextBoxFuncionarioResp.Text = String.Empty;
                this.TextBoxFechaDictamenJust.Text = String.Empty;
                this.TextBoxSupDictaminadaJust.Text = String.Empty;
                this.TextBoxMontoDictaminadoJust.Text = String.Empty;
                this.TextBoxGenericoJust.Text = String.Empty;
                this.TextBoxUnidadMedidaSup.Text = string.Empty;
                this.TextBoxEstatusAttJust.Text = string.Empty;
                this.LabelInfo.Text = string.Empty;
                this.LabelInfoSecuencialJust.Text = string.Empty;
                this.TextBoxInstitucionJustipreciacion.Text = string.Empty;
                this.pnlInstitucion.Visible = false;
            }
            else
            {
                this.ObtenerJustipreciacion();
            }
        }

        //limpieza de controles si se quita el foco, y no hay folio de opinion
        protected void TextBoxFolioOpinion_TextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxFolioOpinion.Text.Trim().Length == 0)
            {
                this.LabelTotalSMOI.Text = String.Empty;
                this.LabelInfoFolioOpinion.Text = "<div class='alert alert-warning'>";
                this.LabelInfoFolioOpinion.Text += "<b>¡Aviso! </b>";
                this.LabelInfoFolioOpinion.Text += "De acuerdo a la normatividad en el registro de un contrato de arrendamiento debes proporcionar el folio de emisión de opinión o registra uno si no cuenta con él";
                this.LabelInfo.Text = this.LabelInfoFolioOpinion.Text;
                this.LabelInfoFolioOpinion.Focus();
            }
            else
            {
                this.RecuperaFolioOpinion(this.lblIdInstitucion.Value, this.TextBoxFolioOpinion.Text);
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
            this.btnLanzaDomicilios.Disabled = CheckBoxGenerarRIUF.Checked;
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
                    this.LabelInfoInmuebleArrendamiento.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                }
            }
        }

        protected void DropDownListTipoContratacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DefineTipoAmbiente();
        }

        private void DefineTipoAmbiente()
        {
            this.TextBoxSecuencialJust.Text = "";
            // MZT inclusión de la regla Excepción 178 (MRMSG) 
            //RCA 08/01/2018 se agrego una nueva excepcion para que no pida la justipreciacion            

            if (this.DropDownListTipoContratacion.SelectedValue != "6" &&
                this.DropDownListTipoContratacion.SelectedValue != "4" &&
                this.DropDownListTipoContratacion.SelectedValue != "9")
            {
                this.TextBoxSecuencialJust.Enabled = true;
                this.rfvTextBoxSecuencialJust.Enabled = true;
                this.ButtonObtenerJustipreciacion.Enabled = true;
            }
            else
            {
                this.TextBoxSecuencialJust.Enabled = false;
                this.rfvTextBoxSecuencialJust.Enabled = false;
                this.ButtonObtenerJustipreciacion.Enabled = false;
            }

            if (DropDownListTipoContratacion.SelectedValue == "11")
            {
                //this.TextBoxSecuencialJust.Text = "";
                this.TextBoxSecuencialJust.Enabled = false;
                this.ButtonObtenerJustipreciacion.Enabled = false;
                rfvTextBoxSecuencialJust.Enabled = false;
            }

            if (this.DropDownListTipoContratacion.SelectedValue == "15")
            {
                this.TextBoxSecuencialJust.Enabled = false;
                this.rfvTextBoxSecuencialJust.Enabled = false; //Se deshabilita la validación del control TextBoxSecuencialJust 17/12/2019
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

    }
}