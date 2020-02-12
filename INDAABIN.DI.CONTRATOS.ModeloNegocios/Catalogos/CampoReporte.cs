using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class CampoReporte
    {
        public int IdCampoReporte { get; set; }
        public int IdReporte { get; set; }
        public int OrdenCampoReporte { get; set; }
        public string NombreCampoReporte { get; set; }
        public string DescripcionCampoReporte { get; set; }
        public bool EstatusRegistro { get; set; }
    }
}
