using GlucoMan;
using SharedFunctions;
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

namespace GlucoMan_Forms_Core
{
    public partial class frmGlucose : Form
    {
        BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements(); 

        List<GlucoseRecord> glucoseReadings = new List<GlucoseRecord>(); 
        public frmGlucose()
        {
            InitializeComponent();

            dgvMeasurements.AutoGenerateColumns = false;
            dgvMeasurements.Columns.Clear();
            dgvMeasurements.ColumnCount = 2;
            dgvMeasurements.Columns[1].Name = "Glucose";
            dgvMeasurements.Columns[1].DataPropertyName = "GlucoseValue";
            dgvMeasurements.Columns[1].Width = 80;
            dgvMeasurements.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMeasurements.Columns[0].Name = "Date and time";
            dgvMeasurements.Columns[0].DataPropertyName = "Timestamp";
            dgvMeasurements.Columns[0].Width = 180;
            dgvMeasurements.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void frmGlucose_Load(object sender, EventArgs e)
        {
            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            glucoseReadings = glucoseReadings.OrderBy(n => n.Timestamp).ToList();
            dgvMeasurements.DataSource = glucoseReadings; 
        }
        private void btnAddMeasurement_Click(object sender, EventArgs e)
        {
            double glucose = 0; 
            try {
                glucose = double.Parse(txtGlucose.Text); 
            }
            catch 
            {
                CommonFunctions.NotifyError("");
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
            dgvMeasurements.DataSource = null;
            dgvMeasurements.DataSource = glucoseReadings;
        }

        private void btnRemoveMeasurement_Click(object sender, EventArgs e)
        {
            if (dgvMeasurements.SelectedRows.Count > 0)
            {
                int rowIndex = dgvMeasurements.SelectedRows[0].Index;
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
            dgvMeasurements.DataSource = null;
            dgvMeasurements.DataSource = glucoseReadings;
        }
        private void dgvMeasurements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvMeasurements_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvMeasurements.Rows[e.RowIndex].Selected = true;
            txtGlucose.Text = glucoseReadings[e.RowIndex].GlucoseValue.ToString();
            if (glucoseReadings[e.RowIndex].Timestamp != null)
                dtpEventInstant.Value = (DateTime)glucoseReadings[e.RowIndex].Timestamp;
        }
    }
}
