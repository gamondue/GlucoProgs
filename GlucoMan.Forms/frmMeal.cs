using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeal : Form
    {
        BL_MealAndFood bl = new BL_MealAndFood();
        List<FoodInMeal> foodsInThisMeal;

        internal frmMeal(Meal TheMeal)
        {
            InitializeComponent();
            if (TheMeal == null)
                TheMeal = new Meal();   
            bl.Meal = TheMeal;
            //TypeOfMeal.NotSet; 
        }
        private void frmMeal_Load(object sender, EventArgs e)
        {
            if (bl.Meal.TypeOfMeal == TypeOfMeal.NotSet)
            {
                bl.Meal.TypeOfMeal = Common.SelectMealBasedOnTimeNow();
                SetCorrectRadiobutton(bl.Meal.TypeOfMeal);
            }
            bl.ReadFoodsInMeal(bl.Meal.IdMeal); 
            FromClassToUi();
            RefreshGrid();
            txtFoodChoPercent.Focus();
        }
        private void RefreshGrid()
        {
            bl.ReadFoodsInMeal(bl.Meal.IdMeal); 
            foodsInThisMeal = bl.Foods;
            gridFoods.DataSource = foodsInThisMeal;
            gridFoods.Refresh();
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            //thisMeal.TypeOfMeal = 
            bl.Meal.Carbohydrates.Text = txtChoOfMeal.Text;
            //thisMeal.AccuracyOfChoEstimate
            bl.Meal.TimeStart.DateTime = dtpMealTimeStart.Value;
            bl.Meal.TimeFinish.DateTime = dtpMealTimeFinish.Value;
            //thisMeal.QualitativeAccuracyCHO = Safe.Double();
            bl.Meal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text);
            //////////////thisMeal.TypeOfMeal = Common.TypeOfMeal
            // thisMeal.TypeOfInsulineInjection;
            //thisMeal.IdGlucoseRecord
            //thisMeal.IdInsulineInjection

            bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdMealInFood.Text);
            bl.FoodInMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.FoodInMeal.IdFood = Safe.Int(txtIdFood.Text);
            bl.FoodInMeal.Quantity.Text = txtFoodQuantityGrams.Text; // [g]
            bl.FoodInMeal.CarbohydratesPercent.Text = txtFoodChoPercent.Text;
            bl.FoodInMeal.CarbohydratesGrams.Text = txtFoodChoGrams.Text;
            bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
            bl.FoodInMeal.SugarPercent.Text = txtSugarPercent.Text;
            bl.FoodInMeal.FibersPercent.Text = txtFibersPercent.Text;
            bl.FoodInMeal.Name = txtName.Text;
            bl.FoodInMeal.Description = txtDescription.Text;
            //////////////bl.FoodInMeal.QualitativeAccuracyOfCho = QualitativeAccuracyOfCho;
        }
        private void FromClassToUi()
        {
            SetCorrectRadiobutton(bl.Meal.TypeOfMeal);

            if (bl.Meal.IdMeal != null)
                txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            if (bl.FoodInMeal.IdFood != null)
                txtIdFood.Text = bl.FoodInMeal.IdFood.ToString();

            txtChoOfMeal.Text = bl.Meal.Carbohydrates.Text;
            if (bl.Meal.TimeStart.DateTime != Common.DateNull)
                dtpMealTimeStart.Value = bl.Meal.TimeStart.DateTime;
            if (bl.Meal.TimeFinish.DateTime != Common.DateNull)
                dtpMealTimeFinish.Value = bl.Meal.TimeFinish.DateTime;
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            //cmbAccuracyMeal.Text = bl.Meal.QualitativeAccuracyOfChoEstimate.ToString(); 

            txtIdMealInFood.Text = bl.FoodInMeal.IdFoodInMeal.ToString();
            txtFoodChoPercent.Text = bl.FoodInMeal.CarbohydratesPercent.Text;
            txtFoodChoGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
            txtFoodQuantityGrams.Text = bl.FoodInMeal.Quantity.Text;

            txtFoodQuantityGrams.Text = bl.FoodInMeal.Quantity.Text;
            txtFoodChoGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;

            txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
            txtSugarPercent.Text = bl.FoodInMeal.SugarPercent.Text;
            txtFibersPercent.Text = bl.FoodInMeal.FibersPercent.Text;
            txtName.Text = bl.FoodInMeal.Name;
            txtDescription.Text = bl.FoodInMeal.Description;
            //txtFoodChoPercent.Text = bl.FoodInMeal;
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
                btnSaveMeal_Click(null, null); 
            FromUiToClass();
            // erase Id, so that a new record wil be created
            bl.FoodInMeal.IdFoodInMeal = null; 
            bl.SaveOneFoodInMeal(bl.FoodInMeal);
            bl.CalculateChoOfMeal();
            bl.RecalcTotalCho();
            FromClassToUi();
            RefreshGrid();
        }
        private void btnStartMeal_Click(object sender, EventArgs e) {}
        private void btnEndMeal_Click(object sender, EventArgs e) {}
        private void SetCorrectRadiobutton(Common.TypeOfMeal Type)
        {
            if (Type == TypeOfMeal.NotSet)
                return; 
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
        private void txtFoodChoPercent_TextChanged(object sender, EventArgs e) {}
        private void txtFoodQuantityGrams_TextChanged(object sender, EventArgs e) {}
        private void txtFoodChoGrams_TextChanged(object sender, EventArgs e) 
        {

        }
        private void txtFoodChoGrams_KeyDown(object sender, KeyEventArgs e)
        {
            txtFoodQuantityGrams.Text = "";
            txtFoodChoPercent.Text = "";
        }
        private void txtFoodChoGrams_Leave(object sender, EventArgs e)
        {
            FromUiToClass(); 
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy(); 
            FromClassToUi();
        }
        private void txt_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != "") 
            {
                FromUiToClass();
                bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                FromClassToUi();
            }
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            txtIdMeal.Text = bl.SaveOneMeal().ToString();
            FromClassToUi();
            RefreshGrid();
            bl.RecalcTotalAccuracy();
            bl.RecalcTotalCho();
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void gridFoods_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                bl.FoodInMeal = foodsInThisMeal[e.RowIndex];
                gridFoods.Rows[e.RowIndex].Selected = true;
                FromClassToUi();
            }
        }
        private void gridFoods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    bl.FoodInMeal.Rows[e.RowIndex].Selected = true;
            //    txtIdMeal.Text = bl.FoodInMeal.IdMeal.ToString();
            //    dtpMealTimeStart.Value = bl.FoodInMeal.TimeBegin.DateTime;
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
            bl.SaveOneFoodInMeal(bl.FoodInMeal);
            FromClassToUi();
        }
        private void btnWeighFood_Click(object sender, EventArgs e)
        {
            frmWeighFood fw = new frmWeighFood();
            fw.ShowDialog();
        }
        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            bl.DeleteOneFoodInMeal(bl.FoodInMeal); 
            bl.RecalcTotalCho();
            FromClassToUi(); 
            RefreshGrid();  
        }
        private void btnNewData_Click(object sender, EventArgs e)
        {
            txtFoodChoPercent.Text = "";
            txtFoodQuantityGrams.Text = "";
            txtFoodChoGrams.Text = "";
            txtSugarGrams.Text = "";
            txtAccuracyOfChoFoodInMeal.Text = "";
            cmbAccuracyFoodInMeal.Text = "";
            txtIdMealInFood.Text = "";
            txtIdFood.Text = "";
            txtSugarPercent.Text = "";
            txtFibersPercent.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            bl.Meal.TypeOfMeal = Common.SelectMealBasedOnTimeNow();
            SetCorrectRadiobutton(bl.Meal.TypeOfMeal);
        }
        private void btnSumCho_Click(object sender, EventArgs e)
        {
            bl.FoodInMeal.CarbohydratesGrams.Double = Safe.Double (txtFoodChoGrams.Text);
            bl.RecalcTotalCho();
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
            frmFoodManagement f = new frmFoodManagement(txtName.Text, txtDescription.Text); 
        }

        private void picCalculator_Click(object sender, EventArgs e)
        {
            double value;
            double.TryParse(this.ActiveControl.Text, out value); 
            frmCalculator calculator = new frmCalculator(value);
            calculator.ShowDialog();
            this.ActiveControl.Text = calculator.Result.ToString(); 
        }
    }
}
