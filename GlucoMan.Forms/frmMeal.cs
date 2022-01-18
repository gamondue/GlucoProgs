using System;
using System.Windows.Forms;
using GlucoMan;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeal : Form
    {
        BL_MealAndFood bl = new BL_MealAndFood();
        Meal thisMeal;
        FoodInMeal currentFoodInMeal = new FoodInMeal();
        List<FoodInMeal> foodsInThisMeal; 

        internal frmMeal(Meal TheMeal)
        {
            InitializeComponent();
            
            thisMeal = TheMeal;
        }
        private void frmMeal_Load(object sender, EventArgs e)
        {
            thisMeal.TypeOfMeal = Common.SelectMealBasedOnTimeNow();
            SetCorrectRadiobutton(thisMeal.TypeOfMeal);

            FromClassToUi();  
            RefreshGrid();

            txtFoodChoPercent.Focus();
        }
        private void RefreshGrid()
        {
            foodsInThisMeal = bl.ReadFoodsInMeal(thisMeal.IdMeal);
            gridFoods.DataSource = null;
            gridFoods.DataSource = foodsInThisMeal;
            gridFoods.Refresh();
        }
        private void FromUiToClass()
        {
            thisMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            //thisMeal.TypeOfMeal = 
            thisMeal.Carbohydrates.Text = txtChoOfMeal.Text;
            //thisMeal.AccuracyOfChoEstimate
            thisMeal.TimeStart.DateTime = dtpMealTimeStart.Value; 
            thisMeal.TimeEnd.DateTime = dtpMealTimeFinish.Value;
            //thisMeal.QualitativeAccuracyCHO = Safe.Double();
            thisMeal.AccuracyOfChoEstimate = Safe.Double(txtChoOfMeal.Text); 
            ////////thisMeal.TypeOfMeal = Common.TypeOfMeal
            // thisMeal.TypeOfInsulineInjection;
            //thisMeal.IdGlucoseRecord
            //thisMeal.IdInsulineInjection

            currentFoodInMeal.IdFoodInMeal = Safe.Int(txtIdMealInFood.Text);
            currentFoodInMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            currentFoodInMeal.IdFood = Safe.Int(txtIdFood.Text);
            currentFoodInMeal.Quantity = new DoubleAndText(); // [g]
            currentFoodInMeal.CarbohydratesGrams.Text = txtFoodQuantityGrams.Text;
            currentFoodInMeal.CarbohydratesPercent.Text = txtFoodChoPercent.Text;
            currentFoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
            currentFoodInMeal.SugarPercent.Text = txtSugarPercent.Text;
            currentFoodInMeal.FibersPercent.Text = txtFibersPercent.Text;
            currentFoodInMeal.Name = txtName.Text;
            currentFoodInMeal.Description = txtDescription.Text;
            //////////////currentFoodInMeal.QualitativeAccuracyOfCho = QualitativeAccuracyOfCho;
        }
        private void FromClassToUi()
        {
            SetCorrectRadiobutton(thisMeal.TypeOfMeal);

            txtIdMeal.Text = thisMeal.IdMeal.ToString();
            txtIdMeal.Text = currentFoodInMeal.IdMeal.ToString();
            txtIdFood.Text = currentFoodInMeal.IdFood.ToString();

            txtIdMealInFood.Text = currentFoodInMeal.IdFoodInMeal.ToString();
            txtFoodChoPercent.Text = currentFoodInMeal.CarbohydratesPercent.Text;
            txtFoodChoGrams.Text = currentFoodInMeal.CarbohydratesGrams.Text;
            txtFoodQuantityGrams.Text = currentFoodInMeal.Quantity.Text;

            txtFoodQuantityGrams.Text = currentFoodInMeal.Quantity.Text;
            txtFoodChoGrams.Text = currentFoodInMeal.CarbohydratesGrams.Text;

            txtAccuracyOfChoFoodInMeal.Text = currentFoodInMeal.AccuracyOfChoEstimate.Text;
            txtSugarPercent.Text = currentFoodInMeal.SugarPercent.Text;
            txtFibersPercent.Text = currentFoodInMeal.FibersPercent.Text;
            txtName.Text = currentFoodInMeal.Name;
            txtDescription.Text = currentFoodInMeal.Description;
            //txtFoodChoPercent.Text = currentFoodInMeal;
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
                btnSaveMeal_Click(null, null); 
            currentFoodInMeal.IdMeal = Convert.ToInt32(txtIdMeal.Text);
            FromUiToClass();
            bl.SaveOneFoodInMeal(currentFoodInMeal);
            bl.CalculateChoOfMeal();
            FromClassToUi();
            RefreshGrid();  
        }
        private void btnStartMeal_Click(object sender, EventArgs e)
        {

        }
        private void btnEndMeal_Click(object sender, EventArgs e)
        {

        }
        private void SetCorrectRadiobutton(Common.TypeOfMeal Type)
        {
            switch (Type)
            {
                case (Common.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.Checked = true;
                    break;
                case (Common.TypeOfMeal.Dinner):
                    rdbIsDinner.Checked = true;
                    break;
                case (Common.TypeOfMeal.Lunch):
                    rdbIsLunch.Checked = true;
                    break;
                case (Common.TypeOfMeal.Snack):
                    rdbIsSnack.Checked = true;
                    break;
            }
        }
        private void btnFoodDetail_Click(object sender, EventArgs e)
        {
            frmFoodManagement fd = new frmFoodManagement();
            fd.Show();
        }

        private void btnInsulin_Click(object sender, EventArgs e)
        {
            frmInsulinCalc f = new frmInsulinCalc();
            f.Show();
        }

        private void btnGlucose_Click(object sender, EventArgs e)
        {
            frmGlucose frm = new frmGlucose();
            frm.Show();
        }
        private void txtFoodChoPercent_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtFoodQuantityGrams_TextChanged(object sender, EventArgs e)
        {

        }
        private void txt_Leave(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.CalculateChoOfFoodGrams();
            FromClassToUi();
        }
        private void txtFoodChoGrams_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtFoodChoGrams_KeyDown(object sender, KeyEventArgs e)
        {
            txtFoodQuantityGrams.Text = "";
            txtFoodChoPercent.Text = "";
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneMeal(thisMeal);
            FromClassToUi(); 
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                currentFoodInMeal = foodsInThisMeal[e.RowIndex];
                FromClassToUi(); 
            }
        }
        private void gridFoods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    currentFoodInMeal.Rows[e.RowIndex].Selected = true;
            //    txtIdMeal.Text = currentFoodInMeal.IdMeal.ToString();
            //    dtpMealTimeStart.Value = currentFoodInMeal.TimeBegin.DateTime;
            //}
        }
        private void btnSaveFood_Click(object sender, EventArgs e)
        {
            if (gridFoods.SelectedRows.Count == 0)
            {
                MessageBox.Show("Choose a food to save");
                return; 
            }
            FromUiToClass();
            bl.SaveOneFoodInMeal(currentFoodInMeal);
            FromClassToUi();
        }

        private void btnWeighFood_Click(object sender, EventArgs e)
        {
            frmWeighFood fw = new frmWeighFood();
            fw.ShowDialog();
        }
    }
}
