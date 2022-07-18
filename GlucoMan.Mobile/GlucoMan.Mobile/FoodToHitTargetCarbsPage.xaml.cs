using GlucoMan.BusinessLayer;
using SharedGlucoMan.BusinessLayer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodToHitTargetCarbsPage : ContentPage
    {
        BL_FoodToHitTargetCarbs blFoodToEat = new BL_FoodToHitTargetCarbs();
        BL_General blg = new BL_General();

        public FoodToHitTargetCarbsPage()
        {
            InitializeComponent();
            // read data from other pages
            blFoodToEat.TargetCho.Double = Safe.Double(blg.RestoreParameter("Bolus_ChoToEat"));
            blFoodToEat.NameOfFood = Safe.String(blg.RestoreParameter("FoodInMeal_Name"));

            blFoodToEat.RestoreParametrs();

            FromClassToUi();
        }
        private void FromClassToUi()
        {
            TxtChoAlreadyTaken.Text = blFoodToEat.ChoAlreadyTaken.Text;
            TxtChoOfFood.Text = blFoodToEat.ChoOfFood.Text;
            TxtTargetCho.Text = blFoodToEat.TargetCho.Text;
            TxtChoLeftToTake.Text = blFoodToEat.ChoLeftToTake.Text;
            TxtFoodToHitTarget.Text = blFoodToEat.FoodToHitTarget.Text;
            TxtFoodName.Text = blFoodToEat.NameOfFood;
        }
        internal void FromUiToClass()
        {
            blFoodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            blFoodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            blFoodToEat.TargetCho.Text = TxtTargetCho.Text;
            blFoodToEat.ChoLeftToTake.Text = TxtChoLeftToTake.Text;
            blFoodToEat.FoodToHitTarget.Text = TxtFoodToHitTarget.Text;
            blFoodToEat.NameOfFood = TxtFoodName.Text;
        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            blFoodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            blFoodToEat.Calculations();
            FromClassToUi();
        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            blFoodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            blFoodToEat.Calculations();
            FromClassToUi();
        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            blFoodToEat.TargetCho.Text = TxtTargetCho.Text;
            blFoodToEat.Calculations();
            FromClassToUi();
        }
        private void btnCalculateGrams_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            blFoodToEat.Calculations();
            FromClassToUi();
        }
        private void btnReadAll_Click(object sender, EventArgs e)
        {
            blFoodToEat.TargetCho.Double = Safe.Double(blg.RestoreParameter("Bolus_ChoToEat"));
            blFoodToEat.ChoAlreadyTaken.Double = Safe.Double(blg.RestoreParameter("XXXXXXXXXXXXXX"));
            blFoodToEat.ChoOfFood.Double = Safe.Double(blg.RestoreParameter("FoodInMeal_ChoPercent"));
            blFoodToEat.NameOfFood = Safe.String(blg.RestoreParameter("FoodInMeal_Name"));
            FromClassToUi();
            TxtChoLeftToTake.Text = "";
            TxtFoodToHitTarget.Text = "";
        }
        private void btnReadTargetCho_Click(object sender, EventArgs e)
        {
            blFoodToEat.TargetCho.Double = Safe.Double(blg.RestoreParameter("Bolus_ChoToEat"));
            FromClassToUi();
        }
        private void btnReadChoTaken_Click(object sender, EventArgs e)
        {
            blFoodToEat.ChoAlreadyTaken.Double = Safe.Double(blg.RestoreParameter("XXXXXXXXXXXXXX"));
            FromClassToUi();
        }
        private void btnReadFood_Click(object sender, EventArgs e)
        {
            blFoodToEat.ChoOfFood.Double = Safe.Double(blg.RestoreParameter("FoodInMeal_ChoPercent"));
            blFoodToEat.NameOfFood = Safe.String(blg.RestoreParameter("FoodInMeal_Name"));
            FromClassToUi();
        }
    }
}