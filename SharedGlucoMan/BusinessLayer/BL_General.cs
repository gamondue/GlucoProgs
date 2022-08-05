using GlucoMan;
using System.IO; 
using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.DataLayer;

namespace SharedGlucoMan.BusinessLayer
{
    // Business Layer, general part
    public class BL_General
    {
        DataLayer dl = Common.Database;
        public  long? SaveParameter(string FieldName, string FieldValue)
        {
            return dl.SaveParameter(FieldName, FieldValue);
        }
        public  string RestoreParameter(string FieldName)
        {
            return dl.RestoreParameter(FieldName);
        }
        public bool DeleteDatabase()
        {
            return dl.DeleteDatabase();
        }
        internal bool ExportProgramsFiles()
        {
            try
            {
                // database file
                File.Copy(Common.PathAndFileDatabase, Common.PathExport);
                // log of insulin correction parameters 
                string logOfParameters = Path.Combine(Common.PathConfigurationData,
                    "Log of the Insulin Correction Parameters.txt");
                File.Copy(logOfParameters, Common.PathExport);
                // log of errors 
                File.Copy(Common.LogOfProgram.ErrorsFile, Common.PathExport);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("ExportProgramsFiles. ", ex); 
                return false;
            }
        }
    }
}
