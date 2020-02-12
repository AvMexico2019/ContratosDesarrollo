using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Reflection;
using System.Data.SqlClient;

using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio; //capa BO
using INDAABIN.DI.ModeloNegocio;//bus
using INDAABIN.DI.CONTRATOS.Aplicacion.Exportar;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.TablaSMOI
{
    public partial class AcuseSMOI : System.Web.UI.Page
    {
        String Msj;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = string.Empty;
            if (!IsPostBack)
            {
                if (Request.QueryString["IdFolio"] != null)
                {
                    //Session["intFolioConceptoResp"] = Request.QueryString["IdFolio"].ToString();
                    this.ObtenerInformacionAcuseFolioSMOI();
                    
                }
                else
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('No has seleccionado un folio valido'); window.close();", true);
                }
            }

            LlenarTablasSMOI(Request.QueryString["IdFolio"].ToString());
        }

        public void LlenarTablasSMOI(string fol)
        {
            int IdFolio = 0;
            List<ConceptoRespValor> Lconcepto = new List<ConceptoRespValor>();

            try
            {
                int.TryParse(fol, out IdFolio);
                NGConceptoRespValor nConcepto = new NGConceptoRespValor();

                Lconcepto = nConcepto.ObtenerTablaSMOIFolio(IdFolio);
                LlenarTablaX(Lconcepto.Where( x => x.IdTema == 1).ToList());
                LlenarTablaZ(Lconcepto.Where(x => x.IdTema == 6).ToList());
            }

            catch (Exception) { }
        }

        public void LlenarTablaX(List<ConceptoRespValor> Lconcepto)
        {
            TableRow renglon;

            lblPerX.Text = Lconcepto.Sum(x => x.ValorRespuesta).Value.ToString("#0");

            foreach (ConceptoRespValor concepto in Lconcepto)
            {
                renglon = new TableRow();

                TableCell celda1 = new TableCell();
                TableCell celda2 = new TableCell();
                TableCell celda3 = new TableCell();
                TableCell celda4 = new TableCell();
                TableCell celda5 = new TableCell();

                celda1.Text = concepto.NumOrdenVisual;
                celda2.Text = concepto.DescripcionConcepto;
                celda3.Text = concepto.ValorPonderacionRespuesta.ToString();
                celda4.Text = concepto.ValorRespuesta == null ? "0" : concepto.ValorRespuesta.Value.ToString("#0");
                celda5.Text = ((concepto.ValorRespuesta == null ? 0 : concepto.ValorRespuesta.Value) * concepto.ValorPonderacionRespuesta.Value).ToString("#0.00");

                renglon.Controls.Add(celda1);
                renglon.Controls.Add(celda2);
                renglon.Controls.Add(celda3);
                renglon.Controls.Add(celda4);
                renglon.Controls.Add(celda5);

                tblX.Rows.Add(renglon);
            }

        }

        public void LlenarTablaZ(List<ConceptoRespValor> Lconcepto)
        {
            TableRow renglon;

            lblPerZ.Text = Lconcepto.Sum(x => x.ValorRespuesta).Value.ToString("#0");

            foreach (ConceptoRespValor concepto in Lconcepto)
            {
                renglon = new TableRow();

                TableCell celda1 = new TableCell();
                TableCell celda2 = new TableCell();
                TableCell celda3 = new TableCell();
                TableCell celda4 = new TableCell();
                TableCell celda5 = new TableCell();

                celda1.Text = concepto.NumOrdenVisual;
                celda2.Text = concepto.DescripcionConcepto;
                celda3.Text = concepto.ValorPonderacionRespuesta.ToString();
                celda4.Text = concepto.ValorRespuesta == null ? "0" : concepto.ValorRespuesta.Value.ToString("#0");
                celda5.Text = ((concepto.ValorRespuesta == null ? 0 : concepto.ValorRespuesta.Value) * concepto.ValorPonderacionRespuesta.Value).ToString("#0.00");

                renglon.Controls.Add(celda1);
                renglon.Controls.Add(celda2);
                renglon.Controls.Add(celda3);
                renglon.Controls.Add(celda4);
                renglon.Controls.Add(celda5);

                tblZ.Rows.Add(renglon);
            }
        }

        private Boolean ObtenerInformacionAcuseFolioSMOI()
        {
            Boolean Ok = false;
            AcuseFolio objAcuseSMOIFolio;
            int intFolio = 0;
            bool ConversionOK; //esta nos dice si es un número válido
            try
            {
                ConversionOK = int.TryParse(Request.QueryString["IdFolio"].ToString(), out intFolio);
                if (ConversionOK)
                {
                    objAcuseSMOIFolio = new NGConceptoRespValor().ObtenerAcuseSMOI(intFolio);

                    string fecha = objAcuseSMOIFolio.FechaAutorizacion.ToString();

                    string[] nuevafecha = fecha.Split('/');

                    string[] ano = nuevafecha[2].Split(' ');

                    string dia = nuevafecha[0];

                    string mes = nuevafecha[1];

                    string year = ano[0];

                    //llamar por codebehind 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cambiarlogo", "CambiarLogo(" + '"' + dia + '"' + ',' + '"' + mes + '"' + ',' + '"' + year + '"' + ");", true);

                    if (objAcuseSMOIFolio != null)
                    {
                        //bajar los valores a los controles
                        this.LabelNoFolio.Text = objAcuseSMOIFolio.Folio.ToString();
                        this.LabelInstitucion.Text = objAcuseSMOIFolio.InstitucionSolicitante;
                        this.LabelFechaRegistro.Text = objAcuseSMOIFolio.FechaRegistro;
                        this.LabelHoraRegistro.Text = "Hora de registro: " + objAcuseSMOIFolio.HoraRegistro;
                        this.LabelCadenaOriginal.Text = objAcuseSMOIFolio.CadenaOriginal;
                        this.LabelSelloDigital.Text = objAcuseSMOIFolio.SelloDigital;
                        this.LabelInstitucion.Text = objAcuseSMOIFolio.InstitucionSolicitante;
                        this.LabelDeclaracionAnio.Text = objAcuseSMOIFolio.LeyendaAnio;

                        //RCA 10/08/2018
                        if(!string.IsNullOrEmpty(objAcuseSMOIFolio.QR))
                        {
                            this.LabelQR.Text = objAcuseSMOIFolio.QR;

                            this.LeyendaSMOI.Text = objAcuseSMOIFolio.LeyendaQR;

                            this.FechaAutorizacionSMOI.Text = "Fecha de autorización: " + string.Format("{0:MM/dd/yyyy}", objAcuseSMOIFolio.FechaAutorizacion);
                        }

                        //this.LabelTotalSMOIm2FactorX.Text = String.Format("Factor X = Superficie Máxima a Ocupar por Todos los Niveles: <strong>{0:N} </strong>", objAcuseSMOIFolio.TotalSMOIm2FactorX) + " <strong>m2</strong>";
                        LabelTotalSMOIm2FactorX.Text = objAcuseSMOIFolio.TotalSMOIm2FactorX.Value.ToString("#0.00");
                        factxLbl.Text = objAcuseSMOIFolio.TotalSMOIm2FactorX.Value.ToString("#0.00");
                        //this.LabelTotalSMOIm2FactorY.Text = String.Format("Factor Y = Áreas de Uso Común y Áreas de Circulación (FactorX * 0.44) : <strong>{0:N}</strong>", objAcuseSMOIFolio.TotalSMOIm2FactorY) + " <strong>m2</strong>";
                        //derivado
                        //this.LabelTotalSMOIm2FactorY.Text = String.Format("Factor Y = Áreas de Uso Común y Áreas de Circulación (FactorX * {0:N}): <strong>{1:N}</strong>", objAcuseSMOIFolio.FactorY_Calculado, objAcuseSMOIFolio.TotalSMOIm2FactorY_Derivado) + " <strong>m2</strong>";
                        LabelTotalSMOIm2FactorY.Text = (objAcuseSMOIFolio.TotalSMOIm2FactorY_Derivado).ToString("#0.00");
                        factyLbl.Text = (objAcuseSMOIFolio.TotalSMOIm2FactorY_Derivado).ToString("#0.00");

                        //this.LabelTotalSMOIm2FactorZ.Text = String.Format("Factor Z = Áreas Complementarias: <strong>{0:N}</strong>", objAcuseSMOIFolio.TotalSMOIm2FactorZ) + " <strong>m2</strong>";
                        LabelTotalSMOIm2FactorZ.Text = objAcuseSMOIFolio.TotalSMOIm2FactorZ.Value.ToString("#0.00");
                        factzLbl.Text = objAcuseSMOIFolio.TotalSMOIm2FactorZ.Value.ToString("#0.00");
                        //this.LabelTotalSMOIm2.Text = String.Format(" SMOI = Superficie Máxima a Ocupar por la Institución: <strong><u>{0:N}</u></strong>", objAcuseSMOIFolio.TotalSMOIm2) + " <strong>m2</strong>";
                        LabelTotalSMOIm2.Text = objAcuseSMOIFolio.TotalSMOIm2.Value.ToString("#0.00");
                        lblSmoi.Text = objAcuseSMOIFolio.TotalSMOIm2.Value.ToString("#0.00");

                        this.LabelIncumplimiento.Text = "<strong>Importante:</strong> El porcentaje de espacios complementarios  <strong><u>excede en un 50% al valor X</u></strong>, será meritorio de un análisis particular por parte del INDAABIN";
                        this.LabelIncumplimiento.Visible = !validaPorcentajesXZ(objAcuseSMOIFolio.TotalSMOIm2FactorX.Value, objAcuseSMOIFolio.TotalSMOIm2FactorZ.Value);

                        //Msj = "Este acuse está registrado para que pueda ser consultado las veces que se deseen por algún usuario con cuenta de acceso y que pertenezca a la institución a la que estás adscrito.";
                        //this.LabelInfo.Text = "<div class='alert alert-info'><strong>Sugerencia: </strong> " + Msj + "</div>";
                        //MostrarMensajeJavaScript(Msj);

                    }
                    else
                    {
                        Msj = "No fue posible exponer el acuse de folio SMOI, vuelve intentar o reporta al área de sistemas.";
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                    }
                }
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al exponer la información del acuse de tabla SMOI, del Folio:" + intFolio.ToString() + ". Contacta al área de sistemas.";
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

        private bool validaPorcentajesXZ(decimal pFactorX, decimal pFactorZ)
        {
            decimal v50per = 0;
            v50per = pFactorX * (0.5M);
            if(pFactorZ > v50per)
                return false;
            else 
                return true;       
        }
        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }


        protected void ButtonExportarPdf_Click(object sender, EventArgs e)
        {
            //try
            //{
            AcuseFolio objAcuseSMOIFolio;
            int intFolio = 0;
            bool ConversionOK;
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            page.EnableEventValidation = false;
            page.DesignerInitialize();
            form = this.form1;
            form.FindControl("ButtonExportarPdf").Visible = false;
            page.Controls.Add(form);
            page.RenderControl(htw);

            //string strCabecero ="<!DOCTYPE html> <html> <head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/><title></title><meta charset='utf-8' /></head><body>";
            string strCabecero = "<!DOCTYPE html> <html xmlns='http://www.w3.org/1999/xhtml'> <head runat='server'> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/> <title></title> <link href='https://framework-gb.cdn.gob.mx/assets/styles/main.css' rel='stylesheet'/> <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300' rel='stylesheet' type='text/css'/> <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon'/> <style> @media print { #ZonaNoImrpimible {display:none;} } nav,aside  { display: none; } .auto-style1 { height: 119px; } </style> <script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js'></script> </head> <body>";
            string strPie = "</body> </html>";

                   

            string strBotonExportar = "<asp:Button ID=\"ButtonExportarPdf\" runat=\"server\" CssClass=\"btn btn-default\" OnClick=\"ButtonExportarPdf_Click\" Text=\"Exportar a PDF\" ToolTip=\"Exportar a PDF.\" />";
            string strBotonImprimir = "<input type=\"button\" id=\"ButtonImprimir\" value=\"Imprimir\" onclick=\"javascript: window.print()\" class=\"btn\" />";
            string strBrakePage = "page-break-before: always;";
            
          
            string strCuerpoOriginal = sb.ToString();
            string strPaginaConCuerpo = strCabecero + strCuerpoOriginal + strPie;
            string strCuerpoFormateado = especialesHTML(strPaginaConCuerpo);


            //poner la url del nuevo logo si pasa del 1 de diciembre de 2018
            ConversionOK = int.TryParse(Request.QueryString["IdFolio"].ToString(), out intFolio);
            if (ConversionOK)
            {
                objAcuseSMOIFolio = new NGConceptoRespValor().ObtenerAcuseSMOI(intFolio);

                string fecha = objAcuseSMOIFolio.FechaAutorizacion.ToString();

                string[] nuevafecha = fecha.Split('/');

                string[] ano = nuevafecha[2].Split(' ');

                string dia = nuevafecha[0];

                string mes = nuevafecha[1];

                string year = ano[0];


                strCuerpoFormateado = strCuerpoFormateado.Replace("src=\"http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg\"", "src=\"https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG\"");

                strCuerpoFormateado = strCuerpoFormateado.Replace("url(http://sistemas.indaabin.gob.mx/ImagenesComunes/aguila.png);", "url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png);");

                strCuerpoFormateado = strCuerpoFormateado.Replace("background-position: center;", "background-position: left;");

                strCuerpoFormateado = strCuerpoFormateado.Replace("##font##", "font-family: Montserrat;");

                strCuerpoFormateado = strCuerpoFormateado.Replace("##Viejo##", "display:none;");

                if (Convert.ToInt32(year) >= 2018)
                {
                    if (Convert.ToInt32(mes) >= 12)
                    {
                        if (Convert.ToInt32(dia) >= 1)
                        {

                            strCuerpoFormateado = strCuerpoFormateado.Replace("src=\"http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg\"", "src=\"https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG\"");

                            strCuerpoFormateado = strCuerpoFormateado.Replace("url(http://sistemas.indaabin.gob.mx/ImagenesComunes/aguila.png);", "url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png);");

                            strCuerpoFormateado = strCuerpoFormateado.Replace("background-position: center;", "background-position: left;");

                            strCuerpoFormateado = strCuerpoFormateado.Replace("##font##", "font-family: Montserrat;");

                            strCuerpoFormateado = strCuerpoFormateado.Replace("##Viejo##", "display:none;");

                        }
                        else
                        {
                            strCuerpoFormateado = strCuerpoFormateado.Replace("##Nuevo##", "display:none;");
                        }
                    }
                    else
                    {
                        strCuerpoFormateado = strCuerpoFormateado.Replace("##Nuevo##", "display:none;");
                    }
                }
                else
                {
                    strCuerpoFormateado = strCuerpoFormateado.Replace("##Nuevo##", "display:none;");
                }
            }



            ExportHTML exportar = new ExportHTML();
            exportar.CanPrint = true;
            
            //RCA 10/08/2017
            exportar.GenerarPDF(strCuerpoFormateado);
          


            
        }

        private string especialesHTML(string cuerpo)
        {
            string str = cuerpo;
            //str = str.Replace("<","&lt;");
            //str = str.Replace(">","&gt;");
            //str = str.Replace("&","&amp;");
            str = str.Replace("¡", "&iexcl;");
            //str = str.Replace("\"","&quot;");
            str = str.Replace("¿", "&iquest;");
            str = str.Replace("®", "&reg;");
            str = str.Replace("©", "&copy;");
            str = str.Replace("€", "&euro;");
            str = str.Replace("á", "&aacute;");
            str = str.Replace("é", "&eacute;");
            str = str.Replace("í", "&iacute;");
            str = str.Replace("ó", "&oacute;");
            str = str.Replace("ú", "&uacute;");
            str = str.Replace("ñ", "&ntilde;");
            str = str.Replace("ü", "&uuml;");
            str = str.Replace("Á", "&Aacute;");
            str = str.Replace("É", "&Eacute;");
            str = str.Replace("Í", "&Iacute;");
            str = str.Replace("Ó", "&Oacute;");
            str = str.Replace("Ú", "&Uacute;");
            str = str.Replace("Ñ", "&Ntilde;");
            str = str.Replace("Ü", "&Uuml;");
            return str;
        }


    } //clase
}