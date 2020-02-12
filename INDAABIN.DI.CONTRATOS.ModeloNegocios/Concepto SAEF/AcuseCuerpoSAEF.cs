using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class AcuseCuerpoSAEF
    {
        //cuerpo de SAEF
        public int IdAplicacionConcept { get; set; }
        public int FolioEmision { get; set; }
        public int FolioSAEFCuerpo { get; set; }
        public int IdInmuebleArrendamiento { get; set; }
        public int IdConceptoAccesibilidad { get; set; }
        public string Aplica { get; set; }
        public string Cumple { get; set; }
        public int? Cumplimiento { get; set; }
        public int? IdIndicador { get; set; }
        public string Observaciones { get; set; }

        public AcuseCuerpoSAEF()
        {
            Observaciones = string.Empty;
        }


    }
}
