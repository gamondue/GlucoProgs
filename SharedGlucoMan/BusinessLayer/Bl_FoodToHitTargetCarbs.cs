using GlucoMan;
using SharedData;
using System;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    public class BL_FoodToHitTargetCarbs
    {
        DataLayer dl = Common.Database;

        public  DoubleAndText ChoAlreadyTaken = new DoubleAndText();
        public  DoubleAndText ChoOfFood = new DoubleAndText();
        public  DoubleAndText TargetCho = new DoubleAndText();
        public  DoubleAndText ChoLeftToTake = new DoubleAndText();
        public  DoubleAndText FoodToHitTarget = new DoubleAndText();

        public  BL_FoodToHitTargetCarbs()
        {
            //FoodToHitTarget.Format = "0"; 
        }
        public  void SaveData()
        {
            try
            {
                dl.SaveFoodToHitTarget(this);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("BL_FoodToHitTargetCarbs | SaveData", ex);
            }
        }
        public  void RestoreData()
        {
            dl.RestoreFoodToHitTargetCarbs(this);
        }
        public  void Calculations()
        {
            ChoLeftToTake.Double = (TargetCho.Double - ChoAlreadyTaken.Double);
            FoodToHitTarget.Double = ChoLeftToTake.Double * 100 / ChoOfFood.Double;
            SaveData(); 
        }
    }
}
