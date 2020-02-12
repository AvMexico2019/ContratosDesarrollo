using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebFC.Utilerias
{
    public class DAL
    {
        public int RunCommand(string strConexion, string strCommand)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlComm = null;

            try
            {
                using (sqlConn = new SqlConnection(strConexion))
                {
                    using (sqlComm = new SqlCommand(strCommand, sqlConn))
                    {
                        sqlConn.Open();
                       return sqlComm.ExecuteNonQuery();
                    }

                }

            }

            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                            

                if (sqlComm != null)
                    sqlComm.Dispose();

                if (sqlConn != null)
                {
                    sqlConn.Close();
                    sqlConn.Dispose();
                }

            }

        }

    }
}