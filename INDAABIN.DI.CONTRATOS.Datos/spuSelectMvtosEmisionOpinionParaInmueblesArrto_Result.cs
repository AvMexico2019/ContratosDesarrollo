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
    
    public partial class spuSelectMvtosEmisionOpinionParaInmueblesArrto_Result
    {
        public int IdInmuebleArrendamiento { get; set; }
        public int Fk_IdInstitucion { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public string FechaRegistro { get; set; }
        public string NombreInmueble { get; set; }
        public int Fk_IdPais { get; set; }
        public Nullable<int> Fk_IdEstado { get; set; }
        public Nullable<int> Fk_IdMunicipio { get; set; }
        public Nullable<int> Fk_IdLocalidad { get; set; }
        public int Fk_IdTipoVialidad { get; set; }
        public string NombreVialidad { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public string CodigoPostal { get; set; }
        public string CodigoPostalExtranjero { get; set; }
        public string EstadoExtranjero { get; set; }
        public string CiudadExtranjero { get; set; }
        public string MunicipioExtranjero { get; set; }
        public Nullable<int> IdAplicacionConcepto { get; set; }
        public Nullable<int> FolioAplicacionConcepto { get; set; }
        public string FolioSAEF { get; set; }
        public string TemaAplicacionConcepto { get; set; }
        public Nullable<int> FolioSMOI { get; set; }
        public Nullable<int> FolioContratoAlQueAlplicaOpinion { get; set; }
        public int IsNotReusable { get; set; }
        public string OtraColonia { get; set; }
    }
}
