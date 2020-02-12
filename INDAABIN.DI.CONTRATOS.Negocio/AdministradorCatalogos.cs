using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using INDAABIN.DI.ModeloNegocio;//bus


namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class AdministradorCatalogos
    {
        //dependientes
        private static List<CatalogoDependiente> ListCatalogoMunicipios;
        private static List<CatalogoDependiente> ListCatalogoSectores;
        private static List<CatalogoDependiente> ListCatalogoEspecificos;
 
        //independientes
        private static List<Catalogo> ListCatalogoCargos;
        private static List<Catalogo> ListCatalogoPais;
        private static List<Catalogo> ListCatalogoTipoInmueble;
        private static List<Catalogo> ListCatalogoEstados;
        private static List<Catalogo> ListCatalogoInstituciones;
        private static List<Catalogo> ListCatalogoTipoVialidad;
        private static List<Catalogo> ListCatalogoUsoInmueble;
        private static List<CatalogoElementos> ListCatalogoTipoMoneda;
        private static List<Catalogo> ListaCatalogoUnidadMedidaTerreno;
        private static List<Catalogo> ListaCatalogoUnidadMedidaConstruida;
        private static List<Catalogo> ListaCatalogoUnidadMedidaRentable;
        private static List<Catalogo> ListCatalogoMunicipio;
        private static List<Catalogo> ListaCatalogoUnidadMedida;
        private static List<Catalogo> ListCatalogoUsosGenericos;

        public static String ObtenerNombreUsuarioSSO(int IdUsuario)
        {
            //usuario que registro
            SSO usuario = null;

                if (IdUsuario == 1)
                {
                    return "Cargado por proceso de carga inicial";
                }
                else if (IdUsuario != 1)//usuario != null
                {
                    usuario = new NG().ObtenerUsuarioXId(IdUsuario);//poblar del bus

                    if (usuario == null)
                    {
                        return "Usuario No Encontrado";
                    }
                    else
                    {
                        return usuario.Nombre + " " + usuario.ApellidoP + " " + usuario.ApellidoM;
                    }

                }
                else
                    return "Usuario No Encontrado";
           
        }

        //poblar la lista estatica de: Cargos
        public static List<Catalogo> ObtenerCatalogoCargos()
        {
            if (ListCatalogoCargos == null || ListCatalogoCargos.Count() == 0)
                //poblar del bus
                ListCatalogoCargos = new NG().LlenaCombo("ObtenerCargo").OrderBy(x => x.Descripcion).ToList();

            //poblar manualmente, para implementacion de modo desconectado
            //ListCatalogoCargos = new List<Catalogo> {
            //              new Catalogo { IdValue=4, Descripcion="Director de Finanzas"},
            //              new Catalogo { IdValue=5, Descripcion="DIRECTOR DE INFORMÁTICA"},
            //              new Catalogo { IdValue=49, Descripcion="Coordinador de Desarrollo Institucional"}

            //};

            return ListCatalogoCargos;

        }

        //poblar la lista estatica de: Paises
        public static List<Catalogo> ObtenerCatalogoPais()
        {
            if (ListCatalogoPais == null || ListCatalogoPais.Count() == 0)
                ListCatalogoPais = new NG().LlenaCombo("ObtenerPais").OrderBy(x => x.Descripcion).ToList();

            //poblar manualmente, para implementacion de modo desconectado
            //ListCatalogoPais = new List<Catalogo> {
            //               new Catalogo { IdValue=165, Descripcion="México"},
            //};
            return ListCatalogoPais;
        }

        //poblar la lista estatica de: TipoInmueble
        public static List<Catalogo> ObtenerCatalogoTipoInmueble()
        {
            if (ListCatalogoTipoInmueble == null || ListCatalogoTipoInmueble.Count() == 0)
                //poblar del bus
                ListCatalogoTipoInmueble = new NG().LlenaCombo("ObtenerTipoInmueble").OrderBy(x => x.Descripcion).ToList();

            //poblar manualmente, para implementacion de modo desconectado
            //ListCatalogoTipoInmueble = new List<Catalogo> {
            //               new Catalogo { IdValue=1, Descripcion="TERRENO"},
            //               new Catalogo { IdValue=2, Descripcion="EDIFICACIÓN"},
            //               new Catalogo { IdValue=2, Descripcion="MIXTO"}

            //};
            return ListCatalogoTipoInmueble;
        }

        //poblar la lista estatica de: moneda
        public static List<CatalogoElementos> ObtenerCatalogoMoneda()
        {
            if (ListCatalogoTipoMoneda == null || ListCatalogoTipoMoneda.Count() == 0)
                ListCatalogoTipoMoneda = new NG().LlenaCombos3Datos("ObtenerTipoMoneda").OrderBy(x => x.Descripcion).ToList();

            ////  poblar manualmente, para implementacion de modo desconectado
            //ListCatalogoTipoMoneda = new List<Catalogo> {
            //               new Catalogo { IdValue=101, Descripcion="Peso mexicano"},
            //               new Catalogo { IdValue=147, Descripcion="Dólar estadounidense"}

            //     };
            return ListCatalogoTipoMoneda;
        }
                
        //poblar la lista estatica de: Uso Inmueble
        public static List<Catalogo> ObtenerCatalogoUsoInmueble()
        {
            if (ListCatalogoUsoInmueble == null || ListCatalogoUsoInmueble.Count() == 0)
            {
                ListCatalogoUsoInmueble = new NG().LlenaCombo("ObtenerUsoTipoInmueble").OrderBy(x => x.Descripcion).ToList();
                if (ListCatalogoUsoInmueble.Count > 0)
                    ListCatalogoUsoInmueble.Add(new Catalogo { IdValue = -1, Descripcion = "OTRO" });
            }
            //poblar manualmente, para implementacion de modo desconectado
            //ListCatalogoUsoInmueble = new List<Catalogo> {
            //               new Catalogo { IdValue=1, Descripcion="DEPORTIVO"},
            //               new Catalogo { IdValue=10, Descripcion="INFRAESTRUCTURA"},
            //               new Catalogo { IdValue=11, Descripcion="SALUD"}

            //};
            return ListCatalogoUsoInmueble;
        }

        //poblar la lista estatica de: Estados
        public static List<Catalogo> ObtenerCatalogoEstados()
        {
            if (ListCatalogoEstados == null || ListCatalogoEstados.Count() == 0)
                //poblar del bus
                ListCatalogoEstados = new NG().LlenaCombo("ObtenerEstado").OrderBy(x => x.Descripcion).ToList();

            //poblar manualmente, para implementacion de modo desconectado
            //ListCatalogoEstados = new List<Catalogo> {
            //               new Catalogo { IdValue=1, Descripcion="Aguascalientes"},
            //               new Catalogo { IdValue=9, Descripcion="Ciudad de México"}

            //};
            return ListCatalogoEstados;
        }

        public static List<Catalogo> ObtenerCatalogoInstituciones()
        {
            if (ListCatalogoInstituciones == null || ListCatalogoInstituciones.Count() == 0)
            {
                ListCatalogoInstituciones = new NG().LlenaCombo("ObtenerInstitucion").OrderBy(x => x.Descripcion).ToList();
                ListCatalogoInstituciones.Add(new Catalogo { IdValue = 0, Descripcion = "TODAS" });
            }
            return ListCatalogoInstituciones;
        }

        public static List<Catalogo> ObtenerCatalogoTipoVialidad()
        {
            if (ListCatalogoTipoVialidad == null || ListCatalogoTipoVialidad.Count() == 0)
                //poblar del bus
                ListCatalogoTipoVialidad = new NG().LlenaCombo("ObtenerVialidad").OrderBy(x => x.Descripcion).ToList();

            //// poblar manualmente, para implementacion de modo desconectado
            //  ListCatalogoTipoVialidad = new List<Catalogo> {
            //                new Catalogo { IdValue=5, Descripcion="CALLE"},
            //                new Catalogo { IdValue=3, Descripcion="AVENIDA"},
            //                new Catalogo { IdValue=6, Descripcion="CALLEJÓN"},
            //                new Catalogo { IdValue=4, Descripcion="BOULEVARD"},
            //                new Catalogo { IdValue=8, Descripcion="CERRADA"},
            //                new Catalogo { IdValue=9, Descripcion="CIRCUITO"},
            //                new Catalogo { IdValue=7, Descripcion="CALZADA"},
            //                new Catalogo { IdValue=1, Descripcion="AMPLIACIÓN"},
            //                new Catalogo { IdValue=14, Descripcion="EJE VIAL"},
            //                new Catalogo { IdValue=15, Descripcion="PASAJE"}
            //    };


            return ListCatalogoTipoVialidad;

        }

        //poblar la lista estatica de: Mpos
        public static List<CatalogoDependiente> ObtenerCatalogoMunicipios()
        {
            if (ListCatalogoMunicipios == null || ListCatalogoMunicipios.Count() == 0)
                ListCatalogoMunicipios = new NG().LlenaComboDependiente("ObtenerMunicipio").OrderBy(o => o.Nombre).ToList();
            return ListCatalogoMunicipios;
        }

        //poblar Colonias en funcion del CP
        public static List<Catalogo> ObtenerLocalidadesPorCodigoPostal(string CodigoPostal)
        {
            List<Catalogo> ListLocalidades = null;
            List<FiltroXCP> catalogoFiltrado = null;
            catalogoFiltrado = new NG_Catalogos().ObtenerLocalidades(CodigoPostal);

            if (catalogoFiltrado != null)
            {
                ListLocalidades = catalogoFiltrado.Select(x => new Catalogo
                {
                    IdValue = x.IdLocalidad.Value,
                    Descripcion = x.CP + "-" + x.DescripcionLocalidad
                }).OrderBy(o => o.Descripcion).ToList();
            }

            return ListLocalidades;
        }

        public static FiltroXCP ObtenerDetalleLocalidadPorCodigoPostal(string CodigoPostal)
        {
            List<FiltroXCP> catalogoFiltrado = null;
            catalogoFiltrado = new NG_Catalogos().ObtenerLocalidades(CodigoPostal);
            return catalogoFiltrado.FirstOrDefault();
        }

        public static List<CatalogoElementos> ObtenerLocalidades(int IdPais, int IdEstado, int IdMunicipio, int IdLocalidad = 0)
        {
            List<CatalogoElementos> ListLocalidades = null;
            List<FiltroXCP> catalogoFiltrado = null;

            
            //RCA 16/07/2018
            //poonemos un try cath  para que siga avanzando si el catalofofiltro viene en null
            try
            {
                catalogoFiltrado = new NG_Catalogos().ObtenerLocalidades("0", IdPais, IdEstado, IdMunicipio, IdLocalidad).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("ObtenerLocalidades:{0}", ex.Message));
            }
            finally
            {
                if (catalogoFiltrado != null)
                {
                    ListLocalidades = catalogoFiltrado.Select(x => new CatalogoElementos
                    {
                        IdCatalogo = x.IdLocalidad.Value,
                        Descripcion = x.DescripcionLocalidad,
                        DescripcionComp = x.CP + "-" + x.DescripcionLocalidad
                        //IdValue = x.IdLocalidad.Value,
                        //Descripcion = x.CP + "-" + x.DescripcionLocalidad
                    }).OrderBy(o => o.DescripcionComp).ToList();
                    //}).OrderBy(o => o.Descripcion).ToList();
                }
            }

           

           
            return ListLocalidades;
        }

        //sobrecargado: con parametro de entrada
        public static List<Catalogo> ObtenerMunicipios(int IdEstado)
        {
            List<Catalogo> ListMunicipios = null;
            ObtenerCatalogoMunicipios();
            if (ListCatalogoMunicipios != null)
            {
                //filtrado de municipios por estado
                ListMunicipios = ListCatalogoMunicipios.Where(x => x.IdcatalogoDependiente == IdEstado).
                                Select(x => new Catalogo { IdValue = x.IdCatalogo, Descripcion = x.Nombre }).ToList();
            }
            return ListMunicipios;
        }

        //obtener el nombre de un colonia por su id
        public static String ObtenerNombreLocalidad(int pIdPais, int pIdEstado, int pIdMunicipio, int pIdLocalidad)
        {
            List<CatalogoElementos> ListLocalidades = null;
            string NombreColonia = null;

           
            //RCA 16/07/2018
            //validacion de los paramtros de entrada 
            try
            {
                
                    ListLocalidades = ObtenerLocalidades(pIdPais, pIdEstado, pIdMunicipio, pIdLocalidad);
                

                //ObtenerLocalidades(1);
                if (ListLocalidades != null)
                {
                    //obtner el nombre de la colonia
                    NombreColonia = (string)(from col in ListLocalidades
                                             select col.Descripcion).FirstOrDefault();

                }
                else
                {
                    NombreColonia = string.Empty;
                }

            }
            catch(Exception ex)
            {

            }
                

                
                if (NombreColonia != null)
                {
                    return NombreColonia.ToUpper();
                }
                else
                {
                    return NombreColonia = string.Empty;
                }
                

        }

        //obtener el nombre de un mpo por su id
        public static String ObtenerNombreMunicipio(int? IdEstado, int? pIdMunicipio)
        {
            List<Catalogo> ListMunicipiosxEdo = null;
            string NombreMpo = string.Empty;

            //poblar la lista de mpos, si esta vacia
            ObtenerCatalogoMunicipios();

            if (ListCatalogoMunicipios != null)
            {
                //filtrado de municipios por estado
                ListMunicipiosxEdo = ListCatalogoMunicipios.Where(x => x.IdcatalogoDependiente == IdEstado).
                                Select(x => new Catalogo { IdValue = x.IdCatalogo, Descripcion = x.Nombre }).ToList();

                //obtner el nombre del mpo
                NombreMpo = (string)(from mpo in ListMunicipiosxEdo
                                     where mpo.IdValue == pIdMunicipio
                                     select mpo.Descripcion).FirstOrDefault();
            }
            if (NombreMpo != null)
                return NombreMpo.ToUpper();
            else
                return NombreMpo;
        }

        public static String ObtenerNombreEstado(int? IdEstado)
        {
            string NombreEstado = string.Empty;
            //poblar la lista estatica, si esta vacia
            ObtenerCatalogoEstados();
            if (ListCatalogoEstados != null)
            {
                //obtner el nombre del estado
                NombreEstado = (string)(from edo in ListCatalogoEstados
                                        where edo.IdValue == IdEstado
                                        select edo.Descripcion).FirstOrDefault();
            }
            if (NombreEstado != null)
                return NombreEstado.ToUpper();
            else
                return NombreEstado;
        }

        public static String ObtenerNombrePais(int? IdPais)
        {
            string NombrePais = string.Empty;
            ObtenerCatalogoPais();

            //RCA 13/07/2018
            if (ListCatalogoPais != null && IdPais != null)
            {
                //obtner el nombre del pais
                NombrePais = (string)(from pais in ListCatalogoPais
                                      where pais.IdValue == IdPais
                                      select pais.Descripcion).FirstOrDefault();
            }
            else
            {
                NombrePais = string.Empty;
            }


            if (NombrePais != null)
                return NombrePais.ToUpper();
            else
                return NombrePais;
        }

        public static String ObtenerNombreUsoInmueble(int IdUsoInmueble)
        {
            string NombreUsoInmueble = null;
            ObtenerCatalogoUsoInmueble();

            if (ListCatalogoUsoInmueble != null)
            {
                NombreUsoInmueble = (string)(from tipo in ListCatalogoUsoInmueble
                                             where tipo.IdValue == IdUsoInmueble
                                             select tipo.Descripcion).FirstOrDefault();
            }
            if (NombreUsoInmueble != null)
                return NombreUsoInmueble.ToUpper();
            else
                return NombreUsoInmueble;

        }
        public static String ObtenerNombreTipoInmueble(int IdTipoInmueble)
        {
            string NombreTipoInmueble = null;
            ObtenerCatalogoTipoInmueble();

            if (ListCatalogoTipoInmueble != null)
            {
                NombreTipoInmueble = (string)(from tipo in ListCatalogoTipoInmueble
                                              where tipo.IdValue == IdTipoInmueble
                                              select tipo.Descripcion).FirstOrDefault();
            }
            if (NombreTipoInmueble != null)
                return NombreTipoInmueble.ToUpper();
            else
                return NombreTipoInmueble;
        }

        public static String ObtenerNombreTipoVialidad(int? IdTipoVialidad)
        {
            string NombreTipoVialidad = null;

            //poblar la lista estatica, si esta vacia
            ObtenerCatalogoTipoVialidad();

            //RCA 13/07/2018
            if (ListCatalogoTipoVialidad != null && IdTipoVialidad != null)
            {

                //obtner el nombre de la vialidad
                NombreTipoVialidad = (string)(from x in ListCatalogoTipoVialidad
                                              where x.IdValue == IdTipoVialidad
                                              select x.Descripcion).FirstOrDefault();

            }
            else
            {
                NombreTipoVialidad = string.Empty;
            }


            if (NombreTipoVialidad != null)
                return NombreTipoVialidad.ToUpper();
            else
                return NombreTipoVialidad;
        }

        public static String ObtenerNombreInstitucion(int? IdInstitucion)
        {
            string NombreInstitucion = null;
            //poblar la lista estatica, si esta vacia
            ObtenerCatalogoInstituciones();

            //RCA 13/07/2018
            //validacion de nulos
            if (ListCatalogoInstituciones != null && IdInstitucion != null)
            {

                //obtner el nombre de la institucion
                NombreInstitucion = (string)(from x in ListCatalogoInstituciones
                                             where x.IdValue == IdInstitucion
                                             select x.Descripcion).FirstOrDefault();
            }
            else
            {
                NombreInstitucion = string.Empty;
            }


            if (NombreInstitucion != null)
                return NombreInstitucion.ToUpper();
            else
                return NombreInstitucion;
        }

        public static string ObtenerNombreCargo(int IdCargo)
        {
            string DescCargo = null;
            //poblar la lista estatica, si esta vacia
            ObtenerCatalogoCargos();
            if (ListCatalogoCargos != null)
            {

                //obtner el nombre del cargo
                DescCargo = (string)(from x in ListCatalogoCargos
                                     where x.IdValue == IdCargo
                                     select x.Descripcion).FirstOrDefault();
            }
            if (DescCargo != null)
                return DescCargo.ToUpper();
            else
                return DescCargo;
        }

        public static int ObtenerIdCargo(string NombreCargo)
        {
            int IdCargo = 0;
            //poblar la lista estatica, si esta vacia
            ObtenerCatalogoCargos();
            if (ListCatalogoCargos != null)
            {

                //obtner el nombre del cargo
                IdCargo = (int)(from x in ListCatalogoCargos
                                where x.Descripcion == NombreCargo
                                select x.IdValue).FirstOrDefault();
            }
            return IdCargo;
        }

        public static List<Catalogo> ObtenerCatalogoInstitucion()
        {
            if (ListCatalogoInstituciones == null || ListCatalogoInstituciones.Count() == 0)
            {
                //poblar del bus
                ListCatalogoInstituciones = new NG().LlenaCombo("ObtenerInstitucion").OrderBy(x => x.Descripcion).ToList();
            }

            return ListCatalogoInstituciones;
        }

        public static List<Catalogo> ObtenerCatalogoMunicipio()
        {
            if (ListCatalogoMunicipio == null || ListCatalogoMunicipio.Count() == 0)
            {
                //poblar del bus
                ListCatalogoMunicipio = new NG().LlenaCombo("ObtenerMunicipio").OrderBy(x => x.Descripcion).ToList();
            }

            return ListCatalogoMunicipio;
        }

        public static object ObtenerCatalogoUnidadTerrenoDic()
        {
            if (ListaCatalogoUnidadMedidaTerreno == null || ListaCatalogoUnidadMedidaTerreno.Count() == 0)
            {
                //poblar bus
                ListaCatalogoUnidadMedidaTerreno = new NG().LlenaCombo("ObtenerUnidadMedida").OrderBy(x => x.Descripcion).ToList();
            }
            return ListaCatalogoUnidadMedidaTerreno;
        }

        public static object ObtenerCatalogoUnidadConstruidaDic()
        {
            if (ListaCatalogoUnidadMedidaConstruida == null || ListaCatalogoUnidadMedidaConstruida.Count() == 0)
            {
                //poblar bus
                ListaCatalogoUnidadMedidaConstruida = new NG().LlenaCombo("ObtenerUnidadMedida").OrderBy(x => x.Descripcion).ToList();
            }
            return ListaCatalogoUnidadMedidaConstruida;
        }

        public static object ObtenerCatalogoUnidadRentableDic()
        {
            if (ListaCatalogoUnidadMedidaRentable == null || ListaCatalogoUnidadMedidaRentable.Count() == 0)
            {
                //poblar el bus
                ListaCatalogoUnidadMedidaRentable = new NG().LlenaCombo("ObtenerUnidadMedida").OrderBy(x => x.Descripcion).ToList();
            }
            return ListaCatalogoUnidadMedidaRentable;
        }

        public static List<Catalogo> ObtenerCatalogoUnidadMedida()
        {
            if (ListaCatalogoUnidadMedida == null || ListaCatalogoUnidadMedida.Count() == 0)
            {
                //poblar el bus
                ListaCatalogoUnidadMedida = new NG().LlenaCombo("ObtenerUnidadMedida").OrderBy(x => x.Descripcion).ToList();
            }
            return ListaCatalogoUnidadMedida;
        }

        //filtro para sector x institucion 25/08/2017
        public static List<Catalogo> ObtenerSectores(int IdInstitucion)
        {

            List<Catalogo> ListSectores = null;
            ObtenerCatalogoSectores(IdInstitucion);
            if (ListCatalogoSectores != null)
            {
                //filtrado de instituciones por sector
                //idcatalogo muestra el id de la institucion y en el idcatalogodependiente muestra el idsector
                ListSectores = ListCatalogoSectores.Where(x => x.IdcatalogoDependiente == IdInstitucion).
                                Select(x => new Catalogo { IdValue = x.IdCatalogo, Descripcion = x.Nombre }).ToList();

            }
            return ListSectores;
        }

        public static List<CatalogoDependiente> ObtenerCatalogoSectores(int IdInstitucion)
        {
            if (ListCatalogoSectores != null)
            {
                ListCatalogoSectores = null;

            }
            if (ListCatalogoSectores == null || ListCatalogoSectores.Count() == 0)
            {
                ListCatalogoSectores = new NG().LlenaSector(IdInstitucion).OrderBy(o => o.Nombre).ToList();

            }
            return ListCatalogoSectores;
        }

        //RCA 
        //metodo para el llenado del uso generico
        public static List<Catalogo> ObtenerCatUsoGenerico()
        {
            if (ListCatalogoUsosGenericos == null || ListCatalogoUsosGenericos.Count() == 0)
            {
                //poblar el bus
                ListCatalogoUsosGenericos = new NG().LlenaCombo("ObtenerUsogenerico").OrderBy(x => x.Descripcion).ToList();
            }
            return ListCatalogoUsosGenericos;
        }

        //RCA
        public static List<Catalogo> ObtenerEspecificos(int IdGenerico)
        {
            List<Catalogo> ListEspecificos = null;
            ObtenerCatalogoEspecifico(IdGenerico);
            if (ListCatalogoEspecificos != null)
            {
                //filtro de uso especifico por uso generico
                ListEspecificos = ListCatalogoEspecificos.Where(x => x.IdcatalogoDependiente == IdGenerico)
                    .Select(x => new Catalogo { IdValue = x.IdCatalogo, Descripcion = x.Nombre }).ToList();
            }
            return ListEspecificos;
        }

        //RCA
        //llenar la lista de catalogo de usos especificos
        public static List<CatalogoDependiente> ObtenerCatalogoEspecifico(int IdGenerico)
        {
            if (ListCatalogoEspecificos == null || ListCatalogoEspecificos.Count() == 0)
            {
                ListCatalogoEspecificos = new NG().LlenaComboUsoEspecifico(IdGenerico).OrderBy(o => o.Nombre).ToList();
            }
            return ListCatalogoEspecificos;
        }

    }//clase
}
