
namespace GlucoMan.Forms
{
    partial class frmAlarms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlarms));
            this.btnSetAlarm = new System.Windows.Forms.Button();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dgwAlarms = new System.Windows.Forms.DataGridView();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.btnAddAlarm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpAlarmDate = new System.Windows.Forms.DateTimePicker();
            this.updDay = new System.Windows.Forms.NumericUpDown();
            this.updHour = new System.Windows.Forms.NumericUpDown();
            this.updMinute = new System.Windows.Forms.NumericUpDown();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpAlarmTime = new System.Windows.Forms.DateTimePicker();
            this.chkIsRepeated = new System.Windows.Forms.CheckBox();
            this.chkIsEnabled = new System.Windows.Forms.CheckBox();
            this.txtDurationInMinutes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeleteAlarm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgwAlarms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSetAlarm
            // 
            this.btnSetAlarm.Location = new System.Drawing.Point(295, 197);
            this.btnSetAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetAlarm.Name = "btnSetAlarm";
            this.btnSetAlarm.Size = new System.Drawing.Size(96, 46);
            this.btnSetAlarm.TabIndex = 0;
            this.btnSetAlarm.Text = "Set alarm";
            this.btnSetAlarm.UseVisualStyleBackColor = true;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(12, 30);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(122, 29);
            this.dtpStartDate.TabIndex = 7;
            // 
            // dgwAlarms
            // 
            this.dgwAlarms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwAlarms.Location = new System.Drawing.Point(12, 250);
            this.dgwAlarms.Name = "dgwAlarms";
            this.dgwAlarms.RowHeadersWidth = 62;
            this.dgwAlarms.Size = new System.Drawing.Size(379, 205);
            this.dgwAlarms.TabIndex = 8;
            // 
            // btnAddTime
            // 
            this.btnAddTime.Location = new System.Drawing.Point(12, 197);
            this.btnAddTime.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(76, 46);
            this.btnAddTime.TabIndex = 9;
            this.btnAddTime.Text = "+time";
            this.btnAddTime.UseVisualStyleBackColor = true;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // btnAddAlarm
            // 
            this.btnAddAlarm.Location = new System.Drawing.Point(116, 197);
            this.btnAddAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddAlarm.Name = "btnAddAlarm";
            this.btnAddAlarm.Size = new System.Drawing.Size(76, 46);
            this.btnAddAlarm.TabIndex = 10;
            this.btnAddAlarm.Text = "+ alarm";
            this.btnAddAlarm.UseVisualStyleBackColor = true;
            this.btnAddAlarm.Click += new System.EventHandler(this.btnAddAlarm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 21);
            this.label1.TabIndex = 12;
            this.label1.Text = "day";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 21);
            this.label2.TabIndex = 14;
            this.label2.Text = "hour";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 21);
            this.label3.TabIndex = 16;
            this.label3.Text = "min";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 21);
            this.label5.TabIndex = 19;
            this.label5.Text = "Start time";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 21);
            this.label6.TabIndex = 20;
            this.label6.Text = "Interval of alarm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 21);
            this.label4.TabIndex = 22;
            this.label4.Text = "Alarm time";
            // 
            // dtpAlarmDate
            // 
            this.dtpAlarmDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAlarmDate.Location = new System.Drawing.Point(12, 153);
            this.dtpAlarmDate.Name = "dtpAlarmDate";
            this.dtpAlarmDate.Size = new System.Drawing.Size(122, 29);
            this.dtpAlarmDate.TabIndex = 21;
            // 
            // updDay
            // 
            this.updDay.Location = new System.Drawing.Point(12, 89);
            this.updDay.Name = "updDay";
            this.updDay.Size = new System.Drawing.Size(51, 29);
            this.updDay.TabIndex = 23;
            this.updDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // updHour
            // 
            this.updHour.Location = new System.Drawing.Point(110, 89);
            this.updHour.Name = "updHour";
            this.updHour.Size = new System.Drawing.Size(51, 29);
            this.updHour.TabIndex = 24;
            this.updHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.updHour.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // updMinute
            // 
            this.updMinute.Location = new System.Drawing.Point(216, 89);
            this.updMinute.Name = "updMinute";
            this.updMinute.Size = new System.Drawing.Size(51, 29);
            this.updMinute.TabIndex = 25;
            this.updMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.Location = new System.Drawing.Point(145, 30);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(101, 29);
            this.dtpStartTime.TabIndex = 26;
            // 
            // dtpAlarmTime
            // 
            this.dtpAlarmTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpAlarmTime.Location = new System.Drawing.Point(145, 153);
            this.dtpAlarmTime.Name = "dtpAlarmTime";
            this.dtpAlarmTime.Size = new System.Drawing.Size(101, 29);
            this.dtpAlarmTime.TabIndex = 27;
            // 
            // chkIsRepeated
            // 
            this.chkIsRepeated.AutoSize = true;
            this.chkIsRepeated.Location = new System.Drawing.Point(276, 52);
            this.chkIsRepeated.Name = "chkIsRepeated";
            this.chkIsRepeated.Size = new System.Drawing.Size(94, 25);
            this.chkIsRepeated.TabIndex = 28;
            this.chkIsRepeated.Text = "Repeated";
            this.chkIsRepeated.UseVisualStyleBackColor = true;
            // 
            // chkIsEnabled
            // 
            this.chkIsEnabled.AutoSize = true;
            this.chkIsEnabled.Checked = true;
            this.chkIsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsEnabled.Location = new System.Drawing.Point(276, 21);
            this.chkIsEnabled.Name = "chkIsEnabled";
            this.chkIsEnabled.Size = new System.Drawing.Size(84, 25);
            this.chkIsEnabled.TabIndex = 29;
            this.chkIsEnabled.Text = "Enabled";
            this.chkIsEnabled.UseVisualStyleBackColor = true;
            // 
            // txtDurationInMinutes
            // 
            this.txtDurationInMinutes.Location = new System.Drawing.Point(276, 153);
            this.txtDurationInMinutes.Name = "txtDurationInMinutes";
            this.txtDurationInMinutes.Size = new System.Drawing.Size(53, 29);
            this.txtDurationInMinutes.TabIndex = 30;
            this.txtDurationInMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(270, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 21);
            this.label7.TabIndex = 31;
            this.label7.Text = "Duration of alarm";
            // 
            // btnDeleteAlarm
            // 
            this.btnDeleteAlarm.Location = new System.Drawing.Point(192, 197);
            this.btnDeleteAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteAlarm.Name = "btnDeleteAlarm";
            this.btnDeleteAlarm.Size = new System.Drawing.Size(76, 46);
            this.btnDeleteAlarm.TabIndex = 32;
            this.btnDeleteAlarm.Text = "- alarm";
            this.btnDeleteAlarm.UseVisualStyleBackColor = true;
            // 
            // frmAlarms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 466);
            this.Controls.Add(this.btnDeleteAlarm);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDurationInMinutes);
            this.Controls.Add(this.chkIsEnabled);
            this.Controls.Add(this.chkIsRepeated);
            this.Controls.Add(this.dtpAlarmTime);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.updMinute);
            this.Controls.Add(this.updHour);
            this.Controls.Add(this.updDay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpAlarmDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddAlarm);
            this.Controls.Add(this.btnAddTime);
            this.Controls.Add(this.dgwAlarms);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.btnSetAlarm);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAlarms";
            this.Text = "Alarms";
            this.Load += new System.EventHandler(this.frmAlarms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgwAlarms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnSetAlarm;
        private DateTimePicker dtpStartDate;
        private DataGridView dgwAlarms;
        private Button btnAddTime;
        private Button btnAddAlarm;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label label4;
        private DateTimePicker dtpAlarmDate;
        private NumericUpDown updDay;
        private NumericUpDown updHour;
        private NumericUpDown updMinute;
        private DateTimePicker dtpStartTime;
        private DateTimePicker dtpAlarmTime;
        private CheckBox chkIsRepeated;
        private CheckBox chkIsEnabled;
        private TextBox txtDurationInMinutes;
        private Label label7;
        private Button btnDeleteAlarm;
    }
}