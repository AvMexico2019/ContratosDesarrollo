using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//comunicacion con las capas 
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //entities
using INDAABIN.DI.CONTRATOS.AccesoDatos;
using INDAABIN.DI.ModeloNegocio; //interconexion al BUS
using INDAABIN.DI.CONTRATOS.Datos;

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    /// <summary>
    /// Este clase contiene metodos asociados a los conoceptos de negocio: 
    ///      - Emisión de Opinón
    ///      - Tabla SMOI
    ///  Interconecta con la capa DAL y GUI
    /// </summary>
    public class NGConceptoRespValor
    {

        List<ConceptoRespValor> ListCptosRespuestaVal;

        //obtener cptos de la capa DAL
        //parametro de entrada, IdTema: 
        //1	Concepto SMOI
        //2	Opinión Nuevo Arrendamiento
        //3	Opinión Continuación Arrendamiento
        //4	Opinión Sustitución Arrendamiento
        //public List<ConceptoRespValor> ObtenerCptosRespuestaValor(byte IdTema, int IdInstitucion)
        //{

        //    AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
        //    ListCptosRespuestaVal = Conn.ObtenerCptosRespuestaValor(IdTema, IdInstitucion);
        //    return ListCptosRespuestaVal;
        //}

        //parametro de entrada, IdTema: 
        //2	Opinión Nuevo Arrendamiento
        //3	Opinión Continuación Arrendamiento
        //4	Opinión Sustitución Arrendamiento
        //public List<ConceptoOpinion> ObtenerCptosEmisionOpinion(byte IdTema, int IdInstitucion)
        //{

        //    //AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
        //    List<ConceptoRespValor> ListCptosRespuestaValMultiples = new List<ConceptoRespValor>(); //lista origen con registros de la BD
        //    List<ConceptoRespValor> ListCptosRespuestaValXIdCptoTemp; //lista para seleccion de objetos de lista ListCptosRespuestaVal condicionados por IdCpto
        //    //objetos para nueva lista
        //    List<ConceptoOpinion> ListNuevosCptosOpinionAgrupados = null; //nueva lista de cptos de emisión de opinión
        //    ConceptoOpinion objCptoOpinion = null;
        //    //ListCptosRespuestaValMultiples = Conn.ObtenerCptosRespuestaValor(IdTema, IdInstitucion);
        //    ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();

        //    var result = ctx.spuSelectConceptosXResponderTema(IdTema, IdInstitucion);
        //    foreach (var reg in result)
        //    {
        //        ListCptosRespuestaValMultiples.Add(new ConceptoRespValor {
        //            IdConceptoRespValor = reg.IdConceptoRespValor,
        //            NumOrden = reg.NumOrden,
        //            IdTema = reg.Fk_IdTema,
        //            DescripcionTema = reg.DescripcionTema,
        //            IdConcepto = reg.Fk_IdConcepto,
        //            DescripcionConcepto =  reg.DescripcionConcepto,
        //            FundamentoLegal = reg.FundamentoLegal,
        //            EsDeterminante = reg.EsDeterminante,
        //            IdRespuesta = reg.Fk_IdRespuesta,
        //            DescripcionRespuesta = reg.DescripcionRespuesta,
        //            ValorPonderacionRespuesta = reg.ValorPonderacionRespuesta
        //        });
        //    }                               

        //    //TODO: crear una nuava lista para el tipo de custionario de emisión de opinión, donde 2 registros se convierten a 1
        //    if (ListCptosRespuestaValMultiples.Count > 0)
        //    {
        //        ListNuevosCptosOpinionAgrupados = new List<ConceptoOpinion>();
        //        //recorrer cada objeto de la lista para identificar sus respuestas multiples
        //        foreach (ConceptoRespValor CptoOpinion in ListCptosRespuestaValMultiples)
        //        {
        //            //verificar que exista un objeto de CptoOpinion en la lista
        //            //objCptoOpinion = ListCptosOpinion.Single(a => a.IdConcepto == CptoOpinion.IdConcepto);

        //            //devolver los cptosValorResp que coincidan con el IdConcepto que itera
        //            ListCptosRespuestaValXIdCptoTemp = (from x in ListCptosRespuestaValMultiples
        //                                                where x.IdConcepto == CptoOpinion.IdConcepto
        //                                                select x).ToList();

        //            //iterar por la coleccion
        //            foreach (ConceptoRespValor CptoOpinionXIdCpto in ListCptosRespuestaValXIdCptoTemp)
        //            {
        //                if (ListNuevosCptosOpinionAgrupados.Count > 0)
        //                {
        //                    //buscar en nueva lista de Cptos de opinion, si ya esta agregado el IdCpto
        //                    //objCptoOpinion = ListCptosOpinion.Single(a => a.IdConcepto == CptoOpinionXIdCpto.IdConcepto);
        //                    objCptoOpinion = (from x in ListNuevosCptosOpinionAgrupados
        //                                      where x.IdConcepto == CptoOpinion.IdConcepto
        //                                      select x).FirstOrDefault();
        //                }

        //                //sacar sus 2 valores de respuestas, para ponerlos en los atributos de 1 solo objeto
        //                if (ListCptosRespuestaValXIdCptoTemp.Count > 1)
        //                {
        //                    if (objCptoOpinion == null)
        //                    {
        //                        //crear objeto y remapear valor Min y Max al objeto mapeado
        //                        objCptoOpinion = new ConceptoOpinion
        //                        {
        //                            IdConceptoRespValor = CptoOpinionXIdCpto.IdConceptoRespValor,
        //                            NumOrden = CptoOpinionXIdCpto.NumOrden,
        //                            IdTema = CptoOpinionXIdCpto.IdTema,
        //                            DescripcionTema = CptoOpinionXIdCpto.DescripcionTema,
        //                            IdConcepto = CptoOpinionXIdCpto.IdConcepto,
        //                            DescripcionConcepto = CptoOpinionXIdCpto.DescripcionConcepto,
        //                            FundamentoLegal = CptoOpinionXIdCpto.FundamentoLegal,
        //                            EsDeterminante = CptoOpinionXIdCpto.EsDeterminante,
        //                            IdRespuesta = 0,//no aplica, no es tema
        //                            //la 1ra vez crear el objeto con el valor minimo
        //                            DescValorMinimo = CptoOpinionXIdCpto.DescripcionRespuesta,
        //                            ValorMinimo = CptoOpinionXIdCpto.ValorPonderacionRespuesta
        //                        };

        //                        //agregar a la lista
        //                        ListNuevosCptosOpinionAgrupados.Add(objCptoOpinion);

        //                    }
        //                    else //ya existe el objeto, complementar su propiedad de valor y desc respuesta maaxima
        //                    {
        //                        objCptoOpinion.DescValorMaximo = CptoOpinionXIdCpto.DescripcionRespuesta;
        //                        objCptoOpinion.ValorMaximo = CptoOpinionXIdCpto.ValorPonderacionRespuesta;
        //                    }

        //                }//if de count de lista, de n respuestas
        //                else //solo existe 1 cpto con 1 respuesta, remapearlo y agregarlo a lista
        //                {
        //                    objCptoOpinion = new ConceptoOpinion
        //                       {
        //                           IdConceptoRespValor = CptoOpinionXIdCpto.IdConceptoRespValor,
        //                           NumOrden = CptoOpinionXIdCpto.NumOrden,
        //                           IdTema = CptoOpinionXIdCpto.IdTema,
        //                           DescripcionTema = CptoOpinionXIdCpto.DescripcionTema,
        //                           IdConcepto = CptoOpinionXIdCpto.IdConcepto,
        //                           DescripcionConcepto = CptoOpinionXIdCpto.DescripcionConcepto,
        //                           FundamentoLegal = CptoOpinionXIdCpto.FundamentoLegal,
        //                           EsDeterminante = CptoOpinionXIdCpto.EsDeterminante,
        //                           IdRespuesta = CptoOpinionXIdCpto.IdRespuesta //solo para cuando no hay agurpacion se guarda el Id porque puede tratarse de un Concepto de Tema
        //                           //no poblar propiedades de valor max y min
        //                       };

        //                    //agregar a la lista
        //                    ListNuevosCptosOpinionAgrupados.Add(objCptoOpinion);
        //                }

        //            }//for each
        //        } //foreach
        //    }//if
        //    return ListNuevosCptosOpinionAgrupados;
        //}


        /// <summary>
        /// Insert en la BD las respuestas a la emisión de opinión de nuevo arrendamientos, las respuestsa se pasan en un dataTable como un arreglo de parametros.
        /// </summary>
        /// <param name="IdTipoArrendamiento"></param>
        /// <param name="IdInstitucionUsr"></param>
        /// <param name="IdUsuarioRegistro"></param>
        /// <param name="DataTableRespuestaCptoList"></param>
        /// <param name="strConnectionString"></param>
        /// <returns></returns>
        public int InsertEmisionOpinion(string DescTipoArrendamiento, int IdInstitucionUsr, string CargoUsuarioRegistro, int IdUsuarioRegistro, string Tema, string CadenaOriginal, string SelloDigital, System.Data.DataTable DataTableRespuestaCptoList, String strConnectionString, int IdInmuebleArrendamiento, bool? EsContratoHistorico = null, int? FolioContrato = null, int? FolioSMOI = null, string Justificacion = null, string FolioDisponibilidad = null, string FechaDictamen = null)
        {
            //buscar el IdCargo en la lista de Cargos, porque del SSO solo viene el nombre del Cargo
            // int IdCargoUsr = AdministradorCatalogos.ObtenerIdCargo(CargoUsr);
            int FolioEmisionOpinion = 0;

            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            FolioEmisionOpinion = Conn.InsertEmisionOpinionADO(DescTipoArrendamiento, IdInstitucionUsr, CargoUsuarioRegistro, IdUsuarioRegistro, Tema, CadenaOriginal, SelloDigital, DataTableRespuestaCptoList, strConnectionString, IdInmuebleArrendamiento, EsContratoHistorico, FolioContrato, FolioSMOI,Justificacion, FolioDisponibilidad, FechaDictamen);

            return FolioEmisionOpinion;
        }

        public int Insert_SMOI(int IdInstitucionUsr, string CargoUsuarioRegistro, int IdUsuarioRegistro, string CadenaOriginal, string SelloDigital, System.Data.DataTable DataTableRespuestaCptoList, String strConnectionString,string QR)
        {
            int FolioSMOI = 0;

            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            FolioSMOI = Conn.InsertSMOI_ADO(IdInstitucionUsr, CargoUsuarioRegistro, IdUsuarioRegistro, CadenaOriginal, SelloDigital, DataTableRespuestaCptoList, strConnectionString,QR);
            return FolioSMOI;
        }


        public AcuseFolio ObtenerAcuseSolicitudOpinionConInmueble(int IdFolioAplicacionCpto, string TipoArrendamiento) //TipoArrendamiento=Nuevo, Continuación, Sustitución
        {
            AcuseFolio objOpinionFolio;

            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();

            objOpinionFolio = Conn.ObtenerAcuseSolicitudOpinionConInmueble(IdFolioAplicacionCpto, TipoArrendamiento);

            if (objOpinionFolio != null)
            {
                //obtener el nombre de la institucion, porque se obtuvo el Id de la BD
                objOpinionFolio.InstitucionSolicitante = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(objOpinionFolio.IdInstitucionSolicitante);

            }

            return objOpinionFolio;
        }

        public AcuseFolio ObtenerAcuseSMOI(int IdFolioAplicacionCpto)
        {
            AcuseFolio objOpinionFolio;

            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            
            objOpinionFolio = Conn.ObtenerAcuseSMOI(IdFolioAplicacionCpto);

            if (objOpinionFolio != null)
            {
                //obtener el nombre de la institucion, porque se obtuvo el Id de la BD
                objOpinionFolio.InstitucionSolicitante = Negocio
                    .AdministradorCatalogos
                    .ObtenerNombreInstitucion(objOpinionFolio.IdInstitucionSolicitante);

            }

            return objOpinionFolio;
        }

        public ConceptoRespValor ObtenerFundamentoLegalCpto(byte IdTema, decimal NumOrden)
        {
            ConceptoRespValor objConceptoRespValor;
            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            objConceptoRespValor = Conn.ObtenerFundamentoLegalCpto(IdTema, NumOrden);

            return objConceptoRespValor;
        }

        public List<ModeloNegocios.AplicacionConcepto> ObtenerSolicitudesEmisionOpinionEmitidas(int? IdInstitucion, int FolioAplicacionConcepto, byte? IdTema,int? FolioSAEF)
        {
            List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_Opinion;
            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            ListAplicacionConcepto_Opinion = Conn.ObtenerSolicitudesEmisionOpinionEmitidas(IdInstitucion, FolioAplicacionConcepto, IdTema,FolioSAEF);


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

        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }

        public List<ModeloNegocios.AplicacionConcepto> ObtenerSolicitudesSMOIEmitidas(int? IdInstitucion, int FolioAplicacionConcepto)
        {
            List<ModeloNegocios.AplicacionConcepto> ListAplicacionConcepto_SMOI;
            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            ListAplicacionConcepto_SMOI = Conn.ObtenerSolicitudesSMOIEmitidas(IdInstitucion, FolioAplicacionConcepto);


            //recorrer la lista de objetos y obtener sus correspondientes valores de catalo: llave-valor
            foreach (ModeloNegocios.AplicacionConcepto ObjList in ListAplicacionConcepto_SMOI)
            {

                //obtener nombres de IdCatalogo, porque se obtuvo el Id de la BD, y se requiere exponer la descripcion al usuario
                ObjList.NombreInstitucion = Negocio.AdministradorCatalogos.ObtenerNombreInstitucion(ObjList.IdInstitucion);
                //ObjList.NombreCargo = Negocio.AdministradorCatalogos.ObtenerNombreCargo(ObjList.IdCargo);
                ObjList.NombreUsuario = AdministradorCatalogos.ObtenerNombreUsuarioSSO(ObjList.IdUsuarioRegistro);
            }
            return ListAplicacionConcepto_SMOI;
        }


        public List<ModeloNegocios.ConceptoRespValor> ObtenerCptosRespuestaValorSMOI(byte IdTema, int IdInstitucion)
        {
            List<ModeloNegocios.ConceptoRespValor> ListConceptosSMOI;
            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            ListConceptosSMOI = Conn.ObtenerCptosRespuestaValorSMOI(IdTema, IdInstitucion);

            return ListConceptosSMOI;
        }

        public int ObtenerConteoSolicitudesSMOIxIdInstitucion(int IdInstitucion)
        {

            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            return Conn.ObtenerConteoSolicitudesSMOIxIdInstitucion(IdInstitucion);
        }


        //SMOI
        public decimal ObtenerSupTotalM2SMOI(int IdFolioSMOI, int IdInstitucion)
        {
            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            return Conn.ObtenerSupTotalM2SMOI(IdFolioSMOI, IdInstitucion);

        }

        //SMOI, count de registros relaciondos de SMOI con Emisiones de Opinion
        public int ObtenerCountUsoFolioSMOIenOpionion(int IdFolioSMOI, int IdInstitucion)
        {
            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            return Conn.ObtenerCountUsoFolioSMOIenOpionion(IdFolioSMOI, IdInstitucion);

        }


        public decimal ObtenerSupTotalM2SMOIsinOcupar(int IdFolioSMOI, int IdInstitucion)
        {
            AccesoDatos.SmoiDAL Conn = new AccesoDatos.SmoiDAL();
            return Conn.ObtenerSupTotalM2SMOIsinOcupar(IdFolioSMOI, IdInstitucion);

        }
        //emisión de opinión
        public INDAABIN.DI.CONTRATOS.ModeloNegocios.AplicacionConcepto ObtenerEmisionOpinionPorFolio(int FolioOpinion, int IdInstitucion)
        {
            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            return Conn.ObtenerEmisionOpinionPorFolio(FolioOpinion, IdInstitucion);

        }

        //RCA 10/08/2018
        //metodo para obtener el id smoi
        public int ObtenerIdSMOI(int FolioSMOI,int? Tipo)
        {
            AccesoDatos.EmisionOpinionDAL conn = new AccesoDatos.EmisionOpinionDAL();

            int Id = 0;
            Id = conn.ObtenerIdAplicacionSMOI(FolioSMOI,Tipo);
            return Id;
        }

        //metodo para actualizar el codigo qr 
        public Boolean ActualizarQRSMOI(string QR, int IdapSMOI)
        {
            AccesoDatos.EmisionOpinionDAL conn = new AccesoDatos.EmisionOpinionDAL();

            bool ok = false;
            ok = conn.ActualizarSMOIQR(QR,IdapSMOI);
            return ok;
        }

        //RCA 21/11/2018
        //metodo para obtener el tema
        public int ObtenerIdTema(int FolioOpinion)
        {
            AccesoDatos.EmisionOpinionDAL Conn = new AccesoDatos.EmisionOpinionDAL();
            return Conn.ObtenerIdTemaEmision(FolioOpinion);
        }

        public List<ConceptoRespValor> ObtenerTablaSMOIFolio(int folio)
        {
            List<ConceptoRespValor> Lconcepto = new List<ConceptoRespValor>();

            try
            {
                SmoiDAL sDal = new SmoiDAL();
                Lconcepto = sDal.ObtenerTablaSMOIFolio(folio);
            }

            catch (Exception) { }

            return Lconcepto;
        }

    }//clase
}
