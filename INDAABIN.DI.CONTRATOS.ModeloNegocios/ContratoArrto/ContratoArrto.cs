using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    public class ContratoArrto
    {
        public int IdContratoArrto { get; set; }
        public int? FolioContratoArrto { get; set; }
        public byte Fk_IdTipoContrato { get; set; }
        public string DescripcionTipoContrato { get; set; }
        public byte Fk_IdTipoArrendamiento { get; set; }
        public string DescripcionTipoArrendamiento { get; set; }
        public Nullable<byte> Fk_IdTipoContratacion { get; set; }
        public string DescripcionTipoContratacion { get; set; }
        public int Fk_IdInmuebleArrendamiento { get; set; }
        public Nullable<int> Fk_NumContratoHistorico { get; set; }
        public Nullable<int> Fk_IdContratoArrtoPadre { get; set; }
        public int Fk_IdTipoUsoInm { get; set; }
        public string OtroUsoInmueble { get; set; }
        public Nullable<int> Fk_IdTipoOcupacion { get; set; }
        public string DescripcionTipoOcupacion { get; set; }
        public string OtroTipoOcupacion { get; set; }
        public string ObservacionesContratosReferencia { get; set; }
        public int IsNotReusable { get; set; }
        public int Fk_IdTipoMoneda { get; set; }
        public int Fk_IdInstitucion { get; set; }
        public string NombreInstitucion { get; set; }
        public System.DateTime FechaInicioOcupacion { get; set; }
        public System.DateTime FechaFinOcupacion { get; set; }
        public String PeriodoOcupacion { get; set; } //es la concatenacion de las fechas: Inicio de Ocupacion y Fin de Ocupacion 

        public decimal AreaOcupadaM2 { get; set; }
        public decimal MontoPagoMensual { get; set; }
        public decimal MontoPagoPorCajonesEstacionamiento { get; set; }
        public decimal CuotaMantenimiento { get; set; }
        public decimal PtjeImpuesto { get; set; }

        private decimal _PagoTotalCptosRenta;
        public decimal PagoTotalCptosRenta
        {
            get
            {
                if (PtjeImpuesto > 0)
                    _PagoTotalCptosRenta = (MontoPagoMensual + MontoPagoPorCajonesEstacionamiento + CuotaMantenimiento) * (1+ (PtjeImpuesto/100));
                else
                    _PagoTotalCptosRenta = (MontoPagoMensual + MontoPagoPorCajonesEstacionamiento + CuotaMantenimiento);

                //El descriptor de acceso get debe terminar en una instrucción return o throw
                return _PagoTotalCptosRenta;
            }
            set
            {
                _PagoTotalCptosRenta = value;
            }
         
        }
        public Nullable<int> Fk_IdAplicacionConcepto { get; set; }
        public string NumeroDictamenExcepcionFolioSMOI { get; set; }
        public string RIUF { get; set; }
        public string PropietarioInmueble { get; set; } //pueden ser capturados o se obtienen de la justipreciacion, cuando existe
        public string FuncionarioResponsable { get; set; }//pueden ser capturados o se obtienen de la justipreciacion, cuando existe
        public string Observaciones { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public String strFechaRegistro { get; set; }
        public String strFechaInicioOcupacion { get; set; }
        public String strFechaFinOcupacion { get; set; }
        public Nullable<int> FolioEmisionOpinion { get; set; }//posiblemente se cambie por referencia a objeto (aplica solo a contratos de arrto.)

        //Referencia a una emisión de opinión, desde un ContratoArrto.
        public AplicacionConcepto AplicacionConcepto { get; set; } //aplica solo para contratos Arrto.
     
        //referencias a objetos: Persona Referencia
        public PersonaReferencia PersonaReferenciaResponsableOcupacion { get; set; } //aplica solo a contratos de Otras Fig. de Ocupacion
        public PersonaReferencia PersonaReferenciaTitularOIC { get; set; } 
        public PersonaReferencia PersonaReferenciaCapturista { get; set; }

        //referencias a objeto de: justipreciacion
        public JustripreciacionContrato JustripreciacionContrato { get; set; } //aplica solo a contratos Arrto

        public string CadenaOriginal { get; set; }
        public string SelloDigital { get; set; }

        //referencia a objeto
        public InmuebleArrto InmuebleArrto { get; set; }

        public string DescripcionExcepcionTipoNormativa { get; set; }
        public string ObservacionesExcepcionNormativa { get; set; }
        public int IdTipoArrendamiento { get; set; }
        public int IdConvenio { get; set; }
        public bool? CuentaConDictamen { get; set; }

        public DateTime? FechaDictamen { get; set; }

        //RCA 09/05/2018
        //arrendamiento u otras figuras de ocupacion
        public string TipoRegistro { get; set; }

        //RCA 09/08/2018
        public string QR { get; set; }

        //constructor para inicializar los valores del objeto
        public ContratoArrto()
        {
            QR = string.Empty;
        }

    }//clase
}
