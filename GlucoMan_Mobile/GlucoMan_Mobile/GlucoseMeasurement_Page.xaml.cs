using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlucoseMeasurement_Page : ContentPage
    {
        BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements();
        List<GlucoseRecord> glucoseReadings = new List<GlucoseRecord>();
        public GlucoseMeasurement_Page()
        {
            InitializeComponent();

            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            glucoseReadings = glucoseReadings.OrderBy(n => n.Timestamp).ToList();

            viewMeasurements.ItemsSource = glucoseReadings; 
        }
        //viewMeasurements.AutoGenerateColumns = false;
        //viewMeasurements.Columns.Clear();
        //viewMeasurements.ColumnCount = 2;
        //viewMeasurements.Columns[1].Name = "Glucose";
        //viewMeasurements.Columns[1].DataPropertyName = "GlucoseValue";
        //viewMeasurements.Columns[1].Width = 80;
        //viewMeasurements.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //viewMeasurements.Columns[0].Name = "Date and time";
        //viewMeasurements.Columns[0].DataPropertyName = "Timestamp";
        //viewMeasurements.Columns[0].Width = 180;
        //viewMeasurements.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        public void btnAddMeasurement_Click(object sender, EventArgs e)
        {
            double glucose = 0;
            try
            {
                glucose = double.Parse(txtGlucose.Text);
            }
            catch
            {
                CommonFunctions.NotifyError("Glucose value unreadable");
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
            viewMeasurements.ItemsSource = null;
            viewMeasurements.ItemsSource = glucoseReadings;
        }
        private void btnRemoveMeasurement_Click(object sender, EventArgs e)
        {
            //    if (viewMeasurements.SelectedRows.Count > 0)
            //    {
            //        int rowIndex = viewMeasurements.SelectedRows[0].Index;
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
            //    //viewMeasurements.DataSource = null;
            //    //viewMeasurements.DataSource = glucoseReadings;
        }
    //private void viewMeasurements_CellContentClick(object sender, DataGridViewCellEventArgs e)
    //{

    //}

    //private void viewMeasurements_RowEnter(object sender, DataGridViewCellEventArgs e)
    //{
    //    viewMeasurements.Rows[e.RowIndex].Selected = true;
    //    txtGlucose.Text = glucoseReadings[e.RowIndex].GlucoseValue.ToString();
    //    if (glucoseReadings[e.RowIndex].Timestamp != null)
    //        dtpEventInstant.Value = (DateTime)glucoseReadings[e.RowIndex].Timestamp;
    //}
}
}