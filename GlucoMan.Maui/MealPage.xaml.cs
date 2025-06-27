using gamon;
using GlucoMan.BusinessLayer;
using System.ComponentModel;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class MealPage : ContentPage
{
    // since it is accessed by several pages, to avoid "concurrent" problems 
    // we use a common business layer beetween different pages
    private BL_MealAndFood bl = Common.MealAndFood_CommonBL;

    private bool loadingUi = true;
    private bool firstPass = true;
    //private bool editingNumericAccuracy = false;

    private UiAccuracy accuracyMeal;
    private UiAccuracy accuracyFoodInMeal;

    FoodsPage foodsPage;
    RecipesPage recipesPage;

    InsulinCalcPage insulinCalcPage;
    InjectionsPage injectionsPage;
    GlucoseMeasurementsPage measurementPage;

    private Color initialButtonBackground = Colors.White;
    private Color initalButtonTextColor=Colors.Black;

    public MealPage(Meal Meal)
    {
        InitializeComponent();

        bl.FoodsInMeal = bl.GetFoodsInMeal(bl.Meal.IdMeal);
        gridFoodsInMeal.BindingContext = bl.FoodsInMeal;

        loadingUi = true;
        if (Meal == null)
        {
            Meal = new Meal();
            btnDefaults_Click(null, null);
        }
        bl.Meal = Meal;
        mealSection.BindingContext = bl.Meal;

        initialButtonBackground = btnStartMeal.BackgroundColor;
        initalButtonTextColor = btnStartMeal.TextColor;
        if (bl.Meal.IdMeal == null || (bl.Meal.TimeBegin.DateTime + new TimeSpan(0, 45, 0) > DateTime.Now))
        { 
            btnStartMeal.BackgroundColor = Colors.Red;
            btnStartMeal.TextColor = Colors.Yellow;
        }

        cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        cmbAccuracyFoodInMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

        accuracyMeal = new UiAccuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);
        accuracyFoodInMeal = new UiAccuracy(txtAccuracyOfChoFoodInMeal, cmbAccuracyFoodInMeal);

        if (bl.Meal.IdTypeOfMeal ==  null || bl.Meal.IdTypeOfMeal == TypeOfMeal.NotSet)
        {
            bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
        }
        RefreshUi();

        loadingUi = false;
    }
    private void RefreshUi()
    {
        RefreshMeal();
        RefreshFood();
        RefreshGrid();
    }
    private void FromClassToUi()
    {
        loadingUi = true;
        ShowMealBoxes();
        ShowFoodBoxes();
        loadingUi = false;
    }
    private void RefreshMeal()
    {
        mealSection.BindingContext = null;
        mealSection.BindingContext = bl.Meal;
    }
    private void RefreshFood()
    {
        foodSection.BindingContext = null;
        foodSection.BindingContext = bl.FoodInMeal;
    }
    private void RefreshGrid()
    {
        gridFoodsInMeal.BindingContext = null;
        gridFoodsInMeal.BindingContext = bl.FoodsInMeal;
    }
    //private void FromUiToClasses()
    //{
    //    return; 
    //    loadingUi = true;
    //    FromUiToMeal(bl.Meal);
    //    //FromUiToFood(bl.FoodInMeal);
    //    loadingUi = false;
    //}
    //private void FromUiToMeal(Meal Meal)
    //{
    //    return; 
    //    Meal.IdMeal = Safe.Int(txtIdMeal.Text);
    //    Meal.CarbohydratesGrams.Text = txtMealCarbohydratesGrams.Text;
    //    Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoMeal.Text;
    //    Meal.Notes = txtNotes.Text;
    //}
    private void ShowFoodBoxes()
    {
        // keep the followiong commented lines in case we need to show the Ids again
        //if (bl.FoodInMeal.IdFoodInMeal != null)
        //    txtIdFoodInMeal.Text = bl.FoodInMeal.IdFoodInMeal.ToString();
        //else
        //    txtIdFoodInMeal.Text = "";
        // IdFood is currently not shown
        //if (bl.FoodInMeal.IdFood != null)
        //    txtIdFood.Text = bl.FoodInMeal.IdFood.ToString();
        //else
        //    txtIdFood.Text = "";

        ////////////txtFoodCarbohydratesPercent.Text = bl.FoodInMeal.CarbohydratesPercent.Text;
        ////////////txtFoodQuantityInUnits.Text = bl.FoodInMeal.QuantityInUnits.Text;
        ////////////txtFoodCarbohydratesGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
        ////////////txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
        ////////////txtFoodInMealName.Text = bl.FoodInMeal.Name;
        ////////////btnUnit.Text = bl.FoodInMeal.UnitSymbol;
    }
    private void ShowMealBoxes()
    {
        //txtIdMeal.Text = bl.Meal.IdMeal.ToString();
        //txtMealCarbohydratesGrams.Text = bl.Meal.CarbohydratesGrams.Text;

        //if (bl.Meal.IdMeal != null)
        //    txtIdMeal.Text = bl.Meal.IdMeal.ToString();
        //else
        //    txtIdMeal.Text = "";
        //txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
        //txtNotes.Text = bl.Meal.Notes;
    }
    private void btnSaveAllMeal_Click(object sender, EventArgs e)
    {
        //FromUiToMeal(bl.Meal);
        //////if (bl.Meal.TimeBegin.DateTime == General.DateNull)
        //////    // if the meal has no time, we put Now
        //////    txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, true).ToString();
        //////else
        //////    txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, false).ToString();
        // keep the following commented lines in case we need to show the Ids again
        //txtIdFoodInMeal.Text = bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
        // refresh the bindings by focusing a non focusable control 
        ////txtIdMeal.Focus();
        bl.SaveAllFoodsInMeal();
    }
    private void btnAddFoodInMeal_Click(object sender, EventArgs e)
    {
        //FromUiToClasses();
        // erase Id of FoodInMeal, so that a new record will be created
        bl.FoodInMeal.IdFoodInMeal = null;
        // keep the following commented lines in case we need to show the Ids again
        //txtIdFoodInMeal.Text = bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
        if (bl.FoodsInMeal == null)
            bl.FoodsInMeal = new List<FoodInMeal>();
        bl.SaveOneFoodInMeal(bl.FoodInMeal);
        RefreshGrid();
        bl.RecalcAll();
        ShowMealBoxes();
    }
    private void btnRemoveFoodInMeal_Click(object sender, EventArgs e)
    {
        bl.DeleteOneFoodInMeal(bl.FoodInMeal);
        RefreshGrid();
        bl.RecalcAll();
        ShowMealBoxes();
    }
    private async void btnFoodDetail_ClickAsync(object sender, EventArgs e)
    {
        //FromUiToClasses();
        foodsPage = new FoodsPage(bl.FoodInMeal);
        await Navigation.PushAsync(foodsPage);
    }
    private void btnRecipes_ClickAsync(object sender, EventArgs e)
    {
        //FromUiToClasses();
        recipesPage = new RecipesPage(null);
        Navigation.PushAsync(recipesPage);
    }
    // in this version of the UI we have no buttons to save just one food in meal 
    //private void btnSaveFoodInMeal_Click(object sender, EventArgs e)
    //{
    //    if (gridFoodsInMeal.SelectedRows.Count == 0)
    //    {
    //        MessageBox.Show("Choose a food to save");
    //        return;
    //    }
    //    FromUiToCurrentRecipe();
    //    bl.SaveOneFoodInMeal(bl.FoodInMeal);
    //    FromClassesToUi();
    //    RefreshGrid();
    //}
    private void btnSaveAllFoods_Click(object sender, EventArgs e)
    {
        //FromUiToClasses();
        bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
        bl.SaveAllFoodsInMeal();
        RefreshGrid();
    }
    private void btnDefaults_Click(object sender, EventArgs e)
    {
        txtFoodCarbohydratesPercent.Text = "";
        txtFoodQuantityInUnits.Text = "";
        txtFoodCarbohydratesGrams.Text = "";
        txtAccuracyOfChoFoodInMeal.Text = "";
        cmbAccuracyFoodInMeal.SelectedItem = null;
        // keep the following commented lines in case we need to show the Ids again
        //txtIdFoodInMeal.Text = "";
        //txtIdFood.Text = "";
        txtFoodInMealName.Text = "";
    }
    private void btnCalc_Click(object sender, EventArgs e)
    {
        //FromUiToClasses();
        bl.RecalcAll();
        // refresh the bound UI data related to the Mea, since the it has cjanged
        mealSection.BindingContext = null;
        mealSection.BindingContext = bl.Meal;

        //FromClassToUi();
    }
    private async void btnSearchFood_ClickAsync(object sender, EventArgs e)
    {
        // !!!! TODO pass the current food in meal
        await Navigation.PushAsync(new FoodsPage(txtFoodInMealName.Text, ""));
    }
    private async void btnInsulinCalc_ClickAsync(object sender, EventArgs e)
    {
        //insulinCalcPage = new InsulinCalcPage(bl.Meal.IdBolusCalculation);
        insulinCalcPage = new InsulinCalcPage();
        await Navigation.PushAsync(insulinCalcPage);
    }
    private async void btnGlucose_ClickAsync(object sender, EventArgs e)
    {
        measurementPage = new GlucoseMeasurementsPage(bl.Meal.IdGlucoseRecord);
        await Navigation.PushAsync(measurementPage);
    }
    private async void btnInjection_ClickAsync(object sender, EventArgs e)
    {
        injectionsPage = new InjectionsPage(bl.Meal.IdInjection);
        await Navigation.PushAsync(injectionsPage);
    }
    private async void btnWeighFood_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WeighFoodPage());
    }
    private async void btnFoodCalc_ClickAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FoodToHitTargetCarbsPage());
    }
    private void btnStartMeal_Click(object sender, EventArgs e)
    {
        //FromUiToClasses();
        if (bl.Meal.IdMeal != null)
            bl.SaveOneMeal(bl.Meal, true); // saves with time now 
        btnStartMeal.BackgroundColor = initialButtonBackground;
        btnStartMeal.TextColor = initalButtonTextColor;
        btnStartMeal.ImageSource = "chronograph_started.png"; 
        RefreshUi();
    }
    private async void gridFoodsInMeal_Unfocused(object sender, SelectedItemChangedEventArgs e)
    {
        //FromUiToClasses();
        bl.SaveAllFoodsInMeal();
    }
    private async void gridFoodsInMeal_CellClick(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        bl.FoodInMeal = (FoodInMeal)e.SelectedItem;
        foodSection.BindingContext = bl.FoodInMeal;
        //FoodInMeal previousFoodInMeal = bl.FoodInMeal.DeepCopy();
        //FromUiToClasses();
        //FoodInMeal dummy;
        //if (!firstPass && !previousFoodInMeal.DeepEquals(bl.FoodInMeal, out dummy))
        //{
        //    firstPass = false;
        //    string[] options = { "Save the changes", "Make a new food" };
        //    string chosen = await DisplayActionSheet("The food has changed. Should we:",
        //        "Escape the changes", null, options);
        //    if (chosen == "Save the changes")
        //    {
        //        bl.SaveOneFoodInMeal(bl.FoodInMeal);
        //        RefreshGrid();
        //    }
        //    else if (chosen == "Make a new food")
        //    {
        //        bl.FoodInMeal.IdFoodInMeal = null;
        //        bl.SaveOneFoodInMeal(bl.FoodInMeal);
        //        RefreshGrid();
        //    }
        //    else if (chosen == "Escape the changes" || chosen == null)
        //    {
        //        // nothing 
        //    }
        //}
        //loadingUi = true;
        ////make the tapped row the current food in meal 
        //bl.FoodInMeal = (FoodInMeal)e.SelectedItem;
        //FromClassToUi();
        //bl.SaveFoodInMealParameters();
        //loadingUi = false;
    }
    private void gridFoodsInMeal_Unfocused(object sender, FocusEventArgs e)
    {

    }
    protected override async void OnAppearing()
    {
        if (foodsPage != null && foodsPage.FoodIsChosen)
        {
            bl.FromFoodToFoodInMeal(foodsPage.CurrentFood, bl.FoodInMeal);
            bl.FoodInMeal.CarbohydratesGrams.Text = "0";
            bl.FoodInMeal.QuantityInUnits.Text = "0";
        }
        if (recipesPage != null && recipesPage.RecipeIsChosen)
        {
            bl.FromRecipeToFoodInMeal(recipesPage.CurrentRecipe, bl.FoodInMeal);
            bl.FoodInMeal.CarbohydratesGrams.Text = "0";
            bl.FoodInMeal.QuantityInUnits.Text = "0";
        }
        //bl.Meal.IdBolusCalculation = insulinCalcPage.IdBolusCalculation;
        if (injectionsPage != null && injectionsPage.IdInjection != null)
            bl.Meal.IdInjection = injectionsPage.IdInjection;
        if (measurementPage != null && measurementPage.IdGlucoseRecord != null)
            bl.Meal.IdGlucoseRecord = measurementPage.IdGlucoseRecord;

        FromClassToUi();

        // set focus to a specific field
        // (currently deemed not necessary and commented out)
        // base.OnAppearing();
        // await Task.Delay(1);
        // txtFoodCarbohydratesPercent.Focus();
    }
    //private void txtFoodChoOrQuantity_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    if (!loadingUi)
    //    {
    //        bl.FoodInMeal.QuantityInUnits.Text = txtFoodQuantityInUnits.Text;
    //        ////////bl.FoodInMeal.CarbohydratesPercent.Text = txtFoodCarbohydratesPercent.Text;
    //        bl.CalculateChoOfFoodGrams();
    //        //bl.SaveFoodInMealParameters();
    //        // refresh the data in the binding
    //        mealSection.BindingContext = bl.Meal;
    //        loadingUi = true;
    //        //txtFoodCarbohydratesGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
    //        //loadingUi = false;
    //    }
    //}
    //private void txtFoodCarbohydratesGrams_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    if (!loadingUi)
    //    {
    //        //loadingUi = true;
    //        //txtFoodQuantityInUnits.Text = "";
    //        //txtFoodCarbohydratesPercent.Text = "";
    //        //loadingUi = false;
    //        bl.FoodInMeal.QuantityInUnits.Double = 0;
    //        bl.FoodInMeal.CarbohydratesPercent.Double = 0;
    //        bl.FoodInMeal.CarbohydratesGrams.Text = txtFoodCarbohydratesGrams.Text;
    //    }
    //}
    //private void txtCarbohydrates_TextChanged(object sender, EventArgs e)
    //{
    //    bl.SaveMealParameters();
    //}
    //private void txtCarbohydrates_Unfocused(object sender, FocusEventArgs e)
    //{
    //    editingNumericAccuracy = false;
    //}
    //private void txtAccuracyOfChoFoodInMeal_Unfocused(object sender, FocusEventArgs e)
    //{
    //    editingNumericAccuracy = false;
    //    cmbAccuracyFoodInMeal.SelectedItem = accuracyFoodInMeal.GetQualitativeAccuracyGivenQuantitavive(
    //        double.Parse(txtAccuracyOfChoFoodInMeal.Text));
    //}
    //private void txtAccuracyOfChoMeal_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    editingNumericAccuracy = true;
    //    if (Safe.Int(txtAccuracyOfChoMeal.Text) != null)
    //    {
    //        cmbAccuracyMeal.SelectedItem = accuracyFoodInMeal.GetQualitativeAccuracyGivenQuantitavive(
    //        double.Parse(txtAccuracyOfChoMeal.Text));
    //    }
    //}
    private Entry GetFocusedEntry()
    {
        if (txtFoodQuantityInUnits.IsFocused) return txtFoodQuantityInUnits;
        if (txtFoodCarbohydratesPercent.IsFocused) return txtFoodCarbohydratesPercent;
        if (txtFoodCarbohydratesGrams.IsFocused) return txtFoodCarbohydratesGrams;
        if (txtAccuracyOfChoFoodInMeal.IsFocused) return txtAccuracyOfChoFoodInMeal;
        if (txtMealCarbohydratesGrams.IsFocused) return txtMealCarbohydratesGrams;
        if (txtAccuracyOfChoMeal.IsFocused) return txtAccuracyOfChoMeal;
        return null;
    }
    private async void Calculator_Click(object sender, TappedEventArgs e)
    {
        //// save the entries in the classes
        //FromUiToClasses();

        var focusedEntry = GetFocusedEntry();
        string sValue = focusedEntry?.Text ?? "0";
        double dValue = double.TryParse(sValue, out var val) ? val : 0;

        // start the CalculatorPage passing to it the value of the 
        // control that currently has the focus
        var calculator = new CalculatorPage(dValue);
        await Navigation.PushModalAsync(calculator);
        var result = await calculator.ResultSource.Task;

        //////////// check if the page has given back a result
        //////////if (result.HasValue)
        //////////{
        //////////    // update the data model
        //////////    if (focusedEntry == txtMealCarbohydratesGrams)
        //////////    {
        //////////        //bl.FoodInMeal.CarbohydratesGrams.Text = result.Value.ToString();
        //////////        txtMealCarbohydratesGrams.Text = result.Value.ToString();
        //////////        //txtMealCarbohydratesGrams_TextChanged(null, null);
        //////////    }
        //////////    else if (focusedEntry == txtFoodCarbohydratesPercent)
        //////////    {
        //////////        //bl.FoodInMeal.CarbohydratesPercent.Text = result.Value.ToString();
        //////////        txtFoodCarbohydratesPercent.Text = result.Value.ToString();
        //////////        //txtFoodChoOrQuantity_TextChanged(null, null);
        //////////    }
        //////////    else if (focusedEntry == txtFoodQuantityInUnits)
        //////////    {
        //////////        //bl.FoodInMeal.QuantityInUnits.Text = result.Value.ToString();
        //////////        txtFoodQuantityInUnits.Text = result.Value.ToString();
        //////////        //txtFoodChoOrQuantity_TextChanged(null, null);
        //////////    }
        //////////    else if (focusedEntry == txtFoodCarbohydratesGrams)
        //////////    {
        //////////        //bl.FoodInMeal.CarbohydratesPercent.Text = result.Value.ToString();
        //////////        txtFoodCarbohydratesGrams.Text = result.Value.ToString();
        //////////        //txtFoodCarbohydratesGrams_TextChanged(null, null);
        //////////    }
        //////////    // show the UI starting from the classes
        //////////    FromClassToUi();
        //////////}
    }
    private void ResetUnitToGrams(object sender, EventArgs e)
    {
        bl.FoodInMeal.UnitSymbol = "g";
        bl.FoodInMeal.GramsInOneUnit.Double = 1;
        RefreshUi();
    }
    private void txtChoOrQuantity_Unfocused(object sender, FocusEventArgs e)
    {
        bl.CalculateChoOfFoodGrams();
        //bl.SaveFoodInMealParameters();
        // refresh the data with the binding
        foodSection.BindingContext = null;
        foodSection.BindingContext = bl.FoodInMeal;
    }
    private void txtFoodCarbohydratesGrams_Unfocused(object sender, FocusEventArgs e)
    {
        if (!loadingUi)
        {
            bl.FoodInMeal.QuantityInUnits.Double = 0;
            bl.FoodInMeal.CarbohydratesPercent.Double = 0;
            // refresh the data with the binding
            foodSection.BindingContext = null;
            foodSection.BindingContext = bl.FoodInMeal;
            gridFoodsInMeal.BindingContext = null;
            gridFoodsInMeal.BindingContext = bl.FoodsInMeal;
        }
    }
}