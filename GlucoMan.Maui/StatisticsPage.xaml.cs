using GlucoMan.BusinessLayer;
using GlucoMan;
using gamon;

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

        lblDateRange.Text = $"From: {dateFrom:dd/MM/yyyy} - To: {dateTo:dd/MM/yyyy}";

        // Calculate and display all statistics
        CalculateAllStatistics();
    }

    private void CalculateAllStatistics()
    {
        try
        {
            CalculateGlucoseStatistics();
            CalculateInsulinStatistics();
            CalculateChoStatistics();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateAllStatistics", ex);
        }
    }

    #region Glucose Statistics

    private void CalculateGlucoseStatistics()
    {
        try
        {
            // Try to get sensor records first
            var sensorRecords = _blGlucose.GetSensorsRecords(_dateFrom, _dateTo);
            bool usingSensorData = sensorRecords != null && sensorRecords.Count > 0;

            List<GlucoseRecord> glucoseRecords;
            if (usingSensorData)
            {
                glucoseRecords = sensorRecords;
                lblGlucoseDataSource.Text = "data source is sensors' data collection";
                lblGlucoseDataSource.TextColor = Colors.Gray;
            }
            else
            {
                glucoseRecords = _blGlucose.GetGlucoseRecords(_dateFrom, _dateTo);
                lblGlucoseDataSource.Text = "data source is manual data collection";
                lblGlucoseDataSource.TextColor = Colors.Red;
            }

            // Calculate overall blood glucose statistics
            CalculateAndDisplayGlucoseStats(glucoseRecords, lblGlucoseMean, lblGlucoseStdDev, lblGlucoseSamples, "mg/dL");

            // For time-based glucose, we use all records and filter by time of day
            // Morning: 6:00 - 12:00
            var morningRecords = FilterRecordsByTimeOfDay(glucoseRecords, 6, 12);
            CalculateAndDisplayGlucoseStats(morningRecords, lblMorningGlucoseMean, lblMorningGlucoseStdDev, lblMorningGlucoseSamples, "mg/dL");

            // Midday: 12:00 - 18:00
            var middayRecords = FilterRecordsByTimeOfDay(glucoseRecords, 12, 18);
            CalculateAndDisplayGlucoseStats(middayRecords, lblMiddayGlucoseMean, lblMiddayGlucoseStdDev, lblMiddayGlucoseSamples, "mg/dL");

            // Evening: 18:00 - 22:00
            var eveningRecords = FilterRecordsByTimeOfDay(glucoseRecords, 18, 22);
            CalculateAndDisplayGlucoseStats(eveningRecords, lblEveningGlucoseMean, lblEveningGlucoseStdDev, lblEveningGlucoseSamples, "mg/dL");

            // Night: 22:00 - 6:00
            var nightRecords = FilterRecordsByTimeOfDay(glucoseRecords, 22, 6, isNightPeriod: true);
            CalculateAndDisplayGlucoseStats(nightRecords, lblNightGlucoseMean, lblNightGlucoseStdDev, lblNightGlucoseSamples, "mg/dL");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateGlucoseStatistics", ex);
            SetErrorLabels(lblGlucoseMean, lblGlucoseStdDev, lblGlucoseSamples);
        }
    }

    private List<GlucoseRecord> FilterRecordsByTimeOfDay(List<GlucoseRecord> records, double startHour, double endHour, bool isNightPeriod = false)
    {
        if (records == null) return new List<GlucoseRecord>();

        if (isNightPeriod)
        {
            // Night period crosses midnight (e.g., 22:00 - 6:00)
            return records.Where(r =>
            {
                if (r.EventTime?.DateTime == null) return false;
                double hour = r.EventTime.DateTime.Value.Hour + r.EventTime.DateTime.Value.Minute / 60.0;
                return hour >= startHour || hour < endHour;
            }).ToList();
        }
        else
        {
            return records.Where(r =>
            {
                if (r.EventTime?.DateTime == null) return false;
                double hour = r.EventTime.DateTime.Value.Hour + r.EventTime.DateTime.Value.Minute / 60.0;
                return hour >= startHour && hour < endHour;
            }).ToList();
        }
    }

    private void CalculateAndDisplayGlucoseStats(List<GlucoseRecord> records, Label meanLabel, Label stdDevLabel, Label samplesLabel, string unit)
    {
        if (records == null || records.Count == 0)
        {
            meanLabel.Text = "No data";
            stdDevLabel.Text = "No data";
            samplesLabel.Text = "0";
            return;
        }

        var values = records
            .Where(r => r.GlucoseValue?.Double.HasValue == true)
            .Select(r => r.GlucoseValue.Double.Value)
            .ToList();

        if (values.Count == 0)
        {
            meanLabel.Text = "No valid values";
            stdDevLabel.Text = "No valid values";
            samplesLabel.Text = "0";
            return;
        }

        var (mean, stdDev) = General.CalculateMeanAndStdDev(values);
        meanLabel.Text = $"{mean:F1} {unit}";
        stdDevLabel.Text = $"{stdDev:F1} {unit}";
        samplesLabel.Text = $"{values.Count}";
    }

    #endregion

    #region Insulin Statistics

    private void CalculateInsulinStatistics()
    {
        try
        {
            // Get all injections in the time range
            var allInjections = new List<Injection>();

            // Get Rapid type injections
            var rapidInjections = _blInjections.GetInjections(_dateFrom, _dateTo, Common.TypeOfInsulinAction.Rapid);
            if (rapidInjections != null) allInjections.AddRange(rapidInjections);

            // Get Short type injections
            var shortInjections = _blInjections.GetInjections(_dateFrom, _dateTo, Common.TypeOfInsulinAction.Short);
            if (shortInjections != null) allInjections.AddRange(shortInjections);

            // Get Intermediate type injections
            var intermediateInjections = _blInjections.GetInjections(_dateFrom, _dateTo, Common.TypeOfInsulinAction.Intermediate);
            if (intermediateInjections != null) allInjections.AddRange(intermediateInjections);
            
            // Get Long type injections
            var longInjections = _blInjections.GetInjections(_dateFrom, _dateTo, Common.TypeOfInsulinAction.Long);
            if (longInjections != null) allInjections.AddRange(longInjections);

            CalculateAndDisplayTddStats(allInjections);

            // Total Quick Acting (Rapid + Short)
            var quickActingInjections = new List<Injection>();
            if (rapidInjections != null) quickActingInjections.AddRange(rapidInjections);
            if (shortInjections != null) quickActingInjections.AddRange(shortInjections);
            _blInjections.CalculateAndDisplayInsulinStats(quickActingInjections, lblQuickInsulinMean, lblQuickInsulinStdDev, lblQuickInsulinSamples);
            _blInjections.CalculateAndDisplayInsulinPerDayStats(quickActingInjections, lblQuickInsulinPerDayMean);

            // Total Long Acting (Intermediate + Long)
            var longActingInjections = new List<Injection>();
            if (intermediateInjections != null) longActingInjections.AddRange(intermediateInjections);
            if (longInjections != null) longActingInjections.AddRange(longInjections);
            _blInjections.CalculateAndDisplayInsulinStats(longActingInjections, lblLongInsulinMean, lblLongInsulinStdDev, lblLongInsulinSamples);
            _blInjections.CalculateAndDisplayInsulinPerDayStats(longActingInjections, lblLongInsulinPerDayMean);

            // Filter quick acting by meal time
            var breakfastInsulin = FilterInjectionsByMealTime(quickActingInjections, _breakfastStartHour, _breakfastEndHour);
            _blInjections.CalculateAndDisplayInsulinStats(breakfastInsulin, lblBreakfastInsulinMean, lblBreakfastInsulinStdDev, lblBreakfastInsulinSamples);
            _blInjections.CalculateAndDisplayInsulinPerDayStats(breakfastInsulin, lblBreakfastInsulinPerDayMean);

            var lunchInsulin = FilterInjectionsByMealTime(quickActingInjections, _lunchStartHour, _lunchEndHour);
            _blInjections.CalculateAndDisplayInsulinStats(lunchInsulin, lblLunchInsulinMean, lblLunchInsulinStdDev, lblLunchInsulinSamples);
            _blInjections.CalculateAndDisplayInsulinPerDayStats(lunchInsulin, lblLunchInsulinPerDayMean);

            var dinnerInsulin = FilterInjectionsByMealTime(quickActingInjections, _dinnerStartHour, _dinnerEndHour);
            _blInjections.CalculateAndDisplayInsulinStats(dinnerInsulin, lblDinnerInsulinMean, lblDinnerInsulinStdDev, lblDinnerInsulinSamples);
            _blInjections.CalculateAndDisplayInsulinPerDayStats(dinnerInsulin, lblDinnerInsulinPerDayMean);

            // Other insulin: quick acting not in breakfast, lunch, or dinner time
            var otherInsulin = quickActingInjections.Where(i =>
            {
                if (i.EventTime?.DateTime == null) return true;
                double hour = i.EventTime.DateTime.Value.Hour + i.EventTime.DateTime.Value.Minute / 60.0;
                bool isBreakfast = hour >= _breakfastStartHour && hour < _breakfastEndHour;
                bool isLunch = hour >= _lunchStartHour && hour < _lunchEndHour;
                bool isDinner = hour >= _dinnerStartHour && hour < _dinnerEndHour;
                return !isBreakfast && !isLunch && !isDinner;
            }).ToList();
            _blInjections.CalculateAndDisplayInsulinStats(otherInsulin, lblOtherInsulinMean, lblOtherInsulinStdDev, lblOtherInsulinSamples);
            _blInjections.CalculateAndDisplayInsulinPerDayStats(otherInsulin, lblOtherInsulinPerDayMean);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPage - CalculateInsulinStatistics", ex);
        }
    }

    private List<Injection> FilterInjectionsByMealTime(List<Injection> injections, double startHour, double endHour)
    {
        if (injections == null) return new List<Injection>();

        return injections.Where(i =>
        {
            if (i.EventTime?.DateTime == null) return false;
            double hour = i.EventTime.DateTime.Value.Hour + i.EventTime.DateTime.Value.Minute / 60.0;
            return hour >= startHour && hour < endHour;
        }).ToList();
    }

    private void CalculateAndDisplayTddStats(List<Injection> injections)
    {
        if (injections == null || injections.Count == 0)
        {
            lblTddPerDayMean.Text = "No data";
            SetNoDataLabels(lblTddMean, lblTddStdDev, lblTddSamples);
            return;
        }

        var dailyTotals = injections
            .Where(i => i.EventTime?.DateTime != null && i.InsulinValue?.Double.HasValue == true)
            .GroupBy(i => i.EventTime.DateTime.Value.Date)
            .Select(g => g.Sum(i => i.InsulinValue.Double.Value))
            .ToList();

        if (dailyTotals.Count == 0)
        {
            lblTddPerDayMean.Text = "No data";
            SetNoDataLabels(lblTddMean, lblTddStdDev, lblTddSamples);
            return;
        }

        var (mean, stdDev) = General.CalculateMeanAndStdDev(dailyTotals);
        lblTddPerDayMean.Text = $"{mean:F2} U/day";
        lblTddMean.Text = $"{mean:F2} U";
        lblTddStdDev.Text = $"{stdDev:F2} U";
        lblTddSamples.Text = $"{dailyTotals.Count} days";
    }
    #endregion

    #region CHO Statistics

    private void CalculateChoStatistics()
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

        var (mean, stdDev) = General.CalculateMeanAndStdDev(dailyTotals);
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

        var (mean, stdDev) = General.CalculateMeanAndStdDev(values);
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
        meanLabel.Text = "Error";
        stdDevLabel.Text = "Error";
        samplesLabel.Text = "0";
    }

    private void SetNoDataLabels(Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        meanLabel.Text = "No data";
        stdDevLabel.Text = "No data";
        samplesLabel.Text = "0";
    }

    private void SetPlaceholderLabels(Label meanLabel, Label stdDevLabel, Label samplesLabel)
    {
        meanLabel.Text = "Coming soon";
        stdDevLabel.Text = "Coming soon";
        samplesLabel.Text = "--";
    }

    #endregion
}
