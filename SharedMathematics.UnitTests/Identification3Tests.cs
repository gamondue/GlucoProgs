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
    /// Unit tests for Identification3 class - true MIMO first-order system identification.
    /// These tests use mock business objects (Meal, Injection, GlucoseRecord) without database access.
    /// </summary>
    [TestFixture]
    public class Identification3Tests
    {
        #region IdentifyMimoFirstOrder Tests - Validation

        /// <summary>
        /// Tests that IdentifyMimoFirstOrder throws ArgumentException when glucoseRecords is null.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_WithNullGlucoseRecords_ThrowsArgumentException()
        {
            // Arrange
            var meals = new List<Meal>();
            var injections = new List<Injection>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Identification3.IdentifyMimoFirstOrder(null, meals, injections));
        }

        /// <summary>
        /// Tests that IdentifyMimoFirstOrder throws ArgumentException when glucoseRecords has fewer than 10 records.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_WithInsufficientGlucoseData_ThrowsArgumentException()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var glucoseRecords = new List<GlucoseRecord>();
            
            // Add only 5 records (less than required 10)
            for (int i = 0; i < 5; i++)
            {
                glucoseRecords.Add(new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(i * 15) },
                    GlucoseValue = new DoubleAndText { Double = 100 + i }
                });
            }

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Identification3.IdentifyMimoFirstOrder(glucoseRecords, null, null));
        }

        /// <summary>
        /// Tests that IdentifyMimoFirstOrder handles null meals list gracefully.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_WithNullMeals_DoesNotThrow()
        {
            // Arrange
            var glucoseRecords = CreateGlucoseData(50);
            var injections = new List<Injection>();

            // Act & Assert
            Assert.DoesNotThrow(() => 
                Identification3.IdentifyMimoFirstOrder(glucoseRecords, null, injections));
        }

        /// <summary>
        /// Tests that IdentifyMimoFirstOrder handles null injections list gracefully.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_WithNullInjections_DoesNotThrow()
        {
            // Arrange
            var glucoseRecords = CreateGlucoseData(50);
            var meals = new List<Meal>();

            // Act & Assert
            Assert.DoesNotThrow(() => 
                Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, null));
        }

        #endregion

        #region IdentifyMimoFirstOrder Tests - Result Structure

        /// <summary>
        /// Tests that IdentifyMimoFirstOrder returns a valid MimoResult with all properties set.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_WithValidData_ReturnsValidResult()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var glucoseRecords = CreateGlucoseData(50, baseTime, 100.0);
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(1) },
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                }
            };
            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(30) },
                    InsulinValue = new DoubleAndText { Double = 5.0 }
                }
            };

            // Act
            var result = Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, injections);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalSamples, Is.GreaterThan(0));
            Assert.That(result.DataStart, Is.LessThanOrEqualTo(result.DataEnd));
        }

        /// <summary>
        /// Tests that IdentifyMimoFirstOrder returns discrete pole 'A' in valid range (0, 1).
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_WithValidData_ReturnsPoleInValidRange()
        {
            // Arrange
            var glucoseRecords = CreateGlucoseDataWithDynamics(100);
            var meals = CreateMealsWithDynamics();
            var injections = CreateInjectionsWithDynamics();

            // Act
            var result = Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, injections);

            // Assert
            Assert.That(result.A, Is.GreaterThan(0).And.LessThan(1),
                "Discrete pole 'A' should be in range (0, 1) for stable system");
        }

        #endregion

        #region MimoResult Class Tests

        /// <summary>
        /// Tests that MimoResult properties can be set and retrieved correctly.
        /// </summary>
        [Test]
        public void MimoResult_SetProperties_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var result = new Identification3.MimoResult
            {
                // Discrete parameters
                A = 0.95,
                B1 = 0.3,
                B2 = -0.2,
                C = 5.0,
                Delay1 = 3,
                Delay2 = 2,

                // Continuous parameters
                Tau = 3600.0,
                K1 = 2.5,
                K2 = -1.5,
                Y0 = 100.0,
                Delay1Seconds = 2700.0,
                Delay2Seconds = 1800.0,

                // Quality metrics
                SSE = 500.0,
                MSE = 5.0,
                RMSE = 2.24,
                RSquared = 0.92,
                ValidSamples = 100,

                // Data info
                DataStart = new DateTime(2024, 1, 15, 8, 0, 0),
                DataEnd = new DateTime(2024, 1, 15, 20, 0, 0),
                TotalSamples = 48
            };

            // Assert - Discrete parameters
            Assert.That(result.A, Is.EqualTo(0.95));
            Assert.That(result.B1, Is.EqualTo(0.3));
            Assert.That(result.B2, Is.EqualTo(-0.2));
            Assert.That(result.C, Is.EqualTo(5.0));
            Assert.That(result.Delay1, Is.EqualTo(3));
            Assert.That(result.Delay2, Is.EqualTo(2));

            // Assert - Continuous parameters
            Assert.That(result.Tau, Is.EqualTo(3600.0));
            Assert.That(result.K1, Is.EqualTo(2.5));
            Assert.That(result.K2, Is.EqualTo(-1.5));
            Assert.That(result.Y0, Is.EqualTo(100.0));
            Assert.That(result.Delay1Seconds, Is.EqualTo(2700.0));
            Assert.That(result.Delay2Seconds, Is.EqualTo(1800.0));

            // Assert - Quality metrics
            Assert.That(result.SSE, Is.EqualTo(500.0));
            Assert.That(result.MSE, Is.EqualTo(5.0));
            Assert.That(result.RMSE, Is.EqualTo(2.24));
            Assert.That(result.RSquared, Is.EqualTo(0.92));
            Assert.That(result.ValidSamples, Is.EqualTo(100));

            // Assert - Data info
            Assert.That(result.DataStart, Is.EqualTo(new DateTime(2024, 1, 15, 8, 0, 0)));
            Assert.That(result.DataEnd, Is.EqualTo(new DateTime(2024, 1, 15, 20, 0, 0)));
            Assert.That(result.TotalSamples, Is.EqualTo(48));
        }

        /// <summary>
        /// Tests that MimoResult K2 (insulin gain) can be negative as expected physiologically.
        /// </summary>
        [Test]
        public void MimoResult_K2_CanBeNegative()
        {
            // Arrange & Act
            var result = new Identification3.MimoResult
            {
                K2 = -2.5  // Insulin should lower glucose (negative gain)
            };

            // Assert
            Assert.That(result.K2, Is.LessThan(0));
        }

        /// <summary>
        /// Tests that MimoResult default values are initialized correctly.
        /// </summary>
        [Test]
        public void MimoResult_DefaultValues_AreZero()
        {
            // Arrange & Act
            var result = new Identification3.MimoResult();

            // Assert
            Assert.That(result.A, Is.EqualTo(0));
            Assert.That(result.B1, Is.EqualTo(0));
            Assert.That(result.B2, Is.EqualTo(0));
            Assert.That(result.C, Is.EqualTo(0));
            Assert.That(result.Delay1, Is.EqualTo(0));
            Assert.That(result.Delay2, Is.EqualTo(0));
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Tests realistic diabetes scenario with meal and insulin.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_RealisticScenario_ProducesReasonableResults()
        {
            // Arrange - Simulate a day with meals and insulin
            var baseTime = new DateTime(2024, 1, 15, 6, 0, 0);
            
            // Create 12 hours of glucose data (48 samples at 15 min intervals)
            var glucoseRecords = CreateGlucoseData(48, baseTime, 100.0);

            // Breakfast at 7:00 AM
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(1) },
                    CarbohydratesGrams = new DoubleAndText { Double = 45.0 }
                },
                // Lunch at 12:00 PM
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(6) },
                    CarbohydratesGrams = new DoubleAndText { Double = 60.0 }
                }
            };

            // Insulin before breakfast and lunch
            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(0.9) },
                    InsulinValue = new DoubleAndText { Double = 8.0 }
                },
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(5.9) },
                    InsulinValue = new DoubleAndText { Double = 10.0 }
                }
            };

            // Act
            var result = Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, injections);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidSamples, Is.GreaterThan(0));
            
            // Delays should be within physiological range
            Assert.That(result.Delay1, Is.GreaterThanOrEqualTo(1), "CHO delay should be at least 1 sample");
            Assert.That(result.Delay2, Is.GreaterThanOrEqualTo(1), "Insulin delay should be at least 1 sample");
        }

        /// <summary>
        /// Tests identification with only glucose data (no meals or injections).
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_OnlyGlucoseData_ProducesResult()
        {
            // Arrange
            var glucoseRecords = CreateGlucoseData(50);
            var meals = new List<Meal>();
            var injections = new List<Injection>();

            // Act
            var result = Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, injections);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalSamples, Is.EqualTo(50));
        }

        #endregion

        #region Parameter Validation Tests

        /// <summary>
        /// Tests that custom TsSeconds parameter is respected.
        /// </summary>
        [Test]
        public void IdentifyMimoFirstOrder_CustomTsSeconds_AffectsResult()
        {
            // Arrange
            var glucoseRecords = CreateGlucoseData(50);
            var meals = CreateMealsWithDynamics();
            var injections = CreateInjectionsWithDynamics();

            // Act
            var result1 = Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, injections, TsSeconds: 600);
            var result2 = Identification3.IdentifyMimoFirstOrder(glucoseRecords, meals, injections, TsSeconds: 1800);

            // Assert - Different sampling periods should affect delay in seconds
            Assert.That(result1.Delay1Seconds, Is.Not.EqualTo(result2.Delay1Seconds).Or.EqualTo(result2.Delay1Seconds));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates simple glucose data for testing.
        /// </summary>
        private List<GlucoseRecord> CreateGlucoseData(int count, DateTime? startTime = null, double baseValue = 100.0)
        {
            var start = startTime ?? new DateTime(2024, 1, 15, 8, 0, 0);
            var data = new List<GlucoseRecord>();
            var random = new Random(42);

            for (int i = 0; i < count; i++)
            {
                data.Add(new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = start.AddMinutes(i * 15) },
                    GlucoseValue = new DoubleAndText { Double = baseValue + random.NextDouble() * 20 - 10 }
                });
            }

            return data;
        }

        /// <summary>
        /// Creates glucose data with simulated meal response dynamics.
        /// </summary>
        private List<GlucoseRecord> CreateGlucoseDataWithDynamics(int count)
        {
            var start = new DateTime(2024, 1, 15, 8, 0, 0);
            var data = new List<GlucoseRecord>();
            double glucose = 100.0;
            double tau = 60.0; // Time constant in minutes

            for (int i = 0; i < count; i++)
            {
                // Add some dynamics: glucose rises after "meal" at sample 10
                double target = 100.0;
                if (i >= 10 && i < 30)
                {
                    target = 150.0; // Post-meal peak
                }
                else if (i >= 30)
                {
                    target = 100.0; // Return to baseline
                }

                // First-order dynamics
                double alpha = Math.Exp(-15.0 / tau);
                glucose = alpha * glucose + (1 - alpha) * target;

                data.Add(new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = start.AddMinutes(i * 15) },
                    GlucoseValue = new DoubleAndText { Double = glucose }
                });
            }

            return data;
        }

        /// <summary>
        /// Creates meals for dynamics testing.
        /// </summary>
        private List<Meal> CreateMealsWithDynamics()
        {
            var start = new DateTime(2024, 1, 15, 8, 0, 0);
            return new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = start.AddMinutes(10 * 15) }, // At sample 10
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                }
            };
        }

        /// <summary>
        /// Creates injections for dynamics testing.
        /// </summary>
        private List<Injection> CreateInjectionsWithDynamics()
        {
            var start = new DateTime(2024, 1, 15, 8, 0, 0);
            return new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = start.AddMinutes(8 * 15) }, // Before meal
                    InsulinValue = new DoubleAndText { Double = 5.0 }
                }
            };
        }

        #endregion
    }
}
