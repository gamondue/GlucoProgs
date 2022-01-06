using GlucoMan;
using System;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmFood : Form
    {
        private Food thisFood = null;

        internal frmFood(Food Food)
        {
            InitializeComponent();

            if (Food == null)
            {
                thisFood = new Food();
                ////////Food.RestoreData();
            }
            else
                thisFood = Food;
        }
        private void frmFood_Load(object sender, System.EventArgs e)
        {
            FromClassToUi();
            //txtGlucoseBeforeMeal.Focus();
        }

        private void FromClassToUi()
        {
            txtIdFood.Text = thisFood.IdFood.ToString();

            txtFoodCarbohydrates.Text = thisFood.Carbohydrates.Text;
            txtCalories.Text = thisFood.Energy.Text;
            txtFibers.Text = thisFood.Fibers.Text;
            txtName.Text = thisFood.Name;
            txtSalt.Text = thisFood.Salt.Text;
            txtProteins.Text = thisFood.Proteins.Text;
            txtSaturatedFats.Text = thisFood.SaturatedFats.Text;
            txtSugar.Text = thisFood.Sugar.Text;
            txtTotalFats.Text = thisFood.TotalFats.Text;
        }
        private void FromUiToClass()
        {
            thisFood.IdFood = int.Parse(txtIdFood.Text);

            thisFood.Carbohydrates.Text = txtFoodCarbohydrates.Text;
            thisFood.Energy.Text = txtCalories.Text;
            thisFood.Fibers.Text = txtFibers.Text;
            thisFood.Name = txtName.Text;
            thisFood.Salt.Text = txtSalt.Text;
            thisFood.Proteins.Text = txtProteins.Text;
            thisFood.SaturatedFats.Text = txtSaturatedFats.Text;
            thisFood.Sugar.Text = txtSugar.Text;
            thisFood.TotalFats.Text = txtTotalFats.Text;
        }

        private void btnManageFoods_Click(object sender, EventArgs e)
        {
            frmFoodManagement fm = new frmFoodManagement();
            fm.Show(); 
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            
            // !!!! ????  is it right to have persistence throught the object ???? thisFood.Save(thisFood);
        }
    }
}
