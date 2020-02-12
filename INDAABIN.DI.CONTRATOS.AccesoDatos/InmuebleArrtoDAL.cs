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
using INDAABIN.DI.CONTRATOS.Datos; //EntityFramework //entities
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //Entities //packes

namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class InmuebleArrtoDAL
    {
        /// <summary>
        /// Seleccionar una direccion de un inmueble, identificarlo a partir de proporcionar su IdPK
        /// </summary>
        /// <param name="IdInmuebleArrendamiento"></param>
        /// <returns></returns>
        public InmuebleArrto ObtenerInmuebleArrto(int IdInmuebleArrendamiento)
        {
            ModeloNegocios.InmuebleArrto objInmuebleArrto;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    objInmuebleArrto = Conn.spuSelectInmuebleArrtoXId(IdInmuebleArrendamiento)
                   .Select(RegistroBD => new InmuebleArrto
                   {
                       //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                       IdInmuebleArrendamiento = IdInmuebleArrendamiento,
                       //propiedades aplicables a un inmueble nacional, en otro caso son nulas
                       IdInmueble = RegistroBD.IdInmueble,
                       RIUFInmueble = RegistroBD.RIUFInmueble,
                       IdPais = RegistroBD.Fk_IdPais,
                       IdEstado = RegistroBD.Fk_IdEstado,
                       IdMunicipio = RegistroBD.Fk_IdMunicipio,
                       IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,
                       OtraColonia = RegistroBD.OtraColonia,
                       IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                       //comunes a cualquier dieccion de un inmueble (nunca nulas)
                       NombreVialidad = RegistroBD.NombreVialidad,
                       NumExterior = RegistroBD.NumExterior,
                       NumInterior = RegistroBD.NumInterior,
                       CodigoPostal = RegistroBD.CodigoPostal,
                       //de inmueble con direccion en el extranjero , en otro caso son nulas
                       CodigoPostalExtranjero = RegistroBD.CodigoPostalExtranjero,
                       EstadoExtranjero = RegistroBD.EstadoExtranjero,
                       CiudadExtranjero = RegistroBD.CiudadExtranjero,
                       MunicipioExtranjero = RegistroBD.MunicipioExtranjero,
                       GeoRefLatitud = RegistroBD.GeoRefLatitud,
                       GeoRefLongitud = RegistroBD.GeoRefLongitud,
                       NombreInmueble = RegistroBD.NombreInmueble,
                       EstatusRegistro = RegistroBD.EstatusRegistro, // de la tabla de InmuebleArrendamiento
                       IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,  // de la tabla de InmuebleArrendamiento
                       FechaAlta = RegistroBD.FechaAlta,  // de la tabla de InmuebleArrendamiento

                   }).FirstOrDefault();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerInmuebleArrto: {0}", ex.Message));
                }
               
            }//using
            return objInmuebleArrto;
        }//ObtenerCptosRespuestaValor

        //devuelve el count de inmuebles de arrendamiento registrados a la Institucion del promovente autentificado
        public int ObtenerConteoInmueblesArrtoXInstitucion(int IdInstitucion)
        {
            int?  CountInmueblesArrtoXInstitucion;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    CountInmueblesArrtoXInstitucion = Conn.spuSelectCountInmueblesXIdInstitucion(IdInstitucion).FirstOrDefault();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerConteoInmueblesArrtoXInstitucion: {0}", ex.Message));
                }
             
            }//usisngg

            if (!CountInmueblesArrtoXInstitucion.HasValue)
                   CountInmueblesArrtoXInstitucion = 0;

            return CountInmueblesArrtoXInstitucion.Value;
        }

        //Obtener un Inmueble y sus correspondientes mvtos de emisión de opinión con otros otros conocceptos como: emisión de opinión y/o ContratoArrto.
        public List<ModeloNegocios.InmuebleArrto> ObtenerMvtosEmisionOpinionInmueblesRegistrados(int? IdInstitucion, int? FolioOpinion,  int IdEstado, int IdMpo, int FolioSMOI,int? FolioSAEF)
        {

            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoConAsociacionesCptos = null;
           
            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {

                try
                {
                    List<spuSelectMvtosEmisionOpinionParaInmueblesArrto_Result> LSO = Conn.spuSelectMvtosEmisionOpinionParaInmueblesArrto(IdInstitucion, FolioOpinion, IdEstado, IdMpo, FolioSMOI, FolioSAEF).ToList();
                    ListInmuebleArrtoConAsociacionesCptos = Conn.spuSelectMvtosEmisionOpinionParaInmueblesArrto(IdInstitucion, FolioOpinion, IdEstado, IdMpo, FolioSMOI,FolioSAEF)
                    .Select(RegistroBD => new ModeloNegocios.InmuebleArrto
                    {
                        //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                        IdInmuebleArrendamiento = RegistroBD.IdInmuebleArrendamiento, //PK
                        IdInstitucion = RegistroBD.Fk_IdInstitucion,
                        NombreCargo = RegistroBD.CargoUsuarioRegistro,

                        //RCA 02/01/2018
                        //se comento ya que por medio del stored se obtiene este dato
                        // IdCargo = RegistroBD.Fk_IdCargo,

                        IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,
                        FechaAltaMvtoAInmueble = RegistroBD.FechaRegistro,

                        NombreInmueble = RegistroBD.NombreInmueble,
                        IdPais = RegistroBD.Fk_IdPais,
                        //propiedades de inmueble nacional
                        IdEstado = RegistroBD.Fk_IdEstado, //posible: null
                        IdMunicipio = RegistroBD.Fk_IdMunicipio,//posible: null
                        IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,//posible: null
                        IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                        CodigoPostal = RegistroBD.CodigoPostal,
                        //propiedades comunnes de cualquier inmueble
                        NombreVialidad = RegistroBD.NombreVialidad,
                        NumExterior = RegistroBD.NumExterior,
                        NumInterior = RegistroBD.NumInterior,
                        //propiedades de inmueble extranjero
                        CodigoPostalExtranjero = RegistroBD.CodigoPostalExtranjero,
                        EstadoExtranjero = RegistroBD.EstadoExtranjero,
                        CiudadExtranjero = RegistroBD.CiudadExtranjero,
                        MunicipioExtranjero = RegistroBD.MunicipioExtranjero,

                        //RCA 06/07/2018
                        FolioSAEF = RegistroBD.FolioSAEF,

                        //propiedad que es un apuntador a objeto de negocio embedido: opcional
                        EmisionOpinion = new ModeloNegocios.AplicacionConcepto
                        {
                            IdAplicacionConcepto = RegistroBD.IdAplicacionConcepto,
                            FolioAplicacionConcepto = RegistroBD.FolioAplicacionConcepto,
                            TemaAplicacionConcepto = RegistroBD.TemaAplicacionConcepto,
                            FolioSMOI_AplicadoOpinion = RegistroBD.FolioSMOI,
                            IsNotReusable = RegistroBD.IsNotReusable
                        },

                        FolioContratoArrtoVsInmuebleArrendado = RegistroBD.FolioContratoAlQueAlplicaOpinion,//puede ser nulo, porque quizas el inmueble no se asocia a un ContratoArrto.
                        OtraColonia = RegistroBD.OtraColonia
                    }).OrderByDescending(o => o.EmisionOpinion.IdAplicacionConcepto).ThenByDescending(o => o.FolioContratoArrtoVsInmuebleArrendado).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Ha ocurrido un error al recuperar emisiones de opinón de inmuebles [ObtenerMvtosEmisionOpinionInmueblesRegistrados]: ", ex.Message));
                }
                
            }
            return ListInmuebleArrtoConAsociacionesCptos;

        }

        //Obtener la lista ContratoArrto. registrados
        public List<ModeloNegocios.InmuebleArrto> ObtenerMvtosContratosInmueblesRegistrados(int? OtrasFiguras, int? IdInstitucion, int FolioContratoArrto, int IdPais, int IdEstado, int IdMpo, string RIUF)
        {
            List<ModeloNegocios.InmuebleArrto> ListInmuebleArrtoConAsociacionesCptos;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ListInmuebleArrtoConAsociacionesCptos = Conn.spuSelectMvtosContratosParaInmueblesArrto(OtrasFiguras, IdInstitucion, FolioContratoArrto, IdPais, IdEstado, IdMpo, RIUF)
                   .Where(RegistroBD => RegistroBD.EstatusRegistroContrato != false)
                        .Select(RegistroBD => new ModeloNegocios.InmuebleArrto
                   {
                       //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                       IdInmuebleArrendamiento = RegistroBD.IdInmuebleArrendamiento, //PK
                       IdInstitucion = RegistroBD.Fk_IdInstitucion,

                       //RCA 02/01/2017
                       //se comento ya que el cargo lo obtenemos del stored
                       //IdCargo = RegistroBD.Fk_IdCargo,

                       NombreCargo = RegistroBD.CargoUsuarioRegistro,
                       IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,
                       FechaAltaMvtoAInmueble = RegistroBD.FechaRegistro,

                       NombreInmueble = RegistroBD.NombreInmueble,
                       IdPais = RegistroBD.Fk_IdPais,
                       //propiedades de inmueble nacional
                       IdEstado = RegistroBD.Fk_IdEstado, //posible: null
                       IdMunicipio = RegistroBD.Fk_IdMunicipio,//posible: null
                       IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,//posible: null
                       IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                       CodigoPostal = RegistroBD.CodigoPostal,
                       //propiedades comunnes de cualquier inmueble
                       NombreVialidad = RegistroBD.NombreVialidad,
                       NumExterior = RegistroBD.NumExterior,
                       NumInterior = RegistroBD.NumInterior,
                       //propiedades de inmueble extranjero
                       CodigoPostalExtranjero = RegistroBD.CodigoPostalExtranjero,
                       EstadoExtranjero = RegistroBD.EstadoExtranjero,
                       CiudadExtranjero = RegistroBD.CiudadExtranjero,
                       MunicipioExtranjero = RegistroBD.MunicipioExtranjero,
                       //RCA 15/08/2017
                       FechaFinOcupacion = Convert.ToDateTime(RegistroBD.FechaFinOcupacion),
                       //RCA 17/08/2017
                       Bandera = RegistroBD.EsPadre,
                       
                       //objeto de negocio embedido: opcional
                       EmisionOpinion = new ModeloNegocios.AplicacionConcepto
                       {
                           FolioAplicacionConcepto = RegistroBD.FolioAplicacionConcepto,
                           //TemaAplicacionConcepto = RegistroBD.TemaAplicacionConcepto,
                       },

                       //propiedad que es un objeto de negocio embedido: opcional
                       ContratoArrtoInmueble = new ModeloNegocios.ContratoArrto
                       {
                           FolioContratoArrto = RegistroBD.FolioContratoConInmuebleArrendado,
                           DescripcionTipoContrato = RegistroBD.DescripcionTipoContrato,//Nac, Extj o OtrasFigOcup
                           DescripcionTipoArrendamiento = RegistroBD.DescripcionTipoArrendamiento,//Nuevo, Continuacion, Sust
                           RIUF = RegistroBD.RIUF,
                           ObservacionesContratosReferencia = RegistroBD.Observaciones,
                           IsNotReusable = RegistroBD.IsNotReusable
                       },

                       //FolioContratoArrtoVsInmuebleArrendado = RegistroBD.FolioContratoConInmuebleArrendado,//puede ser nulo, porque quizas el inmueble no se asocia a un ContratoArrto.
                       OtraColonia = RegistroBD.OtraColonia

                       // FolioContratoArrto_FK = RegistroBD.FolioContratoAlQueAlplicaOpinion, //puede venir nulo,se cambio por cero
                   }).OrderByDescending(o => o.ContratoArrtoInmueble.FolioContratoArrto).ThenByDescending(o => o.EmisionOpinion.FolioAplicacionConcepto).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerMvtosContratosInmueblesRegistrados: ", ex.Message));
                }
                
            }//using

            //regresa un count con la lista de parametros
            return ListInmuebleArrtoConAsociacionesCptos;
        }

        public int UpdateIdInmuebleByIdInmuebleArrendamiento(int IdInmueble, int IdInmuebleArrendamiento)
        {
            int num = 0;
            using (var oContext = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var oInmueble = (from s in oContext.InmuebleArrendamiento
                                     where s.IdInmuebleArrendamiento == IdInmuebleArrendamiento
                                     select s).FirstOrDefault();
                    oInmueble.Fk_IdInmueble = IdInmueble;
                    num = oContext.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("UpdateIdInmuebleByIdInmuebleArrendamiento: ", ex.Message));
                }
                
            }//using
            return num;
        }

        public int InsertInmuebleArrto(ModeloNegocios.InmuebleArrto objInmuebleArtto)
        {
            int iAffect;
          
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                   
                        iAffect = Conn.spuInsertInmuebleArrendamiento(
                        objInmuebleArtto.IdInstitucion,
                            //objInmuebleArtto.IdCargo,
                        objInmuebleArtto.NombrePais,
                        objInmuebleArtto.IdPais,
                        objInmuebleArtto.IdTipoInmueble,
                        objInmuebleArtto.IdEstado,
                        objInmuebleArtto.IdMunicipio,
                        objInmuebleArtto.IdLocalidadColonia,
                        objInmuebleArtto.OtraColonia,
                        objInmuebleArtto.IdTipoVialidad,
                        objInmuebleArtto.NombreVialidad,
                        objInmuebleArtto.NumExterior,
                        objInmuebleArtto.NumInterior,
                        objInmuebleArtto.CodigoPostal,
                        objInmuebleArtto.GeoRefLatitud,
                        objInmuebleArtto.GeoRefLongitud,
                        objInmuebleArtto.NombreInmueble,
                        objInmuebleArtto.IdInmueble,
                        objInmuebleArtto.CodigoPostalExtranjero,
                        objInmuebleArtto.EstadoExtranjero,
                        objInmuebleArtto.CiudadExtranjero,
                        objInmuebleArtto.MunicipioExtranjero,
                        objInmuebleArtto.IdUsuarioRegistro,
                        objInmuebleArtto.NombreCargo
                        );

                        Conn.SaveChanges();
  
                }//using
        
            return iAffect; //afectados
        }

        //recuperar el IdEstado y IdMpo de un inmueble
        public ModeloNegocios.InmuebleArrto ObtenerEstadoMpoXIdInmuebleArrto(int IdInmuebleArrto)
        {

            ModeloNegocios.InmuebleArrto objInmuebleArrto;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    objInmuebleArrto = Conn.spuSelectEstadoMpoXIdInmuebleArrto(IdInmuebleArrto)
                   .Select(RegistroBD => new ModeloNegocios.InmuebleArrto
                   {
                       //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                       IdEstado = RegistroBD.Fk_IdEstado,
                       IdMunicipio = RegistroBD.Fk_IdMunicipio

                   }).FirstOrDefault();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerEstadoMpoXIdInmuebleArrto: ", ex.Message));
                }
               
            }//using

            return objInmuebleArrto;
        }

        //RCA 17/08/2017
        public int UpdateBandera(int FolioContrato)
        {

            int IdFolioContrato = 0;

            using (var Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var oInmueble = (from s in Conn.ContratoArrto
                                     where s.FolioContratoArrto == FolioContrato
                                     select s).FirstOrDefault();
                    if (oInmueble != null)
                    {
                        IdFolioContrato = Convert.ToInt32(oInmueble.Fk_IdContratoArrtoPadre);

                        if (IdFolioContrato != 0)
                        {
                            var oInmueble1 = (from s in Conn.ContratoArrto
                                              where s.IdContratoArrto == IdFolioContrato
                                              select s).FirstOrDefault();

                            oInmueble1.EsPadre = true;
                            oInmueble1.EsRUSP = false;
                            Conn.SaveChanges();
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("UpdateBandera: ", ex.Message));
                }

            }//using
            return IdFolioContrato;
        }

        //RCA 11/05/2018
        //mwtodo para mostrar la consulta de a ventana para el estatus del RUSP
        public static List<EstatusRUSPvsRIUF> ObtenerInformacionEstatusRUSP(DateTime? FechaInicio, DateTime? FechaFin, int? IdInstitucion,string RIUF, int? IdPais, int? IdEstado, int? IdMunicipio, string CP, int? TipoRegistro, int? EstatusRUSP,int? FolioContrato)
        {
            List<EstatusRUSPvsRIUF> ListInmueblesRUSP = null;

            using(ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ListInmueblesRUSP = conexion.spuSelecInformacionRIUFHabilitaroDeshabilitar(FechaInicio, FechaFin, IdInstitucion, RIUF, IdPais, IdEstado, IdMunicipio, CP, TipoRegistro, EstatusRUSP,FolioContrato).Select(x => new EstatusRUSPvsRIUF
                    {
                        IdContrato = x.IdContratoArrto,
                        FolioContratoArrto = x.FolioContratoArrto,
                        RIUF = x.RIUF,
                        IdInmuebleArrendamiento = x.IdInmuebleArrendamiento,
                        NombreInmueble = x.NombreInmueble,
                        NombrePais = x.DescripcionPais,
                        NombreEstado = x.DescripcionEstado,
                        NombreMunicipio = x.DescripcionMunicipio,
                        NombreColonia = x.colonia,
                        CodigoPostal = x.CodigoPostal,
                        TipoVialidad = x.DescripcionTipoVialidad,
                        NombreVialidad = x.NombreVialidad,
                        NumeroExterior = x.NumExterior,
                        NumeroInterior = x.NumInterior,
                        FechaAltaMvtoAInmueble = x.FechaRegistro,
                        IdUsuario = x.Fk_IdUsuarioRegistro,
                        NombreCargo = x.CargoUsuarioRegistro,
                        FolioAplicacionConcepto = x.FolioEmisionOpinion,
                        DescripcionTipoContrato = x.DescripcionTipoContrato,
                        DescripcionTipoArrendamiento = x.DescripcionTipoArrendamiento,
                        NombreInstitucion = x.DescripcionInstitucion,
                        ObservacionesContratosReferencia = x.Observaciones,
                        EstatusRUSP = x.EstatusRUSP,
                        FechaFinOcupacion = x.FechaFinOcupacion

                    }).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerInformacionEstatusRUSP: {0}", ex.Message));
                }
            }

            return ListInmueblesRUSP;
        }

        //RCA 14/05/2018
        //metodo para habilitar el contrato con el RIUF
        public static Boolean HabilitarDeshabilitarRIUF (int FolioContrato, int TipoCaso)
        {
            bool ok = false;

            using(ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    conexion.ActualizaRUSP(FolioContrato,TipoCaso);

                    ok = true;
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("HabilitarDeshabilitarRIUF:{0}", ex.Message));
                }
            }

            return ok;
        }

        //RCA 19/07/2018
        //metodo para obtener el encabezado de accesibilidad
        public static String EncabezadoSAEF()
        {
            string Encabezado = string.Empty;

            using(ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var header = conexion.Cat_Parametro.Where(x => x.IdParametro == 12).Select(s => new { s.ValorParametro }).FirstOrDefault();

                    if(header != null)
                    {
                        Encabezado = header.ValorParametro;
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("EncabezadoSAEF:{0}", ex.Message));
                }
            }

            return Encabezado;
        }

        //RCA 19/07/2018
        //metodo para obtener el pie de pagina de saef
        public static String PiePaginaSAEF()
        {
            string PiePagina = string.Empty;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var footer = conexion.Cat_Parametro.Where(x => x.IdParametro == 11).Select(s => new { s.ValorParametro }).FirstOrDefault();

                    if (footer != null)
                    {
                        PiePagina = footer.ValorParametro;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("PiePaginaSAEF:{0}", ex.Message));
                }
            }

            return PiePagina;
        }

        //RCA 19/07/2018
        //metodo para traer el cuerpo del saef
        public static String CuerpoSAEF()
        {
            string CuerpoHTML = string.Empty;

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var BodySAEF = conexion.Cat_Parametro.Where(x => x.IdParametro == 13).Select(s => new { s.ValorParametro }).FirstOrDefault();

                    if (BodySAEF != null)
                    {
                        CuerpoHTML = BodySAEF.ValorParametro;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("CuerpoSAEF:{0}", ex.Message));
                }
            }

            return CuerpoHTML;
        }

    }//clase
}
