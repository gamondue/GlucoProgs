
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
            this.btnInsuline = new System.Windows.Forms.Button();
            this.btnFoodToHitTargetCarbs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHeaderText = new System.Windows.Forms.TextBox();
            this.txtFooterText = new System.Windows.Forms.TextBox();
            this.btnChoCount = new System.Windows.Forms.Button();
            this.btnAlarms = new System.Windows.Forms.Button();
            this.btnBolus = new System.Windows.Forms.Button();
            this.btnGlucoseMeasurement = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHypoPrediction
            // 
            this.btnHypoPrediction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHypoPrediction.Location = new System.Drawing.Point(582, 148);
            this.btnHypoPrediction.Name = "btnHypoPrediction";
            this.btnHypoPrediction.Size = new System.Drawing.Size(89, 54);
            this.btnHypoPrediction.TabIndex = 0;
            this.btnHypoPrediction.Text = "Hypo time prediction";
            this.btnHypoPrediction.UseVisualStyleBackColor = true;
            this.btnHypoPrediction.Click += new System.EventHandler(this.btnHypoPrediction_Click);
            // 
            // btnWeighFood
            // 
            this.btnWeighFood.Enabled = false;
            this.btnWeighFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWeighFood.Location = new System.Drawing.Point(297, 148);
            this.btnWeighFood.Name = "btnWeighFood";
            this.btnWeighFood.Size = new System.Drawing.Size(89, 54);
            this.btnWeighFood.TabIndex = 1;
            this.btnWeighFood.Text = "Weigh Food";
            this.btnWeighFood.UseVisualStyleBackColor = true;
            this.btnWeighFood.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // btnInsuline
            // 
            this.btnInsuline.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInsuline.Location = new System.Drawing.Point(392, 148);
            this.btnInsuline.Name = "btnInsuline";
            this.btnInsuline.Size = new System.Drawing.Size(89, 54);
            this.btnInsuline.TabIndex = 2;
            this.btnInsuline.Text = "Insuline calc";
            this.btnInsuline.UseVisualStyleBackColor = true;
            this.btnInsuline.Click += new System.EventHandler(this.btnInsulineCalc);
            // 
            // btnFoodToHitTargetCarbs
            // 
            this.btnFoodToHitTargetCarbs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFoodToHitTargetCarbs.Location = new System.Drawing.Point(487, 148);
            this.btnFoodToHitTargetCarbs.Name = "btnFoodToHitTargetCarbs";
            this.btnFoodToHitTargetCarbs.Size = new System.Drawing.Size(89, 54);
            this.btnFoodToHitTargetCarbs.TabIndex = 3;
            this.btnFoodToHitTargetCarbs.Text = "Food to hit target Carbs";
            this.btnFoodToHitTargetCarbs.UseVisualStyleBackColor = true;
            this.btnFoodToHitTargetCarbs.Click += new System.EventHandler(this.btnFoodToHitTargetCarbs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 24);
            this.label1.TabIndex = 4;
            // 
            // txtHeaderText
            // 
            this.txtHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtHeaderText.Location = new System.Drawing.Point(12, 12);
            this.txtHeaderText.Multiline = true;
            this.txtHeaderText.Name = "txtHeaderText";
            this.txtHeaderText.Size = new System.Drawing.Size(754, 130);
            this.txtHeaderText.TabIndex = 5;
            // 
            // txtFooterText
            // 
            this.txtFooterText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtFooterText.Location = new System.Drawing.Point(12, 208);
            this.txtFooterText.Multiline = true;
            this.txtFooterText.Name = "txtFooterText";
            this.txtFooterText.Size = new System.Drawing.Size(754, 72);
            this.txtFooterText.TabIndex = 6;
            // 
            // btnChoCount
            // 
            this.btnChoCount.Enabled = false;
            this.btnChoCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChoCount.Location = new System.Drawing.Point(107, 148);
            this.btnChoCount.Name = "btnChoCount";
            this.btnChoCount.Size = new System.Drawing.Size(89, 54);
            this.btnChoCount.TabIndex = 7;
            this.btnChoCount.Text = "Meal";
            this.btnChoCount.UseVisualStyleBackColor = true;
            this.btnChoCount.Click += new System.EventHandler(this.btnChoCount_Click);
            // 
            // btnAlarms
            // 
            this.btnAlarms.Enabled = false;
            this.btnAlarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlarms.Location = new System.Drawing.Point(677, 148);
            this.btnAlarms.Name = "btnAlarms";
            this.btnAlarms.Size = new System.Drawing.Size(89, 54);
            this.btnAlarms.TabIndex = 8;
            this.btnAlarms.Text = "Alarms";
            this.btnAlarms.UseVisualStyleBackColor = true;
            this.btnAlarms.Click += new System.EventHandler(this.btnAlarms_Click);
            // 
            // btnBolus
            // 
            this.btnBolus.Enabled = false;
            this.btnBolus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBolus.Location = new System.Drawing.Point(202, 148);
            this.btnBolus.Name = "btnBolus";
            this.btnBolus.Size = new System.Drawing.Size(89, 54);
            this.btnBolus.TabIndex = 1;
            this.btnBolus.Text = "Bolus";
            this.btnBolus.UseVisualStyleBackColor = true;
            this.btnBolus.Click += new System.EventHandler(this.btnWeighFood_Click);
            // 
            // btnGlucoseMeasurement
            // 
            this.btnGlucoseMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGlucoseMeasurement.Location = new System.Drawing.Point(12, 148);
            this.btnGlucoseMeasurement.Name = "btnGlucoseMeasurement";
            this.btnGlucoseMeasurement.Size = new System.Drawing.Size(89, 54);
            this.btnGlucoseMeasurement.TabIndex = 9;
            this.btnGlucoseMeasurement.Text = "Glucose";
            this.btnGlucoseMeasurement.UseVisualStyleBackColor = true;
            this.btnGlucoseMeasurement.Click += new System.EventHandler(this.btnGlucoseMeasurement_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 293);
            this.Controls.Add(this.btnGlucoseMeasurement);
            this.Controls.Add(this.btnAlarms);
            this.Controls.Add(this.btnChoCount);
            this.Controls.Add(this.txtFooterText);
            this.Controls.Add(this.txtHeaderText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFoodToHitTargetCarbs);
            this.Controls.Add(this.btnInsuline);
            this.Controls.Add(this.btnBolus);
            this.Controls.Add(this.btnWeighFood);
            this.Controls.Add(this.btnHypoPrediction);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "GlucoMan";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHypoPrediction;
        private System.Windows.Forms.Button btnWeighFood;
        private System.Windows.Forms.Button btnInsuline;
        private System.Windows.Forms.Button btnFoodToHitTargetCarbs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHeaderText;
        private System.Windows.Forms.TextBox txtFooterText;
        private System.Windows.Forms.Button btnChoCount;
        private System.Windows.Forms.Button btnAlarms;
        private System.Windows.Forms.Button btnBolus;
        private System.Windows.Forms.Button btnGlucoseMeasurement;
    }
}