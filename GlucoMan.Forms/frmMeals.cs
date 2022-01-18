using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeals : Form
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
        private List<Meal> meals;
        private Meal currentMeal;

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
            meals = bl.ReadMeals(null, null);
            gridMeals.DataSource = meals;
        }
        private void FromUiToClass()
        {
            currentMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            currentMeal.Carbohydrates.Text = txtChoOfMeal.Text;
            currentMeal.TimeStart.DateTime = dtpMealTimeStart.Value;
            currentMeal.TimeEnd.DateTime = dtpMealTimeEnd.Value;
            currentMeal.AccuracyOfChoEstimate = Double.Parse(txtAccuracyOfChoMeal.Text);
            ////////currentMeal.TypeOfMeal = cmbTypeOfMeal.Text;

            //thisMeal.TypeOfInsulineInjection;
            //thisMeal.IdGlucoseRecord
            //thisMeal.IdInsulineInjection
        }
        private void FromClassToUi()
        {
            txtIdMeal.Text = currentMeal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(currentMeal.Carbohydrates.Text);
            if (currentMeal.TimeStart.DateTime != Common.DateNull)
                dtpMealTimeStart.Value = currentMeal.TimeStart.DateTime;
            if (currentMeal.TimeEnd.DateTime != Common.DateNull)
                dtpMealTimeEnd.Value = currentMeal.TimeEnd.DateTime;
            txtAccuracyOfChoMeal.Text = currentMeal.AccuracyOfChoEstimate.ToString();
            cmbTypeOfMeal.Text = currentMeal.TypeOfMeal.ToString();
        }
        private void btnAddMeal_Click(object sender, EventArgs e)
        {
            Meal newMeal = new Meal();
            frmMeal m = new frmMeal(newMeal);  
            m.ShowDialog();
        }
        private void gridMeals_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridMeals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                currentMeal = meals[e.RowIndex];
                FromClassToUi();
            }
        }
        private void gridMeals_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                gridMeals.Rows[e.RowIndex].Selected = true;
                txtIdMeal.Text = currentMeal.IdMeal.ToString();
            }
        }
        private void btnShowThisMeal_Click(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                MessageBox.Show("Choose a food in the grid");
                return;
            }

            frmMeal m = new frmMeal(currentMeal);
            m.ShowDialog();
        }
        private void btnRemoveFood_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            if (gridMeals.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select one meal from the grid"); 
                return ;    
            }
            FromUiToClass();
            bl.SaveOneMeal(currentMeal); 
        }
        private void txtAccuracyOfChoMeal_TextChanged(object sender, EventArgs e)
        {
            currentMeal.AccuracyOfChoEstimate = Safe.Double(txtAccuracyOfChoMeal.Text); 
            bl.NumericalAccuracyChanged(currentMeal.AccuracyOfChoEstimate.Value); 
        }
        private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMeal.AccuracyOfChoEstimate = 
                bl.QualitativeAccuracyChanged(((QualitativeAccuracy)cmbAccuracyMeal.SelectedItem));
        }
    }
}
