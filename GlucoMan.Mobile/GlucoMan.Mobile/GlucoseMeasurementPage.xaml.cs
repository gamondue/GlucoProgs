using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlucoseMeasurementPage : ContentPage
    {
        BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements();
        List<GlucoseRecord> glucoseReadings = new List<GlucoseRecord>();
        public GlucoseMeasurementPage()
        {
            InitializeComponent();

            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            glucoseReadings = glucoseReadings.OrderBy(n => n.Timestamp).ToList();

            gridMeasurements.ItemsSource = glucoseReadings; 
        }
        //gridMeasurements.AutoGenerateColumns = false;
        //gridMeasurements.Columns.Clear();
        //gridMeasurements.ColumnCount = 2;
        //gridMeasurements.Columns[1].Name = "Glucose";
        //gridMeasurements.Columns[1].DataPropertyName = "GlucoseValue";
        //gridMeasurements.Columns[1].Width = 80;
        //gridMeasurements.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //gridMeasurements.Columns[0].Name = "Date and time";
        //gridMeasurements.Columns[0].DataPropertyName = "Timestamp";
        //gridMeasurements.Columns[0].Width = 180;
        //gridMeasurements.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        public void btnAddMeasurement_Click(object sender, EventArgs e)
        {
            double glucose = 0;
            try
            {
                glucose = double.Parse(txtGlucose.Text);
            }
            catch (Exception Ex) 
            {
                Common.LogOfProgram.Error("Glucose value unreadable", Ex);
                return;
            }
            if (chkNowInAdd.IsChecked)
            {
                dtpEventDate.Date = DateTime.Now;
                dtpEventTime.Time = DateTime.Now.TimeOfDay;
            }
            GlucoseRecord newReading = new GlucoseRecord();
            newReading.GlucoseValue = glucose;
            DateTime instant = dtpEventDate.Date;
            instant.Add(dtpEventTime.Time); 
            newReading.Timestamp = instant;
            glucoseReadings.Add(newReading);
            if (chkAutosave.IsChecked)
                bl.SaveGlucoseMeasurements(glucoseReadings);
            gridMeasurements.ItemsSource = null;
            gridMeasurements.ItemsSource = glucoseReadings;
        }
        private void btnRemoveMeasurement_Click(object sender, EventArgs e)
        {
            //    if (gridMeasurements.SelectedRows.Count > 0)
            //    {
            //        int rowIndex = gridMeasurements.SelectedRows[0].Index;
            //        if (MessageBox.Show(string.Format("Should I delete the measurement {0}, {1}",
            //            glucoseReadings[rowIndex].GlucoseValue,
            //            glucoseReadings[rowIndex].Timestamp), "",
            //            MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        {
            //            glucoseReadings.Remove(glucoseReadings[rowIndex]);
            //            if (chkAutosave.Checked)
            //                bl.SaveGlucoseMeasurements(glucoseReadings);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Choose a measurement to delete");
            //        return;
            //    }
            //    //gridMeasurements.DataSource = null;
            //    //gridMeasurements.DataSource = glucoseReadings;
        }
    //private void gridMeasurements_CellContentClick(object sender, DataGridViewCellEventArgs e)
    //{

    //}

    //private void gridMeasurements_RowEnter(object sender, DataGridViewCellEventArgs e)
    //{
    //    gridMeasurements.Rows[e.RowIndex].Selected = true;
    //    txtGlucose.Text = glucoseReadings[e.RowIndex].GlucoseValue.ToString();
    //    if (glucoseReadings[e.RowIndex].Timestamp != null)
    //        dtpEventInstant.Value = (DateTime)glucoseReadings[e.RowIndex].Timestamp;
    //}
    }
}