using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmFoods : Form
    {
        BL_MealAndFood bl = new BL_MealAndFood();
        public Food CurrentFood { get; set; }
        List<Food> allFoods;
        
        bool foodIsChosen;
        public bool FoodIsChosen { get;  }

        public frmFoods(Food Food)
        {
            InitializeComponent();
            CurrentFood = Food;
        }
        public frmFoods(string FoodNameForSearch)
        {
            InitializeComponent();
            CurrentFood.Name = FoodNameForSearch;
        }
        public frmFoods(FoodInMeal FoodInMeal)
        {
            InitializeComponent();
            CurrentFood = bl.FromFoodInMealToFood(FoodInMeal);
        }
        private void frmFoods_Load(object sender, EventArgs e)
        {
            foodIsChosen = false; 
            FromClassToUi(); 
            ///////RefreshUi();
        }
        private void FromClassToUi()
        {
            txtIdFood.Text = CurrentFood.IdFood.ToString();
            txtName.Text = CurrentFood.Name;
            txtDescription.Text = CurrentFood.Description;
            txtCalories.Text = CurrentFood.Energy.Text;
            txtTotalFats.Text = CurrentFood.TotalFats.Text;
            txtSaturatedFats.Text = CurrentFood.SaturatedFats.Text;
            txtFoodCarbohydrates.Text = CurrentFood.Cho.Text;
            txtSugar.Text = CurrentFood.Sugar.Text;
            txtFibers.Text = CurrentFood.Fibers.Text;
            txtProteins.Text = CurrentFood.Proteins.Text;
            txtSalt.Text = CurrentFood.Salt.Text;
            //txtPotassium.Text = CurrentFood.Potassium.Text; 
            //txtGlicemicIndex.Text = CurrentFood.GlycemicIndex.Text;
        }
        private void FromUiToClass()
        {
            CurrentFood.IdFood = Safe.Int(txtIdFood.Text);
            CurrentFood.Name = txtName.Text;
            CurrentFood.Description = txtDescription.Text;
            CurrentFood.Energy.Double = Safe.Double(txtCalories.Text);
            CurrentFood.TotalFats.Double = Safe.Double(txtTotalFats.Text);
            CurrentFood.SaturatedFats.Double = Safe.Double(txtSaturatedFats.Text);
            CurrentFood.Cho.Double = Safe.Double(txtFoodCarbohydrates.Text);
            CurrentFood.Sugar.Double = Safe.Double(txtSugar.Text);
            CurrentFood.Fibers.Double = Safe.Double(txtFibers.Text);
            CurrentFood.Proteins.Double = Safe.Double(txtProteins.Text);
            CurrentFood.Salt.Double = Safe.Double(txtSalt.Text);
            //CurrentFood.Potassium.Double = Safe.Double(txtPotassium.Text);
            //CurrentFood.GlycemicIndex.Double = Safe.Double(txtGlicemicIndex.Text);
        }
        private void RefreshUi()
        {
            FromUiToClass();
            allFoods = bl.SearchFoods(CurrentFood.Name, CurrentFood.Description);
            gridFoods.DataSource = allFoods;
            //gridFoods.Refresh();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneFood(CurrentFood);
            FromClassToUi();
            RefreshUi(); 
        }
        private void btnNewFood_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            CurrentFood.IdFood = null;
            bl.SaveOneFood(CurrentFood);
            RefreshUi();
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > 0)
            {
                CurrentFood = allFoods[e.RowIndex];
                gridFoods.Rows[e.RowIndex].Selected = true;
                RefreshUi(); 
            }
        }
        private void gridFoods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridFoods.SelectedRows.Count == 0)
            {
                MessageBox.Show("Choose a food to save");
                return;
            }
        }
        private void btnFatSecret_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To be implemented yet!");
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            RefreshUi(); 
        }
        private void btnChoose_Click(object sender, EventArgs e)
        {
            foodIsChosen = true;
            FromUiToClass();
            bl.SaveOneFood(CurrentFood);
            this.Close(); 
        }
    }
}
