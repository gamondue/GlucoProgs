using GlucoMan.BusinessLayer;
using GlucoMan;
using Mathematics.Identification1;

namespace GlucoMan.Maui;

public partial class StatisticsPage : ContentPage
{
    private string _dataType;
    private DateTime _dateFrom;
    private DateTime _dateTo;

    public StatisticsPage(string dataType, DateTime dateFrom, DateTime dateTo)
    {
        InitializeComponent();
        _dataType = dataType;
        _dateFrom = dateFrom;
        _dateTo = dateTo;
        lblDataType.Text = $"Data Type: {dataType}";
        lblDateRange.Text = $"From: {dateFrom:dd/MM/yyyy} - To: {dateTo:dd/MM/yyyy}";

        ////////// !!!! 
        ////////_dateTo = DateTime.Now.AddMonths(-15);
        ////////_dateFrom = _dateTo.AddMonths(-12);
    }

    private async void btnIdentify_Click(object sender, EventArgs e)
    {
        try
        {
            // 2. Get data from business layer
            var blMeals = new BL_MealAndFood();
            var blGlucose = new BL_GlucoseMeasurements();
            var blInj = new BL_BolusesAndInjections();

            var meals = blMeals.GetMeals(_dateFrom, _dateTo);
            var injections = blInj.GetInjections(_dateFrom, _dateTo);
            var glucose = blGlucose.GetSensorsRecords(_dateFrom, _dateTo);

            // 3. Convert to TimePoint[]
            var mealPoints = Identification.FromMeals(meals);
            var injPoints = Identification.FromInjections(injections);
            var glucosePoints = Identification.FromGlucoseRecords(glucose);

            // 4. Run identification (sampling 6 min = 360s)
            double Ts = 360.0;
            var result = Identification.IdentifyMisoFirstOrder(
                glucosePoints,
                new[] { mealPoints, injPoints },
                Ts,
                maxDelaySamples: 60, // up to 6 hours
                ridge: 0.01);

            // 5. Compute fit error (SSE) and time constant
            var y = Identification.ResampleLinear(glucosePoints, glucosePoints.Min(p => p.Time), glucosePoints.Max(p => p.Time), Ts);
            var inputs = new[]
            {
                Identification.ResampleLinear(mealPoints, glucosePoints.Min(p => p.Time), glucosePoints.Max(p => p.Time), Ts),
                Identification.ResampleLinear(injPoints, glucosePoints.Min(p => p.Time), glucosePoints.Max(p => p.Time), Ts)
            };
            double sse = Identification.ComputeSse(y, inputs, result.delays, result.a, result.b, result.c);
            double T = Identification.AtoTimeConstant(result.a, Ts);

            // 6. Show results
            entryA.Text = result.a.ToString("G4");
            entryB1.Text = result.b.Length > 0 ? result.b[0].ToString("G4") : "";
            entryB2.Text = result.b.Length > 1 ? result.b[1].ToString("G4") : "";
            entryC.Text = result.c.ToString("G4");
            entryDelayCHO.Text = result.delays.Length > 0 ? result.delays[0].ToString() : "";
            entryDelayInsulin.Text = result.delays.Length > 1 ? result.delays[1].ToString() : "";
            entryT.Text = double.IsInfinity(T) ? "-" : T.ToString("F0");
            entrySSE.Text = sse.ToString("G4");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to identify parameters: {ex.Message}", "OK");
        }
    }

    private async void btnIdentify2_Click(object sender, EventArgs e)
    {
        try
        {
            // 2. Get data from business layer
            var blMeals = new BL_MealAndFood();
            var blGlucose = new BL_GlucoseMeasurements();
            var blInj = new BL_BolusesAndInjections();

            var meals = blMeals.GetMeals(_dateFrom, _dateTo);
            var injections = blInj.GetQuickInjections(_dateFrom, _dateTo);
            var glucose = blGlucose.GetSensorsRecords(_dateFrom, _dateTo);

            // Parameters nomenclature:
            // - "before" = LEFT SIDE (prima dell'evento)
            // - "after" = RIGHT SIDE (dopo l'evento)
            
            // RIGHT SIDE parameters (after event)
            double afterIsolationHours = 3.0;      // No opposite events for 3 hours after event
            double minDataHours = 3.0;             // Minimum glucose data duration after event
            
            // LEFT SIDE parameters (before event)
            double basalCheckHours = 2.0;          // Check 2 hours before event for basal stability
            double maxBasalSlope = 2.5;            // Max 5 mg/dL per hour slope (more strict)
            double beforeIsolationHours = 2.0;     // No opposite events for 2 hours before event
            
            // Other parameters
            int maxDelaySamples = 60;
            double ridge = 0.01;
            double Ts = 900.0;                     // Sampling 15 min = 900s - Actual glucose sensor sampling period
            double TsSeconds = Ts;

            // Ask if user wants debug mode (show graphs for each segment)
            bool debugMode = false;
#if DEBUG
            debugMode = await DisplayAlert("Debug Mode", 
                "Do you want to view the graph for each identified segment?\n\n" +
                "This will show the ChartPage for each segment during identification.",
                "Yes", "No");
#endif

            // Find isolated segments with extended basal stability check and opposite isolation
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                meals, 
                injections, 
                glucose, 
                isolationHours: afterIsolationHours,           // RIGHT SIDE: after event
                minDataHours: minDataHours,
                TsSeconds: TsSeconds,
                basalCheckHours: basalCheckHours,              // LEFT SIDE: basal check
                maxBasalSlope: maxBasalSlope,                  // LEFT SIDE: max slope
                beforeIsolationHours: beforeIsolationHours);   // LEFT SIDE: before event

            // 4. Run identification with two separate dynamics using async version
            var (choResults, insulinResults) = await Identification2.IdentifyTwoChannelsAsync(
                isolatedMeals,
                isolatedInjections,
                glucose,
                TsSeconds: Ts,
                isolationHours: afterIsolationHours,
                minDataHours: minDataHours,
                maxDelaySamples: maxDelaySamples,
                ridge: ridge,
                debugMode: debugMode,
                segmentInfoCallback: async (segmentStart, segmentIndex, inputType) =>
                {
                    // Show graph for this segment in debug mode
                    if (debugMode)
                    {
                        // Create and show chart page
                        var chartPage = new ChartPage("Glucose", segmentStart);
                        await Navigation.PushAsync(chartPage);
                        
                        // Show alert with segment info and wait for user to continue
                        bool continueIdentification = await DisplayAlert("Segment Info", 
                            $"Segment #{segmentIndex}\n" +
                            $"Type: {inputType}\n" +
                            $"Start: {segmentStart:yyyy-MM-dd HH:mm}\n\n" +
                            $"Review the graph, then choose:",
                            "Continue", "Stop");
                        
                        // Pop back to statistics page
                        await Navigation.PopAsync();
                        
                        // If user wants to stop, throw an exception to break the identification
                        if (!continueIdentification)
                        {
                            throw new OperationCanceledException("User cancelled identification during debug review.");
                        }
                    }
                });

            // 5. Display CHO results
            if (choResults != null)
            {
                entryTauCHO.Text = $"{choResults.TauMean:F0} ± {choResults.TauStd:F0}";
                entryGainCHO.Text = $"{choResults.GainMean:G4} ± {choResults.GainStd:G4}";
                entryDelayCHO2.Text = $"{choResults.DelayMean:F1} ± {choResults.DelayStd:F1}";
                entryOffsetCHO.Text = $"{choResults.OffsetMean:F1} ± {choResults.OffsetStd:F1}";
                entryR2CHO.Text = choResults.AvgRSquared.ToString("F3");
                entrySegmentsCHO.Text = choResults.NumSegments.ToString();
            }
            else
            {
                entryTauCHO.Text = "N/A";
                entryGainCHO.Text = "N/A";
                entryDelayCHO2.Text = "N/A";
                entryOffsetCHO.Text = "N/A";
                entryR2CHO.Text = "N/A";
                entrySegmentsCHO.Text = "0";
            }

            // 6. Display Insulin results
            if (insulinResults != null)
            {
                entryTauInsulin.Text = $"{insulinResults.TauMean:F0} ± {insulinResults.TauStd:F0}";
                entryGainInsulin.Text = $"{insulinResults.GainMean:G4} ± {insulinResults.GainStd:G4}";
                entryDelayInsulin2.Text = $"{insulinResults.DelayMean:F1} ± {insulinResults.DelayStd:F1}";
                entryOffsetInsulin.Text = $"{insulinResults.OffsetMean:F1} ± {insulinResults.OffsetStd:F1}";
                entryR2Insulin.Text = insulinResults.AvgRSquared.ToString("F3");
                entrySegmentsInsulin.Text = insulinResults.NumSegments.ToString();
            }
            else
            {
                entryTauInsulin.Text = "N/A";
                entryGainInsulin.Text = "N/A";
                entryDelayInsulin2.Text = "N/A";
                entryOffsetInsulin.Text = "N/A";
                entryR2Insulin.Text = "N/A";
                entrySegmentsInsulin.Text = "0";
            }

            // 7. Show summary message
            // Count non-null items in lists
            int isolatedMealsCount = isolatedMeals?.Where(m => m != null).Count() ?? 0;
            int isolatedInjectionsCount = isolatedInjections?.Where(i => i != null).Count() ?? 0;
            
            int totalSegments = (choResults?.NumSegments ?? 0) + (insulinResults?.NumSegments ?? 0);
            await DisplayAlert("Identification Complete", 
                $"Found {totalSegments} isolated segments:\n" +
                $"- CHO: {choResults?.NumSegments ?? 0} segments\n" +
                $"- Insulin: {insulinResults?.NumSegments ?? 0} segments\n" +
                $"- Isolated Meals: {isolatedMealsCount}\n" +
                $"- Isolated Injections: {isolatedInjectionsCount}\n\n" +
                $"LEFT SIDE (before event):\n" +
                $"  • Basal stability: {basalCheckHours:F1}h, max slope {maxBasalSlope:F1} mg/dL/h\n" +
                $"  • Opposite isolation: {beforeIsolationHours:F1}h\n\n" +
                $"RIGHT SIDE (after event):\n" +
                $"  • Opposite isolation: {afterIsolationHours:F1}h\n" +
                $"  • No glucose stability check (free dynamics)\n\n" +
                $"Sampling: {Ts/60:F0} min ({Ts:F0}s)",
                "OK");
        }
        catch (OperationCanceledException)
        {
            await DisplayAlert("Cancelled", "Identification was cancelled during debug review.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to identify parameters: {ex.Message}\n\n{ex.StackTrace}", "OK");
        }
    }
}
