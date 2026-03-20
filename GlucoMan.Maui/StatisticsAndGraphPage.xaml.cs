using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.Controls;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace GlucoMan.Maui;

public partial class StatisticsAndGraphPage : ContentPage
{
    private BL_ImportData bl = new BL_ImportData();
    bool processingLongCalculations = false;

    public StatisticsAndGraphPage()
    {
        InitializeComponent();

        // Set default date To = now - 1 day (because we do not have full data about today),
        datePicker.Date = DateTime.Today.AddDays(-1);
    }
    private async void btnChart_Clicked(object sender, TappedEventArgs e)
    {
        try
        {
   
            // Get date range (using only dates, not times)
            DateTime date = datePicker.Date ?? DateTime.Now;
      
            // Log the action
            ////////General.LogOfProgram?.Event($"Opening Chart page - From: {dateTimeFrom}, To: {date}");
            
            // Navigate to Chart page
            var chartPage = new ChartPage(date);
            await Navigation.PushAsync(chartPage);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnChart_Clicked", ex);
            await DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Failed to open chart page: {0}", ex.Message), AppStrings.OK);
        }
    }
    private async void btnImportGlucose_Clicked(object sender, TappedEventArgs e)
    {
        if (processingLongCalculations)
        {
            Console.Beep();
            return;
        }
        processingLongCalculations = true;
        try
        {
            // Prompt user confirmation
            bool import = await DisplayAlert(AppStrings.ImportGlucoseConfirmTitle,
                AppStrings.ImportGlucoseConfirmMessage,
                AppStrings.Yes, AppStrings.No);

            if (!import)
                return;

            General.LogOfProgram?.Event("Import glucose data from sensor - starting file selection");

            await ImportSensorDataFromCsvFile();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnImportGlucose_Clicked", ex);
            await DisplayAlert(General.ReplaceNewLine(AppStrings.ImportErrorTitle), 
                string.Format("Failed to import glucose data: {0}", ex.Message), AppStrings.OK);
        }
        processingLongCalculations = true;
    }
    private async Task ImportSensorDataFromCsvFile()
    {
        try
        {
            // Use FilePicker to select the database file (to get the folder location)
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "text/csv", "text/comma-separated-values", "application/csv" } },
                { DevicePlatform.iOS, new[] { "public.comma-separated-values-text" } },
                { DevicePlatform.WinUI, new[] { ".csv" } },
                { DevicePlatform.MacCatalyst, new[] { "public.comma-separated-values-text" } }
            });

            var picked = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = AppStrings.ImportGlucoseConfirmTitle,
                FileTypes = customFileType
            });

            if (picked is null)
            {
                General.LogOfProgram?.Debug("File selection cancelled by user");
                return;
            }

            General.LogOfProgram?.Debug($"Selected database file: {picked.FileName} (Full path: {picked.FullPath})");

            var fileInfo = new FileInfo(picked.FullPath);
            General.LogOfProgram?.Debug($"CSV file found. Size: {fileInfo.Length} bytes");

            // Read and parse the CSV file, save in the database the imported Data
            string summaryString = await bl.ImportDataFromFreeStyleLibre(picked.FullPath);
            
            await DisplayAlert(General.ReplaceNewLine(AppStrings.ImportFinishedTitle), 
                summaryString, AppStrings.OK);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ImportSensorDataFromCsvFile", ex);
            await DisplayAlert(General.ReplaceNewLine(AppStrings.ImportErrorTitle), 
                string.Format("Error importing sensor data: {0}", ex.Message), AppStrings.OK);
        }
    }
    /// <summary>
    /// Gets the selected Data type from radio buttons
    /// </summary>
    /// <returns>String representing the selected Data type</returns>
    private void btnStatistics_Clicked(object sender, TappedEventArgs e)
    {
        if (processingLongCalculations)
        {
            Console.Beep();
            return;
        }
        processingLongCalculations = true;
        int nWeeks = 2;
        int.TryParse(txtNoOfWeeks.Text, out nWeeks);
        var statisticsPage = new StatisticsPage((datePicker.Date ?? DateTime.Now).AddDays(-7 * nWeeks), datePicker.Date ?? DateTime.Now);
        Navigation.PushAsync(statisticsPage);
        processingLongCalculations = false;
    }
    private async void btnCureChoMismatchInMeals_Clicked(object sender, EventArgs e)
    {
        try
        {
            var blMeals = new BL_MealAndFood();
            var allMeals = blMeals.GetMeals(null, null);
            int nChoDifferentMeals = 0;

            // DETECTION: count meals with CHO mismatch (do NOT save yet)
            foreach (var meal in allMeals)
            {
                blMeals.Meal = meal;
                blMeals.GetFoodsInMeal(meal.IdMeal);

                double? storedCho = meal.CarbohydratesGrams.Double;
                double? recalcCho = blMeals.RecalcTotalCho();
                meal.CarbohydratesGrams.Double = storedCho; // restore original value

                if (Math.Abs((storedCho ?? 0) - (recalcCho ?? 0)) > 0.01 &&
                    recalcCho > 0)
                {
                    nChoDifferentMeals++;
                }
            }

            // Ask user only if there are mismatches
            if (nChoDifferentMeals > 0)
            {
                bool result = await DisplayAlert("Outliers", 
                    $"Meals with CHO mismatch: {nChoDifferentMeals}\nDo you want to substitute all stored meals with calculated?", 
                    AppStrings.Yes, AppStrings.No);

                // REPAIR: execute ONLY if user answered Yes
                if (result)
                {
                    foreach (var meal in allMeals)
                    {
                        blMeals.Meal = meal;
                        blMeals.GetFoodsInMeal(meal.IdMeal);

                        double? storedCho = meal.CarbohydratesGrams.Double;
                        double? recalcCho = blMeals.RecalcTotalCho();
                        meal.CarbohydratesGrams.Double = storedCho; // restore original

                        if (Math.Abs((storedCho ?? 0) - (recalcCho ?? 0)) > 0.01 &&
                            recalcCho > 0)
                        {
                            meal.CarbohydratesGrams.Double = recalcCho;
                            blMeals.SaveOneMeal(meal, false);
                        }
                    }
                    await DisplayAlert("Outliers", $"Corrected {nChoDifferentMeals} meals.", AppStrings.OK);
                }
            }
            else
            {
                await DisplayAlert("Outliers", "No meals with CHO mismatch found.", AppStrings.OK);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnCureChoMismatchInMeals_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }

    private void btnCureWrongLongTermInsulinStorage_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnNoTimeInsulin_Clicked(object sender, EventArgs e)
    {
        try
        {
            var blInj = new BL_BolusesAndInjections();
            var nullTimeInjections = blInj.GetInjectionsWithNullTime();

            if (nullTimeInjections == null || nullTimeInjections.Count == 0)
            {
                await DisplayAlert("No Time Injections", "No injections with missing time found.", AppStrings.OK);
                return;
            }

            foreach (var injection in nullTimeInjections)
            {
                // Get the previous injection (with the preceding Id)
                Injection previousInjection = null;
                if (injection.IdInjection > 1)
                {
                    previousInjection = blInj.GetOneInjection(injection.IdInjection - 1);
                }

                if (previousInjection == null || previousInjection.EventTime?.DateTime.HasValue != true)
                {
                    await DisplayAlert("No Time Injections",
                        $"Injection ID {injection.IdInjection}: no valid previous record found.",
                        AppStrings.OK);
                    continue;
                }

                // Navigate to ChartPage for the date of the previous injection
                DateTime previousDate = previousInjection.EventTime.DateTime.Value;
                var chartPage = new ChartPage(previousDate);
                await Navigation.PushAsync(chartPage);

                // Show overlay message with injection info
                await chartPage.DisplayAlert("No Time Injection",
                    $"Null-time Injection ID: {injection.IdInjection}\n" +
                    $"Previous Injection ID: {previousInjection.IdInjection}\n" +
                    $"Previous Timestamp: {previousDate:yyyy-MM-dd HH:mm:ss}",
                    AppStrings.OK);

                // Go back from ChartPage before showing next
                await Navigation.PopAsync();
            }

            await DisplayAlert("No Time Injections",
                $"Review completed. {nullTimeInjections.Count} injections with missing time found.",
                AppStrings.OK);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnNoTimeInsulin_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }

    private async void btnDetectTooNearInsulinInjections_Clicked(object sender, EventArgs e)
    {
        try
        {
            var blInj = new BL_BolusesAndInjections();
            var allInjections = blInj.GetInjections(new DateTime(2000, 1, 1), DateTime.Now);

            if (allInjections == null || allInjections.Count == 0)
            {
                await DisplayAlert("Too Close Injections", "No injections found.", AppStrings.OK);
                return;
            }

            // Group by insulin action type, only valid timestamps and known types
            var byType = allInjections
                .Where(inj => inj.IdTypeOfInsulinAction.HasValue &&
                              inj.IdTypeOfInsulinAction != (int)Common.TypeOfInsulinAction.NotSet &&
                              inj.EventTime?.DateTime.HasValue == true)
                .GroupBy(inj => inj.IdTypeOfInsulinAction)
                .ToList();

            TimeSpan threshold = TimeSpan.FromMinutes(15);

            // DETECTION: collect the triplets (prev, too-close, next)
            var tooCloseList = new List<(Injection Prev, Injection Curr, Injection Next)>();

            foreach (var group in byType)
            {
                var sorted = group.OrderBy(inj => inj.EventTime.DateTime.Value).ToList();

                for (int i = 1; i < sorted.Count - 1; i++)
                {
                    TimeSpan gapFromPrev = sorted[i].EventTime.DateTime.Value
                                        - sorted[i - 1].EventTime.DateTime.Value;
                    TimeSpan gapToNext  = sorted[i + 1].EventTime.DateTime.Value
                                        - sorted[i].EventTime.DateTime.Value;

                    if (gapFromPrev < threshold && gapToNext < threshold)
                        tooCloseList.Add((sorted[i - 1], sorted[i], sorted[i + 1]));
                }
            }

            await DisplayAlert("Too Close Injections",
                $"Injections less than 15 min from both adjacent same-type injections: {tooCloseList.Count}",
                AppStrings.OK);

            // RECOVERY: let the user review each one on the chart
            if (tooCloseList.Count > 0)
            {
                bool review = await DisplayAlert("Too Close Injections",
                    "Do you want to review each injection on the chart?",
                    AppStrings.Yes, AppStrings.No);

                if (review)
                {
                    for (int i = 0; i < tooCloseList.Count; i++)
                    {
                        var (prev, curr, next) = tooCloseList[i];
                        DateTime chartDate = curr.EventTime.DateTime.Value;

                        var chartPage = new ChartPage(chartDate);
                        await Navigation.PushAsync(chartPage);

                        await chartPage.DisplayAlert("Too Close Injection",
                            $"({i + 1} of {tooCloseList.Count}) " +
                            $"Injection ID: {curr.IdInjection}\n" +
                            $"Timestamp:    {chartDate:yyyy-MM-dd HH:mm:ss}\n" +
                            $"Insulin:      {curr.InsulinValue?.Double} IU\n" +
                            $"Type:         {curr.TypeOfInsulinAction}\n" +
                            $"Prev ID: {prev.IdInjection}  at {prev.EventTime.DateTime.Value:HH:mm:ss}" +
                            $"  (gap: {(chartDate - prev.EventTime.DateTime.Value).TotalMinutes:F0} min)\n" +
                            $"Next ID: {next.IdInjection}  at {next.EventTime.DateTime.Value:HH:mm:ss}" +
                            $"  (gap: {(next.EventTime.DateTime.Value - chartDate).TotalMinutes:F0} min)",
                            AppStrings.OK);

                        await Navigation.PopAsync();
                    }

                    await DisplayAlert("Too Close Injections", "Review completed.", AppStrings.OK);
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnDetectTooNearInsulinInjections_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }
}
