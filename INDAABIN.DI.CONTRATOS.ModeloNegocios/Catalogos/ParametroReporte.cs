using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class ParametroReporte
    {

        public Nullable<int> IdInstitucion { get; set; }
        public Nullable<int> IdTipoContrato { get; set; }
        public Nullable<int> IdTipoOcupacion{ get; set; }
        public List<string> ListaCampos { get; set; }
        public string RangoFechaRegistroInicial { get; set; }
        public string RangoFechaRegistroFinal { get; set; }
        public string RangoFechaInicioOcupacionInicial { get; set; }
        public string RangoFechaInicioOcupacionFinal { get; set; }
        public string RangoFechaTerminoOcupacionInicial { get; set; }
        public string RangoFechaTerminoOcupacionFinal { get; set; }

    }
}
