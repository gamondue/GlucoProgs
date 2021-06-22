
namespace GlucoMan_Forms_Core
{
    partial class frmInsulineCalc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInsulineCalc));
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtChoInsulineBreakfast = new System.Windows.Forms.TextBox();
            this.txtChoToEat = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.txtRatioEvening = new System.Windows.Forms.TextBox();
            this.txtRatioMidday = new System.Windows.Forms.TextBox();
            this.txtRatioMorning = new System.Windows.Forms.TextBox();
            this.txtSensitivity1800 = new System.Windows.Forms.TextBox();
            this.txtSensitivity1500 = new System.Windows.Forms.TextBox();
            this.txtTdd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGlucoseBeforeMeal = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.btnRoundInsuline = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGlucoseToBeCorrected = new System.Windows.Forms.TextBox();
            this.txtCorrectionInsuline = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtChoInsulineDinner = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtChoInsulineLunch = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTotalInsulineDinner = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtTotalInsulineLunch = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtTotalInsulineBreakfast = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTypicalBolusMorning = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTypicalBolusMidday = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.txtTypicalBolusEvening = new System.Windows.Forms.TextBox();
            this.txtTypicalBolusNight = new System.Windows.Forms.TextBox();
            this.txtTargetGlucose = new System.Windows.Forms.TextBox();
            this.btnReadGlucose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(284, 207);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(224, 20);
            this.label7.TabIndex = 47;
            this.label7.Text = "Sensitivity to insuline 1800 rule";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 207);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 46;
            this.label6.Text = "TDD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 278);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 45;
            this.label5.Text = "CHO to eat";
            // 
            // txtChoInsulineBreakfast
            // 
            this.txtChoInsulineBreakfast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineBreakfast.Location = new System.Drawing.Point(19, 51);
            this.txtChoInsulineBreakfast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineBreakfast.Name = "txtChoInsulineBreakfast";
            this.txtChoInsulineBreakfast.ReadOnly = true;
            this.txtChoInsulineBreakfast.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulineBreakfast.TabIndex = 39;
            this.txtChoInsulineBreakfast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineBreakfast.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtChoToEat
            // 
            this.txtChoToEat.BackColor = System.Drawing.Color.GreenYellow;
            this.txtChoToEat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoToEat.Location = new System.Drawing.Point(389, 303);
            this.txtChoToEat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoToEat.Name = "txtChoToEat";
            this.txtChoToEat.Size = new System.Drawing.Size(68, 26);
            this.txtChoToEat.TabIndex = 36;
            this.txtChoToEat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoToEat.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(128, 278);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 20);
            this.label4.TabIndex = 42;
            this.label4.Text = "Target glucose";
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(436, 53);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 33);
            this.btnCalc.TabIndex = 100;
            this.btnCalc.Text = "Calc";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // txtRatioEvening
            // 
            this.txtRatioEvening.BackColor = System.Drawing.Color.PaleGreen;
            this.txtRatioEvening.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRatioEvening.Location = new System.Drawing.Point(174, 49);
            this.txtRatioEvening.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRatioEvening.Name = "txtRatioEvening";
            this.txtRatioEvening.Size = new System.Drawing.Size(68, 26);
            this.txtRatioEvening.TabIndex = 6;
            this.txtRatioEvening.Text = "6,5";
            this.txtRatioEvening.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRatioEvening.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtRatioMidday
            // 
            this.txtRatioMidday.BackColor = System.Drawing.Color.PaleGreen;
            this.txtRatioMidday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRatioMidday.Location = new System.Drawing.Point(98, 49);
            this.txtRatioMidday.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRatioMidday.Name = "txtRatioMidday";
            this.txtRatioMidday.Size = new System.Drawing.Size(68, 26);
            this.txtRatioMidday.TabIndex = 3;
            this.txtRatioMidday.Tag = "";
            this.txtRatioMidday.Text = "7";
            this.txtRatioMidday.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRatioMidday.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtRatioMorning
            // 
            this.txtRatioMorning.BackColor = System.Drawing.Color.PaleGreen;
            this.txtRatioMorning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRatioMorning.Location = new System.Drawing.Point(22, 49);
            this.txtRatioMorning.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRatioMorning.Name = "txtRatioMorning";
            this.txtRatioMorning.Size = new System.Drawing.Size(68, 26);
            this.txtRatioMorning.TabIndex = 1;
            this.txtRatioMorning.Text = "4,5";
            this.txtRatioMorning.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRatioMorning.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtSensitivity1800
            // 
            this.txtSensitivity1800.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSensitivity1800.Location = new System.Drawing.Point(348, 232);
            this.txtSensitivity1800.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSensitivity1800.Name = "txtSensitivity1800";
            this.txtSensitivity1800.ReadOnly = true;
            this.txtSensitivity1800.Size = new System.Drawing.Size(68, 26);
            this.txtSensitivity1800.TabIndex = 27;
            this.txtSensitivity1800.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSensitivity1800.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtSensitivity1500
            // 
            this.txtSensitivity1500.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSensitivity1500.Location = new System.Drawing.Point(122, 232);
            this.txtSensitivity1500.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSensitivity1500.Name = "txtSensitivity1500";
            this.txtSensitivity1500.ReadOnly = true;
            this.txtSensitivity1500.Size = new System.Drawing.Size(68, 26);
            this.txtSensitivity1500.TabIndex = 24;
            this.txtSensitivity1500.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSensitivity1500.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtTdd
            // 
            this.txtTdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTdd.Location = new System.Drawing.Point(3, 232);
            this.txtTdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTdd.Name = "txtTdd";
            this.txtTdd.ReadOnly = true;
            this.txtTdd.Size = new System.Drawing.Size(68, 26);
            this.txtTdd.TabIndex = 21;
            this.txtTdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTdd, "TDD = Total Daily Dose of insulin ");
            this.txtTdd.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 278);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 20);
            this.label1.TabIndex = 26;
            this.label1.Text = "Premeal glucose";
            this.toolTip1.SetToolTip(this.label1, "Measured glucose before meal ");
            // 
            // txtGlucoseBeforeMeal
            // 
            this.txtGlucoseBeforeMeal.BackColor = System.Drawing.Color.GreenYellow;
            this.txtGlucoseBeforeMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucoseBeforeMeal.Location = new System.Drawing.Point(3, 303);
            this.txtGlucoseBeforeMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseBeforeMeal.Name = "txtGlucoseBeforeMeal";
            this.txtGlucoseBeforeMeal.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseBeforeMeal.TabIndex = 30;
            this.txtGlucoseBeforeMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtGlucoseBeforeMeal, "Measured glucose before meal");
            this.txtGlucoseBeforeMeal.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(9, 445);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 42);
            this.label11.TabIndex = 53;
            this.label11.Text = "Correction insuline [Ui]";
            this.toolTip1.SetToolTip(this.label11, "Insuline to be included in the bolus to correct for glucose more than target");
            // 
            // btnRoundInsuline
            // 
            this.btnRoundInsuline.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRoundInsuline.Location = new System.Drawing.Point(433, 390);
            this.btnRoundInsuline.Name = "btnRoundInsuline";
            this.btnRoundInsuline.Size = new System.Drawing.Size(75, 46);
            this.btnRoundInsuline.TabIndex = 102;
            this.btnRoundInsuline.Text = "Round insuline";
            this.toolTip1.SetToolTip(this.btnRoundInsuline, "Calculate CHO that gives  integer insuline");
            this.btnRoundInsuline.UseVisualStyleBackColor = true;
            this.btnRoundInsuline.Click += new System.EventHandler(this.btnRoundInsuline_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(44, 207);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(224, 20);
            this.label9.TabIndex = 49;
            this.label9.Text = "Sensitivity to insuline 1500 rule";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(240, 278);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 20);
            this.label10.TabIndex = 51;
            this.label10.Text = "Glucose to correct";
            // 
            // txtGlucoseToBeCorrected
            // 
            this.txtGlucoseToBeCorrected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlucoseToBeCorrected.Location = new System.Drawing.Point(274, 303);
            this.txtGlucoseToBeCorrected.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseToBeCorrected.Name = "txtGlucoseToBeCorrected";
            this.txtGlucoseToBeCorrected.ReadOnly = true;
            this.txtGlucoseToBeCorrected.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseToBeCorrected.TabIndex = 33;
            this.txtGlucoseToBeCorrected.TabStop = false;
            this.txtGlucoseToBeCorrected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGlucoseToBeCorrected.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtCorrectionInsuline
            // 
            this.txtCorrectionInsuline.BackColor = System.Drawing.Color.CadetBlue;
            this.txtCorrectionInsuline.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCorrectionInsuline.Location = new System.Drawing.Point(13, 496);
            this.txtCorrectionInsuline.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCorrectionInsuline.Name = "txtCorrectionInsuline";
            this.txtCorrectionInsuline.ReadOnly = true;
            this.txtCorrectionInsuline.Size = new System.Drawing.Size(68, 26);
            this.txtCorrectionInsuline.TabIndex = 48;
            this.txtCorrectionInsuline.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCorrectionInsuline.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 26);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 20);
            this.label13.TabIndex = 55;
            this.label13.Text = "breakfast";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtChoInsulineDinner);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtChoInsulineLunch);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtChoInsulineBreakfast);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Location = new System.Drawing.Point(96, 349);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 90);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Insuline due to CHO [Ui]";
            // 
            // txtChoInsulineDinner
            // 
            this.txtChoInsulineDinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineDinner.Location = new System.Drawing.Point(171, 51);
            this.txtChoInsulineDinner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineDinner.Name = "txtChoInsulineDinner";
            this.txtChoInsulineDinner.ReadOnly = true;
            this.txtChoInsulineDinner.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulineDinner.TabIndex = 45;
            this.txtChoInsulineDinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineDinner.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(179, 26);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 20);
            this.label17.TabIndex = 59;
            this.label17.Text = "dinner";
            // 
            // txtChoInsulineLunch
            // 
            this.txtChoInsulineLunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineLunch.Location = new System.Drawing.Point(95, 51);
            this.txtChoInsulineLunch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineLunch.Name = "txtChoInsulineLunch";
            this.txtChoInsulineLunch.ReadOnly = true;
            this.txtChoInsulineLunch.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulineLunch.TabIndex = 42;
            this.txtChoInsulineLunch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineLunch.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(106, 26);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 20);
            this.label16.TabIndex = 57;
            this.label16.Text = "lunch";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.txtRatioMorning);
            this.groupBox2.Controls.Add(this.txtRatioMidday);
            this.groupBox2.Controls.Add(this.txtRatioEvening);
            this.groupBox2.Location = new System.Drawing.Point(103, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 88);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ratio CHO / insuline (from dietist)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(177, 24);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(64, 20);
            this.label20.TabIndex = 64;
            this.label20.Text = "evening";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(104, 24);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 20);
            this.label21.TabIndex = 63;
            this.label21.Text = "midday";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(24, 24);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(66, 20);
            this.label22.TabIndex = 62;
            this.label22.Text = "morning";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTotalInsulineDinner);
            this.groupBox3.Controls.Add(this.label24);
            this.groupBox3.Controls.Add(this.txtTotalInsulineLunch);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.txtTotalInsulineBreakfast);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Location = new System.Drawing.Point(96, 445);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(249, 90);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Total insuline in bolus [Ui]";
            // 
            // txtTotalInsulineDinner
            // 
            this.txtTotalInsulineDinner.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.txtTotalInsulineDinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalInsulineDinner.Location = new System.Drawing.Point(171, 51);
            this.txtTotalInsulineDinner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalInsulineDinner.Name = "txtTotalInsulineDinner";
            this.txtTotalInsulineDinner.ReadOnly = true;
            this.txtTotalInsulineDinner.Size = new System.Drawing.Size(68, 26);
            this.txtTotalInsulineDinner.TabIndex = 57;
            this.txtTotalInsulineDinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalInsulineDinner.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(179, 26);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(53, 20);
            this.label24.TabIndex = 59;
            this.label24.Text = "dinner";
            // 
            // txtTotalInsulineLunch
            // 
            this.txtTotalInsulineLunch.BackColor = System.Drawing.Color.LightBlue;
            this.txtTotalInsulineLunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalInsulineLunch.Location = new System.Drawing.Point(95, 51);
            this.txtTotalInsulineLunch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalInsulineLunch.Name = "txtTotalInsulineLunch";
            this.txtTotalInsulineLunch.ReadOnly = true;
            this.txtTotalInsulineLunch.Size = new System.Drawing.Size(68, 26);
            this.txtTotalInsulineLunch.TabIndex = 54;
            this.txtTotalInsulineLunch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalInsulineLunch.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(106, 26);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(47, 20);
            this.label25.TabIndex = 57;
            this.label25.Text = "lunch";
            // 
            // txtTotalInsulineBreakfast
            // 
            this.txtTotalInsulineBreakfast.BackColor = System.Drawing.Color.SkyBlue;
            this.txtTotalInsulineBreakfast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalInsulineBreakfast.Location = new System.Drawing.Point(19, 51);
            this.txtTotalInsulineBreakfast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalInsulineBreakfast.Name = "txtTotalInsulineBreakfast";
            this.txtTotalInsulineBreakfast.ReadOnly = true;
            this.txtTotalInsulineBreakfast.Size = new System.Drawing.Size(68, 26);
            this.txtTotalInsulineBreakfast.TabIndex = 51;
            this.txtTotalInsulineBreakfast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalInsulineBreakfast.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(15, 26);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(76, 20);
            this.label26.TabIndex = 55;
            this.label26.Text = "breakfast";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtTypicalBolusMorning);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.txtTypicalBolusMidday);
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Controls.Add(this.txtTypicalBolusEvening);
            this.groupBox4.Controls.Add(this.txtTypicalBolusNight);
            this.groupBox4.Location = new System.Drawing.Point(103, 105);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(327, 87);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Typical bolus [Ui]";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(259, 28);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 20);
            this.label14.TabIndex = 65;
            this.label14.Text = "night";
            // 
            // txtTypicalBolusMorning
            // 
            this.txtTypicalBolusMorning.BackColor = System.Drawing.Color.PaleGreen;
            this.txtTypicalBolusMorning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTypicalBolusMorning.Location = new System.Drawing.Point(19, 53);
            this.txtTypicalBolusMorning.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTypicalBolusMorning.Name = "txtTypicalBolusMorning";
            this.txtTypicalBolusMorning.Size = new System.Drawing.Size(68, 26);
            this.txtTypicalBolusMorning.TabIndex = 9;
            this.txtTypicalBolusMorning.Text = "10";
            this.txtTypicalBolusMorning.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTypicalBolusMorning.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(173, 28);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 20);
            this.label15.TabIndex = 64;
            this.label15.Text = "evening";
            // 
            // txtTypicalBolusMidday
            // 
            this.txtTypicalBolusMidday.BackColor = System.Drawing.Color.PaleGreen;
            this.txtTypicalBolusMidday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTypicalBolusMidday.Location = new System.Drawing.Point(95, 53);
            this.txtTypicalBolusMidday.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTypicalBolusMidday.Name = "txtTypicalBolusMidday";
            this.txtTypicalBolusMidday.Size = new System.Drawing.Size(68, 26);
            this.txtTypicalBolusMidday.TabIndex = 12;
            this.txtTypicalBolusMidday.Text = "11";
            this.txtTypicalBolusMidday.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTypicalBolusMidday.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(100, 28);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(59, 20);
            this.label27.TabIndex = 63;
            this.label27.Text = "midday";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(20, 28);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(66, 20);
            this.label28.TabIndex = 62;
            this.label28.Text = "morning";
            // 
            // txtTypicalBolusEvening
            // 
            this.txtTypicalBolusEvening.BackColor = System.Drawing.Color.PaleGreen;
            this.txtTypicalBolusEvening.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTypicalBolusEvening.Location = new System.Drawing.Point(171, 53);
            this.txtTypicalBolusEvening.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTypicalBolusEvening.Name = "txtTypicalBolusEvening";
            this.txtTypicalBolusEvening.Size = new System.Drawing.Size(68, 26);
            this.txtTypicalBolusEvening.TabIndex = 14;
            this.txtTypicalBolusEvening.Tag = "15";
            this.txtTypicalBolusEvening.Text = "12";
            this.txtTypicalBolusEvening.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTypicalBolusEvening.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtTypicalBolusNight
            // 
            this.txtTypicalBolusNight.BackColor = System.Drawing.Color.PaleGreen;
            this.txtTypicalBolusNight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTypicalBolusNight.Location = new System.Drawing.Point(247, 53);
            this.txtTypicalBolusNight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTypicalBolusNight.Name = "txtTypicalBolusNight";
            this.txtTypicalBolusNight.Size = new System.Drawing.Size(68, 26);
            this.txtTypicalBolusNight.TabIndex = 34;
            this.txtTypicalBolusNight.Tag = "18";
            this.txtTypicalBolusNight.Text = "16";
            this.txtTypicalBolusNight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTypicalBolusNight.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtTargetGlucose
            // 
            this.txtTargetGlucose.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtTargetGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTargetGlucose.Location = new System.Drawing.Point(159, 303);
            this.txtTargetGlucose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTargetGlucose.Name = "txtTargetGlucose";
            this.txtTargetGlucose.Size = new System.Drawing.Size(68, 26);
            this.txtTargetGlucose.TabIndex = 101;
            this.txtTargetGlucose.Text = "120";
            this.txtTargetGlucose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnReadGlucose
            // 
            this.btnReadGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadGlucose.Location = new System.Drawing.Point(0, 349);
            this.btnReadGlucose.Name = "btnReadGlucose";
            this.btnReadGlucose.Size = new System.Drawing.Size(75, 46);
            this.btnReadGlucose.TabIndex = 103;
            this.btnReadGlucose.Text = "Read glucose";
            this.toolTip1.SetToolTip(this.btnReadGlucose, "Read last glucose value  from recorded glucose measurements");
            this.btnReadGlucose.UseVisualStyleBackColor = true;
            this.btnReadGlucose.Click += new System.EventHandler(this.btnReadGlucose_Click);
            // 
            // frmInsulineCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 546);
            this.Controls.Add(this.btnReadGlucose);
            this.Controls.Add(this.btnRoundInsuline);
            this.Controls.Add(this.txtTargetGlucose);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCorrectionInsuline);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtGlucoseToBeCorrected);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtChoToEat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.txtSensitivity1800);
            this.Controls.Add(this.txtSensitivity1500);
            this.Controls.Add(this.txtTdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGlucoseBeforeMeal);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmInsulineCalc";
            this.Text = "Insuline bolus calculation, with correction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInsulineCalc_FormClosing);
            this.Load += new System.EventHandler(this.frmInsulineCalc_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChoInsulineBreakfast;
        private System.Windows.Forms.TextBox txtChoToEat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.TextBox txtRatioEvening;
        private System.Windows.Forms.TextBox txtRatioMidday;
        private System.Windows.Forms.TextBox txtRatioMorning;
        private System.Windows.Forms.TextBox txtSensitivity1800;
        private System.Windows.Forms.TextBox txtSensitivity1500;
        private System.Windows.Forms.TextBox txtTdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtGlucoseBeforeMeal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGlucoseToBeCorrected;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCorrectionInsuline;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtChoInsulineDinner;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtChoInsulineLunch;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtTotalInsulineDinner;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtTotalInsulineLunch;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtTotalInsulineBreakfast;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTypicalBolusMorning;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtTypicalBolusMidday;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtTypicalBolusEvening;
        private System.Windows.Forms.TextBox txtTypicalBolusNight;
        private System.Windows.Forms.TextBox txtTargetGlucose;
        private System.Windows.Forms.Button btnRoundInsuline;
        private System.Windows.Forms.Button btnReadGlucose;
    }
}