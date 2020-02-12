using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace ConfiguracionCuestionario
{
    public partial class Conceptos : Form
    {
        Dictionary<int, SandBoxDB.Concepto> DictConceptos = new Dictionary<int, SandBoxDB.Concepto>();
        Dictionary<int, int> ConceptosExistentes = new Dictionary<int, int>();
        List<SandBoxDB.Rel_ConceptoRespValor> relaciones = new List<SandBoxDB.Rel_ConceptoRespValor>();
        Dictionary<int, SandBoxDB.Cat_Tema> Temas = new Dictionary<int, SandBoxDB.Cat_Tema>();
        int ConceptoSeleccionado = -1;
        bool _RegistroModificado;

        public delegate void ConceptosDBModificada();
        public event ConceptosDBModificada conceptosDBModificada;

        bool RegistroModificado
        {
            get { return _RegistroModificado; }
            set
            {
                _RegistroModificado = value;
                if (value)
                {
                    Reg_FechaRegistro = DateTime.Today;
                    this.BackColor = System.Drawing.Color.OrangeRed;
                }
                else
                {
                    this.BackColor = System.Drawing.Color.PaleGreen;
                }
            }
        }
        public Conceptos()
        {
            InitializeComponent();
            this.FormClosing += Conceptos_FormClosing;
            InicializaConceptos();
            comboBoxEstatusRegistro.Items.Add("1");
            comboBoxEstatusRegistro.Items.Add("0");
            
        }

        private void Conceptos_FormClosing(object sender, FormClosingEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        void InicializaConceptos()
        {
            DictConceptos.Clear();
            ConceptosExistentes.Clear();
            relaciones.Clear();
            Temas.Clear();
            ConceptoSeleccionado = -1;
            RegistroModificado = false;
            comboBoxConceptos.Items.Clear();
            try
            {
                SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                int i = 0;
                foreach (var concepto in SandBoxCtx.Concepto)
                {
                    ConceptosExistentes.Add(concepto.IdConcepto, i);
                    DictConceptos.Add(i++, concepto);
                    comboBoxConceptos.Items.Add(concepto.IdConcepto + " - " + concepto.DescripcionConcepto);
                }
                var result = from tema in SandBoxCtx.Cat_Tema
                             select tema;
                foreach (var r in result)
                {
                    Temas.Add(r.IdTema, r);
                }
                ClearRegConcepto();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n no se pede continuar pos 2");
                Application.Exit();
            }
            
        }

        private void comboBoxConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConceptoSeleccionado = DictConceptos[comboBoxConceptos.SelectedIndex].IdConcepto;

            listBoxRelaciones.Items.Clear();
            try
            {
                SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                relaciones = (from relacion in SandBoxCtx.Rel_ConceptoRespValor
                              where relacion.Fk_IdConcepto == ConceptoSeleccionado
                              orderby relacion.Fk_IdTema, relacion.NumOrden
                              select relacion).ToList();
                for (int i = 0; i < relaciones.Count; i++)
                {
                    listBoxRelaciones.Items.Add(relaciones[i].IdConceptoRespValor + " - " + Temas[relaciones[i].Fk_IdTema].DescripcionTema);
                }
                disable_Reg_TextChanged();
                Reg_IdConcepto = ConceptoSeleccionado;
                Reg_Descripción = DictConceptos[comboBoxConceptos.SelectedIndex].DescripcionConcepto;
                Reg_DescripciónAlterna = DictConceptos[comboBoxConceptos.SelectedIndex].DescripcionAlternaConcepto;
                Reg_FundamentoLegal = DictConceptos[comboBoxConceptos.SelectedIndex].FundamentoLegal;
                Reg_Observaciones = DictConceptos[comboBoxConceptos.SelectedIndex].Observaciones;
                Reg_EstatusRegistro = DictConceptos[comboBoxConceptos.SelectedIndex].EstatusRegistro;
                Reg_FechaRegistro = DictConceptos[comboBoxConceptos.SelectedIndex].FechaRegistro;
                Reg_Fk_IdUsuarioRegistro = DictConceptos[comboBoxConceptos.SelectedIndex].Fk_IdUsuarioRegistro;
                enable_Reg_TextChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        void ClearRegConcepto()
        {
            Reg_IdConcepto = -1;
            Reg_Descripción = "";
            Reg_DescripciónAlterna = "";
            Reg_FundamentoLegal = "";
            Reg_Observaciones = "";
            Reg_EstatusRegistro = false;
            Reg_FechaRegistro = new DateTime(2020,01,01,0,0,0);
            Reg_Fk_IdUsuarioRegistro = 30546;
            RegistroModificado = false;
        }

        private int Reg_IdConcepto
        {
            get
            {
                if (labelIdConcepto.Text.Equals("IdConcepto"))
                    return -1;
                else
                    return int.Parse(labelIdConcepto.Text);
            }
            set { if (ConceptosExistentes.ContainsKey(value)) labelIdConcepto.Text = value.ToString(); else labelIdConcepto.Text = "IdConcepto";  }
        }

        private string Reg_Descripción
        {
            get
            {
                return textBoxDescripcion.Text.Equals("NULL") ? "" : textBoxDescripcion.Text;
            }
            set { textBoxDescripcion.Text = value == null ? "NULL" : value; }
        }

        private String Reg_DescripciónAlterna
        {
            get
            {
                return textBoxDescripcionAlterna.Text.Equals("") ? null : textBoxDescripcionAlterna.Text;
            }
            set { textBoxDescripcionAlterna.Text = value == null ? "" : value; }
        }

        private string Reg_FundamentoLegal
        {
            get
            {
                return textBoxFundamentoLegal.Text.Equals("") ? null : textBoxFundamentoLegal.Text;
            }
            set { textBoxFundamentoLegal.Text = value == null ? "" : value; }
        }

        private string Reg_Observaciones
        {
            get
            {
                return textBoxObservaciones.Text.Equals("NULL") ? "" : textBoxObservaciones.Text;
            }
            set { textBoxObservaciones.Text = value == null ? "NULL" : value; }
        }

        private bool Reg_EstatusRegistro
        {
            get
            {
                if (comboBoxEstatusRegistro.SelectedIndex == -1)
                    return false;
                else
                {
                    if (comboBoxEstatusRegistro.SelectedItem.Equals("1"))
                        return true;
                    else
                        return false;
                }
            }
            set { comboBoxEstatusRegistro.SelectedIndex = comboBoxEstatusRegistro.Items.IndexOf(value?"1":"0"); }
        }

        private DateTime Reg_FechaRegistro
        {
            get
            {
                return DateTime.Parse(textBoxFechaReg.Text);
            }
            set { textBoxFechaReg.Text = value.ToString(); }
        }

        private int Reg_Fk_IdUsuarioRegistro
        {
            get
            {
                return int.Parse(textBoxIdUsuarioRegistro.Text);
            }
            set { textBoxIdUsuarioRegistro.Text = value.ToString(); }
        }

        private void textBoxDescripcion_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxDescripcionAlterna_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxFundamentoLegal_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxObservaciones_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void comboBoxEstatusRegistro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxFechaReg_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxIdUsuarioRegistro_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void buttonModifica_Click(object sender, EventArgs e)
        {
            if (RegistroModificado)
            {
                RegistroModificado = false;
                try
                {
                    SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                    int reg = Reg_IdConcepto;
                    var result = SandBoxCtx.Concepto.SingleOrDefault(b => b.IdConcepto == reg);
                    if (result != null)
                    {
                        result.DescripcionConcepto = Reg_Descripción;
                        result.DescripcionAlternaConcepto = Reg_DescripciónAlterna;
                        result.FundamentoLegal = Reg_FundamentoLegal;
                        result.Observaciones = Reg_Observaciones;
                        result.EstatusRegistro = Reg_EstatusRegistro;
                        result.FechaRegistro = DateTime.Today;
                        result.Fk_IdUsuarioRegistro = Reg_Fk_IdUsuarioRegistro;
                        SandBoxCtx.SaveChanges();
                    }
                    InicializaConceptos();
                    if (conceptosDBModificada != null) conceptosDBModificada();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n no se modifico el registro");
                }
                
            }
        }

        private void buttonInserta_Click(object sender, EventArgs e)
        {
            if (RegistroModificado)
            {
                RegistroModificado = false;
                try
                {
                    SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                    SandBoxDB.Concepto conceptoNuevo = new SandBoxDB.Concepto
                    {
                        DescripcionConcepto = Reg_Descripción,
                        DescripcionAlternaConcepto = Reg_DescripciónAlterna,
                        FundamentoLegal = Reg_FundamentoLegal,
                        Observaciones = Reg_Observaciones,
                        EstatusRegistro = Reg_EstatusRegistro,
                        FechaRegistro = DateTime.Today,
                        Fk_IdUsuarioRegistro = Reg_Fk_IdUsuarioRegistro
                    };
                    SandBoxCtx.Concepto.Add(conceptoNuevo);
                    SandBoxCtx.SaveChanges();

                    InicializaConceptos();
                    if (conceptosDBModificada != null) conceptosDBModificada();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n no se inserto el registro");
                }
                
            }
        }

        private void buttonLimpiaReg_Click(object sender, EventArgs e)
        {
            ClearRegConcepto();
        }

        private void disable_Reg_TextChanged()
        {
            textBoxDescripcion.TextChanged -= textBoxDescripcion_TextChanged;
            textBoxDescripcionAlterna.TextChanged -= textBoxDescripcionAlterna_TextChanged;
            textBoxFundamentoLegal.TextChanged -= textBoxFundamentoLegal_TextChanged;
            textBoxObservaciones.TextChanged -= textBoxObservaciones_TextChanged;
            comboBoxEstatusRegistro.SelectedIndexChanged -= comboBoxEstatusRegistro_SelectedIndexChanged;
            textBoxFechaReg.TextChanged -= textBoxFechaReg_TextChanged;
            textBoxIdUsuarioRegistro.TextChanged -= textBoxIdUsuarioRegistro_TextChanged;
        }

        private void enable_Reg_TextChanged()
        {
            textBoxDescripcion.TextChanged += textBoxDescripcion_TextChanged;
            textBoxDescripcionAlterna.TextChanged += textBoxDescripcionAlterna_TextChanged;
            textBoxFundamentoLegal.TextChanged += textBoxFundamentoLegal_TextChanged;
            textBoxObservaciones.TextChanged += textBoxObservaciones_TextChanged;
            comboBoxEstatusRegistro.SelectedIndexChanged += comboBoxEstatusRegistro_SelectedIndexChanged;
            textBoxFechaReg.TextChanged += textBoxFechaReg_TextChanged;
            textBoxIdUsuarioRegistro.TextChanged += textBoxIdUsuarioRegistro_TextChanged;
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Conceptos_Load(object sender, EventArgs e)
        {

        }
    }
}
