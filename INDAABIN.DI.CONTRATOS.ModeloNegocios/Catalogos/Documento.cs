using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class Documento
    {
        public int IdDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string DescripcionDocumento { get; set; }
        public string URLDocumento { get; set; }
        public string RutaDocumento { get; set; }
        public bool EstatusRegistro { get; set; }
    }
}
