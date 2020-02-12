using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class EstatusRUSPvsRIUF
    {
        public int IdContrato { get; set; }
        public int? IdInmuebleArrendamiento { get; set; }
        public string NombreInmueble { get; set; }
        private string _NombrePais;
        public string NombrePais
        {
            get
            {
                return _NombrePais.ToUpper();
            }
            set
            {
                _NombrePais = value;
            }
        }

        private string _NombreEstado;
        public string NombreEstado
        {
            get
            {
                if(_NombreEstado == null)
                {
                    return _NombreEstado = "";
                }
                else
                {
                    return _NombreEstado.ToUpper();
                }
                
            }
            set
            {
                _NombreEstado = value;
            }
        }

        private string _NombreMunicipio;
        public string NombreMunicipio
        {
            get
            {
                if(_NombreMunicipio == null)
                {
                    return _NombreMunicipio = "";
                }
                else
                {
                    return _NombreMunicipio.ToUpper();
                }
                
            }
            set
            {
                _NombreMunicipio = value;
            }
        }

        private string _NombreColonia;
        public string NombreColonia
        {
            get
            {
                if(_NombreColonia == null)
                {
                    return _NombreColonia = "";
                }
                else
                {
                    return _NombreColonia.ToUpper();
                }
                
            }
            set
            {
                _NombreColonia = value;
            }
        }

        public string CodigoPostal { get; set; }

        public string TipoVialidad { get; set; }
        public string NombreVialidad { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }

        //agregamos el domicilio completo
        public string _DireccionCompleta;
        public string DireccionCompleta
        {
            get
            {
                if (_NombrePais != null)
                {
                    _DireccionCompleta = NombrePais;

                    if (string.IsNullOrEmpty(NombreEstado))
                    {
                        NombreEstado = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreMunicipio))
                    {
                        NombreMunicipio = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreColonia))
                    {
                        NombreColonia = string.Empty;
                    }

                    if (string.IsNullOrEmpty(CodigoPostal))
                    {
                        CodigoPostal = string.Empty;
                    }

                    if (string.IsNullOrEmpty(TipoVialidad))
                    {
                        TipoVialidad = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreVialidad))
                    {
                        NombreVialidad = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NumeroExterior))
                    {
                        NumeroExterior = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NumeroInterior))
                    {
                        NumeroInterior = string.Empty;
                    }

                    _DireccionCompleta += ", ESTADO: " + NombreEstado.Trim() + " " + ", MUNICIPIO: " + NombreMunicipio.Trim() + ", COLONIA: " + NombreColonia.Trim() + ", CP: " + CodigoPostal.Trim() + ", " + TipoVialidad.Trim() + " " + NombreVialidad.Trim() + ", #EXT: " + NumeroExterior.Trim() + ", #INT: " + NumeroInterior.Trim();

                    return _DireccionCompleta;
                }
                else
                {
                    throw new NotImplementedException("No existe la propiedad del país del inmueble, para exponer su dirección completa");
                }
            }

            set
            {
                _DireccionCompleta = value;
            }
        }

        public DateTime FechaAltaMvtoAInmueble { get; set; }

        //RCA 08/06/2018
        public DateTime FechaFinOcupacion { get; set; }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCargo { get; set; }


        private string _PromoventeConCargo;
        public string PromoventeConCargo
        {
            get
            {
                return _PromoventeConCargo = NombreUsuario + " (" + NombreCargo + ")";
            }
            set
            {
                _PromoventeConCargo = value;
            }
        }

        public int? FolioAplicacionConcepto { get; set; }
        public int FolioContratoArrto { get; set; }
        public string DescripcionTipoContrato { get; set; }
        public string DescripcionTipoArrendamiento { get; set; }
        public string NombreInstitucion { get; set; }
        public string RIUF { get; set; }
        public string ObservacionesContratosReferencia { get; set; }
        public string TipoRegistro { get; set; }
        public string EstatusRUSP { get; set; }

        public string TipoContrato { get; set; }
    }
}
