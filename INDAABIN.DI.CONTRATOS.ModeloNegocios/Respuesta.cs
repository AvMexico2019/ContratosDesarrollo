using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class Respuesta
    {
        public bool respuesta { get; set; }
        public string Mensaje { get; set; }
        public JustripreciacionContrato Justipreciacion { get; set; }
        public string Url { get; set; }
        public List<Convenio> Lconvenio { get; set; }

        public Respuesta()
        {
            respuesta = false;
            Mensaje = string.Empty;
            Justipreciacion = new JustripreciacionContrato();
            Url = string.Empty;
            Lconvenio = new List<Convenio>();
        }
    }
}
