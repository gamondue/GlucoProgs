using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class RecipePage : ContentPage
{
    BL_Recipes bl; // business layer of recipes

    private bool loading = true;

    private Accuracy accuracyRecipe;
    private Accuracy accuracyIngredient;

    bool firstPass = true;
    private FoodsPage foodsPage;

    public bool RecipeIsChosen { get; internal set; }

    public RecipePage(BL_Recipes BlRecipes)
    {
        InitializeComponent();
        bl = BlRecipes;
        recipeSection.BindingContext = bl.CurrentRecipe;
    }
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        cmbAccuracyRecipe.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        cmbAccuracyIngredient.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

        accuracyRecipe = new Accuracy(txtAccuracyOfChoRecipe, cmbAccuracyRecipe);
        accuracyIngredient = new Accuracy(txtAccuracyOfChoIngredient, cmbAccuracyIngredient);

        RefreshUi();
    }
    private void RefreshGrid()
    {
        bl.ReadAllIngredientsInThisRecipe();
        gridIngredients.BindingContext = null;
        gridIngredients.BindingContext = bl.CurrentRecipe.Ingredients;
    }
    private void RefreshUi()
    {
        FromClassesToUi();
        RefreshGrid();
    }
    private void FromClassesToUi()
    {
        loading = true;
        FromRecipeToUi();
        FromIngredientToUi();
        loading = false;
    }
    private void FromUiToClasses()
    {
        loading = true;
        if (bl.CurrentIngredient == null)
            bl.CurrentIngredient = new();
        // first the current ingredient, THEN the recipe!
        FromUiToIngredient(bl.CurrentIngredient);
        FromUiToRecipe(bl.CurrentRecipe);
        loading = false;
    }
    private void FromUiToIngredient(Ingredient Ingredient)
    {
        Ingredient.IdIngredient = SqlSafe.Int(txtIdIngredient.Text);
        Ingredient.IdRecipe = SqlSafe.Int(txtIdRecipe.Text);
        Ingredient.QuantityGrams.Text = txtIngredientQuantityGrams.Text;
        Ingredient.CarbohydratesPercent.Text = txtIngredientCarbohydratesPercent.Text;
        Ingredient.QuantityPercent.Text = txtIngredientQuantityPercent.Text;
        Ingredient.Name = txtIngredientName.Text;
        Ingredient.AccuracyOfChoEstimate.Text = txtAccuracyOfChoIngredient.Text;
    }
    private void FromUiToRecipe(Recipe Recipe)
    {
        Recipe.IdRecipe = SqlSafe.Int(txtIdRecipe.Text);
        Recipe.Name = txtRecipeName.Text;
        Recipe.Description = txtRecipeDescription.Text;
        Recipe.CarbohydratesPercent.Text = txtChoOfRecipePercent.Text;
        Recipe.AccuracyOfChoEstimate.Text = txtAccuracyOfChoRecipe.Text;
    }
    private void FromIngredientToUi()
    {
        //if (bl.CurrentIngredient != null)
        //{
        ingredientSection.BindingContext = null;
        ingredientSection.BindingContext = bl.CurrentIngredient;
        //}
    }
    private void FromRecipeToUi()
    {
        if (bl.CurrentRecipe != null)
        {
            recipeSection.BindingContext = null;
            recipeSection.BindingContext = bl.CurrentRecipe;
        }
    }
    private void txtIngredientCarbohydratesPercent_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            ////////        FromUiToIngredient(localIngredientForCalculations);
            ////////        bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
            ////////        txtIngredientChoGrams.Text = localIngredientForCalculations.CarbohydratesPerUnit.Text;
            ////////        bl.SaveRecipeParameters();
        }
    }
    private void txtIngredientQuantityGrams_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            ////////FromUiToIngredient(localIngredientForCalculations);
            ////////bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
            ////////txtIngredientChoGrams.Text = localIngredientForCalculations.CarbohydratesGrams.Text;
        }
    }
    private void txtIngredientChoGrams_TextChanged(object sender, EventArgs e)
    {
        //if (!loading)
        {
            if (!txtIngredientQuantityGrams.IsFocused && !txtIngredientCarbohydratesPercent.IsFocused)
            {
                //txtIngredientQuantityGrams.Text = "";
                //////////localIngredientForCalculations.QuantityGrams.Double = 0;
                //txtIngredientCarbohydratesPercent.Text = "";
            }
        }
        ////localIngredientForCalculations.CarbohydratesPercent.Text = txtIngredientChoGrams.Text;
        //bl.RecalcAll();
        //FromRecipeToUi();
        //txtChoOfRecipePercent.Text = bl.CurrentRecipe.CarbohydratesPerUnit.Text;
    }
    private void btnSaveAllRecipe_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        txtIdRecipe.Text = bl.SaveOneRecipe(bl.CurrentRecipe).ToString();
    }
    private void btnAddIngredient_Click(object sender, EventArgs e)
    {
        if (txtIngredientName.Text == null || txtIngredientName.Text == "")
        {
            DisplayAlert("", "Insert an ingredient name", "OK");
            return;
        }
        FromUiToClasses();
        // erase Id of CurrentIngredient, so that a new record will be created
        bl.CurrentIngredient.IdIngredient = null;
        // mark this ingredient as belonging to the current recipe
        bl.CurrentIngredient.IdRecipe = bl.CurrentRecipe.IdRecipe;
        // save the ingredient
        if (bl.CurrentRecipe.Ingredients == null)
            bl.CreateNewListOfIngredientsInRecipe();
        txtIdIngredient.Text = bl.SaveOneIngredient(bl.CurrentIngredient).ToString();
        bl.RecalcAll();
        RefreshGrid();
    }
    private void btnFoods_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        foodsPage = new FoodsPage(bl.CurrentIngredient);
        Navigation.PushAsync(foodsPage);
    }
    private void btnSaveAllFoods_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.SaveOneIngredient(bl.CurrentIngredient).ToString();
        bl.SaveListOfIngredients();
        RefreshGrid();
    }
    private void btnDefaults_Click(object sender, EventArgs e)
    {
        bl.CurrentIngredient = null;
        FromIngredientToUi();
        //txtIngredientCarbohydratesPercent.Text = "";
        //txtIngredientQuantityGrams.Text = "";
        ////txtIngredientChoGrams.Text = "";
        //txtAccuracyOfChoRecipe.Text = "";
        //cmbAccuracyRecipe.SelectedItem = null;
        //txtIdRecipe.Text = "";
        //txtRecipeName.Text = "";
        //FromUiToIngredient(bl.CurrentIngredient);
    }
    private void btnCalc_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.RecalcAll();
        txtIdRecipe.Text = bl.SaveOneRecipe(bl.CurrentRecipe).ToString();
        FromClassesToUi();
        RefreshGrid();
    }
    private async void btnSearchFood_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new FoodsPage(txtRecipeName.Text, ""));
    }
    private async void btnWeighFood_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WeighFoodPage());
    }
    private async void gridIngredients_CellClick(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        //make the tapped row the current food in CurrentRecipe 
        bl.CurrentIngredient = (Ingredient)e.SelectedItem;
        ingredientSection.BindingContext = null;
        ingredientSection.BindingContext = bl.CurrentIngredient;
        //bl.UpdatePercentages();
        //RefreshGrid();
    }
    protected override async void OnAppearing()
    {
        if (foodsPage != null && foodsPage.FoodIsChosen)
        {
            bl.FromFoodToIngredient(foodsPage.CurrentFood, bl.CurrentIngredient);
            FromClassesToUi();
            txtIngredientCarbohydratesPercent.Focus();
        }
    }
    private void btnRemoveIngredient_Click(object sender, EventArgs e)
    {
        bl.DeleteOneIngredient(bl.CurrentIngredient);
        bl.CurrentIngredient = null;
        FromClassesToUi();
        RefreshGrid();
    }
    private void btnGetIngredientFood_Click(object sender, EventArgs e)
    {
        FoodsPage p = new FoodsPage(bl.CurrentIngredient);
        Navigation.PushAsync(p);
    }
    private void txtIngredientQuantityGrams_TextChanged(object sender, TextChangedEventArgs e)
    {
        //////////bl.RecalcAll();
        //FromClassesToUi();
    }
    private void txtIngredientCarbohydratesPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!loading)
        {
            //if (!txtIngredientChoPercent.IsFocused && !txtFoodCarbohydratesPercent.IsFocused)
            //{
            //    //txtFoodQuantityGrams.Text = "";
            //    //localFoodInMealForCalculations.QuantityGrams.Double = 0;
            //    //txtFoodCarbohydratesPercent.Text = "";
            //    //localFoodInMealForCalculations.CarbohydratesPerUnit.Double = 0;
            //}
        }
        //localFoodInMealForCalculations.CarbohydratesGrams.Text = txtFoodChoGrams.Text;
    }
    private void txtIngredientQuantityPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        //bl.CurrentIngredient.QuantityGrams.Double = null;
        //txtIngredientQuantityGrams.Text = bl.CurrentIngredient.QuantityGrams.Text;
    }
}