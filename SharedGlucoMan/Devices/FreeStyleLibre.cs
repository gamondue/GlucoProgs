using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    class FreeStyleLibre
    {
        internal static List<FreeStyleLibreRecord> ImportData(string FileName)
        {
            List<List<string>> inputContent = TextFile.FileToListOfLists_GlobalParse(FileName, ',', '\"', '\n');
            List<FreeStyleLibreRecord> listFreeStyle = new List<FreeStyleLibreRecord>();
            //List<FreeStyleLibreRecord> listFreeStyle = new List<FreeStyleLibreRecord>(new FreeStyleLibreRecord[inputContent.Count - 2]);

            for (int i = 2; i < inputContent.Count; i++)
            {
                //int j = i - 2;
                //listFreeStyle[j].IdDeviceType = inputContent[i][0];
                //listFreeStyle[j].IdDevice = inputContent[i][1];
                //listFreeStyle[j].Timestamp = Safe.DateTime(inputContent[i][2]);
                //listFreeStyle[j].TypeOfDocument = Safe.Int(inputContent[i][3]);
                //listFreeStyle[j].GlucoseHistoricValue = Safe.Double(inputContent[i][4]);
                //listFreeStyle[j].GlucoseScanValue = Safe.Double(inputContent[i][5]);
                //listFreeStyle[j].InsulinRapidActionString = inputContent[i][6];
                //listFreeStyle[j].InsulinRapidActionValue = Safe.Double(inputContent[i][7]);
                //listFreeStyle[j].MealFoodString = inputContent[i][8];
                //listFreeStyle[j].CarbohydratesValue_grams = Safe.Double(inputContent[i][9]);
                //listFreeStyle[j].CarbohydratesString = inputContent[i][10];
                //listFreeStyle[j].InsulinSlowActionString = inputContent[i][11];
                //listFreeStyle[j].InsulinSlowActionValue = Safe.Double(inputContent[i][12]);
                //listFreeStyle[j].Notes = inputContent[i][13];
                //listFreeStyle[j].GlucoseStripValue_mg_dL = Safe.Double(inputContent[i][14]);
                //listFreeStyle[j].Chetons_mmol_L = Safe.Double(inputContent[i][15]);
                //listFreeStyle[j].MealInsulin = Safe.Double(inputContent[i][16]);
                //listFreeStyle[j].InsulinCorrection = Safe.Double(inputContent[i][17]);
                //listFreeStyle[j].InsulinWithUsersModifications = Safe.Double(inputContent[i][18]);

                FreeStyleLibreRecord grec = new FreeStyleLibreRecord();
                grec.IdDeviceType = inputContent[i][0];
                grec.IdDevice = inputContent[i][1];
                grec.Timestamp = Safe.DateTime(inputContent[i][2]);
                grec.TypeOfDocument = Safe.Int(inputContent[i][3]);
                grec.GlucoseHistoricValue = Safe.Double(inputContent[i][4]);
                grec.GlucoseScanValue = Safe.Double(inputContent[i][5]);
                grec.InsulinRapidActionString = inputContent[i][6];
                grec.InsulinRapidActionValue = Safe.Double(inputContent[i][7]);
                grec.MealFoodString = inputContent[i][8];
                grec.CarbohydratesValue_g = Safe.Double(inputContent[i][9]);
                grec.CarbohydratesString = inputContent[i][10];
                grec.InsulinSlowActionString = inputContent[i][11];
                grec.InsulinSlowActionValue = Safe.Double(inputContent[i][12]);
                grec.Notes = inputContent[i][13];
                grec.GlucoseStripValue_mg_dL = Safe.Double(inputContent[i][14]);
                grec.Chetons_mmol_L = Safe.Double(inputContent[i][15]);
                grec.MealInsulin = Safe.Double(inputContent[i][16]);
                grec.InsulinCorrection = Safe.Double(inputContent[i][17]);
                grec.InsulinWithUsersModifications = Safe.Double(inputContent[i][18]);

                switch (grec.TypeOfDocument)
                {
                    case 0: // SensorIntermediateValue
                        grec.GlucoseValue = grec.GlucoseHistoricValue;
                        grec.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.SensorIntermediateValue;
                        break;
                    case 1: // SensorScanValue
                        grec.GlucoseValue = grec.GlucoseScanValue;
                        grec.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.SensorScanValue;
                        break;
                    case 4: // insulin, rapid or extended effect 
                        grec.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.NotSet;

                        if (grec.InsulinRapidActionValue != null && grec.InsulinRapidActionValue != 0)
                        {
                            grec.InsulinValue = grec.InsulinRapidActionValue;
                            // default, possibly to be changed by processing other data
                            grec.InsulinInjectionType = Common.TypeOfInsulinInjection.CarbohydratesBolus;
                        }
                        else
                        {
                            grec.InsulinValue = grec.InsulinSlowActionValue;
                            grec.InsulinInjectionType = Common.TypeOfInsulinInjection.ExtendedEffectBolus;
                        }
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    default:
                        break;
                }
                listFreeStyle.Add(grec);
            }
            listFreeStyle.Sort((x, y) => DateTime.Compare((DateTime)x.Timestamp, (DateTime)y.Timestamp));

            return listFreeStyle; 
        }
    }
    class FreeStyleLibreRecord : GlucoseRecord
    {
        int? typeOfDocument;
        double? glucoseHistoricValue;
        double? glucoseScanValue;
        string insulinRapidActionString;
        double? insulinRapidActionValue;
        string insulinSlowActionString;
        double? insulinSlowActionValue;
        double? glucoseStripValue_mg_dL;
        double? chetons_mmol_L;

        public int? TypeOfDocument { get => typeOfDocument; set => typeOfDocument = value; }
        public double? GlucoseHistoricValue { get => glucoseHistoricValue; set => glucoseHistoricValue = value; }
        public double? GlucoseScanValue { get => glucoseScanValue; set => glucoseScanValue = value; }
        public string InsulinRapidActionString { get => insulinRapidActionString; set => insulinRapidActionString = value; }
        public double? InsulinRapidActionValue { get => insulinRapidActionValue; set => insulinRapidActionValue = value; }
        public string InsulinSlowActionString { get => insulinSlowActionString; set => insulinSlowActionString = value; }
        public double? InsulinSlowActionValue { get => insulinSlowActionValue; set => insulinSlowActionValue = value; }
        public double? GlucoseStripValue_mg_dL { get => glucoseStripValue_mg_dL; set => glucoseStripValue_mg_dL = value; }
        public double? Chetons_mmol_L { get => chetons_mmol_L; set => chetons_mmol_L = value; }
        public double? MealInsulin { get; internal set; }
        public double? InsulinCorrection { get; internal set; }
        public double? InsulinWithUsersModifications { get; internal set; }
    }
}
