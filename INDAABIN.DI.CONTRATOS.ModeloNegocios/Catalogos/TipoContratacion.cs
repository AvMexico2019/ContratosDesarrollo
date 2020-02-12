using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{

    /// <summary>
    /// Automático 
        //Dictaminado
        //Excepción Artículo 5 (Acuerdo de montos)
        //Excepción 178 (MRMSG)
    /// </summary>
    public class TipoContratacion
    {

        public byte IdTipoContratacion { get; set; }
        public string DescripcionTipoContratacion { get; set; }
        public int Orden { get; set; }
    }
}
