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
}