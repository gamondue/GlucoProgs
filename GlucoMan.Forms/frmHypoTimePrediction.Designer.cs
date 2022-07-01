
namespace GlucoMan.Forms
{
    partial class frmHypoTimePrediction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHypoTimePrediction));
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
            this.txtFutureGlucose = new System.Windows.Forms.TextBox();
            this.btnCalcFutureGlucc = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnReadGlucose = new System.Windows.Forms.Button();
            this.txtStatusBar = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAlarmAdvanceTime = new System.Windows.Forms.TextBox();
            this.txtFutureTimeMinutes = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpFutureTime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // txtGlucoseLast
            // 
            this.txtGlucoseLast.BackColor = System.Drawing.Color.LightGreen;
            this.txtGlucoseLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtHourLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtMinuteLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtMinutePrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtHourPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtGlucosePrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtGlucoseSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGlucoseSlope.Location = new System.Drawing.Point(103, 238);
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
            this.txtGlucoseTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.txtPredictedMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPredictedMinute.Location = new System.Drawing.Point(179, 308);
            this.txtPredictedMinute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPredictedMinute.Name = "txtPredictedMinute";
            this.txtPredictedMinute.Size = new System.Drawing.Size(68, 26);
            this.txtPredictedMinute.TabIndex = 20;
            this.txtPredictedMinute.Text = "-";
            this.txtPredictedMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtPredictedMinute, "Minute in which glucose should reach target level");
            // 
            // txtPredictedHour
            // 
            this.txtPredictedHour.BackColor = System.Drawing.Color.SkyBlue;
            this.txtPredictedHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPredictedHour.Location = new System.Drawing.Point(103, 308);
            this.txtPredictedHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPredictedHour.Name = "txtPredictedHour";
            this.txtPredictedHour.Size = new System.Drawing.Size(68, 26);
            this.txtPredictedHour.TabIndex = 19;
            this.txtPredictedHour.Text = "-";
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
            this.txtAlarmMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtAlarmMinute.Location = new System.Drawing.Point(178, 371);
            this.txtAlarmMinute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlarmMinute.Name = "txtAlarmMinute";
            this.txtAlarmMinute.Size = new System.Drawing.Size(68, 26);
            this.txtAlarmMinute.TabIndex = 31;
            this.txtAlarmMinute.Text = "-";
            this.txtAlarmMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAlarmMinute, "Minute in which glucose should reach target level");
            // 
            // txtAlarmHour
            // 
            this.txtAlarmHour.BackColor = System.Drawing.Color.SkyBlue;
            this.txtAlarmHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtAlarmHour.Location = new System.Drawing.Point(102, 371);
            this.txtAlarmHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlarmHour.Name = "txtAlarmHour";
            this.txtAlarmHour.Size = new System.Drawing.Size(68, 26);
            this.txtAlarmHour.TabIndex = 30;
            this.txtAlarmHour.Text = "-";
            this.txtAlarmHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtAlarmHour, "Hour in which glucose should reach target level");
            // 
            // btnSetAlarm
            // 
            this.btnSetAlarm.Location = new System.Drawing.Point(269, 365);
            this.btnSetAlarm.Name = "btnSetAlarm";
            this.btnSetAlarm.Size = new System.Drawing.Size(75, 39);
            this.btnSetAlarm.TabIndex = 26;
            this.btnSetAlarm.Text = "Alarm";
            this.toolTip1.SetToolTip(this.btnSetAlarm, "Set an alarm at alarm time");
            this.btnSetAlarm.UseVisualStyleBackColor = true;
            this.btnSetAlarm.Click += new System.EventHandler(this.btnSetAlarm_Click);
            // 
            // txtFutureGlucose
            // 
            this.txtFutureGlucose.BackColor = System.Drawing.Color.SkyBlue;
            this.txtFutureGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtFutureGlucose.Location = new System.Drawing.Point(276, 485);
            this.txtFutureGlucose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFutureGlucose.Name = "txtFutureGlucose";
            this.txtFutureGlucose.Size = new System.Drawing.Size(68, 26);
            this.txtFutureGlucose.TabIndex = 117;
            this.txtFutureGlucose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtFutureGlucose, "Hour in which glucose should reach target level");
            // 
            // btnCalcFutureGlucc
            // 
            this.btnCalcFutureGlucc.Location = new System.Drawing.Point(269, 421);
            this.btnCalcFutureGlucc.Name = "btnCalcFutureGlucc";
            this.btnCalcFutureGlucc.Size = new System.Drawing.Size(75, 39);
            this.btnCalcFutureGlucc.TabIndex = 123;
            this.btnCalcFutureGlucc.Text = "Calc";
            this.toolTip1.SetToolTip(this.btnCalcFutureGlucc, "Predict glucose value");
            this.btnCalcFutureGlucc.UseVisualStyleBackColor = true;
            this.btnCalcFutureGlucc.Click += new System.EventHandler(this.btnCalcFutureGlucc_Click);
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
            this.label8.Location = new System.Drawing.Point(179, 241);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Slope [gluc/h]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(4, 54);
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
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(138, 69);
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
            // btnReadGlucose
            // 
            this.btnReadGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnReadGlucose.Location = new System.Drawing.Point(184, 12);
            this.btnReadGlucose.Name = "btnReadGlucose";
            this.btnReadGlucose.Size = new System.Drawing.Size(75, 39);
            this.btnReadGlucose.TabIndex = 33;
            this.btnReadGlucose.Text = "Read glucose";
            this.btnReadGlucose.UseVisualStyleBackColor = true;
            this.btnReadGlucose.Click += new System.EventHandler(this.btnReadGlucose_Click);
            // 
            // txtStatusBar
            // 
            this.txtStatusBar.Location = new System.Drawing.Point(11, 522);
            this.txtStatusBar.Name = "txtStatusBar";
            this.txtStatusBar.Size = new System.Drawing.Size(333, 26);
            this.txtStatusBar.TabIndex = 114;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 430);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(205, 20);
            this.label11.TabIndex = 115;
            this.label11.Text = "Calculation of a future value";
            // 
            // txtAlarmAdvanceTime
            // 
            this.txtAlarmAdvanceTime.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.txtAlarmAdvanceTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtAlarmAdvanceTime.Location = new System.Drawing.Point(179, 89);
            this.txtAlarmAdvanceTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlarmAdvanceTime.Name = "txtAlarmAdvanceTime";
            this.txtAlarmAdvanceTime.Size = new System.Drawing.Size(68, 26);
            this.txtAlarmAdvanceTime.TabIndex = 28;
            this.txtAlarmAdvanceTime.Text = "30";
            this.txtAlarmAdvanceTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFutureTimeMinutes
            // 
            this.txtFutureTimeMinutes.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.txtFutureTimeMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtFutureTimeMinutes.Location = new System.Drawing.Point(11, 488);
            this.txtFutureTimeMinutes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFutureTimeMinutes.Name = "txtFutureTimeMinutes";
            this.txtFutureTimeMinutes.Size = new System.Drawing.Size(68, 26);
            this.txtFutureTimeMinutes.TabIndex = 116;
            this.txtFutureTimeMinutes.Text = "0";
            this.txtFutureTimeMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 463);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 20);
            this.label12.TabIndex = 118;
            this.label12.Text = "Time diff.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(264, 463);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 20);
            this.label13.TabIndex = 119;
            this.label13.Text = "Future gluc.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(83, 492);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 20);
            this.label14.TabIndex = 120;
            this.label14.Text = "min";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(113, 463);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(132, 20);
            this.label15.TabIndex = 121;
            this.label15.Text = "Time in the future";
            // 
            // dtpFutureTime
            // 
            this.dtpFutureTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFutureTime.Location = new System.Drawing.Point(131, 486);
            this.dtpFutureTime.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
            this.dtpFutureTime.Name = "dtpFutureTime";
            this.dtpFutureTime.Size = new System.Drawing.Size(96, 26);
            this.dtpFutureTime.TabIndex = 124;
            // 
            // frmHypoTimePrediction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 560);
            this.Controls.Add(this.dtpFutureTime);
            this.Controls.Add(this.btnCalcFutureGlucc);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtFutureGlucose);
            this.Controls.Add(this.txtFutureTimeMinutes);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtStatusBar);
            this.Controls.Add(this.btnReadGlucose);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAlarmMinute);
            this.Controls.Add(this.txtAlarmHour);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtAlarmAdvanceTime);
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
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmHypoTimePrediction";
            this.Text = "Predict hypoglicemia";
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAlarmMinute;
        private System.Windows.Forms.TextBox txtAlarmHour;
        private System.Windows.Forms.Button btnReadGlucose;
        private System.Windows.Forms.TextBox txtStatusBar;
        private Label label11;
        private TextBox txtAlarmAdvanceTime;
        private TextBox txtFutureTimeMinutes;
        private TextBox txtFutureGlucose;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Button btnCalcFutureGlucc;
        private DateTimePicker dtpFutureTime;
    }
}

