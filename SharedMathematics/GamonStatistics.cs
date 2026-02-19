

namespace Mathematics
{
    public static class GamonStatistics
    {
        public static (double Mean, double StdDev, int Count) MeanAndStdDev(List<double> Values)
        {
            if (Values == null || Values.Count == 0)
                return (0, 0, 0);
            double mean = Values.Average();
            double sumOfSquaredDifferences = Values.Sum(val => Math.Pow(val - mean, 2));
            double variance = sumOfSquaredDifferences / Values.Count;
            double stdDev = Math.Sqrt(variance);
            return (mean, stdDev, Values.Count);
        }
        public static (double Integral, double IntegralAverage, double IntegralStdDev, double TotalSeconds) 
            IrregularTimeIntegration(IReadOnlyList<(DateTime t, double value)> Data)
        {
            // Calculates the definite integral on irregular temporal data, along with integral mean and standard deviation
            if (Data == null || Data.Count < 2)
                throw new ArgumentException("You needat leat two points to perform the calculation.");

                // order by time (just in case)
                var ordered = Data.OrderBy(d => d.t).ToList();

            double integral = 0.0;

            for (int i = 0; i < ordered.Count - 1; i++)
            {
                double f1 = ordered[i].value;
                double f2 = ordered[i + 1].value;

                double dt = (ordered[i + 1].t - ordered[i].t).TotalSeconds;
                // You can use TotalMilliseconds or TotalMinutes if you prefer

                integral += 0.5 * (f1 + f2) * dt;
            }

            // Calculates the total time and the integral average
            double totalSeconds = (ordered.Last().t - ordered.First().t).TotalSeconds;
            double integralAverage = totalSeconds > 0 ? integral / totalSeconds : 0;

            // Calculates the integral standard deviation on irregular temporal data
            double varianceIntegral = 0.0;
            for (int i = 0; i < ordered.Count - 1; i++)
            {
                double f1Deviation = ordered[i].value - integralAverage;
                double f2Deviation = ordered[i + 1].value - integralAverage;

                double dt = (ordered[i + 1].t - ordered[i].t).TotalSeconds;

                // Integrale di (f - integralMean)^2
                varianceIntegral += 0.5 * (f1Deviation * f1Deviation + f2Deviation * f2Deviation) * dt;
            }

            double variance = totalSeconds > 0 ? varianceIntegral / totalSeconds : 0;
            double integralStdDev = Math.Sqrt(variance);

            return (integral, integralAverage, integralStdDev, totalSeconds);
        }

        // Calculates the integral mean on irregular temporal data
        public static double IntegralAverage(IReadOnlyList<(DateTime t, double value)> Data)
        {
            var result = IrregularTimeIntegration(Data);
            return result.IntegralAverage;
        }

        // Calculates the integral standard deviation on irregular temporal data
        public static double IntegralStdDev(IReadOnlyList<(DateTime t, double value)> Data)
        {
            var result = IrregularTimeIntegration(Data);
            return result.IntegralStdDev;
        }
        public static (List<double> Means, List<double> StDevs, List<int> Counts) 
            DailyTimeBandsMeans (IReadOnlyList<(DateTime t, double value)> Data, 
            List<(DateTime Begin, DateTime End )> BandDurations)
        {
            // Aggregate values per day, then compute daily statistics per band.
            // Finally aggregate daily band statistics across days to produce overall
            // means and standard deviations per band. The residuals (values not
            // falling into any band) are handled per day and then aggregated.

            if (Data == null || Data.Count == 0)
                return (new List<double>(), new List<double>(), new List<int>());

            var orderedValues = Data.OrderBy(d => d.t).ToList();

            // If no bands provided, treat all values as residuals for each day
            var orderedBands = (BandDurations ?? new List<(DateTime Begin, DateTime End)>())
                                .OrderBy(b => b.Begin.TimeOfDay).ToList();

            // validate overlap based on time-of-day (applied to each day)
            for (int i = 0; i < orderedBands.Count - 1; i++)
            {
                // Compare using TimeOfDay to allow bands to be reused each day
                if (orderedBands[i].End.TimeOfDay > orderedBands[i + 1].Begin.TimeOfDay)
                    throw new ArgumentException("Time bands must not overlap.");
            }

            // Group data points by calendar day
            var days = orderedValues.GroupBy(d => d.t.Date)
                                    .OrderBy(g => g.Key)
                                    .ToList();

            // If data contains only a single day, delegate to TimeBandsMeans (per-value statistics)
             if (days.Count == 1)
            {
                // Single-day: compute per-value statistics across the whole day (original behavior)
                var singleMeans = new List<double>();
                var singleStdDevs = new List<double>();
                var singleCounts = new List<int>();

                // Build per-day (single day) band allocations using sequential scan semantics
                var day = days[0].Key;
                var dayPoints = orderedValues.OrderBy(p => p.t).ToList();
                var dayBandValues = new List<List<double>>();
                for (int b = 0; b < orderedBands.Count; b++) dayBandValues.Add(new List<double>());

                int currentBandIndex = 0;
                var usedIndices = new HashSet<int>();
                for (int i = 0; i < dayPoints.Count; i++)
                {
                    var p = dayPoints[i];
                    while (currentBandIndex < orderedBands.Count && p.t > (day.Date + orderedBands[currentBandIndex].End.TimeOfDay))
                    {
                        currentBandIndex++;
                    }

                    if (currentBandIndex < orderedBands.Count)
                    {
                        var bandBegin = day.Date + orderedBands[currentBandIndex].Begin.TimeOfDay;
                        var bandEnd = day.Date + orderedBands[currentBandIndex].End.TimeOfDay;
                        if (bandEnd < bandBegin) bandEnd = bandEnd.AddDays(1);

                        if (p.t >= bandBegin && p.t <= bandEnd)
                        {
                            dayBandValues[currentBandIndex].Add(p.value);
                            usedIndices.Add(i);
                        }
                    }
                }

                // compile results for bands
                for (int b = 0; b < orderedBands.Count; b++)
                {
                    if (dayBandValues[b].Count > 0)
                    {
                        var stats = MeanAndStdDev(dayBandValues[b]);
                        singleMeans.Add(stats.Mean);
                        singleStdDevs.Add(stats.StdDev);
                        singleCounts.Add(stats.Count);
                    }
                }

                // residuals
                var residualPoints = new List<double>();
                for (int i = 0; i < dayPoints.Count; i++) if (!usedIndices.Contains(i)) residualPoints.Add(dayPoints[i].value);
                if (residualPoints.Count > 0)
                {
                    var res = MeanAndStdDev(residualPoints);
                    singleMeans.Add(res.Mean);
                    singleStdDevs.Add(res.StdDev);
                    singleCounts.Add(res.Count);
                }

                return (singleMeans, singleStdDevs, singleCounts);
            }

            // For each band index, collect daily means (one entry per day that has data for that band)
            var dailyBandMeans = new List<List<double>>();
            var dailyResidualMeans = new List<double>();
            // Also collect all individual measurements across all days for each band (for stddev on single measures)
            var aggregatedBandValues = new List<List<double>>();
            var aggregatedResidualValues = new List<double>();

            // Initialize list for each band
            for (int b = 0; b < orderedBands.Count; b++)
            {
                dailyBandMeans.Add(new List<double>());
                aggregatedBandValues.Add(new List<double>());
            }

            // Process each day separately using sequential scan semantics (to avoid boundary double-counting)
            foreach (var dayGroup in days)
            {
                var day = dayGroup.Key;
                var dayPoints = dayGroup.OrderBy(p => p.t).ToList();

                // Prepare per-day lists of values per band
                var dayBandValues = new List<List<double>>();
                for (int b = 0; b < orderedBands.Count; b++) dayBandValues.Add(new List<double>());

                int currentBandIndex = 0;
                var usedIndices = new HashSet<int>();

                for (int i = 0; i < dayPoints.Count; i++)
                {
                    var p = dayPoints[i];
                    // advance band index while point is after current band end
                    while (currentBandIndex < orderedBands.Count && p.t > (day.Date + orderedBands[currentBandIndex].End.TimeOfDay))
                    {
                        currentBandIndex++;
                    }

                    if (currentBandIndex < orderedBands.Count)
                    {
                        var bandBegin = day.Date + orderedBands[currentBandIndex].Begin.TimeOfDay;
                        var bandEnd = day.Date + orderedBands[currentBandIndex].End.TimeOfDay;
                        if (bandEnd < bandBegin) bandEnd = bandEnd.AddDays(1);

                        if (p.t >= bandBegin && p.t <= bandEnd)
                        {
                            dayBandValues[currentBandIndex].Add(p.value);
                            usedIndices.Add(i);
                        }
                    }
                }

                // compute per-day means for each band and accumulate individual values
                for (int b = 0; b < orderedBands.Count; b++)
                {
                    if (dayBandValues[b].Count > 0)
                    {
                        var stats = MeanAndStdDev(dayBandValues[b]);
                        dailyBandMeans[b].Add(stats.Mean);
                        // accumulate individual measurements for overall stddev
                        aggregatedBandValues[b].AddRange(dayBandValues[b]);
                    }
                }

                // residuals: points not used in any band
                var residualPoints = new List<double>();
                for (int i = 0; i < dayPoints.Count; i++)
                {
                    if (!usedIndices.Contains(i)) residualPoints.Add(dayPoints[i].value);
                }
                if (residualPoints.Count > 0)
                {
                    var resStats = MeanAndStdDev(residualPoints);
                    dailyResidualMeans.Add(resStats.Mean);
                    aggregatedResidualValues.AddRange(residualPoints);
                }
            }

            var means = new List<double>();
            var stdDevs = new List<double>();
            var counts = new List<int>();


            // Aggregate across days: for each band, compute mean of daily means (to preserve day-aggregation semantics)
            // but compute stddev on single measurements aggregated across days
            for (int b = 0; b < orderedBands.Count; b++)
            {
                var dailyMeansForBand = dailyBandMeans[b];
                if (dailyMeansForBand.Count > 0)
                {
                    var stats = MeanAndStdDev(dailyMeansForBand);
                    means.Add(stats.Mean);
                    // stddev computed on individual measurements across all days
                    if (aggregatedBandValues[b].Count > 0)
                    {
                        var aggStats = MeanAndStdDev(aggregatedBandValues[b]);
                        stdDevs.Add(aggStats.StdDev);
                    }
                    else
                    {
                        stdDevs.Add(0);
                    }
                    // counts represent number of days contributing to the band
                    counts.Add(dailyMeansForBand.Count);
                }
            }

            // Aggregate residuals across days (as an additional "band" at the end)
            if (dailyResidualMeans.Count > 0)
            {
                var resStats = MeanAndStdDev(dailyResidualMeans);
                means.Add(resStats.Mean);
                // stddev on individual residual measurements across all days
                if (aggregatedResidualValues.Count > 0)
                {
                    var aggRes = MeanAndStdDev(aggregatedResidualValues);
                    stdDevs.Add(aggRes.StdDev);
                }
                else
                {
                    stdDevs.Add(0);
                }
                counts.Add(resStats.Count);
            }

            return (means, stdDevs, counts);
        }

        /// <summary>
        /// Calculates the integral average and integral standard deviation for values in the specified time bands
        /// using the trapezoidal rule for irregular temporal data (analogous to IrregularTimeIntegration).
        /// Also calculates residual statistics for values that don't fall in any time band.
        /// </summary>
        /// <param name="Data">Time series data with timestamps and values</param>
        /// <param name="BandDurations">List of non-overlapping time bands</param>
        /// <returns>Tuple containing lists of integral averages, integral standard deviations, and counts for each band (plus residuals)</returns>
        public static (List<double> IntegralAverages, List<double> IntegralStdDevs, List<int> Counts)
            TimeBandsIrregularTimeIntegration(IReadOnlyList<(DateTime t, double value)> Data, List<(DateTime Begin, DateTime End)> BandDurations)
        {
            if (Data == null || Data.Count == 0)
                return (new List<double>(), new List<double>(), new List<int>());

            var integralAverages = new List<double>();
            var integralStdDevs = new List<double>();
            var counts = new List<int>();
            var bandDataPoints = new List<List<(DateTime t, double value)>>(); // Stores data points for each band

            // Order the values by time (just in case)
            var orderedValues = Data.OrderBy(d => d.t).ToList();
            // Order the time bands by their beginning time (just in case)
            var orderedBands = BandDurations.OrderBy(b => b.Begin).ToList();

            // Determine if the time bands overlap and if so, throw an exception
            for (int i = 0; i < orderedBands.Count - 1; i++)
            {
                if (orderedBands[i].End > orderedBands[i + 1].Begin)
                    throw new ArgumentException("Time bands must not overlap.");
            }

            // Distribute values into time bands or residuals
            int currentBandIndex = 0;
            var residualDataPoints = new List<(DateTime t, double value)>();

            foreach (var dataPoint in orderedValues)
            {
                while (currentBandIndex < orderedBands.Count && dataPoint.t > orderedBands[currentBandIndex].End)
                {
                    currentBandIndex++;
                }

                if (currentBandIndex < orderedBands.Count &&
                    dataPoint.t >= orderedBands[currentBandIndex].Begin &&
                    dataPoint.t <= orderedBands[currentBandIndex].End)
                {
                    // Data point falls in the current time band
                    if (bandDataPoints.Count <= currentBandIndex)
                    {
                        bandDataPoints.Add(new List<(DateTime t, double value)> { dataPoint });
                    }
                    else
                    {
                        bandDataPoints[currentBandIndex].Add(dataPoint);
                    }
                }
                else
                {
                    // Data point does not fall in any time band
                    residualDataPoints.Add(dataPoint);
                }
            }

            // Calculate integral statistics for each band
            foreach (var bandData in bandDataPoints)
            {
                if (bandData.Count >= 2)
                {
                    // Sufficient points for integration
                    var stats = IrregularTimeIntegration(bandData);
                    integralAverages.Add(stats.IntegralAverage);
                    integralStdDevs.Add(stats.IntegralStdDev);
                    counts.Add(bandData.Count);
                }
                else if (bandData.Count == 1)
                {
                    // Single point: use the value itself as the average, stddev = 0
                    integralAverages.Add(bandData[0].value);
                    integralStdDevs.Add(0);
                    counts.Add(1);
                }
            }

            // Calculate integral statistics for residuals
            if (residualDataPoints.Count >= 2)
            {
                var residualStats = IrregularTimeIntegration(residualDataPoints);
                integralAverages.Add(residualStats.IntegralAverage);
                integralStdDevs.Add(residualStats.IntegralStdDev);
                counts.Add(residualDataPoints.Count);
            }
            else if (residualDataPoints.Count == 1)
            {
                integralAverages.Add(residualDataPoints[0].value);
                integralStdDevs.Add(0);
                counts.Add(1);
            }

            return (integralAverages, integralStdDevs, counts);
        }
    }
}
