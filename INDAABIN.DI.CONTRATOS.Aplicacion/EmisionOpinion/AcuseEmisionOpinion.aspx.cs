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

namespace INDAABIN.DI.CONTRATOS.Aplicacion.EmisionOpinion
{
    public partial class AcuseEmisionOpinion1 : System.Web.UI.Page
    {
        string strTipoArrendamiento;
        string Msj;
              

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LabelInfo.Text = string.Empty;

            if (!IsPostBack)
            {
                if (Request.QueryString["IdFolio"] != "" && Request.QueryString["TipoArrto"] != "")
                {
                    //Session["intFolioConceptoResp"] = Request.QueryString["IdFolio"].ToString();
                    strTipoArrendamiento = Request.QueryString["TipoArrto"];
                    this.ObtenerInformacionAcuseFolioEmisionOpinion();
                }
                else
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('No has seleccionado un folio valido'); window.close();", true);
                }                    
            }
        }
        
        private Boolean ObtenerInformacionAcuseFolioEmisionOpinion()
        {
            Boolean Ok = false;
            AcuseFolio objAcuseOpinionFolio;
            int intFolio;
            bool ConversionOK; //esta nos dice si es un número válido

            ConversionOK = int.TryParse(Request.QueryString["IdFolio"].ToString(), out intFolio);

            if (ConversionOK)
            {
                try
                {
                    objAcuseOpinionFolio = new NGConceptoRespValor().ObtenerAcuseSolicitudOpinionConInmueble(intFolio, strTipoArrendamiento);

                    string fecha = objAcuseOpinionFolio.FechaAutorizacion.ToString();

                    string[] nuevafecha = fecha.Split('/');

                    string[] ano = nuevafecha[2].Split(' ');

                    string dia = nuevafecha[0];

                    string mes = nuevafecha[1];

                    string year = ano[0];

                    //llamar por codebehind 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cambiarlogo", "CambiarLogo(" + '"' + dia + '"' + ',' + '"' + mes + '"' + ',' + '"' + year + '"' + ");", true);


                    if (objAcuseOpinionFolio != null)
                    {
                        //bajar los valores a los controles
                        LabelNoFolio.Text = objAcuseOpinionFolio.Folio.ToString();
                        this.LabelTipoContrato.Text = objAcuseOpinionFolio.TipoArrendamientoDesc.ToUpper();
                        this.LabelTextoRespuesta.Text = objAcuseOpinionFolio.ResultadoAplicacionOpinion.ToString();
                        this.LabelInstitucion.Text = objAcuseOpinionFolio.InstitucionSolicitante;
                        this.LabelFechaRegistro.Text = objAcuseOpinionFolio.FechaRegistro;
                        this.LabelHoraRegistro.Text = "HORA DE REGISTRO: " + objAcuseOpinionFolio.HoraRegistro;
                        this.LabelCadenaOriginal.Text = objAcuseOpinionFolio.CadenaOriginal;
                        this.LabelSelloDigital.Text = objAcuseOpinionFolio.SelloDigital;
                        this.LabelInstitucion.Text = objAcuseOpinionFolio.InstitucionSolicitante;
                        this.LabelDeclaracionAnio.Text = objAcuseOpinionFolio.LeyendaAnio;

                        //RCA 10/08/2018
                        if (!string.IsNullOrEmpty(objAcuseOpinionFolio.QR))
                        {
                            this.LabelQR.Text = objAcuseOpinionFolio.QR;

                            this.LeyendaEmision.Text = objAcuseOpinionFolio.LeyendaQR;

                            this.FechaAutorizacionEmision.Text = "Fecha de autorización: " + string.Format("{0:MM/dd/yyyy}", objAcuseOpinionFolio.FechaAutorizacion);
                        }

                        if (LabelPais.Text.Length == 0) //no se cargo la direccion del inmueble con los valores de la session, este metodo lo tiene, pero vienen por Id, hay que obtener del bus
                        {
                            //calle, av....
                            String TipoVialidad = AdministradorCatalogos.ObtenerNombreTipoVialidad(objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdTipoVialidad);

                            //exponer propiedades de la direccion del inmueble para el que se hizo Solicitud de emisión de opinión
                            if (objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NumInterior == null)
                                this.LabelDirVialidadYNums.Text =TipoVialidad + ": " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NombreTipoVialidad + " " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NombreVialidad + ", NÚMERO EXTERIOR: " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NumExterior;
                            else
                                this.LabelDirVialidadYNums.Text = TipoVialidad + ": " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NombreVialidad + ", NÚMERO EXTERIOR: " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NumExterior + ", NÚMERO INTERIOR: " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.NumInterior;

                            //recuperar valores del inmueble asociado a la solicitud de emision
                            //algunos son Id, se deberan obtener los correspondientes nombres vs DB_CAT a traves del BUS
                            String Pais = AdministradorCatalogos.ObtenerNombrePais(objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdPais);
                            this.LabelPais.Text = "PAÍS: " + Pais;

                            if (Pais.ToUpper() == "MÉXICO")
                            {
                                String Colonia;
                                //llamado a metodo estatico
                                if (objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdLocalidadColonia != null)
                                    Colonia = AdministradorCatalogos.ObtenerNombreLocalidad(objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdPais, objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdEstado.Value, objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdMunicipio.Value, objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdLocalidadColonia.Value);
                                else //es colonia escrita por el usuario
                                    Colonia = objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.OtraColonia;
                                LabelColonia.Text = "COLONIA: " + Colonia;

                                String Mpo;
                                //llamado a metodo estatico
                                Mpo = AdministradorCatalogos.ObtenerNombreMunicipio(objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdEstado.Value, objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdMunicipio.Value);
                                this.LabelMpo.Text = "MUNICIPIO: " + Mpo;


                                String Edo = AdministradorCatalogos.ObtenerNombreEstado(objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.IdEstado.Value);
                                LabelEntFedyCP.Text = "ENTIDAD FEDERATIVA: " + Edo + ", código postal: " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.CodigoPostal;
                            }
                            else //direccion en extranjero 
                            {
                                LabelColonia.Text = "CIUDAD Ó EQUIVALENTE: " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.CiudadExtranjero;
                                LabelMpo.Text = "MUNICIPIO Ó EQUIVALENTE: " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.MunicipioExtranjero;
                                LabelEntFedyCP.Text = "ESTADO Ó EQUIVALENTE: "+ objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.EstadoExtranjero + " " + objAcuseOpinionFolio.InmuebleArrtoEmisionOpinion.CodigoPostalExtranjero;
                            }
                            //Msj = "Este acuse está registrado para que pueda ser consultado las veces que se deseen por algún usuario con cuenta de acceso y que pertenezca a la institución a la que estás adscrito.";
                            //this.LabelInfo.Text = "<div class='alert alert-info'><strong>Sugerencia: </strong> " + Msj + "</div>";
                            //MostrarMensajeJavaScript(Msj);                                                      
                        }
                        else
                        {
                            Msj =  "No hay información del país, digite F5 para volver a cargar el acuse o reporte a Sistemas ";
                            this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);                            
                        }                       
                    }
                    else
                    {
                        Msj = "No fue posible exponer el acuse de emisión de opinión, vuelve intentar o reporta al área de sistemas. ";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);                       
                    }                       
                }
                catch (Exception ex)
                {
                    Msj = "Ha ocurrido un error al exponer la información del acuse de la emisión de opinión con Folio:" + intFolio.ToString() + ". Contacta al área de sistemas.";
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
            AcuseFolio objAcuseOpinionFolio;
            strTipoArrendamiento = Request.QueryString["TipoArrto"];
            int intFolio;
            bool ConversionOK; //esta nos dice si es un número válido

            try
            {
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
                //string strCuerpoFormateado = especialesHTML(strPaginaConCuerpo).Replace(strImagenLogoIndaabinHtml, strImagenLogoIndaabinFisica).Replace(strImagenEscudoNacionalHtml, strImagenEscudoNacionalFisica).Replace(strBotonImprimir, "").Replace(strBotonExportar, "").Replace(strBrakePage, "");
                string strCuerpoFormateado = especialesHTML(strPaginaConCuerpo);

                //poner la url del nuevo logo si pasa del 1 de diciembre de 2018
                ConversionOK = int.TryParse(Request.QueryString["IdFolio"].ToString(), out intFolio);
                if (ConversionOK)
                {
                  objAcuseOpinionFolio = new NGConceptoRespValor().ObtenerAcuseSolicitudOpinionConInmueble(intFolio, strTipoArrendamiento);

                    string fecha = objAcuseOpinionFolio.FechaAutorizacion.ToString();

                    string[] nuevafecha = fecha.Split('/');

                    string[] ano = nuevafecha[2].Split(' ');

                    string dia = nuevafecha[0];

                    string mes = nuevafecha[1];

                    string year = ano[0];

                    strCuerpoFormateado = strCuerpoFormateado.Replace("src=\"http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg\"", "src=\"https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG");

                    strCuerpoFormateado = strCuerpoFormateado.Replace("url(http://sistemas.indaabin.gob.mx/ImagenesComunes/aguila.png);", "url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png);");

                    strCuerpoFormateado = strCuerpoFormateado.Replace("background-position: center;", "background-position: left;");

                    strCuerpoFormateado = strCuerpoFormateado.Replace("##font##", "font-family: Montserrat;");

                    strCuerpoFormateado = strCuerpoFormateado.Replace("##Viejo##", "display:none;");

                    //if (Convert.ToInt32(year) >= 2018)
                    //{
                    //    if (Convert.ToInt32(mes) >= 12)
                    //    {
                    //        if (Convert.ToInt32(dia) >= 1)
                    //        {

                    //            strCuerpoFormateado = strCuerpoFormateado.Replace("src=\"http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg\"", "src=\"https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG");

                    //            strCuerpoFormateado = strCuerpoFormateado.Replace("url(http://sistemas.indaabin.gob.mx/ImagenesComunes/aguila.png);", "url(https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png);");

                    //            strCuerpoFormateado = strCuerpoFormateado.Replace("background-position: center;", "background-position: left;");

                    //            strCuerpoFormateado = strCuerpoFormateado.Replace("##font##", "font-family: Montserrat;");

                    //            strCuerpoFormateado = strCuerpoFormateado.Replace("##Viejo##", "display:none;");
                    //        }
                    //        else
                    //        {
                    //            strCuerpoFormateado = strCuerpoFormateado.Replace("##Nuevo##", "display:none;");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        strCuerpoFormateado = strCuerpoFormateado.Replace("##Nuevo##", "display:none;");
                    //    }
                    //}
                    //else
                    //{
                    //    strCuerpoFormateado = strCuerpoFormateado.Replace("##Nuevo##", "display:none;");
                    //}
                }



                ExportHTML exportar = new ExportHTML();
                exportar.CanPrint = true;

                //RCA 10/18/2017
                exportar.GenerarPDF(strCuerpoFormateado);
                //exportar.GenerarPDF(strCuerpoFormateado, Server.MapPath("~"));
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al exportar a PDF. Contacta al área de sistemas.";
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