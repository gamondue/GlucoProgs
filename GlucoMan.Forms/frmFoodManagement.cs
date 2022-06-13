using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmFoodManagement : Form
    {
        BL_MealAndFood bl = new BL_MealAndFood();
        
        Food currentFood = new Food();

        List<Food> currentFoods; 
        public frmFoodManagement()
        {
            InitializeComponent();
        }
        public frmFoodManagement(string FoodName, string FoodDescription)
        {
            InitializeComponent();
            currentFood.Name = FoodName;
            currentFood.Description = FoodDescription;

            currentFoods = bl.SearchFoods(currentFood); 
        }
        private void frmFoodManagement_Load(object sender, EventArgs e)
        {
            // !!!! ????  is it right to get persistence throught the object ???? foods = currentFood.ReadAllFoods();
            //foods = currentFood.OrderBy(n => n.IdFood).ToList(); 
            gridFoods.DataSource = null;
            gridFoods.DataSource = currentFoods;
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void picCalculator_Click(object sender, EventArgs e)
        {
            double value;
            double.TryParse(this.ActiveControl.Text, out value);
            frmCalculator calculator = new frmCalculator(value);
            calculator.ShowDialog();
            this.ActiveControl.Text = calculator.Result.ToString();
        }
    }
}
