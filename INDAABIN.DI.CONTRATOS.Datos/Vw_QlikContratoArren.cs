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
    
    public partial class Vw_QlikContratoArren
    {
        public string RIUF { get; set; }
        public int FolioContratoArrto { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public int Fk_IdInmuebleArrendamiento { get; set; }
        public int Institucion { get; set; }
        public string NombreInstitucion { get; set; }
        public Nullable<int> Fk_IdPais { get; set; }
        public string DescripcionPais { get; set; }
        public Nullable<int> Fk_IdMunicipio { get; set; }
        public string DescripcionMunicipio { get; set; }
        public Nullable<int> Fk_IdEstado { get; set; }
        public string DescripcionEstado { get; set; }
        public string CodigoPostal { get; set; }
        public Nullable<int> Fk_IdTipoVialidad { get; set; }
        public string NombreVialidad { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public decimal AreaOcupadaM2 { get; set; }
        public decimal MontoPagoMensual { get; set; }
        public Nullable<int> Fk_IdTipoInmueble { get; set; }
        public Nullable<int> Fk_IdTipoUsoInm { get; set; }
        public string DescripcionUsoGenerico { get; set; }
        public string usoespecifico { get; set; }
        public string NombreInmueble { get; set; }
        public Nullable<decimal> GeoRefLatitud { get; set; }
        public Nullable<decimal> GeoRefLongitud { get; set; }
        public Nullable<decimal> MontoDictaminado { get; set; }
        public string SuperficieDictaminada { get; set; }
        public string UnidadMedidaSupRentableDictaminada { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int Fk_IdInstitucion { get; set; }
        public string Colonia { get; set; }
    }
}
