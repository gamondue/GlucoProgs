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
        Ingredient.IdIngredient = Safe.Int(txtIdIngredient.Text);
        Ingredient.IdRecipe = Safe.Int(txtIdRecipe.Text);
        Ingredient.QuantityGrams.Text = txtIngredientQuantityGrams.Text;
        Ingredient.CarbohydratesPercent.Text = txtIngredientCarbohydratesPercent.Text;
        Ingredient.QuantityPercent.Text = txtIngredientQuantityPercent.Text;
        Ingredient.Name = txtIngredientName.Text;
        Ingredient.AccuracyOfChoEstimate.Text = txtAccuracyOfChoIngredient.Text;
    }
    private void FromUiToRecipe(Recipe Recipe)
    {
        Recipe.IdRecipe = Safe.Int(txtIdRecipe.Text);
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
        //if (!loadingUi)
        {
            if (!txtIngredientQuantityGrams.IsFocused && !txtIngredientCarbohydratesPercent.IsFocused)
            {
                //txtIngredientQuantityGrams.Text = "";
                //////////localIngredientForCalculations.QuantityInUnits.Double = 0;
                //txtIngredientCarbohydratesPercent.Text = "";
            }
        }
        ////localIngredientForCalculations.CarbohydratesGrams.Text = txtIngredientChoGrams.Text;
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
            //    //localFoodInMealForCalculations.QuantityInUnits.Double = 0;
            //    //txtFoodCarbohydratesPercent.Text = "";
            //    //localFoodInMealForCalculations.CarbohydratesPerUnit.Double = 0;
            //}
        }
        //localFoodInMealForCalculations.CarbohydratesGrams.Text = txtFoodChoGrams.Text;
    }
    private void txtIngredientQuantityPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        //bl.CurrentIngredient.QuantityInUnits.Double = null;
        //txtIngredientQuantityGrams.Text = bl.CurrentIngredient.QuantityInUnits.Text;
    }
    private void Calculator_Click(object sender, TappedEventArgs e)
    {
        // save the entries in the classes
        FromUiToClasses();

        var focusedEntry = GetFocusedEntry();
        string sValue = focusedEntry?.Text ?? "0";
        double dValue = double.TryParse(sValue, out var val) ? val : 0;

        // start the CalculatorPage passing to it the value of the 
        // controller that currently has the focus
        var calculator = new CalculatorPage(dValue);
        ////////await Navigation.PushModalAsync(calculator);
        ////////var result = await calculator.ResultSource.Task;

        // check if the page has given back a result
        ////////if (result.HasValue)
        ////////{
            ////////// update the data model
            ////////if (focusedEntry == txtMealCarbohydratesGrams)
            ////////{
            ////////    //bl.FoodInMeal.CarbohydratesGrams.Text = result.Value.ToString();
            ////////    txtMealCarbohydratesGrams.Text = result.Value.ToString();
            ////////    txtMealCarbohydratesGrams_TextChanged(null, null);
            ////////}
            ////////else if (focusedEntry == txtFoodCarbohydratesPercent)
            ////////{
            ////////    //bl.FoodInMeal.CarbohydratesPerUnit.Text = result.Value.ToString();
            ////////    txtFoodCarbohydratesPercent.Text = result.Value.ToString();
            ////////    //txtFoodChoOrQuantity_TextChanged(null, null);
            ////////}
            ////////else if (focusedEntry == txtFoodQuantityInUnits)
            ////////{
            ////////    //bl.FoodInMeal.QuantityInUnits.Text = result.Value.ToString();
            ////////    txtFoodQuantityInUnits.Text = result.Value.ToString();
            ////////    //txtFoodChoOrQuantity_TextChanged(null, null);
            ////////}
            ////////else if (focusedEntry == txtFoodCarbohydratesGrams)
            ////////{
            ////////    //bl.FoodInMeal.CarbohydratesPerUnit.Text = result.Value.ToString();
            ////////    txtFoodCarbohydratesGrams.Text = result.Value.ToString();
            ////////    //txtFoodCarbohydratesGrams_TextChanged(null, null);
            ////////}
            // show the UI starting from the classes
            //FromClassToUi();
        ////////}
    }
    private Entry GetFocusedEntry()
    {
        ////////if (txtFoodQuantityInUnits.IsFocused) return txtFoodQuantityInUnits;
        ////////if (txtFoodCarbohydratesPercent.IsFocused) return txtFoodCarbohydratesPercent;
        ////////if (txtFoodCarbohydratesGrams.IsFocused) return txtFoodCarbohydratesGrams;
        ////////if (txtAccuracyOfChoFoodInMeal.IsFocused) return txtAccuracyOfChoFoodInMeal;
        ////////if (txtMealCarbohydratesGrams.IsFocused) return txtMealCarbohydratesGrams;
        ////////if (txtAccuracyOfChoMeal.IsFocused) return txtAccuracyOfChoMeal;
        // aggiungi altri Entry se necessario
        return null;
    }
}