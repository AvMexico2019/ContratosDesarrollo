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
    
    public partial class spuSelectInmuebleArrtoXId_Result
    {
        public int Fk_IdPais { get; set; }
        public Nullable<int> Fk_IdEstado { get; set; }
        public Nullable<int> Fk_IdMunicipio { get; set; }
        public Nullable<int> Fk_IdLocalidad { get; set; }
        public string OtraColonia { get; set; }
        public int Fk_IdTipoVialidad { get; set; }
        public string NombreVialidad { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public string CodigoPostal { get; set; }
        public string CodigoPostalExtranjero { get; set; }
        public string EstadoExtranjero { get; set; }
        public string CiudadExtranjero { get; set; }
        public string MunicipioExtranjero { get; set; }
        public Nullable<decimal> GeoRefLatitud { get; set; }
        public Nullable<decimal> GeoRefLongitud { get; set; }
        public string NombreInmueble { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public Nullable<int> IdInmueble { get; set; }
        public string RIUFInmueble { get; set; }
    }
}