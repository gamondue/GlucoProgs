using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
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
        Bl_FoodToHitTargetCarbs foodToEat = new Bl_FoodToHitTargetCarbs();

        public FoodToHitTargetCarbs_Page()
        {
            InitializeComponent();

            foodToEat.RestoreData();
            FromClassToUi();
        }
        private void FromClassToUi()
        {
            TxtChoAlreadyTaken.Text = foodToEat.ChoAlreadyTaken.Text;
            TxtChoOfFood.Text = foodToEat.ChoOfFood.Text;
            TxtTargetCho.Text = foodToEat.TargetCho.Text;
            TxtFoodToHitTarget.Text = foodToEat.FoodToHitTarget.Text;
        }
        private void frmFoodToHitTargetCarbs_Load(object sender, EventArgs e)
        {

        }
        internal void FromUiToClass()
        {
            foodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            foodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            foodToEat.TargetCho.Text = TxtTargetCho.Text;
            foodToEat.FoodToHitTarget.Text = TxtFoodToHitTarget.Text;
        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            foodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            foodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            foodToEat.TargetCho.Text = TxtTargetCho.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void btnReadTarget_Click(object sender, EventArgs e)
        {
            Bl_BolusCalculation bolus = new Bl_BolusCalculation();
            bolus.RestoreData(); // read data from BolusCalculation's file
            // set read data to local TargetCho
            foodToEat.TargetCho.Double = bolus.ChoToEat.Double;
            FromClassToUi();
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            foodToEat.Calculations();
            FromClassToUi();
        }
    }
}