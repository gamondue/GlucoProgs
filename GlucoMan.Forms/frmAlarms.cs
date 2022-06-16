using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmAlarms : Form
    {
        BL_AlarmManagement bl = new BL_AlarmManagement(); 
        
        Alarm currentAlarm = new Alarm(); 

        public frmAlarms()
        {
            InitializeComponent();
            currentAlarm.TimeStart.DateTime = DateTime.Now;
            currentAlarm.Interval = new TimeSpan(0,2,0,0);
            DateTime dummy = (DateTime)currentAlarm.TimeStart.DateTime; 
            currentAlarm.TimeAlarm.DateTime = dummy.Add((TimeSpan)currentAlarm.Interval); 
        }
        private void fromUiToClass()
        {
            currentAlarm.TimeStart.DateTime = new DateTime(dtpStartDate.Value.Year,
                dtpStartDate.Value.Month, dtpStartDate.Value.Day,
                dtpStartTime.Value.Hour, dtpStartTime.Value.Minute, 0);

            currentAlarm.Interval = new TimeSpan((int)updDay.Value, 
                (int) updHour.Value, (int)updMinute.Value, 0);

            currentAlarm.TimeAlarm.DateTime = new DateTime(dtpAlarmDate.Value.Year,
                dtpAlarmDate.Value.Month, dtpAlarmDate.Value.Day,
                dtpAlarmTime.Value.Hour, dtpAlarmTime.Value.Minute, 0);
            
            double durationInMinutes;
            double.TryParse(txtDurationInMinutes.Text, out durationInMinutes);
            int integerDuration = (int)durationInMinutes; 
            currentAlarm.Duration = new TimeSpan(0, integerDuration, 
                (int)((durationInMinutes - integerDuration) * 60)); 
            
            currentAlarm.IsEnabled = chkIsEnabled.Checked; 
            currentAlarm.IsRepeated = chkIsRepeated.Checked;
        }
        private void fromClassToUi()
        {
            // start DateTime
            DateTime dummy = (DateTime)currentAlarm.TimeStart.DateTime; 
            dtpStartDate.Value = new DateTime(dummy.Year, dummy.Month, dummy.Day);
            dtpStartTime.Value = dtpStartDate.Value.Add(new TimeSpan(dummy.Hour,
                dummy.Minute, 0));

            // alarm DateTime
            dummy = (DateTime)currentAlarm.TimeAlarm.DateTime; 
            dtpAlarmDate.Value = new DateTime(dummy.Year, dummy.Month, dummy.Day);
            dtpAlarmTime.Value = dtpAlarmDate.Value.Add(new TimeSpan(dummy.Hour,
                dummy.Minute, 0));

            // interval TimeSpan
            TimeSpan dummy1 = (TimeSpan)currentAlarm.Interval; 
            updDay.Value = dummy1.Days;
            updHour.Value = dummy1.Hours;
            updMinute.Value = dummy1.Minutes;

            // Duration SpanTime
            if (currentAlarm.Duration != null)
            {
                dummy1 = (TimeSpan)currentAlarm.Duration;
                txtDurationInMinutes.Text = dummy1.TotalMinutes.ToString();
            }
        }
        private void frmAlarms_Load(object sender, EventArgs e)
        {
            fromClassToUi();
            RefreshGrid();
        }
        private void RefreshGrid()
        {
            dgwAlarms.DataSource = bl.ReadAllAlarms();
            dgwAlarms.Refresh();
        }
        private void btnAddTime_Click(object sender, EventArgs e)
        {
            fromUiToClass();
            DateTime dummy = (DateTime)currentAlarm.TimeStart.DateTime;

            currentAlarm.TimeAlarm.DateTime = dummy.Add((TimeSpan)currentAlarm.Interval);
            fromClassToUi();
        }
        private void btnAddAlarm_Click(object sender, EventArgs e)
        {
            fromUiToClass();
            bl.AddAlarm(currentAlarm); 
        }
    }
}
