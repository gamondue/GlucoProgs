using System;
using System.Windows.Forms;
using GlucoMan; 

namespace GlucoMan.Forms
{
    public partial class frmMeal : Form
    {
        Meal thisMeal = new Meal();
        public frmMeal()
        {
            InitializeComponent();
        }
        private void frmMeal_Load(object sender, EventArgs e)
        {
            thisMeal.Type = Common.SelectMealBasedOnTimeNow();
            SetCorrectRadiobutton(thisMeal.Type);
        }
        private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Food f = new Food(); 
            frmFood fd = new frmFood(f);
            fd.Show(); 
        }
        private void FromClassToUi()
        {
            SetCorrectRadiobutton(thisMeal.Type); 
        }
        private void FromUiToClass()
        {
            
        }
        private void btnStartMeal_Click(object sender, EventArgs e)
        {

        }
        private void btnEndMeal_Click(object sender, EventArgs e)
        {

        }
        private void SetCorrectRadiobutton(Common.TypeOfMeal Type)
        {
            switch (Type)
            {
                case (Common.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.Checked = true;
                    break;
                case (Common.TypeOfMeal.Dinner):
                    rdbIsDinner.Checked = true;
                    break;
                case (Common.TypeOfMeal.Lunch):
                    rdbIsLunch.Checked = true;
                    break;
                case (Common.TypeOfMeal.Snack):
                    rdbIsSnack.Checked = true;
                    break;
            }
        }
    }
}
