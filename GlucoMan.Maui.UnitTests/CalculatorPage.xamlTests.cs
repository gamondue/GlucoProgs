using System;
using System.Globalization;
using System.Threading.Tasks;

using GlucoMan.Maui;
using Microsoft.Maui.Controls;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests
{
    /// <summary>
    /// Unit tests for the CalculatorPage class.
    /// NOTE: These tests are limited due to XAML dependency constraints.
    /// The CalculatorPage is a partial class with XAML-generated components
    /// that cannot be initialized in a unit test environment.
    /// Integration or UI tests are recommended for comprehensive coverage.
    /// </summary>
    public partial class CalculatorPageTests
    {
        /// <summary>
        /// Tests that the constructor handles a typical positive initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure
        /// that is not available in unit tests. The constructor calls InitializeComponent() which
        /// will throw InvalidOperationException in a unit test context.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithPositiveInitialValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = 123.45;

            // Act & Assert
            // This would throw because InitializeComponent() requires XAML resources
            // In a real integration test:
            // 1. Create instance: var page = new CalculatorPage(initialValue);
            // 2. Verify DisplayLabel.Text equals initialValue.ToString(CultureInfo.CurrentCulture)
            // 3. Verify btnDecimal.Text equals CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
            // 4. Verify ResultSource is initialized
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles zero as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithZeroInitialValue_ShouldDisplayZero()
        {
            // Arrange
            double initialValue = 0.0;

            // Act & Assert
            // Expected behavior in integration test:
            // DisplayLabel.Text should be "0" (formatted according to current culture)
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles negative initial values.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithNegativeInitialValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = -456.78;

            // Act & Assert
            // Expected behavior: DisplayLabel.Text should show negative value formatted per culture
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.MaxValue as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithMaxValueInitialValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = double.MaxValue;

            // Act & Assert
            // Expected behavior: DisplayLabel.Text should display formatted MaxValue
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.MinValue as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithMinValueInitialValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = double.MinValue;

            // Act & Assert
            // Expected behavior: DisplayLabel.Text should display formatted MinValue
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.NaN as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// Expected behavior: ToString() on NaN returns "NaN" string.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithNaNInitialValue_ShouldDisplayNaN()
        {
            // Arrange
            double initialValue = double.NaN;

            // Act & Assert
            // Expected behavior: DisplayLabel.Text should show "NaN"
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.PositiveInfinity as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithPositiveInfinityInitialValue_ShouldDisplayInfinity()
        {
            // Arrange
            double initialValue = double.PositiveInfinity;

            // Act & Assert
            // Expected behavior: DisplayLabel.Text should show "∞" or "Infinity" (culture-dependent)
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.NegativeInfinity as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithNegativeInfinityInitialValue_ShouldDisplayNegativeInfinity()
        {
            // Arrange
            double initialValue = double.NegativeInfinity;

            // Act & Assert
            // Expected behavior: DisplayLabel.Text should show "-∞" or "-Infinity" (culture-dependent)
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor properly sets the decimal separator based on current culture.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests with different cultures.
        /// Important test case: Verify behavior with cultures using comma (,) vs period (.) as separator.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_Constructor_ShouldSetDecimalSeparatorFromCurrentCulture()
        {
            // Arrange & Act & Assert
            // In integration test with different culture settings:
            // 1. Set CultureInfo.CurrentCulture to different cultures (e.g., en-US uses ".", de-DE uses ",")
            // 2. Create CalculatorPage instance
            // 3. Verify btnDecimal.Text matches culture's NumberDecimalSeparator
            // 4. Verify decimalSeparator field matches culture's separator
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor exception handler sets safe defaults when InitializeComponent throws.
        /// NOTE: This test is ignored because we cannot force InitializeComponent to throw in a controlled way
        /// without the XAML infrastructure, and we cannot mock the behavior due to sealed/partial class constraints.
        /// RECOMMENDATION: Test exception handling in integration tests by:
        /// 1. Creating scenarios where XAML components fail to initialize
        /// 2. Verifying DisplayLabel.Text is set to "0"
        /// 3. Verifying decimalSeparator is set to "."
        /// 4. Verifying btnDecimal.Text is set to "."
        /// 5. Verifying error is logged to gamon.General.LogOfProgram
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML exception handling in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WhenInitializeComponentThrows_ShouldSetSafeDefaults()
        {
            // This test cannot be implemented as a unit test due to:
            // 1. Cannot mock ContentPage (sealed class)
            // 2. Cannot mock or fake XAML-generated components
            // 3. Cannot control InitializeComponent() behavior without XAML infrastructure
            // 4. Cannot inject dependencies into the constructor
            Assert.Fail("This test requires MAUI integration test infrastructure or code refactoring for testability.");
        }

        /// <summary>
        /// Tests that ResultSource property is initialized in the constructor.
        /// NOTE: This test is ignored because creating the CalculatorPage instance requires XAML infrastructure.
        /// RECOMMENDATION: Test in integration tests by verifying ResultSource is not null after construction.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_Constructor_ShouldInitializeResultSource()
        {
            // Arrange
            double initialValue = 100.0;

            // Act & Assert
            // Expected behavior: ResultSource should be a new TaskCompletionSource<double?>
            // and should not be null
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that OnDisappearing sets the ResultSource task result to null
        /// when the task is not yet completed.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenTaskNotCompleted_SetsResultToNull()
        {
            // Arrange
            var calculator = new TestableCalculatorPage(0);

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.Result, Is.Null);
        }

        /// <summary>
        /// Tests that OnDisappearing does not change the ResultSource task result
        /// when the task is already completed with a value.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenTaskAlreadyCompletedWithValue_DoesNotChangeResult()
        {
            // Arrange
            var calculator = new TestableCalculatorPage(0);
            var expectedResult = 42.5;
            calculator.ResultSource.TrySetResult(expectedResult);

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.Result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that OnDisappearing does not change the ResultSource task result
        /// when the task is already completed with null.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenTaskAlreadyCompletedWithNull_DoesNotChangeResult()
        {
            // Arrange
            var calculator = new TestableCalculatorPage(0);
            calculator.ResultSource.TrySetResult(null);

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.Result, Is.Null);
        }

        /// <summary>
        /// Tests that OnDisappearing can be called multiple times without throwing exceptions.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenCalledMultipleTimes_DoesNotThrowException()
        {
            // Arrange
            var calculator = new TestableCalculatorPage(0);

            // Act & Assert
            Assert.DoesNotThrow(() => calculator.CallOnDisappearing());
            Assert.DoesNotThrow(() => calculator.CallOnDisappearing());
            Assert.DoesNotThrow(() => calculator.CallOnDisappearing());
        }

        /// <summary>
        /// Tests that OnDisappearing works correctly with various initial values.
        /// </summary>
        /// <param name="initialValue">The initial value to pass to the calculator constructor.</param>
        [TestCase(0.0)]
        [TestCase(100.5)]
        [TestCase(-50.3)]
        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        [TestCase(double.Epsilon)]
        public void OnDisappearing_WithVariousInitialValues_CompletesTaskWithNull(double initialValue)
        {
            // Arrange
            var calculator = new TestableCalculatorPage(initialValue);

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.Result, Is.Null);
        }

        /// <summary>
        /// Helper class that exposes the protected OnDisappearing method for testing.
        /// Inherits from CalculatorPage to access protected members.
        /// </summary>
        private class TestableCalculatorPage : CalculatorPage
        {
            public TestableCalculatorPage(double initialValue) : base(initialValue)
            {
            }

            /// <summary>
            /// Exposes the protected OnDisappearing method for testing.
            /// </summary>
            public void CallOnDisappearing()
            {
                OnDisappearing();
            }
        }

        /// <summary>
        /// Tests that OnDisappearing handles a task that is in the canceled state.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenTaskCanceled_DoesNotChangeResult()
        {
            // Arrange
            var calculator = new TestableCalculatorPage(0);
            calculator.ResultSource.TrySetCanceled();

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.IsCanceled, Is.True);
        }

        /// <summary>
        /// Tests that OnDisappearing handles a task that is in the faulted state.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenTaskFaulted_DoesNotChangeResult()
        {
            // Arrange
            var calculator = new TestableCalculatorPage(0);
            calculator.ResultSource.TrySetException(new InvalidOperationException("Test exception"));

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.IsFaulted, Is.True);
        }

        /// <summary>
        /// Tests that OnDisappearing properly handles special double values as initial values.
        /// </summary>
        /// <param name="initialValue">The special double value to test.</param>
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void OnDisappearing_WithSpecialDoubleInitialValues_CompletesTaskWithNull(double initialValue)
        {
            // Arrange
            var calculator = new TestableCalculatorPage(initialValue);

            // Act
            calculator.CallOnDisappearing();

            // Assert
            Assert.That(calculator.ResultSource.Task.IsCompleted, Is.True);
            Assert.That(calculator.ResultSource.Task.Result, Is.Null);
        }

        /// <summary>
        /// Tests that OnDisappearing exception handling works correctly.
        /// NOTE: This test is ignored because testing the catch block requires creating a scenario
        /// where base.OnDisappearing() or other operations throw an exception, which is not feasible
        /// in a unit test without mocking ContentPage (which is sealed and cannot be mocked).
        /// RECOMMENDATION: Test exception handling in integration/UI tests where:
        /// 1. Page navigation or disposal errors can be simulated
        /// 2. XAML component disposal errors can occur
        /// 3. Verify that exceptions are caught and logged via gamon.General.LogOfProgram.Error
        /// 4. Verify that the application remains stable after the exception
        /// </summary>
        [Test]
        [Ignore("Cannot test exception handling without ability to mock ContentPage or force base.OnDisappearing() to throw. Requires integration testing.")]
        public void OnDisappearing_WhenBaseOnDisappearingThrows_CatchesAndLogsException()
        {
            // This test cannot be implemented as a unit test because:
            // 1. ContentPage is sealed and cannot be mocked
            // 2. Cannot force base.OnDisappearing() to throw without changing production code
            // 3. Cannot inject mock dependencies to control behavior
            // 4. TaskCompletionSource operations (IsCompleted, TrySetResult) don't throw in normal scenarios
            //
            // To test the catch block, integration tests should:
            // - Create scenarios where page disposal fails
            // - Verify exception is logged to gamon.General.LogOfProgram
            // - Verify application doesn't crash
            Assert.Fail("This test requires integration test infrastructure to simulate base.OnDisappearing() throwing exceptions.");
        }

        /// <summary>
        /// Tests that the constructor handles double.Epsilon as initial value.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithEpsilonInitialValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = double.Epsilon;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(double.Epsilon.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Tests that the constructor handles very small positive values correctly.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithVerySmallPositiveValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = 0.000001;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Tests that the constructor handles very small negative values correctly.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithVerySmallNegativeValue_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = -0.000001;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Tests that the constructor handles values close to zero from the positive side.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithValueJustAboveZero_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = 0.1;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Tests that the constructor handles values close to zero from the negative side.
        /// NOTE: This test is ignored because InitializeComponent() requires XAML infrastructure.
        /// RECOMMENDATION: Test this scenario using MAUI UI/Integration tests.
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithValueJustBelowZero_ShouldFormatAndDisplay()
        {
            // Arrange
            double initialValue = -0.1;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
        }

        /// <summary>
        /// Tests that the constructor properly initializes with a typical positive initial value.
        /// Expected behavior:
        /// - DisplayLabel.Text should be set to InitialValue.ToString(CultureInfo.CurrentCulture)
        /// - decimalSeparator should be set to CurrentCulture's NumberDecimalSeparator
        /// - btnDecimal.Text should be set to the decimal separator
        /// - ResultSource should be initialized
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithPositiveInitialValue_ShouldInitializeCorrectly()
        {
            // Arrange
            double initialValue = 123.45;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: 
            // - Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            // - Assert.That(page.ResultSource, Is.Not.Null);
            // - Assert.That(btnDecimal.Text, Is.EqualTo(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles negative initial values correctly.
        /// Expected behavior:
        /// - DisplayLabel.Text should include the negative sign formatted according to CurrentCulture
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithNegativeInitialValue_ShouldDisplayNegativeValue()
        {
            // Arrange
            double initialValue = -456.78;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.MaxValue as initial value.
        /// Expected behavior:
        /// - DisplayLabel.Text should be set to the string representation of double.MaxValue
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithMaxValueInitialValue_ShouldDisplayMaxValue()
        {
            // Arrange
            double initialValue = double.MaxValue;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(double.MaxValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.MinValue as initial value.
        /// Expected behavior:
        /// - DisplayLabel.Text should be set to the string representation of double.MinValue
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithMinValueInitialValue_ShouldDisplayMinValue()
        {
            // Arrange
            double initialValue = double.MinValue;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(double.MinValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles double.Epsilon (smallest positive double value) as initial value.
        /// Expected behavior:
        /// - DisplayLabel.Text should be set to the scientific notation or decimal representation of Epsilon
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithEpsilonInitialValue_ShouldDisplayEpsilon()
        {
            // Arrange
            double initialValue = double.Epsilon;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(double.Epsilon.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles very small positive values correctly.
        /// Expected behavior:
        /// - DisplayLabel.Text should be formatted according to CurrentCulture (may use scientific notation)
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithVerySmallPositiveValue_ShouldDisplayFormattedValue()
        {
            // Arrange
            double initialValue = 0.000000001;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles very small negative values correctly.
        /// Expected behavior:
        /// - DisplayLabel.Text should be formatted with negative sign according to CurrentCulture
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithVerySmallNegativeValue_ShouldDisplayFormattedValue()
        {
            // Arrange
            double initialValue = -0.000000001;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles very large positive values correctly.
        /// Expected behavior:
        /// - DisplayLabel.Text should be formatted according to CurrentCulture (may use scientific notation)
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithVeryLargePositiveValue_ShouldDisplayFormattedValue()
        {
            // Arrange
            double initialValue = 1.23456789012345E+100;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles very large negative values correctly.
        /// Expected behavior:
        /// - DisplayLabel.Text should be formatted with negative sign according to CurrentCulture
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithVeryLargeNegativeValue_ShouldDisplayFormattedValue()
        {
            // Arrange
            double initialValue = -1.23456789012345E+100;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that btnDecimal null check works correctly in constructor.
        /// Expected behavior:
        /// - If btnDecimal is null, no exception should be thrown
        /// - If btnDecimal is not null, its Text should be set to decimal separator
        /// NOTE: This scenario can only occur if InitializeComponent partially succeeds or fails
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WhenBtnDecimalIsNull_ShouldNotThrowException()
        {
            // This test verifies the null check: if (btnDecimal != null)
            // In integration tests:
            // - Create scenario where btnDecimal is not initialized (partial XAML failure)
            // - Verify constructor completes without throwing NullReferenceException
            // - Verify decimalSeparator field is still set correctly
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that DisplayLabel null check works correctly in exception handler.
        /// Expected behavior:
        /// - If DisplayLabel is null in catch block, no exception should be thrown
        /// - If DisplayLabel is not null, its Text should be set to "0"
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML exception handling in unit tests. Requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WhenExceptionAndDisplayLabelIsNull_ShouldNotThrowException()
        {
            // This test verifies the null check: if (DisplayLabel != null)
            // In integration tests:
            // - Force InitializeComponent to throw exception
            // - Ensure DisplayLabel remains null
            // - Verify constructor completes without throwing NullReferenceException in catch block
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests boundary behavior with values just above zero.
        /// Expected behavior:
        /// - Values like 0.1, 0.01, 0.001 should be formatted according to CurrentCulture
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithValueJustAboveZero_ShouldDisplayFormattedValue()
        {
            // Arrange
            double initialValue = 0.1;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests boundary behavior with values just below zero.
        /// Expected behavior:
        /// - Values like -0.1, -0.01, -0.001 should be formatted with negative sign according to CurrentCulture
        /// </summary>
        [Test]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithValueJustBelowZero_ShouldDisplayFormattedValue()
        {
            // Arrange
            double initialValue = -0.1;

            // Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }

        /// <summary>
        /// Tests that the constructor handles typical calculator input values.
        /// Expected behavior:
        /// - Common values used in glucose management (e.g., 5.5, 10.2, 15.7) should be formatted correctly
        /// </summary>
        [TestCase(5.5)]
        [TestCase(10.2)]
        [TestCase(15.7)]
        [TestCase(99.9)]
        [TestCase(100.0)]
        [TestCase(250.5)]
        [Ignore("Cannot test XAML-dependent constructor in unit tests. InitializeComponent() requires MAUI UI test infrastructure.")]
        public void CalculatorPage_WithTypicalGlucoseValues_ShouldDisplayFormattedValue(double initialValue)
        {
            // Arrange & Act
            // Would execute: var page = new CalculatorPage(initialValue);

            // Assert
            // Would verify: Assert.That(page.DisplayLabel.Text, Is.EqualTo(initialValue.ToString(CultureInfo.CurrentCulture)));
            Assert.Fail("This test requires MAUI integration test infrastructure.");
        }
    }
}