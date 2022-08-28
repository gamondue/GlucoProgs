namespace GlucoMan.Forms
{
    partial class frmMiscellaneous
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMiscellaneous));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_mgPerdL = new System.Windows.Forms.TextBox();
            this.txt_mmolPerL = new System.Windows.Forms.TextBox();
            this.btnResetDatabase = new System.Windows.Forms.Button();
            this.btnCopyDatabase = new System.Windows.Forms.Button();
            this.btnShowErrorLog = new System.Windows.Forms.Button();
            this.btnDeleteErrorLog = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnReadDatabase = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_mgPerdL);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Blood glucose unit conversion";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "mg/dL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "mmol/L";
            // 
            // txt_mgPerdL
            // 
            this.txt_mgPerdL.Location = new System.Drawing.Point(17, 62);
            this.txt_mgPerdL.Name = "txt_mgPerdL";
            this.txt_mgPerdL.Size = new System.Drawing.Size(100, 29);
            this.txt_mgPerdL.TabIndex = 0;
            this.txt_mgPerdL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_mgPerdL.TextChanged += new System.EventHandler(this.txt_mgPerdL_TextChanged);
            // 
            // txt_mmolPerL
            // 
            this.txt_mmolPerL.Location = new System.Drawing.Point(149, 74);
            this.txt_mmolPerL.Name = "txt_mmolPerL";
            this.txt_mmolPerL.Size = new System.Drawing.Size(100, 29);
            this.txt_mmolPerL.TabIndex = 1;
            this.txt_mmolPerL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_mmolPerL.TextChanged += new System.EventHandler(this.txt_mmolPerL_TextChanged);
            // 
            // btnResetDatabase
            // 
            this.btnResetDatabase.BackColor = System.Drawing.Color.Red;
            this.btnResetDatabase.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnResetDatabase.ForeColor = System.Drawing.Color.Yellow;
            this.btnResetDatabase.Location = new System.Drawing.Point(92, 119);
            this.btnResetDatabase.Name = "btnResetDatabase";
            this.btnResetDatabase.Size = new System.Drawing.Size(100, 63);
            this.btnResetDatabase.TabIndex = 2;
            this.btnResetDatabase.Text = "Reset database";
            this.btnResetDatabase.UseVisualStyleBackColor = false;
            this.btnResetDatabase.Click += new System.EventHandler(this.btnResetDatabase_Click);
            // 
            // btnCopyDatabase
            // 
            this.btnCopyDatabase.Location = new System.Drawing.Point(92, 188);
            this.btnCopyDatabase.Name = "btnCopyDatabase";
            this.btnCopyDatabase.Size = new System.Drawing.Size(100, 63);
            this.btnCopyDatabase.TabIndex = 4;
            this.btnCopyDatabase.Text = "Copy files";
            this.btnCopyDatabase.UseVisualStyleBackColor = true;
            this.btnCopyDatabase.Click += new System.EventHandler(this.btnCopyDatabase_Click);
            // 
            // btnShowErrorLog
            // 
            this.btnShowErrorLog.Location = new System.Drawing.Point(29, 326);
            this.btnShowErrorLog.Name = "btnShowErrorLog";
            this.btnShowErrorLog.Size = new System.Drawing.Size(100, 63);
            this.btnShowErrorLog.TabIndex = 5;
            this.btnShowErrorLog.Text = "Show error log";
            this.btnShowErrorLog.UseVisualStyleBackColor = true;
            this.btnShowErrorLog.Click += new System.EventHandler(this.btnShowErrorLog_Click);
            // 
            // btnDeleteErrorLog
            // 
            this.btnDeleteErrorLog.Location = new System.Drawing.Point(149, 326);
            this.btnDeleteErrorLog.Name = "btnDeleteErrorLog";
            this.btnDeleteErrorLog.Size = new System.Drawing.Size(100, 63);
            this.btnDeleteErrorLog.TabIndex = 6;
            this.btnDeleteErrorLog.Text = "Delete error log";
            this.btnDeleteErrorLog.UseVisualStyleBackColor = true;
            this.btnDeleteErrorLog.Click += new System.EventHandler(this.btnDeleteErrorLog_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(29, 257);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 63);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Import data";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnReadDatabase
            // 
            this.btnReadDatabase.BackColor = System.Drawing.Color.Red;
            this.btnReadDatabase.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReadDatabase.ForeColor = System.Drawing.Color.Yellow;
            this.btnReadDatabase.Location = new System.Drawing.Point(149, 257);
            this.btnReadDatabase.Name = "btnReadDatabase";
            this.btnReadDatabase.Size = new System.Drawing.Size(100, 63);
            this.btnReadDatabase.TabIndex = 8;
            this.btnReadDatabase.Text = "Read all database";
            this.btnReadDatabase.UseVisualStyleBackColor = false;
            this.btnReadDatabase.Click += new System.EventHandler(this.btnReadDatabase_Click);
            // 
            // frmMiscellaneous
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 402);
            this.Controls.Add(this.btnReadDatabase);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnDeleteErrorLog);
            this.Controls.Add(this.btnShowErrorLog);
            this.Controls.Add(this.btnCopyDatabase);
            this.Controls.Add(this.btnResetDatabase);
            this.Controls.Add(this.txt_mmolPerL);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMiscellaneous";
            this.Text = "Miscellaneous functions";
            this.Load += new System.EventHandler(this.frmMiscellaneous_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private TextBox txt_mgPerdL;
        private TextBox txt_mmolPerL;
        private Button btnResetDatabase;
        private Button btnCopyDatabase;
        private Button btnShowErrorLog;
        private Button btnDeleteErrorLog;
        private Button btnImport;
        private Button btnReadDatabase;
    }
}