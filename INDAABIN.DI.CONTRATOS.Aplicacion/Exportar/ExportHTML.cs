using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using ExpertPdf.HtmlToPdf;
using System.Drawing;
using System.Web.UI;

using itext = iTextSharp.text;
using itextpdf = iTextSharp.text.pdf;



namespace INDAABIN.DI.CONTRATOS.Aplicacion.Exportar
{
    public class ExportHTML
    {
        #region Parámetros de PDF EXCEL y WORD

        public string TituloDocumento
        {
            get
            {
                return "Exportación";
            }
        }
        public string Titulo
        {
            get
            {
                return "Exportación";
            }
        }
        public string Subtitulo
        {
            get
            {
                return "Subtitulo";
            }
        }
        public string Cabecera
        {
            get
            {
                //return "C & A Systems";
                return "";
            }
        }
        public string FondoTitulo
        {
            get
            {
                return "0000FF";
            }
        }
        public string LetraTitulo
        {
            get
            {
                return "FFFFFF";
            }
        }
        public string ColorElemento
        {
            get
            {
                return "EFF6F4";
            }
        }
        public string ColorAlternativo
        {
            get
            {
                return "F2D0B3";
            }
        }
        public string NombreArchivo
        {
            get
            {
                return "Exportacion.PDF";
            }
        }
        #endregion

        #region Propiedades
        public string RutaImagenEncabezado { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
        public bool ShowPageNumber { get; set; }
        public bool DrawHeaderLine { get; set; }
        public bool DrawFooterLine { get; set; }
        public string HeaderText { get; set; }
        public int HeaderHeight { get; set; }
        public HorizontalTextAlign HeaderTextAlign
        {
            get
            {
                return HorizontalTextAlign.Center;
            }
        }
        public string PageNumberingFormat { get; set; }
        public string FooterText { get; set; }
        public string FooterFontType { get; set; }
        public int FooterFontSize { get; set; }
        public bool CanPrint { get; set; }
        #endregion


        //RCA 10/08/2017
        //se cambio el metodo para que recibiera byte y asi no tenga que guardar el archivo en la carpeta de temporales.
        //solo recibe el parametro del cuerpo
        public byte[] GenerarPDF(string Cuerpo)
        //public String GenerarPDF(string Cuerpo, string RutaBase)
        {
            PdfConverter pdfConverter = new PdfConverter();

            //RCA 10/08/2017
            byte[] bPdf = null;

            string error = string.Empty;

            try
            {
                //CARACTERISTICAS Y FORMATO DE ARCHIVO PDF
                pdfConverter.LicenseKey = "f1ROX0dfTk9OTl9KUU9fTE5RTk1RRkZGRg==";

                //RCA 10/08/2017
                //para que lo que se muestre el texto del pdf
                pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;

                pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
                //pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
                //pdfConverter.PdfDocumentOptions.ShowHeader = true;
                //pdfConverter.PdfDocumentOptions.ShowFooter = true;
                pdfConverter.PdfDocumentOptions.BottomMargin = 25;
                pdfConverter.PdfDocumentOptions.TopMargin = 5;
                pdfConverter.PdfDocumentOptions.RightMargin = 50;
                pdfConverter.PdfDocumentOptions.LeftMargin = 50;
                pdfConverter.PdfDocumentOptions.StretchToFit = true;
                //FORMATO Y TEXTO PARA ENCABEZADO DE PÁGINA
                //pdfConverter.PdfSecurityOptions.CanCopyContent = true;
                //pdfConverter.PdfHeaderOptions.HeaderTextFontSize = 6;
                //pdfConverter.PdfFooterOptions.FooterTextFontSize = 6;
                //pdfConverter.PdfHeaderOptions.HeaderHeight = 120;
                //pdfConverter.PdfHeaderOptions.HeaderTextYLocation = 81;
                //pdfConverter.PdfFooterOptions.ShowPageNumber = true;
                //pdfConverter.PdfHeaderOptions.ShowOnEvenPages = true;
                //pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;
                //pdfConverter.PdfFooterOptions.DrawFooterLine = false;
                //pdfConverter.PdfFooterOptions.PageNumberingStartIndex = 0;
                //pdfConverter.PdfFooterOptions.PageNumberingFormatString = "Dictamen Pág. &p; de &P;";
                //pdfConverter.PdfSecurityOptions.CanPrint = true;

                //Se borran los archivos del dia anterior
                /*Directory.GetFiles(RutaBase + @"ArchivosTemporales\")
                 .Select(f => new FileInfo(f))
                 .Where(f => f.LastAccessTime < DateTime.Now.AddDays(-1))
                 .ToList()
                 .ForEach(f => f.Delete());*/

                //string rutaArchivo = RutaBase + @"ArchivosTemporales\" + Path.GetFileName(System.IO.Path.GetRandomFileName());

                //RCA 10/08/2017
                //usar el metodo gettempfile sirve para Crea un archivo temporal de cero bytes y nombre único en el disco y devuelve la ruta de acceso completa a ese archivo.
                //string rutaArchivo = RutaBase + @"ArchivosTemporales\" + Path.GetFileName(System.IO.Path.GetTempFileName());

                //RCA 10/08/2017
                bPdf = pdfConverter.GetPdfBytesFromHtmlString(Cuerpo);

                //pdfConverter.SavePdfFromHtmlStringToFile(Cuerpo, rutaArchivo);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=Acuse.pdf");
                //HttpContext.Current.Response.TransmitFile(rutaArchivo);

                //RCA 10/08/2017
                HttpContext.Current.Response.BinaryWrite(bPdf);
                HttpContext.Current.Response.Flush();

                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                error = "No se puedo generar el archivo PDF debido a: " + ex.ToString();
            }

            //RCA 10/08/2017
            pdfConverter = null;
            return bPdf;

            //return error;
        }

        private string FormatoHTML(string Texto)
        {
            string strhtml = HttpUtility.HtmlEncode(Texto);
            strhtml = strhtml.Replace("\r\n", "\r");
            strhtml = strhtml.Replace("\n", "\r");
            strhtml = strhtml.Replace("\r", "<br>\r\n");
            strhtml = strhtml.Replace("  ", " &nbsp;");
            return strhtml;
        }
    }

}
