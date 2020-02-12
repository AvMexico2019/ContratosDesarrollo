using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Transactions;
//comunicacion con las capas
using INDAABIN.DI.CONTRATOS.Datos; //EntityFramework
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //Entities


namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class EmisionOpinion
    {


        //obtener los cptos de emision de opicion
        //parametros de entrada, IdTema: 
        //2=Opinión Nuevo Arrendamiento
        //3=Opinion de Sustitucion Arrto
        //4=Opinion de Continuacion Arro
        public List<ConceptoRespValor> ObtenerCptosRespuestaValor(byte IdTema)
        {
            List<ConceptoRespValor> listaConceptosOpinion = new List<ConceptoRespValor>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaConceptosOpinion = Conn.spuSelectConceptosXResponderTema(IdTema)
                        .Select(CptoOpinionBD => new ConceptoRespValor
                        {
                            //Re- mapear propiedades del objeto del entity framework al objeto de negocio
                            IdConceptoRespValor = CptoOpinionBD.IdConceptoRespValor,
                            NumOrden = CptoOpinionBD.NumOrden,
                            IdTema = CptoOpinionBD.Fk_IdTema, //fk
                            DescripcionTema = CptoOpinionBD.DescripcionTema,
                            IdConcepto = CptoOpinionBD.Fk_IdConcepto, //fk
                            DescripcionConcepto = CptoOpinionBD.DescripcionConcepto,
                            FundamentoLegal = CptoOpinionBD.FundamentoLegal,
                            EsDeterminante = CptoOpinionBD.EsDeterminante,
                            DescripcionRespuesta = CptoOpinionBD.DescripcionRespuesta,
                            IdRespuesta = CptoOpinionBD.Fk_IdRespuesta, //fk
                            ValorPonderacionRespuesta = CptoOpinionBD.ValorPonderacionRespuesta
                         

                        }).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listaConceptosOpinion;

        }


    }
}
