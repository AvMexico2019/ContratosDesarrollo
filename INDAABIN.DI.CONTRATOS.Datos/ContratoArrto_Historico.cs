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
    
    public partial class ContratoArrto_Historico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContratoArrto_Historico()
        {
            this.ContratoArrto = new HashSet<ContratoArrto>();
        }
    
        public int NumContratoHistorico { get; set; }
        public string InstitucionPromovente { get; set; }
        public Nullable<int> ClaveInstitucion { get; set; }
        public string Estado { get; set; }
        public Nullable<short> IdMpo { get; set; }
        public string Municipio { get; set; }
        public string Secuencial { get; set; }
        public string Propietario { get; set; }
        public string FechaDictamen { get; set; }
        public string Responsable { get; set; }
        public string Sector { get; set; }
        public string Delegacion { get; set; }
        public string Ciudad { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string CP { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public Nullable<decimal> MontoDictaminado { get; set; }
        public string FechaInicioContrato { get; set; }
        public string FechaFinContrato { get; set; }
        public Nullable<short> CajonesEstacionamiento { get; set; }
        public string TipoContratacion { get; set; }
        public Nullable<decimal> MontoRentaMensual { get; set; }
        public Nullable<decimal> AreaRentable { get; set; }
        public string UsoInmueble { get; set; }
        public string OtroUsoInmueble { get; set; }
        public Nullable<decimal> TablaSmoi { get; set; }
        public Nullable<decimal> CuotaManteinimiento { get; set; }
        public string FechaContrato { get; set; }
    
        public virtual Cat_InstitucionContratosHistorico Cat_InstitucionContratosHistorico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContratoArrto> ContratoArrto { get; set; }
    }
}
