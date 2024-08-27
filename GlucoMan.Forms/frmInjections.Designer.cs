namespace GlucoMan.Forms
{
    partial class frmInjections
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInjections));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.rdbSlowInsulin = new System.Windows.Forms.RadioButton();
            this.rdbFastInsulin = new System.Windows.Forms.RadioButton();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnRemoveMeasurement = new System.Windows.Forms.Button();
            this.btnAddMeasurement = new System.Windows.Forms.Button();
            this.chkNowInAdd = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNow = new System.Windows.Forms.Button();
            this.txtIdInsulinInjection = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpInjectionDate = new System.Windows.Forms.DateTimePicker();
            this.dtpInjectionTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInsulinActual = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInsulinCalculated = new System.Windows.Forms.TextBox();
            this.gridInjections = new System.Windows.Forms.DataGridView();
            this.tabFront = new System.Windows.Forms.TabPage();
            this.tabBack = new System.Windows.Forms.TabPage();
            this.tabSensor = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInjections)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabData);
            this.tabControl1.Controls.Add(this.tabFront);
            this.tabControl1.Controls.Add(this.tabBack);
            this.tabControl1.Controls.Add(this.tabSensor);
            this.tabControl1.Location = new System.Drawing.Point(0, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(748, 641);
            this.tabControl1.TabIndex = 0;
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.rdbSlowInsulin);
            this.tabData.Controls.Add(this.rdbFastInsulin);
            this.tabData.Controls.Add(this.txtNotes);
            this.tabData.Controls.Add(this.btnRemoveMeasurement);
            this.tabData.Controls.Add(this.btnAddMeasurement);
            this.tabData.Controls.Add(this.chkNowInAdd);
            this.tabData.Controls.Add(this.btnSave);
            this.tabData.Controls.Add(this.btnNow);
            this.tabData.Controls.Add(this.txtIdInsulinInjection);
            this.tabData.Controls.Add(this.label5);
            this.tabData.Controls.Add(this.dtpInjectionDate);
            this.tabData.Controls.Add(this.dtpInjectionTime);
            this.tabData.Controls.Add(this.label4);
            this.tabData.Controls.Add(this.label3);
            this.tabData.Controls.Add(this.txtInsulinActual);
            this.tabData.Controls.Add(this.label1);
            this.tabData.Controls.Add(this.label2);
            this.tabData.Controls.Add(this.txtInsulinCalculated);
            this.tabData.Controls.Add(this.gridInjections);
            this.tabData.Location = new System.Drawing.Point(4, 30);
            this.tabData.Name = "tabData";
            this.tabData.Size = new System.Drawing.Size(740, 607);
            this.tabData.TabIndex = 3;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;
            // 
            // rdbSlowInsulin
            // 
            this.rdbSlowInsulin.AutoSize = true;
            this.rdbSlowInsulin.Location = new System.Drawing.Point(358, 111);
            this.rdbSlowInsulin.Name = "rdbSlowInsulin";
            this.rdbSlowInsulin.Size = new System.Drawing.Size(112, 25);
            this.rdbSlowInsulin.TabIndex = 129;
            this.rdbSlowInsulin.TabStop = true;
            this.rdbSlowInsulin.Text = "Slow insulin";
            this.rdbSlowInsulin.UseVisualStyleBackColor = true;
            // 
            // rdbFastInsulin
            // 
            this.rdbFastInsulin.AutoSize = true;
            this.rdbFastInsulin.Location = new System.Drawing.Point(216, 111);
            this.rdbFastInsulin.Name = "rdbFastInsulin";
            this.rdbFastInsulin.Size = new System.Drawing.Size(105, 25);
            this.rdbFastInsulin.TabIndex = 128;
            this.rdbFastInsulin.TabStop = true;
            this.rdbFastInsulin.Text = "Fast insulin";
            this.rdbFastInsulin.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(3, 142);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(669, 29);
            this.txtNotes.TabIndex = 127;
            // 
            // btnRemoveMeasurement
            // 
            this.btnRemoveMeasurement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveMeasurement.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemoveMeasurement.Location = new System.Drawing.Point(678, 204);
            this.btnRemoveMeasurement.Name = "btnRemoveMeasurement";
            this.btnRemoveMeasurement.Size = new System.Drawing.Size(56, 56);
            this.btnRemoveMeasurement.TabIndex = 126;
            this.btnRemoveMeasurement.Text = "-";
            this.btnRemoveMeasurement.UseVisualStyleBackColor = true;
            this.btnRemoveMeasurement.Click += new System.EventHandler(this.btnRemoveInjection_Click);
            // 
            // btnAddMeasurement
            // 
            this.btnAddMeasurement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMeasurement.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddMeasurement.Location = new System.Drawing.Point(678, 142);
            this.btnAddMeasurement.Name = "btnAddMeasurement";
            this.btnAddMeasurement.Size = new System.Drawing.Size(56, 56);
            this.btnAddMeasurement.TabIndex = 125;
            this.btnAddMeasurement.Text = "+";
            this.btnAddMeasurement.UseVisualStyleBackColor = true;
            this.btnAddMeasurement.Click += new System.EventHandler(this.btnAddInjection_Click);
            // 
            // chkNowInAdd
            // 
            this.chkNowInAdd.AutoSize = true;
            this.chkNowInAdd.Checked = true;
            this.chkNowInAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNowInAdd.Location = new System.Drawing.Point(557, 31);
            this.chkNowInAdd.Name = "chkNowInAdd";
            this.chkNowInAdd.Size = new System.Drawing.Size(187, 25);
            this.chkNowInAdd.TabIndex = 124;
            this.chkNowInAdd.Text = "Save with Now as time";
            this.chkNowInAdd.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSave.Location = new System.Drawing.Point(652, 61);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 46);
            this.btnSave.TabIndex = 123;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNow
            // 
            this.btnNow.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNow.Location = new System.Drawing.Point(566, 61);
            this.btnNow.Margin = new System.Windows.Forms.Padding(2);
            this.btnNow.Name = "btnNow";
            this.btnNow.Size = new System.Drawing.Size(82, 46);
            this.btnNow.TabIndex = 79;
            this.btnNow.Text = "Now";
            this.btnNow.UseVisualStyleBackColor = true;
            this.btnNow.Click += new System.EventHandler(this.btnNow_Click);
            // 
            // txtIdInsulinInjection
            // 
            this.txtIdInsulinInjection.BackColor = System.Drawing.Color.White;
            this.txtIdInsulinInjection.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtIdInsulinInjection.Location = new System.Drawing.Point(492, 68);
            this.txtIdInsulinInjection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIdInsulinInjection.Name = "txtIdInsulinInjection";
            this.txtIdInsulinInjection.ReadOnly = true;
            this.txtIdInsulinInjection.Size = new System.Drawing.Size(68, 32);
            this.txtIdInsulinInjection.TabIndex = 122;
            this.txtIdInsulinInjection.TabStop = false;
            this.txtIdInsulinInjection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(510, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 22);
            this.label5.TabIndex = 121;
            this.label5.Text = "Id";
            // 
            // dtpInjectionDate
            // 
            this.dtpInjectionDate.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpInjectionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInjectionDate.Location = new System.Drawing.Point(216, 65);
            this.dtpInjectionDate.Name = "dtpInjectionDate";
            this.dtpInjectionDate.Size = new System.Drawing.Size(136, 39);
            this.dtpInjectionDate.TabIndex = 5;
            // 
            // dtpInjectionTime
            // 
            this.dtpInjectionTime.CustomFormat = "HH:mm:ss";
            this.dtpInjectionTime.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dtpInjectionTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInjectionTime.Location = new System.Drawing.Point(358, 65);
            this.dtpInjectionTime.Name = "dtpInjectionTime";
            this.dtpInjectionTime.Size = new System.Drawing.Size(127, 39);
            this.dtpInjectionTime.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(374, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 22);
            this.label4.TabIndex = 117;
            this.label4.Text = "Time";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(268, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 22);
            this.label3.TabIndex = 116;
            this.label3.Text = "Date";
            // 
            // txtInsulinActual
            // 
            this.txtInsulinActual.BackColor = System.Drawing.Color.PaleGreen;
            this.txtInsulinActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtInsulinActual.Location = new System.Drawing.Point(24, 65);
            this.txtInsulinActual.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInsulinActual.Name = "txtInsulinActual";
            this.txtInsulinActual.Size = new System.Drawing.Size(68, 39);
            this.txtInsulinActual.TabIndex = 1;
            this.txtInsulinActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(114, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 48);
            this.label1.TabIndex = 114;
            this.label1.Text = "Calculated  Insulin [Ui]";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 48);
            this.label2.TabIndex = 113;
            this.label2.Text = "Actual insulin injection [Ui]";
            // 
            // txtInsulinCalculated
            // 
            this.txtInsulinCalculated.BackColor = System.Drawing.Color.White;
            this.txtInsulinCalculated.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtInsulinCalculated.Location = new System.Drawing.Point(132, 65);
            this.txtInsulinCalculated.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInsulinCalculated.Name = "txtInsulinCalculated";
            this.txtInsulinCalculated.Size = new System.Drawing.Size(68, 39);
            this.txtInsulinCalculated.TabIndex = 3;
            this.txtInsulinCalculated.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gridInjections
            // 
            this.gridInjections.AllowUserToAddRows = false;
            this.gridInjections.AllowUserToDeleteRows = false;
            this.gridInjections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridInjections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridInjections.Location = new System.Drawing.Point(3, 177);
            this.gridInjections.Name = "gridInjections";
            this.gridInjections.ReadOnly = true;
            this.gridInjections.RowTemplate.Height = 25;
            this.gridInjections.Size = new System.Drawing.Size(669, 430);
            this.gridInjections.TabIndex = 0;
            this.gridInjections.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridInjections_CellClick);
            this.gridInjections.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridInjections_CellContentClick);
            // 
            // tabFront
            // 
            this.tabFront.Location = new System.Drawing.Point(4, 24);
            this.tabFront.Name = "tabFront";
            this.tabFront.Padding = new System.Windows.Forms.Padding(3);
            this.tabFront.Size = new System.Drawing.Size(740, 613);
            this.tabFront.TabIndex = 0;
            this.tabFront.Text = "Front";
            this.tabFront.UseVisualStyleBackColor = true;
            // 
            // tabBack
            // 
            this.tabBack.Location = new System.Drawing.Point(4, 24);
            this.tabBack.Name = "tabBack";
            this.tabBack.Padding = new System.Windows.Forms.Padding(3);
            this.tabBack.Size = new System.Drawing.Size(740, 613);
            this.tabBack.TabIndex = 1;
            this.tabBack.Text = "Back";
            this.tabBack.UseVisualStyleBackColor = true;
            // 
            // tabSensor
            // 
            this.tabSensor.Location = new System.Drawing.Point(4, 24);
            this.tabSensor.Name = "tabSensor";
            this.tabSensor.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensor.Size = new System.Drawing.Size(740, 613);
            this.tabSensor.TabIndex = 2;
            this.tabSensor.Text = "Sensor";
            this.tabSensor.UseVisualStyleBackColor = true;
            // 
            // frmInjections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 643);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmInjections";
            this.Text = "Insuline Injections";
            this.Load += new System.EventHandler(this.frmInjections_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInjections)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabFront;
        private TabPage tabBack;
        private TabPage tabSensor;
        private TabPage tabData;
        private DataGridView gridInjections;
        private TextBox txtInsulinCalculated;
        private Label label1;
        private Label label2;
        private TextBox txtInsulinActual;
        private Label label4;
        private Label label3;
        private DateTimePicker dtpInjectionDate;
        private DateTimePicker dtpInjectionTime;
        private TextBox txtIdInsulinInjection;
        private Label label5;
        private Button btnSave;
        private Button btnNow;
        private CheckBox chkNowInAdd;
        private Button btnRemoveMeasurement;
        private Button btnAddMeasurement;
        private TextBox txtNotes;
        private RadioButton rdbSlowInsulin;
        private RadioButton rdbFastInsulin;
    }
}