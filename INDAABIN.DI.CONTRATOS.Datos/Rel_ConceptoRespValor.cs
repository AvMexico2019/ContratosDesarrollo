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
    
    public partial class Rel_ConceptoRespValor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rel_ConceptoRespValor()
        {
            this.RespuestaConcepto = new HashSet<RespuestaConcepto>();
        }
    
        public int IdConceptoRespValor { get; set; }
        public Nullable<int> Fk_IdInstitucion { get; set; }
        public byte Fk_IdTema { get; set; }
        public int Fk_IdConcepto { get; set; }
        public int Fk_IdRespuesta { get; set; }
        public decimal NumOrden { get; set; }
        public bool EsDeterminante { get; set; }
        public Nullable<decimal> ValorRespuesta { get; set; }
        public decimal ValorMinimo { get; set; }
        public decimal ValorMaximo { get; set; }
        public string Comentario { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual Cat_Tema Cat_Tema { get; set; }
        public virtual Concepto Concepto { get; set; }
        public virtual Respuesta Respuesta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RespuestaConcepto> RespuestaConcepto { get; set; }
    }
}