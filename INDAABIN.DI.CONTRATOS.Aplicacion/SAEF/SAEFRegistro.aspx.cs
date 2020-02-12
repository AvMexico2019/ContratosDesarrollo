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
using System.Data.SqlClient;

//
using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;
using System.Web.UI.HtmlControls;



namespace INDAABIN.DI.CONTRATOS.Aplicacion.SAEF
{
    public partial class SAEFRegistro : System.Web.UI.Page
    {
        private string Msj = string.Empty;
        private List<ConceptoSAEF> ListConcSAEF = null;
        private string FolioEmisionOpinion = string.Empty;
        private int? IdAplicacionConcepto = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!IsPostBack)
            //{
                if (Session["Contexto"] == null)
                {
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));
                }

                String NombreRol = UtilContratosArrto.ObtenerNombreRolUsrApp(((SSO)Session["Contexto"]).LRol);

                //obtenemos el folio de emision de opinion+
                FolioEmisionOpinion = Request.QueryString["FolioEmision"];

                //ponemos los primeros valores 
                this.ObtenerEncabezadoInicial(FolioEmisionOpinion);

                this.LimpiarSessionesGeneradas();

                //generamos la vista dinamicamente

                if (CrearTablaCptosSAEF())
                {
                    this.TableServicioInmueble.Visible = true;

                   
                }
            
            //}

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            List<ValorRespuestaSAEF> Lvalor = new List<ValorRespuestaSAEF>();
            NG_SAEF nSAEF = new NG_SAEF();            

            try
            {
                if (IdAplicacionConcepto != null)
                {
                    Lvalor = nSAEF.ObtenerValorRespuestaAplicacionConcepto(IdAplicacionConcepto.Value);

                    if (Lvalor.Count > 0)
                    {
                        foreach (ValorRespuestaSAEF valor in Lvalor)
                        {
                            Control fila = pnlSAEF.FindControl(valor.ConceptoAccesibilidad.ToString());

                            if (fila != null)
                            {
                                string IdAplica = "checkctrlAplica";
                                string IdExiste = "checkctrlExiste";
                                string IdAcceso = "TextBox";
                                string IdRequiere = "checkctrlSeRequiere";
                                string IdCumple = "CheckCtrlCumple";
                                string IdObservaciones = "TextBoxObservaciones";

                                if (Convert.ToInt32(fila.ID) > 32 && Convert.ToInt32(fila.ID) <= 40)
                                {
                                    IdAplica = IdAplica + "1" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "1" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "AccesoInmueble" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "1" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "1" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "1" + valor.ConceptoAccesibilidad;
                                }

                                else if (Convert.ToInt32(fila.ID) > 55 && Convert.ToInt32(fila.ID) <= 68)
                                {
                                    IdAplica = IdAplica + "2" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "2" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "Vestibulo" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "2" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "2" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "2" + valor.ConceptoAccesibilidad;
                                }

                                else if (Convert.ToInt32(fila.ID) > 79 && Convert.ToInt32(fila.ID) <= 88)
                                {
                                    IdAplica = IdAplica + "3" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "3" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "Circulaciones" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "3" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "3" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "3" + valor.ConceptoAccesibilidad;
                                }

                                else if (Convert.ToInt32(fila.ID) > 99 && Convert.ToInt32(fila.ID) <= 108)
                                {
                                    IdAplica = IdAplica + "4" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "4" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "Senalizacion" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "4" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "4" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "4" + valor.ConceptoAccesibilidad;
                                }

                                else if (Convert.ToInt32(fila.ID) > 118 && Convert.ToInt32(fila.ID) <= 126)
                                {
                                    IdAplica = IdAplica + "5" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "5" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "UsoEdificioServicio" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "5" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "5" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "5" + valor.ConceptoAccesibilidad;
                                }

                                else if (Convert.ToInt32(fila.ID) > 136 && Convert.ToInt32(fila.ID) <= 144)
                                {
                                    IdAplica = IdAplica + "6" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "6" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "SanitariosUsoExclusivo" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "6" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "6" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "6" + valor.ConceptoAccesibilidad;
                                }

                                else if (Convert.ToInt32(fila.ID) > 145 && Convert.ToInt32(fila.ID) <= 154)
                                {
                                    IdAplica = IdAplica + "7" + valor.ConceptoAccesibilidad;
                                    IdExiste = IdExiste + "7" + valor.ConceptoAccesibilidad;
                                    IdAcceso = IdAcceso + "RutaEvacuacionEmergente" + valor.ConceptoAccesibilidad;
                                    IdRequiere = IdRequiere + "7" + valor.ConceptoAccesibilidad;
                                    IdCumple = IdCumple + "7" + valor.ConceptoAccesibilidad;
                                    IdObservaciones = IdObservaciones + "7" + valor.ConceptoAccesibilidad;
                                }

                                Control ctrolAplica = pnlSAEF.FindControl(IdAplica);
                                Control ctrolExiste = pnlSAEF.FindControl(IdExiste);
                                Control ctrolCantidad = pnlSAEF.FindControl(IdAcceso);
                                Control ctrolRequiere = pnlSAEF.FindControl(IdRequiere);
                                Control ctrolCumple = pnlSAEF.FindControl(IdCumple);
                                Control ctrolObservaciones = pnlSAEF.FindControl(IdObservaciones);

                                if (ctrolAplica != null)
                                {
                                    CheckBox chkA = (CheckBox)ctrolAplica;
                                    chkA.Checked = valor.Aplica == null ? false : valor.Aplica.Value;
                                }

                                if (ctrolExiste != null)
                                {
                                    CheckBox chkE = (CheckBox)ctrolExiste;
                                    chkE.Checked = valor.Existe == null ? false : valor.Existe.Value;
                                }

                                if (ctrolCantidad != null)
                                {
                                    TextBox txtC = (TextBox)ctrolCantidad;
                                    txtC.Text = valor.Cantidad == null ? "" : valor.Cantidad.Value.ToString();
                                }

                                if (ctrolRequiere != null)
                                {
                                    CheckBox chkR = (CheckBox)ctrolRequiere;
                                    chkR.Checked = valor.SeRequiere == null ? false : valor.SeRequiere.Value;
                                }

                                if (ctrolCumple != null)
                                {
                                    CheckBox chkR = (CheckBox)ctrolCumple;
                                    chkR.Checked = valor.Cumple == null ? false : valor.Cumple.Value;
                                }

                                if (ctrolObservaciones != null)
                                {
                                    TextBox txtO = (TextBox)ctrolObservaciones;
                                    txtO.Text = valor.Observaciones;
                                    txtO.Enabled = valor.Observaciones.Length == 0 ? false : true;
                                }
                            }
                        }                                         
                    }
                }

                
            }

            catch (Exception ex)
            {
            
            }
        }

        private void ObtenerEncabezadoInicial(string FolioEmision)
        {

            try
            {
                //obtenemos de  DB lo que se necesita de la emision de opinion 
                EmisionOpinionSAEF ObjEmisonSAEF = null;

                ObjEmisonSAEF = new NG_SAEF().ObtenerEmisionSAEF(FolioEmision);

                IdAplicacionConcepto = ObjEmisonSAEF.IdAplicacionConcepto;

                //ponemos en cada etiqueta el valor que le corresponde
                this.txtFolioContratoSAEF.Text = ObjEmisonSAEF.FolioEmisionOpinion.ToString();

                this.txtUsuarioRegistroSAEF.Text = ObjEmisonSAEF.NombreUsuarioEmisionOpinion;

                this.txtFechaRegistroSAEF.Text = string.Format("{0:MM/dd/yyyy}", ObjEmisonSAEF.FechaRegistro);

            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la informacion inicial de la emisión de SAEF. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";


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

        //metodo para crear el formulario de saef dinamicamente
        private Boolean CrearTablaCptosSAEF()
        {
            bool ok = false;
            TableRow Renglon;
            TableCell Columna;

            try
            {
                ListConcSAEF = new NG_SAEF().ObtenerConceptosSAEF();

                if(ListConcSAEF.Count > 0 || ListConcSAEF != null)
                {
                    foreach(ConceptoSAEF ObjConceptoSAEF in ListConcSAEF)
                    {
                        //gregar renglon a al tabla
                        Renglon = new TableRow();

                        #region TableServicioInmueble

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableServicioInmueble***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad < 23)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableServicioInmueble.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 12)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxServicioInmueble" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 12)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxServicioInmueble12")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }

                           
                            Renglon.Controls.Add(Columna);


                        }

                        #endregion

                        #region TableAccesoInmueble

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableAccesoInmueble***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 31 && ObjConceptoSAEF.IdConcAccesibilidad < 41)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableAccesoInmueble.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica1 = new CheckBox();
                            CheckCtrlAplica1.ID = "checkctrlAplica1" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica1);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste1 = new CheckBox();
                            CheckCtrlExiste1.ID = "checkctrlExiste1" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste1);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxAccesoInmueble" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxAccesoInmueble32")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere1 = new CheckBox();
                            CheckCtrlSeRequiere1.ID = "checkctrlSeRequiere1" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere1);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta observaciones (antes cumple) *************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple1 = new CheckBox();
                            CheckCtrlCumple1.ID = "CheckCtrlCumple1" + Renglon.ID;//identificador de control
                            //CheckCtrlCumple1.AutoPostBack = true;//se pone para activar el evento de hacer visible o no el textbos
                            CheckCtrlCumple1.Attributes.Add("onclick","Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple1);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones1 = new TextBox();
                            TextBoxCtrlObservaciones1.ID = "TextBoxObservaciones1" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones1.Width = 100;
                            TextBoxCtrlObservaciones1.MaxLength = 150;
                            TextBoxCtrlObservaciones1.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones1);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 32)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                        }


                        #endregion

                        #region TableVestibulo

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableVestibulo***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 54 && ObjConceptoSAEF.IdConcAccesibilidad < 69)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableVestibulo.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica2 = new CheckBox();
                            CheckCtrlAplica2.ID = "checkctrlAplica2" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica2);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste2 = new CheckBox();
                            CheckCtrlExiste2.ID = "checkctrlExiste2" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste2);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxVestibulo" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxVestibulo55")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere2 = new CheckBox();
                            CheckCtrlSeRequiere2.ID = "checkctrlSeRequiere2" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere2);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta cumple *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple2 = new CheckBox();
                            CheckCtrlCumple2.ID = "CheckCtrlCumple2" + Renglon.ID;//identificador de control
                            CheckCtrlCumple2.Attributes.Add("onclick", "Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple2);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones2 = new TextBox();
                            TextBoxCtrlObservaciones2.ID = "TextBoxObservaciones2" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones2.Width = 100;
                            TextBoxCtrlObservaciones2.MaxLength = 150;
                            TextBoxCtrlObservaciones2.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones2);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 55)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                        }


                        #endregion


                        #region TableCirculaciones

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableCirculaciones***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 78 && ObjConceptoSAEF.IdConcAccesibilidad < 89)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableCirculaciones.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica3 = new CheckBox();
                            CheckCtrlAplica3.ID = "checkctrlAplica3" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica3);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste3 = new CheckBox();
                            CheckCtrlExiste3.ID = "checkctrlExiste3" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste3);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxCirculaciones" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxCirculaciones79")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere3 = new CheckBox();
                            CheckCtrlSeRequiere3.ID = "checkctrlSeRequiere3" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere3);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta cumple *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple3 = new CheckBox();
                            CheckCtrlCumple3.ID = "CheckCtrlCumple3" + Renglon.ID;//identificador de control
                            CheckCtrlCumple3.Attributes.Add("onclick", "Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple3);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones3 = new TextBox();
                            TextBoxCtrlObservaciones3.ID = "TextBoxObservaciones3" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones3.Width = 100;
                            TextBoxCtrlObservaciones3.MaxLength = 150;
                            TextBoxCtrlObservaciones3.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones3);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 79)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                        }

                        #endregion


                        #region TableSenalizacion

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableSenalizacion***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 98 && ObjConceptoSAEF.IdConcAccesibilidad < 109)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableSenalizacion.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica4 = new CheckBox();
                            CheckCtrlAplica4.ID = "checkctrlAplica4" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica4);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste4 = new CheckBox();
                            CheckCtrlExiste4.ID = "checkctrlExiste4" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste4);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxSenalizacion" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxSenalizacion99")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere4 = new CheckBox();
                            CheckCtrlSeRequiere4.ID = "checkctrlSeRequiere4" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere4);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta cumple *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple4 = new CheckBox();
                            CheckCtrlCumple4.ID = "CheckCtrlCumple4" + Renglon.ID;//identificador de control
                            CheckCtrlCumple4.Attributes.Add("onclick", "Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple4);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones4 = new TextBox();
                            TextBoxCtrlObservaciones4.ID = "TextBoxObservaciones4" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones4.Width = 100;
                            TextBoxCtrlObservaciones4.MaxLength = 150;
                            TextBoxCtrlObservaciones4.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones4);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 99)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                        }

                        #endregion

                        #region TableUsoEdificioServicio

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableUsoEdificioServicio***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 117 && ObjConceptoSAEF.IdConcAccesibilidad < 127)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableUsoEdificioServicio.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica5 = new CheckBox();
                            CheckCtrlAplica5.ID = "checkctrlAplica5" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica5);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste5 = new CheckBox();
                            CheckCtrlExiste5.ID = "checkctrlExiste5" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste5);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxUsoEdificioServicio" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxUsoEdificioServicio118")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere5 = new CheckBox();
                            CheckCtrlSeRequiere5.ID = "checkctrlSeRequiere5" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere5);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta cumple *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple5 = new CheckBox();
                            CheckCtrlCumple5.ID = "CheckCtrlCumple5" + Renglon.ID;//identificador de control
                            CheckCtrlCumple5.Attributes.Add("onclick", "Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple5);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones5 = new TextBox();
                            TextBoxCtrlObservaciones5.ID = "TextBoxObservaciones5" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones5.Width = 100;
                            TextBoxCtrlObservaciones5.MaxLength = 150;
                            TextBoxCtrlObservaciones5.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones5);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 118)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);
                        }

                        #endregion

                        #region TableSanitariosUsoExclusivo

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableSanitariosUsoExclusivo***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 135 && ObjConceptoSAEF.IdConcAccesibilidad < 145)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableSanitariosUsoExclusivo.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica6 = new CheckBox();
                            CheckCtrlAplica6.ID = "checkctrlAplica6" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica6);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste6 = new CheckBox();
                            CheckCtrlExiste6.ID = "checkctrlExiste6" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste6);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxSanitariosUsoExclusivo" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxSanitariosUsoExclusivo136")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere6 = new CheckBox();
                            CheckCtrlSeRequiere6.ID = "checkctrlSeRequiere6" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere6);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta cumple *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple6 = new CheckBox();
                            CheckCtrlCumple6.ID = "CheckCtrlCumple6" + Renglon.ID;//identificador de control
                            CheckCtrlCumple6.Attributes.Add("onclick", "Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple6);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones6 = new TextBox();
                            TextBoxCtrlObservaciones6.ID = "TextBoxObservaciones6" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones6.Width = 100;
                            TextBoxCtrlObservaciones6.MaxLength = 150;
                            TextBoxCtrlObservaciones6.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones6);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 136)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                        }


                        #endregion

                        #region TableRutaEvacuacionEmergente

                        //*********************AGREGAMOS LOS CONCEPTOS A LA TABLA DE TableRutaEvacuacionEmergente***********************
                        if (ObjConceptoSAEF.IdConcAccesibilidad > 144 && ObjConceptoSAEF.IdConcAccesibilidad < 155)
                        {


                            //Id control de la tabla
                            Renglon.ID = ObjConceptoSAEF.IdConcAccesibilidad.ToString(); //asignar id a la columna
                            this.TableRutaEvacuacionEmergente.Rows.Add(Renglon);



                            //**************************columna de descricion de concepto ************************************
                            Columna = new TableCell();
                            Columna.HorizontalAlign = HorizontalAlign.Left;
                            Columna.Text = ObjConceptoSAEF.DescConcAccesibilidade; //recupera los datos de los conceptos de SAEF

                            //imprimimos el encabezado de la columna de conceptos
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Acceso";
                                Columna.HorizontalAlign = HorizontalAlign.Left;
                            }
                            //agregamos la columna al renglon 
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta aplica *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlAplica7 = new CheckBox();
                            CheckCtrlAplica7.ID = "checkctrlAplica7" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlAplica7);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Aplica";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta existe *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlExiste7 = new CheckBox();
                            CheckCtrlExiste7.ID = "checkctrlExiste7" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlExiste7);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Existe";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //***********************************columna de respuestas*********************************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrl = new TextBox();
                            TextBoxCtrl.ID = "TextBoxRutaEvacuacionEmergente" + Renglon.ID; //identificador de control
                            TextBoxCtrl.Width = 100;
                            TextBoxCtrl.MaxLength = 6;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrl);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Cantidad";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }

                            if (TextBoxCtrl.ID != "TextBoxRutaEvacuacionEmergente145")
                            {
                                //control de ajax que solo permite numeros
                                AjaxControlToolkit.FilteredTextBoxExtender FilteredTextBoxExtenderSoloNums = new AjaxControlToolkit.FilteredTextBoxExtender();
                                FilteredTextBoxExtenderSoloNums.TargetControlID = TextBoxCtrl.ID;
                                FilteredTextBoxExtenderSoloNums.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                                FilteredTextBoxExtenderSoloNums.ValidChars = "0123456789";
                                Columna.Controls.Add(FilteredTextBoxExtenderSoloNums);
                            }
                            Renglon.Controls.Add(Columna);


                            //*********************************columna respuesta se requiere *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlSeRequiere7 = new CheckBox();
                            CheckCtrlSeRequiere7.ID = "checkctrlSeRequiere7" + Renglon.ID;//identificador de control
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlSeRequiere7);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "Se requiere";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //*********************************columna respuesta cumple *******************************************
                            Columna = new TableCell();

                            CheckBox CheckCtrlCumple7 = new CheckBox();
                            CheckCtrlCumple7.ID = "CheckCtrlCumple7" + Renglon.ID;//identificador de control
                            CheckCtrlCumple7.Attributes.Add("onclick", "Habilitar(this);");
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(CheckCtrlCumple7);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.ColumnSpan = 2;
                                Columna.Text = "Observaciones";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                            //**********************columna respuesta de campo de texto de observaciones ********************************
                            Columna = new TableCell();

                            TextBox TextBoxCtrlObservaciones7 = new TextBox();
                            TextBoxCtrlObservaciones7.ID = "TextBoxObservaciones7" + Renglon.ID; //identificador de control
                            TextBoxCtrlObservaciones7.Width = 100;
                            TextBoxCtrlObservaciones7.MaxLength = 150;
                            TextBoxCtrlObservaciones7.Enabled = false;
                            Columna.HorizontalAlign = HorizontalAlign.Center;
                            Columna.Controls.Add(TextBoxCtrlObservaciones7);
                            if (ObjConceptoSAEF.IdConcAccesibilidad == 145)
                            {
                                Columna.Font.Bold = true;
                                Columna.Text = "";
                                Columna.HorizontalAlign = HorizontalAlign.Center;
                            }
                            Renglon.Controls.Add(Columna);

                        }

                        #endregion



                    }
                }
            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al recuperar la lista de conceptos de la emisión de SAEF. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
               

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
        }

        //metodo para validar que por lo menos por cada renglon este puesta una opcion
        private Boolean ValidarRespuestaSAEF()
        {
            bool ok = true;

            #region servicio inmueble 

            if (TableServicioInmueble.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach(TableRow row in TableServicioInmueble.Rows)
            {
                if(Util.IsNumeric(row.ID))
                {
                    //iterar por cada columna/celda del renglon
                    foreach(TableCell cell in row.Cells)
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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (((TextBox)ctrl).Text.Length > 0)
                                    {
                                        if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                        {
                                           
                                            Msj = "El campo de cantidad debe de contener solo números en servicios del inmueble."; //
                                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                            
                                            ((TextBox)ctrl).Focus();

                                            return false; //romper todos los ciclos
                                        }
                                    }
                                    else
                                    {
                                       
                                        Msj = "Todos los campos de cantidad deben de estar llenos para servicios del inmueble."; //
                                        this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                        this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                        this.SetFocoIdCtrlRowCellCtrl(ctrl.ID,1);
                                        ((TextBox)ctrl).Focus();

                                        return false; //romper todos los ciclos
                                    }
                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon


                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla

            #endregion

            #region acceso inmueble

            if (TableAccesoInmueble.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableAccesoInmueble.Rows)
            {
                int contador1 = 0;
                bool Aplica1 = false;
                bool Existe1 = false;
                bool SeRequiere1 = false;
                bool Cumple1 = false;
                bool Cantidad1 = false;
                bool Observaciones1 = false;

               

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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if(contador1 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad1 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad1 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Cumple1 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones1 = true;
                                              
                                            }

                                        }
                                    }

                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador1 == 0)
                                            {
                                                Aplica1 = true;
                                            }

                                            if (contador1 == 1)
                                            {
                                                Existe1 = true;
                                            }

                                            if (contador1 == 2)
                                            {
                                                SeRequiere1 = true;
                                            }

                                            if (contador1 == 3)
                                            {
                                                Cumple1 = true;

                                            }


                                            contador1++;
                                        }
                                        else
                                        {
                                            contador1++;
                                        }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                    if (Convert.ToInt32(row.ID) != 32)
                    {
                        if (Aplica1 == false && Existe1 == false && Cantidad1 == true && SeRequiere1 == false && Cumple1 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de  acceso al inmueble."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 2);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if(Observaciones1 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  acceso al inmueble."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                   


                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla


            #endregion

            #region vestibulo

            if (TableVestibulo.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableVestibulo.Rows)
            {
                int contador2 = 0;
                bool Aplica2 = false;
                bool Existe2 = false;
                bool SeRequiere2 = false;
                bool Cumple2 = false;
                bool Cantidad2 = false;
                bool Observaciones2= false;



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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (contador2 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad2 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad2 = true;
                                        }
                                        
                                    }
                                    else
                                    {
                                        if (Cumple2 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones2 = true;

                                            }

                                        }

                                    }
                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                    if ((((CheckBox)ctrl).Checked) == true)
                                    {
                                        if (contador2 == 0)
                                        {
                                            Aplica2 = true;
                                        }

                                        if (contador2 == 1)
                                        {
                                            Existe2 = true;
                                        }

                                        if (contador2 == 2)
                                        {
                                            SeRequiere2 = true;
                                        }

                                        if (contador2 == 3)
                                        {
                                            Cumple2 = true;
                                        }


                                        contador2++;
                                    }
                                    else
                                    {
                                        contador2++;
                                    }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                    if (Convert.ToInt32(row.ID) != 55)
                    {
                        if (Aplica2 == false && Existe2 == false && Cantidad2 == true && SeRequiere2 == false && Cumple2 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de vestíbulo."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 3);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if (Observaciones2 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  vestíbulo."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                    

                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla


            #endregion

            #region circulaciones

            if (TableCirculaciones.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableCirculaciones.Rows)
            {
                int contador3 = 0;
                bool Aplica3 = false;
                bool Existe3 = false;
                bool SeRequiere3 = false;
                bool Cumple3 = false;
                bool Cantidad3 = false;
                bool Observaciones3 = false;


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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (contador3 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad3 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad3 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Cumple3 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones3 = true;

                                            }

                                        }

                                    }
                                    
                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                    if ((((CheckBox)ctrl).Checked) == true)
                                    {
                                        if (contador3 == 0)
                                        {
                                            Aplica3 = true;
                                        }

                                        if (contador3 == 1)
                                        {
                                            Existe3 = true;
                                        }

                                        if (contador3 == 2)
                                        {
                                            SeRequiere3 = true;
                                        }

                                        if (contador3 == 3)
                                        {
                                            Cumple3 = true;
                                        }


                                        contador3++;
                                    }
                                    else
                                    {
                                        contador3++;
                                    }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false


                    if (Convert.ToInt32(row.ID) != 79)
                    {
                        if (Aplica3 == false && Existe3 == false && Cantidad3 == true && SeRequiere3 == false && Cumple3 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de circulaciones."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 4);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if (Observaciones3 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  circulaciones."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                    

                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla


            #endregion

            #region señalizacion

            if (TableSenalizacion.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableSenalizacion.Rows)
            {
                int contador4 = 0;
                bool Aplica4 = false;
                bool Existe4 = false;
                bool SeRequiere4 = false;
                bool Cumple4 = false;
                bool Cantidad4 = false;
                bool Observaciones4 = false;


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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (contador4 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad4 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad4 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Cumple4 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones4 = true;

                                            }

                                        }
                                    }
                                    
                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                    if ((((CheckBox)ctrl).Checked) == true)
                                    {
                                        if (contador4 == 0)
                                        {
                                            Aplica4 = true;
                                        }

                                        if (contador4 == 1)
                                        {
                                            Existe4 = true;
                                        }

                                        if (contador4 == 2)
                                        {
                                            SeRequiere4 = true;
                                        }

                                        if (contador4 == 3)
                                        {
                                            Cumple4 = true;
                                        }


                                        contador4++;
                                    }
                                    else
                                    {
                                        contador4++;
                                    }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false


                    if (Convert.ToInt32(row.ID) != 99)
                    {
                        if (Aplica4 == false && Existe4 == false && Cantidad4 == true && SeRequiere4 == false && Cumple4 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de señalización."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 5);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if (Observaciones4 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de señalización."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                   


                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla

            #endregion

            #region uso de edificio y servicio

            if (TableUsoEdificioServicio.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableUsoEdificioServicio.Rows)
            {
                int contador5 = 0;
                bool Aplica5 = false;
                bool Existe5 = false;
                bool SeRequiere5 = false;
                bool Cumple5 = false;
                bool Cantidad5 = false;
                bool Observaciones5 = false;


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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (contador5 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad5 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad5 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Cumple5 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones5 = true;

                                            }

                                        }
                                    }
                                    
                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                    if ((((CheckBox)ctrl).Checked) == true)
                                    {
                                        if (contador5 == 0)
                                        {
                                            Aplica5 = true;
                                        }

                                        if (contador5 == 1)
                                        {
                                            Existe5 = true;
                                        }

                                        if (contador5 == 2)
                                        {
                                            SeRequiere5 = true;
                                        }

                                        if (contador5 == 3)
                                        {
                                            Cumple5 = true;
                                        }


                                        contador5++;
                                    }
                                    else
                                    {
                                        contador5++;
                                    }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false


                    if (Convert.ToInt32(row.ID) != 118)
                    {
                        if (Aplica5 == false && Existe5 == false && Cantidad5 == true && SeRequiere5 == false && Cumple5 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de uso de edificio y servicio."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 6);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if (Observaciones5 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  uso de edificio y servicio."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                    


                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla

            #endregion


            #region sanitarios para uso exclusivo

            if (TableSanitariosUsoExclusivo.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableSanitariosUsoExclusivo.Rows)
            {
                int contador6 = 0;
                bool Aplica6 = false;
                bool Existe6 = false;
                bool SeRequiere6 = false;
                bool Cumple6 = false;
                bool Cantidad6 = false;
                bool Observaciones6 = false;


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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (contador6 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad6 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad6 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Cumple6 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones6 = true;

                                            }

                                        }
                                    }
                                    
                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                    if ((((CheckBox)ctrl).Checked) == true)
                                    {
                                        if (contador6 == 0)
                                        {
                                            Aplica6 = true;
                                        }

                                        if (contador6 == 1)
                                        {
                                            Existe6 = true;
                                        }

                                        if (contador6 == 2)
                                        {
                                            SeRequiere6 = true;
                                        }

                                        if (contador6 == 3)
                                        {
                                            Cumple6 = true;
                                        }


                                        contador6++;
                                    }
                                    else
                                    {
                                        contador6++;
                                    }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                    if (Convert.ToInt32(row.ID) != 136)
                    {
                        if (Aplica6 == false && Existe6 == false && Cantidad6 == true && SeRequiere6 == false && Cumple6 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de sanitarios para uso exclusivo."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 7);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if (Observaciones6 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  sanitarios para uso exclusivo."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                    


                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla

            #endregion

            #region ruta de evacuacion emergente

            if (TableRutaEvacuacionEmergente.Rows.Count == 0)
            {
                ok = false;
            }

            //iterar por cada renglon de la tabla 
            foreach (TableRow row in TableRutaEvacuacionEmergente.Rows)
            {
                int contador7 = 0;
                bool Aplica7 = false;
                bool Existe7 = false;
                bool SeRequiere7 = false;
                bool Cumple7 = false;
                bool Cantidad7 = false;
                bool Observaciones7 = false;


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
                            {
                                ctrl = ctrlInner;
                            }

                            //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                            switch (ctrl.GetType().ToString())
                            {
                                case "System.Web.UI.WebControls.TextBox":

                                    if (contador7 < 3)
                                    {
                                        if (((TextBox)ctrl).Text.Length > 0)
                                        {
                                            if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                            {
                                                Cantidad7 = true;

                                            }
                                        }
                                        else
                                        {

                                            Cantidad7 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (Cumple7 == true)
                                        {
                                            // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                            if (((TextBox)ctrl).Text.Length == 0)
                                            {
                                                Observaciones7 = true;

                                            }

                                        }
                                    }
                                    
                                    break;

                                case "System.Web.UI.WebControls.CheckBox":

                                    if ((((CheckBox)ctrl).Checked) == true)
                                    {
                                        if (contador7 == 0)
                                        {
                                            Aplica7 = true;
                                        }

                                        if (contador7 == 1)
                                        {
                                            Existe7 = true;
                                        }

                                        if (contador7 == 2)
                                        {
                                            SeRequiere7 = true;
                                        }

                                        if (contador7 == 3)
                                        {
                                            Cumple7 = true;
                                        }


                                        contador7++;
                                    }
                                    else
                                    {
                                        contador7++;
                                    }

                                    break;
                            }//fin del switch


                        }//fin iterar por cada control dentro de la celda

                    }// fin de cada columna/celda del renglon

                    //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                    if (Convert.ToInt32(row.ID) != 145)
                    {
                        if (Aplica7 == false && Existe7 == false && Cantidad7 == true && SeRequiere7 == false && Cumple7 == false)
                        {
                            Msj = "Debe de llenar por lo menos uno de los campos en el indicador de ruta de evaluación emergente."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.SetFocoIdCtrlRowCellCtrl("", 8);
                            //((TextBox)ctrl).Focus();

                            return false; //romper todos los ciclos
                        }

                        if (Observaciones7 == true)
                        {
                            Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de ruta de evaluación emergente."; //
                            this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                            //validamos todos los check por codebehind
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                            return false;
                        }
                    }
                    


                }//fin del if de row id
            }//fin del foreach por cada renglon de la tabla

            #endregion

            return ok;
        }

        //Asigna el Foco a un control especificado, en la Tabla (aun en desarrollo)
        private Boolean SetFocoIdCtrlRowCellCtrl(String IdCtrl,int Tipo)
        {

            #region servicio inmueble 

            if(Tipo == 1)
            {
                //iterar por cada renglon de la tabla = concepto
                foreach (TableRow row in TableServicioInmueble.Rows)
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



                            }
                        } //foreach
                    }//foreach
                }//foreach

            }

            #endregion

            #region acceso inmueble


            if (Tipo == 2)
            {
                //iterar por cada renglon de la tabla = concepto
                foreach (TableRow row in TableAccesoInmueble.Rows)
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

                                case "System.Web.UI.WebControls.CheckBox":
                                    if (((CheckBox)ctrl).ID == IdCtrl)
                                    {
                                        ((CheckBox)ctrl).Focus();
                                        return true; //romper todos los ciclos, porque ya se encontro el ctrl.
                                    }

                                    break;



                            }
                        } //foreach
                    }//foreach
                }//foreach

            }


            #endregion

            #region vestibulo
            #endregion

            #region circulaciones
            #endregion

            #region señalizacion
            #endregion

            #region uso de edificio y servicio
            #endregion

            #region ruta evacuacion emergnte
            #endregion



            return false; //si llego a este punto es que no se encontro el control por el Id solicitado en los paramteros de entrada
        }

        protected void ButtonEnviarSAEF_Click(object sender, EventArgs e)
        {
           
            try
            {
                //primero se valida que todos los campos esten llenos (la validacion la dejamos por el moemtno)
                if(this.ValidarRespuestaSAEF())
                {
                    //despues de que se valida que todo este lleno, gaurdamos en base de datos los resultados.
                    if (this.InsertarRespuestaSAEF())
                    {

                        //lanzamos el mensaje de que fue exitoso y mostramos el acuse
                        Msj = "La emisión de accesibilidad ha sido registrada con éxito.";
                        this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Felicidades! </strong>" + Msj + "</div>";
                        this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Felicidades! </strong>" + Msj + "</div>";

                        //mostramos el pdf que se crea
                        this.ButtonEnviarSAEF.Enabled = false;
                        this.ButtonEnviarSAEF.Visible = false;
                        this.ButtonDescargarSAEF.Visible = true;
                        this.ButtonCancelarSAEF.Text = "Regresar";


                    }
                }

            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al guardar la captura de la emisión de SAEF. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";


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

        protected void ButtonCancelarSAEF_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("~/SAEF/BusqMvtosSAEF.aspx", false);
        }

        private Boolean InsertarRespuestaSAEF()
        {
            bool ok = false;
            bool ServicioInmueble = false;
            bool AccesoInmueble = false;
            bool Vestibulo = false;
            bool Circulaciones = false;
            bool Senalizacion = false;
            bool UsoEdificioServicio = false;
            bool SanitariosUsoExclusivo = false;
            bool RutaEvacuacionEmergente = false;
            

            try
            {
                #region servicio del inmueble 
                //iterar pop cada renglon de la tabla
                foreach (TableRow row in TableServicioInmueble.Rows)
                {
                    if (Util.IsNumeric(row.ID))
                    {
                        //iterar por cada columna/ celda del renglo
                        foreach (TableCell cell in row.Cells)
                        {
                            //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                            foreach (Control CtrlInner in cell.Controls)
                            {
                                Control ctrl;
                                if (CtrlInner.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlGenericControl")
                                {
                                    HtmlGenericControl myDiv = (HtmlGenericControl)CtrlInner;
                                    myDiv.Attributes.Clear();
                                    myDiv.Attributes.Add("class", "form-group");
                                    ctrl = CtrlInner.Controls[0];
                                }
                                else
                                {
                                    ctrl = CtrlInner;
                                }

                                int? Cantidad  = null;
                                

                                bool repetido = false;

                                //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                switch (ctrl.GetType().ToString())
                                {
                                    case "AjaxControlToolkit.FilteredTextBoxExtender":
                                        repetido = true;
                                        break;

                                    case "System.Web.UI.WebControls.TextBox":

                                        Cantidad = Convert.ToInt32((((TextBox)ctrl).Text));

                                        break;
                                }//final del switch



                                //realizamos el guardado 

                                if( repetido == false)
                                {
                                    ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();


                                    //llenamos todo el objeto
                                    ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                    ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                    ObjSAEF.Cantidad = Cantidad;
                                    ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                    ServicioInmueble = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty);
                                }

                               

                            }// final del for que itera las celdas
                        }//final del for que itera la columna
                    }// final del if
                }//final del for que itera el renglon

                #endregion


                #region acceso al inmueble

                if(ServicioInmueble == true)
                {

                    //iterar por cada renglon de la tabla
                    foreach(TableRow row in TableAccesoInmueble.Rows)
                    {
                        int contador1 = 0;
                        int? Cantidad1 = null;
                        bool? Aplica1 = null;
                        bool? Existe1 = null;
                        bool? SeRequiere1 = null;
                        bool? Cumple1 = null;
                        bool repetido1 = false;
                        string Observaciones1 = string.Empty;

                        if(Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;
                                    
                                    ctrl = CtrlInner;

                                    
                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido1 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if(contador1 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad1 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if(Cumple1 == true)
                                                {
                                                    if(!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones1 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                           
                                            
                                            break;

                                        case "System.Web.UI.WebControls.CheckBox" :

                                            if((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if(contador1 == 0)
                                                {
                                                    Aplica1 = true;
                                                }

                                                if(contador1 == 1)
                                                {
                                                    Existe1 = true;
                                                }

                                                if(contador1 == 2)
                                                {
                                                    SeRequiere1 = true;
                                                }

                                                if(contador1 == 3)
                                                {
                                                    Cumple1 = true;
                                                }


                                                contador1++;
                                            }
                                            else
                                            {
                                                contador1++;
                                            }


                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda


                            }//fin foreach columna


                            //guardamos los datos de la tabla 

                            if (Convert.ToInt32(row.ID) != 32)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica1;
                                ObjSAEF.Existe = Existe1;
                                ObjSAEF.Cantidad = Cantidad1;
                                ObjSAEF.SeRequiere = SeRequiere1;
                                ObjSAEF.Cumple = Cumple1;
                                ObjSAEF.Observaciones = Observaciones1;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                AccesoInmueble = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }

                        }//fin if de row.id


                    }//fin de foreach por cada renglon
                }//fin del if de validacion para la tabla
 
                

                #endregion

                #region vestibulo

                if(AccesoInmueble == true)
                {

                    //iterar por cada renglon de la tabla
                    foreach (TableRow row in TableVestibulo.Rows)
                    {
                        int contador2 = 0;
                        int? Cantidad2 = null;
                        bool? Aplica2 = null;
                        bool? Existe2 = null;
                        bool? SeRequiere2 = null;
                        bool? Cumple2 = null;
                        bool repetido2 = false;
                        string Observaciones2 = string.Empty;

                        if(Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido2 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador2 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad2 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple2 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones2 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador2 == 0)
                                                {
                                                    Aplica2 = true;
                                                }

                                                if (contador2 == 1)
                                                {
                                                    Existe2 = true;
                                                }

                                                if (contador2 == 2)
                                                {
                                                    SeRequiere2 = true;
                                                }

                                                if (contador2 == 3)
                                                {
                                                    Cumple2 = true;
                                                }


                                                contador2++;
                                            }
                                            else
                                            {
                                                contador2++;
                                            }

                                            break;
                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 55)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica2;
                                ObjSAEF.Existe = Existe2;
                                ObjSAEF.Cantidad = Cantidad2;
                                ObjSAEF.SeRequiere = SeRequiere2;
                                ObjSAEF.Cumple = Cumple2;
                                ObjSAEF.Observaciones = Observaciones2;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                Vestibulo = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }

                        }//fin if de row.id
                    }//fin de foreach por cada renglon
                }//fin del if de validacion para la tabla

                #endregion

                #region circulaciones

                if(Vestibulo == true)
                {
                    //iterar por cada renglon de la tabla
                    foreach (TableRow row in TableCirculaciones.Rows)
                    {
                        int contador3 = 0;
                        int? Cantidad3 = null;
                        bool? Aplica3 = null;
                        bool? Existe3 = null;
                        bool? SeRequiere3 = null;
                        bool? Cumple3 = null;
                        bool repetido3 = false;
                        string Observaciones3 = string.Empty;

                         if(Util.IsNumeric(row.ID))
                         {
                              //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                  //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido3 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador3 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad3 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple3 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones3 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador3 == 0)
                                                {
                                                    Aplica3 = true;
                                                }

                                                if (contador3 == 1)
                                                {
                                                    Existe3 = true;
                                                }

                                                if (contador3 == 2)
                                                {
                                                    SeRequiere3 = true;
                                                }

                                                if (contador3 == 3)
                                                {
                                                    Cumple3 = true;
                                                }


                                                contador3++;
                                            }
                                            else
                                            {
                                                contador3++;
                                            }

                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                             //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 79)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica3;
                                ObjSAEF.Existe = Existe3;
                                ObjSAEF.Cantidad = Cantidad3;
                                ObjSAEF.SeRequiere = SeRequiere3;
                                ObjSAEF.Cumple = Cumple3;
                                ObjSAEF.Observaciones = Observaciones3;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                Circulaciones = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }
                         }//fin if de row.id
                    }//fin de foreach por cada renglon
                }//fin del if de validacion para la tabla
 

                #endregion

                #region señalizacion

                if(Circulaciones == true)
                {
                    //iterar por cada renglon de la tabla
                    foreach (TableRow row in TableSenalizacion.Rows)
                    {
                        int contador4 = 0;
                        int? Cantidad4 = null;
                        bool? Aplica4 = null;
                        bool? Existe4 = null;
                        bool? SeRequiere4 = null;
                        bool? Cumple4 = null;
                        bool repetido4 = false;
                        string Observaciones4 = string.Empty;

                        if(Util.IsNumeric(row.ID))
                        {
                             //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                 //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                     //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido4 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador4 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad4 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple4 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones4 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador4 == 0)
                                                {
                                                    Aplica4 = true;
                                                }

                                                if (contador4 == 1)
                                                {
                                                    Existe4 = true;
                                                }

                                                if (contador4 == 2)
                                                {
                                                    SeRequiere4 = true;
                                                }

                                                if (contador4 == 3)
                                                {
                                                    Cumple4 = true;
                                                }


                                                contador4++;
                                            }
                                            else
                                            {
                                                contador4++;
                                            }

                                            break;
                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                             //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 99)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica4;
                                ObjSAEF.Existe = Existe4;
                                ObjSAEF.Cantidad = Cantidad4;
                                ObjSAEF.SeRequiere = SeRequiere4;
                                ObjSAEF.Cumple = Cumple4;
                                ObjSAEF.Observaciones = Observaciones4;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                Senalizacion = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }

                        }//fin if de row.id
                    }//fin de foreach por cada renglon
                }//fin del if de validacion para la tabla

                #endregion

                #region uso de edificio y servicio

                if(Senalizacion == true)
                {
                    //iterar por cada renglon de la tabla
                    foreach (TableRow row in TableUsoEdificioServicio.Rows)
                    {
                        int contador5 = 0;
                        int? Cantidad5 = null;
                        bool? Aplica5 = null;
                        bool? Existe5 = null;
                        bool? SeRequiere5 = null;
                        bool? Cumple5 = null;
                        bool repetido5 = false;
                        string Observaciones5 = string.Empty;

                        if(Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido5 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador5 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad5 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple5 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones5 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador5 == 0)
                                                {
                                                    Aplica5 = true;
                                                }

                                                if (contador5 == 1)
                                                {
                                                    Existe5 = true;
                                                }

                                                if (contador5 == 2)
                                                {
                                                    SeRequiere5 = true;
                                                }

                                                if (contador5 == 3)
                                                {
                                                    Cumple5 = true;
                                                }


                                                contador5++;
                                            }
                                            else
                                            {
                                                contador5++;
                                            }

                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 118)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica5;
                                ObjSAEF.Existe = Existe5;
                                ObjSAEF.Cantidad = Cantidad5;
                                ObjSAEF.SeRequiere = SeRequiere5;
                                ObjSAEF.Cumple = Cumple5;
                                ObjSAEF.Observaciones = Observaciones5;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                UsoEdificioServicio = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }
                        }//fin if de row.id

                    }//fin de foreach por cada renglon
                }//fin del if de validacion para la tabla

                #endregion

                #region sanitarios para uso exclusivo

                 if(UsoEdificioServicio == true)
                 {
                     //iterar por cada renglon de la tabla
                     foreach (TableRow row in TableSanitariosUsoExclusivo.Rows)
                     {
                        int contador6 = 0;
                        int? Cantidad6 = null;
                        bool? Aplica6 = null;
                        bool? Existe6 = null;
                        bool? SeRequiere6 = null;
                        bool? Cumple6 = null;
                        bool repetido6 = false;
                        string Observaciones6 = string.Empty;

                         if(Util.IsNumeric(row.ID))
                         {
                             //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido6 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador6 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad6 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple6 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones6 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador6 == 0)
                                                {
                                                    Aplica6 = true;
                                                }

                                                if (contador6 == 1)
                                                {
                                                    Existe6 = true;
                                                }

                                                if (contador6 == 2)
                                                {
                                                    SeRequiere6 = true;
                                                }

                                                if (contador6 == 3)
                                                {
                                                    Cumple6 = true;
                                                }


                                                contador6++;
                                            }
                                            else
                                            {
                                                contador6++;
                                            }

                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                             //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 136)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica6;
                                ObjSAEF.Existe = Existe6;
                                ObjSAEF.Cantidad = Cantidad6;
                                ObjSAEF.SeRequiere = SeRequiere6;
                                ObjSAEF.Cumple = Cumple6;
                                ObjSAEF.Observaciones = Observaciones6;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                SanitariosUsoExclusivo = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }

                         }//fin if de row.id
                    }//fin de foreach por cada renglon
                 }//fin del if de validacion para la tabla

                #endregion

                #region ruta de evacuacion

                if(SanitariosUsoExclusivo == true)
                {
                    //iterar por cada renglon de la tabla
                    foreach (TableRow row in TableRutaEvacuacionEmergente.Rows)
                     {
                         int contador7 = 0;
                         int? Cantidad7 = null;
                         bool? Aplica7 = null;
                         bool? Existe7 = null;
                         bool? SeRequiere7 = null;
                         bool? Cumple7 = null;
                         bool repetido7 = false;
                         string Observaciones7 = string.Empty;

                        if(Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach(TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach(Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch(ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido7 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":


                                            if (contador7 < 3)
                                            {

                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad7 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }

                                            }
                                            else
                                            {
                                                if (Cumple7 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones7 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador7 == 0)
                                                {
                                                    Aplica7 = true;
                                                }

                                                if (contador7 == 1)
                                                {
                                                    Existe7 = true;
                                                }

                                                if (contador7 == 2)
                                                {
                                                    SeRequiere7 = true;
                                                }

                                                if (contador7 == 3)
                                                {
                                                    Cumple7 = true;
                                                }


                                                contador7++;
                                            }
                                            else
                                            {
                                                contador7++;
                                            }

                                            break;
                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 145)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica7;
                                ObjSAEF.Existe = Existe7;
                                ObjSAEF.Cantidad = Cantidad7;
                                ObjSAEF.SeRequiere = SeRequiere7;
                                ObjSAEF.Cumple = Cumple7;
                                ObjSAEF.Observaciones = Observaciones7;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                RutaEvacuacionEmergente = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty,string.Empty); 
                            }
                        }//fin if de row.id
                     }//fin de foreach por cada renglon
                }//fin del if de validacion para la tabla

                #endregion



                //validamos que todo se guardo en movimiento 
                if(ServicioInmueble == true && AccesoInmueble == true && Vestibulo == true && Circulaciones == true && Senalizacion == true && UsoEdificioServicio == true && SanitariosUsoExclusivo == true && RutaEvacuacionEmergente == true)
                {
                    Control LabelDireccion = this.ctrlDireccionLectura.FindControl("LabelDireccion");
                    string DireccionInmueble = ((Label)LabelDireccion).Text;

                    Control LabelInstitucion = this.ctrlUsuarioInfo.FindControl("LabelInstitucion");
                    string InstitucionUsr = ((Label)LabelInstitucion).Text;

                    Control LabelNombre = this.ctrlUsuarioInfo.FindControl("LabelUsr");
                    string NombreUsuario = ((Label)LabelNombre).Text;

                    string UrlAbrirQR = ConfigurationManager.AppSettings["URLQR"];

                    //generamos cadena
                    string CadenaOriginalSAEF = "||Invocante:[" + InstitucionUsr + "] || Inmueble:[" + DireccionInmueble + "] || FolioEmision:[" + FolioEmisionOpinion + "] ||Fecha:[" + DateTime.Today.ToLongDateString() + "]||" + Guid.NewGuid().ToString();

                    //generamos sello
                    string SelloDigitalSAEF = UtilContratosArrto.Encrypt(CadenaOriginalSAEF, true, "Accesibilidad");

                    //guaradmos en base de datos toda esta informacion

                    //objetos de saef
                    ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                    //llenamos el objeto
                    ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                    ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                    //GENERAMOS EL QR el tipo 3 es para las emisiones de accesibilidad
                    string QR = UtilContratosArrto.GenerarCodigoQR(FolioEmisionOpinion, 3, NombreUsuario, UrlAbrirQR);

                     

                    ok = new NG_SAEF().GuardarSAEF(ObjSAEF, 2, CadenaOriginalSAEF, SelloDigitalSAEF,QR);

                }

            }
            catch(Exception ex)
            {
                Msj = "Ha ocurrido un error al insertar la  captura de la emisión de SAEF. Contacta al área de sistemas.";
                this.LabelInfoSAEF.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";


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
        }

        protected void ButtonDescargarSAEF_Click(object sender, EventArgs e)
        {
            //se descarga el acuse de la emision de accesibilidad
            INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias.ExportHTML exportar = new INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias.ExportHTML();

            exportar.CuerpoCompletoPlantillaSAEF(null, IdAplicacionConcepto);
        }

        protected void btnGuardarAccesoInmueble_Click(object sender, EventArgs e)
        {
            bool ok = true;

            try
            {                
                bool AccesoInmueble = false;

                if (TableAccesoInmueble.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableAccesoInmueble.Rows)
                {
                    int contador1 = 0;
                    bool Aplica1 = false;
                    bool Existe1 = false;
                    bool SeRequiere1 = false;
                    bool Cumple1 = false;
                    bool Cantidad1 = false;
                    bool Observaciones1 = false;



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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador1 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad1 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad1 = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Cumple1 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones1 = true;

                                                }

                                            }
                                        }

                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador1 == 0)
                                            {
                                                Aplica1 = true;
                                            }

                                            if (contador1 == 1)
                                            {
                                                Existe1 = true;
                                            }

                                            if (contador1 == 2)
                                            {
                                                SeRequiere1 = true;
                                            }

                                            if (contador1 == 3)
                                            {
                                                Cumple1 = true;

                                            }


                                            contador1++;
                                        }
                                        else
                                        {
                                            contador1++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                        if (Convert.ToInt32(row.ID) != 32)
                        {
                            if (Aplica1 == false && Existe1 == false && Cantidad1 == true && SeRequiere1 == false && Cumple1 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de  acceso al inmueble."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 2);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones1 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  acceso al inmueble."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }

                    }//fin del if de row id


                }

                if (ok == true)
                {
                    foreach (TableRow row in TableAccesoInmueble.Rows)
                    {
                        int contador1 = 0;
                        int? Cantidad1 = null;
                        bool? Aplica1 = null;
                        bool? Existe1 = null;
                        bool? SeRequiere1 = null;
                        bool? Cumple1 = null;
                        bool repetido1 = false;
                        string Observaciones1 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;


                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido1 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador1 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad1 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple1 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones1 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }



                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador1 == 0)
                                                {
                                                    Aplica1 = true;
                                                }

                                                if (contador1 == 1)
                                                {
                                                    Existe1 = true;
                                                }

                                                if (contador1 == 2)
                                                {
                                                    SeRequiere1 = true;
                                                }

                                                if (contador1 == 3)
                                                {
                                                    Cumple1 = true;
                                                }


                                                contador1++;
                                            }
                                            else
                                            {
                                                contador1++;
                                            }


                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda


                            }//fin foreach columna


                            //guardamos los datos de la tabla 

                            if (Convert.ToInt32(row.ID) != 32)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica1;
                                ObjSAEF.Existe = Existe1;
                                ObjSAEF.Cantidad = Cantidad1;
                                ObjSAEF.SeRequiere = SeRequiere1;
                                ObjSAEF.Cumple = Cumple1;
                                ObjSAEF.Observaciones = Observaciones1;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                AccesoInmueble = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }
                        }//fin if de row.id
                    }
                }

                if (AccesoInmueble == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        protected void btnGuardarVestibulo_Click(object sender, EventArgs e)
        {
            bool ok = true;
            bool Vestibulo = false;

            try
            {
                if (TableVestibulo.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableVestibulo.Rows)
                {
                    int contador2 = 0;
                    bool Aplica2 = false;
                    bool Existe2 = false;
                    bool SeRequiere2 = false;
                    bool Cumple2 = false;
                    bool Cantidad2 = false;
                    bool Observaciones2 = false;



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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador2 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad2 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad2 = true;
                                            }

                                        }
                                        else
                                        {
                                            if (Cumple2 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones2 = true;

                                                }

                                            }

                                        }
                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador2 == 0)
                                            {
                                                Aplica2 = true;
                                            }

                                            if (contador2 == 1)
                                            {
                                                Existe2 = true;
                                            }

                                            if (contador2 == 2)
                                            {
                                                SeRequiere2 = true;
                                            }

                                            if (contador2 == 3)
                                            {
                                                Cumple2 = true;
                                            }


                                            contador2++;
                                        }
                                        else
                                        {
                                            contador2++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                        if (Convert.ToInt32(row.ID) != 55)
                        {
                            if (Aplica2 == false && Existe2 == false && Cantidad2 == true && SeRequiere2 == false && Cumple2 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de vestíbulo."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 3);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones2 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  vestíbulo."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }


                    }//fin del if de row id
                }

                if (ok == true)
                {
                    foreach (TableRow row in TableVestibulo.Rows)
                    {
                        int contador2 = 0;
                        int? Cantidad2 = null;
                        bool? Aplica2 = null;
                        bool? Existe2 = null;
                        bool? SeRequiere2 = null;
                        bool? Cumple2 = null;
                        bool repetido2 = false;
                        string Observaciones2 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido2 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador2 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad2 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple2 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones2 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador2 == 0)
                                                {
                                                    Aplica2 = true;
                                                }

                                                if (contador2 == 1)
                                                {
                                                    Existe2 = true;
                                                }

                                                if (contador2 == 2)
                                                {
                                                    SeRequiere2 = true;
                                                }

                                                if (contador2 == 3)
                                                {
                                                    Cumple2 = true;
                                                }


                                                contador2++;
                                            }
                                            else
                                            {
                                                contador2++;
                                            }

                                            break;
                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 55)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica2;
                                ObjSAEF.Existe = Existe2;
                                ObjSAEF.Cantidad = Cantidad2;
                                ObjSAEF.SeRequiere = SeRequiere2;
                                ObjSAEF.Cumple = Cumple2;
                                ObjSAEF.Observaciones = Observaciones2;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                Vestibulo = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }

                        }//fin if de row.id
                    }
                }

                if (Vestibulo == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnGuardarCirculaciones_Click(object sender, EventArgs e)
        {
            bool ok = true;
            bool Circulaciones = false;

            try
            {
                if (TableCirculaciones.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableCirculaciones.Rows)
                {
                    int contador3 = 0;
                    bool Aplica3 = false;
                    bool Existe3 = false;
                    bool SeRequiere3 = false;
                    bool Cumple3 = false;
                    bool Cantidad3 = false;
                    bool Observaciones3 = false;


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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador3 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad3 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad3 = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Cumple3 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones3 = true;

                                                }

                                            }

                                        }

                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador3 == 0)
                                            {
                                                Aplica3 = true;
                                            }

                                            if (contador3 == 1)
                                            {
                                                Existe3 = true;
                                            }

                                            if (contador3 == 2)
                                            {
                                                SeRequiere3 = true;
                                            }

                                            if (contador3 == 3)
                                            {
                                                Cumple3 = true;
                                            }


                                            contador3++;
                                        }
                                        else
                                        {
                                            contador3++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false


                        if (Convert.ToInt32(row.ID) != 79)
                        {
                            if (Aplica3 == false && Existe3 == false && Cantidad3 == true && SeRequiere3 == false && Cumple3 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de circulaciones."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 4);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones3 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  circulaciones."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }


                    }//fin del if de row id
                }

                if (ok == true)
                {
                    foreach (TableRow row in TableCirculaciones.Rows)
                    {
                        int contador3 = 0;
                        int? Cantidad3 = null;
                        bool? Aplica3 = null;
                        bool? Existe3 = null;
                        bool? SeRequiere3 = null;
                        bool? Cumple3 = null;
                        bool repetido3 = false;
                        string Observaciones3 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido3 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador3 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad3 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple3 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones3 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador3 == 0)
                                                {
                                                    Aplica3 = true;
                                                }

                                                if (contador3 == 1)
                                                {
                                                    Existe3 = true;
                                                }

                                                if (contador3 == 2)
                                                {
                                                    SeRequiere3 = true;
                                                }

                                                if (contador3 == 3)
                                                {
                                                    Cumple3 = true;
                                                }


                                                contador3++;
                                            }
                                            else
                                            {
                                                contador3++;
                                            }

                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 79)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica3;
                                ObjSAEF.Existe = Existe3;
                                ObjSAEF.Cantidad = Cantidad3;
                                ObjSAEF.SeRequiere = SeRequiere3;
                                ObjSAEF.Cumple = Cumple3;
                                ObjSAEF.Observaciones = Observaciones3;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                Circulaciones = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }
                        }//fin if de row.id
                    }
                }

                if (Circulaciones == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnGuardarSenalizacion_Click(object sender, EventArgs e)
        {
            bool ok = true;
            bool Senalizacion = false;

            try
            {
                if (TableSenalizacion.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableSenalizacion.Rows)
                {
                    int contador4 = 0;
                    bool Aplica4 = false;
                    bool Existe4 = false;
                    bool SeRequiere4 = false;
                    bool Cumple4 = false;
                    bool Cantidad4 = false;
                    bool Observaciones4 = false;


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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador4 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad4 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad4 = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Cumple4 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones4 = true;

                                                }

                                            }
                                        }

                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador4 == 0)
                                            {
                                                Aplica4 = true;
                                            }

                                            if (contador4 == 1)
                                            {
                                                Existe4 = true;
                                            }

                                            if (contador4 == 2)
                                            {
                                                SeRequiere4 = true;
                                            }

                                            if (contador4 == 3)
                                            {
                                                Cumple4 = true;
                                            }


                                            contador4++;
                                        }
                                        else
                                        {
                                            contador4++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false


                        if (Convert.ToInt32(row.ID) != 99)
                        {
                            if (Aplica4 == false && Existe4 == false && Cantidad4 == true && SeRequiere4 == false && Cumple4 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de señalización."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 5);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones4 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de señalización."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }

                    }//fin del if de row id
                }

                if (ok == true)
                {
                    foreach (TableRow row in TableSenalizacion.Rows)
                    {
                        int contador4 = 0;
                        int? Cantidad4 = null;
                        bool? Aplica4 = null;
                        bool? Existe4 = null;
                        bool? SeRequiere4 = null;
                        bool? Cumple4 = null;
                        bool repetido4 = false;
                        string Observaciones4 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido4 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador4 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad4 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple4 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones4 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador4 == 0)
                                                {
                                                    Aplica4 = true;
                                                }

                                                if (contador4 == 1)
                                                {
                                                    Existe4 = true;
                                                }

                                                if (contador4 == 2)
                                                {
                                                    SeRequiere4 = true;
                                                }

                                                if (contador4 == 3)
                                                {
                                                    Cumple4 = true;
                                                }


                                                contador4++;
                                            }
                                            else
                                            {
                                                contador4++;
                                            }

                                            break;
                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 99)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica4;
                                ObjSAEF.Existe = Existe4;
                                ObjSAEF.Cantidad = Cantidad4;
                                ObjSAEF.SeRequiere = SeRequiere4;
                                ObjSAEF.Cumple = Cumple4;
                                ObjSAEF.Observaciones = Observaciones4;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                Senalizacion = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }

                        }//fin if de row.id
                    }
                }

                if (Senalizacion == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BtnUsoEdificio_Click(object sender, EventArgs e)
        {
            bool ok = true;
            bool UsoEdificioServicio = false;

            try
            {
                if (TableUsoEdificioServicio.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableUsoEdificioServicio.Rows)
                {
                    int contador5 = 0;
                    bool Aplica5 = false;
                    bool Existe5 = false;
                    bool SeRequiere5 = false;
                    bool Cumple5 = false;
                    bool Cantidad5 = false;
                    bool Observaciones5 = false;


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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador5 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad5 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad5 = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Cumple5 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones5 = true;

                                                }

                                            }
                                        }

                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador5 == 0)
                                            {
                                                Aplica5 = true;
                                            }

                                            if (contador5 == 1)
                                            {
                                                Existe5 = true;
                                            }

                                            if (contador5 == 2)
                                            {
                                                SeRequiere5 = true;
                                            }

                                            if (contador5 == 3)
                                            {
                                                Cumple5 = true;
                                            }


                                            contador5++;
                                        }
                                        else
                                        {
                                            contador5++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false


                        if (Convert.ToInt32(row.ID) != 118)
                        {
                            if (Aplica5 == false && Existe5 == false && Cantidad5 == true && SeRequiere5 == false && Cumple5 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de uso de edificio y servicio."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 6);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones5 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  uso de edificio y servicio."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }
                    }//fin del if de row id
                }

                if (ok == true)
                {
                    foreach (TableRow row in TableUsoEdificioServicio.Rows)
                    {
                        int contador5 = 0;
                        int? Cantidad5 = null;
                        bool? Aplica5 = null;
                        bool? Existe5 = null;
                        bool? SeRequiere5 = null;
                        bool? Cumple5 = null;
                        bool repetido5 = false;
                        string Observaciones5 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido5 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador5 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad5 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple5 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones5 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador5 == 0)
                                                {
                                                    Aplica5 = true;
                                                }

                                                if (contador5 == 1)
                                                {
                                                    Existe5 = true;
                                                }

                                                if (contador5 == 2)
                                                {
                                                    SeRequiere5 = true;
                                                }

                                                if (contador5 == 3)
                                                {
                                                    Cumple5 = true;
                                                }


                                                contador5++;
                                            }
                                            else
                                            {
                                                contador5++;
                                            }

                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 118)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica5;
                                ObjSAEF.Existe = Existe5;
                                ObjSAEF.Cantidad = Cantidad5;
                                ObjSAEF.SeRequiere = SeRequiere5;
                                ObjSAEF.Cumple = Cumple5;
                                ObjSAEF.Observaciones = Observaciones5;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                UsoEdificioServicio = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }
                        }//fin if de row.id
                    }
                }

                if (UsoEdificioServicio == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BtnSanitarios_Click(object sender, EventArgs e)
        {
            bool ok = true;
            bool SanitariosUsoExclusivo = false;

            try
            {
                if (TableSanitariosUsoExclusivo.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableSanitariosUsoExclusivo.Rows)
                {
                    int contador6 = 0;
                    bool Aplica6 = false;
                    bool Existe6 = false;
                    bool SeRequiere6 = false;
                    bool Cumple6 = false;
                    bool Cantidad6 = false;
                    bool Observaciones6 = false;


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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador6 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad6 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad6 = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Cumple6 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones6 = true;

                                                }

                                            }
                                        }

                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador6 == 0)
                                            {
                                                Aplica6 = true;
                                            }

                                            if (contador6 == 1)
                                            {
                                                Existe6 = true;
                                            }

                                            if (contador6 == 2)
                                            {
                                                SeRequiere6 = true;
                                            }

                                            if (contador6 == 3)
                                            {
                                                Cumple6 = true;
                                            }


                                            contador6++;
                                        }
                                        else
                                        {
                                            contador6++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                        if (Convert.ToInt32(row.ID) != 136)
                        {
                            if (Aplica6 == false && Existe6 == false && Cantidad6 == true && SeRequiere6 == false && Cumple6 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de sanitarios para uso exclusivo."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 7);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones6 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de  sanitarios para uso exclusivo."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }
                    }//fin del if de row id
                }

                if (ok == true)
                {
                    foreach (TableRow row in TableSanitariosUsoExclusivo.Rows)
                    {
                        int contador6 = 0;
                        int? Cantidad6 = null;
                        bool? Aplica6 = null;
                        bool? Existe6 = null;
                        bool? SeRequiere6 = null;
                        bool? Cumple6 = null;
                        bool repetido6 = false;
                        string Observaciones6 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido6 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":

                                            if (contador6 < 3)
                                            {
                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad6 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }
                                            }
                                            else
                                            {
                                                if (Cumple6 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones6 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador6 == 0)
                                                {
                                                    Aplica6 = true;
                                                }

                                                if (contador6 == 1)
                                                {
                                                    Existe6 = true;
                                                }

                                                if (contador6 == 2)
                                                {
                                                    SeRequiere6 = true;
                                                }

                                                if (contador6 == 3)
                                                {
                                                    Cumple6 = true;
                                                }


                                                contador6++;
                                            }
                                            else
                                            {
                                                contador6++;
                                            }

                                            break;

                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 136)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica6;
                                ObjSAEF.Existe = Existe6;
                                ObjSAEF.Cantidad = Cantidad6;
                                ObjSAEF.SeRequiere = SeRequiere6;
                                ObjSAEF.Cumple = Cumple6;
                                ObjSAEF.Observaciones = Observaciones6;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                SanitariosUsoExclusivo = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }

                        }//fin if de row.id
                    }
                }

                if (SanitariosUsoExclusivo == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnRutaEvacuacion_Click(object sender, EventArgs e)
        {
            bool ok = true;
            bool RutaEvacuacionEmergente = false;

            try
            {
                if (TableRutaEvacuacionEmergente.Rows.Count == 0)
                {
                    ok = false;
                }

                //iterar por cada renglon de la tabla 
                foreach (TableRow row in TableRutaEvacuacionEmergente.Rows)
                {
                    int contador7 = 0;
                    bool Aplica7 = false;
                    bool Existe7 = false;
                    bool SeRequiere7 = false;
                    bool Cumple7 = false;
                    bool Cantidad7 = false;
                    bool Observaciones7 = false;


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
                                {
                                    ctrl = ctrlInner;
                                }

                                //la respuesta al concepto puede encontrarse en un control de tipo: TextBox
                                switch (ctrl.GetType().ToString())
                                {
                                    case "System.Web.UI.WebControls.TextBox":

                                        if (contador7 < 3)
                                        {
                                            if (((TextBox)ctrl).Text.Length > 0)
                                            {
                                                if ((Util.IsEnteroNatural(((TextBox)ctrl).Text)) == false)
                                                {
                                                    Cantidad7 = true;

                                                }
                                            }
                                            else
                                            {

                                                Cantidad7 = true;
                                            }
                                        }
                                        else
                                        {
                                            if (Cumple7 == true)
                                            {
                                                // si esta seleccionado cumple tenemos que validar que tenga texto ingresado
                                                if (((TextBox)ctrl).Text.Length == 0)
                                                {
                                                    Observaciones7 = true;

                                                }

                                            }
                                        }

                                        break;

                                    case "System.Web.UI.WebControls.CheckBox":

                                        if ((((CheckBox)ctrl).Checked) == true)
                                        {
                                            if (contador7 == 0)
                                            {
                                                Aplica7 = true;
                                            }

                                            if (contador7 == 1)
                                            {
                                                Existe7 = true;
                                            }

                                            if (contador7 == 2)
                                            {
                                                SeRequiere7 = true;
                                            }

                                            if (contador7 == 3)
                                            {
                                                Cumple7 = true;
                                            }


                                            contador7++;
                                        }
                                        else
                                        {
                                            contador7++;
                                        }

                                        break;
                                }//fin del switch


                            }//fin iterar por cada control dentro de la celda

                        }// fin de cada columna/celda del renglon

                        //mnadamos la leyeda de que valor no esta correcto y retornamos un false

                        if (Convert.ToInt32(row.ID) != 145)
                        {
                            if (Aplica7 == false && Existe7 == false && Cantidad7 == true && SeRequiere7 == false && Cumple7 == false)
                            {
                                Msj = "Debe de llenar por lo menos uno de los campos en el indicador de ruta de evaluación emergente."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.SetFocoIdCtrlRowCellCtrl("", 8);
                                //((TextBox)ctrl).Focus();

                                ok = false; //romper todos los ciclos
                            }

                            if (Observaciones7 == true)
                            {
                                Msj = "si dio check al campo de observaciones, debe de llenar el campo de texto en el indicador de ruta de evaluación emergente."; //
                                this.LabelInfoSAEF.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                                this.LabelInfoSAEF2.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";

                                //validamos todos los check por codebehind
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "validar", "ValidarCheck();", true);

                                ok = false;
                            }
                        }



                    }//fin del if de row id

                }

                if (ok == true)
                {
                    foreach (TableRow row in TableRutaEvacuacionEmergente.Rows)
                    {
                        int contador7 = 0;
                        int? Cantidad7 = null;
                        bool? Aplica7 = null;
                        bool? Existe7 = null;
                        bool? SeRequiere7 = null;
                        bool? Cumple7 = null;
                        bool repetido7 = false;
                        string Observaciones7 = string.Empty;

                        if (Util.IsNumeric(row.ID))
                        {
                            //iterar popr cada columna del renglon
                            foreach (TableCell cell in row.Cells)
                            {
                                //iterar po cada control dentro de la celda y validar contenido de acuerdo al control
                                foreach (Control CtrlInner in cell.Controls)
                                {
                                    Control ctrl;

                                    ctrl = CtrlInner;

                                    //la respuesta al concepto puede encontrarse en un control de tipo: textbox, checkbox 
                                    switch (ctrl.GetType().ToString())
                                    {
                                        case "AjaxControlToolkit.FilteredTextBoxExtender":
                                            repetido7 = true;
                                            break;

                                        case "System.Web.UI.WebControls.TextBox":


                                            if (contador7 < 3)
                                            {

                                                if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                {
                                                    Cantidad7 = Convert.ToInt32((((TextBox)ctrl).Text));
                                                }

                                            }
                                            else
                                            {
                                                if (Cumple7 == true)
                                                {
                                                    if (!string.IsNullOrEmpty((((TextBox)ctrl).Text)))
                                                    {
                                                        Observaciones7 = (((TextBox)ctrl).Text);
                                                    }
                                                }
                                            }

                                            break;

                                        case "System.Web.UI.WebControls.CheckBox":

                                            if ((((CheckBox)ctrl).Checked) == true)
                                            {
                                                if (contador7 == 0)
                                                {
                                                    Aplica7 = true;
                                                }

                                                if (contador7 == 1)
                                                {
                                                    Existe7 = true;
                                                }

                                                if (contador7 == 2)
                                                {
                                                    SeRequiere7 = true;
                                                }

                                                if (contador7 == 3)
                                                {
                                                    Cumple7 = true;
                                                }


                                                contador7++;
                                            }
                                            else
                                            {
                                                contador7++;
                                            }

                                            break;
                                    }//fin del switch
                                }//fin del foreach de cada celda
                            }//fin foreach columna

                            //guardamos los datos de la tabla
                            if (Convert.ToInt32(row.ID) != 145)
                            {
                                //objetos de saef
                                ValorRespuestaSAEF ObjSAEF = new ValorRespuestaSAEF();

                                //llenamos el objeto
                                ObjSAEF.IdAlicacionConcepto = IdAplicacionConcepto;
                                ObjSAEF.ConceptoAccesibilidad = Convert.ToInt32(row.ID);
                                ObjSAEF.Aplica = Aplica7;
                                ObjSAEF.Existe = Existe7;
                                ObjSAEF.Cantidad = Cantidad7;
                                ObjSAEF.SeRequiere = SeRequiere7;
                                ObjSAEF.Cumple = Cumple7;
                                ObjSAEF.Observaciones = Observaciones7;
                                ObjSAEF.IdUsuario = ((SSO)Session["Contexto"]).IdUsuario.Value; //sso

                                RutaEvacuacionEmergente = new NG_SAEF().GuardarSAEF(ObjSAEF, 1, string.Empty, string.Empty, string.Empty);
                            }
                        }//fin if de row.id
                    }
                }

                if (RutaEvacuacionEmergente == true)
                {
                    Msj = "Los datos se guardaron correctamente";
                    this.LabelInfoSAEF.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena!  </strong>" + Msj + "</div>";
                    this.LabelInfoSAEF2.Text = "<div class='alert alert-success'><strong> ¡Enhorabuena! </strong> " + Msj + "</div>";

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}