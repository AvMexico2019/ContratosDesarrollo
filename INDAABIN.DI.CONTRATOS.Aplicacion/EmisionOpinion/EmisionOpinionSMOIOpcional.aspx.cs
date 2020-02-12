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
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;//interconexion con el BUS
using System.Web.UI.HtmlControls;


namespace INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion
{
    public partial class EmisionOpinionSMOIOpcional : System.Web.UI.Page
    {
        //Version Anterior

        private List<ConceptoOpinion> ListCptosOpinion; //lista contenedora de los conceptos de opinion que se recuperan de la BD
        private List<ValorRespuestaConcepto> ListValorRespuestaConcepto; //lista para guardar las respuestas del usuario a cada cpto.
        private static string strTemaOpinion, strTipoArrendamiento, strtFolioEmision;
        string Msj;

        //  Este WebForm recibe un QueryString para cargarse:
        //  Response.Redirect("~/EmisionOpinion/EmisionOpinion.aspx?TemaOpinion=1");
        protected void Page_Load(object sender, EventArgs e)
        {
            Msj = String.Empty;
            //El postback esta comentado deliveradamente porque la tabla dinamica no puede ligar el evento
            //si no es en la carga de la pagina
            //if (!IsPostBack)
            //{

            if (Session["Contexto"] == null)
                Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));

            String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);
            //determinar el tipo de usuario autenticado
            if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                this.ButtonEnviarOpinion.Visible = false; //no puede registrar Solicitudes

            this.LimpiarSessionesGeneradas();

            if (this.IdentificarTemaEmisionOpInionVsQuerySting())
            {
                //cargar la tabla de cptos de emision
                this.ObtenerCptosEmisionOpinion();
                Msj = "Proporciona los valores para cada concepto y al final de la captura da clic en Enviar para que el sistema registre la información y te proporcione un acuse con un número de Folio de Emisión de Opinión de Arrendamiento";
                this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario    
            }
            else
            {
                //el valor del query string no es valido, devolver a la pagina origen de seleccion de tipo de emision
                //Response.Redirect("ControladorEmisionOpinion.aspx");
                Response.Redirect("~/InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx");
            }
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

        private Boolean IdentificarTemaEmisionOpInionVsQuerySting()
        {
            Boolean Ok = false;
            //recepcion del queryString en otro pagina, con el nombre de la variable
            string sName = Request.QueryString["TemaOpinion"];
            if (sName != null)
            {
                switch (sName)
                {
                    case "1":
                        this.LabelIdContrato.Text = "0";
                        this.LabelIdInmuebleArrendamiento.Text = Request.QueryString["IdInmueble"].ToString();
                        strTemaOpinion = "Opinión Nuevo Arrendamiento";
                        strTipoArrendamiento = "Nuevo";
                        //etiquetas de titulos
                        this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA CONTRATACIÓN DE UN NUEVO ARRENDAMIENTO DE Inmueble";
                        Ok = true;
                        break;
                    case "2":
                        // Se recuperan valores del registro seleccionado en la consulta de BusqMvtosContratosInmuebles
                        this.LabelIdContrato.Text = "0";
                        //this.LabelIdContrato.Text = Request.QueryString["IdContrato"].ToString();
                        this.LabelIdInmuebleArrendamiento.Text = Request.QueryString["IdInmueble"].ToString();

                        strTemaOpinion = "Opinión Sustitución Arrendamiento";
                        strTipoArrendamiento = "Sustitución";
                        //etiquetas de titulos

                        this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA SUSTITUCIÓN DE UN ARRENDAMIENTO DE INMUEBLE PARA EL CONTRATO: [" + this.LabelIdContrato.Text + "].";

                        ////validar si se pide sustitucion basada en contrato local o de historico
                        //if (Session["FolioContrato"] != null)
                        //    this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA SUSTITUCIÓN DE UN ARRENDAMIENTO DE INMUEBLE PARA EL CONTRATO: [" + Session["FolioContrato"].ToString() + "].";
                        //else
                        //{
                        //    if (Session["NumContratoHist"] != null)
                        //        this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA SUSTITUCIÓN DE UN ARRENDAMIENTO DE INMUEBLE PARA EL CONTRATO: [" + Session["NumContratoHist"].ToString() + "].";
                        //}

                        Ok = true;
                        break;
                    case "3":
                        // Se recuperan valores del registro seleccionado en la consulta de BusqMvtosContratosInmuebles
                        this.LabelIdContrato.Text = Request.QueryString["IdContrato"].ToString();
                        this.LabelIdInmuebleArrendamiento.Text = Request.QueryString["IdInmueble"].ToString();

                        strTemaOpinion = "Opinión Continuación Arrendamiento";
                        strTipoArrendamiento = "Continuación";
                        //etiquetas de titulos
                        //validar si se pide continuacion basada en contrato local o de historico

                        this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA CONTINUACIÓN DE UN ARRENDAMIENTO DE INMUEBLE PARA EL CONTRATO: [" + this.LabelIdContrato.Text + "].";

                        //if (Session["FolioContrato"] != null)
                        //    this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA CONTINUACIÓN DE UN ARRENDAMIENTO DE INMUEBLE PARA EL CONTRATO: [" + Session["FolioContrato"].ToString() + "].";
                        //else
                        //{
                        //    if (Session["NumContratoHist"] != null)
                        //        this.LabelInfoEncabezadoPanelPrincipal.Text = "EMISIÓN DE OPINIÓN PARA LA CONTINUACIÓN DE UN ARRENDAMIENTO DE INMUEBLE PARA EL CONTRATO: [" + Session["NumContratoHist"].ToString() + "].";
                        //}
                        Ok = true;
                        break;
                }
            }
            return Ok;
        }


        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }


        //creacion de las tablas que conforman, el cuestionario de emisión de opinión, solicitado.
        private Boolean ObtenerCptosEmisionOpinion()
        {
            Boolean Ok = false;
            try
            {
                //obtener el Id del tema, para : Opinión Nuevo Arrendamiento y despues con el Id Obtener los Conceptos Valor-Resp
                byte IdTema = new NG_Catalogos().ObtenerIdTemaXDesc(strTemaOpinion);
                if (IdTema > 0)
                {
                    //institucion del usuario autenticado
                    int InstitucionUserAutenticado = ((SSO)Session["Contexto"]).IdInstitucion.Value;
                    switch (strTemaOpinion)
                    {
                        case "Opinión Nuevo Arrendamiento":
                            if (Session["ListCptosOpinionNuevo"] == null)
                            {
                                //poblar de la BD
                                ListCptosOpinion = new NGConceptoRespValor().ObtenerCptosEmisionOpinion(IdTema, InstitucionUserAutenticado);
                                Session["ListCptosOpinionNuevo"] = ListCptosOpinion; //salvar para no recargar en futuros postback
                            }

                            else //poner en el objeto, la session de cptos ya cargada
                                ListCptosOpinion = (List<ConceptoOpinion>)Session["ListCptosOpinionNuevo"];
                            break;

                        case "Opinión Sustitución Arrendamiento":
                            if (Session["ListCptosOpinionSustitucion"] == null)
                            {
                                //poblar de la BD
                                ListCptosOpinion = new NGConceptoRespValor().ObtenerCptosEmisionOpinion(IdTema, InstitucionUserAutenticado);
                                Session["ListCptosOpinionSustitucion"] = ListCptosOpinion; //salvar para no recargar en futuros postback
                            }

                            else //poner en el objeto, la session de cptos ya cargada
                                ListCptosOpinion = (List<ConceptoOpinion>)Session["ListCptosOpinionSustitucion"];
                            break;

                        case "Opinión Continuación Arrendamiento":
                            if (Session["ListCptosOpinionContinuacion"] == null)
                            {
                                //poblar de la BD
                                ListCptosOpinion = new NGConceptoRespValor().ObtenerCptosEmisionOpinion(IdTema, InstitucionUserAutenticado);
                                Session["ListCptosOpinionContinuacion"] = ListCptosOpinion; //salvar para no recargar en futuros postback
                            }
                            else //poner en el objeto, la session de cptos ya cargada
                                ListCptosOpinion = (List<ConceptoOpinion>)Session["ListCptosOpinionContinuacion"];
                            break;
                    }//switch

                    //si existen CptosValorRespuesta, entonces exponerlos en la vista
                    if (ListCptosOpinion.Count > 0)
                    {

                        if (this.CrearTablaCptosEmisionOpinion())
                        {
                            this.CrearTablaPiePagina();
                            this.TableEmisionOpinion.Visible = true;
                            //this.ButtonEnviarOpinion.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de conceptos de emisión de opinión. Contacta al área de sistemas.";
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
                Ok = false;
            }
            return Ok;
        }

        //se itera por cada objeto de la lista, y para objeto se crea un renglon de la tabla, donde sus propiedades se mapean a valores de celda
        private Boolean CrearTablaCptosEmisionOpinion()
        {
            Boolean Ok = false;
            TableRow Renglon;
            TableCell Columna;

            try
            {
                //crear objeto, si es el 1er postback y aun no se crea la estrucutura, para el momento en que se recolectara la respuesta de cada cpto.
                if (Session["ListValorRespuestaConcepto"] == null)//para no guardar n veces por cada postback
                    //recuperar conceptos de BD
                    ListValorRespuestaConcepto = new List<ValorRespuestaConcepto>();


                //para cada renglon de la lista agregarlo a la tabla 
                //para cada objeto en la lista, recorrer y accesar a sus propiedades
                foreach (ConceptoOpinion objCptoOpinion in ListCptosOpinion)
                {
                    //agregar renglon a la tabla 
                    Renglon = new TableRow();

                    //Id de Control en la Tabla
                    Renglon.ID = objCptoOpinion.NumOrdenVisual; //asiagnar ID a la columna para posteriormente recuperar en la obtencion de las respuestas
                    this.TableEmisionOpinion.Rows.Add(Renglon);


                    //************************* Columna de # de Orden *****************************
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;

                    if (objCptoOpinion.IdRespuesta != 8)//8 es un concpeto que no recibe respuesta
                    {
                        //Columna.Text = objCptoOpinion.NumOrden.ToString(); 
                        Columna.Text = objCptoOpinion.NumOrdenVisual;

                        //Guardar el espacio para  la  respuesta si es el 1er postaback, y no existe
                        if (Session["ListValorRespuestaConcepto"] == null)
                            //poblar objeto en donde se pondra la respuesta para guardar en la BD
                            ListValorRespuestaConcepto.Add(new ValorRespuestaConcepto { IdConceptoRespValor = objCptoOpinion.IdConceptoRespValor, NumOrden = objCptoOpinion.NumOrden, NumOrdenVisual = objCptoOpinion.NumOrdenVisual });
                    }


                    else //es encabezado,  8=N/A es Tema
                    {
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //************************* Columna de Desc Cpto ***********************************/
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.Text = objCptoOpinion.DescripcionConcepto; //recuperacion de datos del contrato
                    if (objCptoOpinion.IdRespuesta == 8) //formatear la celda
                    {
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //****************columna RESPUESTA******************************************/
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.HorizontalAlign = HorizontalAlign.Center;

                    //si el cpto tiene valor de opcion de respuesta, agregar la columna al renglon
                    if (objCptoOpinion.ValorMinimo != null)
                    {

                        //valor
                        DropDownList DropDownListRespuesta = new DropDownList();
                        DropDownListRespuesta.Items.Add(new ListItem("--", "-99"));
                        DropDownListRespuesta.Items.Add(new ListItem(objCptoOpinion.DescValorMinimo, objCptoOpinion.ValorMinimo.ToString()));
                        DropDownListRespuesta.Items.Add(new ListItem(objCptoOpinion.DescValorMaximo, objCptoOpinion.ValorMaximo.ToString()));

                        //Id Ctrl
                        DropDownListRespuesta.ID = Renglon.ID + "DropDownListRespuesta";


                        //cptos condicioandos
                        //RCA 16/01/2018: modificacion para la pregunta 11
                        if (objCptoOpinion.IdConcepto == 38 || objCptoOpinion.IdConcepto == 43 || objCptoOpinion.IdConcepto == 80)//aplica a Cpto de Opinion de Continuacion
                        {
                            DropDownListRespuesta.AutoPostBack = true; //activar para que se active el evento de validar dependencia
                            DropDownListRespuesta.EnableViewState = true;
                            DropDownListRespuesta.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                            DropDownListRespuesta.SelectedIndexChanged += new EventHandler(ComprobarDependenciaAplicacionSigConcepto);
                        }

                        DropDownListRespuesta.CssClass = "form-control";
                        DropDownListRespuesta.Attributes.Add("onchange", "backFromErrorClass(this);");

                        HtmlGenericControl myDiv = new HtmlGenericControl("div");
                        myDiv.Attributes.Add("class", "form-group");
                        myDiv.Controls.Add(DropDownListRespuesta);

                        Columna.Controls.Add(myDiv);


                    }
                    else
                    {

                        if (objCptoOpinion.IdRespuesta != 8) // N/A es Tema)
                        {

                            if (objCptoOpinion.ValorMinimo != null)
                            {
                                TextBox TextBoxCtrl = new TextBox();
                                TextBoxCtrl.Width = 100;
                                TextBoxCtrl.MaxLength = 9;

                                Columna.Controls.Add(TextBoxCtrl);
                            }
                            else //IdRespuesta no es tema
                            {
                                if (objCptoOpinion.IdConcepto == 35) //Cpto: Folio de Opinion
                                {
                                    //agregar caja de texto
                                    
                                    TextBox TextBoxCtrl = new TextBox();
                                    TextBoxCtrl.ID = "TextBoxFolioSMOI"; //identificador de control
                                    TextBoxCtrl.ToolTip = "Propocione el número de folio de SMOI ó deje vacio si cuentas con el número de dictamen de excepción";
                                    TextBoxCtrl.Attributes.Add("PlaceHolder", "#####");
                                    TextBoxCtrl.Width = 100;
                                    TextBoxCtrl.MaxLength = 6;

                                    TextBoxCtrl.AutoPostBack = true;
                                    TextBoxCtrl.TextChanged += new EventHandler(this.ComprobarFolioSMOI);
                                    TextBoxCtrl.CssClass = "form-control";
                                    TextBoxCtrl.Attributes.Add("onblur", "backFromErrorClass(this);");

                                    HtmlGenericControl myDiv = new HtmlGenericControl("div");
                                    myDiv.Attributes.Add("class", "form-group");
                                    myDiv.Controls.Add(TextBoxCtrl);

                                    Columna.Controls.Add(myDiv);

                                    //control de ajax que solo permite numeros
                                    AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                    FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                    FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                                    Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);


                                    //creacion de un boton
                                    Button ButtonCtrl = new Button();
                                    ButtonCtrl.Text = "Comprobar";
                                    ButtonCtrl.Click += new EventHandler(this.ComprobarFolioSMOI);
                                    ButtonCtrl.ControlStyle.CssClass = "";
                                    ButtonCtrl.CssClass = "btn btn-primary btn-sm";


                                    //this.CrearCtrlConEventos(Columna);
                                    Columna.Controls.Add(ButtonCtrl);//se instancio en el load de la pagina, aqui solo se asocia y muestra en la tabla

                                }

                                //RCA 16/01/2018
                                //condicion para crear el campo de texto para que puedan poner la justificacion del certificado de seguridad 
                                else if (objCptoOpinion.IdConcepto == 81) //cpto. justitifacion certificado de seguridad.
                                {
                                    //agregar area de texto    
                                    HtmlTextArea TextAreaCrtl = new HtmlTextArea();

                                    TextAreaCrtl.ID = "TextAreaJustificacionCertificadoSeguridad";//identificador de control
                                    TextAreaCrtl.Rows = 1;
                                    TextAreaCrtl.Cols = 8;
                                    TextAreaCrtl.Disabled = true;//deshabilitar el area de texto
                                    TextAreaCrtl.Attributes.Add("onblur", "backFromErrorClass(this);");

                                    HtmlGenericControl myDiv = new HtmlGenericControl("div");
                                    myDiv.Attributes.Add("class", "form-group");
                                    myDiv.Controls.Add(TextAreaCrtl);

                                    Columna.Controls.Add(myDiv);

                                }

                                else
                                {
                                    //poner etiquetas para cptos de smoi, que se recuperaran cuando se da clic en el boton de obtener el Folio de SMOI
                                    if (objCptoOpinion.NumOrden == (decimal)3.02) //SupM2 resultado tabla SMOI
                                    {
                                        Label LabelValor = new Label();
                                        LabelValor.ID = "LabelSupM2TablaSMOI"; //darle ID, para localizar el control al proporcionar el usr el Folio de Tabla SMOI
                                        LabelValor.Text = "--";
                                        LabelValor.Font.Bold = true;
                                        Columna.Controls.Add(LabelValor);
                                    }
                                    else
                                    {
                                        if (objCptoOpinion.NumOrden == (decimal)3.0)
                                        {
                                            //agregar caja de texto
                                            TextBox TextBoxCtrl = new TextBox();
                                            TextBoxCtrl.ID = "TextBoxSupM2xArrendar"; //identificador de control
                                            TextBoxCtrl.Width = 100;
                                            TextBoxCtrl.MaxLength = 8;
                                            TextBoxCtrl.Attributes.Add("PlaceHolder", "m2");
                                            TextBoxCtrl.AutoPostBack = true;
                                            TextBoxCtrl.TextChanged += new EventHandler(this.LimpiarM2porArrendarVsFolioSMOI);
                                            TextBoxCtrl.CssClass = "form-control";
                                            TextBoxCtrl.Attributes.Add("onblur", "backFromErrorClass(this);");

                                            HtmlGenericControl myDiv = new HtmlGenericControl("div");
                                            myDiv.Attributes.Add("class", "form-group");
                                            myDiv.Controls.Add(TextBoxCtrl);

                                            Columna.Controls.Add(myDiv);

                                            //control ajax que solo permite numeros y el punto
                                            AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                            FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                            FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                            FilteredTextBoxExtenderSoloNums.ValidChars = ".0123456789";
                                            Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                                        }
                                    }

                                }

                            }

                        }
                        else //es Tema
                        {

                            Columna.Font.Bold = true;
                            Columna.BackColor = System.Drawing.Color.LightGray;
                            Columna.BorderColor = System.Drawing.Color.Gray;
                            Columna.Text = "Captura";

                        }



                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //**** columna de Link de Funadamento Lega ***/
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    if (objCptoOpinion.IdRespuesta != 8) // 8=N/A es Tema, no poner el orden si es un concepto de Tema
                    {
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BorderWidth = 1;
                        Columna.HorizontalAlign = HorizontalAlign.Center;
                        LinkButton LinkButtonLegal = new LinkButton();
                        LinkButtonLegal.Text = "Consultar";
                        Columna.Controls.Add(LinkButtonLegal);
                        LinkButtonLegal.ID = objCptoOpinion.NumOrdenVisual + "-LinkButtom"; //darle un identificador

                        //LinkButtonLegal.Click += new EventHandler(this.VerFundamentoLegalCpto);
                        //this.upCaptura.a

                        if (objCptoOpinion.NumOrdenVisual.Contains("."))
                            LinkButtonLegal.Visible = false;

                        //string sIdTema = Request.QueryString["TemaOpinion"];

                        string sIdTema = "1";

                        if (Request.QueryString["TemaOpinion"] == "1")
                            sIdTema = "2";
                        if (Request.QueryString["TemaOpinion"] == "2")
                            sIdTema = "4";
                        if (Request.QueryString["TemaOpinion"] == "3")
                            sIdTema = "3";

                        string strScript = "window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=" + objCptoOpinion.NumOrdenVisual + "&TemaOpinion=" + sIdTema + "', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');";
                        //string strScript = "window.open('FundamentoLegalCpto.aspx?NumOrdenCpto=" + objCptoOpinion.NumOrdenVisual + "', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');";
                        LinkButtonLegal.Attributes.Add("onclick", strScript);

                        //PostBackTrigger syncPostBackTrigger = new PostBackTrigger();
                        //syncPostBackTrigger.ControlID = LinkButtonLegal.UniqueID;
                        //this.upCaptura.Triggers.Add(syncPostBackTrigger);


                        //PageScriptManager.RegisterPostBackControl(LinkButtonLegal);


                    }
                    else //es encabezado
                    {
                        Columna.HorizontalAlign = HorizontalAlign.Center;
                        Columna.Text = "Fundamento Legal";
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);




                }//foreach

                Ok = true;

                //solo si no se creado la session
                if (Session["ListValorRespuestaConcepto"] == null)
                    //guardar en session para cuando el usuario proporciona las respuestas
                    Session["ListValorRespuestaConcepto"] = ListValorRespuestaConcepto;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de conceptos de emisión de opinión. Contacta al área de sistemas.";
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
                Ok = false;
            }
            return Ok;
        }

        //validacion aplicable a la respuesta al concepto 3.3, que depende de SupM2Arrendar vs SupTotalSMOI
        private void ComprobarDependenciaFolioSMOIProporcionado(object sender, EventArgs e)
        {
            String sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI");
            if (Util.IsNumeric(sValorRespuesta) == false)
            {
                AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", true);
                Msj = "Debes propocionar un folio de Tabla SMOI y da clic en Comprobar, para poder responder este concepto";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                MostrarMensajeJavaScript(Msj);
                this.SetFocoIdCtrlRowCellCtrl("TextBoxFolioSMOI");//poner foco en el ctrl
            }
            else
            {
                //recuperar el folio smoi para validar que los valores SupM2Arrendar es mayor o menor y si debe estar activa o no la pregunata 3.3
                sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI");
                if (Util.IsNumeric(sValorRespuesta) == false)
                {
                    Msj = "Debes propocionar un folio de Tabla SMOI y dar clic en Comprobar, para poder responder este concepto";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                    MostrarMensajeJavaScript(Msj);
                    this.SetFocoIdCtrlRowCellCtrl("TextBoxFolioSMOI");//poner foco en el ctrl
                }
                else
                    RecuperaValoresFolioSMOI(Convert.ToInt32(sValorRespuesta), false);
            }
        }


        //evento asociado dinamicamente al Button de la tabla
        //nota: se define en la creacion de la tabla, al load de la pagina
        private void ComprobarDependenciaAplicacionSigConcepto(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ValidarDependenciaAplicacionSigConcepto_OpinionContinuacion(ddl.ID);
        }

        //procedimiento que aplica para validarCptos de Emsion de Opinion de tipo Continuacion.
        private void ValidarDependenciaAplicacionSigConcepto_OpinionContinuacion(string IdControl)
        {
            //si se ha seleccionado el valor del item "No", entonces autoseleccionar y deshabilitar el sigueinte concepto: #5        
            String sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl(IdControl);

            if (sValorRespuesta == "No")
            {
                if (IdControl == "4DropDownListRespuesta")
                {
                    //buscar el siguiente concepto, autoseleccionar el valor e inhabilitar
                    AsignarValorIdCtrlRowCellCtrl("5DropDownListRespuesta", "--", false);
                }

                //RCA 16/01/2018
                //ponemos la opcion opara que habilite el campo de justificacion cuando este sea NO
                else if (IdControl == "11DropDownListRespuesta")
                {
                    //buscar el siguiente concepto, autoseleccionar el valor e inhabilitar
                    AsignarValorIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad", "--", false);
                }

                else
                {
                    if (IdControl == "9DropDownListRespuesta") //es el caso contrario de: 4DropDownListRespuesta
                        //buscar el siguiente concepto, autoseleccionar el valor y habilitar
                        AsignarValorIdCtrlRowCellCtrl("10DropDownListRespuesta", "--", true);
                }
            }
            else
            {
                if (IdControl == "4DropDownListRespuesta")
                {
                    //buscar el siguiente concepto, autoseleccionar el valor y habilitar
                    AsignarValorIdCtrlRowCellCtrl("5DropDownListRespuesta", "--", true);
                }

                //RCA 16/01/2018
                //condicion para bloquear el textarea de justificacion si la resouesta es si
                else if (IdControl == "11DropDownListRespuesta")
                {
                    //buscar el siguiente concepto, autoseleccionar el valor y habilitar
                    AsignarValorIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad", "--", true);
                }

                else
                {
                    if (IdControl == "9DropDownListRespuesta")//es el caso contrario de: 4DropDownListRespuesta
                        //buscar el siguiente concepto, autoseleccionar el valor e inhabilitar
                        AsignarValorIdCtrlRowCellCtrl("10DropDownListRespuesta", "--", false);
                }
            }
        }

        private string ConvirteNumLogicoAFisico(Decimal NumOrden)
        {
            float numDecimal;

            //si su parte decimal es 0.0, exponer como entero
            numDecimal = float.Parse("0," + NumOrden.ToString().Split('.')[1]);

            if (numDecimal == 0.0)//no tiene parte decimal, entonces exponer commo un entero
                return int.Parse(NumOrden.ToString().Split('.')[0]).ToString();//obtener parte entera
            else //si tiene parte decimal >0, exponerla
            {
                //para casos de 3.01 exponer como 3.1, se hace en la implementacion, no en la BD, porque si no lo expone como 3.10, pareciendo que hay 10 items
                if (numDecimal > 1 && numDecimal < 10)
                {
                    //si de la 1er parte es cero, mostrar el siguiente
                    byte Num1Decimal = byte.Parse(numDecimal.ToString().Split()[0]);
                    return int.Parse(NumOrden.ToString().Split('.')[0]).ToString() + ".0" + Num1Decimal.ToString();
                }
                else
                    return NumOrden.ToString(); //expoener tal como esta en la BD
            }
        }

        //evento asociado dinamicamente al link de fundamento legal en la tabla
        //nota: se define en la creacion de la tabla, al load de la pagina
        //private void VerFundamentoLegalCpto(object sender, EventArgs e)
        //{

        //    LinkButton LinkButtonFundamento = (LinkButton)sender;
        //    if (LinkButtonFundamento != null)
        //    {
        //        ConceptoRespValor objConceptoRespValor = null;


        //        string NumOrdenCpto = LinkButtonFundamento.ID;
        //        string[] words = NumOrdenCpto.Split('-');

        //        NumOrdenCpto.IndexOf("-");
        //        NumOrdenCpto = words[0];

        //        string[] nums = NumOrdenCpto.Split('.');

        //        if (nums.Count() > 1)
        //            //convertir a numero decimal correcto: 3.2 ---> 3.02
        //            NumOrdenCpto = ConvirteNumLogicoAFisico(Convert.ToDecimal(NumOrdenCpto));

        //        try
        //        {

        //            objConceptoRespValor = new NGConceptoRespValor().ObtenerFundamentoLegalCpto(Convert.ToByte(2), Convert.ToDecimal(NumOrdenCpto));
        //        }
        //        catch (SqlException ex)
        //        {

        //            Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        //            this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";               

        //        }
        //        catch (Exception ex)
        //        {
        //            Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
        //            this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>"; 
        //        }

        //        if (objConceptoRespValor != null)
        //        {
        //            Session["objConceptoRespValor"] = objConceptoRespValor;
        //            //string queryString = "FundamentoLegalCpto.aspx?NumPregunta=" + LinkButtonFundamento.ID;
        //            string queryString = "FundamentoLegalCpto.aspx?NumOrdenCpto=" + NumOrdenCpto;
        //            string newWin = "window.open('" + queryString + "');";
        //            ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        //        }
        //        else
        //        {
        //            Msj = "No existe fundamento Legal, para este Concepto";
        //            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
        //            MostrarMensajeJavaScript(Msj);

        //        }


        //    }//LinkButtonFundamento

        //}



        //limpia los controles dependientes, cuando se  limpia el contenido de la caja de texto
        private void LimpiarM2porArrendarVsFolioSMOI(object sender, EventArgs e)
        {
            ////Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacio

            //this.LabelInfo.Text = string.Empty;
            //this.LabelInfoEnviar.Text = string.Empty;

            string strSupM2xArrendar = this.EncontrarValorIdCtrlRowCellCtrl("TextBoxSupM2xArrendar");

            //si se limpia el contenido del control, limpiar las otros valores dependientes
            if (strSupM2xArrendar.Length == 0)
            {
                //limpiar valores
                this.AsignarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI", "");
                this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", "--");
                Msj = "Proporciona los valores para cada concepto y al final de la captura da clic en Enviar para que el sistema registre la información y te proporcione un acuse con un número de Folio de Emisión de Opinión de Arrendamiento";
                this.LabelInfo.Text = "<div class='alert alert-info'> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario   

            }
            else //si hay informacion, exponer warning si al modificar los m2 arrendados, se sobrepasan contra los m2 de SMOI
            {
                string strSupM2SMOI = this.EncontrarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI");
                if (Util.IsNumeric(strSupM2SMOI))
                {
                    if (Convert.ToDecimal(strSupM2xArrendar) > Convert.ToDecimal(strSupM2SMOI))
                    {
                        //habiliar el concpto 3.3  de si tiene dictamen de excepcion poer exceder los m2 vs SMOI
                        this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", true); //habilitarlo y resetear seleccion de valor
                        //informar al usuario de excepcion
                        Msj = "La cantidad de metros cuadrados por arrendar es mayor a los especificados en la tabla SMOI, verifica si es correcto el folio proporcionado, porque estaría incumpliendo la normatividad";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                        this.LabelInfo.Focus();
                        MostrarMensajeJavaScript(Msj);

                    }
                    else //si es menor la SupM2 proporcionados contra SMOI, entonces bloquear concepto: 3.3
                    {

                        this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "N/A", false); //habilitarlo y resetear seleccion de valor


                        Msj = "Los metros cuadrados por arrendar son menor o igual a los especificados en la tabla SMOI ! se cumple con la normativa!";
                        this.LabelInfo.Text = "<div class='alert alert-info'>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario

                    }

                }
            }


            // Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
        }

        //evento asociado dinamicamente al Button de la tabla
        //nota: se define en la creacion de la tabla, al load de la pagina
        private void ComprobarFolioSMOI(object sender, EventArgs e)
        {
            ////Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacio

            string strFolioSMOI = this.EncontrarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI");

            if (strFolioSMOI.Length == 0)
            {
                //limpiar valores
                this.AsignarValorIdCtrlRowCellCtrl("TextBoxSupM2xArrendar", "");
                this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", "--");
                this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", true); //habilitarlo y resetear seleccion de valor
            }
            else
                this.ValidarFolioSMOI();

            //Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
        }

        /// <summary>
        /// Valida que la exista la entrada de un FolioSMOI vs la BD, a traves del llamado al metodo: RecuperaValoresFolioSMOI()
        /// </summary>
        /// <returns></returns>
        private Boolean ValidarFolioSMOI()
        {
            //recorrer los renglones-celdas y controles en celda hasta encontrar en ctrl por determinado ID
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Control ctrlInner in cell.Controls)
                    {
                        //si el control que itera es 
                        Control ctrl;
                        if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                        {
                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                            myDiv.Attributes.Clear();
                            myDiv.Attributes.Add("class", "form-group");
                            ctrl = ctrlInner.Controls[0];
                        }
                        else
                            ctrl = ctrlInner;


                        if (ctrl.ID == "TextBoxFolioSMOI")
                        {
                            //reseteo de etiquetas resultado de identificacion de Folio SMOI
                            this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", "--"); //limpiar valores de etiquetas resultado de localizar el FolioSMOI

                            //validar que existan datos en el textobox de supM2 x arrendar
                            this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", true); //habilitarlo y resetear seleccion de valor

                            if (((TextBox)ctrl).Text.Length > 0)
                            {
                                if (Util.IsEnteroNatural(((TextBox)ctrl).Text))
                                {
                                    // if (objAplicacionConcepto != null)//sustituir con la llamada al metodo que devuelve 2 valores, quizas sera un objeto, que  se poblaran las 2 propiedades
                                    if (this.RecuperaValoresFolioSMOI(Convert.ToInt32(((TextBox)ctrl).Text), true))
                                    {
                                        //los mensajes se personalizan en funcion
                                        //Msj = "Se identificó el registro del Folio de Tabla SMOI proporcionado, es posible continuar la captura"; ;
                                        //this.LabelInfo.Text = "<div class='alert alert-info'>" + Msj + "</div>";
                                        //MostrarMensajeJavaScript(Msj);
                                        //this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                        return true; //romper todos los ciclos, se asignaron correctamente los valores buscasdos
                                    }
                                    else
                                    {
                                        //if (Msj.Length == 0)
                                        //    Msj = "No existe registro del folio de tabla SMOI proporcionado, consulta o registra uno en el menu de Folio SMOI";
                                        //this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                        //this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                        //MostrarMensajeJavaScript(Msj);
                                        return false; //romper todos los ciclos, se asignaron correctamente los valores buscasdos
                                    }
                                }
                                else
                                {
                                    Msj = "Debes proporcionar el Folio de tabla SMOI, en el concepto: [" + row.ID.ToString() + "] como un número entero";
                                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                    MostrarMensajeJavaScript(Msj);
                                    ((TextBox)ctrl).Focus();
                                    return false; //romper todos los ciclos
                                }
                            }
                            else
                            {
                                Msj = "Debes proporcionar el Folio de tabla SMOI, en el concepto:  [" + row.ID.ToString() + "].";
                                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario                              
                                MostrarMensajeJavaScript(Msj);
                                ((TextBox)ctrl).Focus();
                                return false; //romper todos los ciclos
                            }
                        }//if
                    }// foreach
                }// foreach
            }// foreach

            return false; //si llega aqui es que no se pudo reccorer la tabla y localizar valores, por ello se devuele falso
        }//funcion

        /// <summary>
        /// Busca un FolioSMOI en la BD y recupera 2 valores: (SupM2XArrendar y SupM2XSMOI) que coloca en un objeto de negocio: [AplicacionConcepto]
        /// </summary>
        /// <param name="FolioTablaSMOI"></param>
        /// <returns></returns>
        private Boolean RecuperaValoresFolioSMOI(int FolioTablaSMOI, Boolean verificaCuentaUsoFolioSMOI)
        {
            Boolean Ok = false;
            //AplicacionConcepto objAplicacionConcepto = null;
            try
            {
                if (FolioTablaSMOI > 0)
                {
                    //obtener los m2 de tabla SMOI del folio proporcionado
                    decimal SupTotalM2SMOI = this.ObtenerSupTotalM2SMOI(FolioTablaSMOI, verificaCuentaUsoFolioSMOI);

                    if (SupTotalM2SMOI > 0)
                    {
                        //obtener el valor del control textbox de los m2 por arrendar
                        string strValorM2Arrendar = this.EncontrarValorIdCtrlRowCellCtrl("TextBoxSupM2xArrendar");
                        if (strValorM2Arrendar.Length > 0 && Util.IsNumeric(strValorM2Arrendar))
                        {
                            decimal ValorM2Arrendar = Convert.ToDecimal(strValorM2Arrendar);

                            //poner formato al # de entrada por el usuario
                            this.AsignarValorIdCtrlRowCellCtrl("TextBoxSupM2xArrendar", String.Format("{0:N}", strValorM2Arrendar));

                            //crear objeto y vaciar los 2 valores a las propiedades del obj de negocio, para exponerlas al usuario
                            //objAplicacionConcepto = new AplicacionConcepto { SupM2XArrendar = Convert.ToDecimal(ValorM2Arrendar), SupM2XSMOI = SupTotalM2SMOI };


                            //colocar los valores recuperados del FolioSMOI en los controles de la vista
                            if (this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", String.Format("{0:N}", SupTotalM2SMOI)))
                            {

                                //autoseleccionar "--" de lista de Existe Dictamen cuando no se Excede Resultado de SMOI vs SupM2Arrendados
                                if (ValorM2Arrendar <= SupTotalM2SMOI) //no se excede la norma recomendada
                                {
                                    //buscar control en la tabla y asignar el valor especificado
                                    this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "N/A", false); //Des-habilitar, no aplica este concepto

                                    //los mensajes se personalizan en funcion
                                    if (Msj.Length > 0 && Msj.IndexOf("ocupado", 0) > -1)
                                        Msj += "-Se identificó el registro del Folio de Tabla SMOI proporcionado y en su comparación a los metros cuadrados cumple con la normatividad";
                                    else
                                        Msj = "Se identificó el registro del Folio de Tabla SMOI proporcionado y en su comparación a los metros cuadrados cumple con la normatividad";

                                    this.LabelInfo.Text = "<div class='alert alert-info'>" + Msj + "</div>";
                                    MostrarMensajeJavaScript(Msj);
                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                    Ok = true;
                                }
                                else //el ValorM2Arrendar es mayor al del SMOI, validar que exista respuesta al cpto 3.3
                                {
                                    //validar si el valor de "3.3DropDownListRespuesta" es "--" y si no exigir valor
                                    if (this.EncontrarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta") == "N/A")
                                    {
                                        Msj = "Debes seleccionar un valor de respuesta para el concepto 3.3";
                                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                        this.AsignarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta", "--", true); //habilitar, no aplica este concepto

                                        MostrarMensajeJavaScript(Msj);
                                        SetFocoIdCtrlRowCellCtrl("3.3DropDownListRespuesta");
                                    }
                                    else //si se supero la SupM2PorArrendar vs SMOI y el usuario ya seleccionó una respuesta
                                    {
                                        if (Msj.Length > 0 && Msj.IndexOf("ocupado", 0) > -1)
                                            Msj += "La cantidad de metros cuadrados por arrendar es mayor a los especificados en la tabla SMOI, verifica si es correcto el folio proporcionado, porque estaría incumpliendo la normatividad";
                                        else
                                            Msj = "-La cantidad de metros cuadrados por arrendar es mayor a los especificados en la tabla SMOI, verifica si es correcto el folio proporcionado, porque estaría incumpliendo la normatividad";
                                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                        MostrarMensajeJavaScript(Msj);
                                        Ok = true;
                                    }
                                }

                            }
                            else
                            {
                                Msj = "Debes proporcionar un Folio SMOI válido para obtener los m2 resultado";
                                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                SetFocoIdCtrlRowCellCtrl("TextBoxFolioSMOI");
                                MostrarMensajeJavaScript(Msj);
                            }
                        }
                        else
                        {
                            Msj = "Debes proporcionar primero el valor de m2 por arrendar en el concepto #3";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                            this.AsignarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI", "", null); //limpiar el folio proporcionado
                            this.MostrarMensajeJavaScript(Msj);
                            SetFocoIdCtrlRowCellCtrl("TextBoxSupM2xArrendar");
                        }
                    }
                    else
                    {
                        if (SupTotalM2SMOI == 0)
                        {
                            Msj = "El folio SMOI proporcionado no existe para la institución a la que tu cuenta de acceso esta adscrita, por favor verifica.";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                            this.MostrarMensajeJavaScript(Msj);
                            SetFocoIdCtrlRowCellCtrl("TextBoxFolioSMOI");
                        }
                    }
                }
                else
                {
                    Msj = "Debes proporcionar un número de folio SMOI como un entero";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.MostrarMensajeJavaScript(Msj);
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar los valores de la tabla SMOI. Contacta al área de sistemas.";
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
                Ok = false;
            }
            return Ok;
            //return objAplicacionConcepto;
        }

        private decimal ObtenerSupTotalM2SMOI(int IdFolioSMOI, Boolean verificaCuentaUsoFolioSMOI)
        {
            decimal SupTotalM2SMOI = 0;
            int InstitucionUserAutenticado = ((SSO)Session["Contexto"]).IdInstitucion.Value;
            try
            {
                SupTotalM2SMOI = new NGConceptoRespValor().ObtenerSupTotalM2SMOIsinOcupar(IdFolioSMOI, InstitucionUserAutenticado);
                if (verificaCuentaUsoFolioSMOI == true && SupTotalM2SMOI > 0)
                {
                    //ver numero de veces que se ha relacioando el folio smoi con emisiones de opinion, si es mas de 1, exponer msj al usuario
                    if (this.ObtenerCountUsoFolioSMOIenOpionion(IdFolioSMOI) > 0)
                    {
                        SupTotalM2SMOI = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                SupTotalM2SMOI = -1;
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;

                if (ex.Message.IndexOf("asignado", 0) > -1)
                {
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.MostrarMensajeJavaScript(Msj);
                    SetFocoIdCtrlRowCellCtrl("TextBoxFolioSMOI");
                }
                else
                {
                    Msj = "Ha ocurrido un error al recuperar el subtotal SMOI. Contacta al área de sistemas.";
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
                this.LabelInfo.Focus();
            }
            return SupTotalM2SMOI;
        }

        //# de veces que se ha relacionado un SMOI con solicitudes de emisión de opinión
        private int ObtenerCountUsoFolioSMOIenOpionion(int IdFolioSMOI)
        {
            int CountUsoFolioSMOIenOpionion = 0;
            int InstitucionUserAutenticado = ((SSO)Session["Contexto"]).IdInstitucion.Value;
            try
            {
                CountUsoFolioSMOIenOpionion = new NGConceptoRespValor().ObtenerCountUsoFolioSMOIenOpionion(IdFolioSMOI, InstitucionUserAutenticado);
                //si ya se utilizo alguna vez exponere warning al usuario
                if (CountUsoFolioSMOIenOpionion > 0)
                {
                    this.AsignarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI", "");
                    this.AsignarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI", "--");
                    Msj = "Nota: El folio SMOI proporcionado (<strong>" + IdFolioSMOI + "</strong>) ya ha sido ocupado en " + CountUsoFolioSMOIenOpionion.ToString() + " solicitud(es) de emisión de opinión y no puede volver a utilizarse  <br/>";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + " </div>";
                    MostrarMensajeJavaScript("Nota: El folio SMOI proporcionado (" + IdFolioSMOI + ") ya ha sido ocupado en " + CountUsoFolioSMOIenOpionion.ToString() + " solicitud(es) de emisión de opinión y no puede volver a utilizarse");
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el conteo de SMOI en emisiones de opinión. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
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
            return CountUsoFolioSMOIenOpionion;
        }

        //Proposito: Identificar el control solicitado por su Id en un determinado renglon-celda-control y "Asignar" el valor especificado en el parametro de entrada
        //parametros de entrada: el IdControl para el que se desea asignar un valor
        //parametros de salida: un true/false si se logro encontrar el control y hacer la asignacion del valor
        private Boolean AsignarValorIdCtrlRowCellCtrl(String IdCtrl, String Valor, Boolean? pEnabled = null)
        {
            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                //iterar por cada columna/celda del renglon
                foreach (TableCell cell in row.Cells)
                {
                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                    foreach (Control ctrlInner in cell.Controls)
                    {
                        Control ctrl;
                        if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                        {
                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                            myDiv.Attributes.Clear();
                            myDiv.Attributes.Add("class", "form-group");
                            ctrl = ctrlInner.Controls[0];
                        }
                        else
                            ctrl = ctrlInner;

                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                        switch (ctrl.GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.Label":
                                if (((Label)ctrl).ID == IdCtrl)
                                {
                                    ((Label)ctrl).Text = Valor; //asignar valor al control solicitado
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                }

                                break;

                            case "System.Web.UI.WebControls.TextBox":

                                if (((TextBox)ctrl).ID == IdCtrl)
                                {
                                    if (pEnabled != null)
                                        ((TextBox)ctrl).Enabled = pEnabled.Value; //la opcion especificada                                   
                                    else //default
                                        ((TextBox)ctrl).Enabled = true; //habilitar 

                                    ((TextBox)ctrl).Text = Valor; //asignar valor al control solicitado
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.

                                }

                                break;

                            //RCA 16/01/2018
                            //case para entrar a habilitar o desahabilitar el textarea de la pregunta 11.01, dependiendo de la respuesta de la preguntaa 11
                            case "System.Web.UI.HtmlControls.HtmlTextArea":

                                if (((HtmlTextArea)ctrl).ID == IdCtrl)
                                {
                                    if (pEnabled != null)
                                    {
                                        ((HtmlTextArea)ctrl).Disabled = pEnabled.Value;//la opcion que especifica
                                    }
                                    else //default
                                    {
                                        ((HtmlTextArea)ctrl).Disabled = true;//la opcion que lo bloquea
                                    }
                                }
                                else
                                {

                                }

                                return true; //romper todos los ciclos, porque ya se encontro el ctrl

                                break;

                            case "System.Web.UI.WebControls.DropDownList":

                                if (((DropDownList)ctrl).ID == IdCtrl)
                                {
                                    if (pEnabled != null)
                                        ((DropDownList)ctrl).Enabled = pEnabled.Value; //la opcion especificada                                   
                                    else //default
                                        ((DropDownList)ctrl).Enabled = true; //habilitar 

                                    if (Valor == "No")
                                    {
                                        ((DropDownList)ctrl).SelectedIndex = 1; //si
                                        //((DropDownList)ctrl).Items.FindByText(Valor).Selected = true; //autoseleccionar  valor al control solicitado
                                        return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                    }
                                    else
                                    {
                                        if (Valor == "Si")
                                        {
                                            ((DropDownList)ctrl).SelectedIndex = 2; //no
                                            //((DropDownList)ctrl).Items.FindByText(Valor).Selected = true; //autoseleccionar  valor al control solicitado
                                            return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                        }
                                        else
                                        {
                                            if (Valor == "N/A") //esta caso para el cpto 3.3, donde no aplica seleccionar una respuesta
                                            {
                                                if (((DropDownList)ctrl).Items.Contains(((DropDownList)ctrl).Items.FindByValue("99")) == false)//si no esta el 99, agregarlo
                                                    ((DropDownList)ctrl).Items.Add(new ListItem("N/A", "99"));

                                                ((DropDownList)ctrl).SelectedIndex = 3; // N/A
                                                return true;
                                            }
                                            else
                                            {
                                                if (((DropDownList)ctrl).Items.Contains(((DropDownList)ctrl).Items.FindByValue("99")) == true)
                                                    ((DropDownList)ctrl).Items.Remove(new ListItem("N/A", "99"));

                                                ((DropDownList)ctrl).SelectedIndex = 0; //"--"
                                                return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    } //foreach
                }//foreach
            }//foreach

            return false; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }

        //Proposito: Identificar el control solicitado por su Id en un determinado renglon-celda-control y "Devolver" el valor especificado en el parametro de entrada
        //parametros de entrada: el IdControl para el que se desea asignar un valor
        //parametros de salida: un true/false si se logro encontrar el control y hacer la asignacion del valor
        private string EncontrarValorIdCtrlRowCellCtrl(String IdCtrl)
        {
            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                //iterar por cada columna/celda del renglon
                foreach (TableCell cell in row.Cells)
                {
                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                    foreach (Control ctrlInner in cell.Controls)
                    {
                        Control ctrl;
                        if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                        {
                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                            myDiv.Attributes.Clear();
                            myDiv.Attributes.Add("class", "form-group");
                            ctrl = ctrlInner.Controls[0];
                        }
                        else
                            ctrl = ctrlInner;

                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                        switch (ctrl.GetType().ToString())
                        {
                            case "System.Web.UI.WebControls.Label":
                                if (((Label)ctrl).ID == IdCtrl)
                                    return ((Label)ctrl).Text; //romper todos los ciclos, porque ya se encontro el ctrl. y se devuelve el valor

                                break;


                            case "System.Web.UI.WebControls.DropDownList":

                                if (((DropDownList)ctrl).ID == IdCtrl)
                                    return ((DropDownList)ctrl).SelectedItem.Text;  //romper todos los ciclos, porque ya se encontro el ctrl. y se devuelve el valor

                                break;

                            case "System.Web.UI.WebControls.TextBox":
                                if (((TextBox)ctrl).ID == IdCtrl)
                                    return ((TextBox)ctrl).Text; //romper todos los ciclos, porque ya se encontro el ctrl. y se devuelve el valor

                                break;

                            //RCA 17/01/2018
                            //caso para validar si tiene texto
                            case "System.Web.UI.HtmlControls.HtmlTextArea":
                                if (((HtmlTextArea)ctrl).ID == IdCtrl)
                                {
                                    return ((HtmlTextArea)ctrl).Value;//rompe los ciclos porque ya se encontro el ctrl y devuelve el valor.
                                }
                                break;

                        }
                    } //foreach
                }//foreach
            }//foreach

            return "Valor no Encontrado"; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }

        //Asigna el Foco a un control especificado, en la Tabla
        private Boolean SetFocoIdCtrlRowCellCtrl(String IdCtrl)
        {
            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                //iterar por cada columna/celda del renglon
                foreach (TableCell cell in row.Cells)
                {
                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                    foreach (Control ctrlInner in cell.Controls)
                    {
                        Control ctrl;
                        if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                        {
                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                            myDiv.Attributes.Clear();
                            myDiv.Attributes.Add("class", "form-group");
                            ctrl = ctrlInner.Controls[0];
                        }
                        else
                            ctrl = ctrlInner;

                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                        switch (ctrl.GetType().ToString())
                        {

                            case "System.Web.UI.WebControls.TextBox":
                                if (((TextBox)ctrl).ID == IdCtrl)
                                {
                                    ((TextBox)ctrl).Focus();
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                }

                                break;

                            case "System.Web.UI.WebControls.Label":
                                if (((Label)ctrl).ID == IdCtrl)
                                {
                                    ((Label)ctrl).Focus();
                                    return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                }

                                break;

                            //RCA 16/01/2018
                            //poner el focus en el textarea
                            case "System.Web.UI.HtmlControls.HtmlTextArea":

                                if (((HtmlTextArea)ctrl).ID == IdCtrl)
                                {
                                    ((HtmlTextArea)ctrl).Focus();
                                }
                                break;


                            case "System.Web.UI.WebControls.DropDownList":

                                if (((DropDownList)ctrl).ID == IdCtrl)
                                {
                                    ((DropDownList)ctrl).Focus();

                                }

                                break;
                        }
                    } //foreach
                }//foreach
            }//foreach

            return false; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }

        private void CrearTablaPiePagina()
        {

            TableRow Renglon;
            TableCell Columna;

            //renglon1
            Renglon = new TableRow();
            this.TablePiePagina.Rows.Add(Renglon);

            //agregar columnas al renglon1
            Columna = new TableCell();
            Columna.ColumnSpan = 4;
            Columna.Font.Bold = true;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BorderWidth = 1;
            Columna.Width = 800;
            Columna.HorizontalAlign = HorizontalAlign.Center;
            Columna.BackColor = System.Drawing.Color.LightGray;
            Columna.BorderColor = System.Drawing.Color.Gray;
            Columna.Text = "La información capturada es responsabilidad del servidor público y  la Institución que la envía; el INDAABIN se reserva el derecho a solicitar información adicional y/o probatoria.";
            Renglon.Controls.Add(Columna);



            //renglon2
            Renglon = new TableRow();
            this.TablePiePagina.Rows.Add(Renglon);

            //agregar columnas al renglon1
            Columna = new TableCell();
            Columna.ColumnSpan = 4;
            Columna.BorderStyle = BorderStyle.Solid;
            Columna.BorderWidth = 1;
            Columna.Width = 800;
            Columna.HorizontalAlign = HorizontalAlign.Left;
            Columna.Text = "Nota: La presente evaluación no exime del cumplimiento de toda la normatividad que al respecto se publica en el Capítulo IX del MANUAL DE RECURSOS MATERIALES Y SERVICIOS GENERALES.";
            Renglon.Controls.Add(Columna);
        }

        protected void ButtonEnviarOpinion_Click(object sender, EventArgs e)
        {
            if (this.InsertRespuestaCptosEmisionOpinion())
            {
                //desocupar sessiones, posiblemente generadas por este modulo.
                Session["ListValorRespuestaConcepto"] = null;
                Session["ListCptosOpinionNuevo"] = null;
                Session["ListCptosOpinionSustitucion"] = null;
                Session["ListCptosOpinionContinuacion"] = null;
                Session["NumContratoHist"] = null;
                Session["FolioContrato"] = null;

                string mensaje = "La emisión ha sido registrada con éxito";
                this.LabelInfoEnviar.Text = "<div class='alert alert-success'>";
                this.LabelInfoEnviar.Text += "<b>¡Felicidades! </b>";
                this.LabelInfoEnviar.Text += mensaje;
                this.LabelInfo.Text = this.LabelInfoEnviar.Text;

                this.pnlControles.Enabled = false;
                this.ButtonEnviarOpinion.Enabled = false;
                this.ButtonverificarNormatividad.Enabled = false;
                this.ButtonCancelar.Text = "Regresar";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAlerta", "alert(\"" + mensaje + "\");", true);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAcuse", "window.open('AcuseEmisionOpinion.aspx?IdFolio=" + strtFolioEmision + "&TipoArrto=" + strTipoArrendamiento + "&isInsert=true', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');", true);
                this.ButtonEnviarOpinion.Enabled = false;
                this.ButtonverificarNormatividad.Enabled = false;
                //window.open('../EmisionOpinion/AcuseEmisionOpinion.aspx?IdFolio=' + folio + '&TipoArrto=' + tipo, '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');

            }
            //else  No implementar porque ya esta implementado en el metodo del if
        }

        //Validar, Recuperar las respuestas para cada concepto y insertar en la BD las respuestas recolectadas.
        private Boolean InsertRespuestaCptosEmisionOpinion()
        {
            Boolean Ok = false;
            //****  1) Validacion de las Respuestas de Conceptos por determinado tipo emisión de opinión  *****
            if (this.ValidarRespuestasCptos()) //validar entrada de datos requerida
            {
                try
                {
                    //validar si existe la lista de objetos que almacenan los espacios de respuestas por concepto.
                    if (Session["ListValorRespuestaConcepto"] != null)
                    {
                        //lista de respuestas, pendientes de valor, recuperar cptos de la session, guardada en la presentacion de los cptos en la vista
                        List<ValorRespuestaConcepto> ListValorRespuestaConcepto = (List<ValorRespuestaConcepto>)Session["ListValorRespuestaConcepto"];
                        int? iFolioSMOI = null;

                        //Recolectar datos de respuesta  del usuario para cada cpto de acuerdo a su Id
                        //Para cada respuesta colocarlo en el objeto de negocio
                        //iterar por cada renglon de la tabla = concepto
                        foreach (TableRow row in TableEmisionOpinion.Rows)
                        {
                            if (Util.IsNumeric(row.ID))
                            {
                                //iterar por cada columna/celda del renglon
                                foreach (TableCell cell in row.Cells)
                                {
                                    //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                                    foreach (Control ctrlInner in cell.Controls)
                                    {
                                        Control ctrl;
                                        if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                                        {
                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                            myDiv.Attributes.Clear();
                                            myDiv.Attributes.Add("class", "form-group");
                                            ctrl = ctrlInner.Controls[0];
                                        }
                                        else
                                            ctrl = ctrlInner;

                                        if (ctrl.ID == "TextBoxFolioSMOI")
                                        {
                                            if ((((TextBox)ctrl).Text.Length > 0))
                                                iFolioSMOI = Convert.ToInt32((((TextBox)ctrl).Text));
                                            else
                                                iFolioSMOI = 0; //default, se nulificara en el sP, porque no existe este FolioSMOI
                                            break; //romper iteraccion para ya no entrar en el swithc
                                        }

                                        //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                                        switch (ctrl.GetType().ToString())
                                        {
                                            case "System.Web.UI.WebControls.DropDownList":

                                                //iterar por la lista de espera de Cpto-Respuesta para encontrar el Id y asignar respuesta desde la tabla de la vista
                                                foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                                {
                                                    if (objResp.NumOrdenVisual == row.ID)
                                                    {
                                                        objResp.ValorResp = Convert.ToDecimal(((DropDownList)ctrl).SelectedValue);
                                                        break; //romper for, pues ya fue encontrado
                                                    }
                                                }
                                                break;
                                            case "System.Web.UI.WebControls.TextBox":
                                                foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                                {
                                                    if (objResp.NumOrdenVisual == row.ID)
                                                    {
                                                        objResp.ValorResp = Convert.ToDecimal((((TextBox)ctrl).Text));
                                                        break; //romper for, pues ya fue encontrado
                                                    }
                                                }
                                                break;

                                            case "System.Web.UI.WebControls.Label":
                                                foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                                {
                                                    if (objResp.NumOrdenVisual == row.ID)
                                                    {
                                                        if ((((Label)ctrl).Text) != "--")
                                                        {
                                                            //se guardo a proposito primero en una variable de tipo Decimal, porque marcaba excepcion, quizas por algun formato interior en la cadena
                                                            Decimal valor = Util.ToDecimal((((Label)ctrl).Text));
                                                            objResp.ValorResp = valor;
                                                        }
                                                        break; //romper for, pues ya fue encontrado
                                                    }
                                                }
                                                break;
                                        }//switch

                                    }//foreach
                                } //foreach
                            }//if ctrl Id

                        }//foreach

                        //****  2) Insercion de Respuestas de Conceptos en la BD  *****
                        //Una vez recolectadas las respuestas para cada cpto, entonces: llamar el objeto de capa de negocio para pasar los datos a la BD

                        //Obtener valores: IdInstitucion y IdUsuarioRegistro del contexto del SSO de la cuenta del autenticado
                        int IdInstitucionUsr = ((SSO)Session["Contexto"]).IdInstitucion.Value; //sso
                        int IdUsuarioRegistro = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso
                        String IdCargoUsr = ((SSO)Session["Contexto"]).Cargo; //SSO


                        Control LabelNombreEmision = this.ctrlUsuarioInfo.FindControl("LabelUsr");
                        string NombreUsuarioEmision = ((Label)LabelNombreEmision).Text;

                        //recuperar del WebConfig la cadena de conexion
                        string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString;

                        //Armar DataTable con valores Cpto-Respuesta
                        int IdConceptoRespValor;
                        decimal ValorResp;

                        //definicion de la estructura de la tabla parametro
                        System.Data.DataTable DataTableRespuestaCptoList = new System.Data.DataTable();
                        DataTableRespuestaCptoList.Columns.Add("IdConceptoRespValor", typeof(int));
                        DataTableRespuestaCptoList.Columns.Add("ValorResp", typeof(decimal));

                        //pasar los datos de lista de negocio para vaciar al DataTable requerido por el SP
                        foreach (ValorRespuestaConcepto objRespCpto in ListValorRespuestaConcepto)
                        {
                            if (objRespCpto.ValorResp.HasValue) //solo cptos con respuesta, se guardan
                            {
                                IdConceptoRespValor = objRespCpto.IdConceptoRespValor;
                                ValorResp = objRespCpto.ValorResp.Value;
                                //agregar el renglon
                                DataTableRespuestaCptoList.Rows.Add(IdConceptoRespValor, ValorResp);
                            }
                        }
                        ListValorRespuestaConcepto = null; // desocupar
                        int FolioEmisionOpinion = 0; //parametro de salida

                        //obtener controles del UserControl
                        Control LabelDireccion = this.ctrlDireccionLectura.FindControl("LabelDireccion");
                        string DireccionInmueble = ((Label)LabelDireccion).Text;

                        //idInmueble para el que se realiza la emisión de opinión
                        Control LabelIdInmuebleArrto = this.ctrlDireccionLectura.FindControl("LabelIdInmuebleArrto");
                        int IdInmuebleArrendamiento = Convert.ToInt32(((Label)LabelIdInmuebleArrto).Text);

                        //RCA 18/01/2018
                        //justificacion para realizar la emision de opinion
                        string Justificacion = "";
                        string TxtJustificacion = this.EncontrarValorIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad");
                        if (!string.IsNullOrEmpty(TxtJustificacion))
                        {
                            Justificacion = TxtJustificacion;
                        }

                        Control LabelInstitucion = this.ctrlUsuarioInfo.FindControl("LabelInstitucion");
                        string InstitucionUsr = ((Label)LabelInstitucion).Text;

                        //crear sello digital
                        string CadenaOriginal = "||Invocante:[" + InstitucionUsr + "] || Inmueble:[" + DireccionInmueble + "]||Fecha:[" + DateTime.Today.ToLongDateString() + "]||" + Guid.NewGuid().ToString();
                        //generar el sello diigital, con la llave de ciframiento
                        string SelloDigital = UtilContratosArrto.Encrypt(CadenaOriginal, true, "EmisionOpinion");

                        //Recuperar el Folio de contrato en una variable y la bandera si es de hisotorico ya puede venir de la tabla de Contratos ó de la tabla de ContratosHistoricos
                        Boolean? EsContratoHistorico = null;
                        int? FolioContrato = null;

                        EsContratoHistorico = false; //el Folio de Contrato no es referenciado de ContratosHistorico
                        if (this.LabelIdContrato.Text != "0")
                            FolioContrato = Convert.ToInt32(this.LabelIdContrato.Text);
                        else
                            FolioContrato = null;

                        //****  3) Inserción en BD de las Respuestas de Conceptos por determinado tipo emisión de opinión *****

                        //   FolioEmisionOpinion = new NGConceptoRespValor().InsertEmisionOpinion("Nuevo", IdInstitucionUsr, IdCargoUsr, IdUsuarioRegistro, "Opinión Nuevo Arrendamiento", CadenaOriginal, SelloDigital, DataTableRespuestaCptoList, strConnectionString, IdInmuebleArrendamiento);
                        FolioEmisionOpinion = new NGConceptoRespValor().InsertEmisionOpinion(strTipoArrendamiento, IdInstitucionUsr, IdCargoUsr, IdUsuarioRegistro, strTemaOpinion, CadenaOriginal, SelloDigital, DataTableRespuestaCptoList, strConnectionString, IdInmuebleArrendamiento, EsContratoHistorico, FolioContrato, iFolioSMOI, Justificacion, string.Empty, string.Empty);

                        if (FolioEmisionOpinion > 0)
                        {
                            strtFolioEmision = FolioEmisionOpinion.ToString();
                            DataTableRespuestaCptoList = null;

                            //RCA 13/08/2018
                            string UrlAbrirQREmision = "AcuseEmisionOpinion.aspx?IdFolio=" + strtFolioEmision + "&TipoArrto=" + strTipoArrendamiento + "&isInsert=true";


                            //actuaaizacion de qr tipo de ddocuemnto 2 ya que es uns emision de opinion
                            string QR = UtilContratosArrto.GenerarCodigoQR(FolioEmisionOpinion.ToString(), 2, NombreUsuarioEmision, UrlAbrirQREmision);

                            //obtenemos el idaplicacion de la emision
                            int IdAplicacionEmision = new NGConceptoRespValor().ObtenerIdSMOI(Convert.ToInt32(FolioEmisionOpinion), 2);

                            if (!string.IsNullOrEmpty(QR) && IdAplicacionEmision > 0)
                            {
                                //actualizamos el campo de la QR
                                Ok = new NGConceptoRespValor().ActualizarQRSMOI(QR, IdAplicacionEmision);
                            }


                            Ok = true;
                        }
                        else
                        {
                            this.LabelInfo.Text = "No fue posible registrar las respuestas de la Emisión de Opinión de: " + strTipoArrendamiento + " Arrendamiento, vuelve a intentar el envío ó reporta al área de Sistemas.";
                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                            this.MostrarMensajeJavaScript(this.LabelInfo.Text);
                            DataTableRespuestaCptoList = null; //desocupar
                        }
                    }//if de validacion de session con list de respuestas
                    else
                    {
                        Msj = "No existe el objeto para recolectar las respuestas, vuelva a intentar o reporte a Sistemas.";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                        this.MostrarMensajeJavaScript(Msj);
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

                    Msj = "Ha ocurrido un error al procesar la emisión de opinión. Contacta al área de sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    MostrarMensajeJavaScript(Msj);
                    this.LabelInfo.Focus();

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

            }//if validacion de ctrl de entrada
            return Ok;
        }

        //Validar  datos de respuesta  del usuario cada cpto de acuerdo a su Id
        //Nota: existen conceptos que tienen una validacion especifica de acuerdo al tipo de Emisión de Opinión.
        private Boolean ValidarRespuestasCptos()
        {
            Boolean Ok = true;
            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacio

            //validar el folio smoi

            if (TableEmisionOpinion.Rows.Count == 0)
                Ok = false;

            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in TableEmisionOpinion.Rows)
            {
                if (Util.IsNumeric(row.ID))
                {
                    //iterar por cada columna/celda del renglon
                    foreach (TableCell cell in row.Cells)
                    {
                        //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
                        foreach (Control ctrlInner in cell.Controls)
                        {
                            Control ctrl;
                            if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                            {
                                HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                myDiv.Attributes.Clear();
                                myDiv.Attributes.Add("class", "form-group");
                                ctrl = ctrlInner.Controls[0];
                            }
                            else
                                ctrl = ctrlInner;

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                            switch (ctrl.GetType().ToString())
                            {

                                case "System.Web.UI.WebControls.DropDownList":

                                    //casos particulares de validacion de respuestas x concepto
                                    switch (strTemaOpinion)
                                    {
                                        case "Opinión Sustitución Arrendamiento":
                                        case "Opinión Nuevo Arrendamiento":


                                            //validar que si el cpto: 3.3 esta como "--" y esta habilitado, se seleccione una valor Si ó No, necesariamente
                                            //el concepto 3.3 se permite la entrada "--" cuando el valor de Supm2SMOI <= SupRentada

                                            if (ctrl.ID == "3.3DropDownListRespuesta")
                                            {

                                                ////obtener el valor del coontrol: TextBoxFolioSMOI, para pasarlo a la funcion de recuperacion de FolioSMOI (sig. instruccion)
                                                //String sFolioSMOI = EncontrarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI");

                                                ////independientemente que el usuario explicitamente dio clic, volver a ejecutar
                                                //this.RecuperaValoresFolioSMOI(Convert.ToInt32(sFolioSMOI));

                                                if (((DropDownList)ctrl).Enabled) //si esta habilitado, se debe seleccionar un valor
                                                {
                                                    if (((DropDownList)ctrl).SelectedItem.Text == "--")
                                                    {
                                                        HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                        myDiv.Attributes.Clear();
                                                        myDiv.Attributes.Add("class", "form-group has-error");
                                                        Msj = "Debes seleccionar una opción en la lista del concepto: [" + row.ID.ToString() + "].";
                                                        this.LabelInfo.Text = this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                        this.MostrarMensajeJavaScript(Msj);
                                                        ((DropDownList)ctrl).Focus();
                                                        return false; //romper todos los ciclos
                                                    }

                                                }

                                            }

                                            //RCA 17/01/2018
                                            //vaidacion en las emisiones de opinion de nuevo para el dropdownlist 11
                                            else if (ctrl.ID == "11DropDownListRespuesta")
                                            {
                                                if (((DropDownList)ctrl).SelectedItem.Text == "No")
                                                {
                                                    //debe de haber una cadena en el concepto 11.01
                                                    string sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad");
                                                    if (string.IsNullOrEmpty(sValorRespuesta))
                                                    //if(sValorRespuesta == null || sValorRespuesta.Length == 0)
                                                    {
                                                        HtmlGenericControl myDiv = new HtmlGenericControl();
                                                        myDiv.Attributes.Clear();
                                                        myDiv.Attributes.Add("class", "form-group has-error");
                                                        Msj = "Debes de escribir alguna justificacion en el concepto [11]";
                                                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                        this.MostrarMensajeJavaScript(Msj);
                                                        this.SetFocoIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad"); //poner el foco en un ctrl. en especifico

                                                        return false; //romper todos los ciclos
                                                    }

                                                    //validar que la longitud maxima sea de 300 caracteres
                                                    if (sValorRespuesta.Length > 300)
                                                    {
                                                        HtmlGenericControl myDiv = new HtmlGenericControl();
                                                        myDiv.Attributes.Clear();
                                                        myDiv.Attributes.Add("class", "form-group has-error");
                                                        Msj = "La longitud excede el máximo de caracteres permitidos";
                                                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                        this.MostrarMensajeJavaScript(Msj);
                                                        this.SetFocoIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad"); //poner el foco en un ctrl. en especifico

                                                        return false; //romper todos los ciclos
                                                    }
                                                }

                                            }

                                            else
                                            {
                                                if (((DropDownList)ctrl).Enabled) //solo si esta habilitado, validar el contenido seleccionado
                                                {
                                                    if (((DropDownList)ctrl).SelectedItem.Text == "--")
                                                    {
                                                        HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                        myDiv.Attributes.Clear();
                                                        myDiv.Attributes.Add("class", "form-group has-error");
                                                        Msj = "Debes seleccionar un valor de respuesta para el concepto: [" + row.ID.ToString() + "].";
                                                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                        this.MostrarMensajeJavaScript(Msj);
                                                        ((DropDownList)ctrl).Focus();

                                                        return false; //romper todos los ciclos
                                                    }
                                                }
                                            }
                                            break;

                                        case "Opinión Continuación Arrendamiento":
                                            //validar que la pregunta #4 si se respondio Si, este seleccionada el valor en la pregunta #5
                                            switch (ctrl.ID)
                                            {

                                                case "4DropDownListRespuesta":
                                                    if (((DropDownList)ctrl).SelectedItem.Text == "Si")
                                                    {
                                                        //debera haber valor en el concepto 5
                                                        string sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl("5DropDownListRespuesta");
                                                        if (sValorRespuesta == "--")
                                                        {
                                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                            myDiv.Attributes.Clear();
                                                            myDiv.Attributes.Add("class", "form-group has-error");
                                                            Msj = "Debes seleccionar una opción en la lista del concepto: [5]";
                                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                            this.MostrarMensajeJavaScript(Msj);
                                                            this.SetFocoIdCtrlRowCellCtrl("5DropDownListRespuesta"); //poner el foco en un ctrl en especifico

                                                            return false; //romper todos los ciclos
                                                        }
                                                    }
                                                    break;

                                                case "9DropDownListRespuesta":
                                                    if (((DropDownList)ctrl).SelectedItem.Text == "No")
                                                    {
                                                        //debera haber valor en el concepto 5
                                                        string sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl("10DropDownListRespuesta");
                                                        if (sValorRespuesta == "--")
                                                        {
                                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                            myDiv.Attributes.Clear();
                                                            myDiv.Attributes.Add("class", "form-group has-error");
                                                            Msj = "Debes seleccionar una opción en la lista del concepto: [10]";
                                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                            this.MostrarMensajeJavaScript(Msj);
                                                            this.SetFocoIdCtrlRowCellCtrl("10DropDownListRespuesta"); //poner el foco en un ctrl. en especifico

                                                            return false; //romper todos los ciclos
                                                        }

                                                    }
                                                    break;

                                                //RCA 17/01/2018
                                                //caso para validar en emisiones de opinion de continuacion el punto 11
                                                case "11DropDownListRespuesta":

                                                    if (((DropDownList)ctrl).SelectedItem.Text == "No")
                                                    {
                                                        //debe de haber una cadena en el concepto 11.01
                                                        string sValorRespuesta = this.EncontrarValorIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad");
                                                        if (sValorRespuesta == null || sValorRespuesta.Length == 0)
                                                        {
                                                            HtmlGenericControl myDiv = new HtmlGenericControl();
                                                            myDiv.Attributes.Clear();
                                                            myDiv.Attributes.Add("class", "form-group has-error");
                                                            Msj = "Debes de escribir alguna justificacion en el concepto [11.1]";
                                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                            this.MostrarMensajeJavaScript(Msj);
                                                            this.SetFocoIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad"); //poner el foco en un ctrl. en especifico

                                                            return false; //romper todos los ciclos
                                                        }

                                                        //validar que la longitud maxima sea de 300 caracteres
                                                        if (sValorRespuesta.Length > 300)
                                                        {
                                                            HtmlGenericControl myDiv = new HtmlGenericControl();
                                                            myDiv.Attributes.Clear();
                                                            myDiv.Attributes.Add("class", "form-group has-error");
                                                            Msj = "La longitud excede el máximo de caracteres permitidos";
                                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                            this.MostrarMensajeJavaScript(Msj);
                                                            this.SetFocoIdCtrlRowCellCtrl("TextAreaJustificacionCertificadoSeguridad"); //poner el foco en un ctrl. en especifico

                                                            return false; //romper todos los ciclos
                                                        }
                                                    }


                                                    break;

                                                default:
                                                    if (((DropDownList)ctrl).Enabled) //solo si esta habilitado, validar el contenido seleccionado
                                                    {
                                                        if (((DropDownList)ctrl).SelectedItem.Text == "--")
                                                        {
                                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                            myDiv.Attributes.Clear();
                                                            myDiv.Attributes.Add("class", "form-group has-error");
                                                            Msj = "Debes seleccionar un valor de respuesta para el concepto: [" + row.ID.ToString() + "].";
                                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                            this.MostrarMensajeJavaScript(Msj);
                                                            ((DropDownList)ctrl).Focus();

                                                            return false; //romper todos los ciclos

                                                        }
                                                    }
                                                    break;

                                            }
                                            break;

                                        default: //es un dropdownlists sin validacion especifica por tipo de opinion
                                            if (((DropDownList)ctrl).Enabled) //solo si esta habilitado, validar el contenido seleccionado
                                            {
                                                if (((DropDownList)ctrl).SelectedItem.Text == "--")
                                                {
                                                    HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                    myDiv.Attributes.Clear();
                                                    myDiv.Attributes.Add("class", "form-group has-error");
                                                    Msj = "Debes seleccionar un valor de respuesta para el concepto: [" + row.ID.ToString() + "].";
                                                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                    this.MostrarMensajeJavaScript(Msj);
                                                    ((DropDownList)ctrl).Focus();

                                                    return false; //romper todos los ciclos

                                                }
                                            }
                                            break;

                                    }

                                    break;




                                case "System.Web.UI.WebControls.TextBox":
                                    if (ctrl.ID == "TextBoxFolioSMOI")
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                myDiv.Attributes.Clear();
                                                myDiv.Attributes.Add("class", "form-group has-error");
                                                Msj = "Debe proporcionar el Folio de tabla SMOI [" + row.ID.ToString() + "]."; //
                                                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                this.MostrarMensajeJavaScript(Msj);
                                                ((TextBox)ctrl).Focus();

                                                return false; //romper todos los ciclos
                                            }
                                            else //si se proporciono el folio de smoi, pero volver a validar por si el usuario cambia el texto y no le da comprobar
                                                if (this.RecuperaValoresFolioSMOI(Convert.ToInt32(((TextBox)ctrl).Text), false) == false)
                                                return false;
                                        }
                                        //RCA 18/04/2018
                                        //validacion por si viene vacio el folio de la tabla SMOI
                                        else //no hay contenido en el ctrl
                                        {
                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                            myDiv.Attributes.Clear();
                                            myDiv.Attributes.Add("class", "form-group has-error");
                                            Msj = "Debe proporcionar el Folio de tabla SMOI [" + row.ID.ToString() + "]."; //
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                            this.MostrarMensajeJavaScript(Msj);
                                            ((TextBox)ctrl).Focus();

                                            return false; //romper todos los ciclos
                                        }

                                    }


                                    if (ctrl.ID == "TextBoxSupM2xArrendar")
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsNumeric(((TextBox)ctrl).Text)) == false)
                                            {
                                                HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                myDiv.Attributes.Clear();
                                                myDiv.Attributes.Add("class", "form-group has-error");
                                                Msj = "Debes proporcionar los metros cuadrados que se arrendan, en el concepto: [" + row.ID.ToString() + "].";
                                                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                this.MostrarMensajeJavaScript(Msj);
                                                ((TextBox)ctrl).Focus();

                                                return false; //romper todos los ciclos
                                            }
                                            else
                                            {
                                                if (Convert.ToDecimal(((TextBox)ctrl).Text) < 1)
                                                {
                                                    HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                                    myDiv.Attributes.Clear();
                                                    myDiv.Attributes.Add("class", "form-group has-error");
                                                    Msj = "Los metros cuadrados que se arrendan deben ser mayor a cero, en el concepto: [" + row.ID.ToString() + "].";
                                                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                                    this.MostrarMensajeJavaScript(Msj);
                                                    ((TextBox)ctrl).Focus();

                                                    return false; //romper todos los ciclos

                                                }
                                            }


                                            //desactivar, dejar Folio SMOI opcional
                                            //else //si se proporciono el folio de smoi, pero volver a validar por si el usuario cambia el texto y no le da comprobar
                                            //{


                                            //    string Folio = this.EncontrarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI");
                                            //    //con ToDecimal quitar el formato de separador de comas
                                            //    this.RecuperaValoresFolioSMOI(Convert.ToInt32(Folio));
                                            //}
                                        }
                                        else //no hay contenido en el ctrl
                                        {
                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                            myDiv.Attributes.Clear();
                                            myDiv.Attributes.Add("class", "form-group has-error");
                                            Msj = "Debe proporcionar los metros cuadrados que se arrendan [" + row.ID.ToString() + "].";
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                            this.MostrarMensajeJavaScript(Msj);
                                            ((TextBox)ctrl).Focus();

                                            return false; //romper todos los ciclos
                                        }
                                    }
                                    break;


                            }//switch

                        }//foreach
                    } //foreach
                }//if ctrl Id

            }//foreach

            //Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
            return Ok;

        }

        //cancelar el registro, limpiar posible generacion de variables de session y salir a un URL.
        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            //desocupar sessiones, posiblemente pobladas.
            Session["ListValorRespuestaConcepto"] = null;
            Session["ListCptosOpinionNuevo"] = null;
            Session["ListCptosOpinionSustitucion"] = null;
            Session["ListCptosOpinionContinuacion"] = null;
            //redireccionar a la presentacion del ACUSE
            //Response.Redirect("ControladorEmisionOpinion.aspx");
            Response.Redirect("~/InmuebleArrto/BusqMvtosEmisionOpinionInmuebles.aspx", false);
        }

        //2 posibles excepciones  a la normtividad
        protected void ButtonverificarNormatividad_Click(object sender, EventArgs e)
        {
            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacion

            //validar captura total
            if (this.ValidarRespuestasCptos())
            {
                Boolean OK_IncumpleNormatividad = false;
                String MsjSalida;

                //1.si no se proporciono el Folio SMOI y se selecciono que no se tiene el dictamen, exponer excepcion a la normtividad

                ////obtener el valor del control: TextBoxFolioSMOI, para pasarlo a la funcion de recuperacion de FolioSMOI (sig. instruccion)
                String sFolioSMOI = EncontrarValorIdCtrlRowCellCtrl("TextBoxFolioSMOI");

                Msj = string.Empty;
                this.LabelInfoEnviar.Text = string.Empty;
                this.LabelInfo.Text = string.Empty;

                if (String.IsNullOrEmpty(sFolioSMOI) && this.EncontrarValorIdCtrlRowCellCtrl("3.3DropDownListRespuesta") == "No")
                {
                    OK_IncumpleNormatividad = true;
                    Msj = "-No se proporciona un folio SMOI y no cuenta con el correspondiente Dictamen del INDAABIN, sobre aplicación de la Tabla SMOI <br/>";
                }
                else
                {
                    string sName = Request.QueryString["TemaOpinion"];
                    if (sName != "3")
                    {
                        //2.si la sup. de m2 a arrendar (item 3) es > Superficie m2 de acuerdo a la tabla SMOI (3.2)
                        string strSupM2xArrendar = this.EncontrarValorIdCtrlRowCellCtrl("TextBoxSupM2xArrendar");
                        string strSupM2SMOI = this.EncontrarValorIdCtrlRowCellCtrl("LabelSupM2TablaSMOI");
                        if (Convert.ToDecimal(strSupM2xArrendar) > Convert.ToDecimal(strSupM2SMOI))
                        {
                            OK_IncumpleNormatividad = true;
                            Msj = Msj + "-Los metros cuadrados por arrendar son mayor a los especificados en la tabla SMOI  <br/>";
                        }
                    }
                }

                if (OK_IncumpleNormatividad)
                {
                    MsjSalida = "<b>Validación de la Normatividad en la información ingresada para la solicitud de una emisión de opinión de arrendamiento </b></br>";
                    MsjSalida = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + MsjSalida + "<br/>" + Msj + "</div>";
                    this.LabelInfoEnviar.Text = MsjSalida;
                    this.LabelInfo.Text = this.LabelInfoEnviar.Text;
                }
                else
                {
                    this.LabelInfoEnviar.Text = "<div class='alert alert-success'>";
                    this.LabelInfoEnviar.Text += "<b>¡Felicidades! </b>";
                    this.LabelInfoEnviar.Text += "La información capturada cumple con la normatividad";
                    this.LabelInfo.Text = this.LabelInfoEnviar.Text;
                }
            }
            //Page.MaintainScrollPositionOnPostBack = true;
        }



    }//clase
}