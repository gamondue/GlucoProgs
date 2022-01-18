
namespace GlucoMan.Forms
{
    partial class frmMeal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeal));
            this.btnAddFood = new System.Windows.Forms.Button();
            this.btnRemoveFood = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbIsSnack = new System.Windows.Forms.RadioButton();
            this.rdbIsBreakfast = new System.Windows.Forms.RadioButton();
            this.rdbIsDinner = new System.Windows.Forms.RadioButton();
            this.rdbIsLunch = new System.Windows.Forms.RadioButton();
            this.dtpMealTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpMealTimeFinish = new System.Windows.Forms.DateTimePicker();
            this.btnStartMeal = new System.Windows.Forms.Button();
            this.btnEndMeal = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtChoOfMeal = new System.Windows.Forms.TextBox();
            this.txtIdMeal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAccuracyOfChoFoodInMeal = new System.Windows.Forms.TextBox();
            this.cmbAccuracyFoodInMeal = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSugarGrams = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFibersPercent = new System.Windows.Forms.TextBox();
            this.txtFoodChoPercent = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFoodQuantityGrams = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.grpFood = new System.Windows.Forms.GroupBox();
            this.btnWeighFood = new System.Windows.Forms.Button();
            this.btnSaveMeal = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIdMealInFood = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblIdFood = new System.Windows.Forms.Label();
            this.txtIdFood = new System.Windows.Forms.TextBox();
            this.btnFoodDetail = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFoodChoGrams = new System.Windows.Forms.TextBox();
            this.txtSugarPercent = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbAccuracyMeal = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAccuracyOfChoMeal = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGlucose = new System.Windows.Forms.Button();
            this.btnInsulin = new System.Windows.Forms.Button();
            this.btnSaveFood = new System.Windows.Forms.Button();
            this.gridFoods = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.grpFood.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddFood
            // 
            this.btnAddFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFood.Location = new System.Drawing.Point(606, 375);
            this.btnAddFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddFood.Name = "btnAddFood";
            this.btnAddFood.Size = new System.Drawing.Size(32, 32);
            this.btnAddFood.TabIndex = 1;
            this.btnAddFood.Text = "+";
            this.btnAddFood.UseVisualStyleBackColor = true;
            this.btnAddFood.Click += new System.EventHandler(this.btnAddFood_Click);
            // 
            // btnRemoveFood
            // 
            this.btnRemoveFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFood.Location = new System.Drawing.Point(646, 375);
            this.btnRemoveFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoveFood.Name = "btnRemoveFood";
            this.btnRemoveFood.Size = new System.Drawing.Size(32, 32);
            this.btnRemoveFood.TabIndex = 2;
            this.btnRemoveFood.Text = "- ";
            this.btnRemoveFood.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbIsSnack);
            this.groupBox1.Controls.Add(this.rdbIsBreakfast);
            this.groupBox1.Controls.Add(this.rdbIsDinner);
            this.groupBox1.Controls.Add(this.rdbIsLunch);
            this.groupBox1.Location = new System.Drawing.Point(7, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 56);
            this.groupBox1.TabIndex = 39;
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
            // dtpMealTimeStart
            // 
            this.dtpMealTimeStart.CustomFormat = "yyyy.MM.dd HH:mm:ss";
            this.dtpMealTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMealTimeStart.Location = new System.Drawing.Point(67, 120);
            this.dtpMealTimeStart.Name = "dtpMealTimeStart";
            this.dtpMealTimeStart.Size = new System.Drawing.Size(187, 26);
            this.dtpMealTimeStart.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 387);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 41;
            this.label1.Text = "Foods in meal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 42;
            this.label2.Text = "Meal start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(260, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 44;
            this.label3.Text = "Meal finish";
            // 
            // dtpMealTimeFinish
            // 
            this.dtpMealTimeFinish.CustomFormat = "yyyy.MM.dd HH:mm:ss";
            this.dtpMealTimeFinish.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMealTimeFinish.Location = new System.Drawing.Point(260, 120);
            this.dtpMealTimeFinish.Name = "dtpMealTimeFinish";
            this.dtpMealTimeFinish.Size = new System.Drawing.Size(187, 26);
            this.dtpMealTimeFinish.TabIndex = 43;
            // 
            // btnStartMeal
            // 
            this.btnStartMeal.Location = new System.Drawing.Point(180, 88);
            this.btnStartMeal.Name = "btnStartMeal";
            this.btnStartMeal.Size = new System.Drawing.Size(75, 29);
            this.btnStartMeal.TabIndex = 45;
            this.btnStartMeal.Text = "Start";
            this.btnStartMeal.UseVisualStyleBackColor = true;
            this.btnStartMeal.Click += new System.EventHandler(this.btnStartMeal_Click);
            // 
            // btnEndMeal
            // 
            this.btnEndMeal.Location = new System.Drawing.Point(372, 88);
            this.btnEndMeal.Name = "btnEndMeal";
            this.btnEndMeal.Size = new System.Drawing.Size(75, 29);
            this.btnEndMeal.TabIndex = 46;
            this.btnEndMeal.Text = "Finish";
            this.btnEndMeal.UseVisualStyleBackColor = true;
            this.btnEndMeal.Click += new System.EventHandler(this.btnEndMeal_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(368, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 20);
            this.label5.TabIndex = 48;
            this.label5.Text = "Total CHO";
            // 
            // txtChoOfMeal
            // 
            this.txtChoOfMeal.BackColor = System.Drawing.Color.Aqua;
            this.txtChoOfMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoOfMeal.Location = new System.Drawing.Point(375, 48);
            this.txtChoOfMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoOfMeal.Name = "txtChoOfMeal";
            this.txtChoOfMeal.ReadOnly = true;
            this.txtChoOfMeal.Size = new System.Drawing.Size(68, 26);
            this.txtChoOfMeal.TabIndex = 47;
            this.txtChoOfMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIdMeal
            // 
            this.txtIdMeal.Location = new System.Drawing.Point(9, 120);
            this.txtIdMeal.Name = "txtIdMeal";
            this.txtIdMeal.Size = new System.Drawing.Size(50, 26);
            this.txtIdMeal.TabIndex = 49;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 50;
            this.label4.Text = "Id Meal";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(348, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 52;
            this.label6.Text = "Accuracy";
            // 
            // txtAccuracyOfChoFoodInMeal
            // 
            this.txtAccuracyOfChoFoodInMeal.Location = new System.Drawing.Point(349, 45);
            this.txtAccuracyOfChoFoodInMeal.Name = "txtAccuracyOfChoFoodInMeal";
            this.txtAccuracyOfChoFoodInMeal.Size = new System.Drawing.Size(73, 26);
            this.txtAccuracyOfChoFoodInMeal.TabIndex = 53;
            this.txtAccuracyOfChoFoodInMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbAccuracyFoodInMeal
            // 
            this.cmbAccuracyFoodInMeal.FormattingEnabled = true;
            this.cmbAccuracyFoodInMeal.Location = new System.Drawing.Point(428, 44);
            this.cmbAccuracyFoodInMeal.Name = "cmbAccuracyFoodInMeal";
            this.cmbAccuracyFoodInMeal.Size = new System.Drawing.Size(158, 28);
            this.cmbAccuracyFoodInMeal.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(266, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 20);
            this.label8.TabIndex = 60;
            this.label8.Text = "Sugar [g]";
            // 
            // txtSugarGrams
            // 
            this.txtSugarGrams.Location = new System.Drawing.Point(265, 45);
            this.txtSugarGrams.Name = "txtSugarGrams";
            this.txtSugarGrams.ReadOnly = true;
            this.txtSugarGrams.Size = new System.Drawing.Size(74, 26);
            this.txtSugarGrams.TabIndex = 59;
            this.txtSugarGrams.TabStop = false;
            this.txtSugarGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(350, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 20);
            this.label7.TabIndex = 58;
            this.label7.Text = "Fibers %";
            // 
            // txtFibersPercent
            // 
            this.txtFibersPercent.Location = new System.Drawing.Point(348, 101);
            this.txtFibersPercent.Name = "txtFibersPercent";
            this.txtFibersPercent.Size = new System.Drawing.Size(74, 26);
            this.txtFibersPercent.TabIndex = 57;
            this.txtFibersPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFoodChoPercent
            // 
            this.txtFoodChoPercent.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodChoPercent.Location = new System.Drawing.Point(8, 45);
            this.txtFoodChoPercent.Name = "txtFoodChoPercent";
            this.txtFoodChoPercent.Size = new System.Drawing.Size(74, 26);
            this.txtFoodChoPercent.TabIndex = 1;
            this.txtFoodChoPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFoodChoPercent.TextChanged += new System.EventHandler(this.txtFoodChoPercent_TextChanged);
            this.txtFoodChoPercent.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 20);
            this.label9.TabIndex = 56;
            this.label9.Text = "CHO %";
            // 
            // txtFoodQuantityGrams
            // 
            this.txtFoodQuantityGrams.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodQuantityGrams.Location = new System.Drawing.Point(89, 45);
            this.txtFoodQuantityGrams.Name = "txtFoodQuantityGrams";
            this.txtFoodQuantityGrams.Size = new System.Drawing.Size(74, 26);
            this.txtFoodQuantityGrams.TabIndex = 2;
            this.txtFoodQuantityGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFoodQuantityGrams.TextChanged += new System.EventHandler(this.txtFoodQuantityGrams_TextChanged);
            this.txtFoodQuantityGrams.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(89, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 62;
            this.label10.Text = "Quant.[g]";
            // 
            // grpFood
            // 
            this.grpFood.Controls.Add(this.btnWeighFood);
            this.grpFood.Controls.Add(this.btnSaveMeal);
            this.grpFood.Controls.Add(this.label15);
            this.grpFood.Controls.Add(this.label16);
            this.grpFood.Controls.Add(this.txtDescription);
            this.grpFood.Controls.Add(this.label14);
            this.grpFood.Controls.Add(this.txtIdMealInFood);
            this.grpFood.Controls.Add(this.txtName);
            this.grpFood.Controls.Add(this.lblIdFood);
            this.grpFood.Controls.Add(this.txtIdFood);
            this.grpFood.Controls.Add(this.btnFoodDetail);
            this.grpFood.Controls.Add(this.label12);
            this.grpFood.Controls.Add(this.txtFoodChoGrams);
            this.grpFood.Controls.Add(this.txtSugarPercent);
            this.grpFood.Controls.Add(this.label11);
            this.grpFood.Controls.Add(this.txtFoodQuantityGrams);
            this.grpFood.Controls.Add(this.label10);
            this.grpFood.Controls.Add(this.label7);
            this.grpFood.Controls.Add(this.label9);
            this.grpFood.Controls.Add(this.txtSugarGrams);
            this.grpFood.Controls.Add(this.cmbAccuracyFoodInMeal);
            this.grpFood.Controls.Add(this.txtFibersPercent);
            this.grpFood.Controls.Add(this.txtFoodChoPercent);
            this.grpFood.Controls.Add(this.label6);
            this.grpFood.Controls.Add(this.txtAccuracyOfChoFoodInMeal);
            this.grpFood.Controls.Add(this.label8);
            this.grpFood.Location = new System.Drawing.Point(6, 177);
            this.grpFood.Name = "grpFood";
            this.grpFood.Size = new System.Drawing.Size(675, 190);
            this.grpFood.TabIndex = 63;
            this.grpFood.TabStop = false;
            this.grpFood.Text = "Food";
            // 
            // btnWeighFood
            // 
            this.btnWeighFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWeighFood.Location = new System.Drawing.Point(510, 88);
            this.btnWeighFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnWeighFood.Name = "btnWeighFood";
            this.btnWeighFood.Size = new System.Drawing.Size(76, 41);
            this.btnWeighFood.TabIndex = 80;
            this.btnWeighFood.Text = "Weigh Food";
            this.btnWeighFood.UseVisualStyleBackColor = true;
            this.btnWeighFood.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // btnSaveMeal
            // 
            this.btnSaveMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveMeal.Location = new System.Drawing.Point(593, 88);
            this.btnSaveMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveMeal.Name = "btnSaveMeal";
            this.btnSaveMeal.Size = new System.Drawing.Size(76, 41);
            this.btnSaveMeal.TabIndex = 79;
            this.btnSaveMeal.Text = "Save Meal";
            this.btnSaveMeal.UseVisualStyleBackColor = true;
            this.btnSaveMeal.Click += new System.EventHandler(this.btnSaveMeal_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(169, 135);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 20);
            this.label15.TabIndex = 76;
            this.label15.Text = "Description";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 78);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(103, 20);
            this.label16.TabIndex = 78;
            this.label16.Text = "Id Meal-Food";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(169, 158);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(503, 26);
            this.txtDescription.TabIndex = 75;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 135);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 20);
            this.label14.TabIndex = 74;
            this.label14.Text = "Name";
            // 
            // txtIdMealInFood
            // 
            this.txtIdMealInFood.Location = new System.Drawing.Point(32, 101);
            this.txtIdMealInFood.Name = "txtIdMealInFood";
            this.txtIdMealInFood.Size = new System.Drawing.Size(50, 26);
            this.txtIdMealInFood.TabIndex = 77;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.LightGreen;
            this.txtName.Location = new System.Drawing.Point(8, 158);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(155, 26);
            this.txtName.TabIndex = 4;
            // 
            // lblIdFood
            // 
            this.lblIdFood.AutoSize = true;
            this.lblIdFood.Location = new System.Drawing.Point(106, 78);
            this.lblIdFood.Name = "lblIdFood";
            this.lblIdFood.Size = new System.Drawing.Size(64, 20);
            this.lblIdFood.TabIndex = 72;
            this.lblIdFood.Text = "Id Food";
            // 
            // txtIdFood
            // 
            this.txtIdFood.Location = new System.Drawing.Point(113, 101);
            this.txtIdFood.Name = "txtIdFood";
            this.txtIdFood.Size = new System.Drawing.Size(50, 26);
            this.txtIdFood.TabIndex = 71;
            // 
            // btnFoodDetail
            // 
            this.btnFoodDetail.Location = new System.Drawing.Point(593, 37);
            this.btnFoodDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFoodDetail.Name = "btnFoodDetail";
            this.btnFoodDetail.Size = new System.Drawing.Size(76, 41);
            this.btnFoodDetail.TabIndex = 70;
            this.btnFoodDetail.Text = "Foods";
            this.btnFoodDetail.UseVisualStyleBackColor = true;
            this.btnFoodDetail.Click += new System.EventHandler(this.btnFoodDetail_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(174, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 20);
            this.label12.TabIndex = 66;
            this.label12.Text = "CHO [g]";
            // 
            // txtFoodChoGrams
            // 
            this.txtFoodChoGrams.BackColor = System.Drawing.Color.LightGreen;
            this.txtFoodChoGrams.Location = new System.Drawing.Point(169, 45);
            this.txtFoodChoGrams.Name = "txtFoodChoGrams";
            this.txtFoodChoGrams.Size = new System.Drawing.Size(74, 26);
            this.txtFoodChoGrams.TabIndex = 3;
            this.txtFoodChoGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFoodChoGrams.TextChanged += new System.EventHandler(this.txtFoodChoGrams_TextChanged);
            this.txtFoodChoGrams.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFoodChoGrams_KeyDown);
            // 
            // txtSugarPercent
            // 
            this.txtSugarPercent.Location = new System.Drawing.Point(265, 101);
            this.txtSugarPercent.Name = "txtSugarPercent";
            this.txtSugarPercent.Size = new System.Drawing.Size(74, 26);
            this.txtSugarPercent.TabIndex = 63;
            this.txtSugarPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(267, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 20);
            this.label11.TabIndex = 64;
            this.label11.Text = "Sugar %";
            // 
            // cmbAccuracyMeal
            // 
            this.cmbAccuracyMeal.FormattingEnabled = true;
            this.cmbAccuracyMeal.Location = new System.Drawing.Point(520, 118);
            this.cmbAccuracyMeal.Name = "cmbAccuracyMeal";
            this.cmbAccuracyMeal.Size = new System.Drawing.Size(145, 28);
            this.cmbAccuracyMeal.TabIndex = 69;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(454, 97);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 67;
            this.label13.Text = "Accuracy";
            // 
            // txtAccuracyOfChoMeal
            // 
            this.txtAccuracyOfChoMeal.Location = new System.Drawing.Point(454, 120);
            this.txtAccuracyOfChoMeal.Name = "txtAccuracyOfChoMeal";
            this.txtAccuracyOfChoMeal.Size = new System.Drawing.Size(60, 26);
            this.txtAccuracyOfChoMeal.TabIndex = 68;
            this.txtAccuracyOfChoMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbAccuracyMeal);
            this.groupBox2.Controls.Add(this.txtAccuracyOfChoMeal);
            this.groupBox2.Controls.Add(this.btnGlucose);
            this.groupBox2.Controls.Add(this.btnInsulin);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtIdMeal);
            this.groupBox2.Controls.Add(this.txtChoOfMeal);
            this.groupBox2.Controls.Add(this.dtpMealTimeStart);
            this.groupBox2.Controls.Add(this.btnEndMeal);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dtpMealTimeFinish);
            this.groupBox2.Controls.Add(this.btnStartMeal);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(675, 162);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meal";
            // 
            // btnGlucose
            // 
            this.btnGlucose.Location = new System.Drawing.Point(589, 69);
            this.btnGlucose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGlucose.Name = "btnGlucose";
            this.btnGlucose.Size = new System.Drawing.Size(76, 41);
            this.btnGlucose.TabIndex = 75;
            this.btnGlucose.Text = "Glucose";
            this.btnGlucose.UseVisualStyleBackColor = true;
            this.btnGlucose.Click += new System.EventHandler(this.btnGlucose_Click);
            // 
            // btnInsulin
            // 
            this.btnInsulin.Location = new System.Drawing.Point(589, 25);
            this.btnInsulin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInsulin.Name = "btnInsulin";
            this.btnInsulin.Size = new System.Drawing.Size(76, 41);
            this.btnInsulin.TabIndex = 73;
            this.btnInsulin.Text = "Insulin";
            this.btnInsulin.UseVisualStyleBackColor = true;
            this.btnInsulin.Click += new System.EventHandler(this.btnInsulin_Click);
            // 
            // btnSaveFood
            // 
            this.btnSaveFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveFood.Location = new System.Drawing.Point(515, 375);
            this.btnSaveFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveFood.Name = "btnSaveFood";
            this.btnSaveFood.Size = new System.Drawing.Size(83, 32);
            this.btnSaveFood.TabIndex = 71;
            this.btnSaveFood.Text = "Save food";
            this.btnSaveFood.UseVisualStyleBackColor = true;
            this.btnSaveFood.Click += new System.EventHandler(this.btnSaveFood_Click);
            // 
            // gridFoods
            // 
            this.gridFoods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFoods.Location = new System.Drawing.Point(9, 412);
            this.gridFoods.Name = "gridFoods";
            this.gridFoods.RowTemplate.Height = 25;
            this.gridFoods.Size = new System.Drawing.Size(669, 189);
            this.gridFoods.TabIndex = 72;
            this.gridFoods.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellClick);
            this.gridFoods.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellContentClick);
            this.gridFoods.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellDoubleClick);
            // 
            // frmMeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 613);
            this.Controls.Add(this.gridFoods);
            this.Controls.Add(this.btnSaveFood);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpFood);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemoveFood);
            this.Controls.Add(this.btnAddFood);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMeal";
            this.Text = "Sum the carbs to ingest";
            this.Load += new System.EventHandler(this.frmMeal_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpFood.ResumeLayout(false);
            this.grpFood.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddFood;
        private System.Windows.Forms.Button btnRemoveFood;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbIsSnack;
        private System.Windows.Forms.RadioButton rdbIsBreakfast;
        private System.Windows.Forms.RadioButton rdbIsDinner;
        private System.Windows.Forms.RadioButton rdbIsLunch;
        private System.Windows.Forms.DateTimePicker dtpMealTimeStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpMealTimeFinish;
        private System.Windows.Forms.Button btnStartMeal;
        private System.Windows.Forms.Button btnEndMeal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChoOfMeal;
        private TextBox txtIdMeal;
        private Label label4;
        private Label label6;
        private TextBox textBox1;
        private ComboBox cmbAccuracyFoodInMeal;
        private Label label8;
        private TextBox txtSugar;
        private Label label7;
        private TextBox txtFibers;
        private TextBox txtFoodChoPercent;
        private Label label9;
        private TextBox txtFoodQuantityGrams;
        private Label label10;
        private GroupBox grpFood;
        private TextBox textBox3;
        private Label label11;
        private Label label12;
        private TextBox txtFoodChoGrams;
        private ComboBox cmbAccuracyMeal;
        private Label label13;
        private TextBox txtAccuracyOfChoMeal;
        private Button btnFoodDetail;
        private Label lblIdFood;
        private TextBox txtIdFood;
        private GroupBox groupBox2;
        private Button btnGlucose;
        private Button btnInsulin;
        private Label label15;
        private TextBox txtDescription;
        private Label label14;
        private TextBox txtName;
        private Label label16;
        private TextBox textBox4;
        private Button btnSaveMeal;
        private Button btnSaveFood;
        private DataGridView gridFoods;
        private TextBox txtIdMealInFood;
        private TextBox txt;
        private TextBox txtAccuracyOfChoFoodInMeal;
        private TextBox txtSugarPercent;
        private TextBox txtFibersPercent;
        private TextBox txtSugarGrams;
        private Button btnWeighFood;
    }
}