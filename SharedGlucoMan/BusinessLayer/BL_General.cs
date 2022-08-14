using GlucoMan;
using System.IO; 
using System;

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
                string exportedDatabase = Path.Combine(Common.PathExport, Common.DatabaseFileName);
                File.Copy(Common.PathAndFileDatabase, exportedDatabase, true);

                // log of errors 
                string exportedLogOfProgram = Path.Combine(Common.PathExport, 
                    Path.GetFileName (Common.LogOfProgram.ErrorsFile)); 
                File.Copy(Common.LogOfProgram.ErrorsFile, exportedLogOfProgram, true);

                // log of insulin correction parameters 
                string exportedLogOfParameters = Path.Combine(Common.PathExport,
                    Common.LogOfParametersFileName); 
                File.Copy(Common.PathAndFileLogOfParameters, exportedLogOfParameters, true);

                return true;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("ExportProgramsFiles. ", ex); 
                return false;
            }
        }
        internal bool ImportFromExternalDatabase(string pathAndFileDatabase, string v)
        {
            // import the foods

            return false; 
        }
    }
}
