using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmFoodToHitTargetCarbs : Form
    {
        BL_FoodToHitTargetCarbs blFoodToEat = new BL_FoodToHitTargetCarbs();
        //BL_General Common.BlGeneral = new BL_General();

        public frmFoodToHitTargetCarbs()
        {
            InitializeComponent();
            // read data from other pages
            blFoodToEat.TargetCho.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("Bolus_ChoToEat"));
            blFoodToEat.NameOfFood = SqlSafe.String(Common.BlGeneral.RestoreParameter("FoodInMeal_Name"));

            blFoodToEat.RestoreParametrs();

            FromClassToUi();
        }
        private void frmFoodToHitTargetCarbs_Load(object sender, EventArgs e)
        {

        }
        private void FromClassToUi()
        {
            TxtChoAlreadyTaken.Text = blFoodToEat.ChoAlreadyTaken.Text;
            TxtChoOfFood.Text = blFoodToEat.ChoOfFood.Text;
            TxtTargetCho.Text = blFoodToEat.TargetCho.Text;
            TxtChoLeftToTake.Text = blFoodToEat.ChoLeftToTake.Text;
            TxtFoodToHitTarget.Text = blFoodToEat.FoodToHitTarget.Text;
            TxtFoodName.Text = blFoodToEat.NameOfFood; 
        }
        internal void FromUiToClass()
        {
            blFoodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            blFoodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            blFoodToEat.TargetCho.Text = TxtTargetCho.Text;
            blFoodToEat.ChoLeftToTake.Text = TxtChoLeftToTake.Text; 
            blFoodToEat.FoodToHitTarget.Text = TxtFoodToHitTarget.Text;
            blFoodToEat.NameOfFood = TxtFoodName.Text;
        }
        private void TxtChoAlreadyTaken_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            blFoodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            blFoodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtChoOfFood_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            blFoodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            blFoodToEat.Calculations();
            FromClassToUi();
        }
        private void TxtTargetCho_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            blFoodToEat.TargetCho.Text = TxtTargetCho.Text;
            blFoodToEat.Calculations(); 
            FromClassToUi();
        }
        private void btnCalculateGrams_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            blFoodToEat.Calculations();
            FromClassToUi();
        }
        private void btnReadAll_Click(object sender, EventArgs e)
        {
            blFoodToEat.TargetCho.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("Bolus_ChoToEat"));
            blFoodToEat.ChoAlreadyTaken.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("Meal_ChoGrams"));
            blFoodToEat.ChoOfFood.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("FoodInMeal_CarbohydratesPercent"));
            blFoodToEat.NameOfFood = SqlSafe.String(Common.BlGeneral.RestoreParameter("FoodInMeal_Name"));
            FromClassToUi();
            TxtChoLeftToTake.Text = "";
            TxtFoodToHitTarget.Text = ""; 
        }
        private void btnReadTargetCho_Click(object sender, EventArgs e)
        {
            blFoodToEat.TargetCho.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("Bolus_ChoToEat"));
            FromClassToUi();
        }
        private void btnReadChoTaken_Click(object sender, EventArgs e)
        {
            blFoodToEat.ChoAlreadyTaken.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("Meal_ChoGrams"));
            FromClassToUi();
        }
        private void btnReadFood_Click(object sender, EventArgs e)
        {
            blFoodToEat.ChoOfFood.Double = SqlSafe.Double(Common.BlGeneral.RestoreParameter("FoodInMeal_CarbohydratesPercent"));
            blFoodToEat.NameOfFood = SqlSafe.String(Common.BlGeneral.RestoreParameter("FoodInMeal_Name"));
            FromClassToUi();
        }
        private void btnInjection_Click(object sender, EventArgs e)
        {
            frmInjections frm = new frmInjections();
            frm.Show();
        }
    }
}
