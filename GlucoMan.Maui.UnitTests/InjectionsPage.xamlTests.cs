using System;

using gamon;
using GlucoMan;
using GlucoMan.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;





/// <summary>
/// Unit tests for the InjectionsPage class.
/// </summary>
/// <remarks>
/// IMPORTANT: This test class contains partial/incomplete tests marked as [Ignore].
/// The InjectionsPage constructor is currently NOT testable in isolation due to:
/// 
/// 1. MAUI Framework Dependency: The class inherits from ContentPage and calls InitializeComponent(),
///    which requires the full MAUI framework to be initialized. This cannot run in standard unit tests.
/// 
/// 2. Static Dependencies: The constructor directly accesses:
///    - Common.Database (static field)
///    - General.LogOfProgram (static field)
///    These cannot be mocked with Moq and violate dependency injection principles.
/// 
/// 3. XAML-Initialized UI Elements: The constructor accesses rdbShortInsulin and rdbLongInsulin,
///    which are initialized by InitializeComponent() from XAML and will be null in unit tests.
/// 
/// 4. Instance Method Calls: Calls to RefreshUi(), SaveOriginalInjection(), and AttachChangeHandlers()
///    also depend on XAML-initialized controls.
/// 
/// TO MAKE THIS CODE TESTABLE:
/// - Refactor to accept DataLayer and Logger as constructor parameters (dependency injection)
/// - Extract initialization logic from constructor into a separate, testable method
/// - Consider using MAUI integration tests instead of unit tests for page constructors
/// - Separate business logic from UI initialization
/// 
/// The tests below document the edge cases and scenarios that SHOULD be tested once the code is refactored.
/// </remarks>
public partial class InjectionsPageTests
{
    /// <summary>
    /// Tests that the constructor completes successfully when all dependencies are available and valid.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored because:
    /// - InitializeComponent() requires MAUI framework initialization
    /// - Common.Database is a static dependency that cannot be mocked
    /// - UI elements (rdbShortInsulin, rdbLongInsulin) are XAML-initialized
    /// 
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should be set from parameters
    /// - IdCurrentShortActingInsulin and IdCurrentLongActingInsulin should be set from parameters
    /// - Radio button content should be set to insulin drug names
    /// - RefreshUi(), SaveOriginalInjection(), and AttachChangeHandlers() should be called
    /// - pageIsLoading should be false after initialization
    /// 
    /// To implement: Refactor constructor to accept IDataLayer and ILogger via dependency injection.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithValidIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = 123;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Assert.That(page.pageIsLoading, Is.False);
        // Verify radio button content is set correctly
        // Verify insulin IDs are set correctly
    }

    /// <summary>
    /// Tests that the constructor handles null IdInjection parameter correctly.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - Constructor should complete successfully even with null IdInjection
    /// - All initialization steps should proceed normally
    /// - No exception should be thrown
    /// 
    /// To implement: Refactor constructor to be testable with dependency injection.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithNullIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = null;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Verify initialization completes without errors
    }

    /// <summary>
    /// Tests that the constructor handles the case when GetParameters() returns null.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior when parameters is null:
    /// - MonthsOfDataShownInTheGrids should retain its default value (3)
    /// - IdCurrentShortActingInsulin should remain null
    /// - IdCurrentLongActingInsulin should remain null
    /// - Radio buttons should display default text "Short act." and "Long act."
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer to return null from GetParameters().
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenParametersIsNull_UsesDefaultValues()
    {
        // Arrange
        // Mock Common.Database.GetParameters() to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3));
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.MonthsOfDataShownInTheGrids is null.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should retain its initial value (3)
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer.GetParameters() to return Parameters with null MonthsOfDataShownInTheGrids.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNull_UsesDefaultValue()
    {
        // Arrange
        // Mock parameters with MonthsOfDataShownInTheGrids = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.MonthsOfDataShownInTheGrids is zero or negative.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should retain its initial value (3) since 0 does not satisfy the > 0 condition
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids <= 0.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsZeroOrNegative_UsesDefaultValue()
    {
        // Arrange
        // Mock parameters with MonthsOfDataShownInTheGrids = 0 or -1

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.IdInsulinDrug_Short is null.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - rdbShortInsulin.Content should be set to "Short act."
    /// - CurrentInjection.IdInsulinDrug should not be set
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer.GetParameters() to return Parameters with null IdInsulinDrug_Short.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinIdIsNull_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock parameters with IdInsulinDrug_Short = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when GetOneInsulinDrug returns null for short insulin.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - rdbShortInsulin.Content should be set to "Short act."
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return null.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetOneInsulinDrugReturnsNullForShort_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock parameters with valid IdInsulinDrug_Short
        // Mock bl.GetOneInsulinDrug() to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when InsulinDrug.Name is null.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - rdbShortInsulin.Content should be set to "Short act." (null coalescing)
    /// - CurrentInjection.IdInsulinDrug should be set to the insulin ID
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return InsulinDrug with null Name.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinNameIsNull_UsesDefaultText()
    {
        // Arrange
        // Mock parameters with valid IdInsulinDrug_Short
        // Mock bl.GetOneInsulinDrug() to return InsulinDrug with Name = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.IdInsulinDrug_Long is null.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - rdbLongInsulin.Content should be set to "Long act."
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer.GetParameters() to return Parameters with null IdInsulinDrug_Long.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenLongInsulinIdIsNull_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock parameters with IdInsulinDrug_Long = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when GetOneInsulinDrug returns null for long insulin.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - rdbLongInsulin.Content should be set to "Long act."
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return null.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetOneInsulinDrugReturnsNullForLong_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock parameters with valid IdInsulinDrug_Long
        // Mock bl.GetOneInsulinDrug() to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor logs an error and displays an alert when an exception occurs during initialization.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior when an exception occurs:
    /// - Exception should be caught and logged via General.LogOfProgram.Error()
    /// - An alert should be displayed via MainThread.BeginInvokeOnMainThread() and DisplayAlert()
    /// - The application should not crash
    /// 
    /// To implement:
    /// - Mock IDataLayer.GetParameters() to throw an exception
    /// - Mock ILogger to verify Error() is called with correct parameters
    /// - Verify DisplayAlert is called (requires mocking the page or extracting alert logic)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenExceptionOccurs_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Mock Common.Database.GetParameters() to throw exception

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify General.LogOfProgram.Error() was called with "InjectionsPage | ctor" and exception
        // Verify DisplayAlert was invoked with error message
        // Verify app did not crash
    }

    /// <summary>
    /// Tests that the constructor handles exception in DisplayAlert gracefully.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - If DisplayAlert throws an exception, it should be swallowed (inner catch block)
    /// - The application should not crash
    /// 
    /// To implement:
    /// - Mock Common.Database.GetParameters() to throw an exception
    /// - Mock DisplayAlert to throw an exception
    /// - Verify no exception propagates from constructor
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenDisplayAlertThrowsException_SwallowsExceptionAndDoesNotCrash()
    {
        // Arrange
        // Mock Common.Database.GetParameters() to throw exception
        // Mock DisplayAlert to throw exception

        // Act & Assert
        // Verify no exception is thrown
        // var page = new InjectionsPage(null);
    }

    /// <summary>
    /// Tests that the constructor sets CurrentInjection.IdInsulinDrug when short insulin is available.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - When IdCurrentShortActingInsulin is not null and currentShortInsulin is not null
    /// - CurrentInjection.IdInsulinDrug should be set to IdCurrentShortActingInsulin
    /// 
    /// To implement: Mock dependencies to provide valid insulin data.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinAvailable_SetsCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Mock parameters with valid IdInsulinDrug_Short
        // Mock bl.GetOneInsulinDrug() to return valid InsulinDrug

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.EqualTo(expectedId));
    }

    /// <summary>
    /// Tests that the constructor sets pageIsLoading correctly during initialization.
    /// </summary>
    /// <remarks>
    /// This test is currently ignored for the same reasons as above.
    /// 
    /// Expected behavior:
    /// - pageIsLoading should be set to true at line 41
    /// - pageIsLoading should be set to false at line 68
    /// - After initialization completes, pageIsLoading should be false
    /// 
    /// To implement: Refactor to make pageIsLoading testable or extract initialization logic.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_DuringInitialization_SetsPageIsLoadingCorrectly()
    {
        // Arrange & Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.pageIsLoading, Is.False);
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when there are no unsaved changes,
    /// allowing the default back navigation behavior.
    /// </summary>
    [Test]
    public void OnBackButtonPressed_NoUnsavedChanges_ReturnsFalse()
    {
        // NOTE: This test requires a fully initialized InjectionsPage which depends on:
        // - Database initialization (Common.Database.GetParameters())
        // - BL_BolusesAndInjections business logic layer
        // - MAUI UI components (ContentPage, controls)
        // These dependencies cannot be mocked due to the concrete implementation in the constructor.
        // 
        // To properly test this method, you would need to:
        // 1. Set up a test database or mock the database layer
        // 2. Initialize the MAUI application context
        // 3. Ensure all UI components are properly initialized
        // 
        // The method being tested (OnBackButtonPressed) delegates to HandleBackNavigation(),
        // which checks the private fields HasUnsavedChanges and IsNavigatingAway.
        // When both are false, it should return false to allow normal back navigation.

        Assert.Inconclusive("This test requires full MAUI application and database infrastructure. " +
                          "The InjectionsPage constructor depends on database initialization and MAUI UI components " +
                          "that cannot be properly mocked in a unit test environment. " +
                          "Consider creating integration tests with a test database and MAUI test harness.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns true when there are unsaved changes
    /// and the user is not already navigating away, preventing default back navigation.
    /// </summary>
    [Test]
    public void OnBackButtonPressed_HasUnsavedChangesAndNotNavigatingAway_ReturnsTrue()
    {
        // NOTE: This test requires a fully initialized InjectionsPage which depends on:
        // - Database initialization (Common.Database.GetParameters())
        // - BL_BolusesAndInjections business logic layer
        // - MAUI UI components (ContentPage, controls)
        // These dependencies cannot be mocked due to the concrete implementation in the constructor.
        // 
        // To properly test this method, you would need to:
        // 1. Set up a test database or mock the database layer
        // 2. Initialize the MAUI application context
        // 3. Ensure all UI components are properly initialized
        // 4. Trigger changes in the UI to set HasUnsavedChanges = true
        // 
        // The method being tested (OnBackButtonPressed) delegates to HandleBackNavigation(),
        // which checks the private fields HasUnsavedChanges and IsNavigatingAway.
        // When HasUnsavedChanges is true and IsNavigatingAway is false, it should return true
        // to prevent back navigation and show the unsaved changes dialog.

        Assert.Inconclusive("This test requires full MAUI application and database infrastructure. " +
                          "The InjectionsPage constructor depends on database initialization and MAUI UI components " +
                          "that cannot be properly mocked in a unit test environment. " +
                          "To test the unsaved changes behavior, you would need to simulate UI interactions " +
                          "that set the HasUnsavedChanges field to true. " +
                          "Consider creating integration tests with a test database and MAUI test harness.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when the user is already navigating away,
    /// even if there are unsaved changes, allowing the navigation to proceed.
    /// </summary>
    [Test]
    public void OnBackButtonPressed_IsNavigatingAway_ReturnsFalse()
    {
        // NOTE: This test requires a fully initialized InjectionsPage which depends on:
        // - Database initialization (Common.Database.GetParameters())
        // - BL_BolusesAndInjections business logic layer
        // - MAUI UI components (ContentPage, controls)
        // These dependencies cannot be mocked due to the concrete implementation in the constructor.
        // 
        // To properly test this method, you would need to:
        // 1. Set up a test database or mock the database layer
        // 2. Initialize the MAUI application context
        // 3. Ensure all UI components are properly initialized
        // 4. Set the IsNavigatingAway field to true (this happens during navigation flow)
        // 
        // The method being tested (OnBackButtonPressed) delegates to HandleBackNavigation(),
        // which checks the private fields HasUnsavedChanges and IsNavigatingAway.
        // When IsNavigatingAway is true, regardless of HasUnsavedChanges state, it should return false
        // to allow the navigation that's already in progress.

        Assert.Inconclusive("This test requires full MAUI application and database infrastructure. " +
                          "The InjectionsPage constructor depends on database initialization and MAUI UI components " +
                          "that cannot be properly mocked in a unit test environment. " +
                          "To test the navigation flow, you would need to simulate the navigation process " +
                          "that sets the IsNavigatingAway field to true. " +
                          "Consider creating integration tests with a test database and MAUI test harness.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when HasUnsavedChanges is true
    /// but IsNavigatingAway is also true, giving precedence to the navigation in progress.
    /// </summary>
    [Test]
    public void OnBackButtonPressed_HasUnsavedChangesButIsNavigatingAway_ReturnsFalse()
    {
        // NOTE: This test requires a fully initialized InjectionsPage which depends on:
        // - Database initialization (Common.Database.GetParameters())
        // - BL_BolusesAndInjections business logic layer
        // - MAUI UI components (ContentPage, controls)
        // These dependencies cannot be mocked due to the concrete implementation in the constructor.
        // 
        // To properly test this method, you would need to:
        // 1. Set up a test database or mock the database layer
        // 2. Initialize the MAUI application context
        // 3. Ensure all UI components are properly initialized
        // 4. Trigger changes in the UI to set HasUnsavedChanges = true
        // 5. Set IsNavigatingAway = true to simulate navigation in progress
        // 
        // The method being tested (OnBackButtonPressed) delegates to HandleBackNavigation(),
        // which checks both HasUnsavedChanges AND !IsNavigatingAway.
        // Even with unsaved changes, if navigation is already in progress (IsNavigatingAway = true),
        // the method returns false to allow the navigation to complete.

        Assert.Inconclusive("This test requires full MAUI application and database infrastructure. " +
                          "The InjectionsPage constructor depends on database initialization and MAUI UI components " +
                          "that cannot be properly mocked in a unit test environment. " +
                          "This edge case requires setting both HasUnsavedChanges and IsNavigatingAway to true, " +
                          "which requires complex state manipulation during integration testing. " +
                          "Consider creating integration tests with a test database and MAUI test harness.");
    }

    /// <summary>
    /// Tests that IdInjection property returns null when CurrentInjection.IdInjection is null.
    /// </summary>
    /// <remarks>
    /// This test requires MAUI ContentPage initialization (InitializeComponent) which depends on:
    /// - XAML compilation and generated code
    /// - UI thread availability
    /// - Database initialization (Common.Database.GetParameters())
    /// - MAUI application host context
    /// 
    /// To make this test executable:
    /// 1. Set up a MAUI test host or headless MAUI environment
    /// 2. Initialize the database with test data
    /// 3. Ensure UI thread synchronization context is available
    /// 4. Mock or stub database dependencies if needed
    /// 
    /// Currently marked as Ignore due to these infrastructure requirements.
    /// </remarks>
    [Test]
    [Ignore("Requires MAUI application context and UI thread initialization. See test comments for details.")]
    public void IdInjection_WhenCurrentInjectionIdIsNull_ReturnsNull()
    {
        // Arrange
        // Note: The following instantiation will fail without MAUI infrastructure:
        // - InitializeComponent() requires XAML-generated code
        // - Constructor accesses database (Common.Database.GetParameters())
        // - Constructor initializes UI controls (rdbShortInsulin, rdbLongInsulin)

        // To properly test:
        // 1. Initialize MAUI test environment
        // 2. Set up test database or mock Common.Database
        // 3. Create InjectionsPage instance
        // 4. Ensure CurrentInjection.IdInjection is null (default for new Injection())

        // Act
        // var result = page.IdInjection;

        // Assert
        // Assert.That(result, Is.Null);

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure. " +
                          "InjectionsPage constructor calls InitializeComponent() which requires XAML compilation, " +
                          "UI thread context, and database availability. Consider refactoring to extract testable " +
                          "logic into a ViewModel or service layer.");
    }

    /// <summary>
    /// Tests that IdInjection property returns the correct value when CurrentInjection.IdInjection has a positive value.
    /// </summary>
    /// <remarks>
    /// This test requires MAUI ContentPage initialization. See IdInjection_WhenCurrentInjectionIdIsNull_ReturnsNull
    /// for detailed infrastructure requirements.
    /// </remarks>
    [Test]
    [Ignore("Requires MAUI application context and UI thread initialization. See test comments for details.")]
    public void IdInjection_WhenCurrentInjectionIdHasPositiveValue_ReturnsThatValue()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI environment and create InjectionsPage
        // 2. Set CurrentInjection.IdInjection = 42 (or use reflection to access private field)
        // 3. Verify page.IdInjection returns 42

        // Act
        // var result = page.IdInjection;

        // Assert
        // Assert.That(result, Is.EqualTo(42));

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure. " +
                          "Additionally, CurrentInjection is a private field with no public setter, " +
                          "requiring either reflection access or a public API to modify it.");
    }

    /// <summary>
    /// Tests that IdInjection property returns zero when CurrentInjection.IdInjection is zero.
    /// </summary>
    /// <remarks>
    /// This test requires MAUI ContentPage initialization. See IdInjection_WhenCurrentInjectionIdIsNull_ReturnsNull
    /// for detailed infrastructure requirements.
    /// </remarks>
    [Test]
    [Ignore("Requires MAUI application context and UI thread initialization. See test comments for details.")]
    public void IdInjection_WhenCurrentInjectionIdIsZero_ReturnsZero()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI environment and create InjectionsPage
        // 2. Set CurrentInjection.IdInjection = 0
        // 3. Verify page.IdInjection returns 0

        // Act
        // var result = page.IdInjection;

        // Assert
        // Assert.That(result, Is.EqualTo(0));

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure.");
    }

    /// <summary>
    /// Tests that IdInjection property returns negative value when CurrentInjection.IdInjection has a negative value.
    /// </summary>
    /// <remarks>
    /// This test verifies edge case behavior with negative IDs, which may or may not be valid
    /// depending on business rules. Tests boundary behavior of int? nullable integer.
    /// </remarks>
    [Test]
    [Ignore("Requires MAUI application context and UI thread initialization. See test comments for details.")]
    public void IdInjection_WhenCurrentInjectionIdIsNegative_ReturnsNegativeValue()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI environment and create InjectionsPage
        // 2. Set CurrentInjection.IdInjection = -1
        // 3. Verify page.IdInjection returns -1

        // Act
        // var result = page.IdInjection;

        // Assert
        // Assert.That(result, Is.EqualTo(-1));

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure.");
    }

    /// <summary>
    /// Tests that IdInjection property correctly returns int.MaxValue boundary value.
    /// </summary>
    /// <remarks>
    /// Tests boundary condition for maximum integer value.
    /// </remarks>
    [Test]
    [Ignore("Requires MAUI application context and UI thread initialization. See test comments for details.")]
    public void IdInjection_WhenCurrentInjectionIdIsIntMaxValue_ReturnsIntMaxValue()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI environment and create InjectionsPage
        // 2. Set CurrentInjection.IdInjection = int.MaxValue (2147483647)
        // 3. Verify page.IdInjection returns int.MaxValue

        // Act
        // var result = page.IdInjection;

        // Assert
        // Assert.That(result, Is.EqualTo(int.MaxValue));

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure.");
    }

    /// <summary>
    /// Tests that IdInjection property correctly returns int.MinValue boundary value.
    /// </summary>
    /// <remarks>
    /// Tests boundary condition for minimum integer value.
    /// </remarks>
    [Test]
    [Ignore("Requires MAUI application context and UI thread initialization. See test comments for details.")]
    public void IdInjection_WhenCurrentInjectionIdIsIntMinValue_ReturnsIntMinValue()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI environment and create InjectionsPage
        // 2. Set CurrentInjection.IdInjection = int.MinValue (-2147483648)
        // 3. Verify page.IdInjection returns int.MinValue

        // Act
        // var result = page.IdInjection;

        // Assert
        // Assert.That(result, Is.EqualTo(int.MinValue));

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure.");
    }
}




/// <summary>
/// Unit tests for the InjectionsPage constructor.
/// </summary>
/// <remarks>
/// IMPORTANT: All tests in this class are marked as [Ignore] because the InjectionsPage constructor
/// is NOT testable in isolation due to:
/// 
/// 1. MAUI Framework Dependency: Inherits from ContentPage and calls InitializeComponent(),
///    which requires full MAUI framework initialization.
/// 
/// 2. Static Dependencies: Direct access to Common.Database and General.LogOfProgram
///    which cannot be mocked with Moq.
/// 
/// 3. XAML-Initialized UI Elements: rdbShortInsulin and rdbLongInsulin are initialized
///    by InitializeComponent() and will be null in unit tests.
/// 
/// 4. Instance Method Calls: RefreshUi(), SaveOriginalInjection(), and AttachChangeHandlers()
///    depend on XAML-initialized controls.
/// 
/// TO MAKE THIS CODE TESTABLE:
/// - Inject IDataLayer and ILogger via constructor parameters
/// - Extract initialization logic into a separate, testable method
/// - Use MAUI integration tests for page constructors
/// - Separate business logic from UI initialization
/// 
/// These tests document the edge cases that SHOULD be tested after refactoring.
/// </remarks>
public partial class InjectionsPageConstructorTests
{
    /// <summary>
    /// Tests that the constructor completes successfully with a valid positive IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// This test is ignored due to MAUI infrastructure requirements.
    /// 
    /// Expected behavior:
    /// - Constructor should complete without throwing
    /// - MonthsOfDataShownInTheGrids should be set from parameters
    /// - Insulin IDs should be retrieved from parameters
    /// - Radio button content should be set appropriately
    /// - pageIsLoading should be false after initialization
    /// 
    /// NOTE: The IdInjection parameter is currently UNUSED in the constructor body.
    /// This may be a bug or incomplete implementation.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithPositiveIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = 123;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Verify initialization completes successfully
    }

    /// <summary>
    /// Tests that the constructor handles int.MaxValue for IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// This test verifies boundary value handling for the IdInjection parameter.
    /// Although the parameter is currently unused in the constructor, this documents
    /// expected behavior if it's used in future implementations.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithMaxValueIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = int.MaxValue;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles int.MinValue for IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// This test verifies boundary value handling for negative IdInjection values.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithMinValueIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = int.MinValue;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles zero for IdInjection parameter.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithZeroIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = 0;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles negative IdInjection parameter.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithNegativeIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = -1;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with very large positive value.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should be updated to the large value since it's > 0
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsVeryLarge_UpdatesValue()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids = 10000.0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(10000.0));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with small positive value.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should be updated to 0.1 since it's > 0
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsSmallPositive_UpdatesValue()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids = 0.1

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(0.1));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with double.PositiveInfinity.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should be updated to PositiveInfinity since infinity > 0
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsPositiveInfinity_UpdatesValue()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids = double.PositiveInfinity

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(double.PositiveInfinity));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with double.NegativeInfinity.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should retain default value (3.0) since NegativeInfinity is not > 0
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNegativeInfinity_UsesDefaultValue()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids = double.NegativeInfinity

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with double.NaN.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should retain default value (3.0) since NaN is not > 0
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNaN_UsesDefaultValue()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids = double.NaN

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles the case when both insulin IDs are null.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Both radio buttons should display default text "Short act." and "Long act."
    /// - CurrentInjection.IdInsulinDrug should not be set
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenBothInsulinIdsAreNull_SetsDefaultRadioButtonTexts()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with null IdInsulinDrug_Short and IdInsulinDrug_Long

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when both GetOneInsulinDrug calls return null.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Both radio buttons should display default text
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenBothGetOneInsulinDrugReturnsNull_SetsDefaultRadioButtonTexts()
    {
        // Arrange
        // Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return null for both calls

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles InsulinDrug.Name being an empty string.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Radio button should display the empty string (null-coalescing doesn't trigger for empty string)
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinNameIsEmptyString_UsesEmptyString()
    {
        // Arrange
        // Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return InsulinDrug with Name = ""

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the constructor handles InsulinDrug.Name being whitespace only.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Radio button should display the whitespace string
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinNameIsWhitespace_UsesWhitespace()
    {
        // Arrange
        // Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return InsulinDrug with Name = "   "

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("   "));
    }

    /// <summary>
    /// Tests that the constructor handles InsulinDrug.Name being very long string.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Radio button should display the full long string
    /// - No exception or truncation should occur
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinNameIsVeryLong_UsesFullString()
    {
        // Arrange
        // string longName = new string('A', 10000);
        // Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return InsulinDrug with Name = longName

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(longName));
    }

    /// <summary>
    /// Tests that the constructor handles InsulinDrug.Name with special characters.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Radio button should display the string with special characters
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinNameHasSpecialCharacters_UsesFullString()
    {
        // Arrange
        // string specialName = "Insulin<>&\"'\t\n\r";
        // Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to return InsulinDrug with Name = specialName

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(specialName));
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being zero.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - GetOneInsulinDrug should be called with 0
    /// - Behavior depends on business logic for ID = 0
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreZero_CallsGetOneInsulinDrugWithZero()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with IdInsulinDrug_Short = 0, IdInsulinDrug_Long = 0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug was called with 0
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being negative.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - GetOneInsulinDrug should be called with negative values
    /// - Behavior depends on business logic for negative IDs
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreNegative_CallsGetOneInsulinDrugWithNegative()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with IdInsulinDrug_Short = -1, IdInsulinDrug_Long = -1

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug was called with -1
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being int.MaxValue.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreMaxValue_CallsGetOneInsulinDrugWithMaxValue()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return Parameters with IdInsulinDrug_Short = int.MaxValue, IdInsulinDrug_Long = int.MaxValue

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug was called with int.MaxValue
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by GetParameters.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Exception should be caught
    /// - Error should be logged via General.LogOfProgram.Error()
    /// - Alert should be displayed
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetParametersThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to throw new InvalidOperationException("Database error")

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify General.LogOfProgram.Error was called with "InjectionsPage | ctor" and the exception
        // Verify DisplayAlert was called
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by GetOneInsulinDrug.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Exception should be caught
    /// - Error should be logged
    /// - Alert should be displayed
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetOneInsulinDrugThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Mock IBL_BolusesAndInjections.GetOneInsulinDrug() to throw new InvalidOperationException("Drug lookup failed")

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error logging and alert display
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by RefreshUi.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Exception should be caught
    /// - Error should be logged
    /// - Alert should be displayed
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenRefreshUiThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Cause RefreshUi to throw exception (may require specific UI state)

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error handling
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by SaveOriginalInjection.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenSaveOriginalInjectionThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Cause SaveOriginalInjection to throw exception

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error handling
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by AttachChangeHandlers.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenAttachChangeHandlersThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Cause AttachChangeHandlers to throw exception

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error handling
    }

    /// <summary>
    /// Tests that the constructor handles exception in MainThread.BeginInvokeOnMainThread gracefully.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - Exception from BeginInvokeOnMainThread or DisplayAlert should be swallowed
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMainThreadBeginInvokeThrowsException_SwallowsException()
    {
        // Arrange
        // Mock Common.Database.GetParameters() to throw
        // Mock MainThread.BeginInvokeOnMainThread or DisplayAlert to throw

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify no exception propagates from constructor
    }

    /// <summary>
    /// Tests that the constructor sets pageIsLoading to true then false during initialization.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - pageIsLoading should be true during initialization (line 41)
    /// - pageIsLoading should be false after initialization completes (line 68)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_DuringNormalInitialization_SetsPageIsLoadingTrueThenFalse()
    {
        // Arrange & Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify pageIsLoading is false after constructor completes
    }

    /// <summary>
    /// Tests that the constructor calls RefreshUi, SaveOriginalInjection, and AttachChangeHandlers in order.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - RefreshUi() should be called first (line 69)
    /// - SaveOriginalInjection() should be called second (line 71)
    /// - AttachChangeHandlers() should be called third (line 73)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInitializing_CallsMethodsInCorrectOrder()
    {
        // Arrange & Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify method call sequence
        // This would require making these methods virtual and using a spy/mock
    }

    /// <summary>
    /// Tests that the constructor properly initializes CurrentInjection.IdInsulinDrug when short insulin is available.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - When both IdCurrentShortActingInsulin and currentShortInsulin are not null
    /// - CurrentInjection.IdInsulinDrug should be set to IdCurrentShortActingInsulin
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinAvailableAndValid_SetsCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Mock to return valid short insulin with ID = 5

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.EqualTo(5));
    }

    /// <summary>
    /// Tests that the constructor does NOT set CurrentInjection.IdInsulinDrug when short insulin is null.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinIsNull_DoesNotSetCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Mock to return null for short insulin

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify CurrentInjection.IdInsulinDrug was not modified
    }

    /// <summary>
    /// Tests that the constructor does NOT set CurrentInjection.IdInsulinDrug when short insulin ID is null.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinIdIsNull_DoesNotSetCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Mock to return null for IdInsulinDrug_Short

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify CurrentInjection.IdInsulinDrug was not set
    }
}




/// <summary>
/// Unit tests for the OnBackButtonPressed method of InjectionsPage.
/// </summary>
/// <remarks>
/// CRITICAL LIMITATION: The OnBackButtonPressed method CANNOT be tested in isolation due to:
/// 
/// 1. Protected Method Access: OnBackButtonPressed is a protected override method inherited from
///    ContentPage. It cannot be called directly from test code without reflection (which is prohibited).
/// 
/// 2. MAUI Framework Dependency: The InjectionsPage class inherits from ContentPage and calls
///    InitializeComponent() in the constructor, which requires the full MAUI framework to be initialized.
///    This cannot run in standard unit tests without a MAUI test harness.
/// 
/// 3. Private Field Dependencies: The method delegates to HandleBackNavigation(), which checks two
///    private fields (HasUnsavedChanges and IsNavigatingAway). These fields cannot be set from test
///    code without reflection or changing the class design to make them internal/protected.
/// 
/// 4. Static Dependencies: The constructor accesses Common.Database (static field) and
///    General.LogOfProgram (static field), which cannot be mocked with Moq.
/// 
/// 5. XAML-Initialized UI Elements: The constructor initializes UI controls from XAML
///    (rdbShortInsulin, rdbLongInsulin, etc.) which will be null in unit test environments.
/// 
/// TO MAKE THIS CODE TESTABLE:
/// - Extract the logic from OnBackButtonPressed into a public or internal method that can be tested
/// - Use dependency injection for database and logging dependencies
/// - Make HasUnsavedChanges and IsNavigatingAway protected or internal for testing
/// - Consider extracting the navigation logic into a separate testable service
/// - Use MAUI integration tests instead of unit tests for page-level functionality
/// 
/// The tests below document all edge cases and scenarios that SHOULD be tested once the code
/// is refactored to be testable. Each test includes detailed comments explaining the expected
/// behavior and what would need to be set up to execute the test.
/// </remarks>
public partial class OnBackButtonPressedTests
{
    /// <summary>
    /// Tests that OnBackButtonPressed returns false when there are no unsaved changes,
    /// allowing the default back navigation behavior to proceed.
    /// </summary>
    /// <remarks>
    /// This test verifies the normal/happy path scenario where the user presses the back button
    /// and there are no unsaved changes to warn about.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges = false
    /// - IsNavigatingAway = false (default state)
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() should check: if (HasUnsavedChanges && !IsNavigatingAway)
    /// - Condition evaluates to: if (false && !false) = if (false && true) = if (false)
    /// - Method returns false
    /// - No dialog should be shown
    /// - Android back button proceeds with default behavior
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Ensure HasUnsavedChanges remains false (default)
    /// 4. Use reflection or subclass to call the protected method
    /// 5. Assert the return value is false
    /// 6. Verify ShowUnsavedChangesDialog was not called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "Class depends on ContentPage initialization, XAML components, and database access " +
            "via static Common.Database. Private fields HasUnsavedChanges and IsNavigatingAway " +
            "cannot be verified or set without reflection. Refactoring needed to make testable.")]
    public void OnBackButtonPressed_NoUnsavedChanges_ReturnsFalse()
    {
        // Arrange
        // Would need: var page = new InjectionsPage(null);
        // Requires: MAUI framework initialization, database setup
        // Default state: HasUnsavedChanges = false, IsNavigatingAway = false

        // Act
        // Would need: bool result = page.OnBackButtonPressed();
        // Problem: Method is protected, cannot call directly

        // Assert
        // Expected: Assert.That(result, Is.False);
        // Expected: ShowUnsavedChangesDialog should not be called
        // Expected: Navigation should proceed normally
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns true when there are unsaved changes
    /// and navigation is not already in progress, preventing default back navigation
    /// and showing the unsaved changes dialog.
    /// </summary>
    /// <remarks>
    /// This test verifies the primary guard condition that prevents users from accidentally
    /// losing unsaved work when pressing the back button.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges = true
    /// - IsNavigatingAway = false
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() should check: if (HasUnsavedChanges && !IsNavigatingAway)
    /// - Condition evaluates to: if (true && !false) = if (true && true) = if (true)
    /// - ShowUnsavedChangesDialog() should be called
    /// - Method returns true
    /// - Android back button navigation is prevented
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Trigger UI changes to set HasUnsavedChanges = true
    ///    (e.g., modify txtUnits, change date/time, etc.)
    /// 4. Ensure IsNavigatingAway = false (default)
    /// 5. Use reflection or subclass to call the protected method
    /// 6. Assert the return value is true
    /// 7. Verify ShowUnsavedChangesDialog was called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "Setting HasUnsavedChanges requires triggering UI change events (OnValueChanged) " +
            "which depend on XAML-initialized controls. Cannot mock or access private fields " +
            "without reflection. Refactoring needed to make testable.")]
    public void OnBackButtonPressed_HasUnsavedChanges_ReturnsTrue()
    {
        // Arrange
        // Would need: var page = new InjectionsPage(null);
        // Requires: MAUI framework, database, UI controls initialized
        // Would need to trigger: page.OnValueChanged(sender, e) to set HasUnsavedChanges = true
        // State: HasUnsavedChanges = true, IsNavigatingAway = false

        // Act
        // Would need: bool result = page.OnBackButtonPressed();
        // Problem: Method is protected, cannot call directly

        // Assert
        // Expected: Assert.That(result, Is.True);
        // Expected: ShowUnsavedChangesDialog should be called
        // Expected: Navigation should be prevented
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when navigation is already in progress
    /// (IsNavigatingAway = true), regardless of unsaved changes, allowing the navigation to complete.
    /// </summary>
    /// <remarks>
    /// This test verifies that once navigation has started (after user confirms in dialog or
    /// saves changes), the back button allows the navigation to proceed.
    /// 
    /// Test scenario:
    /// - IsNavigatingAway = true
    /// - HasUnsavedChanges = false (doesn't matter, but testing clean state)
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() should check: if (HasUnsavedChanges && !IsNavigatingAway)
    /// - Condition evaluates to: if (false && !true) = if (false && false) = if (false)
    /// - Method returns false
    /// - No dialog should be shown
    /// - Navigation proceeds
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Set IsNavigatingAway = true (via reflection or internal setter)
    /// 4. Use reflection or subclass to call the protected method
    /// 5. Assert the return value is false
    /// 6. Verify ShowUnsavedChangesDialog was not called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "IsNavigatingAway is a private field that can only be set during actual navigation flow. " +
            "Cannot access or modify without reflection. Refactoring needed to make testable.")]
    public void OnBackButtonPressed_IsNavigatingAway_ReturnsFalse()
    {
        // Arrange
        // Would need: var page = new InjectionsPage(null);
        // Requires: MAUI framework, database
        // Would need to set: IsNavigatingAway = true (private field, needs reflection)
        // State: HasUnsavedChanges = false, IsNavigatingAway = true

        // Act
        // Would need: bool result = page.OnBackButtonPressed();
        // Problem: Method is protected, cannot call directly

        // Assert
        // Expected: Assert.That(result, Is.False);
        // Expected: ShowUnsavedChangesDialog should not be called
        // Expected: Navigation should proceed
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when both HasUnsavedChanges and
    /// IsNavigatingAway are true, giving precedence to the navigation in progress.
    /// </summary>
    /// <remarks>
    /// This test verifies the critical edge case where there are unsaved changes BUT
    /// navigation is already in progress (user already made their choice to leave).
    /// The IsNavigatingAway flag takes precedence to prevent showing the dialog again.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges = true
    /// - IsNavigatingAway = true
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() should check: if (HasUnsavedChanges && !IsNavigatingAway)
    /// - Condition evaluates to: if (true && !true) = if (true && false) = if (false)
    /// - Method returns false (the !IsNavigatingAway condition short-circuits)
    /// - No dialog should be shown (already shown during initial navigation attempt)
    /// - Navigation proceeds
    /// 
    /// This scenario typically occurs when:
    /// 1. User makes changes (HasUnsavedChanges = true)
    /// 2. User presses back button -> dialog shown
    /// 3. User clicks "Don't Save" or "Save" -> IsNavigatingAway set to true
    /// 4. Navigation proceeds with this second back button check
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Trigger UI changes to set HasUnsavedChanges = true
    /// 4. Set IsNavigatingAway = true (simulating post-dialog navigation)
    /// 5. Use reflection or subclass to call the protected method
    /// 6. Assert the return value is false
    /// 7. Verify ShowUnsavedChangesDialog was not called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "Setting both HasUnsavedChanges and IsNavigatingAway requires complex state manipulation " +
            "via UI changes and navigation flow. Private fields cannot be accessed without reflection. " +
            "This edge case requires integration testing with full MAUI context.")]
    public void OnBackButtonPressed_HasUnsavedChangesAndIsNavigatingAway_ReturnsFalse()
    {
        // Arrange
        // Would need: var page = new InjectionsPage(null);
        // Requires: MAUI framework, database, UI controls
        // Would need to trigger: UI changes to set HasUnsavedChanges = true
        // Would need to set: IsNavigatingAway = true (private field)
        // State: HasUnsavedChanges = true, IsNavigatingAway = true

        // Act
        // Would need: bool result = page.OnBackButtonPressed();
        // Problem: Method is protected, cannot call directly

        // Assert
        // Expected: Assert.That(result, Is.False);
        // Expected: ShowUnsavedChangesDialog should not be called
        // Expected: Navigation should proceed despite unsaved changes
        // Expected: This represents the "user already confirmed" scenario
    }

    /// <summary>
    /// Tests that OnBackButtonPressed correctly delegates to HandleBackNavigation method.
    /// </summary>
    /// <remarks>
    /// This test verifies that OnBackButtonPressed is a simple wrapper that delegates
    /// all logic to the HandleBackNavigation method.
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - Return value should match what HandleBackNavigation returns
    /// 
    /// Code analysis:
    /// Line 95-99 in InjectionsPage.xaml.cs:
    /// protected override bool OnBackButtonPressed()
    /// {
    ///     // Intercept Android back button
    ///     return HandleBackNavigation();
    /// }
    /// 
    /// This is a straightforward delegation pattern. The actual logic is in HandleBackNavigation.
    /// 
    /// To implement this test, you would need to:
    /// 1. Mock or spy on HandleBackNavigation method
    /// 2. Call OnBackButtonPressed
    /// 3. Verify HandleBackNavigation was called exactly once
    /// 4. Verify the return value matches
    /// 
    /// However, this requires either:
    /// - Mocking private methods (not supported by Moq)
    /// - Using a partial mock or subclass (prohibited by instructions)
    /// - Reflection (prohibited by instructions)
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and HandleBackNavigation is private. " +
            "Cannot verify method calls without mocking private methods or using reflection, " +
            "both of which are prohibited. The delegation is visible in source code analysis.")]
    public void OnBackButtonPressed_AlwaysDelegatesToHandleBackNavigation()
    {
        // Arrange
        // Would need: A way to verify HandleBackNavigation is called
        // Problem: HandleBackNavigation is private, cannot mock with Moq
        // Problem: OnBackButtonPressed is protected, cannot call directly

        // Act
        // Would need: page.OnBackButtonPressed();

        // Assert
        // Expected: Verify HandleBackNavigation was called exactly once
        // Expected: Return value matches HandleBackNavigation's return value
        // Reality: Cannot verify private method calls without prohibited techniques
    }
}




/// <summary>
/// Unit tests for the InjectionsPage constructor.
/// </summary>
/// <remarks>
/// IMPORTANT: All tests in this class are marked as [Ignore] because the InjectionsPage constructor
/// is NOT testable in isolation due to:
/// 
/// 1. MAUI Framework Dependency: Inherits from ContentPage and calls InitializeComponent(),
///    which requires full MAUI framework initialization.
/// 
/// 2. Static Dependencies: Direct access to Common.Database and General.LogOfProgram
///    which cannot be mocked with Moq.
/// 
/// 3. XAML-Initialized UI Elements: rdbShortInsulin and rdbLongInsulin are initialized
///    by InitializeComponent() and will be null in unit tests.
/// 
/// 4. Instance Method Calls: RefreshUi(), SaveOriginalInjection(), and AttachChangeHandlers()
///    depend on XAML-initialized controls.
/// 
/// TO MAKE THIS CODE TESTABLE:
/// - Inject IDataLayer and ILogger via constructor parameters
/// - Extract initialization logic into a separate, testable method
/// - Use MAUI integration tests for page constructors
/// - Separate business logic from UI initialization
/// 
/// These tests document the edge cases that SHOULD be tested after refactoring.
/// </remarks>
public partial class InjectionsPageConstructorEdgeCaseTests
{
    /// <summary>
    /// Tests that the constructor completes successfully with a valid positive IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// This test verifies the happy path with a typical positive ID value.
    /// 
    /// NOTE: The IdInjection parameter is currently UNUSED in the constructor body.
    /// This may be a bug or incomplete implementation that should be investigated.
    /// 
    /// Expected behavior:
    /// - Constructor should complete without throwing
    /// - MonthsOfDataShownInTheGrids should be set from parameters (if > 0)
    /// - Insulin IDs should be retrieved from parameters
    /// - Radio button content should reflect insulin names or defaults
    /// - pageIsLoading should be false after initialization
    /// - RefreshUi(), SaveOriginalInjection(), and AttachChangeHandlers() should be called
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithValidPositiveIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = 42;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Assert.That(page.pageIsLoading, Is.False);
        // Verify radio buttons are set correctly
        // Verify insulin drug IDs are retrieved
        // Verify initialization methods were called
    }

    /// <summary>
    /// Tests that the constructor handles null IdInjection parameter correctly.
    /// </summary>
    /// <remarks>
    /// The IdInjection parameter is nullable, so null is a valid input.
    /// Since the parameter is currently unused, behavior should be identical to any other value.
    /// 
    /// Expected behavior:
    /// - Constructor should complete successfully
    /// - All initialization steps should proceed normally
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithNullIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = null;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Verify initialization completes without errors
    }

    /// <summary>
    /// Tests that the constructor handles zero IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// Zero is a boundary value that often requires special handling in ID-based systems.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithZeroIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = 0;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles negative IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// Negative IDs are typically invalid in database systems, but since the parameter
    /// is unused, no validation occurs.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithNegativeIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = -1;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles int.MaxValue for IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// Tests boundary value for maximum integer value.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithMaxValueIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = int.MaxValue;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles int.MinValue for IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// Tests boundary value for minimum integer value.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithMinValueIdInjection_InitializesSuccessfully()
    {
        // Arrange
        int? idInjection = int.MinValue;

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor handles the case when GetParameters() returns null.
    /// </summary>
    /// <remarks>
    /// Expected behavior when parameters is null:
    /// - MonthsOfDataShownInTheGrids should retain its default value (3)
    /// - IdCurrentShortActingInsulin should remain null (due to null-conditional operator)
    /// - IdCurrentLongActingInsulin should remain null
    /// - Radio buttons should display default text "Short act." and "Long act."
    /// - GetOneInsulinDrug should be called with null, likely returning null
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer to return null from GetParameters().
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetParametersReturnsNull_UsesDefaultValues()
    {
        // Arrange
        // Mock IDataLayer.GetParameters() to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
        // Assert.That(page.IdCurrentShortActingInsulin, Is.Null);
        // Assert.That(page.IdCurrentLongActingInsulin, Is.Null);
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.MonthsOfDataShownInTheGrids is null.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - The condition (parameters.MonthsOfDataShownInTheGrids > 0) will be false (null > 0 is false)
    /// - MonthsOfDataShownInTheGrids should retain its initial value (3)
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer.GetParameters() to return Parameters with null MonthsOfDataShownInTheGrids.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNull_UsesDefaultValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.MonthsOfDataShownInTheGrids is zero.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - The condition (parameters.MonthsOfDataShownInTheGrids > 0) is false since 0 is not > 0
    /// - MonthsOfDataShownInTheGrids should retain its initial value (3)
    /// - No exception should be thrown
    /// 
    /// To implement: Mock IDataLayer.GetParameters() to return Parameters with MonthsOfDataShownInTheGrids = 0.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsZero_UsesDefaultValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = 0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.MonthsOfDataShownInTheGrids is negative.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - The condition (parameters.MonthsOfDataShownInTheGrids > 0) is false for negative values
    /// - MonthsOfDataShownInTheGrids should retain its initial value (3)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNegative_UsesDefaultValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = -5.0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with a valid positive value.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - The condition (parameters.MonthsOfDataShownInTheGrids > 0) is true
    /// - MonthsOfDataShownInTheGrids should be updated to the value from parameters (e.g., 6.0)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsPositive_UpdatesValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = 6.0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(6.0));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with a very large positive value.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should be updated to the large value since it's > 0
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsVeryLarge_UpdatesValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = 1000000.0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(1000000.0));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with a small positive value.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - MonthsOfDataShownInTheGrids should be updated to 0.1 since it's > 0
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsSmallPositive_UpdatesValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = 0.1

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(0.1));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with double.PositiveInfinity.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - PositiveInfinity > 0 is true, so value should be updated
    /// - MonthsOfDataShownInTheGrids should be set to PositiveInfinity
    /// - No exception should be thrown (though this may cause issues elsewhere)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsPositiveInfinity_UpdatesValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = double.PositiveInfinity

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(double.PositiveInfinity));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with double.NegativeInfinity.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - NegativeInfinity > 0 is false
    /// - MonthsOfDataShownInTheGrids should retain default value (3.0)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNegativeInfinity_UsesDefaultValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = double.NegativeInfinity

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids with double.NaN.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - NaN > 0 is false (NaN comparisons always return false)
    /// - MonthsOfDataShownInTheGrids should retain default value (3.0)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMonthsOfDataIsNaN_UsesDefaultValue()
    {
        // Arrange
        // Mock Parameters with MonthsOfDataShownInTheGrids = double.NaN

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.IdInsulinDrug_Short is null.
    /// </summary>
    /// <remarks>
    /// Expected behavior when IdInsulinDrug_Short is null:
    /// - IdCurrentShortActingInsulin should be null (null-conditional operator)
    /// - GetOneInsulinDrug(null) should be called
    /// - currentShortInsulin likely returns null
    /// - Condition (IdCurrentShortActingInsulin != null && currentShortInsulin != null) is false
    /// - rdbShortInsulin.Content should be set to "Short act."
    /// - CurrentInjection.IdInsulinDrug should NOT be set
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinIdIsNull_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.IdCurrentShortActingInsulin, Is.Null);
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when GetOneInsulinDrug returns null for short insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when GetOneInsulinDrug returns null:
    /// - IdCurrentShortActingInsulin may have a value (e.g., 5)
    /// - currentShortInsulin is null
    /// - Condition (IdCurrentShortActingInsulin != null && currentShortInsulin != null) is false
    /// - rdbShortInsulin.Content should be set to "Short act."
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetOneInsulinDrugReturnsNullForShort_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = 5
        // Mock BL_BolusesAndInjections.GetOneInsulinDrug(5) to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when InsulinDrug.Name is null for short insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when Name is null:
    /// - IdCurrentShortActingInsulin is not null
    /// - currentShortInsulin is not null
    /// - currentShortInsulin.Name is null
    /// - Null-coalescing operator (Name ?? "Short act.") results in "Short act."
    /// - rdbShortInsulin.Content should be set to "Short act."
    /// - CurrentInjection.IdInsulinDrug should be set to IdCurrentShortActingInsulin
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinNameIsNull_UsesDefaultText()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = 5
        // Mock GetOneInsulinDrug(5) to return InsulinDrug with Name = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.EqualTo(5));
    }

    /// <summary>
    /// Tests that the constructor handles the case when InsulinDrug.Name is an empty string for short insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when Name is empty string:
    /// - Null-coalescing operator does NOT trigger (empty string is not null)
    /// - rdbShortInsulin.Content should be set to empty string ""
    /// - CurrentInjection.IdInsulinDrug should be set
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinNameIsEmptyString_UsesEmptyString()
    {
        // Arrange
        // Mock GetOneInsulinDrug to return InsulinDrug with Name = ""

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the constructor handles the case when InsulinDrug.Name is whitespace only for short insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when Name is whitespace:
    /// - Null-coalescing operator does NOT trigger (whitespace is not null)
    /// - rdbShortInsulin.Content should be set to the whitespace string
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinNameIsWhitespace_UsesWhitespace()
    {
        // Arrange
        // Mock GetOneInsulinDrug to return InsulinDrug with Name = "   "

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("   "));
    }

    /// <summary>
    /// Tests that the constructor handles the case when InsulinDrug.Name is a very long string for short insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when Name is very long:
    /// - rdbShortInsulin.Content should be set to the full long string
    /// - No exception or truncation should occur at this level (UI may truncate during display)
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinNameIsVeryLong_UsesFullString()
    {
        // Arrange
        string longName = new string('A', 10000);
        // Mock GetOneInsulinDrug to return InsulinDrug with Name = longName

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(longName));
    }

    /// <summary>
    /// Tests that the constructor handles the case when InsulinDrug.Name contains special characters for short insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when Name has special characters:
    /// - rdbShortInsulin.Content should be set to the string with special characters
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinNameHasSpecialCharacters_UsesFullString()
    {
        // Arrange
        string specialName = "Insulin™ <>&\"'@#$%";
        // Mock GetOneInsulinDrug to return InsulinDrug with Name = specialName

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(specialName));
    }

    /// <summary>
    /// Tests that the constructor sets CurrentInjection.IdInsulinDrug when short insulin is available.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - When IdCurrentShortActingInsulin is not null AND currentShortInsulin is not null
    /// - CurrentInjection.IdInsulinDrug should be set to IdCurrentShortActingInsulin (e.g., 5)
    /// - rdbShortInsulin.Content should be set to insulin name or "Short act."
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenShortInsulinAvailable_SetsCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = 5
        // Mock GetOneInsulinDrug(5) to return valid InsulinDrug with Name = "Humalog"

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.EqualTo(5));
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Humalog"));
    }

    /// <summary>
    /// Tests that the constructor handles the case when Parameters.IdInsulinDrug_Long is null.
    /// </summary>
    /// <remarks>
    /// Expected behavior when IdInsulinDrug_Long is null:
    /// - IdCurrentLongActingInsulin should be null
    /// - GetOneInsulinDrug(null) should be called
    /// - currentLongInsulin likely returns null
    /// - Condition (IdCurrentLongActingInsulin != null && currentLongInsulin != null) is false
    /// - rdbLongInsulin.Content should be set to "Long act."
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenLongInsulinIdIsNull_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Long = null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.IdCurrentLongActingInsulin, Is.Null);
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when GetOneInsulinDrug returns null for long insulin.
    /// </summary>
    /// <remarks>
    /// Expected behavior when GetOneInsulinDrug returns null:
    /// - IdCurrentLongActingInsulin may have a value
    /// - currentLongInsulin is null
    /// - Condition is false
    /// - rdbLongInsulin.Content should be set to "Long act."
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetOneInsulinDrugReturnsNullForLong_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Long = 10
        // Mock GetOneInsulinDrug(10) to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles the case when both insulin IDs are null.
    /// </summary>
    /// <remarks>
    /// Expected behavior when both IDs are null:
    /// - Both radio buttons should display default text "Short act." and "Long act."
    /// - CurrentInjection.IdInsulinDrug should not be set (remains whatever it was initialized to)
    /// - No exception should be thrown
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenBothInsulinIdsAreNull_SetsDefaultRadioButtonTexts()
    {
        // Arrange
        // Mock Parameters with both IdInsulinDrug_Short and IdInsulinDrug_Long as null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being zero.
    /// </summary>
    /// <remarks>
    /// Expected behavior when IDs are zero:
    /// - GetOneInsulinDrug should be called with 0
    /// - Behavior depends on business logic (may return null or a specific record)
    /// - If null returned, default text should be used
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreZero_CallsGetOneInsulinDrugWithZero()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = 0, IdInsulinDrug_Long = 0

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug(0) was called twice
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being negative.
    /// </summary>
    /// <remarks>
    /// Expected behavior when IDs are negative:
    /// - GetOneInsulinDrug should be called with negative values
    /// - Business logic likely returns null for invalid IDs
    /// - Default text should be used
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreNegative_CallsGetOneInsulinDrugWithNegative()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = -1, IdInsulinDrug_Long = -5

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug(-1) and GetOneInsulinDrug(-5) were called
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being int.MaxValue.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreMaxValue_CallsGetOneInsulinDrugWithMaxValue()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = int.MaxValue, IdInsulinDrug_Long = int.MaxValue

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug(int.MaxValue) was called
    }

    /// <summary>
    /// Tests that the constructor handles insulin IDs being int.MinValue.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInsulinIdsAreMinValue_CallsGetOneInsulinDrugWithMinValue()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = int.MinValue

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify GetOneInsulinDrug(int.MinValue) was called
    }

    /// <summary>
    /// Tests that the constructor logs an error and displays an alert when GetParameters throws an exception.
    /// </summary>
    /// <remarks>
    /// Expected behavior when GetParameters throws:
    /// - Exception should be caught by outer try-catch
    /// - General.LogOfProgram.Error("InjectionsPage | ctor", ex) should be called
    /// - MainThread.BeginInvokeOnMainThread should be called with lambda
    /// - DisplayAlert should be called with error message
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetParametersThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Mock Common.Database.GetParameters() to throw InvalidOperationException

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify General.LogOfProgram.Error was called with "InjectionsPage | ctor" and the exception
        // Verify DisplayAlert was called with error message
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by GetOneInsulinDrug.
    /// </summary>
    /// <remarks>
    /// Expected behavior when GetOneInsulinDrug throws:
    /// - Exception should be caught
    /// - Error should be logged
    /// - Alert should be displayed
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenGetOneInsulinDrugThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Mock BL_BolusesAndInjections.GetOneInsulinDrug to throw SqlException

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error was logged and alert was shown
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by RefreshUi.
    /// </summary>
    /// <remarks>
    /// Expected behavior when RefreshUi throws:
    /// - Exception should be caught
    /// - Error should be logged
    /// - Alert should be displayed
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenRefreshUiThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // RefreshUi would need to be mockable to throw exception

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error handling occurred
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by SaveOriginalInjection.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenSaveOriginalInjectionThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // SaveOriginalInjection would need to be mockable

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error handling
    }

    /// <summary>
    /// Tests that the constructor handles exception thrown by AttachChangeHandlers.
    /// </summary>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenAttachChangeHandlersThrowsException_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // AttachChangeHandlers would need to be mockable

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify error handling
    }

    /// <summary>
    /// Tests that the constructor handles exception in DisplayAlert gracefully.
    /// </summary>
    /// <remarks>
    /// Expected behavior when DisplayAlert throws:
    /// - Exception should be caught by inner catch block (lines 89-92)
    /// - Exception should be swallowed (no throw, no logging)
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenDisplayAlertThrowsException_SwallowsExceptionAndDoesNotCrash()
    {
        // Arrange
        // Mock GetParameters to throw, then mock DisplayAlert to also throw

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify no exception propagates from constructor
    }

    /// <summary>
    /// Tests that the constructor handles exception in MainThread.BeginInvokeOnMainThread gracefully.
    /// </summary>
    /// <remarks>
    /// Expected behavior when BeginInvokeOnMainThread throws:
    /// - Exception should be caught by inner catch block
    /// - Exception should be swallowed
    /// - Application should not crash
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenMainThreadBeginInvokeThrowsException_SwallowsException()
    {
        // Arrange
        // Mock MainThread.BeginInvokeOnMainThread to throw exception

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify exception is swallowed
    }

    /// <summary>
    /// Tests that the constructor sets pageIsLoading correctly during initialization.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - pageIsLoading should be set to true at line 41 (after GetParameters)
    /// - pageIsLoading should be set to false at line 68 (before RefreshUi)
    /// - After initialization completes, pageIsLoading should be false
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_DuringInitialization_SetsPageIsLoadingTrueThenFalse()
    {
        // Arrange & Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.pageIsLoading, Is.False);
    }

    /// <summary>
    /// Tests that the constructor calls initialization methods in the correct order.
    /// </summary>
    /// <remarks>
    /// Expected call order (lines 69-73):
    /// 1. RefreshUi()
    /// 2. SaveOriginalInjection()
    /// 3. AttachChangeHandlers()
    /// 
    /// This order is important for proper initialization.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WhenInitializing_CallsMethodsInCorrectOrder()
    {
        // Arrange
        // Mock dependencies to track call order

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify RefreshUi was called before SaveOriginalInjection
        // Verify SaveOriginalInjection was called before AttachChangeHandlers
    }

    /// <summary>
    /// Tests that the constructor properly initializes with valid insulin data for both short and long acting insulins.
    /// </summary>
    /// <remarks>
    /// Expected behavior with valid insulins:
    /// - Both IdCurrentShortActingInsulin and IdCurrentLongActingInsulin should be set
    /// - Both currentShortInsulin and currentLongInsulin should be not null
    /// - rdbShortInsulin.Content should be set to short insulin name
    /// - rdbLongInsulin.Content should be set to long insulin name
    /// - CurrentInjection.IdInsulinDrug should be set to short insulin ID
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithBothValidInsulins_SetsAllPropertiesCorrectly()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = 5, IdInsulinDrug_Long = 10
        // Mock GetOneInsulinDrug(5) to return InsulinDrug with Name = "Humalog"
        // Mock GetOneInsulinDrug(10) to return InsulinDrug with Name = "Lantus"

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.EqualTo(5));
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Humalog"));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Lantus"));
    }

    /// <summary>
    /// Tests that the constructor does NOT modify CurrentInjection.IdInsulinDrug for long-acting insulin.
    /// </summary>
    /// <remarks>
    /// IMPORTANT: The code only sets CurrentInjection.IdInsulinDrug for short-acting insulin (line 52).
    /// Long-acting insulin only updates the radio button text, not CurrentInjection.IdInsulinDrug.
    /// This may be intentional design or a potential bug to investigate.
    /// </remarks>
    [Test]
    [Ignore("Constructor requires MAUI framework and has unmockable static dependencies. Refactoring needed.")]
    public void Constructor_WithLongInsulinOnly_DoesNotSetCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Mock Parameters with IdInsulinDrug_Short = null, IdInsulinDrug_Long = 10
        // Mock GetOneInsulinDrug(10) to return valid InsulinDrug

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.Null);
        // Assert.That(page.rdbLongInsulin.Content, Is.Not.EqualTo("Long act."));
    }
}



/// <summary>
/// Unit tests for the OnBackButtonPressed method of InjectionsPage.
/// </summary>
/// <remarks>
/// CRITICAL LIMITATION: The OnBackButtonPressed method CANNOT be tested in isolation due to:
/// 
/// 1. Protected Method Access: OnBackButtonPressed is a protected override method inherited from
///    ContentPage. It cannot be called directly from test code without reflection (which is prohibited
///    by the testing guidelines).
/// 
/// 2. MAUI Framework Dependency: The InjectionsPage class inherits from ContentPage and calls
///    InitializeComponent() in the constructor, which requires the full MAUI framework to be initialized.
///    This cannot run in standard unit tests without a MAUI test harness.
/// 
/// 3. Private Field Dependencies: The method delegates to HandleBackNavigation(), which checks two
///    private fields (HasUnsavedChanges and IsNavigatingAway). These fields cannot be set from test
///    code without reflection or changing the class design to make them internal/protected.
/// 
/// 4. Static Dependencies: The constructor accesses DatabaseService.Instance.Database (static field) and
///    General.LogOfProgram (static field), which cannot be mocked with Moq.
/// 
/// 5. XAML-Initialized UI Elements: The constructor initializes UI controls from XAML
///    (rdbShortInsulin, rdbLongInsulin, etc.) which will be null in unit test environments.
/// 
/// TO MAKE THIS CODE TESTABLE:
/// - Extract the logic from OnBackButtonPressed into a public or internal method that can be tested
/// - Use dependency injection for database and logging dependencies
/// - Make HasUnsavedChanges and IsNavigatingAway protected or internal for testing
/// - Consider extracting the navigation logic into a separate testable service
/// - Use MAUI integration tests instead of unit tests for page-level functionality
/// 
/// The tests below document all edge cases and scenarios that SHOULD be tested once the code
/// is refactored to be testable. Each test includes detailed comments explaining the expected
/// behavior and what would need to be set up to execute the test.
/// </remarks>
public partial class InjectionsPageOnBackButtonPressedTests
{
    /// <summary>
    /// Tests that OnBackButtonPressed returns false when there are no unsaved changes,
    /// allowing the default back navigation behavior to proceed.
    /// </summary>
    /// <remarks>
    /// This test verifies the normal/happy path scenario where the user presses the back button
    /// and there are no unsaved changes to warn about.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges = false
    /// - IsNavigatingAway = false (default state)
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() checks: if (HasUnsavedChanges &amp;&amp; !IsNavigatingAway)
    /// - Condition evaluates to: if (false &amp;&amp; !false) = if (false &amp;&amp; true) = if (false)
    /// - Method returns false (line 107)
    /// - No dialog should be shown
    /// - Android back button proceeds with default behavior
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Ensure HasUnsavedChanges remains false (default)
    /// 4. Use reflection or subclass to call the protected method (prohibited)
    /// 5. Assert the return value is false
    /// 6. Verify ShowUnsavedChangesDialog was not called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "Class depends on ContentPage initialization, XAML components, and database access " +
            "via static DatabaseService.Instance. Private fields HasUnsavedChanges and IsNavigatingAway " +
            "cannot be verified or set without reflection. Refactoring needed to make testable.")]
    public void OnBackButtonPressed_NoUnsavedChanges_ReturnsFalse()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI test environment
        // 2. Initialize database (Common.Database or mock)
        // 3. Create InjectionsPage instance (requires InitializeComponent)
        // 4. Ensure HasUnsavedChanges = false (default state)
        // 5. Ensure IsNavigatingAway = false (default state)

        // Act
        // bool result = page.OnBackButtonPressed(); // Cannot call - protected method

        // Assert
        // Assert.That(result, Is.False);
        // Verify ShowUnsavedChangesDialog was not called

        Assert.Inconclusive("This test requires MAUI ContentPage initialization infrastructure " +
                            "and access to protected method OnBackButtonPressed. The method cannot be " +
                            "tested without reflection (prohibited) or refactoring to make it testable.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns true when there are unsaved changes
    /// and navigation is not already in progress, preventing default back navigation
    /// and showing the unsaved changes dialog.
    /// </summary>
    /// <remarks>
    /// This test verifies the primary guard condition that prevents users from accidentally
    /// losing unsaved work when pressing the back button.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges = true
    /// - IsNavigatingAway = false
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() checks: if (HasUnsavedChanges &amp;&amp; !IsNavigatingAway)
    /// - Condition evaluates to: if (true &amp;&amp; !false) = if (true &amp;&amp; true) = if (true)
    /// - ShowUnsavedChangesDialog() should be called (line 104)
    /// - Method returns true (line 105)
    /// - Android back button navigation is prevented
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Trigger UI changes to set HasUnsavedChanges = true
    ///    (e.g., modify txtUnits, change date/time, etc. via OnValueChanged event handler)
    /// 4. Ensure IsNavigatingAway = false (default)
    /// 5. Use reflection or subclass to call the protected method (prohibited)
    /// 6. Assert the return value is true
    /// 7. Verify ShowUnsavedChangesDialog was called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "Setting HasUnsavedChanges requires triggering UI change events (OnValueChanged) " +
            "which depend on XAML-initialized controls. Cannot mock or access private fields " +
            "without reflection. Refactoring needed to make testable.")]
    public void OnBackButtonPressed_HasUnsavedChanges_ReturnsTrue()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI test environment
        // 2. Initialize database
        // 3. Create InjectionsPage instance
        // 4. Trigger changes to set HasUnsavedChanges = true
        //    - This requires modifying UI controls like txtUnits.Text
        //    - OnValueChanged event handler sets HasUnsavedChanges via CheckForChanges()
        // 5. Ensure IsNavigatingAway = false

        // Act
        // bool result = page.OnBackButtonPressed();

        // Assert
        // Assert.That(result, Is.True);
        // Verify ShowUnsavedChangesDialog was called

        Assert.Inconclusive("This test requires MAUI infrastructure and the ability to set " +
                            "HasUnsavedChanges = true by triggering UI events, which depends on " +
                            "XAML-initialized controls. Protected method cannot be accessed without reflection.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when navigation is already in progress
    /// (IsNavigatingAway = true), regardless of unsaved changes, allowing the navigation to complete.
    /// </summary>
    /// <remarks>
    /// This test verifies that once navigation has started (after user confirms in dialog or
    /// saves changes), the back button allows the navigation to proceed.
    /// 
    /// Test scenario:
    /// - IsNavigatingAway = true
    /// - HasUnsavedChanges = false (doesn't matter, but testing clean state)
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() checks: if (HasUnsavedChanges &amp;&amp; !IsNavigatingAway)
    /// - Condition evaluates to: if (false &amp;&amp; !true) = if (false &amp;&amp; false) = if (false)
    /// - Method returns false (line 107)
    /// - No dialog should be shown
    /// - Navigation proceeds
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Set IsNavigatingAway = true (via reflection or internal setter)
    /// 4. Use reflection or subclass to call the protected method (prohibited)
    /// 5. Assert the return value is false
    /// 6. Verify ShowUnsavedChangesDialog was not called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "IsNavigatingAway is a private field that can only be set during actual navigation flow. " +
            "Cannot access or modify without reflection. Refactoring needed to make testable.")]
    public void OnBackButtonPressed_IsNavigatingAway_ReturnsFalse()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI test environment
        // 2. Initialize database
        // 3. Create InjectionsPage instance
        // 4. Set IsNavigatingAway = true
        //    - This is normally set during ShowUnsavedChangesDialog flow
        //    - Would require reflection to set directly
        // 5. HasUnsavedChanges can be any value (testing with false)

        // Act
        // bool result = page.OnBackButtonPressed();

        // Assert
        // Assert.That(result, Is.False);
        // Verify ShowUnsavedChangesDialog was not called

        Assert.Inconclusive("This test requires the ability to set IsNavigatingAway = true, " +
                            "which is a private field accessible only during navigation flow. " +
                            "Protected method cannot be accessed without reflection.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed returns false when both HasUnsavedChanges and
    /// IsNavigatingAway are true, giving precedence to the navigation in progress.
    /// </summary>
    /// <remarks>
    /// This test verifies the critical edge case where there are unsaved changes BUT
    /// navigation is already in progress (user already made their choice to leave).
    /// The IsNavigatingAway flag takes precedence to prevent showing the dialog again.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges = true
    /// - IsNavigatingAway = true
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - HandleBackNavigation() checks: if (HasUnsavedChanges &amp;&amp; !IsNavigatingAway)
    /// - Condition evaluates to: if (true &amp;&amp; !true) = if (true &amp;&amp; false) = if (false)
    /// - Method returns false (line 107 - the !IsNavigatingAway condition short-circuits)
    /// - No dialog should be shown (already shown during initial navigation attempt)
    /// - Navigation proceeds
    /// 
    /// This scenario typically occurs when:
    /// 1. User makes changes (HasUnsavedChanges = true)
    /// 2. User presses back button → dialog shown
    /// 3. User clicks "Don't Save" or "Save" → IsNavigatingAway set to true
    /// 4. Navigation proceeds with this second back button check
    /// 
    /// To implement this test, you would need to:
    /// 1. Initialize MAUI test framework
    /// 2. Create InjectionsPage with properly initialized database
    /// 3. Trigger UI changes to set HasUnsavedChanges = true
    /// 4. Set IsNavigatingAway = true (simulating post-dialog navigation)
    /// 5. Use reflection or subclass to call the protected method (prohibited)
    /// 6. Assert the return value is false
    /// 7. Verify ShowUnsavedChangesDialog was not called
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and requires MAUI infrastructure. " +
            "Setting both HasUnsavedChanges and IsNavigatingAway requires complex state manipulation " +
            "via UI changes and navigation flow. Private fields cannot be accessed without reflection. " +
            "This edge case requires integration testing with full MAUI context.")]
    public void OnBackButtonPressed_HasUnsavedChangesAndIsNavigatingAway_ReturnsFalse()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI test environment
        // 2. Initialize database
        // 3. Create InjectionsPage instance
        // 4. Trigger changes to set HasUnsavedChanges = true
        // 5. Set IsNavigatingAway = true (simulating navigation in progress)

        // Act
        // bool result = page.OnBackButtonPressed();

        // Assert
        // Assert.That(result, Is.False);
        // Verify ShowUnsavedChangesDialog was not called (navigation already approved)

        Assert.Inconclusive("This test requires setting both HasUnsavedChanges = true and " +
                            "IsNavigatingAway = true, which involves complex state manipulation. " +
                            "Protected method and private fields cannot be accessed without reflection. " +
                            "This scenario requires integration testing with full MAUI navigation flow.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed correctly delegates to HandleBackNavigation method.
    /// </summary>
    /// <remarks>
    /// This test verifies that OnBackButtonPressed is a simple wrapper that delegates
    /// all logic to the HandleBackNavigation method.
    /// 
    /// Expected behavior:
    /// - OnBackButtonPressed() should call HandleBackNavigation()
    /// - Return value should match what HandleBackNavigation returns
    /// 
    /// Code analysis (lines 95-99):
    /// protected override bool OnBackButtonPressed()
    /// {
    ///     // Intercept Android back button
    ///     return HandleBackNavigation();
    /// }
    /// 
    /// This is a straightforward delegation pattern. The actual logic is in HandleBackNavigation.
    /// 
    /// To implement this test, you would need to:
    /// 1. Mock or spy on HandleBackNavigation method
    /// 2. Call OnBackButtonPressed
    /// 3. Verify HandleBackNavigation was called exactly once
    /// 4. Verify the return value matches
    /// 
    /// However, this requires either:
    /// - Mocking private methods (not supported by Moq)
    /// - Using a partial mock or subclass (prohibited by instructions)
    /// - Reflection (prohibited by instructions)
    /// </remarks>
    [Test]
    [Ignore("Cannot test: OnBackButtonPressed is protected and HandleBackNavigation is private. " +
            "Cannot verify method calls without mocking private methods or using reflection, " +
            "both of which are prohibited. The delegation is visible in source code analysis.")]
    public void OnBackButtonPressed_AlwaysDelegatesToHandleBackNavigation()
    {
        // Arrange
        // Expected test structure:
        // 1. Create a spy or partial mock of InjectionsPage
        // 2. Setup verification for HandleBackNavigation calls

        // Act
        // bool result = page.OnBackButtonPressed();

        // Assert
        // Verify HandleBackNavigation was called exactly once
        // Verify result matches HandleBackNavigation return value

        Assert.Inconclusive("This test requires the ability to verify that a private method " +
                            "(HandleBackNavigation) is called from a protected method (OnBackButtonPressed). " +
                            "Cannot achieve this without mocking private methods or using reflection, " +
                            "both of which are prohibited. The delegation pattern is evident from source analysis.");
    }

    /// <summary>
    /// Tests that OnBackButtonPressed behavior with boundary condition: HasUnsavedChanges at the moment it transitions from false to true.
    /// </summary>
    /// <remarks>
    /// This test verifies the race condition scenario where HasUnsavedChanges might be changing
    /// at the exact moment the back button is pressed.
    /// 
    /// Test scenario:
    /// - HasUnsavedChanges transitions from false to true during back button press
    /// 
    /// Expected behavior:
    /// - The value of HasUnsavedChanges at the time of the check determines behavior
    /// - If false at check time: returns false
    /// - If true at check time: returns true and shows dialog
    /// 
    /// This is a concurrency edge case that would be very difficult to test without
    /// integration testing and timing control.
    /// </remarks>
    [Test]
    [Ignore("Cannot test: This race condition scenario requires precise timing control and " +
            "MAUI infrastructure. Protected method and private fields cannot be accessed. " +
            "Would require integration testing with concurrent operations.")]
    public void OnBackButtonPressed_RaceConditionWithUnsavedChanges_HandlesDeterministically()
    {
        // Arrange
        // Expected test structure:
        // 1. Initialize MAUI test environment
        // 2. Create InjectionsPage instance
        // 3. Setup concurrent modification of HasUnsavedChanges
        // 4. Call OnBackButtonPressed during transition

        // Act
        // bool result = page.OnBackButtonPressed();

        // Assert
        // Verify behavior is deterministic based on value at check time

        Assert.Inconclusive("This concurrency edge case cannot be reliably tested without " +
                            "integration testing infrastructure and timing control mechanisms.");
    }
}



/// <summary>
/// Unit tests for the InjectionsPage constructor.
/// </summary>
/// <remarks>
/// CRITICAL LIMITATION: This constructor is NOT testable in isolation due to:
/// 
/// 1. MAUI Framework Dependency: Inherits from ContentPage and calls InitializeComponent()
/// 2. Static Dependencies: DatabaseService.Instance and General.LogOfProgram cannot be mocked
/// 3. XAML-Initialized Controls: rdbShortInsulin and rdbLongInsulin are null in unit tests
/// 4. Instance Method Dependencies: RefreshUi(), SaveOriginalInjection(), AttachChangeHandlers()
/// 
/// TO MAKE TESTABLE:
/// - Inject IDataLayer and ILogger via constructor parameters
/// - Extract initialization logic into separate, testable methods
/// - Use MAUI integration tests instead of unit tests
/// - Separate business logic from UI initialization
/// 
/// All tests are marked [Ignore] and document expected behavior for post-refactoring validation.
/// </remarks>
public partial class InjectionsPageConstructorValidationTests
{
    /// <summary>
    /// Tests that the constructor completes successfully with valid nullable IdInjection parameter.
    /// </summary>
    /// <remarks>
    /// NOTE: The IdInjection parameter is currently UNUSED in the constructor body.
    /// This may indicate incomplete implementation or a bug.
    /// 
    /// Expected behavior:
    /// - Constructor should complete without throwing
    /// - Parameters should be loaded from database
    /// - Insulin configurations should be initialized
    /// - UI controls should be configured
    /// - pageIsLoading should be false after initialization
    /// </remarks>
    [TestCase(null)]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(42)]
    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue)]
    [Ignore("Constructor requires MAUI framework initialization and has unmockable static dependencies. " +
            "DatabaseService.Instance and General.LogOfProgram are static and cannot be mocked with Moq. " +
            "UI controls (rdbShortInsulin, rdbLongInsulin) are XAML-initialized and null in unit tests. " +
            "Refactoring to dependency injection pattern required.")]
    public void Constructor_WithVariousIdInjectionValues_InitializesSuccessfully(int? idInjection)
    {
        // Arrange
        // Would need: Mock IDataLayer, Mock ILogger, MAUI test harness

        // Act
        // var page = new InjectionsPage(idInjection);

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Verify initialization methods called
        // Verify radio buttons configured
        // Verify pageIsLoading is false

        Assert.Inconclusive("Constructor requires MAUI infrastructure and dependency injection refactoring.");
    }

    /// <summary>
    /// Tests that the constructor handles null Parameters from database gracefully.
    /// </summary>
    /// <remarks>
    /// Expected behavior when GetParameters() returns null:
    /// - MonthsOfDataShownInTheGrids retains default value (3)
    /// - IdCurrentShortActingInsulin is null (null-conditional operator)
    /// - IdCurrentLongActingInsulin is null
    /// - Radio buttons display default text "Short act." and "Long act."
    /// - No exception thrown
    /// </remarks>
    [Test]
    [Ignore("Cannot mock DatabaseService.Instance.Database.GetParameters() - static singleton pattern. " +
            "Requires dependency injection refactoring to inject IDataLayer.")]
    public void Constructor_WhenGetParametersReturnsNull_UsesDefaultValues()
    {
        // Arrange
        // Would need: Mock IDataLayer.GetParameters() to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3.0));
        // Verify default radio button text

        Assert.Inconclusive("Static DatabaseService.Instance cannot be mocked without refactoring.");
    }

    /// <summary>
    /// Tests that the constructor handles MonthsOfDataShownInTheGrids boundary values correctly.
    /// </summary>
    /// <remarks>
    /// Expected behavior for different MonthsOfDataShownInTheGrids values:
    /// - null: Uses default (3.0)
    /// - 0: Uses default (condition > 0 is false)
    /// - Negative: Uses default
    /// - Positive: Updates to parameter value
    /// - NaN: Uses default (NaN > 0 is false)
    /// - PositiveInfinity: Updates to infinity (infinity > 0 is true)
    /// - NegativeInfinity: Uses default
    /// </remarks>
    [TestCase(null, 3.0)]
    [TestCase(0.0, 3.0)]
    [TestCase(-1.0, 3.0)]
    [TestCase(0.1, 0.1)]
    [TestCase(6.0, 6.0)]
    [TestCase(double.MaxValue, double.MaxValue)]
    [Ignore("Cannot mock Parameters object returned from static DatabaseService.Instance.")]
    public void Constructor_WithVariousMonthsOfDataValues_SetsCorrectValue(double? monthsValue, double expectedResult)
    {
        // Arrange
        // Would need: Mock Parameters with MonthsOfDataShownInTheGrids = monthsValue

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(expectedResult));

        Assert.Inconclusive("Requires dependency injection to mock Parameters.");
    }

    /// <summary>
    /// Tests that the constructor handles special floating-point values for MonthsOfDataShownInTheGrids.
    /// </summary>
    [TestCase(double.NaN, 3.0, "NaN comparison always false, should use default")]
    [TestCase(double.PositiveInfinity, double.PositiveInfinity, "Infinity > 0 is true, should update")]
    [TestCase(double.NegativeInfinity, 3.0, "NegativeInfinity not > 0, should use default")]
    [Ignore("Cannot test without mocking Parameters object from static database singleton.")]
    public void Constructor_WithSpecialDoubleValues_HandlesCorrectly(double monthsValue, double expectedResult, string reason)
    {
        // Arrange
        // Would need: Mock Parameters with MonthsOfDataShownInTheGrids = monthsValue

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(expectedResult), reason);

        Assert.Inconclusive("Requires dependency injection refactoring.");
    }

    /// <summary>
    /// Tests that the constructor handles null/missing insulin configurations correctly.
    /// </summary>
    /// <remarks>
    /// Expected behavior:
    /// - When IdInsulinDrug_Short is null: Radio button shows "Short act."
    /// - When IdInsulinDrug_Long is null: Radio button shows "Long act."
    /// - When GetOneInsulinDrug returns null: Default text used
    /// - When InsulinDrug.Name is null: Null-coalescing provides default
    /// - CurrentInjection.IdInsulinDrug only set for short insulin when both ID and drug are not null
    /// </remarks>
    [Test]
    [Ignore("Cannot mock BL_BolusesAndInjections.GetOneInsulinDrug() - instance field 'bl' not injectable. " +
            "Cannot access XAML controls rdbShortInsulin/rdbLongInsulin without InitializeComponent().")]
    public void Constructor_WhenInsulinConfigurationsNull_SetsDefaultRadioButtonText()
    {
        // Arrange
        // Would need: Mock bl.GetOneInsulinDrug() to return null

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo("Short act."));
        // Assert.That(page.rdbLongInsulin.Content, Is.EqualTo("Long act."));

        Assert.Inconclusive("BL instance and XAML controls not accessible without refactoring.");
    }

    /// <summary>
    /// Tests that the constructor handles InsulinDrug.Name edge cases correctly.
    /// </summary>
    /// <remarks>
    /// Expected behavior for InsulinDrug.Name values:
    /// - null: Null-coalescing operator provides "Short act." or "Long act."
    /// - Empty string: Empty string used (not null, so no coalescing)
    /// - Whitespace: Whitespace used as-is
    /// - Very long string: Full string used
    /// - Special characters: String used as-is
    /// </remarks>
    [TestCase(null, "Short act.", "Null-coalescing should provide default")]
    [TestCase("", "", "Empty string is not null, no coalescing")]
    [TestCase("   ", "   ", "Whitespace preserved")]
    [TestCase("Insulin™ <>&\"'", "Insulin™ <>&\"'", "Special characters preserved")]
    [Ignore("Cannot mock InsulinDrug objects without injectable dependencies.")]
    public void Constructor_WithVariousInsulinNames_SetsContentCorrectly(string? insulinName, string expectedContent, string reason)
    {
        // Arrange
        // Would need: Mock GetOneInsulinDrug to return InsulinDrug with Name = insulinName

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.rdbShortInsulin.Content, Is.EqualTo(expectedContent), reason);

        Assert.Inconclusive("Requires dependency injection for BL_BolusesAndInjections.");
    }

    /// <summary>
    /// Tests that the constructor sets CurrentInjection.IdInsulinDrug only for short-acting insulin.
    /// </summary>
    /// <remarks>
    /// IMPORTANT: The code only sets CurrentInjection.IdInsulinDrug for short-acting insulin (line 52).
    /// Long-acting insulin only updates the radio button text, not CurrentInjection.IdInsulinDrug.
    /// This asymmetry may be intentional design or a potential bug.
    /// </remarks>
    [Test]
    [Ignore("Cannot verify CurrentInjection.IdInsulinDrug without MAUI infrastructure and dependency injection.")]
    public void Constructor_WhenShortInsulinAvailable_SetsCurrentInjectionIdInsulinDrug()
    {
        // Arrange
        // Would need: Mock Parameters with IdInsulinDrug_Short = 5
        // Would need: Mock GetOneInsulinDrug(5) to return valid InsulinDrug

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.CurrentInjection.IdInsulinDrug, Is.EqualTo(5));

        Assert.Inconclusive("Private field CurrentInjection not accessible without refactoring.");
    }

    /// <summary>
    /// Tests that the constructor logs errors and displays alerts when exceptions occur.
    /// </summary>
    /// <remarks>
    /// Expected exception handling behavior:
    /// - Any exception caught in outer try-catch
    /// - General.LogOfProgram?.Error("InjectionsPage | ctor", ex) called
    /// - MainThread.BeginInvokeOnMainThread invoked with DisplayAlert lambda
    /// - Inner try-catch swallows DisplayAlert exceptions
    /// - Application does not crash
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static General.LogOfProgram or trigger controlled exceptions in database access.")]
    public void Constructor_WhenExceptionOccurs_LogsErrorAndDisplaysAlert()
    {
        // Arrange
        // Would need: Mock DatabaseService.Instance.Database.GetParameters() to throw exception
        // Would need: Mock General.LogOfProgram to verify Error() called
        // Would need: Mock MainThread.BeginInvokeOnMainThread (static method)

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify General.LogOfProgram.Error was called with correct parameters
        // Verify no exception propagated from constructor
        // Verify DisplayAlert was attempted

        Assert.Inconclusive("Static dependencies cannot be mocked with Moq.");
    }

    /// <summary>
    /// Tests that the constructor swallows exceptions from DisplayAlert without crashing.
    /// </summary>
    /// <remarks>
    /// Expected behavior when DisplayAlert throws:
    /// - Exception from DisplayAlert or MainThread.BeginInvokeOnMainThread caught by inner catch
    /// - Exception swallowed silently (lines 89-92)
    /// - Constructor completes without throwing
    /// </remarks>
    [Test]
    [Ignore("Cannot test DisplayAlert exception handling without MAUI Page infrastructure and injectable dependencies.")]
    public void Constructor_WhenDisplayAlertThrows_SwallowsExceptionGracefully()
    {
        // Arrange
        // Would need: Mock to trigger exception in GetParameters
        // Would need: Mock DisplayAlert to throw exception
        // Would need: Verify no exception escapes constructor

        // Act & Assert
        // Assert.DoesNotThrow(() => new InjectionsPage(null));

        Assert.Inconclusive("Page.DisplayAlert cannot be mocked without refactoring.");
    }

    /// <summary>
    /// Tests that the constructor sets pageIsLoading correctly during initialization sequence.
    /// </summary>
    /// <remarks>
    /// Expected pageIsLoading lifecycle:
    /// - Line 41: Set to true after GetParameters
    /// - Line 68: Set to false before calling RefreshUi
    /// - Final state: false after initialization completes
    /// </remarks>
    [Test]
    [Ignore("Private field pageIsLoading not accessible without reflection or making it internal/protected.")]
    public void Constructor_DuringInitialization_SetsPageIsLoadingCorrectly()
    {
        // Arrange & Act
        // var page = new InjectionsPage(null);

        // Assert
        // Assert.That(page.pageIsLoading, Is.False, "pageIsLoading should be false after initialization");

        Assert.Inconclusive("Private field not accessible without refactoring to internal or protected visibility.");
    }

    /// <summary>
    /// Tests that the constructor calls initialization methods in the correct order.
    /// </summary>
    /// <remarks>
    /// Expected method call sequence (lines 69-73):
    /// 1. RefreshUi()
    /// 2. SaveOriginalInjection()
    /// 3. AttachChangeHandlers()
    /// 
    /// This order is critical for proper UI initialization and change tracking.
    /// </remarks>
    [Test]
    [Ignore("Cannot verify method call sequence without mocking private methods or extracting to testable service.")]
    public void Constructor_CallsInitializationMethods_InCorrectOrder()
    {
        // Arrange
        // Would need: Partial mock or spy to track method call order
        // Would need: Verify RefreshUi called before SaveOriginalInjection
        // Would need: Verify SaveOriginalInjection called before AttachChangeHandlers

        // Act
        // var page = new InjectionsPage(null);

        // Assert
        // Verify call order using mock verification

        Assert.Inconclusive("Private method call verification requires extracting logic to injectable service.");
    }
}