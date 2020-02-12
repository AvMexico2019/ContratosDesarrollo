using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    [Serializable]
    /// <summary>
    /// Esto objeto de negocio sirve para representar a un Inmueble sin asociar a un contratoArrendamiento, entonces es un Inmueble Candidato de Arrendamiento, que se registra normalmente para una sol. de Opinion
    /// ó
    /// a un inmueble que se asocia con un contrato de arendamiento, entonces es un Inmueble Arrendado
    /// </summary>
    public class InmuebleArrto
    {
        //RCA 17/08/2017
        public int FolioContratoArrto { get; set; }
        public Nullable<int> Fk_IdContratoArrtoPadre { get; set; }
        public Nullable<bool> Bandera { get; set; }
        //estatus registro de contrato
        public Nullable<bool> Estatus_RegistroContrato { get; set; }
        public string RIUFInmueble { get; set; }
        public int? IdInmueble { get; set; }
        public int IdInmuebleArrendamiento { get; set; }
        public int IdInstitucion { get; set; }
        public string NombreInstitucion { get; set; }
        public int IdCargo { get; set; }
        public string NombreCargo { get; set; }        

        private string _PromoventeConCargo;
        public string PromoventeConCargo
        {
            get
            {
                //El descriptor de acceso get debe terminar en una instrucción return o throw
                return _PromoventeConCargo = NombreUsuario + " (" + NombreCargo + ")";
            }
            set //asignar valor al campo privado
            {
                _PromoventeConCargo = value;
            }
        }

        private string _NombreInmueble;
        public string NombreInmueble
        {
            get
            {
                return _NombreInmueble.ToUpper(); //regresar como mayusculas
            }
            set
            {
                _NombreInmueble = value;
            }
        }
        public int IdPais { get; set; } //de direccion solo de inmueble nacional

        private string _NombrePais;
        public string NombrePais
        {
            get
            {
                return _NombrePais.ToUpper(); //regresar como mayusculas
            }
            set
            {
                _NombrePais = value;
            }
        }
        public int? IdEstado { get; set; } //de direccion solo de inmueble nacional

        private string _NombreEstado;
        public string NombreEstado
        {
            get
            {
                return !string.IsNullOrEmpty(_NombreEstado) ? _NombreEstado.ToUpper() : string.Empty; //regresar como mayusculas
            }
            set
            {
                _NombreEstado = value;
            }
        }
        public int? IdMunicipio { get; set; } //de direccion solo de inmueble nacional
        public string NombreMunicipio { get; set; }

        public int? IdLocalidadColonia { get; set; } //de direccion solo de inmueble nacional
        public string NombreLocalidadColonia { get; set; }

        public string OtraColonia { get; set; }

        public string CodigoPostal { get; set; }

        public int IdTipoVialidad { get; set; }
        public string NombreTipoVialidad { get; set; }

        public string NombreVialidad { get; set; } //Nombre de la Calle, Av, etc...

        private string _NumExterior;

        public string NumExterior
        {
            get
            {
                //El descriptor de acceso get debe terminar en una instrucción return o throw
                //return "Num. " + _NumExterior;
                return _NumExterior;
            }
            set //asignar valor al campo privado
            {
                _NumExterior = value;
            }
        }

        private string _NumInterior;

        public string NumInterior    // the Name property, publica, solo ella accesa al campo
        {
            get
            {
                //El descriptor de acceso get debe terminar en una instrucción return o throw
                if (!String.IsNullOrEmpty(_NumInterior))
                    //     return "Num. Int. --";                 
                    //else
                    //return "Num. Int. " + _NumInterior;
                    return _NumInterior;
                else
                    return _NumInterior;
            }
            set //asignar valor al campo privado
            {
                _NumInterior = value;
            }
        }

        public decimal? GeoRefLatitud { get; set; }
        public decimal? GeoRefLongitud { get; set; }

        //datos de dirccion dei inmueble en el extranjero
        public string CodigoPostalExtranjero { get; set; }//de direccion solo de inmueble extranjero
        public string EstadoExtranjero { get; set; } //de direccion solo de inmueble extranjero
        public string CiudadExtranjero { get; set; } //equivalene  a colonia
        public string MunicipioExtranjero { get; set; } //de direccion solo de inmueble extranjero

        public int? Fk_IdTipoInmueble { get; set; } //del bus

        public string DescripcionTipoInmueble { get; set; }//del bus
        //este estatus registro es de inmuebleArrendamiento 
        public Boolean EstatusRegistro { get; set; }

        public int IdUsuarioRegistro { get; set; }

        public string NombreUsuario { get; set; }
        public DateTime FechaAlta { get; set; }

        public string FechaAltaMvtoAInmueble { get; set; }

        
        //RCA 16/08/2017
        public DateTime FechaFinOcupacion { get; set; }


        public string _DireccionCompleta;
        public string DireccionCompleta    // the Name property, publica, solo ella accesa al campo
        {
            get
            {
                //El descriptor de acceso get debe terminar en una instrucción return o throw
                if (NombrePais != null)
                {
                    _DireccionCompleta = NombrePais;

                    // MZT validación de datos para evitar escepción por nulos
                    if (string.IsNullOrEmpty(NombreEstado))
                    {
                        NombreEstado = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreMunicipio))
                    {
                        NombreMunicipio = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreLocalidadColonia))
                    {
                        NombreLocalidadColonia = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreLocalidadColonia))
                    {
                        NombreLocalidadColonia = string.Empty;
                    }

                    if (string.IsNullOrEmpty(CodigoPostal))
                    {
                        CodigoPostal = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreTipoVialidad))
                    {
                        NombreTipoVialidad = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NombreVialidad))
                    {
                        NombreVialidad = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NumExterior))
                    {
                        NumExterior = string.Empty;
                    }

                    if (string.IsNullOrEmpty(NumInterior))
                    {
                        NumInterior = string.Empty;
                    }

                    // MZT validación de datos para evitar escepción por nulos

                    if (QuitarAcentosTexto(NombrePais).ToUpper() == "MEXICO")
                    {
                        if (NombreLocalidadColonia == null)
                            NombreLocalidadColonia = this.OtraColonia;
                        // MZT ya no aplica esta validación
                        //if (NumInterior == null)
                        //    _DireccionCompleta += ", ESTADO: " + NombreEstado.Trim() + " " + ", MUNICIPIO: " + NombreMunicipio.Trim() + ", COLONIA: " + NombreLocalidadColonia.Trim() + ", CP: " + CodigoPostal.Trim() + ", " + NombreTipoVialidad.Trim() + " " + NombreVialidad.Trim() + ", #EXT: " + NumExterior.Trim();
                        //else
                        _DireccionCompleta += ", ESTADO: " + NombreEstado.Trim() + " " + ", MUNICIPIO: " + NombreMunicipio.Trim() + ", COLONIA: " + NombreLocalidadColonia.Trim() + ", CP: " + CodigoPostal.Trim() + ", " + NombreTipoVialidad.Trim() + " " + NombreVialidad.Trim() + ", #EXT: " + NumExterior.Trim() + ", #INT: " + NumInterior.Trim();
                    }
                    else //es pais extranjero
                    {
                        //if (NumInterior == null)
                        //    _DireccionCompleta += "," + EstadoExtranjero.Trim() + ", " + " " + MunicipioExtranjero.Trim() + ", " + CiudadExtranjero.Trim() + ", " + CodigoPostalExtranjero.Trim() + ", " + NombreTipoVialidad.Trim() + " " + NombreVialidad.Trim() + ", #EXT: " + NumExterior.Trim();
                        //else
                        // MZT ya no aplica esta validación
                        _DireccionCompleta += "," + EstadoExtranjero.Trim() + ", " + " " + MunicipioExtranjero.Trim() + ", " + CiudadExtranjero.Trim() + ", " + CodigoPostalExtranjero.Trim() + ", " + NombreTipoVialidad.Trim() + " " + NombreVialidad.Trim() + ", #EXT: " + NumExterior.Trim() + ", #INT: " + NumInterior.Trim();
                    }


                    return _DireccionCompleta;
                }
                else

                    throw new NotImplementedException("No existe la propiedad del país del inmueble, para exponer su dirección completa");
            }
            set //asignar valor al campo privado
            {
                _DireccionCompleta = value;
            }
        }

        public string DireccionCompletaSinPais    // the Name property, publica, solo ella accesa al campo
        {
            get
            {
                //El descriptor de acceso get debe terminar en una instrucción return o throw
                if (NombrePais != null)
                {
                    if (QuitarAcentosTexto(NombrePais).ToUpper() == "MEXICO")
                    {
                        if (NombreLocalidadColonia == null)
                            NombreLocalidadColonia = this.OtraColonia;

                        if (NumInterior == null)
                            _DireccionCompleta = "ESTADO: " + NombreEstado + " " + ", MUNICIPIO: " + NombreMunicipio + ", COLONIA: " + IdLocalidadColonia == null ? OtraColonia : NombreLocalidadColonia.Trim() + ", CP: " + CodigoPostal + ", " + NombreTipoVialidad + " " + NombreVialidad + ", #EXT: " + NumExterior;
                        else
                            _DireccionCompleta = "ESTADO: " + NombreEstado + " " + ", MUNICIPIO: " + NombreMunicipio + ", COLONIA: " + IdLocalidadColonia == null ? OtraColonia : NombreLocalidadColonia.Trim() + ", CP: " + CodigoPostal + ", " + NombreTipoVialidad + " " + NombreVialidad + ", #EXT: " + NumExterior + ", #INT: " + NumInterior;
                    }
                    else //es pais extranjero
                    {
                        if (NumInterior == null)
                            _DireccionCompleta = EstadoExtranjero + ", " + " " + MunicipioExtranjero + ", " + CiudadExtranjero + ", " + CodigoPostalExtranjero + ", " + NombreTipoVialidad + " " + NombreVialidad + ", #EXT: " + NumExterior;
                        else
                            _DireccionCompleta = EstadoExtranjero + ", " + " " + MunicipioExtranjero + ", " + CiudadExtranjero + ", " + CodigoPostalExtranjero + ", " + NombreTipoVialidad + " " + NombreVialidad + ", #EXT: " + NumExterior + ", #INT: " + NumInterior;
                    }
                    return _DireccionCompleta;
                }
                else
                    throw new NotImplementedException("No existe la propiedad del pais del inmueble, para exponer su dirección completa");
            }
            set //asignar valor al campo privado
            {
                _DireccionCompleta = value;
            }
        }

        //propiedad apuntador a una emisión de opinión, para un InmuebleArrto
        public AplicacionConcepto EmisionOpinion { get; set; }

        //para conocer el FolioContrato con el que se asocia un inmueble, nulo porque puede ser que un inmueble no se asocie a un contratoArrto
        public int? FolioContratoArrtoVsInmuebleArrendado { get; set; } //puede ser nulo, porque quizas el inmueble no se asocia a un ContratoArrto.

        public int IdTipoInmueble { get; set; } //TERRENO, EDIFICACIÓN, MIXTO (del bus)
        public string NombreTipoInmueble { get; set; }

        //RCA 05/07/2018
        //Agregamos dos elementos mas para el folio de SAEF
        public string FolioSAEF { get; set; }


        //un objeto inmueble, al que se registra un Contrato
        public ContratoArrto ContratoArrtoInmueble { get; set; }

        //*************  Metodos locales  *********************************************

        private string QuitarAcentosTexto(string Texto)
        {
            if (!string.IsNullOrEmpty(Texto))
            {
                string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
                string textoSinAcentos = reg.Replace(textoNormalizado, "");
                return textoSinAcentos;
            }
            return string.Empty;
        }

    }//clase
}
