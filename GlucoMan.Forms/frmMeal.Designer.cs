
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
            this.gridFoods = new System.Windows.Forms.DataGridView();
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cmbAccuracy = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSugar = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFibers = new System.Windows.Forms.TextBox();
            this.txtFoodChoPercent = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.grpFood = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblIdFood = new System.Windows.Forms.Label();
            this.txtIdFood = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnFoodDetail = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFoodChoGrams = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGlucose = new System.Windows.Forms.Button();
            this.btnBolus = new System.Windows.Forms.Button();
            this.btnInsulin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpFood.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridFoods
            // 
            this.gridFoods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFoods.Location = new System.Drawing.Point(6, 412);
            this.gridFoods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridFoods.Name = "gridFoods";
            this.gridFoods.Size = new System.Drawing.Size(648, 195);
            this.gridFoods.TabIndex = 0;
            this.gridFoods.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellClick);
            this.gridFoods.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellContentClick);
            // 
            // btnAddFood
            // 
            this.btnAddFood.Location = new System.Drawing.Point(584, 375);
            this.btnAddFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddFood.Name = "btnAddFood";
            this.btnAddFood.Size = new System.Drawing.Size(32, 32);
            this.btnAddFood.TabIndex = 1;
            this.btnAddFood.Text = "+ Food";
            this.btnAddFood.UseVisualStyleBackColor = true;
            this.btnAddFood.Click += new System.EventHandler(this.btnAddFood_Click);
            // 
            // btnRemoveFood
            // 
            this.btnRemoveFood.Location = new System.Drawing.Point(624, 375);
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
            this.dtpMealTimeStart.CustomFormat = "yyyy/mm/dd HH:MM.ss";
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
            this.dtpMealTimeFinish.CustomFormat = "yyyy/mm/dd HH:MM.ss";
            this.dtpMealTimeFinish.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMealTimeFinish.Location = new System.Drawing.Point(260, 120);
            this.dtpMealTimeFinish.Name = "dtpMealTimeFinish";
            this.dtpMealTimeFinish.Size = new System.Drawing.Size(187, 26);
            this.dtpMealTimeFinish.TabIndex = 43;
            // 
            // btnStartMeal
            // 
            this.btnStartMeal.Location = new System.Drawing.Point(175, 88);
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
            this.label5.Location = new System.Drawing.Point(456, 97);
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
            this.txtChoOfMeal.Location = new System.Drawing.Point(463, 120);
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
            this.label6.Location = new System.Drawing.Point(334, 77);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 52;
            this.label6.Text = "Accuracy";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(334, 102);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 26);
            this.textBox1.TabIndex = 53;
            // 
            // cmbAccuracy
            // 
            this.cmbAccuracy.FormattingEnabled = true;
            this.cmbAccuracy.Location = new System.Drawing.Point(404, 101);
            this.cmbAccuracy.Name = "cmbAccuracy";
            this.cmbAccuracy.Size = new System.Drawing.Size(158, 28);
            this.cmbAccuracy.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(251, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 20);
            this.label8.TabIndex = 60;
            this.label8.Text = "Sugar [g]";
            // 
            // txtSugar
            // 
            this.txtSugar.Location = new System.Drawing.Point(250, 102);
            this.txtSugar.Name = "txtSugar";
            this.txtSugar.Size = new System.Drawing.Size(74, 26);
            this.txtSugar.TabIndex = 59;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(334, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 20);
            this.label7.TabIndex = 58;
            this.label7.Text = "Fibers %";
            // 
            // txtFibers
            // 
            this.txtFibers.Location = new System.Drawing.Point(334, 154);
            this.txtFibers.Name = "txtFibers";
            this.txtFibers.Size = new System.Drawing.Size(74, 26);
            this.txtFibers.TabIndex = 57;
            // 
            // txtFoodChoPercent
            // 
            this.txtFoodChoPercent.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodChoPercent.Location = new System.Drawing.Point(154, 154);
            this.txtFoodChoPercent.Name = "txtFoodChoPercent";
            this.txtFoodChoPercent.Size = new System.Drawing.Size(74, 26);
            this.txtFoodChoPercent.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(160, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 20);
            this.label9.TabIndex = 56;
            this.label9.Text = "CHO %";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.GreenYellow;
            this.textBox2.Location = new System.Drawing.Point(74, 102);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(74, 26);
            this.textBox2.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(73, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 62;
            this.label10.Text = "Quant.[g]";
            // 
            // grpFood
            // 
            this.grpFood.Controls.Add(this.label15);
            this.grpFood.Controls.Add(this.txtDescription);
            this.grpFood.Controls.Add(this.label14);
            this.grpFood.Controls.Add(this.lblIdFood);
            this.grpFood.Controls.Add(this.txtIdFood);
            this.grpFood.Controls.Add(this.txtName);
            this.grpFood.Controls.Add(this.btnFoodDetail);
            this.grpFood.Controls.Add(this.label12);
            this.grpFood.Controls.Add(this.txtFoodChoGrams);
            this.grpFood.Controls.Add(this.textBox3);
            this.grpFood.Controls.Add(this.label11);
            this.grpFood.Controls.Add(this.textBox2);
            this.grpFood.Controls.Add(this.label10);
            this.grpFood.Controls.Add(this.label7);
            this.grpFood.Controls.Add(this.label9);
            this.grpFood.Controls.Add(this.txtSugar);
            this.grpFood.Controls.Add(this.cmbAccuracy);
            this.grpFood.Controls.Add(this.txtFibers);
            this.grpFood.Controls.Add(this.txtFoodChoPercent);
            this.grpFood.Controls.Add(this.label6);
            this.grpFood.Controls.Add(this.textBox1);
            this.grpFood.Controls.Add(this.label8);
            this.grpFood.Location = new System.Drawing.Point(6, 177);
            this.grpFood.Name = "grpFood";
            this.grpFood.Size = new System.Drawing.Size(648, 190);
            this.grpFood.TabIndex = 63;
            this.grpFood.TabStop = false;
            this.grpFood.Text = "Food";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(170, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 20);
            this.label15.TabIndex = 76;
            this.label15.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(170, 45);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(478, 26);
            this.txtDescription.TabIndex = 75;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(51, 20);
            this.label14.TabIndex = 74;
            this.label14.Text = "Name";
            // 
            // lblIdFood
            // 
            this.lblIdFood.AutoSize = true;
            this.lblIdFood.Location = new System.Drawing.Point(9, 79);
            this.lblIdFood.Name = "lblIdFood";
            this.lblIdFood.Size = new System.Drawing.Size(64, 20);
            this.lblIdFood.TabIndex = 72;
            this.lblIdFood.Text = "Id Food";
            // 
            // txtIdFood
            // 
            this.txtIdFood.Location = new System.Drawing.Point(9, 102);
            this.txtIdFood.Name = "txtIdFood";
            this.txtIdFood.Size = new System.Drawing.Size(50, 26);
            this.txtIdFood.TabIndex = 71;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.LightGreen;
            this.txtName.Location = new System.Drawing.Point(9, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(155, 26);
            this.txtName.TabIndex = 4;
            // 
            // btnFoodDetail
            // 
            this.btnFoodDetail.Location = new System.Drawing.Point(492, 139);
            this.btnFoodDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFoodDetail.Name = "btnFoodDetail";
            this.btnFoodDetail.Size = new System.Drawing.Size(69, 41);
            this.btnFoodDetail.TabIndex = 70;
            this.btnFoodDetail.Text = "Detail";
            this.btnFoodDetail.UseVisualStyleBackColor = true;
            this.btnFoodDetail.Click += new System.EventHandler(this.btnFoodDetail_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(159, 79);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 20);
            this.label12.TabIndex = 66;
            this.label12.Text = "CHO [g]";
            // 
            // txtFoodChoGrams
            // 
            this.txtFoodChoGrams.BackColor = System.Drawing.Color.LightGreen;
            this.txtFoodChoGrams.Location = new System.Drawing.Point(154, 102);
            this.txtFoodChoGrams.Name = "txtFoodChoGrams";
            this.txtFoodChoGrams.Size = new System.Drawing.Size(74, 26);
            this.txtFoodChoGrams.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(251, 154);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(74, 26);
            this.textBox3.TabIndex = 63;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(252, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 20);
            this.label11.TabIndex = 64;
            this.label11.Text = "Sugar %";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(404, 45);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(145, 28);
            this.comboBox1.TabIndex = 69;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(334, 24);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 67;
            this.label13.Text = "Accuracy";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(334, 47);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(60, 26);
            this.textBox5.TabIndex = 68;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGlucose);
            this.groupBox2.Controls.Add(this.btnBolus);
            this.groupBox2.Controls.Add(this.btnInsulin);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.txtIdMeal);
            this.groupBox2.Controls.Add(this.label13);
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
            this.groupBox2.Size = new System.Drawing.Size(648, 162);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meal";
            // 
            // btnGlucose
            // 
            this.btnGlucose.Location = new System.Drawing.Point(556, 68);
            this.btnGlucose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGlucose.Name = "btnGlucose";
            this.btnGlucose.Size = new System.Drawing.Size(76, 41);
            this.btnGlucose.TabIndex = 75;
            this.btnGlucose.Text = "Glucose";
            this.btnGlucose.UseVisualStyleBackColor = true;
            this.btnGlucose.Click += new System.EventHandler(this.btnGlucose_Click);
            // 
            // btnBolus
            // 
            this.btnBolus.Enabled = false;
            this.btnBolus.Location = new System.Drawing.Point(556, 112);
            this.btnBolus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBolus.Name = "btnBolus";
            this.btnBolus.Size = new System.Drawing.Size(76, 41);
            this.btnBolus.TabIndex = 74;
            this.btnBolus.Text = "Bolus";
            this.btnBolus.UseVisualStyleBackColor = true;
            this.btnBolus.Click += new System.EventHandler(this.btnBolus_Click);
            // 
            // btnInsulin
            // 
            this.btnInsulin.Location = new System.Drawing.Point(556, 24);
            this.btnInsulin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInsulin.Name = "btnInsulin";
            this.btnInsulin.Size = new System.Drawing.Size(76, 41);
            this.btnInsulin.TabIndex = 73;
            this.btnInsulin.Text = "Insulin";
            this.btnInsulin.UseVisualStyleBackColor = true;
            this.btnInsulin.Click += new System.EventHandler(this.btnInsulin_Click);
            // 
            // frmMeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 613);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpFood);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridFoods);
            this.Controls.Add(this.btnRemoveFood);
            this.Controls.Add(this.btnAddFood);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMeal";
            this.Text = "Sum the carbs to ingest";
            this.Load += new System.EventHandler(this.frmMeal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpFood.ResumeLayout(false);
            this.grpFood.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridFoods;
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
        private ComboBox cmbAccuracy;
        private Label label8;
        private TextBox txtSugar;
        private Label label7;
        private TextBox txtFibers;
        private TextBox txtFoodChoPercent;
        private Label label9;
        private TextBox textBox2;
        private Label label10;
        private GroupBox grpFood;
        private TextBox textBox3;
        private Label label11;
        private Label label12;
        private TextBox txtFoodChoGrams;
        private ComboBox comboBox1;
        private Label label13;
        private TextBox textBox5;
        private Button btnFoodDetail;
        private Label lblIdFood;
        private TextBox txtIdFood;
        private GroupBox groupBox2;
        private Button btnGlucose;
        private Button btnBolus;
        private Button btnInsulin;
        private Label label15;
        private TextBox txtDescription;
        private Label label14;
        private TextBox txtName;
    }
}