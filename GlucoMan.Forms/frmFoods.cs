using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmFoods : Form
    {
        BL_MealAndFood bl = Common.MealAndFood_CommonBL; 
        public Food CurrentFood { get; set; }
        public bool FoodIsChosen { get => foodIsChosen; }

        List<Food> allFoods;
        private bool loading = false;
        private bool foodIsChosen;

        public frmFoods(Food Food)
        {
            InitializeComponent();
            CurrentFood = Food;
        }
        public frmFoods(string FoodNameForSearch)
        {
            InitializeComponent();
            CurrentFood = new Food();
            CurrentFood.Name = FoodNameForSearch;
        }
        public frmFoods(FoodInMeal FoodInMeal)
        {
            InitializeComponent();
            CurrentFood = new Food(); 
            bl.FromFoodInMealToFood(FoodInMeal, CurrentFood);
        }
        private void frmFoods_Load(object sender, EventArgs e)
        {
            loading = true;

            foodIsChosen = false;
            txtName.Text = "";
            txtDescription.Text = "";
            allFoods = new List<Food>();
            // if a specific food is passed, load its persistent from database 
            if (CurrentFood.IdFood != 0 && CurrentFood.IdFood != null)
            {
                CurrentFood = bl.GetOneFood(CurrentFood.IdFood);
            }
            // if what is passed has not and IdFood,
            // we use the data actually passed 

            // let's show the CurrentFood
            FromClassToUi();

            loading = false;
        }
        private void FromClassToUi()
        {
            txtIdFood.Text = CurrentFood.IdFood.ToString();
            txtName.Text = CurrentFood.Name;
            txtDescription.Text = CurrentFood.Description;
            txtCalories.Text = CurrentFood.Energy.Text;
            txtTotalFats.Text = CurrentFood.TotalFats.Text;
            txtSaturatedFats.Text = CurrentFood.SaturatedFats.Text;
            txtFoodCarbohydrates.Text = CurrentFood.Carbohydrates.Text;
            txtSugar.Text = CurrentFood.Sugar.Text;
            txtFibers.Text = CurrentFood.Fibers.Text;
            txtProteins.Text = CurrentFood.Proteins.Text;
            txtSalt.Text = CurrentFood.Salt.Text;
            //txtPotassium.Text = CurrentFood.Potassium.Text; 
            //txtGlicemicIndex.Text = CurrentFood.GlycemicIndex.Text;
        }
        private void FromUiToClass()
        {
            CurrentFood.IdFood = SqlSafe.Int(txtIdFood.Text);
            CurrentFood.Name = txtName.Text;
            CurrentFood.Description = txtDescription.Text;
            CurrentFood.Energy.Double = SqlSafe.Double(txtCalories.Text);
            CurrentFood.TotalFats.Double = SqlSafe.Double(txtTotalFats.Text);
            CurrentFood.SaturatedFats.Double = SqlSafe.Double(txtSaturatedFats.Text);
            CurrentFood.Carbohydrates.Double = SqlSafe.Double(txtFoodCarbohydrates.Text);
            CurrentFood.Sugar.Double = SqlSafe.Double(txtSugar.Text);
            CurrentFood.Fibers.Double = SqlSafe.Double(txtFibers.Text);
            CurrentFood.Proteins.Double = SqlSafe.Double(txtProteins.Text);
            CurrentFood.Salt.Double = SqlSafe.Double(txtSalt.Text);
            //CurrentFood.Potassium.Double = SqlSafe.Double(txtPotassium.Text);
            //CurrentFood.GlycemicIndex.Double = SqlSafe.Double(txtGlicemicIndex.Text);
        }
        private void RefreshUi()
        {
            FromClassToUi();
            RefreshGrid();
        }
        private void RefreshGrid()
        {
            allFoods = bl.SearchFoods(CurrentFood.Name, CurrentFood.Description, 0);
            gridFoods.DataSource = allFoods;
            gridFoods.Refresh();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneFood(CurrentFood);
            FromClassToUi();
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                loading = true;
                CurrentFood = allFoods[e.RowIndex];
                gridFoods.Rows[e.RowIndex].Selected = true;
                FromClassToUi();
                loading = false;
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
        private void btnCleanFields_Click(object sender, EventArgs e)
        {
            txtIdFood.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtCalories.Text = "";
            txtTotalFats.Text = "";
            txtSaturatedFats.Text = "";
            txtFoodCarbohydrates.Text = "";
            txtSugar.Text = "";
            txtFibers.Text = "";
            txtProteins.Text = "";
            txtSalt.Text = "";
        }
        private void btnNewFood_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            CurrentFood.IdFood = null;
            bl.SaveOneFood(CurrentFood);
            RefreshUi();
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if (gridFoods.SelectedRows.Count == 0)
            {
                MessageBox.Show("Choose a food to delete");
                return; 
            }
            bl.DeleteOneFood(CurrentFood);
            RefreshUi();
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CurrentFood.Name =  txtName.Text;
                allFoods = bl.SearchFoods(txtName.Text, txtDescription.Text, 3); 
                if (allFoods != null)
                {
                    gridFoods.DataSource = allFoods;
                    gridFoods.Refresh();
                }
            }
        }
        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CurrentFood.Description = txtDescription.Text;
                allFoods = bl.SearchFoods(txtName.Text, txtDescription.Text, 3); 
                if (allFoods != null)
                {
                    gridFoods.DataSource = allFoods;
                    gridFoods.Refresh();
                }
            }
        }
    }
}
