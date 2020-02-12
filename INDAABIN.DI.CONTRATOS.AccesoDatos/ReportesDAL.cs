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
    public class ReportesDAL
    {

        public DataTable SelectReporteInmuebles(string strConnectionString, Nullable<int> fk_IdInstitucion, string FechaIInicial, string FechaIFinal, string FechaFInicial, string FechaFFinal, string FechaRInicial, string FechaRFinal, Nullable<int> fk_IdTipoContrato, Nullable<int> fk_IdTipoOcupacion)
        {
            SqlConnection SqlConnectionBD = new System.Data.SqlClient.SqlConnection(strConnectionString);

            try
            {
                SqlCommand cmd = new SqlCommand("dbo.[spuSelectReporteInmuebles]");
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdInstitucion", SqlDbType.Int).Value = fk_IdInstitucion;
                cmd.Parameters.Add("@FechaRInicial", SqlDbType.NVarChar).Value = FechaRInicial;
                cmd.Parameters.Add("@FechaRFinal", SqlDbType.NVarChar).Value = FechaRFinal;
                cmd.Parameters.Add("@FechaIInicial", SqlDbType.NVarChar).Value = FechaIInicial;
                cmd.Parameters.Add("@FechaIFinal", SqlDbType.NVarChar).Value = FechaIFinal;
                cmd.Parameters.Add("@FechaFInicial ", SqlDbType.NVarChar).Value = FechaFInicial;
                cmd.Parameters.Add("@FechaFFinal", SqlDbType.NVarChar).Value = FechaFFinal;
                cmd.Parameters.Add("@IdTipoContrato", SqlDbType.Int).Value = fk_IdTipoContrato;
                cmd.Parameters.Add("@IdTipoOcupacion", SqlDbType.Int).Value = fk_IdTipoOcupacion;

                using (SqlConnectionBD)
                {
                    SqlConnectionBD.Open();
                    cmd.Connection = SqlConnectionBD;

                    DataSet oDataSet = new DataSet();
                    SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                    oAdapter.Fill(oDataSet);

                    if (oDataSet.Tables.Count > 0)
                        if (oDataSet.Tables[0].Rows.Count > 0)
                            return oDataSet.Tables[0];
                }
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("SelectReporteInmuebles: ", ex.Message));
            }

            
            return null;
        }
    }
}
