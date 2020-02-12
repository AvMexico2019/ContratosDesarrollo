//using INDAABIN.DI.BPM.CorreoElectronico.Negocio;
using INDAABIN.DI.ModeloNegocio;

using System.Reflection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using INDAABIN.DI.CONTRATOS.Negocio;

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public class EnviarCorreo
    {
        public bool Exito = false;

        public string Error { get; set; }

          

        //public void EnviaCorreo(string pEmialPara, string pAsunto, string pMsj)
        //{

        //    Boolean Ok = false;
        //    System.Threading.Thread hilo = null;
            
        //    //objeto para valores de quien envia el email
        //    Remitente confRemitente = new Remitente();

        //    //destinatario de email
        //    Correo confCorreo = new Correo();
        //    confCorreo.Destinatarios = pEmialPara;
        //    confCorreo.Asunto = pAsunto;
        //    confCorreo.Mensaje = pMsj;

        //    //obtener valores necesarios del remitente en el webConfig de esta aplicacion
        //    try
        //    {
        //        confRemitente = ObtenerConfiguracionRemitente();
        //        Ok = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //UtileriasGenerales.GuardarBitacora(ex,  "Error al enviar el correo",null);
        //        throw new Exception(ex.Message + " Excepción generada desde: " + MethodInfo.GetCurrentMethod().DeclaringType.ToString());
               
        //    }

        //    //si, se obtuvieron los valores del remitente, entonces hacer el envio
        //    if (Ok)
        //    {
        //        Dictionary<string, object> valores = new Dictionary<string, object>();
        //        valores.Add("Remitente", confRemitente);
        //        valores.Add("Correo", confCorreo);


        //        try
        //        {

        //            //ejecutar el envio del email con un hilo
        //            hilo = new System.Threading.Thread(Envia);
        //            hilo.Start(valores);
        //            Exito = true;
                   
        //        }
        //        catch (Exception ex)
        //        {
        //            //UtileriasGenerales.GuardarBitacora(ex,  "Error al enviar el correo",null);
        //            throw new Exception(ex.Message + " Excepción generada desde: " + MethodInfo.GetCurrentMethod().DeclaringType.ToString());
               
        //        }
               
        //    }
        //}



        //private Remitente ObtenerConfiguracionRemitente()
        //{
        //    Remitente confRemitente = new Remitente();

        //    try
        //    {
        //        string server = ConfigurationManager.AppSettings["Mail_Server"] == null ? string.Empty : ConfigurationManager.AppSettings["Mail_Server"];
        //        string puerto = ConfigurationManager.AppSettings["Mail_Port"] == null ? string.Empty : ConfigurationManager.AppSettings["Mail_Port"];
        //        string dirMail = ConfigurationManager.AppSettings["Mail_Email"] == null ? string.Empty : ConfigurationManager.AppSettings["Mail_Email"];
        //        string user = ConfigurationManager.AppSettings["Mail_User"] == null ? string.Empty : ConfigurationManager.AppSettings["Mail_User"];
        //        string pass = ConfigurationManager.AppSettings["Mail_Pass"] == null ? string.Empty : ConfigurationManager.AppSettings["Mail_Pass"];
        //        int ipuerto = 0;



        //        if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(dirMail) || string.IsNullOrEmpty(user)
        //      || string.IsNullOrEmpty(pass))
        //        {
        //            Error = "Faltan datos de configuración del correo";
        //            throw new InvalidOperationException("Error al obtener los datos del remitente");
        //        }
        //        int.TryParse(puerto, out ipuerto);

        //        confRemitente.ServidorSMTP = server;
        //        if (ipuerto > 0)
        //            confRemitente.Puerto = ipuerto;
        //        confRemitente.CorreoOrigen = dirMail;
        //        confRemitente.Usuario = user;
        //        confRemitente.Contraseña = pass;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return confRemitente;

        //}

        //private void Envia(object parametros)
        //{
        //    EnvioCorreo oEnvio = new EnvioCorreo();

        //    object valorRemitente = null;
        //    object valorCorreo = null;
        //    object solicitud = null; 
           
        //    try
        //    {
        //        Dictionary<string, object> parametro = (Dictionary<string, object>)parametros;

        //        parametro.TryGetValue("Correo", out valorCorreo);
        //        parametro.TryGetValue("Remitente", out valorRemitente);
                
        //        parametro.TryGetValue("FolioSolicitud", out solicitud);

        //        Correo confCorreo = (Correo)valorCorreo;
        //        Remitente remitente = (Remitente)valorRemitente;

        //        oEnvio.EnviarCorreoElectronico(confCorreo, remitente);

        //       // new DALC_Bitacora().GuardarBitacoraCorreo(solicitud.ToString(), true, confCorreo.Destinatarios, string.Format("Se envío correo {0}", confCorreo.Asunto));
        //        //UtileriasGenerales.GuardarBitacora(null, null, string.Format("Se envío el correo electrónico al destinatario {0}", confCorreo.Destinatarios));
        //    }
        //    catch (Exception ex)
        //    {
        //        //UtileriasGenerales.GuardarBitacora(ex,  "Error al enviar el correo",null);
        //        throw new Exception(ex.Message +  " Excepción generada desde: " + MethodInfo.GetCurrentMethod().DeclaringType.ToString());
                
        //    }

        //    oEnvio = null;
        //}


        /// <summary>
        /// Función de envío de correo por el documento o bien dada una configuración
        /// </summary>
        /// <param name="folio">Folio del documento</param>
        /// <param name="IdDocumento">Clave del documento</param>
        /// <param name="IdConfiguracion">Clave de la configuración</param>
        /// <param name="archivos">Lista de Archivos</param>
        /// <returns></returns>
        /// 
        //public bool EnviaCorreo(string folio, int? IdDocumento, int? IdConfiguracion, List<Archivo> archivos)
        //public bool EnviaCorreo_Lulu()
        //{

        //    System.Threading.Thread hilo = null;

        //    Remitente confRemitente = new Remitente();
        //    Correo confCorreo = new Correo();
        //    try
        //    {
        //        confRemitente = ObtenerConfiguracionRemitente();

        //        //  confCorreo = ObtenerConfiguracionCorreo(folio, IdConfiguracion, IdDocumento);
        //        //if (archivos != null && archivos.Count() > 0)
        //        //    AdjuntarArchivos(confCorreo, archivos);

        //        Dictionary<string, object> valores = new Dictionary<string, object>();

        //        valores.Add("Remitente", confRemitente);
        //        valores.Add("Correo", confCorreo);
        //        // valores.Add("FolioSolicitud", folio);


        //        hilo = new System.Threading.Thread(Envia);
        //        hilo.Start(valores);


        //    }
        //    catch (Exception ex)
        //    {
        //        Error = ex.Message;
        //        //    UtileriasGenerales.GuardarBitacora(ex, "Error al enviar el correo", null);
        //        return false;
        //    }
        //    return true;
        //}

    }//clase
}
