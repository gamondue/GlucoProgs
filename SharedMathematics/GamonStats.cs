using System;
using System.Collections.Generic;
using System.Text;

namespace Mathematics
{
    internal static class GamonStats
    {
        public static (double mean, double stdDev, int count) CalculateMeanAndStdDev(List<double> values)
        {
            if (values == null || values.Count == 0)
                return (0, 0, 0);
            double mean = values.Average();
            double sumOfSquaredDifferences = values.Sum(val => Math.Pow(val - mean, 2));
            double variance = sumOfSquaredDifferences / values.Count;
            double stdDev = Math.Sqrt(variance);
            return (mean, stdDev, values.Count);
        }
        public static class IrregularTimeIntegration
        {
        // Calcola l'integrale definito su dati temporali irregolari
        public static double Integrate(IReadOnlyList<(DateTime t, double value)> data)
        {
            if (data == null || data.Count < 2)
                throw new ArgumentException("Servono almeno due punti temporali.");

            // Ordina per timestamp (non si sa mai)
            var ordered = data.OrderBy(d => d.t).ToList();

            double integral = 0.0;

            for (int i = 0; i < ordered.Count - 1; i++)
            {
                double f1 = ordered[i].value;
                double f2 = ordered[i + 1].value;

                double dt = (ordered[i + 1].t - ordered[i].t).TotalSeconds;
                // Puoi usare TotalMilliseconds o TotalMinutes se preferisci

                integral += 0.5 * (f1 + f2) * dt;
            }
            return integral;
        }

        // Media integrale su dati irregolari
        public static double IntegralAverage(IReadOnlyList<(DateTime t, double value)> data)
        {
            var ordered = data.OrderBy(d => d.t).ToList();
            double totalTime = (ordered.Last().t - ordered.First().t).TotalSeconds;

            double integral = Integrate(ordered);
            return integral / totalTime;
        }

        // Deviazione standard integrale
        public static double IntegralStdDev(IReadOnlyList<(DateTime t, double value)> data)
        {
            var ordered = data.OrderBy(d => d.t).ToList();
            double mean = IntegralAverage(ordered);

            // Funzione (f - mean)^2 sui dati
            var varianceData = ordered
                .Select(d => (d.t, value: (d.value - mean) * (d.value - mean)))
                .ToList();

            double totalTime = (ordered.Last().t - ordered.First().t).TotalSeconds;

            double varianceIntegral = Integrate(varianceData);
            double variance = varianceIntegral / totalTime;

            return Math.Sqrt(variance);
        }
    }
}
}
