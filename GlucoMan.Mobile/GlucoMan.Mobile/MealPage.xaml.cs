using GlucoMan.BusinessLayer;
using static GlucoMan.Common;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using gamon;
using System.Collections.Generic;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealPage : ContentPage
    {
        // since it is accessed by several pages, to avoid "concurrent" problems 
        // we use a common business layer beetween different pages
        private BL_MealAndFood bl = Common.MealAndFood_CommonBL;

        private bool loading = true;

        private Accuracy accuracyMeal;
        private Accuracy accuracyFoodInMeal;

        FoodsPage foodsPage;
        InsulinCalcPage insulinCalcPage;
        InjectionsPage injectionsPage;
        GlucoseMeasurementsPage measurementPage; 

        public MealPage(Meal Meal)
        {
            InitializeComponent();
            
            loading = true;
            if (Meal == null)
            {
                Meal = new Meal();
                btnDefaults_Click(null, null);
            }
            bl.Meal = Meal;
            
            cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbAccuracyFoodInMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

            accuracyMeal = new Accuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);
            accuracyFoodInMeal = new Accuracy(txtAccuracyOfChoFoodInMeal, cmbAccuracyFoodInMeal);

            if (bl.Meal.IdTypeOfMeal == TypeOfMeal.NotSet)
            {
                bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
            }
            RefreshUi();

            loading = false;
        }
        private void RefreshGrid()
        {
            bl.FoodsInMeal = bl.GetFoodsInMeal(bl.Meal.IdMeal);
            gridFoodsInMeal.BindingContext = bl.FoodsInMeal;
        }
        private void RefreshUi()
        {
            FromClassToUi(); 
            RefreshGrid();
        }
        private void FromClassToUi()
        {
            loading = true;
            ShowMealBoxes();
            ShowFoodBoxes();
            loading = false;
        }
        private void FromUiToClass()
        {
            loading = true;

            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.Carbohydrates.Text = txtChoOfMealGrams.Text;
            bl.Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoMeal.Text;
            bl.Meal.Notes = txtNotes.Text;

            bl.FoodInMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdFoodInMeal.Text);
            bl.FoodInMeal.IdFood = Safe.Int(txtIdFood.Text);
            bl.FoodInMeal.QuantityGrams.Text = txtFoodQuantityGrams.Text; // [g]
            bl.FoodInMeal.ChoPercent.Text = txtFoodChoPercent.Text;
            bl.FoodInMeal.ChoGrams.Text = txtFoodChoGrams.Text;
            bl.FoodInMeal.Name = txtFoodInMealName.Text;
            bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;

            loading = false;
        }
        private void ShowFoodBoxes()
        {
            if (bl.FoodInMeal.IdFoodInMeal != null)
                txtIdFoodInMeal.Text = bl.FoodInMeal.IdFoodInMeal.ToString();
            else
                txtIdFoodInMeal.Text = "";
            if (bl.FoodInMeal.IdFood != null)
                txtIdFood.Text = bl.FoodInMeal.IdFood.ToString();
            else
                txtIdFood.Text = "";

            txtFoodChoPercent.Text = bl.FoodInMeal.ChoPercent.Text;
            txtFoodQuantityGrams.Text = bl.FoodInMeal.QuantityGrams.Text;
            txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
            txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
            txtFoodInMealName.Text = bl.FoodInMeal.Name;
        }
        private void ShowMealBoxes()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMealGrams.Text = bl.Meal.Carbohydrates.Text;

            if (bl.Meal.IdMeal != null)
                txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            else
                txtIdMeal.Text = "";

            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            txtNotes.Text = bl.Meal.Notes;
        }
        private void txtFoodChoPercent_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                bl.FoodInMeal.ChoPercent.Text = txtFoodChoPercent.Text; 
                bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
                bl.SaveFoodInMealParameters();
            }
        }
        private void txtFoodQuantityGrams_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                bl.FoodInMeal.QuantityGrams.Text = txtFoodQuantityGrams.Text;
                bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
            }
        }
        private void txtFoodChoGrams_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                if (!txtFoodQuantityGrams.IsFocused && !txtFoodChoPercent.IsFocused)
                {
                    txtFoodQuantityGrams.Text = "";
                    bl.FoodInMeal.QuantityGrams.Double = 0;
                    txtFoodChoPercent.Text = "";
                    bl.FoodInMeal.ChoPercent.Double = 0;
                }
            }
            bl.FoodInMeal.ChoGrams.Text = txtFoodChoGrams.Text;
            bl.RecalcAll();
            ShowMealBoxes();
            txtChoOfMealGrams.Text = bl.Meal.Carbohydrates.Text;
        }
        private void txtChoOfMealGrams_TextChanged(object sender, EventArgs e)
        {
            bl.SaveMealParameters();
        }
        private void btnSaveAllMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            if (bl.Meal.TimeBegin.DateTime == General.DateNull)
                // if the meal has no time, we put Now
                txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, true).ToString();
            else
                // if the meal has already a time, we don't touch it  
                txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, false).ToString();
            txtIdFoodInMeal.Text = bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
            bl.SaveAllFoodsInMeal();
        }
        private void btnAddFoodInMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            // erase Id of FoodInMeal, so that a new record will be created
            bl.FoodInMeal.IdFoodInMeal = null;
            txtIdFoodInMeal.Text = bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
            if (bl.FoodsInMeal == null)
                bl.FoodsInMeal = new List<FoodInMeal>();
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
            FromUiToClass();
            foodsPage = new FoodsPage(bl.FoodInMeal); 
            await Navigation.PushAsync(foodsPage);
        }
        // in this UI we have no buttons to save just one food in meal 
        //private void btnSaveFoodInMeal_Click(object sender, EventArgs e)
        //{
        //    if (gridFoodsInMeal.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("Choose a food to save");
        //        return;
        //    }
        //    FromUiToClass();
        //    bl.SaveOneFoodInMeal(bl.FoodInMeal);
        //    FromClassToUi();
        //    RefreshGrid();
        //}
        private void btnSaveAllFoods_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
            bl.SaveAllFoodsInMeal();
            RefreshGrid();
        }
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            txtFoodChoPercent.Text = "";
            txtFoodQuantityGrams.Text = "";
            txtFoodChoGrams.Text = "";
            txtAccuracyOfChoFoodInMeal.Text = "";
            cmbAccuracyFoodInMeal.SelectedItem = null;
            txtIdFoodInMeal.Text = "";
            txtIdFood.Text = "";
            txtFoodInMealName.Text = "";
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.RecalcAll();
            FromClassToUi();
        }
        private async void btnSearchFood_ClickAsync(object sender, EventArgs e)
        {
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
            injectionsPage = new InjectionsPage(bl.Meal.IdInsulinInjection);
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
        private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                //await DisplayAlert("XXXX", "YYYY", "Ok");
                return;
            }
            loading = true;
            //make the tapped row the current food in meal 
            bl.FoodInMeal = (FoodInMeal)e.SelectedItem;
            FromClassToUi();
            bl.SaveFoodInMealParameters();
            loading = false;
        }
        protected override async void OnAppearing()
        {
            if (foodsPage != null && foodsPage.FoodIsChosen)
            {
                bl.FromFoodToFoodInMeal(foodsPage.CurrentFood, bl.FoodInMeal);
                loading = true;
                txtFoodChoGrams.Text = "0";
                txtChoOfMealGrams.Text = "0";
                loading = false;
            }
            //bl.Meal.IdBolusCalculation = insulinCalcPage.IdBolusCalculation;
            if (injectionsPage != null && injectionsPage.IdInsulinInjection != null)
                bl.Meal.IdInsulinInjection = injectionsPage.IdInsulinInjection;
            if (measurementPage != null && measurementPage.IdGlucoseRecord != null)
                bl.Meal.IdGlucoseRecord = measurementPage.IdGlucoseRecord;

            FromClassToUi();

            // set focus to a specific field
            // (currently deemed not necessary and commented out)
            // base.OnAppearing();
            // await Task.Delay(1);
            // txtFoodChoPercent.Focus();
        }
    }
}