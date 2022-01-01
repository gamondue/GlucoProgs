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
                grec.IdDevice = inputContent[i][1];
                grec.Timestamp = SafeRead.DateTime(inputContent[i][2]);
                grec.TypeOfDocument = SafeRead.Int(inputContent[i][3]);
                grec.GlucoseHistoricValue = SafeRead.Double(inputContent[i][4]);
                grec.GlucoseScanValue = SafeRead.Double(inputContent[i][5]);
                grec.InsulinRapidActionString = inputContent[i][6];
                grec.InsulinRapidActionValue = SafeRead.Double(inputContent[i][7]);
                grec.MealFoodString = inputContent[i][8];
                grec.CarbohydratesValue_grams = SafeRead.Double(inputContent[i][9]);
                grec.CarbohydratesString = inputContent[i][10];
                grec.InsulinSlowActionString = inputContent[i][11];
                grec.InsulinSlowActionValue = SafeRead.Double(inputContent[i][12]);
                grec.Notes = inputContent[i][13];
                grec.GlucoseStripValue_mg_dL = SafeRead.Double(inputContent[i][14]);
                grec.Chetons_mmol_L = SafeRead.Double(inputContent[i][15]);
                grec.MealInsulin = SafeRead.Double(inputContent[i][16]);
                grec.InsulinCorrection = SafeRead.Double(inputContent[i][17]);
                grec.InsulinWithUsersModifications = SafeRead.Double(inputContent[i][18]);

                listFreeStyle.Add(grec);
            }
            //listFreeStyle.Sort();
            return listFreeStyle; 
        }
    }
}
