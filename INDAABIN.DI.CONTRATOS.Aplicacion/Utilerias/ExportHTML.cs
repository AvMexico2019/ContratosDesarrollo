using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Configuration;
using ExpertPdf.HtmlToPdf;
using System.Drawing;
using System.Web.UI;




using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.Negocio;
using iTextSharp.text.pdf;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias
{
    public class ExportHTML
    {
        string Header = string.Empty;
        string Footer = string.Empty;
        string Cuerpo = string.Empty;
 
        //obtenemos todo el html para generar el acuse
        public void CuerpoCompletoPlantillaSAEF(string FolioEmision, int? IdAplicacionConcepto)
        {
            byte[] ok = null;

            AcuseHeaderSAEF ObjHeaderSAEF = new AcuseHeaderSAEF();
            List<AcuseCuerpoSAEF> ListCuerpoSAEF = null;
            int? IdApliCOnceptoEmision = 0;
            bool Nuevo = false;
            

            string Path = ConfigurationManager.AppSettings["RutaDocs"];

            //validamos si viene el idaplicacionconcepto
            if(!string.IsNullOrEmpty(FolioEmision))
            {
                //buscamos por folio de emision y optenemos el idaplicacionconcepto
                EmisionOpinionSAEF ObjEmisonSAEF = null;

                ObjEmisonSAEF = new NG_SAEF().ObtenerEmisionSAEF(FolioEmision);

                IdApliCOnceptoEmision = ObjEmisonSAEF.IdAplicacionConcepto;
            }
            else
            {
                IdApliCOnceptoEmision = IdAplicacionConcepto;
            }

            //traemos los datos de la plantilla, cabezera y pie de pagina
            Header = new NG_InmuebleArrto().Encabezado();
            Footer = new NG_InmuebleArrto().PiePagina();
            Cuerpo = new NG_InmuebleArrto().CuerpoSAEF();


            //obtenemos la informacion del header del acuse asi como el sello y cadena
            ObjHeaderSAEF = new NG_SAEF().ObtenerAcuseHeaderSAEF(IdApliCOnceptoEmision);

            //obtenemos la lista de los valores de los indicadores
            ListCuerpoSAEF = new NG_SAEF().ObtenerAcuseCuerpoSAEF(IdApliCOnceptoEmision);

            //obtenemos la leyenda QR
            string LeyendaQR = new NG_SAEF().ObtenerLeyendaQR();

            //realizamos el replaza del logo y del tipo de letra cuando sea la fecha de registro despues del 1/12/2018
            string fecha = ObjHeaderSAEF.FechaRegistro.ToString();


            string[] nuevafecha = fecha.Split('/');

            string[] ano = nuevafecha[2].Split(' ');

            string dia = nuevafecha[0];

            string mes = nuevafecha[1];

            string year = ano[0];

            Header = Header.Replace("##Viejo##", "display:none;");

            Cuerpo = Cuerpo.Replace("##renglom##", "td{font-family: Montserrat;}");
            Cuerpo = Cuerpo.Replace("##parrafo##", "p{font-family: Montserrat;}");

            //if (Convert.ToInt32(year) >= 2018)
            //{
            //    if (Convert.ToInt32(mes) >= 12)
            //    {
            //        if (Convert.ToInt32(dia) >= 1)
            //        {

            //            //Header = Header.Replace("src=\"http://sistemas.indaabin.gob.mx/ImagenesComunes/INDAABIN_01.jpg\"", "src=\"https://sistemas.indaabin.gob.mx/ImagenesComunes/SHCP-INDAABINREDUCIDO.PNG\"");

            //            //Header = Header.Replace("##letra##", "NuvaLetra");

            //            //Header = Header.Replace("##tamano##", "style=\"height: 119px; width: 474px;\"");

            //            Header = Header.Replace("##Viejo##", "display:none;");

            //            Cuerpo = Cuerpo.Replace("##renglom##", "td{font-family: Montserrat;}");
            //            Cuerpo = Cuerpo.Replace("##parrafo##", "p{font-family: Montserrat;}");

            //            //Footer = Footer.Replace("##estilo##", "font-family: Montserrat;");

            //            Footer = Footer.Replace("##estilo##", "display:none;");



            //            Nuevo = true;
            //        }
            //        else
            //        {
            //            Header = Header.Replace("##Nuevo##", "display:none;");
            //        }
            //    }
            //    else
            //    {
            //        Header = Header.Replace("##Nuevo##", "display:none;");
            //    }
            //}
            //else
            //{
            //    Header = Header.Replace("##Nuevo##", "display:none;");
            //}

            //armamos el cuerpo del acuse con html

            //reemplazamos el header
            Header = Header.Replace("##rutadocs##",Path);
            Header = Header.Replace("##FechaRegistro##", string.Format("{0:MM/dd/yyyy}",ObjHeaderSAEF.FechaRegistro));
            Header = Header.Replace("##FechaImpr##", string.Format("{0:MM/dd/yyyy}",DateTime.Today));
            Header = Header.Replace("##NombreDepende##", ObjHeaderSAEF.NombreInstitucion);
            Header = Header.Replace("##UA##", ObjHeaderSAEF.NombreInstitucion);
            Header = Header.Replace("##NombreInmueble##", ObjHeaderSAEF.NombreInmueble);
            Header = Header.Replace("##FolioSAEF##", ObjHeaderSAEF.FolioSAEF);
            Header = Header.Replace("##RIUF##", ObjHeaderSAEF.RIUF);


            //empezamos a pon er los replace al cuerpo del HTML del SAEF

            #region indicador 1

            List<AcuseCuerpoSAEF> indicador1 = ListCuerpoSAEF.Where(x => x.IdIndicador == 1).Select(x => new AcuseCuerpoSAEF { Cumplimiento = x.Cumplimiento,Aplica = x.Aplica, Cumple = x.Cumple ,Observaciones = x.Observaciones, IdConceptoAccesibilidad = x.IdConceptoAccesibilidad}).ToList();

            int? renglon11 = 0;
            int? renglon12 = 0;
            int? renglon13 = 0;
            int? renglon14 = 0;
            int? renglon15 = 0;
            int? renglon16 = 0;
            int? renglon17 = 0;

            decimal? SumaIndicador1 = 0;
            decimal? EquivalenteIndicado1 = 0;
            

            foreach(AcuseCuerpoSAEF ObjIndicador1 in indicador1)
            {
                

                if(ObjIndicador1.IdConceptoAccesibilidad == 33)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd11##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica11##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple11##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon11 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if(string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones11##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones11##", ObjIndicador1.Observaciones);
                    }
                }

                if (ObjIndicador1.IdConceptoAccesibilidad == 16)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd12##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica12##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple12##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon12 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones12##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones12##", ObjIndicador1.Observaciones);
                    }
                }

                if(ObjIndicador1.IdConceptoAccesibilidad == 57)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd13##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica13##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple13##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon13 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones13##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones13##", ObjIndicador1.Observaciones);
                    }
                }

                if (ObjIndicador1.IdConceptoAccesibilidad == 37)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd14##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica14##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple14##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon14 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones14##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones14##", ObjIndicador1.Observaciones);
                    }
                }

                if (ObjIndicador1.IdConceptoAccesibilidad == 34)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd15##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica15##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple15##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon15 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones15##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones15##", ObjIndicador1.Observaciones);
                    }
                }

                if (ObjIndicador1.IdConceptoAccesibilidad == 67)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd16##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica16##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple16##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon16 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones16##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones16##", ObjIndicador1.Observaciones);
                    }
                }

                if (ObjIndicador1.IdConceptoAccesibilidad == 100)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd17##", ObjIndicador1.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica17##", ObjIndicador1.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple17##", ObjIndicador1.Cumple);

                    if (ObjIndicador1.Aplica == "SI" && ObjIndicador1.Cumple == "SI")
                    {
                        renglon17 = ObjIndicador1.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador1.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones17##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones17##", ObjIndicador1.Observaciones);
                    }
                }
               
            }

            Cuerpo = Cuerpo.Replace("##PuntajeIndicador1##", "100.00");

            //sumamos la respuesta de los indicadores
            SumaIndicador1 = renglon11 + renglon12 + renglon13 + renglon14 + renglon15 + renglon16 + renglon17;

            Cuerpo = Cuerpo.Replace("##PuntajeTotalIndicador1##", SumaIndicador1.ToString());

            //sacamos el euivalente del indicador uno
            EquivalenteIndicado1 = (SumaIndicador1 * 35) / 100;

            Cuerpo = Cuerpo.Replace("##EquivalenteIndicador1##", EquivalenteIndicado1.ToString());

            #endregion

            #region indicador 2

            List<AcuseCuerpoSAEF> indicador2 = ListCuerpoSAEF.Where(x => x.IdIndicador == 2).Select(x => new AcuseCuerpoSAEF { Cumplimiento = x.Cumplimiento, Aplica = x.Aplica, Cumple = x.Cumple,Observaciones = x.Observaciones, IdConceptoAccesibilidad = x.IdConceptoAccesibilidad }).ToList();

            int? renglon21 = 0;
            int? renglon22 = 0;
            int? renglon23 = 0;
            int? renglon24 = 0;
            int? renglon25 = 0;
            int? renglon26 = 0;
            int? renglon27 = 0;

            decimal? SumaIndicador2 = 0;
            decimal? EquivalenteIndicado2 = 0;

            foreach (AcuseCuerpoSAEF ObjIndicador2 in indicador2)
            {
                if (ObjIndicador2.IdConceptoAccesibilidad == 83)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd21##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica21##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple21##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon21 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones21##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones21##", ObjIndicador2.Observaciones);
                    }
                }

                if (ObjIndicador2.IdConceptoAccesibilidad == 86)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd22##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica22##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple22##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon22 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones22##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones22##", ObjIndicador2.Observaciones);
                    }
                }

                if (ObjIndicador2.IdConceptoAccesibilidad == 80)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd23##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica23##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple23##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon23 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones23##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones23##", ObjIndicador2.Observaciones);
                    }

                }

                if (ObjIndicador2.IdConceptoAccesibilidad == 19)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd24##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica24##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple24##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon24 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones24##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones24##", ObjIndicador2.Observaciones);
                    }
                }

                if (ObjIndicador2.IdConceptoAccesibilidad == 20)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd25##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica25##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple25##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon25 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones25##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones25##", ObjIndicador2.Observaciones);
                    }
                }

                if (ObjIndicador2.IdConceptoAccesibilidad == 103)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd26##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica26##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple26##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon26 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones26##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones26##", ObjIndicador2.Observaciones);
                    }
                }

                if (ObjIndicador2.IdConceptoAccesibilidad == 106)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd27##", ObjIndicador2.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica27##", ObjIndicador2.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple27##", ObjIndicador2.Cumple);

                    if (ObjIndicador2.Aplica == "SI" && ObjIndicador2.Cumple == "SI")
                    {
                        renglon27 = ObjIndicador2.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador2.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones27##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones27##", ObjIndicador2.Observaciones);
                    }
                }
            }

            Cuerpo = Cuerpo.Replace("##PuntajeIndicador2##", "100.00");

            //sumamos la respuesta de los indicadores
            SumaIndicador2 = renglon21 + renglon22 + renglon23 + renglon24 + renglon25 + renglon26 + renglon27;

            Cuerpo = Cuerpo.Replace("##PuntajeTotalIndicador2##", SumaIndicador2.ToString());

            //sacamos el euivalente del indicador uno
            EquivalenteIndicado2 = (SumaIndicador2 * 35) / 100;

            Cuerpo = Cuerpo.Replace("##EquivalenteIndicador2##", EquivalenteIndicado2.ToString());

            #endregion

            #region indicador 3

            List<AcuseCuerpoSAEF> indicador3 = ListCuerpoSAEF.Where(x => x.IdIndicador == 3).Select(x => new AcuseCuerpoSAEF { Cumplimiento = x.Cumplimiento, Aplica = x.Aplica, Cumple = x.Cumple,Observaciones = x.Observaciones, IdConceptoAccesibilidad = x.IdConceptoAccesibilidad }).ToList();

            int? renglon31 = 0;
            int? renglon32 = 0;
            int? renglon33 = 0;
            int? renglon34 = 0;
            int? renglon35 = 0;
            int? renglon36 = 0;
            int? renglon37 = 0;

            decimal? SumaIndicador3 = 0;
            decimal? EquivalenteIndicado3 = 0;

            foreach (AcuseCuerpoSAEF ObjIndicador3 in indicador3)
            {
                if (ObjIndicador3.IdConceptoAccesibilidad == 123)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd31##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica31##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple31##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon31 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones31##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones31##", ObjIndicador3.Observaciones);
                    }
                }

                if (ObjIndicador3.IdConceptoAccesibilidad == 122)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd32##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica32##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple32##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon32 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones32##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones32##", ObjIndicador3.Observaciones);
                    }
                }

                if (ObjIndicador3.IdConceptoAccesibilidad == 21)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd33##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica33##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple33##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon33 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones33##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones33##", ObjIndicador3.Observaciones);
                    }
                }

                if (ObjIndicador3.IdConceptoAccesibilidad == 149)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd34##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica34##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple34##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon34 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones34##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones34##", ObjIndicador3.Observaciones);
                    }
                }

                if (ObjIndicador3.IdConceptoAccesibilidad == 146)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd35##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica35##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple35##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon35 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones35##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones35##", ObjIndicador3.Observaciones);
                    }
                }

                if (ObjIndicador3.IdConceptoAccesibilidad == 152)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd36##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica36##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple36##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon36 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones36##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones36##", ObjIndicador3.Observaciones);
                    }
                }

                if (ObjIndicador3.IdConceptoAccesibilidad == 22)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd37##", ObjIndicador3.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica37##", ObjIndicador3.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple37##", ObjIndicador3.Cumple);

                    if (ObjIndicador3.Aplica == "SI" && ObjIndicador3.Cumple == "SI")
                    {
                        renglon37 = ObjIndicador3.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador3.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones37##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones37##", ObjIndicador3.Observaciones);
                    }
                }
            }

            Cuerpo = Cuerpo.Replace("##PuntajeIndicador3##", "100.00");

            //sumamos la respuesta de los indicadores
            SumaIndicador3 = renglon31 + renglon32 + renglon33 + renglon34 + renglon35 + renglon36 + renglon37;

            Cuerpo = Cuerpo.Replace("##PuntajeTotalIndicador3##", SumaIndicador3.ToString());

            //sacamos el euivalente del indicador uno
            EquivalenteIndicado3 = (SumaIndicador3 * 25) / 100;

            Cuerpo = Cuerpo.Replace("##EquivalenteIndicador3##", EquivalenteIndicado3.ToString());


            #endregion

            #region indicador 4

            List<AcuseCuerpoSAEF> indicador4 = ListCuerpoSAEF.Where(x => x.IdIndicador == 4).Select(x => new AcuseCuerpoSAEF { Cumplimiento = x.Cumplimiento, Aplica = x.Aplica, Cumple = x.Cumple,Observaciones = x.Observaciones, IdConceptoAccesibilidad = x.IdConceptoAccesibilidad }).ToList();

            int? renglon41 = 0;
            int? renglon42 = 0;
            int? renglon43 = 0;
            int? renglon44 = 0;
            int? renglon45 = 0;
            int? renglon46 = 0;


            decimal? SumaIndicador4 = 0;
            decimal? EquivalenteIndicado4 = 0;

            foreach (AcuseCuerpoSAEF ObjIndicador4 in indicador4)
            {
                if (ObjIndicador4.IdConceptoAccesibilidad == 63)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd41##", ObjIndicador4.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica41##", ObjIndicador4.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple41##", ObjIndicador4.Cumple);

                    if (ObjIndicador4.Aplica == "SI" && ObjIndicador4.Cumple == "SI")
                    {
                        renglon41 = ObjIndicador4.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador4.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones41##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones41##", ObjIndicador4.Observaciones);
                    }
                }

                if (ObjIndicador4.IdConceptoAccesibilidad == 64)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd42##", ObjIndicador4.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica42##", ObjIndicador4.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple42##", ObjIndicador4.Cumple);

                    if (ObjIndicador4.Aplica == "SI" && ObjIndicador4.Cumple == "SI")
                    {
                        renglon42 = ObjIndicador4.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador4.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones42##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones42##", ObjIndicador4.Observaciones);
                    }
                }

                if (ObjIndicador4.IdConceptoAccesibilidad == 65)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd43##", ObjIndicador4.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica43##", ObjIndicador4.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple43##", ObjIndicador4.Cumple);

                    if (ObjIndicador4.Aplica == "SI" && ObjIndicador4.Cumple == "SI")
                    {
                        renglon43 = ObjIndicador4.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador4.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones43##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones43##", ObjIndicador4.Observaciones);
                    }
                }

                if (ObjIndicador4.IdConceptoAccesibilidad == 66)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd44##", ObjIndicador4.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica44##", ObjIndicador4.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple44##", ObjIndicador4.Cumple);

                    if (ObjIndicador4.Aplica == "SI" && ObjIndicador4.Cumple == "SI")
                    {
                        renglon44 = ObjIndicador4.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador4.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones44##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones44##", ObjIndicador4.Observaciones);
                    }
                }

                if (ObjIndicador4.IdConceptoAccesibilidad == 56)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd45##", ObjIndicador4.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica45##", ObjIndicador4.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple45##", ObjIndicador4.Cumple);

                    if (ObjIndicador4.Aplica == "SI" && ObjIndicador4.Cumple == "SI")
                    {
                        renglon45 = ObjIndicador4.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador4.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones45##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones45##", ObjIndicador4.Observaciones);
                    }
                }

                if (ObjIndicador4.IdConceptoAccesibilidad == 124)
                {
                    Cuerpo = Cuerpo.Replace("##PorcentajeInd46##", ObjIndicador4.Cumplimiento.ToString());
                    Cuerpo = Cuerpo.Replace("##Aplica46##", ObjIndicador4.Aplica);
                    Cuerpo = Cuerpo.Replace("##Cumple46##", ObjIndicador4.Cumple);

                    if (ObjIndicador4.Aplica == "SI" && ObjIndicador4.Cumple == "SI")
                    {
                        renglon46 = ObjIndicador4.Cumplimiento;
                    }

                    //validamos si la observacion viene en nula o vacia
                    if (string.IsNullOrEmpty(ObjIndicador4.Observaciones))
                    {
                        Cuerpo = Cuerpo.Replace("##TieneObservaciones46##", "Ocultar");
                    }
                    else
                    {
                        Cuerpo = Cuerpo.Replace("##Observaciones46##", ObjIndicador4.Observaciones);
                    }
                }

            }

            Cuerpo = Cuerpo.Replace("##PuntajeIndicador4##", "100.00");

            //sumamos la respuesta de los indicadores
            SumaIndicador4 = renglon41 + renglon42 + renglon43 + renglon44 + renglon45 + renglon46 ;

            Cuerpo = Cuerpo.Replace("##PuntajeTotalIndicador4##", SumaIndicador4.ToString());

            //sacamos el euivalente del indicador uno
            EquivalenteIndicado4 = (SumaIndicador4 * 5) / 100;

            Cuerpo = Cuerpo.Replace("##EquivalenteIndicador4##", EquivalenteIndicado4.ToString());


            #endregion


            //obtenemos el indicador global
            decimal? IndicadorGlobal = 0;

            IndicadorGlobal = EquivalenteIndicado1 + EquivalenteIndicado2 + EquivalenteIndicado3 + EquivalenteIndicado4;


            Cuerpo = Cuerpo.Replace("##GlobalIndicador##", IndicadorGlobal.ToString());

            Cuerpo = Cuerpo.Replace("##CadenaOriginal##", ObjHeaderSAEF.CadenaSAEF);
            Cuerpo = Cuerpo.Replace("##SelloDigital##", ObjHeaderSAEF.SelloSAEF);
            Cuerpo = Cuerpo.Replace("##QR##", ObjHeaderSAEF.QR);

            Cuerpo = Cuerpo.Replace("##FechaAutorizacion##", string.Format("{0:MM/dd/yyyy}", ObjHeaderSAEF.FechaRegistro));
            Cuerpo = Cuerpo.Replace("##leyendaqr##", LeyendaQR);



            //mandamos todo la cadena al metodo que genera el pdf
           ok = FormarPDF(Cuerpo, "Folio SAEF", Header, Footer,ObjHeaderSAEF.FolioSAEF,Nuevo);
            
        }

        //metodo para crear el pdf al vuelo del saef
        public byte[] FormarPDF(string HTML, string fileName, string Cabecera, string Pie,string Folio,bool nuevo)
        {
            PdfConverter pdfConverter = new PdfConverter();

            byte[] bPdf = null;
            byte[] bResp = null;

            try
            {


                pdfConverter.LicenseKey = "f1ROX0dfTk9OTl9KUU9fTE5RTk1RRkZGRg==";


                //Header
                pdfConverter.PdfDocumentOptions.ShowHeader = true;
                pdfConverter.PdfHeaderOptions.HeaderHeight = 190;
                pdfConverter.PdfHeaderOptions.HtmlToPdfArea = new HtmlToPdfArea(Cabecera, null);
                pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;

                if(nuevo == false)
                {
                    //pie
                    pdfConverter.PdfFooterOptions.FooterHeight = 100;
                    pdfConverter.PdfFooterOptions.HtmlToPdfArea = new HtmlToPdfArea(Pie, null);
                    pdfConverter.PdfDocumentOptions.ShowFooter = true;
                    pdfConverter.PdfFooterOptions.DrawFooterLine = false;
                }
                

                //poner el numero de paginacion
                pdfConverter.PdfFooterOptions.TextArea = new TextArea(5, -5, "Página &p; de &P; ",
                new System.Drawing.Font(new System.Drawing.FontFamily("Arial"), 8,
                System.Drawing.GraphicsUnit.Point));
                pdfConverter.PdfFooterOptions.TextArea.EmbedTextFont = true;
                pdfConverter.PdfFooterOptions.TextArea.TextAlign = HorizontalTextAlign.Right;

                pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;


                //margenes
                pdfConverter.PdfDocumentOptions.LeftMargin = 40;
                pdfConverter.PdfDocumentOptions.RightMargin = 40;
                pdfConverter.PdfDocumentOptions.StretchToFit = true;
                
                bPdf = pdfConverter.GetPdfBytesFromHtmlString(HTML);

                if(nuevo)
                {
                    //metodo para poner el logo de fondo
                    try
                    {
                        string rutaFondo = "https://sistemas.indaabin.gob.mx/ImagenesComunes/nuevoescudo.png";


                        MemoryStream stream = new MemoryStream();

                        iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(bPdf);
                        //crear el objeto pdfstamper que se utiliza para agregar contenido adicional al archivo pdf fuente
                        iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(pdfReader, stream);

                        //iterar a través de todas las páginas del archivo fuente pdf
                        for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
                        {

                            PdfContentByte overContent = pdfStamper.GetOverContent(pageIndex);
                            iTextSharp.text.Image jpeg = iTextSharp.text.Image.GetInstance(rutaFondo);
                            overContent.SaveState();
                            overContent.SetGState(new PdfGState
                            {
                                FillOpacity = 0.3f,
                                StrokeOpacity = 0.3f//0.3
                            });

                            overContent.AddImage(jpeg, 560f, 0f, 0f, 820f, 0f, 0f);

                            overContent.RestoreState();
                        }

                        //cerrar stamper y filestream de salida
                        pdfStamper.Close();
                        stream.Close();
                        pdfReader.Close();

                        bResp = stream.ToArray();

                        pdfStamper = null;
                        pdfReader = null;
                        stream = null;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=Acessibilidad" + " " + fileName + " " + Folio + ".pdf");

                if(nuevo)
                {
                    HttpContext.Current.Response.BinaryWrite(bResp);
                }
                else
                {
                    HttpContext.Current.Response.BinaryWrite(bPdf);
                }
                
                HttpContext.Current.Response.Flush();

                HttpContext.Current.Response.End();

               
                

            }
            catch (Exception ex)
            {
               
            }

            return bPdf;

            //return bPdf;
        }

        public byte[] GeneraPdfFromHtmlStr(string cadHtml)
        {
            PdfConverter pdfConverter = new PdfConverter();
            byte[] bPdf = null;

            string header = "";
            string footer = "";

            string tagIniEncab = "@@ENCAB@@";
            string tagFinEncab = "@@FINENCAB@@";
            string tagIniPie = "@@PIE@@";
            string tagFinPie = "@@FINPIE@@";
            int iniEncab = 0;
            int finEncab = 0;
            int iniPie = 0;
            int finPie = 0;

            iniEncab = cadHtml.IndexOf(tagIniEncab);
            finEncab = cadHtml.IndexOf(tagFinEncab);
            iniPie = cadHtml.IndexOf(tagIniPie);
            finPie = cadHtml.IndexOf(tagFinPie);

            try
            {
                if (iniEncab > 0 && finEncab > 0)
                {
                    header = cadHtml.Substring((iniEncab + tagIniEncab.Length), (finEncab - iniEncab - tagIniEncab.Length)).Trim();
                }

                if (iniPie > 0 && finPie > 0)
                {
                    footer = cadHtml.Substring((iniPie + tagIniPie.Length), (finPie - iniPie - tagIniPie.Length)).Trim();
                }

                pdfConverter.LicenseKey = "f1ROX0dfTk9OTl9KUU9fTE5RTk1RRkZGRg==";

                pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
                pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;                
                pdfConverter.ActiveXEnabledInImage = true;
                //los margenes se manejan en puntos 72 por pulgada                
                pdfConverter.PdfDocumentOptions.LeftMargin = 43;//aproximandamente 1.5cm
                pdfConverter.PdfDocumentOptions.RightMargin = 43;                

                if (header.Length > 0)
                {
                    pdfConverter.PdfDocumentOptions.ShowHeader = true;
                    pdfConverter.PdfHeaderOptions.HeaderHeight = 80;
                    pdfConverter.PdfHeaderOptions.HtmlToPdfArea = new HtmlToPdfArea(header, null);
                    pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;
                }

                else
                {
                    pdfConverter.PdfDocumentOptions.TopMargin = 43;
                }

                //numero de pagina
                pdfConverter.PdfDocumentOptions.ShowFooter = true;
                pdfConverter.PdfFooterOptions.FooterHeight = 43;
                //pdfConverter.PdfFooterOptions.TextArea = new TextArea(0, 5, "Página &p; de &P; ",
                //new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10,
                //System.Drawing.GraphicsUnit.Point));
                //pdfConverter.PdfFooterOptions.TextArea.EmbedTextFont = true;
                //pdfConverter.PdfFooterOptions.TextArea.TextAlign = HorizontalTextAlign.Right;
                pdfConverter.PdfFooterOptions.DrawFooterLine = false;

                if (footer.Length > 0)
                {
                    pdfConverter.PdfFooterOptions.FooterHeight = 72;
                    pdfConverter.PdfFooterOptions.HtmlToPdfArea = new HtmlToPdfArea(footer, null);
                }

                bPdf = pdfConverter.GetPdfBytesFromHtmlString(cadHtml);
            }

            catch (Exception ex)
            {

            }

            pdfConverter = null;

            return bPdf;
        }
    }
}