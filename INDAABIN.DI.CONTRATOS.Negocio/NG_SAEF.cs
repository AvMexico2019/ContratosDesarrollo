using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.AccesoDatos;
using INDAABIN.DI.ModeloNegocio;


namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class NG_SAEF
    {
        public List<ConceptoSAEF> ObtenerConceptosSAEF()
        {
            List<ConceptoSAEF> ListConSAEF = null;
            ListConSAEF = SAEFDAL.ObtenerConceptosSAEF();
            return ListConSAEF;
        }

        public EmisionOpinionSAEF ObtenerEmisionSAEF(string FolioEmision)
        {
            EmisionOpinionSAEF ObjEmisionSAEF = null;
            ObjEmisionSAEF = SAEFDAL.ObtenerDatosEmisionSAEF(FolioEmision);

            //obtenemos el nombre del usuario
            ObjEmisionSAEF.NombreUsuarioEmisionOpinion = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjEmisionSAEF.IdUsuarioEmisionOpinion); 


            return ObjEmisionSAEF;
        }

        //metodo para guardar las respuesta de la emision de saef
        public Boolean GuardarSAEF(ValorRespuestaSAEF ObjSAEF,int TipoGuardao,string Cadena, string Sello,string QR)
        {
            bool ok = false;
            ok = SAEFDAL.GuardarEmisionSAEF(ObjSAEF,TipoGuardao,Cadena,Sello,QR);
            return ok;
        }

        //metodo para obtener el header del acuse saef
        public AcuseHeaderSAEF ObtenerAcuseHeaderSAEF(int? IdAplicacionConcepto)
        {
            AcuseHeaderSAEF ObjHeaderSAEF = new AcuseHeaderSAEF();
            ObjHeaderSAEF = SAEFDAL.ObtenerHeaderAcuseSAEF(IdAplicacionConcepto);
            return ObjHeaderSAEF;
        }

        //metodo para obtener el cuerpo de saef
        public List<AcuseCuerpoSAEF> ObtenerAcuseCuerpoSAEF(int? IdApConcep)
        {
            List<AcuseCuerpoSAEF> ListCuerpoSAEF = null;
            ListCuerpoSAEF = SAEFDAL.ObtenerCuerpoAcuseSAEF(IdApConcep);
            return ListCuerpoSAEF;
        }

        //metodo para obtener el idaplicacionconcepto
        public int ObtenerAplpicacionConcepto(int FolioSaef)
        {
            int Folio = 0;
            Folio = SAEFDAL.ObtenerIdAplicacionEmision(FolioSaef);
            return Folio;
        }

        public List<ModeloNegocios.AplicacionConcepto> ObtenerSolicitudesEmisionOpinionEmitidasSAEF(int? IdInstitucion, int FolioAplicacionConcepto, byte? IdTema, int? FolioSAEF)
        {
            List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_Opinion;
            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            ListAplicacionConcepto_Opinion = Conn.ObtenerSolicitudesEmisionOpinionEmitidas(IdInstitucion, FolioAplicacionConcepto, IdTema, FolioSAEF).Where(x => x.FolioSAEF != null).ToList();


            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor
            foreach (ModeloNegocios.AplicacionConcepto ObjList in ListAplicacionConcepto_Opinion)
            {

                //*** Re-mapear Id-Bus con el BUS para obtener descripciones correspondientes  ***

                //obtener nombre de la institucion
                ObjList.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.IdInstitucion);
                //obtener nombre del cargo
                // ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                //obtener nombre de usuario
                //MZT 09/agosto/2017
                ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
                //MZT 09/agosto/2017
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
                    //obtener nombre de las localidades (pasar IdMpo y IdLocalidad)
                    if (ObjList.InmuebleArrto.IdLocalidadColonia != null)
                        ObjList.InmuebleArrto.NombreLocalidadColonia = Negocio.AdministradorCatalogos.ObtenerNombreLocalidad(ObjList.InmuebleArrto.IdPais, ObjList.InmuebleArrto.IdEstado.Value, ObjList.InmuebleArrto.IdMunicipio.Value, ObjList.InmuebleArrto.IdLocalidadColonia.Value);
                    else
                        ObjList.InmuebleArrto.NombreLocalidadColonia = ObjList.InmuebleArrto.OtraColonia;
                }

            }

            return ListAplicacionConcepto_Opinion;
        }

        public String ObtenerLeyendaQR()
        {
            string Leyenda = string.Empty;
            Leyenda = SAEFDAL.ObtenerLeyendaQR();
            return Leyenda;
        }

        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }

        public List<ValorRespuestaSAEF> ObtenerValorRespuestaAplicacionConcepto(int IdAplicacionConcepto)
        {
            List<ValorRespuestaSAEF> Lvalor = new List<ValorRespuestaSAEF>();

            try
            {
                SAEFDAL sDal = new SAEFDAL();
                Lvalor = sDal.ObtenerValorRespuestaAplicacionConcepto(IdAplicacionConcepto);
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return Lvalor;
        }

    }
}
