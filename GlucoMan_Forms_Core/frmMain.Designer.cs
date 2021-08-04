
namespace GlucoMan_Forms_Core
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
            this.btnChoCount = new System.Windows.Forms.Button();
            this.btnAlarms = new System.Windows.Forms.Button();
            this.btnCorrectionBolusCalculation = new System.Windows.Forms.Button();
            this.btnGlucoseMeasurement = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHypoPrediction
            // 
            this.btnHypoPrediction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnHypoPrediction.Location = new System.Drawing.Point(679, 171);
            this.btnHypoPrediction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnHypoPrediction.Name = "btnHypoPrediction";
            this.btnHypoPrediction.Size = new System.Drawing.Size(104, 62);
            this.btnHypoPrediction.TabIndex = 0;
            this.btnHypoPrediction.Text = "Hypo time prediction";
            this.btnHypoPrediction.UseVisualStyleBackColor = true;
            this.btnHypoPrediction.Click += new System.EventHandler(this.btnHypoPrediction_Click);
            // 
            // btnWeighFood
            // 
            this.btnWeighFood.Enabled = false;
            this.btnWeighFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWeighFood.Location = new System.Drawing.Point(236, 171);
            this.btnWeighFood.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnWeighFood.Name = "btnWeighFood";
            this.btnWeighFood.Size = new System.Drawing.Size(104, 62);
            this.btnWeighFood.TabIndex = 1;
            this.btnWeighFood.Text = "Weigh Food";
            this.btnWeighFood.UseVisualStyleBackColor = true;
            this.btnWeighFood.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // btnInsulin
            // 
            this.btnInsulin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnInsulin.Location = new System.Drawing.Point(346, 171);
            this.btnInsulin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnInsulin.Name = "btnInsulin";
            this.btnInsulin.Size = new System.Drawing.Size(104, 62);
            this.btnInsulin.TabIndex = 2;
            this.btnInsulin.Text = "Meal Insulin";
            this.btnInsulin.UseVisualStyleBackColor = true;
            this.btnInsulin.Click += new System.EventHandler(this.btnInsulinCalc);
            // 
            // btnFoodToHitTargetCarbs
            // 
            this.btnFoodToHitTargetCarbs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFoodToHitTargetCarbs.Location = new System.Drawing.Point(568, 171);
            this.btnFoodToHitTargetCarbs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFoodToHitTargetCarbs.Name = "btnFoodToHitTargetCarbs";
            this.btnFoodToHitTargetCarbs.Size = new System.Drawing.Size(104, 62);
            this.btnFoodToHitTargetCarbs.TabIndex = 3;
            this.btnFoodToHitTargetCarbs.Text = "Food to hit target Carbs";
            this.btnFoodToHitTargetCarbs.UseVisualStyleBackColor = true;
            this.btnFoodToHitTargetCarbs.Click += new System.EventHandler(this.btnFoodToHitTargetCarbs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(9, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 4;
            // 
            // txtHeaderText
            // 
            this.txtHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtHeaderText.Location = new System.Drawing.Point(14, 14);
            this.txtHeaderText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtHeaderText.Multiline = true;
            this.txtHeaderText.Name = "txtHeaderText";
            this.txtHeaderText.Size = new System.Drawing.Size(879, 149);
            this.txtHeaderText.TabIndex = 5;
            // 
            // txtFooterText
            // 
            this.txtFooterText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtFooterText.Location = new System.Drawing.Point(14, 240);
            this.txtFooterText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtFooterText.Multiline = true;
            this.txtFooterText.Name = "txtFooterText";
            this.txtFooterText.Size = new System.Drawing.Size(879, 82);
            this.txtFooterText.TabIndex = 6;
            // 
            // btnChoCount
            // 
            this.btnChoCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnChoCount.Location = new System.Drawing.Point(125, 171);
            this.btnChoCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnChoCount.Name = "btnChoCount";
            this.btnChoCount.Size = new System.Drawing.Size(104, 62);
            this.btnChoCount.TabIndex = 7;
            this.btnChoCount.Text = "Meal";
            this.btnChoCount.UseVisualStyleBackColor = true;
            this.btnChoCount.Click += new System.EventHandler(this.btnChoCount_Click);
            // 
            // btnAlarms
            // 
            this.btnAlarms.Enabled = false;
            this.btnAlarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAlarms.Location = new System.Drawing.Point(790, 171);
            this.btnAlarms.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAlarms.Name = "btnAlarms";
            this.btnAlarms.Size = new System.Drawing.Size(104, 62);
            this.btnAlarms.TabIndex = 8;
            this.btnAlarms.Text = "Alarms";
            this.btnAlarms.UseVisualStyleBackColor = true;
            this.btnAlarms.Click += new System.EventHandler(this.btnAlarms_Click);
            // 
            // btnCorrectionBolusCalculation
            // 
            this.btnCorrectionBolusCalculation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCorrectionBolusCalculation.Location = new System.Drawing.Point(457, 170);
            this.btnCorrectionBolusCalculation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCorrectionBolusCalculation.Name = "btnCorrectionBolusCalculation";
            this.btnCorrectionBolusCalculation.Size = new System.Drawing.Size(104, 62);
            this.btnCorrectionBolusCalculation.TabIndex = 1;
            this.btnCorrectionBolusCalculation.Text = "Correction insulin";
            this.btnCorrectionBolusCalculation.UseVisualStyleBackColor = true;
            this.btnCorrectionBolusCalculation.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // btnGlucoseMeasurement
            // 
            this.btnGlucoseMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnGlucoseMeasurement.Location = new System.Drawing.Point(14, 171);
            this.btnGlucoseMeasurement.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGlucoseMeasurement.Name = "btnGlucoseMeasurement";
            this.btnGlucoseMeasurement.Size = new System.Drawing.Size(104, 62);
            this.btnGlucoseMeasurement.TabIndex = 9;
            this.btnGlucoseMeasurement.Text = "Glucose";
            this.btnGlucoseMeasurement.UseVisualStyleBackColor = true;
            this.btnGlucoseMeasurement.Click += new System.EventHandler(this.btnGlucoseMeasurement_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 338);
            this.Controls.Add(this.btnCorrectionBolusCalculation);
            this.Controls.Add(this.btnGlucoseMeasurement);
            this.Controls.Add(this.btnAlarms);
            this.Controls.Add(this.btnChoCount);
            this.Controls.Add(this.txtFooterText);
            this.Controls.Add(this.txtHeaderText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFoodToHitTargetCarbs);
            this.Controls.Add(this.btnInsulin);
            this.Controls.Add(this.btnWeighFood);
            this.Controls.Add(this.btnHypoPrediction);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
        private System.Windows.Forms.Button btnChoCount;
        private System.Windows.Forms.Button btnAlarms;
        private System.Windows.Forms.Button btnCorrectionBolusCalculation;
        private System.Windows.Forms.Button btnGlucoseMeasurement;
    }
}