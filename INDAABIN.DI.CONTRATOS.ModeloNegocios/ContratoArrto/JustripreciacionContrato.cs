using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class JustripreciacionContrato
    {
        public int IdJustipreciacion { get; set; }
        public Nullable<int> Fk_IdContratoArrto { get; set; }
        public string Secuencial { get; set; }
        public string SuperficieDictaminada { get; set; }
        public Nullable<System.DateTime> FechaDictamen { get; set; }
        public String strFechaDictamen { get; set; }
        public Nullable<decimal> MontoDictaminado { get; set; }
        public string EstatusAtencion { get; set; }
        public string NoGenerico { get; set; }
        public string UnidadMedidaSupRentableDictaminada { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string InstitucionJustipreciacion { get; set; } 
        public string descFechaDictamen { get; set; }

    }
}
