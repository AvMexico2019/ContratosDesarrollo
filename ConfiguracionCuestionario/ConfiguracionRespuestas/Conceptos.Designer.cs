namespace ConfiguracionCuestionario
{
    partial class Conceptos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBack = new System.Windows.Forms.Button();
            this.comboBoxConceptos = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxRelaciones = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelIdConcepto = new System.Windows.Forms.Label();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.textBoxDescripcionAlterna = new System.Windows.Forms.TextBox();
            this.textBoxFundamentoLegal = new System.Windows.Forms.TextBox();
            this.textBoxObservaciones = new System.Windows.Forms.TextBox();
            this.comboBoxEstatusRegistro = new System.Windows.Forms.ComboBox();
            this.textBoxFechaReg = new System.Windows.Forms.TextBox();
            this.textBoxIdUsuarioRegistro = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonModifica = new System.Windows.Forms.Button();
            this.buttonInserta = new System.Windows.Forms.Button();
            this.buttonLimpiaReg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(97, 233);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "Atras";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // comboBoxConceptos
            // 
            this.comboBoxConceptos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConceptos.FormattingEnabled = true;
            this.comboBoxConceptos.Location = new System.Drawing.Point(12, 29);
            this.comboBoxConceptos.Name = "comboBoxConceptos";
            this.comboBoxConceptos.Size = new System.Drawing.Size(403, 21);
            this.comboBoxConceptos.TabIndex = 1;
            this.comboBoxConceptos.SelectedIndexChanged += new System.EventHandler(this.comboBoxConceptos_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Conceptos";
            // 
            // listBoxRelaciones
            // 
            this.listBoxRelaciones.FormattingEnabled = true;
            this.listBoxRelaciones.Location = new System.Drawing.Point(12, 73);
            this.listBoxRelaciones.Name = "listBoxRelaciones";
            this.listBoxRelaciones.Size = new System.Drawing.Size(160, 121);
            this.listBoxRelaciones.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Se usa el concepto en los temas siguientes:";
            // 
            // labelIdConcepto
            // 
            this.labelIdConcepto.AutoSize = true;
            this.labelIdConcepto.Location = new System.Drawing.Point(536, 35);
            this.labelIdConcepto.Name = "labelIdConcepto";
            this.labelIdConcepto.Size = new System.Drawing.Size(62, 13);
            this.labelIdConcepto.TabIndex = 5;
            this.labelIdConcepto.Text = "IdConcepto";
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(310, 70);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(299, 20);
            this.textBoxDescripcion.TabIndex = 6;
            this.textBoxDescripcion.TextChanged += new System.EventHandler(this.textBoxDescripcion_TextChanged);
            // 
            // textBoxDescripcionAlterna
            // 
            this.textBoxDescripcionAlterna.Location = new System.Drawing.Point(310, 97);
            this.textBoxDescripcionAlterna.Name = "textBoxDescripcionAlterna";
            this.textBoxDescripcionAlterna.Size = new System.Drawing.Size(299, 20);
            this.textBoxDescripcionAlterna.TabIndex = 7;
            this.textBoxDescripcionAlterna.TextChanged += new System.EventHandler(this.textBoxDescripcionAlterna_TextChanged);
            // 
            // textBoxFundamentoLegal
            // 
            this.textBoxFundamentoLegal.Location = new System.Drawing.Point(310, 124);
            this.textBoxFundamentoLegal.Name = "textBoxFundamentoLegal";
            this.textBoxFundamentoLegal.Size = new System.Drawing.Size(299, 20);
            this.textBoxFundamentoLegal.TabIndex = 8;
            this.textBoxFundamentoLegal.TextChanged += new System.EventHandler(this.textBoxFundamentoLegal_TextChanged);
            // 
            // textBoxObservaciones
            // 
            this.textBoxObservaciones.Location = new System.Drawing.Point(310, 151);
            this.textBoxObservaciones.Name = "textBoxObservaciones";
            this.textBoxObservaciones.Size = new System.Drawing.Size(299, 20);
            this.textBoxObservaciones.TabIndex = 9;
            this.textBoxObservaciones.TextChanged += new System.EventHandler(this.textBoxObservaciones_TextChanged);
            // 
            // comboBoxEstatusRegistro
            // 
            this.comboBoxEstatusRegistro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEstatusRegistro.FormattingEnabled = true;
            this.comboBoxEstatusRegistro.Location = new System.Drawing.Point(310, 179);
            this.comboBoxEstatusRegistro.Name = "comboBoxEstatusRegistro";
            this.comboBoxEstatusRegistro.Size = new System.Drawing.Size(299, 21);
            this.comboBoxEstatusRegistro.TabIndex = 10;
            this.comboBoxEstatusRegistro.SelectedIndexChanged += new System.EventHandler(this.comboBoxEstatusRegistro_SelectedIndexChanged);
            // 
            // textBoxFechaReg
            // 
            this.textBoxFechaReg.Location = new System.Drawing.Point(310, 206);
            this.textBoxFechaReg.Name = "textBoxFechaReg";
            this.textBoxFechaReg.Size = new System.Drawing.Size(299, 20);
            this.textBoxFechaReg.TabIndex = 11;
            this.textBoxFechaReg.TextChanged += new System.EventHandler(this.textBoxFechaReg_TextChanged);
            // 
            // textBoxIdUsuarioRegistro
            // 
            this.textBoxIdUsuarioRegistro.Enabled = false;
            this.textBoxIdUsuarioRegistro.Location = new System.Drawing.Point(310, 233);
            this.textBoxIdUsuarioRegistro.Name = "textBoxIdUsuarioRegistro";
            this.textBoxIdUsuarioRegistro.Size = new System.Drawing.Size(299, 20);
            this.textBoxIdUsuarioRegistro.TabIndex = 12;
            this.textBoxIdUsuarioRegistro.TextChanged += new System.EventHandler(this.textBoxIdUsuarioRegistro_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(434, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "IdConcepto";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Descripción";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(198, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Desc Alterna";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(198, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Fundamento Legal";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(198, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Observaciones";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(198, 186);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Estatus Reg";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(198, 213);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Fecha Reg";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(198, 240);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Id Usuario Registro";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // buttonModifica
            // 
            this.buttonModifica.Location = new System.Drawing.Point(12, 204);
            this.buttonModifica.Name = "buttonModifica";
            this.buttonModifica.Size = new System.Drawing.Size(75, 23);
            this.buttonModifica.TabIndex = 21;
            this.buttonModifica.Text = "Modifica";
            this.buttonModifica.UseVisualStyleBackColor = true;
            this.buttonModifica.Click += new System.EventHandler(this.buttonModifica_Click);
            // 
            // buttonInserta
            // 
            this.buttonInserta.Location = new System.Drawing.Point(97, 203);
            this.buttonInserta.Name = "buttonInserta";
            this.buttonInserta.Size = new System.Drawing.Size(75, 23);
            this.buttonInserta.TabIndex = 22;
            this.buttonInserta.Text = "Inserta";
            this.buttonInserta.UseVisualStyleBackColor = true;
            this.buttonInserta.Click += new System.EventHandler(this.buttonInserta_Click);
            // 
            // buttonLimpiaReg
            // 
            this.buttonLimpiaReg.Location = new System.Drawing.Point(12, 233);
            this.buttonLimpiaReg.Name = "buttonLimpiaReg";
            this.buttonLimpiaReg.Size = new System.Drawing.Size(75, 23);
            this.buttonLimpiaReg.TabIndex = 23;
            this.buttonLimpiaReg.Text = "Limpia Reg";
            this.buttonLimpiaReg.UseVisualStyleBackColor = true;
            this.buttonLimpiaReg.Click += new System.EventHandler(this.buttonLimpiaReg_Click);
            // 
            // Conceptos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 273);
            this.ControlBox = false;
            this.Controls.Add(this.buttonLimpiaReg);
            this.Controls.Add(this.buttonInserta);
            this.Controls.Add(this.buttonModifica);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxIdUsuarioRegistro);
            this.Controls.Add(this.textBoxFechaReg);
            this.Controls.Add(this.comboBoxEstatusRegistro);
            this.Controls.Add(this.textBoxObservaciones);
            this.Controls.Add(this.textBoxFundamentoLegal);
            this.Controls.Add(this.textBoxDescripcionAlterna);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.labelIdConcepto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxRelaciones);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxConceptos);
            this.Controls.Add(this.buttonBack);
            this.Name = "Conceptos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conceptos";
            this.Load += new System.EventHandler(this.Conceptos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.ComboBox comboBoxConceptos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxRelaciones;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelIdConcepto;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.TextBox textBoxDescripcionAlterna;
        private System.Windows.Forms.TextBox textBoxFundamentoLegal;
        private System.Windows.Forms.TextBox textBoxObservaciones;
        private System.Windows.Forms.ComboBox comboBoxEstatusRegistro;
        private System.Windows.Forms.TextBox textBoxFechaReg;
        private System.Windows.Forms.TextBox textBoxIdUsuarioRegistro;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonModifica;
        private System.Windows.Forms.Button buttonInserta;
        private System.Windows.Forms.Button buttonLimpiaReg;
    }
}
