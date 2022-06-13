
namespace GlucoMan.Forms
{
    partial class frmAlarms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlarms));
            this.btnSetAlarm = new System.Windows.Forms.Button();
            this.txtHeaderText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSetAlarm
            // 
            this.btnSetAlarm.Location = new System.Drawing.Point(28, 128);
            this.btnSetAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetAlarm.Name = "btnSetAlarm";
            this.btnSetAlarm.Size = new System.Drawing.Size(96, 64);
            this.btnSetAlarm.TabIndex = 0;
            this.btnSetAlarm.Text = "Set alarm";
            this.btnSetAlarm.UseVisualStyleBackColor = true;
            // 
            // txtHeaderText
            // 
            this.txtHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHeaderText.Location = new System.Drawing.Point(124, 13);
            this.txtHeaderText.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtHeaderText.Multiline = true;
            this.txtHeaderText.Name = "txtHeaderText";
            this.txtHeaderText.Size = new System.Drawing.Size(149, 117);
            this.txtHeaderText.TabIndex = 6;
            // 
            // frmAlarms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 208);
            this.Controls.Add(this.txtHeaderText);
            this.Controls.Add(this.btnSetAlarm);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAlarms";
            this.Text = "Alarms";
            this.Load += new System.EventHandler(this.frmAlarms_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnSetAlarm;
        private TextBox txtHeaderText;
    }
}