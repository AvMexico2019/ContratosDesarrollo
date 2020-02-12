using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
using System.Reflection;
using System.Data;
//
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //objetos Entities
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio; //para uso del BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion.InmuebleArrto
{
    public partial class DireccionRiuf : System.Web.UI.UserControl
    {
        public string IdInmueble
        {
            get
            {
                return this.LabelIdInmueble.Text;
            }
            set
            {
                this.LabelIdInmueble.Text = value;
            }
        }

        public string RIUF
        {
            get
            {
                return this.TextBoxRIUF.Text;
            }
            set
            {
                this.TextBoxRIUF.Text = value;
            }
        }

        public string ClavePais {
            get
            {
                return this.DropDownListPais.SelectedValue.ToString();
            }
            set 
            {
                this.DropDownListPais.SelectedValue = value;
                if (this.DropDownListPais.SelectedValue == "MEXICO" || this.DropDownListPais.SelectedValue == "MÉXICO")
                    this.PoblarDropDownListEntidadFederativa();
            }
        }

        public string ClaveTipoInmueble
        {
            get
            {
                return this.DropDownListTipoInmueble.SelectedValue.ToString();
            }
            set
            {
                this.DropDownListTipoInmueble.SelectedValue = value;
            }
        }

        public string ClaveEstado
        {
            get
            {
                return this.DropDownListEdo.SelectedValue.ToString();
            }
            set
            {
                if (value.Length > 0)
                {
                    this.DropDownListEdo.SelectedValue = value;
                    this.PoblarDropDownListMposXEntFed();    
                }
            }
        }

        public string ClaveMunicipio
        {
            get
            {
                return this.DropDownListMpo.SelectedValue.ToString();
            }
            set
            {
                if (value.Length > 0)
                {
                    this.DropDownListMpo.SelectedValue = value;
                    this.PoblarDropDownListColoniasXMpo();
                }
            }
        }

        public string DenominacionDeLaDireccion
        {
            get
            {
                return this.TextBoxNombreDireccion.Text;
            }
            set
            {
                this.TextBoxNombreDireccion.Text = value;
            }
        }

        public string CodigoPostal
        {
            get
            {
                return this.TextBoxCP.Text;
            }
            set
            {
                this.TextBoxCP.Text = value;
            }
        }

        public string ClaveColonia
        {
            get
            {
                return this.DropDownListColonia.SelectedValue.ToString();
            }
            set
            {
                if (value != "")
                    this.DropDownListColonia.SelectedValue = value;
                else
                    this.DropDownListColonia.SelectedValue = "-Otra Colonia-";
            }
        }

        public string OtraColonia
        {
            get
            {
                return this.TextBoxOtraColonia.Text;
            }
            set
            {
                this.TextBoxOtraColonia.Text = value;
            }
        }

        public string ClaveTipoVialidad
        {
            get
            {
                return this.DropDownListTipoVialidad.SelectedValue.ToString();
            }
            set
            {
                this.DropDownListTipoVialidad.SelectedValue = value;
            }
        }

        public string NombreVialidad
        {
            get
            {
                return this.TextBoxNombreVialidad.Text;
            }
            set
            {
                this.TextBoxNombreVialidad.Text = value;
            }
        }

        public string NumeroExterior
        {
            get
            {
                return this.TextBoxNumExt.Text;
            }
            set
            {
                this.TextBoxNumExt.Text = value;
            }
        }

        public string NumeroInterior
        {
            get
            {
                return this.TextBoxNumInt.Text;
            }
            set
            {
                this.TextBoxNumInt.Text = value;
            }
        }

        public string GeoReferenciaLatitud
        {
            get
            {
                return this.TextBoxLatitud.Text;
            }
            set
            {
                this.TextBoxLatitud.Text = value;
            }
        }
        public string GeoReferenciaLongitud
        {
            get
            {
                return this.TextBoxLongitud.Text;
            }
            set
            {
                this.TextBoxLongitud.Text = value;
            }
        }
        // Datos Extranjeros
        public string CodigoPostalExtranjero
        {
            get
            {
                return this.TextBoxCPExtranjero.Text;
            }
            set
            {
                this.TextBoxCPExtranjero.Text = value;
            }
        }
        public string EstadoExtranjero
        {
            get
            {
                return this.TextBoxEdoExtranjero.Text;
            }
            set
            {
                this.TextBoxEdoExtranjero.Text = value;
            }
        }

        public string CiudadExtranjero
        {
            get
            {
                return this.TextBoxCiudadExtranjero.Text;
            }
            set
            {
                this.TextBoxCiudadExtranjero.Text = value;
            }
        }

        public string MunicipioExtranjero
        {
            get
            {
                return this.TextBoxMpoExtranjero.Text;
            }
            set
            {
                this.TextBoxMpoExtranjero.Text = value;
            }
        }

        String Msj;
        static List<CatalogoDependiente> ListColonias;
        static List<CatalogoDependiente> ListMpos;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
                this.CargaInicial();
        }
        public void CargaInicial()
        {
            this.CargaInicialCombos();
            this.LabelIdInmueble.Text = "";
            this.LabelInfoInmuebleDir.Text = "";
            this.LabelInfoEnc.Text = "";
        }
        private void CargaInicialCombos()
        {
            // Se cargan los combos BASE
            this.PoblarDropDownTipoOcupacion();
            this.PoblarDropDownListaPais();
            this.PoblarDropDownListEntidadFederativa();
            this.PoblarDropDownListaTipoInmueble();
            this.PoblarDropDownListTipoVialidad();
        }
        public void CargaInicialCampos(bool porRIUF)
        {
            this.DropDownListPais.Enabled = !porRIUF;
            this.DropDownListEdo.Enabled = !porRIUF;
            this.DropDownListMpo.Enabled = !porRIUF;

            if (!porRIUF)
            {
                this.RIUF = "";
                //this.DropDownListColonia.Items.FindByText("--").Selected = true;
                this.OtraColonia = "";
                this.NombreVialidad = "";
                this.DenominacionDeLaDireccion = "";
                this.CodigoPostal = "";
                this.DropDownListTipoVialidad.Items.FindByText("--").Selected = true;
                this.NumeroExterior = "";
                this.NumeroInterior = "";
                this.GeoReferenciaLatitud = "";
                this.GeoReferenciaLongitud = "";
                this.EstadoExtranjero = "";
                this.MunicipioExtranjero = "";
                this.CodigoPostalExtranjero = "";
                this.CiudadExtranjero = "";
            }
        }

        public void HabilitarCampos(bool habilitar)
        {
            this.TextBoxRIUF.Enabled = habilitar;
            this.DropDownListPais.Enabled = habilitar;
            this.DropDownListTipoInmueble.Enabled = habilitar;
            this.DropDownListEdo.Enabled = habilitar;
            this.DropDownListMpo.Enabled = habilitar;
            this.TextBoxNombreDireccion.Enabled = habilitar;
            this.TextBoxCP.Enabled = habilitar;
            this.DropDownListColonia.Enabled = habilitar;
            this.TextBoxOtraColonia.Enabled = habilitar;
            this.DropDownListTipoVialidad.Enabled = habilitar;
            this.TextBoxNombreVialidad.Enabled = habilitar;
            this.TextBoxNumExt.Enabled = habilitar;
            this.TextBoxNumInt.Enabled = habilitar;
            this.TextBoxLatitud.Enabled = habilitar;
            this.TextBoxLongitud.Enabled = habilitar;
            this.TextBoxCPExtranjero.Enabled = habilitar;
            this.TextBoxEdoExtranjero.Enabled = habilitar;
            this.TextBoxCiudadExtranjero.Enabled = habilitar;
            this.TextBoxMpoExtranjero.Enabled = habilitar;
        }

        //llenado de Combo Catalogo de BD Local. 
        private Boolean PoblarDropDownTipoOcupacion()
        {
            Boolean Ok = false;
            List<TipoOcupacion> ListTipoOcupacion;
            try
            {

                ListTipoOcupacion = new NG_Catalogos().ObtenerTipoOcupacion();
                this.DropDownListTipoOcupacion.DataSource = ListTipoOcupacion;
                this.DropDownListTipoOcupacion.DataValueField = "IdTipoOcupacion";
                this.DropDownListTipoOcupacion.DataTextField = "DescripcionTipoOcupacion";
                this.DropDownListTipoOcupacion.DataBind();
                //agregar un elemento para reprsentar que no se ha seleccionado un valor
                this.DropDownListTipoOcupacion.Items.Add("--");
                this.DropDownListTipoOcupacion.Items.FindByText("--").Selected = true;

                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        //Obtener catalogo no depediente de : PAIS
        private Boolean PoblarDropDownListaPais()
        {
            Boolean Ok = false;
            this.DropDownListPais.DataTextField = "Descripcion";
            this.DropDownListPais.DataValueField = "IdValue";

            try
            {
                //obtener informacion de paises del BUS, si la lista ya existe solo obtenerla, sino cargarla del BUS                        
                this.DropDownListPais.DataSource = AdministradorCatalogos.ObtenerCatalogoPais();
                this.DropDownListPais.DataBind();
                //agregar un elemento para representar a todos

                this.DropDownListPais.Items.FindByText("México").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                // Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Paises. Contacta al área de sistemas.";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);

            }
            return Ok;
        }

        //Obtener catalogo no depediente de: Entidad Federativa
        private Boolean PoblarDropDownListEntidadFederativa()
        {
            Boolean Ok = false;
            DropDownListEdo.DataTextField = "Descripcion";
            DropDownListEdo.DataValueField = "IdValue";

            try
            {
                //cargar la lista de estados, si no ha sido cargada poblar, sino presentar
                DropDownListEdo.DataSource = AdministradorCatalogos.ObtenerCatalogoEstados();
                DropDownListEdo.DataBind();
                //agregar un elemento para representar a todos
                DropDownListEdo.Items.Add("--");
                this.DropDownListEdo.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                // Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Entidades Federativas. Contacta al área de sistemas.";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);


            }
            return Ok;
        }

        //Obtener catalogo Depediente de: Municipios de Entidad federativa
        private void PoblarDropDownListMposXEntFed()
        {

            this.DropDownListMpo.DataTextField = "Descripcion";
            this.DropDownListMpo.DataValueField = "IdValue";
            this.DropDownListMpo.DataSource = AdministradorCatalogos.ObtenerMunicipios(Convert.ToInt32(this.DropDownListEdo.SelectedValue));
            this.DropDownListMpo.DataBind();

            //agregar un elemento para representar a todos
            this.DropDownListMpo.Items.Add("--");
            this.DropDownListMpo.Items.FindByText("--").Selected = true;

        }

        //Obtener catalogo no depediente de : TipoInmueble
        private Boolean PoblarDropDownListaTipoInmueble()
        {
            Boolean Ok = false;
            this.DropDownListTipoInmueble.DataTextField = "Descripcion";
            this.DropDownListTipoInmueble.DataValueField = "IdValue";

            try
            {
                //obtener informacion de paises del BUS, si la lista ya existe solo obtenerla, sino cargarla del BUS                        
                this.DropDownListTipoInmueble.DataSource = AdministradorCatalogos.ObtenerCatalogoTipoInmueble();
                this.DropDownListTipoInmueble.DataBind();
                //agregar un elemento para representar a todos

                //agregar un elemento para representar a todos
                this.DropDownListTipoInmueble.Items.Add("--");
                this.DropDownListTipoInmueble.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                // Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Tipo de Inmueble. Contacta al área de sistemas.";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);


            }
            return Ok;
        }

        //Obtener catalogo no depediente de: TipoVialidad
        private Boolean PoblarDropDownListTipoVialidad()
        {
            Boolean Ok = false;
            DropDownListTipoVialidad.DataTextField = "Descripcion";
            DropDownListTipoVialidad.DataValueField = "IdValue";

            try
            {
                DropDownListTipoVialidad.DataSource = AdministradorCatalogos.ObtenerCatalogoTipoVialidad();
                DropDownListTipoVialidad.DataBind();

                //agregar un elemento para representar a todos
                DropDownListTipoVialidad.Items.Add("--");
                this.DropDownListTipoVialidad.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                // Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de vialidades. Contacta al área de sistemas.";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);


            }
            return Ok;
        }

        protected void DropDownListPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DefineVisibilidadPanelesPorPais();
        }

        protected void DropDownListEdo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListEdo.SelectedItem.Text != "--")
                this.PoblarDropDownListMposXEntFed();
            else
            {
                //limpiar mpos porque se ha seleccionado que no se buscara por estado
                this.DropDownListMpo.DataSource = null;
                this.DropDownListMpo.DataBind();
                this.DropDownListMpo.Items.Clear();
            }
        }

        protected void DropDownListMpo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PoblarDropDownListCiudad();
            //PoblarDropDownListColoniasXMpo();

            if (this.DropDownListMpo.SelectedItem.Text != "--")
                this.PoblarDropDownListColoniasXMpo();

            else
            {
                //limpiar colonias porque se ha seleccionado que no se buscara por estado
                this.DropDownListColonia.DataSource = null;
                this.DropDownListColonia.DataBind();
                this.DropDownListColonia.Items.Clear();
            }




        }

        protected void DropDownListColonia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //habiliara caja de texto para escribir la colonia
            if (this.DropDownListColonia.SelectedItem.Text == "-Otra Colonia-")
            {
                this.TextBoxOtraColonia.Enabled = true;
                TextBoxOtraColonia.ToolTip = "Escribe la colonia si no se encuentra en la lista de colonias";
                this.TextBoxOtraColonia.Focus();
            }
            else
            {
                this.TextBoxOtraColonia.Enabled = false;
                TextBoxOtraColonia.ToolTip = "";
                this.TextBoxOtraColonia.Text = String.Empty;

            }
        }

        protected void DropDownListTipoOcupacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListTipoOcupacion.SelectedItem.Text == "Otros")
            {
                this.TextBoxOtrosTipoOcupacion.ReadOnly = false;
                this.TextBoxOtrosTipoOcupacion.Focus();
            }
            else
            {
                this.TextBoxOtrosTipoOcupacion.ReadOnly = true;
                this.TextBoxOtrosTipoOcupacion.Text = String.Empty;
                DropDownListTipoOcupacion.Focus();
            }

        }
        
        //Obtener catalogo Depediente de: Colonias de Mpo
        private void PoblarDropDownListColoniasXMpo()
        {

            this.DropDownListColonia.DataTextField = "Descripcion";
            this.DropDownListColonia.DataValueField = "IdValue";
            this.DropDownListColonia.DataSource = AdministradorCatalogos.ObtenerLocalidades(Convert.ToInt32(this.DropDownListPais.SelectedValue), Convert.ToInt32(this.DropDownListEdo.SelectedValue), Convert.ToInt32(this.DropDownListMpo.SelectedValue));
            this.DropDownListColonia.DataBind();

            //agregar para cuando no exista la colonia
            this.DropDownListColonia.Items.Add("-Otra Colonia-");

            //agregar un elemento para representar a todos
            this.DropDownListColonia.Items.Add("--");
            this.DropDownListColonia.Items.FindByText("--").Selected = true;
            this.rfvDropDownListColonia.InitialValue = "--";
        }

        // Muestreo de Error
        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        public bool ValidarEntradaDatos()
        {
            this.rfvTextBoxNombreDireccion.Validate();
            if (!this.rfvTextBoxNombreDireccion.IsValid)
            {
                this.TextBoxNombreDireccion.Text = "<div class='has-error'></div>";
                return false;
            }

            if (TextBoxNombreDireccion.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar un nombre cualquiera al inmueble de arrendamiento"; ;
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                TextBoxNombreDireccion.Focus();
                MostrarMensajeJavaScript(Msj);
                return false;
            }


            if (this.DropDownListTipoInmueble.SelectedItem.Text == "--")
            {
                Msj = "Debes seleccionar el tipo del Inmueble";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                MostrarMensajeJavaScript(Msj);
                DropDownListMpo.Focus();
                return false;
            }


            //validacion CP
            if (DropDownListPais.SelectedItem.Text == "México")
            {
                if (this.DropDownListEdo.SelectedItem.Text == "Seleccione un Estado")
                {

                    Msj = "Debes seleccionar un estado en el que se ubica la dirección del Inmueble";
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    DropDownListEdo.Focus();
                    return false;
                }

                if (this.DropDownListMpo.SelectedItem.Text == "--")
                {
                    Msj = "Debe seleccionar un municipio en el que se ubica la dirección del Inmueble";
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    DropDownListMpo.Focus();
                    return false;
                }

                if (this.DropDownListColonia.SelectedItem.Text == "--")
                {

                    Msj = "Debes seleccionar una colonia en la que se ubica la dirección del Inmueble";
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    DropDownListColonia.Focus();
                    return false;

                }


                if (this.DropDownListColonia.SelectedItem.Text == "-Otra Colonia-")
                {
                    if (this.TextBoxOtraColonia.Text.Length < 2)
                    {
                        Msj = "Debes proporcionar la colonia en la que se ubica la dirección del Inmueble";
                        this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                        MostrarMensajeJavaScript(Msj);
                        TextBoxOtraColonia.Focus();
                        return false;
                    }
                }


                if (TextBoxCP.Text.Trim().Length < 5)
                {
                    Msj = "Debes proporcionar un código postal de 5 digitos en la dirección del Inmueble";
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    TextBoxCP.Focus();
                    return false;
                }


            }
            else //es validacion de direccion extranjera
            {

                if (TextBoxEdoExtranjero.Text.Trim().Length == 0)
                {
                    Msj = "Debes proporcionar un estado ó equivalente en la dirección del inmueble de: " + ((DropDownList)DropDownListPais).SelectedItem.Text;
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    TextBoxEdoExtranjero.Focus();
                    return false;
                }

                if (TextBoxMpoExtranjero.Text.Trim().Length == 0)
                {
                    Msj = "Debes proporcionar un municipio ó equivalente en la dirección del inmueble de: " + ((DropDownList)DropDownListPais).SelectedItem.Text;
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    TextBoxMpoExtranjero.Focus();
                    return false;
                }

                if (TextBoxCiudadExtranjero.Text.Trim().Length == 0)
                {
                    Msj = "Debes proporcionar una ciudad en la dirección del inmueble de: " + ((DropDownList)DropDownListPais).SelectedItem.Text;
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    TextBoxCiudadExtranjero.Focus();
                    return false;
                }


                if (TextBoxCPExtranjero.Text.Trim().Length == 0)
                {
                    Msj = "Debes proporcionar un código postal o equivalente en la dirección del inmueble de: " + ((DropDownList)DropDownListPais).SelectedItem.Text;
                    this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                    this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                    MostrarMensajeJavaScript(Msj);
                    TextBoxCP.Focus();
                    return false;
                }
            }


            if (this.DropDownListTipoVialidad.SelectedItem.Text == "--")
            {
                Msj = "Debes seleccionar un tipo de vialidad en el que se ubica la dirección del Inmueble";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                MostrarMensajeJavaScript(Msj);
                DropDownListTipoVialidad.Focus();
                return false;
            }


            if (TextBoxNombreVialidad.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar el nombre de la vialidad de la dirección del Inmueble";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                MostrarMensajeJavaScript(Msj);
                TextBoxNombreVialidad.Focus();
                return false;
            }

            if (TextBoxNumExt.Text.Trim().Length == 0)
            {
                Msj = "Debes proporcionar un número exterior de la dirección del Inmueble";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfoEnc.Text = this.LabelInfoInmuebleDir.Text;
                MostrarMensajeJavaScript(Msj);
                TextBoxNumExt.Focus();
                return false;
            }



            return true;
        }

        public void DefineVisibilidadPanelesPorPais()
        {
            if (DropDownListPais.SelectedItem.Text == "México")
            {
                this.PanelNacional.Visible = true;
                this.PanelExtranjero.Visible = false;
            }

            else
            {
                this.PanelNacional.Visible = false;
                this.PanelExtranjero.Visible = true;
            }
        }

        //cargar al inicio todas las colonias, y despues aplicar el filtro al seleccionar un mpo
        //se carga al inicio para no estar cargando n veces por cada postback

        private bool CargaInicialListaMpos_Colonias()
        {
            Boolean Ok = false;
            try
            {
                //cargar la lista de conceptos
                ListColonias = new NG().LlenaComboDependiente("ObtenerLocalidad");

                ListMpos = new NG().LlenaComboDependiente("ObtenerMunicipio");
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                //Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de colonias ó municipios. Contacta al área de sistemas.";
                this.LabelInfoInmuebleDir.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);

            }
            return Ok;
        }

        protected void chkOtraFiguraOcupacion_CheckedChanged(object sender, EventArgs e)
        {
            this.PanelOcupacion.Visible = this.chkOtraFiguraOcupacion.Checked; 
        }
    }
}