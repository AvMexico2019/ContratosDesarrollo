
using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;
using System;
using System.Collections.Generic;

using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI
{

  
    public partial class TablaSMOI : System.Web.UI.Page
    {
        private List<ConceptoRespValor> ListCptosSMOI_FactorX; //cptos de la BD del factorX
        private List<ConceptoRespValor> ListCptosSMOI_FactorZ; //cptos de la BD del factorZ
        private List<ValorRespuestaConcepto> ListValorRespuestaConcepto; //para guardado de las respuestas a los cptos.
        private static string strValorParametroPonderacionFactorZ;
        private string Msj;
        private string FolioGeneradoSmoi;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strTrace = "";
            try
            {
                strTrace += "Inicio -> ";
                if (!this.IsPostBack)
                {
                    this.lblNombreSession.Value = Session.SessionID + "TablaSMOI";
                    Session["ListCptosSMOI_FactorX"] = null;
                    Session["ListCptosSMOI_FactorZ"] = null;
                    Session[this.lblNombreSession.Value] = null;
                    strTrace += "Session -> " + this.lblNombreSession.Value;
                }

                if (Session["Contexto"] == null)
                {
                    strTrace += "Contexto NULL -> ";
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""), false);
                }


                String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                strTrace += "NombreRol por Usuario -> ";

                //determinar el tipo de usuario autenticado
                if (UtilContratosArrto.ValidarRolAcceso("OIC", (SSO)Session["Contexto"]))
                {
                    this.ButtonEnviar.Visible = false; //no puede registrar Solicitudes
                    strTrace += "Usuario OIC -> ";
                }


                if (this.ObtenerCptos() && NombreRol != "OIC")
                {
                    Msj = "Proporciona los valores a los conceptos que en su caso apliquen y da clic en Enviar para registrar la información y se te proporcione un acuse con un número de Folio SMOI.";
                    this.LabelInfo.Text = "<div class='alert alert-info'><strong> Información: </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario 
                }
                strTrace += "Termina Load -> ";
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el valor del concepto. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);

                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message + strTrace,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }
        }

        #region Contruccion de tablas
        private Boolean ObtenerCptos()
        {
            Boolean Ok = false;
            try
            {
                //obtener el Id del tema de los conoceptos  de SMOI-factorX
                byte IdTemaSMOI_FactorX = new NG_Catalogos().ObtenerIdTemaXDesc("SMOI FactorX");
                byte IdTemaSMOI_FactorZ = new NG_Catalogos().ObtenerIdTemaXDesc("SMOI FactorZ");

                if (IdTemaSMOI_FactorX > 0 && IdTemaSMOI_FactorZ > 0)
                {
                    int InstitucionUserAutenticado = ((SSO)Session["Contexto"]).IdInstitucion.Value;


                    if (Session["ListCptosSMOI_FactorX"] == null)
                    {
                        //poblar de la BD
                        ListCptosSMOI_FactorX = new NGConceptoRespValor().ObtenerCptosRespuestaValorSMOI(IdTemaSMOI_FactorX, InstitucionUserAutenticado); //1="SMOI FactorX"
                        Session["ListCptosSMOI_FactorX"] = ListCptosSMOI_FactorX;//guardar en session, para no conectar a BD entre postbacks
                    }
                    else //poner en el objeto, la session de cptos ya cargada
                        ListCptosSMOI_FactorX = (List<ConceptoRespValor>)Session["ListCptosSMOI_FactorX"];


                    //si existen CptosValorRespuesta, entonces exponer la tabla en la vista
                    if (ListCptosSMOI_FactorX.Count > 0)
                    {
                        //exponer tabla de los conpcetos en la vista
                        if (this.CrearTablaCptosSMOI_FactorX())
                        {
                            if (Session["ListCptosSMOI_FactorZ"] == null)
                            {
                                //poblar de la BD
                                //poblado de Tabla con conceptos del SMOI-FactorZ
                                ListCptosSMOI_FactorZ = new NGConceptoRespValor().ObtenerCptosRespuestaValorSMOI(IdTemaSMOI_FactorZ, InstitucionUserAutenticado); //6="SMOI FactorZ"
                                Session["ListCptosSMOI_FactorZ"] = ListCptosSMOI_FactorZ;
                            }
                            else //poner en el objeto, la session de cptos ya cargada
                                ListCptosSMOI_FactorZ = (List<ConceptoRespValor>)Session["ListCptosSMOI_FactorZ"];

                            if (ListCptosSMOI_FactorZ.Count > 0)
                            {
                                //exponer los conceptos en la vista
                                if (this.CrearTablaCptosSMOI_FactorZ())
                                {
                                    if (String.IsNullOrEmpty(strValorParametroPonderacionFactorZ))
                                        //obtener el valor de catParamentros para el Factor-Y
                                        strValorParametroPonderacionFactorZ = new NG_Catalogos().ObtenerValorCatParametro("SMOI m2 Factor-Y");

                                    if (Util.IsNumeric(strValorParametroPonderacionFactorZ))
                                    {
                                        this.CrearTablaTotalResultados();
                                        this.CrearTablaPiePagina();
                                        this.TableSMOI.Visible = true;
                                        Ok = true;
                                    }
                                }
                            }//if factorZ
                        }//if factorX
                    }
                }
                else
                {
                    Msj = "No existen los conceptos de SMOI solicitados, para obtener los conceptos a presentar en la vista, vuelve a intentar o contacta al área de sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong>¡Precaución! </strong>" + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar el valor del concepto. Contacta al área de sistemas.";
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

        private Boolean CrearTablaCptosSMOI_FactorX()
        {
            Boolean Ok = false;
            try
            {
                TableRow Renglon;
                TableCell Columna;

                //****** renglon0: Encabezado
                Renglon = new TableRow();
                this.TableSMOI.Rows.Add(Renglon);

                //columna: de renglon1
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.Font.Bold = true;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.BackColor = System.Drawing.Color.LightGray;
                Columna.BorderColor = System.Drawing.Color.Gray;
                Columna.Text = "Superficie Máxima a Ocupar por Todos los Niveles [Factor X]";
                Renglon.Controls.Add(Columna);


                //crear objeto, si es el 1er postback y aun no se crea la estrucutura, para el momento en que se recolectara la respuesta de cada cpto.
                if (Session[this.lblNombreSession.Value] == null) //para no guardar n veces por cada postback
                    //recuperar conceptos de BD
                    ListValorRespuestaConcepto = new List<ValorRespuestaConcepto>();


                //para cada renglon de la lista agregarlo a la tabla 
                //para cada objeto en la lista, recorrer y accesar a sus propiedades
                foreach (ConceptoRespValor objCptoSMOI in ListCptosSMOI_FactorX)
                {
                    //agregar renglon a la tabla 
                    Renglon = new TableRow();

                    //Id de Control en la Tabla
                    Renglon.ID = objCptoSMOI.NumOrdenVisual; //asiagnar ID  al renglon,  para posteriormente recuperar en la obtencion de las respuestas
                    this.TableSMOI.Rows.Add(Renglon);

                    //Cada renglon, se compondra de 5 columnas

                    //************************* Columna1 de # de Orden *****************************
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;

                    if (objCptoSMOI.IdRespuesta != 8)//es valor de BD
                    {
                        Columna.Text = objCptoSMOI.NumOrdenVisual;
                        //Guardar el espacio para  la  respuesta si es el 1er postaback, y no existe
                        if (Session[this.lblNombreSession.Value] == null)
                            //poblar objeto en donde se pondra la respuesta para guardar en la BD
                            ListValorRespuestaConcepto.Add(new ValorRespuestaConcepto { IdConceptoRespValor = objCptoSMOI.IdConceptoRespValor, NumOrden = objCptoSMOI.NumOrden, NumOrdenVisual = objCptoSMOI.NumOrdenVisual });
                    }
                    else //es encabezado
                    {
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //************************* Columna #2 de Desc Cpto ***********************************/

                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.Text = objCptoSMOI.DescripcionConcepto; //recuperacion de datos del contrato

                    if (objCptoSMOI.IdRespuesta == 8) //formatear la celda
                    {
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //****************columna #3 de valores expuestos [A] ******************************************/
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.HorizontalAlign = HorizontalAlign.Center;
                    Columna.BorderWidth = 1;
                    //Columna.Text = objCptoSMOI.ValorPonderacionRespuesta.ToString(); //recuperacion de datos del contrato

                    if (objCptoSMOI.IdRespuesta != 8) // N/A es Tema)
                    {
                        Label LabelCtrl = new Label();
                        LabelCtrl.ID = objCptoSMOI.NumOrden.ToString() + "-LabelA"; //darle un identificador
                        LabelCtrl.Text = objCptoSMOI.ValorPonderacionRespuesta.ToString();//poner el valor de la BD
                        Columna.Controls.Add(LabelCtrl);
                    }
                    else
                    {
                        Columna.Text = "Superficie Unitaria Máxima por Servidor Público (m2) [A]";
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //****************columna #4 RESPUESTA en ctrl: TextBox [B]******************************************/
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.HorizontalAlign = HorizontalAlign.Center;


                    //si el cpto tiene valor de opcion de respuesta, agregar la columna al renglon              
                    if (objCptoSMOI.IdRespuesta != 8) // N/A es Tema)
                    {
                        TextBox TextBoxCtrl = new TextBox();
                        TextBoxCtrl.Width = 100;
                        TextBoxCtrl.MaxLength = 5;
                        TextBoxCtrl.ID = objCptoSMOI.NumOrden.ToString() + "-TextBoxB"; //darle un identificador
                        TextBoxCtrl.Text = "0";
                        TextBoxCtrl.Style["text-align"] = "center";
                        TextBoxCtrl.Attributes.Add("onKeypress", "return (!(event.keyCode>=65) && event.keyCode!=32);");
                        TextBoxCtrl.AutoPostBack = true; //activar para que se active el evento de validar dependencia
                        TextBoxCtrl.EnableViewState = true;
                        TextBoxCtrl.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        TextBoxCtrl.TextChanged += new EventHandler(MultiplicarValoresSMOI_FactorX);
                        TextBoxCtrl.CssClass = "form-control";
                        TextBoxCtrl.Attributes.Add("onchange", "backFromErrorClass(this);");

                        Label LabelCtrlMax = new Label();
                        LabelCtrlMax.Visible = false;
                        LabelCtrlMax.ID = objCptoSMOI.NumOrden.ToString() + "-LabelMaxB";
                        LabelCtrlMax.Text = objCptoSMOI.ValorMaximo.ToString();

                        Label LabelCtrlError = new Label();
                        LabelCtrlError.Visible = false;
                        LabelCtrlError.ID = objCptoSMOI.NumOrden.ToString() + "-LabelErrorB";
                        LabelCtrlError.Text = "";
                        LabelCtrlError.CssClass = "error text-danger";

                        HtmlGenericControl myDiv = new HtmlGenericControl("div");
                        myDiv.Attributes.Add("class", "form-group");
                        myDiv.Controls.Add(TextBoxCtrl);
                        myDiv.Controls.Add(LabelCtrlMax);
                        myDiv.Controls.Add(LabelCtrlError);

                        Columna.Controls.Add(myDiv);

                        //control que solo permite numeros
                        AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                        FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                        FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                        Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                    }
                    else //es encabezado
                    {
                        Columna.Font.Bold = true;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                        Columna.Text = "Número de Servidores Públicos [B]";
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //********* columna #5 de calculo *******/

                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    if (objCptoSMOI.IdRespuesta != 8) // 8=N/A es Tema, no poner el orden si es un concepto de Tema
                    {
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BorderWidth = 1;
                        Columna.HorizontalAlign = HorizontalAlign.Center;

                        Label LabelCalculo = new Label();
                        LabelCalculo.Text = "0.00";
                        LabelCalculo.ID = objCptoSMOI.NumOrden.ToString() + "-LabelC"; //darle un identificador
                        Columna.Controls.Add(LabelCalculo);
                    }
                    else //es encabezado
                    {
                        Columna.HorizontalAlign = HorizontalAlign.Center;
                        Columna.Text = "Superficie Máxima de Ocupación por Nivel (m2) [C]=[AxB]";
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }
                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);
                }//foreach
                Ok = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al crear la tabla del Factor X. Contacta al área de sistemas.";
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

        private Boolean CrearTablaCptosSMOI_FactorZ()
        {
            Boolean Ok = false;

            try
            {
                TableRow Renglon;
                TableCell Columna;


                //****** renglon0: Encabezado
                Renglon = new TableRow();
                this.TableSMOI.Rows.Add(Renglon);

                //columna: de renglon1
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.Font.Bold = true;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.BackColor = System.Drawing.Color.LightGray;
                Columna.BorderColor = System.Drawing.Color.Gray;
                Columna.Text = "Áreas Complementarias [Factor Z]";
                Renglon.Controls.Add(Columna);

                //recuperar conceptos de BD
                //ListValorRespuestaConcepto = new List<ValorRespuestaConcepto>();


                //para cada renglon de la lista agregarlo a la tabla 
                //para cada objeto en la lista, recorrer y accesar a sus propiedades
                foreach (ConceptoRespValor objCptoSMOI in ListCptosSMOI_FactorZ)
                {
                    //agregar renglon a la tabla 
                    Renglon = new TableRow();

                    //Id de Control en la Tabla
                    Renglon.ID = objCptoSMOI.NumOrdenVisual + "-FactorZ"; //asiagnar ID a la columna para posteriormente recuperar en la obtencion de las respuestas
                    // Renglon.ID = objCptoSMOI.NumOrdenVisual;
                    this.TableSMOI.Rows.Add(Renglon);

                    //Cada renglon, se compondra de 5 columnas

                    //************************* Columna1 de # de Orden *****************************
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;

                    if (objCptoSMOI.IdRespuesta != 8)//es valor de BD
                    {

                        Columna.Text = objCptoSMOI.NumOrdenVisual;
                        //Guardar el espacio para  la  respuesta si es el 1er postaback, y no existe
                        if (Session[this.lblNombreSession.Value] == null)
                            //poblar objeto en donde se pondra la respuesta para guardar en la BD
                            ListValorRespuestaConcepto.Add(new ValorRespuestaConcepto { IdConceptoRespValor = objCptoSMOI.IdConceptoRespValor, NumOrden = objCptoSMOI.NumOrden, NumOrdenVisual = objCptoSMOI.NumOrdenVisual });
                    }
                    else //es encabezado
                    {
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //************************* Columna #2 de Desc Cpto ***********************************/

                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.Text = objCptoSMOI.DescripcionConcepto; //recuperacion de datos del contrato


                    if (objCptoSMOI.IdRespuesta == 8) //formatear la celda
                    {
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //****************columna #3 de valores expuestos [A] ******************************************/

                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.HorizontalAlign = HorizontalAlign.Center;
                    Columna.BorderWidth = 1;
                    //Columna.Text = objCptoSMOI.ValorPonderacionRespuesta.ToString(); //recuperacion de datos del contrato

                    if (objCptoSMOI.IdRespuesta != 8) // N/A es Tema)
                    {
                        Label LabelCtrl = new Label();
                        LabelCtrl.ID = objCptoSMOI.NumOrden.ToString() + "-LabelA-FactorZ"; //darle un identificador
                        LabelCtrl.Text = objCptoSMOI.ValorPonderacionRespuesta.ToString();//poner el valor de la BD
                        Columna.Controls.Add(LabelCtrl);
                    }
                    else
                    {
                        Columna.Text = "Factor de m2 x usuario";
                        Columna.Font.Bold = true;
                        Columna.BorderStyle = BorderStyle.Solid;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);

                    //****************columna #4 RESPUESTA en TextBox [B]******************************************/
                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;
                    Columna.HorizontalAlign = HorizontalAlign.Center;


                    //si el cpto tiene valor de opcion de respuesta, agregar la columna al renglon              
                    if (objCptoSMOI.IdRespuesta != 8) // N/A es Tema)
                    {
                        TextBox TextBoxCtrl = new TextBox();
                        TextBoxCtrl.Width = 100;
                        TextBoxCtrl.MaxLength = 5;
                        //TextBoxCtrl.ReadOnly = true;
                        TextBoxCtrl.ID = objCptoSMOI.NumOrden.ToString() + "-TextBoxB-FactorZ"; //darle un identificador
                        TextBoxCtrl.Text = "0";
                        TextBoxCtrl.Style["text-align"] = "center";
                        //agregar referencia de funcionalidad al control, para responder al textchange
                        TextBoxCtrl.AutoPostBack = true; //activar para que se active el evento de validar dependencia
                        TextBoxCtrl.EnableViewState = true;
                        TextBoxCtrl.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        TextBoxCtrl.TextChanged += new EventHandler(MultiplicarValoresSMOI_FactorZ);
                        Columna.Controls.Add(TextBoxCtrl);
                        //control que solo permite numeros
                        AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                        FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                        FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                        Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                    }
                    else //es encabezado
                    {

                        Columna.Font.Bold = true;
                        Columna.BackColor = System.Drawing.Color.LightGray;
                        Columna.BorderColor = System.Drawing.Color.Gray;
                        Columna.Text = "Número estimado de usuarios";

                    }

                    //agregar la columna al renglon
                    Renglon.Controls.Add(Columna);


                    //********* columna #5 de calculo final *******/

                    Columna = new TableCell();
                    Columna.BorderStyle = BorderStyle.Solid;
                    Columna.BorderWidth = 1;

                    if (objCptoSMOI.IdRespuesta != 8) // 8=N/A es Tema, no poner el orden si es un concepto de Tema
                    {

                        Columna.HorizontalAlign = HorizontalAlign.Center;
                        Label LabelCalculo = new Label();
                        LabelCalculo.Text = "0.00";
                        LabelCalculo.ID = objCptoSMOI.NumOrden.ToString() + "-LabelC-FactorZ"; //darle un identificador
                        Columna.Controls.Add(LabelCalculo);
                    }
                    else //es encabezado
                    {
                        Columna.HorizontalAlign = HorizontalAlign.Center;
                        Columna.Text = "Total en m2";
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
                if (Session[this.lblNombreSession.Value] == null)
                    //guardar en session para cuando el usuario proporciona las respuestas
                    //Session["ListValorRespuestaConcepto-SMOIFactorZ"] = ListValorRespuestaConcepto;
                    Session[this.lblNombreSession.Value] = ListValorRespuestaConcepto;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al crear la tabla del Factor Z. Contacta al área de sistemas.";
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

        private void CrearTablaTotalResultados()
        {
            try
            {
                TableRow Renglon;
                TableCell Columna;

                //****** renglon1: Encabezado
                Renglon = new TableRow();
                this.TableTotalResultados.Rows.Add(Renglon);

                //columna: de renglon1
                Columna = new TableCell();
                Columna.ColumnSpan = 6;
                Columna.Font.Bold = true;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.BackColor = System.Drawing.Color.LightGray;
                Columna.BorderColor = System.Drawing.Color.Gray;
                Columna.Text = "Resultados por Factor";
                Renglon.Controls.Add(Columna);


                //****** renglon2: Encabezado
                Renglon = new TableRow();
                this.TableTotalResultados.Rows.Add(Renglon);

                //agregar columnas al renglon2
                Columna = new TableCell();
                Columna.ColumnSpan = 6;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "La Superficie Máxima a Ocupar por Institución (SMOI), es la sumatoria de la superficie total de todos los espacios para el personal (X), las áreas de uso común (Y) y áreas complementarias.";
                Renglon.Controls.Add(Columna);


                //********** renglon3  Titulo de Columnas
                Renglon = new TableRow();
                this.TableTotalResultados.Rows.Add(Renglon);

                //columna1: de renglon3
                Columna = new TableCell();
                Columna.Font.Bold = true;
                Columna.ColumnSpan = 3;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                //Columna.Width = 400;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "Nombre del Factor";
                Renglon.Controls.Add(Columna);


                //columna2: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.Font.Bold = true;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 200;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "Factor";
                Renglon.Controls.Add(Columna);


                //columna3: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.Font.Bold = true;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Width = 270;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "M2 por Factor";
                Renglon.Controls.Add(Columna);

                //columna4: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.Font.Bold = true;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                //Columna.Width = 270;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "Persona(s)";
                Renglon.Controls.Add(Columna);


                //********** renglon3  [X]
                Renglon = new TableRow();
                this.TableTotalResultados.Rows.Add(Renglon);

                //columna1: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 3;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                //Columna.Width = 400;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "Superficie Máxima a Ocupar por Todos los Niveles (m2)";
                Renglon.Controls.Add(Columna);


                //columna2: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 200;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "X";
                Renglon.Controls.Add(Columna);


                //columna3: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Width = 270;
                //ctrl
                Label LabelTotalX = new Label();
                LabelTotalX.ID = "LabelTotalX"; //darle un identificador
                LabelTotalX.Text = "0.00";
                Columna.Controls.Add(LabelTotalX);//agregar el control label a la columna       
                Renglon.Controls.Add(Columna);

                //columna4: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                //Columna.Width = 270;
                //ctrl
                Label LabelTotalNumPersonasX = new Label();
                LabelTotalNumPersonasX.ID = "LabelTotalNumPersonasX"; //darle un identificador
                LabelTotalNumPersonasX.Text = "0";
                Columna.Controls.Add(LabelTotalNumPersonasX);//agregar el control label a la columna       
                Renglon.Controls.Add(Columna);


                //********** renglon3  [Y]
                Renglon = new TableRow();
                this.TableTotalResultados.Rows.Add(Renglon);

                //columna1: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 3;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                //Columna.Width = 400;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                // Columna.Text = "Áreas de Uso Común y Áreas de Circulación por 0.44 (m2)";
                Columna.Text = "Áreas de Uso Común y Áreas de Circulación por " + strValorParametroPonderacionFactorZ + " (m2)";
                Renglon.Controls.Add(Columna);


                //columna2: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 200;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "Y";
                Renglon.Controls.Add(Columna);


                //columna3: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Width = 270;
                //ctrl
                Label LabelTotalY = new Label();
                LabelTotalY.ID = "LabelTotalY"; //darle un identificador
                LabelTotalY.Text = "0.00";
                Columna.Controls.Add(LabelTotalY);//agregar el control label a la columna       
                Renglon.Controls.Add(Columna);

                //columna4: de renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Renglon.Controls.Add(Columna);

                //********** renglon4 [Z]
                Renglon = new TableRow();

                this.TableTotalResultados.Rows.Add(Renglon);

                //columna1: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 3;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                //Columna.Width = 400;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "Áreas Complementarias (m2)";
                Renglon.Controls.Add(Columna);


                //columna2: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 200;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "Z";
                Renglon.Controls.Add(Columna);


                //columna3: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Width = 270;
                //ctrl
                Label LabelTotalZ = new Label();
                LabelTotalZ.ID = "LabelTotalZ"; //darle un identificador
                LabelTotalZ.Text = "0.00";
                Columna.Controls.Add(LabelTotalZ);//agregar el control label a la columna         
                Renglon.Controls.Add(Columna);

                //columna4: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                //Columna.Width = 270;
                //ctrl
                Label LabelTotalNumPersonasZ = new Label();
                LabelTotalNumPersonasZ.ID = "LabelTotalNumPersonasZ"; //darle un identificador
                LabelTotalNumPersonasZ.Text = "0";
                Columna.Controls.Add(LabelTotalNumPersonasZ);//agregar el control label a la columna         
                Renglon.Controls.Add(Columna);


                //********** renglon5 [SMOI]
                Renglon = new TableRow();
                this.TableTotalResultados.Rows.Add(Renglon);

                //columna1: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 3;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                //Columna.Width = 400;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "Superficie Máxima a Ocupar por la Institución (m2)";
                Renglon.Controls.Add(Columna);


                //columna2: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 200;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Text = "SMOI";
                Renglon.Controls.Add(Columna);


                //columna3: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.Width = 270;
                //ctrl
                Label LabelTotalSMOI = new Label();
                LabelTotalSMOI.Font.Bold = true;
                LabelTotalSMOI.Font.Underline = true;
                LabelTotalSMOI.ID = "LabelTotalSMOI"; //darle un identificador
                LabelTotalSMOI.Text = "0.00";
                Columna.Controls.Add(LabelTotalSMOI);//agregar el control label a la columna          
                Renglon.Controls.Add(Columna);

                //columna4: de renglon4
                Columna = new TableCell();
                Columna.ColumnSpan = 1;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Renglon.Controls.Add(Columna);
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al crear la tabla de resultados. Contacta al área de sistemas.";
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

        private void CrearTablaPiePagina()
        {
            try
            {
                TableRow Renglon;
                TableCell Columna;

                //********** renglon1-Encabezado
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //columna de renglon X
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.Font.Bold = true;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Center;
                Columna.BackColor = System.Drawing.Color.LightGray;
                Columna.BorderColor = System.Drawing.Color.Gray;
                Columna.Text = "Descripción de Operaciones";
                Renglon.Controls.Add(Columna);


                //********** renglon2
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //agregar columnas al renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = " I. Para los grupos jerárquicos del 1 al  7, la superficie unitaria máxima por servidor (A) incluye los siguientes espacios: área de trabajo, mesa de juntas, zona de espera y baño privado.";
                Renglon.Controls.Add(Columna);

                //********** renglon3
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //agregar columnas al renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "II. El producto de la columna C es el resultado de multiplicar las columnas A y B.";
                Renglon.Controls.Add(Columna);

                //********** renglon4
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //agregar columnas al renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "III. Las áreas de uso común y de circulación (Y) incluyen vestíbulos, pasillos, baños comunes, cuartos de máquinas, cuartos de aseo, bodegas, cubos de elevadores, escaleras, entre otros.";
                Renglon.Controls.Add(Columna);

                //********** renglon5
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //agregar columnas al renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "IV. El porcentaje de espacios complementarios no podrá exceder en un 50% al valor X, en caso de ser así será meritorio de un análisis particular por parte del INDAABIN.";
                Renglon.Controls.Add(Columna);


                //********** renglon6
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //agregar columnas al renglon3
                Columna = new TableCell();
                Columna.ColumnSpan = 5;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "V. Las áreas complementarias (Z) constituyen aquellos espacios adicionales requeridos para el funcionamiento de la entidad/dependencia, tales como aulas de capacitación, comedor para servidores públicos, auditorio, áreas para archivo muerto y salones de usos múltiples. Para su cálculo se deberá estimar el número de usuarios y multiplicarlos por el factor de m2 por usuario con base en la siguiente tabla:";
                Renglon.Controls.Add(Columna);

                //********** renglon7
                Renglon = new TableRow();
                this.TablePiePagina.Rows.Add(Renglon);

                //agregar columnas al renglon3
                Columna = new TableCell();
                Columna.Font.Bold = true;
                Columna.ColumnSpan = 5;
                Columna.BorderStyle = BorderStyle.Solid;
                Columna.BorderWidth = 1;
                Columna.Width = 800;
                Columna.HorizontalAlign = HorizontalAlign.Left;
                Columna.Text = "Aquellos espacios no considerados en la tabla, se tendrán que justificar por la dependencia solicitante ante el INDAABIN.";
                Renglon.Controls.Add(Columna);
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al crear el pie de la tabla de resultados. Contacta al área de sistemas.";
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
        #endregion

        //metodo que se liga al cargar la tabla de SMOI-FactorX al textbox de entrada de Factores-X al cambio de texto
        private void MultiplicarValoresSMOI_FactorX(object sender, EventArgs e)
        {
            //tomar el valor al objeto
            TextBox TextBoxCptoSMOIFactorX_ColB = (TextBox)sender;

            //si cadena vacia, entonces poner cero
            if (TextBoxCptoSMOIFactorX_ColB.Text == "")
                TextBoxCptoSMOIFactorX_ColB.Text = "0";

            //al cambiar el valor de entrada
            this.ObtenerResultadoAxB_FactorX(TextBoxCptoSMOIFactorX_ColB.ID);
        }

        //multiplicar las columnas A y B para poner en columna C, de la tabla: TableSMOI-Factor-X
        private void ObtenerResultadoAxB_FactorX(string IdControlFactorB)
        {
            //obtener el valor del control origen disparador de evento
            string[] words = IdControlFactorB.Split('-');
            String strFactor1 = this.EncontrarValorIdCtrlRowCellCtrl(IdControlFactorB, this.TableSMOI);
            String strFactor2 = this.EncontrarValorIdCtrlRowCellCtrl(words[0].ToString() + "-LabelA", this.TableSMOI);
            String strFactorMaximo = this.EncontrarValorIdCtrlRowCellCtrl(words[0].ToString() + "-LabelMaxB", this.TableSMOI);
            String IdControlError = words[0].ToString() + "-LabelErrorB";

            if (Util.IsEnteroNatural(strFactor1))
            {
                if (strFactor2 != "Valor no Encontrado")
                {
                    if (ValidaValorMaximoIdCtrlRowCellCtrl(this.TableSMOI, IdControlFactorB))
                    {
                        decimal Resultado = Convert.ToDecimal(strFactor1) * Convert.ToDecimal(strFactor2);
                        //coloacar el valor de resultado en el control LabelC
                        if (this.AsignarValorIdCtrlRowCellCtrl(this.TableSMOI, words[0].ToString() + "-LabelC", Resultado.ToString()))
                        {
                            this.CalculaTotales_SMOI();
                        }
                        else
                        {
                            Msj = "No fue posible encontrar el control: " + words[0].ToString() + "-LabelC para colocar el resultado de AxB";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);
                        }
                    }
                }
                else
                {
                    Msj = "No fue posible encontrar el valor del factor en la columna A para el Factor-X";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                }
            }
        }

        //metodo que se liga al cargar la tabla de SMOI-FactorZ al textbox de entrada de Factores-X al cambio de texto
        private void MultiplicarValoresSMOI_FactorZ(object sender, EventArgs e)
        {
            //encontrar valor
            TextBox TextBoxCptoSMOIFactorZ_ColB = (TextBox)sender;

            //si cadena vacia, entonces poner cero
            if (TextBoxCptoSMOIFactorZ_ColB.Text == "")
                TextBoxCptoSMOIFactorZ_ColB.Text = "0";

            //al cambiar el valor de entrada
            this.ObtenerResultadoAxB_FactorZ(TextBoxCptoSMOIFactorZ_ColB.ID); //"Id= 1.00-TextBox"

        }

        //multiplicar las columnas A y B para poner en columna C, de la tabla: TableSMOI-Factor-Z
        private void ObtenerResultadoAxB_FactorZ(string IdControlFactorB)
        {
            //obtener el valor del control origen disparador de evento
            string[] words = IdControlFactorB.Split('-');
            String strFactor1 = this.EncontrarValorIdCtrlRowCellCtrl(IdControlFactorB, this.TableSMOI);
            String strFactor2 = this.EncontrarValorIdCtrlRowCellCtrl(words[0].ToString() + "-LabelA-FactorZ", this.TableSMOI);

            if (Util.IsEnteroNatural(strFactor1))
            {
                if (strFactor2 != "Valor no Encontrado")
                {
                    decimal Resultado = Convert.ToDecimal(strFactor1) * Convert.ToDecimal(strFactor2);
                    //coloacar el valor de resultado en el control LabelC
                    if (this.AsignarValorIdCtrlRowCellCtrl(this.TableSMOI, words[0].ToString() + "-LabelC-FactorZ", Resultado.ToString()))
                    {
                        this.CalculaTotales_SMOI();
                    }
                    else
                    {
                        Msj = "No fue posible encontrar el control: " + words[0].ToString() + "-LabelC para colocar el resultado de AxB del Factor-Z";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                    }
                }
                else
                {
                    Msj = "No fue posible encontrar el valor del factor en la columna A correspondiente al Factor-Z";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    MostrarMensajeJavaScript(Msj);
                }
            }
        }

        private void CalculaTotales_SMOI()
        {
            string strSufixLA_X = "-LabelA";
            string strSufixTB_X = "-TextBoxB";
            string strSufixLC_X = "-LabelC";

            string strSufixLA_Z = "-LabelA-FactorZ";
            string strSufixTB_Z = "-TextBoxB-FactorZ";
            string strSufixLC_Z = "-LabelC-FactorZ";

            decimal TotalCValue_X = 0;
            decimal TotalPerson_X = 0;

            decimal TotalCValue_Y = 0;

            decimal TotalCValue_Z = 0;
            decimal TotalPerson_Z = 0;


            foreach (TableRow row in this.TableSMOI.Rows)
            {
                foreach (Control cell in row.Controls)
                {
                    foreach (Control innerItem in cell.Controls)
                    {
                        Control item;

                        if (innerItem.GetType().ToString() == "AjaxControlToolkit.FilteredTextBoxExtender")
                            break;

                        if (innerItem.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                            item = innerItem.Controls[0];
                        else
                            item = innerItem;

                        // Se recorre la tabla buscando valores de Factor X
                        if (item.ID.Contains(strSufixLA_X) && !item.ID.Contains(strSufixLA_Z))
                        {
                            string[] words = item.ID.Split('-');
                            Label ctrlA = (Label)row.FindControl(words[0] + strSufixLA_X);
                            TextBox ctrlB = (TextBox)row.FindControl(words[0] + strSufixTB_X);
                            Label ctrlC = (Label)row.FindControl(words[0] + strSufixLC_X);

                            decimal currentCValue = 0;
                            currentCValue = Convert.ToDecimal(ctrlA.Text) * Convert.ToDecimal(ctrlB.Text);

                            TotalPerson_X += Convert.ToDecimal(ctrlB.Text);
                            TotalCValue_X += currentCValue;
                            break;
                        }

                        // Se recorre la tabla buscando valores de Factor X
                        if (item.ID.Contains(strSufixLA_Z))
                        {
                            string[] words = item.ID.Split('-');
                            Label ctrlA = (Label)row.FindControl(words[0] + strSufixLA_Z);
                            TextBox ctrlB = (TextBox)row.FindControl(words[0] + strSufixTB_Z);
                            Label ctrlC = (Label)row.FindControl(words[0] + strSufixLC_Z);

                            decimal currentCValue = 0;
                            currentCValue = Convert.ToDecimal(ctrlA.Text) * Convert.ToDecimal(ctrlB.Text);

                            TotalPerson_Z += Convert.ToDecimal(ctrlB.Text);
                            TotalCValue_Z += currentCValue;
                            break;
                        }
                    }
                }
            }
            string strValorFactorY = new NG_Catalogos().ObtenerValorCatParametro("SMOI m2 Factor-Y");
            TotalCValue_Y = Convert.ToDecimal(strValorFactorY) * (TotalCValue_X);

            this.AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalX", String.Format("{0:0.00}", TotalCValue_X));
            this.AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalY", String.Format("{0:0.00}", TotalCValue_Y));
            this.AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalZ", String.Format("{0:0.00}", TotalCValue_Z));
            this.AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalSMOI", String.Format("{0:0.00}", (TotalCValue_X + TotalCValue_Y + TotalCValue_Z)));
            this.AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalNumPersonasX", String.Format("{0:0.00}", TotalPerson_X));
            this.AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalNumPersonasZ", String.Format("{0:0.00}", TotalPerson_Z));
        }

        //Proposito: Identificar el control solicitado por su Id en un determinado renglon-celda-control y "Asignar" el valor especificado en el parametro de entrada
        private Boolean AsignarValorIdCtrlRowCellCtrl(Table pTable, String IdCtrl, String Valor, Boolean? pEnabled = null)
        {
            foreach (TableRow row in pTable.Rows)
            {
                Control ctrl = row.FindControl(IdCtrl);
                if (ctrl != null)
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)ctrl).Text = Valor;
                            return true;
                        case "System.Web.UI.WebControls.Label":
                            ((Label)ctrl).Text = Valor;
                            return true;
                        case "System.Web.UI.WebControls.DropDownList":
                            if (pEnabled != null)
                                ((DropDownList)ctrl).Enabled = pEnabled.Value;
                            else
                                ((DropDownList)ctrl).Enabled = true;

                            if (Valor == "No")
                            {
                                ((DropDownList)ctrl).SelectedIndex = 1; //si
                                return true;
                            }
                            else
                            {
                                if (Valor == "Si")
                                {
                                    ((DropDownList)ctrl).SelectedIndex = 2; //no
                                    return true;
                                }
                                else
                                {
                                    ((DropDownList)ctrl).SelectedIndex = 0; //"--"
                                    return true;
                                }
                            }
                    }
                }
            }
            return false;
        }

        private Boolean ValidaValorMaximoIdCtrlRowCellCtrl(Table pTable, String IdControlFactorB)
        {
            string[] words = IdControlFactorB.Split('-');
            String strFactor1 = this.EncontrarValorIdCtrlRowCellCtrl(IdControlFactorB, this.TableSMOI);
            String strFactorMaximo = this.EncontrarValorIdCtrlRowCellCtrl(words[0].ToString() + "-LabelMaxB", this.TableSMOI);
            String IdCtrlError = words[0].ToString() + "-LabelErrorB";

            foreach (TableRow row in pTable.Rows)
            {
                Control ctrl = row.FindControl(IdControlFactorB);
                Control ctrlError = row.FindControl(IdCtrlError);

                if (ctrl != null && ctrlError != null)
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            decimal oMaxValue = System.Convert.ToDecimal(strFactorMaximo);
                            decimal oCurrentValue = System.Convert.ToDecimal(strFactor1);
                            //if (oMaxValue > 0 && oCurrentValue > oMaxValue)

                            //RCA 24/05/2018
                            //modificacion a las validaciones de la tabla SMOI
                            if (oMaxValue == 0 )
                            {
                                HtmlGenericControl myDiv = (HtmlGenericControl)ctrl.Parent;
                                myDiv.Attributes.Clear();
                                myDiv.Attributes.Add("class", "form-group has-error");
                                //Msj = "Debes proporcionar un número menor o igual a " + oMaxValue.ToString();

                                //RCA 24/05/2018
                                //mmensaje de validacion por si deja un cero
                                Msj = "Debes proporcionar un número diferente de cero" + oMaxValue.ToString();
                                ((Label)ctrlError).Text = Msj;
                                ((Label)ctrlError).Visible = true;
                                ((TextBox)ctrl).Focus();
                                return false;
                            }
                            else
                            {
                                HtmlGenericControl myDiv = (HtmlGenericControl)ctrl.Parent;
                                myDiv.Attributes.Clear();
                                myDiv.Attributes.Add("class", "form-group");
                                ((Label)ctrlError).Text = "";
                                ((Label)ctrlError).Visible = false;
                            }
                            break;
                    }
                }
            }
            return true;
        }

        //Proposito: Identificar el control solicitado por su Id en un determinado renglon-celda-control y "Devolver" el valor especificado en el parametro de entrada
        private string EncontrarValorIdCtrlRowCellCtrl(String IdCtrl, Table pTable)
        {
            foreach (TableRow row in pTable.Rows)
            {
                Control ctrl = row.FindControl(IdCtrl);
                if (ctrl != null)
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.Label":
                            return ((Label)ctrl).Text;
                        case "System.Web.UI.WebControls.DropDownList":
                            return ((DropDownList)ctrl).SelectedItem.Text;
                        case "System.Web.UI.WebControls.TextBox":
                            return ((TextBox)ctrl).Text;
                    }
                }
            }
            return "Valor no Encontrado";
        }

        private bool ValidarExistaValoresEnTextBox(Table pTable)
        {
            Boolean Ok = true;
            //Page.MaintainScrollPositionOnPostBack = false; //no mentener la posicion del scroll del navegador, para que se posicione en el focus del ctrl que no pasa la validacio

            if (pTable.Rows.Count == 0)
                Ok = false;

            //iterar por cada renglon de la tabla = concepto
            foreach (TableRow row in pTable.Rows)
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
                                case "System.Web.UI.WebControls.TextBox":

                                    if (((TextBox)ctrl).Text.Length > 0)
                                    {

                                        //validacion de enteros
                                        if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                        {
                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                            myDiv.Attributes.Clear();
                                            myDiv.Attributes.Add("class", "form-group has-error");
                                            Msj = "Debes proporcionar un número igual o mayor a cero [" + row.ID.ToString() + "].";
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                            this.MostrarMensajeJavaScript(Msj);
                                            ((TextBox)ctrl).Focus();
                                            return false; //romper todos los ciclos
                                        }

                                        //validacion de no negativos
                                        if ((Convert.ToInt32(((TextBox)ctrl).Text)) < 0)
                                        {
                                            HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                            myDiv.Attributes.Clear();
                                            myDiv.Attributes.Add("class", "form-group has-error");
                                            Msj = "Debes proporcionar un número igual o mayor a cero [" + row.ID.ToString() + "].";
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                            this.MostrarMensajeJavaScript(Msj);
                                            ((TextBox)ctrl).Focus();
                                            return false; //romper todos los ciclos
                                        }

                                        if (!ValidaValorMaximoIdCtrlRowCellCtrl(pTable, ctrl.ID))
                                        {
                                            Msj = "Se ha exedido el valor permitido para uno o mas campos, verifíque.";
                                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                            this.MostrarMensajeJavaScript(Msj);
                                            ((TextBox)ctrl).Focus();
                                            return false;
                                        }

                                    }
                                    else
                                    {
                                        HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
                                        myDiv.Attributes.Clear();
                                        myDiv.Attributes.Add("class", "form-group has-error");
                                        Msj = "Debes proporcionar un valor numérico o ponga cero en:  [" + row.ID.ToString() + "].";
                                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                                        this.MostrarMensajeJavaScript(Msj);
                                        ((TextBox)ctrl).Focus();
                                        return false; //romper todos los ciclos
                                    }
                                    break;

                            }//switch
                        }//for control
                    }//for cell
                }//if es numeric
            }//foreach principal
            //Page.MaintainScrollPositionOnPostBack = true; //mentener la posicion del scroll del navegador
            return Ok;
        }

        private Boolean InsertRespuestaCptosEmisionOpinion()
        {
            Boolean Ok = false;

            //validar si existe la lista de objetos que almacenan los espacios de respuestas por concepto.
            if (Session[this.lblNombreSession.Value] != null)
            {
                string strIdControl;

                //lista de respuestas, pendientes de valor, recuperar cptos de la session, guardada en la presentacion de los cptos en la vista
                List<ValorRespuestaConcepto> ListValorRespuestaConcepto = (List<ValorRespuestaConcepto>)Session[this.lblNombreSession.Value];


                //Recolectar datos de respuesta  del usuario para cada cpto de acuerdo a su Id
                //Para cada respuesta colocarlo en el objeto de negocio
                //iterar por cada renglon de la tabla = concepto
                foreach (TableRow row in this.TableSMOI.Rows)
                {

                    //if (Util.IsNumeric(row.ID))
                    //{
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


                            //Convertir el [row.ID] que itera porque para los conceptos de Factor-Z vienen con el numero de cpto y se concateno el texto -FactorZ para hacer unico el Id del control, y aqui lo separamos
                            // IdControl.ID = "12-FactorZ"
                            string[] words = row.ID.Split('-');
                            //  el resultado en el arreglo: 
                            strIdControl = words[0].ToString();

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
                            switch (ctrl.GetType().ToString())
                            {

                                //nota: solo recoger el valor de controles de tipo TextBox
                                case "System.Web.UI.WebControls.TextBox":
                                    foreach (ValorRespuestaConcepto objResp in ListValorRespuestaConcepto)
                                    {

                                        if (objResp.NumOrdenVisual == strIdControl)
                                        //if (objResp.NumOrdenVisual == row.ID)
                                        {

                                            objResp.ValorResp = Convert.ToInt32((((TextBox)ctrl).Text));
                                            break;
                                        }
                                    }
                                    break;
                            }//switch
                        }//foreach
                    } //foreach
                }//foreach

                //llamar el objeto de capa de negocio para pasar los datos a la BD

                //Obtener valores: IdInstitucion y IdUsuarioRegistro del contexto del SSO de la cuenta del autenticado
                int IdInstitucionUsr = ((SSO)Session["Contexto"]).IdInstitucion.Value; //sso
                int IdUsuarioRegistro = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso
                String strCargoUsr = ((SSO)Session["Contexto"]).Cargo; //SSO

                

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
                int FolioSMOI = 0; //parametro de salida

                Control LabelInstitucion = this.ctrlUsuarioInfo.FindControl("LabelInstitucion");
                string InstitucionUsr = ((Label)LabelInstitucion).Text;
                String TotalM2SMOI = this.EncontrarValorIdCtrlRowCellCtrl("LabelTotalSMOI", this.TableTotalResultados);

                //crear sello digital
                string CadenaOriginal = "||Invocante:[" + InstitucionUsr + "] || SMOI-M2:[" + TotalM2SMOI + "]||Fecha:[" + DateTime.Today.ToLongDateString() + "]||" + Guid.NewGuid().ToString();
                //generar el sello diigital, con la llave de ciframiento
                string SelloDigital = UtilContratosArrto.Encrypt(CadenaOriginal, true, "SMOI");

                

                try
                {
                    if (System.Convert.ToDecimal(TotalM2SMOI) <= 0)
                    {
                        Msj = "No fue posible contabilizar las respuestas para los conceptos de la Tabla-SMOI, por favor vuelve a intentar el envío ó reporta al área de Sistemas.";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                        MostrarMensajeJavaScript(Msj);
                        DataTableRespuestaCptoList = null;
                    }
                    else
                    {
                        FolioSMOI = new NGConceptoRespValor()
                            .Insert_SMOI(
                            IdInstitucionUsr,
                            strCargoUsr,
                            IdUsuarioRegistro,
                            CadenaOriginal,
                            SelloDigital,
                            DataTableRespuestaCptoList,
                            strConnectionString
                            ,string.Empty);

                        if (FolioSMOI > 0)
                        {
                            this.FolioGeneradoSmoi = FolioSMOI.ToString();
                            DataTableRespuestaCptoList = null;

                            //RCA 10/08/2018
                            string UrlAbrirQRSMOI = "AcuseSMOI.aspx?IdFolio=" + this.FolioGeneradoSmoi + "&isInsert=true";
                            Control LabelNombreSMOI = this.ctrlUsuarioInfo.FindControl("LabelUsr");
                            string NombreUsuarioSMOI = ((Label)LabelNombreSMOI).Text;


                            //RCA 10/08/2018
                            //generamos el QR y se guarda el tipo de guardado 1 es para emisiones SMOI
                            string QR = UtilContratosArrto.GenerarCodigoQR(FolioGeneradoSmoi, 1, NombreUsuarioSMOI, UrlAbrirQRSMOI);

                            //obtenemos el idaplicacion del smoi
                            int IdAplicacionSMOI = new NGConceptoRespValor().ObtenerIdSMOI(Convert.ToInt32(FolioGeneradoSmoi),7);

                            if(!string.IsNullOrEmpty(QR) && IdAplicacionSMOI > 0)
                            {
                                //actualizamos el campo de la QR
                                Ok = new NGConceptoRespValor().ActualizarQRSMOI(QR,IdAplicacionSMOI);
                            }

                            Ok = true;
                        }
                        else
                        {
                            Msj = "No fue posible registrar las respuestas para los conceptos de la Tabla-SMOI, por favor vuelve a intentar el envío ó reporte al área de Sistemas.";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                            MostrarMensajeJavaScript(Msj);
                            DataTableRespuestaCptoList = null;
                        }
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Msj = "Ha ocurrido un error al procesar la tabla SMOI. Contacta al área de sistemas.";
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
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al procesar la tabla SMOI. Contacta al área de sistemas.";
                    this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
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

            }//if de validacion de session con list de respuestas
            else
            {
                Msj = "No existe el objeto para recolectar las respuestas, vuelve a intentar o reporta a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;//etiqueta del final, redundancia de msj al usuario
                MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {
            string strTotalFactorSMOI = this.EncontrarValorIdCtrlRowCellCtrl("LabelTotalSMOI", this.TableTotalResultados);
            string strTotalFactorX = this.EncontrarValorIdCtrlRowCellCtrl("LabelTotalX", this.TableTotalResultados);
            if (strTotalFactorSMOI != "Valor no Encontrado")
            {
                if (Convert.ToDecimal(strTotalFactorSMOI) < 1)
                {
                    Msj = "Debes propocionar por lo menos un valor para calcular el total de m2 de SMOI y emitir un acuse.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    MostrarMensajeJavaScript(Msj);
                    return;
                }
            }

            if (strTotalFactorX != "Valor no Encontrado")
            {
                if (Convert.ToDecimal(strTotalFactorX) < 1)
                {
                    this.TableSMOI.Focus();
                    Msj = "Debes propocionar por lo menos un valor del Factor X para calcular el total de m2 de SMOI y emitir un acuse.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    MostrarMensajeJavaScript(Msj);
                    return;
                }
            }

            if (ValidarExistaValoresEnTextBox(this.TableSMOI))
            {
                if (this.InsertRespuestaCptosEmisionOpinion())
                {
                    Session[this.lblNombreSession.Value] = null;
                    Session["ListCptosSMOI_FactorX"] = null;
                    Session["ListCptosSMOI_FactorZ"] = null;

                    string mensaje = "La tabla SMOI ha sido registrada con éxito.";
                    this.LabelInfo.Text += "<div class='alert alert-success'><strong> ¡Felicidades! </strong></br>" + mensaje + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.pnlTablaSmoi.Enabled = false;
                    this.ButtonEnviar.Enabled = false;
                    this.ButtonCancelar.Text = "Regresar";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAlerta", "alert(\"" + mensaje + "\");", true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAcuse", "window.open('AcuseSMOI.aspx?IdFolio=" + this.FolioGeneradoSmoi + "&isInsert=true', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = yes, resizable = yes, width = 1024, height = 650', 'true');", true);
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "bloqueaPantalla", "", true);
                }
            }
        }

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            //desocupar variables
            Session[this.lblNombreSession.Value] = null;
            Session["ListCptosSMOI_FactorX"] = null;
            Session["ListCptosSMOI_FactorZ"] = null;
            //redireccionar
            Response.Redirect("BusqTablaSMOIEmitidas.aspx");
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        ////Asignar total acumulado de m2 a las etiquetas totales, NombreFactor = "Factor-X", "Factor-Y", "Factor-Z"
        //private bool AsignarTotalesM2XFactor_SMOI(string NombreFactor, decimal pValorIngresado)
        //{
        //    Boolean Ok = false;
        //    string NombreLabelTotal = "";

        //    switch (NombreFactor)
        //    {
        //        case "Factor-X": //nota: al actualizar el valor del factorX, se actuliza el calculo del factorY que es dependiente de factor-X
        //            NombreLabelTotal = "LabelTotalX";
        //            break;

        //        case "Factor-Z":
        //            NombreLabelTotal = "LabelTotalZ";
        //            break;
        //    }

        //    if (NombreLabelTotal.Length > 0)
        //    {
        //        String strTotalFactor = this.EncontrarValorIdCtrlRowCellCtrl(NombreLabelTotal, this.TableTotalResultados);

        //        if (strTotalFactor != "Valor no Encontrado")
        //        {
        //            Decimal TotalFactor = Convert.ToDecimal(strTotalFactor) + pValorIngresado;


        //            //acumular en resultado total del factor: X ó Z
        //            if (AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, NombreLabelTotal, TotalFactor.ToString()))
        //            {

        //                //encontrar el valor del total que va y asignar total de SMOI= Suma de X+Y+Z
        //                String strTotalFactorSMOI = this.EncontrarValorIdCtrlRowCellCtrl("LabelTotalSMOI", this.TableTotalResultados);
        //                decimal TotalFactorSMOI;

        //                if (strTotalFactorSMOI != "Valor no Encontrado")
        //                {
        //                    TotalFactorSMOI = Convert.ToDecimal(strTotalFactorSMOI);

        //                    //si el factor que se actualiza es X, entonces caluclar Y y acumular al total SMOI
        //                    if (NombreLabelTotal == "LabelTotalX")
        //                    {
        //                        //calcular factor-Y = (Factor-X * ParametroValorFactorY)
        //                        decimal TotalFactorY = TotalFactor * Convert.ToDecimal(strValorParametroPonderacionFactorZ);
        //                        TotalFactorSMOI = Convert.ToDecimal(strTotalFactor) + pValorIngresado + TotalFactorY;

        //                        //colocar el valor de Factor-Y en la etiqueta de la vista
        //                        AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalY", String.Format("{0:N}", TotalFactorY));
        //                    }
        //                    else
        //                        TotalFactorSMOI += pValorIngresado;

        //                    //actualizar valor total en etiqueta de totales: LabelTotalSMOI                  
        //                    if (AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, "LabelTotalSMOI", String.Format("{0:N}", TotalFactorSMOI)))
        //                    {

        //                        Ok = true;
        //                    }

        //                    else
        //                    {
        //                        Msj = "No fue posible acumular el Total de SMOI=(X+Y+Z)";
        //                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
        //                        MostrarMensajeJavaScript(Msj);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                Msj = "No fue posible acumular el Total del :" + NombreFactor;
        //                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
        //                MostrarMensajeJavaScript(Msj);
        //            }

        //        }
        //        else
        //        {
        //            Msj = "No fue posible encontrar el control: TotalFactorX para acumular el total de valores correspondientes a la tabla de Factor-X";
        //            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
        //            MostrarMensajeJavaScript(Msj);
        //        }


        //    }//if


        //    return Ok;
        //}


        //private bool AsignarTotalesNumPersonasXFactor_SMOI(string NombreLabelTotalNumPersonasFactor, Int32 pResultado)
        //{
        //    Boolean Ok = false;
        //    String strTotalFactorPersonas = this.EncontrarValorIdCtrlRowCellCtrl(NombreLabelTotalNumPersonasFactor, this.TableTotalResultados);

        //    if (strTotalFactorPersonas.Length > 0)
        //    {

        //        int TotalNumPersonasFactorFactor = Convert.ToInt32(strTotalFactorPersonas) + pResultado;

        //        //acumular en resultado total del factor
        //        if (AsignarValorIdCtrlRowCellCtrl(this.TableTotalResultados, NombreLabelTotalNumPersonasFactor, TotalNumPersonasFactorFactor.ToString()))
        //            Ok = true;
        //        else
        //        {

        //            Msj = "No fue posible encontrar el control: TotalFactorX para acumular el total de valores correspondientes a la tabla de Factor-X";
        //            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
        //            MostrarMensajeJavaScript(Msj);
        //        }

        //    }//if

        //   

        ////Asigna el Foco a un control especificado, en la Tabla
        //private Boolean SetFocoIdCtrlRowCellCtrl(Table pTable, String IdCtrl)
        //{
        //    //iterar por cada renglon de la tabla = concepto
        //    foreach (TableRow row in pTable.Rows)
        //    {
        //        //iterar por cada columna/celda del renglon
        //        foreach (TableCell cell in row.Cells)
        //        {
        //            //iterar por cada control dentro de la celda y validar contenido de acuerdo al tipo de ctrl.
        //            foreach (Control ctrlInner in cell.Controls)
        //            {
        //                Control ctrl;
        //                if (ctrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
        //                {
        //                    HtmlGenericControl myDiv = (HtmlGenericControl)ctrlInner;
        //                    myDiv.Attributes.Clear();
        //                    myDiv.Attributes.Add("class", "form-group");
        //                    ctrl = ctrlInner.Controls[0];
        //                }
        //                else
        //                    ctrl = ctrlInner;

        //                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox, DropDownList o Label
        //                switch (ctrl.GetType().ToString())
        //                {

        //                    case "System.Web.UI.WebControls.TextBox":
        //                        if (((TextBox)ctrl).ID == IdCtrl)
        //                        {
        //                            ((TextBox)ctrl).Focus();
        //                            return true; //romper todos los ciclos, porque ya se encontro el ctrl.
        //                        }

        //                        break;

        //                    case "System.Web.UI.WebControls.Label":
        //                        if (((Label)ctrl).ID == IdCtrl)
        //                        {
        //                            ((Label)ctrl).Focus();
        //                            return true; //romper todos los ciclos, porque ya se encontro el ctrl.
        //                        }

        //                        break;


        //                    case "System.Web.UI.WebControls.DropDownList":

        //                        if (((DropDownList)ctrl).ID == IdCtrl)
        //                        {
        //                            ((DropDownList)ctrl).Focus();

        //                        }

        //                        break;
        //                }
        //            } //foreach
        //        }//foreach
        //    }//foreach

        //    return false; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        //}
    }
}