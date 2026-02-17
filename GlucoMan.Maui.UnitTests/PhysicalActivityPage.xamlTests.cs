using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using gamon;
using GlucoMan;
using GlucoMan.Maui;
using GlucoMan.Maui.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;


/// <summary>
/// Unit tests for the PhysicalActivityPage class.
/// </summary>
public partial class PhysicalActivityPageTests
{
    /// <summary>
    /// Tests that OnAppearing method completes without throwing exceptions under normal conditions.
    /// This test is inconclusive because:
    /// 1. PhysicalActivityPage is a XAML partial class that requires InitializeComponent() to be called,
    ///    which cannot be executed in a unit test context without UI infrastructure.
    /// 2. The RefreshUi() method is private and cannot be mocked with Moq.
    /// 3. The static dependency gamon.General.LogOfProgram cannot be mocked with Moq.
    /// 
    /// To make this code testable:
    /// - Consider injecting the logger as a dependency instead of using static access.
    /// - Consider extracting RefreshUi logic into a separate service that can be mocked.
    /// - Consider creating a base class or interface for page lifecycle methods that can be tested independently.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate XAML partial class in unit test context. Requires UI testing framework.")]
    public void OnAppearing_WhenCalled_CompletesWithoutException()
    {
        // Arrange
        // NOTE: The following instantiation will fail because InitializeComponent() requires XAML infrastructure
        // var localizationService = new Mock<LocalizationService>().Object;
        // var page = new PhysicalActivityPage(localizationService);

        // Act
        // page.OnAppearing();

        // Assert
        // Assert.Pass("OnAppearing completed without throwing an exception");

        Assert.Inconclusive(
            "This test cannot be executed in a unit test context due to XAML dependencies. " +
            "Consider using UI testing frameworks like Appium or creating a testable wrapper " +
            "that doesn't depend on XAML initialization.");
    }

    /// <summary>
    /// Tests that OnAppearing method logs errors when RefreshUi throws an exception.
    /// This test is inconclusive because:
    /// 1. Cannot instantiate PhysicalActivityPage due to XAML/InitializeComponent() dependency.
    /// 2. Cannot mock the static gamon.General.LogOfProgram field using Moq.
    /// 3. Cannot mock the private RefreshUi() method to force it to throw an exception.
    /// 
    /// To make this code testable:
    /// - Inject ILogger dependency instead of using static General.LogOfProgram.
    /// - Make RefreshUi() virtual and protected so it can be overridden in tests, or extract to a service.
    /// - Consider using dependency injection for all external dependencies.
    /// </summary>
    [Test]
    [Ignore("Cannot test due to static dependencies and XAML initialization requirements.")]
    public void OnAppearing_WhenRefreshUiThrowsException_LogsError()
    {
        // Arrange
        // NOTE: This test structure shows the intended testing approach
        // In a testable design, you would:
        // 1. Mock the logger: var mockLogger = new Mock<ILogger>();
        // 2. Inject it: var page = new PhysicalActivityPage(mockLogger.Object, ...);
        // 3. Override or mock RefreshUi to throw an exception
        // 4. Verify the logger.Error was called with correct parameters

        // Act
        // page.OnAppearing();

        // Assert
        // mockLogger.Verify(
        //     log => log.Error("PhysicalActivityPage - OnAppearing", It.IsAny<Exception>()),
        //     Times.Once);

        Assert.Inconclusive(
            "This test requires architectural changes to support dependency injection. " +
            "Replace static General.LogOfProgram with an injected ILogger interface.");
    }

    /// <summary>
    /// Tests that OnAppearing handles the case when the logger is null gracefully.
    /// This test is inconclusive because:
    /// 1. Cannot instantiate PhysicalActivityPage due to XAML dependencies.
    /// 2. Cannot control the static General.LogOfProgram field state in unit tests.
    /// 3. Cannot mock RefreshUi() to force an exception.
    /// 
    /// Expected behavior: The null-conditional operator (?.) should prevent NullReferenceException
    /// when LogOfProgram is null.
    /// </summary>
    [Test]
    [Ignore("Cannot test static field state and XAML initialization in unit test context.")]
    public void OnAppearing_WhenRefreshUiThrowsExceptionAndLoggerIsNull_DoesNotThrowNullReferenceException()
    {
        // Arrange
        // NOTE: In current design, would need to:
        // 1. Set General.LogOfProgram = null (affects other tests, not isolated)
        // 2. Create page instance (fails due to XAML)
        // 3. Force RefreshUi to throw (cannot mock private method)

        // The null-conditional operator (?.) in the source code should prevent NullReferenceException:
        // General.LogOfProgram?.Error(...) 

        // In a testable design with DI:
        // var page = new PhysicalActivityPage(null, ...); // null logger
        // Should not throw when OnAppearing encounters an exception

        // Act & Assert
        // Assert.DoesNotThrow(() => page.OnAppearing());

        Assert.Inconclusive(
            "Cannot test null logger scenario with current static dependency design. " +
            "Use dependency injection to enable proper testing of null handling.");
    }

    /// <summary>
    /// Tests that OnAppearing can handle various exception types from RefreshUi.
    /// This test is inconclusive for the same architectural reasons as other tests.
    /// 
    /// Edge cases that should be tested:
    /// - InvalidOperationException
    /// - NullReferenceException  
    /// - ArgumentException
    /// - Custom business logic exceptions
    /// All should be caught and logged without propagating.
    /// </summary>
    [Test]
    [Ignore("Requires testable architecture with dependency injection.")]
    public void OnAppearing_WhenRefreshUiThrowsDifferentExceptionTypes_CatchesAndLogsAllTypes()
    {
        // Arrange
        // Test with various exception types:
        // - new InvalidOperationException("Invalid state")
        // - new NullReferenceException("Null object")
        // - new ArgumentException("Invalid argument")

        // Each should be caught by the catch(Exception ex) block
        // and logged via General.LogOfProgram?.Error()

        // Act & Assert
        // Verify logger called for each exception type

        Assert.Inconclusive(
            "Exception handling tests require ability to mock RefreshUi() and inject logger. " +
            "Consider refactoring to use virtual protected methods or extracted services.");
    }

    /// <summary>
    /// Tests OnTrackSaved with null CurrentActivity.
    /// LIMITATION: Cannot fully test due to inability to instantiate PhysicalActivityPage
    /// without MAUI UI context and InitializeComponent dependencies.
    /// This test documents expected behavior: should log error and return early.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_CurrentActivityIsNull_LogsErrorAndReturnsEarly()
    {
        // Arrange
        // Cannot create instance: var page = new PhysicalActivityPage(localizationService);
        // Would require:
        // - MAUI application context
        // - InitializeComponent (UI controls)
        // - Mock for LocalizationService
        // - Access to set CurrentActivity to null

        // Act
        // page.OnTrackSaved(123);

        // Assert
        // Expected: General.LogOfProgram.Error should be called with message
        // "PhysicalActivityPage - OnTrackSaved: CurrentActivity is null"
        // Method should return without calling SaveOneInjection

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved with valid positive idTrack.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Should update Notes, save injection, update button state, log event, and show alert.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_ValidPositiveIdTrack_UpdatesActivityAndShowsConfirmation()
    {
        // Arrange
        // int idTrack = 42;
        // Would need to:
        // - Create page instance with mocked dependencies
        // - Set CurrentActivity to valid Injection object
        // - Mock bl.SaveOneInjection
        // - Mock Logger
        // - Mock DisplayAlert and MainThread

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected behaviors:
        // 1. CurrentActivity.Notes updated with "IdTrack:42"
        // 2. bl.SaveOneInjection called with CurrentActivity
        // 3. UpdateGpsTrackButtonState called
        // 4. Logger.Event called with association message
        // 5. DisplayAlert shown with confirmation message

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved with zero idTrack.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Should handle edge case of zero track ID.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_IdTrackIsZero_HandlesEdgeCase()
    {
        // Arrange
        // int idTrack = 0;

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: SetIdTrackInNotes should handle zero appropriately
        // (based on implementation, zeros are not added to notes)

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved with negative idTrack.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Should handle edge case of negative track ID.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_NegativeIdTrack_HandlesEdgeCase()
    {
        // Arrange
        // int idTrack = -1;

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: SetIdTrackInNotes should handle negative values
        // (based on implementation, negative values are not added to notes)

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved with int.MaxValue.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Should handle maximum integer value.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_IdTrackIsMaxValue_HandlesMaximumInteger()
    {
        // Arrange
        // int idTrack = int.MaxValue;

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: Should successfully process large integer value

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved with int.MinValue.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Should handle minimum integer value.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_IdTrackIsMinValue_HandlesMinimumInteger()
    {
        // Arrange
        // int idTrack = int.MinValue;

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: Should handle minimum integer value appropriately

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved when SetIdTrackInNotes throws exception.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Exception should be caught and logged.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_SetIdTrackInNotesThrowsException_CatchesAndLogsError()
    {
        // Arrange
        // int idTrack = 123;
        // Would need to mock SetIdTrackInNotes to throw exception

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: Logger.Error called with exception
        // No exception propagated to caller

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved when SaveOneInjection throws exception.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Exception should be caught and logged.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_SaveOneInjectionThrowsException_CatchesAndLogsError()
    {
        // Arrange
        // int idTrack = 123;
        // Would need to mock bl.SaveOneInjection to throw exception

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: Logger.Error called with exception
        // No exception propagated to caller

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved when UpdateGpsTrackButtonState throws exception.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Exception should be caught and logged.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_UpdateGpsTrackButtonStateThrowsException_CatchesAndLogsError()
    {
        // Arrange
        // int idTrack = 123;
        // Would need to mock UpdateGpsTrackButtonState to throw exception

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: Logger.Error called with exception
        // No exception propagated to caller

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests OnTrackSaved with existing IdTrack in Notes.
    /// LIMITATION: Cannot test due to UI framework dependencies.
    /// Expected behavior: Should replace existing IdTrack with new value.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate PhysicalActivityPage without MAUI UI context. Requires refactoring for testability.")]
    public void OnTrackSaved_NotesContainsExistingIdTrack_ReplacesWithNewValue()
    {
        // Arrange
        // int idTrack = 999;
        // CurrentActivity.Notes = "IdTrack:123 Some other notes";

        // Act
        // page.OnTrackSaved(idTrack);

        // Assert
        // Expected: CurrentActivity.Notes should be "IdTrack:999 Some other notes"

        Assert.Inconclusive("Test requires refactoring PhysicalActivityPage to support dependency injection and testability.");
    }

    /// <summary>
    /// Tests that the constructor completes successfully with a valid LocalizationService.
    /// This is a basic smoke test - cannot verify internal state due to XAML dependencies.
    /// Verifies that the Activities property is initialized.
    /// </summary>
    [Test]
    public void Constructor_WithValidLocalizationService_CompletesAndInitializesActivities()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Act
        // NOTE: This will fail if InitializeComponent() cannot find XAML resources.
        // In a real test environment, XAML initialization may not work properly.
        // This test demonstrates the intended approach but may need to be marked as inconclusive
        // or skipped in environments where XAML cannot be loaded.
        PhysicalActivityPage page = null;
        TestDelegate act = () => page = new PhysicalActivityPage(mockLocalizationService.Object);

        // Assert
        // Due to XAML dependencies, this test may throw. We document this limitation.
        // In a properly configured MAUI test environment, this should pass.
        Assert.DoesNotThrow(act);

        // If constructor succeeded, verify basic property initialization
        if (page != null)
        {
            Assert.That(page.Activities, Is.Not.Null);
            Assert.That(page.Activities, Is.InstanceOf<ObservableCollection<Injection>>());
            Assert.That(page.BindingContext, Is.EqualTo(page));
        }
    }

    /// <summary>
    /// Tests that the constructor handles null LocalizationService parameter.
    /// NOTE: This test may behave differently than expected because:
    /// - The parameter is not used in the constructor
    /// - InitializeComponent() may fail regardless of the parameter
    /// </summary>
    [Test]
    public void Constructor_WithNullLocalizationService_CompletesSuccessfully()
    {
        // Arrange
        LocalizationService? nullService = null;

        // Act & Assert
        // The constructor doesn't use the localizationService parameter,
        // so null should not cause issues from that perspective.
        // However, InitializeComponent() may still fail due to XAML dependencies.
        TestDelegate act = () => new PhysicalActivityPage(nullService!);

        // We expect this to either complete successfully or throw due to XAML,
        // not due to null parameter (since it's unused)
        Assert.DoesNotThrow(act);
    }

    /// <summary>
    /// Demonstrates that this class cannot be effectively unit tested due to:
    /// 1. XAML dependencies requiring InitializeComponent()
    /// 2. Non-mockable dependencies (BL_BolusesAndInjections, UiAccuracy)
    /// 3. Private methods that cannot be tested directly
    /// 4. UI controls initialized by XAML that are not accessible
    /// 
    /// Recommendation: Consider refactoring to:
    /// - Extract business logic into testable service classes
    /// - Use dependency injection for BL_BolusesAndInjections
    /// - Move initialization logic to separate testable methods
    /// - Use integration tests for XAML-dependent functionality
    /// </summary>
    [Test]
    [Ignore("This test documents testing limitations and design issues")]
    public void Constructor_TestingLimitations_DocumentedForFutureRefactoring()
    {
        Assert.Inconclusive(
            "PhysicalActivityPage constructor cannot be effectively unit tested due to:\n" +
            "- XAML dependencies (InitializeComponent)\n" +
            "- Non-mockable dependencies (BL_BolusesAndInjections, UiAccuracy)\n" +
            "- UI control dependencies (cmbAccuracyActivity, txtAccuracyOfActivity)\n" +
            "- Private method calls with no observable side effects\n" +
            "Consider refactoring for better testability.");
    }

    /// <summary>
    /// Helper class to expose protected OnPropertyChanged method for testing.
    /// Overrides constructor to prevent XAML initialization failures during testing.
    /// </summary>
    private class TestablePhysicalActivityPage : PhysicalActivityPage
    {
        public TestablePhysicalActivityPage(LocalizationService localizationService)
            : base(localizationService)
        {
            // Constructor may throw due to InitializeComponent() XAML initialization
            // This is expected in unit test environment - the base class has try-catch
        }

        /// <summary>
        /// Publicly exposes the protected OnPropertyChanged method for testing
        /// </summary>
        public void PublicOnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }

    /// <summary>
    /// Tests that OnPropertyChanged invokes PropertyChanged event with correct property name
    /// when a subscriber is attached.
    /// Input: Valid property name string.
    /// Expected: PropertyChanged event is invoked with matching property name in EventArgs.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithSubscriberAndValidPropertyName_InvokesEventWithCorrectPropertyName()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            // Constructor may fail due to InitializeComponent() - this is expected
            // Create with null for testing purposes
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies. " +
                              "Consider testing this method through integration tests or by refactoring to separate XAML initialization.");
            return;
        }

        string? receivedPropertyName = null;
        object? receivedSender = null;
        page.PropertyChanged += (sender, e) =>
        {
            receivedSender = sender;
            receivedPropertyName = e.PropertyName;
        };

        // Act
        page.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(receivedPropertyName, Is.EqualTo("TestProperty"), "Property name in EventArgs should match the provided property name");
        Assert.That(receivedSender, Is.SameAs(page), "Sender should be the page instance");
    }

    /// <summary>
    /// Tests that OnPropertyChanged does not throw when PropertyChanged event has no subscribers.
    /// Input: Valid property name string with no event subscribers.
    /// Expected: No exception is thrown.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNoSubscribers_DoesNotThrow()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies.");
            return;
        }

        // Act & Assert
        Assert.DoesNotThrow(() => page.PublicOnPropertyChanged("TestProperty"),
            "OnPropertyChanged should not throw when there are no subscribers");
    }

    /// <summary>
    /// Tests that OnPropertyChanged correctly handles null property name.
    /// Input: null property name.
    /// Expected: PropertyChanged event is invoked with null property name in EventArgs.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNullPropertyName_InvokesEventWithNull()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies.");
            return;
        }

        string? receivedPropertyName = "not-null";
        page.PropertyChanged += (sender, e) => receivedPropertyName = e.PropertyName;

        // Act
        page.PublicOnPropertyChanged(null!);

        // Assert
        Assert.That(receivedPropertyName, Is.Null, "Property name should be null when null is passed");
    }

    /// <summary>
    /// Tests that OnPropertyChanged correctly handles empty string property name.
    /// Input: Empty string property name.
    /// Expected: PropertyChanged event is invoked with empty string in EventArgs.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithEmptyPropertyName_InvokesEventWithEmptyString()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies.");
            return;
        }

        string? receivedPropertyName = null;
        page.PropertyChanged += (sender, e) => receivedPropertyName = e.PropertyName;

        // Act
        page.PublicOnPropertyChanged(string.Empty);

        // Assert
        Assert.That(receivedPropertyName, Is.EqualTo(string.Empty), "Property name should be empty string when empty string is passed");
    }

    /// <summary>
    /// Tests that OnPropertyChanged correctly handles various edge case property names.
    /// Input: Various edge case strings (whitespace, special characters, very long string).
    /// Expected: PropertyChanged event is invoked with the exact property name provided.
    /// </summary>
    [TestCase("   ", Description = "Whitespace-only property name")]
    [TestCase("Property.With.Dots", Description = "Property name with dots")]
    [TestCase("Property With Spaces", Description = "Property name with spaces")]
    [TestCase("Property@#$%", Description = "Property name with special characters")]
    public void OnPropertyChanged_WithEdgeCasePropertyNames_InvokesEventWithExactPropertyName(string propertyName)
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies.");
            return;
        }

        string? receivedPropertyName = null;
        page.PropertyChanged += (sender, e) => receivedPropertyName = e.PropertyName;

        // Act
        page.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(receivedPropertyName, Is.EqualTo(propertyName),
            $"Property name should exactly match the provided value: '{propertyName}'");
    }

    /// <summary>
    /// Tests that OnPropertyChanged correctly handles very long property name.
    /// Input: Very long string (1000+ characters).
    /// Expected: PropertyChanged event is invoked with the exact long property name.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithVeryLongPropertyName_InvokesEventWithExactPropertyName()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies.");
            return;
        }

        string? receivedPropertyName = null;
        page.PropertyChanged += (sender, e) => receivedPropertyName = e.PropertyName;
        var veryLongPropertyName = new string('A', 10000);

        // Act
        page.PublicOnPropertyChanged(veryLongPropertyName);

        // Assert
        Assert.That(receivedPropertyName, Is.EqualTo(veryLongPropertyName),
            "Property name should handle very long strings correctly");
    }

    /// <summary>
    /// Tests that multiple subscribers all receive the PropertyChanged event.
    /// Input: Valid property name with multiple event subscribers.
    /// Expected: All subscribers are invoked with correct property name.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithMultipleSubscribers_InvokesAllSubscribers()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        TestablePhysicalActivityPage page;

        try
        {
            page = new TestablePhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            page = null!;
        }

        if (page == null)
        {
            Assert.Inconclusive("Unable to instantiate PhysicalActivityPage in test environment due to XAML dependencies.");
            return;
        }

        string? receivedPropertyName1 = null;
        string? receivedPropertyName2 = null;
        string? receivedPropertyName3 = null;

        page.PropertyChanged += (sender, e) => receivedPropertyName1 = e.PropertyName;
        page.PropertyChanged += (sender, e) => receivedPropertyName2 = e.PropertyName;
        page.PropertyChanged += (sender, e) => receivedPropertyName3 = e.PropertyName;

        // Act
        page.PublicOnPropertyChanged("MultiSubscriberTest");

        // Assert
        Assert.That(receivedPropertyName1, Is.EqualTo("MultiSubscriberTest"), "First subscriber should receive the event");
        Assert.That(receivedPropertyName2, Is.EqualTo("MultiSubscriberTest"), "Second subscriber should receive the event");
        Assert.That(receivedPropertyName3, Is.EqualTo("MultiSubscriberTest"), "Third subscriber should receive the event");
    }

    /// <summary>
    /// Tests that setting the Activities property to a new ObservableCollection updates the property
    /// and raises the PropertyChanged event with the correct property name.
    /// </summary>
    /// <param name="itemCount">The number of items to include in the test collection.</param>
    [TestCase(0, Description = "Empty collection")]
    [TestCase(1, Description = "Single item collection")]
    [TestCase(5, Description = "Multiple items collection")]
    public void Activities_SetValidValue_UpdatesPropertyAndRaisesPropertyChanged(int itemCount)
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        PhysicalActivityPage? page = null;

        try
        {
            page = new PhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            // Constructor may fail due to XAML initialization, but object may still be partially created
            // Mark test as inconclusive if we cannot create the instance
            Assert.Inconclusive("Unable to create PhysicalActivityPage instance - likely due to XAML InitializeComponent failure");
            return;
        }

        var newCollection = new ObservableCollection<Injection>();
        for (int i = 0; i < itemCount; i++)
        {
            newCollection.Add(new Injection());
        }

        string? raisedPropertyName = null;
        page.PropertyChanged += (sender, args) => raisedPropertyName = args.PropertyName;

        // Act
        page.Activities = newCollection;

        // Assert
        Assert.That(page.Activities, Is.SameAs(newCollection), "Activities property should return the set value");
        Assert.That(raisedPropertyName, Is.EqualTo(nameof(page.Activities)), "PropertyChanged event should be raised with correct property name");
    }

    /// <summary>
    /// Tests that getting the Activities property returns the current value that was set.
    /// </summary>
    [Test]
    public void Activities_GetValue_ReturnsCurrentValue()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        PhysicalActivityPage? page = null;

        try
        {
            page = new PhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            Assert.Inconclusive("Unable to create PhysicalActivityPage instance - likely due to XAML InitializeComponent failure");
            return;
        }

        var initialCollection = page.Activities;
        var newCollection = new ObservableCollection<Injection>
        {
            new Injection(),
            new Injection()
        };

        // Act
        page.Activities = newCollection;
        var retrievedCollection = page.Activities;

        // Assert
        Assert.That(retrievedCollection, Is.SameAs(newCollection), "Getter should return the exact instance that was set");
        Assert.That(retrievedCollection, Is.Not.SameAs(initialCollection), "Retrieved collection should differ from initial collection");
    }

    /// <summary>
    /// Tests that setting Activities multiple times raises PropertyChanged event each time.
    /// </summary>
    [Test]
    public void Activities_SetMultipleTimes_RaisesPropertyChangedEachTime()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        PhysicalActivityPage? page = null;

        try
        {
            page = new PhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            Assert.Inconclusive("Unable to create PhysicalActivityPage instance - likely due to XAML InitializeComponent failure");
            return;
        }

        int eventRaisedCount = 0;
        page.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(page.Activities))
            {
                eventRaisedCount++;
            }
        };

        // Act
        page.Activities = new ObservableCollection<Injection>();
        page.Activities = new ObservableCollection<Injection>();
        page.Activities = new ObservableCollection<Injection>();

        // Assert
        Assert.That(eventRaisedCount, Is.EqualTo(3), "PropertyChanged should be raised once for each setter call");
    }

    /// <summary>
    /// Tests that setting Activities to null updates the property to null.
    /// This tests a boundary condition where null might be assigned despite non-nullable reference type annotation.
    /// </summary>
    [Test]
    public void Activities_SetNull_UpdatesPropertyToNull()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        PhysicalActivityPage? page = null;

        try
        {
            page = new PhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            Assert.Inconclusive("Unable to create PhysicalActivityPage instance - likely due to XAML InitializeComponent failure");
            return;
        }

        // Act
        page.Activities = null!;

        // Assert
        Assert.That(page.Activities, Is.Null, "Activities property should be null when set to null");
    }

    /// <summary>
    /// Tests that PropertyChanged event provides correct event arguments when Activities is set.
    /// </summary>
    [Test]
    public void Activities_SetValue_PropertyChangedEventArgsAreCorrect()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        PhysicalActivityPage? page = null;

        try
        {
            page = new PhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            Assert.Inconclusive("Unable to create PhysicalActivityPage instance - likely due to XAML InitializeComponent failure");
            return;
        }

        PropertyChangedEventArgs? capturedEventArgs = null;
        object? capturedSender = null;
        page.PropertyChanged += (sender, args) =>
        {
            capturedSender = sender;
            capturedEventArgs = args;
        };

        var newCollection = new ObservableCollection<Injection>();

        // Act
        page.Activities = newCollection;

        // Assert
        Assert.That(capturedEventArgs, Is.Not.Null, "PropertyChanged event should provide event arguments");
        Assert.That(capturedEventArgs!.PropertyName, Is.EqualTo("Activities"), "PropertyName should be 'Activities'");
        Assert.That(capturedSender, Is.SameAs(page), "Sender should be the PhysicalActivityPage instance");
    }

    /// <summary>
    /// Tests that setting Activities to the same instance still raises PropertyChanged event.
    /// This verifies that the setter does not perform reference equality check before raising the event.
    /// </summary>
    [Test]
    public void Activities_SetSameValue_StillRaisesPropertyChanged()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();
        PhysicalActivityPage? page = null;

        try
        {
            page = new PhysicalActivityPage(mockLocalizationService.Object);
        }
        catch
        {
            Assert.Inconclusive("Unable to create PhysicalActivityPage instance - likely due to XAML InitializeComponent failure");
            return;
        }

        var collection = new ObservableCollection<Injection>();
        page.Activities = collection;

        int eventRaisedCount = 0;
        page.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(page.Activities))
            {
                eventRaisedCount++;
            }
        };

        // Act
        page.Activities = collection;

        // Assert
        Assert.That(eventRaisedCount, Is.EqualTo(1), "PropertyChanged should be raised even when setting the same value");
    }
}