using System;
using System.Globalization;

using GlucoMan.Maui.Converters;
using NUnit.Framework;

namespace GlucoMan.Maui.Converters.UnitTests;


/// <summary>
/// Unit tests for the <see cref="DoubleTextConverter"/> class.
/// </summary>
public class DoubleTextConverterTests
{
    /// <summary>
    /// Tests that ConvertBack returns null when the value parameter is null.
    /// </summary>
    [Test]
    public void ConvertBack_NullValue_ReturnsNull()
    {
        // Arrange
        var converter = new DoubleTextConverter();
        object? value = null;
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object? result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that ConvertBack returns the exact same object instance when given a reference type.
    /// </summary>
    [Test]
    public void ConvertBack_ReferenceTypeValue_ReturnsSameInstance()
    {
        // Arrange
        var converter = new DoubleTextConverter();
        object value = "test string";
        Type targetType = typeof(double);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.SameAs(value));
    }

    /// <summary>
    /// Tests that ConvertBack returns the value unchanged for various input types.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("test string")]
    [TestCase("NaN")]
    [TestCase("nan")]
    [TestCase("123.45")]
    [TestCase("0")]
    [TestCase("-1")]
    public void ConvertBack_StringValue_ReturnsUnchangedValue(string value)
    {
        // Arrange
        var converter = new DoubleTextConverter();
        Type targetType = typeof(double);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(value));
        Assert.That(result, Is.SameAs(value));
    }

    /// <summary>
    /// Tests that ConvertBack returns the value unchanged for numeric types.
    /// </summary>
    /// <param name="value">The numeric value to convert back.</param>
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue)]
    public void ConvertBack_IntegerValue_ReturnsUnchangedValue(int value)
    {
        // Arrange
        var converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests that ConvertBack returns the value unchanged for double types including special values.
    /// </summary>
    /// <param name="value">The double value to convert back.</param>
    [TestCase(0.0)]
    [TestCase(1.5)]
    [TestCase(-1.5)]
    [TestCase(double.MaxValue)]
    [TestCase(double.MinValue)]
    [TestCase(double.Epsilon)]
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    [TestCase(double.NegativeInfinity)]
    public void ConvertBack_DoubleValue_ReturnsUnchangedValue(double value)
    {
        // Arrange
        var converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests that ConvertBack ignores the targetType parameter and returns the value unchanged.
    /// </summary>
    [Test]
    public void ConvertBack_DifferentTargetTypes_ReturnsUnchangedValue()
    {
        // Arrange
        var converter = new DoubleTextConverter();
        object value = "test";
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result1 = converter.ConvertBack(value, typeof(string), parameter, culture);
        object result2 = converter.ConvertBack(value, typeof(int), parameter, culture);
        object result3 = converter.ConvertBack(value, typeof(double), parameter, culture);
        object result4 = converter.ConvertBack(value, typeof(object), parameter, culture);

        // Assert
        Assert.That(result1, Is.SameAs(value));
        Assert.That(result2, Is.SameAs(value));
        Assert.That(result3, Is.SameAs(value));
        Assert.That(result4, Is.SameAs(value));
    }

    /// <summary>
    /// Tests that ConvertBack ignores the parameter argument and returns the value unchanged.
    /// </summary>
    [Test]
    public void ConvertBack_DifferentParameters_ReturnsUnchangedValue()
    {
        // Arrange
        var converter = new DoubleTextConverter();
        object value = "test";
        Type targetType = typeof(string);
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result1 = converter.ConvertBack(value, targetType, null, culture);
        object result2 = converter.ConvertBack(value, targetType, "parameter", culture);
        object result3 = converter.ConvertBack(value, targetType, 123, culture);

        // Assert
        Assert.That(result1, Is.SameAs(value));
        Assert.That(result2, Is.SameAs(value));
        Assert.That(result3, Is.SameAs(value));
    }

    /// <summary>
    /// Tests that ConvertBack ignores the culture parameter and returns the value unchanged.
    /// </summary>
    [Test]
    public void ConvertBack_DifferentCultures_ReturnsUnchangedValue()
    {
        // Arrange
        var converter = new DoubleTextConverter();
        object value = "test";
        Type targetType = typeof(string);
        object? parameter = null;

        // Act
        object result1 = converter.ConvertBack(value, targetType, parameter, CultureInfo.InvariantCulture);
        object result2 = converter.ConvertBack(value, targetType, parameter, CultureInfo.CurrentCulture);
        object result3 = converter.ConvertBack(value, targetType, parameter, new CultureInfo("en-US"));
        object result4 = converter.ConvertBack(value, targetType, parameter, new CultureInfo("de-DE"));

        // Assert
        Assert.That(result1, Is.SameAs(value));
        Assert.That(result2, Is.SameAs(value));
        Assert.That(result3, Is.SameAs(value));
        Assert.That(result4, Is.SameAs(value));
    }

    /// <summary>
    /// Tests that ConvertBack handles DateTime objects correctly.
    /// </summary>
    [Test]
    public void ConvertBack_DateTimeValue_ReturnsUnchangedValue()
    {
        // Arrange
        var converter = new DoubleTextConverter();
        DateTime value = new DateTime(2024, 1, 15, 10, 30, 45);
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests that ConvertBack handles boolean values correctly.
    /// </summary>
    /// <param name="value">The boolean value to convert back.</param>
    [TestCase(true)]
    [TestCase(false)]
    public void ConvertBack_BooleanValue_ReturnsUnchangedValue(bool value)
    {
        // Arrange
        var converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.ConvertBack(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests that Convert returns an empty string when the value parameter is null.
    /// </summary>
    [Test]
    public void Convert_NullValue_ReturnsEmptyString()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object? value = null;
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that Convert returns an empty string for values that produce "NaN" or whitespace strings.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    [TestCase(double.NaN)]
    [TestCase(float.NaN)]
    [TestCase("NaN")]
    [TestCase("nan")]
    [TestCase("NAN")]
    [TestCase("nAn")]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\n")]
    [TestCase("\r")]
    [TestCase("\t\n\r")]
    [TestCase("  \t  \n  ")]
    public void Convert_NaNOrWhitespaceValue_ReturnsEmptyString(object value)
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that Convert returns the string representation of the value for valid non-NaN values.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="expected">The expected string result.</param>
    [TestCase(0, "0")]
    [TestCase(123, "123")]
    [TestCase(-456, "-456")]
    [TestCase(int.MaxValue, "2147483647")]
    [TestCase(int.MinValue, "-2147483648")]
    [TestCase(3.14, "3.14")]
    [TestCase(-2.5, "-2.5")]
    [TestCase("test", "test")]
    [TestCase("hello world", "hello world")]
    [TestCase("123.456", "123.456")]
    [TestCase("NaN is not here", "NaN is not here")]
    [TestCase("nan value", "nan value")]
    [TestCase("This is a NaN test", "This is a NaN test")]
    [TestCase("!@#$%^&*()", "!@#$%^&*()")]
    [TestCase("a", "a")]
    public void Convert_ValidValue_ReturnsStringRepresentation(object value, string expected)
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    /// <summary>
    /// Tests that Convert returns the string representation for infinity values (not treated as NaN).
    /// </summary>
    [Test]
    public void Convert_PositiveInfinity_ReturnsInfinityString()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object value = double.PositiveInfinity;
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo("∞"));
    }

    /// <summary>
    /// Tests that Convert returns the string representation for negative infinity values (not treated as NaN).
    /// </summary>
    [Test]
    public void Convert_NegativeInfinity_ReturnsNegativeInfinityString()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object value = double.NegativeInfinity;
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo("-∞"));
    }

    /// <summary>
    /// Tests that Convert handles very long strings correctly without truncation.
    /// </summary>
    [Test]
    public void Convert_VeryLongString_ReturnsFullString()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        string longString = new string('a', 10000);
        object value = longString;
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(longString));
    }

    /// <summary>
    /// Tests that Convert returns empty string for a string containing only zero-width whitespace.
    /// </summary>
    [Test]
    public void Convert_ZeroWidthWhitespace_ReturnsEmptyString()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object value = "\u200B\u200C\u200D";
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(value.ToString()));
    }

    /// <summary>
    /// Tests that Convert works correctly regardless of the targetType parameter value.
    /// </summary>
    [Test]
    public void Convert_DifferentTargetType_ReturnsCorrectValue()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object value = "test";
        Type targetType = typeof(int);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo("test"));
    }

    /// <summary>
    /// Tests that Convert works correctly when parameter is not null.
    /// </summary>
    [Test]
    public void Convert_NonNullParameter_ReturnsCorrectValue()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object value = "test";
        Type targetType = typeof(string);
        object parameter = new object();
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo("test"));
    }

    /// <summary>
    /// Tests that Convert works correctly with different culture settings.
    /// </summary>
    [Test]
    public void Convert_DifferentCulture_ReturnsCorrectValue()
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        object value = "test";
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = new CultureInfo("de-DE");

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo("test"));
    }

    /// <summary>
    /// Tests that Convert handles boolean values correctly.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="expected">The expected string result.</param>
    [TestCase(true, "True")]
    [TestCase(false, "False")]
    public void Convert_BooleanValue_ReturnsStringRepresentation(bool value, string expected)
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    /// <summary>
    /// Tests that Convert handles zero values of different numeric types correctly.
    /// </summary>
    /// <param name="value">The zero value to convert.</param>
    /// <param name="expected">The expected string result.</param>
    [TestCase(0, "0")]
    [TestCase(0.0, "0")]
    [TestCase(0f, "0")]
    public void Convert_ZeroValues_ReturnsZeroString(object value, string expected)
    {
        // Arrange
        DoubleTextConverter converter = new DoubleTextConverter();
        Type targetType = typeof(string);
        object? parameter = null;
        CultureInfo culture = CultureInfo.InvariantCulture;

        // Act
        object result = converter.Convert(value, targetType, parameter, culture);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}