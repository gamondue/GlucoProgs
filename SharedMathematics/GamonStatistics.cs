 

//using Windows.ApplicationModel.Email;

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
            MeansOfAllValuesInTimeBands(IReadOnlyList<(DateTime t, double value)> Data,
            List<(DateTime Begin, DateTime End)> BandDurations)
        {
            // a value that is at the exact beginning of a band is included in the band
            // a value that is at the exact end of a band is EXCLUDED from the band
            // (to avoid double counting)
            if (Data == null || Data.Count == 0)
                return (new List<double>(), new List<double>(), new List<int>());

            var orderedValues = Data.OrderBy(d => d.t).ToList();
            var orderedBands = (BandDurations ?? new List<(DateTime Begin, DateTime End)>())
                                .OrderBy(b => b.Begin.TimeOfDay).ToList();

            // Validate that bands do not overlap (based on TimeOfDay to allow daily recurrence)
            for (int i = 0; i < orderedBands.Count - 1; i++)
            {
                if (orderedBands[i].End.TimeOfDay > orderedBands[i + 1].Begin.TimeOfDay)
                    throw new ArgumentException("Time bands must not overlap.");
            }

            // define dimensions for intemediate results : one entry per band (plus one for residuals)
            var dayMeans = new List<double>(new double[orderedBands.Count + 1]);
            var dayStdDevs = new List<double>(new double[orderedBands.Count + 1]);
            var dayCounts = new List<int>(new int[orderedBands.Count + 1]);

            // If there are no bands, all values are residuals - return them as a single group
            if (orderedBands.Count == 0)
            {
                var stats = MeanAndStdDev(orderedValues.Select(v => v.value).ToList());
                return (new List<double> { stats.Mean }, new List<double> { stats.StdDev }, new List<int> { stats.Count });
            }

            int bandIndex = 0; // index for the current band (they are ordered and not overlapping)
            // calculate the limits of the first band
            DateTime bandBegin = orderedValues[0].t.Date + orderedBands[0].Begin.TimeOfDay;
            DateTime bandEnd = orderedValues[0].t.Date + orderedBands[0].End.TimeOfDay;

            // we will use dayMeans as the sum of the daily values for each band,
            // and dayStdDevs as the sum of squared differences from the mean (to be finalized after counting)
            // resetting the values to zero to prepare for accumulation
            for (int i = 0; dayMeans.Count > i; i++)
            {
                dayMeans[i] += 0;
                dayStdDevs[i] += 0;
                dayCounts[i] += 0;
            }
            // calculations for the means
            foreach (var value in orderedValues)
            {
                // For each value, find which band it belongs to
                int assignedBand = -1;
                for (int b = 0; b < orderedBands.Count; b++)
                {
                    DateTime bBegin = value.t.Date + orderedBands[b].Begin.TimeOfDay;
                    DateTime bEnd = value.t.Date + orderedBands[b].End.TimeOfDay;
                    if (value.t >= bBegin && value.t < bEnd)
                    {
                        assignedBand = b;
                        break;
                    }
                }

                if (assignedBand >= 0)
                {
                    // Value falls in a band
                    dayMeans[assignedBand] += value.value;
                    dayCounts[assignedBand]++;
                }
                else
                {
                    // Value is a residual (falls outside all bands)
                    dayMeans[dayMeans.Count - 1] += value.value;
                    dayCounts[dayMeans.Count - 1]++;
                }
            }
            // calculate the day means 
            for (int i = 0; i < dayMeans.Count; i++)
            {
                if (dayCounts[i] > 0)
                {
                    dayMeans[i] /= dayCounts[i]; // finalize mean f or the band
                }
                else
                {
                    dayMeans[i] = double.NaN;
                }
            }
            bandIndex = 0; // index for the current band (they are ordered and not overlapping)
            // calculate the limits of the first band
            bandBegin = orderedValues[0].t.Date + orderedBands[0].Begin.TimeOfDay;
            bandEnd = orderedValues[0].t.Date + orderedBands[0].End.TimeOfDay;
            // calculations for the standard deviations (using the means calculated above)
            foreach (var value in orderedValues)
            {
                // For each value, find which band it belongs to (same logic as means calculation)
                int assignedBand = -1;
                for (int b = 0; b < orderedBands.Count; b++)
                {
                    DateTime bBegin = value.t.Date + orderedBands[b].Begin.TimeOfDay;
                    DateTime bEnd = value.t.Date + orderedBands[b].End.TimeOfDay;
                    if (value.t >= bBegin && value.t < bEnd)
                    {
                        assignedBand = b;
                        break;
                    }
                }

                if (assignedBand >= 0)
                {
                    // Value falls in a band
                    dayStdDevs[assignedBand] += Math.Pow(value.value - dayMeans[assignedBand], 2);
                }
                else
                {
                    // Value is a residual
                    dayStdDevs[dayStdDevs.Count - 1] += Math.Pow(value.value - dayMeans[dayMeans.Count - 1], 2);
                }
            }
            // calculate the std devs
            for (int i = 0; i < dayMeans.Count; i++)
            {
                if (dayCounts[i] > 0)
                {
                    dayStdDevs[i] = Math.Sqrt(dayStdDevs[i] / dayCounts[i]);
                }
                else
                {
                    dayStdDevs[i] = double.NaN;
                }
            }
            return (dayMeans, dayStdDevs, dayCounts);
        }
        public static (List<double> Means, List<double> StdDevs, List<int> Counts,
            List<double> EffectiveMeans, List<double> EffectiveStdDevs, List<int> EffectiveCounts)
            MeansOfSumsInTimeBands(IReadOnlyList<(DateTime t, double value)> Data,
                List<(DateTime Begin, DateTime End)> BandDurations)
        {
            // Aggregate values per day, then sum the values in each of the bands of the day
            // If data in one band or residue is missed, then:
            // - its value is set to 0, to have influence in the value of the overall mean
            // - a "CountOfMissing" of the band is increased 
            // Finally calculate the mean and std value across days to produce overall statistics
            // that are given back to the caller subdivided by band, like in MeansOfAllValuesInTimeBands()
            // The statistics of Data not included in any band (residuals) will be in the last row
            // of the result lists. Those corresponding to the bands will have the same index as the band
            // (after ordering by time-of-day).
            // The method also gives as an output the List "Counts" that, for each band,
            // is the number of days found in the data (equal for every band) - CountOfMissing of the band.
            // The "Effective" lists (EffectiveMeans, EffectiveStdDevs, EffectiveCounts) contain statistics
            // computed only from days that have actual data in each band (excluding zero-padded missing days).

            var orderedValues = Data.OrderBy(d => d.t).ToList();

            // If no bands provided, treat all values as residuals for each day
            var orderedBands = (BandDurations ?? new List<(DateTime Begin, DateTime End)>())
                                .OrderBy(b => b.Begin.TimeOfDay).ToList();

            // validate overlap based on TimeOfDay
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

            int nBands = orderedBands.Count;
            int nDays = days.Count;

            // dailySums[b] is a list of daily sums (one per day) for band b (includes 0 for missing days)
            // effectiveDailySums[b] is a list of daily sums only for days that have data in band b
            // last index (nBands) is for residuals
            var dailySums = new List<List<double>>();
            var effectiveDailySums = new List<List<double>>();
            for (int b = 0; b <= nBands; b++)
            {
                dailySums.Add(new List<double>());
                effectiveDailySums.Add(new List<double>());
            }

            // CountOfMissing[b] tracks how many days have no data in band b
            var countOfMissing = new int[nBands + 1];

            foreach (var dayGroup in days)
            {
                var dayDate = dayGroup.Key;
                var dayValues = dayGroup.ToList();

                var bandSums = new double[nBands + 1];
                var bandHasData = new bool[nBands + 1];

                foreach (var val in dayValues)
                {
                    int assignedBand = -1;
                    for (int b = 0; b < nBands; b++)
                    {
                        DateTime bBegin = dayDate + orderedBands[b].Begin.TimeOfDay;
                        DateTime bEnd = dayDate + orderedBands[b].End.TimeOfDay;
                        if (val.t >= bBegin && val.t < bEnd)
                        {
                            assignedBand = b;
                            break;
                        }
                    }

                    if (assignedBand >= 0)
                    {
                        bandSums[assignedBand] += val.value;
                        bandHasData[assignedBand] = true;
                    }
                    else
                    {
                        bandSums[nBands] += val.value;
                        bandHasData[nBands] = true;
                    }
                }

                // Store each day's sum for every band (0 if no values fell in that band)
                for (int b = 0; b <= nBands; b++)
                {
                    dailySums[b].Add(bandSums[b]);
                    if (!bandHasData[b])
                        countOfMissing[b]++;
                    else
                        effectiveDailySums[b].Add(bandSums[b]);
                }
            }

            // Compute mean and sample standard deviation (N-1) of daily sums across days
            var Means = new List<double>();
            var StdDevs = new List<double>();
            var Counts = new List<int>();
            var EffectiveMeans = new List<double>();
            var EffectiveStdDevs = new List<double>();
            var EffectiveCounts = new List<int>();

            for (int b = 0; b <= nBands; b++)
            {
                // All-days statistics (zero-padded for missing days)
                var values = dailySums[b];
                double mean = values.Average();
                double sumSqDiff = values.Sum(v => (v - mean) * (v - mean));
                double stdDev = values.Count > 1
                    ? Math.Sqrt(sumSqDiff / (values.Count - 1))
                    : 0;
                Means.Add(mean);
                StdDevs.Add(stdDev);
                Counts.Add(nDays - countOfMissing[b]);

                // Effective statistics (only days with actual data in the band)
                var effValues = effectiveDailySums[b];
                if (effValues.Count == 0)
                {
                    EffectiveMeans.Add(double.NaN);
                    EffectiveStdDevs.Add(double.NaN);
                    EffectiveCounts.Add(0);
                }
                else
                {
                    double effMean = effValues.Average();
                    double effSumSqDiff = effValues.Sum(v => (v - effMean) * (v - effMean));
                    double effStdDev = effValues.Count > 1
                        ? Math.Sqrt(effSumSqDiff / (effValues.Count - 1))
                        : 0;
                    EffectiveMeans.Add(effMean);
                    EffectiveStdDevs.Add(effStdDev);
                    EffectiveCounts.Add(effValues.Count);
                }
            }

            return (Means, StdDevs, Counts, EffectiveMeans, EffectiveStdDevs, EffectiveCounts);
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
            IrregularTimeIntegrationInTimeBands(IReadOnlyList<(DateTime t, double value)> Data, List<(DateTime Begin, DateTime End)> BandDurations)
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
