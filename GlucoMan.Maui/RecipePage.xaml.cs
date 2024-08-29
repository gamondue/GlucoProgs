using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class RecipePage : ContentPage
{
    BL_Recipes bl = new BL_Recipes();
    private Accuracy accuracyRecipe;
    private Accuracy accuracyIngredient;
   
    public RecipePage(Recipe Recipe)
    {
        InitializeComponent();

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
        bl.Ingredients = bl.ReadIngredientsOfARecipe(bl.Recipe.IdRecipe);
        gridIngredients.BindingContext = bl.Ingredients;
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private void FromClassToUi()
    {
        //loading = true;
        ShowRecipeBoxes();
        ShowIngredientsBoxes();
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
        Recipe.Description = txtDescription.Text;
        Recipe.CarbohydratesPercent.Text = txtChoOfRecipePercent.Text;
        Recipe.AccuracyOfChoEstimate.Text = txtAccuracyOfChoRecipe.Text;
    }
    private void ShowIngredientsBoxes()
    {
        if(bl.Ingredient == null)
        {
            bl.Ingredient = new Ingredient();
        }
        //////////if (bl.Ingredient.IdIngredient != null)
        //////////    txtIdRecipe.Text = bl.Ingredient.IdIngredient.ToString();
        //////////else
        //////////    txtIdRecipe.Text = "";

        txtIngredientCarbohydratesPercent.Text = bl.Ingredient.CarbohydratesPercent.Text;
        txtIngredientQuantityGrams.Text = bl.Ingredient.QuantityGrams.Text;
        txtIngredientChoGrams.Text = bl.Ingredient.CarbohydratesPercent.Text;
        txtAccuracyOfChoIngredient.Text = bl.Ingredient.AccuracyOfChoEstimate.Text;
        //txtAccuracyOfChoRecipe.Text = bl.Ingredient.AccuracyOfChoEstimate.Text;
        txtIngredientName.Text = bl.Ingredient.Name;
    }
    private void ShowRecipeBoxes()
    {
        txtIdRecipe.Text = bl.Recipe.IdRecipe.ToString();
        txtChoOfRecipePercent.Text = bl.Recipe.CarbohydratesPercent.Text;

        if (bl.Recipe.IdRecipe != null)
            txtIdRecipe.Text = bl.Recipe.IdRecipe.ToString();
        else
            txtIdRecipe.Text = "";

        //////////txtAccuracyOfChoRecipe.Text = bl.Recipe.AccuracyOfChoEstimate.Text;
        txtDescription.Text = bl.Recipe.Description;
    }
    Recipe localIngredientForCalculations = new Recipe();
    private void txtIngredientCarbohydratesPercent_TextChanged(object sender, EventArgs e)
    {
        //if (!loading)
        {
            ////////FromUiToIngredient(localIngredientForCalculations);
            ////////bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
            txtIngredientChoGrams.Text = localIngredientForCalculations.CarbohydratesPercent.Text;
            bl.SaveRecipeParameters();
        }
    }
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
        //ShowRecipeBoxes();
        //txtChoOfRecipePercent.Text = bl.Recipe.Carbohydrates.Text;
    }
    private void txtChoOfRecipePercent_TextChanged(object sender, EventArgs e)
    {
        bl.SaveRecipeParameters();
    }
    private void btnSaveAllRecipe_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        //txtIdRecipe.Text = bl.SaveOneIngredient(bl.Ingredient).ToString();
        bl.SaveAllIngredientsInRecipe();
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
        ShowRecipeBoxes();
    }
    private void btnRemoveRecipe_Click(object sender, EventArgs e)
    {
        bl.DeleteOneRecipe(bl.Recipe);
        RefreshGrid();
        bl.RecalcAll();
        ShowRecipeBoxes();
    }
    private async void btnIngredient_ClickAsync(object sender, EventArgs e)
    {
        FromUiToClasses();
        ////////recipesPage = new RecipesPage(bl.Recipe);
        ////////await Navigation.PushAsync(recipesPage);
    }
    //private void btnRecipes_ClickAsync(object sender, EventArgs e)
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
    //    FromClassToUi();
    //    RefreshGrid();
    //}
    private void btnSaveAllFoods_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.SaveOneIngredient(bl.Ingredient).ToString();
        bl.SaveAllIngredientsInRecipe();
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
        //////////txtRecipeName.Text = "";
        FromUiToIngredient(bl.Ingredient);
    }
    private void btnCalc_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.RecalcAll();
        FromClassToUi();
    }
    private async void btnSearchFood_ClickAsync(object sender, EventArgs e)
    {
        ////////await Navigation.PushAsync(new FoodsPage(txtRecipeName.Text, ""));
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
    bool firstPass = true;
    private async void gridIngredients_Unfocused(object sender, SelectedItemChangedEventArgs e)
    {
        FromUiToClasses();
        bl.SaveAllIngredientsInRecipe();
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
        Recipe dummy;
        //////////if (!firstPass && !previousRecipe.DeepEquals(bl.Ingredient, out dummy))
        //////////{
        //////////    firstPass = false;
        //////////    string[] options = { "Save the changes", "Make a new food" };
        //////////    string chosen = await DisplayActionSheet("The food has changed. Should we:",
        //////////        "Abort the changes", null, options);
        //////////    if (chosen == "Save the changes")
        //////////    {
        //////////        bl.SaveOneIngredient(bl.Ingredient);
        //////////        RefreshGrid();
        //////////    }
        //////////    else if (chosen == "Make a new food")
        //////////    {
        //////////        bl.Ingredient.IdIngredient = null;
        //////////        bl.SaveOneIngredient(bl.Ingredient);
        //////////        RefreshGrid();
        //////////    }
        //////////    else if (chosen == "Abort the changes" || chosen == null)
        //////////    {
        //////////        // nothing 
        //////////    }
        //////////}

        //loading = true;
        //make the tapped row the current food in Recipe 
        ////////bl.Ingredient = (Recipe)e.SelectedItem;
        FromClassToUi();
        bl.SaveIngredientParameters();
        //loading = false;
    }
    protected override async void OnAppearing()
    {
        ////////////if (recipesPage != null && recipesPage.FoodIsChosen)
        ////////////{
        ////////////    bl.FromFoodToIngredient(recipesPage.CurrentFood, bl.Ingredient);
        ////////////    // change the calls because FromClassToUi() follows and we don't fire events on textboxes
        ////////////    ////////////bl.Ingredient.ChoGrams.Text = "0";
        ////////////    ////////////bl.Ingredient.QuantityGrams.Text = "0";
        ////////////}
        //if (recipesPage != null && recipesPage.RecipeIsChosen)
        //{
        //    //////////bl.FromFoodToIngredient(recipesPage.CurrentFood, bl.Ingredient);
        //    ////////// change the calls because FromClassToUi() follows and we don't fire events on textboxes
        //    //////////bl.RecipeInRecipe.ChoGrams.Text = "0";
        //    //////////bl.RecipeInRecipe.QuantityGrams.Text = "0";
        //}
        //bl.Recipe.IdBolusCalculation = insulinCalcPage.IdBolusCalculation;
        //if (injectionsPage != null && injectionsPage.IdInsulinInjection != null)
        //    bl.Recipe.IdInsulinInjection = injectionsPage.IdInsulinInjection;
        //if (measurementPage != null && measurementPage.IdGlucoseRecord != null)
        //    bl.Recipe.IdGlucoseRecord = measurementPage.IdGlucoseRecord;

        FromClassToUi();

        // set focus to a specific field
        // (currently deemed not necessary and commented out)
        // base.OnAppearing();
        // await Task.Delay(1);
        // txtIngredientCarbohydratesPercent.Focus();
    }

    ////////////List<Ingredient> ingredients = bl.ReadIngredientsOfARecipe(1);
    ////////////    if (ingredients.Count == 0)
    ////////////    {
    ////////////        Ingredient ingredient = new Ingredient();
    ////////////        ingredient.Name = "Zucchero";
    ////////////        ingredient.QuantityGrams.Double = 10;
    ////////////        ingredient.IdRecipe = 1;
    ////////////        ingredient.QuantityGrams.Double = 10;
    ////////////        ingredient.CarbohydratesPercent.Double = 100;
    ////////////        ingredient.IdFood = 1;
    ////////////        int? index = bl.SaveOneIngredient(ingredient);
    ////////////        // create another recipe ingredient for the first recipe
    ////////////        ingredient = new Ingredient();
    ////////////        ingredient.Name = "Mele";
    ////////////        ingredient.IdRecipe = 1;
    ////////////        ingredient.QuantityGrams.Double = 500;
    ////////////        ingredient.CarbohydratesPercent.Double = 15;
    ////////////        ingredient.IdFood = 2;
    ////////////        index = bl.SaveOneIngredient(ingredient);

    ////////////        // create a recipe ingredient for the second recipe
    ////////////        ingredient = new Ingredient();
    ////////////        ingredient.Name = "Farina tipo 0";
    ////////////        ingredient.QuantityGrams.Double = 100;
    ////////////        ingredient.IdRecipe = 2;
    ////////////        ingredient.QuantityGrams.Double = 70;
    ////////////        ingredient.CarbohydratesPercent.Double = 80;
    ////////////        ingredient.IdFood = 3;
    ////////////        index = bl.SaveOneIngredient(ingredient);
    ////////////        // create another recipe ingredient for the second recipe
    ////////////        ingredient = new Ingredient();
    ////////////        ingredient.Name = "Mozzarella";
    ////////////        ingredient.QuantityGrams.Double = 100;
    ////////////        ingredient.IdRecipe = 2;
    ////////////        ingredient.QuantityPercent.Double = 30;
    ////////////        ingredient.CarbohydratesPercent.Double = 0;
    ////////////        ingredient.IdFood = 4;
    ////////////        index = bl.SaveOneIngredient(ingredient);
    ////////////    }
    ////////////    ingredients = bl.ReadIngredientsOfARecipe(1);
    ////////////}
    public bool RecipeIsChosen { get; internal set; }
    public Recipe CurrentFood { get; }
    private void btnRemoveIngredient_Click(object sender, EventArgs e)
    {

    }
    private void btnRecipes_ClickAsync(object sender, EventArgs e)
    {

    }
}