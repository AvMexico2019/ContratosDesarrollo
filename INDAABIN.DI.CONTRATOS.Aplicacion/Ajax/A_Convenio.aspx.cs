using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Configuration;
using System.Reflection;
using System.IO;

using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.ModeloNegocios.Catalogos;
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Ajax
{
    public partial class A_Convenio : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static Respuesta ObtenerJustipreciacionSecuencial(string secuencial, int IdPais, int IdEstado, int IdMunicipio, int IdInmueble)
        {
            Respuesta Respuesta = new Respuesta();
            ControladorBUS cBus = new ControladorBUS();
            string SupRentable = null;
            string MontoDictaminado = null;
            NG_Inmueble nInm = new NG_Inmueble();
            bool valCP = false;

            try
            {
                List<SolicitudAvaluos> LsolAvaluos = cBus.ObtenerJustipreciacionAvaluos(secuencial);

                ModeloNegocios.InmuebleArrto Inmueble = new NG_InmuebleArrto().ObtenerInmuebleArrto(System.Convert.ToInt32(IdInmueble));

                if (Inmueble.IdPais != Constantes.IdMexico)
                {
                    Respuesta.Mensaje = "El secuencial de justipreciación debe ser para un inmueble nacional. Favor de validar tus datos";
                    Respuesta.respuesta = false;
                    return Respuesta;
                }

                if (LsolAvaluos == null)
                {
                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = "No se encontró ningun resultado en la busqueda del secuencial. Favor de validar tus datos";
                    return Respuesta;
                }

                if (LsolAvaluos.Count == 0)
                {
                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = "No se encontró ningun resultado en la busqueda del secuencial. Favor de validar tus datos";
                    return Respuesta;
                }

                SolicitudAvaluos solAvaluos = LsolAvaluos.FirstOrDefault();

                if (solAvaluos.Estatus.ToUpper() == "CANCELADO")
                {
                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = "El estatus de la justipreciación es cancelado";
                    return Respuesta;
                }

                if (nInm.QuitarAcentosTexto(solAvaluos.EstadoDescripcion.Replace(" ", "").ToUpper()) == nInm.QuitarAcentosTexto(Inmueble.NombreEstado.Replace(" ", "").ToUpper()) && nInm.QuitarAcentosTexto(solAvaluos.MunicipioDescripcion.Replace(" ", "").ToUpper()) == nInm.QuitarAcentosTexto(Inmueble.NombreMunicipio.Replace(" ", "").ToUpper()))
                {
                    valCP = true;

                    if (!string.IsNullOrEmpty(solAvaluos.CP))
                    {
                        var res = solAvaluos.CP.Trim().Replace(" ", "").PadLeft(5, '0');
                        var res1 = Inmueble.CodigoPostal.Trim().Replace(" ", "").PadLeft(5, '0');

                        if (res != res1)
                            valCP = false;
                    }

                    if (!valCP)
                    {
                        Respuesta.respuesta = false;
                        Respuesta.Mensaje = "El secuencial de justipreciación proporcionado no corresponde con el código postal  de la dirección del inmueble. Favor de validar tus datos";
                        return Respuesta;
                    }
                }

                else
                {
                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = "El secuencial de justipreciación proporcionado no corresponde con la entidad federativa y municipio en la dirección del inmueble. Favor de validar tus datos";
                    return Respuesta;
                }

                if (solAvaluos.SuperficieRentableDictaminado != null || solAvaluos.SuperficieRentable != null)
                {
                    if ((solAvaluos.SuperficieRentableDictaminado != null) && (solAvaluos.SuperficieRentableDictaminado > 0))
                        SupRentable = solAvaluos.SuperficieRentableDictaminado.Value.ToString("0.00");
                    if ((solAvaluos.SuperficieRentable != null) && (solAvaluos.SuperficieRentable > 0))
                        SupRentable = solAvaluos.SuperficieRentable.Value.ToString("0.00");
                }

                if (solAvaluos.MontoDictaminado != null)
                    MontoDictaminado = solAvaluos.MontoDictaminado.Value.ToString("0.00");

                if ((SupRentable == null && MontoDictaminado == null) || (Convert.ToDecimal(SupRentable) == 0 || Convert.ToDecimal(MontoDictaminado) == 0))
                {
                    string msjError = "El secuencial de justipreciación proporcionado con estatus de atención: " + solAvaluos.Estatus.ToUpper() + ", aun no cuenta con: <br/> * Monto dictaminado <br/> * Superficie rentable dictaminado ó capturada por el promovente en la solicitud de avalúo <br/> Es necesario que se cuente con esta información para poder registrarlo al contrato, por favor contacte al Indaabin";

                    if (solAvaluos.Estatus.ToUpper() != "CONCLUIDO")
                        msjError = "El secuencial de justipreciación proporcionado con estatus de atención: " + solAvaluos.Estatus.ToUpper() + ", aun no cuenta con: <br/> * Monto dictaminado <br/> * Superficie rentable dictaminado ó capturada por el promovente en la solicitud de avalúo <br/> Es necesario que se cuente con esta información para poder registrarlo al contrato, por favor contacte al Indaabin";

                    Respuesta.Mensaje = msjError;
                    Respuesta.respuesta = false;
                    return Respuesta;
                }

                DateTime? fechaDictamen = null;

                if (string.IsNullOrEmpty(solAvaluos.FechaDictamen.ToString()) == false)
                    fechaDictamen = Convert.ToDateTime(solAvaluos.FechaDictamen.Substring(0, 10));


                JustripreciacionContrato justipreciacion = new JustripreciacionContrato
                {
                    MontoDictaminado = Convert.ToDecimal(MontoDictaminado),
                    SuperficieDictaminada = SupRentable,
                    UnidadMedidaSupRentableDictaminada = solAvaluos.UnidadMedidaRentable,
                    EstatusAtencion = solAvaluos.Estatus,
                    NoGenerico = solAvaluos.NoGenerico,
                    FechaDictamen = fechaDictamen,
                    descFechaDictamen = fechaDictamen == null ? "" : fechaDictamen.Value.ToString("d"),
                    Secuencial = secuencial,
                    InstitucionJustipreciacion = solAvaluos.InstitucionDescripcion,
                };

                Respuesta.Justipreciacion = justipreciacion;
                Respuesta.respuesta = true;
                Respuesta.Mensaje = string.Empty;
            }

            catch (Exception ex)
            {
                Respuesta.respuesta = false;
                Respuesta.Mensaje = "Hubo un problema al realizar la búsqueda del secuencial. Favor de contactar a tu administrador";
            }

            return Respuesta;
        }

        [WebMethod]
        public static Respuesta GenerarRegistroConvenio(int IdUsuario, Convenio Convenio, JustripreciacionContrato JustripreciacionContrato, string Institucion, int IdInmueble)
        {
            Respuesta Respuesta = new Respuesta();
            string msjError = string.Empty;
            NG_Catalogos nCatalogo = new NG_Catalogos();
            string HTML = string.Empty;
            Utilerias.ExportHTML exportHTML = new Utilerias.ExportHTML();
            string fechaRegistro = string.Empty;
            string fechaAutorizacion = string.Empty;
            AcuseContrato AcuseContrato = new AcuseContrato();

            try
            {
                NG_ContratoArrto nContrato = new NG_ContratoArrto();
                ModeloNegocios.InmuebleArrto objInmuebleArrto = new Negocio.NG_InmuebleArrto().ObtenerInmuebleArrto(IdInmueble);

                JustripreciacionContrato.FechaDictamen = null;

                if (!string.IsNullOrEmpty(JustripreciacionContrato.descFechaDictamen))
                    JustripreciacionContrato.FechaDictamen = Convert.ToDateTime(JustripreciacionContrato.descFechaDictamen);

                if (!nContrato.GenerarConvenioModificatorio(Convenio, IdUsuario, JustripreciacionContrato, ref msjError, ref fechaRegistro))
                {
                    if (msjError.Length == 0)
                        msjError = "Hubo un problema al generar el registro del convenio modificatorio. Favor de contactar a tu administrador";

                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = msjError;
                    return Respuesta;
                }

                string Direccion = objInmuebleArrto.DireccionCompleta;
                string CadenaOriginal = "||Invocante:[" + Institucion + "] || Inmueble:[" + Direccion + "]||Fecha:[" + DateTime.Today.ToLongDateString() + "]||" + Guid.NewGuid().ToString();
                string SelloDigital = UtilContratosArrto.Encrypt(CadenaOriginal, true, "ConvenioModificatorio");
                string ruta = ConfigurationManager.AppSettings["RutaDocsAdjuntosEscritura"] + Convenio.FolioConvenio + "\\AcuseConvenio\\";

                Convenio.cadOriginal = CadenaOriginal;
                Convenio.Sello = SelloDigital;
                Convenio.QR = UtilContratosArrto.GenerarCodigoQR(string.Empty, 6, string.Empty, ruta.Replace("\\", "/").Replace(ConfigurationManager.AppSettings["RutaDocsAdjuntosEscritura"], ConfigurationManager.AppSettings["RutaDocsAdjuntosLectura"]) + "AcuseConvenioModificatorio.pdf");
                
                if (!nContrato.AutorizarConvenioModificatorio(Convenio.IdConvenio, CadenaOriginal, SelloDigital, Convenio.QR, IdUsuario, ref fechaAutorizacion))
                {
                    if (msjError.Length == 0)
                        msjError = "Hubo un problema al generar el registro del convenio modificatorio. Favor de contactar a tu administrador";

                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = msjError;
                    return Respuesta;
                }

                Parametro parametro = nCatalogo.ObtenerParametroNombre("PlantillaConvenioModificatorio");
                HTML = parametro.ValorParametro;

                Parametro ParametroQR = nCatalogo.ObtenerParametroNombre("LeyendaQR");

                AcuseContrato = nContrato.ObtenerAcuseContrato(Convenio.FolioContrato);

                string cuerpoTabla = string.Empty;

                if (Convenio.TieneProrroga == 1)
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Prorroga de vigencia:</strong> " + Convenio.descFechaTermino + "</td></tr>";

                if (Convenio.TieneNvaSuperfice == 1)
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Superficie rentable:</strong> " + Convenio.SupM2 + "</td></tr>";

                if (Convenio.TieneNvoMonto == 1)
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Monto de pago mensual:</strong> " + Convenio.ImporteRenta + "</td></tr>";

                if (Convenio.TieneNvoMonto == 1 && Convenio.ImporteRenta > Constantes.MONTO_MINIMO_SECUENCIAL)
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Secuencial de justipreciación:</strong> " + Convenio.Secuencial + "</td></tr>";

                HTML = HTML.Replace("##FechaEfecto##", Convenio.DescFechaEfectoConvenio);
                HTML = HTML.Replace("##Folio##", Convenio.FolioConvenio);
                HTML = HTML.Replace("##InstitucionPublica##", Institucion);
                HTML = HTML.Replace("##Propietario##", AcuseContrato.ContratoArrto.PropietarioInmueble);
                HTML = HTML.Replace("##FunResponsable##", AcuseContrato.ContratoArrto.FuncionarioResponsable);
                HTML = HTML.Replace("##DireccionInmu##", objInmuebleArrto.DireccionCompleta);
                HTML = HTML.Replace("##valorRIUF##", AcuseContrato.ContratoArrto.RIUF.ToString());

                HTML = HTML.Replace("##CadOriginal##", Convenio.cadOriginal);
                HTML = HTML.Replace("##Sello##", Convenio.Sello);
                HTML = HTML.Replace("##QR##", Convenio.QR);
                HTML = HTML.Replace("##LeyendaQR##", ParametroQR.ValorParametro);
                HTML = HTML.Replace("##HoraReg##", Convenio.FechaRegistro.ToString("hh:mm tt"));

                HTML = HTML.Replace("##dia##", fechaRegistro.Split('/')[0]);
                HTML = HTML.Replace("##mes##", Util.ObtenerDescripcionMes(Convert.ToInt32(fechaRegistro.Split('/')[1])));
                HTML = HTML.Replace("##anio##", fechaRegistro.Split('/')[2]);

                HTML = HTML.Replace("##FechaAutorizacion##", fechaAutorizacion);

                HTML = HTML.Replace("##CuerpoTabla##", cuerpoTabla);

                byte[] bPDF = exportHTML.GeneraPdfFromHtmlStr(HTML);

                if (bPDF != null)
                {                    
                    if (!Directory.Exists(ruta))
                        Directory.CreateDirectory(ruta);

                    if (File.Exists(ruta + "AcuseConvenioModificatorio.pdf"))
                        File.Delete(ruta + "AcuseConvenioModificatorio.pdf");

                    File.WriteAllBytes(ruta + "AcuseConvenioModificatorio.pdf", bPDF);

                    Respuesta.Url = ruta.Replace("\\", "/").Replace(ConfigurationManager.AppSettings["RutaDocsAdjuntosEscritura"], ConfigurationManager.AppSettings["RutaDocsAdjuntosLectura"]) + "AcuseConvenioModificatorio.pdf";
                    Respuesta.respuesta = true;
                    Respuesta.Mensaje = string.Empty;
                }
            }

            catch (Exception ex)
            {
                Respuesta.respuesta = false;
                Respuesta.Mensaje = "Hubo un problema al generar el registro del convenio modificatorio. Favor de contactar a tu administrador";
            }

            return Respuesta;
        }

        [WebMethod]
        public static Respuesta ObtenerConveniosModificatorios(int FolioContrato)
        {
            Respuesta Respuesta = new Respuesta();
            NG_ContratoArrto nContrato = new NG_ContratoArrto();

            try
            {
                Respuesta.Lconvenio = nContrato.ObtenerConveniosContrato(FolioContrato);
                Respuesta.respuesta = true;
                Respuesta.Mensaje = string.Empty;
            }

            catch (Exception ex)
            {
                Respuesta.respuesta = false;
                Respuesta.Mensaje = "Hubo un problema al obtener la lista de convenios modificatorios. Favor de contactar a tu administrador";
            }

            return Respuesta;
        }

        [WebMethod]
        public static Respuesta ObtenerAcuseConvenio(string folioConvenio)
        {
            Respuesta Respuesta = new Respuesta();
            Convenio Convenio = new Convenio();
            NG_ContratoArrto nContrato = new NG_ContratoArrto();
            string msjError = string.Empty;
            AcuseContrato AcuseContrato = new AcuseContrato();
            NG_Catalogos nCatalogo = new NG_Catalogos();
            string HTML = string.Empty;
            Utilerias.ExportHTML exportHTML = new Utilerias.ExportHTML();

            try
            {
                Convenio = nContrato.ObtenerConvenioModificatorio(folioConvenio, ref msjError);

                if (Convenio.IdConvenio == 0)
                {
                    if (msjError.Length == 0)
                        msjError = "Hubo un problema al obtener el convenio modificatorio. Favor de contactar a tu administrador";

                    Respuesta.respuesta = false;
                    Respuesta.Mensaje = msjError;
                    return Respuesta;
                }
                string ruta = ConfigurationManager.AppSettings["RutaDocsAdjuntosEscritura"] + folioConvenio + "\\AcuseConvenio\\";

                ModeloNegocios.InmuebleArrto objInmuebleArrto = new NG_InmuebleArrto().ObtenerInmuebleArrto(Convenio.IdInmueble);

                Parametro parametro = nCatalogo.ObtenerParametroNombre("PlantillaConvenioModificatorio");
                HTML = parametro.ValorParametro;

                Parametro ParametroQR = nCatalogo.ObtenerParametroNombre("LeyendaQR");
                AcuseContrato = nContrato.ObtenerAcuseContrato(Convenio.FolioContrato);

                string cuerpoTabla = string.Empty;

                if (Convenio.FechaTermino != null)
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Prorroga de vigencia:</strong> " + Convenio.descFechaTermino + "</td></tr>";

                if (Convenio.SupM2 != null)
                {
                    Convenio.TieneNvaSuperfice = 1;
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Superficie rentable:</strong> " + Convenio.SupM2 + "</td></tr>";
                }


                if (Convenio.ImporteRenta != null)
                {
                    Convenio.TieneNvoMonto = 1;
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Monto de pago mensual:</strong> " + Convenio.ImporteRenta + "</td></tr>";
                }
                    
                if (Convenio.TieneNvaSuperfice == 1 && Convenio.TieneNvoMonto == 1 && Convenio.ImporteRenta != null && Convenio.ImporteRenta > Constantes.MONTO_MINIMO_SECUENCIAL)
                    cuerpoTabla += "<tr font-family: Montserrat'><td><strong>Secuencial de justipreciación:</strong> " + Convenio.Secuencial + "</td></tr>";
                
                HTML = HTML.Replace("##FechaEfecto##", Convenio.DescFechaEfectoConvenio);
                HTML = HTML.Replace("##Folio##", Convenio.FolioConvenio);
                HTML = HTML.Replace("##InstitucionPublica##", AcuseContrato.InstitucionSolicitante);
                HTML = HTML.Replace("##Propietario##", AcuseContrato.ContratoArrto.PropietarioInmueble);
                HTML = HTML.Replace("##FunResponsable##", AcuseContrato.ContratoArrto.FuncionarioResponsable);
                HTML = HTML.Replace("##DireccionInmu##", objInmuebleArrto.DireccionCompleta);
                HTML = HTML.Replace("##valorRIUF##", AcuseContrato.ContratoArrto.RIUF.ToString());

                HTML = HTML.Replace("##CadOriginal##", Convenio.cadOriginal);
                HTML = HTML.Replace("##Sello##", Convenio.Sello);
                HTML = HTML.Replace("##QR##", Convenio.QR);
                HTML = HTML.Replace("##LeyendaQR##", ParametroQR.ValorParametro);
                HTML = HTML.Replace("##HoraReg##", Convenio.FechaRegistro.ToString("hh:mm tt"));

                HTML = HTML.Replace("##dia##", Convenio.descFechaRegistro.Split('/')[0]);
                HTML = HTML.Replace("##mes##", Util.ObtenerDescripcionMes(Convert.ToInt32(Convenio.descFechaRegistro.Split('/')[1])));
                HTML = HTML.Replace("##anio##", Convenio.descFechaRegistro.Split('/')[2]);

                HTML = HTML.Replace("##FechaAutorizacion##", Convenio.descFechaAutorizacion);

                HTML = HTML.Replace("##CuerpoTabla##", cuerpoTabla);

                byte[] bPDF = exportHTML.GeneraPdfFromHtmlStr(HTML);

                if (bPDF != null)
                {
                    if (!Directory.Exists(ruta))
                        Directory.CreateDirectory(ruta);

                    if (File.Exists(ruta + "AcuseConvenioModificatorio.pdf"))
                        File.Delete(ruta + "AcuseConvenioModificatorio.pdf");

                    File.WriteAllBytes(ruta + "AcuseConvenioModificatorio.pdf", bPDF);

                    Respuesta.Url = ruta.Replace("\\", "/").Replace(ConfigurationManager.AppSettings["RutaDocsAdjuntosEscritura"], ConfigurationManager.AppSettings["RutaDocsAdjuntosLectura"]) + "AcuseConvenioModificatorio.pdf";
                    Respuesta.respuesta = true;
                    Respuesta.Mensaje = string.Empty;
                }
            }

            catch (Exception ex)
            {
                Respuesta.respuesta = false;
                Respuesta.Mensaje = "Hubo un problema al obtener el convenio modificatorio. Favor de contactar a tu administrador";
            }

            return Respuesta;
        }
    }
}