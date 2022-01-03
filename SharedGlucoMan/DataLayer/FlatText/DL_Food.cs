using SharedData;
using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DataLayer
    {
        internal void RestoreWeighFood(GlucoMan.BusinessLayer.BL_WeighFood WeighFood)
        {
            if (File.Exists(persistentWeighFood))
                try
                {
                    string[] f = TextFile.FileToArray(persistentWeighFood);
                    WeighFood.T0RawGross.Text = f[0];
                    WeighFood.T0RawTare.Text = f[1];
                    WeighFood.T0RawGross.Text = f[2];
                    WeighFood.S1SauceNet.Text = f[3];
                    WeighFood.DiDish.Text = f[4];
                    WeighFood.TpPortionWithAll.Text = f[5];
                    WeighFood.M0pS1pPeRawFoodAndSauce.Text = f[6];
                    WeighFood.M1MainfoodCooked.Text = f[7];
                    WeighFood.M1pS1CourseCookedPlusSauce.Text = f[8];
                    WeighFood.ACookRatio.Text = f[9];
                    WeighFood.MppSpPortionOfCoursePlusSauce.Text = f[10];
                    WeighFood.PPercPercentageOfPortion.Text = f[11];
                    WeighFood.SpPortionOfSauceInGrams.Text = f[12];
                    WeighFood.Mp0PortionReportedToRaw.Text = f[13];
                    WeighFood.Mp1PortionCooked.Text = f[14];
                    WeighFood.ChoTotalMainfood.Text = f[15];
                    WeighFood.ChoSaucePercent.Text = f[16];
                    WeighFood.ChoMainfoodPercent.Text = f[17];
                    WeighFood.ChoTotalMainfood.Text = f[18];
                    WeighFood.ChoTotalSauce.Text = f[19];
                    WeighFood.S1pPotSaucePlusPot.Text = f[20];

                }
                catch (Exception ex)
                {
                    CommonData.CommonObj.LogOfProgram.Error("DL_WeighFood | RestoreWeighFood", ex);
                }
        }
        internal void SaveWeighFood(GlucoMan.BusinessLayer.BL_WeighFood WeighFood)
        {
            try
            {
                string file = WeighFood.T0RawGross.Text + "\n";
                file += WeighFood.T0RawTare.Text + "\n";
                file += WeighFood.T0RawGross.Text + "\n";
                file += WeighFood.S1SauceNet.Text + "\n";
                file += WeighFood.DiDish.Text + "\n";
                file += WeighFood.TpPortionWithAll.Text + "\n";
                file += WeighFood.M0pS1pPeRawFoodAndSauce.Text + "\n";
                file += WeighFood.M1MainfoodCooked.Text + "\n";
                file += WeighFood.M1pS1CourseCookedPlusSauce.Text + "\n";
                file += WeighFood.ACookRatio.Text + "\n";
                file += WeighFood.MppSpPortionOfCoursePlusSauce.Text + "\n";
                file += WeighFood.PPercPercentageOfPortion.Text + "\n";
                file += WeighFood.SpPortionOfSauceInGrams.Text + "\n";
                file += WeighFood.Mp0PortionReportedToRaw.Text + "\n";
                file += WeighFood.Mp1PortionCooked.Text + "\n";
                file += WeighFood.ChoTotalMainfood.Text + "\n";
                file += WeighFood.ChoSaucePercent.Text + "\n";
                file += WeighFood.ChoMainfoodPercent.Text + "\n";
                file += WeighFood.ChoTotalMainfood.Text + "\n";
                file += WeighFood.ChoTotalSauce.Text + "\n";
                file += WeighFood.S1pPotSaucePlusPot.Text + "\n";

                TextFile.StringToFile(persistentWeighFood, file, false);
            }
            catch (Exception ex)
            {
                CommonData.CommonObj.LogOfProgram.Error("DL_WeighFood | SaveWeighFood", ex);
            }
        }
        internal void SaveSingleFood(Food FoodToSave)
        {
            // read all foods
            List<Food> allFoods = ReadAllFoods();

            if (FoodToSave.IdFood.Int != 0)
            {
                foreach (Food f in allFoods)
                {
                    if (f.IdFood == FoodToSave.IdFood)
                    {
                        allFoods.Remove(f);
                        break;
                    }
                }
                allFoods.Add(FoodToSave);
            }
            else
            {
                // food without Id
                // find max Id
                int maxId = 0;
                foreach (Food f in allFoods)
                {
                    if (f.IdFood.Int > maxId)
                    {
                        maxId = f.IdFood.Int;
                    }
                }
                maxId++;
                FoodToSave.IdFood.Int = maxId;
                allFoods.Add(FoodToSave);
            }
            SaveAllFoods(allFoods);
        }
        internal void SaveAllFoods(List<Food> allFoods)
        {
            string file = "";
            try
            {
                foreach (Food FoodToSave in allFoods)
                {
                    file += FoodToSave.IdFood.Text + "\t";
                    file += FoodToSave.Calories.Text + "\t";
                    file += FoodToSave.Carbohydrates.Text + "\t";
                    file += FoodToSave.Name + "\t";
                    file += FoodToSave.Potassium.Text + "\t";
                    file += FoodToSave.Proteins.Text + "\t";
                    file += FoodToSave.Quantity.Text + "\t";
                    file += FoodToSave.Salt.Text + "\t";
                    file += FoodToSave.SaturatedFats.Text + "\t";
                    file += FoodToSave.Sugar.Text + "\t";
                    file += FoodToSave.TotalFats.Text + "\t";
                    file += FoodToSave.Fibers.Text + "\t";
                    file += "\n";
                }
                TextFile.StringToFile(persistentFoods, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal List<Food> ReadAllFoods()
        {
            List<Food> allFoods = new List<Food>();
            try
            {
                string[,] foodsStrings = TextFile.FileToMatrix(persistentFoods, '\t');
                for (int i = 0; i < foodsStrings.GetLength(0); i++)
                {
                    Food FoodToRead = new Food();
                    FoodToRead.IdFood.Text = foodsStrings[i, 0];
                    FoodToRead.Calories.Text = foodsStrings[i, 1];
                    FoodToRead.Carbohydrates.Text = foodsStrings[i, 2];
                    FoodToRead.Name = foodsStrings[i, 3];
                    FoodToRead.Potassium.Text = foodsStrings[i, 4];
                    FoodToRead.Proteins.Text = foodsStrings[i, 5];
                    FoodToRead.Quantity.Text = foodsStrings[i, 6];
                    FoodToRead.Salt.Text = foodsStrings[i, 7];
                    FoodToRead.SaturatedFats.Text = foodsStrings[i, 8];
                    FoodToRead.Sugar.Text = foodsStrings[i, 9];
                    FoodToRead.TotalFats.Text = foodsStrings[i, 10];
                    FoodToRead.Fibers.Text = foodsStrings[i, 11];

                    allFoods.Add(FoodToRead);
                }
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
            return allFoods;
        }
    }
}
