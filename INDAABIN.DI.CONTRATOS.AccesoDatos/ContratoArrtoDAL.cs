using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Transactions;
//comunicacion con las capas
using INDAABIN.DI.CONTRATOS.Datos; //EntityFramework
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //Entities


namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class ContratoArrtoDAL
    {
        public List<ModeloNegocios.ContratoArrtoHistorico> ObtenerContratosArrtoHistorico(int IdInstitucion, byte IdEstado, String NombreMunicipio)
        {
            List<ModeloNegocios.ContratoArrtoHistorico> listaContratosHistorico;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    listaContratosHistorico = Conn.spuSelectContratosHistoricoXInstitucionEdoMpo(IdInstitucion, IdEstado, NombreMunicipio)
                    .Select(CptoBD => new ContratoArrtoHistorico
                    {
                        //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                        NumContratoHistorico = CptoBD.NumContratoHistorico,
                        DireccionCompleta = CptoBD.Direccion,
                        FechaInicioContrato = CptoBD.FechaInicioContrato,
                        FechaFinContrato = CptoBD.FechaFinContrato,
                        FechaContrato = CptoBD.FechaContrato,
                        Propietario = CptoBD.Propietario

                    }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerContratosArrtoHistorico: {0}", ex.Message));
                }

            }

            return listaContratosHistorico;
        }

        public int UpdateRIUFByFolioContrato(string RIUF, int FolioContratoArrendamiento, int Institucion)
        {
            int num = 0;
            using (var oContext = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var oContrato = (from s in oContext.ContratoArrto
                                     where s.FolioContratoArrto == FolioContratoArrendamiento && s.Fk_IdInstitucion == Institucion
                                     select s).FirstOrDefault();
                    oContrato.RIUF = RIUF;
                    num = oContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("UpdateRIUFByFolioContrato: {0}", ex.Message));
                }

            }
            return num;
        }



        //Insert de un Contrato Arrto.
        //Devuelve el Folio de contrato
        public int InsertContratoArrto(ModeloNegocios.ContratoArrto objContratoArrto)
        {
            int FolioContrato = 0;
            System.Data.Entity.Core.Objects.ObjectParameter parametroFolioContrato = new System.Data.Entity.Core.Objects.ObjectParameter("FolioContrato", FolioContrato);

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    Conn.spuInsertContratoArrto(
                  objContratoArrto.Fk_IdTipoContrato, //1=Nac, 2=Ext u 3=OtrasFig
                  objContratoArrto.Fk_IdTipoArrendamiento, //Nuevo, Sust o Cont
                  objContratoArrto.Fk_IdTipoContratacion, //Automatico, Dictamiando
                  objContratoArrto.Fk_IdInmuebleArrendamiento,
                  objContratoArrto.Fk_NumContratoHistorico,
                  objContratoArrto.Fk_IdContratoArrtoPadre,
                  objContratoArrto.Fk_IdTipoUsoInm, //del BUS
                  objContratoArrto.OtroUsoInmueble,
                  objContratoArrto.Fk_IdTipoOcupacion,//Comodato, Prestamo, Conscesión (aplicable solo a Arrendamiento de Otras Fig.)
                  objContratoArrto.OtroTipoOcupacion,
                  objContratoArrto.Fk_IdTipoMoneda,
                  objContratoArrto.Fk_IdInstitucion,
                  objContratoArrto.NombreInstitucion,
                  objContratoArrto.FechaInicioOcupacion,
                  objContratoArrto.FechaFinOcupacion,
                  objContratoArrto.AreaOcupadaM2,
                  objContratoArrto.MontoPagoMensual,
                  objContratoArrto.MontoPagoPorCajonesEstacionamiento,
                  objContratoArrto.CuotaMantenimiento,
                  objContratoArrto.PtjeImpuesto,
                  objContratoArrto.FolioEmisionOpinion, //se define como atributo y no como objeto, porque solo se 1 dato: el Id
                  objContratoArrto.NumeroDictamenExcepcionFolioSMOI,
                  objContratoArrto.RIUF,
                  objContratoArrto.Observaciones,
                  objContratoArrto.PropietarioInmueble,
                  objContratoArrto.FuncionarioResponsable,
                  objContratoArrto.Fk_IdUsuarioRegistro,
                  objContratoArrto.CargoUsuarioRegistro,
                      //objetos de Persona Referencia
                      //titular del OIC
                      objContratoArrto.PersonaReferenciaTitularOIC.NombreCargo,
                      objContratoArrto.PersonaReferenciaTitularOIC.Nombre,
                      objContratoArrto.PersonaReferenciaTitularOIC.ApellidoPaterno,
                      objContratoArrto.PersonaReferenciaTitularOIC.ApellidoMaterno,
                      objContratoArrto.PersonaReferenciaTitularOIC.Email,
                      //Capturista
                      objContratoArrto.PersonaReferenciaCapturista.NombreCargo,
                      objContratoArrto.PersonaReferenciaCapturista.Nombre,
                      objContratoArrto.PersonaReferenciaCapturista.ApellidoPaterno,
                      objContratoArrto.PersonaReferenciaCapturista.ApellidoMaterno,
                      objContratoArrto.PersonaReferenciaCapturista.Email,
                   //Objeto de Justipreciacion
                   objContratoArrto.JustripreciacionContrato.Secuencial,
                   objContratoArrto.JustripreciacionContrato.SuperficieDictaminada,
                   objContratoArrto.JustripreciacionContrato.FechaDictamen,
                   objContratoArrto.JustripreciacionContrato.MontoDictaminado,
                   objContratoArrto.JustripreciacionContrato.EstatusAtencion,
                   objContratoArrto.JustripreciacionContrato.NoGenerico,
                   objContratoArrto.JustripreciacionContrato.UnidadMedidaSupRentableDictaminada,
                        // apartado de seguridad
                        objContratoArrto.CuentaConDictamen,
                        objContratoArrto.FechaDictamen,
                   //
                   objContratoArrto.CadenaOriginal,
                   objContratoArrto.SelloDigital,
                   objContratoArrto.QR,
                  parametroFolioContrato //parametro ouput
                   );

                    Conn.SaveChanges();

                    if (parametroFolioContrato == null)
                        throw new InvalidOperationException("No se pudo registrar el Contrato de Arrendamiento, vuelva a intentar o reporte a Sistemas");

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("InsertContratoArrto: {0}", ex.Message));
                }

            }//using

            if (parametroFolioContrato.Value != null)
                FolioContrato = Convert.ToInt32(parametroFolioContrato.Value);

            //parametro de retorno, regresa 0 si no hay insert, o el folio > 0 si se realizo el insert
            return FolioContrato;
        }

        //Insert de un Contrato Arrto de Otras Fig. Ocuapacion
        //Devuelve el Folio de contrato
        public int InsertContratoArrtoOtrasFigOcupacion(ModeloNegocios.ContratoArrto objContratoArrto)
        {
            int FolioContrato = 0;
            System.Data.Entity.Core.Objects.ObjectParameter parametroFolioContrato = new System.Data.Entity.Core.Objects.ObjectParameter("FolioContrato", FolioContrato);

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    Conn.spuInsertContratoOtrasFigurasOcupacion(

                 objContratoArrto.Fk_IdTipoOcupacion,//Comodato, Prestamo, Conscesión (aplicable solo a Arrendamiento de Otras Fig.)
                 objContratoArrto.OtroTipoOcupacion,
                 objContratoArrto.Fk_IdInmuebleArrendamiento,
                 objContratoArrto.Fk_IdTipoUsoInm, //del BUS
                 objContratoArrto.OtroUsoInmueble,
                 objContratoArrto.Fk_IdTipoMoneda,
                 objContratoArrto.Fk_IdInstitucion,
                 objContratoArrto.NombreInstitucion,
                 objContratoArrto.FechaInicioOcupacion,
                 objContratoArrto.FechaFinOcupacion,
                 objContratoArrto.AreaOcupadaM2,
                 objContratoArrto.MontoPagoMensual,
                 objContratoArrto.MontoPagoPorCajonesEstacionamiento,
                 objContratoArrto.CuotaMantenimiento,
                 objContratoArrto.PtjeImpuesto,
                 objContratoArrto.RIUF,
                 objContratoArrto.Observaciones,
                 objContratoArrto.PropietarioInmueble,
                  objContratoArrto.FuncionarioResponsable,
                  objContratoArrto.Fk_IdUsuarioRegistro,
                  objContratoArrto.CargoUsuarioRegistro,
                     //objetos de Persona Referencia (3):
                     //Responsable de la Ocupacion
                     objContratoArrto.PersonaReferenciaResponsableOcupacion.NombreCargo,
                     objContratoArrto.PersonaReferenciaResponsableOcupacion.Nombre,
                     objContratoArrto.PersonaReferenciaResponsableOcupacion.ApellidoPaterno,
                     objContratoArrto.PersonaReferenciaResponsableOcupacion.ApellidoMaterno,
                     objContratoArrto.PersonaReferenciaResponsableOcupacion.Email,
                     //titular del OIC
                     objContratoArrto.PersonaReferenciaTitularOIC.NombreCargo,
                     objContratoArrto.PersonaReferenciaTitularOIC.Nombre,
                     objContratoArrto.PersonaReferenciaTitularOIC.ApellidoPaterno,
                     objContratoArrto.PersonaReferenciaTitularOIC.ApellidoMaterno,
                     objContratoArrto.PersonaReferenciaTitularOIC.Email,
                     //Capturista
                     objContratoArrto.PersonaReferenciaCapturista.NombreCargo,
                     objContratoArrto.PersonaReferenciaCapturista.Nombre,
                     objContratoArrto.PersonaReferenciaCapturista.ApellidoPaterno,
                     objContratoArrto.PersonaReferenciaCapturista.ApellidoMaterno,
                     objContratoArrto.PersonaReferenciaCapturista.Email,
                        // apartado de seguridad
                        objContratoArrto.CuentaConDictamen,
                        objContratoArrto.FechaDictamen,

                  //
                  objContratoArrto.CadenaOriginal,
                  objContratoArrto.SelloDigital,
                  objContratoArrto.QR,
                 parametroFolioContrato //parametro ouput
                  );

                    Conn.SaveChanges();

                    if (parametroFolioContrato == null)
                        throw new InvalidOperationException("No se pudo registrar el Contrato de Arrendamiento, vuelva a intentar o reporte a Sistemas");

                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("InsertContratoArrtoOtrasFigOcupacion: {0}", ex.Message));
                }

            }//using

            if (parametroFolioContrato.Value != null)
                FolioContrato = Convert.ToInt32(parametroFolioContrato.Value);

            //parametro de retorno, regresa 0 si no hay insert, o el folio > 0 si se realizo el insert
            return FolioContrato;
        }

        //Acuse de Registro de Contrato: Nacional, Extranjero ó OtrasFigOcupacion 
        public AcuseContrato ObtenerAcuseContrato(int IdFolioContrato)
        {
            AcuseContrato objAcuseContrato;
            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    objAcuseContrato = Conn.spuSelectAcuseContrato(IdFolioContrato)
                    .Select(RegistroBD => new ModeloNegocios.AcuseContrato
                    {
                        //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                        Folio = RegistroBD.FolioContratoArrto,
                        FechaRegistro = RegistroBD.FechaRegistro,
                        HoraRegistro = RegistroBD.HoraRegistro,
                        IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,
                        InstitucionSolicitante = RegistroBD.NombreInstitucion,
                        FolioSAEF = RegistroBD.FolioSAEF,

                        //objeto de negocio embedido, crear para poblar propiedades
                        ContratoArrto = new ModeloNegocios.ContratoArrto
                        {
                            NombreInstitucion = RegistroBD.NombreInstitucion,
                            Fk_NumContratoHistorico = RegistroBD.Fk_NumContratoHistorico,
                            Fk_IdContratoArrtoPadre = RegistroBD.Fk_IdContratoArrtoPadre,
                            DescripcionTipoOcupacion = RegistroBD.DescripcionTipoOcupacion,
                            OtroTipoOcupacion = RegistroBD.OtroTipoOcupacion,
                            Fk_IdTipoMoneda = RegistroBD.Fk_IdTipoMoneda,
                            FechaInicioOcupacion = RegistroBD.FechaInicioOcupacion,
                            FechaFinOcupacion = RegistroBD.FechaFinOcupacion,
                            AreaOcupadaM2 = RegistroBD.AreaOcupadaM2,
                            MontoPagoMensual = RegistroBD.MontoPagoMensual,
                            MontoPagoPorCajonesEstacionamiento = RegistroBD.MontoPagoPorCajonesEstacionamiento,
                            CuotaMantenimiento = RegistroBD.CuotaMantenimiento,
                            PtjeImpuesto = RegistroBD.PtjeImpuesto,
                            NumeroDictamenExcepcionFolioSMOI = RegistroBD.NumeroDictamenExcepcionFolioSMOI,
                            RIUF = RegistroBD.RIUF,
                            PropietarioInmueble = RegistroBD.PropietarioInmueble,
                            FuncionarioResponsable = RegistroBD.FuncionarioResponsable,
                            DescripcionTipoContrato = RegistroBD.DescripcionTipoContrato,
                            DescripcionTipoArrendamiento = RegistroBD.DescripcionTipoArrendamiento,
                            DescripcionTipoContratacion = RegistroBD.DescripcionTipoContratacion,
                            FolioEmisionOpinion = RegistroBD.FolioOpinion,


                            InmuebleArrto = new InmuebleArrto
                            {
                                //IdInmueble = RegistroBD.fk_i IdInmueble, //PK
                                IdPais = RegistroBD.Fk_IdPais,
                                //IdTipoInmueble = RegistroBD.Fk_IdTipoInmueble,
                                IdEstado = RegistroBD.Fk_IdEstado,
                                IdMunicipio = RegistroBD.Fk_IdMunicipio,
                                IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,
                                OtraColonia = RegistroBD.OtraColonia,
                                IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                                NombreVialidad = RegistroBD.NombreVialidad,
                                NumExterior = RegistroBD.NumExterior,
                                NumInterior = RegistroBD.NumInterior,
                                CodigoPostal = RegistroBD.CodigoPostal,
                                //GeoRefLatitud = RegistroBD.GeoRefLatitud,
                                //GeoRefLongitud = RegistroBD.GeoRefLongitud,
                                NombreInmueble = RegistroBD.NombreInmueble,
                                CodigoPostalExtranjero = RegistroBD.CPExtranjero,
                                EstadoExtranjero = RegistroBD.EstadoExtranjero,
                                CiudadExtranjero = RegistroBD.CiudadExtranjero,
                                MunicipioExtranjero = RegistroBD.MpoExtranjero,
                                RIUFInmueble = RegistroBD.RIUF
                            }
                        },

                        //objeto de negocio embedido, crear para poblar propiedades
                        JustripreciacionContrato = new JustripreciacionContrato
                        {
                            Secuencial = RegistroBD.Secuencial,
                            SuperficieDictaminada = RegistroBD.SuperficieDictaminada,
                            FechaDictamen = RegistroBD.FechaDictamen,
                            MontoDictaminado = RegistroBD.MontoDictaminado
                        },

                        CadenaOriginal = RegistroBD.CadenaOriginal,
                        SelloDigital = RegistroBD.SelloDigital,
                        LeyendaAnio = RegistroBD.LeyendaAnio,
                        QR = RegistroBD.QR,
                        Leyenda = RegistroBD.leyenda,
                        FechaAutorizacion = RegistroBD.fechautorizacion

                    }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerAcuseContrato: {0}", ex.Message));
                }

            }//using
            return objAcuseContrato;
        }

        //Obtener ContratoArrto. registrados 
        public List<ModeloNegocios.ContratoArrto> ObtenerContratosArrtoRegistrados(int? IdInstitucion, int? FolioContratoArrto, byte? TipoContato)
        {
            List<ModeloNegocios.ContratoArrto> ListContratosArrtoRegistrados;
            ListContratosArrtoRegistrados = null;
            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var oList = Conn.spuSelectContratoArrto(IdInstitucion, FolioContratoArrto, TipoContato).ToList();

                    if (oList != null)
                    {
                        //ListContratosArrtoRegistrados = Conn.spuSelectContratoArrto(IdInstitucion, FolioContratoArrto, TipoContato)
                        ListContratosArrtoRegistrados = oList
                            .Where(RegistroBD => RegistroBD.EstatusRegistroContratoArrto != false)
                            .Select(RegistroBD => new ModeloNegocios.ContratoArrto
                            {

                                //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                                FolioContratoArrto = RegistroBD.FolioContratoArrto,
                                strFechaRegistro = RegistroBD.FechaRegistro,
                                DescripcionTipoContrato = RegistroBD.DescripcionTipoContrato,
                                DescripcionTipoArrendamiento = RegistroBD.DescripcionTipoArrendamiento,
                                DescripcionTipoOcupacion = RegistroBD.DescripcionTipoOcupacion,
                                DescripcionTipoContratacion = RegistroBD.DescripcionTipoContratacion,
                                PeriodoOcupacion = RegistroBD.PeriodoContratacion,
                                strFechaInicioOcupacion = RegistroBD.FechaInicioOcupacion,
                                strFechaFinOcupacion = RegistroBD.FechaFinOcupacion,
                                PropietarioInmueble = RegistroBD.PropietarioInmueble,
                                FuncionarioResponsable = RegistroBD.FuncionarioResponsable,
                                RIUF = RegistroBD.RIUF,
                                Fk_IdTipoUsoInm = RegistroBD.Fk_IdTipoUsoInm.Value,
                                Fk_IdTipoMoneda = RegistroBD.Fk_IdTipoMoneda,
                                Fk_IdTipoContratacion = RegistroBD.Fk_IdTipoContratacion,
                                Observaciones = RegistroBD.Observaciones,
                                AreaOcupadaM2 = RegistroBD.AreaOcupadaM2,
                                MontoPagoMensual = RegistroBD.MontoPagoMensual,
                                MontoPagoPorCajonesEstacionamiento = RegistroBD.MontoPagoPorCajonesEstacionamiento,
                                CuotaMantenimiento = RegistroBD.CuotaMantenimiento,
                                PagoTotalCptosRenta = RegistroBD.PagoTotalCptosRenta.Value,
                                PtjeImpuesto = RegistroBD.PtjeImpuesto,

                                InmuebleArrto = new ModeloNegocios.InmuebleArrto
                                {
                                    RIUFInmueble = RegistroBD.RIUF,
                                    IdInstitucion = RegistroBD.Fk_IdInstitucion,
                                    NombreInmueble = RegistroBD.NombreInmueble,
                                    IdPais = RegistroBD.Fk_IdPais,
                                    IdEstado = RegistroBD.Fk_IdEstado, //posible: null
                                    IdMunicipio = RegistroBD.Fk_IdMunicipio,//posible: null
                                    IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,//posible: null
                                    OtraColonia = RegistroBD.OtraColonia,
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
                                    MunicipioExtranjero = RegistroBD.MunicipioExtranjero
                                },

                                JustripreciacionContrato = new ModeloNegocios.JustripreciacionContrato
                                {
                                    strFechaDictamen = RegistroBD.FechaDictamen,
                                    IdJustipreciacion = RegistroBD.IdJustipreciacion,
                                    MontoDictaminado = RegistroBD.MontoDictaminado,
                                    NoGenerico = RegistroBD.NoGenerico,
                                    Secuencial = RegistroBD.Secuencial,
                                    SuperficieDictaminada = RegistroBD.SuperficieDictaminada,
                                    EstatusAtencion = RegistroBD.EstatusAtencion
                                },

                                PersonaReferenciaResponsableOcupacion = new ModeloNegocios.PersonaReferencia
                                {
                                    NombreCargo = RegistroBD.OFONombreCargo,
                                    Nombre = RegistroBD.OFONombre,
                                    ApellidoPaterno = RegistroBD.OFOApellidoPaterno,
                                    ApellidoMaterno = RegistroBD.OFOApellidoMaterno,
                                    Email = RegistroBD.OFOEmail
                                },

                                PersonaReferenciaTitularOIC = new ModeloNegocios.PersonaReferencia
                                {
                                    NombreCargo = RegistroBD.OICNombreCargo,
                                    Nombre = RegistroBD.OICNombre,
                                    ApellidoPaterno = RegistroBD.OICApellidoPaterno,
                                    ApellidoMaterno = RegistroBD.OICApellidoMaterno,
                                    Email = RegistroBD.OICEmail
                                },

                            }).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerContratosArrtoRegistrados DAL:{0}", ex.Message));
                }


            }//using
            return ListContratosArrtoRegistrados;
        }

        //Obtener Excepciones de Normativada para ContratoArrto. registrados 
        public List<ModeloNegocios.ContratoArrto> ObtenerExcepcionNormatividadContratosArrtoRegistrados(int? IdInstitucion, int? FolioContratoArrto, byte? TipoContato)
        {
            List<ModeloNegocios.ContratoArrto> ListContratosArrtoRegistrados;
            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ListContratosArrtoRegistrados = Conn.spuSelectExcepcionNormativaContratoArrto(IdInstitucion, FolioContratoArrto, TipoContato)
                   .Select(RegistroBD => new ModeloNegocios.ContratoArrto
                   {
                       //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                       NombreInstitucion = RegistroBD.NombreInstitucion,
                       FolioContratoArrto = RegistroBD.FolioContratoArrto,
                       Fk_IdUsuarioRegistro = RegistroBD.Fk_IdUsuarioRegistro,
                       CargoUsuarioRegistro = RegistroBD.CargoUsuarioRegistro,
                       strFechaRegistro = RegistroBD.FechaRegistro,
                       PeriodoOcupacion = RegistroBD.PeriodoContratacion,
                       PagoTotalCptosRenta = RegistroBD.TotalRentaUnitaria.Value,
                       DescripcionExcepcionTipoNormativa = RegistroBD.DescripcionExcepcionTipoNormativa,
                       ObservacionesExcepcionNormativa = RegistroBD.ObservacionesExcepcionNormativa,

                       DescripcionTipoContrato = RegistroBD.DescripcionTipoContrato,


                       InmuebleArrto = new ModeloNegocios.InmuebleArrto
                       {

                           NombreInmueble = RegistroBD.NombreInmueble,
                           IdPais = RegistroBD.Fk_IdPais,
                           IdEstado = RegistroBD.Fk_IdEstado, //posible: null
                           IdMunicipio = RegistroBD.Fk_IdMunicipio,//posible: null
                           IdLocalidadColonia = RegistroBD.Fk_IdLocalidad,//posible: null
                           OtraColonia = RegistroBD.OtraColonia,
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
                           MunicipioExtranjero = RegistroBD.MunicipioExtranjero

                       },

                   }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerExcepcionNormatividadContratosArrtoRegistrados: {0}", ex.Message));
                }

            }//using
            return ListContratosArrtoRegistrados;
        }

        //devuelve un valor scalar: un string
        public string ObteneExcepcionNormatividadPreviaContrato(byte TipoContrato, decimal AreaOcupadaM2, decimal RentaMensualUnitaria,
                         int? FolioEmisionOpinion, string SuperficieDictaminada_Justipreciacion,
                         decimal? MontoDictaminado_Justipreciacion, string NumeroDictamenExcepcionFolioSMOI)
        {
            string strValorParametro;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    strValorParametro = Conn.spuSelectExcepcionNormatividadPreviaContrato(TipoContrato, AreaOcupadaM2, RentaMensualUnitaria,
                   FolioEmisionOpinion, SuperficieDictaminada_Justipreciacion, MontoDictaminado_Justipreciacion,
                   NumeroDictamenExcepcionFolioSMOI).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObteneExcepcionNormatividadPreviaContrato: {0}", ex.Message));
                }

            }//using
            return strValorParametro;
        }


        //RCA 13/08/2018
        //busqueda del id de contrato arto 
        public int IdContratoArrto(string FolioContrato)
        {
            int IdContrato = 0;

            int folio = Convert.ToInt32(FolioContrato);

            using (ArrendamientoInmuebleEntities conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var id = conexion.ContratoArrto.Where(x => x.FolioContratoArrto == folio).Select(x => new { x.IdContratoArrto }).FirstOrDefault();

                    IdContrato = id.IdContratoArrto;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("IdContratoArrto:{0}", ex.Message));
                }
            }

            return IdContrato;
        }

        //metodo para actualizar el campo de QR de las tablas contrato
        public Boolean ActualizarConytratoQR(string QR, int IdAplicacionContrato)
        {
            bool ok = false;

            using (var conexion = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var QRContrato = (from s in conexion.SelloDigital
                                      where s.Fk_IdRegistroTablaOrigen == IdAplicacionContrato && s.Fk_IdCatTabla == 2
                                      select s).FirstOrDefault();

                    QRContrato.QR = QR;

                    conexion.SaveChanges();

                    ok = true;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ActualizarConytratoQR:{0}", ex.Message));
                }
            }

            return ok;
        }

        public List<ModeloNegocios.InmuebleArrto> ObtenerContratosConvenioModificatorio(int IdInstitucion, int FolioContrato, string RIUF, int IdPais, int IdEdo, int IdMunicipio)
        {
            List<ModeloNegocios.InmuebleArrto> LInmuebleArrto = new List<ModeloNegocios.InmuebleArrto>();

            try
            {
                using (ArrendamientoInmuebleEntities aInmuebles = new ArrendamientoInmuebleEntities())
                {
                    LInmuebleArrto = aInmuebles.spuObtenerInmueblesConvenioModificatorio(IdInstitucion, FolioContrato, RIUF, IdPais, IdEdo, IdMunicipio).Select(x => new ModeloNegocios.InmuebleArrto
                    {
                        IdInmuebleArrendamiento = x.IdInmuebleArrendamiento == null ? 0 : x.IdInmuebleArrendamiento.Value,
                        NombreInmueble = x.NombreInmueble,
                        FolioContratoArrto = x.FolioContratoArrto == null ? 0 : x.FolioContratoArrto.Value,
                        IdInstitucion = x.IdInstitucion == null ? 0 : x.IdInstitucion.Value,
                        IdPais = x.IdPais == null ? 0 : x.IdPais.Value,
                        IdEstado = x.IdEstado,
                        IdMunicipio = x.IdMunicipio,
                        IdTipoInmueble = x.IdTipoInmueble == null ? 0 : x.IdTipoInmueble.Value,
                        OtraColonia = x.OtraColonia,
                        IdTipoVialidad = x.IdTipoVialidad == null ? 0 : x.IdTipoVialidad.Value,
                        NombreVialidad = x.NombreVialidad,
                        NumExterior = x.NumExterior,
                        NumInterior = x.NumInterior,
                        CodigoPostal = x.CodigoPostal,
                        CodigoPostalExtranjero = x.CodPostalExtranjero,
                        RIUFInmueble = x.RIUF,

                        ContratoArrtoInmueble = new ModeloNegocios.ContratoArrto
                        {
                            IdContratoArrto = x.IdContrato == null ? 0 : x.IdContrato.Value,
                            IdConvenio = x.IdConvenio == null ? 0 : x.IdConvenio.Value,
                            IdTipoArrendamiento = x.IdTipoArrendamiento == null ? 0 : x.IdTipoArrendamiento.Value,
                            DescripcionTipoArrendamiento = x.DescTipoArrendamiento
                        }

                    }).ToList();
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerContratosConvenioModificatorio:{0}", ex.Message));
            }

            return LInmuebleArrto;
        }

        public Convenio ObtenerConvenioModificatorio(int IdContrato, int IdConvenio)
        {
            Convenio Convenio = new Convenio();

            try
            {
                using (ArrendamientoInmuebleEntities aInmueble = new ArrendamientoInmuebleEntities())
                {
                    Convenio = aInmueble.Convenio_Modificatorio.Where(x => x.Fk_IdContratoArrto == IdContrato && x.IdConvenio == IdConvenio).Select(x => new Convenio
                    {
                        IdConvenio = x.IdConvenioModif,
                        FolioContrato = x.ContratoArrto.FolioContratoArrto,
                        ConsecutivoConvenio = x.IdConvenio,
                        FechaConvenio = x.FechaConvenio.Value,
                        FechaTermino = x.FechaTerminacion,
                        ImporteRenta = x.Importe_Renta,
                        //FechaInicioImporte = x.FechaImporte_Renta,
                        Secuencial = x.Fk_IdJustipreciacion,
                        NombreOIC = x. Nombre,
                        PApellidoOIC = x.Primer_Apellido,
                        SApellidoOIC = x.Segundo_Apellido,
                        CargoOIC = x.Nombre_Cargo,
                        CorreoOIC = x.Email,
                    }).FirstOrDefault();

                    Convenio.FolioConvenio = Convenio.FolioContrato + "-" + Convenio.ConsecutivoConvenio;
                    Convenio.descFechaConvenio = Convenio.FechaConvenio.ToString("d");

                    if (Convenio.FechaTermino != null)
                        Convenio.descFechaTermino = Convenio.FechaTermino.Value.ToString("d");

                    if (Convenio.FechaInicioImporte != null)
                        Convenio.descFechaInicioImporte = Convenio.FechaInicioImporte.Value.ToString("d");
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerConvenioModificatorio:{0}", ex.Message));
            }

            return Convenio;
        }

        public bool GenerarConvenioModificatorio(Convenio Convenio, int IdUsuario, JustripreciacionContrato JustripreciacionContrato, ref string msjError, ref string fechaRegistro)
        {
            bool respuesta = false;
            int IdConvenio = 0;

            try
            {
                using (ArrendamientoInmuebleEntities aInmuebles = new ArrendamientoInmuebleEntities())
                {
                    Datos.ContratoArrto Contrato = aInmuebles.ContratoArrto.Where(x => x.FolioContratoArrto == Convenio.FolioContrato && x.EstatusRegistro == true).FirstOrDefault();

                    if (Contrato == null)
                    {
                        msjError = "No se encuentra el registro del contrato. Favor de contactar a tu administrador";
                        return false;
                    }

                    if (Convenio.TieneNvoMonto == 1 && Convenio.ImporteRenta > Constantes.MONTO_MINIMO_SECUENCIAL)
                    {                        
                        JustipreciacionArrto justipreciacion = aInmuebles.JustipreciacionArrto.Where(x => x.Fk_IdContratoArrto == Contrato.IdContratoArrto && x.EstatusRegistro == true).FirstOrDefault();

                        if (justipreciacion != null)
                        {
                            if (Convenio.Secuencial == justipreciacion.Secuencial)
                            {
                                msjError = "El secuencial no puede ser igual al secuencial del contrato. Favor de validar tus datos";
                                return false;
                            }

                            List<Convenio_Modificatorio> Lconvenio = aInmuebles.Convenio_Modificatorio.Where(x => x.Fk_IdJustipreciacion == Convenio.Secuencial).ToList();

                            if (Lconvenio != null)
                            {
                                if (Lconvenio.Count > 0)
                                {
                                    msjError = "El secuencial ingresado se encuentra relacionado a otro convenio modificatorio. Favor de validar tus datos";
                                    return false;
                                }
                            }
                        }
                    }

                    IdConvenio = aInmuebles.Convenio_Modificatorio.Where(x => x.Fk_IdContratoArrto == Contrato.IdContratoArrto).Count();
                    IdConvenio = IdConvenio + 1;

                    using (TransactionScope transaccion = new TransactionScope())
                    {
                        Convenio_Modificatorio convenio = new Convenio_Modificatorio();
                        convenio.Fk_IdContratoArrto = Contrato.IdContratoArrto;
                        convenio.IdConvenio = (short)IdConvenio;
                        convenio.FechaConvenio = Convert.ToDateTime(Convenio.descFechaConvenio);
                        convenio.Nombre = Convenio.NombreOIC;
                        convenio.Primer_Apellido = Convenio.PApellidoOIC;
                        convenio.Segundo_Apellido = Convenio.SApellidoOIC;
                        convenio.Nombre_Cargo = Convenio.CargoOIC;
                        convenio.Email = Convenio.CorreoOIC;
                        convenio.FechaEfecConvenio = Convert.ToDateTime(Convenio.DescFechaEfectoConvenio);

                        if (Convenio.TieneProrroga == 1)
                            convenio.FechaTerminacion = Convert.ToDateTime(Convenio.descFechaTermino);

                        if (Convenio.TieneNvaSuperfice == 1)
                            convenio.Nueva_Superficie = Convenio.SupM2;

                        if (Convenio.TieneNvoMonto == 1)                        
                            convenio.Importe_Renta = Convenio.ImporteRenta;
                            
                        

                        if (Convenio.TieneNvoMonto == 1 && Convenio.ImporteRenta > Constantes.MONTO_MINIMO_SECUENCIAL)
                            convenio.Fk_IdJustipreciacion = Convenio.Secuencial;

                        convenio.FechaRegistro = DateTime.Now;
                        convenio.Institución_justipreciacion = 1;

                        aInmuebles.Convenio_Modificatorio.Add(convenio);
                        aInmuebles.SaveChanges();
                        fechaRegistro = convenio.FechaRegistro.ToString("d");
                        Convenio.FechaRegistro = convenio.FechaRegistro;

                        Convenio.IdConvenio = convenio.IdConvenioModif;
                        Convenio.ConsecutivoConvenio = convenio.IdConvenio;

                        if (Convenio.TieneNvoMonto == 1 && Convenio.ImporteRenta > Constantes.MONTO_MINIMO_SECUENCIAL)                        
                            convenio.Fk_IdJustipreciacion = Convenio.Secuencial;                                                                                                       

                        aInmuebles.SaveChanges();
                       
                        transaccion.Complete();
                        respuesta = true;
                    }
                    
                    Convenio.ConsecutivoConvenio = IdConvenio;
                    Convenio.FolioConvenio = Convenio.FolioContrato + "-" + IdConvenio.ToString().PadLeft(2,'0');
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("GenerarConvenioModificatorio:{0}", ex.Message));
            }

            return respuesta;
        }

        public bool AutorizarConvenioModificatorio(int IdConvenioModificatorio, string CadOrignal, string Sello, string QR, int IdUsuario, ref string fechaRegistro)
        {
            bool respuesta = false;

            try
            {
                using (ArrendamientoInmuebleEntities aInmueble = new ArrendamientoInmuebleEntities())
                {
                    Convenio_Modificatorio cModificatorio = aInmueble.Convenio_Modificatorio.Where(x => x.IdConvenioModif == IdConvenioModificatorio).FirstOrDefault();

                    if (cModificatorio != null)
                    {
                        using (TransactionScope transaccion = new TransactionScope())
                        {
                            SelloDigital sello = new SelloDigital
                            {
                                Fk_IdCatTabla = 4,
                                Fk_IdRegistroTablaOrigen = IdConvenioModificatorio,
                                CadenaOriginal = CadOrignal,
                                SelloDigital1 = Sello,
                                GUID = Guid.NewGuid().ToString(),
                                EstatusRegistro = true,
                                FechaRegistro = DateTime.Now,
                                Fk_IdUsuarioRegistro = IdUsuario,
                                QR = QR
                            };

                            aInmueble.SelloDigital.Add(sello);
                            aInmueble.SaveChanges();

                            fechaRegistro = sello.FechaRegistro.ToString("d");

                            transaccion.Complete();
                            respuesta = true;
                        }                            
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("GenerarConvenioModificatorio:{0}", ex.Message));
            }

            return respuesta;
        }

        public List<Convenio> ObtenerConveniosContrato(int FolioContrato)
        {
            List<Convenio> Lconvenio = new List<Convenio>();            

            try
            {
                using (ArrendamientoInmuebleEntities aInmueble = new ArrendamientoInmuebleEntities())
                {                    
                    Lconvenio = aInmueble.Convenio_Modificatorio.Where(x => x.ContratoArrto.FolioContratoArrto == FolioContrato).Select(x => new Convenio
                    {
                        IdConvenio = x.IdConvenioModif,
                        ConsecutivoConvenio = x.IdConvenio,                        
                    }).ToList();

                    foreach (Convenio Convenio in Lconvenio)                    
                        Convenio.FolioConvenio = FolioContrato + "-" + Convenio.ConsecutivoConvenio.ToString().PadLeft(2, '0');                    
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerConveniosContrato:{0}", ex.Message));
            }

            return Lconvenio;
        }

        public Convenio ObtenerConvenioModificatorio(string folioConvenio, ref string msjError)
        {
            Convenio Convenio = new Convenio();
            int folioContrato = 0;
            int IdConvenio = 0;

            try
            {
                int.TryParse(folioConvenio.Split('-')[0], out folioContrato);
                int.TryParse(folioConvenio.Split('-')[1], out IdConvenio);

                if (folioContrato == 0 || IdConvenio == 0)
                {
                    msjError = "No se encuentra la información del convenio modificatorio. Favor de contactar a tu administrador";
                    return new Convenio();
                }

                using (ArrendamientoInmuebleEntities aInmueble = new ArrendamientoInmuebleEntities())
                {
                    Convenio = aInmueble.Convenio_Modificatorio.Where(x => x.ContratoArrto.FolioContratoArrto == folioContrato && x.IdConvenio == IdConvenio).Select(x => new Convenio
                    {
                        IdConvenio = x.IdConvenioModif,
                        IdInmueble = x.ContratoArrto.Fk_IdInmuebleArrendamiento,
                        FolioConvenio = folioConvenio,
                        FolioContrato = x.ContratoArrto.FolioContratoArrto,
                        ConsecutivoConvenio = x.IdConvenio,
                        FechaConvenio = x.FechaConvenio == null ? new DateTime() : x.FechaConvenio.Value,
                        FechaTermino = x.FechaTerminacion,
                        SupM2 = x.Nueva_Superficie,
                        Secuencial = x.Fk_IdJustipreciacion,
                        ImporteRenta = x.Importe_Renta,
                        FechaEfectoConvenio = x.FechaEfecConvenio,
                        NombreOIC = x.Nombre,
                        PApellidoOIC = x.Primer_Apellido,
                        SApellidoOIC = x.Segundo_Apellido,
                        CargoOIC = x.Nombre_Cargo,
                        CorreoOIC = x.Email,
                        FechaRegistro = x.FechaRegistro,
                    }).FirstOrDefault();

                    Convenio.descFechaTermino = Convenio.FechaTermino == null ? "" : Convenio.FechaTermino.Value.ToString("d");
                    Convenio.descFechaRegistro = Convenio.FechaRegistro.ToString("d");
                    Convenio.DescFechaEfectoConvenio = Convenio.FechaEfectoConvenio.ToString("d");

                    SelloDigital sDigital = aInmueble.SelloDigital.Where(x => x.EstatusRegistro == true && x.Fk_IdCatTabla == 4 && x.Fk_IdRegistroTablaOrigen == Convenio.IdConvenio).FirstOrDefault();

                    Convenio.cadOriginal = sDigital.CadenaOriginal;
                    Convenio.Sello = sDigital.SelloDigital1;
                    Convenio.QR = sDigital.QR;
                    Convenio.descFechaAutorizacion = sDigital.FechaRegistro.ToString("d");


                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerConvenioModificatorio:{0}", ex.Message));
            }

            return Convenio;
        }
    }
}
