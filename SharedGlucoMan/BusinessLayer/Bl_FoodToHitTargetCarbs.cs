using gamon;

namespace GlucoMan.BusinessLayer
{
    public class BL_FoodToHitTargetCarbs
    {
        DataLayer dl = Common.Database;

        public DoubleAndText ChoAlreadyTaken = new DoubleAndText();
        public DoubleAndText ChoOfFood = new DoubleAndText();
        public string NameOfFood = "";
        public DoubleAndText TargetCho = new DoubleAndText();
        public  DoubleAndText ChoLeftToTake = new DoubleAndText();
        public DoubleAndText FoodToHitTarget = new DoubleAndText();

        public BL_FoodToHitTargetCarbs()
        {
            //FoodToHitTarget.Format = "0"; 
        }
        public  void SaveParameters()
        {
            try
            {
                dl.SaveParameter("Hit_ChoAlreadyTaken", ChoAlreadyTaken.Text);
                dl.SaveParameter("Hit_ChoOfFood", ChoOfFood.Text);
                dl.SaveParameter("Hit_TargetCho", TargetCho.Text);
                dl.SaveParameter("Hit_NameOfFood", NameOfFood);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("BL_FoodToHitTargetCarbs | SaveData", ex);
            }
        }
        public  void RestoreParameters()
        {
            ChoAlreadyTaken.Text = dl.RestoreParameter("Hit_ChoAlreadyTaken");
            ChoOfFood.Text = dl.RestoreParameter("Hit_ChoOfFood");
            TargetCho.Text = dl.RestoreParameter("Hit_TargetCho");
            NameOfFood = dl.RestoreParameter("Hit_NameOfFood");
        }
        public  void Calculations()
        {
            ChoLeftToTake.Double = (TargetCho.Double - ChoAlreadyTaken.Double);
            FoodToHitTarget.Double = ChoLeftToTake.Double * 100 / ChoOfFood.Double;
            SaveParameters(); 
        }
    }
}
