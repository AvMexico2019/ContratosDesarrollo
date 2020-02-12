using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//comunicacion con las capas 
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //entities
using INDAABIN.DI.CONTRATOS.AccesoDatos;
using INDAABIN.DI.ModeloNegocio; //interconexion al BUS
using INDAABIN.DI.CONTRATOS.ModeloNegocios.Catalogos;

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class NG_Catalogos
    {
       
        //obtener cptos de catalogo de Tipos de Arrto. : Nuevo, Continuacion o Sustitucion
        public List<TipoArrendamiento> ObtenerCptosTipoArrendamiento()
        {
                        
           AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
           List<TipoArrendamiento> ListCptosTiposArrendamiento;
           ListCptosTiposArrendamiento = Conn.ObtenerCptosTipoArrendamiento();
           
            return ListCptosTiposArrendamiento;
        }

        //obtener cptos de catalogo de: Tema de Conceptos Valor/Resp
        public List<TemaConcepto> ObtenerTemaCptos()
        {

            AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
            List<TemaConcepto> listaTemasCpto = Conn.ObtenerTemasCptos();
            return listaTemasCpto;
        }

        public byte ObtenerIdTemaXDesc(string DescTema)
        {
            List<TemaConcepto> ListTemaCptos;
            TemaConcepto objTemaConceptoNuevoArrto;
           
            ListTemaCptos = new NG_Catalogos().ObtenerTemaCptos();
            objTemaConceptoNuevoArrto = (from x in ListTemaCptos
                                            where x.DescripcionTema == DescTema
                                            select x).FirstOrDefault();

            return objTemaConceptoNuevoArrto.IdTema;
            
          }

         public string ObtenerValorCatParametro(string ParametroNombre)
        {
            string strValorParametro;
            AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
            strValorParametro = Conn.ObtenerValorCatParametro(ParametroNombre);
            return strValorParametro;
        }

         //obtener cptos de catalogo de tipo de contratato: Nacional. Extranjero o Otras Fig. Publicas
         public List<TipoContrato> ObtenerTipoContrato()
         {

             AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
             List<TipoContrato> listaTipoContrato = Conn.ObtenerCptosTipoContrato();
             return listaTipoContrato;
         }

         //obtener cptos de catalogo de tipo de CONTRATACIÓN : Automatico, dictaminado...
         public List<TipoContratacion> ObtenerTipoContratacion()
         {

             AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
             List<TipoContratacion> listaTipoContratacion = Conn.ObtenerTipoContratacion();
             return listaTipoContratacion;
         }

         //obtener cptos de catalogo de tipo de ocupacion (aplicable solo al tipo Contrato de: Otras Fig. Ocupacion): ComoDato...
         public List<TipoOcupacion> ObtenerTipoOcupacion()
         {

             AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
             List<TipoOcupacion> listaTipoOcupacion = Conn.ObtenerTipoOcupacion();
             return listaTipoOcupacion;
         }

         public List<Documento> ObtenerDocumentos(int IdTipoDocumento)
         {
             AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
             List<Documento> listaDocumentos = Conn.ObtenerDocumentos(IdTipoDocumento);
             return listaDocumentos;
         }

         public List<CampoReporte> ObtenerCamposReporte(int IdReporte)
         {
             AccesoDatos.CatalogosDAL Conn = new AccesoDatos.CatalogosDAL();
             List<CampoReporte> listaCamposReporte = Conn.ObtenerCamposReporte(IdReporte);
             return listaCamposReporte;
         }

         public List<FiltroXCP> ObtenerLocalidades(string codigoPostal = "0", int IdPais = 0, int IdEstado = 0, int IdMunicipio = 0, int IdLocalidad = 0)
         {
             ControladorBUS ws_bus = new ControladorBUS();

             List<FiltroXCP> listaLocalidades = null;

             listaLocalidades = ws_bus.ObtenerCatalogoLocalidades(codigoPostal, IdPais, IdEstado, IdMunicipio, IdLocalidad);

             return listaLocalidades;
         }

        public Parametro ObtenerParametroNombre(string NombreParametro)
        {
            Parametro Parametro = new Parametro();

            try
            {
                CatalogosDAL dCatalogo = new CatalogosDAL();
                Parametro = dCatalogo.ObtenerParametroNombre(NombreParametro);
            }

            catch (Exception) { }

            return Parametro;
        }

    }
}
