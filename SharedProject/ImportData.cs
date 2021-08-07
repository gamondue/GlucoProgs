using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal class ImportData
    {
        internal List<GlucoseRecord> ImportDataFromFreeStyleLibre(string FileName)
        {
            List<List<string>> inputContent = TextFile.FileToListOfLists(FileName, ',', '\"');
            List<GlucoseRecord> listFreeStyle = new List<GlucoseRecord>();
            for (int i = 2; i < inputContent.Count; i++)
            {
                FreeStyleLibreRecord grec = new FreeStyleLibreRecord();
                grec.DeviceType = inputContent[i][0];
                grec.DeviceId = inputContent[i][1];
                grec.Timestamp = SafeRead.SafeDateTime(inputContent[i][2]);
                grec.TypeOfDocument = SafeRead.SafeInt(inputContent[i][3]);
                grec.GlucoseHistoricValue = SafeRead.SafeDouble(inputContent[i][4]);
                grec.GlucoseScanValue = SafeRead.SafeDouble(inputContent[i][5]);
                grec.InsulinRapidActionString = inputContent[i][6];
                grec.InsulinRapidActionValue = SafeRead.SafeDouble(inputContent[i][7]);
                grec.MealFoodString = inputContent[i][8];
                grec.CarbohydratesValue_grams = SafeRead.SafeDouble(inputContent[i][9]);
                grec.CarbohydratesString = inputContent[i][10];
                grec.InsulinSlowActionString = inputContent[i][11];
                grec.InsulinSlowActionValue = SafeRead.SafeDouble(inputContent[i][12]);
                grec.Notes = inputContent[i][13];
                grec.GlucoseStripValue_mg_dL = SafeRead.SafeDouble(inputContent[i][14]);
                grec.Chetons_mmol_L = SafeRead.SafeDouble(inputContent[i][15]);
                grec.MealInsulin = SafeRead.SafeDouble(inputContent[i][16]);
                grec.InsulinCorrection = SafeRead.SafeDouble(inputContent[i][17]);
                grec.InsulinWithUsersModifications = SafeRead.SafeDouble(inputContent[i][18]);

                listFreeStyle.Add(grec);
            }
            //listFreeStyle.Sort();
            return listFreeStyle; 
        }
    }
}
