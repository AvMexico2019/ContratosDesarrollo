//Web Developer: Ricardo Rangel Ramírez
//Contacto: riccardorangel@hotmail.com   

using System.Web.UI.WebControls;
using System;

public class Email
{
    public static void NotificaPSC(string mail, string mensaje)
    {
        System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
        
        //------------------------------------------------------------------------------------
        //De
        string strDe = null;
        strDe = System.Configuration.ConfigurationManager.AppSettings["cuenta_privada"].ToString();
        correo.From = new System.Net.Mail.MailAddress(strDe);

        //------------------------------------------------------------------------------------
        //Para                
        correo.To.Add(mail);

        //------------------------------------------------------------------------------------
        //Datos del correo
        correo.Subject = "Notificación Automatizada de Fondo Creación";
        correo.Body = mensaje;

        correo.IsBodyHtml = false;
        correo.Priority = System.Net.Mail.MailPriority.Normal;

        //------------------------------------------------------------------------------------
        //Envio del correo
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

        //Servidor de correo
        smtp.Host = System.Configuration.ConfigurationManager.AppSettings["servidor_correo"].ToString();

        string n1 = System.Configuration.ConfigurationManager.AppSettings["netcredencial1"].ToString();
        string n2 = System.Configuration.ConfigurationManager.AppSettings["netcredencial2"].ToString();

        //Autenticacion
        smtp.Credentials = new System.Net.NetworkCredential(n1, n2);

        //Envialo
        try
        {
            smtp.Send(correo);
        }
        catch (Exception ex)
        {            
            throw new Exception("Se ha producido un error en el metodo Notifica", ex);
 
            //lblMsj.Text = ex.Message;
        }//try

    }//void

    public static bool ValidaFormatoEmail(String p_Email)
    {
        System.Text.RegularExpressions.Regex regexEmail = new System.Text.RegularExpressions.Regex(@"^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$");
        if (!string.IsNullOrEmpty(p_Email))
        {
            if (regexEmail.Match(p_Email).Success == false)
            {
                return false;
            }
            else
            {
                return true;
            }//end if
        }
        else
        {
            return true;
        } //end if

    }//end method        

}//end class