using gamon;
using GlucoMan.Maui.Resources.Strings;
using Mathematics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace GlucoMan.Maui.ViewModels;

/// <summary>
/// Represents statistics for a single time band (e.g., breakfast, lunch, dinner, other/residual).
/// </summary>
public class  TimeBandStatistics : INotifyPropertyChanged
{
    private string _mean = "--";
    private string _stdDev = "--";
    private string _samples = "--";
    private string _perDayMean = "--";

    public string Mean
    {
        get => _mean;
        set { _mean = value; OnPropertyChanged(); }
    }

    public string StdDev
    {
        get => _stdDev;
        set { _stdDev = value; OnPropertyChanged(); }
    }

    public string Samples
    {
        get => _samples;
        set { _samples = value; OnPropertyChanged(); }
    }

    public string PerDayMean
    {
        get => _perDayMean;
        set { _perDayMean = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void SetNoData()
    {
        Mean = AppStrings.NoData;
        StdDev = AppStrings.NoData;
        Samples = "0";
        PerDayMean = AppStrings.NoData;
    }

    public void SetFromStatistics(double mean, double stdDev, int count, string unit)
    {
        Mean = double.IsNaN(mean) ? AppStrings.NoData : $"{mean:F1} {unit}";
        StdDev = double.IsNaN(stdDev) ? AppStrings.NoData : $"{stdDev:F1} {unit}";
        Samples = count.ToString();
    }

    public void SetFromStatisticsWithPerDay(double mean, double stdDev, int count, 
        double perDayMean, string unit, string perDayUnit)
    {
        Mean = double.IsNaN(mean) ? AppStrings.NoData : $"{mean:F2} {unit}";
        StdDev = double.IsNaN(stdDev) ? AppStrings.NoData : $"{stdDev:F2} {unit}";
        Samples = count.ToString();
        PerDayMean = double.IsNaN(perDayMean) ? AppStrings.NoData : $"{perDayMean:F2} {perDayUnit}";
    }
}

/// <summary>
/// ViewModel for StatisticsPage. Uses GamonStatistics.MeansOfSumsInTimeBands 
/// to calculate CHO statistics (daily sums per band, then mean/stddev across days).
/// </summary>
public class StatisticsPageViewModel : INotifyPropertyChanged
{
    private DateTime _dateFrom;
    private DateTime _dateTo;
    
    // Meal time settings
    private double _breakfastStartHour;
    private double _breakfastEndHour;
    private double _lunchStartHour;
    private double _lunchEndHour;
    private double _dinnerStartHour;
    private double _dinnerEndHour;

    // Business layer references
    private BL_GlucoseMeasurements _blGlucose;
    private BL_BolusesAndInjections _blInjections;
    private BL_MealAndFood _blMealAndFood;

    #region Bindable Properties

    private string _dateRange;
    public string DateRange
    {
        get => _dateRange;
        set { _dateRange = value; OnPropertyChanged(); }
    }

    private string _glucoseDataSource;
    public string GlucoseDataSource
    {
        get => _glucoseDataSource;
        set { _glucoseDataSource = value; OnPropertyChanged(); }
    }

    private Color _glucoseDataSourceColor = Colors.Black;
    public Color GlucoseDataSourceColor
    {
        get => _glucoseDataSourceColor;
        set { _glucoseDataSourceColor = value; OnPropertyChanged(); }
    }

    // Glucose Statistics
    public TimeBandStatistics GlucoseTotal { get; } = new();
    public TimeBandStatistics GlucoseBreakfast { get; } = new();
    public TimeBandStatistics GlucoseLunch { get; } = new();
    public TimeBandStatistics GlucoseDinner { get; } = new();
    public TimeBandStatistics GlucoseOther { get; } = new();

    // Glucose Effective Statistics (excluding days without data per band)
    public TimeBandStatistics GlucoseBreakfastEff { get; } = new();
    public TimeBandStatistics GlucoseLunchEff { get; } = new();
    public TimeBandStatistics GlucoseDinnerEff { get; } = new();
    public TimeBandStatistics GlucoseOtherEff { get; } = new();

    // CHO Statistics
    public TimeBandStatistics ChoTotal { get; } = new();
    public TimeBandStatistics ChoBreakfast { get; } = new();
    public TimeBandStatistics ChoLunch { get; } = new();
    public TimeBandStatistics ChoDinner { get; } = new();
    public TimeBandStatistics ChoOther { get; } = new();

    // CHO Effective Statistics (excluding days without data per band)
    public TimeBandStatistics ChoBreakfastEff { get; } = new();
    public TimeBandStatistics ChoLunchEff { get; } = new();
    public TimeBandStatistics ChoDinnerEff { get; } = new();
    public TimeBandStatistics ChoOtherEff { get; } = new();

    private string _choNumberOfDays = "--";
    public string ChoNumberOfDays
    {
        get => _choNumberOfDays;
        set { _choNumberOfDays = value; OnPropertyChanged(); }
    }

    // Insulin Statistics (these are special - TDD, Quick, Long are not time-banded)
    public TimeBandStatistics InsulinTdd { get; } = new();
    public TimeBandStatistics InsulinQuick { get; } = new();
    public TimeBandStatistics InsulinLong { get; } = new();
    public TimeBandStatistics InsulinBreakfast { get; } = new();
    public TimeBandStatistics InsulinLunch { get; } = new();
    public TimeBandStatistics InsulinDinner { get; } = new();
    public TimeBandStatistics InsulinOther { get; } = new();

    // Insulin Effective Statistics (excluding days without data per band)
    public TimeBandStatistics InsulinBreakfastEff { get; } = new();
    public TimeBandStatistics InsulinLunchEff { get; } = new();
    public TimeBandStatistics InsulinDinnerEff { get; } = new();
    public TimeBandStatistics InsulinOtherEff { get; } = new();

    #endregion

    public StatisticsPageViewModel(DateTime dateFrom, DateTime dateTo)
    {
        _dateFrom = dateFrom;
        _dateTo = dateTo;

        _blGlucose = new BL_GlucoseMeasurements();
        _blInjections = new BL_BolusesAndInjections();
        _blMealAndFood = new BL_MealAndFood();

        // Load meal time settings from Common
        _breakfastStartHour = Common.breakfastStartHour ?? 6;
        _breakfastEndHour = Common.breakfastEndHour ?? 10;
        _lunchStartHour = Common.lunchStartHour ?? 11;
        _lunchEndHour = Common.lunchEndHour ?? 15;
        _dinnerStartHour = Common.dinnerStartHour ?? 17;
        _dinnerEndHour = Common.dinnerEndHour ?? 21;

        DateRange = string.Format(AppStrings.DateRangeLabel,
            dateFrom.ToString("dd/MM/yyyy"), dateTo.ToString("dd/MM/yyyy"));
    }

    /// <summary>
    /// Calculates all statistics using the new unified method.
    /// </summary>
    public void CalculateAllStatistics()
    {
        try
        {
            CalculateGlucoseStatistics();
            CalculateChoStatistics();
            CalculateInsulinStatistics();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPageViewModel - CalculateAllStatistics", ex);
        }
    }

    /// <summary>
    /// Creates the time bands list for breakfast, lunch, dinner.
    /// The residual (Other) is automatically calculated by MeansOfSumsInTimeBands.
    /// </summary>
    private List<(DateTime Begin, DateTime End)> CreateMealTimeBands()
    {
        // Use a reference date - only TimeOfDay matters
        var refDate = new DateTime(2026, 1, 1);
        return new List<(DateTime Begin, DateTime End)>
        {
            (refDate.AddHours(_breakfastStartHour), refDate.AddHours(_breakfastEndHour)),
            (refDate.AddHours(_lunchStartHour), refDate.AddHours(_lunchEndHour)),
            (refDate.AddHours(_dinnerStartHour), refDate.AddHours(_dinnerEndHour))
        };
    }

    #region Glucose Statistics

    private void CalculateGlucoseStatistics()
    {
        try
        {
            DateTime baseTime = new DateTime(2024, 1, 1, 0, 0, 0);
            List<GlucoseRecord> glucoseRecordsForStatistics = _blGlucose.GetGlucoseRecords(_dateFrom, _dateTo);
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime.AddHours(_breakfastStartHour), baseTime.AddHours(_breakfastEndHour)),
                (baseTime.AddHours(_lunchStartHour), baseTime.AddHours(_lunchEndHour)),
                (baseTime.AddHours(_dinnerStartHour), baseTime.AddHours(_dinnerEndHour)),
            };
            // Transform records to time-value pairs (timestamps required for time-band assignment)
            var glucoseData = glucoseRecordsForStatistics
                .Where(r => r.EventTime?.DateTime != null && r.GlucoseValue?.Double.HasValue == true)
                .Select(r => (r.EventTime.DateTime.Value, r.GlucoseValue.Double.Value))
                .ToList();

            if (glucoseData.Count == 0)
            {
                SetAllGlucoseNoData();
                return;
            }

            // Use MeansOfSumsInTimeBands for individual readings (not daily sums like CHO)
            var allStats = GamonStatistics.MeansOfSumsInTimeBands(glucoseData, bands);
            var bandStats = (Means: allStats.Means, StDevs: allStats.StdDevs, Counts: allStats.Counts);

            // Set data source indicator
            if (_blGlucose.UsingSensorData)
            {
                GlucoseDataSource = AppStrings.GlucoseDataSourceSensors;
                GlucoseDataSourceColor = Colors.Black;
            }
            else
            {
                GlucoseDataSource = AppStrings.GlucoseDataSourceManual;
                GlucoseDataSourceColor = Colors.Red;
            }

            // Calculate total (all day) glucose statistics using existing BL method
            var totalStats = _blGlucose.CalculateGlucoseStatistics(0, 24);
            if (totalStats != null && totalStats.NSamples.HasValue && totalStats.NSamples > 0)
            {
                GlucoseTotal.SetFromStatistics(
                    totalStats.Mean ?? 0, 
                    totalStats.StandardDeviation ?? 0, 
                    (int)(totalStats.NSamples ?? 0), 
                    "mg/dL");
            }
            else
            {
                GlucoseTotal.SetNoData();
            }

            // Set per-band statistics from bandStats (Breakfast, Lunch, Dinner, Other)
            int glucoseNDays = glucoseData.Select(d => d.Item1.Date).Distinct().Count();
            if (bandStats.Means.Count >= 4)
            {
                SetGlucoseBandStatsFromTuple(GlucoseBreakfast, bandStats, 0, glucoseNDays);
                SetGlucoseBandStatsFromTuple(GlucoseLunch, bandStats, 1, glucoseNDays);
                SetGlucoseBandStatsFromTuple(GlucoseDinner, bandStats, 2, glucoseNDays);
                SetGlucoseBandStatsFromTuple(GlucoseOther, bandStats, 3, glucoseNDays);
            }

            // Effective statistics (excluding days without data per band)
            var effBandStats = (Means: allStats.EffectiveMeans, StDevs: allStats.EffectiveStdDevs, Counts: allStats.EffectiveCounts);
            if (effBandStats.Means.Count >= 4)
            {
                SetGlucoseBandStatsFromTuple(GlucoseBreakfastEff, effBandStats, 0);
                SetGlucoseBandStatsFromTuple(GlucoseLunchEff, effBandStats, 1);
                SetGlucoseBandStatsFromTuple(GlucoseDinnerEff, effBandStats, 2);
                SetGlucoseBandStatsFromTuple(GlucoseOtherEff, effBandStats, 3);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPageViewModel - CalculateGlucoseStatistics", ex);
            SetAllGlucoseNoData();
        }
    }

    private void SetGlucoseBandStatsFromTuple(TimeBandStatistics bandStat,
        (List<double> Means, List<double> StDevs, List<int> Counts) bandStats, int bandIndex, int? totalDays = null)
    {
        double mean = bandStats.Means[bandIndex];
        double stdDev = bandStats.StDevs[bandIndex];
        int count = bandStats.Counts[bandIndex];

        if (double.IsNaN(mean) || count <= 0)
        {
            bandStat.SetNoData();
            return;
        }

        bandStat.SetFromStatistics(mean, stdDev, count, "mg/dL");
        if (totalDays.HasValue)
            bandStat.Samples = totalDays.Value.ToString();
    }

    private void SetAllGlucoseNoData()
    {
        GlucoseTotal.SetNoData();
        GlucoseBreakfast.SetNoData();
        GlucoseLunch.SetNoData();
        GlucoseDinner.SetNoData();
        GlucoseOther.SetNoData();
        GlucoseBreakfastEff.SetNoData();
        GlucoseLunchEff.SetNoData();
        GlucoseDinnerEff.SetNoData();
        GlucoseOtherEff.SetNoData();
    }

    #endregion

    #region CHO Statistics

    private void CalculateChoStatistics()
    {
        try
        {
            var meals = _blMealAndFood.GetMeals(_dateFrom, _dateTo);

            if (meals == null || meals.Count == 0)
            {
                SetAllChoNoData();
                return;
            }

            // Convert meals to time-value pairs (CHO grams)
            var choData = meals
                .Where(m => m.EventTime?.DateTime != null && m.CarbohydratesGrams?.Double != null)
                .Select(m => (m.EventTime.DateTime.Value, m.CarbohydratesGrams.Double.Value))
                .ToList();

            if (choData.Count == 0)
            {
                SetAllChoNoData();
                return;
            }

            // Calculate total daily CHO (grouped by day)
            CalculateTotalDayCho(choData);

            // Calculate statistics per time band using MeansOfSumsInTimeBands
            var bands = CreateMealTimeBands();
            var allStats = GamonStatistics.MeansOfSumsInTimeBands(choData, bands);
            var bandStats = (Means: allStats.Means, StDevs: allStats.StdDevs, Counts: allStats.Counts);

            // Number of days in the data
            int nDays = choData.Select(d => d.Item1.Date).Distinct().Count();
            ChoNumberOfDays = nDays.ToString();

            // bandStats returns: [Breakfast, Lunch, Dinner, Residual/Other]
            if (bandStats.Means.Count >= 4)
            {
                SetChoBandStats(ChoBreakfast, bandStats, 0, nDays);
                SetChoBandStats(ChoLunch, bandStats, 1, nDays);
                SetChoBandStats(ChoDinner, bandStats, 2, nDays);
                SetChoBandStats(ChoOther, bandStats, 3, nDays);
            }

            // Effective statistics (excluding days without data per band)
            var effBandStats = (Means: allStats.EffectiveMeans, StDevs: allStats.EffectiveStdDevs, Counts: allStats.EffectiveCounts);
            if (effBandStats.Means.Count >= 4)
            {
                SetChoBandStats(ChoBreakfastEff, effBandStats, 0);
                SetChoBandStats(ChoLunchEff, effBandStats, 1);
                SetChoBandStats(ChoDinnerEff, effBandStats, 2);
                SetChoBandStats(ChoOtherEff, effBandStats, 3);
            }
        } 
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPageViewModel - CalculateChoStatistics", ex);
            SetAllChoNoData();
        }
    }

    private void CalculateTotalDayCho(List<(DateTime t, double value)> choData)
    {
        // Group by day and sum CHO for each day
        var dailyTotals = choData
            .GroupBy(c => c.t.Date)
            .Select(g => g.Sum(c => c.value))
            .ToList();

        if (dailyTotals.Count == 0)
        {
            ChoTotal.SetNoData();
            return;
        }

        var stats = GamonStatistics.MeanAndStdDev(dailyTotals);
        ChoTotal.SetFromStatisticsWithPerDay(stats.Mean, stats.StdDev, stats.Count,
            stats.Mean, "g", "g/day");
        ChoTotal.Samples = $"{dailyTotals.Count} days";
    }

    private void SetChoBandStats(TimeBandStatistics bandStat,
        (List<double> Means, List<double> StDevs, List<int> Counts) bandStats, int bandIndex, int? totalDays = null)
    {
        double mean = bandStats.Means[bandIndex];
        double stdDev = bandStats.StDevs[bandIndex];
        int count = bandStats.Counts[bandIndex];

        if (double.IsNaN(mean))
        {
            bandStat.SetNoData();
            return;
        }

        // MeansOfSumsInTimeBands already gives the mean of daily sums
        bandStat.Mean = $"{mean:F2} g";
        bandStat.StdDev = $"{stdDev:F2} g";
        bandStat.Samples = totalDays.HasValue ? totalDays.Value.ToString() : count.ToString();
    }

    private void SetAllChoNoData()
    {
        ChoTotal.SetNoData();
        ChoBreakfast.SetNoData();
        ChoLunch.SetNoData();
        ChoDinner.SetNoData();
        ChoOther.SetNoData();
        ChoBreakfastEff.SetNoData();
        ChoLunchEff.SetNoData();
        ChoDinnerEff.SetNoData();
        ChoOtherEff.SetNoData();
        ChoNumberOfDays = "0";
    }

    #endregion

    #region Insulin Statistics

    private void CalculateInsulinStatistics()
    {
        try
        {
            // Load injections separated by type (Rapid, Short, Intermediate, Long)
            // This populates internal lists used by CalculateTddInsulin, 
            // CalculateTotalQuickActingInsulin, CalculateTotalLongActingInsulin
            _blInjections.GetInjectionsForStatistics(_dateFrom, _dateTo);

            // TDD (Total Daily Dose) uses all injection types
            var tddStats = _blInjections.CalculateTddInsulin();
            SetInsulinBandStats(InsulinTdd, tddStats);

            // Quick acting (Rapid + Short)
            var quickStats = _blInjections.CalculateTotalQuickActingInsulin();
            SetInsulinBandStats(InsulinQuick, quickStats);

            // Long acting (Intermediate + Long)
            var longStats = _blInjections.CalculateTotalLongActingInsulin();
            SetInsulinBandStats(InsulinLong, longStats);

            // For time-band statistics use ONLY quick-acting insulin (Rapid + Short)
            // Long-acting insulin (basal) is typically injected outside meal times
            // and would incorrectly inflate the "Other" band
            var quickInjections = _blInjections.GetQuickInjections(_dateFrom, _dateTo);

            if (quickInjections == null || quickInjections.Count == 0)
            {
                SetTimeBandInsulinNoData();
                return;
            }

            var insulinData = quickInjections
                .Where(m => m.EventTime?.DateTime != null && m.InsulinValue?.Double != null)
                .Select(m => (m.EventTime.DateTime.Value, m.InsulinValue.Double.Value))
                .ToList();

            if (insulinData.Count == 0)
            {
                SetTimeBandInsulinNoData();
                return;
            }

            // Calculate statistics per time band using MeansOfSumsInTimeBands
            var bands = CreateMealTimeBands();
            var allStats = GamonStatistics.MeansOfSumsInTimeBands(insulinData, bands);
            var bandStats = (Means: allStats.Means, StDevs: allStats.StdDevs, Counts: allStats.Counts);
            int insulinNDays = insulinData.Select(d => d.Item1.Date).Distinct().Count();

            // bandStats returns: [Breakfast, Lunch, Dinner, Residual/Other]
            if (bandStats.Means.Count >= 4)
            {
                SetInsulinBandStats(InsulinBreakfast, bandStats, 0, insulinNDays);
                SetInsulinBandStats(InsulinLunch, bandStats, 1, insulinNDays);
                SetInsulinBandStats(InsulinDinner, bandStats, 2, insulinNDays);
                SetInsulinBandStats(InsulinOther, bandStats, 3, insulinNDays);
            }

            // Effective statistics (excluding days without data per band)
            var effBandStats = (Means: allStats.EffectiveMeans, StDevs: allStats.EffectiveStdDevs, Counts: allStats.EffectiveCounts);
            if (effBandStats.Means.Count >= 4)
            {
                SetInsulinBandStats(InsulinBreakfastEff, effBandStats, 0);
                SetInsulinBandStats(InsulinLunchEff, effBandStats, 1);
                SetInsulinBandStats(InsulinDinnerEff, effBandStats, 2);
                SetInsulinBandStats(InsulinOtherEff, effBandStats, 3);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("StatisticsPageViewModel - CalculateInsulinStatistics", ex);
            SetAllInsulinNoData();
        }
    }

    private void SetTimeBandInsulinNoData()
    {
        InsulinBreakfast.SetNoData();
        InsulinLunch.SetNoData();
        InsulinDinner.SetNoData();
        InsulinOther.SetNoData();
        InsulinBreakfastEff.SetNoData();
        InsulinLunchEff.SetNoData();
        InsulinDinnerEff.SetNoData();
        InsulinOtherEff.SetNoData();
    }

    private void SetInsulinBandStats(TimeBandStatistics bandStat,
        (List<double> Means, List<double> StDevs, List<int> Counts) bandStats, int bandIndex, int? totalDays = null)
    {
        double mean = bandStats.Means[bandIndex];
        double stdDev = bandStats.StDevs[bandIndex];
        int count = bandStats.Counts[bandIndex];

        if (double.IsNaN(mean) || count <= 0)
        {
            bandStat.SetNoData();
            return;
        }

        bandStat.Mean = $"{mean:F2} U";
        bandStat.StdDev = $"{stdDev:F2} U";
        bandStat.Samples = totalDays.HasValue ? totalDays.Value.ToString() : count.ToString();
    }

    private void SetInsulinBandStats(TimeBandStatistics bandStat, StatisticsData sd)
    {
        if (sd == null || !sd.NSamples.HasValue || sd.NSamples <= 0)
        {
            bandStat.SetNoData();
            return;
        }

        bandStat.SetFromStatisticsWithPerDay(
            sd.Mean ?? 0, 
            sd.StandardDeviation ?? 0, 
            (int)(sd.NSamples ?? 0),
            sd.DailyMean ?? 0, 
            "U", 
            "U/day");
    }

    private void SetAllInsulinNoData()
    {
        InsulinTdd.SetNoData();
        InsulinQuick.SetNoData();
        InsulinLong.SetNoData();
        InsulinBreakfast.SetNoData();
        InsulinLunch.SetNoData();
        InsulinDinner.SetNoData();
        InsulinOther.SetNoData();
        InsulinBreakfastEff.SetNoData();
        InsulinLunchEff.SetNoData();
        InsulinDinnerEff.SetNoData();
        InsulinOtherEff.SetNoData();
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
