using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using GlucoMan;

namespace Mathematics.Identification1
{
    /// <summary>
    /// Small identification tools for MISO first-order dynamics (sum of first-order transfer functions)
    /// Intended to work with time-stamped input/output series (sparse) by resampling to a uniform grid.
    /// Uses MathNet.Numerics for linear algebra.
    /// </summary>
    public static class Identification
    {
        public record TimePoint(DateTime Time, double Value);

        /// <summary>
        /// Convert a collection of Meal objects to TimePoint[] using Meal.EventTime and Meal.CarbohydratesGrams.Double.
        /// Meals without timestamp or carbohydrate value are skipped.
        /// </summary>
        public static TimePoint[] FromMeals(IEnumerable<Meal> meals)
        {
            if (meals == null) return Array.Empty<TimePoint>();
            var list = new List<TimePoint>();
            foreach (var m in meals)
            {
                try
                {
                    if (m == null) continue;
                    var dt = m.EventTime?.DateTime;
                    var val = m.CarbohydratesGrams?.Double;
                    if (dt != null && dt != DateTime.MinValue && val != null)
                    {
                        list.Add(new TimePoint(dt.Value, val.Value));
                    }
                }
                catch { /* skip malformed */ }
            }
            return list.OrderBy(p => p.Time).ToArray();
        }

        /// <summary>
        /// Convert a collection of Injection objects to TimePoint[] using Injection.EventTime and insulin value.
        /// Uses InsulinCalculated if available, otherwise InsulinValue.
        /// </summary>
        public static TimePoint[] FromInjections(IEnumerable<Injection> injections)
        {
            if (injections == null) return Array.Empty<TimePoint>();
            var list = new List<TimePoint>();
            foreach (var inj in injections)
            {
                try
                {
                    if (inj == null) continue;
                    var dt = inj.EventTime?.DateTime;
                    double? val = null;
                    if (inj.InsulinCalculated != null && inj.InsulinCalculated.Double != null)
                        val = inj.InsulinCalculated.Double;
                    else if (inj.InsulinValue != null && inj.InsulinValue.Double != null)
                        val = inj.InsulinValue.Double;

                    if (dt != null && dt != DateTime.MinValue && val != null)
                        list.Add(new TimePoint(dt.Value, val.Value));
                }
                catch { }
            }
            return list.OrderBy(p => p.Time).ToArray();
        }

        /// <summary>
        /// Convert GlucoseRecord collection to TimePoint[] using EventTime and GlucoseValue.Double.
        /// Records without timestamp or glucose value are skipped.
        /// </summary>
        public static TimePoint[] FromGlucoseRecords(IEnumerable<GlucoseRecord> records)
        {
            if (records == null) return Array.Empty<TimePoint>();
            var list = new List<TimePoint>();
            foreach (var r in records)
            {
                try
                {
                    if (r == null) continue;
                    var dt = r.EventTime?.DateTime;
                    var val = r.GlucoseValue?.Double;
                    if (dt != null && dt != DateTime.MinValue && val != null)
                        list.Add(new TimePoint(dt.Value, val.Value));
                }
                catch { }
            }
            return list.OrderBy(p => p.Time).ToArray();
        }

        /// <summary>
        /// Resample (linear interpolation) a time-stamped series on a uniform grid.
        /// If the requested time lies outside the original range, returns Nan for that sample.
        /// </summary>
        public static double[] ResampleLinear(TimePoint[] series, DateTime start, DateTime end, double TsSeconds)
        {
            if (series == null || series.Length == 0)
                return Enumerable.Repeat(double.NaN, (int)Math.Floor((end - start).TotalSeconds / TsSeconds) + 1).ToArray();

            Array.Sort(series, (a, b) => a.Time.CompareTo(b.Time));
            int N = (int)Math.Floor((end - start).TotalSeconds / TsSeconds) + 1;
            var result = new double[N];

            int idx = 0; // index in series
            for (int k = 0; k < N; k++)
            {
                var t = start.AddSeconds(k * TsSeconds);
                // advance idx so series[idx].Time <= t < series[idx+1].Time
                while (idx + 1 < series.Length && series[idx + 1].Time <= t) idx++;

                if (series[idx].Time == t)
                {
                    result[k] = series[idx].Value;
                }
                else if (idx + 1 < series.Length && series[idx].Time <= t && t <= series[idx + 1].Time)
                {
                    var t0 = series[idx].Time;
                    var t1 = series[idx + 1].Time;
                    var v0 = series[idx].Value;
                    var v1 = series[idx + 1].Value;
                    if (t1 == t0)
                        result[k] = v0;
                    else
                        result[k] = v0 + (v1 - v0) * (t - t0).TotalSeconds / (t1 - t0).TotalSeconds;
                }
                else
                {
                    // outside known range -> NaN
                    result[k] = double.NaN;
                }
            }
            return result;
        }

        /// <summary>
        /// Fit ARX-first-order model given integer delays for each input (delays in samples)
        /// Model: y[k] = a*y[k-1] + sum_i b_i * u_i[k - d_i] + c + e[k]
        /// Returns (a, b[], c) solved by regularized least squares (ridge).
        /// Rows with any NaN in regressors or y are skipped.
        /// </summary>
        public static (double a, double[] b, double c) FitGivenDelays(double[] y, double[][] inputs, int[] delays, double ridge = 0.0)
        {
            if (y == null) throw new ArgumentNullException(nameof(y));
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            int m = inputs.Length;
            if (delays == null || delays.Length != m) throw new ArgumentException("delays length must match inputs length");

            var rows = new List<double[]>();
            var rhs = new List<double>();

            for (int k = 1; k < y.Length; k++)
            {
                if (double.IsNaN(y[k]) || double.IsNaN(y[k - 1])) continue; // skip if output missing
                var row = new double[1 + m + 1]; // [y_{k-1}, u1_{k-d1},..., 1]
                row[0] = y[k - 1];
                bool skip = false;
                for (int i = 0; i < m; i++)
                {
                    int idx = k - delays[i];
                    double val = (idx >= 0 && idx < inputs[i].Length) ? inputs[i][idx] : double.NaN;
                    row[1 + i] = val;
                    if (double.IsNaN(val)) { skip = true; break; }
                }
                if (skip) continue;
                row[1 + m] = 1.0;
                rows.Add(row);
                rhs.Add(y[k]);
            }

            if (rows.Count == 0)
                return (0, new double[m], 0);

            var M = Matrix<double>.Build.DenseOfRows(rows);
            var Y = Vector<double>.Build.DenseOfArray(rhs.ToArray());
            var MtM = M.TransposeThisAndMultiply(M);
            if (ridge > 0) MtM += Matrix<double>.Build.DiagonalIdentity(MtM.RowCount) * ridge;
            var MtY = M.TransposeThisAndMultiply(Y);
            var sol = MtM.Solve(MtY);
            double a = sol[0];
            double[] b = sol.SubVector(1, m).ToArray();
            double c = sol[1 + m];
            return (a, b, c);
        }

        /// <summary>
        /// Greedy grid-search for delays (integer samples). For each input it searches delay in [0..maxDelaySamples]
        /// iteratively improving delays one input at a time. Returns estimated (a, b[], c, delays[]).
        /// </summary>
        public static (double a, double[] b, double c, int[] delays) IdentifyMisoFirstOrder(
            TimePoint[] outputSeries,
            TimePoint[][] inputSeries,
            double TsSeconds,
            int maxDelaySamples = 60,
            double ridge = 0.0)
        {
            if (outputSeries == null) throw new ArgumentNullException(nameof(outputSeries));
            if (inputSeries == null) throw new ArgumentNullException(nameof(inputSeries));
            int m = inputSeries.Length;

            // establish common overlapping interval
            DateTime start = outputSeries.Min(p => p.Time);
            DateTime end = outputSeries.Max(p => p.Time);
            foreach (var inp in inputSeries)
            {
                start = start > inp.Min(p => p.Time) ? start : inp.Min(p => p.Time);
                end = end < inp.Max(p => p.Time) ? end : inp.Max(p => p.Time);
            }
            if (end <= start) throw new InvalidOperationException("No overlapping time interval between signals.");

            int N = (int)Math.Floor((end - start).TotalSeconds / TsSeconds) + 1;

            // resample output and inputs
            var y = ResampleLinear(outputSeries, start, end, TsSeconds);
            var inputs = new double[m][];
            for (int i = 0; i < m; i++) inputs[i] = ResampleLinear(inputSeries[i], start, end, TsSeconds);

            // initialize delays to zero
            var delays = Enumerable.Repeat(0, m).ToArray();

            // greedy improvement
            bool improved = true;
            double bestSse = double.PositiveInfinity;
            (double a, double[] b, double c) bestParams = (0, new double[m], 0);

            // initial fit
            bestParams = FitGivenDelays(y, inputs, delays, ridge);
            bestSse = ComputeSse(y, inputs, delays, bestParams.a, bestParams.b, bestParams.c);

            int iter = 0;
            while (improved && iter < 5)
            {
                improved = false;
                iter++;
                for (int i = 0; i < m; i++)
                {
                    int bestDi = delays[i];
                    double bestDiSse = bestSse;
                    for (int d = 0; d <= maxDelaySamples; d++)
                    {
                        var trialDelays = (int[])delays.Clone();
                        trialDelays[i] = d;
                        var p = FitGivenDelays(y, inputs, trialDelays, ridge);
                        var sse = ComputeSse(y, inputs, trialDelays, p.a, p.b, p.c);
                        if (sse < bestDiSse)
                        {
                            bestDiSse = sse;
                            bestDi = d;
                            bestParams = p;
                        }
                    }
                    if (bestDi != delays[i])
                    {
                        delays[i] = bestDi;
                        bestSse = bestDiSse;
                        improved = true;
                    }
                }
            }

            return (bestParams.a, bestParams.b, bestParams.c, delays);
        }

        /// <summary>
        /// Compute SSE between measured y and simulated model with given params and delays.
        /// Rows with NaN are ignored.
        /// </summary>
        public static double ComputeSse(double[] y, double[][] inputs, int[] delays, double a, double[] b, double c)
        {
            int N = y.Length;
            double sse = 0;
            double[] ySim = new double[N];
            for (int k = 0; k < N; k++)
            {
                if (k == 0)
                {
                    ySim[k] = double.IsNaN(y[k]) ? double.NaN : y[k];
                    continue;
                }
                if (double.IsNaN(ySim[k - 1])) { ySim[k] = double.NaN; continue; }
                double sum = 0;
                bool anyNaN = false;
                for (int i = 0; i < inputs.Length; i++)
                {
                    int idx = k - delays[i];
                    double ui = (idx >= 0 && idx < inputs[i].Length) ? inputs[i][idx] : double.NaN;
                    if (double.IsNaN(ui)) { anyNaN = true; break; }
                    sum += b[i] * ui;
                }
                if (anyNaN || double.IsNaN(ySim[k - 1])) { ySim[k] = double.NaN; continue; }
                ySim[k] = a * ySim[k - 1] + sum + c;
                if (!double.IsNaN(y[k]))
                {
                    var e = y[k] - ySim[k];
                    sse += e * e;
                }
            }
            return sse;
        }

        /// <summary>
        /// Convert 'a' (discrete coefficient) to continuous time constant T (seconds)
        /// a = exp(-Ts / T) => T = -Ts / ln(a)
        /// </summary>
        public static double AtoTimeConstant(double a, double TsSeconds)
        {
            if (a <= 0 || a >= 1) return double.PositiveInfinity;
            return -TsSeconds / Math.Log(a);
        }
    }
}
