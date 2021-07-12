
namespace GlucoMan_Forms_Core
{
    partial class frmInsulinCalc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInsulinCalc));
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtChoToEat = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.txtChoInsulineRatioDinner = new System.Windows.Forms.TextBox();
            this.txtChoInsulineRatioLunch = new System.Windows.Forms.TextBox();
            this.txtChoInsulineRatioBreakfast = new System.Windows.Forms.TextBox();
            this.txtInsulineSensitivity = new System.Windows.Forms.TextBox();
            this.txtTdd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGlucoseBeforeMeal = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.btnRoundInsuline = new System.Windows.Forms.Button();
            this.btnReadGlucose = new System.Windows.Forms.Button();
            this.btnSaveBolus = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInsulinSensitivityCalculation = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGlucoseToBeCorrected = new System.Windows.Forms.TextBox();
            this.txtCorrectionInsuline = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbIsSnack = new System.Windows.Forms.RadioButton();
            this.rdbIsBreakfast = new System.Windows.Forms.RadioButton();
            this.rdbIsDinner = new System.Windows.Forms.RadioButton();
            this.rdbIsLunch = new System.Windows.Forms.RadioButton();
            this.txtChoInsulineMeal = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
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
            this.cmbSensitivityFactor = new System.Windows.Forms.ComboBox();
            this.txtStatusBar = new System.Windows.Forms.TextBox();
            this.txtTotalInsuline = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(151, 207);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(192, 25);
            this.label7.TabIndex = 47;
            this.label7.Text = "Sensitivity to insuline";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(361, 133);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 25);
            this.label6.TabIndex = 46;
            this.label6.Text = "TDD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 278);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 25);
            this.label5.TabIndex = 45;
            this.label5.Text = "CHO to eat";
            // 
            // txtChoToEat
            // 
            this.txtChoToEat.BackColor = System.Drawing.Color.GreenYellow;
            this.txtChoToEat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoToEat.Location = new System.Drawing.Point(389, 303);
            this.txtChoToEat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoToEat.Name = "txtChoToEat";
            this.txtChoToEat.Size = new System.Drawing.Size(68, 30);
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
            this.label4.Size = new System.Drawing.Size(142, 25);
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
            // txtChoInsulineRatioDinner
            // 
            this.txtChoInsulineRatioDinner.BackColor = System.Drawing.Color.PaleGreen;
            this.txtChoInsulineRatioDinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineRatioDinner.Location = new System.Drawing.Point(174, 49);
            this.txtChoInsulineRatioDinner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineRatioDinner.Name = "txtChoInsulineRatioDinner";
            this.txtChoInsulineRatioDinner.Size = new System.Drawing.Size(68, 30);
            this.txtChoInsulineRatioDinner.TabIndex = 6;
            this.txtChoInsulineRatioDinner.Text = "6,5";
            this.txtChoInsulineRatioDinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineRatioDinner.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtChoInsulineRatioLunch
            // 
            this.txtChoInsulineRatioLunch.BackColor = System.Drawing.Color.PaleGreen;
            this.txtChoInsulineRatioLunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineRatioLunch.Location = new System.Drawing.Point(98, 49);
            this.txtChoInsulineRatioLunch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineRatioLunch.Name = "txtChoInsulineRatioLunch";
            this.txtChoInsulineRatioLunch.Size = new System.Drawing.Size(68, 30);
            this.txtChoInsulineRatioLunch.TabIndex = 3;
            this.txtChoInsulineRatioLunch.Tag = "";
            this.txtChoInsulineRatioLunch.Text = "7";
            this.txtChoInsulineRatioLunch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineRatioLunch.TextChanged += new System.EventHandler(this.txtRatioMidday_TextChanged);
            this.txtChoInsulineRatioLunch.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtChoInsulineRatioBreakfast
            // 
            this.txtChoInsulineRatioBreakfast.BackColor = System.Drawing.Color.PaleGreen;
            this.txtChoInsulineRatioBreakfast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineRatioBreakfast.Location = new System.Drawing.Point(22, 49);
            this.txtChoInsulineRatioBreakfast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineRatioBreakfast.Name = "txtChoInsulineRatioBreakfast";
            this.txtChoInsulineRatioBreakfast.Size = new System.Drawing.Size(68, 30);
            this.txtChoInsulineRatioBreakfast.TabIndex = 1;
            this.txtChoInsulineRatioBreakfast.Text = "4,5";
            this.txtChoInsulineRatioBreakfast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineRatioBreakfast.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtInsulineSensitivity
            // 
            this.txtInsulineSensitivity.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtInsulineSensitivity.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtInsulineSensitivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInsulineSensitivity.Location = new System.Drawing.Point(194, 232);
            this.txtInsulineSensitivity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInsulineSensitivity.Name = "txtInsulineSensitivity";
            this.txtInsulineSensitivity.Size = new System.Drawing.Size(68, 30);
            this.txtInsulineSensitivity.TabIndex = 27;
            this.txtInsulineSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtInsulineSensitivity.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtTdd
            // 
            this.txtTdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTdd.Location = new System.Drawing.Point(348, 158);
            this.txtTdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTdd.Name = "txtTdd";
            this.txtTdd.ReadOnly = true;
            this.txtTdd.Size = new System.Drawing.Size(68, 30);
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
            this.label1.Size = new System.Drawing.Size(157, 25);
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
            this.txtGlucoseBeforeMeal.Size = new System.Drawing.Size(68, 30);
            this.txtGlucoseBeforeMeal.TabIndex = 30;
            this.txtGlucoseBeforeMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtGlucoseBeforeMeal, "Measured glucose before meal");
            this.txtGlucoseBeforeMeal.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(90, 418);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 42);
            this.label11.TabIndex = 53;
            this.label11.Text = "Correction insuline [Ui]";
            this.toolTip1.SetToolTip(this.label11, "Insuline to be included in the bolus to correct for glucose more than target");
            // 
            // btnRoundInsuline
            // 
            this.btnRoundInsuline.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRoundInsuline.Location = new System.Drawing.Point(436, 353);
            this.btnRoundInsuline.Name = "btnRoundInsuline";
            this.btnRoundInsuline.Size = new System.Drawing.Size(75, 46);
            this.btnRoundInsuline.TabIndex = 102;
            this.btnRoundInsuline.Text = "Round insuline";
            this.toolTip1.SetToolTip(this.btnRoundInsuline, "Calculate CHO that gives  integer insuline");
            this.btnRoundInsuline.UseVisualStyleBackColor = true;
            this.btnRoundInsuline.Click += new System.EventHandler(this.btnRoundInsuline_Click);
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
            // btnSaveBolus
            // 
            this.btnSaveBolus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveBolus.Location = new System.Drawing.Point(436, 455);
            this.btnSaveBolus.Name = "btnSaveBolus";
            this.btnSaveBolus.Size = new System.Drawing.Size(75, 46);
            this.btnSaveBolus.TabIndex = 105;
            this.btnSaveBolus.Text = "Save bolus";
            this.toolTip1.SetToolTip(this.btnSaveBolus, "Calculate CHO that gives  integer insuline");
            this.btnSaveBolus.UseVisualStyleBackColor = true;
            this.btnSaveBolus.Click += new System.EventHandler(this.btnSaveBolus_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(198, 418);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 42);
            this.label8.TabIndex = 111;
            this.label8.Text = "Insuline due to CHO [Ui]";
            this.toolTip1.SetToolTip(this.label8, "Insuline to be included in the bolus to correct for glucose more than target");
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(311, 418);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 48);
            this.label2.TabIndex = 112;
            this.label2.Text = "Total insuline in bolus [Ui]";
            this.toolTip1.SetToolTip(this.label2, "Insuline to be included in the bolus to correct for glucose more than target");
            // 
            // btnInsulinSensitivityCalculation
            // 
            this.btnInsulinSensitivityCalculation.Location = new System.Drawing.Point(345, 225);
            this.btnInsulinSensitivityCalculation.Name = "btnInsulinSensitivityCalculation";
            this.btnInsulinSensitivityCalculation.Size = new System.Drawing.Size(75, 33);
            this.btnInsulinSensitivityCalculation.TabIndex = 115;
            this.btnInsulinSensitivityCalculation.Text = "Calc";
            this.toolTip1.SetToolTip(this.btnInsulinSensitivityCalculation, "Calculation of insuline sensitivity");
            this.btnInsulinSensitivityCalculation.UseVisualStyleBackColor = true;
            this.btnInsulinSensitivityCalculation.Click += new System.EventHandler(this.btnInsulinSensitivityCalculation_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 207);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 25);
            this.label9.TabIndex = 49;
            this.label9.Text = "Sensitivity factor ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(240, 278);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(169, 25);
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
            this.txtGlucoseToBeCorrected.Size = new System.Drawing.Size(68, 30);
            this.txtGlucoseToBeCorrected.TabIndex = 33;
            this.txtGlucoseToBeCorrected.TabStop = false;
            this.txtGlucoseToBeCorrected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGlucoseToBeCorrected.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtCorrectionInsuline
            // 
            this.txtCorrectionInsuline.BackColor = System.Drawing.Color.CadetBlue;
            this.txtCorrectionInsuline.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCorrectionInsuline.Location = new System.Drawing.Point(103, 465);
            this.txtCorrectionInsuline.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCorrectionInsuline.Name = "txtCorrectionInsuline";
            this.txtCorrectionInsuline.ReadOnly = true;
            this.txtCorrectionInsuline.Size = new System.Drawing.Size(68, 30);
            this.txtCorrectionInsuline.TabIndex = 48;
            this.txtCorrectionInsuline.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCorrectionInsuline.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbIsSnack);
            this.groupBox1.Controls.Add(this.rdbIsBreakfast);
            this.groupBox1.Controls.Add(this.rdbIsDinner);
            this.groupBox1.Controls.Add(this.rdbIsLunch);
            this.groupBox1.Location = new System.Drawing.Point(84, 339);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 56);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type of meal";
            // 
            // rdbIsSnack
            // 
            this.rdbIsSnack.AutoSize = true;
            this.rdbIsSnack.Location = new System.Drawing.Point(248, 25);
            this.rdbIsSnack.Name = "rdbIsSnack";
            this.rdbIsSnack.Size = new System.Drawing.Size(85, 29);
            this.rdbIsSnack.TabIndex = 109;
            this.rdbIsSnack.TabStop = true;
            this.rdbIsSnack.Text = "snack";
            this.rdbIsSnack.UseVisualStyleBackColor = true;
            // 
            // rdbIsBreakfast
            // 
            this.rdbIsBreakfast.AutoSize = true;
            this.rdbIsBreakfast.Location = new System.Drawing.Point(6, 25);
            this.rdbIsBreakfast.Name = "rdbIsBreakfast";
            this.rdbIsBreakfast.Size = new System.Drawing.Size(113, 29);
            this.rdbIsBreakfast.TabIndex = 106;
            this.rdbIsBreakfast.TabStop = true;
            this.rdbIsBreakfast.Text = "breakfast";
            this.rdbIsBreakfast.UseVisualStyleBackColor = true;
            // 
            // rdbIsDinner
            // 
            this.rdbIsDinner.AutoSize = true;
            this.rdbIsDinner.Location = new System.Drawing.Point(173, 25);
            this.rdbIsDinner.Name = "rdbIsDinner";
            this.rdbIsDinner.Size = new System.Drawing.Size(87, 29);
            this.rdbIsDinner.TabIndex = 108;
            this.rdbIsDinner.TabStop = true;
            this.rdbIsDinner.Text = "dinner";
            this.rdbIsDinner.UseVisualStyleBackColor = true;
            // 
            // rdbIsLunch
            // 
            this.rdbIsLunch.AutoSize = true;
            this.rdbIsLunch.Location = new System.Drawing.Point(104, 25);
            this.rdbIsLunch.Name = "rdbIsLunch";
            this.rdbIsLunch.Size = new System.Drawing.Size(80, 29);
            this.rdbIsLunch.TabIndex = 107;
            this.rdbIsLunch.TabStop = true;
            this.rdbIsLunch.Text = "lunch";
            this.rdbIsLunch.UseVisualStyleBackColor = true;
            // 
            // txtChoInsulineMeal
            // 
            this.txtChoInsulineMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChoInsulineMeal.Location = new System.Drawing.Point(214, 465);
            this.txtChoInsulineMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulineMeal.Name = "txtChoInsulineMeal";
            this.txtChoInsulineMeal.ReadOnly = true;
            this.txtChoInsulineMeal.Size = new System.Drawing.Size(68, 30);
            this.txtChoInsulineMeal.TabIndex = 45;
            this.txtChoInsulineMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulineMeal.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.txtChoInsulineRatioBreakfast);
            this.groupBox2.Controls.Add(this.txtChoInsulineRatioLunch);
            this.groupBox2.Controls.Add(this.txtChoInsulineRatioDinner);
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
            this.label20.Location = new System.Drawing.Point(180, 24);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(69, 25);
            this.label20.TabIndex = 64;
            this.label20.Text = "Dinner";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(106, 24);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(66, 25);
            this.label21.TabIndex = 63;
            this.label21.Text = "Lunch";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(17, 24);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(94, 25);
            this.label22.TabIndex = 62;
            this.label22.Text = "Breakfast";
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
            this.groupBox4.Location = new System.Drawing.Point(3, 105);
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
            this.label14.Size = new System.Drawing.Size(54, 25);
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
            this.txtTypicalBolusMorning.Size = new System.Drawing.Size(68, 30);
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
            this.label15.Size = new System.Drawing.Size(81, 25);
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
            this.txtTypicalBolusMidday.Size = new System.Drawing.Size(68, 30);
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
            this.label27.Size = new System.Drawing.Size(75, 25);
            this.label27.TabIndex = 63;
            this.label27.Text = "midday";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(20, 28);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(82, 25);
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
            this.txtTypicalBolusEvening.Size = new System.Drawing.Size(68, 30);
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
            this.txtTypicalBolusNight.Size = new System.Drawing.Size(68, 30);
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
            this.txtTargetGlucose.Size = new System.Drawing.Size(68, 30);
            this.txtTargetGlucose.TabIndex = 101;
            this.txtTargetGlucose.Text = "120";
            this.txtTargetGlucose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbSensitivityFactor
            // 
            this.cmbSensitivityFactor.FormattingEnabled = true;
            this.cmbSensitivityFactor.Items.AddRange(new object[] {
            "1800",
            "1700",
            "1500"});
            this.cmbSensitivityFactor.Location = new System.Drawing.Point(46, 230);
            this.cmbSensitivityFactor.Name = "cmbSensitivityFactor";
            this.cmbSensitivityFactor.Size = new System.Drawing.Size(68, 33);
            this.cmbSensitivityFactor.TabIndex = 104;
            this.cmbSensitivityFactor.Text = "1800";
            // 
            // txtStaturBar
            // 
            this.txtStatusBar.Location = new System.Drawing.Point(12, 507);
            this.txtStatusBar.Name = "txtStaturBar";
            this.txtStatusBar.Size = new System.Drawing.Size(497, 30);
            this.txtStatusBar.TabIndex = 113;
            // 
            // txtTotalInsuline
            // 
            this.txtTotalInsuline.BackColor = System.Drawing.Color.Aqua;
            this.txtTotalInsuline.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalInsuline.Location = new System.Drawing.Point(332, 465);
            this.txtTotalInsuline.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalInsuline.Name = "txtTotalInsuline";
            this.txtTotalInsuline.ReadOnly = true;
            this.txtTotalInsuline.Size = new System.Drawing.Size(68, 30);
            this.txtTotalInsuline.TabIndex = 114;
            this.txtTotalInsuline.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmInsulineCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 544);
            this.Controls.Add(this.btnInsulinSensitivityCalculation);
            this.Controls.Add(this.txtTotalInsuline);
            this.Controls.Add(this.txtStatusBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtChoInsulineMeal);
            this.Controls.Add(this.btnSaveBolus);
            this.Controls.Add(this.cmbSensitivityFactor);
            this.Controls.Add(this.btnReadGlucose);
            this.Controls.Add(this.btnRoundInsuline);
            this.Controls.Add(this.txtTargetGlucose);
            this.Controls.Add(this.groupBox4);
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
            this.Controls.Add(this.txtInsulineSensitivity);
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
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChoToEat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.TextBox txtChoInsulineRatioDinner;
        private System.Windows.Forms.TextBox txtChoInsulineRatioLunch;
        private System.Windows.Forms.TextBox txtChoInsulineRatioBreakfast;
        private System.Windows.Forms.TextBox txtInsulineSensitivity;
        private System.Windows.Forms.TextBox txtTdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtGlucoseBeforeMeal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGlucoseToBeCorrected;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCorrectionInsuline;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtChoInsulineMeal;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
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
        private System.Windows.Forms.ComboBox cmbSensitivityFactor;
        private System.Windows.Forms.Button btnSaveBolus;
        private System.Windows.Forms.RadioButton rdbIsSnack;
        private System.Windows.Forms.RadioButton rdbIsBreakfast;
        private System.Windows.Forms.RadioButton rdbIsDinner;
        private System.Windows.Forms.RadioButton rdbIsLunch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatusBar;
        private System.Windows.Forms.TextBox txtTotalInsuline;
        private System.Windows.Forms.Button btnInsulinSensitivityCalculation;
    }
}