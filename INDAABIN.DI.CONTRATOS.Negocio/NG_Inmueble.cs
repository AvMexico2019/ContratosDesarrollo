using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//comunicacion con las capas 
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //entities
using INDAABIN.DI.CONTRATOS.AccesoDatos; //DAL
using INDAABIN.DI.ModeloNegocio; //interconexion al BUS


namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class NG_Inmueble
    {
        public List<ModeloNegocios.Inmueble> ObtenerInmuebles(int IdPais, int IdEstado, int IdMunicipio, string RIUF, string Direccion, ModeloNegocios.InmuebleArrto oInmuebleArrendamiento = null)
        {
            List<ModeloNegocios.Inmueble> ListInmuebles;
            AccesoDatos.InmuebleDAL Conn = new AccesoDatos.InmuebleDAL();
            //obtener informacion de la BD
            ListInmuebles = Conn.ObtenerInmuebles(IdPais, IdEstado, IdMunicipio, RIUF);

            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor
            foreach (ModeloNegocios.Inmueble ObjList in ListInmuebles)
            {
                //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                //obtener nombre de usuario
                //ObjList.UsuarioRegistroDescripcion = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
                //obtener nombre del pais
                ObjList.PaisDescripcion = Negocio.AdministradorCatalogos.ObtenerNombrePais(ObjList.IdPais);
                //obtener nombre del tipo de  vialidad
                ObjList.TipoVialidadDescripcion = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(ObjList.IdTipoVialidad);

                if (QuitarAcentosTexto(ObjList.PaisDescripcion.ToUpper()) == "MEXICO")
                {
                    //obtener nombre de la ent. fed
                    ObjList.EstadoDescripcion = Negocio.AdministradorCatalogos.ObtenerNombreEstado(ObjList.IdEstado.Value);
                    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                    ObjList.MunicipioDescripcion = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(ObjList.IdEstado.Value, ObjList.IdMunicipio.Value);
                    if (ObjList.IdLocalidad != null)
                        //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                        ObjList.LocalidadDescripcion = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.IdPais, ObjList.IdEstado.Value, ObjList.IdMunicipio.Value, ObjList.IdLocalidad.Value);
                    else
                        ObjList.LocalidadDescripcion = ObjList.OtraColonia;
                }
            }

            if (oInmuebleArrendamiento == null)
            {
                if (Direccion.Trim() != "")
                    ListInmuebles = ListInmuebles.Where(c => c.DireccionCompleta.Contains(Direccion)).ToList();
                if (RIUF.Trim() != "")
                    ListInmuebles = ListInmuebles.Where(c => c.RIUF.RIUF1.Contains(RIUF.Trim())).ToList();

                return ListInmuebles;
            }
            else
            {
                if(oInmuebleArrendamiento.IdLocalidadColonia != null)
                    return ListInmuebles.Where(c => (c.DireccionCompleta.Contains(oInmuebleArrendamiento.NombreVialidad) || c.DireccionCompleta.Contains(oInmuebleArrendamiento.NombreLocalidadColonia))).ToList();
                else
                    return ListInmuebles.Where(c => (c.DireccionCompleta.Contains(oInmuebleArrendamiento.NombreVialidad) || c.DireccionCompleta.Contains(oInmuebleArrendamiento.OtraColonia))).ToList();
            }
                
        }

        public string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }

        //insert de un inmueble.
        public int InsertInmueble(string strConnectionString, ModeloNegocios.Inmueble objInmueble, int OtraFigura)
        {
            int IdInmuebleNuevo;
            string result;
            AccesoDatos.InmuebleDAL Conn = new AccesoDatos.InmuebleDAL();
            result = Conn.InsertInmueble(strConnectionString, objInmueble.PaisDescripcion, objInmueble.IdPais, objInmueble.IdTipoInmueble, objInmueble.IdEstado, objInmueble.IdMunicipio, objInmueble.IdLocalidad, objInmueble.OtraColonia, objInmueble.IdTipoVialidad, objInmueble.NombreVialidad, objInmueble.NumExterior, objInmueble.NumInterior, objInmueble.CodigoPostal, objInmueble.GeoRefLatitud, objInmueble.GeoRefLongitud, objInmueble.NombreInmueble, objInmueble.CodigoPostalExtranjero, objInmueble.EstadoExtranjero, objInmueble.CiudadExtranjero, objInmueble.MunicipioExtranjero, objInmueble.GeneraRIUF, objInmueble.RIUF.RIUF1, OtraFigura, objInmueble.IdUsuarioRegistro, objInmueble.CargoUsuarioRegistro);

            if (result == "ERROR")
                return 0;
            else
            {
                IdInmuebleNuevo = System.Convert.ToInt32(result.Split('@')[0].ToString());
                if (result.Split('@')[1].ToString() != "0" && result.Split('@')[1].ToString().Trim().Length > 1)
                    objInmueble.RIUF.RIUF1 = result.Split('@')[1].ToString();
                else
                    objInmueble.RIUF.RIUF1 = result.Split('@')[2].ToString();
                return IdInmuebleNuevo;
            }
        }      
    }
}

