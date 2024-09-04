using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class RecipePage : ContentPage
{
    BL_Recipes bl = new BL_Recipes();

    private Accuracy accuracyRecipe;
    private Accuracy accuracyIngredient;

    bool firstPass = true;
    private FoodsPage foodsPage;

    Ingredient currentIngredient = new Ingredient();

    public RecipePage(Recipe Recipe)
    {
        InitializeComponent();
        bl.Recipe = Recipe;
    }
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        cmbAccuracyRecipe.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        cmbAccuracyRecipe.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

        accuracyRecipe = new Accuracy(txtAccuracyOfChoRecipe, cmbAccuracyRecipe);
        accuracyIngredient = new Accuracy(txtAccuracyOfChoRecipe, cmbAccuracyRecipe);

        RefreshUi();
    }
    private void RefreshGrid()
    {
        bl.Ingredients = bl.ReadAllIngredientsInARecipe(bl.Recipe.IdRecipe);
        gridIngredients.BindingContext = bl.Ingredients;


    }
    private void RefreshUi()
    {
        FromClassesToUi();
        RefreshGrid();
    }
    private void FromClassesToUi()
    {
        //loading = true;
        FromRecipeToUi();
        FromIngredientToUi();
        //loading = false;
    }
    private void FromUiToClasses()
    {
        //loading = true;
        FromUiToRecipe(bl.Recipe);
        FromUiToIngredient(bl.Ingredient);
        //loading = false;
    }
    private void FromUiToIngredient(Ingredient Ingredient)
    {
        Ingredient.IdIngredient = SqlSafe.Int(txtIdIngredient.Text);
        Ingredient.IdRecipe = SqlSafe.Int(txtIdRecipe.Text);
        Ingredient.QuantityPercent.Text = txtIngredientQuantityPercent.Text;
        Ingredient.QuantityGrams.Text = txtIngredientQuantityGrams.Text;
        Ingredient.CarbohydratesPercent.Text = txtIngredientCarbohydratesPercent.Text;
        Ingredient.QuantityPercent.Text = txtIngredientChoGrams.Text;
        Ingredient.Name = txtIngredientName.Text;
        Ingredient.AccuracyOfChoEstimate.Text = txtAccuracyOfChoIngredient.Text;
    }
    private void FromUiToRecipe(Recipe Recipe)
    {
        Recipe.IdRecipe = SqlSafe.Int(txtIdRecipe.Text);
        Recipe.Name = txtRecipeName.Text;
        //Recipe.Description = txtDescription.Text;
        Recipe.CarbohydratesPercent.Text = txtChoOfRecipePercent.Text;
        Recipe.AccuracyOfChoEstimate.Text = txtAccuracyOfChoRecipe.Text;
    }
    private void FromIngredientToUi()
    {
        if (bl.Ingredient == null)
        {
            bl.Ingredient = new Ingredient();
        }
        //////////if (bl.Ingredient.IdIngredient != null)
        //////////    txtIdRecipe.Text = bl.Ingredient.IdIngredient.ToString();
        //////////else
        //////////    txtIdRecipe.Text = "";

        //bl.RecalcAll();
        txtIdIngredient.Text = bl.Ingredient.IdIngredient.ToString();
        txtIngredientName.Text = bl.Ingredient.Name;
        txtIngredientQuantityGrams.Text = bl.Ingredient.QuantityGrams.Text;
        txtIngredientCarbohydratesPercent.Text = bl.Ingredient.CarbohydratesPercent.Text;
        //bl.Ingredient.CarbohydratesGrams.Double = bl.Ingredient.CarbohydratesPercent.Double * bl.Ingredient.QuantityGrams.Double / 100;
        txtIngredientChoGrams.Text = bl.Ingredient.CarbohydratesGrams.Text;
        txtAccuracyOfChoIngredient.Text = bl.Ingredient.AccuracyOfChoEstimate.Text;
        txtAccuracyOfChoRecipe.Text = bl.Ingredient.AccuracyOfChoEstimate.Text;
    }
    private void FromRecipeToUi()
    {
        if (bl.Recipe != null)
        {
            if (bl.Recipe.IdRecipe != null)
                txtIdRecipe.Text = bl.Recipe.IdRecipe.ToString();
            else
                txtIdRecipe.Text = "-";
            txtChoOfRecipePercent.Text = bl.Recipe.CarbohydratesPercent.Text;
            //txtDescription.Text = bl.Recipe.Description;
            txtRecipeName.Text = bl.Recipe.Name;
            txtAccuracyOfChoRecipe.Text = bl.Recipe.AccuracyOfChoEstimate.Text;
            txtTotalWeight.Text = bl.Recipe.TotalWeight.Text;
        }
    }
    Recipe localIngredientForCalculations = new Recipe();
    ////////private void txtIngredientCarbohydratesPercent_TextChanged(object sender, EventArgs e)
    ////////{
    ////////    //if (!loading)
    ////////    {
    ////////        FromUiToIngredient(localIngredientForCalculations);
    ////////        bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
    ////////        txtIngredientChoGrams.Text = localIngredientForCalculations.CarbohydratesPercent.Text;
    ////////        bl.SaveRecipeParameters();
    ////////    }
    ////////}
    private void txtIngredientQuantityGrams_TextChanged(object sender, EventArgs e)
    {
        //if (!loading)
        {
            ////////FromUiToIngredient(localIngredientForCalculations);
            ////////bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
            ////////txtIngredientChoGrams.Text = localIngredientForCalculations.ChoGrams.Text;
        }
    }
    private void txtIngredientChoGrams_TextChanged(object sender, EventArgs e)
    {
        //if (!loading)
        {
            if (!txtIngredientQuantityGrams.IsFocused && !txtIngredientCarbohydratesPercent.IsFocused)
            {
                txtIngredientQuantityGrams.Text = "";
                //////////localIngredientForCalculations.QuantityGrams.Double = 0;
                txtIngredientCarbohydratesPercent.Text = "";
                localIngredientForCalculations.CarbohydratesPercent.Double = 0;
            }
        }
        localIngredientForCalculations.CarbohydratesPercent.Text = txtIngredientChoGrams.Text;
        //bl.RecalcAll();
        //FromRecipeToUi();
        //txtChoOfRecipePercent.Text = bl.Recipe.Carbohydrates.Text;
    }
    private void txtChoOfRecipePercent_TextChanged(object sender, EventArgs e)
    {
        bl.SaveRecipeParameters();
    }
    private void btnSaveAllRecipe_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        txtIdRecipe.Text = bl.SaveOneRecipe(bl.Recipe).ToString();
        bl.SaveAllIngredientsInARecipe(bl.Recipe.Ingredients);
    }
    private void btnAddIngredient_Click(object sender, EventArgs e)
    {
        if (txtIngredientName.Text == null || txtIngredientName.Text == "")
        {
            DisplayAlert("", "Insert an ingredient name", "OK");
            return;
        }
        FromUiToClasses();
        // erase Id of Ingredient, so that a new record will be created
        bl.Ingredient.IdIngredient = null;
        // mark this ingredient as belonging to the current recipe
        // (not much useful, actually, you can join data with SQL)
        bl.Ingredient.IdRecipe = bl.Recipe.IdRecipe;
        // save the ingredient
        txtIdIngredient.Text = bl.SaveOneIngredient(bl.Ingredient).ToString();
        if (bl.Ingredients == null)
            bl.Ingredients = new List<Ingredient>();
        RefreshGrid();
        bl.RecalcAll();
        FromRecipeToUi();
    }
    private void btnRemoveRecipe_Click(object sender, EventArgs e)
    {
        bl.DeleteOneRecipe(bl.Recipe);
        RefreshGrid();
        bl.RecalcAll();
        FromRecipeToUi();
    }
    private void btnFoods_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        foodsPage = new FoodsPage(currentIngredient);
        Navigation.PushAsync(foodsPage);
    }
    //private void btnRecipes_Click(object sender, EventArgs e)
    //{
    //    FromUiToClasses();
    //    ////////recipesPage = new RecipesPage(bl.RecipeInRecipe);
    //    //recipesPage = new RecipesPage();
    //    Navigation.PushAsync(recipesPage);
    //}
    // in this UI we have no buttons to save just one food in Recipe 
    //private void btnSaveRecipe_Click(object sender, EventArgs e)
    //{
    //    if (gridIngredients.SelectedRows.Count == 0)
    //    {
    //        MessageBox.Show("Choose a food to save");
    //        return;
    //    }
    //    FromUiToClass();
    //    bl.SaveOneIngredient(bl.Ingredient);
    //    FromClassesToUi();
    //    RefreshGrid();
    //}
    private void btnSaveAllFoods_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.SaveOneIngredient(bl.Ingredient).ToString();
        bl.SaveAllIngredientsInARecipe(bl.Ingredients);
        RefreshGrid();
    }
    private void btnDefaults_Click(object sender, EventArgs e)
    {
        txtIngredientCarbohydratesPercent.Text = "";
        txtIngredientQuantityGrams.Text = "";
        txtIngredientChoGrams.Text = "";
        txtAccuracyOfChoRecipe.Text = "";
        cmbAccuracyRecipe.SelectedItem = null;
        txtIdRecipe.Text = "";
        txtRecipeName.Text = "";
        FromUiToIngredient(bl.Ingredient);
    }
    private void btnCalc_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.RecalcAll();
        FromClassesToUi();
        RefreshGrid();
    }
    private async void btnSearchFood_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new FoodsPage(txtRecipeName.Text, ""));
    }
    private async void btnInsulinCalc_ClickAsync(object sender, EventArgs e)
    {
        ////insulinCalcPage = new InsulinCalcPage(bl.Recipe.IdBolusCalculation);
        //insulinCalcPage = new InsulinCalcPage();
        //await Navigation.PushAsync(insulinCalcPage);
    }
    private async void btnGlucose_ClickAsync(object sender, EventArgs e)
    {
        //measurementPage = new GlucoseMeasurementsPage(bl.Recipe.IdGlucoseRecord);
        //await Navigation.PushAsync(measurementPage);
    }
    private async void btnInjection_ClickAsync(object sender, EventArgs e)
    {
        //injectionsPage = new InjectionsPage(bl.Recipe.IdInsulinInjection);
        //await Navigation.PushAsync(injectionsPage);
    }
    private async void btnWeighFood_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WeighFoodPage());
    }
    private async void btnFoodCalc_ClickAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FoodToHitTargetCarbsPage());
    }
    private void btnStartRecipe_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        ////////bl.SaveOneRecipe(bl.Recipe, true); // saves with time now 
        //btnStartRecipe.BackgroundColor = defaultButtonBackground;
        //btnStartRecipe.TextColor = defaultButtonText;
        RefreshUi();
    }
    private async void gridIngredients_Unfocused(object sender, SelectedItemChangedEventArgs e)
    {
        FromUiToClasses();
        bl.SaveAllIngredientsInARecipe(bl.Ingredients);
    }
    private async void gridIngredients_CellClick(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }

        ////////Recipe previousRecipe = bl.Ingredient.DeepCopy();
        FromUiToClasses();
        //make the tapped row the current food in Recipe 
        bl.Ingredient = (Ingredient)e.SelectedItem;
        FromIngredientToUi();
        //////////bl.SaveIngredientParameters();
    }
    protected override async void OnAppearing()
    {
        ////////////if (recipesPage != null && recipesPage.FoodIsChosen)
        ////////////{
        ////////////    bl.FromFoodToIngredient(recipesPage.CurrentFood, bl.Ingredient);
        ////////////    // change the calls because FromClassesToUi() follows and we don't fire events on textboxes
        ////////////    ////////////bl.Ingredient.ChoGrams.Text = "0";
        ////////////    ////////////bl.Ingredient.QuantityGrams.Text = "0";
        ////////////}
        //if (recipesPage != null && recipesPage.RecipeIsChosen)
        //{
        //    //////////bl.FromFoodToIngredient(recipesPage.CurrentFood, bl.Ingredient);
        //    ////////// change the calls because FromClassesToUi() follows and we don't fire events on textboxes
        //    //////////bl.RecipeInRecipe.ChoGrams.Text = "0";
        //    //////////bl.RecipeInRecipe.QuantityGrams.Text = "0";
        //}
        //bl.Recipe.IdBolusCalculation = insulinCalcPage.IdBolusCalculation;
        //if (injectionsPage != null && injectionsPage.IdInsulinInjection != null)
        //    bl.Recipe.IdInsulinInjection = injectionsPage.IdInsulinInjection;
        //if (measurementPage != null && measurementPage.IdGlucoseRecord != null)
        //    bl.Recipe.IdGlucoseRecord = measurementPage.IdGlucoseRecord;

        FromClassesToUi();

        // set focus to a specific field
        // (currently deemed not necessary and commented out)
        // base.OnAppearing();
        // await Task.Delay(1);
        // txtIngredientCarbohydratesPercent.Focus();
    }
    public bool RecipeIsChosen { get; internal set; }
    public Recipe CurrentFood { get; }
    private void btnRemoveIngredient_Click(object sender, EventArgs e)
    {
        bl.DeleteOneIngredient(bl.Ingredient);
    }
    private void btnRecipes_Click(object sender, EventArgs e)
    {

    }
    private void btnIngredient_Click(object sender, EventArgs e)
    {

    }
    private void txtIngredientQuantityGrams_TextChanged(object sender, TextChangedEventArgs e)
    {
        ///////////////bl.RecalcAll();
        FromClassesToUi();
    }

    private void txtIngredientCarbohydratesPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        //bl.Ingredient.
        /////////////////////bl.RecalcAll();
        //FromClassesToUi();
    }
    private void txtIngredientQuantityPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        bl.Ingredient.QuantityGrams.Double = null;
        txtIngredientQuantityGrams.Text = bl.Ingredient.QuantityGrams.Text;
    }
}