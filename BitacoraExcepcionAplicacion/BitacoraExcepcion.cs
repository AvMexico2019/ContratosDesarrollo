using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using System.Data.SqlClient;
//log4net
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using System.Xml.Schema;

    public class BitacoraExcepcion
    {

        //Propiedades de Objeto
        string _CadenaconexionBD;

        public string CadenaconexionBD
        {
            get { return _CadenaconexionBD; }
            set { _CadenaconexionBD = value; }
        }


        string _Aplicacion;

        public string Aplicacion
        {
            get { return _Aplicacion; }
            set { _Aplicacion = value; }
        }
        string _Modulo;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }
        string _Funcion;

        public string Funcion
        {
            get { return _Funcion; }
            set { _Funcion = value; }
        }

        string _DescExcepcion;

        public string DescExcepcion
        {
            get { return _DescExcepcion; }
            set { _DescExcepcion = value; }
        }

        string _Usr;

        public string Usr
        {
            get { return _Usr; }
            set { _Usr = String.IsNullOrEmpty(value) ?  "--": value; }
        }


//***********************Metodos de Acceso a Datos *******************************************************
        private SqlConnection con;
        private SqlCommand cmd;


        public void RegistrarBitacoraExcepcion()
        {
            cmd = new SqlCommand("spuInsertBitocaraExcepcionAplicacion");
            cmd.CommandType = CommandType.StoredProcedure;
            //pasar parametros de objeto al comando
            cmd.Parameters.Add("@Aplicacion", SqlDbType.VarChar, 50).Value = this.Aplicacion;
            cmd.Parameters.Add("@Modulo", SqlDbType.VarChar, 50).Value = this.Modulo;
            cmd.Parameters.Add("@Funcion", SqlDbType.VarChar, 50).Value = this.Funcion;
            cmd.Parameters.Add("@UsuarioRegistro", SqlDbType.VarChar, 20).Value = this.Usr;
            cmd.Parameters.Add("@DescExcepcion", SqlDbType.VarChar).Value = this.DescExcepcion;
            cmd.CommandTimeout = 60; //60 seg.

            try
            {
                con = new SqlConnection(this.CadenaconexionBD);
                cmd.Connection = con;
                con.Open();

                //Ejecutar el comando
                cmd.ExecuteNonQuery();
            }

            //si ocurre excepcion al intentar escbribir en la BD, entonces escribir información del objeto Excepcion como archivo con Log4Net
            //esta exepcion puede ocurrir cuando por ejemplo la cadena de conexion no es correcta o el servidor no esta disponible.
            catch (Exception ex)
            {
                //escribir en archivo Log4Net, porque no hay servidor de BD.

                //escribir en archivo Log4net
                log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log4net.Config.XmlConfigurator.Configure();
                log.Error("[Aplicacion] " + this.Aplicacion);
                log.Error("[Modulo] " + this.Modulo);
                log.Error("[Función] " + this.Funcion);
                log.Error("[Usuario] " + this.Usr);
                log.Error("[Excepcion] " + this.DescExcepcion);
                log.Error("[Nota] " + "Se registra esta excepion con Log4net porque no se logro registrar en la BD. [" + ex.Message + "]");
                log.Error("******************************************");
            }
            finally
            {
                cmd.Dispose();
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }


        //guardado en archivo
        public void RegistrarBitacoraEnArchivo(BitacoraExcepcion pBitacoraExcep)
        {
            //escribir en archivo Log4net
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
            log.Error("[Aplicacion] " + pBitacoraExcep.Aplicacion);
            log.Error("[Modulo] " + pBitacoraExcep.Modulo);
            log.Error("[Función] " + pBitacoraExcep.Funcion);
            log.Error("[Usuario] " + pBitacoraExcep.Usr);
            log.Error("[Excepcion] " + pBitacoraExcep.DescExcepcion);
            log.Error("******************************************");
        }


    }
