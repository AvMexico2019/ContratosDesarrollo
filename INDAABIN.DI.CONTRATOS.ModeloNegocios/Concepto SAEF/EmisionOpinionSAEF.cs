using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class EmisionOpinionSAEF
    {
        public int IdAplicacionConcepto { get; set; }
        public int FolioEmisionOpinion { get; set; }
        public int IdUsuarioEmisionOpinion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NombreUsuarioEmisionOpinion { get; set; }
    }
}
