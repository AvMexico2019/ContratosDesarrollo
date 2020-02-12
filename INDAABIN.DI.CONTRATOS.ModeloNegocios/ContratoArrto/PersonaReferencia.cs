using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class PersonaReferencia
    {
        public int IdPersonaReferencia { get; set; }
        public int Fk_IdContratoArrendamiento { get; set; }
        public byte Fk_IdTipoPersonaRef { get; set; }
        public string NombreCargo { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Email { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }

    }
}
