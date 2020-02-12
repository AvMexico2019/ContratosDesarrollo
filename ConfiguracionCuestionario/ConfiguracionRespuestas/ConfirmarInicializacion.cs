using System;
using System.Windows.Forms;

namespace ConfiguracionCuestionario
{
    public partial class ConfirmarInicializacion : Form
    {
        public bool Respuesta = false;
        public ConfirmarInicializacion(string Msg)
        {
            InitializeComponent();
            labelMsg.Text = Msg;
            
        }

        private void buttonSi_Click(object sender, EventArgs e)
        {
            Respuesta = true;
            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
