using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class Convenio
    {

        public int IdConvenio { get; set; }
        public int FolioContrato { get; set; }
        public int ConsecutivoConvenio { get; set; }
        public string FolioConvenio { get; set; }
        public DateTime FechaConvenio { get; set; }
        public string descFechaConvenio { get; set; }
        public int TieneProrroga { get; set; }
        public DateTime? FechaTermino { get; set; }
        public string descFechaTermino { get; set; }
        public int TieneNvaSuperfice { get; set; }
        public decimal? SupM2 { get; set; }
        public int TieneNvoMonto { get; set; }
        public decimal? ImporteRenta { get; set; }
        public DateTime? FechaInicioImporte { get; set; }
        public string descFechaInicioImporte { get; set; }
        public string Secuencial { get; set; }
        public string NombreOIC { get; set; }
        public string PApellidoOIC { get; set; }
        public string SApellidoOIC { get; set; }
        public string CargoOIC { get; set; }
        public string CorreoOIC { get; set; }
        public string cadOriginal { get; set; }
        public string Sello { get; set; }
        public string QR { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string descFechaAutorizacion { get; set; }
        public string descFechaRegistro { get; set; }
        public int IdInmueble { get; set; }
        public DateTime FechaEfectoConvenio { get; set; }
        public string DescFechaEfectoConvenio { get; set; }
        public Convenio()
        {
            IdConvenio = 0;
            IdInmueble = 0;
            FolioContrato = 0;
            ConsecutivoConvenio = 0;
            FolioConvenio = string.Empty;
            descFechaAutorizacion = string.Empty;
            descFechaRegistro = string.Empty;
            FechaConvenio = new DateTime();
            descFechaConvenio = string.Empty;
            TieneProrroga = 0;
            FechaTermino = new DateTime();
            descFechaTermino = string.Empty;
            TieneNvaSuperfice = 0;
            SupM2 = 0;
            TieneNvoMonto = 0;
            ImporteRenta = 0;
            FechaInicioImporte = new DateTime();
            descFechaInicioImporte = string.Empty;
            Secuencial = string.Empty;
            NombreOIC = string.Empty;
            PApellidoOIC = string.Empty;
            SApellidoOIC = string.Empty;
            CargoOIC = string.Empty;
            CorreoOIC = string.Empty;
            cadOriginal = string.Empty;
            Sello = string.Empty;
            QR = string.Empty;
            FechaRegistro = new DateTime();
            FechaEfectoConvenio = new DateTime();
            DescFechaEfectoConvenio = string.Empty;
        }
    }
}
