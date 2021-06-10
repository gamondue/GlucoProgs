
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPredictHypo));
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
            this.txtGlucoseTarget = new System.Windows.Forms.TextBox();
            this.txtPredictedMinute = new System.Windows.Forms.TextBox();
            this.txtPredictedHour = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtAlarmMinute = new System.Windows.Forms.TextBox();
            this.txtAlarmHour = new System.Windows.Forms.TextBox();
            this.btnSetAlarm = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPaste = new System.Windows.Forms.Button();
            this.txtAlarmAdvanceTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
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
            this.txtGlucoseSlope.BackColor = System.Drawing.Color.AliceBlue;
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
            this.btnNow.Location = new System.Drawing.Point(27, 12);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(75, 39);
            this.btnNow.TabIndex = 15;
            this.btnNow.Text = "Now";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // btnPredict
            // 
            this.btnPredict.Location = new System.Drawing.Point(270, 12);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(75, 39);
            this.btnPredict.TabIndex = 16;
            this.btnPredict.Text = "Predict";
            this.btnPredict.UseVisualStyleBackColor = true;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            // 
            // txtGlucoseTarget
            // 
            this.txtGlucoseTarget.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtGlucoseTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucoseTarget.Location = new System.Drawing.Point(27, 89);
            this.txtGlucoseTarget.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseTarget.Name = "txtGlucoseTarget";
            this.txtGlucoseTarget.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseTarget.TabIndex = 0;
            this.txtGlucoseTarget.Text = "75";
            this.txtGlucoseTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtGlucoseTarget, "Measured glucose to which we take anti-hypoglicemia action");
            // 
            // txtPredictedMinute
            // 
            this.txtPredictedMinute.BackColor = System.Drawing.Color.SkyBlue;
            this.txtPredictedMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPredictedMinute.Location = new System.Drawing.Point(179, 308);
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
            this.txtPredictedHour.Location = new System.Drawing.Point(103, 308);
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
            this.label5.Location = new System.Drawing.Point(3, 283);
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
            // txtAlarmMinute
            // 
            this.txtAlarmMinute.BackColor = System.Drawing.Color.SkyBlue;
            this.txtAlarmMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlarmMinute.Location = new System.Drawing.Point(178, 371);
            this.txtAlarmMinute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlarmMinute.Name = "txtAlarmMinute";
            this.txtAlarmMinute.Size = new System.Drawing.Size(68, 26);
            this.txtAlarmMinute.TabIndex = 31;
            this.txtAlarmMinute.Text = "XXXX";
            this.txtAlarmMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAlarmMinute, "Minute in which glucose should reach target level");
            // 
            // txtAlarmHour
            // 
            this.txtAlarmHour.BackColor = System.Drawing.Color.SkyBlue;
            this.txtAlarmHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlarmHour.Location = new System.Drawing.Point(102, 371);
            this.txtAlarmHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlarmHour.Name = "txtAlarmHour";
            this.txtAlarmHour.Size = new System.Drawing.Size(68, 26);
            this.txtAlarmHour.TabIndex = 30;
            this.txtAlarmHour.Text = "XXXX";
            this.txtAlarmHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAlarmHour, "Hour in which glucose should reach target level");
            // 
            // btnSetAlarm
            // 
            this.btnSetAlarm.Location = new System.Drawing.Point(270, 365);
            this.btnSetAlarm.Name = "btnSetAlarm";
            this.btnSetAlarm.Size = new System.Drawing.Size(75, 39);
            this.btnSetAlarm.TabIndex = 26;
            this.btnSetAlarm.Text = "Alarm";
            this.toolTip1.SetToolTip(this.btnSetAlarm, "Set an alarm at alarm time");
            this.btnSetAlarm.UseVisualStyleBackColor = true;
            this.btnSetAlarm.Click += new System.EventHandler(this.btnSetAlarm_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(103, 12);
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
            this.label8.Location = new System.Drawing.Point(174, 243);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Slope [gluc/h]";
            // 
            // btnPaste
            // 
            this.btnPaste.Enabled = false;
            this.btnPaste.Location = new System.Drawing.Point(270, 83);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 39);
            this.btnPaste.TabIndex = 27;
            this.btnPaste.TabStop = false;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // txtAlarmAdvanceTime
            // 
            this.txtAlarmAdvanceTime.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.txtAlarmAdvanceTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlarmAdvanceTime.Location = new System.Drawing.Point(179, 89);
            this.txtAlarmAdvanceTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlarmAdvanceTime.Name = "txtAlarmAdvanceTime";
            this.txtAlarmAdvanceTime.Size = new System.Drawing.Size(68, 26);
            this.txtAlarmAdvanceTime.TabIndex = 28;
            this.txtAlarmAdvanceTime.Text = "30";
            this.txtAlarmAdvanceTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 69);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 15);
            this.label4.TabIndex = 18;
            this.label4.Tag = "";
            this.label4.Text = "Target hypoglycemia glucose";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(141, 54);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 15);
            this.label9.TabIndex = 29;
            this.label9.Tag = "";
            this.label9.Text = "Alarm advance time [min]";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(131, 346);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 20);
            this.label10.TabIndex = 32;
            this.label10.Text = "alarm time";
            // 
            // frmPredictHypo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 412);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAlarmMinute);
            this.Controls.Add(this.txtAlarmHour);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtAlarmAdvanceTime);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnSetAlarm);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmPredictHypo";
            this.Text = "Predict Hypo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPredictHypo_FormClosing);
            this.Load += new System.EventHandler(this.frmPredictHypo_Load);
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
        private System.Windows.Forms.TextBox txtGlucoseTarget;
        private System.Windows.Forms.TextBox txtPredictedMinute;
        private System.Windows.Forms.TextBox txtPredictedHour;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSetAlarm;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.TextBox txtAlarmAdvanceTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAlarmMinute;
        private System.Windows.Forms.TextBox txtAlarmHour;
    }
}

