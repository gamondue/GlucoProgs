using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using GlucoMan;

namespace Mathematics.Identification1
{
    /// <summary>
    /// True MIMO first-order system identification with a single shared time constant.
    /// 
    /// Model (continuous time):
    ///   ?·(dy/dt) + y = k?·u?(t - d?) + k?·u?(t - d?) + y?
    /// 
    /// Transfer function form:
    ///   Y(s) = [K?/(?s+1)]·U?(s) + [K?/(?s+1)]·U?(s) + Y?/(?s+1)
    /// 
    /// Discrete-time ARX model (zero-order hold discretization):
    ///   y[k] = a·y[k-1] + b?·u?[k-d?] + b?·u?[k-d?] + c
    /// 
    /// where:
    ///   a = exp(-Ts/?)           (single shared pole for both inputs)
    ///   b? = K?·(1-a)            (discrete gain for CHO)
    ///   b? = K?·(1-a)            (discrete gain for Insulin, expected negative)
    ///   c = Y?·(1-a)             (offset term)
    ///   
    /// This differs from Identification2 which uses SEPARATE time constants for each input.
    /// Here we enforce a SINGLE shared dynamics for the glucose metabolism system.
    /// </summary>
    public static class Identification3
    {
        /// <summary>
        /// Result of true MIMO first-order identification
        /// </summary>
        public class MimoResult
        {
            // Discrete parameters
            public double A { get; set; }              // Shared discrete pole (autoregressive coefficient)
            public double B1 { get; set; }             // Discrete gain for CHO
            public double B2 { get; set; }             // Discrete gain for Insulin
            public double C { get; set; }              // Offset term
            public int Delay1 { get; set; }            // Delay for CHO in samples
            public int Delay2 { get; set; }            // Delay for Insulin in samples

            // Continuous-time physical parameters
            public double Tau { get; set; }            // Shared time constant ? (seconds)
            public double K1 { get; set; }             // Static gain for CHO (mg/dL per g CHO)
            public double K2 { get; set; }             // Static gain for Insulin (mg/dL per U, expected negative)
            public double Y0 { get; set; }             // Equilibrium glucose level (mg/dL)

            // Delay in physical units
            public double Delay1Seconds { get; set; } // CHO delay in seconds
            public double Delay2Seconds { get; set; } // Insulin delay in seconds

            // Quality metrics
            public double SSE { get; set; }            // Sum of squared errors
            public double MSE { get; set; }            // Mean squared error
            public double RMSE { get; set; }           // Root mean squared error
            public double RSquared { get; set; }       // Coefficient of determination
            public int ValidSamples { get; set; }      // Number of valid data points used

            // Data range info
            public DateTime DataStart { get; set; }
            public DateTime DataEnd { get; set; }
            public int TotalSamples { get; set; }
        }

        /// <summary>
        /// Identify true MIMO first-order system parameters.
        /// Uses grid search over delays and ridge regression for parameter estimation.
        /// </summary>
        /// <param name="glucoseRecords">Glucose measurements (output)</param>
        /// <param name="meals">CHO input events (meals)</param>
        /// <param name="injections">Insulin input events (injections)</param>
        /// <param name="TsSeconds">Sampling period in seconds (default 900 = 15 min)</param>
        /// <param name="maxDelaySamples">Maximum delay to search (default 40 = 10 hours at 15 min)</param>
        /// <param name="ridge">Ridge regularization parameter</param>
        /// <returns>Identified MIMO model parameters</returns>
        public static MimoResult IdentifyMimoFirstOrder(
            List<GlucoseRecord> glucoseRecords,
            List<Meal> meals,
            List<Injection> injections,
            double TsSeconds = 900.0,
            int maxDelaySamples = 40,
            double ridge = 0.01)
        {
            if (glucoseRecords == null || glucoseRecords.Count < 10)
                throw new ArgumentException("Insufficient glucose data for identification");

            // Convert to TimePoint arrays
            var glucosePoints = Identification.FromGlucoseRecords(glucoseRecords);
            var mealPoints = Identification.FromMeals(meals ?? new List<Meal>());
            var injectionPoints = Identification.FromInjections(injections ?? new List<Injection>());

            if (glucosePoints.Length < 10)
                throw new ArgumentException("Insufficient valid glucose data points");

            // Determine common time range
            DateTime start = glucosePoints.Min(p => p.Time);
            DateTime end = glucosePoints.Max(p => p.Time);

            // Resample all signals to uniform grid
            var y = Identification.ResampleLinear(glucosePoints, start, end, TsSeconds);
            var u1 = ResampleInputAsImpulse(mealPoints, start, end, TsSeconds);  // CHO
            var u2 = ResampleInputAsImpulse(injectionPoints, start, end, TsSeconds);  // Insulin

            int N = y.Length;

            // Grid search over delay combinations
            double bestSSE = double.PositiveInfinity;
            double bestA = 0, bestB1 = 0, bestB2 = 0, bestC = 0;
            int bestD1 = 0, bestD2 = 0;

            // Constrain delays based on physiology
            // CHO: typically 15-90 min delay (1-6 samples at 15 min)
            // Insulin: typically 15-60 min delay (1-4 samples at 15 min)
            int minD1 = 1, maxD1 = Math.Min(maxDelaySamples, 12);  // CHO: up to 3 hours
            int minD2 = 1, maxD2 = Math.Min(maxDelaySamples, 8);   // Insulin: up to 2 hours

            for (int d1 = minD1; d1 <= maxD1; d1++)
            {
                for (int d2 = minD2; d2 <= maxD2; d2++)
                {
                    var (a, b1, b2, c) = FitMimoGivenDelays(y, u1, u2, d1, d2, ridge);
                    
                    // Skip if 'a' is out of stable range
                    if (a <= 0 || a >= 1) continue;

                    double sse = ComputeMimoSSE(y, u1, u2, d1, d2, a, b1, b2, c);

                    if (sse < bestSSE)
                    {
                        bestSSE = sse;
                        bestA = a;
                        bestB1 = b1;
                        bestB2 = b2;
                        bestC = c;
                        bestD1 = d1;
                        bestD2 = d2;
                    }
                }
            }

            // Compute quality metrics
            int validSamples = CountValidSamples(y, u1, u2, bestD1, bestD2);
            double mse = validSamples > 0 ? bestSSE / validSamples : double.NaN;
            double rmse = Math.Sqrt(mse);

            // R² calculation
            var validY = y.Where(v => !double.IsNaN(v)).ToArray();
            double yMean = validY.Length > 0 ? validY.Average() : 0;
            double sst = validY.Sum(v => Math.Pow(v - yMean, 2));
            double rSquared = sst > 0 ? 1 - bestSSE / sst : 0;

            // Convert to continuous-time parameters
            double tau = Identification.AtoTimeConstant(bestA, TsSeconds);
            double oneMinusA = 1.0 - bestA;
            double k1 = Math.Abs(oneMinusA) > 1e-10 ? bestB1 / oneMinusA : 0;
            double k2 = Math.Abs(oneMinusA) > 1e-10 ? bestB2 / oneMinusA : 0;
            double y0 = Math.Abs(oneMinusA) > 1e-10 ? bestC / oneMinusA : 0;

            // Debug output
            System.Diagnostics.Debug.WriteLine(
                $"MIMO Identification Complete:\n" +
                $"  Discrete: a={bestA:G6}, b1={bestB1:G6}, b2={bestB2:G6}, c={bestC:G6}\n" +
                $"  Delays: d1={bestD1} ({bestD1 * TsSeconds / 60:F0} min), d2={bestD2} ({bestD2 * TsSeconds / 60:F0} min)\n" +
                $"  Physical: ?={tau:F0}s ({tau / 60:F1} min), K1={k1:G4}, K2={k2:G4}, Y0={y0:F1}\n" +
                $"  Quality: SSE={bestSSE:G6}, RMSE={rmse:F2}, R²={rSquared:F4}, N={validSamples}");

            return new MimoResult
            {
                // Discrete parameters
                A = bestA,
                B1 = bestB1,
                B2 = bestB2,
                C = bestC,
                Delay1 = bestD1,
                Delay2 = bestD2,

                // Continuous parameters
                Tau = tau,
                K1 = k1,
                K2 = k2,
                Y0 = y0,
                Delay1Seconds = bestD1 * TsSeconds,
                Delay2Seconds = bestD2 * TsSeconds,

                // Quality metrics
                SSE = bestSSE,
                MSE = mse,
                RMSE = rmse,
                RSquared = rSquared,
                ValidSamples = validSamples,

                // Data info
                DataStart = start,
                DataEnd = end,
                TotalSamples = N
            };
        }

        /// <summary>
        /// Resample sparse input events as impulses on a uniform grid.
        /// Each event becomes an impulse of its magnitude at the corresponding sample.
        /// </summary>
        private static double[] ResampleInputAsImpulse(
            Identification.TimePoint[] events,
            DateTime start,
            DateTime end,
            double TsSeconds)
        {
            int N = (int)Math.Floor((end - start).TotalSeconds / TsSeconds) + 1;
            var result = new double[N];

            if (events == null) return result;

            foreach (var e in events)
            {
                if (e.Time < start || e.Time > end) continue;
                int k = (int)Math.Round((e.Time - start).TotalSeconds / TsSeconds);
                if (k >= 0 && k < N)
                    result[k] += e.Value;  // Accumulate if multiple events at same sample
            }

            return result;
        }

        /// <summary>
        /// Fit MIMO ARX model with given delays using ridge regression.
        /// Model: y[k] = a·y[k-1] + b1·u1[k-d1] + b2·u2[k-d2] + c
        /// </summary>
        private static (double a, double b1, double b2, double c) FitMimoGivenDelays(
            double[] y,
            double[] u1,
            double[] u2,
            int d1,
            int d2,
            double ridge)
        {
            var rows = new List<double[]>();
            var rhs = new List<double>();

            for (int k = 1; k < y.Length; k++)
            {
                if (double.IsNaN(y[k]) || double.IsNaN(y[k - 1])) continue;

                int idx1 = k - d1;
                int idx2 = k - d2;
                if (idx1 < 0 || idx2 < 0) continue;
                if (idx1 >= u1.Length || idx2 >= u2.Length) continue;

                double u1Val = u1[idx1];
                double u2Val = u2[idx2];
                if (double.IsNaN(u1Val) || double.IsNaN(u2Val)) continue;

                // Regressor vector: [y_{k-1}, u1_{k-d1}, u2_{k-d2}, 1]
                var row = new double[] { y[k - 1], u1Val, u2Val, 1.0 };
                rows.Add(row);
                rhs.Add(y[k]);
            }

            if (rows.Count < 5)
                return (0.9, 0, 0, 0);  // Default stable pole if insufficient data

            try
            {
                var M = Matrix<double>.Build.DenseOfRows(rows);
                var Y = Vector<double>.Build.DenseOfArray(rhs.ToArray());
                var MtM = M.TransposeThisAndMultiply(M);

                if (ridge > 0)
                    MtM += Matrix<double>.Build.DiagonalIdentity(MtM.RowCount) * ridge;

                var MtY = M.TransposeThisAndMultiply(Y);
                var sol = MtM.Solve(MtY);

                return (sol[0], sol[1], sol[2], sol[3]);
            }
            catch
            {
                return (0.9, 0, 0, 0);
            }
        }

        /// <summary>
        /// Compute SSE for MIMO model simulation (one-step-ahead prediction)
        /// </summary>
        private static double ComputeMimoSSE(
            double[] y,
            double[] u1,
            double[] u2,
            int d1,
            int d2,
            double a,
            double b1,
            double b2,
            double c)
        {
            double sse = 0;

            for (int k = 1; k < y.Length; k++)
            {
                if (double.IsNaN(y[k]) || double.IsNaN(y[k - 1])) continue;

                int idx1 = k - d1;
                int idx2 = k - d2;
                if (idx1 < 0 || idx2 < 0) continue;
                if (idx1 >= u1.Length || idx2 >= u2.Length) continue;

                double u1Val = u1[idx1];
                double u2Val = u2[idx2];
                if (double.IsNaN(u1Val) || double.IsNaN(u2Val)) continue;

                double yPred = a * y[k - 1] + b1 * u1Val + b2 * u2Val + c;
                double error = y[k] - yPred;
                sse += error * error;
            }

            return sse;
        }

        /// <summary>
        /// Count valid samples used in SSE computation
        /// </summary>
        private static int CountValidSamples(
            double[] y,
            double[] u1,
            double[] u2,
            int d1,
            int d2)
        {
            int count = 0;

            for (int k = 1; k < y.Length; k++)
            {
                if (double.IsNaN(y[k]) || double.IsNaN(y[k - 1])) continue;

                int idx1 = k - d1;
                int idx2 = k - d2;
                if (idx1 < 0 || idx2 < 0) continue;
                if (idx1 >= u1.Length || idx2 >= u2.Length) continue;

                double u1Val = u1[idx1];
                double u2Val = u2[idx2];
                if (double.IsNaN(u1Val) || double.IsNaN(u2Val)) continue;

                count++;
            }

            return count;
        }

        /// <summary>
        /// Simulate the identified MIMO model for validation/plotting
        /// </summary>
        public static double[] SimulateModel(
            MimoResult model,
            double[] y0Init,  // Initial glucose value(s)
            double[] u1,      // CHO input
            double[] u2,      // Insulin input
            double TsSeconds)
        {
            int N = Math.Max(u1.Length, u2.Length);
            var ySim = new double[N];

            // Initialize with first valid measurement or model equilibrium
            ySim[0] = y0Init != null && y0Init.Length > 0 && !double.IsNaN(y0Init[0])
                ? y0Init[0]
                : model.Y0;

            for (int k = 1; k < N; k++)
            {
                int idx1 = k - model.Delay1;
                int idx2 = k - model.Delay2;

                double u1Val = (idx1 >= 0 && idx1 < u1.Length) ? u1[idx1] : 0;
                double u2Val = (idx2 >= 0 && idx2 < u2.Length) ? u2[idx2] : 0;

                ySim[k] = model.A * ySim[k - 1] + model.B1 * u1Val + model.B2 * u2Val + model.C;
            }

            return ySim;
        }

        /// <summary>
        /// Format result as human-readable string for display
        /// </summary>
        public static string FormatResult(MimoResult result, double TsSeconds = 900.0)
        {
            if (result == null) return "No result available";

            string text = "=== MIMO First-Order Identification ===\n\n";

            text += "PHYSICAL PARAMETERS:\n";
            text += $"  Time constant ? = {result.Tau:F0} s ({result.Tau / 60:F1} min)\n";
            text += $"  CHO gain K? = {result.K1:G4} mg/dL per g\n";
            text += $"  Insulin gain K? = {result.K2:G4} mg/dL per U\n";
            text += $"  Equilibrium Y? = {result.Y0:F1} mg/dL\n\n";

            text += "DELAYS:\n";
            text += $"  CHO delay = {result.Delay1Seconds / 60:F0} min ({result.Delay1} samples)\n";
            text += $"  Insulin delay = {result.Delay2Seconds / 60:F0} min ({result.Delay2} samples)\n\n";

            text += "DISCRETE PARAMETERS:\n";
            text += $"  a = {result.A:G6}\n";
            text += $"  b? = {result.B1:G6}\n";
            text += $"  b? = {result.B2:G6}\n";
            text += $"  c = {result.C:G6}\n\n";

            text += "FIT QUALITY:\n";
            text += $"  R² = {result.RSquared:F4}\n";
            text += $"  RMSE = {result.RMSE:F2} mg/dL\n";
            text += $"  Valid samples = {result.ValidSamples}\n\n";

            text += "DATA RANGE:\n";
            text += $"  {result.DataStart:yyyy-MM-dd} to {result.DataEnd:yyyy-MM-dd}\n";
            text += $"  Total samples = {result.TotalSamples}";

            return text;
        }
    }
}
