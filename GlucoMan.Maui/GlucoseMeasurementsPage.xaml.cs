using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.Controls;
using System.Linq;

namespace GlucoMan.Maui;

public partial class GlucoseMeasurementsPage : ContentPage
{
    BL_GlucoseMeasurements bl = new BL_GlucoseMeasurements();
    GlucoseRecord currentGlucose = new GlucoseRecord();
    List<GlucoseRecord> glucoseReadings = new List<GlucoseRecord>();
    double? MonthsOfDataShownInTheGrids = 3;

    public int? IdGlucoseRecord
    {
        get
        {
        return currentGlucose.IdGlucoseRecord;
    }

    }

    // Support a direct tap on the item Frame to set selection (helps on Android)
    private void OnItemTapped(object? sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is GlucoseRecord tapped)
        {
            // set selected item which will trigger SelectionChanged
            cvMeasurements.SelectedItem = tapped;
        }
    }
    public GlucoseMeasurementsPage(int? IdGlucoseRecord)
    {
        InitializeComponent();
        // The named CollectionView `cvMeasurements` is available via generated partial class after InitializeComponent
        Parameters parameters = DatabaseService.Instance.Database.GetParameters();
        if (parameters != null && parameters.MonthsOfDataShownInTheGrids > 0)
            MonthsOfDataShownInTheGrids = parameters.MonthsOfDataShownInTheGrids;   
        if (IdGlucoseRecord != null)
            currentGlucose = bl.GetOneGlucoseRecord(IdGlucoseRecord);
        RefreshUi();
    }
    public GlucoseMeasurementsPage() : this(null) { }
    private void RefreshUi()
    {
        FromClassToUi();
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
        currentGlucose.GlucoseValue.Double = glucose;
        DateTime instant = new DateTime(dtpEventDate.Date.Value.Year, dtpEventDate.Date.Value.Month, dtpEventDate.Date.Value.Day,
            dtpEventTime.Time.Value.Hours, dtpEventTime.Time.Value.Minutes, dtpEventTime.Time.Value.Seconds);
        currentGlucose.Notes = txtNotes.Text;
        currentGlucose.EventTime.DateTime = instant;
        //currentGlucose.EventTime = dtpEventDate.Date;
    }
    private void FromClassToUi()
    {
        if (currentGlucose.GlucoseValue.Double != null && !double.IsNaN((double)currentGlucose.GlucoseValue.Double))
            txtGlucose.Text = currentGlucose.GlucoseValue.ToString();
        if (currentGlucose.EventTime != null
            && currentGlucose.EventTime.DateTime != new DateTime(1, 1, 1, 0, 0, 0)
            && currentGlucose.EventTime.DateTime != General.DateNull)
        {
            dtpEventDate.Date = (DateTime)Safe.DateTime(currentGlucose.EventTime.DateTime);
            dtpEventTime.Time = ((DateTime)currentGlucose.EventTime.DateTime).TimeOfDay;
        }
        else
        {
            dtpEventDate.Date = DateTime.Now;
            dtpEventTime.Time = DateTime.Now.TimeOfDay;
        }
        txtNotes.Text = currentGlucose.Notes;
        txtIdGlucoseRecord.Text = currentGlucose.IdGlucoseRecord.ToString();
    }
    private void RefreshGrid()
    {
        DateTime now = DateTime.Now;
        var readings = bl.ReadGlucoseMeasurements(
            now.Subtract(new TimeSpan((int)(MonthsOfDataShownInTheGrids * 365 / 12),
                1, 0, 0)), now.AddDays(1));
        
        glucoseReadings.Clear();
        foreach (var reading in readings)
        {
            glucoseReadings.Add(reading);
        }
        
        // Always reset ItemsSource to refresh CollectionView
        cvMeasurements.ItemsSource = null;
        cvMeasurements.ItemsSource = glucoseReadings;
    }
    public void btnClearData_Click(object sender, EventArgs e)
    {
        txtGlucose.Text = "";
        dtpEventDate.Date = DateTime.Now;
        dtpEventTime.Time = ((DateTime)currentGlucose.EventTime.DateTime).TimeOfDay;
        txtIdGlucoseRecord.Text = "";
        txtNotes.Text = "";
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
        // Determine selected item by IsSelectedInList flag since we clear SelectedItem for visual reasons
        GlucoseRecord gr = glucoseReadings.FirstOrDefault(g => g.IsSelectedInList);
        if (gr != null)
        {
            bool remove = await DisplayAlert(String.Format(
                AppStrings.DeleteGlucoseMeasurementConfirm,
                gr.GlucoseValue.ToString(),
                gr.EventTime.ToString(),
                gr.IdGlucoseRecord.ToString()),
                "", AppStrings.Yes, AppStrings.No);
            if (remove)
            {
                bl.DeleteOneGlucoseMeasurement(gr);
                RefreshGrid();
            }
        }
        else
        {
            await DisplayAlert(AppStrings.SavingNotPossible, AppStrings.ChooseGlucoseMeasurementToDelete, AppStrings.OK);
            return;
        }
        RefreshGrid();
    }
    private async void btnSave_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdGlucoseRecord.Text == "")
        {
            await DisplayAlert(AppStrings.SelectOneGlucoseMeasurement, AppStrings.ChooseGlucoseMeasurementToSave, AppStrings.OK);
            return;
        }
        FromUiToClass();
        bl.SaveOneGlucoseMeasurement(currentGlucose);
        RefreshGrid();
    }
    private void btnNow_Click(object sender, EventArgs e)
    {
        dtpEventDate.Date = DateTime.Now;
        dtpEventTime.Time = DateTime.Now.TimeOfDay;
    }
    void OnGridSelection(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        var selectedGlucose = (GlucoseRecord)e.CurrentSelection[0];

        // Deselect all others
        foreach (var glucose in glucoseReadings)
        {
            glucose.IsSelectedInList = false;
        }

        selectedGlucose.IsSelectedInList = true;

        // Ensure the selected item is visible and centered
        try
        {
            if (cvMeasurements != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Make the selected item visible without centering it in the viewport
                    cvMeasurements.ScrollTo(selectedGlucose, position: ScrollToPosition.MakeVisible, animate: false);
                });

                // Deselect in CollectionView to avoid default selection visuals
                // but keep IsSelectedInList flag so other code can determine the selected item
                cvMeasurements.SelectedItem = null;
            }
        }
        catch
        {
            // ignore scroll failures - non-critical
        }

        currentGlucose = selectedGlucose;
        FromClassToUi();
    }
}