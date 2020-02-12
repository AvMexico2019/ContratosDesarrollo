//Web Developer: Ricardo Rangel Ramírez
//Contacto: riccardorangel@hotmail.com   

using System.Web.UI.WebControls;
using System;

public class MathCode
{
    public static bool ValidaRangoNumeroDecimal(Decimal ? p_Numero, Decimal ? p_ValorInicial, Decimal ? p_ValorFinal)
    {
        if (p_Numero < p_ValorInicial || p_Numero > p_ValorFinal)
        {
            return false;

        }
        else
        {
            return true; 

        }//end if

    }// end bool     

    public static bool ValidaRangoNumeroEntero(Int32 ? p_Numero, Int32 ? p_ValorInicial, Int32 ? p_ValorFinal)
    {
        if (p_Numero < p_ValorInicial || p_Numero > p_ValorFinal)
        {
            return false;

        }
        else
        {
            return true;

        }//end if

    }// end bool     

}//end class