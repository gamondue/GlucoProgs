using GlucoMan;
using SharedData;
using System;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    public class BL_FoodToHitTargetCarbs
    {
        DataLayer dl = Common.Database;

        public  DoubleAndText Hit_ChoAlreadyTaken = new DoubleAndText();
        public  DoubleAndText Hit_ChoOfFood = new DoubleAndText();
        public  DoubleAndText Hit_TargetCho = new DoubleAndText();
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
            ChoLeftToTake.Double = (Hit_TargetCho.Double - Hit_ChoAlreadyTaken.Double);
            FoodToHitTarget.Double = ChoLeftToTake.Double * 100 / Hit_ChoOfFood.Double;
            SaveData(); 
        }
    }
}
