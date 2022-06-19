namespace GlucoMan.Forms
{
    partial class frmFoods
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
            this.btnNewFood = new System.Windows.Forms.Button();
            this.btnFatSecret = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMonoinsaturatedFats = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSaveFood = new System.Windows.Forms.Button();
            this.btnSearchFood = new System.Windows.Forms.Button();
            this.lblIdFood = new System.Windows.Forms.Label();
            this.txtIdFood = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTotalFats = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPolinsaturatedFats = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSugar = new System.Windows.Forms.TextBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpFood = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSaturatedFats = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProteins = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSalt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFibers = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCalories = new System.Windows.Forms.TextBox();
            this.txtFoodCarbohydrates = new System.Windows.Forms.TextBox();
            this.gridFoods = new System.Windows.Forms.DataGridView();
            this.grpFood.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNewFood
            // 
            this.btnNewFood.Location = new System.Drawing.Point(213, 28);
            this.btnNewFood.Name = "btnNewFood";
            this.btnNewFood.Size = new System.Drawing.Size(75, 58);
            this.btnNewFood.TabIndex = 90;
            this.btnNewFood.Text = "New Food";
            this.btnNewFood.UseVisualStyleBackColor = true;
            this.btnNewFood.Click += new System.EventHandler(this.btnNewFood_Click);
            // 
            // btnFatSecret
            // 
            this.btnFatSecret.Enabled = false;
            this.btnFatSecret.Location = new System.Drawing.Point(539, 28);
            this.btnFatSecret.Name = "btnFatSecret";
            this.btnFatSecret.Size = new System.Drawing.Size(75, 58);
            this.btnFatSecret.TabIndex = 89;
            this.btnFatSecret.Text = "Fat Secret";
            this.btnFatSecret.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(354, 27);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(120, 21);
            this.label16.TabIndex = 31;
            this.label16.Text = "Mono fats  [g]%";
            this.label16.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 21);
            this.label1.TabIndex = 75;
            this.label1.Text = "Foods";
            // 
            // txtMonoinsaturatedFats
            // 
            this.txtMonoinsaturatedFats.Location = new System.Drawing.Point(354, 50);
            this.txtMonoinsaturatedFats.Name = "txtMonoinsaturatedFats";
            this.txtMonoinsaturatedFats.Size = new System.Drawing.Size(100, 29);
            this.txtMonoinsaturatedFats.TabIndex = 30;
            this.txtMonoinsaturatedFats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtMonoinsaturatedFats.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(294, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 58);
            this.btnSave.TabIndex = 88;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(353, 145);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 21);
            this.label14.TabIndex = 27;
            this.label14.Text = "Potassium  [g]%";
            this.label14.Visible = false;
            // 
            // btnSaveFood
            // 
            this.btnSaveFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveFood.Location = new System.Drawing.Point(1357, 89);
            this.btnSaveFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveFood.Name = "btnSaveFood";
            this.btnSaveFood.Size = new System.Drawing.Size(83, 41);
            this.btnSaveFood.TabIndex = 82;
            this.btnSaveFood.Text = "Save food";
            this.btnSaveFood.UseVisualStyleBackColor = true;
            // 
            // btnSearchFood
            // 
            this.btnSearchFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSearchFood.Location = new System.Drawing.Point(375, 28);
            this.btnSearchFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearchFood.Name = "btnSearchFood";
            this.btnSearchFood.Size = new System.Drawing.Size(76, 58);
            this.btnSearchFood.TabIndex = 81;
            this.btnSearchFood.Text = "Search";
            this.btnSearchFood.UseVisualStyleBackColor = true;
            // 
            // lblIdFood
            // 
            this.lblIdFood.AutoSize = true;
            this.lblIdFood.Location = new System.Drawing.Point(-1, 24);
            this.lblIdFood.Name = "lblIdFood";
            this.lblIdFood.Size = new System.Drawing.Size(62, 21);
            this.lblIdFood.TabIndex = 72;
            this.lblIdFood.Text = "Id Food";
            // 
            // txtIdFood
            // 
            this.txtIdFood.Location = new System.Drawing.Point(6, 47);
            this.txtIdFood.Name = "txtIdFood";
            this.txtIdFood.Size = new System.Drawing.Size(50, 29);
            this.txtIdFood.TabIndex = 71;
            this.txtIdFood.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(364, 168);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(100, 29);
            this.textBox11.TabIndex = 26;
            this.textBox11.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(231, 145);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(129, 21);
            this.label15.TabIndex = 25;
            this.label15.Text = "Cholesterol  [g]%";
            this.label15.Visible = false;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(245, 168);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(100, 29);
            this.textBox12.TabIndex = 24;
            this.textBox12.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(120, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 21);
            this.label12.TabIndex = 23;
            this.label12.Text = "Total fats [g]%";
            // 
            // txtTotalFats
            // 
            this.txtTotalFats.Location = new System.Drawing.Point(125, 50);
            this.txtTotalFats.Name = "txtTotalFats";
            this.txtTotalFats.Size = new System.Drawing.Size(100, 29);
            this.txtTotalFats.TabIndex = 22;
            this.txtTotalFats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(480, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 21);
            this.label10.TabIndex = 21;
            this.label10.Text = "Poli fats  [g]%";
            this.label10.Visible = false;
            // 
            // txtPolinsaturatedFats
            // 
            this.txtPolinsaturatedFats.Location = new System.Drawing.Point(482, 50);
            this.txtPolinsaturatedFats.Name = "txtPolinsaturatedFats";
            this.txtPolinsaturatedFats.Size = new System.Drawing.Size(100, 29);
            this.txtPolinsaturatedFats.TabIndex = 20;
            this.txtPolinsaturatedFats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPolinsaturatedFats.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(132, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 21);
            this.label8.TabIndex = 17;
            this.label8.Text = "Sugar [g]%";
            // 
            // txtSugar
            // 
            this.txtSugar.Location = new System.Drawing.Point(125, 107);
            this.txtSugar.Name = "txtSugar";
            this.txtSugar.Size = new System.Drawing.Size(100, 29);
            this.txtSugar.TabIndex = 16;
            this.txtSugar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnChoose
            // 
            this.btnChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnChoose.Location = new System.Drawing.Point(457, 28);
            this.btnChoose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(76, 58);
            this.btnChoose.TabIndex = 83;
            this.btnChoose.Text = "Choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "CHO [g]%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 15;
            this.label2.Text = "Saturated fats";
            // 
            // grpFood
            // 
            this.grpFood.Controls.Add(this.label18);
            this.grpFood.Controls.Add(this.txtDescription);
            this.grpFood.Controls.Add(this.label19);
            this.grpFood.Controls.Add(this.txtName);
            this.grpFood.Controls.Add(this.groupBox1);
            this.grpFood.Controls.Add(this.btnNewFood);
            this.grpFood.Controls.Add(this.btnFatSecret);
            this.grpFood.Controls.Add(this.btnSave);
            this.grpFood.Controls.Add(this.btnChoose);
            this.grpFood.Controls.Add(this.btnSaveFood);
            this.grpFood.Controls.Add(this.btnSearchFood);
            this.grpFood.Controls.Add(this.lblIdFood);
            this.grpFood.Controls.Add(this.txtIdFood);
            this.grpFood.Controls.Add(this.txtFoodCarbohydrates);
            this.grpFood.Controls.Add(this.label3);
            this.grpFood.Location = new System.Drawing.Point(0, 1);
            this.grpFood.Name = "grpFood";
            this.grpFood.Size = new System.Drawing.Size(627, 303);
            this.grpFood.TabIndex = 79;
            this.grpFood.TabStop = false;
            this.grpFood.Text = "Calculator";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(167, 96);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 21);
            this.label18.TabIndex = 95;
            this.label18.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(167, 119);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(450, 29);
            this.txtDescription.TabIndex = 94;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 96);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(52, 21);
            this.label19.TabIndex = 93;
            this.label19.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.LightGreen;
            this.txtName.Location = new System.Drawing.Point(6, 119);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(155, 29);
            this.txtName.TabIndex = 92;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMonoinsaturatedFats);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.textBox11);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.textBox12);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtTotalFats);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtPolinsaturatedFats);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtSugar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSaturatedFats);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtProteins);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSalt);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtFibers);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtCalories);
            this.groupBox1.Location = new System.Drawing.Point(0, 154);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(621, 172);
            this.groupBox1.TabIndex = 91;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Food nutrition data";
            // 
            // txtSaturatedFats
            // 
            this.txtSaturatedFats.Location = new System.Drawing.Point(244, 50);
            this.txtSaturatedFats.Name = "txtSaturatedFats";
            this.txtSaturatedFats.Size = new System.Drawing.Size(100, 29);
            this.txtSaturatedFats.TabIndex = 14;
            this.txtSaturatedFats.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 21);
            this.label6.TabIndex = 13;
            this.label6.Text = "Proteins [g]%";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(518, 168);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(64, 29);
            this.textBox1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "Id Food";
            // 
            // txtProteins
            // 
            this.txtProteins.Location = new System.Drawing.Point(6, 168);
            this.txtProteins.Name = "txtProteins";
            this.txtProteins.Size = new System.Drawing.Size(100, 29);
            this.txtProteins.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(139, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 21);
            this.label7.TabIndex = 11;
            this.label7.Text = "Salt [g]%";
            // 
            // txtSalt
            // 
            this.txtSalt.Location = new System.Drawing.Point(125, 168);
            this.txtSalt.Name = "txtSalt";
            this.txtSalt.Size = new System.Drawing.Size(100, 29);
            this.txtSalt.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(251, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 21);
            this.label5.TabIndex = 7;
            this.label5.Text = "Fibers [g]%";
            // 
            // txtFibers
            // 
            this.txtFibers.Location = new System.Drawing.Point(245, 107);
            this.txtFibers.Name = "txtFibers";
            this.txtFibers.Size = new System.Drawing.Size(100, 29);
            this.txtFibers.TabIndex = 6;
            this.txtFibers.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 21);
            this.label11.TabIndex = 3;
            this.label11.Text = "Calories [kCal]";
            // 
            // txtCalories
            // 
            this.txtCalories.Location = new System.Drawing.Point(6, 50);
            this.txtCalories.Name = "txtCalories";
            this.txtCalories.Size = new System.Drawing.Size(100, 29);
            this.txtCalories.TabIndex = 2;
            this.txtCalories.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFoodCarbohydrates
            // 
            this.txtFoodCarbohydrates.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodCarbohydrates.Location = new System.Drawing.Point(86, 47);
            this.txtFoodCarbohydrates.Name = "txtFoodCarbohydrates";
            this.txtFoodCarbohydrates.Size = new System.Drawing.Size(71, 29);
            this.txtFoodCarbohydrates.TabIndex = 0;
            this.txtFoodCarbohydrates.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gridFoods
            // 
            this.gridFoods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFoods.Location = new System.Drawing.Point(6, 305);
            this.gridFoods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridFoods.Name = "gridFoods";
            this.gridFoods.Size = new System.Drawing.Size(621, 237);
            this.gridFoods.TabIndex = 78;
            this.gridFoods.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellClick);
            this.gridFoods.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellContentClick);
            this.gridFoods.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellDoubleClick);
            // 
            // frmFoods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 547);
            this.Controls.Add(this.grpFood);
            this.Controls.Add(this.gridFoods);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFoods";
            this.Text = "Foods";
            this.Load += new System.EventHandler(this.frmFoods_Load);
            this.grpFood.ResumeLayout(false);
            this.grpFood.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnNewFood;
        private Button btnFatSecret;
        private Label label16;
        private Label label1;
        private TextBox txtMonoinsaturatedFats;
        private Button btnSave;
        private Label label14;
        private Button btnSaveFood;
        private Button btnSearchFood;
        private Label lblIdFood;
        private TextBox txtIdFood;
        private TextBox textBox11;
        private Label label15;
        private TextBox textBox12;
        private Label label12;
        private TextBox txtTotalFats;
        private Label label10;
        private TextBox txtPolinsaturatedFats;
        private Label label8;
        private TextBox txtSugar;
        private Button btnChoose;
        private Label label3;
        private Label label2;
        private GroupBox grpFood;
        private Label label18;
        private TextBox txtDescription;
        private Label label19;
        private TextBox txtName;
        private GroupBox groupBox1;
        private TextBox txtSaturatedFats;
        private Label label6;
        private TextBox textBox1;
        private Label label4;
        private TextBox txtProteins;
        private Label label7;
        private TextBox txtSalt;
        private Label label5;
        private TextBox txtFibers;
        private Label label11;
        private TextBox txtCalories;
        private TextBox txtFoodCarbohydrates;
        private DataGridView gridFoods;
    }
}