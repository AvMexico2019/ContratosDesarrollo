using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Utilerias
{
    public class SessionHelper
    {
        public const string CONTADORVAR = "Contador";
        public const string TEXTOVAR = "Texto";


         public static T Lee<T>(string variable)
       {
           object valor = HttpContext.Current.Session[variable];
           if (valor == null)
              return default(T);
          else
               return ((T)valor);
       }

        public static void Escribe(string variable, object valor)
      {
         HttpContext.Current.Session[variable] = valor;
      }
   
        /** Implementacion de Propiedades para cada persistencia de informacion entre postback: Session ***/


        //por ejemplo para usar esta propiedad como una session, seria: SessionHelper.Contador++;
      public static int Contador { 
          get
         {
               return Lee<int>(CONTADORVAR);
          }
          set
          {
              Escribe(CONTADORVAR, value);
           } 
      }
  

        //
       public static string Texto
      {
          get
          {
               return Lee<string>(TEXTOVAR);
          }
        set
          {
               Escribe(TEXTOVAR, value);
         }
     }

    }//clase
}