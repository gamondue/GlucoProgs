using GlucoMan.BusinessLayer;
using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DataLayer
    {
        internal void RestoreFoodToHitTargetCarbs(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget)
        {
            if (File.Exists(persistentFoodToEatTarget))
                try
                {
                    string[] f = TextFile.FileToArray(persistentFoodToEatTarget);
                    CalculationsOfChoMassToHitTarget.ChoAlreadyTaken.Text = f[0];
                    CalculationsOfChoMassToHitTarget.ChoOfFood.Text = f[1];
                    CalculationsOfChoMassToHitTarget.TargetCho.Text = f[2];
                    //FoodToHitTarget.Text = f[3];
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
        }

        internal void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget)
        {
            try
            {
                string file = CalculationsOfChoMassToHitTarget.ChoAlreadyTaken.Text + "\n";
                file += CalculationsOfChoMassToHitTarget.ChoOfFood.Text + "\n";
                file += CalculationsOfChoMassToHitTarget.TargetCho.Text + "\n";
                file += CalculationsOfChoMassToHitTarget.TargetCho.Text + "\n";

                TextFile.StringToFile(persistentFoodToEatTarget, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
    }
}
