
namespace GlucoMan_Forms_Core
{
    partial class frmPredictHypo
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
            this.components = new System.ComponentModel.Container();
            this.txtGlucoseLast = new System.Windows.Forms.TextBox();
            this.txtHourLast = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMinuteLast = new System.Windows.Forms.TextBox();
            this.txtMinutePrevious = new System.Windows.Forms.TextBox();
            this.txtHourPrevious = new System.Windows.Forms.TextBox();
            this.txtGlucosePrevious = new System.Windows.Forms.TextBox();
            this.txtGlucoseSlope = new System.Windows.Forms.TextBox();
            this.btnNow = new System.Windows.Forms.Button();
            this.btnPredict = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGlucoseTarget = new System.Windows.Forms.TextBox();
            this.txtPredictedMinute = new System.Windows.Forms.TextBox();
            this.txtPredictedHour = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnNext = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtGlucoseLast
            // 
            this.txtGlucoseLast.BackColor = System.Drawing.Color.LightGreen;
            this.txtGlucoseLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucoseLast.Location = new System.Drawing.Point(27, 149);
            this.txtGlucoseLast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseLast.Name = "txtGlucoseLast";
            this.txtGlucoseLast.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseLast.TabIndex = 1;
            this.txtGlucoseLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGlucoseLast.Leave += new System.EventHandler(this.txtGlucoseLast_Leave);
            // 
            // txtHourLast
            // 
            this.txtHourLast.BackColor = System.Drawing.Color.LightGreen;
            this.txtHourLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHourLast.Location = new System.Drawing.Point(103, 149);
            this.txtHourLast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHourLast.Name = "txtHourLast";
            this.txtHourLast.Size = new System.Drawing.Size(68, 26);
            this.txtHourLast.TabIndex = 2;
            this.txtHourLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 120);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Glucose";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(115, 120);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hour";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 120);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Minute";
            // 
            // txtMinuteLast
            // 
            this.txtMinuteLast.BackColor = System.Drawing.Color.LightGreen;
            this.txtMinuteLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinuteLast.Location = new System.Drawing.Point(179, 149);
            this.txtMinuteLast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMinuteLast.Name = "txtMinuteLast";
            this.txtMinuteLast.Size = new System.Drawing.Size(68, 26);
            this.txtMinuteLast.TabIndex = 3;
            this.txtMinuteLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMinutePrevious
            // 
            this.txtMinutePrevious.BackColor = System.Drawing.Color.LightGreen;
            this.txtMinutePrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinutePrevious.Location = new System.Drawing.Point(179, 185);
            this.txtMinutePrevious.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMinutePrevious.Name = "txtMinutePrevious";
            this.txtMinutePrevious.Size = new System.Drawing.Size(68, 26);
            this.txtMinutePrevious.TabIndex = 8;
            this.txtMinutePrevious.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHourPrevious
            // 
            this.txtHourPrevious.BackColor = System.Drawing.Color.LightGreen;
            this.txtHourPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHourPrevious.Location = new System.Drawing.Point(103, 185);
            this.txtHourPrevious.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHourPrevious.Name = "txtHourPrevious";
            this.txtHourPrevious.Size = new System.Drawing.Size(68, 26);
            this.txtHourPrevious.TabIndex = 7;
            this.txtHourPrevious.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtGlucosePrevious
            // 
            this.txtGlucosePrevious.BackColor = System.Drawing.Color.LightGreen;
            this.txtGlucosePrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucosePrevious.Location = new System.Drawing.Point(27, 185);
            this.txtGlucosePrevious.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucosePrevious.Name = "txtGlucosePrevious";
            this.txtGlucosePrevious.Size = new System.Drawing.Size(68, 26);
            this.txtGlucosePrevious.TabIndex = 6;
            this.txtGlucosePrevious.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtGlucoseSlope
            // 
            this.txtGlucoseSlope.Enabled = false;
            this.txtGlucoseSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucoseSlope.Location = new System.Drawing.Point(103, 240);
            this.txtGlucoseSlope.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseSlope.Name = "txtGlucoseSlope";
            this.txtGlucoseSlope.ReadOnly = true;
            this.txtGlucoseSlope.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseSlope.TabIndex = 13;
            this.txtGlucoseSlope.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnNow
            // 
            this.btnNow.Location = new System.Drawing.Point(27, 9);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(75, 39);
            this.btnNow.TabIndex = 15;
            this.btnNow.Text = "Now";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // btnPredict
            // 
            this.btnPredict.Location = new System.Drawing.Point(179, 9);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(75, 39);
            this.btnPredict.TabIndex = 16;
            this.btnPredict.Text = "Predict";
            this.btnPredict.UseVisualStyleBackColor = true;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(271, 20);
            this.label4.TabIndex = 18;
            this.label4.Tag = "Measured glucose to which we take anti-hypoglicemia action";
            this.label4.Text = "Target glucose for hypoglycemia alert";
            // 
            // txtGlucoseTarget
            // 
            this.txtGlucoseTarget.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtGlucoseTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucoseTarget.Location = new System.Drawing.Point(103, 85);
            this.txtGlucoseTarget.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseTarget.Name = "txtGlucoseTarget";
            this.txtGlucoseTarget.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseTarget.TabIndex = 0;
            this.txtGlucoseTarget.Text = "80";
            this.txtGlucoseTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPredictedMinute
            // 
            this.txtPredictedMinute.BackColor = System.Drawing.Color.SkyBlue;
            this.txtPredictedMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPredictedMinute.Location = new System.Drawing.Point(179, 310);
            this.txtPredictedMinute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPredictedMinute.Name = "txtPredictedMinute";
            this.txtPredictedMinute.Size = new System.Drawing.Size(68, 26);
            this.txtPredictedMinute.TabIndex = 20;
            this.txtPredictedMinute.Text = "XXXX";
            this.txtPredictedMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPredictedMinute, "Minute in which glucose should reach target level");
            // 
            // txtPredictedHour
            // 
            this.txtPredictedHour.BackColor = System.Drawing.Color.SkyBlue;
            this.txtPredictedHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPredictedHour.Location = new System.Drawing.Point(103, 310);
            this.txtPredictedHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPredictedHour.Name = "txtPredictedHour";
            this.txtPredictedHour.Size = new System.Drawing.Size(68, 26);
            this.txtPredictedHour.TabIndex = 19;
            this.txtPredictedHour.Text = "XXXX";
            this.txtPredictedHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPredictedHour, "Hour in which glucose should reach target level");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 288);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(352, 20);
            this.label5.TabIndex = 21;
            this.label5.Text = "expected time of achievement of the target value";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(255, 152);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 20);
            this.label6.TabIndex = 22;
            this.label6.Text = "Current";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(255, 188);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.TabIndex = 23;
            this.label7.Text = "Previous";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(269, 9);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 39);
            this.btnNext.TabIndex = 24;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(185, 243);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Slope [gluc/h]";
            // 
            // frmPredictHypo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 344);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPredictedMinute);
            this.Controls.Add(this.txtPredictedHour);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtGlucoseTarget);
            this.Controls.Add(this.btnPredict);
            this.Controls.Add(this.btnNow);
            this.Controls.Add(this.txtGlucoseSlope);
            this.Controls.Add(this.txtMinutePrevious);
            this.Controls.Add(this.txtHourPrevious);
            this.Controls.Add(this.txtGlucosePrevious);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMinuteLast);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHourLast);
            this.Controls.Add(this.txtGlucoseLast);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmPredictHypo";
            this.Text = "Predict Hypo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPredictHypo_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGlucoseLast;
        private System.Windows.Forms.TextBox txtHourLast;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinuteLast;
        private System.Windows.Forms.TextBox txtMinutePrevious;
        private System.Windows.Forms.TextBox txtHourPrevious;
        private System.Windows.Forms.TextBox txtGlucosePrevious;
        private System.Windows.Forms.TextBox txtGlucoseSlope;
        private System.Windows.Forms.Button btnNow;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGlucoseTarget;
        private System.Windows.Forms.TextBox txtPredictedMinute;
        private System.Windows.Forms.TextBox txtPredictedHour;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label8;
    }
}

