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
    public class EmisionOpinionDAL
    {
        /// <summary>
        /// Proposito: Obtener los cptos de emisión de opicion
        /// IdTema: 
        ///         2=Opinión Nuevo Arrendamiento
        ///         3=Opinion de Sustitucion Arrto
        ///         4=Opinion de Continuacion Arro
        ///  Fecha de Creacion: 20/07/2016    
        /// </summary>
        /// <param name="IdTema"></param>
        /// <returns></returns>
        public List<ConceptoRespValor> ObtenerCptosRespuestaValor(byte IdTema, int IdInstitucion)
        {
            List<ConceptoRespValor> listaConceptosOpinion;
           
            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    listaConceptosOpinion = Conn.spuSelectConceptosXResponderTema(IdTema, IdInstitucion)
                    .Select(CptoOpinionBD => new ConceptoRespValor
                    {
                        //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                        IdConceptoRespValor = CptoOpinionBD.IdConceptoRespValor,
                        NumOrden = CptoOpinionBD.NumOrden,
                        IdTema = CptoOpinionBD.Fk_IdTema, //fk
                        DescripcionTema = CptoOpinionBD.DescripcionTema,
                        IdConcepto = CptoOpinionBD.Fk_IdConcepto, //fk
                        DescripcionConcepto = CptoOpinionBD.DescripcionConcepto,
                        FundamentoLegal = CptoOpinionBD.FundamentoLegal,
                        EsDeterminante = CptoOpinionBD.EsDeterminante,
                        DescripcionRespuesta = CptoOpinionBD.DescripcionRespuesta,
                        IdRespuesta = CptoOpinionBD.Fk_IdRespuesta, //fk
                        ValorPonderacionRespuesta = CptoOpinionBD.ValorPonderacionRespuesta


                    }).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerCptosRespuestaValor: {0}", ex.Message));
                }
                
            }//using

                                   
            return listaConceptosOpinion;

        }


        /// <summary>
        /// Proposito: Insert en la BD las respuestas a la emisión de opinión de nuevo arrendamientos, las respuestsa se pasan en un dataTable como un arreglo de parametros.
        /// Fecha de Creacion: 28/07/2016
        /// </summary>
        /// <param name="IdTipoArrendamiento"></param>
        /// <param name="IdInstitucionUsr"></param>
        /// <param name="IdUsuarioRegistro"></param>
        /// <param name="DataTableRespuestaCptoList"></param>
        /// <param name="strConnectionString"></param>
        /// <returns></returns>
        public int InsertEmisionOpinionADO(String DescTipoArrendamiento, int IdInstitucionUsr, string CargoUsuarioRegistro, int IdUsuarioRegistro, string Tema, string CadenaOriginal, string SelloDigital, DataTable DataTableRespuestaCptoList, String strConnectionString, int IdInmuebleArrendamiento, bool? EsContratoHistorico=null, int? FolioContrato=null, int? FolioSMOI=null, string Justificacion = null, string FolioDisponibilidad = null, string FechaDictamen = null)
        {
            int FolioEmisionOpinion = 0;
            int iAffect = 0;
            try
            {
                SqlConnection SqlConnectionBD = new System.Data.SqlClient.SqlConnection(strConnectionString);

                SqlCommand cmd = new SqlCommand("dbo.[spuInsertRespuestasAplicacionEmisionOpinion]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DescTipoArrendamiento", SqlDbType.VarChar, 50).Value = DescTipoArrendamiento;
                cmd.Parameters.Add("@IdInstitucion", SqlDbType.Int).Value = IdInstitucionUsr;
                cmd.Parameters.Add("@IdUsuarioRegistro", SqlDbType.Int).Value = IdUsuarioRegistro;
                cmd.Parameters.Add("@CargoUsuarioRegistro", SqlDbType.VarChar, 170).Value = CargoUsuarioRegistro;
                //agregar lista de parametros tipo arreglo, contenidos en un DataTable
                SqlParameter tvparam = cmd.Parameters.AddWithValue("@ListRespCpto", DataTableRespuestaCptoList);
                tvparam.SqlDbType = SqlDbType.Structured;

                cmd.Parameters.Add("@Tema", SqlDbType.VarChar, 100).Value = Tema;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar).Value = CadenaOriginal;
                cmd.Parameters.Add("@SelloDigital", SqlDbType.VarChar).Value = SelloDigital;
                cmd.Parameters.Add("@QR", SqlDbType.VarChar).Value = string.Empty;
                cmd.Parameters.Add("@IdInmuebleArrendamiento", SqlDbType.Int).Value = IdInmuebleArrendamiento;
                //
                cmd.Parameters.Add("@EsContratoHistorico", SqlDbType.Bit).Value = EsContratoHistorico;
                cmd.Parameters.Add("@FolioContrato", SqlDbType.Int).Value = FolioContrato;//opcional, aplica para cuando se trata de una emisión de opinión de Sustitucion o Continuacion, de que contrato: de esta BD o de TablaContratosHist
                cmd.Parameters.Add("@FolioSMOI", SqlDbType.Int).Value = FolioSMOI;//opcional, aplica para cuando se trata de Emisin de Opinion: nueva o sustitucion, pero aun asi puede ser opcional, por lo casos de excepcion por dictamen
                cmd.Parameters.Add("@Justificacion", SqlDbType.VarChar).Value = Justificacion;
                cmd.Parameters.Add("@FolioDictamen", SqlDbType.VarChar).Value = FolioDisponibilidad;
                cmd.Parameters.Add("@FechaDictamen", SqlDbType.VarChar).Value = FechaDictamen;

                //configurar el parametro de retorno.
                SqlParameter prmRet;
                prmRet = new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4);
                prmRet.Direction = ParameterDirection.ReturnValue;
                //Agregar el parametro al comando.
                cmd.Parameters.Add(prmRet);

                using (SqlConnectionBD)
                {
                    SqlConnectionBD.Open();
                    cmd.Connection = SqlConnectionBD;

                    // execute query, consume results, etc. here
                    iAffect = cmd.ExecuteNonQuery();
                    if (iAffect > 0)
                        FolioEmisionOpinion = (int)cmd.Parameters["@RETURN_VALUE"].Value;

                }
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("InsertEmisionOpinionADO: {0}", ex.Message));
            }
                    
            return FolioEmisionOpinion;
        }//insert


        public int InsertSMOI_ADO( int IdInstitucionUsr, string CargoUsuarioRegistro, int IdUsuarioRegistro,  string CadenaOriginal, string SelloDigital, DataTable DataTableRespuestaCptoList, String strConnectionString,string QR)
        {
            int FolioSMOI = 0;
            int iAffect = 0;

            try
            {
                SqlConnection SqlConnectionBD = new System.Data.SqlClient.SqlConnection(strConnectionString);

                SqlCommand cmd = new SqlCommand("dbo.[spuInsertRespuestasTablaSMOI]");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdInstitucion", SqlDbType.Int).Value = IdInstitucionUsr;
                cmd.Parameters.Add("@IdUsuarioRegistro", SqlDbType.Int).Value = IdUsuarioRegistro;
                cmd.Parameters.Add("@CargoUsuarioRegistro", SqlDbType.VarChar, 170).Value = CargoUsuarioRegistro;
                //agregar lista de parametros tipo arreglo, contenidos en un DataTable
                SqlParameter tvparam = cmd.Parameters.AddWithValue("@ListRespCpto", DataTableRespuestaCptoList);
                tvparam.SqlDbType = SqlDbType.Structured;

                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar).Value = CadenaOriginal;
                cmd.Parameters.Add("@SelloDigital", SqlDbType.VarChar).Value = SelloDigital;
                cmd.Parameters.Add("@QR", SqlDbType.VarChar).Value = QR;

                //configurar el parametro de retorno.
                SqlParameter prmRet;
                prmRet = new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4);
                prmRet.Direction = ParameterDirection.ReturnValue;
                //Agregar el parametro al comando.
                cmd.Parameters.Add(prmRet);

                using (SqlConnectionBD)
                {
                    SqlConnectionBD.Open();
                    cmd.Connection = SqlConnectionBD;

                    // execute query, consume results, etc. here
                    iAffect = cmd.ExecuteNonQuery();
                    if (iAffect > 0)
                        FolioSMOI = (int)cmd.Parameters["@RETURN_VALUE"].Value;

                }
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("InsertSMOI_ADO: {0}", ex.Message));
            }
           
            return FolioSMOI;
        }//insert
        

        //Acuse de Solicitud de emisión de opinión con la informacion del  inmueble para el que se aplica la solicitud
        public AcuseFolio ObtenerAcuseSolicitudOpinionConInmueble(int IdFolioAplicacionCpto, string TipoArrendamiento) //TipoArrendamiento=Nuevo, Continuación, Sustitución,seguridad
        {
            
            AcuseFolio objAcuseOpinionFolio;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    objAcuseOpinionFolio = Conn.spuSelectAcuseEmisionOpinionXIdFolio(IdFolioAplicacionCpto, TipoArrendamiento)
                    .Select(RegistroBD => new AcuseFolio
                    {
                        //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                        Folio = RegistroBD.FolioAplicacionConcepto.Value,
                        FechaRegistro = RegistroBD.FechaRegistro,
                        HoraRegistro = RegistroBD.HoraRegistro,
                        TipoArrendamientoDesc = RegistroBD.DescripcionTipoArrendamiento,
                        ResultadoAplicacionOpinion = RegistroBD.Resultado,
                        IdUsuarioRegistro = RegistroBD.IdUsuarioRegistro.Value,
                        IdInstitucionSolicitante = RegistroBD.IdInstitucionUsrRegistro.Value,
                        CadenaOriginal = RegistroBD.CadenaOriginal,
                        SelloDigital = RegistroBD.SelloDigital,
                        LeyendaAnio = RegistroBD.LeyendaAnio,

                        //RCA 13/08/2018
                        QR = RegistroBD.QR,
                        LeyendaQR = RegistroBD.leyendaqr,
                        FechaAutorizacion = RegistroBD.fechaautorizacion,

                        //objeto de negocio embedido, crear para poblar propiedades
                        InmuebleArrtoEmisionOpinion = new InmuebleArrto
                        {
                            IdPais = RegistroBD.Fk_IdPais,
                            //propiedades de inmueble nacional
                            IdEstado = RegistroBD.Fk_IdEstado,
                            IdMunicipio = RegistroBD.Fk_IdMunicipio,
                            IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,
                            OtraColonia = RegistroBD.otraColonia,
                            CodigoPostal = RegistroBD.CodigoPostal,
                            //propiedades comunnes de cualquier inmueble
                            IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                            NombreVialidad = RegistroBD.NombreVialidad,
                            NumExterior = RegistroBD.NumExterior,
                            NumInterior = RegistroBD.NumInterior,
                            GeoRefLatitud = RegistroBD.GeoRefLatitud,
                            GeoRefLongitud = RegistroBD.GeoRefLongitud,
                            //propiedades de inmueble extranjero
                            CodigoPostalExtranjero = RegistroBD.CodigoPostalExtranjero,
                            EstadoExtranjero = RegistroBD.EstadoExtranjero,
                            CiudadExtranjero = RegistroBD.CiudadExtranjero,
                            MunicipioExtranjero = RegistroBD.MunicipioExtranjero

                        }


                    }).FirstOrDefault();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerAcuseSolicitudOpinionConInmueble: {0}", ex.Message));
                }
                
                }//using
  
            return objAcuseOpinionFolio;

        }//ObtenerCptosRespuestaValor


        public ConceptoRespValor ObtenerFundamentoLegalCpto(byte IdTema, decimal NumOrden)
        {

            ConceptoRespValor objConceptoRespValor;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    objConceptoRespValor = Conn.spuObtenerFundamentoLegalXCptoNumOrden(IdTema, NumOrden)
                   .Select(RegistroBD => new ConceptoRespValor
                   {
                       //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                       DescripcionTema = RegistroBD.DescripcionTema,
                       IdConcepto = RegistroBD.IdConcepto,
                       DescripcionConcepto = RegistroBD.DescripcionConcepto,
                       FundamentoLegal = RegistroBD.FundamentoLegal,


                   }).FirstOrDefault();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerFundamentoLegalCpto: {0}", ex.Message));
                }
               
            }//using

            return objConceptoRespValor;

        }


        /// <summary>
        /// Obtener las solicitudes de emisión de opinión emitidas
        /// pasar a los parametros cuando se deseen ignorar
        /// </summary>
        /// <param name="IdFolioAplicacionCpto"></param>
        /// <returns></returns>
        public List<ModeloNegocios.AplicacionConcepto> ObtenerSolicitudesEmisionOpinionEmitidas(int? IdInstitucion, int FolioAplicacionConcepto, byte? IdTema,int? FolioSAEF)
        {

            List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    //spuSelectSolicitudesEmisionOpinionEmitidas_Result x = Conn.spuSelectSolicitudesEmisionOpinionEmitidas(IdInstitucion, FolioAplicacionConcepto);

                    ListAplicacionConcepto = Conn.spuSelectSolicitudesEmisionOpinionEmitidas(IdInstitucion, FolioAplicacionConcepto, IdTema,FolioSAEF)
                        .Select(RegistroBD => new ModeloNegocios.AplicacionConcepto
                        {
                            //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                            IdAplicacionConcepto = RegistroBD.IdAplicacionConcepto,
                            FolioAplicacionConcepto = RegistroBD.FolioAplicacionConcepto,
                            TemaAplicacionConcepto = RegistroBD.TemaAplicacionConcepto,
                            ResultadoEmisionOpinion = RegistroBD.ResultadoOpinion,
                            IdInstitucion = RegistroBD.Fk_IdInstitucion,
                            NombreCargo = RegistroBD.CargoUsuarioRegistro,
                            IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,
                            FechaRegistro = RegistroBD.FechaRegistro,
                            Observaciones = RegistroBD.Observaciones,
                            TipoArrendamiento = RegistroBD.DescripcionTipoArrendamiento,

                            //RCA 08/08/2018
                            FolioSAEF = RegistroBD.FolioSAEF,

                            //objeto de negocio embedido
                            InmuebleArrto = new ModeloNegocios.InmuebleArrto
                            {
                                NombreInmueble = RegistroBD.NombreInmueble,
                                IdPais = RegistroBD.Fk_IdPais,
                                //propiedades de inmueble nacional
                                IdEstado = RegistroBD.Fk_IdEstado,
                                IdMunicipio = RegistroBD.Fk_IdMunicipio,
                                IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,
                                OtraColonia = RegistroBD.OtraColonia,

                                //propiedades comunnes de cualquier inmueble
                                IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                                NombreVialidad = RegistroBD.NombreVialidad,
                                NumExterior = RegistroBD.NumExterior,
                                NumInterior = RegistroBD.NumInterior,
                                CodigoPostal = RegistroBD.CodigoPostal,

                                //propiedades de inmueble extranjero
                                CodigoPostalExtranjero = RegistroBD.CodigoPostalExtranjero,
                                EstadoExtranjero = RegistroBD.EstadoExtranjero,
                                CiudadExtranjero = RegistroBD.CiudadExtranjero,
                                MunicipioExtranjero = RegistroBD.MunicipioExtranjero,
                                //datos del contratoArrto con el que se asocia un inmueble arrendado, por lo que pueden ser nulos


                                FolioContratoArrtoVsInmuebleArrendado = RegistroBD.FolioContratoAlQueAlplicaOpinion//puede ser nulo, porque quizas el inmueble no se asocia a un ContratoArrto.

                            },

                            FolioContratoArrto_FK = RegistroBD.FolioContratoAlQueAlplicaOpinion, //puede venir nulo,se cambio por cero
                            FolioSMOI_AplicadoOpinion = RegistroBD.FolioSMOI

                        }).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerSolicitudesEmisionOpinionEmitidas: {0}", ex.Message));
                }

            }//using

            return ListAplicacionConcepto;

        }

        //devuelve un valor scalar
        public ModeloNegocios.AplicacionConcepto ObtenerEmisionOpinionPorFolio(int FolioOpinion, int IdInstitucion)
        {
            //decimal TotalM2_SMOI;
            ModeloNegocios.AplicacionConcepto oEmision;
            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                //try
                //{
                    //TotalM2_SMOI = Conn.spuSelectTotalSMOIxFolioOpinion(  Convert.ToDecimal(Conn.spuSelectTotalSMOIxFolioOpinion(FolioOpinion, IdInstitucion).FirstOrDefault());
                    oEmision = Conn.spuSelectTotalSMOIxFolioOpinion(FolioOpinion, IdInstitucion)
                    .Select(RegistroBD => new ModeloNegocios.AplicacionConcepto
                    {
                        IdAplicacionConcepto = RegistroBD.IdAplicacionConcepto,
                        SupM2XSMOI = RegistroBD.TotalM2_SMOI,
                        FolioSAEF = RegistroBD.FolioSAEF,
                        InmuebleArrto = new InmuebleArrto { IdInmuebleArrendamiento = RegistroBD.Fk_IdInmuebleArrendamiento.Value }
                    }).FirstOrDefault();
                //}
                //catch (Exception ex)
                //{

                //    throw new Exception(string.Format("ObtenerEmisionOpinionPorFolio: {0}", ex.Message));
                //}
               
            }//using
            return oEmision;
        }

        //RCA 10/08/2018

        //metodo para obtener el id de aplicacion del folio smoi
        public int ObtenerIdAplicacionSMOI(int FolioSMOI,int? TipoFolio)
        {
            int Aplicacion = 0;

            using(ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    if(TipoFolio == 7)
                    {
                        var Id = conexion.AplicacionConcepto.Where(x => x.FolioAplicacionConcepto == FolioSMOI && x.Fk_IdTema == 7)
                        .Select(x => new { x.IdAplicacionConcepto }).FirstOrDefault();

                        Aplicacion = Id.IdAplicacionConcepto;
                    }
                    else
                    {
                        var Id = conexion.AplicacionConcepto.Where(x => x.FolioAplicacionConcepto == FolioSMOI && x.Fk_IdTema != 7)
                        .Select(x => new { x.IdAplicacionConcepto }).FirstOrDefault();

                        Aplicacion = Id.IdAplicacionConcepto;
                    }
                    
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerIdAplicacionSMOI:{0}", ex.Message));
                }
            }

            return Aplicacion;
        }

        //metodo para actualizar el campo de QR de las tablas SMOI
        public Boolean ActualizarSMOIQR(string QR,int IdAplicacionSMOI)
        {
            bool ok = false;

            using(var conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var QRSMOI = (from s in conexion.SelloDigital
                                  where s.Fk_IdRegistroTablaOrigen == IdAplicacionSMOI && s.Fk_IdCatTabla == 1
                                  select s).FirstOrDefault();

                    QRSMOI.QR = QR;

                    conexion.SaveChanges();

                    ok = true;
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ActualizarSMOIQR:{0}", ex.Message));
                }
            }

            return ok;
        }

        //RCA 21/11/2018
        //metodo para obtener el id tema del folio de emision de opinion
        public int ObtenerIdTemaEmision(int FolioOpinion)
        {
            int IdCat_Tema = 0;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var Tema = conexion.AplicacionConcepto.Where(x => x.FolioAplicacionConcepto == FolioOpinion && x.SMOIm2FactorY == null).Select(x => new { x.Fk_IdTema}).FirstOrDefault();

                    if(Tema != null)
                    {
                        IdCat_Tema = Tema.Fk_IdTema;
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerIdTemaEmision:{0}", ex.Message));
                }
            }

                return IdCat_Tema;
        }

        
    }//clase
}
