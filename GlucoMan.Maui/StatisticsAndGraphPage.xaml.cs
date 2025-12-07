using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.Controls;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

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
        datePicker.Date = DateTime.Now.Date;
        // timePickerTo.Time = DateTime.Now.TimeOfDay; // Temporarily commented - using date only

    }

    private async void btnChart_Clicked(object sender, TappedEventArgs e)
    {
        try
        {
   
            // Get date range (using only dates, not times)
            DateTime date = datePicker.Date;
      
            // Log the action
            ////////General.LogOfProgram?.Event($"Opening Chart page - From: {dateTimeFrom}, To: {date}");
            
            // Navigate to Chart page (to be implemented)
            var chartPage = new ChartPage(date);
            await Navigation.PushAsync(chartPage);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnChart_Clicked", ex);
            await DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Failed to open chart page: {0}", ex.Message), AppStrings.OK);
        }
    }
    
    private async void btnImportGlucose_Clicked(object sender, EventArgs e)
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

    private void btnStatistics_Clicked(object sender, TappedEventArgs e)
    {
        int nWeeks = 2;
        int.TryParse(txtNoOfWeeks.Text, out nWeeks);
        // Navigate to Chart page (to be implemented)
        var statisticsPage = new StatisticsPage(datePicker.Date.AddDays(-7 * nWeeks), datePicker.Date);
        Navigation.PushAsync(statisticsPage);
    }

    private void btnIdentification_Click(object sender, EventArgs e)
    {
        try
        {
            ////////// Get date range (using midnight for From, end of day for To)
            ////////DateTime dateTimeFrom = datePickerFrom.Date; // Midnight of selected day
            ////////DateTime dateTimeTo = datePicker.Date.AddDays(1).AddSeconds(-1); // 23:59:59 of selected day

            DateTime dateTimeTo = datePicker.Date.AddDays(1).AddSeconds(-1); // 23:59:59 of selected day

            ////////// Validate date range
            ////////if (dateTimeFrom > dateTimeTo)
            ////////{
            ////////    DisplayAlert(AppStrings.InvalidDateRangeTitle, AppStrings.InvalidDateRangeMessage, AppStrings.OK);
            ////////    datePicker.Date = datePickerFrom.Date.AddDays(1);
            ////////    return;
            ////////}

            // Log the action
            //////////General.LogOfProgram?.Event($"Opening Statistics page - From: {dateTimeFrom}, To: {dateTimeTo}");

            // Navigate to Statistics page (to be implemented)
            //////////var statisticsPage = new IdentificationPage(dateTimeFrom, dateTimeTo);
            //////////Navigation.PushAsync(statisticsPage);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsAndGraphPage - btnStatistics_Clicked", ex);
            DisplayAlert(AppStrings.ImportErrorTitle, string.Format("Failed to open statistics page: {0}", ex.Message), AppStrings.OK);
        }
    }
}
