using GlucoMan.BusinessLayer;
using SharedGlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmFoodToHitTargetCarbs : Form
    {
        BL_FoodToHitTargetCarbs foodToEat = new BL_FoodToHitTargetCarbs();

        public frmFoodToHitTargetCarbs()
        {
            InitializeComponent();

            foodToEat.RestoreData();
            FromClassToUi();
        }
        private void frmFoodToHitTargetCarbs_Load(object sender, EventArgs e)
        {

        }
        private void FromClassToUi()
        {
            TxtChoAlreadyTaken.Text = foodToEat.ChoAlreadyTaken.Text;
            TxtChoOfFood.Text = foodToEat.ChoOfFood.Text;
            TxtTargetCho.Text = foodToEat.TargetCho.Text;
            TxtChoLeftToTake.Text = foodToEat.ChoLeftToTake.Text;
            TxtFoodToHitTarget.Text = foodToEat.FoodToHitTarget.Text;
        }
        internal void FromUiToClass()
        {
            foodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            foodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            foodToEat.TargetCho.Text = TxtTargetCho.Text;
            foodToEat.ChoLeftToTake.Text = TxtChoLeftToTake.Text; 
            foodToEat.FoodToHitTarget.Text = TxtFoodToHitTarget.Text; 
        }
        private void TxtChoAlreadyTaken_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
        {
            foodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtChoOfFood_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtChoOfFood_Leave(object sender, EventArgs e)
        {
            foodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
            foodToEat.Calculations();

            FromClassToUi();
        }
        private void TxtTargetCho_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtTargetCho_Leave(object sender, EventArgs e)
        {
            foodToEat.TargetCho.Text = TxtTargetCho.Text;
            foodToEat.Calculations(); 
            FromClassToUi();
        }
        private void btnCalculateGrams_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            foodToEat.Calculations();
            FromClassToUi();
        }
        private void btnReadTarget_Click(object sender, EventArgs e)
        {
            BL_General bl = new BL_General();
            foodToEat.TargetCho.Double = Safe.Double(bl.RestoreParameter("ChoToEat"));
            FromClassToUi();
        }
        private void btnReadAll_Click(object sender, EventArgs e)
        {
            BL_General bl = new BL_General();
            foodToEat.TargetCho.Double = Safe.Double(bl.RestoreParameter("ChoToEat"));
            FromClassToUi();
        }
        private void btnReadChoTaken_Click(object sender, EventArgs e)
        {

        }
        private void btnReadFood_Click(object sender, EventArgs e)
        {

        }
    }
}
