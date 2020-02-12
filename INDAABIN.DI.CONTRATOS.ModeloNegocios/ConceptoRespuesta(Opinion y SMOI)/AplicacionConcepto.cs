using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class AplicacionConcepto
    {
        public int? IdAplicacionConcepto { get; set; }
        public int? FolioAplicacionConcepto { get; set; }
        public string TemaAplicacionConcepto { get; set; } //Opinion de Nuevo Arrenadameinto, Opinion de Sustitucion Arrto, Opinion de Continuacion Arrto, SMOI 

        public string TipoArrendamiento { get; set; }

        public int IdInstitucion { get; set; }//del promovente que registro la Solicitud de Opinion
        public string NombreInstitucion { get; set; } //del promovente que registro la Solicitud de Opinion
               
        public int IdCargo { get; set; }//del promovente que registro la Solicitud de Opinion
        public string NombreCargo { get; set; }//del promovente que registro la Solicitud de Opinion

        public string DescAplicacionCpto { get; set; } //emisión de opinión ó Tabla SMOI

      
       
        //propiedades para una aplicacion de tipo SMOI
        public decimal SupM2XArrendar { get; set; }
        public decimal? SupM2XSMOI { get; set; }

        public int IdUsuarioRegistro { get; set; }
        public int IsNotReusable { get; set; }
        public string NombreUsuario { get; set; }
        
        public DateTime FechaRegistro { get; set; }
        public string Observaciones { get; set; }
        
        //FK de Contrato con el que se asocia el objeto
        public int? FolioContratoArrto_FK { get; set; } //nulo porque puede ser que existen solicitudes de opinion que no se asocian a un contratoArrto.
        
        //un inmueble, al que se aplica una emisión de opinión
        public InmuebleArrto InmuebleArrto { get; set; }

        public string ResultadoEmisionOpinion { get; set; }

        public int? FolioSMOI_AplicadoOpinion { get; set; }

        //para conocer para que Folio SMOI se han usado en una emission de Opinion
        public int? FolioEmisionOpinion_DondeSeAplicoFolioSMOI { get; set; }

        public int? IdInmuebleArrendamiento { get; set; }

        //RCA 08/08/2018
        public string FolioSAEF { get; set; }
        public DateTime FechaRegistroSAEF { get; set; }
        public int? PorcentajeAccesibiliadad { get; set; }

        
    }
}
