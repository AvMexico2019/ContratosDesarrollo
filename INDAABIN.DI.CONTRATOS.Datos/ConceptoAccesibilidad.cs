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
    
    public partial class ConceptoAccesibilidad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConceptoAccesibilidad()
        {
            this.Movimiento = new HashSet<Movimiento>();
        }
    
        public int IdConcAccesibilidad { get; set; }
        public Nullable<int> Fk_IdIndicador { get; set; }
        public Nullable<int> Fk_IdAreaPrioridad { get; set; }
        public string DescConcAccesibilidad { get; set; }
        public Nullable<int> Cumplimiento { get; set; }
        public int EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual AreaPrioridad AreaPrioridad { get; set; }
        public virtual Cat_Indicador Cat_Indicador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movimiento> Movimiento { get; set; }
    }
}
