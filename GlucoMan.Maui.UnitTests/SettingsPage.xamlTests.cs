using System;
using System.Collections.Generic;

using GlucoMan;
using GlucoMan.Maui;
using GlucoMan.Maui.Services;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;




/// <summary>
/// Unit tests for the SettingsPage class.
/// </summary>
[TestFixture]
public partial class SettingsPageTests
{
    /// <summary>
    /// Tests that OnDisappearing unsubscribes from the LocalizationService.CultureChanged event.
    /// This test verifies that the event handler is properly removed to prevent memory leaks.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate SettingsPage in unit tests due to XAML dependencies (InitializeComponent) and UI control access in constructor. This test requires integration testing or refactoring the constructor to separate XAML initialization from business logic.")]
    public void OnDisappearing_WhenCalled_UnsubscribesFromCultureChangedEvent()
    {
        // Arrange
        // NOTE: This test cannot be executed as a unit test because:
        // 1. SettingsPage constructor calls InitializeComponent() which is XAML-generated code
        // 2. Constructor accesses UI controls (cmbShortActingInsulin, cmbLongActingInsulin, etc.)
        // 3. Constructor instantiates BL_BolusesAndInjections and calls business logic methods
        // 
        // To make this testable, consider:
        // - Moving business logic initialization out of constructor to a separate Initialize method
        // - Using property injection or lazy initialization for UI control bindings
        // - Testing this as an integration test with XAML infrastructure available
        //
        // Expected behavior:
        // When OnDisappearing is called, it should unsubscribe the OnCultureChanged handler
        // from _localizationService.CultureChanged event to prevent memory leaks.

        var mockLocalizationService = new Mock<LocalizationService>();

        // This line would fail because constructor calls InitializeComponent():
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Act
        // page.OnDisappearing(); // Cannot call because it's protected

        // Assert
        // Verify that CultureChanged event handler was unsubscribed
        // In practice, this would be verified by checking that the event has no subscribers
        // or by verifying that OnCultureChanged is not invoked when CultureChanged is raised
        // after OnDisappearing has been called.
    }

    /// <summary>
    /// Tests that OnDisappearing can be called multiple times without errors.
    /// This verifies that unsubscribing from an event that's already unsubscribed doesn't cause exceptions.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate SettingsPage in unit tests due to XAML dependencies. See OnDisappearing_WhenCalled_UnsubscribesFromCultureChangedEvent for details.")]
    public void OnDisappearing_WhenCalledMultipleTimes_DoesNotThrowException()
    {
        // Arrange
        // NOTE: Same limitations as above test apply.
        //
        // Expected behavior:
        // Calling OnDisappearing multiple times should not throw an exception.
        // C# event unsubscription with -= is idempotent, so unsubscribing twice is safe.

        var mockLocalizationService = new Mock<LocalizationService>();

        // This line would fail:
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Act & Assert
        // Assert.DoesNotThrow(() =>
        // {
        //     page.OnDisappearing(); // First call
        //     page.OnDisappearing(); // Second call should not throw
        // });
    }

    /// <summary>
    /// Tests that OnDisappearing calls base.OnDisappearing() to ensure proper lifecycle handling.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate SettingsPage in unit tests due to XAML dependencies. This would require a testable base class or integration testing.")]
    public void OnDisappearing_WhenCalled_CallsBaseOnDisappearing()
    {
        // Arrange
        // NOTE: Same limitations apply.
        //
        // Expected behavior:
        // OnDisappearing should call base.OnDisappearing() to ensure ContentPage's
        // lifecycle methods execute properly.
        //
        // This is difficult to verify in isolation without creating a custom ContentPage
        // implementation or using integration tests with the full MAUI framework.

        var mockLocalizationService = new Mock<LocalizationService>();

        // Cannot create instance due to XAML dependencies

        // Act & Assert
        // Would need to verify base.OnDisappearing() was called
    }

    /// <summary>
    /// Tests that the constructor throws NullReferenceException when localizationService parameter is null.
    /// This test verifies null parameter handling.
    /// Expected: NullReferenceException is thrown.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML control dependencies in InitializeComponent(). " +
            "The SettingsPage constructor calls InitializeComponent() which requires XAML-initialized controls " +
            "(cmbShortActingInsulin, cmbLongActingInsulin, txtInsulinShortDuration, txtInsulinLongDuration, pickerLanguage). " +
            "Additionally, it uses static methods (BlGeneral.GetSettingsPageParameters()) and concrete business layer instances " +
            "that cannot be mocked. Refactor to dependency injection pattern for testability.")]
    public void Constructor_NullLocalizationService_ThrowsNullReferenceException()
    {
        // Arrange
        LocalizationService? localizationService = null;

        // Act & Assert
        // Note: This would likely throw during InitializeComponent() or when accessing _localizationService.CurrentCulture
        Assert.Throws<NullReferenceException>(() => new SettingsPage(localizationService!));
    }

    /// <summary>
    /// Tests that the constructor initializes with valid LocalizationService parameter.
    /// This test verifies basic construction with valid dependencies.
    /// Expected: Object is created without exceptions.
    /// Note: This test cannot execute due to XAML and static dependency limitations.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML control dependencies in InitializeComponent(). " +
            "The SettingsPage constructor calls InitializeComponent() which requires XAML-initialized controls. " +
            "Static method BlGeneral.GetSettingsPageParameters() cannot be mocked. " +
            "Business layer instance 'bl' (BL_BolusesAndInjections) is created in field initializer and cannot be mocked. " +
            "To make this testable, refactor to: " +
            "1. Inject BL_BolusesAndInjections via constructor or property " +
            "2. Wrap static calls in an injectable service " +
            "3. Use ViewModel pattern to separate UI logic from controls")]
    public void Constructor_ValidLocalizationService_InitializesSuccessfully()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Note: The following would be needed but cannot be properly mocked:
        // - InitializeComponent() creates XAML controls
        // - bl.GetAllInsulinDrugs() is called on concrete instance
        // - BlGeneral.GetSettingsPageParameters() is a static method
        // - pickerLanguage, cmbShortActingInsulin, cmbLongActingInsulin controls must exist
        // - txtInsulinShortDuration, txtInsulinLongDuration controls must exist

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Would verify:
        // - settingsPage.ShortActingInsulins is populated from bl.GetAllInsulinDrugs(TypeOfInsulinAction.Short)
        // - settingsPage.LongActingInsulins is populated from bl.GetAllInsulinDrugs(TypeOfInsulinAction.Long)
        // - settingsPage.Parameters is set from BlGeneral.GetSettingsPageParameters()
        // - Event subscription: _localizationService.CultureChanged += OnCultureChanged
    }

    /// <summary>
    /// Tests constructor behavior when business layer returns null insulin lists.
    /// This verifies handling of null return values from GetAllInsulinDrugs.
    /// Expected: Properties are set to null and controls handle null gracefully.
    /// Note: Cannot execute due to inability to mock business layer instance.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to inability to mock BL_BolusesAndInjections instance. " +
            "The 'bl' field is created in field initializer and GetAllInsulinDrugs() cannot be mocked. " +
            "Refactor to inject BL_BolusesAndInjections via constructor to enable testing.")]
    public void Constructor_GetAllInsulinDrugsReturnsNull_HandlesNullGracefully()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Cannot mock: bl.GetAllInsulinDrugs() returns null
        // Would need to inject BL_BolusesAndInjections to mock this behavior

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Would verify:
        // - settingsPage.ShortActingInsulins == null
        // - settingsPage.LongActingInsulins == null
        // - No NullReferenceException when setting cmbShortActingInsulin.ItemsSource
    }

    /// <summary>
    /// Tests constructor behavior when GetSettingsPageParameters returns null.
    /// This verifies the else branch (lines 48-59) that handles null Parameters.
    /// Expected: Selected insulins set to null, combo boxes cleared, text fields set to empty.
    /// Note: Cannot execute due to inability to mock static method.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to inability to mock BlGeneral.GetSettingsPageParameters() static method. " +
            "Refactor to inject an IParametersService to enable mocking.")]
    public void Constructor_GetSettingsPageParametersReturnsNull_SetsDefaultValues()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Cannot mock: BlGeneral.GetSettingsPageParameters() is static
        // Would need to wrap in injectable service interface

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Would verify (when Parameters is null):
        // - settingsPage.SelectedShortActingInsulin == null
        // - settingsPage.SelectedLongActingInsulin == null
        // - cmbShortActingInsulin.SelectedItem == null
        // - cmbLongActingInsulin.SelectedItem == null
        // - txtInsulinShortDuration.Text == string.Empty
        // - txtInsulinLongDuration.Text == string.Empty
    }

    /// <summary>
    /// Tests constructor behavior when Parameters is not null with matching insulin IDs.
    /// This verifies the if branch (lines 36-47) that sets up selected insulins.
    /// Expected: BindingContext set, selected insulins found and set, durations displayed.
    /// Note: Cannot execute due to static method and XAML control dependencies.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to static method BlGeneral.GetSettingsPageParameters() and XAML control dependencies.")]
    public void Constructor_ParametersNotNullWithMatchingInsulinIds_SetsSelectedInsulins()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Cannot mock:
        // - BlGeneral.GetSettingsPageParameters() to return non-null Parameters
        // - bl.GetAllInsulinDrugs() to return lists with specific insulins
        // - XAML controls for setting BindingContext and SelectedItem

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Would verify (when Parameters is not null):
        // - settingsPage.BindingContext == settingsPage.Parameters
        // - settingsPage.SelectedShortActingInsulin matches insulin with Parameters.IdInsulinDrug_Short
        // - cmbShortActingInsulin.SelectedItem == settingsPage.SelectedShortActingInsulin
        // - txtInsulinShortDuration.Text == SelectedShortActingInsulin.DurationInHours.ToString()
        // - settingsPage.SelectedLongActingInsulin matches insulin with Parameters.IdInsulinDrug_Long
        // - cmbLongActingInsulin.SelectedItem == settingsPage.SelectedLongActingInsulin
        // - txtInsulinLongDuration.Text == SelectedLongActingInsulin.DurationInHours.ToString()
    }

    /// <summary>
    /// Tests constructor behavior when no insulin matches the parameter IDs.
    /// This verifies FirstOrDefault returns null when no matching insulin is found.
    /// Expected: SelectedShortActingInsulin and SelectedLongActingInsulin are null.
    /// Note: Cannot execute due to business layer mocking limitations.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to inability to control business layer return values.")]
    public void Constructor_NoMatchingInsulinIds_SetsSelectedInsulinsToNull()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need to mock:
        // - bl.GetAllInsulinDrugs() to return lists without matching IDs
        // - BlGeneral.GetSettingsPageParameters() to return Parameters with non-matching IDs

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When FirstOrDefault returns null:
        // - settingsPage.SelectedShortActingInsulin == null
        // - settingsPage.SelectedLongActingInsulin == null
        // - txtInsulinShortDuration.Text == string.Empty (due to null-coalescing)
        // - txtInsulinLongDuration.Text == string.Empty (due to null-coalescing)
    }

    /// <summary>
    /// Tests constructor behavior when selected insulin has null DurationInHours.
    /// This verifies null-conditional and null-coalescing operators handle null durations.
    /// Expected: Text fields set to empty string.
    /// Note: Cannot execute due to business layer and XAML dependencies.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to business layer and XAML control dependencies.")]
    public void Constructor_SelectedInsulinWithNullDuration_SetsTextToEmpty()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need insulin with DurationInHours == null

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When SelectedShortActingInsulin.DurationInHours is null:
        // - txtInsulinShortDuration.Text == string.Empty
        // When SelectedLongActingInsulin.DurationInHours is null:
        // - txtInsulinLongDuration.Text == string.Empty
    }

    /// <summary>
    /// Tests that constructor subscribes to CultureChanged event.
    /// This verifies event subscription at line 65.
    /// Expected: OnCultureChanged is registered as event handler.
    /// Note: Cannot execute due to XAML dependencies, but event subscription logic exists.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML dependencies, but serves as documentation of expected behavior.")]
    public void Constructor_SubscribesToCultureChangedEvent()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Would verify:
        // - _localizationService.CultureChanged event has OnCultureChanged handler registered
        // This happens at line 65: _localizationService.CultureChanged += OnCultureChanged;
    }

    /// <summary>
    /// Tests that SetupLanguagePicker is called during construction.
    /// This verifies language picker initialization at line 62.
    /// Expected: pickerLanguage is populated with language options and current culture is selected.
    /// Note: Cannot execute due to XAML control (pickerLanguage) dependency.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML control (pickerLanguage) dependency in SetupLanguagePicker method.")]
    public void Constructor_CallsSetupLanguagePicker()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Act
        // var settingsPage = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Would verify SetupLanguagePicker execution:
        // - pickerLanguage.ItemsSource contains LanguageOption with CultureCode "en" and "it"
        // - pickerLanguage.SelectedItem matches _localizationService.CurrentCulture.TwoLetterISOLanguageName
    }

    /// <summary>
    /// Tests that OnDisappearing handles null LocalizationService gracefully.
    /// This verifies defensive programming for edge case where service might be null.
    /// Input: SettingsPage with null _localizationService field.
    /// Expected: NullReferenceException on line 103 when attempting event unsubscription.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate SettingsPage in unit tests due to XAML dependencies. " +
            "Additionally, _localizationService is readonly and set in constructor, making null assignment impossible. " +
            "See OnDisappearing_WhenCalled_UnsubscribesFromCultureChangedEvent for detailed explanation.")]
    public void OnDisappearing_WithNullLocalizationService_ThrowsNullReferenceException()
    {
        // Arrange
        // NOTE: _localizationService is readonly and initialized in constructor
        // Cannot set to null after construction without reflection (which is prohibited)

        // If constructor accepted null:
        // var page = new SettingsPage(null);

        // Act & Assert
        // Expected behavior:
        // Line 103: _localizationService.CultureChanged -= OnCultureChanged
        // Would throw NullReferenceException if _localizationService is null
        // Assert.Throws<NullReferenceException>(() => page.OnDisappearing());
    }
}


/// <summary>
/// Unit tests for the SettingsPage constructor.
/// </summary>
[TestFixture]
public partial class SettingsPageConstructorTests
{
    /// <summary>
    /// Tests that constructor throws NullReferenceException when localizationService parameter is null.
    /// This verifies null parameter handling.
    /// Input: null localizationService parameter.
    /// Expected: NullReferenceException thrown during initialization.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML dependencies in InitializeComponent(). " +
            "The constructor calls InitializeComponent() which requires XAML-initialized controls " +
            "(cmbShortActingInsulin, cmbLongActingInsulin, txtInsulinShortDuration, txtInsulinLongDuration, pickerLanguage). " +
            "Business layer instances (BL_BolusesAndInjections, BL_General) are field-initialized and cannot be mocked. " +
            "Refactor to dependency injection pattern for testability.")]
    public void Constructor_NullLocalizationService_ThrowsNullReferenceException()
    {
        // Arrange
        LocalizationService? localizationService = null;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => new SettingsPage(localizationService!));
    }

    /// <summary>
    /// Tests constructor with valid LocalizationService when business layer returns valid data.
    /// This verifies successful initialization with all dependencies available.
    /// Input: Valid LocalizationService, mocked business layer returning valid insulin lists and parameters.
    /// Expected: All properties initialized, insulin lists populated, parameters set, event subscribed.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to unmockable dependencies. " +
            "BL_BolusesAndInjections and BL_General are instantiated as fields and cannot be injected. " +
            "InitializeComponent() requires XAML infrastructure. " +
            "XAML controls (cmbShortActingInsulin, cmbLongActingInsulin, txtInsulinShortDuration, txtInsulinLongDuration, pickerLanguage) must be initialized. " +
            "To test: inject business layer interfaces via constructor, separate XAML initialization from business logic.")]
    public void Constructor_ValidLocalizationService_InitializesAllPropertiesSuccessfully()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Verify:
        // - page.ShortActingInsulins populated from bl.GetAllInsulinDrugs(TypeOfInsulinAction.Short)
        // - page.LongActingInsulins populated from bl.GetAllInsulinDrugs(TypeOfInsulinAction.Long)
        // - page.Parameters set from BlGeneral.GetSettingsPageParameters()
        // - cmbShortActingInsulin.ItemsSource == ShortActingInsulins
        // - cmbLongActingInsulin.ItemsSource == LongActingInsulins
        // - _localizationService.CultureChanged event has OnCultureChanged handler subscribed
        // - SetupLanguagePicker() was called (pickerLanguage populated)
    }

    /// <summary>
    /// Tests constructor when GetSettingsPageParameters returns null Parameters.
    /// This verifies the null parameter handling branch (lines 49-60).
    /// Input: Valid LocalizationService, business layer returns null Parameters.
    /// Expected: Selected insulins set to null, combo selections cleared, text fields set to empty strings.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to inability to mock BL_General.GetSettingsPageParameters() static call " +
            "and XAML control dependencies. Requires dependency injection refactoring.")]
    public void Constructor_NullParameters_SetsDefaultValues()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need to mock BlGeneral.GetSettingsPageParameters() to return null

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When Parameters is null:
        // - page.SelectedShortActingInsulin == null
        // - page.SelectedLongActingInsulin == null
        // - cmbShortActingInsulin.SelectedItem == null
        // - cmbLongActingInsulin.SelectedItem == null
        // - txtInsulinShortDuration.Text == string.Empty
        // - txtInsulinLongDuration.Text == string.Empty
    }

    /// <summary>
    /// Tests constructor when Parameters contains valid insulin IDs that match insulins in lists.
    /// This verifies the matching logic using FirstOrDefault (lines 41-47).
    /// Input: Parameters with IdInsulinDrug_Short and IdInsulinDrug_Long matching insulin list items.
    /// Expected: Selected insulins found and set, combo boxes updated, duration text fields populated.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to unmockable business layer and XAML dependencies. " +
            "Cannot control return values from bl.GetAllInsulinDrugs() or BlGeneral.GetSettingsPageParameters().")]
    public void Constructor_ParametersWithMatchingInsulinIds_SetsSelectedInsulins()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need to mock:
        // - bl.GetAllInsulinDrugs() to return specific insulin lists
        // - BlGeneral.GetSettingsPageParameters() to return Parameters with matching IDs

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When Parameters.IdInsulinDrug_Short matches an insulin in ShortActingInsulins:
        // - page.SelectedShortActingInsulin != null
        // - page.SelectedShortActingInsulin.IdInsulinDrug == Parameters.IdInsulinDrug_Short
        // - cmbShortActingInsulin.SelectedItem == SelectedShortActingInsulin
        // - txtInsulinShortDuration.Text == SelectedShortActingInsulin.DurationInHours.ToString()
        // When Parameters.IdInsulinDrug_Long matches an insulin in LongActingInsulins:
        // - page.SelectedLongActingInsulin != null
        // - page.SelectedLongActingInsulin.IdInsulinDrug == Parameters.IdInsulinDrug_Long
        // - cmbLongActingInsulin.SelectedItem == SelectedLongActingInsulin
        // - txtInsulinLongDuration.Text == SelectedLongActingInsulin.DurationInHours.ToString()
    }

    /// <summary>
    /// Tests constructor when Parameters contains insulin IDs that don't match any insulins.
    /// This verifies FirstOrDefault returns null for non-matching predicates (lines 41, 45).
    /// Input: Parameters with insulin IDs not present in insulin lists.
    /// Expected: SelectedShortActingInsulin and SelectedLongActingInsulin remain null, text fields empty.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to unmockable business layer dependencies.")]
    public void Constructor_ParametersWithNonMatchingInsulinIds_SetsSelectedInsulinsToNull()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need insulin lists without matching IDs and Parameters with specific IDs

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When FirstOrDefault finds no match:
        // - page.SelectedShortActingInsulin == null
        // - page.SelectedLongActingInsulin == null
        // - txtInsulinShortDuration.Text == string.Empty (null-coalescing on line 43)
        // - txtInsulinLongDuration.Text == string.Empty (null-coalescing on line 47)
    }

    /// <summary>
    /// Tests constructor when selected insulin has null DurationInHours property.
    /// This verifies null-conditional operator handling (lines 43, 47).
    /// Input: Insulin with DurationInHours = null.
    /// Expected: Text fields set to empty string via null-coalescing operator.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to unmockable business layer. Cannot control InsulinDrug.DurationInHours values.")]
    public void Constructor_InsulinWithNullDuration_SetsTextFieldsToEmpty()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need insulin objects with DurationInHours == null

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When SelectedShortActingInsulin?.DurationInHours is null:
        // - txtInsulinShortDuration.Text == string.Empty (via ?? string.Empty on line 43)
        // When SelectedLongActingInsulin?.DurationInHours is null:
        // - txtInsulinLongDuration.Text == string.Empty (via ?? string.Empty on line 47)
    }

    /// <summary>
    /// Tests constructor when GetAllInsulinDrugs returns empty lists.
    /// This verifies handling of empty collections (not null).
    /// Input: GetAllInsulinDrugs returns empty List&lt;InsulinDrug&gt;.
    /// Expected: Empty lists assigned, FirstOrDefault returns null, default values set.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to unmockable BL_BolusesAndInjections instance. " +
            "Cannot control GetAllInsulinDrugs() return values.")]
    public void Constructor_EmptyInsulinLists_HandlesEmptyCollectionsGracefully()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need bl.GetAllInsulinDrugs() to return empty lists

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When ShortActingInsulins and LongActingInsulins are empty:
        // - page.ShortActingInsulins.Count == 0
        // - page.LongActingInsulins.Count == 0
        // - FirstOrDefault returns null (no items to match)
        // - Selected insulins remain null
    }

    /// <summary>
    /// Tests constructor subscribes to LocalizationService.CultureChanged event.
    /// This verifies event subscription on line 66.
    /// Input: Valid LocalizationService.
    /// Expected: OnCultureChanged handler registered to CultureChanged event.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML dependencies in InitializeComponent(). " +
            "Cannot verify event subscription without instantiating the page.")]
    public void Constructor_SubscribesToCultureChangedEvent()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Verify _localizationService.CultureChanged has OnCultureChanged handler subscribed
        // This occurs at line 66: _localizationService.CultureChanged += OnCultureChanged
    }

    /// <summary>
    /// Tests constructor calls SetupLanguagePicker to initialize language selection.
    /// This verifies SetupLanguagePicker invocation on line 63.
    /// Input: Valid LocalizationService with CurrentCulture set.
    /// Expected: pickerLanguage populated with language options and current culture selected.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to XAML control (pickerLanguage) dependency in SetupLanguagePicker. " +
            "SetupLanguagePicker accesses pickerLanguage.ItemsSource, ItemDisplayBinding, and SelectedItem.")]
    public void Constructor_CallsSetupLanguagePicker_InitializesLanguageSelection()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // Verify SetupLanguagePicker execution (line 63):
        // - pickerLanguage.ItemsSource contains LanguageOption for "en" and "it"
        // - pickerLanguage.SelectedItem matches CurrentCulture.TwoLetterISOLanguageName
    }

    /// <summary>
    /// Tests constructor sets BindingContext when Parameters is not null.
    /// This verifies BindingContext assignment on line 39.
    /// Input: Valid Parameters object.
    /// Expected: page.BindingContext == page.Parameters.
    /// </summary>
    [Test]
    [Ignore("Cannot execute due to unmockable GetSettingsPageParameters() and XAML dependencies.")]
    public void Constructor_NonNullParameters_SetsBindingContext()
    {
        // Arrange
        var mockLocalizationService = new Mock<LocalizationService>();

        // Would need BlGeneral.GetSettingsPageParameters() to return non-null

        // Act
        // var page = new SettingsPage(mockLocalizationService.Object);

        // Assert
        // When Parameters != null:
        // - page.BindingContext == page.Parameters (line 39)
    }
}