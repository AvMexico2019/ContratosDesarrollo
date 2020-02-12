using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using System.Reflection;

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class ProcesadorParametrosURL
    {
        public ProcesadorParametrosURL()
        {

        }

        #region validacion parametros

        //Función que evalua si los parámetros estan encriptados y si no los actualiza
        public ParametrosURL ObtenerParametros(object clase, Dictionary<string, object> parametros)
        {
            ParametrosURL parametroFinal = new ParametrosURL();
            try
            {

                //si el parametro de Data es que vienen los datos encriptados

                if (parametros.ContainsKey(INDAABIN.DI.CONTRATOS.ModeloNegocios.Constantes.IndenParametroEncrip))
                {
                    string paramData = parametros[INDAABIN.DI.CONTRATOS.ModeloNegocios.Constantes.IndenParametroEncrip].ToString();

                    if (!string.IsNullOrEmpty(paramData) && !string.IsNullOrWhiteSpace(paramData))
                    {
                        parametroFinal = (ParametrosURL)ObtenerParametrosEncriptado(clase, paramData);
                    }
                }

                else
                {

                    parametroFinal = (ParametrosURL)ObtenerParametrosURL(clase, parametros);
                }


            }
            catch (Exception ex)
            {
                //bitacora.GuardarMensajeBitacora(ex, null, "Error al hacer conexion con el bus");
            }

            return parametroFinal;
        }

        //Función que extrae los parámetros de la URL, En caso de que los parametros en la URL vengan en una sola cadena y encriptados
        private object ObtenerParametrosEncriptado(object clase, string paramURL)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            int maximoDatos = 2;
            string cadenaParametros = string.Empty;

            Cifrado.Cifrado procesadorCifrado = new Cifrado.Cifrado();

            try
            {

                //primero desencripta la cadena
                cadenaParametros = procesadorCifrado.Decifrar(paramURL);

                //Posteriormente desglosa los parametros
                if (string.IsNullOrEmpty(cadenaParametros) || string.IsNullOrWhiteSpace(cadenaParametros))
                {
                    throw new InvalidOperationException("Error la cadena que contiene los parametros se encuentra vacía.");
                }

                //separa la cadena

                List<string> cadenas = cadenaParametros.Split('&').ToList();

                foreach (string cadena in cadenas)
                {
                    try
                    {
                        //Separa el valor del parametro
                        List<string> datos = cadena.Split('=').ToList();

                        if (datos.Count == 0 || datos.Count < maximoDatos)
                        {
                            throw new InvalidOperationException("Error no se pudo obtener la lista");
                        }

                        parametros.Add(datos[0], datos[1]);

                    }
                    catch (Exception ex)
                    {
                        // bitacora.GuardarMensajeBitacora(ex, null, "Error al leeer la cadena del parametro ");
                    }

                }

                //Posteriormente obtiene  los parametros

                clase = ObtenerParametrosURL(clase, parametros);


            }
            catch (Exception ex)
            {
                //bitacora.GuardarMensajeBitacora(ex, null, "Error al obtener los parámetros: ");
            }
            return clase;
        }

        //Función que se encarga de la transformación del diccionario de parametros a la calse de parametros
        private object ObtenerParametrosURL(object clase, Dictionary<string, object> parametros)
        {
            List<PropertyInfo> propiedades = new List<PropertyInfo>();

            Type tipoClase = clase.GetType();
            //Primero obtenemos las propiedades
            propiedades = tipoClase.GetProperties().ToList();

            foreach (PropertyInfo propiedad in propiedades)
            {
                try
                {
                    if (!parametros.ContainsKey(propiedad.Name))
                    {
                        throw new InvalidOperationException(string.Format("No existe información para el parametro {0}", propiedad.Name));
                    }


                    object valor = parametros[propiedad.Name];

                    if (valor == null)
                    {
                        throw new InvalidOperationException(string.Format("Error al obtener la información del parametro {0}", propiedad.Name));
                    }


                    Type tipo = propiedad.PropertyType;


                    if (tipo == typeof(Guid))
                    {
                        tipoClase.GetProperty(propiedad.Name).SetValue(clase, new Guid(valor.ToString()));
                    }
                    else
                    {
                        tipoClase.GetProperty(propiedad.Name).SetValue(clase, Convert.ChangeType(valor, tipo));
                    }

                }
                catch (Exception ex)
                {
                    //bitacora.GuardarMensajeBitacora(ex, null, string.Format("Error al obtener el parametro  ", propiedad.Name));
                }
            }
            return clase;
        }

        #endregion

        #region generación cadena parametro

        //Genera en base a una cadena de parametros, la cadena encriptada
        public string GenerarCadenaParametrosEncriptada(string URL)
        {
            string parametrosCadena = string.Empty;
            string parametro = string.Empty;
            string UrlDir = string.Empty;
            Cifrado.Cifrado procesadorCifrado = new Cifrado.Cifrado();

            try
            {
                if (!string.IsNullOrEmpty(URL) && URL.Contains("?"))
                {
                    List<string> cadenas = URL.Split('?').ToList();
                    UrlDir = cadenas.FirstOrDefault();

                    if (cadenas.Count() > 1)
                    {
                        parametro = cadenas[1];
                    }

                    //encripta los parametros
                    parametro = procesadorCifrado.Cifrar(parametro);

                    //Arma la cadena final

                    parametrosCadena = string.Format("{0}?{1}={2}", UrlDir, INDAABIN.DI.CONTRATOS.ModeloNegocios.Constantes.IndenParametroEncrip, parametro);

                }
                else
                {
                    parametro = URL;
                }

            }
            catch (Exception ex)
            {
                //bitacora.GuardarMensajeBitacora(ex, 0, "Error al generar la cadena de los parámetros");
                throw;
            }

            return parametrosCadena;
        }

        //Función que genera los parametros base para la URL
        private string GenerarCadenaParametrosBase(object clase, List<string> parametros)
        {
            string parametrosCadena = string.Empty;
            bool valido = true;
            object valor = null;

            try
            {
                Type tipoClase = clase.GetType();

                foreach (string parametro in parametros)
                {

                    valido = true;

                    PropertyInfo propiedad = tipoClase.GetProperty(parametro);

                    if (propiedad == null)
                    {
                        valido = false;
                    }
                    else
                    {
                        valor = propiedad.GetValue(clase);
                        if (valor == null)
                            valido = false;
                    }

                    if (valido == false)
                    {
                        parametrosCadena += parametro + "=&";
                    }
                    else
                    {
                        parametrosCadena += propiedad.Name + "=" + valor.ToString() + "&";
                    }

                }


            }
            catch (Exception ex)
            {
                //bitacora.GuardarMensajeBitacora(ex, 0, "Error al generar la cadena de los parámetros");
                throw;
            }
            return parametrosCadena;
        }

        public string ObtenerConfiguracionURL(string URL, object datos)
        {
            List<string> listaParametros = new List<string>();
            string cadena = string.Empty;

            try
            {
                //Verifica que la URL enviada no sea vacía o nula
                if (string.IsNullOrEmpty(URL) || string.IsNullOrWhiteSpace(URL))
                {
                    return URL;
                }


                //primero detecta si hay parametros
                if (!URL.Contains("?"))
                {
                    return URL;
                }


                //Divide la cadena de los parametros
                cadena = URL.Substring(0, URL.IndexOf('?') + 1);
                //obtiene la lista de parametros personalizados
                if (URL.Length > URL.IndexOf('?'))
                {
                    string cad = URL.Substring(URL.IndexOf('?') + 1);
                    listaParametros = (URL.Substring(URL.IndexOf('?') + 1)).Replace("=", "").Split('&').ToList();
                }

                //Al final obtiene los parametros de los datos
                cadena += GenerarCadenaParametrosBase(datos, listaParametros);


            }
            catch (Exception ex)
            {

                //bitacora.GuardarMensajeBitacora(ex, 0, "Error al evaluar si se trata de un día inhábil");
                throw;
            }
            return cadena;
        }

        #endregion
    }
}
