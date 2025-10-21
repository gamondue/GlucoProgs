using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class RecipesPage : ContentPage
{
    BL_Recipes bl = new BL_Recipes();
    bool recipeIsChosen = false;
    public bool RecipeIsChosen { get => recipeIsChosen; }
    public Recipe CurrentRecipe
    {
        get
        {
            return bl.Recipe;
        }
    }

    // Add TaskCompletionSource to handle page completion
    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);

    List<Recipe> allRecipes;
    private RecipePage recipePage;
    private bool loading = false;
    public RecipesPage(Recipe Recipe)
    {
        InitializeComponent();
        bl.Recipe = Recipe;
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }
    public RecipesPage(string RecipeNameForSearch, string RecipeDescriptionForSearch)
    {
        InitializeComponent();
        if (bl.Recipe == null)
            bl.Recipe = new Recipe();
        bl.Recipe.Name = RecipeNameForSearch;
        bl.Recipe.Description = RecipeDescriptionForSearch;
        _taskCompletionSource = new TaskCompletionSource<bool>();
        RefreshGrid();
    }
    /*public RecipesPage(Ingredient Ingredient)
    {
        InitializeComponent();
        if (bl.Recipe == null)
            bl.Recipe = new Food();
        bl.FromIngredientToFood(Ingredient, bl.Recipe);
    }*/
    private void PageLoaded(object sender, EventArgs e)
    {
        recipeIsChosen = false;

        txtIdRecipe.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        if (bl.Recipe == null)
            bl.Recipe = new Recipe();
        bl.Recipe.Name = "";
        bl.Recipe.Description = "";
        allRecipes = new List<Recipe>();
        // if a specific food is passed, load its persistent data from database 
        if (bl.Recipe.IdRecipe != 0 && bl.Recipe.IdRecipe != null)
        {
            bl.Recipe = bl.GetOneRecipe(bl.Recipe.IdRecipe);
            // if what is passed has not and IdFood,
            // we use the data actually passed 

            // let's show the current Recipe
            FromClassToUi();
            this.BindingContext = bl.Recipe;
            gridRecipes.ItemsSource = allRecipes;
        }
    }
    private void FromClassToUi()
    {
        txtIdRecipe.Text = bl.Recipe.IdRecipe.ToString();
        txtName.Text = bl.Recipe.Name;
        txtDescription.Text = bl.Recipe.Description;
        txtRecipeCarbohydrates.Text = bl.Recipe.CarbohydratesPercent.Text;
        chkCooked.IsChecked = (bool)Safe.Bool(bl.Recipe.IsCooked);
        txtRawToCookedRatio.Text = bl.Recipe.RawToCookedRatio.Text;
        // XXXX = bl.Recipe.AccuracyOfChoEstimate;
    }
    private void FromUiToCurrentRecipe()
    {
        bl.Recipe.IdRecipe = Safe.Int(txtIdRecipe.Text);
        bl.Recipe.Name = txtName.Text;
        bl.Recipe.Description = txtDescription.Text;
        bl.Recipe.CarbohydratesPercent.Double = Safe.Double(txtRecipeCarbohydrates.Text);
        bl.Recipe.IsCooked = (bool)Safe.Bool(chkCooked.IsChecked);
        bl.Recipe.RawToCookedRatio.Double = Safe.Double(txtRawToCookedRatio.Text);
        // bl.Recipe.AccuracyOfChoEstimate = XXXX;
    }
    private void OnGridSelection(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        loading = true;
        //make the tapped row the current food
        bl.Recipe = (Recipe)gridRecipes.SelectedItem;
        this.BindingContext = bl.Recipe;
        FromClassToUi();
        loading = false;
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private void RefreshGrid()
    {
        if (bl.Recipe.Name != "" && bl.Recipe.Description != "")
            allRecipes = bl.SearchRecipes(bl.Recipe.Name, bl.Recipe.Description, 0);
        gridRecipes.ItemsSource = allRecipes;
    }
    //private void btnRecipeDetails_Click(object sender, EventArgs e)
    //{
    //    FromUiToCurrentRecipe();
    //    recipePage = new RecipePage(bl.Recipe);
    //    await Navigation.PushAsync(recipePage);
    //    if (recipePage.RecipeIsChosen)
    //    {
    //        //bl.FromFoodToIngredient(recipePage.bl.Recipe, bl.Ingredient);
    //        FromClassesToUi();
    //    }
    //}
    private void btnRecipeDetails_Click(object sender, EventArgs e)
    {
        FromUiToCurrentRecipe();
        recipePage = new RecipePage(bl);
        Navigation.PushAsync(recipePage);
        if (recipePage.RecipeIsChosen)
        {
            //bl.FromFoodToIngredient(recipePage.bl.Recipe, bl.Ingredient);
            FromClassToUi();
        }
    }
    private void btnSaveRecipe_Click(object sender, EventArgs e)
    {
        if (txtIdRecipe.Text == "")
        {
            DisplayAlert("Select one recipe from the list", "Choose a recipe to save", "Ok");
            return;
        }
        FromUiToCurrentRecipe();
        bl.SaveOneRecipe(bl.Recipe);
        FromClassToUi();
        RefreshUi();
    }
    private void btnAddRecipe_Click(object sender, EventArgs e)
    {
        FromUiToCurrentRecipe();
        bl.Recipe.IdRecipe = null;
        bl.SaveOneRecipe(bl.Recipe);
        RefreshUi();
    }
    private void btnRemoveRecipe_Click(object sender, EventArgs e)
    {
        bl.DeleteOneRecipe(bl.Recipe);
        RefreshUi();
    }
    private void btnSearchRecipe_Click(object sender, EventArgs e)
    {
        FromUiToCurrentRecipe();
        allRecipes = bl.SearchRecipes(bl.Recipe.Name, bl.Recipe.Description, 0);
        gridRecipes.ItemsSource = allRecipes;
    }
    private async void btnChoose_Click(object sender, EventArgs e)
    {
        recipeIsChosen = true;
        FromUiToCurrentRecipe();
        bl.SaveOneRecipe(bl.Recipe);
        
        // Set the result and close the page
     _taskCompletionSource?.SetResult(true);
        await this.Navigation.PopAsync();
    }
    private void btnClearFields_Click(object sender, EventArgs e)
    {
        loading = true;
        txtIdRecipe.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        txtRecipeCarbohydrates.Text = "";

        bl.Recipe.Name = "";
        bl.Recipe.Description = "";

        loading = false;
        //FromUiToCurrentRecipe();
        RefreshUi();
    }
    private void txtName_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            bl.Recipe.Name = txtName.Text;
            allRecipes = bl.SearchRecipes(txtName.Text, txtDescription.Text, 3);
            if (allRecipes != null)
            {
                gridRecipes.ItemsSource = allRecipes;
            }
        }
    }
    private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!loading)
        {
            allRecipes = bl.SearchRecipes(txtName.Text, txtDescription.Text, 3);
            if (allRecipes != null)
            {
                gridRecipes.ItemsSource = allRecipes;
            }
        }
    }
    private void chkCooked_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (chkCooked.IsChecked)
        {
            txtRawToCookedRatio.IsVisible = false;
            lblCookedRaw.IsVisible = false;
        }
        else
        {
            txtRawToCookedRatio.IsVisible = true;
            lblCookedRaw.IsVisible = true;
        }
    }
    
    protected override void OnDisappearing()
    {
   try
   {
        base.OnDisappearing();
       
        // Complete the task when the page is closed
      if (_taskCompletionSource != null && !_taskCompletionSource.Task.IsCompleted)
        {
      _taskCompletionSource.SetResult(recipeIsChosen);
        }
    }
    catch (Exception ex)
    {
        General.LogOfProgram?.Error("RecipesPage - OnDisappearing", ex);
        // Ensure task is completed even if there's an error
        if (_taskCompletionSource != null && !_taskCompletionSource.Task.IsCompleted)
        {
      _taskCompletionSource.SetResult(false);
        }
    }
}
}