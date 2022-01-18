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
        BL_FoodToHitTargetCarbs foodToEat = new BL_FoodToHitTargetCarbs();
        public FoodToHitTargetCarbsPage()
        {
            InitializeComponent();

            foodToEat.RestoreData();
            FromClassToUi();
        }
        private void frmFoodToHitTargetCarbs_Load(object sender, EventArgs e)
        {

        }
        private void FromClassToUi()
        {
            TxtChoAlreadyTaken.Text = foodToEat.Hit_ChoAlreadyTaken.Text;
            TxtChoOfFood.Text = foodToEat.Hit_ChoOfFood.Text;
            TxtTargetCho.Text = foodToEat.Hit_TargetCho.Text;
            TxtChoLeftToTake.Text = foodToEat.ChoLeftToTake.Text;
            TxtFoodToHitTarget.Text = foodToEat.FoodToHitTarget.Text;
        }
        internal void FromUiToClass()
        {
            foodToEat.Hit_ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            foodToEat.Hit_ChoOfFood.Text = TxtChoOfFood.Text;
            foodToEat.Hit_TargetCho.Text = TxtTargetCho.Text;
            foodToEat.ChoLeftToTake.Text = TxtChoLeftToTake.Text;
            foodToEat.FoodToHitTarget.Text = TxtFoodToHitTarget.Text;
        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            foodToEat.Hit_ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            foodToEat.Hit_ChoOfFood.Text = TxtChoOfFood.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            foodToEat.Hit_TargetCho.Text = TxtTargetCho.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void btnReadTarget_Click(object sender, EventArgs e)
        {
            BL_General bl = new BL_General();
            foodToEat.Hit_TargetCho.Double = Safe.Double(bl.RestoreParameter("ChoToEat"));
            FromClassToUi();
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            foodToEat.Calculations();
            FromClassToUi();
        }
        private void btnReadChoTaken_Click(object sender, EventArgs e)
        {
        }
        private void btnReadFood_Click(object sender, EventArgs e)
        {
        }
        private void btnReadAll_Click(object sender, EventArgs e)
        {
        }
    }
}