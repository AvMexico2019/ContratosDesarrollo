using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
//using elGuille;


public static class Util
{

    //Recibe un numero string con formato y lo convierte a decimal
    //ToDecimal("$258,222.00")
    //salida= 258222 (le quita el formato)
    static public decimal ToDecimal(string Value)
    {
        if (Value.Length == 0)
            return 0;
        else
        {


            //quitar el formato
            return Decimal.Parse(Value.Replace(" ", ""), NumberStyles.AllowThousands
            | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowLeadingSign);

            /* --se quito esta comprobacion porque no funciona y no tiene sentido
                decimal valor; //variable para leer el número
                bool OkConversion; //esta nos dice si es un número válido

                OkConversion = decimal.TryParse(Value, out valor); //almacenamos true si la conversión se realizó
                if (OkConversion) //este if es solo para mostrar un mensaje de error si no se realizó la conversión
                    return valor;
            */

        }
    }

    public static string FormateaFechaAñoMesDia(string Fecha)
    {

        String FechaAñoMesDia = "";

        if ((Fecha.Length == 10) && IsDate(Fecha))
        {
            FechaAñoMesDia = FechaAñoMesDia + Fecha.Substring(6, 4);//año
            FechaAñoMesDia = FechaAñoMesDia + "/" + Fecha.Substring(3, 2);//mes
            FechaAñoMesDia = FechaAñoMesDia + "/" + Fecha.Substring(0, 2);//dia
        }
        else
            throw new Exception("No es una fecha con el formato dd/mm/aaaa");

        return FechaAñoMesDia;

    }

    //uso: bool result = Util.IsNumeric("123");  2e1 si es nuemeric, solo que expresado como exponencial
    public static bool IsNumeric(object Expression)
    {
        bool isNum;
        //double retNum;

        isNum = Information.IsNumeric(Expression);//validar con funcion de VB.NET

        //isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    public static bool IsEnteroNatural(object Expression)
    {
        bool EsEntero;
        int valido;
        EsEntero = int.TryParse(Convert.ToString(Expression), out valido);

        return EsEntero;


    }

    /// <summary>
    /// Comprueba si el valor es una fecha.
    /// </summary>
    /// <param name="cadenaFecha"></param>
    /// <returns>Se comprueba en la Cultura es-MX (Mexico).</returns>
    static public Boolean IsDate(string cadenaFecha)
    {
        CultureInfo es = new CultureInfo("es-MX");//formato cultural
        DateTime fechaTemp;

        return DateTime.TryParseExact(cadenaFecha, "dd/MM/yyyy", es, DateTimeStyles.AdjustToUniversal, out fechaTemp);
    }

    /// <summary>
    /// Convierte una cadena en fecha. Si falla en la conversión devuelve la fecha actual.
    /// </summary>
    /// <param name="fecha"></param>
    /// <returns></returns>
    static public DateTime convierteEnFecha(string fecha)
    {
        CultureInfo es = new CultureInfo("es-MX");
        DateTime fechaTemp = DateTime.Now;

        if (DateTime.TryParseExact(fecha, "dd/MM/yyyy", es, DateTimeStyles.None, out fechaTemp) == false)
        { fechaTemp = DateTime.Now; }

        return fechaTemp;
    }

    static public Boolean EnviaEmail(string pEmailPara, string pSuject, string pBody)
    {

        Boolean EmailEnviadoOK = false;
        System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();

        //------------------------------------------------------------------------------------
        //De
        string strDe = null;
        strDe = System.Configuration.ConfigurationManager.AppSettings["cuenta_privada"].ToString();
        correo.From = new System.Net.Mail.MailAddress(strDe);

        //------------------------------------------------------------------------------------
        //Para
        //correo.To.Add(oNotificacion.Correo);
        correo.To.Add(pEmailPara);

        //------------------------------------------------------------------------------------
        //Datos del correo
        correo.Subject = pSuject;
        correo.Body = pBody;

        correo.IsBodyHtml = false;
        correo.Priority = System.Net.Mail.MailPriority.Normal;

        //------------------------------------------------------------------------------------
        //Envio del correo
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

        //Servidor de correo
        smtp.Host = System.Configuration.ConfigurationManager.AppSettings["servidor_correo"].ToString();
        //puerto designado para envio de email. Nota: puede variar dependiendo del proveedor de Servicios de Internet.
        smtp.Port = 587;

        string n1 = System.Configuration.ConfigurationManager.AppSettings["usuario"].ToString();
        string n2 = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();

        //Autenticacion
        smtp.Credentials = new System.Net.NetworkCredential(n1, n2);

        //Envialo
        try
        {

            smtp.Send(correo);
            EmailEnviadoOK = true;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);

        }//try

        return EmailEnviadoOK;
    }

    //sepera de 4 en 4 con un guion
    static public string SeparaCadenaConGuiones(String pCadenaASeparar)
    {
        //string cadena = this.TextBoxCadena.Text;
        string subcadena, cadenaFinal = "";
        int tamaño = pCadenaASeparar.Length;
        //cortes cada 4 carateres.
        int VecesCabeSeparador4pos = tamaño / 4;
        byte CorteInicial = 0;


        while (VecesCabeSeparador4pos >= 0)
        {

            if (VecesCabeSeparador4pos == 0)//tomar lo que queda de la cadena
                                            //substraer toda la cadena.
                subcadena = pCadenaASeparar.Substring(CorteInicial, pCadenaASeparar.Length - CorteInicial);
            else
                //substraer 4 caracteres de la cadena.
                subcadena = pCadenaASeparar.Substring(CorteInicial, 4);


            //agrgar guion a la cadena cortada.
            cadenaFinal = cadenaFinal + "-" + subcadena;


            //aumentar el cortador
            CorteInicial = Convert.ToByte(CorteInicial + 4);
            //disminuir el contador de iteracciones.
            VecesCabeSeparador4pos--;

        }

        //quitar el 1er caracter que es un guion no valido.
        return cadenaFinal.Substring(1, cadenaFinal.Length - 1);
    }

    //separa de 4 en 4 con un espacio
    static public string SeparaCadenaConEspacio(String pCadenaASeparar)
    {
        //string cadena = this.TextBoxCadena.Text;
        string subcadena, cadenaFinal = "";
        int tamaño = pCadenaASeparar.Length;
        //cortes cada 4 carateres.
        int VecesCabeSeparador4pos = tamaño / 4;
        byte CorteInicial = 0;


        while (VecesCabeSeparador4pos >= 0)
        {

            if (VecesCabeSeparador4pos == 0)//tomar lo que queda de la cadena
                                            //substraer toda la cadena.
                subcadena = pCadenaASeparar.Substring(CorteInicial, pCadenaASeparar.Length - CorteInicial);
            else
                //substraer 4 caracteres de la cadena.
                subcadena = pCadenaASeparar.Substring(CorteInicial, 4);


            //agrgar guion a la cadena cortada.
            cadenaFinal = cadenaFinal + " " + subcadena;


            //aumentar el cortador
            CorteInicial = Convert.ToByte(CorteInicial + 4);
            //disminuir el contador de iteracciones.
            VecesCabeSeparador4pos--;

        }

        //quitar el 1er caracter que es un guion no valido.
        return cadenaFinal.Substring(1, cadenaFinal.Length - 1);
    }

    //De  19/05/2011 a 2011/05/19
    public static String CambiaFechaDeEspañolAIngles(String p_Fecha)
    {
        if (p_Fecha != String.Empty)
        {
            string fecha;

            fecha = Microsoft.VisualBasic.Strings.Mid(p_Fecha, 7, 4) + "/" +
                        Microsoft.VisualBasic.Strings.Mid(p_Fecha, 4, 2) + "/" +
                        Microsoft.VisualBasic.Strings.Mid(p_Fecha, 1, 2) + " ";

            return fecha;

        }
        else
        {
            return null;

        }//end if       

    }//end method  

    //De 2011/05/19 a 19/05/2011
    public static String CambiaFechaDeInglesAEspañol(String p_Fecha)
    {
        if (p_Fecha != String.Empty)
        {
            string fecha;

            fecha = Microsoft.VisualBasic.Strings.Mid(p_Fecha, 9, 2) + "/" +
                        Microsoft.VisualBasic.Strings.Mid(p_Fecha, 6, 2) + "/" +
                        Microsoft.VisualBasic.Strings.Mid(p_Fecha, 1, 4) + " ";

            return fecha;
        }
        else
        {
            return null;

        }//end if        

    }//end method  

    public static String ObtenPrimerDiaMesDeFecha(String p_Fecha)
    {
        if (p_Fecha != String.Empty)
        {

            DateTime oDate = Util.convierteEnFecha(p_Fecha);
            DateTime firstDay = oDate.AddDays(-(oDate.Day - 1));

            return firstDay.ToShortDateString();
        }
        else
        {
            return String.Empty;

        }//end if        

    }//end string

    public static String ObtenUltimoDiaMesDeFecha(String p_Fecha)
    {
        if (p_Fecha != String.Empty)
        {
            DateTime oDate = Util.convierteEnFecha(p_Fecha);
            DateTime firstDay = oDate.AddDays(-(oDate.Day - 1)); //first day
            oDate = oDate.AddMonths(1);
            DateTime lastDay = oDate.AddDays(-(oDate.Day)); //last day    

            return lastDay.ToShortDateString();
        }
        else
        {
            return String.Empty;

        }//end if        



    }//end string

    //Remover acentos
    public static string RemoverAcentosConRegEx(string inputString)
    {
        Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
        Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
        Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
        Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
        Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
        inputString = replace_a_Accents.Replace(inputString, "a");
        inputString = replace_e_Accents.Replace(inputString, "e");
        inputString = replace_i_Accents.Replace(inputString, "i");
        inputString = replace_o_Accents.Replace(inputString, "o");
        inputString = replace_u_Accents.Replace(inputString, "u");
        return inputString;
    }

    public static string QuitarAcentosTexto(string Texto)
    {
        string textoNormalizado = Texto.Normalize(System.Text.NormalizationForm.FormD);
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 ]");
        string textoSinAcentos = reg.Replace(textoNormalizado, "");
        return textoSinAcentos;
    }

    public static string ObtenerDescripcionMes(int Mes)
    {
        string descMes = string.Empty;

        switch (Mes)
        {
            case 1:
                descMes = "Enero";
                break;
            case 2:
                descMes = "Febrero";
                break;
            case 3:
                descMes = "Marzo";
                break;
            case 4:
                descMes = "Abril";
                break;
            case 5:
                descMes = "Mayo";
                break;
            case 6:
                descMes = "Junio";
                break;
            case 7:
                descMes = "Julio";
                break;
            case 8:
                descMes = "Agosto";
                break;
            case 9:
                descMes = "Septiembre";
                break;
            case 10:
                descMes = "Octubre";
                break;
            case 11:
                descMes = "Noviembre";
                break;
            case 12:
                descMes = "Diciembre";
                break;
        }

        return descMes;
    }


}
