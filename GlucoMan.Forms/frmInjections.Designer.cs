namespace GlucoMan.Forms
{
    partial class frmInjections
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInjections));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.tabFront = new System.Windows.Forms.TabPage();
            this.tabBack = new System.Windows.Forms.TabPage();
            this.tabSensor = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabData);
            this.tabControl1.Controls.Add(this.tabFront);
            this.tabControl1.Controls.Add(this.tabBack);
            this.tabControl1.Controls.Add(this.tabSensor);
            this.tabControl1.Location = new System.Drawing.Point(0, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1017, 619);
            this.tabControl1.TabIndex = 0;
            // 
            // tabData
            // 
            this.tabData.Location = new System.Drawing.Point(4, 30);
            this.tabData.Name = "tabData";
            this.tabData.Size = new System.Drawing.Size(1009, 585);
            this.tabData.TabIndex = 3;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // tabFront
            // 
            this.tabFront.Location = new System.Drawing.Point(4, 24);
            this.tabFront.Name = "tabFront";
            this.tabFront.Padding = new System.Windows.Forms.Padding(3);
            this.tabFront.Size = new System.Drawing.Size(1009, 591);
            this.tabFront.TabIndex = 0;
            this.tabFront.Text = "Front";
            this.tabFront.UseVisualStyleBackColor = true;
            // 
            // tabBack
            // 
            this.tabBack.Location = new System.Drawing.Point(4, 24);
            this.tabBack.Name = "tabBack";
            this.tabBack.Padding = new System.Windows.Forms.Padding(3);
            this.tabBack.Size = new System.Drawing.Size(1009, 591);
            this.tabBack.TabIndex = 1;
            this.tabBack.Text = "Back";
            this.tabBack.UseVisualStyleBackColor = true;
            // 
            // tabSensor
            // 
            this.tabSensor.Location = new System.Drawing.Point(4, 24);
            this.tabSensor.Name = "tabSensor";
            this.tabSensor.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensor.Size = new System.Drawing.Size(1009, 591);
            this.tabSensor.TabIndex = 2;
            this.tabSensor.Text = "Sensor";
            this.tabSensor.UseVisualStyleBackColor = true;
            // 
            // frmInjections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 622);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmInjections";
            this.Text = "Insuline Injections";
            this.Load += new System.EventHandler(this.frmInjections_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabFront;
        private TabPage tabBack;
        private TabPage tabSensor;
        private TabPage tabData;
    }
}