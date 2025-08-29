using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan
{
    public static partial class Common
    {
        public static void GeneralInitializationsAsync()
        {
            // generation of folders. Since File.Create() doesn't work if the folder
            // doesn't exist, the following statements should be executed before
            // any code that creates or uses files is run 
            General.MakeFolderIfDontExist(Common.PathLogs);
            General.MakeFolderIfDontExist(Common.PathDatabase);
            General.MakeFolderIfDontExist(Common.PathImportExport);

            Common.Database = new DL_Sqlite();
            Common.BlGeneral = new BL_General();
            Common.MealAndFood_CommonBL = new BusinessLayer.BL_MealAndFood();

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
