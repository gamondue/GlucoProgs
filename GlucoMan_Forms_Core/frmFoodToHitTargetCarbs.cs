using GlucoMan.BusinessLayer;
using SharedData;
using SharedGlucoMan.BusinessLayer;
using System;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    public partial class frmFoodToHitTargetCarbs : Form
    {
        FoodToHitTargetCarbs food = new FoodToHitTargetCarbs();

        public frmFoodToHitTargetCarbs()
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
        private void TxtChoAlreadyTaken_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            food.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            food.Calculations();

            FromClassToUi();
        }

        private void TxtChoOfFood_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            food.ChoOfFood.Text = TxtChoOfFood.Text;
            food.Calculations();

            FromClassToUi();
        }
        private void TxtTargetCho_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            food.TargetCho.Text = TxtTargetCho.Text;
            food.Calculations(); 

            FromClassToUi();
        }
        private void btnReadTarget_Click(object sender, EventArgs e)
        {
            BolusCalculation bolus = new BolusCalculation();
            bolus.RestoreData();
            TxtTargetCho.Text = bolus.ChoToEat.Text; 
        }
        private void btnCalculateGrams_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
        }
    }
}
