using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Forms
{
    public partial class frmMeals : Form
    {
        private BL_MealAndFood bl = new BL_MealAndFood();

        private Meal currentMeal = new Meal(); 
        private List<Meal> allTheMeals;

        public frmMeals()
        {
            InitializeComponent();
        }
        private void frmMeals_Load(object sender, EventArgs e)
        {
            cmbAccuracyMeal.DataSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbTypeOfMeal.DataSource = Enum.GetValues(typeof(TypeOfMeal));

            RefreshGrid();
        }
        private void RefreshGrid()
        {
            allTheMeals = bl.ReadMeals(dtpMealTimeStart.Value.Subtract(new TimeSpan(60,00,0,0)), dtpMealTimeStart.Value.AddDays(1));
            gridMeals.DataSource = allTheMeals;
        }
        private void FromUiToClass()
        {
            currentMeal.IdMeal = Safe.Int(txtIdMeal.Text);
            currentMeal.CarbohydratesGrams.Text = Safe.Double(txtChoOfMeal.Text).ToString();
            currentMeal.TimeStart.DateTime = dtpMealTimeStart.Value;
            currentMeal.TimeFinish.DateTime = dtpMealTimeFinish.Value;
            currentMeal.AccuracyOfChoEstimate.Double = (double?)Safe.Double(txtAccuracyOfChoMeal.Text);

            currentMeal.TypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;
            currentMeal.QualitativeAccuracyOfChoEstimate = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
        }
        private void FromClassToUi()
        {
            txtIdMeal.Text = currentMeal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(currentMeal.CarbohydratesGrams.Text);
            
            if (currentMeal.TimeStart.DateTime != Common.DateNull)
                dtpMealTimeStart.Value = (DateTime)currentMeal.TimeStart.DateTime;
            if (currentMeal.TimeFinish.DateTime != Common.DateNull)
                dtpMealTimeFinish.Value = (DateTime)currentMeal.TimeFinish.DateTime;

            txtAccuracyOfChoMeal.Text = currentMeal.AccuracyOfChoEstimate.Text;
            cmbAccuracyMeal.SelectedItem = currentMeal.QualitativeAccuracyOfChoEstimate;

            cmbTypeOfMeal.SelectedItem = currentMeal.TypeOfMeal;
        }
        private void btnAddMeal_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            txtIdMeal.Text = bl.SaveOneMeal(currentMeal).ToString();   
            frmMeal m = new frmMeal(currentMeal); 
            m.ShowDialog(); 
        }
        private void gridMeals_CellContentClick(object sender, DataGridViewCellEventArgs e) {}
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
                frmMeal m = new frmMeal(currentMeal);
                m.ShowDialog();
            }
        }
        private void gridMeals_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                currentMeal = allTheMeals[e.RowIndex];
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
            FromUiToClass();
            frmMeal m = new frmMeal(currentMeal);
            m.ShowDialog();
        }
        private void btnRemoveMeal_Click(object sender, EventArgs e) 
        {
            bl.DeleteOneMeal(currentMeal);
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
            bl.SaveOneMeal(currentMeal);
            RefreshGrid(); 
        }
        private void txtAccuracyOfChoMeal_TextChanged(object sender, EventArgs e)
        {
            currentMeal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text); 
            bl.NumericalAccuracyChanged(currentMeal.AccuracyOfChoEstimate.Double); 
        }
        private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //////////currentMeal.AccuracyOfChoEstimate =
            //////////    bl.QualitativeAccuracyChanged((QualitativeAccuracy)cmbAccuracyMeal.SelectedItem);
        }
        private void btnNowStart_Click(object sender, EventArgs e)
        {
            dtpMealTimeStart.Value = DateTime.Now;
        }
        private void btnNowFinish_Click(object sender, EventArgs e)
        {
            dtpMealTimeFinish.Value = DateTime.Now;
        }
    }
}
