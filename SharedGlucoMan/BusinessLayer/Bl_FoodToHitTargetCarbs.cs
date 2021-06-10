using GlucoMan;
using SharedData;
using SharedFunctions;
using System;
using System.IO;

namespace SharedGlucoMan.BusinessLayer
{
    class Bl_FoodToHitTargetCarbs
    {
        string persistentStorage = CommonData.PathConfigurationData + @"FoodToHitTargetCarbs.txt";

        internal DoubleAndText ChoAlreadyTaken = new DoubleAndText();
        internal DoubleAndText ChoOfFood = new DoubleAndText();
        internal DoubleAndText TargetCho = new DoubleAndText();
        internal DoubleAndText FoodToHitTarget = new DoubleAndText();

        internal Bl_FoodToHitTargetCarbs()
        {
            FoodToHitTarget.Format = "0"; 
        }
        internal Bl_FoodToHitTargetCarbs RestoreData()
        {
            Bl_FoodToHitTargetCarbs w = null;
            if (File.Exists(persistentStorage))
                try
                {
                    string[] f = TextFile.FileToArray(persistentStorage);
                    ChoAlreadyTaken.Text = f[0];
                    ChoOfFood.Text = f[1];
                    TargetCho.Text = f[2];
                    //FoodToHitTarget.Text = f[3];
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
            return w;
        }
        internal void SaveData()
        {
            try
            {
                string file = ChoAlreadyTaken.Text + "\n";
                file += ChoOfFood.Text + "\n";
                file += TargetCho.Text + "\n";
                file += TargetCho.Text + "\n";
                //file += FoodToHitTarget.Text + "\n";
 
                TextFile.StringToFile(persistentStorage, file, false);
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
