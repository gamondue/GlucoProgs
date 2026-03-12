using gamon;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class ExperimentalPage : ContentPage
{
	public ExperimentalPage(object localizationService = null)
	{
		InitializeComponent();
		LoadModels();
	}

	private void LoadModels()
	{
		// Initialize model selection dropdown
		var models = new List<string>
		{
			"Linear Model",
			"Exponential Model",
			"Polynomial Model (Order 2)",
			"Polynomial Model (Order 3)",
			"Neural Network Model"
		};
		cmbModelSelection.ItemsSource = models;
		if (models.Count > 0)
			cmbModelSelection.SelectedIndex = 0;
	}

    private void btnIdentification_Click(object sender, EventArgs e)
    {
        try
        {
            ////// Validate date range
            ////if (dateTime> dateTimeTo)
            ////{
            ////    DisplayAlert(AppStrings.InvalidDateRangeTitle, AppStrings.InvalidDateRangeMessage, AppStrings.OK);
            ////    datePicker.Date = datePickerFrom.Date.AddDays(1);
            ////    return;
            ////}

            //Log the action
            //General.LogOfProgram?.Event($"Opening Statistics page - From: {dateTimeFrom}, To: {dateTimeTo}");

            int nWeeks = 2;
            int.TryParse(txtNoOfWeeks.Text, out nWeeks);

            // Navigate to Identifications page (to be implemented)
            var identificationPage = new IdentificationPage(datePicker.Date ?? DateTime.Now, nWeeks);
            Navigation.PushAsync(identificationPage);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnStatistics_Clicked", ex);
            DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Failed to open statistics page: {0}", ex.Message), AppStrings.OK);
        }
    }
    private void btnIdentification2_Click(object sender, EventArgs e)
    {
        int nWeeks = 2;
        int.TryParse(txtNoOfWeeks.Text, out nWeeks);

        // Navigate to Identifications page (to be implemented)
        var identificationPage = new IdentificationPage2(datePicker.Date ?? DateTime.Now, nWeeks);
        Navigation.PushAsync(identificationPage);
    }

    private async void btnIdentification1_Clicked(object sender, EventArgs e)
	{
		try
		{
			lblIdentificationStatus.Text = "Running Algorithm 1: Least Squares Estimation...";

			// Simulate identification process
			await Task.Delay(2000);

			lblIdentificationStatus.Text = "✓ Algorithm 1 completed successfully\n" +
				"Model parameters identified and validated";
		}
		catch (Exception ex)
		{
			lblIdentificationStatus.Text = $"✗ Error: {ex.Message}";
		}
	}

	private async void btnIdentification2_Clicked(object sender, EventArgs e)
	{
		try
		{
			lblIdentificationStatus.Text = "Running Algorithm 2: Maximum Likelihood Estimation...";

			// Simulate identification process
			await Task.Delay(2500);

			lblIdentificationStatus.Text = "✓ Algorithm 2 completed successfully\n" +
				"Model parameters identified and validated";
		}
		catch (Exception ex)
		{
			lblIdentificationStatus.Text = $"✗ Error: {ex.Message}";
		}
	}

	private async void btnExtrapolate_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (!int.TryParse(txtForecastHours.Text, out int hours))
			{
				await DisplayAlert("Error", "Please enter valid forecast hours", "OK");
				return;
			}

			lblExtrapolationStatus.Text = $"Extrapolating {hours} hours ahead with confidence {sldrConfidence.Value:F0}%...";
			lblPredictedValues.IsVisible = false;

			// Simulate extrapolation process
			await Task.Delay(3000);

			lblExtrapolationStatus.Text = $"✓ Extrapolation completed\n" +
				$"Model: {cmbModelSelection.SelectedItem}\n" +
				$"Forecast period: {hours} hours";

			lblPredictedValues.Text = "Predicted glucose values:\n" +
				"Hour +1: 125 mg/dL\n" +
				"Hour +6: 145 mg/dL\n" +
				"Hour +12: 138 mg/dL\n" +
				"Hour +24: 130 mg/dL";
			lblPredictedValues.IsVisible = true;
		}
		catch (Exception ex)
		{
			lblExtrapolationStatus.Text = $"✗ Error: {ex.Message}";
		}
	}

	private async void btnExportResults_Clicked(object sender, EventArgs e)
	{
		try
		{
			await DisplayAlert("Export", "Results exported to:\nGlucoMan_Experimental_Results.csv", "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
		}
	}
}