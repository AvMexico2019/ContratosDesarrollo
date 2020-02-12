using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{

    /*
     Este clase se pobla la cargar el cuestionario de emisión de opinión y actualiza al guardar la informacion en la BD
     * */
    public class ValorRespuestaConcepto
    {
        public int IdConceptoRespValor { get; set; } //FK

        public decimal NumOrden { get; set; }

        public String NumOrdenVisual { get; set; }

        public decimal? ValorResp { get; set; } //esta propiedad se proporciona hasta que el usuario da las respuestas
       
    }
}
