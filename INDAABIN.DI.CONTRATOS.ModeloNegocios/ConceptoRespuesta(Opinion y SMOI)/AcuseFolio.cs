using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{

    //este objeto de negocio se utiliza para Acuse de Emisio de Opinion y de SMOI
    public class AcuseFolio
    {
        public int Folio { get; set; }
        public string FechaRegistro { get; set; }
        public string HoraRegistro { get; set; }
        public string TipoArrendamientoDesc { get; set; }
        public string ResultadoAplicacionOpinion { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public int IdInstitucionSolicitante { get; set; }//IdFK con el BUS
        public string InstitucionSolicitante { get; set; }
        public string CargoUsrRegistro { get; set; }
        public string LeyendaAnio { get; set; }
        public string CadenaOriginal { get; set; }
        public string SelloDigital { get; set; }

        //propiedades de acuse de SMOI
        public decimal? TotalSMOIm2FactorX { get; set; }
        public decimal? TotalSMOIm2FactorY { get; set; }
        public decimal? TotalSMOIm2FactorZ { get; set; }

        //factor Y, derivado
        private decimal _TotalSMOIm2FactorY_Derivado;
        public decimal TotalSMOIm2FactorY_Derivado
        {
            get
            {
                decimal value = 0;
                try
                {
                    //El descriptor de acceso get debe terminar en una instrucción return o throw
                    _TotalSMOIm2FactorY_Derivado = TotalSMOIm2.Value - (TotalSMOIm2FactorX.Value + TotalSMOIm2FactorZ.Value);
                    _TotalSMOIm2FactorY_Derivado = TotalSMOIm2FactorX != null || TotalSMOIm2FactorX.Value != 0 ? _TotalSMOIm2FactorY_Derivado / TotalSMOIm2FactorX.Value : 0; // MZT

                    value = (TotalSMOIm2FactorX.Value * _TotalSMOIm2FactorY_Derivado);
                }
                catch (Exception)
                {
                }

                return value;
            }
            set //asignar valor al campo privado
            {
                _TotalSMOIm2FactorY_Derivado = value;
            }

        }

        //Es el factor que se suma al factorX para obtener el factorY
        //en 2016 = 0.44
        private decimal _FactorY_Calculado;
        public decimal FactorY_Calculado
        {
            get
            {
                decimal value = 0;
                try
                {
                    _FactorY_Calculado = TotalSMOIm2.Value - (TotalSMOIm2FactorX.Value + TotalSMOIm2FactorZ.Value);
                    _FactorY_Calculado = TotalSMOIm2FactorX.Value != null || TotalSMOIm2FactorX.Value != 0 ? _FactorY_Calculado / TotalSMOIm2FactorX.Value : 0; // MZT

                    value = _FactorY_Calculado;
                }
                catch
                {
                }

                return value;
            }
        }

        public decimal? TotalSMOIm2 { get; set; }

        //objeto de negocio, embedido en un Acuse emisión Opinion
        public InmuebleArrto InmuebleArrtoEmisionOpinion { get; set; }

        //RCA 10/08/2018
        public string QR { get; set; }
        public string LeyendaQR { get; set; }
        public DateTime? FechaAutorizacion { get; set; }


    }
}
