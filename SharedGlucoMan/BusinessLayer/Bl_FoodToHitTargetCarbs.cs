using GlucoMan;
using SharedData;
using System;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    class BL_FoodToHitTargetCarbs
    {
        DataLayer dl = Common.Database;

        internal DoubleAndText Hit_ChoAlreadyTaken = new DoubleAndText();
        internal DoubleAndText Hit_ChoOfFood = new DoubleAndText();
        internal DoubleAndText Hit_TargetCho = new DoubleAndText();
        internal DoubleAndText ChoLeftToTake = new DoubleAndText();
        internal DoubleAndText FoodToHitTarget = new DoubleAndText();

        internal BL_FoodToHitTargetCarbs()
        {
            //FoodToHitTarget.Format = "0"; 
        }
        internal void SaveData()
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
        internal void RestoreData()
        {
            dl.RestoreFoodToHitTargetCarbs(this);
        }
        internal void Calculations()
        {
            ChoLeftToTake.Double = (Hit_TargetCho.Double - Hit_ChoAlreadyTaken.Double);
            FoodToHitTarget.Double = ChoLeftToTake.Double * 100 / Hit_ChoOfFood.Double;
            SaveData(); 
        }
    }
}
