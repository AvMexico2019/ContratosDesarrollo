using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class AcuseContrato
    {

        public int Folio { get; set; }
        public string FechaRegistro { get; set; }
        public string HoraRegistro { get; set; }
        public string TipoArrendamientoDesc { get; set; }
        public string ResultadoAplicacionOpinion { get; set; }
        public int IdUsuarioRegistro { get; set; }
 
        public string InstitucionSolicitante { get; set; }
        public string CargoUsrRegistro { get; set; }
        public string CadenaOriginal { get; set; }
        public string SelloDigital { get; set; }

        public string LeyendaAnio { get; set; }

        //referencia a objeto de negocio
        public ContratoArrto ContratoArrto { get; set; }
        public InmuebleArrto InmuebleArrto { get; set; }

        public JustripreciacionContrato JustripreciacionContrato { get; set; }

        //RCA 13/08/2018
        public string QR { get; set; }
        public string Leyenda { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public string FolioSAEF { get; set; }

        public AcuseContrato()
        {
            FolioSAEF = string.Empty;
        }

    }
}
