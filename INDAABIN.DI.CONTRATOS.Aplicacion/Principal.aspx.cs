using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//
using System.Configuration;
using INDAABIN.DI.ModeloNegocio; //bus
using INDAABIN.DI.CONTRATOS.Negocio;

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    /// <summary>
    ///Proposito: Carga la session:   Session["Contexto"]  
    ///que contiene el perfil del Usuario que se autentifica del SSO
    ///un usuario del SSO, contiene una lista de Aplicaciones que a su vez contiene una lista de Roles (aunque solo debe tener un rol por aplicacion)
    /// </summary>
    public partial class Principal : System.Web.UI.Page
    {

        string UserName;
        string Token;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = string.Empty;
            Token = string.Empty;

            if (!IsPostBack)
            {
                ColocarURLCierreSession();

                if (InterconectarCredencialesConSSO() == false) //SSO real-interconexion al BUS
                //if( this.SimularUserAdminContratos_AppSSO() == false) //simualacionSSO-desconectado de Admin
                //if  (SimularUserOIC_AppSSO() == false)
                //if (this.SimularUserPromovente_AppSSO() == false)  //simualacionSSO--desconectado de Promovente
                {
                    if (Session["Msj"] != null)
                    {
                        Response.Redirect("~/Msj.aspx", false);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "sesCorrecta", "sesiónCorrecta();", true);
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), "valExplorador", "ValidaExplorador();", true);
            }
        }

        private void ColocarURLCierreSession()
        {
            try
            {
                string valor = ConfigurationManager.AppSettings.Get("URL_SSO");
                HyperLinkCerrarSession.NavigateUrl = valor;
            }

            catch (Exception ex)
            {
                string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Session["Msj"] = msj;
                Response.Redirect("Msj.aspx", false);
            }
        }

        //interconexion con el BUS
        private bool InterconectarCredencialesConSSO()
        {
            UserName = Request.QueryString["UserName"];
            Token = Request.QueryString["Token"];

            if (Request.QueryString["DebugMode"] != null)
            {
                //UserName = "organocontrolinterno01@correo.gob.mx";
                //Token = "organocontrolinterno01";

                //UserName = "promovente01@correo.gob.mx";
                //Token = "promovente01";

                //UserName = "administrador01@correo.gob.mx";
                //Token = "administrador01"; 

                //UserName = "desa11";
                //Token = "desa11";

                //UserName = "desa14";
                //Token = "desa14";

                //UserName = "vruiz@inami.gob.mx";
                //Token = "vruiz@inami.gob.mx";

                UserName = "desa03";
                Token = "desa03";

                //UserName = "genguntza";
                //Token = "genguntza"; 

                //UserName = "desa10";
                //Token = "desa10";

                //UserName = "desa04";
                //Token = "desa04";
            }

            Boolean Ok = false;

            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Token))
            {
                redireccionSSO();
            }
            else
            {
                ////interconexion con el BUS para ejecutar metodo
                SSO usuario = null;
                try
                {
                    usuario = new NG().ObtenerUsuario(UserName);
                    Session["Contexto"] = usuario;

                    if (usuario != null)
                    {
                        String RolUsr = UtilContratosArrto.ObtenerNombreRolUsrApp(usuario.LRol);
                        //if (RolUsr == UtilContratosArrto.Roles.OIC.ToString())
                        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "soloLectura", "ValidarSoloLectura('none');", true);
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "soloLectura", "ValidarSoloLectura('none');", true);
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), "alertUsuario", "alert('No se ha podido recuperar el objeto Contexto del usuario firmado, contacte al administrador del sistema.');", true);
                    }                
                    Ok = true;
                }
                catch (Exception ex)
                {
                    string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    Session["Msj"] = msj;
                    Response.Redirect("Msj.aspx", false);
                }
            }
            return Ok;
        }

        private void redireccionSSO()
        {
            string valor = ConfigurationManager.AppSettings.Get("URL_SSO");
            string aplicacion = ConfigurationManager.AppSettings.Get("TokenApp");
            try
            {
                if (valor == null || aplicacion == null)
                {
                    // throw new InvalidOperationException("Error en la obtención de los parametros de sesión");
                    Session["Msj"] = "Error en la obtención de los parametros de sesión del SSO, por favor reporte al área de Sistemas.";
                    Response.Redirect("Msj.aspx");

                }
            }
            catch (Exception ex)
            {
                string msj = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                Session["Msj"] = msj;
                Response.Redirect("Msj.aspx", false);
            }

            Response.Redirect(valor + "?IDApplication=" + aplicacion.Replace("-", ""));
            //Response.Redirect(valor + aplicacion.Replace("-", ""));
        }


        //simular un objeto Contexto del usuario: SSO (desconestado de BUS)
        //Usuario-->lista de Aplciacion --> Lsita de Roles de Aplicacion
        private Boolean SimularUserPromovente_AppSSO()
        {
            SSO UsrHardCode = null;
            Boolean Ok = false;

            //crear una lista de roles con un objeto Rol en especifico
            List<ModeloNegocio.Rol> listRolesApp = new List<ModeloNegocio.Rol>()
                                                    {new ModeloNegocio.Rol()
                                                        {
                                                           idRol = 2, nombreRol = "Promovente"
                                                           
                                                        }
                                                    };

            //Crear una lista de Aplicaciones con un objeto de Aplicacion
            //agregar la lista de roles a la lista de aplicacion
            List<ModeloNegocio.Aplicacion> listAplicaciones = new List<ModeloNegocio.Aplicacion>()
                         {new  ModeloNegocio.Aplicacion()
                               { IdAplicacion = 1, NombreAplicacion = "ArrendamientoInmueble", LRol=listRolesApp  }
                         };


            //****   hardcode de simulacion de usuario SSO   *******
            //se agregan 2 listas al usuario: la lista de aplicaciones y de roles
            UsrHardCode = new SSO()
            {

                ////**** Promovente  *****
                IdUsuario = 30546,
                UserName = "desa25",
                Nombre = "Edwin",
                ApellidoP = "Serrano",
                ApellidoM = "Aguado",
                Cargo = "DIRECTOR DE INFORMATICA", //real del catalogo de: Cargos= idCargo= 49
                //IdInstitucion =72,
                //IdInstitucion = 76,
                //IdInstitucion = 318,
                IdInstitucion = 135,
                //NombreInstitucion = "COMISIÓN FEDERAL DE TELECOMUNICACIONES", //=72 (para pruebas de Contiuancion o Sustitucion)
                //NombreInstitucion = "PRESIDENCIA DE LA REPÚBLICA", //IdInstitucion = 1
                // NombreInstitucion = "DELEGACIÓN FEDERAL DEL TRABAJO", //IdInstitucion = 387
                // NombreInstitucion = "PROCURADURÍA GENERAL DE LA REPÚBLICA", //IdInstitucion = 2,
                //NombreInstitucion = "COMISIÓN FEDERAL DE ELECTRICIDAD" , //idInstitucion = 318  (para comprobar el secuencial: 4372)
                NombreInstitucion = "LUZ Y FUERZA DEL CENTRO, EN LIQUIDACIÓN", //135
                //NombreInstitucion = "SECRETARÍA DE LA DEFENSA NACIONAL", ////IdInstitucion = 76
                // NombreSector = "PRESIDENCIA DE LA REPÚBLICA",
                LAplicaciones = listAplicaciones,
                LRol = listRolesApp

            };

            Session["Contexto"] = UsrHardCode;
            Ok = true;

            return Ok;
        }

        //(desconestado de BUS) para sumular un usuario con rol de Administrador de Contratos
        private Boolean SimularUserAdminContratos_AppSSO()
        {
            SSO UsrHardCode = null;
            UserName = "fvaldez@funcionpublica.gob.mx";
            Token = "XXXXX";
            Boolean Ok = false;


            //crear una lista de roles con un objeto Rol en especifico
            List<ModeloNegocio.Rol> listRolesApp = new List<ModeloNegocio.Rol>()
                                                    {new ModeloNegocio.Rol()
                                                        {
                                                            //idRol = 0, nombreRol = "Administrador"
                                                            idRol = 1, nombreRol = "Administrador de Contratos"
                                                            //idRol = 2, nombreRol = "Promovente"
                                                            //idRol = 3, nombreRol = "OIC"
                                                        }
                                                    };

            //Crear una lista de Aplicaciones con un objeto de Aplicacion
            //agregar la lista de roles a la lista de aplicacion
            List<ModeloNegocio.Aplicacion> listAplicaciones = new List<ModeloNegocio.Aplicacion>()
                         {new  ModeloNegocio.Aplicacion()
                               { IdAplicacion = 1, NombreAplicacion = "ArrendamientoInmueble", LRol=listRolesApp  }
                         };


            //****   hardcode de simulacion de usuario SSO   *******
            //se agregan 2 listas al usuario: la lista de aplicaciones y de roles
            UsrHardCode = new SSO()
            {

                ////**** Administrador de Contratos  ****
                IdUsuario = 11940,
                UserName = "fvaldez@funcionpublica.gob.mx",
                Nombre = "FERNANDO",
                ApellidoP = "VALDES",
                ApellidoM = "LUCIO",
                Cargo = "Subdirector de Programas Inmobiliarios", //IdCargo=45
                IdInstitucion = 74,
                NombreInstitucion = "INSTITUTO DE ADMINISTRACIÓN Y AVALÚOS DE BIENES NACIONALES",
                LAplicaciones = listAplicaciones,
                LRol = listRolesApp

            };

            Session["Contexto"] = UsrHardCode;
            Ok = true;

            return Ok;
        }


        //simular un objeto Contexto del usuario: SSO (desconestado de BUS)
        //Usuario-->lista de Aplicacion --> Lsita de Roles de Aplicacion
        private Boolean SimularUserOIC_AppSSO()
        {
            SSO UsrHardCode = null;
            Boolean Ok = false;

            //crear una lista de roles con un objeto Rol en especifico
            List<ModeloNegocio.Rol> listRolesApp = new List<ModeloNegocio.Rol>()
                                                    {new ModeloNegocio.Rol()
                                                        {
                                                           idRol = 3, nombreRol = "OIC"
                                                           
                                                        }
                                                    };

            //Crear una lista de Aplicaciones con un objeto de Aplicacion
            //agregar la lista de roles a la lista de aplicacion
            List<ModeloNegocio.Aplicacion> listAplicaciones = new List<ModeloNegocio.Aplicacion>()
                         {new  ModeloNegocio.Aplicacion()
                               { IdAplicacion = 1, NombreAplicacion = "ArrendamientoInmueble", LRol=listRolesApp  }
                         };


            //****   hardcode de simulacion de usuario SSO   *******
            //se agregan 2 listas al usuario: la lista de aplicaciones y de roles
            UsrHardCode = new SSO()
            {

                //***OIC * ***
                IdUsuario = 4,
                UserName = "desa25",
                Nombre = "Jose Luis",
                ApellidoP = "Piña",
                ApellidoM = "Lara",
                Cargo = "Coordinador de Desarrollo Institucional",
                IdInstitucion = 1,
                NombreSector = "PRESIDENCIA DE LA REPÚBLICA",
                NombreInstitucion = "PRESIDENCIA DE LA REPÚBLICA",
                LAplicaciones = listAplicaciones,
                LRol = listRolesApp

            };

            Session["Contexto"] = UsrHardCode;
            Ok = true;

            return Ok;
        }


        private void MostrarMensajeJavaScript(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", "alert(\"" + mensaje + "\");", true);
        }

    }//class
}

