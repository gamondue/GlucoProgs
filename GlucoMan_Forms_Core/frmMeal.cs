using System;
using System.Windows.Forms;
using GlucoMan;

namespace GlucoMan_Forms_Core
{
    public partial class frmMeal : Form
    {
        Meal thisMeal = new Meal(); 

        public frmMeal()
        {
            InitializeComponent();
        }
        private void viewFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void viewFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Food f = new Food(); 
            frmFood fd = new frmFood(f);
            fd.Show(); 
        }

        private void frmMeal_Load(object sender, EventArgs e)
        {

        }
        private void FromClassToUi()
        {
            switch (thisMeal.Type)
            {
                case (Meal.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.Checked = true;
                    break;
                case (Meal.TypeOfMeal.Dinner):
                    rdbIsDinner.Checked = true;
                    break;
                case (Meal.TypeOfMeal.Lunch):
                    rdbIsLunch.Checked = true;
                    break;
                case (Meal.TypeOfMeal.Snack):
                    rdbIsSnack.Checked = true;
                    break;
            }
        }
    }
}
