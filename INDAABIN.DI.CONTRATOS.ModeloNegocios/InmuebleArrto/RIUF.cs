using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class RIUF
    {
        public Nullable<int> IdRIUF { get; set; }
        public string RIUF1 { get; set; }
        public Nullable<int> IdEstado { get; set; }
        public Nullable<int> IdEstadoRIUF { get; set; }
        public Nullable<int> Consecutivo { get; set; }
        public Nullable<int> Digitoverificador { get; set; }
        public bool EstatusRegistro { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }


    }
}
