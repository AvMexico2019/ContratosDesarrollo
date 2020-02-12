using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios.Catalogos
{
    [Serializable]
    public class Parametro
    {
        public int IdParametro { get; set; }
        public string NombreParametro { get; set; }
        public string DescripcionParametro { get; set; }
        public string ValorParametro { get; set; }
        public string ValorAdicional { get; set; }
        public Parametro()
        {
            IdParametro = 0;
            NombreParametro = string.Empty;
            DescripcionParametro = string.Empty;
            ValorParametro = string.Empty;
            ValorAdicional = string.Empty;                
        }
    }
}
