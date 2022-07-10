using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmFoods : Form
    {
        BL_MealAndFood bl = new BL_MealAndFood();

        Food currentFood = new Food();
        public Food CurrentFood { get => currentFood; set => currentFood = value; }
        //string currentnName;
        //string currentDescription;

        List<Food> allFoods;
        private FoodInMeal foodInMeal;
        
        private bool foodIsChosen;
        public bool FoodIsChosen { get => foodIsChosen;  }

        public frmFoods(Food Food)
        {
            InitializeComponent();
            currentFood = Food;
        }
        public frmFoods(string FoodNameForSearch)
        {
            InitializeComponent();
            currentFood.Name = FoodNameForSearch;
        }
        public frmFoods(FoodInMeal FoodInMeal)
        {
            InitializeComponent();
            currentFood = FormFoodInMealToFood(FoodInMeal);
        }
        private void frmFoods_Load(object sender, EventArgs e)
        {
            foodIsChosen = false; 
            FromClassToUi(); 
            ///////RefreshUi();
        }
        private void FromClassToUi()
        {
            txtIdFood.Text = currentFood.IdFood.ToString();
            txtName.Text = currentFood.Name;
            txtDescription.Text = currentFood.Description;
            txtCalories.Text = currentFood.Energy.Text;
            txtTotalFats.Text = currentFood.TotalFats.Text;
            txtSaturatedFats.Text = currentFood.SaturatedFats.Text;
            txtFoodCarbohydrates.Text = currentFood.Cho.Text;
            txtSugar.Text = currentFood.Sugar.Text;
            txtFibers.Text = currentFood.Fibers.Text;
            txtProteins.Text = currentFood.Proteins.Text;
            txtSalt.Text = currentFood.Salt.Text;
            //txtPotassium.Text = currentFood.Potassium.Text; 
            //txtGlicemicIndex.Text = currentFood.GlycemicIndex.Text;
        }
        private void FromUiToClass()
        {
            currentFood.IdFood = Safe.Int(txtIdFood.Text);
            currentFood.Name = txtName.Text;
            currentFood.Description = txtDescription.Text;
            currentFood.Energy.Double = Safe.Double(txtCalories.Text);
            currentFood.TotalFats.Double = Safe.Double(txtTotalFats.Text);
            currentFood.SaturatedFats.Double = Safe.Double(txtSaturatedFats.Text);
            currentFood.Cho.Double = Safe.Double(txtFoodCarbohydrates.Text);
            currentFood.Sugar.Double = Safe.Double(txtSugar.Text);
            currentFood.Fibers.Double = Safe.Double(txtFibers.Text);
            currentFood.Proteins.Double = Safe.Double(txtProteins.Text);
            currentFood.Salt.Double = Safe.Double(txtSalt.Text);
            //currentFood.Potassium.Double = Safe.Double(txtPotassium.Text);
            //currentFood.GlycemicIndex.Double = Safe.Double(txtGlicemicIndex.Text);
        }
        private void RefreshUi()
        {
            FromUiToClass();
            allFoods = bl.SearchFoods(currentFood.Name, currentFood.Description);
            gridFoods.DataSource = allFoods;
            gridFoods.Refresh();
        }
        private Food FormFoodInMealToFood(FoodInMeal foodInMeal)
        {
            Food f =  new Food();
            f.Name = foodInMeal.Name;
            f.Description = foodInMeal.Description;
            f.Cho = foodInMeal.ChoPercent;
            f.Sugar = foodInMeal.SugarPercent;
            f.Fibers = foodInMeal.FibersPercent;
            return f; 
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bl.SaveOneFood(currentFood);
            FromClassToUi();
            RefreshUi(); 
        }
        private void btnNewFood_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            currentFood.IdFood = null;
            bl.SaveOneFood(currentFood);
            RefreshUi();
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > 0)
            {
                currentFood = allFoods[e.RowIndex];
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
            bl.SaveOneFood(currentFood);
            this.Close(); 
        }
    }
}
