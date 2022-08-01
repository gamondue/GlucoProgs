
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
            this.btnHypoPrediction = new System.Windows.Forms.Button();
            this.btnWeighFood = new System.Windows.Forms.Button();
            this.btnInsulin = new System.Windows.Forms.Button();
            this.btnFoodToHitTargetCarbs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHeaderText = new System.Windows.Forms.TextBox();
            this.txtFooterText = new System.Windows.Forms.TextBox();
            this.btnMeals = new System.Windows.Forms.Button();
            this.btnAlarms = new System.Windows.Forms.Button();
            this.btnGlucoseMeasurement = new System.Windows.Forms.Button();
            this.bntMiscellaneous = new System.Windows.Forms.Button();
            this.btnInjectionSites = new System.Windows.Forms.Button();
            this.btnFoods = new System.Windows.Forms.Button();
            this.btnNewMeal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHypoPrediction
            // 
            this.btnHypoPrediction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnHypoPrediction.Location = new System.Drawing.Point(447, 284);
            this.btnHypoPrediction.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnHypoPrediction.Name = "btnHypoPrediction";
            this.btnHypoPrediction.Size = new System.Drawing.Size(134, 87);
            this.btnHypoPrediction.TabIndex = 0;
            this.btnHypoPrediction.Text = "Hypo time prediction";
            this.btnHypoPrediction.UseVisualStyleBackColor = true;
            this.btnHypoPrediction.Click += new System.EventHandler(this.btnHypoPrediction_Click);
            // 
            // btnWeighFood
            // 
            this.btnWeighFood.Enabled = false;
            this.btnWeighFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWeighFood.Location = new System.Drawing.Point(590, 189);
            this.btnWeighFood.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnWeighFood.Name = "btnWeighFood";
            this.btnWeighFood.Size = new System.Drawing.Size(134, 87);
            this.btnWeighFood.TabIndex = 1;
            this.btnWeighFood.Text = "Weigh Food";
            this.btnWeighFood.UseVisualStyleBackColor = true;
            this.btnWeighFood.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // btnInsulin
            // 
            this.btnInsulin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInsulin.Location = new System.Drawing.Point(18, 284);
            this.btnInsulin.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnInsulin.Name = "btnInsulin";
            this.btnInsulin.Size = new System.Drawing.Size(134, 87);
            this.btnInsulin.TabIndex = 2;
            this.btnInsulin.Text = "Meal Insulin";
            this.btnInsulin.UseVisualStyleBackColor = true;
            this.btnInsulin.Click += new System.EventHandler(this.btnInsulinCalc);
            // 
            // btnFoodToHitTargetCarbs
            // 
            this.btnFoodToHitTargetCarbs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFoodToHitTargetCarbs.Location = new System.Drawing.Point(304, 284);
            this.btnFoodToHitTargetCarbs.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFoodToHitTargetCarbs.Name = "btnFoodToHitTargetCarbs";
            this.btnFoodToHitTargetCarbs.Size = new System.Drawing.Size(134, 87);
            this.btnFoodToHitTargetCarbs.TabIndex = 3;
            this.btnFoodToHitTargetCarbs.Text = "Food to hit target Carbs";
            this.btnFoodToHitTargetCarbs.UseVisualStyleBackColor = true;
            this.btnFoodToHitTargetCarbs.Click += new System.EventHandler(this.btnFoodToHitTargetCarbs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 88);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 4;
            // 
            // txtHeaderText
            // 
            this.txtHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHeaderText.Location = new System.Drawing.Point(18, 20);
            this.txtHeaderText.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtHeaderText.Multiline = true;
            this.txtHeaderText.Name = "txtHeaderText";
            this.txtHeaderText.Size = new System.Drawing.Size(849, 161);
            this.txtHeaderText.TabIndex = 5;
            // 
            // txtFooterText
            // 
            this.txtFooterText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtFooterText.Location = new System.Drawing.Point(14, 382);
            this.txtFooterText.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtFooterText.Multiline = true;
            this.txtFooterText.Name = "txtFooterText";
            this.txtFooterText.Size = new System.Drawing.Size(853, 113);
            this.txtFooterText.TabIndex = 6;
            // 
            // btnMeals
            // 
            this.btnMeals.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMeals.Location = new System.Drawing.Point(161, 189);
            this.btnMeals.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnMeals.Name = "btnMeals";
            this.btnMeals.Size = new System.Drawing.Size(134, 87);
            this.btnMeals.TabIndex = 7;
            this.btnMeals.Text = "Meals";
            this.btnMeals.UseVisualStyleBackColor = true;
            this.btnMeals.Click += new System.EventHandler(this.btnMeals_Click);
            // 
            // btnAlarms
            // 
            this.btnAlarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAlarms.Location = new System.Drawing.Point(590, 284);
            this.btnAlarms.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAlarms.Name = "btnAlarms";
            this.btnAlarms.Size = new System.Drawing.Size(134, 87);
            this.btnAlarms.TabIndex = 8;
            this.btnAlarms.Text = "Alarms";
            this.btnAlarms.UseVisualStyleBackColor = true;
            this.btnAlarms.Click += new System.EventHandler(this.btnAlarms_Click);
            // 
            // btnGlucoseMeasurement
            // 
            this.btnGlucoseMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnGlucoseMeasurement.Location = new System.Drawing.Point(18, 189);
            this.btnGlucoseMeasurement.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnGlucoseMeasurement.Name = "btnGlucoseMeasurement";
            this.btnGlucoseMeasurement.Size = new System.Drawing.Size(134, 87);
            this.btnGlucoseMeasurement.TabIndex = 9;
            this.btnGlucoseMeasurement.Text = "Glucose";
            this.btnGlucoseMeasurement.UseVisualStyleBackColor = true;
            this.btnGlucoseMeasurement.Click += new System.EventHandler(this.btnGlucoseMeasurement_Click);
            // 
            // bntMiscellaneous
            // 
            this.bntMiscellaneous.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bntMiscellaneous.Location = new System.Drawing.Point(733, 284);
            this.bntMiscellaneous.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.bntMiscellaneous.Name = "bntMiscellaneous";
            this.bntMiscellaneous.Size = new System.Drawing.Size(134, 87);
            this.bntMiscellaneous.TabIndex = 10;
            this.bntMiscellaneous.Text = "Miscellan. functions";
            this.bntMiscellaneous.UseVisualStyleBackColor = true;
            this.bntMiscellaneous.Click += new System.EventHandler(this.bntMiscellaneous_Click);
            // 
            // btnInjectionSites
            // 
            this.btnInjectionSites.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInjectionSites.Location = new System.Drawing.Point(161, 284);
            this.btnInjectionSites.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnInjectionSites.Name = "btnInjectionSites";
            this.btnInjectionSites.Size = new System.Drawing.Size(134, 87);
            this.btnInjectionSites.TabIndex = 11;
            this.btnInjectionSites.Text = "Injections";
            this.btnInjectionSites.UseVisualStyleBackColor = true;
            this.btnInjectionSites.Click += new System.EventHandler(this.btnInjections_Click);
            // 
            // btnFoods
            // 
            this.btnFoods.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFoods.Location = new System.Drawing.Point(449, 189);
            this.btnFoods.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnFoods.Name = "btnFoods";
            this.btnFoods.Size = new System.Drawing.Size(134, 87);
            this.btnFoods.TabIndex = 12;
            this.btnFoods.Text = "Foods";
            this.btnFoods.UseVisualStyleBackColor = true;
            this.btnFoods.Click += new System.EventHandler(this.btnFoods_Click);
            // 
            // btnNewMeal
            // 
            this.btnNewMeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNewMeal.Location = new System.Drawing.Point(305, 189);
            this.btnNewMeal.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnNewMeal.Name = "btnNewMeal";
            this.btnNewMeal.Size = new System.Drawing.Size(134, 87);
            this.btnNewMeal.TabIndex = 13;
            this.btnNewMeal.Text = "New meal";
            this.btnNewMeal.UseVisualStyleBackColor = true;
            this.btnNewMeal.Click += new System.EventHandler(this.btnNewMeal_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 500);
            this.Controls.Add(this.btnNewMeal);
            this.Controls.Add(this.btnFoods);
            this.Controls.Add(this.btnInjectionSites);
            this.Controls.Add(this.bntMiscellaneous);
            this.Controls.Add(this.btnGlucoseMeasurement);
            this.Controls.Add(this.btnAlarms);
            this.Controls.Add(this.btnMeals);
            this.Controls.Add(this.txtFooterText);
            this.Controls.Add(this.txtHeaderText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFoodToHitTargetCarbs);
            this.Controls.Add(this.btnInsulin);
            this.Controls.Add(this.btnWeighFood);
            this.Controls.Add(this.btnHypoPrediction);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmMain";
            this.Text = "GlucoMan";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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