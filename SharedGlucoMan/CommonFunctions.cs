using SharedData;
using SharedGlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        public static void GeneralInitializations()
        {
            General.MakeFolderIfDontExist(Common.PathConfigurationData);
            General.MakeFolderIfDontExist(Common.PathProgramsData);
            General.MakeFolderIfDontExist(Common.PathLogs);
            General.MakeFolderIfDontExist(Common.PathDatabase);

            Common.LogOfProgram = new Logger(Common.PathLogs, true,
                @"GlucoMan_Log.txt",
                @"GlucoMan_Errors.txt",
                @"GlucoMan_Debug.txt",
                @"GlucoMan_Prompts.txt",
                @"GlucoMan_Data.txt");

            Common.Database = new DL_Sqlite();
            Common.Bl = new BL_General(); 
            Common.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        public static TypeOfMeal SelectTypeOfMealBasedOnTimeNow()
        {
            TypeOfMeal type;
            int hour = DateTime.Now.Hour;
            if (hour > breakfastStartHour && hour < breakfastEndHour)
                type = TypeOfMeal.Breakfast;
            else if (hour > lunchStartHour && hour < lunchEndHour)
                type = TypeOfMeal.Lunch;
            else if (hour > dinnerStartHour && hour < dinnerEndHour)
                type = TypeOfMeal.Dinner;
            else
                type = TypeOfMeal.Snack;
            return type;
        }
        public static double mgPerdL_To_mmolPerL(double BloodGlucose_mgPerdL)
        {
            return BloodGlucose_mgPerdL / 18; 
        }
        public static double mmolPerL_To_mgPerdL(double BloodGlucose_mmolPerL)
        {
            return 18 * BloodGlucose_mmolPerL;
        }
    }
}
