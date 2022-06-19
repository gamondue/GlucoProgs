using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeal : Form
    {
        BL_MealAndFood bl = new BL_MealAndFood();
        
        internal frmMeal(Meal TheMeal)
        {
            InitializeComponent();
            if (TheMeal == null)
                TheMeal = new Meal();   
            bl.Meal = TheMeal;
            cmbAccuracyMeal.DataSource = bl.GetAllAccuracies();
            cmbAccuracyFoodInMeal.DataSource = bl.GetAllAccuracies();

            gridFoods.AutoGenerateColumns = true;
        }
        private void frmMeal_Load(object sender, EventArgs e)
        {
            if (bl.Meal.TypeOfMeal == TypeOfMeal.NotSet)
            {
                bl.Meal.TypeOfMeal = Common.SelectMealBasedOnTimeNow();
            }
            FromClassToUi();
            RefreshGrid();
            txtFoodChoPercent.Focus();
        }
        private void RefreshGrid()
        {
            bl.Foods = bl.ReadFoodsInMeal(bl.Meal.IdMeal); 
            gridFoods.DataSource = bl.Foods;
            gridFoods.Refresh();
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.CarbohydratesGrams.Text = txtChoOfMeal.Text;
            bl.Meal.TimeStart.DateTime = dtpMealTimeStart.Value;
            bl.Meal.TimeFinish.DateTime = dtpMealTimeFinish.Value;

            bl.Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
            if (cmbAccuracyMeal.SelectedItem == null)
                bl.Meal.QualitativeAccuracyOfChoEstimate =((QualitativeAccuracy)cmbAccuracyMeal.SelectedItem);
            bl.Meal.TypeOfMeal = GetTypeOfMealFromRadiobuttons();

            bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdMealInFood.Text);
            bl.FoodInMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.FoodInMeal.IdFood = Safe.Int(txtIdFood.Text);
            bl.FoodInMeal.Quantity.Text = txtFoodQuantityGrams.Text; // [g]
            bl.FoodInMeal.CarbohydratesPercent.Text = txtFoodChoPercent.Text;
            bl.FoodInMeal.CarbohydratesGrams.Text = txtFoodChoGrams.Text;
            bl.FoodInMeal.Name = txtName.Text;
        }

        private TypeOfMeal GetTypeOfMealFromRadiobuttons()
        {
            TypeOfMeal thisType; 
            if (rdbIsBreakfast.Checked)
            {
                thisType = TypeOfMeal.Breakfast;
            }
            else if (rdbIsDinner.Checked)
            {
                thisType = TypeOfMeal.Dinner;
            }
            else if (rdbIsLunch.Checked)
            {
                thisType = TypeOfMeal.Lunch;
            }
            else if (rdbIsSnack.Checked)
            {
                thisType = TypeOfMeal.Snack;
            }
            else
            {
                thisType = TypeOfMeal.NotSet;
            }
            return thisType; 
        }
        private void FromClassToUi()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMeal.Text = bl.Meal.CarbohydratesGrams.Text;

            SetTypeOfMealRadiobutton(bl.Meal.TypeOfMeal);

            if (bl.FoodInMeal.IdMeal != null)
                txtIdMeal.Text = bl.FoodInMeal.IdMeal.ToString();
            if (bl.FoodInMeal.IdFood != null)
                txtIdFood.Text = bl.FoodInMeal.IdFood.ToString();
            if (bl.Meal.TimeStart.DateTime != Common.DateNull)
                dtpMealTimeStart.Value = (DateTime)bl.Meal.TimeStart.DateTime;
            if (bl.Meal.TimeFinish.DateTime != Common.DateNull)
                dtpMealTimeFinish.Value = (DateTime)bl.Meal.TimeFinish.DateTime;
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            cmbAccuracyMeal.SelectedItem = bl.Meal.QualitativeAccuracyOfChoEstimate;

            txtIdMealInFood.Text = bl.FoodInMeal.IdFoodInMeal.ToString();
            txtFoodChoPercent.Text = bl.FoodInMeal.CarbohydratesPercent.Text;
            txtFoodQuantityGrams.Text = bl.FoodInMeal.Quantity.Text;
            txtFoodChoGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
            txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
            cmbAccuracyFoodInMeal.SelectedItem = bl.FoodInMeal.QualitativeAccuracy;

            txtName.Text = bl.FoodInMeal.Name;
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
                btnSaveMeal_Click(null, null); 
            FromUiToClass();
            // erase Id, so that a new record will be created
            bl.FoodInMeal.IdFoodInMeal = null; 
            bl.SaveOneFoodInMeal(bl.FoodInMeal);
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy();
            FromClassToUi();
            RefreshGrid();
        }
        private void btnStartMeal_Click(object sender, EventArgs e) {}
        private void btnEndMeal_Click(object sender, EventArgs e) {}
        private void SetTypeOfMealRadiobutton(Common.TypeOfMeal Type)
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
            frmFoods fd = new frmFoods();
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
        private void txtFoodChoPercent_Leave(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
            FromClassToUi();
        }
        private void txtFoodQuantityGrams_TextChanged(object sender, EventArgs e) {}
        private void txtFoodQuantityGrams_Leave(object sender, EventArgs e)
        {
                FromUiToClass();
                bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                FromClassToUi();
        }
        private void txtFoodChoGrams_Leave(object sender, EventArgs e)
        {
            FromUiToClass(); 
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy();
            txtFoodQuantityGrams.Text = "";
            txtFoodChoPercent.Text = "";
            FromClassToUi();
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            txtIdMeal.Text = bl.SaveOneMeal(bl.Meal).ToString();
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
                bl.FoodInMeal = bl.Foods[e.RowIndex];
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
            RefreshGrid(); 
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
            txtAccuracyOfChoFoodInMeal.Text = "";
            cmbAccuracyFoodInMeal.Text = "";
            txtIdMealInFood.Text = "";
            txtIdFood.Text = "";
            txtName.Text = "";
            bl.Meal.TypeOfMeal = Common.SelectMealBasedOnTimeNow();
            SetTypeOfMealRadiobutton(bl.Meal.TypeOfMeal);
        }
        private void btnSumCho_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy(); 
            FromClassToUi(); 
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
            frmFoods f = new frmFoods(txtName.Text); 
        }
        private void picCalculator_Click(object sender, EventArgs e)
        {
            double value;
            double.TryParse(this.ActiveControl.Text, out value); 
            frmCalculator calculator = new frmCalculator(value);
            calculator.ShowDialog();
            if (calculator.ClosedWithOk)
                this.ActiveControl.Text = calculator.Result.ToString(); 
        }
        private void btnSaveAllMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            txtIdMeal.Text = bl.SaveOneMeal(bl.Meal).ToString();
            bl.SaveAllFoodsInMeal(bl.FoodInMeal);
            bl.RecalcTotalAccuracy();
            bl.RecalcTotalCho();
            FromClassToUi();
            RefreshGrid();
        }

        private void btnSaveAllFoods_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.RecalcTotalAccuracy();
            bl.RecalcTotalCho();
            FromClassToUi();
            RefreshGrid();
        }
    }
}
