
namespace GlucoMan_Forms_Core
{
    partial class frmCorrectionBolus
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtChoInsulinRatioBreakfast = new System.Windows.Forms.TextBox();
            this.txtChoInsulinRatioLunch = new System.Windows.Forms.TextBox();
            this.txtChoInsulinRatioDinner = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.txtChoInsulinRatioBreakfast);
            this.groupBox2.Controls.Add(this.txtChoInsulinRatioLunch);
            this.groupBox2.Controls.Add(this.txtChoInsulinRatioDinner);
            this.groupBox2.Location = new System.Drawing.Point(21, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 87);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ratio CHO / insulin (from dietist)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(180, 24);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 20);
            this.label20.TabIndex = 64;
            this.label20.Text = "Dinner";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(106, 24);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 20);
            this.label21.TabIndex = 63;
            this.label21.Text = "Lunch";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(17, 24);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(78, 20);
            this.label22.TabIndex = 62;
            this.label22.Text = "Breakfast";
            // 
            // txtChoInsulinRatioBreakfast
            // 
            this.txtChoInsulinRatioBreakfast.BackColor = System.Drawing.Color.PaleGreen;
            this.txtChoInsulinRatioBreakfast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinRatioBreakfast.Location = new System.Drawing.Point(22, 49);
            this.txtChoInsulinRatioBreakfast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinRatioBreakfast.Name = "txtChoInsulinRatioBreakfast";
            this.txtChoInsulinRatioBreakfast.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinRatioBreakfast.TabIndex = 1;
            this.txtChoInsulinRatioBreakfast.Text = "4,5";
            this.txtChoInsulinRatioBreakfast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChoInsulinRatioLunch
            // 
            this.txtChoInsulinRatioLunch.BackColor = System.Drawing.Color.PaleGreen;
            this.txtChoInsulinRatioLunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinRatioLunch.Location = new System.Drawing.Point(98, 49);
            this.txtChoInsulinRatioLunch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinRatioLunch.Name = "txtChoInsulinRatioLunch";
            this.txtChoInsulinRatioLunch.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinRatioLunch.TabIndex = 3;
            this.txtChoInsulinRatioLunch.Tag = "";
            this.txtChoInsulinRatioLunch.Text = "7";
            this.txtChoInsulinRatioLunch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChoInsulinRatioDinner
            // 
            this.txtChoInsulinRatioDinner.BackColor = System.Drawing.Color.PaleGreen;
            this.txtChoInsulinRatioDinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinRatioDinner.Location = new System.Drawing.Point(174, 49);
            this.txtChoInsulinRatioDinner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinRatioDinner.Name = "txtChoInsulinRatioDinner";
            this.txtChoInsulinRatioDinner.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinRatioDinner.TabIndex = 6;
            this.txtChoInsulinRatioDinner.Text = "6,5";
            this.txtChoInsulinRatioDinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmCorrectionBolus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 539);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmCorrectionBolus";
            this.Text = "Correction Bolus";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtChoInsulinRatioBreakfast;
        private System.Windows.Forms.TextBox txtChoInsulinRatioLunch;
        private System.Windows.Forms.TextBox txtChoInsulinRatioDinner;
    }
}