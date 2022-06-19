namespace GlucoMan.Forms
{
    partial class frmInjectionSite
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFront = new System.Windows.Forms.TabPage();
            this.tabBack = new System.Windows.Forms.TabPage();
            this.tabSensor = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFront);
            this.tabControl1.Controls.Add(this.tabBack);
            this.tabControl1.Controls.Add(this.tabSensor);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1017, 619);
            this.tabControl1.TabIndex = 0;
            // 
            // tabFront
            // 
            this.tabFront.Location = new System.Drawing.Point(4, 30);
            this.tabFront.Name = "tabFront";
            this.tabFront.Padding = new System.Windows.Forms.Padding(3);
            this.tabFront.Size = new System.Drawing.Size(1009, 585);
            this.tabFront.TabIndex = 0;
            this.tabFront.Text = "Front";
            this.tabFront.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabBack.Location = new System.Drawing.Point(4, 30);
            this.tabBack.Name = "tabPage2";
            this.tabBack.Padding = new System.Windows.Forms.Padding(3);
            this.tabBack.Size = new System.Drawing.Size(1009, 585);
            this.tabBack.TabIndex = 1;
            this.tabBack.Text = "Back";
            this.tabBack.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabSensor.Location = new System.Drawing.Point(4, 30);
            this.tabSensor.Name = "tabPage1";
            this.tabSensor.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensor.Size = new System.Drawing.Size(1009, 585);
            this.tabSensor.TabIndex = 2;
            this.tabSensor.Text = "Sensor";
            this.tabSensor.UseVisualStyleBackColor = true;
            // 
            // frmInjectionSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 630);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmInjectionSite";
            this.Text = "Injection sites";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabFront;
        private TabPage tabBack;
        private TabPage tabSensor;
    }
}