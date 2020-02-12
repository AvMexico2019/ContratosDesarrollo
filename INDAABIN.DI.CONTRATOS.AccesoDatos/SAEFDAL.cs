using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

//conexion entre capas
using INDAABIN.DI.CONTRATOS.Datos;
using INDAABIN.DI.CONTRATOS.ModeloNegocios;

namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class SAEFDAL
    {

        //metodo que obtiene los conceptos para el formulario de SAEF
        public static List<ConceptoSAEF> ObtenerConceptosSAEF()
        {
            List<ConceptoSAEF> ListConcepSAEF = null;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ListConcepSAEF = conexion.ConceptoAccesibilidad.Where(x => x.EstatusRegistro == 1)
                        .Select(x => new ConceptoSAEF
                        {
                            IdConcAccesibilidad = x.IdConcAccesibilidad,
                            Fk_IdIndicador = x.Fk_IdIndicador,
                            Fk_IdAreaPrioridad = x.Fk_IdAreaPrioridad,
                            DescConcAccesibilidade = x.DescConcAccesibilidad,
                            Cumplimiento = x.Cumplimiento
                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerConceptosSAEF:{0}", ex.Message));
                }
            }

            return ListConcepSAEF;
        }

        public static EmisionOpinionSAEF ObtenerDatosEmisionSAEF(string FolioEmision)
        {
            EmisionOpinionSAEF ObjEmisionSAEF = null;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ObjEmisionSAEF = conexion.spuObtenerEmisionOpinion(FolioEmision).Select(x => new EmisionOpinionSAEF
                    {
                        IdAplicacionConcepto = x.IdAplicacionConcepto,
                        FolioEmisionOpinion = x.FolioAplicacionConcepto,
                        IdUsuarioEmisionOpinion = x.Fk_IdUsuarioRegistro,
                        FechaRegistro = x.FechaRegistro

                    }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerDatosEmisionSAEF:{0}", ex.Message));
                }
            }

            return ObjEmisionSAEF;
        }

        //metodo para guardar los resultados de la emision de saef
        public static Boolean GuardarEmisionSAEF(ValorRespuestaSAEF ObjRespuestaSAEF, int TipoGuardado, string Cadena, string Sello, string QR)
        {
            bool ok = false;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    conexion.spuInsertarMovimientoSAEF(ObjRespuestaSAEF.IdAlicacionConcepto, ObjRespuestaSAEF.ConceptoAccesibilidad, ObjRespuestaSAEF.Aplica, ObjRespuestaSAEF.Existe, ObjRespuestaSAEF.Cantidad, ObjRespuestaSAEF.SeRequiere, ObjRespuestaSAEF.Cumple, ObjRespuestaSAEF.Observaciones, ObjRespuestaSAEF.IdUsuario, TipoGuardado, Cadena, Sello, QR);

                    ok = true;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("GuardarEmisionSAEF:{0}", ex.Message));
                }
            }


            return ok;
        }

        //metodo para obtener la informacion de la cabeza del acuse 
        public static AcuseHeaderSAEF ObtenerHeaderAcuseSAEF(int? IdAplicacionConcepto)
        {
            AcuseHeaderSAEF ObjHeaderSAEF = new AcuseHeaderSAEF();

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ObjHeaderSAEF = conexion.spuObtenerAcuseHeaderSelloSAEF(IdAplicacionConcepto)
                        .Select(x => new AcuseHeaderSAEF
                        {
                            FechaRegistro = x.FechaRegistro,
                            NombreInstitucion = x.DescripcionInstitucion,
                            NombreInmueble = x.NombreInmueble,
                            FolioSAEF = x.FolioSAEF,
                            RIUF = x.RIUF,
                            SelloSAEF = x.SelloDigital,
                            CadenaSAEF = x.CadenaOriginal,
                            QR = x.QR

                        }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerHeaderAcuseSAEF:{0}", ex.Message));
                }
            }

            return ObjHeaderSAEF;
        }

        //metodo para obtener el cuerpo de SAEF
        public static List<AcuseCuerpoSAEF> ObtenerCuerpoAcuseSAEF(int? IdApConcep)
        {
            List<AcuseCuerpoSAEF> ListAcuseBodySAEF = null;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ListAcuseBodySAEF = conexion.spuObtenerAcuseCuerpoSAEF(IdApConcep)
                        .Select(x => new AcuseCuerpoSAEF
                        {
                            IdIndicador = x.Fk_IdIndicador,
                            Cumplimiento = x.Cumplimiento,
                            Aplica = x.Aplica,
                            Cumple = x.Cumple,
                            Observaciones = x.Observaciones,
                            IdConceptoAccesibilidad = x.Fk_IdConcAccesibilidad
                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerCuerpoAcuseSAEF:{0}", ex.Message));
                }
            }

            return ListAcuseBodySAEF;
        }

        //metodo para obtener la idaplicacionconcepto
        public static int ObtenerIdAplicacionEmision(int FolioSAEF)
        {
            int IdApliConcep = 0;



            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var Aplicacion = conexion.AplicacionConcepto.Where(x => x.FolioSAEF == FolioSAEF.ToString()).Select(x => new { x.IdAplicacionConcepto }).FirstOrDefault();

                    IdApliConcep = Aplicacion.IdAplicacionConcepto;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerIdAplicacionEmision:{0}", ex.Message));
                }
            }

            return IdApliConcep;
        }

        //metodo para obtener el QR 
        public static String ObtenerQR(int IdRegistro, int? TipoTablaOrigen)
        {
            string QR = string.Empty;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {

                    var qr = conexion.SelloDigital.Where(x => x.Fk_IdRegistroTablaOrigen == IdRegistro && x.Fk_IdCatTabla == TipoTablaOrigen).Select(x => new { x.QR }).FirstOrDefault();

                    QR = qr.QR;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerQR:{0}", ex.Message));
                }
            }
            return QR;
        }

        //metodo para obtener la leyenda del QR
        public static String ObtenerLeyendaQR()
        {
            string Leyendaqr = string.Empty;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var Leyenda = conexion.Cat_Parametro.Where(s => s.IdParametro == 14).Select(s => new { s.ValorParametro }).FirstOrDefault();

                    Leyendaqr = Leyenda.ValorParametro;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerLeyendaQR:{0}", ex.Message));
                }
            }

            return Leyendaqr;
        }

        public List<ValorRespuestaSAEF> ObtenerValorRespuestaAplicacionConcepto(int IdAplicacionConcepto)
        {
            List<ValorRespuestaSAEF> Lvalor = new List<ValorRespuestaSAEF>();

            try
            {
                using (ArrendamientoInmuebleEntities aInmuebles = new ArrendamientoInmuebleEntities())
                {
                    Lvalor = aInmuebles.Movimiento.Where(x => x.FK_IdAplicacionConcepto == IdAplicacionConcepto && x.EstatusRegistro == true).Select(x => new ValorRespuestaSAEF
                    {
                        IdAlicacionConcepto = x.FK_IdAplicacionConcepto,
                        ConceptoAccesibilidad = x.Fk_IdConcAccesibilidad,
                        Aplica = x.Aplica,
                        Existe = x.Existe,
                        Cantidad = x.Cantidad,
                        SeRequiere = x.SeRequiere,
                        Cumple = x.Cumple,
                        Observaciones = x.Observaciones,

                    }).ToList();
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerValorRespuestaAplicacionConcepto:{0}", ex.Message));
            }

            return Lvalor;
        }
    }
}
