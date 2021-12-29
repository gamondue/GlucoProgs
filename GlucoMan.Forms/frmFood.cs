using GlucoMan;
using System;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmFood : Form
    {
        private Food currentFood = null;

        public frmFood(Food Food)
        {
            InitializeComponent();

            currentFood = Food;

            //bolusCalculation = new BL_BolusCalculation();
            //glucoseMeasuremet = new BL_GlucoseMeasurements();
        }
        private void frmFood_Load(object sender, System.EventArgs e)
        {
            //////////Food.RestoreData();
            //bolusCalculation.MealOfBolus.SelectMealBasedOnTimeNow();

            FromClassToUi();
            //txtGlucoseBeforeMeal.Focus();
        }

        private void FromClassToUi()
        {
            txtIdFood.Text = currentFood.IdFood.ToString();

            txtFoodCarbohydrates.Text = currentFood.Carbohydrates.ToString();
            txtCalories.Text = currentFood.Calories.ToString();
            txtFibers.Text = currentFood.Fibers.ToString();
            txtName.Text = currentFood.Name;
            txtSalt.Text = currentFood.Salt.ToString();
            txtProteins.Text = currentFood.Proteins.ToString();
            txtSaturatedFats.Text = currentFood.SaturatedFats.ToString();
            txtSugar.Text = currentFood.Sugar.ToString();
            txtTotalFats.Text = currentFood.TotalFats.ToString(); 

            txtQuantity.Text = currentFood.Quantity.ToString(); 
        }

        private void btnManageFoods_Click(object sender, EventArgs e)
        {
            frmFoodManagement fm = new frmFoodManagement();
            fm.Show(); 
        }
    }
}
