namespace ConfiguracionCuestionario
{
    partial class Inicio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicio));
            this.buttonConceptos = new System.Windows.Forms.Button();
            this.buttonCuestionario = new System.Windows.Forms.Button();
            this.buttonSalir = new System.Windows.Forms.Button();
            this.buttonInicializaSandBox = new System.Windows.Forms.Button();
            this.buttonSandBox2dbo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonConceptos
            // 
            this.buttonConceptos.Location = new System.Drawing.Point(12, 293);
            this.buttonConceptos.Name = "buttonConceptos";
            this.buttonConceptos.Size = new System.Drawing.Size(75, 23);
            this.buttonConceptos.TabIndex = 0;
            this.buttonConceptos.Text = "Conceptos";
            this.buttonConceptos.UseVisualStyleBackColor = true;
            this.buttonConceptos.Click += new System.EventHandler(this.buttonConceptos_Click);
            // 
            // buttonCuestionario
            // 
            this.buttonCuestionario.Location = new System.Drawing.Point(122, 293);
            this.buttonCuestionario.Name = "buttonCuestionario";
            this.buttonCuestionario.Size = new System.Drawing.Size(75, 23);
            this.buttonCuestionario.TabIndex = 1;
            this.buttonCuestionario.Text = "Cuestionario";
            this.buttonCuestionario.UseVisualStyleBackColor = true;
            this.buttonCuestionario.Click += new System.EventHandler(this.buttonCuestionario_Click);
            // 
            // buttonSalir
            // 
            this.buttonSalir.Location = new System.Drawing.Point(222, 293);
            this.buttonSalir.Name = "buttonSalir";
            this.buttonSalir.Size = new System.Drawing.Size(53, 23);
            this.buttonSalir.TabIndex = 2;
            this.buttonSalir.Text = "Salir";
            this.buttonSalir.UseVisualStyleBackColor = true;
            this.buttonSalir.Click += new System.EventHandler(this.buttonSalir_Click);
            // 
            // buttonInicializaSandBox
            // 
            this.buttonInicializaSandBox.Location = new System.Drawing.Point(13, 152);
            this.buttonInicializaSandBox.Name = "buttonInicializaSandBox";
            this.buttonInicializaSandBox.Size = new System.Drawing.Size(262, 64);
            this.buttonInicializaSandBox.TabIndex = 3;
            this.buttonInicializaSandBox.Text = "Inicializa SandBox, elimina cualquier información en la base SandBox y recrea la " +
    "estructura necesaria";
            this.buttonInicializaSandBox.UseVisualStyleBackColor = true;
            this.buttonInicializaSandBox.Click += new System.EventHandler(this.buttonInicializaSandBox_Click);
            // 
            // buttonSandBox2dbo
            // 
            this.buttonSandBox2dbo.Location = new System.Drawing.Point(13, 222);
            this.buttonSandBox2dbo.Name = "buttonSandBox2dbo";
            this.buttonSandBox2dbo.Size = new System.Drawing.Size(263, 65);
            this.buttonSandBox2dbo.TabIndex = 4;
            this.buttonSandBox2dbo.Text = "Copia la Base de Datos SandBox a la base de datos del Sistema. SE PUEDE PERDER IN" +
    "FORMACION";
            this.buttonSandBox2dbo.UseVisualStyleBackColor = true;
            this.buttonSandBox2dbo.Click += new System.EventHandler(this.buttonSandBox2dbo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 117);
            this.label1.TabIndex = 6;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 331);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSandBox2dbo);
            this.Controls.Add(this.buttonInicializaSandBox);
            this.Controls.Add(this.buttonSalir);
            this.Controls.Add(this.buttonCuestionario);
            this.Controls.Add(this.buttonConceptos);
            this.Name = "Inicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuracion Conceptos y Cuestionario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConceptos;
        private System.Windows.Forms.Button buttonCuestionario;
        private System.Windows.Forms.Button buttonSalir;
        private System.Windows.Forms.Button buttonInicializaSandBox;
        private System.Windows.Forms.Button buttonSandBox2dbo;
        private System.Windows.Forms.Label label1;
    }
}
