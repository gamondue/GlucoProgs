using GlucoMan.BusinessLayer;
using GlucoMan;
using Mathematics.Identification1;

namespace GlucoMan.Maui;

public partial class IdentificationPage2 : ContentPage
{
    private DateTime _dateFrom;
    private DateTime _dateTo;

    public IdentificationPage2(DateTime dateTo, int nWeeks)
    {
        InitializeComponent();
        _dateTo = dateTo;
        _dateFrom = dateTo.AddDays(-7 * nWeeks);
        lblDateRange.Text = $"From: {_dateFrom:dd/MM/yyyy} - To: {_dateTo:dd/MM/yyyy}";
    }

    private async void btnIdentify3_Click(object sender, EventArgs e)
    {
        try
        {
            // Disable button during processing
            btnIdentify3.IsEnabled = false;
            lblStatus.Text = "Identifying MIMO model...";

            // Get data from business layer
            var blMeals = new BL_MealAndFood();
            var blGlucose = new BL_GlucoseMeasurements();
            var blInj = new BL_BolusesAndInjections();

            var meals = blMeals.GetMeals(_dateFrom, _dateTo);
            var injections = blInj.GetQuickInjections(_dateFrom, _dateTo);
            var glucose = blGlucose.GetSensorsRecords(_dateFrom, _dateTo);

            // Validate data availability
            if (glucose == null || glucose.Count < 10)
            {
                await DisplayAlert("Error", "Insufficient glucose data. Need at least 10 measurements.", "OK");
                lblStatus.Text = "Identification failed: insufficient data";
                return;
            }

            // Run MIMO identification with default parameters
            // Ts = 900s (15 min) matches typical CGM sampling
            double TsSeconds = 900.0;
            int maxDelaySamples = 40;  // Up to 10 hours delay search
            double ridge = 0.01;

            var result = Identification3.IdentifyMimoFirstOrder(
                glucose,
                meals,
                injections,
                TsSeconds,
                maxDelaySamples,
                ridge);

            if (result == null)
            {
                await DisplayAlert("Error", "Identification failed. Check data quality.", "OK");
                lblStatus.Text = "Identification failed";
                return;
            }

            // Display Physical Parameters
            lblTau.Text = $"{result.Tau:F0} s ({result.Tau / 60:F1} min)";
            lblK1.Text = $"{result.K1:G4} mg/dL per g";
            lblK2.Text = $"{result.K2:G4} mg/dL per U";
            lblY0.Text = $"{result.Y0:F1} mg/dL";

            // Display Delays
            lblDelay1.Text = $"{result.Delay1Seconds / 60:F0} min ({result.Delay1} samples)";
            lblDelay2.Text = $"{result.Delay2Seconds / 60:F0} min ({result.Delay2} samples)";

            // Display Discrete Parameters
            lblA.Text = result.A.ToString("G6");
            lblB1.Text = result.B1.ToString("G6");
            lblB2.Text = result.B2.ToString("G6");
            lblC.Text = result.C.ToString("G6");

            // Display Fit Quality
            lblR2.Text = result.RSquared.ToString("F4");
            lblRMSE.Text = $"{result.RMSE:F2} mg/dL";
            lblSamples.Text = result.ValidSamples.ToString();

            // Update status
            lblStatus.Text = $"Identification complete. Data: {result.DataStart:dd/MM} - {result.DataEnd:dd/MM}";

            // Show summary alert
            await DisplayAlert("MIMO Identification Complete",
                $"Time constant ? = {result.Tau / 60:F1} min\n" +
                $"CHO gain K? = {result.K1:G3} mg/dL per g\n" +
                $"Insulin gain K? = {result.K2:G3} mg/dL per U\n" +
                $"Equilibrium Y? = {result.Y0:F0} mg/dL\n\n" +
                $"Delays: CHO = {result.Delay1Seconds / 60:F0} min, Insulin = {result.Delay2Seconds / 60:F0} min\n\n" +
                $"Fit: R² = {result.RSquared:F3}, RMSE = {result.RMSE:F1} mg/dL",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Identification failed: {ex.Message}", "OK");
            lblStatus.Text = $"Error: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"MIMO Identification Error: {ex}");
        }
        finally
        {
            btnIdentify3.IsEnabled = true;
        }
    }
}
