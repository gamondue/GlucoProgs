
namespace GlucoMan_Forms_Core
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
            this.viewFoods = new System.Windows.Forms.DataGridView();
            this.btnAddFood = new System.Windows.Forms.Button();
            this.btnRemoveFood = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbIsSnack = new System.Windows.Forms.RadioButton();
            this.rdbIsBreakfast = new System.Windows.Forms.RadioButton();
            this.rdbIsDinner = new System.Windows.Forms.RadioButton();
            this.rdbIsLunch = new System.Windows.Forms.RadioButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.btnStartMeal = new System.Windows.Forms.Button();
            this.btnEndMeal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.viewFoods)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewFoods
            // 
            this.viewFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewFoods.Location = new System.Drawing.Point(13, 205);
            this.viewFoods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewFoods.Name = "viewFoods";
            this.viewFoods.Size = new System.Drawing.Size(379, 239);
            this.viewFoods.TabIndex = 0;
            this.viewFoods.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewFoods_CellClick);
            this.viewFoods.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewFoods_CellContentClick);
            // 
            // btnAddFood
            // 
            this.btnAddFood.Location = new System.Drawing.Point(322, 154);
            this.btnAddFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddFood.Name = "btnAddFood";
            this.btnAddFood.Size = new System.Drawing.Size(69, 50);
            this.btnAddFood.TabIndex = 1;
            this.btnAddFood.Text = "+ Food";
            this.btnAddFood.UseVisualStyleBackColor = true;
            this.btnAddFood.Click += new System.EventHandler(this.btnAddFood_Click);
            // 
            // btnRemoveFood
            // 
            this.btnRemoveFood.Location = new System.Drawing.Point(245, 154);
            this.btnRemoveFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoveFood.Name = "btnRemoveFood";
            this.btnRemoveFood.Size = new System.Drawing.Size(69, 50);
            this.btnRemoveFood.TabIndex = 2;
            this.btnRemoveFood.Text = "- Food";
            this.btnRemoveFood.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbIsSnack);
            this.groupBox1.Controls.Add(this.rdbIsBreakfast);
            this.groupBox1.Controls.Add(this.rdbIsDinner);
            this.groupBox1.Controls.Add(this.rdbIsLunch);
            this.groupBox1.Location = new System.Drawing.Point(39, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 56);
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
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy:mm:dd HH:MM.ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(17, 99);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(187, 26);
            this.dateTimePicker1.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 41;
            this.label1.Text = "Foods";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 42;
            this.label2.Text = "Meal start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 44;
            this.label3.Text = "Meal finish";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy:mm:dd HH:MM.ss";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(210, 99);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(187, 26);
            this.dateTimePicker2.TabIndex = 43;
            // 
            // btnStartMeal
            // 
            this.btnStartMeal.Location = new System.Drawing.Point(125, 67);
            this.btnStartMeal.Name = "btnStartMeal";
            this.btnStartMeal.Size = new System.Drawing.Size(75, 29);
            this.btnStartMeal.TabIndex = 45;
            this.btnStartMeal.Text = "Start";
            this.btnStartMeal.UseVisualStyleBackColor = true;
            // 
            // btnEndMeal
            // 
            this.btnEndMeal.Location = new System.Drawing.Point(322, 67);
            this.btnEndMeal.Name = "btnEndMeal";
            this.btnEndMeal.Size = new System.Drawing.Size(75, 29);
            this.btnEndMeal.TabIndex = 46;
            this.btnEndMeal.Text = "Finish";
            this.btnEndMeal.UseVisualStyleBackColor = true;
            // 
            // frmMeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 458);
            this.Controls.Add(this.btnEndMeal);
            this.Controls.Add(this.btnStartMeal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRemoveFood);
            this.Controls.Add(this.btnAddFood);
            this.Controls.Add(this.viewFoods);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMeal";
            this.Text = "Sum the carbs to ingest";
            this.Load += new System.EventHandler(this.frmMeal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewFoods)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView viewFoods;
        private System.Windows.Forms.Button btnAddFood;
        private System.Windows.Forms.Button btnRemoveFood;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbIsSnack;
        private System.Windows.Forms.RadioButton rdbIsBreakfast;
        private System.Windows.Forms.RadioButton rdbIsDinner;
        private System.Windows.Forms.RadioButton rdbIsLunch;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button btnStartMeal;
        private System.Windows.Forms.Button btnEndMeal;
    }
}