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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlucose));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGlucose = new System.Windows.Forms.TextBox();
            this.dtpEventInstant = new System.Windows.Forms.DateTimePicker();
            this.chkNowInAdd = new System.Windows.Forms.CheckBox();
            this.chkAutosave = new System.Windows.Forms.CheckBox();
            this.txtIdGlucoseRecord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNow = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddMeasurement = new System.Windows.Forms.Button();
            this.btnRemoveMeasurement = new System.Windows.Forms.Button();
            this.gridMeasurements = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridMeasurements)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Glucose";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Time of measurement";
            // 
            // txtGlucose
            // 
            this.txtGlucose.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtGlucose.Location = new System.Drawing.Point(12, 36);
            this.txtGlucose.Name = "txtGlucose";
            this.txtGlucose.Size = new System.Drawing.Size(71, 46);
            this.txtGlucose.TabIndex = 2;
            this.txtGlucose.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dtpEventInstant
            // 
            this.dtpEventInstant.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtpEventInstant.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpEventInstant.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEventInstant.Location = new System.Drawing.Point(89, 34);
            this.dtpEventInstant.Name = "dtpEventInstant";
            this.dtpEventInstant.Size = new System.Drawing.Size(337, 50);
            this.dtpEventInstant.TabIndex = 3;
            // 
            // chkNowInAdd
            // 
            this.chkNowInAdd.AutoSize = true;
            this.chkNowInAdd.Checked = true;
            this.chkNowInAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNowInAdd.Location = new System.Drawing.Point(445, 32);
            this.chkNowInAdd.Name = "chkNowInAdd";
            this.chkNowInAdd.Size = new System.Drawing.Size(180, 25);
            this.chkNowInAdd.TabIndex = 4;
            this.chkNowInAdd.Text = "New has Now as time";
            this.chkNowInAdd.UseVisualStyleBackColor = true;
            // 
            // chkAutosave
            // 
            this.chkAutosave.AutoSize = true;
            this.chkAutosave.Checked = true;
            this.chkAutosave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutosave.Location = new System.Drawing.Point(445, 63);
            this.chkAutosave.Name = "chkAutosave";
            this.chkAutosave.Size = new System.Drawing.Size(93, 25);
            this.chkAutosave.TabIndex = 5;
            this.chkAutosave.Text = "Autosave";
            this.chkAutosave.UseVisualStyleBackColor = true;
            // 
            // txtIdGlucoseRecord
            // 
            this.txtIdGlucoseRecord.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtIdGlucoseRecord.Location = new System.Drawing.Point(627, 42);
            this.txtIdGlucoseRecord.Name = "txtIdGlucoseRecord";
            this.txtIdGlucoseRecord.ReadOnly = true;
            this.txtIdGlucoseRecord.Size = new System.Drawing.Size(71, 35);
            this.txtIdGlucoseRecord.TabIndex = 6;
            this.txtIdGlucoseRecord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(627, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Id";
            // 
            // btnNow
            // 
            this.btnNow.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNow.Location = new System.Drawing.Point(708, 31);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(100, 56);
            this.btnNow.TabIndex = 8;
            this.btnNow.Text = "Now";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSave.Location = new System.Drawing.Point(814, 31);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 56);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddMeasurement
            // 
            this.btnAddMeasurement.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddMeasurement.Location = new System.Drawing.Point(858, 94);
            this.btnAddMeasurement.Name = "btnAddMeasurement";
            this.btnAddMeasurement.Size = new System.Drawing.Size(56, 56);
            this.btnAddMeasurement.TabIndex = 10;
            this.btnAddMeasurement.Text = "+";
            this.btnAddMeasurement.UseVisualStyleBackColor = true;
            this.btnAddMeasurement.Click += new System.EventHandler(this.btnAddMeasurement_Click);
            // 
            // btnRemoveMeasurement
            // 
            this.btnRemoveMeasurement.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveMeasurement.Location = new System.Drawing.Point(858, 156);
            this.btnRemoveMeasurement.Name = "btnRemoveMeasurement";
            this.btnRemoveMeasurement.Size = new System.Drawing.Size(56, 56);
            this.btnRemoveMeasurement.TabIndex = 11;
            this.btnRemoveMeasurement.Text = "-";
            this.btnRemoveMeasurement.UseVisualStyleBackColor = true;
            this.btnRemoveMeasurement.Click += new System.EventHandler(this.btnRemoveMeasurement_Click);
            // 
            // gridMeasurements
            // 
            this.gridMeasurements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMeasurements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMeasurements.Location = new System.Drawing.Point(12, 93);
            this.gridMeasurements.Name = "gridMeasurements";
            this.gridMeasurements.Size = new System.Drawing.Size(840, 525);
            this.gridMeasurements.TabIndex = 12;
            this.gridMeasurements.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMeasurements_CellContentClick);
            this.gridMeasurements.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMeasurements_RowEnter);
            // 
            // frmGlucose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 630);
            this.Controls.Add(this.gridMeasurements);
            this.Controls.Add(this.btnRemoveMeasurement);
            this.Controls.Add(this.btnAddMeasurement);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNow);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIdGlucoseRecord);
            this.Controls.Add(this.chkAutosave);
            this.Controls.Add(this.chkNowInAdd);
            this.Controls.Add(this.dtpEventInstant);
            this.Controls.Add(this.txtGlucose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmGlucose";
            this.Text = "Glucose measurements";
            this.Load += new System.EventHandler(this.frmGlucose_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMeasurements)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtGlucose;
        private DateTimePicker dtpEventInstant;
        private CheckBox chkNowInAdd;
        private CheckBox chkAutosave;
        private TextBox txtIdGlucoseRecord;
        private Label label3;
        private Button btnNow;
        private Button btnSave;
        private Button btnAddMeasurement;
        private Button btnRemoveMeasurement;
        private DataGridView gridMeasurements;
    }
}