//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace INDAABIN.DI.CONTRATOS.Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class SelloDigital
    {
        public short Fk_IdCatTabla { get; set; }
        public int Fk_IdRegistroTablaOrigen { get; set; }
        public string CadenaOriginal { get; set; }
        public string SelloDigital1 { get; set; }
        public string GUID { get; set; }
        public bool EstatusRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public string QR { get; set; }
    
        public virtual Cat_Tabla Cat_Tabla { get; set; }
    }
}
