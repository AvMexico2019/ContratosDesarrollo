using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ConfiguracionCuestionario
{
    public class DataChanged
    {
        private string connectionString = null;
        private SqlConnection m_sqlConn = null;

        public delegate void NewMessage();
        public event NewMessage OnNewMessage;

        public delegate void DataChangedDBError();
        public event DataChangedDBError dataChangedDBError;

        INFOChange padre;

        public DataChanged(INFOChange padre)
        {
            this.padre = padre;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["GranHermano"].ConnectionString;
                SqlDependency.Stop(connectionString);

                SqlDependency.Start(connectionString);

                m_sqlConn = new SqlConnection(connectionString);
            }
            catch (Exception)
            {
                if (dataChangedDBError != null) dataChangedDBError();
            }
        }

        ~DataChanged()
        {
            SqlDependency.Stop(connectionString);
        }

        public DataTable GetChanges()
        {
            DataTable dt = new DataTable();

            try
            {
                // Create command
                // Command must use two part names for tables
                // SELECT <field> FROM dbo.Table rather than 
                // SELECT <field> FROM Table
                // Query also can not use *, fields must be designated
                SqlCommand cmd = new SqlCommand("[SandBox].[GetINFOChanged]", m_sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Ahora", SqlDbType.DateTime).Value = padre.Ahora;

                // Clear any existing notifications
                cmd.Notification = null;

                // Create the dependency for this command
                SqlDependency dependency = new SqlDependency(cmd);

                // Add the event handler
                dependency.OnChange += new OnChangeEventHandler(OnChange);

                // Open the connection if necessary
                if (m_sqlConn.State == ConnectionState.Closed)
                    m_sqlConn.Open();

                // Get the messages
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            }
            catch (Exception)
            {
                if (dataChangedDBError != null) dataChangedDBError();
            }

            return dt;
        }


        void OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;

            // Notices are only a one shot deal
            // so remove the existing one so a new 
            // one can be added
            dependency.OnChange -= OnChange;

            // Fire the event
            if (OnNewMessage != null)
            {
                OnNewMessage();
            }
        }

    }
}
