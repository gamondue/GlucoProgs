using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using GlucoMan;
using GlucoMan.Maui;
using GlucoMan.Maui.Resources;
using GlucoMan.Maui.Resources.Strings;
using GlucoMan.Maui.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;


/// <summary>
/// Unit tests for TrackPage constructor that accepts a track ID parameter.
/// 
/// NOTE: This class has severe testability limitations due to its design:
/// - Inherits from ContentPage (MAUI UI framework class)
/// - Calls InitializeComponent() which requires XAML compilation and MAUI runtime
/// - Accesses Application.Current.Handler.MauiContext.Services in base constructor
/// - Manipulates UI controls directly in constructor
/// - Uses static dependencies (General.LogOfProgram) that cannot be mocked
/// - Calls async void method (LoadAndDisplayTrack) in constructor
/// 
/// These tests are provided as scaffolding with clear explanations of limitations.
/// To make this code properly testable, consider:
/// 1. Extract business logic from UI layer
/// 2. Use dependency injection for all dependencies
/// 3. Avoid UI operations in constructors
/// 4. Create a view model or presenter pattern to separate concerns
/// </summary>
[TestFixture]
public partial class TrackPageTests
{
    /// <summary>
    /// Tests that the constructor with idTrack parameter sets isViewOnlyMode and loadedTrackId fields.
    /// 
    /// LIMITATION: This test cannot execute due to MAUI framework dependencies.
    /// - InitializeComponent() requires XAML runtime
    /// - Application.Current is null in unit tests
    /// - UI controls (buttons, labels, WebView) are not available
    /// 
    /// This test is marked as Inconclusive to document expected behavior.
    /// </summary>
    /// <param name="idTrack">The track ID to load</param>
    [TestCase(1)]
    [TestCase(100)]
    [TestCase(int.MaxValue)]
    public void Constructor_WithValidIdTrack_ShouldSetViewOnlyModeAndLoadedTrackId(int idTrack)
    {
        // ARRANGE
        // Cannot arrange due to framework dependencies

        // ACT & ASSERT
        Assert.Inconclusive(
            "This constructor cannot be unit tested due to MAUI framework dependencies:\n" +
            "- InitializeComponent() requires XAML compilation and MAUI runtime\n" +
            "- Application.Current.Handler.MauiContext.Services requires running MAUI application\n" +
            "- UI controls are accessed in constructor (lblCurrentPosition, buttons, mapWebView)\n" +
            "- BL_GpsTracking is directly instantiated without dependency injection\n" +
            "\n" +
            "Expected behavior (cannot be verified):\n" +
            $"- isViewOnlyMode should be set to true\n" +
            $"- loadedTrackId should be set to {idTrack}\n" +
            $"- LoadAndDisplayTrack({idTrack}) should be called\n" +
            "\n" +
            "To make this testable:\n" +
            "1. Extract LoadAndDisplayTrack call to OnAppearing or separate initialization method\n" +
            "2. Inject dependencies (IBackgroundGpsService, BL_GpsTracking) via constructor\n" +
            "3. Use MVVM pattern to separate UI from business logic\n" +
            "4. Consider integration tests using MAUI testing framework instead"
        );
    }

    /// <summary>
    /// Tests that the constructor with negative idTrack parameter handles the value correctly.
    /// 
    /// LIMITATION: Cannot verify actual behavior due to framework dependencies.
    /// </summary>
    /// <param name="idTrack">Negative or zero track ID values</param>
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    public void Constructor_WithInvalidIdTrack_ShouldStillSetFieldsAndAttemptLoad(int idTrack)
    {
        // ARRANGE & ACT & ASSERT
        Assert.Inconclusive(
            $"Cannot test constructor with idTrack={idTrack} due to MAUI framework dependencies.\n" +
            "\n" +
            "Expected behavior (based on code analysis):\n" +
            "- Constructor should not validate idTrack parameter\n" +
            $"- isViewOnlyMode would be set to true\n" +
            $"- loadedTrackId would be set to {idTrack}\n" +
            $"- LoadAndDisplayTrack({idTrack}) would be called\n" +
            "- LoadAndDisplayTrack would likely fail to find track and show error dialog\n" +
            "\n" +
            "Recommendation: Add parameter validation in constructor to fail fast:\n" +
            "if (idTrack <= 0) throw new ArgumentOutOfRangeException(nameof(idTrack), \"Track ID must be positive\");"
        );
    }

    /// <summary>
    /// Tests exception handling when LoadAndDisplayTrack throws an exception.
    /// 
    /// LIMITATION: Cannot test actual exception handling due to framework dependencies.
    /// The exception would be caught and logged via General.LogOfProgram?.Error().
    /// </summary>
    [Test]
    public void Constructor_WhenLoadAndDisplayTrackThrows_ShouldCatchAndLogException()
    {
        // ARRANGE & ACT & ASSERT
        Assert.Inconclusive(
            "Cannot test exception handling in constructor due to MAUI framework dependencies.\n" +
            "\n" +
            "Expected behavior (based on code analysis):\n" +
            "- If LoadAndDisplayTrack throws any exception, it should be caught\n" +
            "- Exception should be logged via General.LogOfProgram?.Error()\n" +
            "- Constructor should complete without throwing (exception is swallowed)\n" +
            "- The error message format would be: \"TrackPage - Constructor with IdTrack={idTrack}\"\n" +
            "\n" +
            "Note: Exception swallowing in constructors is generally an anti-pattern.\n" +
            "Consider allowing exceptions to propagate or using factory pattern for async initialization."
        );
    }

    /// <summary>
    /// Documents that the constructor calls the parameterless constructor which has extensive dependencies.
    /// 
    /// LIMITATION: The base constructor chain makes testing impossible without a running MAUI application.
    /// </summary>
    [Test]
    public void Constructor_CallsParameterlessConstructor_WithAllDependencies()
    {
        // ARRANGE & ACT & ASSERT
        Assert.Inconclusive(
            "The constructor chains to the parameterless constructor via ': this()', which:\n" +
            "1. Calls InitializeComponent() - requires XAML compilation\n" +
            "2. Creates new BL_GpsTracking() - direct instantiation\n" +
            "3. Accesses Application.Current.Handler.MauiContext.Services - requires MAUI runtime\n" +
            "4. Retrieves IBackgroundGpsService from DI container\n" +
            "5. Subscribes to backgroundGpsService.OnPositionRecorded event\n" +
            "6. Accesses UI controls: lblCurrentPosition, btnStartTracking, btnStopTracking, etc.\n" +
            "7. Calls InitializeMap() which creates WebView HTML content\n" +
            "\n" +
            "All of these operations require a fully initialized MAUI application context.\n" +
            "Unit testing is not feasible without significant refactoring.\n" +
            "\n" +
            "Recommended refactoring:\n" +
            "- Move initialization logic to OnAppearing or separate Initialize method\n" +
            "- Use dependency injection for all dependencies\n" +
            "- Extract business logic to separate testable classes\n" +
            "- Consider MVVM pattern with testable ViewModel"
        );
    }

    /// <summary>
    /// Documents the expected call to LoadAndDisplayTrack with various track ID values.
    /// 
    /// LIMITATION: Cannot verify method invocation due to async void method and framework dependencies.
    /// </summary>
    /// <param name="idTrack">Track ID parameter values</param>
    [TestCase(1)]
    [TestCase(999)]
    [TestCase(int.MaxValue)]
    public void Constructor_WithIdTrack_ShouldCallLoadAndDisplayTrack(int idTrack)
    {
        // ARRANGE & ACT & ASSERT
        Assert.Inconclusive(
            $"Cannot verify that LoadAndDisplayTrack({idTrack}) is called due to:\n" +
            "- LoadAndDisplayTrack is async void (cannot await or verify completion)\n" +
            "- Method accesses bl.GetOneTrack(idTrack) which requires database\n" +
            "- Method calls WaitForMapReady() which waits for WebView initialization\n" +
            "- Method uses MainThread.InvokeOnMainThreadAsync which requires MAUI dispatcher\n" +
            "- Method calls UpdateStatus, DisplayAlert, UpdateStatistics (all UI operations)\n" +
            "\n" +
            "Expected behavior:\n" +
            $"- LoadAndDisplayTrack should be called with idTrack={idTrack}\n" +
            "- Execution continues asynchronously after constructor completes\n" +
            "- Any exception in LoadAndDisplayTrack is caught by the try-catch in constructor\n" +
            "\n" +
            "Problem: async void methods cannot be properly tested or awaited.\n" +
            "Recommendation: Change LoadAndDisplayTrack to return Task and call it from OnAppearing."
        );
    }

    /// <summary>
    /// Tests that OnAppearing calls base.OnAppearing, EnsureEventSubscription, and WaitForMapReady
    /// in view-only mode and then calls SetViewOnlyMode.
    /// 
    /// Note: This test cannot be fully implemented as a unit test because:
    /// 1. TrackPage constructor calls InitializeComponent() which requires XAML compilation
    /// 2. Constructor accesses Application.Current.Handler.MauiContext which doesn't exist in unit tests
    /// 3. UI controls (buttons, labels, mapWebView) are initialized from XAML
    /// 4. Cannot mock ContentPage.OnAppearing() base method
    /// 
    /// Recommended approach: Integration test using MAUI test host or manual testing.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure and XAML-initialized controls. Requires integration testing.")]
    public async Task OnAppearing_ViewOnlyMode_CallsSetViewOnlyMode()
    {
        // This test requires:
        // 1. MAUI application context with initialized handler
        // 2. XAML compilation for UI controls
        // 3. Ability to override protected methods or mock ContentPage behavior
        // 
        // To test this scenario in an integration test:
        // - Create TrackPage with idTrack parameter to enable view-only mode
        // - Trigger OnAppearing lifecycle event
        // - Verify that all tracking buttons are disabled
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing calls CheckAndRequestLocationPermission when not in view-only mode
    /// and backgroundGpsService is null.
    /// 
    /// Note: This test cannot be fully implemented as a unit test because:
    /// 1. TrackPage constructor requires MAUI infrastructure
    /// 2. CheckAndRequestLocationPermission uses static Permissions API
    /// 3. Cannot inject backgroundGpsService without modifying constructor
    /// 
    /// Recommended approach: Integration test or refactor to use dependency injection.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure and uses static Permissions API. Requires integration testing.")]
    public async Task OnAppearing_NotViewOnlyMode_BackgroundServiceNull_CallsCheckAndRequestLocationPermission()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. Mocked or testable Permissions API
        // 3. Ability to inject null backgroundGpsService
        // 
        // To test this scenario:
        // - Refactor to inject IBackgroundGpsService via constructor or property
        // - Mock Permissions API or use integration test
        // - Verify CheckAndRequestLocationPermission is called
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure and refactoring for testability.");
    }

    /// <summary>
    /// Tests that OnAppearing shows recovery dialog when backgroundGpsService is tracking,
    /// hasShownRecoveryDialog is false, isTracking is false, and existingPositions > 0.
    /// 
    /// Note: This test cannot be fully implemented as a unit test because:
    /// 1. TrackPage constructor requires MAUI infrastructure
    /// 2. ShowRecoveryDialog uses DisplayActionSheet which requires UI context
    /// 3. Cannot mock private fields (isTracking, hasShownRecoveryDialog)
    /// 
    /// Recommended approach: Integration test or refactor to improve testability.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure and cannot mock private field state. Requires integration testing.")]
    public async Task OnAppearing_RecoveryScenario_ShowsRecoveryDialog()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. Ability to set private field values (isTracking=false, hasShownRecoveryDialog=false)
        // 3. Mock IBackgroundGpsService with IsTracking=true, GetPositionsCount()>0
        // 4. Mock DisplayActionSheet behavior
        // 
        // To test this scenario:
        // - Create testable subclass that exposes field setters
        // - Mock IBackgroundGpsService appropriately
        // - Verify ShowRecoveryDialog is called with correct position count
        // - Verify hasShownRecoveryDialog is set to true
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure and enhanced testability design.");
    }

    /// <summary>
    /// Tests that OnAppearing resumes tracking without dialog when service is tracking,
    /// isTracking is false, but hasShownRecoveryDialog is true (to prevent duplicate dialogs).
    /// 
    /// Note: This test cannot be fully implemented as a unit test due to infrastructure dependencies.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure and dependency injection refactoring. Requires integration testing.")]
    public async Task OnAppearing_ServiceTrackingButDialogAlreadyShown_ResumesWithoutDialog()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. Ability to set hasShownRecoveryDialog=true before OnAppearing
        // 3. Mock IBackgroundGpsService with IsTracking=true
        // 4. Verify SyncAndDisplayPositionsFromBackgroundService is called
        // 5. Verify buttons are updated correctly
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing initializes new track when service is tracking,
    /// isTracking is false, and bl.CurrentTrack is null.
    /// 
    /// Note: This test cannot be fully implemented because bl is a concrete field
    /// initialized in constructor and cannot be mocked.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure and BL_GpsTracking cannot be mocked. Requires integration testing.")]
    public async Task OnAppearing_ServiceTrackingWithNullCurrentTrack_InitializesNewTrack()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. Ability to inject or mock BL_GpsTracking
        // 3. Mock IBackgroundGpsService with IsTracking=true
        // 4. Verify bl.StartNewTrack() is called
        // 5. Verify SyncAndDisplayPositionsFromBackgroundService is called
        Assert.Inconclusive("This test requires dependency injection for BL_GpsTracking and MAUI infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing sets tracking start time from service or current time
    /// when resuming tracking.
    /// 
    /// Note: This test cannot be fully implemented due to infrastructure dependencies.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure. Requires integration testing.")]
    public async Task OnAppearing_ResumingTracking_SetsTrackingStartTime()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. Mock IBackgroundGpsService with TrackingStartTime set
        // 3. Verify trackingStartTime field is set correctly
        // 4. Test both cases: service has TrackingStartTime and when it's null
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing updates button states correctly when resuming tracking.
    /// Expected: Start disabled, Stop enabled, Save disabled, Clear disabled.
    /// 
    /// Note: This test cannot be fully implemented because buttons are XAML-initialized controls.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires XAML-initialized controls. Requires integration testing.")]
    public async Task OnAppearing_ResumingTracking_UpdatesButtonStates()
    {
        // This test requires:
        // 1. MAUI application context with XAML compilation
        // 2. Access to button controls (btnStartTracking, btnStopTracking, etc.)
        // 3. Mock IBackgroundGpsService appropriately
        // 4. Verify IsEnabled properties are set correctly
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure with XAML support.");
    }

    /// <summary>
    /// Tests that OnAppearing calls UpdateStatistics when resuming or refreshing tracking.
    /// 
    /// Note: This test cannot be fully implemented because UpdateStatistics accesses
    /// XAML-initialized label controls and BL_GpsTracking instance.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires XAML-initialized controls and BL_GpsTracking. Requires integration testing.")]
    public async Task OnAppearing_ResumingOrRefreshing_CallsUpdateStatistics()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. XAML-compiled labels for statistics display
        // 3. Mock or stub BL_GpsTracking for statistics calculation
        // 4. Verify UpdateStatistics is called in appropriate scenarios
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing just refreshes positions when already tracking
    /// (page reappearing from background).
    /// 
    /// Note: This test cannot be fully implemented due to infrastructure dependencies.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure. Requires integration testing.")]
    public async Task OnAppearing_AlreadyTracking_RefreshesPositions()
    {
        // This test requires:
        // 1. MAUI application context
        // 2. Ability to set isTracking=true before OnAppearing
        // 3. Mock IBackgroundGpsService with IsTracking=true
        // 4. Verify SyncAndDisplayPositionsFromBackgroundService is called
        // 5. Verify button states are NOT changed
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing handles null backgroundGpsService gracefully
    /// when not in view-only mode.
    /// 
    /// Note: Partial implementation showing expected behavior.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure. Requires integration testing.")]
    public async Task OnAppearing_NullBackgroundService_NoException()
    {
        // Expected behavior:
        // - CheckAndRequestLocationPermission is called
        // - No exception is thrown due to null service
        // - backgroundGpsService null check prevents service access
        // 
        // This test requires MAUI application context to instantiate TrackPage
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that OnAppearing handles edge case where GetPositionsCount returns 0
    /// even though IsTracking is true.
    /// 
    /// Note: This test documents expected behavior for integration testing.
    /// </summary>
    [Test]
    [Ignore("TrackPage requires MAUI infrastructure. Requires integration testing.")]
    public async Task OnAppearing_ServiceTrackingWithZeroPositions_NoRecoveryDialog()
    {
        // Expected behavior:
        // - Service IsTracking=true, GetPositionsCount()=0
        // - Recovery dialog should NOT be shown (existingPositions > 0 check fails)
        // - Should fall through to "else if (!isTracking)" branch
        // - Should resume tracking without dialog
        // 
        // This test requires:
        // 1. Mock IBackgroundGpsService with IsTracking=true, GetPositionsCount()=0
        // 2. Verify ShowRecoveryDialog is NOT called
        // 3. Verify tracking resumes normally
        Assert.Inconclusive("This test requires MAUI integration testing infrastructure.");
    }

    /// <summary>
    /// Tests that the parameterless constructor initializes the page correctly.
    /// </summary>
    /// <remarks>
    /// This test is ignored because TrackPage inherits from ContentPage and requires:
    /// 1. MAUI framework infrastructure to be initialized
    /// 2. InitializeComponent() to successfully parse and load XAML
    /// 3. Application.Current to be set with a valid Handler and MauiContext
    /// 4. UI controls (lblCurrentPosition, btnStartTracking, etc.) to be initialized
    /// 
    /// Without a full MAUI host environment, instantiating TrackPage will throw exceptions.
    /// 
    /// To test this code:
    /// - Use integration tests with MAUI TestHost
    /// - Refactor constructor to accept IBackgroundGpsService as a parameter
    /// - Move initialization logic to a separate testable method
    /// </remarks>
    [Test]
    [Ignore("TrackPage constructor requires MAUI framework infrastructure and cannot be unit tested. " +
            "InitializeComponent() requires XAML compilation, and Application.Current.Handler.MauiContext.Services " +
            "requires a full MAUI application host. Consider refactoring to accept dependencies via constructor " +
            "parameters and moving initialization logic to a separate method that can be tested in isolation.")]
    public void Constructor_WithNoParameters_InitializesPageWithDefaultState()
    {
        // Arrange
        // Cannot arrange - would require:
        // 1. Mocking Application.Current (static property, cannot mock)
        // 2. Setting up Handler.MauiContext.Services (requires MAUI infrastructure)
        // 3. Ensuring InitializeComponent() succeeds (requires XAML infrastructure)
        // 4. UI controls being initialized (requires successful XAML load)

        // Act
        // var page = new TrackPage(); // This will throw NullReferenceException or similar

        // Assert
        // Cannot assert on page state because page cannot be created

        Assert.Inconclusive("This test cannot be executed without MAUI framework infrastructure. " +
                           "Refactor the constructor to accept dependencies as parameters.");
    }

    /// <summary>
    /// Tests that constructor handles null IBackgroundGpsService correctly.
    /// </summary>
    /// <remarks>
    /// This test is ignored because it requires:
    /// 1. Ability to control what Application.Current.Handler.MauiContext.Services.GetService returns
    /// 2. Successfully calling InitializeComponent()
    /// 3. Accessing UI controls (lblCurrentPosition, buttons)
    /// 
    /// The code expects to:
    /// - Log an error via General.LogOfProgram?.Error()
    /// - Call UpdateStatus() which updates lblStatus
    /// - Not subscribe to OnPositionRecorded event
    /// 
    /// To test this scenario, refactor to inject IBackgroundGpsService via constructor parameter.
    /// </remarks>
    [Test]
    [Ignore("Cannot test null backgroundGpsService scenario without ability to mock Application.Current " +
            "and MAUI infrastructure. Refactor to inject IBackgroundGpsService via constructor.")]
    public void Constructor_WhenBackgroundGpsServiceIsNull_LogsErrorAndUpdatesStatus()
    {
        // This test would verify:
        // 1. General.LogOfProgram?.Error() is called with appropriate message
        // 2. UpdateStatus() is called with error message and Red color
        // 3. OnPositionRecorded event is not subscribed to

        Assert.Inconclusive("Cannot test without dependency injection refactoring.");
    }

    /// <summary>
    /// Tests that constructor subscribes to OnPositionRecorded when service is available and not tracking.
    /// </summary>
    /// <remarks>
    /// This test is ignored because it requires full MAUI infrastructure.
    /// 
    /// The expected behavior when backgroundGpsService is not null and IsTracking is false:
    /// - Subscribe to OnPositionRecorded event
    /// - Set lblCurrentPosition.Text to AppStrings.TrackWaitingForGPS
    /// - Set isTracking to false
    /// - Enable btnStartTracking, disable all other buttons
    /// - Call UpdateStatus() with "Ready" message and Gray color
    /// - Call InitializeMap()
    /// 
    /// To test: Inject IBackgroundGpsService and extract initialization to testable method.
    /// </remarks>
    [Test]
    [Ignore("Cannot test event subscription and UI initialization without MAUI infrastructure. " +
            "Refactor to enable dependency injection and separate initialization logic.")]
    public void Constructor_WhenServiceAvailableAndNotTracking_InitializesWithReadyState()
    {
        Assert.Inconclusive("Requires constructor refactoring to accept IBackgroundGpsService parameter.");
    }

    /// <summary>
    /// Tests that constructor handles service already tracking scenario.
    /// </summary>
    /// <remarks>
    /// This test is ignored because it requires MAUI infrastructure.
    /// 
    /// Expected behavior when backgroundGpsService.IsTracking is true:
    /// - Subscribe to OnPositionRecorded event
    /// - Set lblCurrentPosition.Text to AppStrings.TrackWaitingForGPS
    /// - Disable all buttons (Start, Stop, Save, Clear)
    /// - Call UpdateStatus() with "Checking" message and Orange color
    /// - Call InitializeMap()
    /// 
    /// To test: Refactor to inject IBackgroundGpsService with configurable IsTracking property.
    /// </remarks>
    [Test]
    [Ignore("Cannot test tracking state initialization without MAUI infrastructure and dependency injection.")]
    public void Constructor_WhenServiceAlreadyTracking_InitializesWithCheckingState()
    {
        Assert.Inconclusive("Requires constructor refactoring for testability.");
    }

    /// <summary>
    /// Tests that constructor handles exceptions during initialization.
    /// </summary>
    /// <remarks>
    /// This test is ignored because it requires MAUI infrastructure.
    /// 
    /// Expected behavior when exception occurs in try-catch block:
    /// - Exception is caught
    /// - General.LogOfProgram?.Error() is called with exception
    /// - InitializeMap() is still called after catch block
    /// 
    /// To test: Refactor to separate concerns and enable exception testing.
    /// </remarks>
    [Test]
    [Ignore("Cannot test exception handling without MAUI infrastructure. Refactor initialization logic.")]
    public void Constructor_WhenExceptionOccursDuringInitialization_LogsErrorAndContinues()
    {
        Assert.Inconclusive("Exception handling cannot be tested without refactoring.");
    }
}