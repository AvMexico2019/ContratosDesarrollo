using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    /// <summary>
    /// Esto objeto de negocio sirve para representar a un Inmueble sin asociar a un contratoArrendamiento, entonces es un Inmueble Candidato de Arrendamiento, que se registra normalmente para una sol. de Opinion
    /// ó
    /// a un inmueble que se asocia con un contrato de arendamiento, entonces es un Inmueble Arrendado
    /// </summary>
    public class Inmueble
    {
        public int IdInmueble { get; set; }
        public int IdPais { get; set; }
        public string PaisDescripcion { get; set; }
        public Nullable<int> IdTipoInmueble { get; set; }
        public string TipoInmuebleDescripcion { get; set; }
        public Nullable<int> IdEstado { get; set; }
        public string EstadoDescripcion { get; set; }
        public Nullable<int> IdMunicipio { get; set; }
        public string MunicipioDescripcion { get; set; }
        public Nullable<int> IdLocalidad { get; set; }
        public string LocalidadDescripcion { get; set; }
        public string OtraColonia { get; set; }
        public int IdTipoVialidad { get; set; }
        public string TipoVialidadDescripcion { get; set; }
        public string NombreVialidad { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public string CodigoPostal { get; set; }
        public Nullable<decimal> GeoRefLatitud { get; set; }
        public Nullable<decimal> GeoRefLongitud { get; set; }
        public string NombreInmueble { get; set; }
        public Nullable<int> IdRIUF { get; set; }
        public bool EstatusRegistro { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public string UsuarioRegistroDescripcion { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public RIUF RIUF { get; set; }
        public int GeneraRIUF { get; set; }
        public string CodigoPostalExtranjero { get; set; } 
        public string EstadoExtranjero { get; set; } 
        public string CiudadExtranjero { get; set; } 
        public string MunicipioExtranjero { get; set; } 

        private string _DireccionCompleta;
        public string DireccionCompleta    // the Name property, publica, solo ella accesa al campo
        {
            get
            {
                //El descriptor de acceso get debe terminar en una instrucción return o throw
                if (PaisDescripcion != null)
                {
                    _DireccionCompleta = PaisDescripcion;

                    if (QuitarAcentosTexto(PaisDescripcion).ToUpper() == "MEXICO")
                    {
                        if (IdLocalidad == null)
                            LocalidadDescripcion = OtraColonia;

                        if (NumInterior == null)
                            _DireccionCompleta += ", ESTADO: " + EstadoDescripcion + " " + ", MUNICIPIO: " + MunicipioDescripcion + ", COLONIA: " + LocalidadDescripcion + ", CP: " + CodigoPostal + ", " + TipoVialidadDescripcion + ": " + NombreVialidad + ", #EXT: " + NumExterior;
                        else
                            _DireccionCompleta += ", ESTADO: " + EstadoDescripcion + " " + ", MUNICIPIO: " + MunicipioDescripcion + ", COLONIA: " + LocalidadDescripcion + ", CP: " + CodigoPostal + ", " + TipoVialidadDescripcion + ": " + NombreVialidad + ", #EXT: " + NumExterior + ", #INT: " + NumInterior;
                    }
                    else //es pais extranjero
                    {
                        if (NumInterior == null)
                            _DireccionCompleta += "," + EstadoExtranjero + ", " + " " + MunicipioExtranjero + ", " + CiudadExtranjero + ", " + CodigoPostalExtranjero + ", " + TipoVialidadDescripcion + " " + NombreVialidad + ", #EXT: " + NumExterior;
                        else
                            _DireccionCompleta += "," + EstadoExtranjero + ", " + " " + MunicipioExtranjero + ", " + CiudadExtranjero + ", " + CodigoPostalExtranjero + ", " + TipoVialidadDescripcion + " " + NombreVialidad + ", #EXT: " + NumExterior + ", #INT: " + NumInterior;
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

        //*************  Metodos locales  *********************************************

        private string QuitarAcentosTexto(string Texto)
        {
            string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            return textoSinAcentos;
        }

    }//clase
}
