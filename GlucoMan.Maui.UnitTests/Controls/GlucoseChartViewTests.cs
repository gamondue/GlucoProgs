using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using GlucoMan.Maui.Controls;
using NUnit.Framework;
using SkiaSharp;

namespace GlucoMan.Maui.Controls.UnitTests;



/// <summary>
/// Unit tests for the GlucoseChartView class.
/// </summary>
public partial class GlucoseChartViewTests
{
    /// <summary>
    /// Tests that the constructor successfully creates an instance of GlucoseChartView.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_CreatesInstance()
    {
        // Arrange & Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.That(chartView, Is.Not.Null);
        Assert.That(chartView, Is.InstanceOf<GlucoseChartView>());
    }

    /// <summary>
    /// Tests that the constructor initializes numeric properties with correct default values.
    /// </summary>
    /// <param name="propertyName">The name of the property to test.</param>
    /// <param name="expectedValue">The expected default value.</param>
    [TestCase("MinY", 0f)]
    [TestCase("MaxY", 500f)]
    [TestCase("YStep", 50f)]
    [TestCase("MinX", 0f)]
    [TestCase("MaxX", 24f)]
    [TestCase("XStep", 2f)]
    [TestCase("HighThreshold", 240f)]
    [TestCase("LowThreshold", 80f)]
    [TestCase("SafeZoneMin", 60f)]
    [TestCase("SafeZoneMax", 180f)]
    public void Constructor_WhenCalled_InitializesNumericPropertyWithDefaultValue(string propertyName, float expectedValue)
    {
        // Arrange & Act
        var chartView = new GlucoseChartView();
        var propertyInfo = typeof(GlucoseChartView).GetProperty(propertyName);
        var actualValue = (float)propertyInfo!.GetValue(chartView)!;

        // Assert
        Assert.That(actualValue, Is.EqualTo(expectedValue));
    }

    /// <summary>
    /// Tests that the constructor initializes BackgroundColor with the correct default value.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_InitializesBackgroundColorToWhite()
    {
        // Arrange & Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.That(chartView.BackgroundColor, Is.EqualTo(SKColors.White));
    }

    /// <summary>
    /// Tests that the constructor initializes AxisColor with the correct default value.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_InitializesAxisColorToBlack()
    {
        // Arrange & Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.That(chartView.AxisColor, Is.EqualTo(SKColors.Black));
    }

    /// <summary>
    /// Tests that the constructor initializes CurveColor with the correct default value.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_InitializesCurveColorToDarkRed()
    {
        // Arrange
        var expectedColor = SKColor.Parse("#8B0000");

        // Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.That(chartView.CurveColor, Is.EqualTo(expectedColor));
    }

    /// <summary>
    /// Tests that the constructor initializes SafeZoneColor with the correct default value.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_InitializesSafeZoneColorToLightGreen()
    {
        // Arrange
        var expectedColor = SKColor.Parse("#E8F5E9");

        // Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.That(chartView.SafeZoneColor, Is.EqualTo(expectedColor));
    }

    /// <summary>
    /// Tests that the constructor initializes ThresholdColor with the correct default value.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_InitializesThresholdColorToRed()
    {
        // Arrange & Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.That(chartView.ThresholdColor, Is.EqualTo(SKColors.Red));
    }

    /// <summary>
    /// Tests that the constructor initializes all chart configuration properties correctly in a single comprehensive test.
    /// </summary>
    [Test]
    public void Constructor_WhenCalled_InitializesAllPropertiesWithCorrectDefaults()
    {
        // Arrange & Act
        var chartView = new GlucoseChartView();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(chartView.MinY, Is.EqualTo(0f), "MinY should default to 0");
            Assert.That(chartView.MaxY, Is.EqualTo(500f), "MaxY should default to 500");
            Assert.That(chartView.YStep, Is.EqualTo(50f), "YStep should default to 50");
            Assert.That(chartView.MinX, Is.EqualTo(0f), "MinX should default to 0");
            Assert.That(chartView.MaxX, Is.EqualTo(24f), "MaxX should default to 24");
            Assert.That(chartView.XStep, Is.EqualTo(2f), "XStep should default to 2");
            Assert.That(chartView.HighThreshold, Is.EqualTo(240f), "HighThreshold should default to 240");
            Assert.That(chartView.LowThreshold, Is.EqualTo(80f), "LowThreshold should default to 80");
            Assert.That(chartView.SafeZoneMin, Is.EqualTo(60f), "SafeZoneMin should default to 60");
            Assert.That(chartView.SafeZoneMax, Is.EqualTo(180f), "SafeZoneMax should default to 180");
            Assert.That(chartView.BackgroundColor, Is.EqualTo(SKColors.White), "BackgroundColor should default to White");
            Assert.That(chartView.AxisColor, Is.EqualTo(SKColors.Black), "AxisColor should default to Black");
            Assert.That(chartView.CurveColor, Is.EqualTo(SKColor.Parse("#8B0000")), "CurveColor should default to #8B0000");
            Assert.That(chartView.SafeZoneColor, Is.EqualTo(SKColor.Parse("#E8F5E9")), "SafeZoneColor should default to #E8F5E9");
            Assert.That(chartView.ThresholdColor, Is.EqualTo(SKColors.Red), "ThresholdColor should default to Red");
        });
    }

    /// <summary>
    /// Helper class to expose protected/private members of GlucoseChartView for testing.
    /// </summary>
    private class TestableGlucoseChartView : GlucoseChartView
    {
    }


    /// <summary>
    /// Tests that SetData with null parameter assigns a new empty list to the data points field.
    /// </summary>
    [Test]
    public void SetData_WithNull_SetsEmptyList()
    {
        // Arrange
        var chartView = new GlucoseChartView();

        // Act
        chartView.SetData(null!);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that SetData with an empty list assigns the empty list to the data points field.
    /// </summary>
    [Test]
    public void SetData_WithEmptyList_SetsEmptyList()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var emptyList = new List<(float Hour, float Value)>();

        // Act
        chartView.SetData(emptyList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that SetData with a single data point correctly assigns the list to the data points field.
    /// </summary>
    [Test]
    public void SetData_WithSingleDataPoint_SetsSingleDataPoint()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)> { (12.5f, 120.0f) };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(1));
        Assert.That(dataPoints[0].Hour, Is.EqualTo(12.5f));
        Assert.That(dataPoints[0].Value, Is.EqualTo(120.0f));
    }

    /// <summary>
    /// Tests that SetData with multiple data points correctly assigns all data points to the field.
    /// </summary>
    [Test]
    public void SetData_WithMultipleDataPoints_SetsAllDataPoints()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)>
        {
            (0.0f, 80.0f),
            (6.0f, 100.0f),
            (12.0f, 150.0f),
            (18.0f, 120.0f),
            (24.0f, 90.0f)
        };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(5));
        Assert.That(dataPoints[0], Is.EqualTo((0.0f, 80.0f)));
        Assert.That(dataPoints[2], Is.EqualTo((12.0f, 150.0f)));
        Assert.That(dataPoints[4], Is.EqualTo((24.0f, 90.0f)));
    }

    /// <summary>
    /// Tests that SetData accepts and stores data points with duplicate values.
    /// </summary>
    [Test]
    public void SetData_WithDuplicateDataPoints_AcceptsDuplicates()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)>
        {
            (12.0f, 100.0f),
            (12.0f, 100.0f),
            (12.0f, 100.0f)
        };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(3));
        Assert.That(dataPoints[0], Is.EqualTo((12.0f, 100.0f)));
        Assert.That(dataPoints[1], Is.EqualTo((12.0f, 100.0f)));
        Assert.That(dataPoints[2], Is.EqualTo((12.0f, 100.0f)));
    }

    /// <summary>
    /// Tests that SetData accepts data points with special float values (NaN, PositiveInfinity, NegativeInfinity).
    /// </summary>
    /// <param name="hour">The hour value to test.</param>
    /// <param name="value">The glucose value to test.</param>
    [TestCase(float.NaN, 100.0f)]
    [TestCase(float.PositiveInfinity, 100.0f)]
    [TestCase(float.NegativeInfinity, 100.0f)]
    [TestCase(12.0f, float.NaN)]
    [TestCase(12.0f, float.PositiveInfinity)]
    [TestCase(12.0f, float.NegativeInfinity)]
    [TestCase(float.NaN, float.NaN)]
    [TestCase(float.PositiveInfinity, float.PositiveInfinity)]
    public void SetData_WithSpecialFloatValues_AcceptsSpecialValues(float hour, float value)
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)> { (hour, value) };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(1));
        Assert.That(dataPoints[0].Hour, Is.EqualTo(hour));
        Assert.That(dataPoints[0].Value, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests that SetData accepts data points with negative values for both Hour and Value.
    /// </summary>
    [Test]
    public void SetData_WithNegativeValues_AcceptsNegativeValues()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)>
        {
            (-5.0f, 100.0f),
            (12.0f, -50.0f),
            (-10.0f, -75.0f)
        };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(3));
        Assert.That(dataPoints[0], Is.EqualTo((-5.0f, 100.0f)));
        Assert.That(dataPoints[1], Is.EqualTo((12.0f, -50.0f)));
        Assert.That(dataPoints[2], Is.EqualTo((-10.0f, -75.0f)));
    }

    /// <summary>
    /// Tests that SetData accepts data points with boundary float values (MinValue, MaxValue, Zero).
    /// </summary>
    /// <param name="hour">The hour value to test.</param>
    /// <param name="value">The glucose value to test.</param>
    [TestCase(float.MinValue, 100.0f)]
    [TestCase(float.MaxValue, 100.0f)]
    [TestCase(0.0f, 0.0f)]
    [TestCase(12.0f, float.MinValue)]
    [TestCase(12.0f, float.MaxValue)]
    [TestCase(float.MinValue, float.MinValue)]
    [TestCase(float.MaxValue, float.MaxValue)]
    public void SetData_WithBoundaryFloatValues_AcceptsBoundaryValues(float hour, float value)
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)> { (hour, value) };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(1));
        Assert.That(dataPoints[0].Hour, Is.EqualTo(hour));
        Assert.That(dataPoints[0].Value, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests that SetData replaces existing data points when called multiple times.
    /// </summary>
    [Test]
    public void SetData_CalledMultipleTimes_ReplacesExistingData()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var firstDataList = new List<(float Hour, float Value)> { (10.0f, 100.0f), (11.0f, 110.0f) };
        var secondDataList = new List<(float Hour, float Value)> { (20.0f, 200.0f) };

        // Act
        chartView.SetData(firstDataList);
        chartView.SetData(secondDataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(1));
        Assert.That(dataPoints[0], Is.EqualTo((20.0f, 200.0f)));
    }

    /// <summary>
    /// Tests that SetData with zero values for both Hour and Value is accepted.
    /// </summary>
    [Test]
    public void SetData_WithZeroValues_AcceptsZeroValues()
    {
        // Arrange
        var chartView = new GlucoseChartView();
        var dataList = new List<(float Hour, float Value)> { (0.0f, 0.0f) };

        // Act
        chartView.SetData(dataList);

        // Assert
        var dataPointsField = typeof(GlucoseChartView).GetField("_dataPoints", BindingFlags.NonPublic | BindingFlags.Instance);
        var dataPoints = (List<(float Hour, float Value)>)dataPointsField!.GetValue(chartView)!;
        Assert.That(dataPoints, Is.Not.Null);
        Assert.That(dataPoints.Count, Is.EqualTo(1));
        Assert.That(dataPoints[0].Hour, Is.EqualTo(0.0f));
        Assert.That(dataPoints[0].Value, Is.EqualTo(0.0f));
    }
}