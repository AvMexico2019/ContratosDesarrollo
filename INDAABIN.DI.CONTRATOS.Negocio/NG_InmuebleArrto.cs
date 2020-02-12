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
    public class NG_InmuebleArrto
    {
        //obj entity
        ModeloNegocios.InmuebleArrto objInmuebleArrto;

        public InmuebleArrto ObtenerInmuebleArrto(int IdInmuebleArrendamiento)
        {
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            objInmuebleArrto = Conn.ObtenerInmuebleArrto(IdInmuebleArrendamiento);

            
            if (objInmuebleArrto != null)
            {
                //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                //obtener nombre de la institucion
                objInmuebleArrto.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(objInmuebleArrto.IdInstitucion);
                //obtener nombre del cargo
                //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                //obtener nombre de usuario
                objInmuebleArrto.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(objInmuebleArrto.IdUsuarioRegistro);
                //obtener nombre del pais
                objInmuebleArrto.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(objInmuebleArrto.IdPais);
                //obtener nombre del tipo de  vialidad
                objInmuebleArrto.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(objInmuebleArrto.IdTipoVialidad);

                if (QuitarAcentosTexto(objInmuebleArrto.NombrePais.ToUpper()) == "MEXICO")
                {
                    //obtener nombre de la ent. fed
                    objInmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(objInmuebleArrto.IdEstado.Value);
                    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                    objInmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(objInmuebleArrto.IdEstado.Value, objInmuebleArrto.IdMunicipio.Value);
                    if (objInmuebleArrto.IdLocalidadColonia != null)
                        //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                        objInmuebleArrto.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(objInmuebleArrto.IdPais, objInmuebleArrto.IdEstado.Value, objInmuebleArrto.IdMunicipio.Value, objInmuebleArrto.IdLocalidadColonia.Value);
                    else
                        objInmuebleArrto.NombreLocalidadColonia = objInmuebleArrto.OtraColonia;
                }
            }
            return objInmuebleArrto;
        }

        public List<ModeloNegocios.InmuebleArrto> ObtenerMvtosEmisionOpinionInmueblesRegistrados(
            int? IdInstitucion,
            int? FolioOpinion,
            int IdEstado,
            int IdMunicipio,
            int FolioSMOI,
            int? FolioSAEF)
        {
            List<ModeloNegocios.InmuebleArrto> ListInmueblesArrto;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();

            //obtener informacion de la BD

            ListInmueblesArrto = Conn.ObtenerMvtosEmisionOpinionInmueblesRegistrados(
                IdInstitucion,
                FolioOpinion,
                IdEstado,
                IdMunicipio,
                FolioSMOI,
                FolioSAEF);

            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor
            // MZT test

          
                foreach (ModeloNegocios.InmuebleArrto ObjList in ListInmueblesArrto)
                {

                    //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                    //obtener nombre de la institucion                   
                    ObjList.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.IdInstitucion);

                    //RCA 02/01/2018
                    //se comento ya que del stored se obtiene el nombre del cargo del stored
                    //obtener nombre del cargo
                    //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);

                    //obtener nombre de usuario
                    // MZT Consulta muy lenta y retraza el proceso en producción, se obtiene el nombre durante el databound del grid
                    ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);

                

                    // MZT Consulta muy lenta y retraza el proceso en producción, se obtiene el nombre durante el databound del grid
                    //obtener nombre del pais
                    ObjList.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(ObjList.IdPais);


                    //obtener nombre del tipo de  vialidad
                    ObjList.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(ObjList.IdTipoVialidad);


                    if (QuitarAcentosTexto(ObjList.NombrePais.ToUpper()) == "MEXICO")
                    {
                        //obtener nombre de la ent. fed
                        ObjList.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(ObjList.IdEstado.Value);


                        //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                        ObjList.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(ObjList.IdEstado.Value, ObjList.IdMunicipio.Value);

                         
                        if (ObjList.IdLocalidadColonia != null)
                        {
                            //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                            ObjList.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.IdPais, ObjList.IdEstado.Value, ObjList.IdMunicipio.Value, ObjList.IdLocalidadColonia.Value);
                        }              
                        else
                        {
                            ObjList.NombreLocalidadColonia = ObjList.OtraColonia; 
                        }
                            
                    }
                }
           
            //MZT
            return ListInmueblesArrto;
        }

        public List<ModeloNegocios.InmuebleArrto> ObtenerMvtosContratosInmueblesRegistrados(int? OtrasFiguras, int? IdInstitucion, int FolioContratoArrto, int IdPais, int IdEstado, int IdMunicipio, string RIUF)
        {
            List<ModeloNegocios.InmuebleArrto> ListInmueblesArrto;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            //obtener informacion de la BD
            ListInmueblesArrto = Conn.ObtenerMvtosContratosInmueblesRegistrados(OtrasFiguras, IdInstitucion, FolioContratoArrto, IdPais, IdEstado, IdMunicipio, RIUF);

                foreach (ModeloNegocios.InmuebleArrto ObjList in ListInmueblesArrto)
                {
                    
                        //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                        //obtener nombre de la institucion
                        ObjList.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.IdInstitucion);
                        //obtener nombre de usuario
                        //MZT 09/agosto/2017, se pasara a consulta en el databound del grid
                        ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
                        //MZT 09/agosto/2017, se pasara a consulta en el databound del grid
                        //obtener nombre del pais
                        ObjList.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(ObjList.IdPais);
                        //obtener nombre del tipo de  vialidad
                        ObjList.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(ObjList.IdTipoVialidad);

                        if (QuitarAcentosTexto(ObjList.NombrePais.ToUpper()) == "MEXICO")
                        {
                            //obtener nombre de la ent. fed
                            ObjList.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(ObjList.IdEstado.Value);
                            //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                            ObjList.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(ObjList.IdEstado.Value, ObjList.IdMunicipio.Value);
                            if (ObjList.IdLocalidadColonia != null)
                            {
                                //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                                ObjList.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.IdPais, ObjList.IdEstado.Value, ObjList.IdMunicipio.Value, ObjList.IdLocalidadColonia.Value);
                            }
                            else
                            {
                                ObjList.NombreLocalidadColonia = ObjList.OtraColonia;
                            }
                         }
                  }
  
            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor      
            return ListInmueblesArrto;
        }

        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }

        public int ObtenerConteoInmueblesArrtoXInstitucion(int IdInstitucion)
        {
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            return Conn.ObtenerConteoInmueblesArrtoXInstitucion(IdInstitucion);
        }

        public int UpdateIdInmuebleByIdInmuebleArrendamiento(int IdInmueble, int IdInmuebleArrendamiento)
        {
            int iAffect;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            iAffect = Conn.UpdateIdInmuebleByIdInmuebleArrendamiento(IdInmueble, IdInmuebleArrendamiento);
            return iAffect;
        }

        //insert de un inmueble arrto.
        public int InsertInmuebleArrto(ModeloNegocios.InmuebleArrto objInmuebleArtto)
        {
            int iAffect;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            iAffect = Conn.InsertInmuebleArrto(objInmuebleArtto);
            return iAffect;
        }

        //obtener el nombre de un estado y un mpo de un IdInmuebleArrto.
        public ModeloNegocios.InmuebleArrto ObtenerEstadoMpoXIdInmuebleArrto(int IdInmuebleArrto)
        {
            ModeloNegocios.InmuebleArrto objInmuebleArrto;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            objInmuebleArrto = Conn.ObtenerEstadoMpoXIdInmuebleArrto(IdInmuebleArrto);

            //obtener nombre de la ent. fed
            objInmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(objInmuebleArrto.IdEstado.Value);
            //obtener nombre de los mpos (pasar IdEstado y IdMpo)
            objInmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(objInmuebleArrto.IdEstado.Value, objInmuebleArrto.IdMunicipio.Value);

            return objInmuebleArrto;
        }

        //RCA 17/08/2017
        public int UpdateBandera(int FolioContrato)
        {
            int iAffect;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();
            iAffect = Conn.UpdateBandera(FolioContrato);
            return iAffect;
        }

        //RCA 11/05/2018
        //metodo para el estatus del RUSP
        public List<EstatusRUSPvsRIUF> ObtenerEstatusRUSPvsRIUF(DateTime? FechaInicio, DateTime? FechaFin, int? IdInstitucion, string RIUF, int? IdPais, int? IdEstado, int? IdMunicipio, string CP, int? TipoRegistro, int? EstatusRUSP, int? FolioContrato)
        {
            List<EstatusRUSPvsRIUF> ListInmueblesRUSP = null;
            ListInmueblesRUSP = InmuebleArrtoDAL.ObtenerInformacionEstatusRUSP(FechaInicio,FechaFin,IdInstitucion,RIUF,IdPais,IdEstado,IdMunicipio,CP,TipoRegistro,EstatusRUSP,FolioContrato);

            //obtener la informacion del usuario
            foreach(EstatusRUSPvsRIUF ObjList in ListInmueblesRUSP)
            {
                ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuario);
            }

            return ListInmueblesRUSP;
        }

        //RCA 14/05/2018
        //metodo para habilitar o deshabilitar los RIUF
        public Boolean HabilitarDeshabilitarRIUF(int FolioContrato,int TipoCaso)
        {
            bool ok = false;
            ok = InmuebleArrtoDAL.HabilitarDeshabilitarRIUF(FolioContrato, TipoCaso);
            return ok;
        }

        //RCA 06/07/2018
        //metodo para obtener solo los inmuebles ue tiene smoi registrados
        public List<ModeloNegocios.InmuebleArrto> ObtenerMvtosSAEFInmueblesRegistrados(int? IdInstitucion, int? FolioOpinion, int IdEstado, int IdMunicipio, int FolioSMOI,int? FolioSAEF)
        {
            List<ModeloNegocios.InmuebleArrto> ListInmuebleSAEF = null;
            AccesoDatos.InmuebleArrtoDAL Conn = new AccesoDatos.InmuebleArrtoDAL();

            //obtener la informacion de la DB
            ListInmuebleSAEF = Conn.ObtenerMvtosEmisionOpinionInmueblesRegistrados(IdInstitucion, FolioOpinion, IdEstado, IdMunicipio, FolioSMOI,FolioSAEF).Where(x => x.EmisionOpinion.FolioAplicacionConcepto != null).ToList();

            //recorres la  lista del objeto y obtener sus correspondientes valores
            foreach(ModeloNegocios.InmuebleArrto ObjListSAEF in ListInmuebleSAEF )
            {
                //obtener del bus las descripcciones correspondientes

                //obtener la institucion
                ObjListSAEF.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjListSAEF.IdInstitucion);

                //obtener el nombre del usuario
                ObjListSAEF.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjListSAEF.IdUsuarioRegistro);

                //obtener nombre del pais
                ObjListSAEF.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(ObjListSAEF.IdPais);

                //obtener el nombre de la vialidad
                ObjListSAEF.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(ObjListSAEF.IdTipoVialidad);

                //quitar el acento al pais de mexico
                if (QuitarAcentosTexto(ObjListSAEF.NombrePais.ToUpper()) == "MEXICO")
                {
                    //obtener nombre de la ent. fed
                    ObjListSAEF.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(ObjListSAEF.IdEstado.Value);
                    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                    ObjListSAEF.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(ObjListSAEF.IdEstado.Value, ObjListSAEF.IdMunicipio.Value);

                    if (ObjListSAEF.IdLocalidadColonia != null)
                    {
                        //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                        ObjListSAEF.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjListSAEF.IdPais, ObjListSAEF.IdEstado.Value, ObjListSAEF.IdMunicipio.Value, ObjListSAEF.IdLocalidadColonia.Value);
                    }
                        
                    else
                    {
                        ObjListSAEF.NombreLocalidadColonia = ObjListSAEF.OtraColonia;
                    }
                       
                }
            }


            return ListInmuebleSAEF;
        }

        //RCA 19/07/2018
        //metodo para eo encabezado
        public string Encabezado()
        {
            string Encabezado = string.Empty;
            Encabezado = InmuebleArrtoDAL.EncabezadoSAEF();
            return Encabezado;
        }

        //RCA 19/07/2018
        //metodo para el pie de pagina
        public string PiePagina()
        {
            string PiePagina = string.Empty;
            PiePagina = InmuebleArrtoDAL.PiePaginaSAEF();
            return PiePagina;
        }

        //RCA 19/07/2018
        //metodo para el cuerpo de saef
        public string CuerpoSAEF()
        {
            string Cuerpo = string.Empty;
            Cuerpo = InmuebleArrtoDAL.CuerpoSAEF();
            return Cuerpo;
        }

    }//clase
}
