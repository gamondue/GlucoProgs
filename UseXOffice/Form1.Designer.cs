namespace gamon.XOffice
{
    partial class Form1
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
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnWord = new System.Windows.Forms.Button();
            this.rdbMSOffice = new System.Windows.Forms.RadioButton();
            this.rdbLibreOffice = new System.Windows.Forms.RadioButton();
            this.btnWord_PDF = new System.Windows.Forms.Button();
            this.btnExcelPDF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(170, 51);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(112, 32);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "Spreadsheet";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnWord
            // 
            this.btnWord.Location = new System.Drawing.Point(34, 51);
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(112, 32);
            this.btnWord.TabIndex = 2;
            this.btnWord.Text = "Rich Text";
            this.btnWord.UseVisualStyleBackColor = true;
            this.btnWord.Click += new System.EventHandler(this.btnWord_Click);
            // 
            // rdbMSOffice
            // 
            this.rdbMSOffice.AutoSize = true;
            this.rdbMSOffice.Checked = true;
            this.rdbMSOffice.Location = new System.Drawing.Point(34, 107);
            this.rdbMSOffice.Name = "rdbMSOffice";
            this.rdbMSOffice.Size = new System.Drawing.Size(72, 17);
            this.rdbMSOffice.TabIndex = 3;
            this.rdbMSOffice.TabStop = true;
            this.rdbMSOffice.Text = "MS Office";
            this.rdbMSOffice.UseVisualStyleBackColor = true;
            // 
            // rdbLibreOffice
            // 
            this.rdbLibreOffice.AutoSize = true;
            this.rdbLibreOffice.Location = new System.Drawing.Point(34, 130);
            this.rdbLibreOffice.Name = "rdbLibreOffice";
            this.rdbLibreOffice.Size = new System.Drawing.Size(79, 17);
            this.rdbLibreOffice.TabIndex = 4;
            this.rdbLibreOffice.Text = "Libre Office";
            this.rdbLibreOffice.UseVisualStyleBackColor = true;
            // 
            // btnWord_PDF
            // 
            this.btnWord_PDF.Location = new System.Drawing.Point(34, 166);
            this.btnWord_PDF.Name = "btnWord_PDF";
            this.btnWord_PDF.Size = new System.Drawing.Size(112, 45);
            this.btnWord_PDF.TabIndex = 5;
            this.btnWord_PDF.Text = "Rich Text  PDF";
            this.btnWord_PDF.UseVisualStyleBackColor = true;
            this.btnWord_PDF.Click += new System.EventHandler(this.btnWord_PDF_Click);
            // 
            // btnExcelPDF
            // 
            this.btnExcelPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcelPDF.Location = new System.Drawing.Point(170, 166);
            this.btnExcelPDF.Name = "btnExcelPDF";
            this.btnExcelPDF.Size = new System.Drawing.Size(112, 45);
            this.btnExcelPDF.TabIndex = 6;
            this.btnExcelPDF.Text = "Spreadsheet PDF";
            this.btnExcelPDF.UseVisualStyleBackColor = true;
            this.btnExcelPDF.Click += new System.EventHandler(this.btnExcelPDF_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 243);
            this.Controls.Add(this.btnExcelPDF);
            this.Controls.Add(this.btnWord_PDF);
            this.Controls.Add(this.rdbLibreOffice);
            this.Controls.Add(this.rdbMSOffice);
            this.Controls.Add(this.btnWord);
            this.Controls.Add(this.btnExcel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnWord;
        private System.Windows.Forms.RadioButton rdbMSOffice;
        private System.Windows.Forms.RadioButton rdbLibreOffice;
        private System.Windows.Forms.Button btnWord_PDF;
        private System.Windows.Forms.Button btnExcelPDF;
    }
}

