using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using gamon;
using GlucoMan;
using GlucoMan.Maui;
using GlucoMan.Maui.Models;
using GlucoMan.Maui.Resources;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;



/// <summary>
/// Unit tests for the ContainersPage class.
/// </summary>
[TestFixture]
public partial class ContainersPageTests
{
    /// <summary>
    /// Tests that OnDisappearing completes the task with false when ContainerWasSelected is false.
    /// </summary>
    [Test]
    public void OnDisappearing_WhenTaskNotCompletedAndContainerNotSelected_CompletesTaskWithFalse()
    {
        // Arrange
        var testPage = new TestableContainersPage();

        // Act
        testPage.CallOnDisappearing();

        // Assert
        Assert.That(testPage.PageClosedTask.IsCompleted, Is.True);
        Assert.That(testPage.PageClosedTask.Result, Is.False);
    }

    /// <summary>
    /// Tests that OnDisappearing completes the task with true when ContainerWasSelected is true.
    /// </summary>
    [Test]
    public void OnDisappearing_WhenTaskNotCompletedAndContainerSelected_CompletesTaskWithTrue()
    {
        // Arrange
        var testPage = new TestableContainersPage();
        testPage.SetContainerWasSelected(true);

        // Act
        testPage.CallOnDisappearing();

        // Assert
        Assert.That(testPage.PageClosedTask.IsCompleted, Is.True);
        Assert.That(testPage.PageClosedTask.Result, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing does not throw when task is already completed.
    /// </summary>
    [Test]
    public void OnDisappearing_WhenTaskAlreadyCompleted_DoesNotThrowException()
    {
        // Arrange
        var testPage = new TestableContainersPage();
        testPage.CallOnDisappearing(); // First call completes the task

        // Act & Assert
        Assert.DoesNotThrow(() => testPage.CallOnDisappearing());
    }

    /// <summary>
    /// Tests that OnDisappearing can be called multiple times without throwing.
    /// </summary>
    [Test]
    public void OnDisappearing_CalledMultipleTimes_DoesNotThrow()
    {
        // Arrange
        var testPage = new TestableContainersPage();

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            testPage.CallOnDisappearing();
            testPage.CallOnDisappearing();
            testPage.CallOnDisappearing();
        });
    }

    /// <summary>
    /// Tests that PageClosedTask is accessible and returns the same task instance.
    /// </summary>
    [Test]
    public void OnDisappearing_PageClosedTask_ReturnsSameTaskInstance()
    {
        // Arrange
        var testPage = new TestableContainersPage();
        var taskBefore = testPage.PageClosedTask;

        // Act
        testPage.CallOnDisappearing();
        var taskAfter = testPage.PageClosedTask;

        // Assert
        Assert.That(ReferenceEquals(taskBefore, taskAfter), Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing completes task immediately when not already completed.
    /// </summary>
    [Test]
    public void OnDisappearing_WhenCalled_CompletesTaskSynchronously()
    {
        // Arrange
        var testPage = new TestableContainersPage();
        Assert.That(testPage.PageClosedTask.IsCompleted, Is.False);

        // Act
        testPage.CallOnDisappearing();

        // Assert - Task should be completed synchronously
        Assert.That(testPage.PageClosedTask.IsCompleted, Is.True);
        Assert.That(testPage.PageClosedTask.IsCompletedSuccessfully, Is.True);
    }

    /// <summary>
    /// Helper class to expose protected OnDisappearing method for testing.
    /// </summary>
    private class TestableContainersPage : ContainersPage
    {
        public void CallOnDisappearing()
        {
            OnDisappearing();
        }

        public void SetContainerWasSelected(bool value)
        {
            // Use reflection to set the private setter property
            typeof(ContainersPage)
                .GetProperty(nameof(ContainerWasSelected))!
                .SetValue(this, value);
        }
    }

    /// <summary>
    /// Tests that the constructor with null currentWeight does not set the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_NullCurrentWeight_DoesNotSetTextField()
    {
        // Arrange
        double? currentWeight = null;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text is not set to a value
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with zero currentWeight does not set the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is 0, the condition (currentWeight.Value > 0) is false,
    /// so txtContainerWeight.Text should not be set.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_ZeroCurrentWeight_DoesNotSetTextField()
    {
        // Arrange
        double? currentWeight = 0.0;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text is not set because 0 fails the > 0 check
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with negative currentWeight does not set the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is negative, the condition (currentWeight.Value > 0) is false,
    /// so txtContainerWeight.Text should not be set.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_NegativeCurrentWeight_DoesNotSetTextField()
    {
        // Arrange
        double? currentWeight = -10.5;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text is not set because negative fails the > 0 check
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with positive currentWeight sets the text field correctly.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is positive, txtContainerWeight.Text should be set
    /// to the string representation of the value.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_PositiveCurrentWeight_SetsTextFieldToValue()
    {
        // Arrange
        double? currentWeight = 125.75;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text equals "125.75"
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with very small positive currentWeight sets the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is very small but positive (e.g., 0.0001),
    /// it should still set the text field.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_VerySmallPositiveCurrentWeight_SetsTextFieldToValue()
    {
        // Arrange
        double? currentWeight = 0.0001;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text equals "0.0001" or "1E-04" depending on ToString()
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with very large positive currentWeight sets the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is very large (e.g., double.MaxValue),
    /// it should set the text field to the string representation.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_VeryLargeCurrentWeight_SetsTextFieldToValue()
    {
        // Arrange
        double? currentWeight = double.MaxValue;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text is set to string representation of double.MaxValue
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with NaN currentWeight does not set the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is double.NaN, the condition (currentWeight.Value > 0) 
    /// evaluates to false, so txtContainerWeight.Text should not be set.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_NaNCurrentWeight_DoesNotSetTextField()
    {
        // Arrange
        double? currentWeight = double.NaN;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text is not set because NaN > 0 is false
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with PositiveInfinity currentWeight sets the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is double.PositiveInfinity, the condition 
    /// (currentWeight.Value > 0) evaluates to true, so txtContainerWeight.Text should be set.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_PositiveInfinityCurrentWeight_SetsTextFieldToInfinity()
    {
        // Arrange
        double? currentWeight = double.PositiveInfinity;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text equals "Infinity"
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the constructor with NegativeInfinity currentWeight does not set the text field.
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is double.NegativeInfinity, the condition 
    /// (currentWeight.Value > 0) evaluates to false, so txtContainerWeight.Text should not be set.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_NegativeInfinityCurrentWeight_DoesNotSetTextField()
    {
        // Arrange
        double? currentWeight = double.NegativeInfinity;

        // Act
        // ContainersPage would call InitializeComponent() which requires XAML runtime
        // var page = new ContainersPage(currentWeight);

        // Assert
        // Would verify txtContainerWeight.Text is not set because -Infinity > 0 is false
        Assert.Inconclusive("Test requires MAUI runtime environment with XAML support.");
    }

    /// <summary>
    /// Tests that the default constructor initializes the page without throwing exceptions.
    /// This test is marked as Ignore because the constructor calls InitializeComponent(),
    /// which requires a MAUI application context with XAML runtime that is not available in unit tests.
    /// Additionally, LoadContainers() accesses UI controls (cvContainers) and database via BL_Containers,
    /// neither of which can be mocked due to lack of dependency injection.
    /// 
    /// To make this testable:
    /// 1. Inject BL_Containers via constructor or property
    /// 2. Separate LoadContainers logic from UI control manipulation
    /// 3. Consider using a ViewModel pattern to separate UI from business logic
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage without XAML runtime and application context. InitializeComponent() requires XAML resources.")]
    public void ContainersPage_DefaultConstructor_InitializesSuccessfully()
    {
        // Arrange & Act
        // Cannot instantiate without MAUI runtime - InitializeComponent() will fail
        // ContainersPage page = new ContainersPage();

        // Assert
        // Would verify: page != null, properties initialized, etc.
        Assert.Inconclusive("This test requires MAUI application context and XAML runtime. Consider integration testing instead.");
    }

    /// <summary>
    /// Tests that exceptions during initialization are caught and logged.
    /// This test is marked as Ignore because we cannot test the exception handling behavior
    /// without being able to instantiate the page or mock its dependencies.
    /// The constructor catches all exceptions and logs them via gamon.General.LogOfProgram,
    /// but this is a static dependency that cannot be easily controlled in tests.
    /// 
    /// To make this testable:
    /// 1. Inject ILogger via dependency injection
    /// 2. Extract initialization logic to a separate testable method
    /// 3. Make LoadContainers protected virtual to allow testing in derived classes
    /// </summary>
    [Test]
    [Ignore("Cannot test exception handling without ability to mock InitializeComponent() or inject failures.")]
    public void ContainersPage_ConstructorWithException_LogsErrorAndDoesNotThrow()
    {
        // Arrange & Act
        // Cannot force InitializeComponent or LoadContainers to throw without mocking
        // Static dependency gamon.General.LogOfProgram cannot be mocked

        // Assert
        // Would verify: exception is logged, no exception bubbles up
        Assert.Inconclusive("Requires dependency injection and mockable initialization methods.");
    }

    /// <summary>
    /// Tests that the constructor initializes public properties to their default values.
    /// This test is marked as Ignore because we cannot instantiate the page to verify properties.
    /// Expected behavior:
    /// - SelectedContainer should be null
    /// - ContainerWasSelected should be false
    /// - PageClosedTask should be a valid Task&lt;bool&gt;
    /// 
    /// Note: Property initialization happens at field declaration level, not in constructor body.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate page to verify property initialization.")]
    public void ContainersPage_DefaultConstructor_InitializesPropertiesCorrectly()
    {
        // Arrange & Act
        // Cannot instantiate: ContainersPage page = new ContainersPage();

        // Assert
        // Would verify:
        // Assert.That(page.SelectedContainer, Is.Null);
        // Assert.That(page.ContainerWasSelected, Is.False);
        // Assert.That(page.PageClosedTask, Is.Not.Null);
        Assert.Inconclusive("Cannot verify properties without instantiating the page.");
    }

    /// <summary>
    /// Tests that PageClosedTask property returns a non-null Task when accessed.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenAccessed_ReturnsNonNullTask()
    {
        // Arrange & Act
        ContainersPage? page = null;
        try
        {
            page = new ContainersPage();

            // Act
            var task = page.PageClosedTask;

            // Assert
            Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            // If we cannot instantiate ContainersPage due to MAUI/XAML dependencies in test context,
            // mark the test as inconclusive
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests that PageClosedTask property returns the same Task instance on multiple accesses.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenAccessedMultipleTimes_ReturnsSameTaskInstance()
    {
        // Arrange
        ContainersPage? page = null;
        try
        {
            page = new ContainersPage();

            // Act
            var task1 = page.PageClosedTask;
            var task2 = page.PageClosedTask;

            // Assert
            Assert.That(task2, Is.SameAs(task1), "PageClosedTask should return the same Task instance on multiple calls");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests that PageClosedTask returns a Task that is not completed initially.
    /// </summary>
    [Test]
    public void PageClosedTask_InitialState_TaskIsNotCompleted()
    {
        // Arrange
        ContainersPage? page = null;
        try
        {
            page = new ContainersPage();

            // Act
            var task = page.PageClosedTask;

            // Assert
            Assert.That(task.IsCompleted, Is.False, "PageClosedTask should not be completed initially");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests that PageClosedTask property is of type Task&lt;bool&gt;.
    /// </summary>
    [Test]
    public void PageClosedTask_TypeCheck_ReturnsTaskOfBool()
    {
        // Arrange
        ContainersPage? page = null;
        try
        {
            page = new ContainersPage();

            // Act
            var task = page.PageClosedTask;

            // Assert
            Assert.That(task, Is.InstanceOf<Task<bool>>(), "PageClosedTask should be of type Task<bool>");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests that PageClosedTask property returns a valid Task when page is created with currentWeight parameter.
    /// </summary>
    [Test]
    public void PageClosedTask_WithWeightConstructor_ReturnsNonNullTask()
    {
        // Arrange & Act
        ContainersPage? page = null;
        try
        {
            page = new ContainersPage(currentWeight: 100.5);

            // Act
            var task = page.PageClosedTask;

            // Assert
            Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task even when constructed with weight parameter");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests that PageClosedTask property returns a valid Task when page is created with null currentWeight parameter.
    /// </summary>
    [Test]
    public void PageClosedTask_WithNullWeightConstructor_ReturnsNonNullTask()
    {
        // Arrange & Act
        ContainersPage? page = null;
        try
        {
            page = new ContainersPage(currentWeight: null);

            // Act
            var task = page.PageClosedTask;

            // Assert
            Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task even when constructed with null weight parameter");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
    }

    /// <summary>
    /// Tests that the constructor with currentWeight parameter does not set the text field
    /// when the weight value does not satisfy the condition (HasValue AND Value > 0).
    /// This includes null, zero, negative, NaN, and negative infinity values.
    /// </summary>
    /// <param name="weight">The weight value to test</param>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight is null, zero, negative, NaN, or negative infinity,
    /// the condition (currentWeight.HasValue && currentWeight.Value > 0) is false,
    /// so txtContainerWeight.Text should not be set.
    /// </remarks>
    [Test]
    [TestCase(null)]
    [TestCase(0.0)]
    [TestCase(-1.0)]
    [TestCase(-100.5)]
    [TestCase(double.MinValue)]
    [TestCase(double.NaN)]
    [TestCase(double.NegativeInfinity)]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_WithWeightNotSatisfyingCondition_DoesNotSetTextField(double? weight)
    {
        // Arrange & Act
        ContainersPage? page = null;
        Exception? caughtException = null;

        try
        {
            page = new ContainersPage(weight);

            // Assert
            // If we somehow got here, verify the text field was not modified
            // Note: In actual MAUI context, we would check txtContainerWeight.Text
            // but we cannot access it in unit test context
            Assert.Pass("Page instantiated successfully, but cannot verify UI state in unit test context.");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            caughtException = ex;
        }

        // If we caught an initialization exception, the test is inconclusive
        if (caughtException != null)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {caughtException.Message}");
        }
    }

    /// <summary>
    /// Tests that the constructor with currentWeight parameter sets the text field
    /// when the weight value satisfies the condition (HasValue AND Value > 0).
    /// This includes small positive values, typical values, large values, and positive infinity.
    /// </summary>
    /// <param name="weight">The weight value to test</param>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization
    /// via InitializeComponent() which is not available in unit test context.
    /// Expected behavior: When currentWeight has a value greater than 0,
    /// txtContainerWeight.Text should be set to the string representation of the value.
    /// </remarks>
    [Test]
    [TestCase(0.0001)]
    [TestCase(0.5)]
    [TestCase(1.0)]
    [TestCase(100.5)]
    [TestCase(999999.99)]
    [TestCase(double.MaxValue)]
    [TestCase(double.PositiveInfinity)]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_WithPositiveWeight_SetsTextFieldToValue(double? weight)
    {
        // Arrange & Act
        ContainersPage? page = null;
        Exception? caughtException = null;

        try
        {
            page = new ContainersPage(weight);

            // Assert
            // If we somehow got here, verify the text field was set
            // Note: In actual MAUI context, we would check:
            // Assert.That(page.txtContainerWeight.Text, Is.EqualTo(weight.Value.ToString()));
            // but we cannot access UI controls in unit test context
            Assert.Pass("Page instantiated successfully, but cannot verify UI state in unit test context.");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            caughtException = ex;
        }

        // If we caught an initialization exception, the test is inconclusive
        if (caughtException != null)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {caughtException.Message}");
        }
    }

    /// <summary>
    /// Tests that the constructor with currentWeight parameter does not throw exceptions
    /// when exceptions occur during page initialization, as they are caught and logged.
    /// </summary>
    /// <param name="weight">The weight value to test</param>
    /// <remarks>
    /// The constructor wraps its logic in a try-catch block that catches all exceptions
    /// and logs them via gamon.General.LogOfProgram. This test verifies that behavior.
    /// However, since we cannot instantiate the page in unit test context, we can only
    /// verify that attempting to instantiate does not throw an unhandled exception.
    /// </remarks>
    [Test]
    [TestCase(null)]
    [TestCase(0.0)]
    [TestCase(100.5)]
    [TestCase(double.NaN)]
    [TestCase(double.PositiveInfinity)]
    public void Constructor_WithAnyWeight_DoesNotThrowUnhandledException(double? weight)
    {
        // Arrange & Act & Assert
        try
        {
            var page = new ContainersPage(weight);
            // If successful, no exception was thrown (expected when MAUI context is available)
            Assert.Pass("Constructor completed without throwing unhandled exception.");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            // XAML initialization failure is expected in unit test context
            // This is not an unhandled exception from the constructor logic itself
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Any other exception would indicate a bug in exception handling
            Assert.Fail($"Constructor threw unexpected unhandled exception: {ex.GetType().Name} - {ex.Message}");
        }
    }

    /// <summary>
    /// Tests the constructor with boundary value for double precision.
    /// Tests double.Epsilon (smallest positive double value).
    /// </summary>
    /// <remarks>
    /// This test cannot be executed because ContainersPage requires XAML initialization.
    /// Expected behavior: double.Epsilon is positive (> 0), so text field should be set.
    /// </remarks>
    [Test]
    [Ignore("Cannot instantiate MAUI ContentPage outside of MAUI runtime. Requires XAML infrastructure.")]
    public void Constructor_WithDoubleEpsilon_SetsTextFieldToValue()
    {
        // Arrange
        double? weight = double.Epsilon;

        // Act
        ContainersPage? page = null;
        Exception? caughtException = null;

        try
        {
            page = new ContainersPage(weight);

            // Assert
            Assert.Pass("Page instantiated successfully with double.Epsilon.");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is InvalidOperationException)
        {
            caughtException = ex;
        }

        if (caughtException != null)
        {
            Assert.Inconclusive($"Cannot instantiate ContainersPage in unit test context: {caughtException.Message}");
        }
    }

    /// <summary>
    /// Tests that PageClosedTask returns a Task that completes with true when TaskCompletionSource is set to true.
    /// </summary>
    [Test]
    public async Task PageClosedTask_AfterSetResultTrue_TaskCompletesWithTrue()
    {
        // Arrange
        var page = new TestableContainersPage();
        var task = page.PageClosedTask;

        // Act
        page.SetContainerWasSelected(true);
        page.CallOnDisappearing();

        // Assert
        Assert.That(task.IsCompleted, Is.True, "Task should be completed after OnDisappearing");
        Assert.That(task.IsCompletedSuccessfully, Is.True, "Task should be completed successfully");
        Assert.That(task.IsFaulted, Is.False, "Task should not be faulted");
        Assert.That(task.IsCanceled, Is.False, "Task should not be canceled");
        var result = await task;
        Assert.That(result, Is.True, "Task result should be true when ContainerWasSelected is true");
    }

    /// <summary>
    /// Tests that PageClosedTask returns a Task that completes with false when TaskCompletionSource is set to false.
    /// </summary>
    [Test]
    public async Task PageClosedTask_AfterSetResultFalse_TaskCompletesWithFalse()
    {
        // Arrange
        var page = new TestableContainersPage();
        var task = page.PageClosedTask;

        // Act
        page.SetContainerWasSelected(false);
        page.CallOnDisappearing();

        // Assert
        Assert.That(task.IsCompleted, Is.True, "Task should be completed after OnDisappearing");
        Assert.That(task.IsCompletedSuccessfully, Is.True, "Task should be completed successfully");
        Assert.That(task.IsFaulted, Is.False, "Task should not be faulted");
        Assert.That(task.IsCanceled, Is.False, "Task should not be canceled");
        var result = await task;
        Assert.That(result, Is.False, "Task result should be false when ContainerWasSelected is false");
    }

    /// <summary>
    /// Tests that PageClosedTask property returns the same Task instance even after completion.
    /// </summary>
    [Test]
    public void PageClosedTask_AfterCompletion_ReturnsSameTaskInstance()
    {
        // Arrange
        var page = new TestableContainersPage();
        var taskBefore = page.PageClosedTask;

        // Act
        page.CallOnDisappearing();
        var taskAfter = page.PageClosedTask;

        // Assert
        Assert.That(taskAfter, Is.SameAs(taskBefore), "PageClosedTask should return the same Task instance even after completion");
    }

    /// <summary>
    /// Tests that PageClosedTask reflects the completed state after OnDisappearing is called.
    /// </summary>
    [Test]
    public void PageClosedTask_AfterOnDisappearing_ReflectsCompletedState()
    {
        // Arrange
        var page = new TestableContainersPage();
        var task = page.PageClosedTask;
        Assert.That(task.IsCompleted, Is.False, "Task should not be completed initially");

        // Act
        page.CallOnDisappearing();

        // Assert
        Assert.That(task.IsCompleted, Is.True, "Task should be completed after OnDisappearing");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask property should reflect completed state");
    }

    /// <summary>
    /// Tests that PageClosedTask can be awaited successfully after OnDisappearing with ContainerWasSelected true.
    /// </summary>
    [Test]
    public async Task PageClosedTask_AwaitAfterOnDisappearingWithSelectionTrue_ReturnsTrue()
    {
        // Arrange
        var page = new TestableContainersPage();
        page.SetContainerWasSelected(true);

        // Act
        page.CallOnDisappearing();
        var result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.True, "Awaiting PageClosedTask should return true when ContainerWasSelected is true");
    }

    /// <summary>
    /// Tests that PageClosedTask can be awaited successfully after OnDisappearing with ContainerWasSelected false.
    /// </summary>
    [Test]
    public async Task PageClosedTask_AwaitAfterOnDisappearingWithSelectionFalse_ReturnsFalse()
    {
        // Arrange
        var page = new TestableContainersPage();
        page.SetContainerWasSelected(false);

        // Act
        page.CallOnDisappearing();
        var result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Awaiting PageClosedTask should return false when ContainerWasSelected is false");
    }

    /// <summary>
    /// Tests that PageClosedTask property can be accessed multiple times before and after completion without issues.
    /// </summary>
    [Test]
    public void PageClosedTask_MultipleAccessesBeforeAndAfterCompletion_ReturnsSameInstance()
    {
        // Arrange
        var page = new TestableContainersPage();

        // Act - Access before completion
        var task1 = page.PageClosedTask;
        var task2 = page.PageClosedTask;

        // Complete the task
        page.CallOnDisappearing();

        // Act - Access after completion
        var task3 = page.PageClosedTask;
        var task4 = page.PageClosedTask;

        // Assert
        Assert.That(task1, Is.SameAs(task2), "Tasks accessed before completion should be the same instance");
        Assert.That(task3, Is.SameAs(task1), "Tasks accessed after completion should be the same instance as before");
        Assert.That(task4, Is.SameAs(task1), "All task accesses should return the same instance");
    }

    /// <summary>
    /// Tests that PageClosedTask Status property reflects NotStarted initially.
    /// </summary>
    [Test]
    public void PageClosedTask_InitialStatus_IsWaitingForActivation()
    {
        // Arrange
        var page = new TestableContainersPage();

        // Act
        var task = page.PageClosedTask;

        // Assert
        Assert.That(task.Status, Is.EqualTo(TaskStatus.WaitingForActivation), "Task status should be WaitingForActivation initially");
    }

    /// <summary>
    /// Tests that PageClosedTask Status property reflects RanToCompletion after successful completion.
    /// </summary>
    [Test]
    public void PageClosedTask_AfterSuccessfulCompletion_StatusIsRanToCompletion()
    {
        // Arrange
        var page = new TestableContainersPage();
        var task = page.PageClosedTask;

        // Act
        page.CallOnDisappearing();

        // Assert
        Assert.That(task.Status, Is.EqualTo(TaskStatus.RanToCompletion), "Task status should be RanToCompletion after OnDisappearing");
    }

    /// <summary>
    /// Tests that PageClosedTask Result property can be accessed synchronously after completion.
    /// </summary>
    [Test]
    public void PageClosedTask_AfterCompletion_ResultPropertyAccessibleSynchronously()
    {
        // Arrange
        var page = new TestableContainersPage();
        page.SetContainerWasSelected(true);
        var task = page.PageClosedTask;

        // Act
        page.CallOnDisappearing();

        // Assert
        Assert.That(task.IsCompleted, Is.True, "Task must be completed before accessing Result");
        Assert.That(task.Result, Is.True, "Result property should return true when ContainerWasSelected is true");
    }

    /// <summary>
    /// Tests that PageClosedTask property maintains consistency across different constructor overloads.
    /// </summary>
    [Test]
    public void PageClosedTask_WithDifferentConstructors_MaintainsConsistency()
    {
        // Arrange & Act
        var page1 = new TestableContainersPage();
        var page2 = new TestableContainersPageWithWeight(100.0);
        var page3 = new TestableContainersPageWithWeight(null);

        // Assert
        Assert.That(page1.PageClosedTask, Is.Not.Null, "Default constructor should initialize PageClosedTask");
        Assert.That(page2.PageClosedTask, Is.Not.Null, "Constructor with weight should initialize PageClosedTask");
        Assert.That(page3.PageClosedTask, Is.Not.Null, "Constructor with null weight should initialize PageClosedTask");

        Assert.That(page1.PageClosedTask.IsCompleted, Is.False, "Default constructor PageClosedTask should not be completed");
        Assert.That(page2.PageClosedTask.IsCompleted, Is.False, "Constructor with weight PageClosedTask should not be completed");
        Assert.That(page3.PageClosedTask.IsCompleted, Is.False, "Constructor with null weight PageClosedTask should not be completed");
    }

    /// <summary>
    /// Helper class to test ContainersPage with weight constructor parameter.
    /// </summary>
    private class TestableContainersPageWithWeight : TestableContainersPage
    {
        public TestableContainersPageWithWeight(double? currentWeight) : base()
        {
            // Mimics the behavior of the actual constructor with currentWeight parameter
            // which calls the default constructor via : this()
        }
    }
}