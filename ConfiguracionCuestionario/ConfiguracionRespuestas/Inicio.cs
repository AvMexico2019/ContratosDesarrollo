using System;
using System.Windows.Forms;

namespace ConfiguracionCuestionario
{
    public partial class Inicio : Form
    {
        Cuestionario cuestionario = null;
        Conceptos conceptos = null;
        INFOChange modificacionINFO = new INFOChange();
        public Inicio()
        {
            InitializeComponent();
            //modificacionINFO.Show();
            
        }

        private void buttonSalir_Click(object sender, EventArgs e)
        {
            if (conceptos != null)
                conceptos.Close();
            if (cuestionario != null)
                cuestionario.Close();
            this.Close();
        }

        private void buttonCuestionario_Click(object sender, EventArgs e)
        {
            if (cuestionario != null)
                cuestionario.Show();
            else
            {
                cuestionario = new Cuestionario();
                cuestionario.FormClosed += Cuestionario_FormClosed;
                cuestionario.Show();
            }
            cuestionario.BringToFront();
        }

        private void buttonConceptos_Click(object sender, EventArgs e)
        {
            
            if (conceptos != null)
                conceptos.Show();
            else
            {
                conceptos = new Conceptos();
                conceptos.conceptosDBModificada += Conceptos_conceptosDBModificada;
                conceptos.FormClosed += Conceptos_FormClosed;
                conceptos.Show();
            }
            conceptos.BringToFront();
        }

        private void Conceptos_conceptosDBModificada()
        {
            if (cuestionario != null)
                cuestionario.Actualiza_ConceptosDB();
        }

        private void Conceptos_FormClosed(object sender, FormClosedEventArgs e)
        {
            conceptos = null;
        }

        private void Cuestionario_FormClosed(object sender, FormClosedEventArgs e)
        {
            conceptos = null;
        }

        private void buttonInicializaSandBox_Click(object sender, EventArgs e)
        {
            ConfirmarInicializacion confirmarMsg = new ConfirmarInicializacion("¿Confirma que quiere inicializar el SandBox?");
            confirmarMsg.StartPosition = FormStartPosition.CenterParent;
            confirmarMsg.ShowDialog();
            if (confirmarMsg.Respuesta)
            {
                if (cuestionario != null)
                {
                    cuestionario.Close();
                    cuestionario = new Cuestionario();
                    cuestionario.FormClosed += Cuestionario_FormClosed;
                    cuestionario.Show();

                }
                if (conceptos != null)
                {
                    conceptos.Close();
                    conceptos = new Conceptos();
                    conceptos.conceptosDBModificada += Conceptos_conceptosDBModificada;
                    conceptos.FormClosed += Conceptos_FormClosed;
                    conceptos.Show();
                }
                SandBoxDB.SandBoxEntities ctx = new SandBoxDB.SandBoxEntities();
                ctx.InicializaSandBox();
                ctx.Dispose();
            }
        }

        private void buttonSandBox2dbo_Click(object sender, EventArgs e)
        {
            ConfirmarInicializacion confirmarMsg = new ConfirmarInicializacion("¿Confirma que quiere copia el SandBox a dbo?");
            confirmarMsg.StartPosition = FormStartPosition.CenterParent;
            confirmarMsg.ShowDialog();
            if (confirmarMsg.Respuesta)
            {
                if (cuestionario != null)
                {
                    cuestionario.Close();
                    cuestionario.Dispose();
                }
                if (conceptos != null)
                {
                    conceptos.Close();
                    conceptos.Dispose();
                }
                SandBoxDB.SandBoxEntities ctx = new SandBoxDB.SandBoxEntities();
                ctx.CopiaSandBox2dbo();
                ctx.Dispose();
            }
        }
    }
}
