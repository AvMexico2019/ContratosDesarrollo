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
    
    public partial class Cat_Prioridad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cat_Prioridad()
        {
            this.AreaPrioridad = new HashSet<AreaPrioridad>();
        }
    
        public int IdPrioridad { get; set; }
        public string DescripcionPrioridad { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AreaPrioridad> AreaPrioridad { get; set; }
    }
}
