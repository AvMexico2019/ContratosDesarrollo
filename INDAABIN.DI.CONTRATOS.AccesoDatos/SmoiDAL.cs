using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

//comunicacion con las capas
using INDAABIN.DI.CONTRATOS.Datos; //EntityFramework
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //Entities

namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class SmoiDAL
    {
        public List<ConceptoRespValor> ObtenerCptosRespuestaValorSMOI(byte IdTema, int IdInstitucion)
        {
            List<ConceptoRespValor> listaConceptosSMOI;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {

                try
                {
                    listaConceptosSMOI = Conn.spuSelectConceptosXResponderSMOI(IdTema, IdInstitucion)
                   .Select(CptoBD => new ConceptoRespValor
                   {
                       //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                       IdConceptoRespValor = CptoBD.IdConceptoRespValor,
                       NumOrden = CptoBD.NumOrden,
                       DescripcionTema = CptoBD.DescripcionTema,
                       IdConcepto = CptoBD.IdConcepto, //fk
                       DescripcionConcepto = CptoBD.DescripcionConcepto,
                       IdRespuesta = CptoBD.Fk_IdRespuesta,
                       ValorPonderacionRespuesta = CptoBD.ValorRespuesta,
                       ValorMaximo = CptoBD.ValorMaximo,
                       ValorMinimo = CptoBD.ValorMinimo
                   }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerCptosRespuestaValorSMOI: ", ex.Message));
                }

            }//using
            return listaConceptosSMOI;
        }


        //Acuse de Solicitud de emisión de opinión con la informacion del  inmueble para el que se aplica la solicitud
        public AcuseFolio ObtenerAcuseSMOI(int IdFolioAplicacionCpto)
        {
            AcuseFolio objAcuseOpinionFolio;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {

                //try
                //{
                objAcuseOpinionFolio = Conn.spuSelectAcuseSMOI(IdFolioAplicacionCpto)
               .Select(RegistroBD => new ModeloNegocios.AcuseFolio
               {
                   //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                   Folio = RegistroBD.FolioAplicacionConcepto.Value,
                   FechaRegistro = RegistroBD.FechaRegistro,
                   HoraRegistro = RegistroBD.HoraRegistro,
                   IdUsuarioRegistro = RegistroBD.IdUsuarioRegistro.Value,
                   IdInstitucionSolicitante = RegistroBD.IdInstitucionUsrRegistro.Value,
                   CadenaOriginal = RegistroBD.CadenaOriginal,
                   SelloDigital = RegistroBD.SelloDigital,
                   TotalSMOIm2FactorX = RegistroBD.TotalSMOIm2FactorX,
                   TotalSMOIm2FactorY = RegistroBD.TotalSMOIFactorY,
                   TotalSMOIm2FactorZ = RegistroBD.TotalSMOIm2FactorZ,
                   TotalSMOIm2 = RegistroBD.TotalSMOIm2,
                   LeyendaAnio = RegistroBD.LeyendaAnio,
                   QR = RegistroBD.QR,
                   LeyendaQR = RegistroBD.Leyendaqr,
                   FechaAutorizacion = RegistroBD.FechaAutorizacion
               }).FirstOrDefault();
                //}
                //catch(Exception ex)
                //{
                //    throw new Exception(string.Format("ObtenerAcuseSMOI: ", ex.Message));
                //}

            }//using
            return objAcuseOpinionFolio;
        }


        //devuelve un valor scalar que es el count de registrados de SMOI emitidos a la Institucion del promovente autentificado
        public int ObtenerConteoSolicitudesSMOIxIdInstitucion(int IdInstitucion)
        {
            int? CountNumSolicitudesSMOIxIdInstituto;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    CountNumSolicitudesSMOIxIdInstituto = Conn.spuSelectCountSolicitudesSMOIXIdInstitucion(IdInstitucion).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerConteoSolicitudesSMOIxIdInstitucion: ", ex.Message));
                }

            }//using

            if (!CountNumSolicitudesSMOIxIdInstituto.HasValue)
                CountNumSolicitudesSMOIxIdInstituto = 0;

            return CountNumSolicitudesSMOIxIdInstituto.Value;

        }


        //obtener solicitudes de SMOI emitidas por institucion
        public List<ModeloNegocios.AplicacionConcepto> ObtenerSolicitudesSMOIEmitidas(int? IdInstitucion, int FolioAplicacionConcepto)
        {

            List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {


                try
                {
                    //spuSelectSolicitudesEmisionOpinionEmitidas_Result x = Conn.spuSelectSolicitudesEmisionOpinionEmitidas(IdInstitucion, FolioAplicacionConcepto);

                    ListAplicacionConcepto = Conn.spuSelectSolicitudesSMOIEmitidas(IdInstitucion, FolioAplicacionConcepto)
                        .Select(RegistroBD => new ModeloNegocios.AplicacionConcepto
                        {
                            //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                            IdAplicacionConcepto = RegistroBD.IdAplicacionConcepto,
                            FolioAplicacionConcepto = RegistroBD.FolioAplicacionConcepto,
                            IdInstitucion = RegistroBD.Fk_IdInstitucion,
                            NombreCargo = RegistroBD.CargoUsuarioRegistro,
                            IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,
                            FechaRegistro = RegistroBD.FechaRegistro,
                            Observaciones = RegistroBD.Observaciones,
                            SupM2XSMOI = RegistroBD.TotalSMOIM2Total,
                            FolioEmisionOpinion_DondeSeAplicoFolioSMOI = RegistroBD.FolioEmisionOpinionDondeSeAplico


                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerSolicitudesSMOIEmitidas: ", ex.Message));
                }

            }//using

            return ListAplicacionConcepto;
        }

        //devuelve el SupTotalM2SMOI de un Folio
        public decimal ObtenerSupTotalM2SMOI(int IdFolioSMOI, int IdInstitucion)
        {
            decimal? SupTotalM2SMOI;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    SupTotalM2SMOI = Conn.spuObtenerSupTotalM2SMOIxIdFolio(IdFolioSMOI, IdInstitucion).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerSupTotalM2SMOI: ", ex.Message));
                }

            }//using

            if (!SupTotalM2SMOI.HasValue)
                SupTotalM2SMOI = 0;

            return SupTotalM2SMOI.Value;
        }

        //devuelve el num. de veces que se ha relacionado un SMOI con registros de emisión de opinión
        public int ObtenerCountUsoFolioSMOIenOpionion(int IdFolioSMOI, int IdInstitucion)
        {
            int? CountUsoFolioSMOIenOpionion;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    CountUsoFolioSMOIenOpionion = Conn.spuSelectCountUsoFolioSMOIenOpionion(IdFolioSMOI, IdInstitucion).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerCountUsoFolioSMOIenOpionion: ", ex.Message));
                }

            }//using

            if (!CountUsoFolioSMOIenOpionion.HasValue)
                CountUsoFolioSMOIenOpionion = 0;


            return CountUsoFolioSMOIenOpionion.Value;
        }


        //devuelve el SupTotalM2SMOI de un Folio
        public decimal ObtenerSupTotalM2SMOIsinOcupar(int IdFolioSMOI, int IdInstitucion)
        {
            decimal? SupTotalM2SMOI;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    SupTotalM2SMOI = Conn.spuObtenerSupTotalM2SMOIxIdFolioSinOcupar(IdFolioSMOI, IdInstitucion).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerSupTotalM2SMOIsinOcupar: ", ex.Message));
                }

            }//using

            if (!SupTotalM2SMOI.HasValue)
                SupTotalM2SMOI = 0;

            return SupTotalM2SMOI.Value;
        }

        public List<ConceptoRespValor> ObtenerTablaSMOIFolio(int folio)
        {
            List<ConceptoRespValor> Lconcepto = new List<ConceptoRespValor>();

            try
            {
                using (ArrendamientoInmuebleEntities conn = new ArrendamientoInmuebleEntities())
                {
                    Lconcepto = conn.spuObtenerSMOIfolio(folio).Select( x => new ConceptoRespValor
                    {
                        IdConceptoRespValor = x.IdConceptoRespValor,
                        IdTema = x.Fk_IdTema,
                        DescripcionTema = x.DescripcionTema,
                        IdConcepto = x.Fk_IdConcepto,
                        DescripcionConcepto = x.DescripcionConcepto,
                        IdRespuesta = x.Fk_IdRespuesta,
                        NumOrden = x.NumOrden,
                        ValorPonderacionRespuesta = x.ValorRespuesta,
                        ValorMinimo = x.ValorMinimo,
                        ValorMaximo = x.ValorMaximo,
                        ValorRespuesta = x.ValorResp,
                    }).ToList();
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerTablaSMOIFolio: ", ex.Message));
            }

            return Lconcepto;
        }

    }//clase
}
