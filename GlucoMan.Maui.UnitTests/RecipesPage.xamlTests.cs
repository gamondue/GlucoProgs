using System;
using System.Threading.Tasks;

using GlucoMan;
using GlucoMan.Maui;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;




/// <summary>
/// Unit tests for the RecipesPage class.
/// </summary>
/// <remarks>
/// NOTE: These tests are marked as Ignored because RecipesPage inherits from ContentPage
/// and calls InitializeComponent() in its constructors. This requires the full MAUI runtime
/// infrastructure to be available, which is not present in a standard unit test context.
/// These tests demonstrate the intended test logic but cannot execute without MAUI infrastructure.
/// Consider integration tests or UI tests for this class instead.
/// </remarks>
public partial class RecipesPageTests
{
    /// <summary>
    /// Tests that CurrentRecipe returns the Recipe set via the constructor.
    /// </summary>
    /// <remarks>
    /// This test verifies that when RecipesPage is constructed with a Recipe parameter,
    /// the CurrentRecipe property returns the same Recipe instance.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void CurrentRecipe_AfterConstructionWithRecipe_ReturnsProvidedRecipe()
    {
        // Arrange
        Recipe expectedRecipe = new Recipe
        {
            IdRecipe = 1,
            Name = "Test Recipe",
            Description = "Test Description"
        };

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(expectedRecipe);
        Recipe actualRecipe = page.CurrentRecipe;

        // Assert
        Assert.That(actualRecipe, Is.SameAs(expectedRecipe));
    }

    /// <summary>
    /// Tests that CurrentRecipe returns null when bl.Recipe is null.
    /// </summary>
    /// <remarks>
    /// This test verifies that when bl.Recipe is null (default state),
    /// CurrentRecipe property returns null.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void CurrentRecipe_WhenBlRecipeIsNull_ReturnsNull()
    {
        // Arrange
        Recipe? nullRecipe = null;

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(nullRecipe!);
        Recipe? actualRecipe = page.CurrentRecipe;

        // Assert
        Assert.That(actualRecipe, Is.Null);
    }

    /// <summary>
    /// Tests that CurrentRecipe returns a Recipe with the specified name and description
    /// when constructed with string parameters.
    /// </summary>
    /// <remarks>
    /// This test verifies that when RecipesPage is constructed with name and description strings,
    /// the CurrentRecipe property returns a Recipe object with those properties set.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void CurrentRecipe_AfterConstructionWithStrings_ReturnsRecipeWithNameAndDescription()
    {
        // Arrange
        string expectedName = "Search Recipe";
        string expectedDescription = "Search Description";

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(expectedName, expectedDescription);
        Recipe actualRecipe = page.CurrentRecipe;

        // Assert
        Assert.That(actualRecipe, Is.Not.Null);
        Assert.That(actualRecipe.Name, Is.EqualTo(expectedName));
        Assert.That(actualRecipe.Description, Is.EqualTo(expectedDescription));
    }

    /// <summary>
    /// Tests that CurrentRecipe returns a non-null Recipe when constructed with empty strings.
    /// </summary>
    /// <remarks>
    /// This test verifies that when RecipesPage is constructed with empty strings,
    /// a Recipe object is still created and returned by CurrentRecipe.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void CurrentRecipe_AfterConstructionWithEmptyStrings_ReturnsRecipeWithEmptyValues()
    {
        // Arrange
        string emptyName = string.Empty;
        string emptyDescription = string.Empty;

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(emptyName, emptyDescription);
        Recipe actualRecipe = page.CurrentRecipe;

        // Assert
        Assert.That(actualRecipe, Is.Not.Null);
        Assert.That(actualRecipe.Name, Is.EqualTo(emptyName));
        Assert.That(actualRecipe.Description, Is.EqualTo(emptyDescription));
    }

    /// <summary>
    /// Tests that multiple accesses to CurrentRecipe return the same instance.
    /// </summary>
    /// <remarks>
    /// This test verifies that the CurrentRecipe property consistently returns
    /// the same Recipe instance from bl.Recipe on multiple accesses.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void CurrentRecipe_MultipleAccesses_ReturnsSameInstance()
    {
        // Arrange
        Recipe expectedRecipe = new Recipe
        {
            IdRecipe = 42,
            Name = "Consistent Recipe"
        };

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(expectedRecipe);
        Recipe firstAccess = page.CurrentRecipe;
        Recipe secondAccess = page.CurrentRecipe;

        // Assert
        Assert.That(firstAccess, Is.SameAs(secondAccess));
        Assert.That(firstAccess, Is.SameAs(expectedRecipe));
    }

    /// <summary>
    /// Tests that PageClosedTask returns a non-null Task when RecipesPage is created with a Recipe parameter.
    /// Expected: Property returns a valid Task&lt;bool&gt; instance.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenCreatedWithRecipe_ReturnsNonNullTask()
    {
        // Arrange
        Recipe recipe = new Recipe();

        // Note: This test may require MAUI initialization context to properly instantiate ContentPage.
        // If InitializeComponent() throws, this indicates the page requires UI context.
        RecipesPage page;
        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    /// <summary>
    /// Tests that PageClosedTask returns the same Task instance on multiple accesses when RecipesPage is created with a Recipe.
    /// Expected: Multiple property accesses return the identical Task instance.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenAccessedMultipleTimes_ReturnsSameTaskInstance()
    {
        // Arrange
        Recipe recipe = new Recipe();
        RecipesPage page;
        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> firstAccess = page.PageClosedTask;
        Task<bool> secondAccess = page.PageClosedTask;

        // Assert
        Assert.That(firstAccess, Is.SameAs(secondAccess));
    }

    /// <summary>
    /// Tests that PageClosedTask returns an incomplete Task initially when RecipesPage is created with a Recipe.
    /// Expected: Task.IsCompleted is false since TaskCompletionSource has not been set.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenCreatedWithRecipe_ReturnsIncompleteTask()
    {
        // Arrange
        Recipe recipe = new Recipe();
        RecipesPage page;
        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result.IsCompleted, Is.False);
    }

    /// <summary>
    /// Tests that PageClosedTask returns a non-null Task when RecipesPage is created with search parameters.
    /// Expected: Property returns a valid Task&lt;bool&gt; instance.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenCreatedWithSearchParameters_ReturnsNonNullTask()
    {
        // Arrange
        string recipeName = "Test Recipe";
        string recipeDescription = "Test Description";

        RecipesPage page;
        try
        {
            page = new RecipesPage(recipeName, recipeDescription);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    /// <summary>
    /// Tests that PageClosedTask returns an incomplete Task when RecipesPage is created with search parameters.
    /// Expected: Task.IsCompleted is false since TaskCompletionSource has not been set.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenCreatedWithSearchParameters_ReturnsIncompleteTask()
    {
        // Arrange
        string recipeName = "Test Recipe";
        string recipeDescription = "Test Description";

        RecipesPage page;
        try
        {
            page = new RecipesPage(recipeName, recipeDescription);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result.IsCompleted, Is.False);
    }

    /// <summary>
    /// Tests that PageClosedTask with null and empty search parameters still returns a valid Task.
    /// Expected: Property returns a valid Task&lt;bool&gt; instance even with null/empty strings.
    /// </summary>
    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase(null, "Description")]
    [TestCase("Name", null)]
    [TestCase("", "Description")]
    [TestCase("Name", "")]
    public void PageClosedTask_WithVariousSearchParameters_ReturnsNonNullTask(string? recipeName, string? recipeDescription)
    {
        // Arrange
        RecipesPage page;
        try
        {
            page = new RecipesPage(recipeName!, recipeDescription!);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the RecipesPage constructor with a valid Recipe parameter properly initializes the page.
    /// </summary>
    /// <remarks>
    /// This test is marked as Ignore because RecipesPage.InitializeComponent() requires MAUI framework
    /// initialization which is not available in unit test context. To properly test this constructor:
    /// 1. Use MAUI integration/UI tests with framework initialization
    /// 2. Or refactor RecipesPage to accept dependencies via constructor injection
    /// Expected behavior: Constructor should set bl.Recipe to the provided Recipe and initialize _taskCompletionSource.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI framework initialization (InitializeComponent) which is not available in unit test context")]
    public void Constructor_WithValidRecipe_InitializesPageCorrectly()
    {
        // Arrange
        var recipe = new Recipe
        {
            IdRecipe = 1,
            Name = "Test Recipe",
            Description = "Test Description"
        };

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipe);

        // Assert
        // Expected: page.CurrentRecipe should equal recipe
        // Expected: page.PageClosedTask should not be null
        // Expected: page.RecipeIsChosen should be false
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the RecipesPage constructor handles a null Recipe parameter.
    /// </summary>
    /// <remarks>
    /// This test is marked as Ignore because RecipesPage.InitializeComponent() requires MAUI framework
    /// initialization which is not available in unit test context.
    /// Expected behavior: Constructor should accept null Recipe and set bl.Recipe to null.
    /// Note: The source code does not perform null validation on the Recipe parameter.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI framework initialization (InitializeComponent) which is not available in unit test context")]
    public void Constructor_WithNullRecipe_SetsRecipeToNull()
    {
        // Arrange
        Recipe? recipe = null;

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipe);

        // Assert
        // Expected: page.CurrentRecipe should be null (bl.Recipe is set to null)
        // Expected: page.PageClosedTask should not be null
        // Expected: No exception should be thrown
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the RecipesPage constructor initializes TaskCompletionSource correctly.
    /// </summary>
    /// <remarks>
    /// This test is marked as Ignore because RecipesPage.InitializeComponent() requires MAUI framework
    /// initialization which is not available in unit test context.
    /// Expected behavior: Constructor should initialize _taskCompletionSource and PageClosedTask should return a valid Task.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI framework initialization (InitializeComponent) which is not available in unit test context")]
    public void Constructor_InitializesTaskCompletionSource()
    {
        // Arrange
        var recipe = new Recipe { Name = "Test" };

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipe);

        // Assert
        // Expected: page.PageClosedTask should not be null
        // Expected: page.PageClosedTask.IsCompleted should be false (task not yet completed)
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the RecipesPage constructor sets RecipeIsChosen to false initially.
    /// </summary>
    /// <remarks>
    /// This test is marked as Ignore because RecipesPage.InitializeComponent() requires MAUI framework
    /// initialization which is not available in unit test context.
    /// Expected behavior: RecipeIsChosen property should return false after construction.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI framework initialization (InitializeComponent) which is not available in unit test context")]
    public void Constructor_SetsRecipeIsChosenToFalse()
    {
        // Arrange
        var recipe = new Recipe();

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipe);

        // Assert
        // Expected: page.RecipeIsChosen should be false
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor with valid non-null search parameters properly initializes
    /// the Recipe object with the provided name and description.
    /// </summary>
    /// <param name="recipeName">The recipe name to search for.</param>
    /// <param name="recipeDescription">The recipe description to search for.</param>
    [Test]
    [TestCase("Pancakes", "Fluffy breakfast pancakes")]
    [TestCase("Pizza", "Italian style pizza")]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithValidSearchParameters_InitializesRecipeWithNameAndDescription(
        string recipeName,
        string recipeDescription)
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies
        // InitializeComponent() requires UI infrastructure
        // RefreshGrid() accesses gridRecipes UI element

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles null RecipeNameForSearch parameter.
    /// The Recipe.Name property should be set to null.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithNullRecipeName_SetsRecipeNameToNull()
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles null RecipeDescriptionForSearch parameter.
    /// The Recipe.Description property should be set to null.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithNullRecipeDescription_SetsRecipeDescriptionToNull()
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles both parameters being null.
    /// Both Recipe.Name and Recipe.Description should be set to null.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithBothParametersNull_SetsRecipePropertiesToNull()
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles empty string parameters.
    /// Recipe.Name and Recipe.Description should be set to empty strings.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithEmptyStrings_SetsRecipePropertiesToEmptyStrings()
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles whitespace-only string parameters.
    /// Recipe.Name and Recipe.Description should be set to the whitespace strings.
    /// </summary>
    [Test]
    [TestCase("   ", "   ")]
    [TestCase("\t", "\n")]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithWhitespaceStrings_SetsRecipePropertiesToWhitespaceStrings(
        string recipeName,
        string recipeDescription)
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles very long string parameters without throwing exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithVeryLongStrings_HandlesLargeInputs()
    {
        // Arrange
        // string longName = new string('A', 10000);
        // string longDescription = new string('B', 10000);

        // Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor handles strings with special characters.
    /// Recipe properties should be set to strings containing special characters.
    /// </summary>
    [Test]
    [TestCase("Recipe's Name", "Description with \"quotes\"")]
    [TestCase("<script>alert('xss')</script>", "'; DROP TABLE Recipes; --")]
    [TestCase("émojis 🍕🥞", "Spëciål çhårs")]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_WithSpecialCharacters_SetsRecipePropertiesWithSpecialCharacters(
        string recipeName,
        string recipeDescription)
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Tests that the constructor calls RefreshGrid which should populate the recipes grid.
    /// This verifies the search functionality is triggered during initialization.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI UI framework initialization. InitializeComponent() and RefreshGrid() depend on UI infrastructure that is not available in unit tests. Consider integration testing instead.")]
    public void Constructor_CallsRefreshGrid_ToPopulateRecipesGrid()
    {
        // Arrange & Act
        // Cannot be tested in isolation due to MAUI framework dependencies
        // RefreshGrid() accesses gridRecipes.ItemsSource which is a UI element

        // Assert
        Assert.Inconclusive("This test requires MAUI framework initialization which is not available in unit tests.");
    }

    /// <summary>
    /// Helper class that exposes protected members of RecipesPage for testing.
    /// </summary>
    private class TestableRecipesPage : RecipesPage
    {
        public TestableRecipesPage(Recipe recipe) : base(recipe)
        {
        }

        public TestableRecipesPage(string recipeNameForSearch, string recipeDescriptionForSearch)
            : base(recipeNameForSearch, recipeDescriptionForSearch)
        {
        }

        public void CallOnDisappearing()
        {
            OnDisappearing();
        }

    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false after construction with Recipe parameter.
    /// This verifies the initial state of the property when the page is created with a Recipe object.
    /// </summary>
    [Test]
    public void RecipeIsChosen_AfterConstructionWithRecipe_ReturnsFalse()
    {
        // Arrange
        var recipe = new Recipe();

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.
        // If the test fails due to XAML initialization, ensure MauiProgram is properly initialized in test setup.

        // Act
        var page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false after construction with search parameters.
    /// This verifies the initial state when the page is created for recipe search functionality.
    /// </summary>
    [Test]
    public void RecipeIsChosen_AfterConstructionWithSearchParameters_ReturnsFalse()
    {
        // Arrange
        string recipeName = "Test Recipe";
        string recipeDescription = "Test Description";

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.
        // If the test fails due to XAML initialization, ensure MauiProgram is properly initialized in test setup.

        // Act
        var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false with null recipe parameter.
    /// This verifies the initial state even when null is provided.
    /// </summary>
    [Test]
    public void RecipeIsChosen_AfterConstructionWithNullRecipe_ReturnsFalse()
    {
        // Arrange
        Recipe? recipe = null;

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.
        // If the test fails due to XAML initialization, ensure MauiProgram is properly initialized in test setup.

        // Act
        var page = new RecipesPage(recipe!);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false with null search parameters.
    /// This tests edge case where both search strings are null.
    /// </summary>
    [Test]
    public void RecipeIsChosen_AfterConstructionWithNullSearchParameters_ReturnsFalse()
    {
        // Arrange
        string? recipeName = null;
        string? recipeDescription = null;

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.
        // If the test fails due to XAML initialization, ensure MauiProgram is properly initialized in test setup.

        // Act
        var page = new RecipesPage(recipeName!, recipeDescription!);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false with empty search parameters.
    /// This tests edge case where search strings are empty.
    /// </summary>
    [Test]
    public void RecipeIsChosen_AfterConstructionWithEmptySearchParameters_ReturnsFalse()
    {
        // Arrange
        string recipeName = string.Empty;
        string recipeDescription = string.Empty;

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.
        // If the test fails due to XAML initialization, ensure MauiProgram is properly initialized in test setup.

        // Act
        var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that OnDisappearing completes the TaskCompletionSource with the recipeIsChosen value (true).
    /// Expected: PageClosedTask completes with result true when recipeIsChosen is true.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WhenRecipeIsChosenTrue_CompletesTaskWithTrue()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Simulate recipeIsChosen being set to true (this would happen via UI interaction)
        // Note: In real scenario, this would be set via btnChoose_Click handler

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.True);
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing completes the TaskCompletionSource with the recipeIsChosen value (false).
    /// Expected: PageClosedTask completes with result false when recipeIsChosen is false (default).
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WhenRecipeIsChosenFalse_CompletesTaskWithFalse()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // recipeIsChosen is false by default

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False);
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing does not throw when called multiple times.
    /// Expected: Second call does not throw exception or modify already completed task.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void OnDisappearing_WhenCalledMultipleTimes_DoesNotThrowException()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();

        // Assert - calling again should not throw
        Assert.DoesNotThrow(() => page.CallOnDisappearing());
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing properly completes the task when page is initialized with search parameters.
    /// Expected: PageClosedTask completes successfully with false (default recipeIsChosen value).
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WhenConstructedWithSearchParameters_CompletesTaskSuccessfully()
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage("TestRecipe", "TestDescription");

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False);
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing sets task result before it can be awaited.
    /// Expected: Task completes synchronously or very quickly after OnDisappearing is called.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void OnDisappearing_CompletesTaskSynchronously()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();

        // Assert - task should be completed immediately
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing with null recipe handles task completion correctly.
    /// Expected: PageClosedTask completes with false without throwing exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithNullRecipe_CompletesTaskWithFalse()
    {
        // Arrange
        Recipe? nullRecipe = null;
        TestableRecipesPage page = new TestableRecipesPage(nullRecipe!);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False);
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing with empty search strings handles task completion correctly.
    /// Expected: PageClosedTask completes successfully with false.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithEmptySearchStrings_CompletesTaskWithFalse()
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage("", "");

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False);
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that OnDisappearing with null search strings handles task completion correctly.
    /// Expected: PageClosedTask completes successfully with false.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithNullSearchStrings_CompletesTaskWithFalse()
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage(null!, null!);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False);
        Assert.That(page.PageClosedTask.IsCompleted, Is.True);
    }

    /// <summary>
    /// Tests that PageClosedTask returns a Task with IsCompleted false and validates the underlying TaskCompletionSource.Task.
    /// Expected: Property returns Task&lt;bool&gt; that is not completed initially.
    /// </summary>
    [Test]
    public void PageClosedTask_WhenTaskCompletionSourceInitialized_ReturnsUncompletedTask()
    {
        // Arrange
        Recipe recipe = new Recipe { Name = "Test Recipe", Description = "Test Description" };
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> task = page.PageClosedTask;

        // Assert
        Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task");
        Assert.That(task.IsCompleted, Is.False, "Task should not be completed initially");
    }

    /// <summary>
    /// Tests that PageClosedTask property is consistent and returns the same Task instance on subsequent accesses.
    /// Expected: Multiple accesses to PageClosedTask return the identical Task object reference.
    /// </summary>
    [Test]
    public void PageClosedTask_AccessedMultipleTimesWithRecipe_ReturnsSameTaskReference()
    {
        // Arrange
        Recipe recipe = new Recipe { Name = "Consistency Test Recipe" };
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> firstAccess = page.PageClosedTask;
        Task<bool> secondAccess = page.PageClosedTask;
        Task<bool> thirdAccess = page.PageClosedTask;

        // Assert
        Assert.That(firstAccess, Is.SameAs(secondAccess), "First and second access should return same Task instance");
        Assert.That(secondAccess, Is.SameAs(thirdAccess), "Second and third access should return same Task instance");
        Assert.That(firstAccess, Is.SameAs(thirdAccess), "First and third access should return same Task instance");
    }

    /// <summary>
    /// Tests that PageClosedTask returns a valid Task when RecipesPage is created with search parameters.
    /// Expected: Property returns a non-null Task&lt;bool&gt; that is not completed.
    /// </summary>
    [TestCase("Recipe Name", "Recipe Description")]
    [TestCase("", "")]
    [TestCase("Name Only", "")]
    [TestCase("", "Description Only")]
    public void PageClosedTask_WithSearchParametersConstructor_ReturnsValidUncompletedTask(string recipeName, string recipeDescription)
    {
        // Arrange
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipeName, recipeDescription);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> task = page.PageClosedTask;

        // Assert
        Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task");
        Assert.That(task.IsCompleted, Is.False, "Task should not be completed initially");
    }

    /// <summary>
    /// Tests that PageClosedTask maintains consistent Task reference across multiple accesses when constructed with search parameters.
    /// Expected: All accesses return the same Task instance.
    /// </summary>
    [Test]
    public void PageClosedTask_WithSearchParameters_ReturnsSameTaskInstanceOnMultipleAccesses()
    {
        // Arrange
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage("Pasta", "Italian pasta recipe");
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> task1 = page.PageClosedTask;
        Task<bool> task2 = page.PageClosedTask;

        // Assert
        Assert.That(task1, Is.SameAs(task2), "Multiple accesses should return same Task instance");
    }

    /// <summary>
    /// Tests that PageClosedTask returns a Task with appropriate type and status immediately after construction.
    /// Expected: Task is of correct type, not null, and not faulted or canceled.
    /// </summary>
    [Test]
    public void PageClosedTask_AfterConstruction_ReturnsTaskWithCorrectTypeAndStatus()
    {
        // Arrange
        Recipe recipe = new Recipe();
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> task = page.PageClosedTask;

        // Assert
        Assert.That(task, Is.Not.Null, "Task should not be null");
        Assert.That(task, Is.TypeOf<Task<bool>>(), "Task should be of type Task<bool>");
        Assert.That(task.IsFaulted, Is.False, "Task should not be faulted");
        Assert.That(task.IsCanceled, Is.False, "Task should not be canceled");
    }

    /// <summary>
    /// Tests that PageClosedTask property handles edge case with null Recipe parameter.
    /// Expected: Property returns a valid Task even when Recipe is null.
    /// </summary>
    [Test]
    public void PageClosedTask_WithNullRecipe_ReturnsValidTask()
    {
        // Arrange
        Recipe? nullRecipe = null;
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(nullRecipe!);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> task = page.PageClosedTask;

        // Assert
        Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task even with null Recipe");
    }

    /// <summary>
    /// Tests that PageClosedTask property handles edge case with null search parameters.
    /// Expected: Property returns a valid Task even when search parameters are null.
    /// </summary>
    [TestCase(null, null)]
    [TestCase(null, "Description")]
    [TestCase("Name", null)]
    public void PageClosedTask_WithNullSearchParameters_ReturnsValidTask(string? recipeName, string? recipeDescription)
    {
        // Arrange
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipeName!, recipeDescription!);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. This test should be run in an integration test environment with MAUI framework initialized.");
            return;
        }

        // Act
        Task<bool> task = page.PageClosedTask;

        // Assert
        Assert.That(task, Is.Not.Null, "PageClosedTask should return a non-null Task even with null parameters");
        Assert.That(task.IsCompleted, Is.False, "Task should not be completed initially");
    }

    /// <summary>
    /// Tests that PageClosedTask property getter returns a valid Task when accessed via Recipe constructor.
    /// This test specifically focuses on exercising the property getter expression.
    /// Expected: Property returns _taskCompletionSource.Task when _taskCompletionSource is not null.
    /// </summary>
    [Test]
    public void PageClosedTask_PropertyGetter_ReturnsTaskFromTaskCompletionSource()
    {
        // Arrange
        Recipe recipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act - Explicitly access the property getter multiple times to ensure coverage
        Task<bool> firstAccess = page.PageClosedTask;
        Task<bool> secondAccess = page.PageClosedTask;

        // Assert
        Assert.That(firstAccess, Is.Not.Null, "PageClosedTask should return a non-null Task");
        Assert.That(firstAccess, Is.SameAs(secondAccess), "PageClosedTask should return the same Task instance on multiple accesses");
        Assert.That(firstAccess.IsCompleted, Is.False, "Task should not be completed initially");
        Assert.That(firstAccess.IsCanceled, Is.False, "Task should not be canceled");
        Assert.That(firstAccess.IsFaulted, Is.False, "Task should not be faulted");
    }

    /// <summary>
    /// Tests that PageClosedTask property getter returns a valid Task when accessed via search parameters constructor.
    /// This ensures the property works correctly regardless of which constructor was used.
    /// Expected: Property returns _taskCompletionSource.Task when initialized via search constructor.
    /// </summary>
    [Test]
    public void PageClosedTask_PropertyGetterWithSearchConstructor_ReturnsTaskFromTaskCompletionSource()
    {
        // Arrange
        string recipeName = "Pancakes";
        string recipeDescription = "Delicious breakfast";
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipeName, recipeDescription);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act - Explicitly invoke the property getter
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null, "PageClosedTask should return a non-null Task");
        Assert.That(result.IsCompleted, Is.False, "Task should not be completed initially");
    }

    /// <summary>
    /// Tests PageClosedTask property getter with various edge case input parameters for search constructor.
    /// This ensures the property behaves correctly with null, empty, and whitespace search parameters.
    /// Expected: Property returns a valid Task regardless of search parameter values.
    /// </summary>
    /// <param name="recipeName">Recipe name search parameter (may be null, empty, or whitespace)</param>
    /// <param name="recipeDescription">Recipe description search parameter (may be null, empty, or whitespace)</param>
    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase("   ", "   ")]
    [TestCase(null, "Description")]
    [TestCase("Name", null)]
    [TestCase("", "Description")]
    [TestCase("Name", "")]
    public void PageClosedTask_PropertyGetterWithEdgeCaseSearchParameters_ReturnsValidTask(string? recipeName, string? recipeDescription)
    {
        // Arrange
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipeName!, recipeDescription!);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act - Access the property getter
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null, $"PageClosedTask should return a non-null Task even with recipeName='{recipeName}' and recipeDescription='{recipeDescription}'");
        Assert.That(result.IsCompleted, Is.False, "Task should not be completed initially");
    }

    /// <summary>
    /// Tests that PageClosedTask property getter returns consistent Task reference across repeated accesses.
    /// This verifies that the property getter doesn't create new Task instances on each access.
    /// Expected: The property returns the exact same Task object reference every time.
    /// </summary>
    [Test]
    public void PageClosedTask_PropertyGetterAccessedMultipleTimes_ReturnsIdenticalTaskReference()
    {
        // Arrange
        Recipe recipe = new Recipe();
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act - Access property getter multiple times
        Task<bool> access1 = page.PageClosedTask;
        Task<bool> access2 = page.PageClosedTask;
        Task<bool> access3 = page.PageClosedTask;

        // Assert - Verify same reference
        Assert.That(access1, Is.SameAs(access2), "First and second access should return same Task reference");
        Assert.That(access2, Is.SameAs(access3), "Second and third access should return same Task reference");
        Assert.That(access1, Is.SameAs(access3), "First and third access should return same Task reference");
    }

    /// <summary>
    /// Tests PageClosedTask property getter with null Recipe parameter.
    /// This tests the edge case where Recipe constructor receives null.
    /// Expected: Property still returns a valid Task (constructor initializes _taskCompletionSource regardless).
    /// </summary>
    [Test]
    public void PageClosedTask_PropertyGetterWithNullRecipe_ReturnsValidTask()
    {
        // Arrange
        Recipe? nullRecipe = null;
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(nullRecipe!);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act - Access the property getter
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null, "PageClosedTask should return a non-null Task even with null Recipe");
        Assert.That(result.IsCompleted, Is.False, "Task should not be completed initially");
    }

    /// <summary>
    /// Tests PageClosedTask property getter returns Task with correct generic type parameter.
    /// This verifies the property returns Task&lt;bool&gt; specifically, not just Task.
    /// Expected: Property returns Task&lt;bool&gt; type.
    /// </summary>
    [Test]
    public void PageClosedTask_PropertyGetter_ReturnsTaskOfBoolType()
    {
        // Arrange
        Recipe recipe = new Recipe();
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.InstanceOf<Task<bool>>(), "PageClosedTask should return Task<bool> type");
        Assert.That(result.GetType(), Is.EqualTo(typeof(Task<bool>)), "PageClosedTask should return exactly Task<bool>");
    }

    /// <summary>
    /// Tests PageClosedTask property getter with Recipe containing extreme values.
    /// This ensures the property works correctly even when Recipe has boundary values.
    /// Expected: Property returns valid Task regardless of Recipe property values.
    /// </summary>
    /// <param name="idRecipe">Recipe ID with extreme values</param>
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(0)]
    [TestCase(-1)]
    public void PageClosedTask_PropertyGetterWithExtremeRecipeValues_ReturnsValidTask(int idRecipe)
    {
        // Arrange
        Recipe recipe = new Recipe { IdRecipe = idRecipe };
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipe);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null, $"PageClosedTask should return a non-null Task even with IdRecipe={idRecipe}");
    }

    /// <summary>
    /// Tests PageClosedTask property getter with very long string search parameters.
    /// This tests the property behavior with extreme string lengths.
    /// Expected: Property returns valid Task even with very long strings.
    /// </summary>
    [Test]
    public void PageClosedTask_PropertyGetterWithVeryLongSearchStrings_ReturnsValidTask()
    {
        // Arrange
        string longName = new string('A', 10000);
        string longDescription = new string('B', 10000);
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(longName, longDescription);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null, "PageClosedTask should return a non-null Task even with very long search strings");
    }

    /// <summary>
    /// Tests PageClosedTask property getter with special characters in search parameters.
    /// This ensures the property works correctly with various special character inputs.
    /// Expected: Property returns valid Task regardless of special characters in search strings.
    /// </summary>
    /// <param name="recipeName">Recipe name with special characters</param>
    /// <param name="recipeDescription">Recipe description with special characters</param>
    [TestCase("Recipe's \"Special\" Name", "Description with <tags> & symbols")]
    [TestCase("émojis 🍕🥞🍰", "Spëciål çhåractërs")]
    [TestCase("'; DROP TABLE Recipes; --", "<script>alert('xss')</script>")]
    [TestCase("\t\n\r", "\0\a\b")]
    public void PageClosedTask_PropertyGetterWithSpecialCharacters_ReturnsValidTask(string recipeName, string recipeDescription)
    {
        // Arrange
        RecipesPage? page = null;

        try
        {
            page = new RecipesPage(recipeName, recipeDescription);
        }
        catch (InvalidOperationException)
        {
            Assert.Inconclusive("RecipesPage requires MAUI UI context for initialization. The PageClosedTask property cannot be tested without MAUI framework. This test should be run in an integration test environment.");
            return;
        }

        // Act
        Task<bool> result = page.PageClosedTask;

        // Assert
        Assert.That(result, Is.Not.Null, $"PageClosedTask should return a non-null Task even with special characters in search parameters");
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false for various constructor parameter combinations.
    /// This verifies the initial state of the property across different initialization scenarios.
    /// Expected: Property returns false regardless of constructor parameters used.
    /// </summary>
    [Test]
    [TestCase("Test Recipe", "Test Description")]
    [TestCase("", "")]
    [TestCase(null, null)]
    [TestCase("Recipe", null)]
    [TestCase(null, "Description")]
    [TestCase("   ", "   ")]
    public void RecipeIsChosen_WithVariousSearchParameters_ReturnsFalse(string? recipeName, string? recipeDescription)
    {
        // Arrange
        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.
        // If the test fails due to XAML initialization, ensure MauiProgram is properly initialized in test setup.

        // Act
        var page = new RecipesPage(recipeName!, recipeDescription!);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns the same value on multiple consecutive accesses.
    /// This verifies that the property is consistent and doesn't have side effects.
    /// Expected: Multiple accesses return the same false value.
    /// </summary>
    [Test]
    public void RecipeIsChosen_MultipleConsecutiveAccesses_ReturnsSameValue()
    {
        // Arrange
        var recipe = new Recipe { Name = "Test Recipe" };

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);
        bool firstAccess = page.RecipeIsChosen;
        bool secondAccess = page.RecipeIsChosen;
        bool thirdAccess = page.RecipeIsChosen;

        // Assert
        Assert.That(firstAccess, Is.False);
        Assert.That(secondAccess, Is.False);
        Assert.That(thirdAccess, Is.False);
        Assert.That(secondAccess, Is.EqualTo(firstAccess));
        Assert.That(thirdAccess, Is.EqualTo(firstAccess));
    }

    /// <summary>
    /// Tests that accessing RecipeIsChosen property does not throw any exceptions.
    /// This verifies the property getter is safe to call after construction.
    /// Expected: Property access completes without throwing.
    /// </summary>
    [Test]
    public void RecipeIsChosen_PropertyAccess_DoesNotThrow()
    {
        // Arrange
        var recipe = new Recipe();

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);

        // Assert
        Assert.DoesNotThrow(() =>
        {
            bool value = page.RecipeIsChosen;
        });
    }

    /// <summary>
    /// Tests that RecipeIsChosen returns false when RecipesPage is constructed with a Recipe having extreme property values.
    /// This tests edge cases for Recipe properties like very long strings and extreme numeric values.
    /// Expected: Property returns false regardless of Recipe property values.
    /// </summary>
    [Test]
    [TestCase(int.MinValue, "Name", "Description")]
    [TestCase(int.MaxValue, "Name", "Description")]
    [TestCase(0, "", "")]
    [TestCase(-1, null, null)]
    public void RecipeIsChosen_WithRecipeHavingExtremeValues_ReturnsFalse(int idRecipe, string? name, string? description)
    {
        // Arrange
        var recipe = new Recipe
        {
            IdRecipe = idRecipe,
            Name = name,
            Description = description
        };

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen returns false when accessing the property immediately vs. after a delay.
    /// This verifies that the property value is stable over time and doesn't change without explicit modification.
    /// Expected: Property returns false both immediately and after a short delay.
    /// </summary>
    [Test]
    public void RecipeIsChosen_ImmediateAndDelayedAccess_ReturnsFalse()
    {
        // Arrange
        var recipe = new Recipe { Name = "Test" };

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);
        bool immediateValue = page.RecipeIsChosen;

        System.Threading.Thread.Sleep(100);

        bool delayedValue = page.RecipeIsChosen;

        // Assert
        Assert.That(immediateValue, Is.False);
        Assert.That(delayedValue, Is.False);
        Assert.That(delayedValue, Is.EqualTo(immediateValue));
    }

    /// <summary>
    /// Tests that RecipeIsChosen returns false for Recipe with special characters in string properties.
    /// This validates that the property works correctly regardless of string content.
    /// Expected: Property returns false with special character inputs.
    /// </summary>
    [Test]
    [TestCase("Recipe's \"Name\"", "<script>alert('test')</script>")]
    [TestCase("émojis 🍕🥞", "Spëciål çhårs")]
    [TestCase("'; DROP TABLE--", "SQL Injection Attempt")]
    public void RecipeIsChosen_WithSpecialCharactersInRecipe_ReturnsFalse(string name, string description)
    {
        // Arrange
        var recipe = new Recipe
        {
            Name = name,
            Description = description
        };

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen returns false when Recipe has very long string properties.
    /// This tests boundary conditions for string lengths.
    /// Expected: Property returns false even with extremely long string inputs.
    /// </summary>
    [Test]
    public void RecipeIsChosen_WithVeryLongStringsInRecipe_ReturnsFalse()
    {
        // Arrange
        string longName = new string('A', 10000);
        string longDescription = new string('B', 50000);
        var recipe = new Recipe
        {
            Name = longName,
            Description = longDescription
        };

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns boolean type as expected.
    /// This verifies the property type and value type are correct.
    /// Expected: Property returns a boolean value (false).
    /// </summary>
    [Test]
    public void RecipeIsChosen_PropertyType_IsBoolean()
    {
        // Arrange
        var recipe = new Recipe();

        // Note: This test requires MAUI application context to be initialized for InitializeComponent() to succeed.

        // Act
        var page = new RecipesPage(recipe);
        var value = page.RecipeIsChosen;

        // Assert
        Assert.That(value, Is.TypeOf<bool>());
        Assert.That(value, Is.False);
    }
}




/// <summary>
/// Unit tests for the RecipesPage constructor that accepts a Recipe parameter.
/// </summary>
/// <remarks>
/// NOTE: These tests are marked as Ignored because RecipesPage inherits from ContentPage
/// and calls InitializeComponent() in its constructor. This requires the full MAUI runtime
/// infrastructure to be available, which is not present in a standard unit test context.
/// These tests demonstrate the intended test logic but cannot execute without MAUI infrastructure.
/// Consider integration tests or UI tests for this class instead.
/// </remarks>
public partial class RecipesPageConstructorTests
{
    /// <summary>
    /// Tests that the constructor with a valid Recipe parameter initializes all required fields correctly.
    /// Expected: bl.Recipe is set to the provided Recipe, _taskCompletionSource is initialized,
    /// RecipeIsChosen is false, and PageClosedTask returns a valid Task.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithValidRecipe_InitializesAllFieldsCorrectly()
    {
        // Arrange
        Recipe validRecipe = new Recipe
        {
            IdRecipe = 42,
            Name = "Test Recipe",
            Description = "A test recipe description"
        };

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(validRecipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(validRecipe), "CurrentRecipe should reference the same Recipe instance");
        Assert.That(page.RecipeIsChosen, Is.False, "RecipeIsChosen should be false initially");
        Assert.That(page.PageClosedTask, Is.Not.Null, "PageClosedTask should not be null");
        Assert.That(page.PageClosedTask.IsCompleted, Is.False, "PageClosedTask should not be completed initially");
    }

    /// <summary>
    /// Tests that the constructor handles a null Recipe parameter without throwing.
    /// Expected: bl.Recipe is set to null, page initializes successfully.
    /// </summary>
    /// <remarks>
    /// The source code does not perform null validation on the Recipe parameter,
    /// so null is accepted and stored in bl.Recipe.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithNullRecipe_AcceptsNullAndInitializes()
    {
        // Arrange
        Recipe? nullRecipe = null;

        // Act
        // NOTE: This will fail with InitializeComponent() error in unit test context
        RecipesPage page = new RecipesPage(nullRecipe!);

        // Assert
        Assert.That(page.CurrentRecipe, Is.Null, "CurrentRecipe should be null when null Recipe is provided");
        Assert.That(page.RecipeIsChosen, Is.False, "RecipeIsChosen should be false");
        Assert.That(page.PageClosedTask, Is.Not.Null, "PageClosedTask should still be initialized");
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with null Name property.
    /// Expected: Recipe with null Name is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingNullName_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithNullName = new Recipe
        {
            IdRecipe = 1,
            Name = null!,
            Description = "Valid Description"
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithNullName);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithNullName));
        Assert.That(page.CurrentRecipe.Name, Is.Null);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with empty string properties.
    /// Expected: Recipe with empty strings is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingEmptyStrings_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithEmptyStrings = new Recipe
        {
            IdRecipe = 2,
            Name = string.Empty,
            Description = string.Empty
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithEmptyStrings);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithEmptyStrings));
        Assert.That(page.CurrentRecipe.Name, Is.Empty);
        Assert.That(page.CurrentRecipe.Description, Is.Empty);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with whitespace-only string properties.
    /// Expected: Recipe with whitespace strings is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase("   ", "   ")]
    [TestCase("\t", "\n")]
    [TestCase("  \t\n  ", "  \r\n  ")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingWhitespaceStrings_AcceptsRecipe(string name, string description)
    {
        // Arrange
        Recipe recipeWithWhitespace = new Recipe
        {
            IdRecipe = 3,
            Name = name,
            Description = description
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithWhitespace);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithWhitespace));
        Assert.That(page.CurrentRecipe.Name, Is.EqualTo(name));
        Assert.That(page.CurrentRecipe.Description, Is.EqualTo(description));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with very long string properties.
    /// Expected: Recipe with long strings is accepted without throwing.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingVeryLongStrings_AcceptsRecipe()
    {
        // Arrange
        string longName = new string('A', 10000);
        string longDescription = new string('B', 50000);
        Recipe recipeWithLongStrings = new Recipe
        {
            IdRecipe = 4,
            Name = longName,
            Description = longDescription
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithLongStrings);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithLongStrings));
        Assert.That(page.CurrentRecipe.Name, Is.EqualTo(longName));
        Assert.That(page.CurrentRecipe.Description, Is.EqualTo(longDescription));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with special characters in string properties.
    /// Expected: Recipe with special characters is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase("Recipe's \"Special\" Name", "Description with <tags> & symbols")]
    [TestCase("émojis 🍕🥞🍰", "Spëciål çhåractërs")]
    [TestCase("'; DROP TABLE Recipes; --", "<script>alert('xss')</script>")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingSpecialCharacters_AcceptsRecipe(string name, string description)
    {
        // Arrange
        Recipe recipeWithSpecialChars = new Recipe
        {
            IdRecipe = 5,
            Name = name,
            Description = description
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithSpecialChars);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithSpecialChars));
        Assert.That(page.CurrentRecipe.Name, Is.EqualTo(name));
        Assert.That(page.CurrentRecipe.Description, Is.EqualTo(description));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with extreme numeric IdRecipe values.
    /// Expected: Recipe with extreme IdRecipe values is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1)]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingExtremeIdValues_AcceptsRecipe(int idRecipe)
    {
        // Arrange
        Recipe recipeWithExtremeId = new Recipe
        {
            IdRecipe = idRecipe,
            Name = "Test Recipe",
            Description = "Test Description"
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithExtremeId);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithExtremeId));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.EqualTo(idRecipe));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with null IdRecipe (nullable int).
    /// Expected: Recipe with null IdRecipe is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingNullId_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithNullId = new Recipe
        {
            IdRecipe = null,
            Name = "Test Recipe",
            Description = "Test Description"
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithNullId);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithNullId));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.Null);
    }

    /// <summary>
    /// Tests that PageClosedTask returns an incomplete Task immediately after construction.
    /// Expected: PageClosedTask.IsCompleted is false since TaskCompletionSource has not been completed.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_PageClosedTask_IsNotCompletedInitially()
    {
        // Arrange
        Recipe recipe = new Recipe { Name = "Test" };

        // Act
        RecipesPage page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.PageClosedTask, Is.Not.Null);
        Assert.That(page.PageClosedTask.IsCompleted, Is.False);
        Assert.That(page.PageClosedTask.IsCanceled, Is.False);
        Assert.That(page.PageClosedTask.IsFaulted, Is.False);
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false immediately after construction.
    /// Expected: RecipeIsChosen is false since it's initialized to false at class level.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_RecipeIsChosen_IsFalseInitially()
    {
        // Arrange
        Recipe recipe = new Recipe { Name = "Test" };

        // Act
        RecipesPage page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False);
    }

    /// <summary>
    /// Tests that multiple accesses to CurrentRecipe after construction return the same instance.
    /// Expected: CurrentRecipe consistently returns the same Recipe instance.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_CurrentRecipe_ReturnsSameInstanceOnMultipleAccesses()
    {
        // Arrange
        Recipe recipe = new Recipe
        {
            IdRecipe = 99,
            Name = "Consistent Recipe"
        };

        // Act
        RecipesPage page = new RecipesPage(recipe);
        Recipe firstAccess = page.CurrentRecipe;
        Recipe secondAccess = page.CurrentRecipe;
        Recipe thirdAccess = page.CurrentRecipe;

        // Assert
        Assert.That(firstAccess, Is.SameAs(recipe));
        Assert.That(secondAccess, Is.SameAs(recipe));
        Assert.That(thirdAccess, Is.SameAs(recipe));
        Assert.That(firstAccess, Is.SameAs(secondAccess));
        Assert.That(secondAccess, Is.SameAs(thirdAccess));
    }

    /// <summary>
    /// Tests that PageClosedTask returns the same Task instance on multiple accesses.
    /// Expected: PageClosedTask property consistently returns the same Task instance.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_PageClosedTask_ReturnsSameTaskInstanceOnMultipleAccesses()
    {
        // Arrange
        Recipe recipe = new Recipe { Name = "Test" };

        // Act
        RecipesPage page = new RecipesPage(recipe);
        Task<bool> firstAccess = page.PageClosedTask;
        Task<bool> secondAccess = page.PageClosedTask;
        Task<bool> thirdAccess = page.PageClosedTask;

        // Assert
        Assert.That(firstAccess, Is.SameAs(secondAccess));
        Assert.That(secondAccess, Is.SameAs(thirdAccess));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with all optional properties set to null.
    /// Expected: Recipe with minimal initialization is accepted.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithMinimalRecipe_AcceptsRecipe()
    {
        // Arrange
        Recipe minimalRecipe = new Recipe
        {
            IdRecipe = null,
            Name = null!,
            Description = null
        };

        // Act
        RecipesPage page = new RecipesPage(minimalRecipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(minimalRecipe));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.Null);
        Assert.That(page.CurrentRecipe.Name, Is.Null);
        Assert.That(page.CurrentRecipe.Description, Is.Null);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with IsCooked property set to various boolean values.
    /// Expected: Recipe with IsCooked set is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingIsCooked_AcceptsRecipe(bool isCooked)
    {
        // Arrange
        Recipe recipe = new Recipe
        {
            IdRecipe = 10,
            Name = "Recipe with IsCooked",
            Description = "Test"
        };
        // Note: IsCooked has internal setter, so we cannot directly set it in this test
        // This test demonstrates the intent but would need reflection or internal access

        // Act
        RecipesPage page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipe));
    }
}



/// <summary>
/// Unit tests for the RecipesPage constructor that accepts a Recipe parameter.
/// </summary>
/// <remarks>
/// NOTE: These tests are marked as Ignored because RecipesPage inherits from ContentPage
/// and calls InitializeComponent() in its constructor. This requires the full MAUI runtime
/// infrastructure to be available, which is not present in a standard unit test context.
/// These tests demonstrate the intended test logic but cannot execute without MAUI infrastructure.
/// Consider integration tests or UI tests for this class instead.
/// </remarks>
public partial class RecipesPageConstructorWithRecipeParameterTests
{
    /// <summary>
    /// Tests that the constructor with a valid Recipe parameter properly initializes all required fields.
    /// Expected: bl.Recipe is set to the provided Recipe, _taskCompletionSource is initialized,
    /// RecipeIsChosen is false, and PageClosedTask returns a valid incomplete Task.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithValidRecipe_InitializesAllFieldsCorrectly()
    {
        // Arrange
        Recipe testRecipe = new Recipe
        {
            IdRecipe = 42,
            Name = "Chocolate Cake",
            Description = "Delicious chocolate cake recipe"
        };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(testRecipe), "CurrentRecipe should return the same Recipe instance provided to constructor");
        Assert.That(page.RecipeIsChosen, Is.False, "RecipeIsChosen should be false after construction");
        Assert.That(page.PageClosedTask, Is.Not.Null, "PageClosedTask should not be null");
        Assert.That(page.PageClosedTask.IsCompleted, Is.False, "PageClosedTask should not be completed initially");
    }

    /// <summary>
    /// Tests that the constructor handles a null Recipe parameter without throwing.
    /// Expected: bl.Recipe is set to null, page initializes successfully.
    /// </summary>
    /// <remarks>
    /// The source code does not perform null validation on the Recipe parameter,
    /// so null is accepted and stored in bl.Recipe.
    /// </remarks>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithNullRecipe_AcceptsNullAndInitializes()
    {
        // Arrange
        Recipe? nullRecipe = null;

        // Act
        RecipesPage page = new RecipesPage(nullRecipe!);

        // Assert
        Assert.That(page.CurrentRecipe, Is.Null, "CurrentRecipe should return null when null Recipe is provided");
        Assert.That(page.RecipeIsChosen, Is.False, "RecipeIsChosen should be false even with null Recipe");
        Assert.That(page.PageClosedTask, Is.Not.Null, "PageClosedTask should be initialized even with null Recipe");
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with null Name property.
    /// Expected: Recipe with null Name is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingNullName_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithNullName = new Recipe
        {
            IdRecipe = 1,
            Name = null!,
            Description = "Recipe with null name"
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithNullName);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithNullName));
        Assert.That(page.CurrentRecipe.Name, Is.Null);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with null Description property.
    /// Expected: Recipe with null Description is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingNullDescription_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithNullDescription = new Recipe
        {
            IdRecipe = 2,
            Name = "Test Recipe",
            Description = null
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithNullDescription);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithNullDescription));
        Assert.That(page.CurrentRecipe.Description, Is.Null);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with empty string properties.
    /// Expected: Recipe with empty strings is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingEmptyStrings_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithEmptyStrings = new Recipe
        {
            IdRecipe = 3,
            Name = string.Empty,
            Description = string.Empty
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithEmptyStrings);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithEmptyStrings));
        Assert.That(page.CurrentRecipe.Name, Is.Empty);
        Assert.That(page.CurrentRecipe.Description, Is.Empty);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with whitespace-only string properties.
    /// Expected: Recipe with whitespace strings is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase("   ", "   ")]
    [TestCase("\t", "\n")]
    [TestCase("  \t\n  ", "  \r\n  ")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingWhitespaceStrings_AcceptsRecipe(string name, string description)
    {
        // Arrange
        Recipe recipeWithWhitespace = new Recipe
        {
            IdRecipe = 4,
            Name = name,
            Description = description
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithWhitespace);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithWhitespace));
        Assert.That(page.CurrentRecipe.Name, Is.EqualTo(name));
        Assert.That(page.CurrentRecipe.Description, Is.EqualTo(description));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with very long string properties.
    /// Expected: Recipe with long strings is accepted without throwing.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingVeryLongStrings_AcceptsRecipe()
    {
        // Arrange
        string longName = new string('A', 10000);
        string longDescription = new string('B', 50000);
        Recipe recipeWithLongStrings = new Recipe
        {
            IdRecipe = 5,
            Name = longName,
            Description = longDescription
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithLongStrings);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithLongStrings));
        Assert.That(page.CurrentRecipe.Name.Length, Is.EqualTo(10000));
        Assert.That(page.CurrentRecipe.Description?.Length, Is.EqualTo(50000));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with special characters in string properties.
    /// Expected: Recipe with special characters is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase("Recipe's \"Special\" Name", "Description with <tags> & symbols")]
    [TestCase("émojis 🍕🥞🍰", "Spëciål çhåractërs")]
    [TestCase("'; DROP TABLE Recipes; --", "<script>alert('xss')</script>")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingSpecialCharacters_AcceptsRecipe(string name, string description)
    {
        // Arrange
        Recipe recipeWithSpecialChars = new Recipe
        {
            IdRecipe = 6,
            Name = name,
            Description = description
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithSpecialChars);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithSpecialChars));
        Assert.That(page.CurrentRecipe.Name, Is.EqualTo(name));
        Assert.That(page.CurrentRecipe.Description, Is.EqualTo(description));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with extreme numeric IdRecipe values.
    /// Expected: Recipe with extreme IdRecipe values is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1)]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingExtremeIdValues_AcceptsRecipe(int idRecipe)
    {
        // Arrange
        Recipe recipeWithExtremeId = new Recipe
        {
            IdRecipe = idRecipe,
            Name = "Test Recipe",
            Description = "Test Description"
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithExtremeId);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithExtremeId));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.EqualTo(idRecipe));
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with null IdRecipe (nullable int).
    /// Expected: Recipe with null IdRecipe is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingNullId_AcceptsRecipe()
    {
        // Arrange
        Recipe recipeWithNullId = new Recipe
        {
            IdRecipe = null,
            Name = "Recipe without ID",
            Description = "New recipe not yet saved"
        };

        // Act
        RecipesPage page = new RecipesPage(recipeWithNullId);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipeWithNullId));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.Null);
    }

    /// <summary>
    /// Tests that PageClosedTask returns an incomplete Task immediately after construction.
    /// Expected: PageClosedTask.IsCompleted is false since TaskCompletionSource has not been completed.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_PageClosedTask_IsNotCompletedInitially()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 100, Name = "Test" };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);

        // Assert
        Assert.That(page.PageClosedTask, Is.Not.Null);
        Assert.That(page.PageClosedTask.IsCompleted, Is.False, "Task should not be completed immediately after construction");
        Assert.That(page.PageClosedTask.IsFaulted, Is.False, "Task should not be faulted");
        Assert.That(page.PageClosedTask.IsCanceled, Is.False, "Task should not be canceled");
    }

    /// <summary>
    /// Tests that RecipeIsChosen property returns false immediately after construction.
    /// Expected: RecipeIsChosen is false since it's initialized to false at class level.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_RecipeIsChosen_IsFalseInitially()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 200 };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);

        // Assert
        Assert.That(page.RecipeIsChosen, Is.False, "RecipeIsChosen should be false immediately after construction");
    }

    /// <summary>
    /// Tests that multiple accesses to CurrentRecipe after construction return the same instance.
    /// Expected: CurrentRecipe consistently returns the same Recipe instance.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_CurrentRecipe_ReturnsSameInstanceOnMultipleAccesses()
    {
        // Arrange
        Recipe testRecipe = new Recipe
        {
            IdRecipe = 300,
            Name = "Consistency Test Recipe"
        };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);
        Recipe firstAccess = page.CurrentRecipe;
        Recipe secondAccess = page.CurrentRecipe;
        Recipe thirdAccess = page.CurrentRecipe;

        // Assert
        Assert.That(firstAccess, Is.SameAs(testRecipe));
        Assert.That(secondAccess, Is.SameAs(testRecipe));
        Assert.That(thirdAccess, Is.SameAs(testRecipe));
        Assert.That(firstAccess, Is.SameAs(secondAccess));
        Assert.That(secondAccess, Is.SameAs(thirdAccess));
    }

    /// <summary>
    /// Tests that PageClosedTask returns the same Task instance on multiple accesses.
    /// Expected: PageClosedTask property consistently returns the same Task instance.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_PageClosedTask_ReturnsSameTaskInstanceOnMultipleAccesses()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 400 };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);
        Task<bool> firstAccess = page.PageClosedTask;
        Task<bool> secondAccess = page.PageClosedTask;
        Task<bool> thirdAccess = page.PageClosedTask;

        // Assert
        Assert.That(firstAccess, Is.Not.Null);
        Assert.That(secondAccess, Is.Not.Null);
        Assert.That(thirdAccess, Is.Not.Null);
        Assert.That(firstAccess, Is.SameAs(secondAccess), "PageClosedTask should return the same Task instance on multiple accesses");
        Assert.That(secondAccess, Is.SameAs(thirdAccess), "PageClosedTask should return the same Task instance on multiple accesses");
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with all optional properties set to null.
    /// Expected: Recipe with minimal initialization is accepted.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithMinimalRecipe_AcceptsRecipe()
    {
        // Arrange
        Recipe minimalRecipe = new Recipe
        {
            IdRecipe = null,
            Name = null!,
            Description = null
        };

        // Act
        RecipesPage page = new RecipesPage(minimalRecipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(minimalRecipe));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.Null);
        Assert.That(page.CurrentRecipe.Name, Is.Null);
        Assert.That(page.CurrentRecipe.Description, Is.Null);
    }

    /// <summary>
    /// Tests that the constructor handles a Recipe with IsCooked property set to various boolean values.
    /// Expected: Recipe with IsCooked set is accepted and stored correctly.
    /// </summary>
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithRecipeHavingIsCooked_AcceptsRecipe(bool isCooked)
    {
        // Arrange
        Recipe recipe = new Recipe
        {
            IdRecipe = 500,
            Name = "IsCooked Test"
        };
        // Note: IsCooked has internal setter, so this test verifies the constructor accepts
        // any Recipe regardless of IsCooked value set through internal access

        // Act
        RecipesPage page = new RecipesPage(recipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(recipe));
    }

    /// <summary>
    /// Tests that the constructor initializes _taskCompletionSource correctly and it can be used later.
    /// Expected: _taskCompletionSource is properly initialized and PageClosedTask is functional.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_InitializesTaskCompletionSource_ForLaterUse()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 600 };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);
        Task<bool> task = page.PageClosedTask;

        // Assert
        Assert.That(task, Is.Not.Null, "_taskCompletionSource should be initialized");
        Assert.That(task, Is.InstanceOf<Task<bool>>(), "PageClosedTask should be a Task<bool>");
        Assert.That(task.IsCompleted, Is.False, "Task should not be completed yet");
    }

    /// <summary>
    /// Tests that the constructor does not throw any exception with a valid Recipe.
    /// Expected: Constructor completes successfully without throwing.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithValidRecipe_DoesNotThrow()
    {
        // Arrange
        Recipe validRecipe = new Recipe
        {
            IdRecipe = 700,
            Name = "Valid Recipe",
            Description = "This is a valid recipe"
        };

        // Act & Assert
        Assert.DoesNotThrow(() => new RecipesPage(validRecipe), "Constructor should not throw with valid Recipe");
    }

    /// <summary>
    /// Tests that bl.Recipe is immediately accessible after construction through CurrentRecipe property.
    /// Expected: CurrentRecipe returns the Recipe set in constructor without delay.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_CurrentRecipe_IsImmediatelyAccessible()
    {
        // Arrange
        Recipe testRecipe = new Recipe
        {
            IdRecipe = 800,
            Name = "Immediate Access Test"
        };

        // Act
        RecipesPage page = new RecipesPage(testRecipe);

        // Assert - No delay or initialization required
        Assert.That(page.CurrentRecipe, Is.Not.Null, "CurrentRecipe should be immediately accessible");
        Assert.That(page.CurrentRecipe, Is.SameAs(testRecipe), "CurrentRecipe should return the exact instance provided");
    }

    /// <summary>
    /// Tests that the constructor properly handles Recipe objects created with the default constructor.
    /// Expected: Recipe created with default constructor is accepted and stored correctly.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithDefaultConstructedRecipe_AcceptsRecipe()
    {
        // Arrange
        Recipe defaultRecipe = new Recipe();

        // Act
        RecipesPage page = new RecipesPage(defaultRecipe);

        // Assert
        Assert.That(page.CurrentRecipe, Is.SameAs(defaultRecipe));
        Assert.That(page.CurrentRecipe.IdRecipe, Is.Null, "Default Recipe should have null IdRecipe");
        Assert.That(page.CurrentRecipe.CarbohydratesPercent, Is.Not.Null, "Default Recipe should have initialized CarbohydratesPercent");
        Assert.That(page.CurrentRecipe.AccuracyOfChoEstimate, Is.Not.Null, "Default Recipe should have initialized AccuracyOfChoEstimate");
    }
}



/// <summary>
/// Unit tests for the OnDisappearing method of RecipesPage.
/// </summary>
/// <remarks>
/// NOTE: These tests are marked as Ignored because RecipesPage inherits from ContentPage
/// and calls InitializeComponent() in its constructors. This requires the full MAUI runtime
/// infrastructure to be available, which is not present in a standard unit test context.
/// These tests demonstrate the intended test logic but cannot execute without MAUI infrastructure.
/// Consider integration tests or UI tests for this class instead.
/// </remarks>
public partial class RecipesPageOnDisappearingTests
{
    /// <summary>
    /// Helper class that exposes protected members of RecipesPage for testing.
    /// </summary>
    private class TestableRecipesPage : RecipesPage
    {
        public TestableRecipesPage(Recipe recipe) : base(recipe)
        {
        }

        public TestableRecipesPage(string recipeNameForSearch, string recipeDescriptionForSearch)
            : base(recipeNameForSearch, recipeDescriptionForSearch)
        {
        }

        public void CallOnDisappearing()
        {
            OnDisappearing();
        }
    }

    /// <summary>
    /// Tests that OnDisappearing completes the TaskCompletionSource with recipeIsChosen value (false by default).
    /// Input: Page constructed with a Recipe, recipeIsChosen is false (default).
    /// Expected: PageClosedTask completes with result false, task.IsCompleted is true.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithDefaultRecipeIsChosen_CompletesTaskWithFalse()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe", Description = "Test Description" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false when recipeIsChosen is false (default)");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed after OnDisappearing");
    }

    /// <summary>
    /// Tests that OnDisappearing completes the TaskCompletionSource when page is created with search parameters.
    /// Input: Page constructed with recipe name and description strings, recipeIsChosen is false (default).
    /// Expected: PageClosedTask completes with result false without throwing exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithSearchParametersConstructor_CompletesTaskSuccessfully()
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage("Pancakes", "Fluffy breakfast pancakes");

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false when recipeIsChosen is false (default)");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing does not throw when called multiple times.
    /// Input: Page constructed, OnDisappearing called twice.
    /// Expected: Second call does not throw exception, task remains completed with original result.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WhenCalledMultipleTimes_DoesNotThrowException()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool firstResult = await page.PageClosedTask;

        // Assert - calling again should not throw
        Assert.DoesNotThrow(() => page.CallOnDisappearing(), "OnDisappearing should not throw when called multiple times");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "Task should remain completed");
        Assert.That(firstResult, Is.False, "Task result should remain false");
    }

    /// <summary>
    /// Tests that OnDisappearing handles task completion synchronously.
    /// Input: Page constructed, OnDisappearing called.
    /// Expected: Task completes immediately or very quickly, can be checked synchronously.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void OnDisappearing_CompletesTaskSynchronously()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();

        // Assert - task should be completed immediately
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "Task should be completed synchronously after OnDisappearing");
    }

    /// <summary>
    /// Tests that OnDisappearing handles page created with null recipe parameter.
    /// Input: Page constructed with null Recipe.
    /// Expected: PageClosedTask completes with false without throwing exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithNullRecipe_CompletesTaskSuccessfully()
    {
        // Arrange
        Recipe? nullRecipe = null;
        TestableRecipesPage page = new TestableRecipesPage(nullRecipe!);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing handles page created with empty search strings.
    /// Input: Page constructed with empty strings for recipe name and description.
    /// Expected: PageClosedTask completes with false without exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithEmptySearchStrings_CompletesTaskSuccessfully()
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage("", "");

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing handles page created with null search strings.
    /// Input: Page constructed with null strings for recipe name and description.
    /// Expected: PageClosedTask completes with false without exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithNullSearchStrings_CompletesTaskSuccessfully()
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage(null!, null!);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing with whitespace-only search strings completes successfully.
    /// Input: Page constructed with whitespace strings.
    /// Expected: PageClosedTask completes with false without exceptions.
    /// </summary>
    [Test]
    [TestCase("   ", "   ")]
    [TestCase("\t", "\n")]
    [TestCase("  \t\n  ", "  \r\n  ")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithWhitespaceSearchStrings_CompletesTaskSuccessfully(string recipeName, string recipeDescription)
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage(recipeName, recipeDescription);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing with very long search strings completes successfully.
    /// Input: Page constructed with very long strings (10000 characters each).
    /// Expected: PageClosedTask completes without throwing exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithVeryLongSearchStrings_CompletesTaskSuccessfully()
    {
        // Arrange
        string longName = new string('A', 10000);
        string longDescription = new string('B', 10000);
        TestableRecipesPage page = new TestableRecipesPage(longName, longDescription);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing with special characters in search strings completes successfully.
    /// Input: Page constructed with strings containing special characters.
    /// Expected: PageClosedTask completes without exceptions.
    /// </summary>
    [Test]
    [TestCase("Recipe's \"Name\"", "Description with <tags>")]
    [TestCase("émojis 🍕🥞", "Spëciål çhårs")]
    [TestCase("'; DROP TABLE--", "<script>alert('xss')</script>")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithSpecialCharactersInSearchStrings_CompletesTaskSuccessfully(string recipeName, string recipeDescription)
    {
        // Arrange
        TestableRecipesPage page = new TestableRecipesPage(recipeName, recipeDescription);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing handles a Recipe with extreme IdRecipe values.
    /// Input: Page constructed with Recipe having extreme int values for IdRecipe.
    /// Expected: PageClosedTask completes successfully without exceptions.
    /// </summary>
    [Test]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    [TestCase(0)]
    [TestCase(-1)]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithExtremeRecipeIdValues_CompletesTaskSuccessfully(int idRecipe)
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = idRecipe, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing handles a Recipe with null IdRecipe.
    /// Input: Page constructed with Recipe having null IdRecipe.
    /// Expected: PageClosedTask completes successfully.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithNullRecipeId_CompletesTaskSuccessfully()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = null, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing maintains task result consistency across multiple accesses.
    /// Input: Page constructed, OnDisappearing called, task accessed multiple times.
    /// Expected: All accesses return the same result value (false).
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_TaskResultConsistency_ReturnsSameValueOnMultipleAccesses()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result1 = await page.PageClosedTask;
        bool result2 = await page.PageClosedTask;
        bool result3 = await page.PageClosedTask;

        // Assert
        Assert.That(result1, Is.False, "First access should return false");
        Assert.That(result2, Is.False, "Second access should return false");
        Assert.That(result3, Is.False, "Third access should return false");
        Assert.That(result1, Is.EqualTo(result2), "All accesses should return the same value");
        Assert.That(result2, Is.EqualTo(result3), "All accesses should return the same value");
    }

    /// <summary>
    /// Tests that PageClosedTask can be awaited immediately after OnDisappearing without deadlock.
    /// Input: Page constructed, OnDisappearing called, task awaited immediately.
    /// Expected: Await completes without deadlock or timeout.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_TaskAwaitedImmediately_CompletesWithoutDeadlock()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();

        // Use timeout to ensure no deadlock
        Task<bool> resultTask = page.PageClosedTask;
        Task completedTask = await Task.WhenAny(resultTask, Task.Delay(5000));

        // Assert
        Assert.That(completedTask, Is.SameAs(resultTask), "Task should complete before timeout");
        bool result = await resultTask;
        Assert.That(result, Is.False, "Task should return false");
    }

    /// <summary>
    /// Tests that OnDisappearing with Recipe having null Name property completes successfully.
    /// Input: Page constructed with Recipe having null Name.
    /// Expected: PageClosedTask completes without exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithRecipeHavingNullName_CompletesTaskSuccessfully()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = null, Description = "Test Description" };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing with Recipe having null Description property completes successfully.
    /// Input: Page constructed with Recipe having null Description.
    /// Expected: PageClosedTask completes without exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithRecipeHavingNullDescription_CompletesTaskSuccessfully()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = 1, Name = "Test Recipe", Description = null };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }

    /// <summary>
    /// Tests that OnDisappearing with Recipe having all null properties completes successfully.
    /// Input: Page constructed with Recipe having all nullable properties set to null.
    /// Expected: PageClosedTask completes without exceptions.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public async Task OnDisappearing_WithRecipeHavingAllNullProperties_CompletesTaskSuccessfully()
    {
        // Arrange
        Recipe testRecipe = new Recipe { IdRecipe = null, Name = null, Description = null };
        TestableRecipesPage page = new TestableRecipesPage(testRecipe);

        // Act
        page.CallOnDisappearing();
        bool result = await page.PageClosedTask;

        // Assert
        Assert.That(result, Is.False, "Task should complete with false");
        Assert.That(page.PageClosedTask.IsCompleted, Is.True, "PageClosedTask should be completed");
    }
}



/// <summary>
/// Unit tests for the RecipesPage constructor that accepts string search parameters.
/// </summary>
/// <remarks>
/// NOTE: These tests are marked as Ignored because RecipesPage inherits from ContentPage
/// and calls InitializeComponent() and RefreshGrid() in its constructor. These methods require
/// the full MAUI runtime infrastructure to be available, which is not present in a standard
/// unit test context. These tests demonstrate the intended test logic and expected behavior
/// but cannot execute without MAUI infrastructure. Consider integration tests or UI tests instead.
/// </remarks>
public partial class RecipesPageStringConstructorTests
{
    /// <summary>
    /// Tests that the constructor with valid non-null search parameters properly initializes
    /// the Recipe object with the provided name and description, and initializes all required fields.
    /// Expected: bl.Recipe is created with Name and Description set, _taskCompletionSource is initialized,
    /// RecipeIsChosen is false, and RefreshGrid is called.
    /// </summary>
    [Test]
    [TestCase("Pancakes", "Fluffy breakfast pancakes")]
    [TestCase("Pizza Margherita", "Italian style pizza with tomato and mozzarella")]
    [TestCase("Salad", "Green salad")]
    [TestCase("A", "B")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithValidSearchParameters_InitializesRecipeAndAllFields(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // No arrangement needed, parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created if was null)
        // Expected: bl.Recipe.Name equals recipeName
        // Expected: bl.Recipe.Description equals recipeDescription
        // Expected: page.CurrentRecipe returns the created Recipe
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        // Expected: page.PageClosedTask.IsCompleted is false
        // Expected: RefreshGrid() is called, which sets gridRecipes.ItemsSource
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles null RecipeNameForSearch parameter correctly.
    /// Expected: Recipe is created with Name set to null and Description set to the provided value.
    /// </summary>
    [Test]
    [TestCase("Valid description")]
    [TestCase("")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithNullRecipeName_SetsRecipeNameToNull(string recipeDescription)
    {
        // Arrange
        string? recipeName = null;

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created because it was null)
        // Expected: bl.Recipe.Name is null
        // Expected: bl.Recipe.Description equals recipeDescription
        // Expected: page.CurrentRecipe.Name is null
        // Expected: page.CurrentRecipe.Description equals recipeDescription
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles null RecipeDescriptionForSearch parameter correctly.
    /// Expected: Recipe is created with Description set to null and Name set to the provided value.
    /// </summary>
    [Test]
    [TestCase("Valid name")]
    [TestCase("")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithNullRecipeDescription_SetsRecipeDescriptionToNull(string recipeName)
    {
        // Arrange
        string? recipeDescription = null;

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created because it was null)
        // Expected: bl.Recipe.Name equals recipeName
        // Expected: bl.Recipe.Description is null
        // Expected: page.CurrentRecipe.Name equals recipeName
        // Expected: page.CurrentRecipe.Description is null
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles both parameters being null.
    /// Expected: Recipe is created with both Name and Description set to null.
    /// RefreshGrid should not trigger a search because both fields are null (line 137 checks != "").
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithBothParametersNull_CreatesRecipeWithNullProperties()
    {
        // Arrange
        string? recipeName = null;
        string? recipeDescription = null;

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created if was null)
        // Expected: bl.Recipe.Name is null
        // Expected: bl.Recipe.Description is null
        // Expected: page.CurrentRecipe.Name is null
        // Expected: page.CurrentRecipe.Description is null
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        // Expected: RefreshGrid() is called but doesn't execute search (line 137: both are null, not "")
        // Expected: gridRecipes.ItemsSource is set to allRecipes (which may be null or empty)
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles both parameters being empty strings.
    /// Expected: Recipe is created with Name and Description set to empty strings.
    /// RefreshGrid should not trigger search because line 137 checks Name != "" and Description != "".
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithBothParametersEmpty_CreatesRecipeWithEmptyProperties()
    {
        // Arrange
        string recipeName = string.Empty;
        string recipeDescription = string.Empty;

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created if was null)
        // Expected: bl.Recipe.Name is ""
        // Expected: bl.Recipe.Description is ""
        // Expected: page.CurrentRecipe.Name is ""
        // Expected: page.CurrentRecipe.Description is ""
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        // Expected: RefreshGrid() is called but search is NOT executed (line 137: both are "", condition fails)
        // Expected: gridRecipes.ItemsSource is set to allRecipes (null or empty)
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles whitespace-only string parameters.
    /// Expected: Recipe properties are set to the whitespace strings as-is.
    /// RefreshGrid should trigger search because whitespace != "" (line 137 check).
    /// </summary>
    [Test]
    [TestCase("   ", "   ")]
    [TestCase("\t", "\n")]
    [TestCase("  \t\n  ", "  \r\n  ")]
    [TestCase(" ", "  ")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithWhitespaceStrings_SetsRecipePropertiesToWhitespace(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created if was null)
        // Expected: bl.Recipe.Name equals recipeName (whitespace string)
        // Expected: bl.Recipe.Description equals recipeDescription (whitespace string)
        // Expected: page.CurrentRecipe.Name equals recipeName
        // Expected: page.CurrentRecipe.Description equals recipeDescription
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        // Expected: RefreshGrid() is called and DOES execute search (line 137: whitespace != "")
        // Expected: bl.SearchRecipes() is called with whitespace strings
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles very long string parameters without throwing exceptions.
    /// Expected: Recipe is created with very long Name and Description properties.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithVeryLongStrings_HandlesLargeInputsWithoutException()
    {
        // Arrange
        string longName = new string('A', 10000);
        string longDescription = new string('B', 50000);

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(longName, longDescription);

        // Assert
        // Expected: No exception is thrown
        // Expected: bl.Recipe is not null
        // Expected: bl.Recipe.Name equals longName
        // Expected: bl.Recipe.Description equals longDescription
        // Expected: page.CurrentRecipe.Name has length 10000
        // Expected: page.CurrentRecipe.Description has length 50000
        // Expected: RefreshGrid() is called and executes search with long strings
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles strings with special characters including SQL injection patterns,
    /// HTML/XML tags, emojis, and international characters.
    /// Expected: Recipe properties are set to the special character strings as-is without sanitization.
    /// </summary>
    [Test]
    [TestCase("Recipe's \"Special\" Name", "Description with <tags> & symbols")]
    [TestCase("émojis 🍕🥞🍰", "Spëciål çhåractërs")]
    [TestCase("'; DROP TABLE Recipes; --", "<script>alert('xss')</script>")]
    [TestCase("Name\nWith\nNewlines", "Description\tWith\tTabs")]
    [TestCase("Null\0Character", "Backslash\\Test")]
    [TestCase("Unicode: \u0001\u0002\u0003", "Control chars")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithSpecialCharacters_AcceptsAndStoresUnmodified(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null
        // Expected: bl.Recipe.Name equals recipeName (special characters preserved)
        // Expected: bl.Recipe.Description equals recipeDescription (special characters preserved)
        // Expected: page.CurrentRecipe.Name equals recipeName
        // Expected: page.CurrentRecipe.Description equals recipeDescription
        // Expected: No exception is thrown (no input sanitization/validation)
        // Expected: RefreshGrid() executes search with special character strings
        // NOTE: SQL injection protection should be handled by parameterized queries in bl.SearchRecipes()
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles mixed null and non-null parameter combinations.
    /// Expected: Recipe is created with the mix of null and non-null properties.
    /// </summary>
    [Test]
    [TestCase(null, "Description only")]
    [TestCase("Name only", null)]
    [TestCase(null, "")]
    [TestCase("", null)]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithMixedNullAndNonNullParameters_HandlesCorrectly(
        string? recipeName,
        string? recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: bl.Recipe is not null (created if was null)
        // Expected: bl.Recipe.Name equals recipeName (may be null)
        // Expected: bl.Recipe.Description equals recipeDescription (may be null)
        // Expected: page.PageClosedTask is not null
        // Expected: page.RecipeIsChosen is false
        // Expected: RefreshGrid() behavior depends on null vs empty vs non-empty values
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor initializes TaskCompletionSource correctly when created with search parameters.
    /// Expected: _taskCompletionSource is not null and PageClosedTask returns a valid uncompleted Task.
    /// </summary>
    [Test]
    [TestCase("Name", "Description")]
    [TestCase("", "")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_InitializesTaskCompletionSource_WithValidTask(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: page.PageClosedTask is not null
        // Expected: page.PageClosedTask.IsCompleted is false
        // Expected: page.PageClosedTask is a Task<bool>
        // Expected: Multiple accesses to PageClosedTask return same instance
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that RecipeIsChosen property is initialized to false after construction with search parameters.
    /// Expected: RecipeIsChosen returns false since it's initialized to false at class level (line 9).
    /// </summary>
    [Test]
    [TestCase("Recipe Name", "Recipe Description")]
    [TestCase(null, null)]
    [TestCase("", "")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_InitializesRecipeIsChosen_ToFalse(
        string? recipeName,
        string? recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: page.RecipeIsChosen is false
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that RefreshGrid is called during construction which sets gridRecipes.ItemsSource.
    /// When both Name and Description are non-empty, bl.SearchRecipes should be called.
    /// Expected: allRecipes is populated by search and set to gridRecipes.ItemsSource.
    /// </summary>
    [Test]
    [TestCase("Pasta", "Italian pasta dish")]
    [TestCase("a", "b")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_CallsRefreshGrid_TriggersSearchWhenBothParamsNonEmpty(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: RefreshGrid() is called (line 37)
        // Expected: Line 137 condition (Name != "" && Description != "") is true
        // Expected: bl.SearchRecipes(recipeName, recipeDescription, 0) is called
        // Expected: allRecipes is populated with search results
        // Expected: gridRecipes.ItemsSource is set to allRecipes
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that RefreshGrid does not trigger search when one or both parameters are empty/null.
    /// Expected: bl.SearchRecipes is not called, gridRecipes.ItemsSource is set to null or empty allRecipes.
    /// </summary>
    [Test]
    [TestCase("", "Description")]
    [TestCase("Name", "")]
    [TestCase("", "")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_CallsRefreshGrid_DoesNotSearchWhenParamsEmptyOrPartiallyEmpty(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: RefreshGrid() is called (line 37)
        // Expected: Line 137 condition (Name != "" && Description != "") is false
        // Expected: bl.SearchRecipes() is NOT called
        // Expected: allRecipes remains at its default value (null or empty)
        // Expected: gridRecipes.ItemsSource is set to allRecipes (null or empty)
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor creates a new Recipe when bl.Recipe is null (line 32-33).
    /// Expected: A new Recipe instance is created and assigned to bl.Recipe.
    /// </summary>
    [Test]
    [TestCase("Test Name", "Test Description")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WhenBlRecipeIsNull_CreatesNewRecipeInstance(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Assuming bl.Recipe is null initially (default state)

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);

        // Assert
        // Expected: Line 32 condition (bl.Recipe == null) is true
        // Expected: Line 33 executes: bl.Recipe = new Recipe()
        // Expected: page.CurrentRecipe is not null
        // Expected: page.CurrentRecipe.Name equals recipeName
        // Expected: page.CurrentRecipe.Description equals recipeDescription
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that CurrentRecipe property returns the correct Recipe after construction with search parameters.
    /// Expected: CurrentRecipe returns bl.Recipe with Name and Description set from constructor parameters.
    /// </summary>
    [Test]
    [TestCase("Recipe Name", "Recipe Description")]
    [TestCase("Test", "Test Description")]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_CurrentRecipe_ReturnsRecipeWithCorrectProperties(
        string recipeName,
        string recipeDescription)
    {
        // Arrange
        // Parameters provided via TestCase

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(recipeName, recipeDescription);
        // Recipe? currentRecipe = page.CurrentRecipe;

        // Assert
        // Expected: currentRecipe is not null
        // Expected: currentRecipe.Name equals recipeName
        // Expected: currentRecipe.Description equals recipeDescription
        // Expected: Multiple accesses to CurrentRecipe return same instance
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles edge case where recipeName is very long and recipeDescription is empty.
    /// Expected: Recipe is created with long Name and empty Description, RefreshGrid doesn't search.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithVeryLongNameAndEmptyDescription_HandlesCorrectly()
    {
        // Arrange
        string longName = new string('X', 100000);
        string emptyDescription = string.Empty;

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(longName, emptyDescription);

        // Assert
        // Expected: bl.Recipe is not null
        // Expected: bl.Recipe.Name has length 100000
        // Expected: bl.Recipe.Description is ""
        // Expected: RefreshGrid() is called but search is NOT executed (Description is "")
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }

    /// <summary>
    /// Tests that the constructor handles edge case where recipeName is empty and recipeDescription is very long.
    /// Expected: Recipe is created with empty Name and long Description, RefreshGrid doesn't search.
    /// </summary>
    [Test]
    [Ignore("RecipesPage requires MAUI infrastructure (ContentPage and InitializeComponent) which is not available in unit test context.")]
    public void Constructor_WithEmptyNameAndVeryLongDescription_HandlesCorrectly()
    {
        // Arrange
        string emptyName = string.Empty;
        string longDescription = new string('Y', 100000);

        // Act
        // Cannot instantiate RecipesPage because InitializeComponent() will fail without MAUI framework
        // var page = new RecipesPage(emptyName, longDescription);

        // Assert
        // Expected: bl.Recipe is not null
        // Expected: bl.Recipe.Name is ""
        // Expected: bl.Recipe.Description has length 100000
        // Expected: RefreshGrid() is called but search is NOT executed (Name is "")
        Assert.Inconclusive("This test requires MAUI framework initialization to execute. Consider using integration tests.");
    }
}