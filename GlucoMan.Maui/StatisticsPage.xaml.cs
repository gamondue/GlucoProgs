using gamon;
using GlucoMan;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using Mathematics;
using MathNet.Numerics.Statistics;

namespace GlucoMan.Maui;

public partial class StatisticsPage : ContentPage
{
    private DateTime _dateFrom;
    private DateTime _dateTo;
    private BL_BolusesAndInjections _blInjections;
    private BL_GlucoseMeasurements _blGlucose;
    private BL_MealAndFood _blMealAndFood;

    // Meal time settings from Common
    private double _breakfastStartHour;
    private double _breakfastEndHour;
    private double _lunchStartHour;
    private double _lunchEndHour;
    private double _dinnerStartHour;
    private double _dinnerEndHour;

    public StatisticsPage(DateTime dateFrom, DateTime dateTo)
    {
        InitializeComponent();

        _dateFrom = dateFrom;
        _dateTo = dateTo;
        _blInjections = new BL_BolusesAndInjections();
        _blGlucose = new BL_GlucoseMeasurements();
        _blMealAndFood = new BL_MealAndFood();

        // Load meal time settings from Common
        _breakfastStartHour = Common.breakfastStartHour ?? 6;
        _breakfastEndHour = Common.breakfastEndHour ?? 10;
        _lunchStartHour = Common.lunchStartHour ?? 11;
        _lunchEndHour = Common.lunchEndHour ?? 15;
        _dinnerStartHour = Common.dinnerStartHour ?? 17;
        _dinnerEndHour = Common.dinnerEndHour ?? 21;

        lblDateRange.Text = string.Format(AppStrings.DateRangeLabel, dateFrom.ToString("dd/MM/yyyy"), dateTo.ToString("dd/MM/yyyy"));

        // Calculate and display all statistics
        CalculateAllStatistics();
    }
    private void CalculateAllStatistics()
    {
        try
        {
            CalculateAndShowCarbohydratesStatistics();
            CalculateAndShowInsulinStatistics();
            CalculateAndShuwChoStatistics();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateAllStatistics", ex);
        }
    }
    private void CalculateAndShowCarbohydratesStatistics()
    {
        _blGlucose.GetGlucoseRecordsForStatistics(_dateFrom, _dateTo);
        if (_blGlucose.UsingSensorData)
        {
            lblGlucoseDataSource.Text = AppStrings.GlucoseDataSourceSensors;
            lblGlucoseDataSource.TextColor = Colors.Black;
        }
        else
        {
            lblGlucoseDataSource.Text = AppStrings.GlucoseDataSourceManual;
            lblGlucoseDataSource.TextColor = Colors.Red;
        }
        // Calculate overall blood glucose statistics
        // For time-based glucose, we use all data and filter by time of day
        StatisticsData sd = _blGlucose.CalculateGlucoseStatistics(0, 24);
        DisplayGlucoseStats(sd, lblGlucoseMean, lblGlucoseStdDev, lblGlucoseSamples);
        // Morning: 6:00 - 12:00
        sd = _blGlucose.CalculateGlucoseStatistics(6, 12);
        DisplayGlucoseStats(sd, lblMorningGlucoseMean, lblMorningGlucoseStdDev,
            lblMorningGlucoseSamples);
        // Midday: 12:00 - 18:00
        sd = _blGlucose.CalculateGlucoseStatistics(12, 18);
        DisplayGlucoseStats(sd, lblMiddayGlucoseMean,
            lblMiddayGlucoseStdDev, lblMiddayGlucoseSamples);
        // Evening: 18:00 - 22:00
        sd = _blGlucose.CalculateGlucoseStatistics(18, 22);
        DisplayGlucoseStats(sd, lblEveningGlucoseMean,
            lblEveningGlucoseStdDev, lblEveningGlucoseSamples);
        // Night: 22:00 - 6:00
        sd = _blGlucose.CalculateGlucoseStatistics(22, 6);
        DisplayGlucoseStats(sd, lblNightGlucoseMean,
            lblNightGlucoseStdDev, lblNightGlucoseSamples);
    }
    #region Glucose Statistics
    private void DisplayGlucoseStats(StatisticsData data, 
        Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        if (data == null)
        {
            meanLabel.Text = "No statistics";
            stdDevLabel.Text = "No statistics";
            samplesLabel.Text = "-";
            return;
        }
        //dailyMeanLabel.Text = $"{dailyMean:F1} {data.Daily Mean}";
        //meanLabel.Text = $"{mean:F1} {data.Mean}";
        //stdDevLabel.Text = $"{stdDev:F1} {data.StandardDeviation}";
        //samplesLabel.Text = $"{data.NSamples}";

        meanLabel.Text = $"{data.Mean:F1} mg/dL";
        stdDevLabel.Text = $"{data.StandardDeviation:F1} mg/dL";
        samplesLabel.Text = $"{data.NSamples:F1}";
    }
    #endregion
    #region Insulin Statistics
    private void CalculateAndShowInsulinStatistics()
    {
        try
        {
            _blInjections.GetInjectionsForStatistics(_dateFrom, _dateTo);

            // Display TDD stats (!!!!!!!!!!!! mettere le label giuste !!!!!!!!!)
            StatisticsData sd = _blInjections.CalculateTddInsulin();
            DisplayInsulinStats(sd, lblTddPerDayMean, lblTddMean, lblTddStdDev,
                lblTddSamples);

            // Total Quick Acting (Rapid + Short)
            sd = _blInjections.CalculateTotalQuickActingInsulin();
            DisplayInsulinStats(sd, lblQuickInsulinPerDayMean, lblQuickInsulinMean, lblQuickInsulinStdDev, 
                lblQuickInsulinSamples);
            //var quickActingInjections = new List<Injection>();
            //if (rapidInjections != null) quickActingInjections.AddRange(rapidInjections);
            //if (shortInjections != null) quickActingInjections.AddRange(shortInjections);
            //_blInjections.CalculateAndDisplayInsulinStats(quickActingInjections, lblQuickInsulinMean, lblQuickInsulinStdDev, lblQuickInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(quickActingInjections, lblQuickInsulinPerDayMean);

            // Total Long Acting (Intermediate + Long)
            sd = _blInjections.CalculateTotalLongActingInsulin();
            DisplayInsulinStats(sd, lblLongInsulinPerDayMean, lblLongInsulinMean, lblLongInsulinStdDev, 
                lblLongInsulinSamples);

            //var longActingInjections = new List<Injection>();
            //if (intermediateInjections != null) longActingInjections.AddRange(intermediateInjections);
            //if (longInjections != null) longActingInjections.AddRange(longInjections);
            //_blInjections.CalculateAndDisplayInsulinStats(longActingInjections, lblLongInsulinMean, lblLongInsulinStdDev, lblLongInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(longActingInjections, lblLongInsulinPerDayMean);

            // quick acting at breakfast time
            sd = _blInjections.CalculateRapidActingBreakfast();
            DisplayInsulinStats(sd, lblBreakfastInsulinPerDayMean, lblBreakfastInsulinMean, 
                lblBreakfastInsulinStdDev, lblBreakfastInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(breakfastInsulin, lblBreakfastInsulinPerDayMean);
            //var breakfastInsulin = FilterInjectionsByMealTime(quickActingInjections, _breakfastStartHour, _breakfastEndHour);
            //_blInjections.CalculateAndDisplayInsulinStats(breakfastInsulin, lblBreakfastInsulinMean, lblBreakfastInsulinStdDev, lblBreakfastInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(breakfastInsulin, lblBreakfastInsulinPerDayMean);

            // quick acting at lunch time
            sd = _blInjections.CalculateRapidActingBreakfast();
            DisplayInsulinStats(sd, lblLunchInsulinPerDayMean, lblLunchInsulinMean, 
                lblLunchInsulinStdDev, lblLunchInsulinSamples);

            //var lunchInsulin = FilterInjectionsByMealTime(quickActingInjections, _lunchStartHour, _lunchEndHour);
            //_blInjections.CalculateAndDisplayInsulinStats(lunchInsulin, lblLunchInsulinMean, lblLunchInsulinStdDev, lblLunchInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(lunchInsulin, lblLunchInsulinPerDayMean);

            // quick acting at dinner time
            sd = _blInjections.CalculateRapidActingDinner();
            DisplayInsulinStats(sd, lblDinnerInsulinPerDayMean, lblDinnerInsulinMean, 
                lblDinnerInsulinStdDev, lblDinnerInsulinSamples);

            //var dinnerInsulin = FilterInjectionsByMealTime(quickActingInjections, _dinnerStartHour, _dinnerEndHour);
            //_blInjections.CalculateAndDisplayInsulinStats(dinnerInsulin, lblDinnerInsulinMean, lblDinnerInsulinStdDev, lblDinnerInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(dinnerInsulin, lblDinnerInsulinPerDayMean);

            // quick acting not in breakfast, lunch  nor dinner time
            sd = _blInjections.CalculateRapidActingOtherTimes();
            DisplayInsulinStats(sd, lblOtherInsulinPerDayMean, lblOtherInsulinMean, 
                lblOtherInsulinStdDev, lblOtherInsulinSamples);

            //_blInjections.CalculateAndDisplayInsulinStats(otherInsulin, lblOtherInsulinMean, lblOtherInsulinStdDev, lblOtherInsulinSamples);
            //_blInjections.CalculateAndDisplayInsulinPerDayStats(otherInsulin, lblOtherInsulinPerDayMean);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateInsulinStatistics", ex);
        }
    }

    private void DisplayInsulinStats(StatisticsData sd, Label lblPerDayMean,
        Label lblMean, Label lblStdDev, Label lblNSamples)
    {
        if (sd == null || sd.NSamples == 0)
        {
            lblPerDayMean.Text = "No data";
            //SetNoDataLabels(lblTddMean, lblTddStdDev, lblTddSamples);
            return;
        }

        //var dailyTotals = injections
        //    .Where(i => i.EventTime?.DateTime != null && i.InsulinValue?.Double.HasValue == true)
        //    .GroupBy(i => i.EventTime.DateTime.Value.Date)
        //    .Select(g => g.Sum(i => i.InsulinValue.Double.Value))
        //    .ToList();

        //if (dailyTotals.Count == 0)
        //{
        //    lblTddPerDayMean.Text = "No data";
        //    SetNoDataLabels(lblTddMean, lblTddStdDev, lblTddSamples);
        //    return;
        //}
        lblPerDayMean.Text = $"{sd.DailyMean:F2} U/day";
        lblMean.Text = $"{sd.Mean:F2} U";
        lblStdDev.Text = $"{sd.StandardDeviation:F2} U";
        lblNSamples.Text = $"{sd.NSamples}";
    }
    #endregion
    #region CHO Statistics
    private void CalculateAndShuwChoStatistics()
    {
        try
        {
            // Get all meals in the time range
            var meals = _blMealAndFood.GetMeals(_dateFrom, _dateTo);

            if (meals == null || meals.Count == 0)
            {
                SetNoDataLabels(lblTotalChoMean, lblTotalChoStdDev, lblTotalChoSamples);
                lblTotalChoPerDayMean.Text = "No data";
                SetNoDataLabels(lblBreakfastChoMean, lblBreakfastChoStdDev, lblBreakfastChoSamples);
                lblBreakfastChoPerDayMean.Text = "No data";
                SetNoDataLabels(lblLunchChoMean, lblLunchChoStdDev, lblLunchChoSamples);
                lblLunchChoPerDayMean.Text = "No data";
                SetNoDataLabels(lblDinnerChoMean, lblDinnerChoStdDev, lblDinnerChoSamples);
                lblDinnerChoPerDayMean.Text = "No data";
                SetNoDataLabels(lblOtherChoMean, lblOtherChoStdDev, lblOtherChoSamples);
                lblOtherChoPerDayMean.Text = "No data";
                return;
            }

            // Total Day CHO - calculate daily totals first, then average
            CalculateTotalDayChoStats(meals);

            // Filter meals by type or time
            var breakfastMeals = FilterMealsByMealTime(meals, _breakfastStartHour, _breakfastEndHour);
            CalculateAndDisplayChoStats(breakfastMeals, lblBreakfastChoMean, lblBreakfastChoStdDev, lblBreakfastChoSamples);
            CalculateAndDisplayChoPerDayStats(breakfastMeals, lblBreakfastChoPerDayMean);

            var lunchMeals = FilterMealsByMealTime(meals, _lunchStartHour, _lunchEndHour);
            CalculateAndDisplayChoStats(lunchMeals, lblLunchChoMean, lblLunchChoStdDev, lblLunchChoSamples);
            CalculateAndDisplayChoPerDayStats(lunchMeals, lblLunchChoPerDayMean);

            var dinnerMeals = FilterMealsByMealTime(meals, _dinnerStartHour, _dinnerEndHour);
            CalculateAndDisplayChoStats(dinnerMeals, lblDinnerChoMean, lblDinnerChoStdDev, lblDinnerChoSamples);
            CalculateAndDisplayChoPerDayStats(dinnerMeals, lblDinnerChoPerDayMean);

            // Other CHO: meals not in breakfast, lunch, or dinner time
            var otherMeals = meals.Where(m =>
            {
                if (m.EventTime?.DateTime == null) return true;
                double hour = m.EventTime.DateTime.Value.Hour + m.EventTime.DateTime.Value.Minute / 60.0;
                bool isBreakfast = hour >= _breakfastStartHour && hour < _breakfastEndHour;
                bool isLunch = hour >= _lunchStartHour && hour < _lunchEndHour;
                bool isDinner = hour >= _dinnerStartHour && hour < _dinnerEndHour;
                return !isBreakfast && !isLunch && !isDinner;
            }).ToList();
            CalculateAndDisplayChoStats(otherMeals, lblOtherChoMean, lblOtherChoStdDev, lblOtherChoSamples);
            CalculateAndDisplayChoPerDayStats(otherMeals, lblOtherChoPerDayMean);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateChoStatistics", ex);
            SetErrorLabels(lblTotalChoMean, lblTotalChoStdDev, lblTotalChoSamples);
        }
    }
    private void CalculateTotalDayChoStats(List<Meal> meals)
    {
        // Group meals by day and sum CHO for each day
        var dailyTotals = meals
            .Where(m => m.EventTime?.DateTime != null && m.CarbohydratesGrams?.Double.HasValue == true)
            .GroupBy(m => m.EventTime.DateTime.Value.Date)
            .Select(g => g.Sum(m => m.CarbohydratesGrams.Double.Value))
            .ToList();

        if (dailyTotals.Count == 0)
        {
            SetNoDataLabels(lblTotalChoMean, lblTotalChoStdDev, lblTotalChoSamples);
            lblTotalChoPerDayMean.Text = "No data";
            return;
        }

        var (mean, stdDev, count) = GamonStatistics.MeanAndStdDev(dailyTotals);
        lblTotalChoMean.Text = $"{mean:F1} g";
        lblTotalChoStdDev.Text = $"{stdDev:F1} g";
        lblTotalChoSamples.Text = $"{dailyTotals.Count} days";
        lblTotalChoPerDayMean.Text = $"{mean:F1} g/day";
    }
    private List<Meal> FilterMealsByMealTime(List<Meal> meals, double startHour, double endHour)
    {
        if (meals == null) return new List<Meal>();

        return meals.Where(m =>
        {
            if (m.EventTime?.DateTime == null) return false;
            double hour = m.EventTime.DateTime.Value.Hour + m.EventTime.DateTime.Value.Minute / 60.0;
            return hour >= startHour && hour < endHour;
        }).ToList();
    }
    private void CalculateAndDisplayChoStats(List<Meal> meals, Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        if (meals == null || meals.Count == 0)
        {
            SetNoDataLabels(meanLabel, stdDevLabel, samplesLabel);
            return;
        }

        var values = meals
            .Where(m => m.CarbohydratesGrams?.Double.HasValue == true)
            .Select(m => m.CarbohydratesGrams.Double.Value)
            .ToList();

        if (values.Count == 0)
        {
            meanLabel.Text = "No valid values";
            stdDevLabel.Text = "No valid values";
            samplesLabel.Text = "0";
            return;
        }

        var (mean, stdDev, count) = GamonStatistics.MeanAndStdDev(values);
        meanLabel.Text = $"{mean:F1} g";
        stdDevLabel.Text = $"{stdDev:F1} g";
        samplesLabel.Text = $"{values.Count}";
    }
    private void CalculateAndDisplayChoPerDayStats(List<Meal> meals, Label perDayMeanLabel)
    {
        if (meals == null || meals.Count == 0)
        {
            perDayMeanLabel.Text = "No data";
            return;
        }

        // Group meals by day and sum CHO for each day
        var dailyTotals = meals
            .Where(m => m.EventTime?.DateTime != null && m.CarbohydratesGrams?.Double.HasValue == true)
            .GroupBy(m => m.EventTime.DateTime.Value.Date)
            .Select(g => g.Sum(m => m.CarbohydratesGrams.Double.Value))
            .ToList();

        if (dailyTotals.Count == 0)
        {
            perDayMeanLabel.Text = "No data";
            return;
        }

        double meanPerDay = dailyTotals.Average();
        perDayMeanLabel.Text = $"{meanPerDay:F1} g/day";
    }
    #endregion
    #region Helper Methods
    private void SetErrorLabels(Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        meanLabel.Text = AppStrings.Error;
        stdDevLabel.Text = AppStrings.Error;
        samplesLabel.Text = "0";
    }
    private void SetNoDataLabels(Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        meanLabel.Text = AppStrings.NoData;
        stdDevLabel.Text = AppStrings.NoData;
        samplesLabel.Text = "0";
    }
    private void SetPlaceholderLabels(Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        meanLabel.Text = AppStrings.ComingSoon;
        stdDevLabel.Text = AppStrings.ComingSoon;
        samplesLabel.Text = "--";
    }
    #endregion
}
