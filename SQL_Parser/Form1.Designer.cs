namespace SQL_Parser
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.listLexico = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbLexico = new System.Windows.Forms.Label();
            this.lbSintactico = new System.Windows.Forms.Label();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listTables = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(12, 25);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(260, 70);
            this.txtQuery.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Query";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(76, 101);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // listLexico
            // 
            this.listLexico.FormattingEnabled = true;
            this.listLexico.Location = new System.Drawing.Point(12, 147);
            this.listLexico.Name = "listLexico";
            this.listLexico.Size = new System.Drawing.Size(197, 329);
            this.listLexico.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Analisis Lexico";
            // 
            // lbLexico
            // 
            this.lbLexico.AutoSize = true;
            this.lbLexico.Location = new System.Drawing.Point(12, 485);
            this.lbLexico.Name = "lbLexico";
            this.lbLexico.Size = new System.Drawing.Size(82, 13);
            this.lbLexico.TabIndex = 5;
            this.lbLexico.Text = "Analisis Lexico: ";
            // 
            // lbSintactico
            // 
            this.lbSintactico.AutoSize = true;
            this.lbSintactico.Location = new System.Drawing.Point(12, 498);
            this.lbSintactico.Name = "lbSintactico";
            this.lbSintactico.Size = new System.Drawing.Size(98, 13);
            this.lbSintactico.TabIndex = 6;
            this.lbSintactico.Text = "Analisis Sintactico: ";
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(325, 25);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(290, 70);
            this.txtMsg.TabIndex = 7;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(215, 147);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(197, 329);
            this.listBox1.TabIndex = 8;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listTables
            // 
            this.listTables.FormattingEnabled = true;
            this.listTables.Location = new System.Drawing.Point(418, 147);
            this.listTables.Name = "listTables";
            this.listTables.Size = new System.Drawing.Size(197, 329);
            this.listTables.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 520);
            this.Controls.Add(this.listTables);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.lbSintactico);
            this.Controls.Add(this.lbLexico);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listLexico);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtQuery);
            this.Name = "Form1";
            this.Text = "SQL Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.ListBox listLexico;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbLexico;
        private System.Windows.Forms.Label lbSintactico;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listTables;
    }
}

