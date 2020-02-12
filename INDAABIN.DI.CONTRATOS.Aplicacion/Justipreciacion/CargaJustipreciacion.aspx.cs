using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;//acceder al webconfig
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using INDAABIN.DI.CONTRATOS.ModeloNegocios;
using INDAABIN.DI.CONTRATOS.Negocio;
using INDAABIN.DI.ModeloNegocio;
using System.Text;//interconexion con el BUS

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Justipreciacion
{
    public partial class CargaJustipreciacion : System.Web.UI.Page
    {
        String Msj;
        String RolUsr;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["Contexto"] == null)
                {
                    Response.Redirect(ConfigurationManager.AppSettings.Get("URL_SSO") + ConfigurationManager.AppSettings.Get("TokenApp").Replace("-", ""));
                }

                SSO usuario = (SSO)Session["Contexto"];
                String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(usuario.LRol);

                if (!RolUsr.Equals("ADMINISTRADOR DE CONTRATOS", StringComparison.InvariantCultureIgnoreCase))
                //if (!RolUsr.Equals("ADMINISTRADOR", StringComparison.InvariantCultureIgnoreCase))
                {
                    Response.Redirect("~/Principal.aspx", true);
                }



                //this.PoblarDropDownSector();
                this.PoblarDropDownInstitucion();

                this.PoblarDropDownListEntidadFederativa();

                this.PoblarDropDrownUnidadTerDic();

                this.PoblarDropDrownUnidadConsDic();

                this.PoblarDropDrownUnidadRenDic();
            }
        }

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
                //this.DropDownListEdo.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Entidades Federativas. Contacta al área de sistemas.";
                //this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        private void PoblarDropDownListMposXEntFed()
        {
            try
            {
                this.DropDownListMpo.DataTextField = "Descripcion";
                this.DropDownListMpo.DataValueField = "IdValue";
                this.DropDownListMpo.DataSource = AdministradorCatalogos.ObtenerMunicipios(Convert.ToInt32(this.DropDownListEdo.SelectedValue));
                this.DropDownListMpo.DataBind();

                //agregar un elemento para representar a todos
                this.DropDownListMpo.Items.Add("--");
                this.DropDownListMpo.Items.FindByText("--").Selected = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error al cargar la lista de Municipios. Contacta al área de sistemas.";
                //this.LabelInfoGridResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);

                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",

                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                    
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }
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

        private Boolean PoblarDropDrownUnidadTerDic()
        {
            Boolean Ok = false;
            DropDownListUniTer.DataTextField = "Descripcion";
            DropDownListUniTer.DataValueField = "IdValue";

            try
            {
                //cargar la lista de unidades de medida, si no ha sido cargada poblar, sino presentar
                DropDownListUniTer.DataSource = AdministradorCatalogos.ObtenerCatalogoUnidadTerrenoDic();
                DropDownListUniTer.DataBind();
                //agregar un elemento para representar a todos
                DropDownListUniTer.Items.Add("--");
                this.DropDownListUniTer.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Unidades de medida. Contacta al área de sistemas.";
                //this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        private Boolean PoblarDropDrownUnidadConsDic()
        {
            Boolean Ok = false;
            DropDownListUniCons.DataTextField = "Descripcion";
            DropDownListUniCons.DataValueField = "IdValue";

            try
            {
                //cargar la lista de unidades de medida, si no ha sido cargada poblar, sino presentar
                DropDownListUniCons.DataSource = AdministradorCatalogos.ObtenerCatalogoUnidadConstruidaDic();
                DropDownListUniCons.DataBind();
                //agregar un elemento para representar a todos
                DropDownListUniCons.Items.Add("--");
                this.DropDownListUniCons.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Unidades de medida. Contacta al área de sistemas.";
                //this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        private Boolean PoblarDropDrownUnidadRenDic()
        {
            Boolean Ok = false;
            DropDownListRenDic.DataTextField = "Descripcion";
            DropDownListRenDic.DataValueField = "IdValue";

            try
            {
                //cargar la lista de unidades de medida, si no ha sido cargada poblar, sino presentar
                DropDownListRenDic.DataSource = AdministradorCatalogos.ObtenerCatalogoUnidadRentableDic();
                DropDownListRenDic.DataBind();
                //agregar un elemento para representar a todos
                DropDownListRenDic.Items.Add("--");
                this.DropDownListRenDic.Items.FindByText("--").Selected = true;
                Ok = true;
            }
            catch (Exception ex)
            {
                //msj al usuario
                Msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Msj = "Ocurrió una excepción al cargar la lista de Unidades de medida. Contacta al área de sistemas.";
                //this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);
            }
            return Ok;
        }

        private Boolean PoblarDropDownInstitucion()
        {
            Boolean Ok = false;
            DropDownListInstitucion.DataTextField = "Descripcion";
            DropDownListInstitucion.DataValueField = "IdValue";

            try
            {
                //cargar la lista de las instituciones, si no ha sido cargada poblar, sino presentar
                DropDownListInstitucion.DataSource = AdministradorCatalogos.ObtenerCatalogoInstitucion();
                DropDownListInstitucion.DataBind();

            }
            catch (Exception)
            {
                //msj al usuario
                Msj = "Ocurrió una excepción al cargar la lista de instituciones. Contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                this.LabelInfo.Text = Msj;
                MostrarMensajeJavaScript(this.LabelInfo.Text);

            }
            return Ok;
        }

        //RCA 26/12/2017
        //metodo para cargar la lista de colonias de acuerdo al codigo postal
        protected void TextBoxCP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TextBoxCP.Text))
                {
                    int i;

                    if (int.TryParse(this.TextBoxCP.Text.Trim(), out i))
                    {
                        if (i > 0)
                        {
                            this.DropDownListColonia.DataTextField = "Descripcion";
                            this.DropDownListColonia.DataValueField = "IdValue";
                            this.DropDownListColonia.DataSource = AdministradorCatalogos.ObtenerLocalidadesPorCodigoPostal(this.TextBoxCP.Text.Trim().PadLeft(2, '0'));
                            this.DropDownListColonia.DataBind();

                            //agregar para cuando no existe la colonia.
                            this.DropDownListColonia.Items.Add("- Otra colonia -");

                            //agregar un elemento para representar a todos
                            this.DropDownListColonia.Items.Add("--");
                            this.DropDownListColonia.Items.FindByText("--").Selected = true;
                            this.rfvDropDownListColonia.InitialValue = "--";

                            FiltroXCP oLocalidad = AdministradorCatalogos.ObtenerDetalleLocalidadPorCodigoPostal(this.TextBoxCP.Text.Trim().PadLeft(2, '0'));
                            this.DropDownListEdo.SelectedValue = oLocalidad.IdEstado.Value.ToString();
                            this.PoblarDropDownListMposXEntFed();
                            this.DropDownListMpo.SelectedValue = oLocalidad.IdMunicipio.Value.ToString();

                        }
                        else
                        {
                            this.TextBoxCP.Text = "";
                            this.DropDownListColonia.Items.Clear();
                            this.DropDownListColonia.DataSource = null;
                            this.DropDownListColonia.DataBind();
                            Msj = "El codigo postal es invalido, verifica";
                            this.LabelInfo.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                            MostrarMensajeJavaScript(Msj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.TextBoxCP.Text = "";
                this.DropDownListColonia.Items.Clear();
                this.DropDownListColonia.DataSource = null;
                this.DataBind();
                Msj = "No se ha podido recuperar la información del código postal. <br />Valida que el código postal exista o de lo contrario contacta al área de sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                MostrarMensajeJavaScript(Msj);

                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }
        }

        //obtener el sector de acuerdo a la institucion 25/08/2017
        private void PoblarDropDownListSectorxInstitucion()
        {
            try
            {
                this.DropDownSector.DataTextField = "Descripcion";
                this.DropDownSector.DataValueField = "IdValue";
                this.DropDownSector.DataSource = AdministradorCatalogos.ObtenerSectores(Convert.ToInt32(this.DropDownListInstitucion.SelectedValue));
                this.DropDownSector.DataBind();

                ////agregar un elemento para representar a todos
                //this.DropDownSector.Items.Add("--");
                //this.DropDownSector.Items.FindByText("--").Selected = true;
            }
            catch (Exception ex)
            {
                Msj = "Ha ocurrido un error en la craga de sectores. Contacta al área de sistemas.";
                //this.LabelInfoGridResult.Text = "<div class='alert alert-danger'> " + Msj + "</div>";
                //MostrarMensajeJavaScript(Msj);

                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",

                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }
        }

        protected void DropDownListInstitucion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownListInstitucion.SelectedItem.Text != "--")
                this.PoblarDropDownListSectorxInstitucion();
            else
            {
                //limpiar mpos porque se ha seleccionado que no se buscara por estado
                this.DropDownSector.DataSource = null;
                this.DropDownSector.DataBind();
                this.DropDownSector.Items.Clear();
            }
        }

        //habilita el campo de otra colonia si es seleccionada
        protected void DropDownListColonia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //habilitar la caja de texto para escribir la otra colonia
            if (this.DropDownListColonia.SelectedItem.Text == "- Otra colonia -")
            {
                this.TextBoxOtraColonia.Enabled = true;
                this.TextBoxOtraColonia.ToolTip = "Escribe la colonia si no se encuentra en la lista";
                this.TextBoxOtraColonia.Focus();
            }
            else
            {
                this.TextBoxOtraColonia.Enabled = false;
                this.TextBoxOtraColonia.ToolTip = "";
                this.TextBoxOtraColonia.Text = String.Empty;
                this.TextBoxCP.Text = this.DropDownListColonia.SelectedItem.Text.Split('-')[0].Trim();

                if (this.DropDownListColonia.SelectedItem.Text != "--")
                {
                    FiltroXCP oLocalidad = AdministradorCatalogos.ObtenerDetalleLocalidadPorCodigoPostal(this.TextBoxCP.Text.Trim().PadLeft(2, '0'));
                    this.DropDownListEdo.SelectedValue = oLocalidad.IdEstado.Value.ToString();
                    this.DropDownListMpo.SelectedValue = oLocalidad.IdMunicipio.Value.ToString();

                }
            }
        }

        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

        protected void ButtonEnviar_Click(object sender, EventArgs e)
        {
            if (this.ValidaEntradaDatos())
            {
                if (this.InsertarJustipreciacion())
                {
                    string mensaje = "La justipreciacion ha sido registrado con éxito y el archivo se a guardado con exito.";
                    this.LabelInfo.Text += "<div class='alert alert-success'><strong> ¡Felicidades! </strong></br>" + mensaje + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;

                    this.pnlControles.Enabled = false;
                    
                    this.ButtonEnviar.Enabled = false;
                    this.ButtonCancelar.Text = "Regresar";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "lanzaAlerta", "alert(\"" + mensaje + "\");", true);
                  
                    this.ButtonEnviar.Enabled = false;
                    
                }
            }//fin del if validardatos

        }

        private Boolean ValidaEntradaDatos()
        {

            //si esta vacio secuencial 
            if (this.txtFolioBPM.Text.Trim().Length == 0)
            {
                string mensaje = "Debes porporcionar un secuencial";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.txtFolioBPM.Focus();
                return false;
            }

            //si esta vacio generico
            if (this.TextBox1.Text.Trim().Length == 0)
            {
                string mensaje = "Debes porporcionar un generico";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBox1.Focus();
                return false;
            }

            //si esta vacio sector o no a seleccionado una 
            if ((this.DropDownSector.SelectedItem.Text == "--") || (this.DropDownSector.SelectedItem.Text == ""))
            {
                string mensaje = "Debes seleccionar un tipo de sector";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownSector.Focus();
                return false;
            }

            //si esta vacio la superficie de terreno dictaminado
            if (this.TextBoxSupDic.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir una cantidad";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxSupDic.Focus();
                return false;
            }

            //si esta vacio o contiene los -- la unidad de terreno dictaminado 
            if ((this.DropDownListUniTer.Text == "--") || (this.DropDownListUniTer.SelectedItem.Text == ""))
            {
                string mensaje = "Debes seleccionar un tipo de unidad";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListUniTer.Focus();
                return false;
            }


            //si esta vacio superficie contruida dictaminado
            if (this.TextBoxConsDic.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir una cantidad";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxConsDic.Focus();
                return false;
            }

            //si esta vacio o contiene los -- la unidad de construida  dictaminado 
            if ((this.DropDownListUniCons.Text == "--") || (this.DropDownListUniCons.SelectedItem.Text == ""))
            {
                string mensaje = "Debes seleccionar un tipo de unidad";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListUniCons.Focus();
                return false;
            }

            //si esta vacio la superficie rentable dictaminado
            if (this.TextBoxRenDic.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir una cantidad";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxRenDic.Focus();
                return false;
            }

            //si esta vacio o contiene los -- la unidad de superficie rentada dictaminado 
            if ((this.DropDownListRenDic.Text == "--") || (this.DropDownListRenDic.SelectedItem.Text == ""))
            {
                string mensaje = "Debes seleccionar un tipo de unidad";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListRenDic.Focus();
                return false;
            }

            //si esta vacio monto dictaminado
            if (this.TextBoxMOntoDic.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir un monto";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxMOntoDic.Focus();
                return false;
            }

            //si esta tiene algo en campon fecha 
            if (this.TextBoxFechaDictamen.Text.Length > 0)
            {
                if (Util.IsDate(TextBoxFechaDictamen.Text) == false)
                {
                    string mensaje = "Debes proporcionar una fecha de dictamen, valida en el formato (dd/mm/aaaa)";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxFechaDictamen.Focus();
                    return false;
                }
            }
            else
            {
                string mensaje = "Debes proporcionar una fecha de dictamen, valida en el formato (dd/mm/aaaa)";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxFechaDictamen.Focus();
                return false;
            }

            //si esta vacio la calle
            if (this.TextBoxCalle.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir una calle";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxCalle.Focus();
                return false;
            }

            //si esta vacio el numero exterior 
            if (this.TextBoxNumExt.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir un número exterior";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxNumExt.Focus();
                return false;
            }

           

            //si el municipio esta vacio o tiene -- que no a seleccionado nada 
            if ((this.DropDownListMpo.Text == "--") || (this.DropDownListMpo.SelectedItem.Text == ""))
            {
                string mensaje = "Debes seleccionar un municipio";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListMpo.Focus();
                return false;
            }

            //poner validacion que contenga los 5 numeros 
            //si esta vacio el codigo postal 
            if (this.TextBoxCP.Text.Trim().Length == 0)
            {
                string mensaje = "Debes introducir un código postal";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxCP.Focus();
                return false;
            }
            else if (this.TextBoxCP.Text.Trim().Length < 5)
            {

                string mensaje = "La longitud del código postal debe de ser 5 digitos.";
                this.LabelInfo.Text += "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.TextBoxCP.Focus();
                return false;
            }


            if (this.DropDownListColonia.Text == "Selecciona una localidad" || this.DropDownListColonia.Text == "--")
            {
                string mensaje = "Debes de seleccionar una colonia";
                this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                this.DropDownListColonia.Focus();
                return false;
            }
           
            //verificar si al seleccionar otra colonia se ponga algo dentro del campo de texto de otra colonia, ya que selecciono que dentro de la lista de colonias no se encontraba la que busca
            if (this.DropDownListColonia.Text == "- Otra colonia -")
            {
                if(this.TextBoxOtraColonia.Text.Trim().Length == 0)
                {
                    string mensaje = "Debes introducir una colonia en el campo de texto.";
                    this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.TextBoxOtraColonia.Focus();
                    return false;
                }
            }

            
            //validar la extension del archivo
            //si aun no han ingresado un documento 
            //valida si subieron algo si contiene un archivo
            if (FUJustipreciacion.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(FUJustipreciacion.FileName);
                if(fileExtension != ".pdf")
                {
                    
                    string mensaje = "el formato del archivo es incorrecto.";
                    this.LabelInfo.Text += "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    this.FUJustipreciacion.Focus();
                    return false;
                }
                
            }//fin de if FUJustipreciacion.HasFile
            else
            {
                //notifica que el archivo no fue cargado
                Msj = "No se especifico un archivo para cargar";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.FUJustipreciacion.Focus();
                return false;
            }

            return true;
        }

        //RCA 21/08/2017
        private Boolean InsertarJustipreciacion()
        {
            Boolean Ok = true;
            try
            {
                string strSecuencial = null;
                if (this.txtFolioBPM.Text.Length > 0)
                    strSecuencial = this.txtFolioBPM.Text.Trim();

                string strGenerico = null;
                if (this.TextBox1.Text.Length > 0)
                    strGenerico = this.TextBox1.Text.Trim();

                //me da un numero que es el idsector
                string strsector = null;
                if (this.DropDownSector.SelectedItem.Text != "")
                    strsector = this.DropDownSector.SelectedValue;

                //me da un numero que es el id de institucion 
                string strInstitucion = null;
                if (this.DropDownListInstitucion.SelectedItem.Text != "")
                    strInstitucion = this.DropDownListInstitucion.SelectedValue;

                decimal SupTerDic = 0;
                if (this.TextBoxSupDic.Text.Length > 0)
                    SupTerDic = Convert.ToDecimal(this.TextBoxSupDic.Text.Trim());

                string UnidadSupTerDic = null;
                if (this.DropDownListUniTer.SelectedItem.Text != "")
                    UnidadSupTerDic = this.DropDownListUniTer.SelectedValue;

                decimal SupConsDic = 0;
                if (this.TextBoxConsDic.Text.Length > 0)
                    SupConsDic = Convert.ToDecimal(this.TextBoxConsDic.Text.Trim());

                string UnidadSupConsDic = null;
                if (this.DropDownListUniCons.SelectedItem.Text != "")
                    UnidadSupConsDic = this.DropDownListUniCons.SelectedValue;

                decimal SupRenDic = 0;
                if (this.TextBoxRenDic.Text.Length > 0)
                    SupRenDic = Convert.ToDecimal(this.TextBoxRenDic.Text.Trim());

                string UnidadSupRenDic = null;
                if (this.DropDownListRenDic.SelectedItem.Text != "")
                    UnidadSupRenDic = this.DropDownListRenDic.SelectedValue;

                decimal MontoDic = 0;
                if (this.TextBoxMOntoDic.Text.Length > 0)
                    MontoDic = Convert.ToDecimal(this.TextBoxMOntoDic.Text.Trim());

                string UnidadResponsable = null;
                if (this.TextBoxUnidadrespon.Text.Length > 0)
                    UnidadResponsable = TextBoxUnidadrespon.Text.Trim();

                string Calle = null;
                if (this.TextBoxCalle.Text.Length > 0)
                    Calle = this.TextBoxCalle.Text.Trim();

                string NumExt = null;
                if (this.TextBoxNumExt.Text.Length > 0)
                    NumExt = this.TextBoxNumExt.Text.Trim();

                //no es necesario que este lleno este campo
                string NumInt = null;
                if (this.TextBoxNumInt.Text.Length > 0)
                    NumInt = this.TextBoxNumInt.Text.Trim();

                //me da un numero que es su id 
                string Estado = null;
                if (this.DropDownListEdo.SelectedItem.Text != "")
                    Estado = this.DropDownListEdo.SelectedValue;

                //me da un numero que es su id
                string Municipio = null;
                if (this.DropDownListMpo.SelectedItem.Text != "")
                    Municipio = this.DropDownListMpo.SelectedValue;

                string CP = null;
                if (this.TextBoxCP.Text.Length > 0)
                    CP = this.TextBoxCP.Text.Trim();

                //colonia
                string Colonia = null; 
                if (this.DropDownListColonia.SelectedItem.Text == "- Otra colonia -")
                {
                    if(this.TextBoxOtraColonia.Text.Length > 0)
                    {
                        Colonia = this.TextBoxOtraColonia.Text.Trim();
                    }
                  
                }
                else
                {
                    Colonia = this.DropDownListColonia.SelectedItem.Text;
                }
                
                
                string ruta = null;
                if (FUJustipreciacion.HasFile)
                    //llamo a un metodo para guardar el archivo 
                    ruta = SaveFile(FUJustipreciacion.PostedFile);

                int IdUser = ((SSO)Session["Contexto"]).IdUsuario.Value;

                if (!string.IsNullOrEmpty(ruta))
                {
                    ModeloNegocios.SolicitudAvaluosExt objJustipreciacion = new ModeloNegocios.SolicitudAvaluosExt();
                    //poblar propiedades
                    objJustipreciacion.NoSecuencial = strSecuencial;
                    objJustipreciacion.NoGenerico = strGenerico;
                    objJustipreciacion.SectorId = Convert.ToInt32(strsector);
                    objJustipreciacion.InstitucionId = Convert.ToInt32(strInstitucion);
                    objJustipreciacion.SuperficieTerrenoDictaminado = SupTerDic;
                    objJustipreciacion.UnidadMedidaTerrenoDictaminado = UnidadSupTerDic;
                    objJustipreciacion.SuperficieConstruidaDictaminado = SupConsDic;
                    objJustipreciacion.UnidadMedidaConstruidaDictaminado = UnidadSupConsDic;
                    objJustipreciacion.SuperficieRentableDictaminado = SupRenDic;
                    objJustipreciacion.UnidadMedidaRentableDictaminado = UnidadSupRenDic;
                    objJustipreciacion.MontoDictaminado = MontoDic;
                    objJustipreciacion.UnidadResponsable = UnidadResponsable;
                    objJustipreciacion.FechaDictamen = Convert.ToDateTime(this.TextBoxFechaDictamen.Text.Trim());//convertimos de string a dategtime
                    objJustipreciacion.Calle = Calle;
                    objJustipreciacion.NoExterior = NumExt;
                    objJustipreciacion.NoInterior = NumInt;
                    objJustipreciacion.ColoniaInmueble = Colonia;
                    objJustipreciacion.EstadoId = Convert.ToInt32(Estado);
                    objJustipreciacion.MunicipioId = Convert.ToInt32(Municipio);
                    objJustipreciacion.CP = CP;
                    objJustipreciacion.RutaDocumento = ruta;
                    objJustipreciacion.IdUsuarioRegistro = IdUser;

                    //se inserta la justipreciacion en el sistema
                    bool Correcto = new NegocioJustipreciacionExt().InsertarJustipreciacionAvaluos(objJustipreciacion);

                    if (Correcto == true)
                    {
                        objJustipreciacion = null;
                        Ok = true;
                    }
                    else
                    {
                        Msj = "No fue posible registrar la justipreciacion, por favor vuelva a intentar ó verifique si ya fue captura el secuencial.";
                        this.LabelInfo.Text = "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + Msj + "</div>";
                        this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                        MostrarMensajeJavaScript(Msj);
                        this.LabelInfoEnviar.Focus();
                        objJustipreciacion = null;
                        Ok = false;
                    }
                }//fin del if para validar si la ruta es diferente de null
                else
                {
                    string mensaje = "el tamaño del archivo es mayor de 900kb .";
                    this.LabelInfo.Text += "<div class='alert alert-warning'><strong> ¡Precaución! </strong> " + mensaje + "</div>";
                    this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                    return Ok = false;
                }
            }
            
            catch (System.Data.SqlClient.SqlException ex)
            {
                //TODO: implementar registro de Bitacora de Excepciones
                Msj = "Ocurrió una excepción al procesar el registro de la información de la justipreciacion, por favor vuelva a intentar ó reporte a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.LabelInfoEnviar.Focus();

                //Registra informacion acerca de una excepcion
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("o es posible") || ex.InnerException.Message.Contains("ya fue aplicado"))
                    {
                        Msj = ex.InnerException.Message;
                        this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                        MostrarMensajeJavaScript(Msj);
                        Ok = false;
                    }
                }

                //msj al usuario
                Msj = "Ocurrió una excepción al procesar el registro de la información de la justipreciacion, por favor vuelva a intentar ó reporte a Sistemas.";
                this.LabelInfo.Text = "<div class='alert alert-danger'><strong> Error </strong>" + Msj + "</div>";
                this.LabelInfoEnviar.Text = this.LabelInfo.Text;
                MostrarMensajeJavaScript(Msj);
                this.LabelInfoEnviar.Focus();

                //Registra informacion acerca de una excepcion
                BitacoraExcepcion BitacoraExcepcionAplictivo = new BitacoraExcepcion
                {
                    CadenaconexionBD = System.Configuration.ConfigurationManager.ConnectionStrings["cnArrendamientoInmueble"].ConnectionString,
                    Aplicacion = "ContratosArrto",
                    Modulo = MethodInfo.GetCurrentMethod().DeclaringType.ToString() + ".aspx",
                    Funcion = MethodBase.GetCurrentMethod().Name + "()",
                    DescExcepcion = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Usr = ((SSO)Session["Contexto"]).UserName.ToString()
                };
                BitacoraExcepcionAplictivo.RegistrarBitacoraExcepcion();
                BitacoraExcepcionAplictivo = null;
            }

            return Ok;
        }

        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Justipreciacion/ConsultaJustipreciacion.aspx");
        }

        public string SaveFile(HttpPostedFile file)
        {
            string SaveFile = string.Empty;

            //obtiene el tamaño del archivo
            int TamañoArchivo = file.ContentLength;

            if (TamañoArchivo > 0 && TamañoArchivo <= 31457280)
            {
                //especifica la ruta(path) donde se guardara el archivo
                //string savePath = "c:\\Users\\Desa03\\Desktop\\";
                string  savePath = ConfigurationManager.AppSettings["RutaDocs"];

                if (!string.IsNullOrEmpty(savePath))
                {
                    if (System.IO.Directory.Exists(savePath))
                    {

                        //obten el nombre del archivo que fue cargado
                        string fileName = FUJustipreciacion.FileName;
                        //string fileName = FUJustipreciacion.PostedFile.FileName;

                        //crea la ruta y el nombre de archivo para validar si existen duplicados
                        string pathToCheck = savePath + fileName;

                        //crea un archivo temporal para checar si exissten duplicados
                        string tempfileName = "";

                        //comprobar si ya existe un archivo con el mismo nombre del archivo que se va a cargar 
                        if (System.IO.File.Exists(pathToCheck))
                        {
                            int counter = 2;

                            while (System.IO.File.Exists(pathToCheck))
                            {
                                //si existe el nombre del archivo, este lo prefija con un numero 
                                tempfileName = "(" + counter.ToString() + ")" + fileName;
                                pathToCheck = savePath + tempfileName;
                                counter++;
                            }

                        }

                        //toma el valor de la ruta para pasarla a la base de datos
                        SaveFile = string.Format(@"{0}{1}", savePath, fileName);

                        //llama al metodo SaveAs para guardar el archivo en el directorio especificado 
                        try
                        {
                            FUJustipreciacion.SaveAs(SaveFile);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error al guardar archivo: {0}", ex.Message));
                        }

                    }// if del System.IO.Directory.Exists
                    else
                    {
                        // directorio no existe..
                        throw new Exception(string.Format("Error el directorio no existe: {0}", savePath));
                    }

                }//termina el if del IsNullOrEmpty
                else
                {
                    // valor de ruta invalida
                    throw new Exception(string.Format("Error la ruta es invalida: {0}", savePath));
                }

            }//fin del if de tamañoarchivo
            else
            {
                return SaveFile = null;
            }
            return SaveFile;
        }

    }
}