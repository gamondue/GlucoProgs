using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeals : Form
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
        Accuracy accuracyClass; 

        private List<Meal> allTheMeals;
        //private bool clickedAccuracy;

        public frmMeals()
        {
            InitializeComponent();
        }
        private void frmMeals_Load(object sender, EventArgs e)
        {
            cmbAccuracyMeal.DataSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbTypeOfMeal.DataSource = Enum.GetValues(typeof(TypeOfMeal));

            accuracyClass = new Accuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal, bl);

            RefreshUi();
        }
        private void SetCorrectRadioButtons()
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
            txtChoOfMeal.Text = Safe.String(bl.Meal.ChoGrams.Text);
            cmbTypeOfMeal.SelectedItem = bl.Meal.IdTypeOfMeal;
            if (bl.Meal.TimeBegin.DateTime != Common.DateNull)
                dtpMealTimeBegin.Value = (DateTime)bl.Meal.TimeBegin.DateTime;
            if (bl.Meal.TimeEnd.DateTime != Common.DateNull)
                dtpMealTimeEnd.Value = (DateTime)bl.Meal.TimeEnd.DateTime;
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;

            SetCorrectRadioButtons();
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.ChoGrams.Text = txtChoOfMeal.Text;

            bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;

            bl.Meal.AccuracyOfChoEstimate.Double = (double?)Safe.Double(txtAccuracyOfChoMeal.Text);

            bl.Meal.TimeBegin.DateTime = dtpMealTimeBegin.Value;
            bl.Meal.TimeEnd.DateTime = dtpMealTimeEnd.Value;

            // since the combo has more options, it gets the priority 
            // over the radiobuttons, but if the combo is in one of the 
            // states represented by radiobutton, the radiobutton gets the priority
            if ((TypeOfMeal)cmbTypeOfMeal.SelectedItem != Common.TypeOfMeal.Other
                && (TypeOfMeal)cmbTypeOfMeal.SelectedItem != Common.TypeOfMeal.NotSet)
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
        }
        private void RefreshUi()
        {
            FromClassToUi();
            allTheMeals = bl.GetMeals(dtpMealTimeBegin.Value.Subtract(new TimeSpan(60, 00, 0, 0)), 
                dtpMealTimeBegin.Value.AddDays(1));
            gridMeals.DataSource = allTheMeals;
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
            txtIdMeal.Text = bl.SaveOneMeal(bl.Meal).ToString();   
            
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
            bl.SaveOneMeal(bl.Meal);
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
        private void gridMeals_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void gridMeals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                gridMeals.Rows[e.RowIndex].Selected = true;
                FromUiToClass();
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
            if (e.RowIndex > -1)
            {
                bl.Meal = allTheMeals[e.RowIndex];
                gridMeals.Rows[e.RowIndex].Selected = true;
                FromClassToUi();
            }
        }
        private void txtChoOfMeal_TextChanged(object sender, EventArgs e)
        {
            bl.SaveMealParameters();
        }
        //private void txtAccuracyOfChoMeal_TextChanged(object sender, EventArgs e)
        //{
        //    double acc;
        //    double.TryParse(txtAccuracyOfChoMeal.Text, out acc); 
        //    if (acc != 0) //
        //    {
        //        cmbAccuracyMeal.SelectedItem =
        //            bl.GetQualitativeAccuracyGivenQuantitavive(acc);
        //        txtAccuracyOfChoMeal.BackColor = bl.AccuracyBackColor(acc);
        //        txtAccuracyOfChoMeal.ForeColor = bl.AccuracyForeColor(acc);
        //    }
        //    else
        //    {
        //        cmbAccuracyMeal.SelectedItem = null;
        //        txtAccuracyOfChoMeal.BackColor = Color.White;
        //        txtAccuracyOfChoMeal.ForeColor = Color.Black;
        //    }
        //}
        //private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (clickedAccuracy)
        //    {
        //        clickedAccuracy = false;
        //        QualitativeAccuracy qa = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
        //        txtAccuracyOfChoMeal.Text = ((int)qa).ToString(); 
        //        FromClassToUi();
        //    }
        //}
        //private void cmbAccuracyMeal_Enter(object sender, MouseEventArgs e)
        //{
        //    clickedAccuracy = true;
        //}
        //private void cmbAccuracyMeal_Leave(object sender, EventArgs e)
        //{
        //    clickedAccuracy = false;
        //}
    }
}
