//Web Developer: Ricardo Rangel Ramírez
//Contacto: riccardorangel@hotmail.com   

using System.Web.UI.WebControls;
using System;
using System.IO;

public class FileCode
{
    public static Byte[] ReadFile(String p_Path, String p_FileName, String p_ContentType)
    {
        FileStream oFileStream = File.OpenRead(Path.Combine(p_Path, p_FileName));
        Byte[] oFile = new byte[oFileStream.Length];
        oFileStream.Read(oFile, 0, oFile.Length);

        oFileStream.Close();
        oFileStream.Dispose();
        oFileStream = null;

        System.Web.HttpContext.Current.Response.ContentType = p_ContentType;
        System.Web.HttpContext.Current.Response.OutputStream.Write(oFile, 0, oFile.Length);        

        return oFile;

    }//end Byte      

}//end class