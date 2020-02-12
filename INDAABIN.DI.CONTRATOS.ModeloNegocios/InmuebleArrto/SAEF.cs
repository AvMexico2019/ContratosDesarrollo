using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class SAEF
    {
        public int? FolioSAEF { get; set; }

        public DateTime FechaRegistroSAEF { get; set; }
        public int? FolioEmisionOpinion { get; set; }
        public string DescripcionInstitucion { get; set; }
        public InmuebleArrto InmuebleArrto { get; set; }

        public string Promovente { get; set; }
        public string Cargo { get; set; }
        public int? FolioEmisionVsSAEF { get; set; }
        public int? PorcentajeAccesibiliadad { get; set; }


    }
}
