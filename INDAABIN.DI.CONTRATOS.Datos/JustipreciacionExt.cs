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
    
    public partial class JustipreciacionExt
    {
        public int IdJustipreciacionExt { get; set; }
        public string Secuencial { get; set; }
        public string NoGenerico { get; set; }
        public System.DateTime FechaDictamen { get; set; }
        public string UnidadResponsable { get; set; }
        public decimal TerrenoDictaminado { get; set; }
        public short Fk_IdUnidadMedidaTerrenoDict { get; set; }
        public decimal RentableDictamindo { get; set; }
        public short Fk_IdUnidadMedidaRentableDict { get; set; }
        public decimal ConstruidaDictaminado { get; set; }
        public short Fk_IdUnidadMedidaConstruidaDict { get; set; }
        public decimal MontoDictaminado { get; set; }
        public short Fk_IdSector { get; set; }
        public short Fk_IdInstitucion { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public string CodigoPostal { get; set; }
        public short Fk_IdEstado { get; set; }
        public short Fk_IdMunicipio { get; set; }
        public string RutaDocumento { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    }
}
