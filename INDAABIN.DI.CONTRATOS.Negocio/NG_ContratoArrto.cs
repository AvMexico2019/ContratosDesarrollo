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
    public class NG_ContratoArrto
    {
        public List<ModeloNegocios.ContratoArrtoHistorico> ObtenerContratosArrtoHistorico(int IdInstitucion, byte IdEstado, String NombreMunicipio)
        {
            List<ModeloNegocios.ContratoArrtoHistorico> ListContratosArrtoHistorico;
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            ListContratosArrtoHistorico = Conn.ObtenerContratosArrtoHistorico(IdInstitucion, IdEstado, NombreMunicipio);

            return ListContratosArrtoHistorico;
        }

        public int UpdateRIUFByFolioContrato(string RIUF, int FolioContratoArrendamiento, int Institucion)
        {
            int FolioContrato;
            //llamado a la capa de datos, para insertar la informacion
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            FolioContrato = Conn.UpdateRIUFByFolioContrato(RIUF, FolioContratoArrendamiento, Institucion);
            return FolioContrato;
        }

        //insert de un Contrato Arrto.
        public int InsertContratoArrto(ModeloNegocios.ContratoArrto objContratoArrto)
        {
            int FolioContrato;
            //llamado a la capa de datos, para insertar la informacion
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            FolioContrato = Conn.InsertContratoArrto(objContratoArrto);
            return FolioContrato;
        }

        //insert de un Contrato Arrto de Otras Fig. de Ocuapcion
        public int InsertContratoArrtoOtrasFigOcupacion(ModeloNegocios.ContratoArrto objContratoArrto)
        {
            int FolioContrato;
            //llamado a la capa de datos, para insertar la informacion
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            FolioContrato = Conn.InsertContratoArrtoOtrasFigOcupacion(objContratoArrto);
            return FolioContrato;
        }

        //Acuse de Contrato
        public AcuseContrato ObtenerAcuseContrato(int IdFolioContrato)
        {
            AcuseContrato objAcuseContrato;

            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            objAcuseContrato = Conn.ObtenerAcuseContrato(IdFolioContrato);

            //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
            objAcuseContrato.ContratoArrto.InmuebleArrto.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(objAcuseContrato.ContratoArrto.InmuebleArrto.IdTipoVialidad);
            objAcuseContrato.ContratoArrto.InmuebleArrto.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(objAcuseContrato.ContratoArrto.InmuebleArrto.IdPais);


            if (QuitarAcentosTexto(objAcuseContrato.ContratoArrto.InmuebleArrto.NombrePais.ToUpper()) == "MEXICO")
            {
                //obtener nombre de la ent. fed
                objAcuseContrato.ContratoArrto.InmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(objAcuseContrato.ContratoArrto.InmuebleArrto.IdEstado.Value);
                //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                objAcuseContrato.ContratoArrto.InmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(objAcuseContrato.ContratoArrto.InmuebleArrto.IdEstado.Value, objAcuseContrato.ContratoArrto.InmuebleArrto.IdMunicipio.Value);
                if (objAcuseContrato.ContratoArrto.InmuebleArrto.IdLocalidadColonia != null)
                    //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                    objAcuseContrato.ContratoArrto.InmuebleArrto.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(objAcuseContrato.ContratoArrto.InmuebleArrto.IdPais, objAcuseContrato.ContratoArrto.InmuebleArrto.IdEstado.Value, objAcuseContrato.ContratoArrto.InmuebleArrto.IdMunicipio.Value, objAcuseContrato.ContratoArrto.InmuebleArrto.IdLocalidadColonia.Value);
                else
                    objAcuseContrato.ContratoArrto.InmuebleArrto.NombreLocalidadColonia = objAcuseContrato.ContratoArrto.InmuebleArrto.OtraColonia;
            }
            return objAcuseContrato;
        }


        public List<ModeloNegocios.ContratoArrto> ObtenerContratosArrtoRegistrados(int? IdInstitucion, int? FolioContratoArrto, byte? TipoContato)
        {
            List<ModeloNegocios.ContratoArrto> ListContratosArrtoRegistrados;
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            ListContratosArrtoRegistrados = Conn.ObtenerContratosArrtoRegistrados(IdInstitucion, FolioContratoArrto, TipoContato);

            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor
            foreach (ModeloNegocios.ContratoArrto ObjList in ListContratosArrtoRegistrados)
            {
                //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                //obtener nombre de la institucion
                ObjList.InmuebleArrto.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.InmuebleArrto.IdInstitucion);
                //obtener nombre del cargo
                //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                ////obtener nombre de usuario
                //ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
                //obtener nombre del pais
                ObjList.InmuebleArrto.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(ObjList.InmuebleArrto.IdPais);
                //obtener nombre del tipo de  vialidad
                ObjList.InmuebleArrto.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(ObjList.InmuebleArrto.IdTipoVialidad);

                if (QuitarAcentosTexto(ObjList.InmuebleArrto.NombrePais.ToUpper()) == "MEXICO")
                {
                    //obtener nombre de la ent. fed
                    ObjList.InmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(ObjList.InmuebleArrto.IdEstado.Value);
                    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                    ObjList.InmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(ObjList.InmuebleArrto.IdEstado.Value, ObjList.InmuebleArrto.IdMunicipio.Value);
                    if (ObjList.InmuebleArrto.IdLocalidadColonia != null)
                    {
                        //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                        ObjList.InmuebleArrto.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.InmuebleArrto.IdPais, ObjList.InmuebleArrto.IdEstado.Value, ObjList.InmuebleArrto.IdMunicipio.Value, ObjList.InmuebleArrto.IdLocalidadColonia.Value);
                    }
                    else
                    {
                        ObjList.InmuebleArrto.NombreLocalidadColonia = ObjList.InmuebleArrto.OtraColonia;
                    }
                }//fin del if 
            }//fin del foreach
            return ListContratosArrtoRegistrados;
        }

        public ContratoArrto ObtenerContratoArrto(int IdInstitucion, int FolioContratoArrto)
        {
            ModeloNegocios.ContratoArrto objContratoArrto;
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            objContratoArrto = Conn.ObtenerContratosArrtoRegistrados(IdInstitucion, FolioContratoArrto, null).FirstOrDefault();

            if (objContratoArrto != null)
            {
                //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                //obtener nombre de la institucion
                objContratoArrto.InmuebleArrto.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(objContratoArrto.InmuebleArrto.IdInstitucion);
                //obtener nombre del cargo
                //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                ////obtener nombre de usuario
                //ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
                //obtener nombre del pais
                objContratoArrto.InmuebleArrto.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(objContratoArrto.InmuebleArrto.IdPais);
                //obtener nombre del tipo de  vialidad
                objContratoArrto.InmuebleArrto.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(objContratoArrto.InmuebleArrto.IdTipoVialidad);

                if (QuitarAcentosTexto(objContratoArrto.InmuebleArrto.NombrePais.ToUpper()) == "MEXICO")
                {
                    //obtener nombre de la ent. fed
                    objContratoArrto.InmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(objContratoArrto.InmuebleArrto.IdEstado.Value);
                    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                    objContratoArrto.InmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(objContratoArrto.InmuebleArrto.IdEstado.Value, objContratoArrto.InmuebleArrto.IdMunicipio.Value);
                    if (objContratoArrto.InmuebleArrto.IdLocalidadColonia != null)
                        //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                        objContratoArrto.InmuebleArrto.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(objContratoArrto.InmuebleArrto.IdPais, objContratoArrto.InmuebleArrto.IdEstado.Value, objContratoArrto.InmuebleArrto.IdMunicipio.Value, objContratoArrto.InmuebleArrto.IdLocalidadColonia.Value);
                    else
                        objContratoArrto.InmuebleArrto.NombreLocalidadColonia = objContratoArrto.InmuebleArrto.OtraColonia;
                }
            }
            return objContratoArrto;
        }

        public List<ModeloNegocios.ContratoArrto> ObtenerExcepcionNormatividadContratosArrtoRegistrados(int? IdInstitucion, int? FolioContratoArrto, byte? TipoContato)
        {
            List<ModeloNegocios.ContratoArrto> ListContratosArrtoRegistrados;
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            ListContratosArrtoRegistrados = Conn.ObtenerExcepcionNormatividadContratosArrtoRegistrados(IdInstitucion, FolioContratoArrto, TipoContato);


            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor
            foreach (ModeloNegocios.ContratoArrto ObjList in ListContratosArrtoRegistrados)
            {

                //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***
                //obtener nombre de la institucion
                ObjList.InmuebleArrto.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.InmuebleArrto.IdInstitucion);
                //obtener nombre del cargo
                //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                ////obtener nombre de usuario
                //ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
                //obtener nombre del pais
                ObjList.InmuebleArrto.NombrePais = Negocio.AdministradorCatalogos.ObtenerNombrePais(ObjList.InmuebleArrto.IdPais);
                //obtener nombre del tipo de  vialidad
                ObjList.InmuebleArrto.NombreTipoVialidad = Negocio.AdministradorCatalogos.ObtenerNombreTipoVialidad(ObjList.InmuebleArrto.IdTipoVialidad);



                if (QuitarAcentosTexto(ObjList.InmuebleArrto.NombrePais.ToUpper()) == "MEXICO")
                {
                    //obtener nombre de la ent. fed
                    ObjList.InmuebleArrto.NombreEstado = Negocio.AdministradorCatalogos.ObtenerNombreEstado(ObjList.InmuebleArrto.IdEstado.Value);
                    //obtener nombre de los mpos (pasar IdEstado y IdMpo)
                    ObjList.InmuebleArrto.NombreMunicipio = Negocio.AdministradorCatalogos.ObtenerNombreMunicipio(ObjList.InmuebleArrto.IdEstado.Value, ObjList.InmuebleArrto.IdMunicipio.Value);
                    if (ObjList.InmuebleArrto.IdLocalidadColonia != null)
                        //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                        ObjList.InmuebleArrto.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.InmuebleArrto.IdPais, ObjList.InmuebleArrto.IdEstado.Value, ObjList.InmuebleArrto.IdMunicipio.Value, ObjList.InmuebleArrto.IdLocalidadColonia.Value);
                    else
                        ObjList.InmuebleArrto.NombreLocalidadColonia = ObjList.InmuebleArrto.OtraColonia;
                }

            }

            return ListContratosArrtoRegistrados;
        }


        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }


        //devuelve un scalar como String
        public string ObteneExcepcionNormatividadPreviaContrato(byte TipoContrato, decimal AreaOcupadaM2, decimal RentaMensualUnitaria,
                         int? FolioEmisionOpinion, string SuperficieDictaminada_Justipreciacion,
                         decimal? MontoDictaminado_Justipreciacion, string NumeroDictamenExcepcionFolioSMOI)
        {


            String MsjExcepcionNormatividad;
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();
            MsjExcepcionNormatividad = Conn.ObteneExcepcionNormatividadPreviaContrato(
                                TipoContrato, AreaOcupadaM2, RentaMensualUnitaria,
                                FolioEmisionOpinion, SuperficieDictaminada_Justipreciacion,
                                MontoDictaminado_Justipreciacion, NumeroDictamenExcepcionFolioSMOI
                                );

            return MsjExcepcionNormatividad;
        }

        //RCA 13/08/2018
        //metodo para obtener el id del contrato
        public int IdContrato(string FolioContrato)
        {
            AccesoDatos.ContratoArrtoDAL Conn = new AccesoDatos.ContratoArrtoDAL();

            int IdContrato = 0;
            IdContrato = Conn.IdContratoArrto(FolioContrato);
            return IdContrato;
        }

        //metodo para actualizar el qr en contrato arrto 
        public Boolean ActualizarQRContrato(string QR, int IdapContrato)
        {
            AccesoDatos.ContratoArrtoDAL conn = new AccesoDatos.ContratoArrtoDAL();

            bool ok = false;
            ok = conn.ActualizarConytratoQR(QR, IdapContrato);
            return ok;
        }

        public List<InmuebleArrto> ObtenerContratosConvenioModificatorio(int IdInstitucion, int FolioContrato, string RIUF, int IdPais, int IdEdo, int IdMunicipio)
        {
            List<InmuebleArrto> Linmueble = new List<InmuebleArrto>();

            try
            {
                ContratoArrtoDAL cDAL = new ContratoArrtoDAL();
                Linmueble = cDAL.ObtenerContratosConvenioModificatorio(IdInstitucion, FolioContrato, RIUF, IdPais, IdEdo, IdMunicipio);

                if (Linmueble.Count > 0)
                {
                    foreach (InmuebleArrto ObjList in Linmueble)
                    {
                        ObjList.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.IdInstitucion);
                        //obtener nombre del cargo
                        //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                        ////obtener nombre de usuario
                        //ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
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
                                //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                                ObjList.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.IdPais, ObjList.IdEstado.Value, ObjList.IdMunicipio.Value, ObjList.IdLocalidadColonia.Value);
                            else
                                ObjList.NombreLocalidadColonia = ObjList.OtraColonia;
                        }
                    }
                }
            }

            catch (Exception) { throw; }

            return Linmueble;
        }

        public bool GenerarConvenioModificatorio(Convenio Convenio, int IdUsuario, JustripreciacionContrato JustripreciacionContrato, ref string msjError, ref string fechaRegistro)
        {
            bool respuesta = false;

            try
            {
                ContratoArrtoDAL cDAL = new ContratoArrtoDAL();
                respuesta = cDAL.GenerarConvenioModificatorio(Convenio, IdUsuario, JustripreciacionContrato, ref msjError, ref fechaRegistro);
            }

            catch (Exception) { throw; }

            return respuesta;
        }

        public bool AutorizarConvenioModificatorio(int IdConvenioModificatorio, string CadOrignal, string Sello, string QR, int IdUsuario, ref string fechaRegistro)
        {
            bool respuesta = false;

            try
            {
                ContratoArrtoDAL cDAL = new ContratoArrtoDAL();
                respuesta = cDAL.AutorizarConvenioModificatorio(IdConvenioModificatorio, CadOrignal, Sello, QR, IdUsuario, ref fechaRegistro);
            }

            catch (Exception) { throw; }

            return respuesta;
        }

        public List<Convenio> ObtenerConveniosContrato(int FolioContrato)
        {
            List<Convenio> Lconvenio = new List<Convenio>();

            try
            {
                ContratoArrtoDAL cDAL = new ContratoArrtoDAL();
                Lconvenio = cDAL.ObtenerConveniosContrato(FolioContrato);
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

            try
            {
                ContratoArrtoDAL cDal = new ContratoArrtoDAL();
                Convenio = cDal.ObtenerConvenioModificatorio(folioConvenio, ref msjError);
            }

            catch (Exception ex)
            {
                throw new Exception(string.Format("ObtenerConveniosContrato:{0}", ex.Message));
            }

            return Convenio;
        }
    }
}
