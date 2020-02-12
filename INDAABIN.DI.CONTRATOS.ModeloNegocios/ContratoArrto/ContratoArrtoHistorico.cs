using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class ContratoArrtoHistorico
    {

        public int NumContratoHistorico { get; set; }
        public string Secuencial { get; set; }
        public string Propietario { get; set; }
        public Nullable<System.DateTime> FechaDictamen { get; set; }
        public string Responsable { get; set; }
        public string Sector { get; set; }
        public string InstitucionPromovente { get; set; }
        public string Delegacion { get; set; }
        public string Ciudad { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string CP { get; set; }
        public string NoInt { get; set; }
        public string NoExt { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }

        public string DireccionCompleta { get; set; }

        public Nullable<decimal> MontoDictaminado { get; set; }

        public String FechaContrato { get; set; }

        //Periodo de ocupacion
        public String FechaInicioContrato { get; set; }
        public String FechaFinContrato { get; set; }
        public Nullable<short> CajonesEstacionamiento { get; set; }
        public string TipoContratacion { get; set; }
        public Nullable<decimal> MontRentaMensual { get; set; }
        public Nullable<decimal> AreaRentable { get; set; }
        public string UsoInmueble { get; set; }
        public string OtroUsoInmueble { get; set; }
        public Nullable<decimal> TablaSmoi { get; set; }
        public Nullable<decimal> CuotaMantenimiento { get; set; }

      

    }
}
