//Web Developer: Ricardo Rangel Ramírez
//Contacto: riccardorangel@hotmail.com   

using System.Web.UI.WebControls;
using System;
using System.Text;
using System.Collections.Specialized;

public class XML_Code
{      
    public static StringBuilder CreaEstructuraXML(GridView gv, int celdaCheckBox, string NombreCheckBox, string tema, string elemento, NameValueCollection atributos)
    {            
        string strInicioXML = "<" + tema + ">" + "\r\n";
        string strFinXML = "</" + tema + ">" + "\r\n";

        //Recorrer la rejilla para armar el archivo XML
        System.Text.StringBuilder SKUsXML = new System.Text.StringBuilder();
        Int16 i = default(Int16);

        try
        {
            if (gv.Rows.Count > 0)
            {
                SKUsXML.Append(strInicioXML);

                for (i = 0; i <= gv.Rows.Count - 1; i++)
                {
                    //Por cada fila seleccionada por el usuario
                    if (((CheckBox)gv.Rows[i].Cells[celdaCheckBox].FindControl(NombreCheckBox)).Checked == true)
                    {                        
                        SKUsXML.Append("<" + elemento + " ");

                        //Por cada atributo
                        for (int x = 0; x < atributos.Count; x++)
                        {                      
                            //Columnas 
                            Int32 y;
                            y = Convert.ToInt32(atributos.GetKey(x));
                            SKUsXML.Append(atributos.Get(x) + "=" + "\"" + gv.Rows[i].Cells[y].Text.Replace("ñ", "n").Replace("Ñ", "N") + "\"" + " ");                            
                        }                    

                        SKUsXML.Append("/>" + "\r\n");                        
                        
                    }//end if                       

                }//end for

                SKUsXML.Append(strFinXML);

            }//end if          

        }
        catch (Exception ex)
        {
            throw new Exception("Se ha producido un error en el metodo CreaEstructuraXML", ex);

        }//end try

        return SKUsXML;

    }//end StringBuilder

    public static StringBuilder CreaEstructuraXML(ListBox lv, string tema, string elemento, NameValueCollection atributos)
    {
        string strInicioXML = "<" + tema + ">" + "\r\n";
        string strFinXML = "</" + tema + ">" + "\r\n";

        //Recorrer la rejilla para armar el archivo XML
        System.Text.StringBuilder SKUsXML = new System.Text.StringBuilder();
        Int16 i = default(Int16);

        try
        {
            if (lv.Items.Count > 0)
            {
                SKUsXML.Append(strInicioXML);

                for (i = 0; i <= lv.Items.Count - 1; i++)
                {
                    SKUsXML.Append("<" + elemento + " ");

                    //Por cada atributo
                    for (int x = 0; x < atributos.Count; x++)
                    {
                        //Columnas 
                        Int32 y;
                        y = Convert.ToInt32(atributos.GetKey(x));

                        if (y == 0)
                        {
                            SKUsXML.Append(atributos.Get(x) + "=" + "\"" + lv.Items[i].Value + "\"" + " ");
                        }
                        else if (y == 1)
                        {
                            SKUsXML.Append(atributos.Get(x) + "=" + "\"" + lv.Items[i].Text.Replace("ñ", "n").Replace("Ñ", "N") + "\"" + " ");
                        }

                    }

                    SKUsXML.Append("/>" + "\r\n");

                }//end for

                SKUsXML.Append(strFinXML);

            }//end if          

        }
        catch (Exception ex)
        {
            throw new Exception("Se ha producido un error en el metodo CreaEstructuraXML", ex);

        }//end try

        return SKUsXML;

    }//end StringBuilder

     public static StringBuilder CreaEstructuraXMLSoloElementosSeleccionados(ListBox lv, string tema, string elemento, NameValueCollection atributos)
    {
        string strInicioXML = "<" + tema + ">" + "\r\n";
        string strFinXML = "</" + tema + ">" + "\r\n";

        //Recorrer la rejilla para armar el archivo XML
        System.Text.StringBuilder SKUsXML = new System.Text.StringBuilder();
        Int16 i = default(Int16);
        
        try
        {
            if (lv.Items.Count > 0)
            {
                SKUsXML.Append(strInicioXML);

                for (i = 0; i <= lv.Items.Count - 1; i++)
                {
                    //Verifica si el elemento fue seleccionado
                    //ListItem item = default(ListItem);
                    //item = lv.Items.FindByValue(lv.SelectedValue);

                    if (lv.Items[i].Selected == true)
                    {
                        SKUsXML.Append("<" + elemento + " ");

                        //Por cada atributo
                        for (int x = 0; x < atributos.Count; x++)
                        {
                            //Columnas 
                            Int32 y;
                            y = Convert.ToInt32(atributos.GetKey(x));

                            if (y == 0)
                            {
                                SKUsXML.Append(atributos.Get(x) + "=" + "\"" + lv.Items[i].Value + "\"" + " ");
                            }
                            else if (y == 1)
                            {
                                SKUsXML.Append(atributos.Get(x) + "=" + "\"" + lv.Items[i].Text.Replace("ñ", "n").Replace("Ñ", "N") + "\"" + " ");
                            }

                        }

                        SKUsXML.Append("/>" + "\r\n");

                    } //end if                           

                }//end for

                SKUsXML.Append(strFinXML);

            }//end if          

        }
        catch (Exception ex)
        {
            throw new Exception("Se ha producido un error en el metodo CreaEstructuraXML", ex);

        }//end try

        return SKUsXML;

    }//end StringBuilder

    

    public static StringBuilder CreaEstructuraXML_SeparadosPorGuion(ListBox lv, string tema, string elemento, NameValueCollection atributos)
    {        
        string strInicioXML = "<" + tema + ">" + "\r\n";
        string strFinXML = "</" + tema + ">" + "\r\n";

        //Recorrer la rejilla para armar el archivo XML
        System.Text.StringBuilder SKUsXML = new System.Text.StringBuilder();
        Int16 i = default(Int16);

        string cadena = String.Empty;        
        string contrato = String.Empty;
        string importe = String.Empty;
        string EsTramiteDeclinado = String.Empty;       
        try
        {
            if (lv.Items.Count > 0)
            {
                SKUsXML.Append(strInicioXML);

                for (i = 0; i <=  lv.Items.Count - 1; i++)
                {
                        cadena = String.Empty;

                        int primerSeparador = 0;                          
                        primerSeparador = lv.Items[i].Text.IndexOf("--");

                        contrato = lv.Items[i].Text.Substring(0, primerSeparador);
                        contrato = contrato.Replace("ñ", "n").Replace("Ñ", "N");

                        int segundoSeparador = 0;
                        segundoSeparador = lv.Items[i].Text.LastIndexOf("--");                        
                        
                        importe = lv.Items[i].Text.Substring(primerSeparador+2, segundoSeparador - (primerSeparador+2));                        

                        EsTramiteDeclinado = lv.Items[i].Text.Substring(segundoSeparador + 2);                        

                        SKUsXML.Append("<" + elemento + " ");

                        //Por cada atributo
                        for (int x = 0; x < atributos.Count; x++)
                        {                            
                            //Columnas 
                            Int32 y;
                            y = Convert.ToInt32(atributos.GetKey(x));                                                                                                                                           
                            
                            if (atributos.Get(x).ToString() == "Contrato")
                            {
                                cadena += (atributos.Get(x) + "=" + "\"" + contrato + "\"" + " ");

                            }
                            else if (atributos.Get(x).ToString() == "Importe")
                            {
                                cadena += (atributos.Get(x) + "=" + "\"" + importe + "\"" + " ");

                            }//end if
                            else if (atributos.Get(x).ToString() == "EsTramiteDeclinado")
                            {
                                cadena += (atributos.Get(x) + "=" + "\"" + EsTramiteDeclinado + "\"" + " ");

                            }//end if
                                                            
                        }//end for

                        SKUsXML.Append(cadena);      

                        SKUsXML.Append("/>" + "\r\n");                    

                }//end for

                SKUsXML.Append(strFinXML);

            }//end if          

        }
        catch (Exception ex)
        {
            throw new Exception("Se ha producido un error en el metodo CreaEstructuraXML", ex);

        }//end try

        return SKUsXML;

    }//end StringBuilder

    public static StringBuilder CreaEstructuraXML_Dropdownlist(GridView gv, int celdaDropDownList, string listaDesplegable, string tema, string elemento, NameValueCollection atributos)
    {
        string strInicioXML = "<" + tema + ">" + "\r\n";
        string strFinXML = "</" + tema + ">" + "\r\n";
        string texto;

        //Recorrer la rejilla para armar el archivo XML
        System.Text.StringBuilder SKUsXML = new System.Text.StringBuilder();
        Int16 i = default(Int16);

        try
        {
            SKUsXML.Append(strInicioXML);

            if (gv.Rows.Count > 0)
            {
                for (i = 0; i <= gv.Rows.Count - 1; i++) 
                {
                    SKUsXML.Append("<" + elemento + " ");

                    //Por cada atributo
                    for (int x = 0; x < atributos.Count; x++)
                    {
                        //Columnas 
                        Int32 y;
                        y = Convert.ToInt32(atributos.GetKey(x));

                        if (y == 0)
                        {
                            SKUsXML.Append(atributos.Get(x) + "=" + "\"" + gv.Rows[i].Cells[y].Text.Replace("ñ", "n").Replace("Ñ", "N") + "\"" + " ");
                        }
                        else if (y == 1)
                        {
                            DropDownList ddlgv = (DropDownList)gv.Rows[i].Cells[5].FindControl(listaDesplegable);
                            texto = ddlgv.SelectedValue;
                            texto = texto.Replace("ñ", "n");
                            texto = texto.Replace("Ñ", "N");

                            SKUsXML.Append(atributos.Get(x) + "=" + "\"" + texto + "\"" + " ");

                            SKUsXML.Append("/>" + "\r\n");
                        }

                    }//end for              
                
                }//end for                

                SKUsXML.Append(strFinXML);

            }//end if          

        }
        catch (Exception ex)
        {
            throw new Exception("Se ha producido un error en el metodo CreaEstructuraXML_Dropdownlist", ex);            

        }//end try

        return SKUsXML;

    }

}//end class