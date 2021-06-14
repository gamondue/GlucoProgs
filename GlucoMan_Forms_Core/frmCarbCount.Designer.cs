
namespace GlucoMan_Forms_Core
{
    partial class frmCarbCount
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
            this.grdFoods = new System.Windows.Forms.DataGridView();
            this.btnAddFood = new System.Windows.Forms.Button();
            this.btnRemoveFood = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdFoods)).BeginInit();
            this.SuspendLayout();
            // 
            // grdFoods
            // 
            this.grdFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFoods.Location = new System.Drawing.Point(18, 59);
            this.grdFoods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grdFoods.Name = "grdFoods";
            this.grdFoods.Size = new System.Drawing.Size(360, 243);
            this.grdFoods.TabIndex = 0;
            // 
            // btnAddFood
            // 
            this.btnAddFood.Location = new System.Drawing.Point(309, 312);
            this.btnAddFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddFood.Name = "btnAddFood";
            this.btnAddFood.Size = new System.Drawing.Size(69, 35);
            this.btnAddFood.TabIndex = 1;
            this.btnAddFood.Text = "+ Food";
            this.btnAddFood.UseVisualStyleBackColor = true;
            // 
            // btnRemoveFood
            // 
            this.btnRemoveFood.Location = new System.Drawing.Point(18, 312);
            this.btnRemoveFood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoveFood.Name = "btnRemoveFood";
            this.btnRemoveFood.Size = new System.Drawing.Size(69, 35);
            this.btnRemoveFood.TabIndex = 2;
            this.btnRemoveFood.Text = "- Food";
            this.btnRemoveFood.UseVisualStyleBackColor = true;
            // 
            // frmCarbCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 349);
            this.Controls.Add(this.btnRemoveFood);
            this.Controls.Add(this.btnAddFood);
            this.Controls.Add(this.grdFoods);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmCarbCount";
            this.Text = "Sum the carbs to ingest";
            ((System.ComponentModel.ISupportInitialize)(this.grdFoods)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdFoods;
        private System.Windows.Forms.Button btnAddFood;
        private System.Windows.Forms.Button btnRemoveFood;
    }
}