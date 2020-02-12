using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Security.Cryptography;

//uso del BUS
using INDAABIN.DI.ModeloNegocio; //para uso del BUS
using INDAABIN.DI.CONTRATOS.Negocio;

using ZXing;
using ZXing.Common;
using System.Configuration;


namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public class RolUsr
    {

        public int IdRol { get; set; }
        public string NombreRol { get; set; }


    }

    public static class UtilContratosArrto
    {

      
        //los nombres de los roles, se asignan su valor con el correspondiente id en el SSO
        public enum Roles
        {
            Administrador = 0, //para sistemas, pero no se creado en el SSO, porque no se ha descubierto alguna fucnionalidad particular al rol
            AdministradorDeContratos = 4328,
            Promovente = 4327,
            OIC = 4329,
            PromoventeLectura=4331

        };

        //recibe el Nombre del Rol y el objeto SSO
        public static bool ValidarRolAcceso(string sRol, SSO objSSO)
        {

            int intTieneElRol = 0;


            if (objSSO != null)
            {

                //validar acceso por Rol del usuario
                List<Rol> ListRoles = objSSO.LRol;
                intTieneElRol = (from x in ListRoles
                                 where x.nombreRol == sRol
                                 select x.idRol).Count();

            }
            else
                //redireccionar al SSO para autenticar
               
               throw new Exception("Para poder evaluar el Rol del usuario debe autentificarse 1ro en el SSO");


            return Convert.ToBoolean(intTieneElRol);

        }


        //recibe el Id del Rol y el objeto SSO
        public static bool ValidarRolAcceso(int IdRol, SSO objSSO)
        {

            int intTieneElRol = 0;


            if (objSSO != null)
            {

                //validar acceso por Rol del usuario
                List<Rol> ListRoles = objSSO.LRol;
                intTieneElRol = (from x in ListRoles
                                 where x.idRol == IdRol
                                 select x.idRol).Count();

            }
            else
                throw new Exception("Para poder evaluar el Rol del usuario debe autentificarse 1ro en el SSO");


            return Convert.ToBoolean(intTieneElRol);

        }


        //SobreCargado con una lista de Roles
        //Compara 2 listas una con los roles a buscar y la otra con la lista de roles del usuario
        public static bool ValidarRolAcceso(List<RolUsr> ListaRolesXBusar, List<Rol> ListRolesUsrSSO)
        {

            int intTieneElRol = 0;


            if (ListRolesUsrSSO != null)
            {

                //join entre list Roles de usuario SSO vs ListaRoles Comparacion
                intTieneElRol = (from Item1 in ListRolesUsrSSO
                                 join Item2 in ListaRolesXBusar
                                 on Item1.idRol equals Item2.IdRol // join on some property
                                 select new { Item1, Item2 }).Count();



            }
            else
                throw new Exception("Para poder evaluar el Rol del usuario debe autentificarse 1ro en el SSO");


            return Convert.ToBoolean(intTieneElRol);

        }

        //SobreCargado con una lista de Roles
        //Compara 2 listas una con los roles a buscar y la otra con la lista de roles del usuario
        public static bool ValidarDescRolAcceso(List<RolUsr> ListaRolesXBusar, SSO objSSO)
        {

            int intTieneElRol = 0;


            //obtener de la lista de aplicaciones, el objeto Aplicacion que corresponda a este aplicativo
            ModeloNegocio.Aplicacion AppContratos = (from App in objSSO.LAplicaciones
                                    where App.NombreAplicacion == "ArrendamientoContratos"
                                    select App).FirstOrDefault();

            //del objeto Aplicacion contiene una propiedad que es una lista de Roles, que se comparara con los roles buscados
            //join entre list Roles de usuario SSO vs ListaRoles Comparacion
            intTieneElRol = (from Item1 in AppContratos.LRol
                             join Item2 in ListaRolesXBusar
                                 //como se compara contra valores de string, se quitan los espacios
                             on Item1.nombreRol.Replace(" ", "") equals Item2.NombreRol.Replace(" ", "") // join on some property 
                             select new { Item1, Item2 }).Count();


            if (objSSO != null)
            {

                //join entre list Roles de usuario SSO vs ListaRoles Comparacion
                intTieneElRol = (from Item1 in objSSO.LRol
                                 join Item2 in ListaRolesXBusar
                                 on Item1.nombreRol equals Item2.NombreRol // join on some property 
                                 select new { Item1, Item2 }).Count();



            }
            else
                throw new Exception("Para poder evaluar el Rol del usuario debe autentificarse 1ro en el SSO");


            return Convert.ToBoolean(intTieneElRol);

        }

        //obtener el Rol que tiene el usuario en la aplicacion
        public static string ObtenerRolApp(List<RolUsr> ListaRolesXBuscar, List<Rol> ListRolesUsrSSO)
        {

            string Rol;


            if (ListRolesUsrSSO != null)
            {

                //join entre list Roles de usuario SSO vs ListaRoles Comparacion
                Rol = (from Item1 in ListRolesUsrSSO
                       join Item2 in ListaRolesXBuscar
                       on Item1.idRol equals Item2.IdRol // join on some property
                       select Item1.nombreRol).Last();


            }
            else
                throw new Exception("Para poder evaluar el Rol del usuario debe autentificarse 1ro en el SSO");


            return Rol;

        }

        //se parte de que un usuario solo puede tener un rol
        public static string ObtenerNombreRolUsrApp(List<Rol> ListRolesUsrSSO)
        {
            string Rol = string.Empty;

            if (ListRolesUsrSSO != null)
            {
                var sso = ListRolesUsrSSO.Where(x => x.nombreRol.Equals("ADMINISTRADOR", StringComparison.InvariantCultureIgnoreCase) || x.nombreRol.Equals("ADMINISTRADOR DE CONTRATOS", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (sso != null)
                {
                    Rol = sso.nombreRol;
                }
                else
                {
                    Rol = (from Item1 in ListRolesUsrSSO
                           select Item1.nombreRol).Last();
                }

            }
            else
                throw new Exception("Para poder evaluar el Rol del usuario debe autentificarse 1ro en el SSO");

            return Rol;

        }


        public static string Encrypt(string toEncrypt, bool useHashing, string strkey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            // Get the key from config file

            string key = strkey;

            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing, string strkey)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            string key = strkey;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        //Recibe el Id de la  entidad federativa del inmueble
        //y el consecutivo que es el número máximo de inmuebles en dicho estado
        //para el caso del RIUF el consecutivo es: 99999
        public static Int32 Digitoverificador(Int32 edo, Int32 consecu)
        {
            try
            {
                Int32 digito = new Int32();

                Int32 septimo = new Int32();
                Int32 sexto = new Int32();
                Int32 quinto = new Int32();
                Int32 cuarto = new Int32();
                Int32 tercero = new Int32();

                Int32 segundo = new Int32();
                Int32 primero = new Int32();

                septimo = edo / 10;
                septimo = septimo * 7;

                sexto = edo % 10;
                sexto = sexto * 6;


                quinto = consecu / 10000;
                consecu = consecu - (quinto * 10000);
                quinto = quinto * 5;


                cuarto = consecu / 1000;
                consecu = consecu - (cuarto * 1000);
                cuarto = cuarto * 4;

                tercero = consecu / 100;
                consecu = consecu - (tercero * 100);
                tercero = tercero * 3;

                segundo = consecu / 10;
                consecu = consecu - (segundo * 10);
                segundo = segundo * 2;


                primero = consecu / 1;

                digito = septimo + sexto + quinto + cuarto + tercero + segundo + primero;
                digito = digito % 11;

                if (digito == 0 || digito == 1)
                {
                    digito = 0;
                }
                else
                {
                    digito = 11 - digito;
                }

                return digito;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //RCA 09/08/2018
        //metodo para generar el QR
        public static String GenerarCodigoQR(string Folio,int TipoDocumento,string Username, string LigaDescargaQR)
        {
            //out string URL
            string imagenQR = "<img border=\"2\" src=\"data:image/png;base64, ";

            string UrlAbrirQR = ConfigurationManager.AppSettings["URLQR"];

            //string URLDescarga = "Folio=" + Folio + "&UserName=" + Username + "&TipoDocumento=" + TipoDocumento;

            //INDAABIN.DI.Cifrado.Cifrado Cifrador = new Cifrado.Cifrado();

             //URLDescarga = LigaDescargaQR + Cifrador.Cifrar(URLDescarga);

            string URLDescarga = string.Empty;

            //de acuerdo al tipo se concatena la carpeta 

            if(TipoDocumento == 1)
            {
                URLDescarga = UrlAbrirQR + "TablaSMOI/" + LigaDescargaQR;
            }

            if(TipoDocumento == 2)
            {
                URLDescarga = UrlAbrirQR + "EmisionOpinion/" + LigaDescargaQR;
                
            }

            if(TipoDocumento == 3)
            {
                string URLDescargSAEF = "Folio=" + Folio + "&UserName=" + Username + "&TipoDocumento=" + TipoDocumento;

                INDAABIN.DI.Cifrado.Cifrado Cifrador = new Cifrado.Cifrado();

                URLDescarga = LigaDescargaQR + "QR/VistaQR.aspx?data=" + Cifrador.Cifrar(URLDescargSAEF);
            }

            if(TipoDocumento == 4)
            {
                URLDescarga = UrlAbrirQR + "Contrato/" + LigaDescargaQR;
            }

            if(TipoDocumento == 5)
            {
                URLDescarga = UrlAbrirQR + "Contrato/" + LigaDescargaQR;
            }

            if (TipoDocumento == 6)
            {
                URLDescarga = LigaDescargaQR;
            }

            var writer = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions()
                {
                    Height = 180,
                    Width = 180,
                    Margin = 1, // el margen que tendrá el código con el resto de la imagen
                },
            };

            System.Drawing.Bitmap bitmap = writer.Write(URLDescarga);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage);

            imagenQR = imagenQR + SigBase64 + "\">";



            return imagenQR;
       
        }



    }//clase
}