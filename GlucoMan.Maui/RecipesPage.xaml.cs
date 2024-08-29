using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class RecipesPage : ContentPage
{
    BL_Recipes bl = new BL_Recipes();
    public Recipe CurrentRecipe { get; set; }
    bool recipeIsChosen = false;
    public bool RecipeIsChosen { get => recipeIsChosen; }
    List<Recipe> allRecipes;
    private RecipePage recipePage;
    private bool loading = false;
    public RecipesPage()
    {
        InitializeComponent();
        CurrentRecipe = null;
    }
    public RecipesPage(Recipe Recipe)
    {
        InitializeComponent();
        CurrentRecipe = Recipe;
    }
    public RecipesPage(string RecipeNameForSearch, string RecipeDescriptionForSearch)
    {
        InitializeComponent();
        if (CurrentRecipe == null)
            //////////CurrentRecipe = new Recipe();
            CurrentRecipe.Name = RecipeNameForSearch;
        CurrentRecipe.Description = RecipeDescriptionForSearch;
        RefreshGrid();
    }
    /*public RecipesPage(Ingredient Ingredient)
    {
        InitializeComponent();
        //////////if (CurrentRecipe == null)
        //////////    CurrentRecipe = new Food();
        //////////bl.FromIngredientToFood(Ingredient, CurrentRecipe);
    }*/
    private void PageLoaded(object sender, EventArgs e)
    {
        recipeIsChosen = false;

        txtIdRecipe.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        if (CurrentRecipe == null)
            CurrentRecipe = new Recipe();
        CurrentRecipe.Name = "";
        CurrentRecipe.Description = "";
        allRecipes = new List<Recipe>();
        // if a specific food is passed, load its persistent data from database 
        if (CurrentRecipe.IdRecipe != 0 && CurrentRecipe.IdRecipe != null)
        {
            CurrentRecipe = bl.GetOneRecipe(CurrentRecipe.IdRecipe);
            // if what is passed has not and IdFood,
            // we use the data actually passed 

            // let's show the CurrentRecipe
            FromClassToUi();
            this.BindingContext = CurrentRecipe;
            gridRecipes.ItemsSource = allRecipes; ;
        }
    }
    private void FromClassToUi()
    {
        txtIdRecipe.Text = CurrentRecipe.IdRecipe.ToString();
        txtName.Text = CurrentRecipe.Name;
        txtDescription.Text = CurrentRecipe.Description;
        txtRecipeCarbohydrates.Text = CurrentRecipe.CarbohydratesPercent.Text;
    }
    private void FromUiToClass()
    {
        CurrentRecipe.IdRecipe = SqlSafe.Int(txtIdRecipe.Text);
        CurrentRecipe.Name = txtName.Text;
        CurrentRecipe.Description = txtDescription.Text;
        CurrentRecipe.CarbohydratesPercent.Double = SqlSafe.Double(txtRecipeCarbohydrates.Text);
    }
    private void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        loading = true;
        //make the tapped row the current food
        CurrentRecipe = (Recipe)gridRecipes.SelectedItem;
        this.BindingContext = CurrentRecipe;
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
        if (CurrentRecipe.Name != "" && CurrentRecipe.Description != "")
            allRecipes = bl.SearchRecipes(CurrentRecipe.Name, CurrentRecipe.Description, 0);
        gridRecipes.ItemsSource = allRecipes;
    }
    //private void btnRecipeDetails_Click(object sender, EventArgs e)
    //{
    //    FromUiToClass();
    //    recipePage = new RecipePage(CurrentRecipe);
    //    await Navigation.PushAsync(recipePage);
    //    if (recipePage.RecipeIsChosen)
    //    {
    //        //bl.FromFoodToIngredient(recipePage.CurrentRecipe, bl.Ingredient);
    //        FromClassToUi();
    //    }
    //}
    private void btnRecipeDetails_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        recipePage = new RecipePage(CurrentRecipe);
        Navigation.PushAsync(recipePage);
        if (recipePage.RecipeIsChosen)
        {
            //bl.FromFoodToIngredient(recipePage.CurrentRecipe, bl.Ingredient);
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
        FromUiToClass();
        bl.SaveOneRecipe(CurrentRecipe);
        FromClassToUi();
        RefreshUi();
    }
    private void btnAddRecipe_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        CurrentRecipe.IdRecipe= null;
        bl.SaveOneRecipe(CurrentRecipe);
        RefreshUi();
    }
    private void btnRemoveRecipe_Click(object sender, EventArgs e)
    {
        bl.DeleteOneRecipe(CurrentRecipe);
        RefreshUi();
    }
    private void btnSearchRecipe_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        allRecipes = bl.SearchRecipes(CurrentRecipe.Name, CurrentRecipe.Description, 0);
        gridRecipes.ItemsSource = allRecipes;
    }
    private void btnChoose_Click(object sender, EventArgs e)
    {
        recipeIsChosen = true;
        FromUiToClass();
        bl.SaveOneRecipe(CurrentRecipe);
        this.Navigation.PopAsync();
    }
    private void btnCleanFields_Click(object sender, EventArgs e)
    {
        loading = true;
        txtIdRecipe.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        txtRecipeCarbohydrates.Text = "";

        CurrentRecipe.Name = "";
        CurrentRecipe.Description = "";

        loading = false;
        FromUiToClass();
        RefreshUi();
    }
    private void txtName_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            CurrentRecipe.Name = txtName.Text;
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
}