using GlucoMan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmGlucose : Form
    {
        BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements();

        GlucoseRecord currentGlucose = new GlucoseRecord();
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
            RefreshGrid(); 
        }
        private void RefreshGrid()
        {
            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            // sort list descending
            glucoseReadings = glucoseReadings.OrderByDescending(n => n.Timestamp).ToList();
            // bind list to grid 
            gridMeasurements.DataSource = glucoseReadings;
        }
        private void btnAddMeasurement_Click(object sender, EventArgs e)
        {
            if (chkNowInAdd.Checked)
                dtpEventInstant.Value = DateTime.Now;
            FromUiToClass();
            glucoseReadings.Add(currentGlucose);
            if (chkAutosave.Checked)
                bl.SaveGlucoseMeasurements(glucoseReadings);
            glucoseReadings = glucoseReadings.OrderByDescending(n => n.Timestamp).ToList();
            RefreshGrid();
        }
        private void btnRemoveMeasurement_Click(object sender, EventArgs e)
        {
            if (gridMeasurements.SelectedRows.Count > 0)
            {
                int rowIndex = gridMeasurements.SelectedRows[0].Index;
                GlucoseRecord gr = (GlucoseRecord)glucoseReadings[rowIndex];
                if (MessageBox.Show(string.Format("Should I delete the measurement {0}, {1}, Id {2}?",
                    gr.GlucoseValue, 
                    gr.Timestamp,
                    gr.IdGlucoseRecord), "",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    glucoseReadings.Remove(gr);
                    if (chkAutosave.Checked)
                        bl.SaveGlucoseMeasurements(glucoseReadings);
                }
            }
            else
            {
                MessageBox.Show("Choose a measurement to delete");
                return;
            }
            RefreshGrid();
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
            currentGlucose = new GlucoseRecord();
            currentGlucose.IdGlucoseRecord = Safe.Int(txtIdGlucoseRecord.Text);
            currentGlucose.GlucoseValue = glucose;
            currentGlucose.Timestamp = dtpEventInstant.Value;
        }
        private void FromClassToUi()
        {
            txtGlucose.Text = currentGlucose.GlucoseValue.ToString();
            dtpEventInstant.Value = (DateTime)Safe.DateTime(currentGlucose.Timestamp);
            txtIdGlucoseRecord.Text = currentGlucose.IdGlucoseRecord.ToString();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneGlucoseMeasurement(currentGlucose);
            RefreshGrid();
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            dtpEventInstant.Value = DateTime.Now;
        }
        private void gridMeasurements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridMeasurements_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                gridMeasurements.Rows[e.RowIndex].Selected = true;

                currentGlucose = (GlucoseRecord)glucoseReadings[e.RowIndex];
                FromClassToUi();
            }
        }
    }
}
