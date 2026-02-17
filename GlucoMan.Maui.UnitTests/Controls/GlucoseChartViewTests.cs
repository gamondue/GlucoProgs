using System;
using System.Collections.Generic;
using System.Linq;

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

}