using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeal : Form
    {
        // since it is accessed by several pages, to avoid "concurrent" problems 
        // we use a common business layer beetween different pages
        private BL_MealAndFood bl = Common.MealAndFood_CommonBL;

        private bool loading = true;

        Accuracy accuracyMeal;
        Accuracy accuracyFoodInMeal;

        frmFoods foodsPage;
        frmInsulinCalc insulinCalcPage;
        frmInjections injectionsPage;
        frmGlucose measurementPage;

        internal frmMeal(Meal Meal)
        {
            InitializeComponent();
            
            loading = true;
            if (Meal == null)
            {
                Meal = new Meal();
                btnDefaults_Click(null, null);
            }
            bl.Meal = Meal;

            cmbAccuracyMeal.DataSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbAccuracyFoodInMeal.DataSource = Enum.GetValues(typeof(QualitativeAccuracy));

            accuracyMeal = new Accuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);
            accuracyFoodInMeal = new Accuracy(txtAccuracyOfChoFoodInMeal, cmbAccuracyFoodInMeal);

            gridFoodsInMeal.AutoGenerateColumns = true;
        }
        private void frmMeal_Load(object sender, EventArgs e)
        {
            if (bl.Meal.IdTypeOfMeal == TypeOfMeal.NotSet)
            {
                bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
            }
            RefreshUi();

            txtFoodChoPercent.Focus();

            loading = false;
        }
        private void RefreshGrid()
        {
            bl.FoodsInMeal = bl.GetFoodsInMeal(bl.Meal.IdMeal); 
            gridFoodsInMeal.DataSource = bl.FoodsInMeal;
            gridFoodsInMeal.Refresh();
        }
        private void RefreshUi()
        {
            FromClassToUi(); 
            RefreshGrid(); 
        }
        internal void FromClassToUi()
        {
            loading = true;

            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMealGrams.Text = bl.Meal.Carbohydrates.Text;

            SetTypeOfMealRadiobutton(bl.Meal.IdTypeOfMeal);

            if (bl.Meal.TimeBegin.DateTime != General.DateNull)
                dtpMealTimeStart.Value = (DateTime)bl.Meal.TimeBegin.DateTime;
            if (bl.Meal.TimeEnd.DateTime != General.DateNull)
                dtpMealTimeFinish.Value = (DateTime)bl.Meal.TimeEnd.DateTime;
            
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            txtNotes.Text = bl.Meal.Notes;

            if (bl.FoodInMeal.IdFoodInMeal != null)
                txtIdFoodInMeal.Text = bl.FoodInMeal.IdFoodInMeal.ToString();
            if (bl.FoodInMeal.IdFood != null)
                txtIdFood.Text = bl.FoodInMeal.IdFoodInMeal.ToString();

            txtFoodChoPercent.Text = bl.FoodInMeal.ChoPercent.Text;
            txtFoodQuantityGrams.Text = bl.FoodInMeal.QuantityGrams.Text;
            txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
            txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
            txtFoodInMealName.Text = bl.FoodInMeal.Name;

            loading = false; 
        }
        private void FromUiToClass()
        {
            loading = true;

            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.Carbohydrates.Text = txtChoOfMealGrams.Text;
            bl.Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoMeal.Text;
            bl.Meal.Notes = txtNotes.Text;

            bl.Meal.TimeBegin.DateTime = dtpMealTimeStart.Value;
            bl.Meal.TimeEnd.DateTime = dtpMealTimeFinish.Value;
            bl.Meal.IdTypeOfMeal = GetTypeOfMealFromRadiobuttons();

            bl.FoodInMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdFoodInMeal.Text);
            bl.FoodInMeal.IdFood = Safe.Int(txtIdFood.Text);
            bl.FoodInMeal.QuantityGrams.Text = txtFoodQuantityGrams.Text; // [g]
            bl.FoodInMeal.ChoPercent.Text = txtFoodChoPercent.Text;
            bl.FoodInMeal.ChoGrams.Text = txtFoodChoGrams.Text;
            bl.FoodInMeal.Name = txtFoodInMealName.Text;
            bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
          
            loading = false;
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
        private void txtFoodChoPercent_TextChanged(object sender, EventArgs e) 
        {
            if (!loading)
            {
                bl.FoodInMeal.ChoPercent.Text = txtFoodChoPercent.Text;
                bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
                bl.SaveFoodInMealParameters();
            }
        }
        private void txtFoodQuantityGrams_TextChanged(object sender, EventArgs e) 
        {
            if (!loading)
            {
                bl.FoodInMeal.QuantityGrams.Text = txtFoodQuantityGrams.Text;
                bl.CalculateChoOfFoodGrams(bl.FoodInMeal);
                txtFoodChoGrams.Text = bl.FoodInMeal.ChoGrams.Text;
            }
        }
        private void txtFoodChoGrams_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                if (!txtFoodQuantityGrams.Focused && !txtFoodChoPercent.Focused)
                {
                    txtFoodQuantityGrams.Text = "";
                    bl.FoodInMeal.QuantityGrams.Double = 0;
                    txtFoodChoPercent.Text = "";
                    bl.FoodInMeal.ChoPercent.Double = 0;
                }
            }
            bl.FoodInMeal.ChoGrams.Text = txtFoodChoGrams.Text;
            bl.RecalcAll();
            txtChoOfMealGrams.Text = bl.Meal.Carbohydrates.Text;
        }
        //private void txtFoodChoGrams_Leave(object sender, EventArgs e)
        //{
        //    FromUiToClass();
        //    bl.RecalcTotalCho();
        //    bl.RecalcTotalAccuracy();
        //    txtFoodQuantityGrams.Text = "";
        //    bl.FoodInMeal.QuantityGrams.Double = 0;
        //    txtFoodChoPercent.Text = "";
        //    bl.FoodInMeal.ChoPercent.Double = 0;
        //    FromClassToUi();

        //    bl.SaveFoodInMealParameters();
        //}
        private void txtChoOfMealGrams_TextChanged(object sender, EventArgs e)
        {
            bl.SaveMealParameters();
        }
        //private void txtAccuracyOfChoFoodInMeal_TextChanged(object sender, EventArgs e)
        //{
        //    bl.FoodInMeal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoFoodInMeal.Text);
        //    bl.RecalcTotalAccuracy();
        //    FromClassToUi();
        //}
        //private void txtAccuracyOfChoFoodInMeal_Leave(object sender, EventArgs e)
        //{

        //}
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            if (bl.Meal.TimeBegin.DateTime == General.DateNull)
                // if the meal has no date, we put Now
                txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, true).ToString();
            else
                // if the meal has already a time, we don't touch it  
                txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, false).ToString();
        }
        private void btnSaveAllMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            if (bl.Meal.TimeBegin.DateTime == General.DateNull)
                // if the meal has no date, we put Now
                txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, true).ToString();
            else
                // if the meal has already a time, we don't touch it  
                txtIdMeal.Text = bl.SaveOneMeal(bl.Meal, false).ToString();
            txtIdFoodInMeal.Text = bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
            bl.SaveAllFoodsInMeal();
        }
        private void btnAddFoodInMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            // erase Id, so that a new record will be created
            bl.FoodInMeal.IdFoodInMeal = null;
            txtIdFoodInMeal.Text = bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
            bl.RecalcAll();
            RefreshUi();
        }
        private void btnRemoveFoodInMeal_Click(object sender, EventArgs e)
        {
            bl.DeleteOneFoodInMeal(bl.FoodInMeal);
            RefreshUi();
        }
        private void btnStartMeal_Click(object sender, EventArgs e) 
        {
            dtpMealTimeStart.Value = DateTime.Now;
            dtpMealTimeFinish.Value = DateTime.Now;
        }
        private void btnEndMeal_Click(object sender, EventArgs e) 
        {
            dtpMealTimeFinish.Value = DateTime.Now;
        }
        private void btnFoodDetail_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            frmFoods fd = new frmFoods(bl.FoodInMeal);
            fd.ShowDialog();
            if (fd.FoodIsChosen)
            {
                bl.FromFoodToFoodInMeal(fd.CurrentFood, bl.FoodInMeal);
                FromClassToUi();
            }
        }
        private void btnSaveFoodInMeal_Click(object sender, EventArgs e)
        {
            if (gridFoodsInMeal.SelectedRows.Count == 0)
            {
                MessageBox.Show("Choose a food to save");
                return;
            }
            FromUiToClass();
            bl.SaveOneFoodInMeal(bl.FoodInMeal);
            RefreshGrid(); 
        }
        private void btnSaveAllFoods_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneFoodInMeal(bl.FoodInMeal).ToString();
            bl.SaveAllFoodsInMeal();
            RefreshGrid();
        }
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            txtFoodChoPercent.Text = "";
            txtFoodQuantityGrams.Text = "";
            txtFoodChoGrams.Text = "";
            txtAccuracyOfChoFoodInMeal.Text = "";
            cmbAccuracyFoodInMeal.SelectedItem = null;
            txtIdFoodInMeal.Text = "";
            txtIdFood.Text = "";
            txtFoodInMealName.Text = "";

            bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
            SetTypeOfMealRadiobutton(bl.Meal.IdTypeOfMeal);
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.RecalcAll();
            FromClassToUi(); 
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            frmFoods f = new frmFoods(txtFoodInMealName.Text); 
        }
        private void btnInsulinCalc_Click(object sender, EventArgs e)
        {
            //insulinCalcPage = new InsulinCalcPage(bl.Meal.IdBolusCalculation);
            insulinCalcPage = new frmInsulinCalc();
            insulinCalcPage.ShowDialog();
        }
        private void btnGlucose_Click(object sender, EventArgs e)
        {
            //measurementPage = new frmGlucose(bl.Meal.IdGlucoseRecord);
            measurementPage = new frmGlucose();
            measurementPage.Show();
        }
        private void btnInjection_Click(object sender, EventArgs e)
        {
            frmInjections f = new frmInjections();
            f.Show();
        }
        private void btnWeighFood_Click(object sender, EventArgs e)
        {
            frmWeighFood fw = new frmWeighFood();
            fw.ShowDialog();
        }
        private void btnFoodCalc_Click(object sender, EventArgs e)
        {
            frmFoodToHitTargetCarbs f = new frmFoodToHitTargetCarbs(); 
            f.Show(); 
        }
        private void gridFoodsInMeal_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void gridFoodsInMeal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                loading = true;
                //make the clicked row the current food in meal 
                bl.FoodInMeal = bl.FoodsInMeal[e.RowIndex];
                FromClassToUi();
                gridFoodsInMeal.Rows[e.RowIndex].Selected = true;
                bl.SaveFoodInMealParameters();
                loading = false;
            }
        }
        private void gridFoodsInMeal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    bl.FoodInMeal.Rows[e.RowIndex].Selected = true;
            //    txtIdMeal.Text = bl.FoodInMeal.IdMeal.ToString();
            //    dtpMealTimeStart.Value = bl.FoodInMeal.TimeBegin.DateTime;
            //}
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
    }
}
