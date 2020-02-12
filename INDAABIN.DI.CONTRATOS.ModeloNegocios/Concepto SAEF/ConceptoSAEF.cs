using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class ConceptoSAEF
    {
        public int IdConcAccesibilidad { get; set; }
        public decimal NumOrden { get; set; }
        public int? Fk_IdIndicador { get; set; }
        public int? Fk_IdAreaPrioridad { get; set; }
        public string DescConcAccesibilidade { get; set; }
        public int? Cumplimiento { get; set; }

    }
}
