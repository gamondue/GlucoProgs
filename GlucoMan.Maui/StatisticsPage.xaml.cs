using GlucoMan.BusinessLayer;
using GlucoMan;
using gamon;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class StatisticsPage : ContentPage
{
    private string _dataType;
    private DateTime _dateFrom;
    private DateTime _dateTo;
    private BL_BolusesAndInjections _blInjections;

    public StatisticsPage(DateTime dateFrom, DateTime dateTo)
    {
        InitializeComponent();
        //////_dataType = GetSelectedDataType(); ;
        _dateFrom = dateFrom;
        _dateTo = dateTo;
        _blInjections = new BL_BolusesAndInjections();


        // Get selected data type
        //////string dataType = GetSelectedDataType();
        //////////lblDataType.Text = $"Data Type: {dataType}";
        lblDateRange.Text = $"From: {dateFrom:dd/MM/yyyy} - To: {dateTo:dd/MM/yyyy}";

        // Calculate and display statistics
        CalculateAndDisplayStatistics();

        ////////// !!!! 
        ////////_dateTo = DateTime.Now.AddMonths(-15);
        ////////_dateFrom = _dateTo.AddMonths(-12);
    }

    private void CalculateAndDisplayStatistics()
    {
        try
        {
            // Determine which type of insulin to retrieve based on data type
            Common.TypeOfInsulinAction insulinType = DetermineInsulinType();

            if (insulinType == Common.TypeOfInsulinAction.NotSet)
            {
                lblMeanValue.Text = "N/A";
                lblStdDeviation.Text = "N/A";
                lblSampleCount.Text = "0";
                return;
            }

            // Get injections for the specified time range and insulin type
            var injections = _blInjections.GetInjections(_dateFrom, _dateTo, insulinType);

            if (injections == null || injections.Count == 0)
            {
                lblMeanValue.Text = "No data";
                lblStdDeviation.Text = "No data";
                lblSampleCount.Text = "0";
                return;
            }

            // Extract insulin values (filter out null values)
            var insulinValues = injections
                .Where(i => i.InsulinValue?.Double.HasValue == true)
                .Select(i => i.InsulinValue.Double.Value)
                .ToList();

            if (insulinValues.Count == 0)
            {
                lblMeanValue.Text = "No valid values";
                lblStdDeviation.Text = "No valid values";
                lblSampleCount.Text = "0";
                return;
            }

            // Calculate mean
            double mean = insulinValues.Average();

            // Calculate standard deviation
            double sumOfSquaredDifferences = insulinValues.Sum(val => Math.Pow(val - mean, 2));
            double variance = sumOfSquaredDifferences / insulinValues.Count;
            double stdDeviation = Math.Sqrt(variance);

            // Display results
            lblMeanValue.Text = $"{mean:F2} U";
            lblStdDeviation.Text = $"{stdDeviation:F2} U";
            lblSampleCount.Text = $"{insulinValues.Count}";
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateAndDisplayStatistics", ex);
            lblMeanValue.Text = "Error";
            lblStdDeviation.Text = "Error";
            lblSampleCount.Text = "0";
        }
    }

    private Common.TypeOfInsulinAction DetermineInsulinType()
    {
        ////////// Map the data type string to insulin action type
        ////////if (_dataType.Contains("Short", StringComparison.OrdinalIgnoreCase))
        ////////{
        ////////    return Common.TypeOfInsulinAction.Short;
        ////////}
        ////////else if (_dataType.Contains("Long", StringComparison.OrdinalIgnoreCase))
        ////////{
        ////////    return Common.TypeOfInsulinAction.Long;
        ////////}
        ////////else if (_dataType.Contains("Rapid", StringComparison.OrdinalIgnoreCase))
        ////////{
        ////////    return Common.TypeOfInsulinAction.Rapid;
        ////////}
        
        return Common.TypeOfInsulinAction.NotSet;
    }

    private void btnIdentify_Click(object sender, EventArgs e)
    {

    }
    ////////private string GetSelectedDataType()
    ////////{
    ////////    if (rbGlucose.IsChecked) return AppStrings.Glucose;
    ////////    if (rbShortInsulin.IsChecked) return AppStrings.ShortInsulin;
    ////////    if (rbLongInsulin.IsChecked) return AppStrings.LongInsulin;
    ////////    if (rbFoods.IsChecked) return AppStrings.Foods;
    ////////    if (rbMeals.IsChecked) return AppStrings.Meals;
    ////////    if (rbRecipes.IsChecked) return AppStrings.Recipes;

    ////////    return AppStrings.Glucose; // Default
    ////////}
}
