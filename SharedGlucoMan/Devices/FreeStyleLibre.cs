using gamon;
using System;
using System.Collections.Generic;
using static GlucoMan.Common;

namespace GlucoMan
{
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
        public TypeOfGlucoseMeasurement GlucoseMeasurementType { get; internal set; }
    }
    class FreeStyleLibre
    {
        internal static List<FreeStyleLibreRecord> ImportData(string FileName)
        {
            List<List<string>> inputContent = TextFile.FileToListOfLists_GlobalParse(FileName, ',', '\"', '\n');
            List<FreeStyleLibreRecord> listFreeStyle = new List<FreeStyleLibreRecord>();
            // data starts from the third row
            for (int i = 2; i < inputContent.Count - 1; i++)
            {
                FreeStyleLibreRecord singleRecord = new FreeStyleLibreRecord();
                // the TypeOfGlucoseMeasurementDevice is decided in code, based on the TypeOfDocument field
                //singleRecord.TypeOfGlucoseMeasurementDevice = inputContent[i][0];
                singleRecord.IdDevice = inputContent[i][1];
                singleRecord.Timestamp.DateTime = Safe.DateTime(inputContent[i][2]);
                singleRecord.TypeOfDocument = Safe.Int(inputContent[i][3]);
                singleRecord.GlucoseHistoricValue = Safe.Double(inputContent[i][4]);
                singleRecord.GlucoseScanValue = Safe.Double(inputContent[i][5]);
                singleRecord.InsulinRapidActionString = inputContent[i][6];
                singleRecord.InsulinRapidActionValue = Safe.Double(inputContent[i][7]);
                singleRecord.MealFoodString = inputContent[i][8];
                singleRecord.CarbohydratesValue_grams = Safe.Double(inputContent[i][9]);
                singleRecord.CarbohydratesString = inputContent[i][10];
                singleRecord.InsulinSlowActionString = inputContent[i][11];
                singleRecord.InsulinSlowActionValue = Safe.Double(inputContent[i][12]);
                singleRecord.Notes = inputContent[i][13];
                singleRecord.GlucoseStripValue_mg_dL = Safe.Double(inputContent[i][14]);
                singleRecord.Chetons_mmol_L = Safe.Double(inputContent[i][15]);
                singleRecord.MealInsulin = Safe.Double(inputContent[i][16]);
                singleRecord.InsulinCorrection = Safe.Double(inputContent[i][17]);
                singleRecord.InsulinWithUsersModifications = Safe.Double(inputContent[i][18]);
                switch (singleRecord.TypeOfDocument)
                {
                    case 0: // SensorIntermediateValue, taken autonomously from the sensor
                        singleRecord.GlucoseValue.Double = singleRecord.GlucoseHistoricValue;
                        singleRecord.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.SensorIntermediateValue;
                        singleRecord.TypeOfGlucoseMeasurementDevice = TypeOfGlucoseMeasurementDevice.UnderSkinSensor;
                        break;
                    case 1: // SensorScanValue, explicitly scanned with NFC from user
                        singleRecord.GlucoseValue.Double = singleRecord.GlucoseScanValue;
                        singleRecord.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.SensorScanValue;
                        singleRecord.TypeOfGlucoseMeasurementDevice = TypeOfGlucoseMeasurementDevice.UnderSkinSensor;
                        break;
                    case 2: // glucose reactive strip 
                        singleRecord.GlucoseValue.Double = singleRecord.GlucoseStripValue_mg_dL;
                        singleRecord.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.GlucoseReactiveStripValue;
                        singleRecord.TypeOfGlucoseMeasurementDevice = TypeOfGlucoseMeasurementDevice.FingerPuncture;
                        break;
                    case 3: // no rows in my files, can't infere the meaning
                        break;
                    case 4: // insulin injection, rapid or extended effect 
                        // program gives the data of the two cases the same format
                        // default
                        singleRecord.GlucoseMeasurementType = Common.TypeOfGlucoseMeasurement.NotSet;
                        if (singleRecord.InsulinRapidActionValue != null && singleRecord.InsulinRapidActionValue != 0)
                        {
                            singleRecord.InsulinValue = singleRecord.InsulinRapidActionValue;
                            singleRecord.InsulinInjectionType = Common.InsulinDrug.BolusInsulin;
                        }
                        else
                        {
                            singleRecord.InsulinValue = singleRecord.InsulinSlowActionValue;
                            singleRecord.InsulinInjectionType = Common.InsulinDrug.BasalInsulin;
                        }
                        break;
                    case 5: // carbohydrates ingested with food 
                        // value is in CarbohydratesValue_grams and doesn't need further processing
                        break;
                    case 6: // I have some rows in my files with no data in the other columns, can't infere the meaning
                        break;
                    default:
                        break;
                }
                listFreeStyle.Add(singleRecord);
            }
            // sort by date, since the original file is ordered by TypeOfDocument
            listFreeStyle.Sort((x, y) => DateTime.Compare((DateTime)x.Timestamp.DateTime, (DateTime)y.Timestamp.DateTime));

            return listFreeStyle;
        }
    }
}
