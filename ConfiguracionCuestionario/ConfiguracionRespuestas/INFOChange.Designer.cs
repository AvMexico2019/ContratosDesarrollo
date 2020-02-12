namespace ConfiguracionCuestionario
{
    partial class INFOChange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(INFOChange));
            this.listBoxINFOModificada = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxINFOModificada
            // 
            this.listBoxINFOModificada.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxINFOModificada, "listBoxINFOModificada");
            this.listBoxINFOModificada.Name = "listBoxINFOModificada";
            // 
            // INFOChange
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ControlBox = false;
            this.Controls.Add(this.listBoxINFOModificada);
            this.Name = "INFOChange";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxINFOModificada;
    }
}