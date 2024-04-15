
namespace GlucoMan.Forms
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            btnHypoPrediction = new Button();
            btnWeighFood = new Button();
            btnInsulin = new Button();
            btnFoodToHitTargetCarbs = new Button();
            label1 = new Label();
            txtHeaderText = new TextBox();
            txtFooterText = new TextBox();
            btnMeals = new Button();
            btnAlarms = new Button();
            btnGlucoseMeasurement = new Button();
            bntMiscellaneous = new Button();
            btnInjectionSites = new Button();
            btnFoods = new Button();
            btnNewMeal = new Button();
            SuspendLayout();
            // 
            // btnHypoPrediction
            // 
            btnHypoPrediction.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnHypoPrediction.Location = new Point(447, 284);
            btnHypoPrediction.Margin = new Padding(5, 4, 5, 4);
            btnHypoPrediction.Name = "btnHypoPrediction";
            btnHypoPrediction.Size = new Size(134, 87);
            btnHypoPrediction.TabIndex = 0;
            btnHypoPrediction.Text = "Hypo time prediction";
            btnHypoPrediction.UseVisualStyleBackColor = true;
            btnHypoPrediction.Click += btnHypoPrediction_Click;
            // 
            // btnWeighFood
            // 
            btnWeighFood.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnWeighFood.Location = new Point(590, 189);
            btnWeighFood.Margin = new Padding(5, 4, 5, 4);
            btnWeighFood.Name = "btnWeighFood";
            btnWeighFood.Size = new Size(134, 87);
            btnWeighFood.TabIndex = 1;
            btnWeighFood.Text = "Weigh Food";
            btnWeighFood.UseVisualStyleBackColor = true;
            btnWeighFood.Click += btnWeighFood_Click;
            // 
            // btnInsulin
            // 
            btnInsulin.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnInsulin.Location = new Point(18, 284);
            btnInsulin.Margin = new Padding(5, 4, 5, 4);
            btnInsulin.Name = "btnInsulin";
            btnInsulin.Size = new Size(134, 87);
            btnInsulin.TabIndex = 2;
            btnInsulin.Text = "Meal Insulin";
            btnInsulin.UseVisualStyleBackColor = true;
            btnInsulin.Click += btnInsulinCalc;
            // 
            // btnFoodToHitTargetCarbs
            // 
            btnFoodToHitTargetCarbs.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            btnFoodToHitTargetCarbs.Location = new Point(304, 284);
            btnFoodToHitTargetCarbs.Margin = new Padding(5, 4, 5, 4);
            btnFoodToHitTargetCarbs.Name = "btnFoodToHitTargetCarbs";
            btnFoodToHitTargetCarbs.Size = new Size(134, 87);
            btnFoodToHitTargetCarbs.TabIndex = 3;
            btnFoodToHitTargetCarbs.Text = "Food to hit target Carbs";
            btnFoodToHitTargetCarbs.UseVisualStyleBackColor = true;
            btnFoodToHitTargetCarbs.Click += btnFoodToHitTargetCarbs_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 88);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(0, 24);
            label1.TabIndex = 4;
            // 
            // txtHeaderText
            // 
            txtHeaderText.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtHeaderText.Location = new Point(18, 20);
            txtHeaderText.Margin = new Padding(5, 4, 5, 4);
            txtHeaderText.Multiline = true;
            txtHeaderText.Name = "txtHeaderText";
            txtHeaderText.Size = new Size(849, 161);
            txtHeaderText.TabIndex = 5;
            // 
            // txtFooterText
            // 
            txtFooterText.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtFooterText.Location = new Point(14, 382);
            txtFooterText.Margin = new Padding(5, 4, 5, 4);
            txtFooterText.Multiline = true;
            txtFooterText.Name = "txtFooterText";
            txtFooterText.Size = new Size(853, 113);
            txtFooterText.TabIndex = 6;
            // 
            // btnMeals
            // 
            btnMeals.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnMeals.Location = new Point(161, 189);
            btnMeals.Margin = new Padding(5, 4, 5, 4);
            btnMeals.Name = "btnMeals";
            btnMeals.Size = new Size(134, 87);
            btnMeals.TabIndex = 7;
            btnMeals.Text = "Meals";
            btnMeals.UseVisualStyleBackColor = true;
            btnMeals.Click += btnMeals_Click;
            // 
            // btnAlarms
            // 
            btnAlarms.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnAlarms.Location = new Point(590, 284);
            btnAlarms.Margin = new Padding(5, 4, 5, 4);
            btnAlarms.Name = "btnAlarms";
            btnAlarms.Size = new Size(134, 87);
            btnAlarms.TabIndex = 8;
            btnAlarms.Text = "Alarms";
            btnAlarms.UseVisualStyleBackColor = true;
            btnAlarms.Click += btnAlarms_Click;
            // 
            // btnGlucoseMeasurement
            // 
            btnGlucoseMeasurement.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnGlucoseMeasurement.Location = new Point(18, 189);
            btnGlucoseMeasurement.Margin = new Padding(5, 4, 5, 4);
            btnGlucoseMeasurement.Name = "btnGlucoseMeasurement";
            btnGlucoseMeasurement.Size = new Size(134, 87);
            btnGlucoseMeasurement.TabIndex = 9;
            btnGlucoseMeasurement.Text = "Glucose";
            btnGlucoseMeasurement.UseVisualStyleBackColor = true;
            btnGlucoseMeasurement.Click += btnGlucoseMeasurement_Click;
            // 
            // bntMiscellaneous
            // 
            bntMiscellaneous.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            bntMiscellaneous.Location = new Point(733, 284);
            bntMiscellaneous.Margin = new Padding(5, 4, 5, 4);
            bntMiscellaneous.Name = "bntMiscellaneous";
            bntMiscellaneous.Size = new Size(134, 87);
            bntMiscellaneous.TabIndex = 10;
            bntMiscellaneous.Text = "Miscellan. functions";
            bntMiscellaneous.UseVisualStyleBackColor = true;
            bntMiscellaneous.Click += bntMiscellaneous_Click;
            // 
            // btnInjectionSites
            // 
            btnInjectionSites.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnInjectionSites.Location = new Point(161, 284);
            btnInjectionSites.Margin = new Padding(5, 4, 5, 4);
            btnInjectionSites.Name = "btnInjectionSites";
            btnInjectionSites.Size = new Size(134, 87);
            btnInjectionSites.TabIndex = 11;
            btnInjectionSites.Text = "Injections";
            btnInjectionSites.UseVisualStyleBackColor = true;
            btnInjectionSites.Click += btnInjections_Click;
            // 
            // btnFoods
            // 
            btnFoods.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnFoods.Location = new Point(449, 189);
            btnFoods.Margin = new Padding(5, 4, 5, 4);
            btnFoods.Name = "btnFoods";
            btnFoods.Size = new Size(134, 87);
            btnFoods.TabIndex = 12;
            btnFoods.Text = "Foods";
            btnFoods.UseVisualStyleBackColor = true;
            btnFoods.Click += btnFoods_Click;
            // 
            // btnNewMeal
            // 
            btnNewMeal.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnNewMeal.Location = new Point(305, 189);
            btnNewMeal.Margin = new Padding(5, 4, 5, 4);
            btnNewMeal.Name = "btnNewMeal";
            btnNewMeal.Size = new Size(134, 87);
            btnNewMeal.TabIndex = 13;
            btnNewMeal.Text = "New meal";
            btnNewMeal.UseVisualStyleBackColor = true;
            btnNewMeal.Click += btnNewMeal_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 500);
            Controls.Add(btnNewMeal);
            Controls.Add(btnFoods);
            Controls.Add(btnInjectionSites);
            Controls.Add(bntMiscellaneous);
            Controls.Add(btnGlucoseMeasurement);
            Controls.Add(btnAlarms);
            Controls.Add(btnMeals);
            Controls.Add(txtFooterText);
            Controls.Add(txtHeaderText);
            Controls.Add(label1);
            Controls.Add(btnFoodToHitTargetCarbs);
            Controls.Add(btnInsulin);
            Controls.Add(btnWeighFood);
            Controls.Add(btnHypoPrediction);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5, 4, 5, 4);
            Name = "frmMain";
            Text = "GlucoMan";
            Load += frmMain_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnHypoPrediction;
        private System.Windows.Forms.Button btnWeighFood;
        private System.Windows.Forms.Button btnInsulin;
        private System.Windows.Forms.Button btnFoodToHitTargetCarbs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHeaderText;
        private System.Windows.Forms.TextBox txtFooterText;
        private System.Windows.Forms.Button btnMeals;
        private System.Windows.Forms.Button btnAlarms;
        private System.Windows.Forms.Button btnGlucoseMeasurement;
        private Button bntMiscellaneous;
        private Button btnInjectionSites;
        private Button btnFoods;
        private Button btnNewMeal;
    }
}