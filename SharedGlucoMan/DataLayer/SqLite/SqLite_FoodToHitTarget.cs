using GlucoMan.BusinessLayer;
using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal  partial class DL_Sqlite : DataLayer
    {
        internal  override void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs Parameters)
        {
            SaveParameter("Hit_ChoAlreadyTaken", Parameters.ChoAlreadyTaken.Text);
            SaveParameter("Hit_ChoOfFood", Parameters.ChoOfFood.Text);
            SaveParameter("Hit_TargetCho", Parameters.TargetCho.Text);
        }
        internal  override void RestoreFoodToHitTargetCarbs(BL_FoodToHitTargetCarbs Parameters)
        {
            Parameters.ChoAlreadyTaken.Text = RestoreParameter("Hit_ChoAlreadyTaken");
            Parameters.ChoOfFood.Text = RestoreParameter("Hit_ChoOfFood");
            Parameters.TargetCho.Text = RestoreParameter("Hit_TargetCho");
        }
    }
}
