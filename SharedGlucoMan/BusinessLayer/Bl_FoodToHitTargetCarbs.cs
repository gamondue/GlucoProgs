using GlucoMan;
using SharedData;
using SharedFunctions;
using System;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    class Bl_FoodToHitTargetCarbs
    {
        DataLayer dl = new DataLayer();

        internal DoubleAndText ChoAlreadyTaken = new DoubleAndText();
        internal DoubleAndText ChoOfFood = new DoubleAndText();
        internal DoubleAndText TargetCho = new DoubleAndText();
        internal DoubleAndText FoodToHitTarget = new DoubleAndText();

        internal Bl_FoodToHitTargetCarbs()
        {
            //FoodToHitTarget.Format = "0"; 
        }
        internal void RestoreData()
        {
            dl.RestoreFoodToHitTargetCarbs(this);
        }
        internal void SaveData()
        {
            try
            {
                dl.SaveFoodToHitTarget(this);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void Calculations()
        {
            FoodToHitTarget.Double = (TargetCho.Double - ChoAlreadyTaken.Double) *
                100 / ChoOfFood.Double;
            SaveData(); 
        }
    }
}
