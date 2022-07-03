using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeals : Form
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
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

            accuracyClass = new Accuracy(this, txtAccuracyOfChoMeal, cmbAccuracyMeal, bl);

            RefreshUi();
        }
        public void FromClassToUi()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(bl.Meal.CarbohydratesGrams.Text);

            if (bl.Meal.TimeBegin.DateTime != Common.DateNull)
                dtpMealTimeBegin.Value = (DateTime)bl.Meal.TimeBegin.DateTime;
            if (bl.Meal.TimeEnd.DateTime != Common.DateNull)
                dtpMealTimeEnd.Value = (DateTime)bl.Meal.TimeEnd.DateTime;

            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
            cmbAccuracyMeal.SelectedItem = bl.Meal.QualitativeAccuracyOfChoEstimate;

            cmbTypeOfMeal.SelectedItem = bl.Meal.IdTypeOfMeal;
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.CarbohydratesGrams.Text = Safe.Double(txtChoOfMeal.Text).ToString();
            bl.Meal.TimeBegin.DateTime = dtpMealTimeBegin.Value;
            bl.Meal.TimeEnd.DateTime = dtpMealTimeEnd.Value;
            bl.Meal.AccuracyOfChoEstimate.Double = (double?)Safe.Double(txtAccuracyOfChoMeal.Text);

            bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;
            bl.Meal.QualitativeAccuracyOfChoEstimate = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
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
            bl.DeleteOneMeal(bl.Meal);
            RefreshUi();  
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                MessageBox.Show("Select one meal from the grid"); 
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
        private void btnNowStart_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtpMealTimeBegin.Value = now;
            dtpMealTimeEnd.Value = now;
        }
        private void btnNowEnd_Click(object sender, EventArgs e)
        {
            dtpMealTimeEnd.Value = DateTime.Now;
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
    }
}
