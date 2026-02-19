using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
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
        lblDateRange.Text = string.Format(AppStrings.DateRangeFromTo, _dateFrom.ToString("dd/MM/yyyy"), _dateTo.ToString("dd/MM/yyyy"));
    }

    private async void btnIdentify3_Click(object sender, EventArgs e)
    {
        try
        {
            // Disable button during processing
            btnIdentify3.IsEnabled = false;
            lblStatus.Text = "Identifying MIMO model...";

            // Get Data from business layer
            var blMeals = new BL_MealAndFood();
            var blGlucose = new BL_GlucoseMeasurements();
            var blInj = new BL_BolusesAndInjections();

            var meals = blMeals.GetMeals(_dateFrom, _dateTo);
            var injections = blInj.GetQuickInjections(_dateFrom, _dateTo);
            var glucose = blGlucose.GetSensorsRecords(_dateFrom, _dateTo);

            // Validate Data availability
            if (glucose == null || glucose.Count < 10)
            {
                await DisplayAlert(AppStrings.Error, AppStrings.InsufficientGlucoseData, AppStrings.OK);
                lblStatus.Text = AppStrings.IdentificationFailedInsufficientData;
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
                await DisplayAlert(AppStrings.Error, AppStrings.IdentificationFailedCheckData, AppStrings.OK);
                lblStatus.Text = AppStrings.IdentificationFailed;
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
            await DisplayAlert(AppStrings.MIMOIdentificationComplete,
                string.Format(AppStrings.MIMOIdentificationCompleteMessage,
                    result.Tau / 60,
                    result.K1,
                    result.K2,
                    result.Y0,
                    result.Delay1Seconds / 60,
                    result.Delay2Seconds / 60,
                    result.RSquared,
                    result.RMSE),
                AppStrings.OK);
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppStrings.Error, string.Format(AppStrings.IdentificationFailedWithMessage, ex.Message), AppStrings.OK);
            lblStatus.Text = string.Format(AppStrings.IdentificationFailedWithMessage, ex.Message);
            System.Diagnostics.Debug.WriteLine($"MIMO Identification Error: {ex}");
        }
        finally
        {
            btnIdentify3.IsEnabled = true;
        }
    }
}

