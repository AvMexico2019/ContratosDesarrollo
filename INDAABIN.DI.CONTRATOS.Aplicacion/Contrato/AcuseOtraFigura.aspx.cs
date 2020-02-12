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

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Contrato
{
    public partial class AcuseOtraFigura : System.Web.UI.Page
    {
        String Msj;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = string.Empty;

            if (!IsPostBack)
            {
                if (Request.QueryString["IdFolio"] != null)
                {
                    //Session["FolioContrato"] = Request.QueryString["IdFolio"];
                    this.ObtenerInformacionAcuseFolioContrato();
                }
            }
        }

        private Boolean ObtenerInformacionAcuseFolioContrato()
        {
            Boolean Ok = false;
            ModeloNegocios.AcuseContrato objAcuseContrato;
            int intFolio;
            bool ConversionOK; //esta nos dice si es un número válido
            ConversionOK = int.TryParse(Request.QueryString["IdFolio"], out intFolio);
            if (ConversionOK)
            {
                try
                {
                    objAcuseContrato = new NG_ContratoArrto().ObtenerAcuseContrato(intFolio);


                    string fecha = objAcuseContrato.FechaAutorizacion.ToString();

                    string[] nuevafecha = fecha.Split('/');

                    string[] ano = nuevafecha[2].Split(' ');

                    string dia = nuevafecha[0];

                    string mes = nuevafecha[1];

                    string year = ano[0];

                    //llamar por codebehind 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cambiarlogo", "CambiarLogo(" + '"' + dia + '"' + ',' + '"' + mes + '"' + ',' + '"' + year + '"' + ");", true);

                    if (objAcuseContrato != null)
                    {
                        //bajar los valores a los controles
                        this.LabelNoFolio.Text = objAcuseContrato.Folio.ToString();

                        this.LabelFechaRegistro.Text = objAcuseContrato.FechaRegistro;
                        this.LabelHoraRegistro.Text = "<strong>Hora de registro:</strong> " + objAcuseContrato.HoraRegistro;
                        this.LabelCadenaOriginal.Text = objAcuseContrato.CadenaOriginal;
                        this.LabelSelloDigital.Text = objAcuseContrato.SelloDigital;
                        this.LabelInstitucion.Text = "<strong>Institución Pública: </strong>" + objAcuseContrato.InstitucionSolicitante;
                        //etiquetas en el cuerpo del Acuse
                        this.LabelTipoContrato.Text = "<strong>Tipo de Contrato:</strong> " + objAcuseContrato.ContratoArrto.DescripcionTipoContrato; //Nac, Ext o OtrasFigOcupacion
                        this.LabelDeclaracionAnio.Text = objAcuseContrato.LeyendaAnio;

                        //RCA 10/08/2018
                        if (!string.IsNullOrEmpty(objAcuseContrato.QR))
                        {
                            this.LabelQR.Text = objAcuseContrato.QR;

                            this.LeyendaContrato.Text = objAcuseContrato.Leyenda;

                            this.FechaAutorizacionContrato.Text = "Fecha de autorización: " + string.Format("{0:MM/dd/yyyy}", objAcuseContrato.FechaAutorizacion);
                        }

                        //si el tipo de contrato es de Otras Fig. Ocupacion
                        if (objAcuseContrato.ContratoArrto.DescripcionTipoContrato == "Otras Figuras de Ocupación" || objAcuseContrato.ContratoArrto.DescripcionTipoContrato == "Otras Figuras de Ocupación en el Extranjero")
                        {
                            this.LabelTipoOcupacion.Visible = true;
                            this.LabelTipoArrendamiento.Visible = false; //no aplica a otras Fig. ocupacion
                            this.LabelTipoContratacion.Visible = false; //no aplica a otras Fig. ocupacion

                            //valores de referencia del objeto de contrato
                            if (objAcuseContrato.ContratoArrto.DescripcionTipoOcupacion != "Otros")
                                this.LabelTipoOcupacion.Text = "<strong>Tipo de Ocupación: </strong>" + objAcuseContrato.ContratoArrto.DescripcionTipoOcupacion;
                            else
                                this.LabelTipoOcupacion.Text = "<strong>Tipo de Ocupación (otros): </strong>" + objAcuseContrato.ContratoArrto.OtroTipoOcupacion;
                        }
                        else //es Arrto: Nacional o Extranjero
                        {
                            this.LabelTipoOcupacion.Visible = false; //este cpto no aplica  a Otras Fig. Ocupacion

                            this.LabelTipoArrendamiento.Visible = true; //Nuevo, Continuacion, Sustitucion
                            this.LabelTipoArrendamiento.Text = "<strong>Tipo de Arrendamiento: </strong>" + objAcuseContrato.ContratoArrto.DescripcionTipoArrendamiento;

                            this.LabelTipoContratacion.Visible = true; //automatico, dictaminado..
                            this.LabelTipoContratacion.Text = "<strong>Tipo de Contratación: </strong>" + objAcuseContrato.ContratoArrto.DescripcionTipoContratacion;

                            if (objAcuseContrato.ContratoArrto.DescripcionTipoContrato == "Nacional")
                            {
                                if (objAcuseContrato.JustripreciacionContrato.Secuencial != null)
                                    this.LabelSecuencialJust.Text = "<strong>Secuencial de Justipreciación:</strong> " + objAcuseContrato.JustripreciacionContrato.Secuencial;
                                else
                                    this.LabelSecuencialJust.Text = "<strong>Secuencial de Justipreciación:</strong> --";

                                if (objAcuseContrato.ContratoArrto.FolioEmisionOpinion != null)
                                {
                                    this.LabelFolioOpinion.Text = "<strong>Folio de Emisión de Opinión:</strong> " + objAcuseContrato.ContratoArrto.FolioEmisionOpinion;
                                    this.LabelFolioSAEF.Text = "<strong>Folio de accesibilidad:</strong> " + objAcuseContrato.FolioSAEF;
                                }
                                else
                                {
                                    this.LabelFolioOpinion.Text = "<strong>Folio de Emisión de Opinión:</strong> --";
                                }
                                    

                                this.LabelPropietarioInmueble.Text = "<strong>Propietario: </strong>" + objAcuseContrato.ContratoArrto.PropietarioInmueble;
                                this.LabelFuncionarioResponsable.Text = "<strong>Funcionario responsable: </strong>" + objAcuseContrato.ContratoArrto.FuncionarioResponsable;
                            }
                        }

                        this.LabelCadenaOriginal.Text = objAcuseContrato.CadenaOriginal;
                        this.LabelSelloDigital.Text = objAcuseContrato.SelloDigital;

                        this.LabelFechaInicioOcupacion.Text = "<strong>Fecha Inicio de Ocupación:</strong> " + objAcuseContrato.ContratoArrto.FechaInicioOcupacion.ToShortDateString();
                        this.LabelFechaFinOcupacion.Text = "<strong>Fecha Fin de Ocupación: </strong>" + objAcuseContrato.ContratoArrto.FechaFinOcupacion.ToShortDateString();

                        this.labelAreaOcupadaM2.Text = String.Format("<strong>Área Ocupada :</strong> {0:N} ", objAcuseContrato.ContratoArrto.AreaOcupadaM2) + "m2";
                        this.LabelMontoPagoMensual.Text = String.Format("<strong>Monto Pago Mensual:</strong> {0:C}", +objAcuseContrato.ContratoArrto.PagoTotalCptosRenta);

                        //this.LabelDirInmueble.Text = objAcuseContrato.InmuebleArrto.DireccionCompleta;
                        //if (Session["DireccionInmueble"] != null)
                        //    this.LabelDirInmueble.Text = "<strong>Dirección del Inmueble: </strong>" + Session["DireccionInmueble"].ToString();
                        //else
                        //    this.LabelDirInmueble.Text = "Sin dirección, vuelva a recargar la pagina, para exponerlo";


                        if (objAcuseContrato.ContratoArrto.InmuebleArrto != null)
                            this.LabelDirInmueble.Text = "<strong>Dirección del Inmueble: </strong>" + objAcuseContrato.ContratoArrto.InmuebleArrto.DireccionCompleta;
                        else
                            this.LabelDirInmueble.Text = "Sin dirección, vuelva a recargar la pagina, para exponerlo";

                        this.LabelRIUF.Text = "<strong>RIUF: </strong>" + objAcuseContrato.ContratoArrto.RIUF;

                        //Msj = "Este acuse está registrado para que pueda ser consultado las veces que se deseen por algún usuario con cuenta de acceso y que pertenezca a la institución a la que estás adscrito.";
                        //this.LabelInfo.Text = "<div class='alert alert-info'><strong>Sugerencia: </strong> " + Msj + "</div>";
                    }
                    else
                    {
                        Msj = "No fue posible exponer el acuse de registro del contrato, vuelve intentar o reporta al área de sistemas. ";
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                    }
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al exponer la información del acuse del contrato con Folio:" + intFolio.ToString() + ". Contacta al área de sistemas.";
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
            return Ok;
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void ButtonExportarPdf_Click(object sender, EventArgs e)
        {
            ModeloNegocios.AcuseContrato objAcuseContrato;
            int intFolio;
            bool ConversionOK; //esta nos dice si es un número válido
            ConversionOK = int.TryParse(Request.QueryString["IdFolio"], out intFolio);

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

               
                string strCabecero = "<!DOCTYPE html> <html xmlns='http://www.w3.org/1999/xhtml'> <head runat='server'> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/> <title></title> <link href='https://framework-gb.cdn.gob.mx/assets/styles/main.css' rel='stylesheet'/> <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300' rel='stylesheet' type='text/css'/> <link href='https://framework-gb.cdn.gob.mx/favicon.ico' rel='shortcut icon'/> <style> @media print { #ZonaNoImrpimible {display:none;} } nav,aside  { display: none; } .auto-style1 { height: 119px; } </style> <script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js'></script> </head> <body>";
                string strPie = "</body> </html>";

              
                string strBotonExportar = "<asp:Button ID=\"ButtonExportarPdf\" runat=\"server\" CssClass=\"btn btn-default\" OnClick=\"ButtonExportarPdf_Click\" Text=\"Exportar a PDF\" ToolTip=\"Exportar a PDF.\" />";
                string strBotonImprimir = "<input type=\"button\" id=\"ButtonImprimir\" value=\"Imprimir\" onclick=\"javascript: window.print()\" class=\"btn\" />";

                //RCA 10/08/2017
               
                string strImagenLogoIndaabinHtml = "url('http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg')";
                string strImagenLogoIndaabinFisica = "../Imagenes/logoindaabin.jpg";

                string strBrakePage = "page-break-before: always;";

                //RCA 10/08/2017
                //cambio de ruta a rutas relativas
                
                string strImagenEscudoNacionalHtml = "url('http://sistemas.indaabin.gob.mx/ImagenesComunes/aguila.png')";
                string strImagenEscudoNacionalFisica = "../Imagenes/EscudoNacional.png";

                string strCuerpoOriginal = sb.ToString();
                string strPaginaConCuerpo = strCabecero + strCuerpoOriginal + strPie;
                string strCuerpoFormateado = especialesHTML(strPaginaConCuerpo).Replace(strImagenLogoIndaabinHtml, strImagenLogoIndaabinFisica).Replace(strImagenEscudoNacionalHtml, strImagenEscudoNacionalFisica).Replace(strBotonImprimir, "").Replace(strBotonExportar, "").Replace(strBrakePage, "");

            //cambiamos formato si pasa del 1/12/2018
            //poner la url del nuevo logo si pasa del 1 de diciembre de 2018
            if (ConversionOK)
            {
                objAcuseContrato = new NG_ContratoArrto().ObtenerAcuseContrato(intFolio);

                string fecha = objAcuseContrato.FechaAutorizacion.ToString();

                string[] nuevafecha = fecha.Split('/');

                string[] ano = nuevafecha[2].Split(' ');

                string dia = nuevafecha[0];

                string mes = nuevafecha[1];

                string year = ano[0];


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

                //RCA 10/18/2017
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
    }
}