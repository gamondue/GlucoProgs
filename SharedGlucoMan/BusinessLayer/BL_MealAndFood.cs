using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    internal class BL_MealAndFood
    {
        DataLayer dl = Common.Database;

        internal BL_MealAndFood()
        {

        }
        internal void RestoreMealData()
        {
            //dl.RestoreMealData(this);
        }
        internal void SaveMeal()
        {
            try
            {
                //dl.SaveMeal(this);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("BL_FoodToHitTargetCarbs | SaveData", ex);
            }
        }
    }
}
