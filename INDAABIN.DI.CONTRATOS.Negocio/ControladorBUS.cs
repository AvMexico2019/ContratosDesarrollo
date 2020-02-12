using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INDAABIN.DI.CONTRATOS.ModeloNegocios;
//componentes de librerias del BUS
using INDAABIN.DI.Utilerias;
using INDAABIN.DI.ModeloNegocio;
using INDAABIN.DI.Constantes;
using INDAABIN.DI.BUS.Servicio.Contratos;
using INDAABIN.DI.CONTRATOS.AccesoDatos;

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class ControladorBUS
    {
        IWS_BUS WS_BUS = null;

        //constructor
        public ControladorBUS()
        {
            WS_BUS = new CanalesWS_BUS().CreaCanal();
        }

        //*** Operacion al SSO  ***
        public List<SSO> ObtenerUsuario(string usuario)
        {
            ParametrosEntrada.ParametrosEntrada parametros = new ParametrosEntrada.ParametrosEntrada();
            ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();

            List<SSO> usuarios = new List<SSO>();
            INDAABIN.DI.ModeloNegocio.FiltroBusqueda busqueda = new FiltroBusqueda();

            try
            {
                //using (WS_BUS.WS_BUSClient cliente = new WS_BUS.WS_BUSClient())
                //{
                parametros.TipoOperacion = Operaciones.OBTENER_USUARIOXFILTRO;
                busqueda.UserName = usuario;
                parametros.FiltroBusqueda = busqueda;
                parametrosSalida = WS_BUS.WSBus(parametros);

                if (parametrosSalida.Respuesta)
                {
                    if (parametrosSalida.Lsso != null)
                    {
                        usuarios = parametrosSalida.Lsso.ToList();
                    }
                }

                if (!parametrosSalida.Respuesta)
                    throw new InvalidOperationException(string.Format("Error:{0}", parametrosSalida.Mensaje_Error));
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS. Valida la version de las lilbrerias: " + ex.Message);
            }
            return usuarios;
        }

        //*** Operacion al SSO  ***
        public List<SSO> ObtenerUsuarioXId(int IdUsuario)
        {
            ParametrosEntrada.ParametrosEntrada parametros = new ParametrosEntrada.ParametrosEntrada();
            ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();

            List<SSO> usuarios = new List<SSO>();
            INDAABIN.DI.ModeloNegocio.FiltroBusqueda busqueda = new FiltroBusqueda();

            try
            {
                //using (WS_BUS.WS_BUSClient cliente = new WS_BUS.WS_BUSClient())
                //{
                parametros.TipoOperacion = Operaciones.OBTENER_USUARIOXFILTRO;
                busqueda.IdUser = IdUsuario;
                parametros.FiltroBusqueda = busqueda;
                parametrosSalida = WS_BUS.WSBus(parametros);
                usuarios = parametrosSalida.Lsso.ToList();

                if (!parametrosSalida.Respuesta)
                    throw new InvalidOperationException(string.Format("Error:{0}", parametrosSalida.Mensaje_Error));
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(ObtenerUsuario). Valida la version de las lilbrerias: " + ex.Message);
            }
            return usuarios;
        }

        //***  Operacion al SSO  ****
        public List<SSO> ObtenerRoles(string usuario)
        {
            ParametrosEntrada.ParametrosEntrada parametros = new ParametrosEntrada.ParametrosEntrada();
            ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();

            List<SSO> usuarios = new List<SSO>();
            INDAABIN.DI.ModeloNegocio.FiltroBusqueda busqueda = new FiltroBusqueda();

            try
            {
                parametros.TipoOperacion = Operaciones.OBTENER_ROLESXAPLICACION;
                busqueda.UserName = usuario;
                parametros.FiltroBusqueda = busqueda;
                parametrosSalida = WS_BUS.WSBus(parametros);

                if (!parametrosSalida.Respuesta)
                    throw new InvalidOperationException(string.Format("Error:{0}", parametrosSalida.Mensaje_Error));

                usuarios = parametrosSalida.Lsso.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(ObtenerRoles). Valida la version de las lilbrerias: " + ex.Message);
            }
            return usuarios;
        }

        //****** Operacion a DB_CAT   ******
        public List<Catalogo> LlenaCombos(string ConsultaCatalogo)
        {
            try
            {
                ParametrosEntrada.ParametrosEntrada parametrosEntrada = new ParametrosEntrada.ParametrosEntrada();
                ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();
                parametrosEntrada.TipoOperacion = Constantes.Operaciones.OBTENERCATALOGO;
                parametrosEntrada.ConsultaCatalogo = ConsultaCatalogo;
                parametrosSalida = WS_BUS.WSBus(parametrosEntrada);
                return parametrosSalida.LCatalogo;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(LlenaCombos). Valida la version de las lilbrerias: " + ex.Message);
            }
        }

        //****** Operacion a DB_CAT   ******
        public List<CatalogoDependiente> LlenaCombosElemento(string ConsultaCatalogo)
        {
            try
            {

                ParametrosEntrada.ParametrosEntrada parametrosEntrada = new ParametrosEntrada.ParametrosEntrada();
                ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();
                parametrosEntrada.TipoOperacion = Constantes.Operaciones.OBTENERCATALOGODEPENDIENTE;
                parametrosEntrada.ConsultaCatalogo = ConsultaCatalogo;
                parametrosSalida = WS_BUS.WSBus(parametrosEntrada);
                return parametrosSalida.LCatalogoDependiente;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(LlenaCombosElemento). Valida la version de las lilbrerias: " + ex.Message);
            }
        }

        //****** Operacion a DB_CAT   ****** //RCA 25/08/2017
        public List<CatalogoDependiente> LlenaCombosSectores(int IdInstitucion)
        {
            try
            {

                ParametrosEntrada.ParametrosEntrada parametrosEntrada = new ParametrosEntrada.ParametrosEntrada();
                ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();
                parametrosEntrada.TipoOperacion = Constantes.Operaciones.OBTENER_SECTOR_X_INSTITUCION;
                parametrosEntrada.IdInstitucion = IdInstitucion;
                parametrosSalida = WS_BUS.WSBus(parametrosEntrada);
                return parametrosSalida.LCatalogoDependiente;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(LlenaCombosElemento). Valida la version de las lilbrerias: " + ex.Message);
            }
        }


        //MZT se agrega consumo de uso especifico de inmueble
        public List<CatalogoDependiente> LlenaCombosUsoEspecifico(int IdUsoGenerico)
        {
            try
            {
                ParametrosEntrada.ParametrosEntrada parametrosEntrada = new ParametrosEntrada.ParametrosEntrada();
                ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();
                parametrosEntrada.TipoOperacion = Constantes.Operaciones.OBTENERCATALOGODEPENDIENTE;
                parametrosEntrada.ConsultaCatalogo = "ObtenerUsoEspecifico";
                parametrosEntrada.IdUsoGenerico = IdUsoGenerico;
                parametrosSalida = WS_BUS.WSBus(parametrosEntrada);
                return parametrosSalida.LCatalogoDependiente;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(LlenaCombosElemento). Valida la version de las lilbrerias: " + ex.Message);
            }
        }




        //****** Operacion a DB_CAT   ******
        public List<CatalogoElementos> LlenaCombos3Datos(string ConsultaCatalogo)
        {
            try
            {

                ParametrosEntrada.ParametrosEntrada parametrosEntrada = new ParametrosEntrada.ParametrosEntrada();
                ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();
                parametrosEntrada.TipoOperacion = Constantes.Operaciones.OBTENER_CATALOGO3DATOS;
                parametrosEntrada.ConsultaCatalogo = ConsultaCatalogo;
                parametrosSalida = WS_BUS.WSBus(parametrosEntrada);
                return parametrosSalida.LCatalogoElementos;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(LlenaCombos3Datos). Valida la version de las lilbrerias: " + ex.Message);
            }
        }

        //***  Operacion a Avaluos  ****
        public List<SolicitudAvaluos> ObtenerJustipreciacionAvaluos(string pSecuencial)
        {
            ParametrosEntrada.ParametrosEntrada parametroEntrada = new ParametrosEntrada.ParametrosEntrada();
            ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();

            List<SolicitudAvaluos> LSolicitudAvaluos = null;
            parametroEntrada.TipoOperacion = Operaciones.OBTENER_JUSTIPRECIACION_RENTA;
            parametroEntrada.NoSecuencial = pSecuencial;

            try
            {
                parametrosSalida = WS_BUS.WSBus(parametroEntrada);

                if (parametrosSalida.Respuesta)
                {
                    //MZT 15/08/2017
                    LSolicitudAvaluos = parametrosSalida.LSolAvaluos.ToList();
                }
                else if (!string.IsNullOrEmpty(parametrosSalida.Mensaje_Error))
                {
                    throw new Exception(parametrosSalida.Mensaje_Error);
                }
                //MZT 15/08/2017
                //else
                //{
                //    throw new Exception("No ex¡No fue posible encontrar la justipreciación con el secuencial solicitado!");
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("No ex"))
                {
                    throw new Exception(ex.Message.Replace("No ex", ""));
                }
                else
                {
                    throw new Exception("Ha ocurrido un error al recuperar información del BUS(ObtenerJustipreciacionAvaluos). Valida la version de las lilbrerias: " + ex.Message);
                }
            }
            return LSolicitudAvaluos;
        }

        ///código postal a encontrar</param>
        /// <param name="IdPais">Clave del País</param>
        /// <param name="IdEstado">Clave del Estado</param>
        /// <param name="IdMunicipio"> clave del municipío </param>
        /// <returns>Lista de catalogos</returns>
        public List<FiltroXCP> ObtenerCatalogoLocalidades(string codigoPostal, int IdPais, int IdEstado, int IdMunicipio, int IdLocalidad)
        {
            ParametrosEntrada.ParametrosEntrada parametros = new ParametrosEntrada.ParametrosEntrada();
            ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();

            List<Catalogo> listaCodigos = new List<Catalogo>();

            try
            {
                //Prepara los parámetros 
                parametros.TipoOperacion = (int)Constantes.Operaciones.FILTRO_X_CP;
                parametros.CodigoPostal = new CodigoPostal();

                if (codigoPostal != "0")
                {
                    parametros.CodigoPostal.Codigo = codigoPostal;
                }
                else
                {
                    parametros.CodigoPostal.Fk_IdPais = IdPais;
                    parametros.CodigoPostal.Fk_IdEstado = IdEstado;
                    parametros.CodigoPostal.Fk_IdMunicipio = IdMunicipio;
                    if (IdLocalidad != 0)
                        parametros.CodigoPostal.IdLocalidad = IdLocalidad;
                }

                parametrosSalida = WS_BUS.WSBus(parametros);
                return parametrosSalida.LColoniasXCp;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al recuperar información del BUS(ObtenerCatalogoLocalidades). Valida la version de las lilbrerias: " + ex.Message);
            }
        }



        public void EnviarCorreo(string encabezado, string msgCuerpo, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(msgCuerpo) && !string.IsNullOrEmpty(encabezado) && !string.IsNullOrEmpty(email))
                {
                    ParametrosEntrada.ParametrosEntrada parametroEntrada = new ParametrosEntrada.ParametrosEntrada();
                    ParametrosSalida.ParametrosSalida parametrosSalida = new ParametrosSalida.ParametrosSalida();


                    parametroEntrada.TipoOperacion = Operaciones.ENVIA_CORREOS_ZIMBRA;
                    parametroEntrada.Destinatario = email;

                    parametroEntrada.Mensaje = encabezado;
                    parametroEntrada.Cuerpo = msgCuerpo;

                    parametrosSalida = WS_BUS.WSBus(parametroEntrada);

                    if (!parametrosSalida.Respuesta)
                    {
                        if (!string.IsNullOrEmpty(parametrosSalida.Mensaje_Error))
                        {
                            throw new Exception(parametrosSalida.Mensaje_Error);
                        }
                        else
                        {
                            throw new Exception("¡Error al enviar notificación por correo!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("EnviarCorreo: {0}", ex.Message));
            }
        }

    }
}