using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ConfiguracionCuestionario
{
    public partial class INFOChange : Form
    {
        private DataChanged dataChanged = null;
        public DateTime Ahora = DateTime.Now;
        private DateTime LastInicialization = DateTime.Now;
        public INFOChange()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.INFOChange_Load);

            /*
             * Para monitorear la base de datos es necesario abilitat ENABLE_BROKER
             * use [ArrendamientoInmueble]
             * ALTER DATABASE [Chatter] SET  ENABLE_BROKER   --   IMPORTANTE
             */
            try
            {
                SqlClientPermission perm = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);
                perm.Demand();

            }
            catch
            {
                throw new ApplicationException("No permission");
            }
        }

        private void INFOChange_Load(object sender, EventArgs e)
        {
            dataChanged = new DataChanged(this);
            dataChanged.OnNewMessage += new DataChanged.NewMessage(OnNewMessageHandler);
            dataChanged.dataChangedDBError += DataChangedDBError;
            LoadMessages();
            DateTime LastInicialization = DateTime.Now;
        }

        void DataChangedDBError()
        {
            if ((DateTime.Now - LastInicialization).TotalMilliseconds >= 1.0)
            {
                dataChanged = new DataChanged(this);
                dataChanged.OnNewMessage += new DataChanged.NewMessage(OnNewMessageHandler);
                dataChanged.dataChangedDBError += DataChangedDBError;
                LoadMessages();
                LastInicialization = DateTime.Now;
            }
            else
            {
                MessageBox.Show("Problema de conexión con la BD");
                Application.Exit();
            }
            
        }
        void OnNewMessageHandler()
        {
            ISynchronizeInvoke i = (ISynchronizeInvoke)this;

            // Check if the event was generated from another
            // thread and needs invoke instead
            if (i.InvokeRequired)
            {
                DataChanged.NewMessage tempDelegate = new DataChanged.NewMessage(OnNewMessageHandler);
                i.BeginInvoke(tempDelegate, null);
                return;
            }

            // If not coming from a seperate thread
            // we can access the Windows form controls
            LoadMessages();
        }

        private void LoadMessages()
        {
            // Clear the listbox
            listBoxINFOModificada.Items.Clear();

            listBoxINFOModificada.Items.Add("Empezamos a partir de " + Ahora.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            // Get the messages
            DataTable dt = dataChanged.GetChanges();

            // Iterate through the records and add them
            // to the listbox
            foreach (DataRow row in dt.Rows)
            {
                listBoxINFOModificada.Items.Add(row["INFO"]);
            }
        }
    }
}
