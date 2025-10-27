using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;

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
    public GlucoseMeasurementsPage(int? IdGlucoseRecord)
    {
        InitializeComponent();
        Parameters parameters = Common.Database.GetParameters();
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
        DateTime instant = new DateTime(dtpEventDate.Date.Year, dtpEventDate.Date.Month, dtpEventDate.Date.Day,
            dtpEventTime.Time.Hours, dtpEventTime.Time.Minutes, dtpEventTime.Time.Seconds);
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
        glucoseReadings = bl.ReadGlucoseMeasurements(
            now.Subtract(new TimeSpan((int)(MonthsOfDataShownInTheGrids * 365 / 12),
                1, 0, 0)), now.AddDays(1));
        this.BindingContext = glucoseReadings;
        //gridMeasurements.ItemsSource = glucoseReadings;
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
        GlucoseRecord gr = (GlucoseRecord)gridMeasurements.SelectedItem;
        if (gr != null)
        {
            bool remove = await DisplayAlert(String.Format(
                "Should I delete the measurement {0}, {1}, Id {2}?",
                gr.GlucoseValue.ToString(),
                gr.EventTime.ToString(),
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
    private async void btnSave_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdGlucoseRecord.Text == "")
        {
            await DisplayAlert("Select one glicemia measurement from the list", "Choose a measurement to save", "Ok");
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