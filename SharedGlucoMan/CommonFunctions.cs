using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal static partial class Common
    {
        internal static void GeneralInitializations()
        {
            General.MakeFolderIfDontExist(Common.PathConfigurationData);
            General.MakeFolderIfDontExist(Common.PathProgramsData);
            General.MakeFolderIfDontExist(Common.PathLogs);

            Common.LogOfProgram = new Logger(Common.PathLogs, true,
                @"GlucoMan_Log.txt",
                @"GlucoMan_Errors.txt",
                @"GlucoMan_Debug.txt",
                @"GlucoMan_Prompts.txt",
                @"GlucoMan_Data.txt");

            Common.Database = new DL_FlatText();

            Common.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        internal static TypeOfMeal SelectMealBasedOnTimeNow()
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
    }
}
