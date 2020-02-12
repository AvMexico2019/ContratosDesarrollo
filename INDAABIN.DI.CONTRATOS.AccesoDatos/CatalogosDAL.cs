using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Transactions;
//comunicacion con las capas
using INDAABIN.DI.CONTRATOS.Datos;
using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.ModeloNegocios.Catalogos;


namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class CatalogosDAL
    {

        //Catalogo de TipoContrato: Nac. Ext, otrasFig
        public List<TipoContrato> ObtenerCptosTipoContrato()
        {
            List<TipoContrato> listaTipoContrato = new List<TipoContrato>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaTipoContrato = Conn.Cat_TipoContrato.Select(a => new TipoContrato
                    {
                        IdTipoContrato = a.IdTipoContrato,
                        DescripcionTipoContrato = a.DescripcionTipoContrato
                    }
                ).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerCptosTipoContrato: {0}", ex.Message));
            }
            return listaTipoContrato;

        }


        //Nuevo, Sust. Continuacion
        public List<TipoArrendamiento> ObtenerCptosTipoArrendamiento()
        {
            List<TipoArrendamiento> listaTipoArrendamiento = new List<TipoArrendamiento>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaTipoArrendamiento = Conn.Cat_TipoArrendamiento.Select(a => new TipoArrendamiento
                    {
                        IdTipoArrendamiento = a.IdTipoArrendamiento,
                        DescTipoArrendamiento = a.DescripcionTipoArrendamiento
                    }
                        ).ToList();
                }
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerCptosTipoArrendamiento: {0}", ex.Message));
            }
            return listaTipoArrendamiento;

        }

        //Catalogo de  Tema: SMOI, Opionion Nuevo arrto...
        public List<TemaConcepto> ObtenerTemasCptos()
        {
            List<TemaConcepto> listaTemasCpto = new List<TemaConcepto>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaTemasCpto = Conn.Cat_Tema.Select(a => new TemaConcepto
                    {
                        IdTema = a.IdTema,
                        DescripcionTema = a.DescripcionTema
                    }
                        ).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerTemasCptos: {0}", ex.Message));
            }
            return listaTemasCpto;

        }


        //Catalogo de  Tipo Contratacion: Automatico, Dictaminado...
        public List<ModeloNegocios.TipoContratacion> ObtenerTipoContratacion()
        {
            List<TipoContratacion> listaTipoContratacion = new List<TipoContratacion>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaTipoContratacion = Conn.Cat_TipoContratacion.Where(x => x.EstatusRegistro == true).Select(a => new TipoContratacion
                    {
                        IdTipoContratacion = a.IdTipoContratacion,
                        DescripcionTipoContratacion = a.DescripcionTipoContratacion,
                        Orden = a.Orden == null ? 0 : a.Orden.Value,
                    }
                    //RCA 08/01/2018
                    // se agrego que la lista se ordene de manera ascendente de acuerdo al id
                        ).OrderBy(a => a.Orden).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerTipoContratacion: {0}", ex.Message));
            }
            return listaTipoContratacion;

        }

        //Catalogo de  Tipo Ocupacion (aplica a Otras Fig Ocupacion): Comodato, Prestamo... 
        public List<TipoOcupacion> ObtenerTipoOcupacion()
        {
            List<TipoOcupacion> listaTipoOcupacion = new List<TipoOcupacion>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaTipoOcupacion = Conn.Cat_TipoOcupacion.Select(a => new TipoOcupacion
                    {
                        IdTipoOcupacion = a.IdTipoOcupacion,
                        DescripcionTipoOcupacion = a.DescripcionTipoOcupacion
                    }
                        ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerTipoOcupacion: {0}", ex.Message));
            }
            return listaTipoOcupacion;

        }

        //Catalogo de Documentos
        public List<ModeloNegocios.Documento> ObtenerDocumentos(int IdTipoDocumento)
        {
            List<Documento> listaDocumentos = new List<Documento>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaDocumentos = Conn.Cat_Documento.Select(a => new Documento
                    {
                        IdDocumento = a.IdDocumento,
                        IdTipoDocumento = a.Fk_IdTipoDocumento,
                        DescripcionDocumento= a.DescripcionDocumento,
                        NombreDocumento = a.NombreDocumento,
                        URLDocumento = a.URLDocumento,
                        RutaDocumento = a.RutaDocumento,
                        EstatusRegistro = a.EstatusRegistro
                    }
                    ).Where(a => a.IdTipoDocumento == IdTipoDocumento && a.EstatusRegistro == true).ToList();

                }
                //return listaDocumentos.Where(a => a.IdTipoDocumento == IdTipoDocumento).ToList();
                //return listaDocumentos;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerDocumentos: {0}", ex.Message));
            }
            return listaDocumentos;
        }

        public List<ModeloNegocios.CampoReporte> ObtenerCamposReporte(int IdReporte)
        {
            List<CampoReporte> listaCampos = new List<CampoReporte>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaCampos = Conn.Cat_CampoReporte.Select(a => new CampoReporte
                    {
                        IdCampoReporte = a.IdCampoReporte,
                        IdReporte = a.Fk_IdReporte,
                        OrdenCampoReporte = a.OrdenCampoReporte,
                        NombreCampoReporte = a.NombreCampoReporte,
                        DescripcionCampoReporte = a.DescripcionCampoReporte,
                        EstatusRegistro = a.EstatusRegistro
                    }
                    ).Where(a => a.IdReporte == IdReporte && a.EstatusRegistro == true).OrderBy(o => o.OrdenCampoReporte).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerCamposReporte: {0}", ex.Message));
            }
            return listaCampos;
        }

        //devuelve un valor scalar, correspondiente al valor de un parametro.
        public string ObtenerValorCatParametro(string ParametroNombre)
        {
            string strValorParametro;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                //try
                //{
                    strValorParametro = Conn.spuObtenerValorParametro(ParametroNombre).FirstOrDefault();
                //}
                //catch(Exception ex)
                //{
                //    throw new Exception(string.Format("ObtenerValorCatParametro: {0}", ex.Message));
                //}
                
            }

            return strValorParametro;
        }

        public Parametro ObtenerParametroNombre(string NombreParametro)
        {
            Parametro Parametro = new Parametro();

            try
            {
                using (ArrendamientoInmuebleEntities aInmueble = new ArrendamientoInmuebleEntities())
                {
                    Parametro = aInmueble.Cat_Parametro.Where(x => x.ParametroNombre == NombreParametro && x.EstatusRegistro == true).Select(x => new Parametro
                    {
                        IdParametro = x.IdParametro,
                        NombreParametro = x.ParametroNombre,
                        DescripcionParametro = x.DescripcionParametro,
                        ValorParametro = x.ValorParametro,
                        ValorAdicional = x.ValorAdicionalParametro
                    }).FirstOrDefault();
                }
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerParametroNombre: {0}", ex.Message));
            }

            return Parametro;
        }


    } //clase
}
