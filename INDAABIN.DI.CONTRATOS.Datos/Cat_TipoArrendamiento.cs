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
    
    public partial class Cat_TipoArrendamiento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cat_TipoArrendamiento()
        {
            this.AplicacionConcepto = new HashSet<AplicacionConcepto>();
            this.ContratoArrto = new HashSet<ContratoArrto>();
        }
    
        public byte IdTipoArrendamiento { get; set; }
        public string DescripcionTipoArrendamiento { get; set; }
        public string Observaciones { get; set; }
        public bool EstatusRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AplicacionConcepto> AplicacionConcepto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoArrto> ContratoArrto { get; set; }
    }
}
