using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace ConfiguracionCuestionario
{
    public partial class Cuestionario : Form
    {
        DateTime NullDateTime = new DateTime(1900, 1, 1);
        
        public Cuestionario()
        {
            InitializeComponent();
            InicializaCustionario();
            ClearRegistro();
            this.FormClosing += Cuestionario_FormClosing;
        }

        private void Cuestionario_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RegistroModificado)
            {
                MessageBox.Show("Modificaste los dato del registro y los vas a perder");
            }
        }

        public void Actualiza_ConceptosDB()
        {
            int tema = comboBoxTema.SelectedIndex;
            ListTema memTemaSeleccionado = TemaSeleccionado;
            InicializaCustionario();
            comboBoxTema.SelectedIndex = tema;
            TemaSeleccionado = memTemaSeleccionado;
        }

        class MyConcepto
        {
            public int Indice;
            public SandBoxDB.Concepto concepto;
        }

        List<ListTema> temas = new List<ListTema>();
        List<MyConcepto> conceptos = new List<MyConcepto>();
        List<SandBoxDB.Respuesta> respuestas = new List<SandBoxDB.Respuesta>();
        List<SandBoxDB.Rel_ConceptoRespValor> ListaPreguntasTema = new List<SandBoxDB.Rel_ConceptoRespValor>();
        ListTema TemaSeleccionado;
        SandBoxDB.Rel_ConceptoRespValor preguntaSeleccionada;
        bool _RegistroModificado;
        
        bool RegistroModificado
        {
            get { return _RegistroModificado; }
            set {
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

        private Nullable<int> Reg_IdInstitucion
        {
            get {
                if (textBoxIdInstitucion.Text.Equals("NULL"))
                    return null;
                else
                    return int.Parse(textBoxIdInstitucion.Text);
            }
            set { textBoxIdInstitucion.Text = (value == null) ? "NULL" :  value.ToString(); }
        }

        private Nullable<byte> Reg_IdTema
        {
            get { if (labelTEMA.Text.Equals("IdTema")) return null; else return temas.Find(t => t.Descripcion.Equals(labelTEMA.Text)).IdTema; }
            set { labelTEMA.Text = (value == null) ? "IdTema" : temas.Find(t => t.IdTema == value).Descripcion; }
        }

        private int Reg_IdConcepto 
        {
            get { return (comboBoxIdConcepto.SelectedIndex == -1) ? -1 : conceptos.Find(c => c.concepto.DescripcionConcepto.Equals(GetSItem(comboBoxIdConcepto.SelectedItem.ToString()))).concepto.IdConcepto; }
            set
            {
                if (value == -1)
                {
                    comboBoxIdConcepto.ResetText();
                }
                else
                comboBoxIdConcepto.SelectedIndex = conceptos.Find(r => r.concepto.IdConcepto == value).Indice;
            }
        }

        private int Reg_IdRespuesta
        {
            get { return (comboBoxIdRespuesta.SelectedIndex == -1) ? -1 : respuestas.Find(r => r.DescripcionRespuesta.Equals(comboBoxIdRespuesta.SelectedItem.ToString())).IdRespuesta; }
            set
            {
                if (value == -1)
                {
                    comboBoxIdRespuesta.ResetText();   
                }
                else
                    comboBoxIdRespuesta.SelectedIndex = comboBoxIdRespuesta.Items.IndexOf(respuestas.Find(r => r.IdRespuesta == value).DescripcionRespuesta);
            }
        }

        private decimal Reg_NumOrden
        {
            get
            { return (textBoxNumOrden.Text.Equals(""))? (decimal)-1.0 : Convert.ToDecimal(textBoxNumOrden.Text); }
            set
            {
                if (value == -1)
                {
                    textBoxNumOrden.Text = "";
                }
                else
                    textBoxNumOrden.Text = value.ToString();
            }
        }

        private Nullable<bool> Reg_EsDeterminante
        {
            get
            {
                int index;
                if ((index = comboBoxEsDeterminante.SelectedIndex) == -1)
                    return null;
                else
                {
                    return (bool)comboBoxEsDeterminante.Items[index];
                }
            }
            set {
                if (value == null)
                {
                    comboBoxEsDeterminante.SelectedIndex = -1;
                }
                else
                    comboBoxEsDeterminante.SelectedIndex = comboBoxEsDeterminante.Items.IndexOf(value); }
        }

        private Nullable<decimal> Reg_ValorRespuesta
        {
            get
            {
                if (textBoxValorRespuesta.Text.Equals("NULL"))
                    return null;
                else
                    return Convert.ToDecimal(textBoxValorRespuesta.Text);
            }
            set { textBoxValorRespuesta.Text = value == null ? "NULL" : value.ToString(); }
        }

        private Nullable<decimal> Reg_ValorMinimo
        {
            get {
                if (textBoxValorMinimo.Text.Equals("NULL"))
                    return null;
                else
                    return Convert.ToDecimal(textBoxValorMinimo.Text);
            }
            set { textBoxValorMinimo.Text = value == null ? "NULL" : value.ToString(); }
        }

        private Nullable<decimal> Reg_ValorMaximo
        {
            get
            {
                if (textBoxValorMaximo.Text.Equals("NULL"))
                    return null;
                else
                    return Convert.ToDecimal(textBoxValorMaximo.Text);
            }
            set { textBoxValorMaximo.Text = value == null ? "NULL" : value.ToString(); }
        }

        private String Reg_Comentario
        {
            get
            {
                return textBoxComentario.Text.Equals("NULL") ? "" : textBoxComentario.Text;
            }
            set
            {
                textBoxComentario.Text = value == null ? "NULL" : value;
            }
        }

        private Nullable<bool> Reg_EstatusRegistro
        {
            get
            {
                int index;
                if ((index = comboBoxEstatusRegistro.SelectedIndex) == -1)
                    return null;
                else
                {
                    return (bool)comboBoxEstatusRegistro.Items[index];
                }
            }
            set { comboBoxEstatusRegistro.SelectedIndex = value == null ? -1 : comboBoxEstatusRegistro.Items.IndexOf(value); }
        }

        private int Reg_IdUsuarioRegistro
        {
            get
            {
                int index;
                if ((index = comboBoxIdUsuarioRegistro.SelectedIndex) == -1)
                    return index;
                else
                    return (int) comboBoxIdUsuarioRegistro.Items[index];
            }
            set { comboBoxIdUsuarioRegistro.SelectedIndex = value == 1 ? -1 : comboBoxIdUsuarioRegistro.Items.IndexOf(value); }
        }

        private System.DateTime Reg_FechaRegistro
        {
            get
            {
                if (labelFechaRegistro.Text.Equals("Fecha Registro") ||
                    labelFechaRegistro.Text.Equals(""))
                    return DateTime.Now;
                else
                    return Convert.ToDateTime(labelFechaRegistro.Text);
            }
            set { labelFechaRegistro.Text = value.Equals(NullDateTime) ? "Fecha Registro" : value.ToLongDateString(); }
        }

        private void InicializaCustionario()
        {
            temas.Clear();
            conceptos.Clear();
            respuestas.Clear();
            TemaSeleccionado = null;
            SandBoxDB.SandBoxEntities ctx = null;
            try
            {
                ctx = new SandBoxDB.SandBoxEntities();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n no podemos continuar");
                Application.Exit();
            }

            temas = (from tema in ctx.Cat_Tema
                     select new ListTema { IdTema = tema.IdTema, Descripcion = tema.DescripcionTema }).ToList();
            int comboBoxIndex = 0;
            comboBoxTema.Items.Clear();
            foreach (ListTema tema in temas)
            {
                tema.comboBoxIndex = comboBoxIndex++;
                comboBoxTema.Items.Add(tema.Descripcion);
            }

            var result = from concepto in ctx.Concepto select concepto;
            int ConceptoIndice = 0;
            comboBoxIdConcepto.Items.Clear();
            foreach (var concepto in result)
            {
                conceptos.Add(new MyConcepto {Indice = ConceptoIndice++, concepto = concepto });
                comboBoxIdConcepto.Items.Add(concepto.IdConcepto + " - " + concepto.DescripcionConcepto);
            }

            var respuestasResult = from respuesta in ctx.Respuesta
                                   select respuesta;
            comboBoxIdRespuesta.Items.Clear();
            foreach (var respuesta in respuestasResult)
            {
                respuestas.Add(respuesta);
                comboBoxIdRespuesta.Items.Add(respuesta.DescripcionRespuesta);
            }

            var EsDeterminante = (from pregunta in ctx.Rel_ConceptoRespValor
                                   select pregunta.EsDeterminante).Distinct();
            comboBoxEsDeterminante.Items.Clear();
            foreach (bool estatus in EsDeterminante)
            {
                comboBoxEsDeterminante.Items.Add(estatus);
            }

            var EstatusRegistro = (from pregunta in ctx.Rel_ConceptoRespValor
                                   select pregunta.EstatusRegistro).Distinct();
            comboBoxEstatusRegistro.Items.Clear();
            foreach (bool estatus in EstatusRegistro)
            {
                comboBoxEstatusRegistro.Items.Add(estatus);
            }

            var IdUsuario = (from pregunta in ctx.Rel_ConceptoRespValor
                             select pregunta.Fk_IdUsuarioRegistro).Distinct();
            comboBoxIdUsuarioRegistro.Items.Clear();
            foreach (int usr in IdUsuario)
            {
                comboBoxIdUsuarioRegistro.Items.Add(usr);
            }
            listBoxPreguntas.HorizontalScrollbar = true;
            
            labelTEMA.Text = "";
            ctx.Dispose();
        }

        
        private void Cuestionario_Load(object sender, EventArgs e)
        {

        }

        private void DeleteSelected_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 6;
            progressBar.Value = 0;
            progressBar.Value++;
            int tema = comboBoxTema.SelectedIndex;
            ListTema memTemaSeleccionado = TemaSeleccionado;
            try
            {
                SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                progressBar.Value++;
                var reg = new SandBoxDB.Rel_ConceptoRespValor() { IdConceptoRespValor = preguntaSeleccionada.IdConceptoRespValor };
                SandBoxCtx.Rel_ConceptoRespValor.Attach(reg);
                SandBoxCtx.Rel_ConceptoRespValor.Remove(reg);
                progressBar.Value++;
                SandBoxCtx.SaveChanges();
                progressBar.Value++;
                InicializaCustionario();
                progressBar.Value++;
                TemaSeleccionado = memTemaSeleccionado;
                ShowCuestionario(TemaSeleccionado.IdTema);
                comboBoxTema.SelectedIndex = tema;
                progressBar.Value++;
                ClearRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n no se borro el registro");
            }
            progressBar.Value = 0;
        }

        private void ShowCuestionario(int Tema)
        {
            try
            {
                SandBoxDB.SandBoxEntities ctx = new SandBoxDB.SandBoxEntities();

                List<SandBoxDB.Rel_ConceptoRespValor> cuestionario = (from pregunta in ctx.Rel_ConceptoRespValor
                                                                      where pregunta.Fk_IdTema == Tema
                                                                      orderby pregunta.Fk_IdTema, pregunta.NumOrden
                                                                      select pregunta).ToList();
                ListaPreguntasTema.Clear();
                listBoxPreguntas.Items.Clear();
                foreach (var pregunta in cuestionario)
                {
                    ListaPreguntasTema.Add(pregunta);
                    listBoxPreguntas.Items.Add(pregunta.NumOrden + " - " + conceptos.Find(c => c.concepto.IdConcepto == pregunta.Fk_IdConcepto).concepto.DescripcionConcepto);
                }
                labelTEMA.Text = TemaSeleccionado.Descripcion;
                ctx.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n no se puede continuar pos 1");
                Application.Exit();
            }
            
        }

        private void comboBoxTema_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemaSeleccionado = temas.Find(t => t.comboBoxIndex == comboBoxTema.SelectedIndex);
            ShowCuestionario(TemaSeleccionado.IdTema);
        }

        private void listBoxPreguntas_SelectedIndexChanged(object sender, EventArgs e)
        {
            disable_Reg_TextChanged();
            preguntaSeleccionada = ListaPreguntasTema[listBoxPreguntas.SelectedIndex];
            Reg_IdInstitucion = preguntaSeleccionada.Fk_IdInstitucion;
            Reg_IdTema = preguntaSeleccionada.Fk_IdTema;
            Reg_IdConcepto = preguntaSeleccionada.Fk_IdConcepto;
            Reg_IdRespuesta = preguntaSeleccionada.Fk_IdRespuesta;
            Reg_NumOrden = preguntaSeleccionada.NumOrden;
            Reg_EsDeterminante = preguntaSeleccionada.EsDeterminante;
            Reg_ValorRespuesta = preguntaSeleccionada.ValorRespuesta;
            Reg_ValorMinimo = preguntaSeleccionada.ValorMinimo;
            Reg_ValorMaximo = preguntaSeleccionada.ValorMaximo;
            Reg_Comentario = preguntaSeleccionada.Comentario;
            Reg_EstatusRegistro = preguntaSeleccionada.EstatusRegistro;
            Reg_IdUsuarioRegistro = preguntaSeleccionada.Fk_IdUsuarioRegistro;
            Reg_FechaRegistro = preguntaSeleccionada.FechaRegistro;
            enable_Reg_TextChanged();
        }

        private void ClearRegistro()
        { 
            Reg_IdInstitucion = null;
            Reg_IdTema = null;
            Reg_IdConcepto = -1;
            Reg_IdRespuesta = -1;
            Reg_NumOrden = -1;
            Reg_EsDeterminante = null;
            Reg_ValorRespuesta = null;
            Reg_ValorMinimo = null;
            Reg_ValorMaximo = null;
            Reg_Comentario = null;
            Reg_EstatusRegistro = null;
            Reg_IdUsuarioRegistro = 1;
            Reg_FechaRegistro = NullDateTime;
            RegistroModificado = false;
        }

        private string GetSItem(string item)
        {
            return Regex.Replace(item,@"\d(.){0,1}\d* - ","");
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void buttonInsertRelacion_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 11;
            progressBar.Value = 0;
            progressBar.Value++;
            if (Reg_IdTema == null)
            {
                MessageBox.Show("Seleccionar el tema al que pertenece la pregunta");
                return;
            }
            progressBar.Value++;
            if (Reg_EsDeterminante == null)
            {
                MessageBox.Show("Definir si es determinante o no");
                return;
            }
            progressBar.Value++;
            if (Reg_ValorMinimo == null)
            {
                MessageBox.Show("Definir el valor mínimo");
                return;
            }
            progressBar.Value++;
            if (Reg_ValorMaximo == null)
            {
                MessageBox.Show("Definir el valor máximo");
                return;
            }
            progressBar.Value++;
            if (Reg_EstatusRegistro == null)
            {
                MessageBox.Show("Definir el estatus del registro");
                return;
            }
            progressBar.Value++;
            if (Reg_IdUsuarioRegistro == -1)
            {
                MessageBox.Show("Definir Id Usuario Registro");
                return;
            }
            progressBar.Value++;
            if (RegistroModificado)
            {
                int tema = comboBoxTema.SelectedIndex;
                ListTema memTemaSeleccionado = TemaSeleccionado;
                try
                {
                    SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                    progressBar.Value++;
                    SandBoxDB.Rel_ConceptoRespValor NuevaRelación = new SandBoxDB.Rel_ConceptoRespValor()
                    {
                        Fk_IdInstitucion = Reg_IdInstitucion,
                        Fk_IdTema = (byte)Reg_IdTema,
                        Fk_IdConcepto = Reg_IdConcepto,
                        Fk_IdRespuesta = Reg_IdRespuesta,
                        NumOrden = Reg_NumOrden,
                        EsDeterminante = (bool)Reg_EsDeterminante,
                        ValorRespuesta = Reg_ValorRespuesta,
                        ValorMinimo = (decimal)Reg_ValorMinimo,
                        ValorMaximo = (decimal)Reg_ValorMaximo,
                        Comentario = Reg_Comentario,
                        EstatusRegistro = (bool)Reg_EstatusRegistro,
                        Fk_IdUsuarioRegistro = Reg_IdUsuarioRegistro,
                        FechaRegistro = DateTime.Today
                    };
                    SandBoxCtx.Rel_ConceptoRespValor.Add(NuevaRelación);
                    progressBar.Value++;
                    SandBoxCtx.SaveChanges();
                    progressBar.Value++;
                    InicializaCustionario();
                    TemaSeleccionado = memTemaSeleccionado;
                    ShowCuestionario(TemaSeleccionado.IdTema);
                    progressBar.Value++;
                    comboBoxTema.SelectedIndex = tema;
                    ClearRegistro();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n no se inserto el registro");
                }
                
            }
            progressBar.Value = 0;
        }

        private void buttonModifica_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 11;
            progressBar.Value = 0;
            progressBar.Value++;
            int tema = comboBoxTema.SelectedIndex;
            ListTema memTemaSeleccionado = TemaSeleccionado;
            if (Reg_IdTema == null)
            {
                MessageBox.Show("Seleccionar el tema al que pertenece la pregunta");
                return;
            }
            progressBar.Value++;
            if (Reg_EsDeterminante == null)
            {
                MessageBox.Show("Definir si es determinante o no");
                return;
            }
            progressBar.Value++;
            if (Reg_ValorMinimo == null)
            {
                MessageBox.Show("Definir el valor mínimo");
                return;
            }
            progressBar.Value++;
            if (Reg_ValorMaximo == null)
            {
                MessageBox.Show("Definir el valor máximo");
                return;
            }
            progressBar.Value++;
            if (Reg_EstatusRegistro == null)
            {
                MessageBox.Show("Definir el estatus del registro");
                return;
            }
            progressBar.Value++;
            if (Reg_IdUsuarioRegistro == -1)
            {
                MessageBox.Show("Definir Id Usuario Registro");
                return;
            }
            progressBar.Value++;
            if (RegistroModificado)
            {
                if (preguntaSeleccionada != null)
                {
                    int reg = preguntaSeleccionada.IdConceptoRespValor;
                    try
                    {
                        SandBoxDB.SandBoxEntities SandBoxCtx = new SandBoxDB.SandBoxEntities();
                        var result = SandBoxCtx.Rel_ConceptoRespValor.SingleOrDefault(b => b.IdConceptoRespValor == reg);
                        progressBar.Value++;
                        if (result != null)
                        {
                            result.Fk_IdInstitucion = Reg_IdInstitucion;
                            result.Fk_IdTema = (byte)Reg_IdTema;
                            result.Fk_IdConcepto = Reg_IdConcepto;
                            result.Fk_IdRespuesta = Reg_IdRespuesta;
                            result.NumOrden = Reg_NumOrden;
                            result.EsDeterminante = (bool)Reg_EsDeterminante;
                            result.ValorRespuesta = Reg_ValorRespuesta;
                            result.ValorMinimo = (decimal)Reg_ValorMinimo;
                            result.ValorMaximo = (decimal)Reg_ValorMaximo;
                            result.Comentario = Reg_Comentario;
                            result.EstatusRegistro = (bool)Reg_EstatusRegistro;
                            result.Fk_IdUsuarioRegistro = Reg_IdUsuarioRegistro;
                            result.FechaRegistro = DateTime.Today;
                            progressBar.Value++;
                            SandBoxCtx.SaveChanges();
                            InicializaCustionario();
                            progressBar.Value++;
                            TemaSeleccionado = memTemaSeleccionado;
                            ShowCuestionario(TemaSeleccionado.IdTema);
                            progressBar.Value++;
                            comboBoxTema.SelectedIndex = tema;
                            ClearRegistro();
                        };
                    }
                    catch (Exception ex){
                        MessageBox.Show(ex.Message + "\n no se modifico el registro");
                    }
                    
                }
            }
            progressBar.Value = 0;
        }

        private void disable_Reg_TextChanged()
        {
            textBoxIdInstitucion.TextChanged -= textBoxIdInstitucion_TextChanged;
            comboBoxIdConcepto.SelectedIndexChanged -= comboBoxIdConcepto_SelectedIndexChanged;
            comboBoxIdRespuesta.SelectedIndexChanged -= comboBoxIdRespuesta_SelectedIndexChanged;
            textBoxNumOrden.TextChanged -= textBoxNumOrden_TextChanged;
            comboBoxEsDeterminante.SelectedIndexChanged -= comboBoxEsDeterminante_SelectedIndexChanged;
            textBoxValorRespuesta.TextChanged -= textBoxValorRespuesta_TextChanged;
            textBoxValorMinimo.TextChanged -= textBoxValorMinimo_TextChanged;
            textBoxValorMinimo.TextChanged -= textBoxValorMinimo_TextChanged;
            textBoxValorMaximo.TextChanged -= textBoxValorMaximo_TextChanged;
            textBoxComentario.TextChanged -= textBoxComentario_TextChanged;
            comboBoxEstatusRegistro.SelectedIndexChanged -= comboBoxEstatusRegistro_SelectedIndexChanged;
            comboBoxIdUsuarioRegistro.SelectedIndexChanged -= comboBoxIdUsuarioRegistro_SelectedIndexChanged;

        }

        private void enable_Reg_TextChanged()
        {
            textBoxIdInstitucion.TextChanged += textBoxIdInstitucion_TextChanged;
            comboBoxIdConcepto.SelectedIndexChanged += comboBoxIdConcepto_SelectedIndexChanged;
            comboBoxIdRespuesta.SelectedIndexChanged += comboBoxIdRespuesta_SelectedIndexChanged;
            textBoxNumOrden.TextChanged += textBoxNumOrden_TextChanged;
            comboBoxEsDeterminante.SelectedIndexChanged += comboBoxEsDeterminante_SelectedIndexChanged;
            textBoxValorRespuesta.TextChanged += textBoxValorRespuesta_TextChanged;
            textBoxValorMinimo.TextChanged += textBoxValorMinimo_TextChanged;
            textBoxValorMinimo.TextChanged += textBoxValorMinimo_TextChanged;
            textBoxValorMaximo.TextChanged += textBoxValorMaximo_TextChanged;
            textBoxComentario.TextChanged += textBoxComentario_TextChanged;
            comboBoxEstatusRegistro.SelectedIndexChanged += comboBoxEstatusRegistro_SelectedIndexChanged;
            comboBoxIdUsuarioRegistro.SelectedIndexChanged += comboBoxIdUsuarioRegistro_SelectedIndexChanged;
        }

        private void textBoxIdInstitucion_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void comboBoxIdConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void comboBoxIdRespuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxNumOrden_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void comboBoxEsDeterminante_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxValorRespuesta_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxValorMinimo_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxValorMaximo_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void textBoxComentario_TextChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void comboBoxEstatusRegistro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void comboBoxIdUsuarioRegistro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RegistroModificado = true;
        }

        private void buttonLimpiarRelación_Click(object sender, EventArgs e)
        {
            ClearRegistro();
        }

        private void buttonSetTema_Click(object sender, EventArgs e)
        {
            if (TemaSeleccionado != null)
                Reg_IdTema = TemaSeleccionado.IdTema;
        }
    }
}
