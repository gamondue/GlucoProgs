using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.Controls;

namespace GlucoMan.Maui;

public partial class StatisticsAndGraphPage : ContentPage
{
    private BL_ImportData bl = new BL_ImportData();
    
    public StatisticsAndGraphPage()
    {
        InitializeComponent();
        
        // Initialize date pickers with default values
        InitializeDatePickers();
    }

    private void InitializeDatePickers()
    {
        // Set default dates: To = now, From = 2 weeks before
        datePickerTo.Date = DateTime.Now.Date;
        // timePickerTo.Time = DateTime.Now.TimeOfDay; // Temporarily commented - using date only

        // !!!! temporary !!!!
        datePickerTo.Date = DateTime.Now.AddMonths(-10).Date;

        datePickerFrom.Date = datePickerTo.Date.AddDays(-14); // 2 weeks = 14 days

        // !!!! temporary !!!!
        datePickerFrom.Date = datePickerTo.Date.AddMonths(-12);
    }

    private async void btnStatistics_Clicked(object sender, TappedEventArgs e)
    {
        try
        {
            // Get selected data type
            string dataType = GetSelectedDataType();
            
            // Get date range (using midnight for From, end of day for To)
            DateTime dateTimeFrom = datePickerFrom.Date; // Midnight of selected day
            DateTime dateTimeTo = datePickerTo.Date.AddDays(1).AddSeconds(-1); // 23:59:59 of selected day
            
            // Validate date range
            if (dateTimeFrom > dateTimeTo)
            {
                await DisplayAlert(AppStrings.InvalidDateRangeTitle, AppStrings.InvalidDateRangeMessage, AppStrings.OK);
                datePickerTo.Date = datePickerFrom.Date.AddDays(1);
                return;
            }
            
            // Log the action
            General.LogOfProgram?.Event($"Opening Statistics page - Type: {dataType}, From: {dateTimeFrom}, To: {dateTimeTo}");
            
            // Navigate to Statistics page (to be implemented)
            var statisticsPage = new StatisticsPage(dataType, dateTimeFrom, dateTimeTo);
            await Navigation.PushAsync(statisticsPage);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnStatistics_Clicked", ex);
            await DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Failed to open statistics page: {0}", ex.Message), AppStrings.OK);
        }
    }

    private async void btnChart_Clicked(object sender, TappedEventArgs e)
    {
        try
        {
            // Get selected data type
            string dataType = GetSelectedDataType();
   
            // Get date range (using only dates, not times)
            DateTime dateTimeFrom = datePickerFrom.Date; // Midnight of selected day
            DateTime dateTimeTo = datePickerTo.Date.AddDays(1).AddSeconds(-1); // 23:59:59 of selected day
      
            // Log the action
            General.LogOfProgram?.Event($"Opening Chart page - Type: {dataType}, From: {dateTimeFrom}, To: {dateTimeTo}");
            
            // Navigate to Chart page (to be implemented)
            var chartPage = new ChartPage(dataType, dateTimeTo);
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
            await DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Failed to import glucose data: {0}", ex.Message), AppStrings.OK);
        }
    }

    private async Task ImportSensorDataFromCsvFile()
    {
        try
        {
            // Use FilePicker to select the database file (to get the folder location)
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/x-sqlite3", "application/octet-stream", ".csv"} },
                { DevicePlatform.iOS, new[] { "public.data", ".csv" } },
                { DevicePlatform.WinUI, new[] { ".csv" } },
                { DevicePlatform.MacCatalyst, new[] { ".csv" } }
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

            // Read and parse the CSV file, save in the database the imported data
            string summaryString = await bl.ImportDataFromFreeStyleLibre(picked.FullPath);
            
            await DisplayAlert(AppStrings.ImportFinishedTitle, summaryString, AppStrings.OK);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ImportSensorDataFromCsvFile", ex);
            await DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Error importing sensor data: {0}", ex.Message), AppStrings.OK);
        }
    }   
    /// <summary>
    /// Gets the selected data type from radio buttons
    /// </summary>
    /// <returns>String representing the selected data type</returns>
    private string GetSelectedDataType()
    {
        if (rbGlucose.IsChecked) return AppStrings.Glucose;
        if (rbShortInsulin.IsChecked) return AppStrings.ShortInsulin;
        if (rbLongInsulin.IsChecked) return AppStrings.LongInsulin;
        if (rbFoods.IsChecked) return AppStrings.Foods;
        if (rbMeals.IsChecked) return AppStrings.Meals;
        if (rbRecipes.IsChecked) return AppStrings.Recipes;
        
        return AppStrings.Glucose; // Default
    }
}
