using GlucoMan;
using System;
using System.Windows.Forms;
using GlucoMan.BusinessLayer; 

namespace GlucoMan_Forms_Core
{
    public partial class frmFood : Form
    {
        private Food thisFood = null;
        internal frmFood(Food Food)
        {
            InitializeComponent();

            if (Food == null)
                thisFood = new Food(); 
            else
                thisFood = Food;
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
            txtIdFood.Text = thisFood.IdFood.Text;

            txtFoodCarbohydrates.Text = thisFood.Carbohydrates.Text;
            txtCalories.Text = thisFood.Calories.Text;
            txtFibers.Text = thisFood.Fibers.Text;
            txtName.Text = thisFood.Name;
            txtSalt.Text = thisFood.Salt.Text;
            txtProteins.Text = thisFood.Proteins.Text;
            txtSaturatedFats.Text = thisFood.SaturatedFats.Text;
            txtSugar.Text = thisFood.Sugar.Text;
            txtTotalFats.Text = thisFood.TotalFats.Text; 

            txtQuantity.Text = thisFood.Quantity.Text; 
        }
        private void FromUiToClass()
        {
            thisFood.IdFood.Text = txtIdFood.Text;

            thisFood.Carbohydrates.Text = txtFoodCarbohydrates.Text;
            thisFood.Calories.Text = txtCalories.Text;
            thisFood.Fibers.Text = txtFibers.Text;
            thisFood.Name = txtName.Text;
            thisFood.Salt.Text = txtSalt.Text;
            thisFood.Proteins.Text = txtProteins.Text;
            thisFood.SaturatedFats.Text = txtSaturatedFats.Text;
            thisFood.Sugar.Text = txtSugar.Text;
            thisFood.TotalFats.Text = txtTotalFats.Text;

            thisFood.Quantity.Text = txtQuantity.Text;
        }
        private void btnManageFoods_Click(object sender, EventArgs e)
        {
            frmFoodManagement fm = new frmFoodManagement();
            fm.Show(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            thisFood.Save(thisFood); 
        }
    }
}
