using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


//comunicacion con las capas
using INDAABIN.DI.CONTRATOS.Datos; //EntityFramework
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //Entities

namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class InmuebleDAL
    {

        //Obtener la lista Contrato. registrados
        public List<ModeloNegocios.Inmueble> ObtenerInmuebles(int? IdPais, int? IdEstado, int? IdMunicipio, string RIUF)
        {
            List<ModeloNegocios.Inmueble> ListInmuebles;

            using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
            {
                try
                {
                    ListInmuebles = Conn.spuSelectInmuebles().Select(RegistroBD => new ModeloNegocios.Inmueble
                    {
                        IdInmueble = RegistroBD.IdInmueble, //PK
                        IdPais = RegistroBD.Fk_IdPais,
                        IdTipoInmueble = RegistroBD.Fk_IdTipoInmueble,
                        IdEstado = RegistroBD.Fk_IdEstado,
                        IdMunicipio = RegistroBD.Fk_IdMunicipio,
                        IdLocalidad = RegistroBD.Fk_IdLocalidad,
                        OtraColonia = RegistroBD.OtraColonia,
                        IdTipoVialidad = RegistroBD.Fk_IdTipoVialidad,
                        NombreVialidad = RegistroBD.NombreVialidad,
                        NumExterior = RegistroBD.NumExterior,
                        NumInterior = RegistroBD.NumInterior,
                        CodigoPostal = RegistroBD.CodigoPostal,
                        GeoRefLatitud = RegistroBD.GeoRefLatitud,
                        GeoRefLongitud = RegistroBD.GeoRefLongitud,
                        NombreInmueble = RegistroBD.NombreInmueble,
                        CodigoPostalExtranjero = RegistroBD.CodigoPostalExtranjero,
                        EstadoExtranjero = RegistroBD.EstadoExtranjero,
                        CiudadExtranjero = RegistroBD.CiudadExtranjero,
                        MunicipioExtranjero = RegistroBD.MunicipioExtranjero,
                        IdRIUF = RegistroBD.Fk_IdRIUF,
                        EstatusRegistro = RegistroBD.EstatusRegistro,
                        RIUF = new ModeloNegocios.RIUF
                        {
                            IdRIUF = (RegistroBD.IdRIUF) == null ? 0 : RegistroBD.IdRIUF,
                            IdEstadoRIUF = (RegistroBD.Fk_IdEstadoRIUF) == null ? 0 : RegistroBD.Fk_IdEstadoRIUF,
                            RIUF1 = (RegistroBD.RIUF1) == null ? "" : RegistroBD.RIUF1,
                            Digitoverificador = (RegistroBD.DigitoVerificador) == null ? 0 : RegistroBD.DigitoVerificador,
                            Consecutivo = (RegistroBD.Consecutivo) == null ? 0 : RegistroBD.Consecutivo
                        }
                    }).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception(string.Format("ObtenerInmuebles: ", ex.Message));
                }
                
            }//using

            List<ModeloNegocios.Inmueble> oLista;
            //if (RIUF != "")
            //{
            //    oLista = ListInmuebles.Where(
            //        tc => tc.EstatusRegistro == true
            //    && tc.RIUF.RIUF1.ToString() == RIUF.ToString()).ToList();
            //}
            //else
            //{
                if (IdPais != 165)
                {
                    oLista = ListInmuebles.Where(
                        tc => tc.EstatusRegistro == true
                    && tc.IdPais == IdPais).ToList();
                }
                else
                {
                    if (IdMunicipio != 0)
                    {
                        oLista = ListInmuebles.Where(
                            tc => tc.EstatusRegistro == true
                        && tc.IdEstado == IdEstado
                        && tc.IdMunicipio == IdMunicipio).ToList();
                    }
                    else
                    {
                        if (IdEstado != 0)
                        {
                            oLista = ListInmuebles.Where(
                                tc => tc.EstatusRegistro == true
                            && tc.IdEstado == IdEstado).ToList();
                        }
                        else
                        {
                            oLista = ListInmuebles.Where(
                                tc => tc.EstatusRegistro == true
                            && tc.IdPais == IdPais).ToList();
                        }
                    }
                }
           // }
            return oLista;
        }

        public string InsertInmueble(string strConnectionString, string nombrePais, Nullable<int> fk_IdPais, Nullable<int> fk_IdTipoInmueble, Nullable<int> fk_IdEstado, Nullable<int> fk_IdMunicipio, Nullable<int> fk_IdLocalidad, string otraColonia, Nullable<int> fk_IdTipoVialidad, string nombreVialidad, string numExterior, string numInterior, string codigoPostal, Nullable<decimal> geoRefLatitud, Nullable<decimal> geoRefLongitud, string nombreInmueble, string codigoPostal_Extranjero, string estado_Extranjero, string ciudad_Extranjero, string municipio_Extranjero, Nullable<int> generarRIUF, string numeroRIUF, Nullable<int> OtraFigura, Nullable<int> fk_IdUsuarioRegistro, string cargoUsuarioRegistro)
        {
            SqlConnection SqlConnectionBD = new System.Data.SqlClient.SqlConnection(strConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("dbo.[spuInsertInmueble]");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NombrePais", SqlDbType.VarChar, 150).Value = nombrePais.ToUpper();
                cmd.Parameters.Add("@Fk_IdPais", SqlDbType.Int).Value = fk_IdPais;
                cmd.Parameters.Add("@Fk_IdTipoInmueble", SqlDbType.Int).Value = fk_IdTipoInmueble;
                cmd.Parameters.Add("@Fk_IdEstado", SqlDbType.Int).Value = fk_IdEstado;
                cmd.Parameters.Add("@Fk_IdMunicipio", SqlDbType.Int).Value = fk_IdMunicipio;
                cmd.Parameters.Add("@Fk_IdLocalidad", SqlDbType.Int).Value = fk_IdLocalidad;
                cmd.Parameters.Add("@OtraColonia", SqlDbType.VarChar, 50).Value = otraColonia == null ? otraColonia : otraColonia.ToUpper();
                cmd.Parameters.Add("@Fk_IdTipoVialidad", SqlDbType.Int).Value = fk_IdTipoVialidad;

                cmd.Parameters.Add("@NombreVialidad", SqlDbType.VarChar, 255).Value = nombreVialidad.ToUpper();
                cmd.Parameters.Add("@NumExterior", SqlDbType.VarChar, 50).Value = numExterior.ToUpper();
                cmd.Parameters.Add("@NumInterior", SqlDbType.VarChar, 50).Value = numInterior == null ? numInterior : numInterior.ToUpper();
                cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 5).Value = codigoPostal;
                cmd.Parameters.Add("@GeoRefLatitud", SqlDbType.Decimal).Value = geoRefLatitud;
                cmd.Parameters.Add("@GeoRefLongitud", SqlDbType.Decimal).Value = geoRefLongitud;

                cmd.Parameters.Add("@NombreInmueble", SqlDbType.VarChar, 150).Value = nombreInmueble.ToUpper();
                cmd.Parameters.Add("@CodigoPostal_Extranjero", SqlDbType.VarChar, 20).Value = codigoPostal_Extranjero == null ? codigoPostal_Extranjero : codigoPostal_Extranjero.ToUpper();
                cmd.Parameters.Add("@Estado_Extranjero", SqlDbType.VarChar, 150).Value = estado_Extranjero == null ? estado_Extranjero : estado_Extranjero.ToUpper();
                cmd.Parameters.Add("@Ciudad_Extranjero", SqlDbType.VarChar, 150).Value = ciudad_Extranjero == null ? ciudad_Extranjero : ciudad_Extranjero.ToUpper();
                cmd.Parameters.Add("@Municipio_Extranjero", SqlDbType.VarChar, 150).Value = municipio_Extranjero == null ? municipio_Extranjero : municipio_Extranjero.ToUpper();

                cmd.Parameters.Add("@GenerarRIUF", SqlDbType.Int).Value = generarRIUF;
                cmd.Parameters.Add("@NumeroRIUF", SqlDbType.VarChar, 10).Value = numeroRIUF;
                cmd.Parameters.Add("@IdOtraFigura", SqlDbType.Int).Value = OtraFigura;
                cmd.Parameters.Add("@Fk_IdUsuarioRegistro", SqlDbType.Int).Value = fk_IdUsuarioRegistro;
                cmd.Parameters.Add("@CargoUsuarioRegistro", SqlDbType.VarChar, 170).Value = cargoUsuarioRegistro.ToUpper();

                //configurar el parametro de retorno.
                SqlParameter prmRet;
                prmRet = new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4);
                prmRet.Direction = ParameterDirection.ReturnValue;
                //Agregar el parametro al comando.
                cmd.Parameters.Add(prmRet);

                using (SqlConnectionBD)
                {
                    SqlConnectionBD.Open();
                    cmd.Connection = SqlConnectionBD;

                    DataSet oDataSet = new DataSet();
                    SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                    oAdapter.Fill(oDataSet);

                    if (oDataSet.Tables.Count > 0)
                        if (oDataSet.Tables[0].Rows.Count > 0)
                            if (oDataSet.Tables[0].Columns.Contains("IDInmueble"))
                                return oDataSet.Tables[0].Rows[0]["IDInmueble"].ToString().Trim() + "@" + oDataSet.Tables[0].Rows[0]["RIUFNUEVO"].ToString().Trim() + "@" + oDataSet.Tables[0].Rows[0]["RIUFACTUAL"].ToString().Trim();

                    //if (iAffect > 0)                     
                    //    FolioEmisionOpinion = (int)cmd.Parameters["@RETURN_VALUE"].Value;

                }
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("InsertInmueble: ", ex.Message));
            }

            return "ERROR";
        }//insert



        //public int InsertInmueble(ModeloNegocios.Inmueble objInmueble)
        //{
        //    int iAffect;

        //    using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
        //    {

        //        iAffect = Conn.spuInsertInmueble(
        //objInmueble.PaisDescripcion,
        //objInmueble.IdPais,
        //objInmueble.IdTipoInmueble,
        //objInmueble.IdEstado,
        //objInmueble.IdMunicipio,
        //objInmueble.IdLocalidad,
        //objInmueble.OtraColonia,
        //objInmueble.IdTipoVialidad,
        //objInmueble.NombreVialidad,
        //objInmueble.NumExterior,
        //objInmueble.NumInterior,
        //objInmueble.CodigoPostal,
        //objInmueble.GeoRefLatitud,
        //objInmueble.GeoRefLongitud,
        //objInmueble.NombreInmueble,
        //objInmueble.CodigoPostalExtranjero,
        //objInmueble.EstadoExtranjero,
        //objInmueble.CiudadExtranjero,
        //objInmueble.MunicipioExtranjero,
        //objInmueble.GeneraRIUF,
        //objInmueble.RIUF.RIUF1,
        //objInmueble.IdUsuarioRegistro,
        //objInmueble.CargoUsuarioRegistro
        //            );

        //        Conn.SaveChanges();


        //    }//using


        //    return iAffect; //afectados
        //}
    }
}
