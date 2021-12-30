
namespace GlucoMan.Forms
{
    partial class frmGlucose
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlucose));
            this.txtGlucose = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddMeasurement = new System.Windows.Forms.Button();
            this.btnRemoveMeasurement = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEventInstant = new System.Windows.Forms.DateTimePicker();
            this.chkNowInAdd = new System.Windows.Forms.CheckBox();
            this.chkAutosave = new System.Windows.Forms.CheckBox();
            this.gridMeasurements = new System.Windows.Forms.DataGridView();
            this.glucoseRecordBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnNow = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeasurements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glucoseRecordBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtGlucose
            // 
            this.txtGlucose.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGlucose.Location = new System.Drawing.Point(12, 36);
            this.txtGlucose.Name = "txtGlucose";
            this.txtGlucose.Size = new System.Drawing.Size(100, 45);
            this.txtGlucose.TabIndex = 1;
            this.txtGlucose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Glucose";
            // 
            // btnAddMeasurement
            // 
            this.btnAddMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddMeasurement.Location = new System.Drawing.Point(835, 90);
            this.btnAddMeasurement.Name = "btnAddMeasurement";
            this.btnAddMeasurement.Size = new System.Drawing.Size(46, 45);
            this.btnAddMeasurement.TabIndex = 3;
            this.btnAddMeasurement.Text = "+";
            this.btnAddMeasurement.UseVisualStyleBackColor = true;
            this.btnAddMeasurement.Click += new System.EventHandler(this.btnAddMeasurement_Click);
            // 
            // btnRemoveMeasurement
            // 
            this.btnRemoveMeasurement.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveMeasurement.Location = new System.Drawing.Point(835, 141);
            this.btnRemoveMeasurement.Name = "btnRemoveMeasurement";
            this.btnRemoveMeasurement.Size = new System.Drawing.Size(46, 45);
            this.btnRemoveMeasurement.TabIndex = 4;
            this.btnRemoveMeasurement.Text = "-";
            this.btnRemoveMeasurement.UseVisualStyleBackColor = true;
            this.btnRemoveMeasurement.Click += new System.EventHandler(this.btnRemoveMeasurement_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(127, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Time";
            // 
            // dtpEventInstant
            // 
            this.dtpEventInstant.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtpEventInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpEventInstant.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEventInstant.Location = new System.Drawing.Point(131, 36);
            this.dtpEventInstant.Name = "dtpEventInstant";
            this.dtpEventInstant.Size = new System.Drawing.Size(338, 45);
            this.dtpEventInstant.TabIndex = 7;
            // 
            // chkNowInAdd
            // 
            this.chkNowInAdd.AutoSize = true;
            this.chkNowInAdd.Checked = true;
            this.chkNowInAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNowInAdd.Location = new System.Drawing.Point(485, 34);
            this.chkNowInAdd.Name = "chkNowInAdd";
            this.chkNowInAdd.Size = new System.Drawing.Size(179, 24);
            this.chkNowInAdd.TabIndex = 8;
            this.chkNowInAdd.Text = "Add with Now as time";
            this.chkNowInAdd.UseVisualStyleBackColor = true;
            // 
            // chkAutosave
            // 
            this.chkAutosave.AutoSize = true;
            this.chkAutosave.Checked = true;
            this.chkAutosave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutosave.Location = new System.Drawing.Point(485, 62);
            this.chkAutosave.Name = "chkAutosave";
            this.chkAutosave.Size = new System.Drawing.Size(95, 24);
            this.chkAutosave.TabIndex = 9;
            this.chkAutosave.Text = "Autosave";
            this.chkAutosave.UseVisualStyleBackColor = true;
            // 
            // gridMeasurements
            // 
            this.gridMeasurements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMeasurements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMeasurements.Location = new System.Drawing.Point(12, 87);
            this.gridMeasurements.Name = "gridMeasurements";
            this.gridMeasurements.Size = new System.Drawing.Size(817, 422);
            this.gridMeasurements.TabIndex = 10;
            this.gridMeasurements.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeasurements_RowEnter);
            // 
            // btnNow
            // 
            this.btnNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNow.Location = new System.Drawing.Point(670, 37);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(79, 45);
            this.btnNow.TabIndex = 11;
            this.btnNow.Text = "Now";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSave.Location = new System.Drawing.Point(750, 37);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(79, 45);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmGlucose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 521);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNow);
            this.Controls.Add(this.gridMeasurements);
            this.Controls.Add(this.chkAutosave);
            this.Controls.Add(this.chkNowInAdd);
            this.Controls.Add(this.dtpEventInstant);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRemoveMeasurement);
            this.Controls.Add(this.btnAddMeasurement);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGlucose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmGlucose";
            this.Text = "Glucose measurements";
            this.Load += new System.EventHandler(this.frmGlucose_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMeasurements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glucoseRecordBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtGlucose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddMeasurement;
        private System.Windows.Forms.Button btnRemoveMeasurement;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEventInstant;
        private System.Windows.Forms.CheckBox chkNowInAdd;
        private System.Windows.Forms.CheckBox chkAutosave;
        private System.Windows.Forms.BindingSource glucoseRecordBindingSource;
        private System.Windows.Forms.DataGridView gridMeasurements;
        private Button btnNow;
        private Button btnSave;
    }
}