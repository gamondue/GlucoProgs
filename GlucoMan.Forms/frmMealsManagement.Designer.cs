namespace GlucoMan.Forms
{
    partial class frmMealsManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMealsManagement));
            this.gridMeasurements = new System.Windows.Forms.DataGridView();
            this.btnRemoveFood = new System.Windows.Forms.Button();
            this.btnAddFood = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeasurements)).BeginInit();
            this.SuspendLayout();
            // 
            // gridMeasurements
            // 
            this.gridMeasurements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMeasurements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMeasurements.Location = new System.Drawing.Point(0, 60);
            this.gridMeasurements.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.gridMeasurements.Name = "gridMeasurements";
            this.gridMeasurements.Size = new System.Drawing.Size(344, 205);
            this.gridMeasurements.TabIndex = 11;
            // 
            // btnRemoveFood
            // 
            this.btnRemoveFood.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveFood.Location = new System.Drawing.Point(232, 11);
            this.btnRemoveFood.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRemoveFood.Name = "btnRemoveFood";
            this.btnRemoveFood.Size = new System.Drawing.Size(38, 46);
            this.btnRemoveFood.TabIndex = 13;
            this.btnRemoveFood.Text = "- Food";
            this.btnRemoveFood.UseVisualStyleBackColor = true;
            // 
            // btnAddFood
            // 
            this.btnAddFood.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddFood.Location = new System.Drawing.Point(274, 11);
            this.btnAddFood.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddFood.Name = "btnAddFood";
            this.btnAddFood.Size = new System.Drawing.Size(43, 46);
            this.btnAddFood.TabIndex = 12;
            this.btnAddFood.Text = "+ Food";
            this.btnAddFood.UseVisualStyleBackColor = true;
            // 
            // frmMealsManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 266);
            this.Controls.Add(this.btnRemoveFood);
            this.Controls.Add(this.btnAddFood);
            this.Controls.Add(this.gridMeasurements);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMealsManagement";
            this.Text = "Meals management";
            ((System.ComponentModel.ISupportInitialize)(this.gridMeasurements)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView gridMeasurements;
        private Button btnRemoveFood;
        private Button btnAddFood;
    }
}