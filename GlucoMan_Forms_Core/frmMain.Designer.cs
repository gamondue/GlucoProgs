
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
            this.SuspendLayout();
            // 
            // btnHypoPrediction
            // 
            this.btnHypoPrediction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHypoPrediction.Location = new System.Drawing.Point(13, 22);
            this.btnHypoPrediction.Name = "btnHypoPrediction";
            this.btnHypoPrediction.Size = new System.Drawing.Size(89, 54);
            this.btnHypoPrediction.TabIndex = 0;
            this.btnHypoPrediction.Text = "Hypo time prediction";
            this.btnHypoPrediction.UseVisualStyleBackColor = true;
            this.btnHypoPrediction.Click += new System.EventHandler(this.btnHypoPrediction_Click);
            // 
            // btnWeighFood
            // 
            this.btnWeighFood.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWeighFood.Location = new System.Drawing.Point(108, 22);
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
            this.btnInsuline.Location = new System.Drawing.Point(203, 22);
            this.btnInsuline.Name = "btnInsuline";
            this.btnInsuline.Size = new System.Drawing.Size(89, 54);
            this.btnInsuline.TabIndex = 2;
            this.btnInsuline.Text = "Insuline calc";
            this.btnInsuline.UseVisualStyleBackColor = true;
            this.btnInsuline.Click += new System.EventHandler(this.btnInsulineCalc);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 97);
            this.Controls.Add(this.btnInsuline);
            this.Controls.Add(this.btnWeighFood);
            this.Controls.Add(this.btnHypoPrediction);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "GlucoRecord";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnHypoPrediction;
        private System.Windows.Forms.Button btnWeighFood;
        private System.Windows.Forms.Button btnInsuline;
    }
}