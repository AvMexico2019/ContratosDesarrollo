using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
//comunicacion con las capas 
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //entities
using INDAABIN.DI.CONTRATOS.AccesoDatos;
using INDAABIN.DI.ModeloNegocio; //interconexion al BUS

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class NG_Reportes
    {
        public DataTable SelectReporteInmuebles(string strConnectionString, ModeloNegocios.ParametroReporte objParametros)
        {
            DataTable result;
            AccesoDatos.ReportesDAL Conn = new AccesoDatos.ReportesDAL();
            result = Conn.SelectReporteInmuebles(strConnectionString, objParametros.IdInstitucion,
                objParametros.RangoFechaInicioOcupacionInicial, objParametros.RangoFechaInicioOcupacionFinal,
                objParametros.RangoFechaTerminoOcupacionInicial, objParametros.RangoFechaTerminoOcupacionFinal,
                objParametros.RangoFechaRegistroInicial, objParametros.RangoFechaRegistroFinal, objParametros.IdTipoContrato,
                objParametros.IdTipoOcupacion);

            if (result != null)
            {
                foreach (DataRow item in result.Rows)
                {
                    try
                    {
                        if (item["Fk_IdPais"] != null)
                        {
                            item["Pais"] = Negocio.AdministradorCatalogos.ObtenerNombrePais(System.Convert.ToInt32(item["Fk_IdPais"].ToString()));
                        }

                        if (QuitarAcentosTexto(item["Pais"].ToString().ToUpper()) == "MEXICO")
                        {
                            if (item["Fk_IdEstado"] != null)
                            {
                                item["Estado"] = Negocio.AdministradorCatalogos.ObtenerNombreEstado(System.Convert.ToInt32(item["Fk_IdEstado"].ToString()));
                                item["Municipio"] = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(System.Convert.ToInt32(item["Fk_IdEstado"].ToString()), System.Convert.ToInt32(item["Fk_IdMunicipio"].ToString()));
                            }
                        }
                        //item["Estado"] = Negocio.AdministradorCatalogos.ObtenerNombreEstado(System.Convert.ToInt32(item["Fk_IdEstado"].ToString()));
                        //item["Municipio"] = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(System.Convert.ToInt32(item["Fk_IdEstado"].ToString()), System.Convert.ToInt32(item["Fk_IdMunicipio"].ToString()));
                        if (item["Fk_IdTipoInmueble"] != null)
                        {
                            item["TipoInmueble"] = Negocio.AdministradorCatalogos.ObtenerNombreTipoInmueble(System.Convert.ToInt32(item["Fk_IdTipoInmueble"].ToString()));
                        }
                        if (item["Fk_IdTipoUsoInm"] != null)
                        {
                            item["TipoUsoInmueble"] = Negocio.AdministradorCatalogos.ObtenerNombreUsoInmueble(System.Convert.ToInt32(item["Fk_IdTipoUsoInm"].ToString()));
                        }                        
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("SelectReporteInmuebles", ex);
                    }

                }
                DataTable oFieldFilteredTable = result.DefaultView.ToTable(false, objParametros.ListaCampos.ToArray());
                return oFieldFilteredTable;
            }
            return null;
        }
        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }
    }
}
