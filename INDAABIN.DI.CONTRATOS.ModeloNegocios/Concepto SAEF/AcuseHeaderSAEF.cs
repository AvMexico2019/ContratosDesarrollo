using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class AcuseHeaderSAEF
    {
        // el encabezado del acuse de accesibilidad
        public int IdAplicacionConcepto { get; set; }
        public int FolioAplicacionConceptoEmision { get; set; }
        public string FolioSAEF { get; set; }
        public int IdInmuebleArrendamiento { get; set; }
        public string SelloSAEF { get; set; }
        public string CadenaSAEF { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int IdInstitucion { get; set; }
        public string NombreInmueble { get; set; }
        public int? IdInmueble { get; set; }
        public string NombreInstitucion { get; set; }
        public int? IdRIUF { get; set; }
        public string RIUF { get; set; }
        public string QR { get; set; }


   


    }
}
