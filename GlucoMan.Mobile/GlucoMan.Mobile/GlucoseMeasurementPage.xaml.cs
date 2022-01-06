using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
        private void RefreshGrid()
        {
            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            glucoseReadings = glucoseReadings.OrderByDescending(n => n.Timestamp).ToList();
            //ObservableCollection<GlucoseRecord> collection = new ObservableCollection<GlucoseRecord>(glucoseReadings);
            //this.BindingContext = collection;
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
            glucoseReadings.Add(currentGlucose);
            glucoseReadings = glucoseReadings.OrderByDescending(n => n.Timestamp).ToList();
            // erase Id to save a new record
            currentGlucose.IdGlucoseRecord = null; 
            if (chkAutosave.IsChecked)
                bl.SaveGlucoseMeasurements(glucoseReadings);
            glucoseReadings = bl.ReadGlucoseMeasurements(null, null);
            this.BindingContext = glucoseReadings;
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
                    glucoseReadings.Remove(gr);
                    if (chkAutosave.IsChecked)
                        bl.SaveGlucoseMeasurements(glucoseReadings);
                }
            }
            else
            {
                await DisplayAlert("Saving not possible","Choose a measurement to delete", "Ok");
                return;
            }
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
        }
        private void FromClassToUi()
        {
            txtGlucose.Text = currentGlucose.GlucoseValue.ToString();  
            dtpEventDate.Date = (DateTime)Safe.DateTime(currentGlucose.Timestamp);
            dtpEventTime.Time = (DateTime)currentGlucose.Timestamp - dtpEventDate.Date;
            txtIdGlucoseRecord.Text = currentGlucose.IdGlucoseRecord.ToString();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneGlucoseMeasurement(currentGlucose);
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            dtpEventDate.Date = DateTime.Now;
            dtpEventTime.Time = DateTime.Now.TimeOfDay;
        }
        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            currentGlucose = (GlucoseRecord)e.SelectedItem;
            FromClassToUi();
        }
    }
}