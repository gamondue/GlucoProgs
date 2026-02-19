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
    /// Unit tests for Identification1 class - time series conversion and resampling.
    /// These tests use mock business objects (Meal, Injection, GlucoseRecord) without database access.
    /// </summary>
    [TestFixture]
    public class Identification1Tests
    {
        #region FromMeals Tests

        /// <summary>
        /// Tests that FromMeals returns empty array when input is null.
        /// </summary>
        [Test]
        public void FromMeals_WithNullInput_ReturnsEmptyArray()
        {
            // Act
            var result = Identification.FromMeals(null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that FromMeals returns empty array when input list is empty.
        /// </summary>
        [Test]
        public void FromMeals_WithEmptyList_ReturnsEmptyArray()
        {
            // Arrange
            var emptyList = new List<Meal>();

            // Act
            var result = Identification.FromMeals(emptyList);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that FromMeals correctly converts valid meals to TimePoints.
        /// </summary>
        [Test]
        public void FromMeals_WithValidMeals_ReturnsCorrectTimePoints()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 12, 0, 0);
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                },
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(4) },
                    CarbohydratesGrams = new DoubleAndText { Double = 60.0 }
                }
            };

            // Act
            var result = Identification.FromMeals(meals);

            // Assert
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Time, Is.EqualTo(baseTime));
            Assert.That(result[0].Value, Is.EqualTo(50.0));
            Assert.That(result[1].Time, Is.EqualTo(baseTime.AddHours(4)));
            Assert.That(result[1].Value, Is.EqualTo(60.0));
        }

        /// <summary>
        /// Tests that FromMeals skips meals without valid timestamp.
        /// </summary>
        [Test]
        public void FromMeals_WithMissingTimestamp_SkipsInvalidMeals()
        {
            // Arrange
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = DateTime.MinValue },
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                },
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = new DateTime(2024, 1, 15, 12, 0, 0) },
                    CarbohydratesGrams = new DoubleAndText { Double = 60.0 }
                }
            };

            // Act
            var result = Identification.FromMeals(meals);

            // Assert
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(60.0));
        }

        /// <summary>
        /// Tests that FromMeals skips meals without valid carbohydrate value.
        /// </summary>
        [Test]
        public void FromMeals_WithMissingCarbs_SkipsInvalidMeals()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 12, 0, 0);
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    CarbohydratesGrams = new DoubleAndText { Double = null }
                },
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(1) },
                    CarbohydratesGrams = new DoubleAndText { Double = 45.0 }
                }
            };

            // Act
            var result = Identification.FromMeals(meals);

            // Assert
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(45.0));
        }

        /// <summary>
        /// Tests that FromMeals returns meals in chronological order.
        /// </summary>
        [Test]
        public void FromMeals_WithUnorderedMeals_ReturnsSortedByTime()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 12, 0, 0);
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(3) },
                    CarbohydratesGrams = new DoubleAndText { Double = 70.0 }
                },
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
                },
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(1) },
                    CarbohydratesGrams = new DoubleAndText { Double = 60.0 }
                }
            };

            // Act
            var result = Identification.FromMeals(meals);

            // Assert
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Value, Is.EqualTo(50.0));
            Assert.That(result[1].Value, Is.EqualTo(60.0));
            Assert.That(result[2].Value, Is.EqualTo(70.0));
        }

        #endregion

        #region FromInjections Tests

        /// <summary>
        /// Tests that FromInjections returns empty array when input is null.
        /// </summary>
        [Test]
        public void FromInjections_WithNullInput_ReturnsEmptyArray()
        {
            // Act
            var result = Identification.FromInjections(null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that FromInjections correctly converts valid injections using InsulinValue.
        /// </summary>
        [Test]
        public void FromInjections_WithInsulinValue_ReturnsCorrectTimePoints()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    InsulinValue = new DoubleAndText { Double = 10.0 }
                },
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(6) },
                    InsulinValue = new DoubleAndText { Double = 12.0 }
                }
            };

            // Act
            var result = Identification.FromInjections(injections);

            // Assert
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Value, Is.EqualTo(10.0));
            Assert.That(result[1].Value, Is.EqualTo(12.0));
        }

        /// <summary>
        /// Tests that FromInjections prefers InsulinCalculated over InsulinValue when both present.
        /// </summary>
        [Test]
        public void FromInjections_WithBothInsulinValues_PrefersCalculated()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    InsulinCalculated = new DoubleAndText { Double = 11.5 },
                    InsulinValue = new DoubleAndText { Double = 10.0 }
                }
            };

            // Act
            var result = Identification.FromInjections(injections);

            // Assert
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(11.5), "Should prefer InsulinCalculated");
        }

        /// <summary>
        /// Tests that FromInjections skips injections without valid insulin value.
        /// </summary>
        [Test]
        public void FromInjections_WithMissingInsulin_SkipsInvalidInjections()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 8, 0, 0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    InsulinValue = new DoubleAndText { Double = null }
                },
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddHours(1) },
                    InsulinValue = new DoubleAndText { Double = 8.0 }
                }
            };

            // Act
            var result = Identification.FromInjections(injections);

            // Assert
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(8.0));
        }

        #endregion

        #region FromGlucoseRecords Tests

        /// <summary>
        /// Tests that FromGlucoseRecords returns empty array when input is null.
        /// </summary>
        [Test]
        public void FromGlucoseRecords_WithNullInput_ReturnsEmptyArray()
        {
            // Act
            var result = Identification.FromGlucoseRecords(null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that FromGlucoseRecords correctly converts valid glucose records.
        /// </summary>
        [Test]
        public void FromGlucoseRecords_WithValidRecords_ReturnsCorrectTimePoints()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 7, 0, 0);
            var records = new List<GlucoseRecord>
            {
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    GlucoseValue = new DoubleAndText { Double = 95.0 }
                },
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(15) },
                    GlucoseValue = new DoubleAndText { Double = 102.0 }
                },
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(30) },
                    GlucoseValue = new DoubleAndText { Double = 110.0 }
                }
            };

            // Act
            var result = Identification.FromGlucoseRecords(records);

            // Assert
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Value, Is.EqualTo(95.0));
            Assert.That(result[1].Value, Is.EqualTo(102.0));
            Assert.That(result[2].Value, Is.EqualTo(110.0));
        }

        /// <summary>
        /// Tests that FromGlucoseRecords skips records without valid glucose value.
        /// </summary>
        [Test]
        public void FromGlucoseRecords_WithMissingGlucoseValue_SkipsInvalidRecords()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 7, 0, 0);
            var records = new List<GlucoseRecord>
            {
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    GlucoseValue = new DoubleAndText { Double = null }
                },
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(15) },
                    GlucoseValue = new DoubleAndText { Double = 100.0 }
                }
            };

            // Act
            var result = Identification.FromGlucoseRecords(records);

            // Assert
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0].Value, Is.EqualTo(100.0));
        }

        /// <summary>
        /// Tests that FromGlucoseRecords returns records in chronological order.
        /// </summary>
        [Test]
        public void FromGlucoseRecords_WithUnorderedRecords_ReturnsSortedByTime()
        {
            // Arrange
            var baseTime = new DateTime(2024, 1, 15, 7, 0, 0);
            var records = new List<GlucoseRecord>
            {
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(30) },
                    GlucoseValue = new DoubleAndText { Double = 110.0 }
                },
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime },
                    GlucoseValue = new DoubleAndText { Double = 95.0 }
                },
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = baseTime.AddMinutes(15) },
                    GlucoseValue = new DoubleAndText { Double = 102.0 }
                }
            };

            // Act
            var result = Identification.FromGlucoseRecords(records);

            // Assert
            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Time, Is.EqualTo(baseTime));
            Assert.That(result[1].Time, Is.EqualTo(baseTime.AddMinutes(15)));
            Assert.That(result[2].Time, Is.EqualTo(baseTime.AddMinutes(30)));
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Tests a realistic scenario with meals, injections, and glucose records together.
        /// </summary>
        [Test]
        public void RealWorldScenario_BreakfastWithInsulinAndGlucose_ConvertsCorrectly()
        {
            // Arrange - Breakfast scenario
            var breakfastTime = new DateTime(2024, 1, 15, 7, 30, 0);
            
            var meals = new List<Meal>
            {
                new Meal
                {
                    EventTime = new DateTimeAndText { DateTime = breakfastTime },
                    CarbohydratesGrams = new DoubleAndText { Double = 45.0 }
                }
            };

            var injections = new List<Injection>
            {
                new Injection
                {
                    EventTime = new DateTimeAndText { DateTime = breakfastTime.AddMinutes(-5) },
                    InsulinValue = new DoubleAndText { Double = 8.0 }
                }
            };

            var glucoseRecords = new List<GlucoseRecord>
            {
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = breakfastTime.AddMinutes(-10) },
                    GlucoseValue = new DoubleAndText { Double = 98.0 }
                },
                new GlucoseRecord
                {
                    EventTime = new DateTimeAndText { DateTime = breakfastTime.AddHours(2) },
                    GlucoseValue = new DoubleAndText { Double = 142.0 }
                }
            };

            // Act
            var mealPoints = Identification.FromMeals(meals);
            var injectionPoints = Identification.FromInjections(injections);
            var glucosePoints = Identification.FromGlucoseRecords(glucoseRecords);

            // Assert
            Assert.That(mealPoints.Length, Is.EqualTo(1));
            Assert.That(mealPoints[0].Value, Is.EqualTo(45.0));
            
            Assert.That(injectionPoints.Length, Is.EqualTo(1));
            Assert.That(injectionPoints[0].Value, Is.EqualTo(8.0));
            
            Assert.That(glucosePoints.Length, Is.EqualTo(2));
            Assert.That(glucosePoints[0].Value, Is.EqualTo(98.0));
            Assert.That(glucosePoints[1].Value, Is.EqualTo(142.0));
        }

        #endregion
    }
}
