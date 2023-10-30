using gamon;
using System;
using System.Collections.Generic;

namespace GlucoMan
{
    internal class DiabetesM
    {
        internal static List<DiabetesMRecord> ImportData(string FileName)
        {
            List<List<string>> inputContent = TextFile.FileToListOfLists_GlobalParse(FileName, ',', '\"', '\n');
            List<DiabetesMRecord> listDiabetesM = new List<DiabetesMRecord>();
            //List<DiabetesMRecord> listDiabetesM = new List<DiabetesMRecord>(new DiabetesMRecord[inputContent.Count - 2]);

            for (int i = 2; i < inputContent.Count; i++)
            {
                DiabetesMRecord dbMRec = new DiabetesMRecord();
                dbMRec.DateTimeFormatted = SqlSafe.DateTime(inputContent[i][0].Substring(1, 10));
                dbMRec.Timestamp = dbMRec.DateTimeFormatted;
                dbMRec.Glucose = SqlSafe.Double(inputContent[i][1]);
                dbMRec.GlucoseValue.Double = SqlSafe.Double(dbMRec.Glucose);
                dbMRec.Carbs = SqlSafe.Double(inputContent[i][2]);
                dbMRec.CarbohydratesValue_g = dbMRec.Carbs;
                dbMRec.Proteins = SqlSafe.Double(inputContent[i][3]);
                dbMRec.Fats = SqlSafe.Double(inputContent[i][4]);
                dbMRec.Calories = SqlSafe.Double(inputContent[i][5]);
                dbMRec.Carb_bolus = SqlSafe.Double(inputContent[i][6]);
                dbMRec.Correction_bolus = SqlSafe.Double(inputContent[i][7]);
                dbMRec.Extended_bolus = SqlSafe.Double(inputContent[i][8]);
                dbMRec.Extended_bolus_duration = SqlSafe.Double(inputContent[i][9]);
                dbMRec.Basal = SqlSafe.Double(inputContent[i][10]);
                dbMRec.Basal_is_rate = SqlSafe.Bool(inputContent[i][11]);
                dbMRec.Bolus_insulin_type = SqlSafe.Double(inputContent[i][12]);
                dbMRec.Basal_insulin_type = SqlSafe.Double(inputContent[i][13]);
                dbMRec.Weight_entry = SqlSafe.Double(inputContent[i][14]);
                dbMRec.Weight = SqlSafe.Double(inputContent[i][15]);
                dbMRec.Category = SqlSafe.Int(inputContent[i][16]);
                dbMRec.Category_name = SqlSafe.String(inputContent[i][17]);
                dbMRec.Carb_ratio_factor = SqlSafe.Double(inputContent[i][18]);
                dbMRec.Insulin_sensitivity_factor = SqlSafe.Double(inputContent[i][19]);
                dbMRec.Notes = SqlSafe.String(inputContent[i][20]); // Field of base
                dbMRec.Is_sensor = SqlSafe.Bool(inputContent[i][21]);
                dbMRec.Pressure_sys = SqlSafe.Double(inputContent[i][22]);
                dbMRec.Pressure_dia = SqlSafe.Double(inputContent[i][23]);
                dbMRec.Pulse = SqlSafe.Double(inputContent[i][24]);
                dbMRec.Injection_bolus_site = SqlSafe.Int(inputContent[i][25]);
                dbMRec.Injection_basal_site = SqlSafe.Int(inputContent[i][26]);
                dbMRec.Finger_test_site = SqlSafe.Int(inputContent[i][27]);
                dbMRec.Ketones = SqlSafe.Double(inputContent[i][28]);
                dbMRec.Google_fit_source = SqlSafe.String(inputContent[i][29]);
                dbMRec.Timezone = SqlSafe.String(inputContent[i][30]);
                dbMRec.Exercise_index = SqlSafe.Int(inputContent[i][31]);
                dbMRec.Exercise_comment = SqlSafe.String(inputContent[i][32]);
                dbMRec.Exercise_duration = SqlSafe.Double(inputContent[i][33]);
                dbMRec.Medications = SqlSafe.String(inputContent[i][34]);
                dbMRec.Food = SqlSafe.String(inputContent[i][35]);
                dbMRec.Us_units = SqlSafe.Bool(inputContent[i][36]);
                dbMRec.Hba1c = SqlSafe.Double(inputContent[i][37]);
                dbMRec.Cholesterol_total = SqlSafe.Double(inputContent[i][38]);
                dbMRec.Cholesterol_ldl = SqlSafe.Double(inputContent[i][39]);
                dbMRec.Cholesterol_hdl = SqlSafe.Double(inputContent[i][40]);
                dbMRec.Triglycerides = SqlSafe.Double(inputContent[i][41]);
                dbMRec.Microalbumin_test_type = SqlSafe.Int(inputContent[i][42]);
                dbMRec.Microalbumin = SqlSafe.Double(inputContent[i][43]);
                dbMRec.Creatinine_clearance = SqlSafe.Double(inputContent[i][44]);
                dbMRec.Egfr = SqlSafe.Double(inputContent[i][45]);
                dbMRec.Cystatin_c = SqlSafe.Double(inputContent[i][46]);
                dbMRec.Albumin = SqlSafe.Double(inputContent[i][47]);
                dbMRec.Creatinine = SqlSafe.Double(inputContent[i][48]);
                dbMRec.Calcium = SqlSafe.Double(inputContent[i][49]);
                dbMRec.Total_protein = SqlSafe.Double(inputContent[i][50]);
                dbMRec.Sodium = SqlSafe.Double(inputContent[i][51]);
                dbMRec.Potassium = SqlSafe.Double(inputContent[i][52]);
                dbMRec.Bicarbonate = SqlSafe.Double(inputContent[i][53]);
                dbMRec.Chloride = SqlSafe.Double(inputContent[i][54]);
                dbMRec.Alp = SqlSafe.Double(inputContent[i][55]);
                dbMRec.Alt = SqlSafe.Double(inputContent[i][56]);
                dbMRec.Ast = SqlSafe.Double(inputContent[i][57]);
                dbMRec.Bilirubin = SqlSafe.Double(inputContent[i][58]);
                dbMRec.Bun = SqlSafe.Double(inputContent[i][59]);

                listDiabetesM.Add(dbMRec);
            }
            listDiabetesM.Sort((x, y) => DateTime.Compare((DateTime)x.Timestamp, (DateTime)y.Timestamp));
            return listDiabetesM;
        }
    }
    class DiabetesMRecord : GlucoseRecord
    {
        // from base class
        // 

        public DateTime? DateTimeFormatted { get; internal set; } // read also base value
        public double? Glucose { get; internal set; } // read also base value
        public double? Carbs { get; internal set; }
        public double? Proteins { get; internal set; }
        public double? Fats { get; internal set; }
        public double? Calories { get; internal set; }
        public double? Carb_bolus { get; internal set; }
        public double? Correction_bolus { get; internal set; }
        public double? Extended_bolus { get; internal set; }
        public double? Extended_bolus_duration { get; internal set; }
        public double? Basal { get; internal set; }
        public bool? Basal_is_rate { get; internal set; }
        public double? Bolus_insulin_type { get; internal set; }
        public double? Basal_insulin_type { get; internal set; }
        public double? Weight_entry { get; internal set; }
        public double? Weight { get; internal set; }
        public int? Category { get; internal set; }
        public string Category_name { get; internal set; }
        public double? Carb_ratio_factor { get; internal set; }
        public double? Insulin_sensitivity_factor { get; internal set; }
        //public string? Notes { get; internal set;} // GET FROM BASE
        public bool? Is_sensor { get; internal set; }
        public double? Pressure_sys { get; internal set; }
        public double? Pressure_dia { get; internal set; }
        public double? Pulse { get; internal set; }
        public int? Injection_bolus_site { get; internal set; }
        public int? Injection_basal_site { get; internal set; }
        public int? Finger_test_site { get; internal set; }
        public double? Ketones { get; internal set; }
        public string Google_fit_source { get; internal set; }
        public string Timezone { get; internal set; }
        public double? Exercise_index { get; internal set; }
        public string Exercise_comment { get; internal set; }
        public double? Exercise_duration { get; internal set; }
        public string Medications { get; internal set; }
        public string Food { get; internal set; }
        public bool? Us_units { get; internal set; }
        public double? Hba1c { get; internal set; }
        public double? Cholesterol_total { get; internal set; }
        public double? Cholesterol_ldl { get; internal set; }
        public double? Cholesterol_hdl { get; internal set; }
        public double? Triglycerides { get; internal set; }
        public double? Microalbumin_test_type { get; internal set; }
        public double? Microalbumin { get; internal set; }
        public double? Creatinine_clearance { get; internal set; }
        public double? Egfr { get; internal set; }
        public double? Cystatin_c { get; internal set; }
        public double? Albumin { get; internal set; }
        public double? Creatinine { get; internal set; }
        public double? Calcium { get; internal set; }
        public double? Total_protein { get; internal set; }
        public double? Sodium { get; internal set; }
        public double? Potassium { get; internal set; }
        public double? Bicarbonate { get; internal set; }
        public double? Chloride { get; internal set; }
        public double? Alp { get; internal set; }
        public double? Alt { get; internal set; }
        public double? Ast { get; internal set; }
        public double? Bilirubin { get; internal set; }
        public double? Bun { get; internal set; }
    }
}
