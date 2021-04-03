using GlucoMan.BusinessLayer;
using SharedGlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodToHitTargetCarbs_Page : ContentPage
    {
        FoodToHitTargetCarbs food = new FoodToHitTargetCarbs();

        public FoodToHitTargetCarbs_Page()
        {
            InitializeComponent();

            food.RestoreData();
            FromClassToUi();
        }
        private void FromClassToUi()
        {
            TxtChoAlreadyTaken.Text = food.ChoAlreadyTaken.Text;
            TxtChoOfFood.Text = food.ChoOfFood.Text;
            TxtTargetCho.Text = food.TargetCho.Text;
            TxtFoodToHitTarget.Text = food.FoodToHitTarget.Text;
        }
        private void frmFoodToHitTargetCarbs_Load(object sender, EventArgs e)
        {

        }
        internal void FromUiToClass()
        {
            food.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            food.ChoOfFood.Text = TxtChoOfFood.Text;
            food.TargetCho.Text = TxtTargetCho.Text;
            food.FoodToHitTarget.Text = TxtFoodToHitTarget.Text;
        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            food.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            food.Calculations();

            FromClassToUi();
        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            food.ChoOfFood.Text = TxtChoOfFood.Text;
            food.Calculations();

            FromClassToUi();
        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            food.TargetCho.Text = TxtTargetCho.Text;
            food.Calculations();

            FromClassToUi();
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            food.Calculations();
            FromClassToUi();
        }
        private void btnReadTarget_Click(object sender, EventArgs e)
        {
            BolusCalculation bolus = new BolusCalculation();
            bolus.RestoreData();
            food.TargetCho.Double = bolus.ChoToEat.Double;
            FromClassToUi();
        }
    }
}