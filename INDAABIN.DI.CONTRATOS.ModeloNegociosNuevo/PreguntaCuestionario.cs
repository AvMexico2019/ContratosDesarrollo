using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegociosNuevo
{
    public class PreguntaCuestionario
    {
        public int IdPregunta;
        public decimal Orden;
        public byte Fk_IdTema;
        public int Fk_IdConcepto;
        public int Fk_IdDataType;
        public bool EsDeterminante;
        public bool EdoInicial;
        public string ReglaNegocio;
        public DateTime FechaRegistro;
        public string DescripcionConcepto;
        public string FundamentoLegal;
        public string DescripcionDataType;
    }
}
