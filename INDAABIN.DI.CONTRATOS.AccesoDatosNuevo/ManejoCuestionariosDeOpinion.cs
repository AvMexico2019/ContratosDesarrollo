using System;
using System.Collections.Generic;
using System.Linq;
using SandBoxDB;
using INDAABIN.DI.CONTRATOS.ModeloNegociosNuevo;

namespace INDAABIN.DI.CONTRATOS.AccesoDatosNuevo
{
    public static class ManejoCuestionariosDeOpinion
    {
        /// <summary>
        /// Proposito: Obtener el cuestionario que corresponde al tema
        /// IdTema: 
        ///         2=Opinión Nuevo Arrendamiento
        ///         3=Opinion de Sustitucion Arrto
        ///         4=Opinion de Continuacion Arro
        ///  Fecha de Creacion: 10/feb/2020   
        /// </summary>
        /// <param name="IdTema"></param>
        /// <returns></returns>
        public static List<PreguntaCuestionario> ObtenerCuestionario(byte IdTema)
        // TODO: Existe una institución que se trata distinto la IdInstitucion = 2, esto sería mejor que se manejar con permisos de acceso de la manera con fue hecho se puede obtener la información que se quiere manejar de forma distinta no parecerazonable hacerlo como se está haciendo.
        {
            List<PreguntaCuestionario> Cuestionario;

            using (SandBoxEntities ctx = new SandBoxEntities())
            {
                try
                {
                    Cuestionario = (from pregunta in ctx.Cuestionarios
                                    where pregunta.Fk_IdTema == IdTema
                                    join tema in ctx.Cat_Tema
                                    on pregunta.Fk_IdTema equals tema.IdTema
                                    join cncepto in ctx.Concepto
                                    on pregunta.Fk_IdConcepto equals cncepto.IdConcepto
                                    join dT in ctx.Cat_DataTypes
                                    on pregunta.Fk_IdDataType equals dT.IdDataType
                                    orderby pregunta.Orden
                                    select (new PreguntaCuestionario {
                                        IdPregunta = pregunta.IdPregunta,
                                        Orden = pregunta.Orden,
                                        Fk_IdTema = pregunta.Fk_IdTema,
                                        Fk_IdConcepto = pregunta.Fk_IdConcepto,
                                        Fk_IdDataType = pregunta.Fk_IdDataType,
                                        EsDeterminante = pregunta.EsDeterminante,
                                        EdoInicial = pregunta.EdoInicial,
                                        ReglaNegocio = pregunta.ReglaNegocio,
                                        FechaRegistro = pregunta.FechaRegistro,
                                        DescripcionConcepto = cncepto.DescripcionConcepto,
                                        FundamentoLegal = cncepto.FundamentoLegal,
                                        DescripcionDataType = dT.Descripcion
                                    })).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("ObtenerCuestionario: {0}", ex.Message));
                }
            }//using
            return Cuestionario;
        }
    }
}
