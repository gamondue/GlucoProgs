using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlucoseMeasurementPage : ContentPage
    {
        BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements();
        GlucoseRecord currentGlucose;
        List<GlucoseRecord> glucoseReadings = new List<GlucoseRecord>();

        public GlucoseMeasurementPage()
        {
            InitializeComponent();

            currentGlucose = new GlucoseRecord();   

            RefreshGrid();
        }
        private void FromUiToClass()
        {
            double? glucose = Safe.Double(txtGlucose.Text);
            if (glucose == null)
            {
                txtGlucose.Text = "";
                //Console.Beep();
                return;
            }
            currentGlucose = new GlucoseRecord();
            currentGlucose.IdGlucoseRecord = Safe.Int(txtIdGlucoseRecord.Text);
            currentGlucose.GlucoseValue = glucose;
            DateTime instant = new DateTime(dtpEventDate.Date.Year, dtpEventDate.Date.Month, dtpEventDate.Date.Day,
                dtpEventTime.Time.Hours, dtpEventTime.Time.Minutes, dtpEventTime.Time.Seconds);
            currentGlucose.Timestamp = instant;
            //currentGlucose.Timestamp = dtpEventDate.Date;
        }
        private void FromClassToUi()
        {
            txtGlucose.Text = currentGlucose.GlucoseValue.ToString();
            if (currentGlucose.Timestamp != null 
                && currentGlucose.Timestamp != new DateTime(1, 1, 1, 0, 0, 0)
                && currentGlucose.Timestamp != Common.DateNull) 
            { 
                dtpEventDate.Date = (DateTime)Safe.DateTime(currentGlucose.Timestamp);
                dtpEventTime.Time = (DateTime)currentGlucose.Timestamp - dtpEventDate.Date;
            }
            else
            {
                dtpEventDate.Date = DateTime.Now;
                dtpEventTime.Time = new TimeSpan(0);
            }
            txtIdGlucoseRecord.Text = currentGlucose.IdGlucoseRecord.ToString();
        }
        private void RefreshGrid()
        {
            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            this.BindingContext = glucoseReadings; 
            //gridMeasurements.ItemsSource = glucoseReadings;
        }
        public void btnAddMeasurement_Click(object sender, EventArgs e)
        {
            if (chkNowInAdd.IsChecked)
            {
                dtpEventDate.Date = DateTime.Now;
                dtpEventTime.Time = DateTime.Now.TimeOfDay;
            }
            FromUiToClass();
            //glucoseReadings.Add(currentGlucose);
            // erase Id to save a new record
            currentGlucose.IdGlucoseRecord = null;
            bl.SaveOneGlucoseMeasurement(currentGlucose);
            RefreshGrid();
        }
        private async void btnRemoveMeasurement_Click(object sender, EventArgs e)
        {
            GlucoseRecord gr = (GlucoseRecord)gridMeasurements.SelectedItem; 
            if (gr != null)
            {
                bool remove = await DisplayAlert(String.Format("Should I delete the measurement {0}, {1}, Id {2}?",
                    gr.GlucoseValue.ToString(),
                    gr.Timestamp.ToString(),
                    gr.IdGlucoseRecord.ToString()),
                    "", "Yes", "No");
                if (remove)
                {
                    bl.DeleteOneGlucoseMeasurement(gr);
                    RefreshGrid();
                }
            }
            else
            {
                await DisplayAlert("Saving not possible", "Choose a measurement to delete", "Ok");
                return;
            }
            RefreshGrid();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneGlucoseMeasurement(currentGlucose);
            RefreshGrid();
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            dtpEventDate.Date = DateTime.Now;
            dtpEventTime.Time = DateTime.Now.TimeOfDay;
        }
        void OnGridSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
                // make the tapped row current
            currentGlucose = (GlucoseRecord)e.SelectedItem;
            FromClassToUi();
        }
    }
}