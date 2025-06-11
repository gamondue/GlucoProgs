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
            return bl.CurrentRecipe;
        }
    }

    List<Recipe> allRecipes;
    private RecipePage recipePage;
    private bool loading = false;
    public RecipesPage(Recipe Recipe)
    {
        InitializeComponent();
        bl.CurrentRecipe = Recipe;
    }
    public RecipesPage(string RecipeNameForSearch, string RecipeDescriptionForSearch)
    {
        InitializeComponent();
        if (bl.CurrentRecipe == null)
            bl.CurrentRecipe = new Recipe();
        bl.CurrentRecipe.Name = RecipeNameForSearch;
        bl.CurrentRecipe.Description = RecipeDescriptionForSearch;
        RefreshGrid();
    }
    /*public RecipesPage(CurrentIngredient CurrentIngredient)
    {
        InitializeComponent();
        if (bl.CurrentRecipe == null)
            bl.CurrentRecipe = new Food();
        bl.FromIngredientToFood(CurrentIngredient, bl.CurrentRecipe);
    }*/
    private void PageLoaded(object sender, EventArgs e)
    {
        recipeIsChosen = false;

        txtIdRecipe.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        if (bl.CurrentRecipe == null)
            bl.CurrentRecipe = new Recipe();
        bl.CurrentRecipe.Name = "";
        bl.CurrentRecipe.Description = "";
        allRecipes = new List<Recipe>();
        // if a specific food is passed, load its persistent data from database 
        if (bl.CurrentRecipe.IdRecipe != 0 && bl.CurrentRecipe.IdRecipe != null)
        {
            bl.CurrentRecipe = bl.GetOneRecipe(bl.CurrentRecipe.IdRecipe);
            // if what is passed has not and IdFood,
            // we use the data actually passed 

            // let's show the current CurrentRecipe
            FromClassToUi();
            this.BindingContext = bl.CurrentRecipe;
            gridRecipes.ItemsSource = allRecipes;
        }
    }
    private void FromClassToUi()
    {
        txtIdRecipe.Text = bl.CurrentRecipe.IdRecipe.ToString();
        txtName.Text = bl.CurrentRecipe.Name;
        txtDescription.Text = bl.CurrentRecipe.Description;
        txtRecipeCarbohydrates.Text = bl.CurrentRecipe.CarbohydratesPercent.Text;
        chkCooked.IsChecked = (bool)Safe.Bool(bl.CurrentRecipe.IsCooked);
        txtRawToCookedRatio.Text = bl.CurrentRecipe.RawToCookedRatio.Text;
        // XXXX = bl.CurrentRecipe.AccuracyOfChoEstimate;
    }
    private void FromUiToCurrentRecipe()
    {
        bl.CurrentRecipe.IdRecipe = Safe.Int(txtIdRecipe.Text);
        bl.CurrentRecipe.Name = txtName.Text;
        bl.CurrentRecipe.Description = txtDescription.Text;
        bl.CurrentRecipe.CarbohydratesPercent.Double = Safe.Double(txtRecipeCarbohydrates.Text);
        bl.CurrentRecipe.IsCooked = (bool)Safe.Bool(chkCooked.IsChecked);
        bl.CurrentRecipe.RawToCookedRatio.Double = Safe.Double(txtRawToCookedRatio.Text);
        // bl.CurrentRecipe.AccuracyOfChoEstimate = XXXX;
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
        bl.CurrentRecipe = (Recipe)gridRecipes.SelectedItem;
        this.BindingContext = bl.CurrentRecipe;
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
        if (bl.CurrentRecipe.Name != "" && bl.CurrentRecipe.Description != "")
            allRecipes = bl.SearchRecipes(bl.CurrentRecipe.Name, bl.CurrentRecipe.Description, 0);
        gridRecipes.ItemsSource = allRecipes;
    }
    //private void btnRecipeDetails_Click(object sender, EventArgs e)
    //{
    //    FromUiToCurrentRecipe();
    //    recipePage = new RecipePage(bl.CurrentRecipe);
    //    await Navigation.PushAsync(recipePage);
    //    if (recipePage.RecipeIsChosen)
    //    {
    //        //bl.FromFoodToIngredient(recipePage.bl.CurrentRecipe, bl.CurrentIngredient);
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
            //bl.FromFoodToIngredient(recipePage.bl.CurrentRecipe, bl.CurrentIngredient);
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
        bl.SaveOneRecipe(bl.CurrentRecipe);
        FromClassToUi();
        RefreshUi();
    }
    private void btnAddRecipe_Click(object sender, EventArgs e)
    {
        FromUiToCurrentRecipe();
        bl.CurrentRecipe.IdRecipe = null;
        bl.SaveOneRecipe(bl.CurrentRecipe);
        RefreshUi();
    }
    private void btnRemoveRecipe_Click(object sender, EventArgs e)
    {
        bl.DeleteOneRecipe(bl.CurrentRecipe);
        RefreshUi();
    }
    private void btnSearchRecipe_Click(object sender, EventArgs e)
    {
        FromUiToCurrentRecipe();
        allRecipes = bl.SearchRecipes(bl.CurrentRecipe.Name, bl.CurrentRecipe.Description, 0);
        gridRecipes.ItemsSource = allRecipes;
    }
    private void btnChoose_Click(object sender, EventArgs e)
    {
        recipeIsChosen = true;
        FromUiToCurrentRecipe();
        bl.SaveOneRecipe(bl.CurrentRecipe);
        this.Navigation.PopAsync();
    }
    private void btnCleanFields_Click(object sender, EventArgs e)
    {
        loading = true;
        txtIdRecipe.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        txtRecipeCarbohydrates.Text = "";

        bl.CurrentRecipe.Name = "";
        bl.CurrentRecipe.Description = "";

        loading = false;
        //FromUiToCurrentRecipe();
        RefreshUi();
    }
    private void txtName_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            bl.CurrentRecipe.Name = txtName.Text;
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
}