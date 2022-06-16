using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeals : Form
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
        private List<Meal> allTheMeals;

        public frmMeals()
        {
            InitializeComponent();
        }
        private void frmMeals_Load(object sender, EventArgs e)
        {
            RefreshGrid(); 

            //gridMeals.Columns[0].Visible = false;
            //gridMeals.Columns[5].Visible = false;
            // time begin
            //gridMeals.Columns[2].i
        }
        private void RefreshGrid()
        {
            bl.ReadMeals(null, null);
            allTheMeals = bl.Meals; 
            gridMeals.DataSource = allTheMeals;
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.Carbohydrates.Text = txtChoOfMeal.Text;
            bl.Meal.TimeStart.DateTime = dtpMealTimeStart.Value;
            bl.Meal.TimeFinish.DateTime = dtpMealTimeEnd.Value;
            bl.Meal.AccuracyOfChoEstimate.Double = Double.Parse(txtAccuracyOfChoMeal.Text);
            ////////bl.Meal.TypeOfMeal = cmbTypeOfMeal.Text;

            //thisMeal.TypeOfInsulineInjection;
            //thisMeal.IdGlucoseRecord
            //thisMeal.IdInsulineInjection
        }
        private void FromClassToUi()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(bl.Meal.Carbohydrates.Text);
            if (bl.Meal.TimeStart.DateTime != Common.DateNull)
                dtpMealTimeStart.Value = (DateTime)bl.Meal.TimeStart.DateTime;
            if (bl.Meal.TimeFinish.DateTime != Common.DateNull)
                dtpMealTimeEnd.Value = (DateTime)bl.Meal.TimeFinish.DateTime;
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.ToString();
            cmbTypeOfMeal.Text = bl.Meal.TypeOfMeal.ToString();
        }
        private void btnAddMeal_Click(object sender, EventArgs e)
        {
            Meal newMeal = new Meal();
            frmMeal m = new frmMeal(newMeal);  
            m.ShowDialog();
        }
        private void gridMeals_CellContentClick(object sender, DataGridViewCellEventArgs e) {}
        private void gridMeals_CellClick(object sender, DataGridViewCellEventArgs e)        {}
        private void gridMeals_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                gridMeals.Rows[e.RowIndex].Selected = true;
                txtIdMeal.Text = bl.Meal.IdMeal.ToString();
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
        private void btnShowThisMeal_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                MessageBox.Show("Choose a meal in the grid");
                return;
            }
            frmMeal m = new frmMeal(bl.Meal);
            m.ShowDialog();
        }
        private void btnRemoveMeal_Click(object sender, EventArgs e) 
        {
            bl.DeleteOneMeal(bl.Meal);
            RefreshGrid();  
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            if (gridMeals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select one meal from the grid"); 
                return ;    
            }
            FromUiToClass();
            bl.SaveOneMeal(); 
        }
        private void txtAccuracyOfChoMeal_TextChanged(object sender, EventArgs e)
        {
            bl.Meal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text); 
            bl.NumericalAccuracyChanged(bl.Meal.AccuracyOfChoEstimate.Double); 
        }
        private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //////////bl.Meal.AccuracyOfChoEstimate =
            //////////    bl.QualitativeAccuracyChanged((QualitativeAccuracy)cmbAccuracyMeal.SelectedItem);
        }
    }
}
