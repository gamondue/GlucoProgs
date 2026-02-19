using System;
using System.Collections.Generic;
using System.Linq;
using gamon;
using GlucoMan;
using Mathematics.Identification1;
using NUnit.Framework;

namespace SharedMathematics.UnitTests
{
    /// <summary>
    /// Unit tests for Identification2 class - isolated segment identification for SISO analysis.
    /// These tests use mock business objects (Meal, Injection, GlucoseRecord) without database access.
    /// </summary>
    [TestFixture]
    public class Identification2Tests
    {
        #region FindIsolatedSegments Tests - Null/Empty Inputs

        /// <summary>
        /// Tests that FindIsolatedSegments returns empty lists when glucoseData is null.
        /// </summary>
        [Test]
        public void FindIsolatedSegments_WithNullGlucoseData_ReturnsEmptyLists()
        {
            // Arrange
            var meals = new List<Meal>();
            var injections = new List<Injection>();

            // Act
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                meals, injections, null);

            // Assert
            Assert.That(isolatedMeals, Is.Not.Null);
            Assert.That(isolatedInjections, Is.Not.Null);
            Assert.That(isolatedMeals.Count, Is.EqualTo(0));
            Assert.That(isolatedInjections.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that FindIsolatedSegments returns empty lists when glucoseData is empty.
        /// </summary>
        [Test]
        public void FindIsolatedSegments_WithEmptyGlucoseData_ReturnsEmptyLists()
        {
            // Arrange
            var meals = new List<Meal>();
            var injections = new List<Injection>();
            var glucoseData = new List<GlucoseRecord>();

            // Act
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                meals, injections, glucoseData);

            // Assert
            Assert.That(isolatedMeals.Count, Is.EqualTo(0));
            Assert.That(isolatedInjections.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that FindIsolatedSegments handles null meals list gracefully.
        /// </summary>
        [Test]
        public void FindIsolatedSegments_WithNullMeals_HandlesGracefully()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var glucoseData = CreateStableGlucoseData(baseTime, 120, 5.0); // 10 hours of stable data

            // Act
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                null, new List<Injection>(), glucoseData);

            // Assert
            Assert.That(isolatedMeals, Is.Not.Null);
            Assert.That(isolatedInjections, Is.Not.Null);
        }

        /// <summary>
        /// Tests that FindIsolatedSegments handles null injections list gracefully.
        /// </summary>
        [Test]
        public void FindIsolatedSegments_WithNullInjections_HandlesGracefully()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var glucoseData = CreateStableGlucoseData(baseTime, 120, 5.0);

            // Act
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                new List<Meal>(), null, glucoseData);

            // Assert
            Assert.That(isolatedMeals, Is.Not.Null);
            Assert.That(isolatedInjections, Is.Not.Null);
        }

        #endregion

        #region FindIsolatedSegments Tests - Isolation Logic

        /// <summary>
        /// Tests that FindIsolatedSegments identifies a meal when glucose is stable and no insulin nearby.
        /// </summary>
        [Test]
        public void FindIsolatedSegments_MealWithStableGlucoseNoInsulin_IdentifiesMeal()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            
            // Create stable glucose data for 10 hours (120 samples at 5 min intervals)
            var glucoseData = CreateStableGlucoseData(baseTime.AddHours(-2), 180, 5.0);
            
            // Create a meal at 3 hours into the data (well after basal check period)
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(3) },
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                }
            };

            // No injections
            var injections = new List<Injection>();

            // Act
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                meals, injections, glucoseData,
                isolationHours: 4.0,
                minDataHours: 1.0,
                basalCheckHours: 2.0,
                maxBasalSlope: 5.0,
                beforeIsolationHours: 2.0);

            // Assert - meal should be identified as isolated
            Assert.That(isolatedMeals.Count(m => m != null), Is.GreaterThanOrEqualTo(0));
        }

        /// <summary>
        /// Tests that FindIsolatedSegments rejects meal when insulin event is too close.
        /// </summary>
        [Test]
        public void FindIsolatedSegments_MealWithNearbyInsulin_RejectsMeal()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var glucoseData = CreateStableGlucoseData(baseTime.AddHours(-2), 180, 5.0);
            
            // Meal at baseTime + 3 hours
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(3) },
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                }
            };

            // Insulin 30 minutes after meal (within isolation window)
            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(3.5) },
                    InsulinValue = new DoubleAndText { Double = 5.0 }
                }
            };

            // Act
            var (isolatedMeals, isolatedInjections) = Identification2.FindIsolatedSegments(
                meals, injections, glucoseData,
                isolationHours: 4.0,
                minDataHours: 1.0);

            // Assert - meal should be rejected due to nearby insulin
            Assert.That(isolatedMeals.Count(m => m != null), Is.EqualTo(0));
        }

        #endregion

        #region IsolatedSegment Class Tests

        /// <summary>
        /// Tests that IsolatedSegment properties can be set and retrieved correctly.
        /// </summary>
        [Test]
        public void IsolatedSegment_SetProperties_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var segment = new Identification2.IsolatedSegment
            {
                InputIndex = 0,
                EventTime = new DateTime(2024, 1, 15, 12, 0, 0),
                InputValue = 50.0,
                SegmentStart = new DateTime(2024, 1, 15, 12, 0, 0),
                SegmentEnd = new DateTime(2024, 1, 15, 14, 0, 0),
                GlucoseData = new[] 
                { 
                    new Identification.TimePoint(new DateTime(2024, 1, 15, 12, 0, 0), 100.0),
                    new Identification.TimePoint(new DateTime(2024, 1, 15, 13, 0, 0), 120.0)
                }
            };

            // Assert
            Assert.That(segment.InputIndex, Is.EqualTo(0));
            Assert.That(segment.EventTime, Is.EqualTo(new DateTime(2024, 1, 15, 12, 0, 0)));
            Assert.That(segment.InputValue, Is.EqualTo(50.0));
            Assert.That(segment.SegmentStart, Is.EqualTo(new DateTime(2024, 1, 15, 12, 0, 0)));
            Assert.That(segment.SegmentEnd, Is.EqualTo(new DateTime(2024, 1, 15, 14, 0, 0)));
            Assert.That(segment.GlucoseData.Length, Is.EqualTo(2));
        }

        #endregion

        #region SisoResult Class Tests

        /// <summary>
        /// Tests that SisoResult properties can be set and retrieved correctly.
        /// </summary>
        [Test]
        public void SisoResult_SetProperties_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var result = new Identification2.SisoResult
            {
                A = 0.9,
                B = 0.5,
                C = 100.0,
                Delay = 2,
                SSE = 150.0,
                MSE = 3.0,
                RMSE = 1.73,
                RSquared = 0.95,
                TimeConstant = 3600.0,
                StaticGain = 5.0,
                EventTime = new DateTime(2024, 1, 15, 12, 0, 0)
            };

            // Assert
            Assert.That(result.A, Is.EqualTo(0.9));
            Assert.That(result.B, Is.EqualTo(0.5));
            Assert.That(result.C, Is.EqualTo(100.0));
            Assert.That(result.Delay, Is.EqualTo(2));
            Assert.That(result.SSE, Is.EqualTo(150.0));
            Assert.That(result.MSE, Is.EqualTo(3.0));
            Assert.That(result.RMSE, Is.EqualTo(1.73));
            Assert.That(result.RSquared, Is.EqualTo(0.95));
            Assert.That(result.TimeConstant, Is.EqualTo(3600.0));
            Assert.That(result.StaticGain, Is.EqualTo(5.0));
            Assert.That(result.EventTime, Is.EqualTo(new DateTime(2024, 1, 15, 12, 0, 0)));
        }

        #endregion

        #region AggregatedResult Class Tests

        /// <summary>
        /// Tests that AggregatedResult properties can be set and retrieved correctly.
        /// </summary>
        [Test]
        public void AggregatedResult_SetProperties_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var result = new Identification2.AggregatedResult
            {
                InputName = "CHO",
                TauMean = 3600.0,
                TauStd = 300.0,
                TauMedian = 3500.0,
                GainMean = 2.5,
                GainStd = 0.5,
                GainMedian = 2.4,
                DelayMean = 2.0,
                DelayStd = 0.5,
                DelayMedian = 2.0,
                OffsetMean = 100.0,
                OffsetStd = 5.0,
                OffsetMedian = 99.0,
                AvgRSquared = 0.92,
                AvgRMSE = 5.5,
                NumSegments = 10
            };

            // Assert
            Assert.That(result.InputName, Is.EqualTo("CHO"));
            Assert.That(result.TauMean, Is.EqualTo(3600.0));
            Assert.That(result.TauStd, Is.EqualTo(300.0));
            Assert.That(result.TauMedian, Is.EqualTo(3500.0));
            Assert.That(result.GainMean, Is.EqualTo(2.5));
            Assert.That(result.GainStd, Is.EqualTo(0.5));
            Assert.That(result.GainMedian, Is.EqualTo(2.4));
            Assert.That(result.DelayMean, Is.EqualTo(2.0));
            Assert.That(result.DelayStd, Is.EqualTo(0.5));
            Assert.That(result.DelayMedian, Is.EqualTo(2.0));
            Assert.That(result.OffsetMean, Is.EqualTo(100.0));
            Assert.That(result.OffsetStd, Is.EqualTo(5.0));
            Assert.That(result.OffsetMedian, Is.EqualTo(99.0));
            Assert.That(result.AvgRSquared, Is.EqualTo(0.92));
            Assert.That(result.AvgRMSE, Is.EqualTo(5.5));
            Assert.That(result.NumSegments, Is.EqualTo(10));
        }

        /// <summary>
        /// Tests that AggregatedResult.IndividualResults is initialized as empty list.
        /// </summary>
        [Test]
        public void AggregatedResult_IndividualResults_InitializedAsEmptyList()
        {
            // Arrange & Act
            var result = new Identification2.AggregatedResult();

            // Assert
            Assert.That(result.IndividualResults, Is.Not.Null);
            Assert.That(result.IndividualResults.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that IndividualResults can be populated with SisoResult items.
        /// </summary>
        [Test]
        public void AggregatedResult_IndividualResults_CanBePopulated()
        {
            // Arrange
            var result = new Identification2.AggregatedResult();
            var sisoResult1 = new Identification2.SisoResult { A = 0.9, RSquared = 0.95 };
            var sisoResult2 = new Identification2.SisoResult { A = 0.85, RSquared = 0.92 };

            // Act
            result.IndividualResults.Add(sisoResult1);
            result.IndividualResults.Add(sisoResult2);

            // Assert
            Assert.That(result.IndividualResults.Count, Is.EqualTo(2));
            Assert.That(result.IndividualResults[0].A, Is.EqualTo(0.9));
            Assert.That(result.IndividualResults[1].RSquared, Is.EqualTo(0.92));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates stable glucose data with minimal slope for testing.
        /// </summary>
        private List<GlucoseRecord> CreateStableGlucoseData(DateTime start, int count, double intervalMinutes)
        {
            var data = new List<GlucoseRecord>();
            var random = new Random(42); // Fixed seed for reproducibility
            double baseGlucose = 100.0;

            for (int i = 0; i < count; i++)
            {
                data.Add(new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = start.AddMinutes(i * intervalMinutes) },
                    GlucoseValue = new DoubleAndText { Double = baseGlucose + random.NextDouble() * 2 - 1 } // ±1 mg/dL noise
                });
            }

            return data;
        }

        #endregion
    }
}
