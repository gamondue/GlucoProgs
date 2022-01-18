namespace GlucoMan.Forms
{
    partial class frmMeals
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeals));
            this.gridMeals = new System.Windows.Forms.DataGridView();
            this.btnRemoveFood = new System.Windows.Forms.Button();
            this.btnAddMeal = new System.Windows.Forms.Button();
            this.btnShowThisMeal = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIdMeal = new System.Windows.Forms.TextBox();
            this.dtpMealTimeStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaveMeal = new System.Windows.Forms.Button();
            this.dtpMealTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAccuracyMeal = new System.Windows.Forms.ComboBox();
            this.txtAccuracyOfChoMeal = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbTypeOfMeal = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChoOfMeal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeals)).BeginInit();
            this.SuspendLayout();
            // 
            // gridMeals
            // 
            this.gridMeals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMeals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMeals.Location = new System.Drawing.Point(7, 119);
            this.gridMeals.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.gridMeals.Name = "gridMeals";
            this.gridMeals.Size = new System.Drawing.Size(579, 198);
            this.gridMeals.TabIndex = 11;
            this.gridMeals.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMeals_CellClick);
            this.gridMeals.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMeals_CellContentClick);
            this.gridMeals.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMeals_CellDoubleClick);
            // 
            // btnRemoveFood
            // 
            this.btnRemoveFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFood.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveFood.Location = new System.Drawing.Point(540, 70);
            this.btnRemoveFood.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveFood.Name = "btnRemoveFood";
            this.btnRemoveFood.Size = new System.Drawing.Size(46, 46);
            this.btnRemoveFood.TabIndex = 13;
            this.btnRemoveFood.Text = "- Food";
            this.btnRemoveFood.UseVisualStyleBackColor = true;
            this.btnRemoveFood.Click += new System.EventHandler(this.btnRemoveFood_Click);
            // 
            // btnAddMeal
            // 
            this.btnAddMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMeal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddMeal.Location = new System.Drawing.Point(490, 70);
            this.btnAddMeal.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddMeal.Name = "btnAddMeal";
            this.btnAddMeal.Size = new System.Drawing.Size(46, 46);
            this.btnAddMeal.TabIndex = 12;
            this.btnAddMeal.Text = "+ Food";
            this.btnAddMeal.UseVisualStyleBackColor = true;
            this.btnAddMeal.Click += new System.EventHandler(this.btnAddMeal_Click);
            // 
            // btnShowThisMeal
            // 
            this.btnShowThisMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowThisMeal.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnShowThisMeal.Location = new System.Drawing.Point(504, 20);
            this.btnShowThisMeal.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowThisMeal.Name = "btnShowThisMeal";
            this.btnShowThisMeal.Size = new System.Drawing.Size(82, 46);
            this.btnShowThisMeal.TabIndex = 14;
            this.btnShowThisMeal.Text = "This";
            this.btnShowThisMeal.UseVisualStyleBackColor = true;
            this.btnShowThisMeal.Click += new System.EventHandler(this.btnShowThisMeal_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 21);
            this.label4.TabIndex = 54;
            this.label4.Text = "Id Meal";
            // 
            // txtIdMeal
            // 
            this.txtIdMeal.Location = new System.Drawing.Point(7, 26);
            this.txtIdMeal.Name = "txtIdMeal";
            this.txtIdMeal.Size = new System.Drawing.Size(50, 29);
            this.txtIdMeal.TabIndex = 53;
            this.txtIdMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dtpMealTimeStart
            // 
            this.dtpMealTimeStart.CustomFormat = "yyyy.MM.dd HH:mm:ss";
            this.dtpMealTimeStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMealTimeStart.Location = new System.Drawing.Point(91, 84);
            this.dtpMealTimeStart.Name = "dtpMealTimeStart";
            this.dtpMealTimeStart.Size = new System.Drawing.Size(179, 29);
            this.dtpMealTimeStart.TabIndex = 51;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 21);
            this.label2.TabIndex = 52;
            this.label2.Text = "Meal start";
            // 
            // btnSaveMeal
            // 
            this.btnSaveMeal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMeal.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveMeal.Location = new System.Drawing.Point(407, 20);
            this.btnSaveMeal.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveMeal.Name = "btnSaveMeal";
            this.btnSaveMeal.Size = new System.Drawing.Size(82, 46);
            this.btnSaveMeal.TabIndex = 55;
            this.btnSaveMeal.Text = "Save";
            this.btnSaveMeal.UseVisualStyleBackColor = true;
            this.btnSaveMeal.Click += new System.EventHandler(this.btnSaveMeal_Click);
            // 
            // dtpMealTimeEnd
            // 
            this.dtpMealTimeEnd.CustomFormat = "yyyy.MM.dd HH:mm:ss";
            this.dtpMealTimeEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMealTimeEnd.Location = new System.Drawing.Point(275, 84);
            this.dtpMealTimeEnd.Name = "dtpMealTimeEnd";
            this.dtpMealTimeEnd.Size = new System.Drawing.Size(179, 29);
            this.dtpMealTimeEnd.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 57;
            this.label1.Text = "Meal end";
            // 
            // cmbAccuracyMeal
            // 
            this.cmbAccuracyMeal.FormattingEnabled = true;
            this.cmbAccuracyMeal.Location = new System.Drawing.Point(242, 26);
            this.cmbAccuracyMeal.Name = "cmbAccuracyMeal";
            this.cmbAccuracyMeal.Size = new System.Drawing.Size(145, 29);
            this.cmbAccuracyMeal.TabIndex = 72;
            this.cmbAccuracyMeal.SelectedIndexChanged += new System.EventHandler(this.cmbAccuracyMeal_SelectedIndexChanged);
            // 
            // txtAccuracyOfChoMeal
            // 
            this.txtAccuracyOfChoMeal.Location = new System.Drawing.Point(176, 26);
            this.txtAccuracyOfChoMeal.Name = "txtAccuracyOfChoMeal";
            this.txtAccuracyOfChoMeal.Size = new System.Drawing.Size(60, 29);
            this.txtAccuracyOfChoMeal.TabIndex = 71;
            this.txtAccuracyOfChoMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAccuracyOfChoMeal.TextChanged += new System.EventHandler(this.txtAccuracyOfChoMeal_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(176, 3);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 21);
            this.label13.TabIndex = 70;
            this.label13.Text = "Accuracy";
            // 
            // cmbTypeOfMeal
            // 
            this.cmbTypeOfMeal.FormattingEnabled = true;
            this.cmbTypeOfMeal.Location = new System.Drawing.Point(63, 26);
            this.cmbTypeOfMeal.Name = "cmbTypeOfMeal";
            this.cmbTypeOfMeal.Size = new System.Drawing.Size(107, 29);
            this.cmbTypeOfMeal.TabIndex = 73;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 21);
            this.label3.TabIndex = 74;
            this.label3.Text = "Type of meal";
            // 
            // txtChoOfMeal
            // 
            this.txtChoOfMeal.BackColor = System.Drawing.Color.Aqua;
            this.txtChoOfMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtChoOfMeal.Location = new System.Drawing.Point(7, 87);
            this.txtChoOfMeal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtChoOfMeal.Name = "txtChoOfMeal";
            this.txtChoOfMeal.Size = new System.Drawing.Size(68, 26);
            this.txtChoOfMeal.TabIndex = 75;
            this.txtChoOfMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 21);
            this.label5.TabIndex = 76;
            this.label5.Text = "Meal CHO";
            // 
            // frmMeals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 327);
            this.Controls.Add(this.txtChoOfMeal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbTypeOfMeal);
            this.Controls.Add(this.cmbAccuracyMeal);
            this.Controls.Add(this.txtAccuracyOfChoMeal);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.dtpMealTimeEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSaveMeal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIdMeal);
            this.Controls.Add(this.dtpMealTimeStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnShowThisMeal);
            this.Controls.Add(this.btnRemoveFood);
            this.Controls.Add(this.btnAddMeal);
            this.Controls.Add(this.gridMeals);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMeals";
            this.Text = "Meals management";
            this.Load += new System.EventHandler(this.frmMeals_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMeals)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView gridMeals;
        private Button btnRemoveFood;
        private Button btnAddMeal;
        private Button btnShowThisMeal;
        private Label label4;
        private TextBox txtIdMeal;
        private DateTimePicker dtpMealTimeStart;
        private Label label2;
        private DataGridViewTextBoxColumn IdMeal;
        private DataGridViewTextBoxColumn TypeOfMeal;
        private DataGridViewTextBoxColumn TimeBegin;
        private DataGridViewTextBoxColumn TimeEnd;
        private Button btnSaveMeal;
        private DateTimePicker dtpMealTimeEnd;
        private Label label1;
        private ComboBox cmbAccuracyMeal;
        private TextBox txtAccuracyOfChoMeal;
        private Label label13;
        private ComboBox cmbTypeOfMeal;
        private Label label3;
        private TextBox txtChoOfMeal;
        private Label label5;
    }
}