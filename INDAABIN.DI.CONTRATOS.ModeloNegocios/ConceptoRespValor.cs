using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class ConceptoRespValor
    {
        public int IdConceptoRespValor { get; set; }
        public decimal NumOrden { get; set; }

        public int IdTema { get; set; }

        public String DescripcionTema { get; set; }

        public int IdConcepto { get; set; }

        public string DescripcionConcepto { get; set; }

        public string FundamentoLegal { get; set; }
        public bool EsDeterminante { get; set; }

        public int IdRespuesta { get; set; }
        
        public string DescripcionRespuesta { get; set; }


        public byte ?ValorPonderacionRespuesta { get; set; }

    }
}
