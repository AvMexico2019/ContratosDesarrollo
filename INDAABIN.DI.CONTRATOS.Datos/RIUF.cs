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
    
    public partial class RIUF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RIUF()
        {
            this.Inmueble = new HashSet<Inmueble>();
        }
    
        public int IdRIUF { get; set; }
        public string RIUF1 { get; set; }
        public Nullable<int> Fk_IdEstado { get; set; }
        public Nullable<int> Fk_IdEstadoRIUF { get; set; }
        public int Consecutivo { get; set; }
        public int DigitoVerificador { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inmueble> Inmueble { get; set; }
    }
}
