
namespace GlucoMan.Forms
{
    partial class frmFoodManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFoodManagement));
            this.gridFoods = new System.Windows.Forms.DataGridView();
            this.grpFood = new System.Windows.Forms.GroupBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnSaveFood = new System.Windows.Forms.Button();
            this.btnSearchFood = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIdMealInFood = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblIdFood = new System.Windows.Forms.Label();
            this.txtIdFood = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFoodChoGrams = new System.Windows.Forms.TextBox();
            this.txtSugarPercent = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFoodQuantityGrams = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSugarGrams = new System.Windows.Forms.TextBox();
            this.cmbAccuracyFoodInMeal = new System.Windows.Forms.ComboBox();
            this.txtFibersPercent = new System.Windows.Forms.TextBox();
            this.txtFoodChoPercent = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAccuracyOfChoFoodInMeal = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.picCalculator = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).BeginInit();
            this.grpFood.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCalculator)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFoods
            // 
            this.gridFoods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFoods.Location = new System.Drawing.Point(13, 218);
            this.gridFoods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridFoods.Name = "gridFoods";
            this.gridFoods.Size = new System.Drawing.Size(665, 310);
            this.gridFoods.TabIndex = 1;
            this.gridFoods.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFoods_CellContentClick);
            // 
            // grpFood
            // 
            this.grpFood.Controls.Add(this.picCalculator);
            this.grpFood.Controls.Add(this.textBox1);
            this.grpFood.Controls.Add(this.btnChoose);
            this.grpFood.Controls.Add(this.btnSaveFood);
            this.grpFood.Controls.Add(this.btnSearchFood);
            this.grpFood.Controls.Add(this.label15);
            this.grpFood.Controls.Add(this.label16);
            this.grpFood.Controls.Add(this.txtDescription);
            this.grpFood.Controls.Add(this.label14);
            this.grpFood.Controls.Add(this.txtIdMealInFood);
            this.grpFood.Controls.Add(this.txtName);
            this.grpFood.Controls.Add(this.lblIdFood);
            this.grpFood.Controls.Add(this.txtIdFood);
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
            this.grpFood.Location = new System.Drawing.Point(3, 2);
            this.grpFood.Name = "grpFood";
            this.grpFood.Size = new System.Drawing.Size(675, 190);
            this.grpFood.TabIndex = 77;
            this.grpFood.TabStop = false;
            this.grpFood.Text = "Calculator";
            // 
            // btnChoose
            // 
            this.btnChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnChoose.Location = new System.Drawing.Point(509, 89);
            this.btnChoose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(76, 41);
            this.btnChoose.TabIndex = 83;
            this.btnChoose.Text = "Choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            // 
            // btnSaveFood
            // 
            this.btnSaveFood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveFood.Location = new System.Drawing.Point(978, 89);
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
            this.btnSearchFood.Location = new System.Drawing.Point(593, 89);
            this.btnSearchFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearchFood.Name = "btnSearchFood";
            this.btnSearchFood.Size = new System.Drawing.Size(76, 41);
            this.btnSearchFood.TabIndex = 81;
            this.btnSearchFood.Text = "Search";
            this.btnSearchFood.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(169, 135);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 21);
            this.label15.TabIndex = 76;
            this.label15.Text = "Description";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 78);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(102, 21);
            this.label16.TabIndex = 78;
            this.label16.Text = "Id Meal-Food";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(169, 158);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(503, 29);
            this.txtDescription.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 135);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 21);
            this.label14.TabIndex = 74;
            this.label14.Text = "Name";
            // 
            // txtIdMealInFood
            // 
            this.txtIdMealInFood.Location = new System.Drawing.Point(32, 101);
            this.txtIdMealInFood.Name = "txtIdMealInFood";
            this.txtIdMealInFood.Size = new System.Drawing.Size(50, 29);
            this.txtIdMealInFood.TabIndex = 77;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.LightGreen;
            this.txtName.Location = new System.Drawing.Point(8, 158);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(155, 29);
            this.txtName.TabIndex = 4;
            // 
            // lblIdFood
            // 
            this.lblIdFood.AutoSize = true;
            this.lblIdFood.Location = new System.Drawing.Point(106, 78);
            this.lblIdFood.Name = "lblIdFood";
            this.lblIdFood.Size = new System.Drawing.Size(62, 21);
            this.lblIdFood.TabIndex = 72;
            this.lblIdFood.Text = "Id Food";
            // 
            // txtIdFood
            // 
            this.txtIdFood.Location = new System.Drawing.Point(113, 101);
            this.txtIdFood.Name = "txtIdFood";
            this.txtIdFood.Size = new System.Drawing.Size(50, 29);
            this.txtIdFood.TabIndex = 71;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(174, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 21);
            this.label12.TabIndex = 66;
            this.label12.Text = "CHO [g]";
            // 
            // txtFoodChoGrams
            // 
            this.txtFoodChoGrams.BackColor = System.Drawing.Color.LightGreen;
            this.txtFoodChoGrams.Location = new System.Drawing.Point(169, 45);
            this.txtFoodChoGrams.Name = "txtFoodChoGrams";
            this.txtFoodChoGrams.Size = new System.Drawing.Size(74, 29);
            this.txtFoodChoGrams.TabIndex = 3;
            this.txtFoodChoGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSugarPercent
            // 
            this.txtSugarPercent.Location = new System.Drawing.Point(265, 101);
            this.txtSugarPercent.Name = "txtSugarPercent";
            this.txtSugarPercent.Size = new System.Drawing.Size(74, 29);
            this.txtSugarPercent.TabIndex = 8;
            this.txtSugarPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(267, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 21);
            this.label11.TabIndex = 64;
            this.label11.Text = "Sugar %";
            // 
            // txtFoodQuantityGrams
            // 
            this.txtFoodQuantityGrams.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodQuantityGrams.Location = new System.Drawing.Point(89, 45);
            this.txtFoodQuantityGrams.Name = "txtFoodQuantityGrams";
            this.txtFoodQuantityGrams.Size = new System.Drawing.Size(74, 29);
            this.txtFoodQuantityGrams.TabIndex = 2;
            this.txtFoodQuantityGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(89, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 21);
            this.label10.TabIndex = 62;
            this.label10.Text = "Quant.[g]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(350, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 21);
            this.label7.TabIndex = 58;
            this.label7.Text = "Fibers %";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 21);
            this.label9.TabIndex = 56;
            this.label9.Text = "CHO %";
            // 
            // txtSugarGrams
            // 
            this.txtSugarGrams.Location = new System.Drawing.Point(265, 45);
            this.txtSugarGrams.Name = "txtSugarGrams";
            this.txtSugarGrams.ReadOnly = true;
            this.txtSugarGrams.Size = new System.Drawing.Size(74, 29);
            this.txtSugarGrams.TabIndex = 59;
            this.txtSugarGrams.TabStop = false;
            this.txtSugarGrams.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmbAccuracyFoodInMeal
            // 
            this.cmbAccuracyFoodInMeal.FormattingEnabled = true;
            this.cmbAccuracyFoodInMeal.Location = new System.Drawing.Point(428, 44);
            this.cmbAccuracyFoodInMeal.Name = "cmbAccuracyFoodInMeal";
            this.cmbAccuracyFoodInMeal.Size = new System.Drawing.Size(158, 29);
            this.cmbAccuracyFoodInMeal.TabIndex = 7;
            // 
            // txtFibersPercent
            // 
            this.txtFibersPercent.Location = new System.Drawing.Point(348, 101);
            this.txtFibersPercent.Name = "txtFibersPercent";
            this.txtFibersPercent.Size = new System.Drawing.Size(74, 29);
            this.txtFibersPercent.TabIndex = 9;
            this.txtFibersPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFoodChoPercent
            // 
            this.txtFoodChoPercent.BackColor = System.Drawing.Color.GreenYellow;
            this.txtFoodChoPercent.Location = new System.Drawing.Point(8, 45);
            this.txtFoodChoPercent.Name = "txtFoodChoPercent";
            this.txtFoodChoPercent.Size = new System.Drawing.Size(74, 29);
            this.txtFoodChoPercent.TabIndex = 1;
            this.txtFoodChoPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(348, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 21);
            this.label6.TabIndex = 52;
            this.label6.Text = "Accuracy";
            // 
            // txtAccuracyOfChoFoodInMeal
            // 
            this.txtAccuracyOfChoFoodInMeal.Location = new System.Drawing.Point(348, 45);
            this.txtAccuracyOfChoFoodInMeal.Name = "txtAccuracyOfChoFoodInMeal";
            this.txtAccuracyOfChoFoodInMeal.Size = new System.Drawing.Size(73, 29);
            this.txtAccuracyOfChoFoodInMeal.TabIndex = 6;
            this.txtAccuracyOfChoFoodInMeal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(266, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 21);
            this.label8.TabIndex = 60;
            this.label8.Text = "Sugar [g]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 21);
            this.label1.TabIndex = 75;
            this.label1.Text = "Foods";
            // 
            // picCalculator
            // 
            this.picCalculator.Image = global::GlucoMan.Forms.Properties.Resources.Calculator1;
            this.picCalculator.Location = new System.Drawing.Point(593, 41);
            this.picCalculator.Name = "picCalculator";
            this.picCalculator.Size = new System.Drawing.Size(35, 35);
            this.picCalculator.TabIndex = 87;
            this.picCalculator.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightGreen;
            this.textBox1.Location = new System.Drawing.Point(576, -5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(74, 29);
            this.textBox1.TabIndex = 86;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmFoodManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 543);
            this.Controls.Add(this.grpFood);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridFoods);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmFoodManagement";
            this.Text = "Food management";
            this.Load += new System.EventHandler(this.frmFoodManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridFoods)).EndInit();
            this.grpFood.ResumeLayout(false);
            this.grpFood.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCalculator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridFoods;
        private GroupBox grpFood;
        private Button btnSaveFood;
        private Button btnSearchFood;
        private Label label15;
        private Label label16;
        private TextBox txtDescription;
        private Label label14;
        private TextBox txtIdMealInFood;
        private TextBox txtName;
        private Label lblIdFood;
        private TextBox txtIdFood;
        private Label label12;
        private TextBox txtFoodChoGrams;
        private TextBox txtSugarPercent;
        private Label label11;
        private TextBox txtFoodQuantityGrams;
        private Label label10;
        private Label label7;
        private Label label9;
        private TextBox txtSugarGrams;
        private ComboBox cmbAccuracyFoodInMeal;
        private TextBox txtFibersPercent;
        private TextBox txtFoodChoPercent;
        private Label label6;
        private TextBox txtAccuracyOfChoFoodInMeal;
        private Label label8;
        private Label label1;
        private Button btnChoose;
        private PictureBox picCalculator;
        private TextBox textBox1;
    }
}