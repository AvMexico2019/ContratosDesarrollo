using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.AccesoDatos;
using INDAABIN.DI.ModeloNegocio;
using INDAABIN.DI.BPM.CorreoElectronico.Negocio;

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class NG
    {

        public SSO ObtenerUsuario(string usuario)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            SSO user = ws_bus.ObtenerUsuario(usuario).FirstOrDefault();
            return user;
        }

        public SSO ObtenerUsuarioXId(int IdUsuario)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            SSO user = ws_bus.ObtenerUsuarioXId(IdUsuario).FirstOrDefault();
            return user;
        }

        public List<Catalogo> LlenaCombo(string ConsultaCatalogo)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            List<Catalogo> Combo = ws_bus.LlenaCombos(ConsultaCatalogo);
            return Combo;
        }

        public List<CatalogoElementos> LlenaCombos3Datos(string ConsultaCatalogo)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            List<CatalogoElementos> Combo = ws_bus.LlenaCombos3Datos(ConsultaCatalogo);
            return Combo;
        }

        public List<CatalogoDependiente> LlenaComboDependiente(string ConsultaCatalogo)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            List<CatalogoDependiente> ComboEstado = ws_bus.LlenaCombosElemento(ConsultaCatalogo);
            ws_bus = null;
            return ComboEstado;
        }

        public List<CatalogoDependiente> LlenaSector(int IdInstitucion)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            List<CatalogoDependiente> ComboSector = ws_bus.LlenaCombosSectores(IdInstitucion);
            ws_bus = null;
            return ComboSector;

        }



        public SolicitudAvaluos ObtenerJustipreciacionAvaluos(string pSecuencial)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            SolicitudAvaluos objSolicitudAvaluos = null;

            // MZT 15/08/2017
            var avaluo = JustipreciacionDAL.Consulta(pSecuencial);

            if (avaluo == null)
            {
                // MZT 15/08/2017
                var justipreciaciones = ws_bus.ObtenerJustipreciacionAvaluos(pSecuencial);

                if (justipreciaciones != null)
                {
                    objSolicitudAvaluos = justipreciaciones.FirstOrDefault();
                }
                // MZT 15/08/2017
            }
            else
            {
                //nombre de la institucion 
                var instucion = AdministradorCatalogos.ObtenerCatalogoInstitucion().FirstOrDefault(x => x.IdValue == avaluo.InstitucionId);

                var MunicipioDes = AdministradorCatalogos.ObtenerCatalogoMunicipio().FirstOrDefault(x => x.IdValue == avaluo.MunicipioId);

                var EstadoDesc = AdministradorCatalogos.ObtenerCatalogoEstados().FirstOrDefault(x => x.IdValue == avaluo.EstadoId);

                var UnidadMedida = AdministradorCatalogos.ObtenerCatalogoUnidadMedida().FirstOrDefault(x => x.IdValue == Convert.ToInt32(avaluo.UnidadMedidaRentableDictaminado));

                // MZT 16/08/2017
                objSolicitudAvaluos = new SolicitudAvaluos
                {
                    Calle = avaluo.Calle,
                    Cargo = avaluo.Cargo,
                    Ciudad = avaluo.Ciudad,
                    CP = avaluo.CP,
                    Email = avaluo.Email,
                    EstadoDescripcion = EstadoDesc != null ? EstadoDesc.Descripcion : string.Empty,
                    EstadoId = avaluo.EstadoId,
                    Estatus = "CONCLUIDO",
                    FechaDictamen = avaluo.FechaDictamen.ToString("d"),//convertimos de datetime a string
                    //FechaDictamen = avaluo.FechaDictamen,
                    InstitucionDescripcion = instucion != null ? instucion.Descripcion : string.Empty,
                    InstitucionId = avaluo.InstitucionId,
                    MontoDictaminado = avaluo.MontoDictaminado,
                    MunicipioDescripcion = MunicipioDes != null ? MunicipioDes.Descripcion : string.Empty,
                    MunicipioId = avaluo.MunicipioId,
                    NoExterior = avaluo.NoExterior,
                    NoGenerico = avaluo.NoGenerico,
                    NoInterior = avaluo.NoInterior,
                    NoSecuencial = avaluo.NoSecuencial,

                    //Propietario = avaluo.Nombre,
                    Responsable = avaluo.Responsable,
                    SectorDescripcion = avaluo.SectorDescripcion,
                    SectorId = avaluo.SectorId,
                    SuperficieConstruida = avaluo.SuperficieConstruida,
                    SuperficieConstruidaDictaminado = avaluo.SuperficieConstruidaDictaminado,
                    SuperficieRentable = avaluo.SuperficieRentable,
                    SuperficieRentableDictaminado = avaluo.SuperficieRentableDictaminado,
                    SuperficieTerreno = avaluo.SuperficieTerreno,
                    SuperficieTerrenoDictaminado = avaluo.SuperficieTerrenoDictaminado,
                    UnidadMedidaConstruida = avaluo.UnidadMedidaConstruida,
                    UnidadMedidaConstruidaDictaminado = avaluo.UnidadMedidaConstruidaDictaminado,
                    UnidadMedidaRentable = UnidadMedida != null ? UnidadMedida.Descripcion : string.Empty,
                    UnidadMedidaRentableDictaminado = UnidadMedida != null ? UnidadMedida.Descripcion : string.Empty,
                    UnidadMedidaTerreno = avaluo.UnidadMedidaTerreno,
                    UnidadMedidaTerrenoDictaminado = avaluo.UnidadMedidaTerrenoDictaminado
                };
                // MZT 16/08/2017
            }

            return objSolicitudAvaluos;
        }




        public static void NotificarPorcorreo(string encabezado, string msgCuerpo, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(msgCuerpo) && !string.IsNullOrEmpty(encabezado) && !string.IsNullOrEmpty(email))
                {

                    //RCA 09/01/2018
                    //utilizamos la nueva forma de envio de correos por medio del SMTP
                    EnvioCorreo EnviaCorreo = new EnvioCorreo();
                    Correo correo = new Correo();
                    Remitente RemitenteCorreo = new Remitente();

                    correo.Destinatarios = email;
                    correo.Mensaje = msgCuerpo;
                    correo.Asunto = encabezado;

                    EnviaCorreo.EnviarCorreoElectronico(correo,RemitenteCorreo);

                    //ControladorBUS bus = new ControladorBUS();
                    // bus.EnviarCorreo(encabezado, msgCuerpo, email);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public static List<SSO> ObtenerJefeDepartamento(int idaplicacion, int idRole)
        //{
        //    List<SSO> ssos = null;
        //    try
        //    {
        //        ControladorBUS bus = new ControladorBUS();

        //        ssos = bus.ObtenerUsuariosPoAplicacion(idaplicacion, idRole);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return ssos;
        //}

        public static object ConsultarBitacora()
        {
            return null;
        }

        public List<CatalogoDependiente> LlenaComboUsoEspecifico(int IdUsoGenerica)
        {
            ControladorBUS ws_bus = new ControladorBUS();
            List<CatalogoDependiente> catalogo = null;
            catalogo = ws_bus.LlenaCombosUsoEspecifico(IdUsoGenerica);
            ws_bus = null;
            return catalogo;
        }
    }
}
