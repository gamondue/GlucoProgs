
namespace GlucoMan.Forms
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
            this.btnSetParameters = new System.Windows.Forms.Button();
            this.txtInsulinCorrectionSensitivity = new System.Windows.Forms.TextBox();
            this.txtTotalDailyDoseOfInsulin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGlucoseBeforeMeal = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.btnRoundInsulin = new System.Windows.Forms.Button();
            this.btnReadGlucose = new System.Windows.Forms.Button();
            this.btnSaveBolus = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpenGlucose = new System.Windows.Forms.Button();
            this.txtReadCho = new System.Windows.Forms.Button();
            this.btnInjection = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGlucoseToBeCorrected = new System.Windows.Forms.TextBox();
            this.txtCorrectionInsulin = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbIsSnack = new System.Windows.Forms.RadioButton();
            this.rdbIsBreakfast = new System.Windows.Forms.RadioButton();
            this.rdbIsDinner = new System.Windows.Forms.RadioButton();
            this.rdbIsLunch = new System.Windows.Forms.RadioButton();
            this.txtChoInsulinMeal = new System.Windows.Forms.TextBox();
            this.txtTargetGlucose = new System.Windows.Forms.TextBox();
            this.txtStatusBar = new System.Windows.Forms.TextBox();
            this.txtTotalInsulin = new System.Windows.Forms.TextBox();
            this.btnBolusCalculations = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtChoInsulinRatioBreakfast = new System.Windows.Forms.TextBox();
            this.txtChoInsulinRatioLunch = new System.Windows.Forms.TextBox();
            this.txtChoInsulinRatioDinner = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(104, 117);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 20);
            this.label7.TabIndex = 47;
            this.label7.Text = "Sensitivity to insulin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 117);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 46;
            this.label6.Text = "TDD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 187);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 45;
            this.label5.Text = "CHO to eat";
            // 
            // txtChoToEat
            // 
            this.txtChoToEat.BackColor = System.Drawing.Color.GreenYellow;
            this.txtChoToEat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoToEat.Location = new System.Drawing.Point(391, 212);
            this.txtChoToEat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoToEat.Name = "txtChoToEat";
            this.txtChoToEat.Size = new System.Drawing.Size(68, 26);
            this.txtChoToEat.TabIndex = 3;
            this.txtChoToEat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoToEat.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(130, 187);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 20);
            this.label4.TabIndex = 42;
            this.label4.Text = "Target glucose";
            // 
            // btnSetParameters
            // 
            this.btnSetParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSetParameters.Location = new System.Drawing.Point(264, 130);
            this.btnSetParameters.Name = "btnSetParameters";
            this.btnSetParameters.Size = new System.Drawing.Size(92, 50);
            this.btnSetParameters.TabIndex = 100;
            this.btnSetParameters.Text = "Set parameters";
            this.btnSetParameters.UseVisualStyleBackColor = true;
            this.btnSetParameters.Click += new System.EventHandler(this.btnSetParameters_Click);
            // 
            // txtInsulinCorrectionSensitivity
            // 
            this.txtInsulinCorrectionSensitivity.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtInsulinCorrectionSensitivity.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtInsulinCorrectionSensitivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtInsulinCorrectionSensitivity.Location = new System.Drawing.Point(142, 142);
            this.txtInsulinCorrectionSensitivity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInsulinCorrectionSensitivity.Name = "txtInsulinCorrectionSensitivity";
            this.txtInsulinCorrectionSensitivity.ReadOnly = true;
            this.txtInsulinCorrectionSensitivity.Size = new System.Drawing.Size(68, 26);
            this.txtInsulinCorrectionSensitivity.TabIndex = 5;
            this.txtInsulinCorrectionSensitivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtInsulinCorrectionSensitivity.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtTotalDailyDoseOfInsulin
            // 
            this.txtTotalDailyDoseOfInsulin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTotalDailyDoseOfInsulin.Location = new System.Drawing.Point(27, 142);
            this.txtTotalDailyDoseOfInsulin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalDailyDoseOfInsulin.Name = "txtTotalDailyDoseOfInsulin";
            this.txtTotalDailyDoseOfInsulin.ReadOnly = true;
            this.txtTotalDailyDoseOfInsulin.Size = new System.Drawing.Size(68, 26);
            this.txtTotalDailyDoseOfInsulin.TabIndex = 21;
            this.txtTotalDailyDoseOfInsulin.TabStop = false;
            this.txtTotalDailyDoseOfInsulin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtTotalDailyDoseOfInsulin, "TDD = Total Daily Dose of insulin ");
            this.txtTotalDailyDoseOfInsulin.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 187);
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
            this.txtGlucoseBeforeMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGlucoseBeforeMeal.Location = new System.Drawing.Point(32, 212);
            this.txtGlucoseBeforeMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseBeforeMeal.Name = "txtGlucoseBeforeMeal";
            this.txtGlucoseBeforeMeal.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseBeforeMeal.TabIndex = 1;
            this.txtGlucoseBeforeMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.txtGlucoseBeforeMeal, "Measured glucose before meal");
            this.txtGlucoseBeforeMeal.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(92, 377);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 42);
            this.label11.TabIndex = 53;
            this.label11.Text = "Correction insulin [Ui]";
            this.toolTip1.SetToolTip(this.label11, "Insulin to be included in the bolus to correct for glucose more than target");
            // 
            // btnRoundInsulin
            // 
            this.btnRoundInsulin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRoundInsulin.Location = new System.Drawing.Point(436, 130);
            this.btnRoundInsulin.Name = "btnRoundInsulin";
            this.btnRoundInsulin.Size = new System.Drawing.Size(75, 50);
            this.btnRoundInsulin.TabIndex = 102;
            this.btnRoundInsulin.Text = "Round insulin";
            this.toolTip1.SetToolTip(this.btnRoundInsulin, "Calculate CHO that gives  integer insulin");
            this.btnRoundInsulin.UseVisualStyleBackColor = true;
            this.btnRoundInsulin.Click += new System.EventHandler(this.btnRoundInsulin_Click);
            // 
            // btnReadGlucose
            // 
            this.btnReadGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnReadGlucose.Location = new System.Drawing.Point(27, 248);
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
            this.btnSaveBolus.Enabled = false;
            this.btnSaveBolus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveBolus.Location = new System.Drawing.Point(434, 414);
            this.btnSaveBolus.Name = "btnSaveBolus";
            this.btnSaveBolus.Size = new System.Drawing.Size(75, 46);
            this.btnSaveBolus.TabIndex = 105;
            this.btnSaveBolus.Text = "Save bolus";
            this.toolTip1.SetToolTip(this.btnSaveBolus, "Calculate CHO that gives  integer insulin");
            this.btnSaveBolus.UseVisualStyleBackColor = true;
            this.btnSaveBolus.Click += new System.EventHandler(this.btnSaveBolus_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(198, 377);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 42);
            this.label8.TabIndex = 111;
            this.label8.Text = "Insulin due to CHO [Ui]";
            this.toolTip1.SetToolTip(this.label8, "Insulin to be included in the bolus to correct for glucose more than target");
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(314, 377);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 48);
            this.label2.TabIndex = 112;
            this.label2.Text = "Total insulin in bolus [Ui]";
            this.toolTip1.SetToolTip(this.label2, "Insulin to be included in the bolus to correct for glucose more than target");
            // 
            // btnOpenGlucose
            // 
            this.btnOpenGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenGlucose.Location = new System.Drawing.Point(346, 36);
            this.btnOpenGlucose.Name = "btnOpenGlucose";
            this.btnOpenGlucose.Size = new System.Drawing.Size(75, 50);
            this.btnOpenGlucose.TabIndex = 122;
            this.btnOpenGlucose.Text = "Open glucose";
            this.toolTip1.SetToolTip(this.btnOpenGlucose, "Open glucose measurements window");
            this.btnOpenGlucose.UseVisualStyleBackColor = true;
            this.btnOpenGlucose.Click += new System.EventHandler(this.btnOpenGlucose_Click);
            // 
            // txtReadCho
            // 
            this.txtReadCho.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtReadCho.Location = new System.Drawing.Point(388, 244);
            this.txtReadCho.Name = "txtReadCho";
            this.txtReadCho.Size = new System.Drawing.Size(75, 50);
            this.txtReadCho.TabIndex = 123;
            this.txtReadCho.Text = "Read CHO";
            this.toolTip1.SetToolTip(this.txtReadCho, "Read CHO from other Window");
            this.txtReadCho.UseVisualStyleBackColor = true;
            this.txtReadCho.Click += new System.EventHandler(this.txtReadCho_Click);
            // 
            // btnInjection
            // 
            this.btnInjection.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInjection.Location = new System.Drawing.Point(273, 246);
            this.btnInjection.Name = "btnInjection";
            this.btnInjection.Size = new System.Drawing.Size(75, 46);
            this.btnInjection.TabIndex = 124;
            this.btnInjection.Text = "Injection";
            this.toolTip1.SetToolTip(this.btnInjection, "Read last glucose value  from recorded glucose measurements");
            this.btnInjection.UseVisualStyleBackColor = true;
            this.btnInjection.Click += new System.EventHandler(this.btnInjection_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(241, 187);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 20);
            this.label10.TabIndex = 51;
            this.label10.Text = "Glucose to correct";
            // 
            // txtGlucoseToBeCorrected
            // 
            this.txtGlucoseToBeCorrected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGlucoseToBeCorrected.Location = new System.Drawing.Point(276, 212);
            this.txtGlucoseToBeCorrected.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGlucoseToBeCorrected.Name = "txtGlucoseToBeCorrected";
            this.txtGlucoseToBeCorrected.ReadOnly = true;
            this.txtGlucoseToBeCorrected.Size = new System.Drawing.Size(68, 26);
            this.txtGlucoseToBeCorrected.TabIndex = 33;
            this.txtGlucoseToBeCorrected.TabStop = false;
            this.txtGlucoseToBeCorrected.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGlucoseToBeCorrected.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtCorrectionInsulin
            // 
            this.txtCorrectionInsulin.BackColor = System.Drawing.Color.CadetBlue;
            this.txtCorrectionInsulin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCorrectionInsulin.Location = new System.Drawing.Point(103, 424);
            this.txtCorrectionInsulin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCorrectionInsulin.Name = "txtCorrectionInsulin";
            this.txtCorrectionInsulin.ReadOnly = true;
            this.txtCorrectionInsulin.Size = new System.Drawing.Size(68, 26);
            this.txtCorrectionInsulin.TabIndex = 48;
            this.txtCorrectionInsulin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCorrectionInsulin.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbIsSnack);
            this.groupBox1.Controls.Add(this.rdbIsBreakfast);
            this.groupBox1.Controls.Add(this.rdbIsDinner);
            this.groupBox1.Controls.Add(this.rdbIsLunch);
            this.groupBox1.Location = new System.Drawing.Point(85, 310);
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
            this.rdbIsSnack.Size = new System.Drawing.Size(69, 24);
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
            this.rdbIsBreakfast.Size = new System.Drawing.Size(94, 24);
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
            this.rdbIsDinner.Size = new System.Drawing.Size(71, 24);
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
            this.rdbIsLunch.Size = new System.Drawing.Size(65, 24);
            this.rdbIsLunch.TabIndex = 107;
            this.rdbIsLunch.TabStop = true;
            this.rdbIsLunch.Text = "lunch";
            this.rdbIsLunch.UseVisualStyleBackColor = true;
            // 
            // txtChoInsulinMeal
            // 
            this.txtChoInsulinMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinMeal.Location = new System.Drawing.Point(214, 424);
            this.txtChoInsulinMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinMeal.Name = "txtChoInsulinMeal";
            this.txtChoInsulinMeal.ReadOnly = true;
            this.txtChoInsulinMeal.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinMeal.TabIndex = 45;
            this.txtChoInsulinMeal.TabStop = false;
            this.txtChoInsulinMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoInsulinMeal.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtTargetGlucose
            // 
            this.txtTargetGlucose.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtTargetGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTargetGlucose.Location = new System.Drawing.Point(153, 212);
            this.txtTargetGlucose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTargetGlucose.Name = "txtTargetGlucose";
            this.txtTargetGlucose.Size = new System.Drawing.Size(68, 26);
            this.txtTargetGlucose.TabIndex = 7;
            this.txtTargetGlucose.Text = "120";
            this.txtTargetGlucose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStatusBar
            // 
            this.txtStatusBar.Location = new System.Drawing.Point(3, 466);
            this.txtStatusBar.Name = "txtStatusBar";
            this.txtStatusBar.Size = new System.Drawing.Size(506, 26);
            this.txtStatusBar.TabIndex = 113;
            // 
            // txtTotalInsulin
            // 
            this.txtTotalInsulin.BackColor = System.Drawing.Color.Aqua;
            this.txtTotalInsulin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTotalInsulin.Location = new System.Drawing.Point(332, 424);
            this.txtTotalInsulin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalInsulin.Name = "txtTotalInsulin";
            this.txtTotalInsulin.ReadOnly = true;
            this.txtTotalInsulin.Size = new System.Drawing.Size(68, 26);
            this.txtTotalInsulin.TabIndex = 114;
            this.txtTotalInsulin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnBolusCalculations
            // 
            this.btnBolusCalculations.Location = new System.Drawing.Point(436, 36);
            this.btnBolusCalculations.Name = "btnBolusCalculations";
            this.btnBolusCalculations.Size = new System.Drawing.Size(75, 50);
            this.btnBolusCalculations.TabIndex = 120;
            this.btnBolusCalculations.Text = "Calc. bolus";
            this.btnBolusCalculations.UseVisualStyleBackColor = true;
            this.btnBolusCalculations.Click += new System.EventHandler(this.btnBolusCalculations_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.txtChoInsulinRatioBreakfast);
            this.groupBox2.Controls.Add(this.txtChoInsulinRatioLunch);
            this.groupBox2.Controls.Add(this.txtChoInsulinRatioDinner);
            this.groupBox2.Location = new System.Drawing.Point(5, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(252, 88);
            this.groupBox2.TabIndex = 4;
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
            this.txtChoInsulinRatioBreakfast.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtChoInsulinRatioBreakfast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinRatioBreakfast.Location = new System.Drawing.Point(22, 49);
            this.txtChoInsulinRatioBreakfast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinRatioBreakfast.Name = "txtChoInsulinRatioBreakfast";
            this.txtChoInsulinRatioBreakfast.ReadOnly = true;
            this.txtChoInsulinRatioBreakfast.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinRatioBreakfast.TabIndex = 5;
            this.txtChoInsulinRatioBreakfast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChoInsulinRatioLunch
            // 
            this.txtChoInsulinRatioLunch.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtChoInsulinRatioLunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinRatioLunch.Location = new System.Drawing.Point(98, 49);
            this.txtChoInsulinRatioLunch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinRatioLunch.Name = "txtChoInsulinRatioLunch";
            this.txtChoInsulinRatioLunch.ReadOnly = true;
            this.txtChoInsulinRatioLunch.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinRatioLunch.TabIndex = 7;
            this.txtChoInsulinRatioLunch.Tag = "";
            this.txtChoInsulinRatioLunch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtChoInsulinRatioDinner
            // 
            this.txtChoInsulinRatioDinner.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtChoInsulinRatioDinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoInsulinRatioDinner.Location = new System.Drawing.Point(174, 49);
            this.txtChoInsulinRatioDinner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoInsulinRatioDinner.Name = "txtChoInsulinRatioDinner";
            this.txtChoInsulinRatioDinner.ReadOnly = true;
            this.txtChoInsulinRatioDinner.Size = new System.Drawing.Size(68, 26);
            this.txtChoInsulinRatioDinner.TabIndex = 9;
            this.txtChoInsulinRatioDinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmInsulinCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 498);
            this.Controls.Add(this.btnInjection);
            this.Controls.Add(this.txtReadCho);
            this.Controls.Add(this.btnOpenGlucose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnBolusCalculations);
            this.Controls.Add(this.txtTotalInsulin);
            this.Controls.Add(this.txtStatusBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtChoInsulinMeal);
            this.Controls.Add(this.btnSaveBolus);
            this.Controls.Add(this.btnReadGlucose);
            this.Controls.Add(this.btnRoundInsulin);
            this.Controls.Add(this.txtTargetGlucose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCorrectionInsulin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtGlucoseToBeCorrected);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtChoToEat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSetParameters);
            this.Controls.Add(this.txtInsulinCorrectionSensitivity);
            this.Controls.Add(this.txtTotalDailyDoseOfInsulin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGlucoseBeforeMeal);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmInsulinCalc";
            this.Text = "Calculation of insulin meal bolus, with corrections";
            this.Load += new System.EventHandler(this.frmInsulinCalc_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChoToEat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetParameters;
        private System.Windows.Forms.TextBox txtInsulinCorrectionSensitivity;
        private System.Windows.Forms.TextBox txtTotalDailyDoseOfInsulin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtGlucoseBeforeMeal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtGlucoseToBeCorrected;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCorrectionInsulin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtChoInsulinMeal;
        private System.Windows.Forms.TextBox txtTargetGlucose;
        private System.Windows.Forms.Button btnRoundInsulin;
        private System.Windows.Forms.Button btnReadGlucose;
        private System.Windows.Forms.Button btnSaveBolus;
        private System.Windows.Forms.RadioButton rdbIsSnack;
        private System.Windows.Forms.RadioButton rdbIsBreakfast;
        private System.Windows.Forms.RadioButton rdbIsDinner;
        private System.Windows.Forms.RadioButton rdbIsLunch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatusBar;
        private System.Windows.Forms.TextBox txtTotalInsulin;
        private Button btnBolusCalculations;
        private GroupBox groupBox2;
        private Label label20;
        private Label label21;
        private Label label22;
        private TextBox txtChoInsulinRatioBreakfast;
        private TextBox txtChoInsulinRatioLunch;
        private TextBox txtChoInsulinRatioDinner;
        private Button btnOpenGlucose;
        private Button txtReadCho;
        private Button btnInjection;
    }
}