using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeals : Form
    {
        // since it is accessed by several pages, to avoid "concurrent" problems 
        // we use a common business layer object, beetween different forms
        private BL_MealAndFood bl = Common.MealAndFood_CommonBL;

        Accuracy accuracyClass; 

        private List<Meal> allTheMeals;


        public frmMeals()
        {
            InitializeComponent();
        }
        private void frmMeals_Load(object sender, EventArgs e)
        {
            cmbAccuracyMeal.DataSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbTypeOfMeal.DataSource = Enum.GetValues(typeof(TypeOfMeal));
            bl.Meal = new Meal();
            bl.FoodInMeal = new FoodInMeal(); 

            accuracyClass = new Accuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);

            bl.SetTypeOfMealBasedOnTimeNow(); 
            RefreshGrid();
        }
        private void SetCorrectTypeOfMealRadioButton()
        {
            // un-check all
            rdbIsBreakfast.Checked = false;
            rdbIsSnack.Checked = false;
            rdbIsLunch.Checked = false;
            rdbIsDinner.Checked = false;

            // check the correct (if any!) 
            if (bl.Meal.IdTypeOfMeal == Common.TypeOfMeal.Breakfast)
                rdbIsBreakfast.Checked = true;
            else if (bl.Meal.IdTypeOfMeal == Common.TypeOfMeal.Snack)
                rdbIsSnack.Checked = true;
            else if (bl.Meal.IdTypeOfMeal == Common.TypeOfMeal.Lunch)
                rdbIsLunch.Checked = true;
            else if (bl.Meal.IdTypeOfMeal == Common.TypeOfMeal.Dinner)
                rdbIsDinner.Checked = true;
        }
        public void FromClassToUi()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(bl.Meal.Carbohydrates.Text);
            cmbTypeOfMeal.SelectedItem = bl.Meal.IdTypeOfMeal;
            if (bl.Meal.TimeBegin.DateTime != General.DateNull)
                dtpMealTimeBegin.Value = (DateTime)bl.Meal.TimeBegin.DateTime;
            if (bl.Meal.TimeEnd.DateTime != General.DateNull)
                dtpMealTimeEnd.Value = (DateTime)bl.Meal.TimeEnd.DateTime;
            if (bl.Meal.AccuracyOfChoEstimate.Double != null)
                txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            txtNotes.Text = bl.Meal.Notes; 
            SetCorrectTypeOfMealRadioButton();
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.Carbohydrates.Text = txtChoOfMeal.Text;

            bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;

            bl.Meal.AccuracyOfChoEstimate.Double = (double?)Safe.Double(txtAccuracyOfChoMeal.Text);

            bl.Meal.TimeBegin.DateTime = dtpMealTimeBegin.Value;
            bl.Meal.TimeEnd.DateTime = dtpMealTimeEnd.Value;
            bl.Meal.Notes = txtNotes.Text;
            // TypeOfMeal treated by controls' events
        }
        private void RefreshUi()
        {
            FromClassToUi();
            RefreshGrid(); 
        }
        private void RefreshGrid()
        {
            DateTime now = DateTime.Now;
            allTheMeals = bl.GetMeals(
                now.Subtract(new TimeSpan(120, 00, 0, 0)),
                now.AddDays(1));
            gridMeals.DataSource = allTheMeals;
        }
        private void gridMeals_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void gridMeals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                gridMeals.Rows[e.RowIndex].Selected = true;
                bl.Meal = allTheMeals[e.RowIndex];
                FromClassToUi();
            }
        }
        private void gridMeals_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                gridMeals.Rows[e.RowIndex].Selected = true;
                FromUiToClass();
                frmMeal m = new frmMeal(bl.Meal);
                m.ShowDialog();
            }
        }
        private void gridMeals_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    bl.Meal = allTheMeals[e.RowIndex];
            //    gridMeals.Rows[e.RowIndex].Selected = true;
            //    FromClassToUi();
            //}
        }
        private void btnAddMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            // erase Id to create a new meal
            bl.Meal.IdMeal = null; 
            if (chkNowInAdd.Checked)
            {
                DateTime now = DateTime.Now;
                bl.Meal.TimeBegin.DateTime = now;
                bl.Meal.TimeEnd.DateTime = now;
            }        
            frmMeal m = new frmMeal(bl.Meal); 
            m.ShowDialog();
            RefreshUi(); 
        }
        private void btnRemoveMeal_Click(object sender, EventArgs e) 
        {
            if (txtIdMeal.Text == "")
            {
                MessageBox.Show("Select one meal from the grid");
                return;
            }
            if (MessageBox.Show(string.Format ("Should we delete meal at {0}, Id {1}", 
                    bl.Meal.TimeBegin.Text, 
                    bl.Meal.IdMeal), "",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            { 
                bl.DeleteOneMeal(bl.Meal);
                RefreshUi();
            }
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                MessageBox.Show("Select one meal from the grid or save a new meal"); 
                return ;    
            }
            FromUiToClass();
            bl.SaveOneMeal(bl.Meal, chkNowInAdd.Checked);
            RefreshUi(); 
        }
        private void btnShowThisMeal_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                MessageBox.Show("Choose a meal in the grid");
                return;
            }
            FromUiToClass();
            bl.FoodInMeal = new FoodInMeal(); 
            frmMeal m = new frmMeal(bl.Meal);
            m.ShowDialog();
            RefreshUi();
        }
        private void btnNowBegin_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtpMealTimeBegin.Value = now;
            dtpMealTimeEnd.Value = now;
        }
        private void btnNowEnd_Click(object sender, EventArgs e)
        {
            dtpMealTimeEnd.Value = DateTime.Now;
        }
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            bl.NewDefaults();
            FromClassToUi();
        }
        private void txtChoOfMeal_TextChanged(object sender, EventArgs e)
        {
            bl.SaveMealParameters();
        }
        private void rdb_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbIsBreakfast.Checked)
                bl.Meal.IdTypeOfMeal = Common.TypeOfMeal.Breakfast;
            else if (rdbIsSnack.Checked)
                bl.Meal.IdTypeOfMeal = Common.TypeOfMeal.Snack;
            else if (rdbIsLunch.Checked)
                bl.Meal.IdTypeOfMeal = Common.TypeOfMeal.Lunch;
            else if (rdbIsDinner.Checked)
                bl.Meal.IdTypeOfMeal = Common.TypeOfMeal.Dinner;
        }
        private void cmbTypeOfMeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;
        }
    }
}
