using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class ValorRespuestaSAEF
    {
        public int? IdAlicacionConcepto { get; set; }
        public int? ConceptoAccesibilidad { get; set; }
        public bool? Aplica { get; set; }
        public bool? Existe { get; set; }
        public int? Cantidad { get; set; }
        public bool? SeRequiere { get; set; }
        public bool? Cumple { get; set; }
        public string Observaciones { get; set; }
        public int? IdUsuario { get; set; }

        //constructor para inicializar todos los valores
        public ValorRespuestaSAEF()
        {
            IdAlicacionConcepto = null;
            ConceptoAccesibilidad = null;
            Aplica = null;
            Existe = null;
            Cantidad = null;
            SeRequiere = null;
            Cumple = null;
            Observaciones = string.Empty;
            IdUsuario = null;
        }



    }
}
