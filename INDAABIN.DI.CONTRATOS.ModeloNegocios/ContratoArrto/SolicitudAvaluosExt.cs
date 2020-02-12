using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class SolicitudAvaluosExt
    {
        public string Calle { get; set; }
        public string Cargo { get; set; }
        public string Ciudad { get; set; }
        public string CP { get; set; }
        public string Email { get; set; }
        public string EstadoDescripcion { get; set; }
        public int? EstadoId { get; set; }
        public string Estatus { get; set; }
       //se cambio de string a datetime la fecha de dictamen
        public DateTime FechaDictamen { get; set; }
        public string InstitucionDescripcion { get; set; }
        public int InstitucionId { get; set; }
        public decimal MontoDictaminado { get; set; }
        public string MunicipioDescripcion { get; set; }
        public int? MunicipioId { get; set; }
        public string NoExterior { get; set; }
        public string NoGenerico { get; set; }
        public string NoInterior { get; set; }
        public string NoSecuencial { get; set; }
        public string Responsable { get; set; }
        public string SectorDescripcion { get; set; }
        public int SectorId { get; set; }
        public decimal SuperficieConstruida { get; set; }
        public decimal SuperficieConstruidaDictaminado { get; set; }
        public decimal SuperficieRentable { get; set; }
        public decimal SuperficieRentableDictaminado { get; set; }
        public decimal SuperficieTerreno { get; set; }
        public decimal SuperficieTerrenoDictaminado { get; set; }
        public string UnidadMedidaConstruida { get; set; }
        public string UnidadMedidaConstruidaDictaminado { get; set; }
        public string UnidadMedidaRentable { get; set; }
        public string UnidadMedidaRentableDictaminado { get; set; }
        public string UnidadMedidaTerreno { get; set; }
        public string UnidadMedidaTerrenoDictaminado { get; set; }
        public string UnidadResponsable { get; set; }
        public string RutaDocumento { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public string ColoniaInmueble { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
