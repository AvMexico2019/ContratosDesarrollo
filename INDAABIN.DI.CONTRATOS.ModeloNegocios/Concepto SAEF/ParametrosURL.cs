using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class ParametrosURL
    {
        public int Incidencia { get; set; }
        public string Folio { get; set; }
        public int IdProceso { get; set; }
        public string TerminacionFolio { get; set; }
        public int IdFlujo { get; set; }

        public int IdTarea { get; set; }

        public int IdTipoDocumento { get; set; }
        public int IdDocumento { get; set; }
        public int IdUsuario { get; set; }
        public string LRol { get; set; }
        public bool EsVistoBueno { get; set; }
        public bool EsSello { get; set; }

        public string opciones { get; set; }

        public string DescTarea { get; set; }
        public Guid Token { get; set; }

        public string UserName { get; set; }

        public bool EnviarDocumentacion { get; set; }

        public bool EsDigitalizado { get; set; }

        public bool AsignaTipoDocumento { get; set; }

        public bool EsConsulta { get; set; }

        public int TipoDocumento { get; set; }
        public ParametrosURL()
        {
            Folio = string.Empty;
            LRol = string.Empty;
            opciones = string.Empty;
            UserName = string.Empty;
        }
    }
}
