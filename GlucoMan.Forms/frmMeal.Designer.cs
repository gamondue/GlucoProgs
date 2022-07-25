
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbIsSnack = new System.Windows.Forms.RadioButton();
            this.rdbIsBreakfast = new System.Windows.Forms.RadioButton();
            this.rdbIsDinner = new System.Windows.Forms.RadioButton();
            this.rdbIsLunch = new System.Windows.Forms.RadioButton();
            this.dtpMealTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpMealTimeFinish = new System.Windows.Forms.DateTimePicker();
            this.btnStartMeal = new System.Windows.Forms.Button();
            this.btnEndMeal = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtChoOfMealGrams = new System.Windows.Forms.TextBox();
            this.txtIdMeal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAccuracyOfChoFoodInMeal = new System.Windows.Forms.TextBox();
            this.cmbAccuracyFoodInMeal = new System.Windows.Forms.ComboBox();
            this.txtFoodChoPercent = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFoodQuantityGrams = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.grpFood = new System.Windows.Forms.GroupBox();
            this.picCalculator = new System.Windows.Forms.PictureBox();
            this.btnSearchFood = new System.Windows.Forms.Button();
            this.btnWeighFood = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIdFoodInMeal = new System.Windows.Forms.TextBox();
            this.txtFoodInMealName = new System.Windows.Forms.TextBox();
            this.lblIdFood = new System.Windows.Forms.Label();
            this.txtIdFood = new System.Windows.Forms.TextBox();
            this.btnFoodDetail = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFoodChoGrams = new System.Windows.Forms.TextBox();
            this.btnSaveMeal = new System.Windows.Forms.Button();
            this.cmbAccuracyMeal = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAccuracyOfChoMeal = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveAllMeal = new System.Windows.Forms.Button();
            this.btnSumCho = new System.Windows.Forms.Button();
            this.btnGlucose = new System.Windows.Forms.Button();
            this.btnInsulin = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSaveAllFoods = new System.Windows.Forms.Button();
            this.btnSaveFoodInMeal = new System.Windows.Forms.Button();
            this.btnRemoveFoodInMeal = new System.Windows.Forms.Button();
            this.grpFoodsInMeals = new System.Windows.Forms.GroupBox();
            this.gridFoodsInMeal = new System.Windows.Forms.DataGridView();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.btnAddFood = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grpFood.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCalculator)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpFoodsInMeals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoodsInMeal)).BeginInit();
            this.SuspendLayout();
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
            // txtChoOfMealGrams
            // 
            this.txtChoOfMealGrams.BackColor = System.Drawing.Color.Aqua;
            this.txtChoOfMealGrams.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoOfMealGrams.Location = new System.Drawing.Point(375, 48);
            this.txtChoOfMealGrams.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoOfMealGrams.Name = "txtChoOfMealGrams";
            this.txtChoOfMealGrams.ReadOnly = true;
            this.txtChoOfMealGrams.Size = new System.Drawing.Size(68, 26);
            this.txtChoOfMealGrams.TabIndex = 47;
            this.txtChoOfMealGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChoOfMealGrams.TextChanged += new System.EventHandler(this.txtChoOfMealGrams_TextChanged);
            // 
            // txtIdMeal
            // 
            this.txtIdMeal.Location = new System.Drawing.Point(9, 120);
            this.txtIdMeal.Name = "txtIdMeal";
            this.txtIdMeal.ReadOnly = true;
            this.txtIdMeal.Size = new System.Drawing.Size(50, 26);
            this.txtIdMeal.TabIndex = 49;
            this.txtIdMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.label6.Location = new System.Drawing.Point(461, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 52;
            this.label6.Text = "Accuracy";
            // 
            // txtAccuracyOfChoFoodInMeal
            // 
            this.txtAccuracyOfChoFoodInMeal.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtAccuracyOfChoFoodInMeal.Location = new System.Drawing.Point(461, 48);
            this.txtAccuracyOfChoFoodInMeal.Name = "txtAccuracyOfChoFoodInMeal";
            this.txtAccuracyOfChoFoodInMeal.Size = new System.Drawing.Size(60, 26);
            this.txtAccuracyOfChoFoodInMeal.TabIndex = 4;
            this.txtAccuracyOfChoFoodInMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAccuracyOfChoFoodInMeal.TextChanged += new System.EventHandler(this.txtAccuracyOfChoFoodInMeal_TextChanged);
            this.txtAccuracyOfChoFoodInMeal.Leave += new System.EventHandler(this.txtAccuracyOfChoFoodInMeal_Leave);
            // 
            // cmbAccuracyFoodInMeal
            // 
            this.cmbAccuracyFoodInMeal.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.cmbAccuracyFoodInMeal.FormattingEnabled = true;
            this.cmbAccuracyFoodInMeal.Location = new System.Drawing.Point(527, 46);
            this.cmbAccuracyFoodInMeal.Name = "cmbAccuracyFoodInMeal";
            this.cmbAccuracyFoodInMeal.Size = new System.Drawing.Size(158, 28);
            this.cmbAccuracyFoodInMeal.TabIndex = 7;
            // 
            // txtFoodChoPercent
            // 
            this.txtFoodChoPercent.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodChoPercent.Location = new System.Drawing.Point(180, 49);
            this.txtFoodChoPercent.Name = "txtFoodChoPercent";
            this.txtFoodChoPercent.Size = new System.Drawing.Size(74, 26);
            this.txtFoodChoPercent.TabIndex = 1;
            this.txtFoodChoPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFoodChoPercent.TextChanged += new System.EventHandler(this.txtFoodChoPercent_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(186, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 20);
            this.label9.TabIndex = 56;
            this.label9.Text = "CHO %";
            // 
            // txtFoodQuantityGrams
            // 
            this.txtFoodQuantityGrams.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodQuantityGrams.Location = new System.Drawing.Point(261, 49);
            this.txtFoodQuantityGrams.Name = "txtFoodQuantityGrams";
            this.txtFoodQuantityGrams.Size = new System.Drawing.Size(74, 26);
            this.txtFoodQuantityGrams.TabIndex = 2;
            this.txtFoodQuantityGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFoodQuantityGrams.TextChanged += new System.EventHandler(this.txtFoodQuantityGrams_TextChanged);
            this.txtFoodQuantityGrams.Leave += new System.EventHandler(this.txtFoodQuantityGrams_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(261, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 62;
            this.label10.Text = "Quant.[g]";
            // 
            // grpFood
            // 
            this.grpFood.Controls.Add(this.picCalculator);
            this.grpFood.Controls.Add(this.btnSearchFood);
            this.grpFood.Controls.Add(this.btnWeighFood);
            this.grpFood.Controls.Add(this.label16);
            this.grpFood.Controls.Add(this.label14);
            this.grpFood.Controls.Add(this.txtIdFoodInMeal);
            this.grpFood.Controls.Add(this.txtFoodInMealName);
            this.grpFood.Controls.Add(this.lblIdFood);
            this.grpFood.Controls.Add(this.txtIdFood);
            this.grpFood.Controls.Add(this.btnFoodDetail);
            this.grpFood.Controls.Add(this.label12);
            this.grpFood.Controls.Add(this.txtFoodChoGrams);
            this.grpFood.Controls.Add(this.txtFoodQuantityGrams);
            this.grpFood.Controls.Add(this.label10);
            this.grpFood.Controls.Add(this.label9);
            this.grpFood.Controls.Add(this.cmbAccuracyFoodInMeal);
            this.grpFood.Controls.Add(this.txtFoodChoPercent);
            this.grpFood.Controls.Add(this.label6);
            this.grpFood.Controls.Add(this.txtAccuracyOfChoFoodInMeal);
            this.grpFood.Location = new System.Drawing.Point(6, 177);
            this.grpFood.Name = "grpFood";
            this.grpFood.Size = new System.Drawing.Size(708, 148);
            this.grpFood.TabIndex = 63;
            this.grpFood.TabStop = false;
            this.grpFood.Text = "Food";
            // 
            // picCalculator
            // 
            this.picCalculator.Image = global::GlucoMan.Forms.Properties.Resources.Calculator1;
            this.picCalculator.Location = new System.Drawing.Point(421, 46);
            this.picCalculator.Name = "picCalculator";
            this.picCalculator.Size = new System.Drawing.Size(34, 33);
            this.picCalculator.TabIndex = 85;
            this.picCalculator.TabStop = false;
            this.picCalculator.Click += new System.EventHandler(this.picCalculator_Click);
            // 
            // btnSearchFood
            // 
            this.btnSearchFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSearchFood.Location = new System.Drawing.Point(258, 97);
            this.btnSearchFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearchFood.Name = "btnSearchFood";
            this.btnSearchFood.Size = new System.Drawing.Size(76, 41);
            this.btnSearchFood.TabIndex = 81;
            this.btnSearchFood.Text = "Search Food";
            this.btnSearchFood.UseVisualStyleBackColor = true;
            this.btnSearchFood.Click += new System.EventHandler(this.btnSearchFood_Click);
            // 
            // btnWeighFood
            // 
            this.btnWeighFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWeighFood.Location = new System.Drawing.Point(338, 97);
            this.btnWeighFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnWeighFood.Name = "btnWeighFood";
            this.btnWeighFood.Size = new System.Drawing.Size(76, 41);
            this.btnWeighFood.TabIndex = 80;
            this.btnWeighFood.Text = "Weigh Food";
            this.btnWeighFood.UseVisualStyleBackColor = true;
            this.btnWeighFood.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 27);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(103, 20);
            this.label16.TabIndex = 78;
            this.label16.Text = "Id Meal-Food";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 81);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 20);
            this.label14.TabIndex = 74;
            this.label14.Text = "Name";
            // 
            // txtIdFoodInMeal
            // 
            this.txtIdFoodInMeal.Location = new System.Drawing.Point(32, 49);
            this.txtIdFoodInMeal.Name = "txtIdFoodInMeal";
            this.txtIdFoodInMeal.ReadOnly = true;
            this.txtIdFoodInMeal.Size = new System.Drawing.Size(50, 26);
            this.txtIdFoodInMeal.TabIndex = 77;
            this.txtIdFoodInMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFoodInMealName
            // 
            this.txtFoodInMealName.BackColor = System.Drawing.Color.LightGreen;
            this.txtFoodInMealName.Location = new System.Drawing.Point(9, 104);
            this.txtFoodInMealName.Name = "txtFoodInMealName";
            this.txtFoodInMealName.Size = new System.Drawing.Size(155, 26);
            this.txtFoodInMealName.TabIndex = 3;
            this.txtFoodInMealName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblIdFood
            // 
            this.lblIdFood.AutoSize = true;
            this.lblIdFood.Location = new System.Drawing.Point(106, 27);
            this.lblIdFood.Name = "lblIdFood";
            this.lblIdFood.Size = new System.Drawing.Size(64, 20);
            this.lblIdFood.TabIndex = 72;
            this.lblIdFood.Text = "Id Food";
            // 
            // txtIdFood
            // 
            this.txtIdFood.Location = new System.Drawing.Point(113, 49);
            this.txtIdFood.Name = "txtIdFood";
            this.txtIdFood.ReadOnly = true;
            this.txtIdFood.Size = new System.Drawing.Size(50, 26);
            this.txtIdFood.TabIndex = 71;
            this.txtIdFood.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnFoodDetail
            // 
            this.btnFoodDetail.Location = new System.Drawing.Point(178, 97);
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
            this.label12.Location = new System.Drawing.Point(346, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 20);
            this.label12.TabIndex = 66;
            this.label12.Text = "CHO [g]";
            // 
            // txtFoodChoGrams
            // 
            this.txtFoodChoGrams.BackColor = System.Drawing.Color.LightGreen;
            this.txtFoodChoGrams.Location = new System.Drawing.Point(341, 49);
            this.txtFoodChoGrams.Name = "txtFoodChoGrams";
            this.txtFoodChoGrams.Size = new System.Drawing.Size(74, 26);
            this.txtFoodChoGrams.TabIndex = 5;
            this.txtFoodChoGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFoodChoGrams.TextChanged += new System.EventHandler(this.txtFoodChoGrams_TextChanged);
            this.txtFoodChoGrams.Leave += new System.EventHandler(this.txtFoodChoGrams_Leave);
            // 
            // btnSaveMeal
            // 
            this.btnSaveMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveMeal.Location = new System.Drawing.Point(536, 25);
            this.btnSaveMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveMeal.Name = "btnSaveMeal";
            this.btnSaveMeal.Size = new System.Drawing.Size(84, 41);
            this.btnSaveMeal.TabIndex = 79;
            this.btnSaveMeal.Text = "Save Meal";
            this.btnSaveMeal.UseVisualStyleBackColor = true;
            this.btnSaveMeal.Click += new System.EventHandler(this.btnSaveMeal_Click);
            // 
            // cmbAccuracyMeal
            // 
            this.cmbAccuracyMeal.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.cmbAccuracyMeal.CausesValidation = false;
            this.cmbAccuracyMeal.FormattingEnabled = true;
            this.cmbAccuracyMeal.Location = new System.Drawing.Point(527, 120);
            this.cmbAccuracyMeal.Name = "cmbAccuracyMeal";
            this.cmbAccuracyMeal.Size = new System.Drawing.Size(158, 28);
            this.cmbAccuracyMeal.TabIndex = 69;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(461, 99);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 67;
            this.label13.Text = "Accuracy";
            // 
            // txtAccuracyOfChoMeal
            // 
            this.txtAccuracyOfChoMeal.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.txtAccuracyOfChoMeal.Location = new System.Drawing.Point(461, 122);
            this.txtAccuracyOfChoMeal.Name = "txtAccuracyOfChoMeal";
            this.txtAccuracyOfChoMeal.Size = new System.Drawing.Size(60, 26);
            this.txtAccuracyOfChoMeal.TabIndex = 68;
            this.txtAccuracyOfChoMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveAllMeal);
            this.groupBox2.Controls.Add(this.btnSumCho);
            this.groupBox2.Controls.Add(this.cmbAccuracyMeal);
            this.groupBox2.Controls.Add(this.txtAccuracyOfChoMeal);
            this.groupBox2.Controls.Add(this.btnSaveMeal);
            this.groupBox2.Controls.Add(this.btnGlucose);
            this.groupBox2.Controls.Add(this.btnInsulin);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtIdMeal);
            this.groupBox2.Controls.Add(this.txtChoOfMealGrams);
            this.groupBox2.Controls.Add(this.dtpMealTimeStart);
            this.groupBox2.Controls.Add(this.btnEndMeal);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dtpMealTimeFinish);
            this.groupBox2.Controls.Add(this.btnStartMeal);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(708, 162);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meal";
            // 
            // btnSaveAllMeal
            // 
            this.btnSaveAllMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAllMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveAllMeal.Location = new System.Drawing.Point(536, 69);
            this.btnSaveAllMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveAllMeal.Name = "btnSaveAllMeal";
            this.btnSaveAllMeal.Size = new System.Drawing.Size(84, 41);
            this.btnSaveAllMeal.TabIndex = 75;
            this.btnSaveAllMeal.Text = "Save meal and foods";
            this.toolTip1.SetToolTip(this.btnSaveAllMeal, "Save food data in persistent foods");
            this.btnSaveAllMeal.UseVisualStyleBackColor = true;
            this.btnSaveAllMeal.Click += new System.EventHandler(this.btnSaveAllMeal_Click);
            // 
            // btnSumCho
            // 
            this.btnSumCho.Location = new System.Drawing.Point(461, 47);
            this.btnSumCho.Name = "btnSumCho";
            this.btnSumCho.Size = new System.Drawing.Size(60, 29);
            this.btnSumCho.TabIndex = 76;
            this.btnSumCho.Text = "Calc.";
            this.btnSumCho.UseVisualStyleBackColor = true;
            this.btnSumCho.Click += new System.EventHandler(this.btnSumCho_Click);
            // 
            // btnGlucose
            // 
            this.btnGlucose.Location = new System.Drawing.Point(623, 69);
            this.btnGlucose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGlucose.Name = "btnGlucose";
            this.btnGlucose.Size = new System.Drawing.Size(84, 41);
            this.btnGlucose.TabIndex = 75;
            this.btnGlucose.Text = "Glucose";
            this.btnGlucose.UseVisualStyleBackColor = true;
            this.btnGlucose.Click += new System.EventHandler(this.btnGlucose_Click);
            // 
            // btnInsulin
            // 
            this.btnInsulin.Location = new System.Drawing.Point(623, 25);
            this.btnInsulin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInsulin.Name = "btnInsulin";
            this.btnInsulin.Size = new System.Drawing.Size(84, 41);
            this.btnInsulin.TabIndex = 73;
            this.btnInsulin.Text = "Insulin";
            this.btnInsulin.UseVisualStyleBackColor = true;
            this.btnInsulin.Click += new System.EventHandler(this.btnInsulin_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Add a new Meal\'s food";
            // 
            // btnSaveAllFoods
            // 
            this.btnSaveAllFoods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAllFoods.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveAllFoods.Location = new System.Drawing.Point(613, 22);
            this.btnSaveAllFoods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveAllFoods.Name = "btnSaveAllFoods";
            this.btnSaveAllFoods.Size = new System.Drawing.Size(83, 45);
            this.btnSaveAllFoods.TabIndex = 82;
            this.btnSaveAllFoods.Text = "Save all foods";
            this.toolTip1.SetToolTip(this.btnSaveAllFoods, "Save food data in persistent foods");
            this.btnSaveAllFoods.UseVisualStyleBackColor = true;
            this.btnSaveAllFoods.Click += new System.EventHandler(this.btnSaveAllFoods_Click);
            // 
            // btnSaveFoodInMeal
            // 
            this.btnSaveFoodInMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFoodInMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveFoodInMeal.Location = new System.Drawing.Point(525, 22);
            this.btnSaveFoodInMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveFoodInMeal.Name = "btnSaveFoodInMeal";
            this.btnSaveFoodInMeal.Size = new System.Drawing.Size(83, 45);
            this.btnSaveFoodInMeal.TabIndex = 79;
            this.btnSaveFoodInMeal.Text = "Save this food";
            this.toolTip1.SetToolTip(this.btnSaveFoodInMeal, "Save food data in persistent foods");
            this.btnSaveFoodInMeal.UseVisualStyleBackColor = true;
            this.btnSaveFoodInMeal.Click += new System.EventHandler(this.btnSaveFoodInMeal_Click);
            // 
            // btnRemoveFoodInMeal
            // 
            this.btnRemoveFoodInMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFoodInMeal.Location = new System.Drawing.Point(437, 22);
            this.btnRemoveFoodInMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoveFoodInMeal.Name = "btnRemoveFoodInMeal";
            this.btnRemoveFoodInMeal.Size = new System.Drawing.Size(83, 45);
            this.btnRemoveFoodInMeal.TabIndex = 77;
            this.btnRemoveFoodInMeal.Text = "- Food";
            this.toolTip1.SetToolTip(this.btnRemoveFoodInMeal, "Remove the Meal\'s food");
            this.btnRemoveFoodInMeal.UseVisualStyleBackColor = true;
            this.btnRemoveFoodInMeal.Click += new System.EventHandler(this.btnRemoveFoodInMeal_Click);
            // 
            // grpFoodsInMeals
            // 
            this.grpFoodsInMeals.Controls.Add(this.btnSaveAllFoods);
            this.grpFoodsInMeals.Controls.Add(this.gridFoodsInMeal);
            this.grpFoodsInMeals.Controls.Add(this.btnDefaults);
            this.grpFoodsInMeals.Controls.Add(this.btnSaveFoodInMeal);
            this.grpFoodsInMeals.Controls.Add(this.btnRemoveFoodInMeal);
            this.grpFoodsInMeals.Controls.Add(this.btnAddFood);
            this.grpFoodsInMeals.Location = new System.Drawing.Point(6, 323);
            this.grpFoodsInMeals.Name = "grpFoodsInMeals";
            this.grpFoodsInMeals.Size = new System.Drawing.Size(707, 320);
            this.grpFoodsInMeals.TabIndex = 76;
            this.grpFoodsInMeals.TabStop = false;
            this.grpFoodsInMeals.Text = "Foods in meal";
            // 
            // gridFoodsInMeal
            // 
            this.gridFoodsInMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFoodsInMeal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFoodsInMeal.Location = new System.Drawing.Point(1, 75);
            this.gridFoodsInMeal.Name = "gridFoodsInMeal";
            this.gridFoodsInMeal.RowTemplate.Height = 25;
            this.gridFoodsInMeal.Size = new System.Drawing.Size(698, 245);
            this.gridFoodsInMeal.TabIndex = 81;
            this.gridFoodsInMeal.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoodsInMeal_CellClick);
            this.gridFoodsInMeal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoodsInMeal_CellContentClick);
            this.gridFoodsInMeal.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoodsInMeal_CellDoubleClick);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDefaults.Location = new System.Drawing.Point(261, 22);
            this.btnDefaults.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(83, 45);
            this.btnDefaults.TabIndex = 80;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // btnAddFood
            // 
            this.btnAddFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFood.Location = new System.Drawing.Point(349, 22);
            this.btnAddFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddFood.Name = "btnAddFood";
            this.btnAddFood.Size = new System.Drawing.Size(83, 45);
            this.btnAddFood.TabIndex = 76;
            this.btnAddFood.Text = "+ Food";
            this.btnAddFood.UseVisualStyleBackColor = true;
            this.btnAddFood.Click += new System.EventHandler(this.btnAddFoodInMeal_Click);
            // 
            // frmMeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 653);
            this.Controls.Add(this.grpFoodsInMeals);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpFood);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMeal";
            this.Text = "Foods in a meal";
            this.Load += new System.EventHandler(this.frmMeal_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpFood.ResumeLayout(false);
            this.grpFood.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCalculator)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpFoodsInMeals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFoodsInMeal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddFood;
        private System.Windows.Forms.Button btnRemoveFoodInMeal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbIsSnack;
        private System.Windows.Forms.RadioButton rdbIsBreakfast;
        private System.Windows.Forms.RadioButton rdbIsDinner;
        private System.Windows.Forms.RadioButton rdbIsLunch;
        private System.Windows.Forms.DateTimePicker dtpMealTimeStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpMealTimeFinish;
        private System.Windows.Forms.Button btnStartMeal;
        private System.Windows.Forms.Button btnEndMeal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChoOfMealGrams;
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
        private Label label14;
        private TextBox txtFoodInMealName;
        private Label label16;
        private TextBox textBox4;
        private Button btnSaveMeal;
        private Button btnSaveFoodInMeal;
        private DataGridView gridFoodsInMeal;
        private TextBox txtIdFoodInMeal;
        private TextBox txt;
        private TextBox txtAccuracyOfChoFoodInMeal;
        private TextBox txtSugarPercent;
        private TextBox txtFibersPercent;
        private TextBox txtSugarGrams;
        private Button btnWeighFood;
        private Button btnNewData;
        private Button btnSumCho;
        private Button btnSearchFood;
        private ToolTip toolTip1;
        private PictureBox picCalculator;
        private Button btnSaveAllMeal;
        private Button btnSaveAllFoods;
        private GroupBox grpFoodsInMeals;
        private Button btnDefaults;
    }
}