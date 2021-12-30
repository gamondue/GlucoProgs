using GlucoMan;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmGlucose : Form
    {
        BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements();

        GlucoseRecord currentMeasurement = new GlucoseRecord();
        List<GlucoseRecord> glucoseReadings = new List<GlucoseRecord>(); 
        public frmGlucose()
        {
            InitializeComponent();

            gridMeasurements.AutoGenerateColumns = false;
            gridMeasurements.Columns.Clear();
            gridMeasurements.ColumnCount = 2;
            gridMeasurements.Columns[1].Name = "Glucose";
            gridMeasurements.Columns[1].DataPropertyName = "GlucoseValue";
            gridMeasurements.Columns[1].Width = 80;
            gridMeasurements.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridMeasurements.Columns[0].Name = "Date and time";
            gridMeasurements.Columns[0].DataPropertyName = "Timestamp";
            gridMeasurements.Columns[0].Width = 180;
            gridMeasurements.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            gridMeasurements.AllowUserToAddRows = false;
            gridMeasurements.ReadOnly = true;

            foreach (DataGridViewColumn column in gridMeasurements.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }
        private void frmGlucose_Load(object sender, EventArgs e)
        {
            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            // sort list descending
            glucoseReadings = glucoseReadings.OrderByDescending(n => n.Timestamp).ToList();
            // binf list to grid 
            gridMeasurements.DataSource = glucoseReadings;
        }
        private void btnAddMeasurement_Click(object sender, EventArgs e)
        {
            double glucose = 0; 
            try {
                glucose = double.Parse(txtGlucose.Text); 
            }
            catch 
            {
                MessageBox.Show("Input a number in the glucose box!");
                return; 
            }
            if (chkNowInAdd.Checked)
                dtpEventInstant.Value = DateTime.Now;
            GlucoseRecord newReading = new GlucoseRecord();
            newReading.GlucoseValue = glucose;
            newReading.Timestamp = dtpEventInstant.Value; 
            glucoseReadings.Add(newReading);
            if (chkAutosave.Checked)
                bl.SaveGlucoseMeasurements(glucoseReadings);
            gridMeasurements.DataSource = null;
            gridMeasurements.DataSource = glucoseReadings; 
        }
        private void btnRemoveMeasurement_Click(object sender, EventArgs e)
        {
            if (gridMeasurements.SelectedRows.Count > 0)
            {
                int rowIndex = gridMeasurements.SelectedRows[0].Index;
                if (MessageBox.Show(string.Format("Should I delete the measurement {0}, {1}",
                    glucoseReadings[rowIndex].GlucoseValue,
                    glucoseReadings[rowIndex].Timestamp), "", 
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    glucoseReadings.Remove(glucoseReadings[rowIndex]); 
                    if (chkAutosave.Checked)
                        bl.SaveGlucoseMeasurements(glucoseReadings);
                }
            }
            else
            {
                MessageBox.Show("Choose a measurement to delete");
                return;
            }
            gridMeasurements.DataSource = null;
            gridMeasurements.DataSource = glucoseReadings;
        }
        private void dgvMeasurements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvMeasurements_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gridMeasurements.Rows[e.RowIndex].Selected = true;
            txtGlucose.Text = glucoseReadings[e.RowIndex].GlucoseValue.ToString();
            if (glucoseReadings[e.RowIndex].Timestamp != null)
                dtpEventInstant.Value = (DateTime)glucoseReadings[e.RowIndex].Timestamp;
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            dtpEventInstant.Value = DateTime.Now;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            bl.SaveOneGlucoseMeasurement(currentMeasurement); 
        }

        private void FromUiToClass()
        {
            double glucose; 
            double.TryParse(txtGlucose.Text, out glucose);
            if (glucose < 0)
            {
                MessageBox.Show("Input a number in the glucose box!");
                return; 
            }
            currentMeasurement.GlucoseValue = glucose;
            currentMeasurement.Timestamp = dtpEventInstant.Value;
        }
    }
}
