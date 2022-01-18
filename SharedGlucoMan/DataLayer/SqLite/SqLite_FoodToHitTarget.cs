using GlucoMan.BusinessLayer;
using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public  partial class DL_Sqlite : DataLayer
    {
        public  override void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs Parameters)
        {
            SaveParameter("Hit_ChoAlreadyTaken", Parameters.Hit_ChoAlreadyTaken.Text);
            SaveParameter("Hit_ChoOfFood", Parameters.Hit_ChoOfFood.Text);
            SaveParameter("Hit_TargetCho", Parameters.Hit_TargetCho.Text);
        }
        public  override void RestoreFoodToHitTargetCarbs(BL_FoodToHitTargetCarbs Parameters)
        {
            Parameters.Hit_ChoAlreadyTaken.Text = RestoreParameter("Hit_ChoAlreadyTaken");
            Parameters.Hit_ChoOfFood.Text = RestoreParameter("Hit_ChoOfFood");
            Parameters.Hit_TargetCho.Text = RestoreParameter("Hit_TargetCho");
        }
    }
}
