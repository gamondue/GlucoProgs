using System;
using System.Collections.Generic;
using System.Linq;
using Mathematics;
using NUnit.Framework;

namespace SharedMathematics.UnitTests
{
    /// <summary>
    /// Unit tests for the GamonStatistics class.
    /// Tests statistical calculations and time-series integration methods.
    /// </summary>
    [TestFixture]
    public class GamonStatisticsTests
    {
        #region MeanAndStdDev Tests

        /// <summary>
        /// Tests that MeanAndStdDev returns (0, 0, 0) when input list is null.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithNullList_ReturnsZeros()
        {
            // Arrange
            List<double>? nullList = null;

            // Act
            var result = GamonStatistics.MeanAndStdDev(nullList!);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(0));
            Assert.That(result.StdDev, Is.EqualTo(0));
            Assert.That(result.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that MeanAndStdDev returns (0, 0, 0) when input list is empty.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithEmptyList_ReturnsZeros()
        {
            // Arrange
            var emptyList = new List<double>();

            // Act
            var result = GamonStatistics.MeanAndStdDev(emptyList);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(0));
            Assert.That(result.StdDev, Is.EqualTo(0));
            Assert.That(result.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that MeanAndStdDev correctly calculates for a single value.
        /// For a single value, mean should equal the value and stdDev should be 0.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithSingleValue_ReturnsCorrectMeanAndZeroStdDev()
        {
            // Arrange
            var values = new List<double> { 42.5 };

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(42.5));
            Assert.That(result.StdDev, Is.EqualTo(0));
            Assert.That(result.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that MeanAndStdDev correctly calculates mean for identical values.
        /// When all values are the same, stdDev should be 0.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithIdenticalValues_ReturnsCorrectMeanAndZeroStdDev()
        {
            // Arrange
            var values = new List<double> { 100.0, 100.0, 100.0, 100.0 };

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(100.0));
            Assert.That(result.StdDev, Is.EqualTo(0));
            Assert.That(result.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Tests that CalculateintegralMeanAndintegralStdDev correctly calculates for simple dataset.
        /// Dataset: [2, 4, 6] → integralMean = 4, variance = 8/3, integralStdDev ≈ 1.633
        /// </summary>
        [Test]
        public void CalculateintegralMeanAndintegralStdDev_WithSimpleDataset_ReturnsCorrectStatistics()
        {
            // Arrange
            var values = new List<double> { 2.0, 4.0, 6.0 };

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(4.0));
            Assert.That(result.StdDev, Is.EqualTo(1.632993).Within(0.00001));
            Assert.That(result.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that MeanAndStdDev correctly calculates for glucose-like values.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithGlucoseValues_ReturnsCorrectStatistics()
        {
            // Arrange - Typical blood glucose values in mg/dL
            var values = new List<double> { 95.0, 102.0, 110.0, 88.0, 115.0 };

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(102.0));
            // Actual population stdDev for this dataset is 9.7775 (not sample stdDev)
            Assert.That(result.StdDev, Is.EqualTo(9.7775).Within(0.01));
            Assert.That(result.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Tests that MeanAndStdDev handles negative values correctly.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithNegativeValues_ReturnsCorrectStatistics()
        {
            // Arrange
            var values = new List<double> { -10.0, -5.0, 0.0, 5.0, 10.0 };

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(0.0));
            Assert.That(result.StdDev, Is.EqualTo(7.071).Within(0.01));
            Assert.That(result.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Tests that MeanAndStdDev handles large dataset correctly.
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithLargeDataset_ReturnsCorrectStatistics()
        {
            // Arrange - 1000 values uniformly distributed
            var values = Enumerable.Range(1, 1000).Select(x => (double)x).ToList();

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(500.5));
            Assert.That(result.StdDev, Is.EqualTo(288.675).Within(0.01));
            Assert.That(result.Count, Is.EqualTo(1000));
        }

        /// <summary>
        /// Tests that MeanAndStdDev handles extreme values (outliers).
        /// </summary>
        [Test]
        public void MeanAndStdDev_WithOutliers_ReturnsCorrectStatistics    ()
        {
            // Arrange
            var values = new List<double> { 100.0, 100.0, 100.0, 1000.0 };

            // Act
            var result = GamonStatistics.MeanAndStdDev(values);

            // Assert
            Assert.That(result.Mean, Is.EqualTo(325.0));
            Assert.That(result.StdDev, Is.EqualTo(389.711).Within(0.01));
            Assert.That(result.Count, Is.EqualTo(4));
        }

        #endregion

        #region IrregularTimeIntegration Tests

        /// <summary>
        /// Tests that IrregularTimeIntegration throws ArgumentException when data is null.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithNullData_ThrowsArgumentException()
        {
            // Arrange
            IReadOnlyList<(DateTime t, double value)>? nullData = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                GamonStatistics.IrregularTimeIntegration(nullData!));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration throws ArgumentException when data has less than 2 points.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithSinglePoint_ThrowsArgumentException()
        {
            // Arrange
            var data = new List<(DateTime t, double value)>
            {
                (DateTime.Now, 100.0)
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                GamonStatistics.IrregularTimeIntegration(data));
            Assert.That(ex.Message, Does.Contain("at leat two points"));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration returns 0 when data has two points at same time.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithTwoPointsSameTime_ReturnsZero()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime, 150.0)
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);

            // Assert
            Assert.That(result.Integral, Is.EqualTo(0));
            Assert.That(result.TotalSeconds, Is.EqualTo(0));
            Assert.That(result.IntegralAverage, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration correctly calculates for constant function.
        /// For f(t) = 100 over 60 seconds: integral = 100 * 60 = 6000
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithConstantFunction_ReturnsCorrectIntegral()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddSeconds(60), 100.0)
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);

            // Assert
            // Trapezoidal rule: 0.5 * (100 + 100) * 60 = 6000
            Assert.That(result.Integral, Is.EqualTo(6000.0));
            Assert.That(result.IntegralAverage, Is.EqualTo(100.0)); // integralAverage should be 100
            Assert.That(result.IntegralStdDev, Is.EqualTo(0).Within(0.0001)); // integralStdDev should be 0 for constant
            Assert.That(result.TotalSeconds, Is.EqualTo(60.0));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration correctly calculates for linear function.
        /// For f(t) going from 0 to 100 over 60 seconds: integral = 0.5 * 100 * 60 = 3000
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithLinearFunction_ReturnsCorrectIntegral()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 0.0),
                (baseTime.AddSeconds(60), 100.0)
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);

            // Assert
            // Trapezoidal rule: 0.5 * (0 + 100) * 60 = 3000
            Assert.That(result.Integral, Is.EqualTo(3000.0));
            Assert.That(result.IntegralAverage, Is.EqualTo(50.0)); // integralAverage = 3000 / 60
            Assert.That(result.IntegralStdDev, Is.GreaterThan(0)); // integralStdDev should be > 0 for varying values
            Assert.That(result.TotalSeconds, Is.EqualTo(60.0));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration correctly handles irregular time intervals.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithIrregularIntervals_ReturnsCorrectIntegral()
        {
            // Arrange - Points at 0s, 10s, 15s, 30s with values 100, 110, 120, 100
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),              // t=0
                (baseTime.AddSeconds(10), 110.0), // t=10
                (baseTime.AddSeconds(15), 120.0), // t=15
                (baseTime.AddSeconds(30), 100.0)  // t=30
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);

            // Assert
            // Segment 1 (0→10): 0.5*(100+110)*10 = 1050
            // Segment 2 (10→15): 0.5*(110+120)*5 = 575
            // Segment 3 (15→30): 0.5*(120+100)*15 = 1650
            // Total: 3275
            Assert.That(result.Integral, Is.EqualTo(3275.0));
            Assert.That(result.IntegralAverage, Is.EqualTo(109.1667).Within(0.001)); // 3275 / 30
            Assert.That(result.IntegralStdDev, Is.GreaterThan(0));
            Assert.That(result.TotalSeconds, Is.EqualTo(30.0));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration correctly sorts unsorted data by time.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithUnsortedData_SortsAndIntegratesCorrectly()
        {
            // Arrange - Deliberately unsorted
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddSeconds(30), 100.0),
                (baseTime, 100.0),
                (baseTime.AddSeconds(15), 120.0),
                (baseTime.AddSeconds(10), 110.0)
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);

            // Assert - Should produce same result as sorted version
            Assert.That(result.Integral, Is.EqualTo(3275.0));
            Assert.That(result.IntegralAverage, Is.EqualTo(109.1667).Within(0.001));
            Assert.That(result.TotalSeconds, Is.EqualTo(30.0));
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration handles glucose-like time series data.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_WithGlucoseTimeSeries_ReturnsCorrectIntegral()
        {
            // Arrange - Glucose measurements every 5 minutes
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 95.0),                    // 8:00 AM
                (baseTime.AddMinutes(5), 102.0),     // 8:05 AM
                (baseTime.AddMinutes(10), 110.0),    // 8:10 AM
                (baseTime.AddMinutes(15), 105.0)     // 8:15 AM
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);

            // Assert
            // Converting to seconds: 5 min = 300 seconds
            // Segment 1: 0.5*(95+102)*300 = 29550
            // Segment 2: 0.5*(102+110)*300 = 31800
            // Segment 3: 0.5*(110+105)*300 = 32250
            // Total: 93600
            Assert.That(result.Integral, Is.EqualTo(93600.0));
            Assert.That(result.IntegralAverage, Is.EqualTo(104.0).Within(0.1)); // 93600 / 900
            Assert.That(result.IntegralStdDev, Is.GreaterThan(0).And.LessThan(20));
            Assert.That(result.TotalSeconds, Is.EqualTo(900.0)); // 15 minutes
        }

        /// <summary>
        /// Tests that IrregularTimeIntegration returns a complete tuple with all correct values.
        /// Verifies consistency between tuple return and separate IntegralAverage/IntegralStdDev methods.
        /// </summary>
        [Test]
        public void IrregularTimeIntegration_ReturnsCompleteTuple_ConsistentWithHelperMethods()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddSeconds(10), 110.0),
                (baseTime.AddSeconds(15), 120.0),
                (baseTime.AddSeconds(30), 100.0)
            };

            // Act
            var result = GamonStatistics.IrregularTimeIntegration(data);
            var averageFromMethod = GamonStatistics.IntegralAverage(data);
            var stdDevFromMethod = GamonStatistics.IntegralStdDev(data);

            // Assert - All tuple components are present and non-zero
            Assert.That(result.Integral, Is.GreaterThan(0), "Integral should be positive");
            Assert.That(result.IntegralAverage, Is.GreaterThan(0), "integralAverage should be positive");
            Assert.That(result.IntegralStdDev, Is.GreaterThanOrEqualTo(0), "integralStdDev should be non-negative");
            Assert.That(result.TotalSeconds, Is.EqualTo(30.0), "Total seconds should be 30");

            // Assert - Tuple values match helper method results
            Assert.That(result.IntegralAverage, Is.EqualTo(averageFromMethod).Within(0.0001), 
                "Tuple integralAverage should match IntegralAverage result");
            Assert.That(result.IntegralStdDev, Is.EqualTo(stdDevFromMethod).Within(0.0001), 
                "Tuple integralStdDev should match IntegralStdDev result");

            // Assert - integralAverage is integral/time as expected
            Assert.That(result.IntegralAverage, Is.EqualTo(result.Integral / result.TotalSeconds).Within(0.0001),
                "integralAverage should equal integral divided by total seconds");
        }

        #endregion

        #region IntegralAverage Tests

        /// <summary>
        /// Tests that IntegralAverage correctly calculates time-weighted average for constant function.
        /// </summary>
        [Test]
        public void IntegralAverage_WithConstantFunction_ReturnsConstantValue()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddSeconds(60), 100.0)
            };

            // Act
            double result = GamonStatistics.IntegralAverage(data);

            // Assert
            // Average should be 100.0
            Assert.That(result, Is.EqualTo(100.0));
        }

        /// <summary>
        /// Tests that IntegralAverage correctly calculates for linear increase.
        /// </summary>
        [Test]
        public void IntegralAverage_WithLinearIncrease_ReturnsCorrectAverage()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 0.0),
                (baseTime.AddSeconds(60), 100.0)
            };

            // Act
            double result = GamonStatistics.IntegralAverage(data);

            // Assert
            // Integral = 3000, time = 60 → average = 50.0
            Assert.That(result, Is.EqualTo(50.0));
        }

        /// <summary>
        /// Tests that IntegralAverage handles irregular intervals correctly.
        /// </summary>
        [Test]
        public void IntegralAverage_WithIrregularIntervals_ReturnsCorrectAverage()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddSeconds(10), 110.0),
                (baseTime.AddSeconds(15), 120.0),
                (baseTime.AddSeconds(30), 100.0)
            };

            // Act
            double result = GamonStatistics.IntegralAverage(data);

            // Assert
            // Integral = 3275, time = 30 → average = 109.1667
            Assert.That(result, Is.EqualTo(109.1667).Within(0.001));
        }

        #endregion

        #region IntegralStdDev Tests

        /// <summary>
        /// Tests that IntegralStdDev returns 0 for constant function.
        /// </summary>
        [Test]
        public void IntegralStdDev_WithConstantFunction_ReturnsZero()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddSeconds(30), 100.0),
                (baseTime.AddSeconds(60), 100.0)
            };

            // Act
            double result = GamonStatistics.IntegralStdDev(data);

            // Assert
            Assert.That(result, Is.EqualTo(0).Within(0.0001));
        }

        /// <summary>
        /// Tests that IntegralStdDev correctly calculates for varying data.
        /// </summary>
        [Test]
        public void IntegralStdDev_WithVaryingData_ReturnsCorrectStdDev()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddSeconds(60), 110.0)
            };

            // Act
            double result = GamonStatistics.IntegralStdDev(data);

            // Assert
            // integralMean = 105, variance integral should give non-zero result
            Assert.That(result, Is.GreaterThan(0));
            Assert.That(result, Is.EqualTo(5.0).Within(0.1));
        }

        /// <summary>
        /// Tests that IntegralStdDev handles glucose-like variability.
        /// </summary>
        [Test]
        public void IntegralStdDev_WithGlucoseVariability_ReturnsReasonableStdDev()
        {
            // Arrange - Glucose with some variability
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 95.0),
                (baseTime.AddMinutes(5), 102.0),
                (baseTime.AddMinutes(10), 110.0),
                (baseTime.AddMinutes(15), 105.0)
            };

            // Act
            double result = GamonStatistics.IntegralStdDev(data);

            // Assert
            // Should be in reasonable range for glucose (few mg/dL)
            Assert.That(result, Is.GreaterThan(0));
            Assert.That(result, Is.LessThan(20.0));
        }

        #endregion

        #region TimeBandsMeans Tests

        /// <summary>
        /// Tests that DailyTimeBandsMeans returns empty lists when data is null.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithNullData_ReturnsEmptyLists()
        {
            // Arrange
            IReadOnlyList<(DateTime t, double value)>? nullData = null;
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 1, 9, 0, 0))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(nullData!, bands);

            // Assert
            Assert.That(result.Means, Is.Empty);
            Assert.That(result.StDevs, Is.Empty);
            Assert.That(result.Counts, Is.Empty);
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans returns empty lists when data is empty.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithEmptyData_ReturnsEmptyLists()
        {
            // Arrange
            var emptyData = new List<(DateTime t, double value)>();
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 1, 9, 0, 0))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(emptyData, bands);

            // Assert
            Assert.That(result.Means, Is.Empty);
            Assert.That(result.StDevs, Is.Empty);
            Assert.That(result.Counts, Is.Empty);
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans throws ArgumentException when bands overlap.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithOverlappingBands_ThrowsArgumentException()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0)
            };
            var overlappingBands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(2)),         // 8:00-10:00
                (baseTime.AddHours(1), baseTime.AddHours(3)) // 9:00-11:00 (overlaps!)
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                GamonStatistics.DailyTimeBandsMeans(data, overlappingBands));
            Assert.That(ex.Message, Does.Contain("overlap"));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly calculates mean and stddev for a single band with multiple values.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithSingleBandMultipleValues_ReturnsCorrectMeanAndStdDev()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 110.0),
                (baseTime.AddMinutes(30), 120.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)) // 8:00-9:00
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means.Count, Is.EqualTo(1));
            Assert.That(result.Means[0], Is.EqualTo(110.0)); // (100+110+120)/3
            Assert.That(result.StDevs[0], Is.EqualTo(8.1649).Within(0.001)); // Population stddev
            Assert.That(result.Counts[0], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly calculates means and stddevs for multiple non-overlapping bands.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithMultipleBands_ReturnsCorrectMeansAndStdDevsForEachBand()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                // Band 1 values (8:00-9:00)
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 110.0),
                // Band 2 values (10:00-11:00)
                (baseTime.AddHours(2).AddMinutes(10), 200.0),
                (baseTime.AddHours(2).AddMinutes(20), 220.0),
                (baseTime.AddHours(2).AddMinutes(30), 240.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)),                // 8:00-9:00
                (baseTime.AddHours(2), baseTime.AddHours(3))     // 10:00-11:00
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means.Count, Is.EqualTo(2));
            Assert.That(result.Means[0], Is.EqualTo(105.0)); // (100+110)/2
            Assert.That(result.StDevs[0], Is.EqualTo(5.0)); // stddev of [100, 110]
            Assert.That(result.Counts[0], Is.EqualTo(2));
            Assert.That(result.Means[1], Is.EqualTo(220.0)); // (200+220+240)/3
            Assert.That(result.StDevs[1], Is.EqualTo(16.3299).Within(0.001)); // stddev of [200, 220, 240]
            Assert.That(result.Counts[1], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly handles residual values outside all bands.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithResidualValues_IncludesResidualMeanAndStdDev()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                // Inside band
                (baseTime.AddMinutes(30), 100.0),
                // Outside band (residuals)
                (baseTime.AddHours(2), 200.0),
                (baseTime.AddHours(3), 300.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)) // 8:00-9:00
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means.Count, Is.EqualTo(2)); // 1 band + 1 residual
            Assert.That(result.Means[0], Is.EqualTo(100.0)); // Band mean
            Assert.That(result.StDevs[0], Is.EqualTo(0)); // Single value, stddev = 0
            Assert.That(result.Counts[0], Is.EqualTo(1));
            Assert.That(result.Means[1], Is.EqualTo(250.0)); // Residual mean (200+300)/2
            Assert.That(result.StDevs[1], Is.EqualTo(50.0)); // Residual stddev
            Assert.That(result.Counts[1], Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans handles values on band boundaries correctly.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithValuesOnBoundaries_IncludesThemInBand()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),                  // Exactly at band start
                (baseTime.AddMinutes(30), 110.0),
                (baseTime.AddHours(1), 120.0)       // Exactly at band end
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)) // 8:00-9:00
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means.Count, Is.EqualTo(1));
            Assert.That(result.Means[0], Is.EqualTo(110.0)); // (100+110+120)/3
            Assert.That(result.StDevs[0], Is.EqualTo(8.1649).Within(0.001));
            Assert.That(result.Counts[0], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly sorts unsorted data before processing.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithUnsortedData_SortsAndProcessesCorrectly()
        {
            // Arrange - Deliberately unsorted data
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(30), 120.0),
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 110.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means[0], Is.EqualTo(110.0)); // Same result regardless of input order
            Assert.That(result.StDevs[0], Is.EqualTo(8.1649).Within(0.001));
            Assert.That(result.Counts[0], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly sorts unsorted bands before processing.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithUnsortedBands_SortsAndProcessesCorrectly()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddHours(2).AddMinutes(10), 200.0)
            };
            // Deliberately unsorted bands
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime.AddHours(2), baseTime.AddHours(3)),  // 10:00-11:00 (should be second)
                (baseTime, baseTime.AddHours(1))               // 8:00-9:00 (should be first)
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert - Means should be ordered by band time
            Assert.That(result.Means.Count, Is.EqualTo(2));
            Assert.That(result.Means[0], Is.EqualTo(100.0)); // First band (8:00-9:00)
            Assert.That(result.StDevs[0], Is.EqualTo(0)); // Single value
            Assert.That(result.Means[1], Is.EqualTo(200.0)); // Second band (10:00-11:00)
            Assert.That(result.StDevs[1], Is.EqualTo(0)); // Single value
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans handles empty bands (no values fall in band).
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithEmptyBands_HandlesCorrectly()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddHours(5), 100.0) // Value outside any band
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)), // 8:00-9:00 - no values
                (baseTime.AddHours(2), baseTime.AddHours(3)) // 10:00-11:00 - no values
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert - Only residual should be present
            Assert.That(result.Means.Count, Is.EqualTo(1));
            Assert.That(result.Means[0], Is.EqualTo(100.0)); // Residual mean
            Assert.That(result.StDevs[0], Is.EqualTo(0)); // Single value
            Assert.That(result.Counts[0], Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans handles glucose meal tracking scenario.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithGlucoseMealScenario_ReturnsCorrectMeansAndStdDevs()
        {
            // Arrange - Typical meal tracking: pre-meal band and post-meal band
            var mealTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                // Pre-meal readings (30 min before)
                (mealTime.AddMinutes(-30), 95.0),
                (mealTime.AddMinutes(-15), 98.0),
                // Post-meal readings (2 hours after)
                (mealTime.AddHours(1), 140.0),
                (mealTime.AddHours(2), 125.0),
                // Late reading (outside bands)
                (mealTime.AddHours(4), 105.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (mealTime.AddMinutes(-30), mealTime),         // Pre-meal band
                (mealTime.AddHours(1), mealTime.AddHours(2))  // Post-meal band
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means.Count, Is.EqualTo(3)); // 2 bands + residual
            Assert.That(result.Means[0], Is.EqualTo(96.5).Within(0.01)); // Pre-meal mean
            Assert.That(result.StDevs[0], Is.EqualTo(1.5)); // Pre-meal stddev
            Assert.That(result.Counts[0], Is.EqualTo(2));
            Assert.That(result.Means[1], Is.EqualTo(132.5).Within(0.01)); // Post-meal mean
            Assert.That(result.StDevs[1], Is.EqualTo(7.5)); // Post-meal stddev
            Assert.That(result.Counts[1], Is.EqualTo(2));
            Assert.That(result.Means[2], Is.EqualTo(105.0)); // Residual (late reading)
            Assert.That(result.StDevs[2], Is.EqualTo(0)); // Single value
            Assert.That(result.Counts[2], Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans handles empty band list with all values becoming residuals.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithEmptyBandList_AllValuesAreResiduals()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddHours(1), 200.0),
                (baseTime.AddHours(2), 150.0)
            };
            var emptyBands = new List<(DateTime Begin, DateTime End)>();

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, emptyBands);

            // Assert - All values should be residuals
            Assert.That(result.Means.Count, Is.EqualTo(1));
            Assert.That(result.Means[0], Is.EqualTo(150.0)); // (100+200+150)/3
            Assert.That(result.StDevs[0], Is.EqualTo(40.8248).Within(0.001)); // stddev of [100, 200, 150]
            Assert.That(result.Counts[0], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans handles adjacent bands correctly (no gap between them).
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithAdjacentBands_ProcessesCorrectly()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(30), 100.0),   // In band 1
                (baseTime.AddHours(1), 150.0),      // On boundary (end of band 1 = start of band 2)
                (baseTime.AddHours(1).AddMinutes(30), 200.0)  // In band 2
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)),                    // 8:00-9:00
                (baseTime.AddHours(1), baseTime.AddHours(2))         // 9:00-10:00
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert - Boundary value should be in first band (based on <= check)
            Assert.That(result.Means.Count, Is.EqualTo(2));
            Assert.That(result.Means[0], Is.EqualTo(125.0)); // (100+150)/2
            Assert.That(result.StDevs[0], Is.EqualTo(25.0)); // stddev of [100, 150]
            Assert.That(result.Counts[0], Is.EqualTo(2));
            Assert.That(result.Means[1], Is.EqualTo(200.0));
            Assert.That(result.StDevs[1], Is.EqualTo(0)); // Single value
            Assert.That(result.Counts[1], Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly calculates stddev for identical values (should be 0).
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithIdenticalValuesInBand_ReturnsZeroStdDev()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 100.0),
                (baseTime.AddMinutes(30), 100.0),
                (baseTime.AddMinutes(40), 100.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means[0], Is.EqualTo(100.0));
            Assert.That(result.StDevs[0], Is.EqualTo(0)); // All values identical
            Assert.That(result.Counts[0], Is.EqualTo(4));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans correctly calculates stddev for high variability data.
        /// </summary>
        [Test]
        public void TimeBandsMeans_WithHighVariability_ReturnsCorrectStdDev()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 50.0),
                (baseTime.AddMinutes(20), 150.0),
                (baseTime.AddMinutes(30), 50.0),
                (baseTime.AddMinutes(40), 150.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            Assert.That(result.Means[0], Is.EqualTo(100.0)); // (50+150+50+150)/4
            Assert.That(result.StDevs[0], Is.EqualTo(50.0)); // High variability
            Assert.That(result.Counts[0], Is.EqualTo(4));
        }
        [Test]
        public void TimeBandsMeans_WithMoreThanOneDay()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 0, 0, 0);
            var currentTime = baseTime;
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddHours(6), 0.0),
                (baseTime.AddHours(7), 2.0),
                (baseTime.AddHours(8.5), 2.0),
                (baseTime.AddHours(10), 4.0),
                (baseTime.AddHours(11.5), 1),
                (baseTime.AddHours(17), 6),
                (baseTime.AddHours(18.5), -1),
                (baseTime.AddHours(19), 3),
                (baseTime.AddHours(24 + 5.5), 3),
                (baseTime.AddHours(24 + 9), 1),
                (baseTime.AddHours(24 + 10.5), 0),
                (baseTime.AddHours(24 + 14), 6),
                (baseTime.AddHours(24 + 16), 0),
                (baseTime.AddHours(24 + 16), 12),
                (baseTime.AddHours(24 + 23), 3),
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime.AddHours(8), baseTime.AddHours(11)),
                (baseTime.AddHours(16), baseTime.AddHours(18))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            // Band 0 daily means: day1 = 3, day2 = 0.5 → average = 1.75
            Assert.That(result.Means[0], Is.EqualTo(1.75));
            Assert.That(result.Means[1], Is.EqualTo(6));
            // Residual daily means: day1 = 1, day2 = 4 → average = 2.5
            Assert.That(result.Means[2], Is.EqualTo(2.5));
            // stddev computed on individual measurements across all days
            Assert.That(result.StDevs[0], Is.EqualTo(1.47902).Within(0.001));
            Assert.That(result.StDevs[1], Is.EqualTo(4.89898).Within(0.001));
            Assert.That(result.StDevs[2], Is.EqualTo(2.02722).Within(0.001));
            Assert.That(result.Counts[0], Is.EqualTo(2));
            Assert.That(result.Counts[1], Is.EqualTo(2));
            Assert.That(result.Counts[2], Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that DailyTimeBandsMeans returns consistent tuple structure with 3 lists of equal length.
        /// </summary>
        [Test]
        public void TimeBandsMeans_AlwaysReturnsConsistentTupleStructure()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddHours(2).AddMinutes(10), 200.0),
                (baseTime.AddHours(5), 300.0) // Residual
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)),
                (baseTime.AddHours(2), baseTime.AddHours(3))
            };

            // Act
            var result = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert - All three lists should have the same length
            Assert.That(result.Means.Count, Is.EqualTo(result.StDevs.Count));
            Assert.That(result.Means.Count, Is.EqualTo(result.Counts.Count));
            Assert.That(result.Means.Count, Is.EqualTo(3)); // 2 bands + 1 residual
        }

        #endregion

        #region TimeBandsIrregularTimeIntegration Tests

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration returns empty lists when data is null.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithNullData_ReturnsEmptyLists()
        {
            // Arrange
            IReadOnlyList<(DateTime t, double value)>? nullData = null;
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 1, 9, 0, 0))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(nullData!, bands);

            // Assert
            Assert.That(result.IntegralAverages, Is.Empty);
            Assert.That(result.IntegralStdDevs, Is.Empty);
            Assert.That(result.Counts, Is.Empty);
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration returns empty lists when data is empty.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithEmptyData_ReturnsEmptyLists()
        {
            // Arrange
            var emptyData = new List<(DateTime t, double value)>();
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (new DateTime(2024, 1, 1, 8, 0, 0), new DateTime(2024, 1, 1, 9, 0, 0))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(emptyData, bands);

            // Assert
            Assert.That(result.IntegralAverages, Is.Empty);
            Assert.That(result.IntegralStdDevs, Is.Empty);
            Assert.That(result.Counts, Is.Empty);
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration throws ArgumentException when bands overlap.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithOverlappingBands_ThrowsArgumentException()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),
                (baseTime.AddMinutes(30), 110.0)
            };
            var overlappingBands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(2)),
                (baseTime.AddHours(1), baseTime.AddHours(3))
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                GamonStatistics.TimeBandsIrregularTimeIntegration(data, overlappingBands));
            Assert.That(ex.Message, Does.Contain("overlap"));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration correctly calculates integral average for constant function in a band.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithConstantFunctionInBand_ReturnsConstantValue()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 100.0),
                (baseTime.AddMinutes(30), 100.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(1));
            Assert.That(result.IntegralAverages[0], Is.EqualTo(100.0));
            Assert.That(result.IntegralStdDevs[0], Is.EqualTo(0).Within(0.0001)); // Constant function has 0 stddev
            Assert.That(result.Counts[0], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration correctly calculates integral average for linear function.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithLinearFunctionInBand_ReturnsCorrectIntegralAverage()
        {
            // Arrange - Linear increase from 0 to 100 over 60 seconds
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 0.0),
                (baseTime.AddMinutes(11), 100.0) // 60 seconds later
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert
            // Integral average of linear function from 0 to 100 should be 50
            Assert.That(result.IntegralAverages[0], Is.EqualTo(50.0));
            Assert.That(result.IntegralStdDevs[0], Is.GreaterThan(0)); // Varying values should have stddev > 0
            Assert.That(result.Counts[0], Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration handles multiple bands with different data.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithMultipleBands_ReturnsCorrectIntegralAveragesForEachBand()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                // Band 1 values (8:00-9:00): constant at 100
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 100.0),
                (baseTime.AddMinutes(30), 100.0),
                // Band 2 values (10:00-11:00): linear from 200 to 240
                (baseTime.AddHours(2).AddMinutes(10), 200.0),
                (baseTime.AddHours(2).AddMinutes(20), 220.0),
                (baseTime.AddHours(2).AddMinutes(30), 240.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)),
                (baseTime.AddHours(2), baseTime.AddHours(3))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(2));
            Assert.That(result.IntegralAverages[0], Is.EqualTo(100.0)); // Constant function
            Assert.That(result.IntegralStdDevs[0], Is.EqualTo(0).Within(0.0001));
            Assert.That(result.IntegralAverages[1], Is.EqualTo(220.0)); // Integral average of linear function
            Assert.That(result.IntegralStdDevs[1], Is.GreaterThan(0));
            Assert.That(result.Counts[0], Is.EqualTo(3));
            Assert.That(result.Counts[1], Is.EqualTo(3));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration correctly handles residual values.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithResidualValues_IncludesResidualIntegralStats()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                // Inside band
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 100.0),
                // Outside band (residuals)
                (baseTime.AddHours(2), 200.0),
                (baseTime.AddHours(3), 300.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(2)); // 1 band + 1 residual
            Assert.That(result.IntegralAverages[0], Is.EqualTo(100.0)); // Band: constant
            Assert.That(result.IntegralStdDevs[0], Is.EqualTo(0).Within(0.0001));
            Assert.That(result.IntegralAverages[1], Is.EqualTo(250.0)); // Residual: linear from 200 to 300
            Assert.That(result.IntegralStdDevs[1], Is.GreaterThan(0));
            Assert.That(result.Counts[1], Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration handles single point in a band.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithSinglePointInBand_ReturnsValueAsAverage()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(30), 150.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert - Single point: value is the average, stddev is 0
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(1));
            Assert.That(result.IntegralAverages[0], Is.EqualTo(150.0));
            Assert.That(result.IntegralStdDevs[0], Is.EqualTo(0));
            Assert.That(result.Counts[0], Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration handles irregular time intervals correctly.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithIrregularIntervals_ReturnsCorrectIntegralAverage()
        {
            // Arrange - Points at irregular intervals with different weights
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),              // t=0
                (baseTime.AddSeconds(10), 110.0), // t=10s
                (baseTime.AddSeconds(15), 120.0), // t=15s
                (baseTime.AddSeconds(30), 100.0)  // t=30s
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert
            // Integral = 3275, TotalSeconds = 30, IntegralAverage = 109.1667
            Assert.That(result.IntegralAverages[0], Is.EqualTo(109.1667).Within(0.001));
            Assert.That(result.IntegralStdDevs[0], Is.GreaterThan(0));
            Assert.That(result.Counts[0], Is.EqualTo(4));
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration handles glucose meal tracking scenario.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_WithGlucoseMealScenario_ReturnsCorrectIntegralStats()
        {
            // Arrange - Typical meal tracking with CGM-like data (irregular readings)
            var mealTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                // Pre-meal readings (30 min before) - relatively stable
                (mealTime.AddMinutes(-30), 95.0),
                (mealTime.AddMinutes(-15), 98.0),
                (mealTime, 100.0),
                // Post-meal readings (2 hours after) - glucose spike and return
                (mealTime.AddHours(1), 140.0),
                (mealTime.AddMinutes(90), 160.0),
                (mealTime.AddHours(2), 125.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (mealTime.AddMinutes(-30), mealTime),         // Pre-meal band
                (mealTime.AddHours(1), mealTime.AddHours(2))  // Post-meal band
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(2));
            // Pre-meal: nearly constant around 97
            Assert.That(result.IntegralAverages[0], Is.EqualTo(97.5).Within(1.0));
            Assert.That(result.IntegralStdDevs[0], Is.LessThan(5)); // Low variability pre-meal
            // Post-meal: higher variability glucose spike
            Assert.That(result.IntegralAverages[1], Is.GreaterThan(130).And.LessThan(150));
            Assert.That(result.IntegralStdDevs[1], Is.GreaterThan(result.IntegralStdDevs[0])); // Higher variability post-meal
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration returns consistent tuple structure.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_AlwaysReturnsConsistentTupleStructure()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 110.0),
                (baseTime.AddHours(2).AddMinutes(10), 200.0),
                (baseTime.AddHours(2).AddMinutes(20), 210.0),
                (baseTime.AddHours(5), 300.0), // Residual
                (baseTime.AddHours(6), 310.0)  // Residual
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)),
                (baseTime.AddHours(2), baseTime.AddHours(3))
            };

            // Act
            var result = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);

            // Assert - All three lists should have the same length
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(result.IntegralStdDevs.Count));
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(result.Counts.Count));
            Assert.That(result.IntegralAverages.Count, Is.EqualTo(3)); // 2 bands + 1 residual
        }

        /// <summary>
        /// Tests that TimeBandsIrregularTimeIntegration results are consistent with IrregularTimeIntegration
        /// when called on the same data subset.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_ResultsConsistentWithIrregularTimeIntegration()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime.AddMinutes(10), 100.0),
                (baseTime.AddMinutes(20), 110.0),
                (baseTime.AddMinutes(30), 120.0),
                (baseTime.AddMinutes(40), 100.0)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1)) // Single band containing all data
            };

            // Act
            var bandResult = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);
            var directResult = GamonStatistics.IrregularTimeIntegration(data);

            // Assert - Results should match
            Assert.That(bandResult.IntegralAverages[0], Is.EqualTo(directResult.IntegralAverage).Within(0.0001));
            Assert.That(bandResult.IntegralStdDevs[0], Is.EqualTo(directResult.IntegralStdDev).Within(0.0001));
            Assert.That(bandResult.Counts[0], Is.EqualTo(data.Count));
        }

        /// <summary>
        /// Tests comparison between arithmetic mean (DailyTimeBandsMeans) and integral average (TimeBandsIrregularTimeIntegration)
        /// for irregularly spaced data - they should differ when spacing is uneven.
        /// </summary>
        [Test]
        public void TimeBandsIrregularTimeIntegration_DiffersFromArithmeticMean_WithIrregularSpacing()
        {
            // Arrange - Data with very uneven spacing
            // Value 100 held for 90 seconds, then jumps to 200 for only 10 seconds
            var baseTime = new DateTime(2024, 1, 1, 8, 0, 0);
            var data = new List<(DateTime t, double value)>
            {
                (baseTime, 100.0),                   // t=0
                (baseTime.AddSeconds(90), 100.0),    // t=90s (100 for 90 seconds)
                (baseTime.AddSeconds(100), 200.0)    // t=100s (200 for only 10 seconds)
            };
            var bands = new List<(DateTime Begin, DateTime End)>
            {
                (baseTime, baseTime.AddHours(1))
            };

            // Act
            var integralResult = GamonStatistics.TimeBandsIrregularTimeIntegration(data, bands);
            var arithmeticResult = GamonStatistics.DailyTimeBandsMeans(data, bands);

            // Assert
            // Arithmetic mean: (100 + 100 + 200) / 3 = 133.33
            Assert.That(arithmeticResult.Means[0], Is.EqualTo(133.33).Within(0.1));

            // Integral average should be weighted by time:
            // 100 * 90s + 150 * 10s (trapezoidal) = 9000 + 1500 = 10500
            // Total time = 100s, so integral average ≈ 105
            Assert.That(integralResult.IntegralAverages[0], Is.EqualTo(105.0).Within(0.1));

            // They should be different!
            Assert.That(integralResult.IntegralAverages[0], Is.Not.EqualTo(arithmeticResult.Means[0]).Within(1.0));
        }

        #endregion
    }
}
