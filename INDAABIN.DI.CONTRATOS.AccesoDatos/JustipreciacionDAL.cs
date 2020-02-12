using INDAABIN.DI.CONTRATOS.Datos;
using INDAABIN.DI.CONTRATOS.ModeloNegocios;
//using INDAABIN.DI.CONTRATOS.ModeloNegocios.ContratoArrto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    //MZT 16/08/2017
    public class JustipreciacionDAL
    {
        public Boolean Inserta(SolicitudAvaluosExt avaluo)
        {
            bool bandera = true;

            using (ArrendamientoInmuebleEntities db = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var item = db.JustipreciacionExt.FirstOrDefault(x => x.Secuencial.Equals(avaluo.NoSecuencial));

                    if (item == null)
                    {
                        //RCA 22/08/2017
                        db.JustipreciacionExt.Add(new JustipreciacionExt
                        {
                            Secuencial = avaluo.NoSecuencial,
                            NoGenerico = avaluo.NoGenerico,
                            FechaDictamen = avaluo.FechaDictamen,// los dos son tipo date time 
                            UnidadResponsable = avaluo.UnidadResponsable,
                            TerrenoDictaminado = avaluo.SuperficieTerrenoDictaminado,
                            Fk_IdUnidadMedidaTerrenoDict = short.Parse(avaluo.UnidadMedidaTerrenoDictaminado),
                            RentableDictamindo = avaluo.SuperficieRentableDictaminado,
                            Fk_IdUnidadMedidaRentableDict = short.Parse(avaluo.UnidadMedidaRentableDictaminado),
                            ConstruidaDictaminado = avaluo.SuperficieConstruidaDictaminado,
                            Fk_IdUnidadMedidaConstruidaDict = short.Parse(avaluo.UnidadMedidaConstruidaDictaminado),
                            MontoDictaminado = avaluo.MontoDictaminado,
                            Fk_IdSector = (short)avaluo.SectorId,
                            Fk_IdInstitucion = (short)avaluo.InstitucionId,
                            Calle = avaluo.Calle,
                            NumExterior = avaluo.NoExterior,
                            NumInterior = avaluo.NoInterior,//puede ser nulo 
                            Colonia = avaluo.ColoniaInmueble,
                            CodigoPostal = avaluo.CP,
                            Fk_IdEstado = (short)avaluo.EstadoId,
                            Fk_IdMunicipio = (short)avaluo.MunicipioId,
                            RutaDocumento = avaluo.RutaDocumento,//debe de guardar la ruta del documento 
                            EstatusRegistro = true,//modificar despues cuando se borre o actualize el dato
                            Fk_IdUsuarioRegistro = avaluo.IdUsuarioRegistro,//poder el usuario de lupita
                            FechaRegistro = System.DateTime.Now, //la fecha del servidor 

                        });

                        try
                        {
                            db.SaveChanges();
                            //bandera = true;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Inserta: {0}", ex.Message));
                            //bandera = false;
                        }

                    }
                    else
                    {
                        bandera = false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Inserta metodo: ", ex.Message));
                }
               
            }//using
            return bandera;
        }

        public static void Actualiza(SolicitudAvaluosExt avaluo)
        {
            using (ArrendamientoInmuebleEntities db = new ArrendamientoInmuebleEntities())
            {
                var item = db.JustipreciacionExt.FirstOrDefault(x => x.Secuencial.Equals(avaluo.NoSecuencial));

                if (item != null)
                {
                    db.Entry<JustipreciacionExt>(item).State = System.Data.Entity.EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Actualiza: {0}", ex.Message));
                    }
                }
            }
        }
        public static void Elimina(string secuencail)
        {
            using (ArrendamientoInmuebleEntities db = new ArrendamientoInmuebleEntities())
            {
                var item = db.JustipreciacionExt.FirstOrDefault(x => x.Secuencial.Equals(secuencail));

                if (item != null)
                {
                    db.JustipreciacionExt.Remove(item);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Elimina: {0}", ex.Message));
                    }
                }
            }
        }

        public static SolicitudAvaluosExt Consulta(string secuencial)
        {
            SolicitudAvaluosExt avaluo = null;

            using (ArrendamientoInmuebleEntities db = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    avaluo = db.JustipreciacionExt
                        .Where(x => x.Secuencial.Equals(secuencial))
                    .Select(x => new SolicitudAvaluosExt
                    {
                        Calle = x.Calle,
                        CP = x.CodigoPostal,
                        EstadoId = x.Fk_IdEstado,
                        FechaDictamen = x.FechaDictamen,
                        InstitucionId = x.Fk_IdInstitucion,
                        MontoDictaminado = x.MontoDictaminado,
                        MunicipioId = x.Fk_IdMunicipio,
                        NoExterior = x.NumExterior,
                        NoGenerico = x.NoGenerico,
                        NoInterior = x.NumInterior,
                        NoSecuencial = x.Secuencial,
                        SectorId = x.Fk_IdSector,
                        SuperficieConstruidaDictaminado = x.ConstruidaDictaminado,
                        SuperficieRentable = x.RentableDictamindo, //se quito de la vase de datos 
                        SuperficieRentableDictaminado = x.RentableDictamindo,
                        SuperficieTerrenoDictaminado = x.TerrenoDictaminado,
                        UnidadMedidaRentable = x.Fk_IdUnidadMedidaRentableDict.ToString(),
                        UnidadMedidaRentableDictaminado = x.Fk_IdUnidadMedidaRentableDict.ToString(),
                    })
                    .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Consulta: {0}", ex.Message));
                }
            }
            return avaluo;
        }

        public static List<ModeloNegocios.SolicitudAvaluosExt> ObtenerJustipreciacionesRegistrados(Filtro filtro)
        {
            List<ModeloNegocios.SolicitudAvaluosExt> ListJustipreciacionesRegistrados = null;
            ListJustipreciacionesRegistrados = null;
            using (ArrendamientoInmuebleEntities db = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    var result = db.JustipreciacionExt
                        .Select(x => new SolicitudAvaluosExt
                        {
                            Calle = x.Calle,
                            CP = x.CodigoPostal,
                            EstadoId = x.Fk_IdEstado,
                            FechaDictamen = x.FechaDictamen,
                            InstitucionId = x.Fk_IdInstitucion,
                            MontoDictaminado = x.MontoDictaminado,
                            MunicipioId = x.Fk_IdMunicipio,
                            NoExterior = x.NumExterior,
                            NoGenerico = x.NoGenerico,
                            NoInterior = x.NumInterior,
                            NoSecuencial = x.Secuencial,
                            SectorId = x.Fk_IdSector,
                            SuperficieConstruidaDictaminado = x.ConstruidaDictaminado,
                            SuperficieRentable = x.RentableDictamindo, //se quito de la base de datos 
                            SuperficieRentableDictaminado = x.RentableDictamindo,
                            SuperficieTerrenoDictaminado = x.TerrenoDictaminado,
                            UnidadMedidaRentable = x.Fk_IdUnidadMedidaRentableDict.ToString(),
                            UnidadMedidaRentableDictaminado = x.Fk_IdUnidadMedidaRentableDict.ToString(),

                        });

                    if (!string.IsNullOrEmpty(filtro.NoSecuencial))
                    {

                        ListJustipreciacionesRegistrados = result.Where(x => x.NoSecuencial.Equals(filtro.NoSecuencial, StringComparison.InvariantCultureIgnoreCase))
                                .ToList();
                    }
                    else if (!string.IsNullOrEmpty(filtro.NoGenerico))
                    {

                        ListJustipreciacionesRegistrados = result.Where(x => x.NoGenerico.Equals(filtro.NoGenerico, StringComparison.InvariantCultureIgnoreCase))
                            .ToList();
                    }
                    else if (filtro.FechaRegistro != null)
                    {
                        ListJustipreciacionesRegistrados = result.Where(x => x.FechaRegistro.ToString("d") == filtro.FechaRegistro.Value.ToString("d"))
                            .ToList();
                    }
                    else if (filtro.FechaDictamen != null)
                    {
                        ListJustipreciacionesRegistrados = result.Where(x => x.FechaDictamen.ToString("d") == filtro.FechaDictamen.Value.ToString("d"))
                            .ToList();
                    }
                    else
                    {
                        ListJustipreciacionesRegistrados = result.ToList();
                    }
                    // mas lo que le quieras agregar al filtro
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Consulta: {0}", ex.Message));
                }
            }
            return ListJustipreciacionesRegistrados;
        }
    }
}
