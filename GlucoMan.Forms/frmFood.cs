namespace GlucoMan.Forms
{
    public partial class frmFood : Form
    {
        private Food thisFood = null;

        internal frmFood(Food Food)
        {
            InitializeComponent();

            if (Food == null)
            {
                thisFood = new Food(new UnitOfFood("g", 1));
            }
            else
                thisFood = Food;
        }
        private void frmFood_Load(object sender, System.EventArgs e)
        {
            FromClassToUi();
        }

        private void FromClassToUi()
        {
            txtIdFood.Text = thisFood.IdFood.ToString();

            txtFoodCarbohydrates.Text = thisFood.CarbohydratesPercent.Text;
            txtCalories.Text = thisFood.Energy.Text;
            txtFibers.Text = thisFood.FibersPercent.Text;
            txtName.Text = thisFood.Name;
            txtSalt.Text = thisFood.SaltPercent.Text;
            txtProteins.Text = thisFood.ProteinsPercent.Text;
            txtSaturatedFats.Text = thisFood.SaturatedFatsPercent.Text;
            txtSugar.Text = thisFood.SugarPercent.Text;
            txtTotalFats.Text = thisFood.TotalFatsPercent.Text;
        }
        private void FromUiToClass()
        {
            thisFood.IdFood = int.Parse(txtIdFood.Text);

            thisFood.CarbohydratesPercent.Text = txtFoodCarbohydrates.Text;
            thisFood.Energy.Text = txtCalories.Text;
            thisFood.FibersPercent.Text = txtFibers.Text;
            thisFood.Name = txtName.Text;
            thisFood.SaltPercent.Text = txtSalt.Text;
            thisFood.ProteinsPercent.Text = txtProteins.Text;
            thisFood.SaturatedFatsPercent.Text = txtSaturatedFats.Text;
            thisFood.SugarPercent.Text = txtSugar.Text;
            thisFood.TotalFatsPercent.Text = txtTotalFats.Text;
        }

        private void btnManageFoods_Click(object sender, EventArgs e)
        {
            ////////frmFoodsPersistent fm = new frmFoodsPersistent();
            ////////fm.Show(); 
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            this.Close();
        }
    }
}
