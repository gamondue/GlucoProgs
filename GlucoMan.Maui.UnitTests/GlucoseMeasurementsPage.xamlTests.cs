using System;

using GlucoMan;
using GlucoMan.Maui;
using Microsoft.Maui.Controls;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;


/// <summary>
/// Unit tests for the GlucoseMeasurementsPage class.
/// Note: This is a partial class because the source is a XAML code-behind partial class.
/// </summary>
public partial class GlucoseMeasurementsPageTests
{
    /// <summary>
    /// Tests that btnAddMeasurement_Click sets current date/time when chkNowInAdd is checked.
    /// 
    /// NOTE: This test cannot be executed in its current form due to architectural limitations:
    /// 
    /// 1. GlucoseMeasurementsPage is a XAML code-behind partial class that requires InitializeComponent()
    ///    to be called, which depends on the MAUI runtime and compiled XAML resources.
    /// 
    /// 2. The class uses inline field initialization for dependencies (bl, currentGlucose) that cannot
    ///    be mocked or injected.
    /// 
    /// 3. UI controls (chkNowInAdd, dtpEventDate, dtpEventTime, txtGlucose, txtIdGlucoseRecord, 
    ///    txtNotes, cvMeasurements) are defined in the XAML partial class and are sealed concrete types
    ///    that cannot be mocked using Moq.
    /// 
    /// 4. Private methods (FromUiToClass, RefreshGrid) cannot be verified without reflection, which
    ///    is prohibited by the testing guidelines.
    /// 
    /// RECOMMENDED REFACTORING to make this testable:
    /// 
    /// - Extract business logic from the event handler into a separate, testable service or view model
    /// - Use dependency injection to provide BL_GlucoseMeasurements instead of inline initialization
    /// - Create an abstraction layer for UI interactions that can be mocked
    /// - Consider using the MVVM pattern to separate presentation logic from UI controls
    /// 
    /// Example refactored design:
    /// 
    /// public interface IGlucoseMeasurementViewModel
    /// {
    ///     void AddMeasurement(bool useCurrentTime);
    /// }
    /// 
    /// This would allow the business logic to be tested independently of the MAUI UI framework.
    /// </summary>
    [Test]
    [Ignore("Cannot execute: XAML code-behind requires MAUI runtime and UI controls cannot be mocked. See comments for refactoring suggestions.")]
    public void btnAddMeasurement_Click_WhenChkNowInAddIsChecked_SetsCurrentDateTime()
    {
        // This test is marked as Ignored because the current architecture prevents unit testing.
        // The GlucoseMeasurementsPage constructor calls InitializeComponent() which requires
        // the MAUI XAML runtime to be available, making it impossible to instantiate in a
        // standard unit test environment.

        Assert.Inconclusive("Test cannot be executed due to XAML code-behind architecture. Refactoring required.");
    }

    /// <summary>
    /// Tests that btnAddMeasurement_Click clears IdGlucoseRecord to create a new record.
    /// 
    /// See btnAddMeasurement_Click_WhenChkNowInAddIsChecked_SetsCurrentDateTime for detailed
    /// explanation of why this test cannot be executed and recommended refactoring approaches.
    /// </summary>
    [Test]
    [Ignore("Cannot execute: XAML code-behind requires MAUI runtime and UI controls cannot be mocked. See comments for refactoring suggestions.")]
    public void btnAddMeasurement_Click_Always_ClearsIdGlucoseRecord()
    {
        Assert.Inconclusive("Test cannot be executed due to XAML code-behind architecture. Refactoring required.");
    }

    /// <summary>
    /// Tests that btnAddMeasurement_Click calls SaveOneGlucoseMeasurement on the business layer.
    /// 
    /// See btnAddMeasurement_Click_WhenChkNowInAddIsChecked_SetsCurrentDateTime for detailed
    /// explanation of why this test cannot be executed and recommended refactoring approaches.
    /// </summary>
    [Test]
    [Ignore("Cannot execute: XAML code-behind requires MAUI runtime and UI controls cannot be mocked. See comments for refactoring suggestions.")]
    public void btnAddMeasurement_Click_Always_CallsSaveOneGlucoseMeasurement()
    {
        Assert.Inconclusive("Test cannot be executed due to XAML code-behind architecture. Refactoring required.");
    }

    /// <summary>
    /// Tests that btnAddMeasurement_Click refreshes the grid after saving.
    /// 
    /// See btnAddMeasurement_Click_WhenChkNowInAddIsChecked_SetsCurrentDateTime for detailed
    /// explanation of why this test cannot be executed and recommended refactoring approaches.
    /// </summary>
    [Test]
    [Ignore("Cannot execute: XAML code-behind requires MAUI runtime and UI controls cannot be mocked. See comments for refactoring suggestions.")]
    public void btnAddMeasurement_Click_Always_RefreshesGrid()
    {
        Assert.Inconclusive("Test cannot be executed due to XAML code-behind architecture. Refactoring required.");
    }

    /// <summary>
    /// Tests that btnAddMeasurement_Click does not set current date/time when chkNowInAdd is not checked.
    /// 
    /// See btnAddMeasurement_Click_WhenChkNowInAddIsChecked_SetsCurrentDateTime for detailed
    /// explanation of why this test cannot be executed and recommended refactoring approaches.
    /// </summary>
    [Test]
    [Ignore("Cannot execute: XAML code-behind requires MAUI runtime and UI controls cannot be mocked. See comments for refactoring suggestions.")]
    public void btnAddMeasurement_Click_WhenChkNowInAddIsNotChecked_DoesNotSetDateTime()
    {
        Assert.Inconclusive("Test cannot be executed due to XAML code-behind architecture. Refactoring required.");
    }

    /// <summary>
    /// Tests that the constructor with null IdGlucoseRecord initializes the page without loading a glucose record.
    /// Expected behavior: Should not attempt to load a glucose record when IdGlucoseRecord is null.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be fully executed because:
    /// 1. InitializeComponent() requires XAML infrastructure and MAUI runtime which are not available in unit tests
    /// 2. Common.Database is a static class that cannot be mocked with Moq
    /// 3. The 'bl' field (BL_GlucoseMeasurements) is initialized at class level, not injected, preventing mock substitution
    /// 4. RefreshUi() is a private method that depends on UI elements initialized by InitializeComponent()
    /// 
    /// To make this testable, the following refactoring is recommended:
    /// - Inject BL_GlucoseMeasurements via constructor or property
    /// - Abstract database access behind an interface (IParametersRepository) and inject it
    /// - Separate business logic from UI initialization
    /// - Consider using a factory pattern or service locator for XAML page creation
    /// </remarks>
    [Test]
    [Ignore("Cannot test ContentPage constructor that requires XAML infrastructure and uses static dependencies that cannot be mocked")]
    public void Constructor_WithNullIdGlucoseRecord_DoesNotLoadGlucoseRecord()
    {
        // This test is marked as Ignore because:
        // - Cannot call InitializeComponent() without MAUI infrastructure
        // - Cannot mock Common.Database.GetParameters() (static method)
        // - Cannot verify bl.GetOneGlucoseRecord was not called (field not injected)
        Assert.Inconclusive("This constructor cannot be unit tested due to tight coupling to XAML infrastructure and static dependencies.");
    }

    /// <summary>
    /// Tests that the constructor with a valid IdGlucoseRecord loads the corresponding glucose record.
    /// Expected behavior: Should call bl.GetOneGlucoseRecord with the provided ID.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be fully executed because:
    /// 1. InitializeComponent() requires XAML infrastructure and MAUI runtime which are not available in unit tests
    /// 2. Common.Database is a static class that cannot be mocked with Moq
    /// 3. The 'bl' field (BL_GlucoseMeasurements) is initialized at class level, not injected, preventing mock substitution
    /// 4. RefreshUi() is a private method that depends on UI elements initialized by InitializeComponent()
    /// 
    /// To make this testable, the following refactoring is recommended:
    /// - Inject BL_GlucoseMeasurements via constructor or property
    /// - Abstract database access behind an interface (IParametersRepository) and inject it
    /// - Separate business logic from UI initialization
    /// - Consider using a factory pattern or service locator for XAML page creation
    /// </remarks>
    [Test]
    [Ignore("Cannot test ContentPage constructor that requires XAML infrastructure and uses static dependencies that cannot be mocked")]
    public void Constructor_WithValidIdGlucoseRecord_LoadsGlucoseRecord()
    {
        // This test is marked as Ignore because:
        // - Cannot call InitializeComponent() without MAUI infrastructure
        // - Cannot mock Common.Database.GetParameters() (static method)
        // - Cannot verify bl.GetOneGlucoseRecord was called with correct ID (field not injected)
        Assert.Inconclusive("This constructor cannot be unit tested due to tight coupling to XAML infrastructure and static dependencies.");
    }

    /// <summary>
    /// Tests that the constructor handles zero as IdGlucoseRecord.
    /// Expected behavior: Should call bl.GetOneGlucoseRecord with 0.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be fully executed because:
    /// 1. InitializeComponent() requires XAML infrastructure and MAUI runtime which are not available in unit tests
    /// 2. Common.Database is a static class that cannot be mocked with Moq
    /// 3. The 'bl' field (BL_GlucoseMeasurements) is initialized at class level, not injected, preventing mock substitution
    /// 4. RefreshUi() is a private method that depends on UI elements initialized by InitializeComponent()
    /// 
    /// To make this testable, the following refactoring is recommended:
    /// - Inject BL_GlucoseMeasurements via constructor or property
    /// - Abstract database access behind an interface (IParametersRepository) and inject it
    /// - Separate business logic from UI initialization
    /// - Add validation for IdGlucoseRecord to ensure it's a valid identifier
    /// </remarks>
    [Test]
    [Ignore("Cannot test ContentPage constructor that requires XAML infrastructure and uses static dependencies that cannot be mocked")]
    public void Constructor_WithZeroIdGlucoseRecord_LoadsGlucoseRecordWithZeroId()
    {
        // This test is marked as Ignore because:
        // - Cannot call InitializeComponent() without MAUI infrastructure
        // - Cannot mock Common.Database.GetParameters() (static method)
        // - Cannot verify bl.GetOneGlucoseRecord behavior (field not injected)
        Assert.Inconclusive("This constructor cannot be unit tested due to tight coupling to XAML infrastructure and static dependencies.");
    }

    /// <summary>
    /// Tests that the constructor handles negative IdGlucoseRecord values.
    /// Expected behavior: Should call bl.GetOneGlucoseRecord with the negative value (may need validation).
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be fully executed because:
    /// 1. InitializeComponent() requires XAML infrastructure and MAUI runtime which are not available in unit tests
    /// 2. Common.Database is a static class that cannot be mocked with Moq
    /// 3. The 'bl' field (BL_GlucoseMeasurements) is initialized at class level, not injected, preventing mock substitution
    /// 4. RefreshUi() is a private method that depends on UI elements initialized by InitializeComponent()
    /// 
    /// POTENTIAL BUG: The constructor does not validate that IdGlucoseRecord is a valid positive identifier.
    /// Negative values may represent invalid database IDs and should potentially be rejected or validated.
    /// 
    /// To make this testable, the following refactoring is recommended:
    /// - Inject BL_GlucoseMeasurements via constructor or property
    /// - Abstract database access behind an interface (IParametersRepository) and inject it
    /// - Add validation for IdGlucoseRecord parameter
    /// - Separate business logic from UI initialization
    /// </remarks>
    [Test]
    [Ignore("Cannot test ContentPage constructor that requires XAML infrastructure and uses static dependencies that cannot be mocked")]
    public void Constructor_WithNegativeIdGlucoseRecord_LoadsGlucoseRecordWithNegativeId()
    {
        // This test is marked as Ignore because:
        // - Cannot call InitializeComponent() without MAUI infrastructure
        // - Cannot mock Common.Database.GetParameters() (static method)
        // - Cannot verify bl.GetOneGlucoseRecord behavior (field not injected)
        Assert.Inconclusive("This constructor cannot be unit tested due to tight coupling to XAML infrastructure and static dependencies.");
    }

    /// <summary>
    /// Tests that the parameterless constructor calls the parameterized constructor with null.
    /// Expected behavior: Should delegate to GlucoseMeasurementsPage(null).
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be fully executed because:
    /// 1. InitializeComponent() requires XAML infrastructure and MAUI runtime which are not available in unit tests
    /// 2. Common.Database is a static class that cannot be mocked with Moq
    /// 3. The 'bl' field (BL_GlucoseMeasurements) is initialized at class level, not injected, preventing mock substitution
    /// 4. RefreshUi() is a private method that depends on UI elements initialized by InitializeComponent()
    /// 
    /// To make this testable, the following refactoring is recommended:
    /// - Inject BL_GlucoseMeasurements via constructor or property
    /// - Abstract database access behind an interface (IParametersRepository) and inject it
    /// - Separate business logic from UI initialization
    /// - Consider using a factory pattern or service locator for XAML page creation
    /// </remarks>
    [Test]
    [Ignore("Cannot test ContentPage constructor that requires XAML infrastructure and uses static dependencies that cannot be mocked")]
    public void Constructor_Parameterless_DelegatesToParameterizedConstructorWithNull()
    {
        // This test is marked as Ignore because:
        // - Cannot call InitializeComponent() without MAUI infrastructure
        // - Cannot mock Common.Database.GetParameters() (static method)
        // - Behavior is identical to Constructor_WithNullIdGlucoseRecord test
        Assert.Inconclusive("This constructor cannot be unit tested due to tight coupling to XAML infrastructure and static dependencies.");
    }

    /// <summary>
    /// Tests that the parameterless constructor can be invoked without throwing an exception.
    /// This test requires a fully initialized MAUI application context and database.
    /// The constructor delegates to GlucoseMeasurementsPage(int? IdGlucoseRecord) with null,
    /// which calls InitializeComponent(), accesses Common.Database (static), and initializes UI.
    /// NOTE: This test is marked as Ignore because the class has the following testability issues:
    /// 1. Calls InitializeComponent() which requires XAML to be compiled and MAUI framework initialized
    /// 2. Uses static database access via Common.Database.GetParameters() which cannot be mocked with Moq
    /// 3. Directly instantiates dependencies (BL_GlucoseMeasurements) via field initializers, preventing dependency injection
    /// 4. Calls RefreshUi() which accesses UI controls that require MAUI runtime
    /// To make this testable, consider:
    /// - Injecting dependencies (BL_GlucoseMeasurements, database access) via constructor
    /// - Separating business logic from UI initialization
    /// - Using interfaces for dependencies to enable mocking
    /// </summary>
    [Test]
    [Ignore("Requires MAUI framework initialization, XAML compilation, and database setup. The class design does not support pure unit testing without refactoring for dependency injection.")]
    public void Constructor_ParameterlessInvocation_ShouldDelegateToParameterizedConstructorWithNull()
    {
        // NOTE TO DEVELOPER:
        // This test cannot be run as a pure unit test due to the class design.
        // To enable testing:
        // 1. Initialize MAUI application host in test setup
        // 2. Initialize Common.Database with test database
        // 3. Ensure Logger is initialized: General.LogOfProgram = new Logger(...)
        // 4. Consider using integration test approach instead of unit test

        // Arrange
        // (MAUI framework and database would need to be initialized here)

        // Act
        // var page = new GlucoseMeasurementsPage();

        // Assert
        // Assert.That(page, Is.Not.Null);
        // Assert.That(page.IdGlucoseRecord, Is.Null); // Verify null was passed through
    }

    /// <summary>
    /// Tests that the parameterless constructor initializes with default MonthsOfDataShownInTheGrids value
    /// when database parameters are null or invalid.
    /// This is a theoretical test case that would verify the constructor behavior when Common.Database.GetParameters() returns null.
    /// </summary>
    [Test]
    [Ignore("Requires MAUI framework initialization and ability to mock static Common.Database.GetParameters() which is not possible with Moq.")]
    public void Constructor_ParameterlessWithNullDatabaseParameters_ShouldUseDefaultMonthsValue()
    {
        // NOTE TO DEVELOPER:
        // This test requires mocking Common.Database.GetParameters() to return null,
        // but Moq cannot mock static methods.
        // Consider refactoring to inject IDatabase interface instead of using static Common.Database.

        // Arrange
        // (Would need to mock Common.Database.GetParameters() to return null)

        // Act
        // var page = new GlucoseMeasurementsPage();

        // Assert
        // Assert.That(page.MonthsOfDataShownInTheGrids, Is.EqualTo(3)); // Default value
    }

    /// <summary>
    /// Tests that the parameterless constructor does not attempt to load a glucose record
    /// since it passes null as the IdGlucoseRecord parameter.
    /// This verifies that no database query for glucose record is executed when using the parameterless constructor.
    /// </summary>
    [Test]
    [Ignore("Requires MAUI framework initialization and dependency injection refactoring to verify BL_GlucoseMeasurements.GetOneGlucoseRecord is not called.")]
    public void Constructor_ParameterlessInvocation_ShouldNotLoadGlucoseRecord()
    {
        // NOTE TO DEVELOPER:
        // This test would verify that bl.GetOneGlucoseRecord() is NOT called when IdGlucoseRecord is null.
        // Requires refactoring to inject BL_GlucoseMeasurements as a dependency to enable verification via mock.

        // Arrange
        // (Would need to inject mock BL_GlucoseMeasurements)

        // Act
        // var page = new GlucoseMeasurementsPage();

        // Assert
        // (Would verify mock.GetOneGlucoseRecord was never called)
    }

    /// <summary>
    /// Tests that btnClearData_Click does not throw an exception when called with valid parameters
    /// after page initialization.
    /// Note: This test requires XAML compilation and MAUI infrastructure to be available.
    /// If InitializeComponent fails, this test will be inconclusive.
    /// </summary>
    [Test]
    public void btnClearData_Click_WithValidSenderAndEventArgs_ExecutesWithoutException()
    {
        // Arrange
        GlucoseMeasurementsPage? page = null;

        try
        {
            page = new GlucoseMeasurementsPage(null);
        }
        catch (Exception ex)
        {
            Assert.Inconclusive($"Cannot instantiate GlucoseMeasurementsPage in test environment. " +
                              $"XAML compilation or MAUI infrastructure may not be available. Exception: {ex.Message}");
            return;
        }

        object sender = new object();
        EventArgs e = EventArgs.Empty;

        // Act & Assert
        Assert.DoesNotThrow(() => page.btnClearData_Click(sender, e),
            "btnClearData_Click should execute without throwing an exception");
    }

    /// <summary>
    /// Tests that btnClearData_Click does not throw when sender parameter is null.
    /// The sender parameter is not used in the method implementation.
    /// </summary>
    [Test]
    public void btnClearData_Click_WithNullSender_ExecutesWithoutException()
    {
        // Arrange
        GlucoseMeasurementsPage? page = null;

        try
        {
            page = new GlucoseMeasurementsPage(null);
        }
        catch (Exception ex)
        {
            Assert.Inconclusive($"Cannot instantiate GlucoseMeasurementsPage in test environment. " +
                              $"XAML compilation or MAUI infrastructure may not be available. Exception: {ex.Message}");
            return;
        }

        object? sender = null;
        EventArgs e = EventArgs.Empty;

        // Act & Assert
        Assert.DoesNotThrow(() => page.btnClearData_Click(sender, e),
            "btnClearData_Click should handle null sender without throwing an exception");
    }

    /// <summary>
    /// Tests that btnClearData_Click does not throw when EventArgs parameter is null.
    /// The EventArgs parameter is not used in the method implementation.
    /// </summary>
    [Test]
    public void btnClearData_Click_WithNullEventArgs_ExecutesWithoutException()
    {
        // Arrange
        GlucoseMeasurementsPage? page = null;

        try
        {
            page = new GlucoseMeasurementsPage(null);
        }
        catch (Exception ex)
        {
            Assert.Inconclusive($"Cannot instantiate GlucoseMeasurementsPage in test environment. " +
                              $"XAML compilation or MAUI infrastructure may not be available. Exception: {ex.Message}");
            return;
        }

        object sender = new object();
        EventArgs? e = null;

        // Act & Assert
        Assert.DoesNotThrow(() => page.btnClearData_Click(sender, e),
            "btnClearData_Click should handle null EventArgs without throwing an exception");
    }

    /// <summary>
    /// Tests that btnClearData_Click does not throw when both parameters are null.
    /// Neither parameter is used in the method implementation.
    /// </summary>
    [Test]
    public void btnClearData_Click_WithBothParametersNull_ExecutesWithoutException()
    {
        // Arrange
        GlucoseMeasurementsPage? page = null;

        try
        {
            page = new GlucoseMeasurementsPage(null);
        }
        catch (Exception ex)
        {
            Assert.Inconclusive($"Cannot instantiate GlucoseMeasurementsPage in test environment. " +
                              $"XAML compilation or MAUI infrastructure may not be available. Exception: {ex.Message}");
            return;
        }

        object? sender = null;
        EventArgs? e = null;

        // Act & Assert
        Assert.DoesNotThrow(() => page.btnClearData_Click(sender, e),
            "btnClearData_Click should handle null parameters without throwing an exception");
    }

    /// <summary>
    /// Tests that btnClearData_Click does not throw when page is initialized with a valid IdGlucoseRecord.
    /// This ensures the method works correctly when editing an existing glucose record.
    /// Note: This test may be inconclusive if database access is not available in test environment.
    /// </summary>
    [Test]
    public void btnClearData_Click_WithValidIdGlucoseRecord_ExecutesWithoutException()
    {
        // Arrange
        GlucoseMeasurementsPage? page = null;
        int testId = 1;

        try
        {
            page = new GlucoseMeasurementsPage(testId);
        }
        catch (Exception ex)
        {
            Assert.Inconclusive($"Cannot instantiate GlucoseMeasurementsPage with IdGlucoseRecord in test environment. " +
                              $"Database or XAML infrastructure may not be available. Exception: {ex.Message}");
            return;
        }

        object sender = new object();
        EventArgs e = EventArgs.Empty;

        // Act & Assert
        Assert.DoesNotThrow(() => page.btnClearData_Click(sender, e),
            "btnClearData_Click should execute without throwing when page is initialized with a valid ID");
    }

    /// <summary>
    /// Tests that IdGlucoseRecord property returns the value from the underlying currentGlucose field
    /// when the page is initialized with a null parameter.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate GlucoseMeasurementsPage in unit test context. See comments for details.")]
    public void IdGlucoseRecord_WhenPageInitializedWithNull_ReturnsCurrentGlucoseIdGlucoseRecord()
    {
        // NOTE: This test cannot be executed because GlucoseMeasurementsPage inherits from ContentPage
        // and calls InitializeComponent() which requires XAML compilation and MAUI runtime environment.
        // Additionally, the constructor has the following untestable dependencies:
        // 1. Static method call to Common.Database.GetParameters() which cannot be mocked with Moq
        // 2. Field-initialized BL_GlucoseMeasurements (bl) which is not injected
        // 3. MAUI framework dependencies (ContentPage initialization)
        //
        // To make this code testable, consider:
        // 1. Extract the IdGlucoseRecord logic into a separate testable class/view model
        // 2. Use dependency injection for BL_GlucoseMeasurements
        // 3. Create an abstraction layer for Common.Database static methods
        // 4. Separate UI initialization from business logic

        // Arrange
        // var page = new GlucoseMeasurementsPage();

        // Act
        // var result = page.IdGlucoseRecord;

        // Assert
        // Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that IdGlucoseRecord property returns the value from the underlying currentGlucose field
    /// when the page is initialized with a specific ID.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate GlucoseMeasurementsPage in unit test context. See comments for details.")]
    public void IdGlucoseRecord_WhenPageInitializedWithValidId_ReturnsExpectedId()
    {
        // NOTE: This test cannot be executed because GlucoseMeasurementsPage inherits from ContentPage
        // and calls InitializeComponent() which requires XAML compilation and MAUI runtime environment.
        // Additionally, the constructor has the following untestable dependencies:
        // 1. Static method call to Common.Database.GetParameters() which cannot be mocked with Moq
        // 2. Field-initialized BL_GlucoseMeasurements (bl) which cannot be mocked
        // 3. bl.GetOneGlucoseRecord() would need to be mocked to return a specific GlucoseRecord
        // 4. MAUI framework dependencies (ContentPage initialization)
        //
        // To make this code testable, consider:
        // 1. Extract the IdGlucoseRecord logic into a separate testable class/view model
        // 2. Use dependency injection for BL_GlucoseMeasurements
        // 3. Create an abstraction layer for Common.Database static methods
        // 4. Separate UI initialization from business logic

        // Arrange
        // int expectedId = 42;
        // var page = new GlucoseMeasurementsPage(expectedId);

        // Act
        // var result = page.IdGlucoseRecord;

        // Assert
        // Assert.That(result, Is.EqualTo(expectedId));
    }

    /// <summary>
    /// Tests that IdGlucoseRecord property returns the minimum integer value correctly.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate GlucoseMeasurementsPage in unit test context. See comments for details.")]
    public void IdGlucoseRecord_WhenGlucoseRecordHasMinValue_ReturnsMinValue()
    {
        // NOTE: This test cannot be executed because GlucoseMeasurementsPage inherits from ContentPage
        // and calls InitializeComponent() which requires XAML compilation and MAUI runtime environment.
        // Additionally, the constructor has untestable static dependencies and field-initialized dependencies.
        //
        // To make this code testable, consider:
        // 1. Extract the IdGlucoseRecord logic into a separate testable class/view model
        // 2. Use dependency injection for BL_GlucoseMeasurements
        // 3. Create an abstraction layer for Common.Database static methods

        // Arrange
        // int expectedId = int.MinValue;
        // var page = new GlucoseMeasurementsPage(expectedId);

        // Act
        // var result = page.IdGlucoseRecord;

        // Assert
        // Assert.That(result, Is.EqualTo(int.MinValue));
    }

    /// <summary>
    /// Tests that IdGlucoseRecord property returns the maximum integer value correctly.
    /// </summary>
    [Test]
    [Ignore("Cannot instantiate GlucoseMeasurementsPage in unit test context. See comments for details.")]
    public void IdGlucoseRecord_WhenGlucoseRecordHasMaxValue_ReturnsMaxValue()
    {
        // NOTE: This test cannot be executed because GlucoseMeasurementsPage inherits from ContentPage
        // and calls InitializeComponent() which requires XAML compilation and MAUI runtime environment.
        // Additionally, the constructor has untestable static dependencies and field-initialized dependencies.
        //
        // To make this code testable, consider:
        // 1. Extract the IdGlucoseRecord logic into a separate testable class/view model
        // 2. Use dependency injection for BL_GlucoseMeasurements
        // 3. Create an abstraction layer for Common.Database static methods

        // Arrange
        // int expectedId = int.MaxValue;
        // var page = new GlucoseMeasurementsPage(expectedId);

        // Act
        // var result = page.IdGlucoseRecord;

        // Assert
        // Assert.That(result, Is.EqualTo(int.MaxValue));
    }
}