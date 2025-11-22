using GlucoMan;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace Mathematics.Identification1
{
    /// <summary>
    /// Advanced identification tools for MISO systems with separate first-order dynamics per input.
    /// This class implements identification for the model:
    /// y(t) = G1(s)*u1(t) + G2(s)*u2(t)
    /// where G1(s) = k1/(?1*s+1) and G2(s) = k2/?2*s+1)
    /// with independent time constants ?1, ?2 and gains k1, k2.
    /// </summary>
    public static class Identification2
    {
        /// <summary>
        /// Represents a data segment where only one input is active (for isolated identification)
        /// </summary>
        public class IsolatedSegment
        {
            public int InputIndex { get; set; }          // 0=CHO, 1=Insulin
            public DateTime EventTime { get; set; }      // Time of input event
            public double InputValue { get; set; }       // Value of input (g CHO or U insulin)
            public DateTime SegmentStart { get; set; }   // Start of analysis window
            public DateTime SegmentEnd { get; set; }     // End of analysis window
            public Identification.TimePoint[] GlucoseData { get; set; }  // Glucose measurements in window
        }

        /// <summary>
        /// Result of single-channel SISO identification
        /// </summary>
        public class SisoResult
        {
            public double A { get; set; }              // Discrete pole (autoregressive coefficient)
            public double B { get; set; }              // Discrete gain
            public double C { get; set; }              // Offset
            public int Delay { get; set; }             // Delay in samples
            public double SSE { get; set; }            // Sum of squared errors
            public double MSE { get; set; }            // Mean squared error
            public double RMSE { get; set; }           // Root mean squared error
            public double RSquared { get; set; }       // Coefficient of determination
            public double TimeConstant { get; set; }   // Time constant ? (seconds)
            public double StaticGain { get; set; }     // Static gain k
            public DateTime EventTime { get; set; }    // Time of input event
        }

        /// <summary>
        /// Aggregated statistics from multiple SISO identifications
        /// </summary>
        public class AggregatedResult
        {
            public string InputName { get; set; }        // "CHO" or "Insulin"
            
            // Time constant statistics
            public double TauMean { get; set; }
            public double TauStd { get; set; }
            public double TauMedian { get; set; }
            
            // Gain statistics
            public double GainMean { get; set; }
            public double GainStd { get; set; }
            public double GainMedian { get; set; }
            
            // Delay statistics
            public double DelayMean { get; set; }
            public double DelayStd { get; set; }
            public double DelayMedian { get; set; }
            
            // Offset statistics
            public double OffsetMean { get; set; }
            public double OffsetStd { get; set; }
            public double OffsetMedian { get; set; }
            
            // Quality metrics
            public double AvgRSquared { get; set; }
            public double AvgRMSE { get; set; }
            public int NumSegments { get; set; }
            
            // Individual results
            public List<SisoResult> IndividualResults { get; set; } = new List<SisoResult>();
        }

        /// <summary>
        /// Find isolated segments where only one input is active.
        /// New criteria (updated):
        /// - LEFT SIDE (before event):
        ///   * Basal glucose stable: linear regression slope absolute value <= maxBasalSlope over basalCheckHours
        ///   * No opposite type events in previous beforeIsolationHours
        ///   * Same-type events allowed before
        /// - RIGHT SIDE (after event):
        ///   * No opposite type events for isolationHours after the event (same-type events allowed)
        /// - At least minDataHours glucose data available (segment limited by isolationHours window and available data)
        /// </summary>
        public static (List<Meal>, List<Injection>) FindIsolatedSegments(
            List<Meal> choMeals,
            List<Injection> insulinEvents,
            List<GlucoseRecord> glucoseData,
            double isolationHours = 4.0,          // after-event opposite isolation window
            double minDataHours = 1.0,
            double TsSeconds = 900.0,             // sampling period seconds
            double basalCheckHours = 2.0,         // hours for basal slope regression (before event)
            double maxBasalSlope = 5.0,           // mg/dL per hour slope threshold
            double beforeIsolationHours = 2.0)    // hours before event with no opposite events
        {
            // Validate inputs
            if (glucoseData == null || glucoseData.Count == 0)
                return (new List<Meal>(), new List<Injection>());

            if (choMeals == null) choMeals = new List<Meal>();
            if (insulinEvents == null) insulinEvents = new List<Injection>();

            // Sort lists by time (in case they're not already sorted)
            choMeals = choMeals.OrderBy(p => p.EventTime.DateTime).ToList();
            insulinEvents = insulinEvents.OrderBy(p => p.EventTime.DateTime).ToList();

            // Compute required samples based on minDataHours and sampling period
            int requiredSamples = Math.Max(3, (int)Math.Ceiling((minDataHours * 3600.0) / TsSeconds));

            var isolatedMeals = new List<Meal>();
            var isolatedInjections = new List<Injection>();

            int i = 0, j = 0;
            int nCho = choMeals.Count;
            int nIns = insulinEvents.Count;

            // Helper to get last glucose time (for events with no future opposite event)
            DateTime glucoseMaxTime = glucoseData.Max(g => g.EventTime.DateTime ?? DateTime.MinValue);

            while (i < nCho || j < nIns)
            {
                // pick earliest next event
                Event current;
                bool currentIsCho;
                if (i < nCho && (j >= nIns || choMeals[i].EventTime.DateTime <= insulinEvents[j].EventTime.DateTime))
                {
                    current = choMeals[i];
                    currentIsCho = true;
                    i++; // advance index for cho (we process current now)
                }
                else
                {
                    current = insulinEvents[j];
                    currentIsCho = false;
                    j++; // advance insulin index
                }

                // Safeguard null event time
                if (current.EventTime?.DateTime == null)
                {
                    System.Diagnostics.Debug.WriteLine("Skip event: null EventTime");
                    continue;
                }
                DateTime eventTime = current.EventTime.DateTime.Value;

                // LEFT: basal stability
                if (!CheckBasalStability(glucoseData, eventTime, basalCheckHours, maxBasalSlope))
                {
                    System.Diagnostics.Debug.WriteLine($"Reject {eventTime}: basal unstable");
                    continue;
                }

                // LEFT: no opposite events in beforeIsolationHours
                if (!HasNoOppositeEventsBefore(currentIsCho, eventTime, choMeals, insulinEvents, beforeIsolationHours))
                {
                    System.Diagnostics.Debug.WriteLine($"Reject {eventTime}: opposite events before");
                    continue;
                }

                // RIGHT: no opposite events after within isolationHours
                if (!HasNoOppositeEventsAfter(currentIsCho, eventTime, choMeals, insulinEvents, isolationHours))
                {
                    System.Diagnostics.Debug.WriteLine($"Reject {eventTime}: opposite events after");
                    continue;
                }

                // Determine segment end (limited by minDataHours, isolation window and last glucose time)
                DateTime segmentStart = eventTime;
                DateTime segmentEndRequested = eventTime.AddHours(minDataHours);
                DateTime isolationWindowEnd = eventTime.AddHours(isolationHours);
                DateTime segmentEnd = segmentEndRequested <= isolationWindowEnd ? segmentEndRequested : isolationWindowEnd;
                if (segmentEnd > glucoseMaxTime) segmentEnd = glucoseMaxTime;

                // Collect glucose data for window
                var glucoseSegment = glucoseData
                    .Where(g => g.EventTime.DateTime >= segmentStart && g.EventTime.DateTime <= segmentEnd)
                    .ToList();

                if (glucoseSegment.Count >= requiredSamples)
                {
                    if (currentIsCho)
                    {
                        isolatedMeals.Add((Meal)current);
                        isolatedMeals.Add(null); // segment separator
                    }
                    else
                    {
                        isolatedInjections.Add((Injection)current);
                        isolatedInjections.Add(null);
                    }
                    System.Diagnostics.Debug.WriteLine($"Accept {eventTime}: samples={glucoseSegment.Count}, window={(segmentEnd - segmentStart).TotalHours:F2}h");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Reject {eventTime}: insufficient glucose samples {glucoseSegment.Count}/{requiredSamples}");
                }
            }

            System.Diagnostics.Debug.WriteLine($"Segments chosen: meals={isolatedMeals.Where(m=>m!=null).Count()}, injections={isolatedInjections.Where(x=>x!=null).Count()}");
            return (isolatedMeals, isolatedInjections);
        }

        // Helper: check no opposite events BEFORE
        private static bool HasNoOppositeEventsBefore(bool currentIsCho, DateTime eventTime, List<Meal> choMeals, List<Injection> insulinEvents, double hours)
        {
            DateTime windowStart = eventTime.AddHours(-hours);
            if (currentIsCho)
            {
                return !insulinEvents.Any(inj => inj.EventTime?.DateTime != null && inj.EventTime.DateTime >= windowStart && inj.EventTime.DateTime < eventTime);
            }
            else
            {
                return !choMeals.Any(meal => meal.EventTime?.DateTime != null && meal.EventTime.DateTime >= windowStart && meal.EventTime.DateTime < eventTime);
            }
        }

        // Helper: check no opposite events AFTER
        private static bool HasNoOppositeEventsAfter(bool currentIsCho, DateTime eventTime, List<Meal> choMeals, List<Injection> insulinEvents, double hours)
        {
            DateTime windowEnd = eventTime.AddHours(hours);
            if (currentIsCho)
            {
                return !insulinEvents.Any(inj => inj.EventTime?.DateTime != null && inj.EventTime.DateTime > eventTime && inj.EventTime.DateTime <= windowEnd);
            }
            else
            {
                return !choMeals.Any(meal => meal.EventTime?.DateTime != null && meal.EventTime.DateTime > eventTime && meal.EventTime.DateTime <= windowEnd);
            }
        }

        /// <summary>
        /// Check if glucose is sufficiently stable (flat) before an event.
        /// Uses linear regression to compute slope of glucose trend.
        /// </summary>
        /// <param name="glucoseData">All glucose data</param>
        /// <param name="eventTime">Time of the event to check</param>
        /// <param name="checkHoursBefore">Hours before event to analyze (e.g., 1.0)</param>
        /// <param name="maxSlope">Maximum acceptable slope in mg/dL per hour</param>
        /// <returns>True if basal is stable (slope below threshold)</returns>
        private static bool CheckBasalStability(
            List<GlucoseRecord> glucoseData,
            DateTime eventTime,
            double checkHoursBefore,
            double maxSlope)
        {
            // Define time window before event
            DateTime windowStart = eventTime.AddHours(-checkHoursBefore);
            DateTime windowEnd = eventTime;

            // Extract raw glucose data in window (strictly before event)
            var basalRaw = glucoseData
                .Where(g => g.EventTime?.DateTime != null &&
                            g.GlucoseValue != null &&
                            !double.IsNaN((double)g.GlucoseValue.Double) &&
                            g.GlucoseValue.Double > 0 &&
                            g.EventTime.DateTime >= windowStart &&
                            g.EventTime.DateTime < windowEnd)
                .OrderBy(g => g.EventTime.DateTime)
                .ToList();

            // Need a minimum number of points
            if (basalRaw.Count < 4)
            {
                System.Diagnostics.Debug.WriteLine($"Basal check REJECT (points<4): {eventTime:yyyy-MM-dd HH:mm}");
                return false;
            }

            // Build time (hours) and glucose arrays
            var x = new List<double>();
            var y = new List<double>();
            foreach (var r in basalRaw)
            {
                double t = (r.EventTime.DateTime.Value - windowStart).TotalHours;
                x.Add(t);
                y.Add(r.GlucoseValue.Double.Value);
            }

            // Simple smoothing (3-point moving average) to reduce sensor noise
            if (y.Count >= 5)
            {
                var ySmooth = new double[y.Count];
                for (int k = 0; k < y.Count; k++)
                {
                    int k0 = Math.Max(0, k - 1);
                    int k1 = Math.Min(y.Count - 1, k + 1);
                    double sum = 0; int n = 0;
                    for (int j = k0; j <= k1; j++) { sum += y[j]; n++; }
                    ySmooth[k] = sum / n;
                }
                y = ySmooth.ToList();
            }

            int nPts = x.Count;
            double sumX = x.Sum();
            double sumY = y.Sum();
            double sumXY = x.Zip(y, (a, b) => a * b).Sum();
            double sumX2 = x.Sum(a => a * a);
            double denom = nPts * sumX2 - sumX * sumX;
            double olsSlope;
            if (Math.Abs(denom) < 1e-12)
                olsSlope = 0.0; // treat as flat
            else
                olsSlope = (nPts * sumXY - sumX * sumY) / denom;

            // Robust Theil-Sen slope
            var slopes = new List<double>();
            for (int a = 0; a < nPts - 1; a++)
            {
                for (int b = a + 1; b < nPts; b++)
                {
                    double dt = x[b] - x[a];
                    if (Math.Abs(dt) < 1e-9) continue;
                    slopes.Add((y[b] - y[a]) / dt);
                }
            }
            double senSlope = slopes.Count > 0 ? Median(slopes) : olsSlope;

            // Choose representative slope (robust but keep sign of senSlope)
            double repSlope = Math.Abs(senSlope) < Math.Abs(olsSlope) ? senSlope : olsSlope;

            // Additional flatness criterion: total change small
            double totalChange = Math.Abs(y.Last() - y.First());
            double maxAllowedTotalChange = maxSlope * checkHoursBefore * 0.8; // slightly stricter than linear bound

            bool slopeOk = Math.Abs(repSlope) <= maxSlope;
            bool rangeOk = totalChange <= maxAllowedTotalChange;

            bool stable = slopeOk && rangeOk;

            System.Diagnostics.Debug.WriteLine(
                $"Basal check at {eventTime:yyyy-MM-dd HH:mm}: OLS={olsSlope:F2} Sen={senSlope:F2} Rep={repSlope:F2} |?|={totalChange:F1} (<= {maxAllowedTotalChange:F1}) Stable={stable} Points={nPts}");

            return stable;
        }

        private static double Median(List<double> values)
        {
            if (values == null || values.Count == 0) return 0.0;
            var arr = values.OrderBy(v => v).ToArray();
            int mid = arr.Length / 2;
            if (arr.Length % 2 == 0)
                return (arr[mid - 1] + arr[mid]) / 2.0;
            return arr[mid];
        }

        /// <summary>
        /// Identify SISO model on a single isolated segment.
        /// Model: y[k] = a*y[k-1] + b*u[k-d] + c + e[k]
        /// Uses grid search over delays and ridge regression.
        /// </summary>
        public static SisoResult IdentifySisoSegment(
            IsolatedSegment segment,
            double TsSeconds = 900.0,        // Default 15 minutes (900s) - typical CGM sampling
            int maxDelaySamples = 60,
            double ridge = 0.01)
        {
            if (segment == null || segment.GlucoseData == null || segment.GlucoseData.Length < 5)
                return null;

            // Resample glucose on uniform grid
            var start = segment.SegmentStart;
            var end = segment.SegmentEnd;
            var y = Identification.ResampleLinear(segment.GlucoseData, start, end, TsSeconds);
            
            // Create impulse input at event time
            int N = y.Length;
            var u = new double[N];
            int eventSample = (int)Math.Round((segment.EventTime - start).TotalSeconds / TsSeconds);
            if (eventSample >= 0 && eventSample < N)
                u[eventSample] = segment.InputValue;

            // Grid search over delays
            double bestSSE = double.PositiveInfinity;
            double bestA = 0, bestB = 0, bestC = 0;
            int bestDelay = 0;

            for (int delay = 0; delay <= maxDelaySamples && delay < N; delay++)
            {
                var (a, b, c) = FitSisoGivenDelay(y, u, delay, ridge);
                var sse = ComputeSisoSSE(y, u, delay, a, b, c);
                
                if (sse < bestSSE)
                {
                    bestSSE = sse;
                    bestA = a;
                    bestB = b;
                    bestC = c;
                    bestDelay = delay;
                }
            }

            // Compute quality metrics
            int validSamples = y.Count(val => !double.IsNaN(val));
            double mse = validSamples > 0 ? bestSSE / validSamples : double.NaN;
            double rmse = Math.Sqrt(mse);
            
            double yMean = y.Where(val => !double.IsNaN(val)).DefaultIfEmpty(0).Average();
            double sst = y.Where(val => !double.IsNaN(val)).Sum(val => Math.Pow(val - yMean, 2));
            double rSquared = sst > 0 ? 1 - bestSSE / sst : 0;

            // Convert to physical parameters
            double tau = Identification.AtoTimeConstant(bestA, TsSeconds);
            double k = Math.Abs(bestA - 1.0) > 1e-10 ? bestB / (1.0 - bestA) : 0;

            // Debug output with event info and identified parameters
            string inputType = segment.InputIndex == 0 ? "CHO" : "Insulin";
            System.Diagnostics.Debug.WriteLine(
                $"SISO Identification | Type={inputType} | Event={segment.EventTime:yyyy-MM-dd HH:mm} | " +
                $"A={bestA:G6}, B={bestB:G6}, C={bestC:G6}, Delay={bestDelay}, " +
                $"Tau={tau:F0}s ({tau/60.0:F1} min), K={k:G6}, RMSE={rmse:F3}, R2={rSquared:F3}, SSE={bestSSE:G6}");

            return new SisoResult
            {
                A = bestA,
                B = bestB,
                C = bestC,
                Delay = bestDelay,
                SSE = bestSSE,
                MSE = mse,
                RMSE = rmse,
                RSquared = rSquared,
                TimeConstant = tau,
                StaticGain = k,
                EventTime = segment.EventTime
            };
        }

        /// <summary>
        /// Fit SISO ARX model with given delay using ridge regression.
        /// Model: y[k] = a*y[k-1] + b*u[k-d] + c + e[k]
        /// </summary>
        private static (double a, double b, double c) FitSisoGivenDelay(
            double[] y, 
            double[] u, 
            int delay, 
            double ridge)
        {
            var rows = new List<double[]>();
            var rhs = new List<double>();

            for (int k = 1; k < y.Length; k++)
            {
                if (double.IsNaN(y[k]) || double.IsNaN(y[k - 1])) continue;
                
                int uIdx = k - delay;
                if (uIdx < 0 || uIdx >= u.Length) continue;
                double uVal = u[uIdx];
                if (double.IsNaN(uVal)) continue;

                var row = new double[3]; // [y_{k-1}, u_{k-d}, 1]
                row[0] = y[k - 1];
                row[1] = uVal;
                row[2] = 1.0;
                
                rows.Add(row);
                rhs.Add(y[k]);
            }

            if (rows.Count < 3) return (0, 0, 0);

            try
            {
                var M = Matrix<double>.Build.DenseOfRows(rows);
                var Y = Vector<double>.Build.DenseOfArray(rhs.ToArray());
                var MtM = M.TransposeThisAndMultiply(M);
                
                if (ridge > 0)
                    MtM += Matrix<double>.Build.DiagonalIdentity(MtM.RowCount) * ridge;
                
                var MtY = M.TransposeThisAndMultiply(Y);
                var sol = MtM.Solve(MtY);
                
                return (sol[0], sol[1], sol[2]);
            }
            catch
            {
                return (0, 0, 0);
            }
        }

        /// <summary>
        /// Compute SSE for SISO model simulation
        /// </summary>
        private static double ComputeSisoSSE(double[] y, double[] u, int delay, double a, double b, double c)
        {
            double sse = 0;
            var ySim = new double[y.Length];
            
            for (int k = 0; k < y.Length; k++)
            {
                if (k == 0)
                {
                    ySim[k] = double.IsNaN(y[k]) ? double.NaN : y[k];
                    continue;
                }

                if (double.IsNaN(ySim[k - 1]))
                {
                    ySim[k] = double.NaN;
                    continue;
                }

                int uIdx = k - delay;
                double uVal = (uIdx >= 0 && uIdx < u.Length) ? u[uIdx] : 0;
                
                ySim[k] = a * ySim[k - 1] + b * uVal + c;
                
                if (!double.IsNaN(y[k]))
                {
                    var e = y[k] - ySim[k];
                    sse += e * e;
                }
            }
            
            return sse;
        }

        /// <summary>
        /// Aggregate results from multiple SISO identifications to compute statistics
        /// </summary>
        public static AggregatedResult AggregateResults(List<SisoResult> results, string inputName)
        {
            if (results == null || results.Count == 0)
                return null;

            var validResults = results.Where(r => r != null && !double.IsNaN(r.TimeConstant) && !double.IsInfinity(r.TimeConstant)).ToList();
            if (validResults.Count == 0)
                return null;

            var taus = validResults.Select(r => r.TimeConstant).ToArray();
            var gains = validResults.Select(r => r.StaticGain).ToArray();
            var delays = validResults.Select(r => (double)r.Delay).ToArray();
            var offsets = validResults.Select(r => r.C).ToArray();
            var rSquareds = validResults.Select(r => r.RSquared).ToArray();
            var rmses = validResults.Select(r => r.RMSE).ToArray();

            return new AggregatedResult
            {
                InputName = inputName,
                TauMean = taus.Average(),
                TauStd = ComputeStdDev(taus),
                TauMedian = ComputeMedian(taus),
                GainMean = gains.Average(),
                GainStd = ComputeStdDev(gains),
                GainMedian = ComputeMedian(gains),
                DelayMean = delays.Average(),
                DelayStd = ComputeStdDev(delays),
                DelayMedian = ComputeMedian(delays),
                OffsetMean = offsets.Average(),
                OffsetStd = ComputeStdDev(offsets),
                OffsetMedian = ComputeMedian(offsets),
                AvgRSquared = rSquareds.Average(),
                AvgRMSE = rmses.Average(),
                NumSegments = validResults.Count,
                IndividualResults = validResults
            };
        }

        /// <summary>
        /// Main identification method: finds isolated segments and performs identification on each.
        /// The input lists contain segments separated by null markers.
        /// </summary>
        /// <param name="debugMode">If true, returns segment information for visualization</param>
        /// <param name="segmentInfoCallback">Callback invoked for each segment with start time for debugging</param>
        public static async Task<(AggregatedResult choResults, AggregatedResult insulinResults)> IdentifyTwoChannelsAsync(
            List<Meal> choMeals,
            List<Injection> insulinInjections,
            List<GlucoseRecord> glucoseData,
            double TsSeconds = 900.0,        // Default 15 minutes (900s) - typical CGM sampling
            double isolationHours = 7.0,
            double minDataHours = 4.0,
            int maxDelaySamples = 60,
            double ridge = 0.01,
            bool debugMode = false,
            Func<DateTime, int, string, Task>? segmentInfoCallback = null)
        {
            var choResults = new List<SisoResult>();
            var insulinResults = new List<SisoResult>();
            int segmentIndex = 0;

            // Process CHO segments (separated by null markers)
            if (choMeals != null && choMeals.Count > 0)
            {
                var currentSegment = new List<Meal>();
                
                foreach (var meal in choMeals)
                {
                    if (meal == null)
                    {
                        // End of segment - process accumulated meals
                        if (currentSegment.Count > 0)
                        {
                            var segment = CreateSegmentFromMeals(currentSegment, glucoseData, minDataHours, TsSeconds);
                            if (segment != null)
                            {
                                // Debug mode: notify about segment
                                if (debugMode && segmentInfoCallback != null)
                                {
                                    segmentIndex++;
                                    await segmentInfoCallback(segment.SegmentStart, segmentIndex, "CHO");
                                }

                                var result = IdentifySisoSegment(segment, TsSeconds, maxDelaySamples, ridge);
                                if (result != null)
                                    choResults.Add(result);
                            }
                            currentSegment.Clear();
                        }
                    }
                    else
                    {
                        currentSegment.Add(meal);
                    }
                }
                
                // Process last segment if not terminated by null
                if (currentSegment.Count > 0)
                {
                    var segment = CreateSegmentFromMeals(currentSegment, glucoseData, minDataHours, TsSeconds);
                    if (segment != null)
                    {
                        // Debug mode: notify about segment
                        if (debugMode && segmentInfoCallback != null)
                        {
                            segmentIndex++;
                            await segmentInfoCallback(segment.SegmentStart, segmentIndex, "CHO");
                        }

                        var result = IdentifySisoSegment(segment, TsSeconds, maxDelaySamples, ridge);
                        if (result != null)
                            choResults.Add(result);
                    }
                }
            }

            // Process Insulin segments (separated by null markers)
            if (insulinInjections != null && insulinInjections.Count > 0)
            {
                var currentSegment = new List<Injection>();
                
                foreach (var injection in insulinInjections)
                {
                    if (injection == null)
                    {
                        // End of segment - process accumulated injections
                        if (currentSegment.Count > 0)
                        {
                            var segment = CreateSegmentFromInjections(currentSegment, glucoseData, minDataHours, TsSeconds);
                            if (segment != null)
                            {
                                // Debug mode: notify about segment
                                if (debugMode && segmentInfoCallback != null)
                                {
                                    segmentIndex++;
                                    await segmentInfoCallback(segment.SegmentStart, segmentIndex, "Insulin");
                                }

                                var result = IdentifySisoSegment(segment, TsSeconds, maxDelaySamples, ridge);
                                if (result != null)
                                    insulinResults.Add(result);
                            }
                            currentSegment.Clear();
                        }
                    }
                    else
                    {
                        currentSegment.Add(injection);
                    }
                }
                
                // Process last segment if not terminated by null
                if (currentSegment.Count > 0)
                {
                    var segment = CreateSegmentFromInjections(currentSegment, glucoseData, minDataHours, TsSeconds);
                    if (segment != null)
                    {
                        // Debug mode: notify about segment
                        if (debugMode && segmentInfoCallback != null)
                        {
                            segmentIndex++;
                            await segmentInfoCallback(segment.SegmentStart, segmentIndex, "Insulin");
                        }

                        var result = IdentifySisoSegment(segment, TsSeconds, maxDelaySamples, ridge);
                        if (result != null)
                            insulinResults.Add(result);
                    }
                }
            }

            // Aggregate results
            var choAgg = AggregateResults(choResults, "CHO");
            var insulinAgg = AggregateResults(insulinResults, "Insulin");

            // Debug logs for aggregated outcomes
            LogAggregatedResult(choAgg);
            LogAggregatedResult(insulinAgg);

            return (choAgg, insulinAgg);
        }

        /// <summary>
        /// Synchronous version - calls async version
        /// </summary>
        public static (AggregatedResult choResults, AggregatedResult insulinResults) IdentifyTwoChannels(
            List<Meal> choMeals,
            List<Injection> insulinInjections,
            List<GlucoseRecord> glucoseData,
            double TsSeconds = 900.0,        // Default 15 minutes (900s) - typical CGM sampling
            double isolationHours = 7.0,
            double minDataHours = 4.0,
            int maxDelaySamples = 60,
            double ridge = 0.01,
            bool debugMode = false,
            Func<DateTime, int, string, Task>? segmentInfoCallback = null)
        {
            return IdentifyTwoChannelsAsync(choMeals, insulinInjections, glucoseData, 
                TsSeconds, isolationHours, minDataHours, maxDelaySamples, ridge, 
                debugMode, segmentInfoCallback).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create an IsolatedSegment from a list of meals (typically one meal per segment)
        /// </summary>
        private static IsolatedSegment CreateSegmentFromMeals(
            List<Meal> meals, 
            List<GlucoseRecord> glucoseData, 
            double minDataHours, 
            double TsSeconds)
        {
            if (meals == null || meals.Count == 0) return null;
            
            // Use first meal as reference event
            var firstMeal = meals[0];
            if (firstMeal.EventTime?.DateTime == null) return null;

            DateTime eventTime = firstMeal.EventTime.DateTime.Value;
            DateTime segmentStart = eventTime;
            DateTime segmentEnd = eventTime.AddHours(minDataHours);

            // Sum CHO from all meals in segment (usually just one)
            double totalCho = meals
                .Where(m => m.CarbohydratesGrams?.Double.HasValue == true)
                .Sum(m => m.CarbohydratesGrams.Double.Value);

            if (totalCho <= 0) return null;

            // Extract glucose data for this segment
            var glucoseSegment = glucoseData
                .Where(g => g.EventTime?.DateTime != null && 
                           g.EventTime.DateTime >= segmentStart && 
                           g.EventTime.DateTime <= segmentEnd)
                .OrderBy(g => g.EventTime.DateTime)
                .ToArray();

            // Convert to TimePoint[]
            var glucosePoints = Identification.FromGlucoseRecords(glucoseSegment.ToList());

            return new IsolatedSegment
            {
                InputIndex = 0, // CHO
                EventTime = eventTime,
                InputValue = totalCho,
                SegmentStart = segmentStart,
                SegmentEnd = segmentEnd,
                GlucoseData = glucosePoints
            };
        }

        /// <summary>
        /// Create an IsolatedSegment from a list of injections (typically one injection per segment)
        /// </summary>
        private static IsolatedSegment CreateSegmentFromInjections(
            List<Injection> injections, 
            List<GlucoseRecord> glucoseData, 
            double minDataHours, 
            double TsSeconds)
        {
            if (injections == null || injections.Count == 0) return null;
            
            // Use first injection as reference event
            var firstInjection = injections[0];
            if (firstInjection.EventTime?.DateTime == null) return null;

            DateTime eventTime = firstInjection.EventTime.DateTime.Value;
            DateTime segmentStart = eventTime;
            DateTime segmentEnd = eventTime.AddHours(minDataHours);

            // Sum insulin from all injections in segment (usually just one)
            double totalInsulin = injections
                .Where(i => i.InsulinValue?.Double.HasValue == true)
                .Sum(i => i.InsulinValue.Double.Value);

            if (totalInsulin <= 0) return null;

            // Extract glucose data for this segment
            var glucoseSegment = glucoseData
                .Where(g => g.EventTime?.DateTime != null && 
                           g.EventTime.DateTime >= segmentStart && 
                           g.EventTime.DateTime <= segmentEnd)
                .OrderBy(g => g.EventTime.DateTime)
                .ToArray();

            // Convert to TimePoint[]
            var glucosePoints = Identification.FromGlucoseRecords(glucoseSegment.ToList());

            return new IsolatedSegment
            {
                InputIndex = 1, // Insulin
                EventTime = eventTime,
                InputValue = totalInsulin,
                SegmentStart = segmentStart,
                SegmentEnd = segmentEnd,
                GlucoseData = glucosePoints
            };
        }

        private static double ComputeStdDev(double[] values)
        {
            if (values == null || values.Length < 2) return 0;
            double mean = values.Average();
            double sumSq = values.Sum(v => Math.Pow(v - mean, 2));
            return Math.Sqrt(sumSq / values.Length);
        }

        private static double ComputeMedian(double[] values)
        {
            if (values == null || values.Length == 0) return 0;
            var sorted = values.OrderBy(v => v).ToArray();
            int mid = sorted.Length / 2;
            if (sorted.Length % 2 == 0)
                return (sorted[mid - 1] + sorted[mid]) / 2.0;
            else
                return sorted[mid];
        }

        private static void LogAggregatedResult(AggregatedResult? agg)
        {
            if (agg == null)
            {
                System.Diagnostics.Debug.WriteLine("AggregatedResult: null (no valid segments)");
                return;
            }

            System.Diagnostics.Debug.WriteLine(
                $"MISO (separate dynamics) | Input={agg.InputName} | Segments={agg.NumSegments} | " +
                $"Tau(mean±std/med)={agg.TauMean:F0}±{agg.TauStd:F0}/{agg.TauMedian:F0}s, " +
                $"K(mean±std/med)={agg.GainMean:G5}±{agg.GainStd:G5}/{agg.GainMedian:G5}, " +
                $"Delay(mean±std/med)={agg.DelayMean:F1}±{agg.DelayStd:F1}/{agg.DelayMedian:F1} samp, " +
                $"Offset(mean±std/med)={agg.OffsetMean:F1}±{agg.OffsetStd:F1}/{agg.OffsetMedian:F1}, " +
                $"R2(avg)={agg.AvgRSquared:F3}, RMSE(avg)={agg.AvgRMSE:F2}");
        }
    }
}
