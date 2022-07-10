using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GlucoMan.Common;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealPage : ContentPage
    {
        private BL_MealAndFood bl = new BL_MealAndFood();

        private bool loading = true;

        //Accuracy accuracyMeal;
        //Accuracy accuracyFood;

        public MealPage(Meal Meal)
        {
            InitializeComponent();
            if (Meal == null)
                Meal = new Meal();
            bl.Meal = Meal;
            cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbAccuracyFoodInMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

            if (bl.Meal.IdTypeOfMeal == TypeOfMeal.NotSet)
            {
                bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
            }
            FromClassToUi();
            RefreshGrid();

            loading = false;
        }
        protected override async void OnAppearing()
        {
            // set focus to a specific field
            // (currently deemed not necessaryand commented out)

            // base.OnAppearing();
            // await Task.Delay(1);
            // txtFoodChoPercent.Focus();
        }
        private void RefreshGrid()
        {
            bl.FoodsInMeal = bl.GetFoodsInMeal(bl.Meal.IdMeal);
            gridFoodsInMeal.BindingContext = bl.FoodsInMeal;
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.ChoGrams.Text = txtChoOfMeal.Text;

            bl.Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoMeal.Text;
            if (cmbAccuracyMeal.SelectedItem != null)
                bl.Meal.QualitativeAccuracyOfChoEstimate = ((QualitativeAccuracy)cmbAccuracyMeal.SelectedItem);

            bl.FoodInMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdFoodInMeal.Text);
            bl.FoodInMeal.IdFood = Safe.Int(txtIdFood.Text);
            bl.FoodInMeal.QuantityGrams.Text = txtFoodQuantityGrams.Text; // [g]
            bl.FoodInMeal.ChoPercent.Text = txtFoodChoPercent.Text;
            bl.FoodInMeal.ChoGrams.Text = txtFoodChoGrams.Text;
            bl.FoodInMeal.Name = txtFoodInMealName.Text;

            bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
            if (cmbAccuracyFoodInMeal.SelectedItem != null)
                bl.FoodInMeal.QualitativeAccuracyOfCho = ((QualitativeAccuracy)cmbAccuracyFoodInMeal.SelectedItem);
        }
        private void FromClassToUi()
        {
            loading = true;

            if (bl.Meal.IdMeal != null)
                txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            else
                txtIdMeal.Text = "";
            txtChoOfMeal.Text = bl.Meal.ChoGrams.Text;
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            cmbAccuracyMeal.SelectedItem = bl.Meal.QualitativeAccuracyOfChoEstimate;


            if (bl.FoodInMeal.IdFoodInMeal != null)
                txtIdFoodInMeal.Text = bl.FoodInMeal.IdMeal.ToString();
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
            cmbAccuracyFoodInMeal.SelectedItem = bl.FoodInMeal.QualitativeAccuracyOfCho;
            txtFoodInMealName.Text = bl.FoodInMeal.Name;

            loading = false;
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            // erase Id, so that a new record will be created
            bl.FoodInMeal.IdFoodInMeal = null;
            bl.SaveOneFoodInMeal(bl.FoodInMeal);
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy();
            FromClassToUi();
            RefreshGrid();
        }
        private void btnInsulin_Click(object sender, EventArgs e)
        {
            //////frmInsulinCalc f = new frmInsulinCalc();
            //////f.Show();
        }
        private void btnGlucose_Click(object sender, EventArgs e)
        {
            //////frmGlucose frm = new frmGlucose();
            //////frm.Show();
        }
        private void btnWeighFood_Click(object sender, EventArgs e)
        {
            //////frmWeighFood fw = new frmWeighFood();
            //////fw.ShowDialog();
        }
        private void txtFoodChoPercent_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                bl.FoodInMeal.ChoPercent.Text = txtFoodChoPercent.Text;
                if (bl.FoodInMeal.QuantityGrams.Text != "")
                {
                    bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                    txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
                }
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
        }
        //private async Task btnShowThisMeal_ClickAsync(object sender, EventArgs e)
        //{
        //    // open Meal 
        //    if (txtIdMeal.Text == "")
        //    {
        //        return;
        //    }
        //    await Navigation.PushAsync(new MealPage(bl.Meal));
        //}
        private void btnFoodDetail_Click(object sender, EventArgs e)
        {
            //////////FromUiToClass();
            //////////frmFoods fd = new frmFoods(bl.FoodInMeal);
            //////////fd.ShowDialog();
            //////////if (fd.FoodIsChosen)
            //////////{
            //////////    currentFoodInMeal = bl.FromFoodToFoodInMeal(fd.CurrentFood);
            //////////}
        }
        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            bl.DeleteOneFoodInMeal(bl.FoodInMeal);
            bl.RecalcTotalCho();
            FromClassToUi();
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
            //////make the next function
            //////bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
        }
        private void btnSumCho_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy();
            FromClassToUi();
        }
        private void txtAccuracyOfChoFoodInMeal_TextChanged(object sender, EventArgs e) { }
        private void txtAccuracyOfChoFoodInMeal_Leave(object sender, EventArgs e)
        {
            bl.FoodInMeal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoFoodInMeal.Text);
            bl.RecalcTotalAccuracy();
            FromClassToUi();
        }
        private void cmbAccuracyFoodInMeal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            //////frmFoods f = new frmFoods(txtName.Text);
        }
        private void btnSaveAllMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            txtIdMeal.Text = bl.SaveOneMeal(bl.Meal).ToString();
            bl.SaveAllFoodsInMeal();
            //bl.RecalcTotalAccuracy();
            //bl.RecalcTotalCho();
            //FromClassToUi();
            //RefreshGrid();
        }
        private void btnSaveAllFoods_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.RecalcTotalAccuracy();
            bl.RecalcTotalCho();
            bl.SaveAllFoodsInMeal(); 
            FromClassToUi();
            RefreshGrid();
        }
        private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                //await DisplayAlert("XXXX", "YYYY", "Ok");
                return;
            }
            //make the tapped row the current meal 
            bl.FoodInMeal = (FoodInMeal)gridFoodsInMeal.SelectedItem;
            FromClassToUi();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave_ClickAsync(sender, e);
        }
        private async Task btnSave_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                await DisplayAlert("Saving not possible", "Choose the meal to modify", "Ok");
                return;
            }
            FromUiToClass();
            bl.SaveOneMeal(bl.Meal);
            RefreshGrid();
        }
    }
}