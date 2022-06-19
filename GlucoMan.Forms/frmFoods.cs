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
        List<Food> allFoods;
        public frmFoods()
        {
            InitializeComponent();
        }
        public frmFoods(string FoodNameForSearch)
        {
            InitializeComponent();
            currentFood.Name = FoodNameForSearch;
            allFoods = bl.SearchFoods(currentFood);
            gridFoods.DataSource = bl.ReadFoods();
        }
        private void frmFoods_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            fromUiToClass();
            bl.SaveOneFood(currentFood);
            fromClassToUi();
            RefreshGrid(); 
        }

        private void RefreshGrid()
        {
            Food dummy = new Food();
            dummy.Name = "prova";
            allFoods = bl.SearchFoods(dummy);
            gridFoods.DataSource = allFoods;
            gridFoods.Refresh();
        }

        private void btnNewFood_Click(object sender, EventArgs e)
        {
            fromUiToClass();
            currentFood.IdFood = null;
            bl.SaveOneFood(currentFood);
            fromClassToUi();
            RefreshGrid();
        }
        private void fromClassToUi()
        {
            txtIdFood.Text = currentFood.IdFood.ToString();
            txtName.Text = currentFood.Name;
            txtDescription.Text = currentFood.Description;
            txtCalories.Text = currentFood.Energy.Text;
            txtTotalFats.Text = currentFood.TotalFats.Text;
            txtSaturatedFats.Text = currentFood.SaturatedFats.Text;
            txtFoodCarbohydrates.Text = currentFood.Carbohydrates.ToString();
            txtSugar.Text = currentFood.Sugar.Text;
            txtFibers.Text = currentFood.Fibers.Text;
            txtProteins.Text = currentFood.Proteins.Text;
            txtSalt.Text = currentFood.Salt.Text;
            //txtPotassium.Text = currentFood.Potassium.Text; 
            //txtGlicemicIndex.Text = currentFood.GlycemicIndex.Text;
        }
        private void fromUiToClass()
        {
            currentFood.IdFood = Safe.Int(txtIdFood.Text);
            currentFood.Name = txtName.Text;
            currentFood.Description = txtDescription.Text;
            currentFood.Energy.Double = Safe.Double(txtCalories.Text);
            currentFood.TotalFats.Double = Safe.Double(txtTotalFats.Text);
            currentFood.SaturatedFats.Double = Safe.Double(txtSaturatedFats.Text);
            currentFood.Carbohydrates.Double = Safe.Double(txtFoodCarbohydrates.Text);
            currentFood.Sugar.Double = Safe.Double(txtSugar.Text);
            currentFood.Fibers.Double = Safe.Double(txtFibers.Text);
            currentFood.Proteins.Double = Safe.Double(txtProteins.Text);
            currentFood.Salt.Double = Safe.Double(txtSalt.Text);
            //currentFood.Potassium.Double = Safe.Double(txtPotassium.Text);
            //currentFood.GlycemicIndex.Double = Safe.Double(txtGlicemicIndex.Text);
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
                fromClassToUi();
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
    }
}
