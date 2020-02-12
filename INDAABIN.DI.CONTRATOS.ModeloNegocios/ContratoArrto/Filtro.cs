using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class Filtro
    {
        public string NoSecuencial { get; set; }

        public string NoGenerico { get; set; }

        public DateTime? FechaDictamen { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
