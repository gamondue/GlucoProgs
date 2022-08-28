using gamon;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiabetesRecords
{
    public class BusinessLayer
    {
        DataLayer dl;
        TimeSpan timeToBeRecent = new TimeSpan(0, 20, 0);

        internal static string DatabaseFileName = @"DiabetesRecords.Sqlite";
        internal static string AppDataDirectoryPath = FileSystem.AppDataDirectory;
        internal static string PathAndFileDatabase = Path.Combine(AppDataDirectoryPath, DatabaseFileName);
        public static string PathExternalPublic;
        internal static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        internal static string PathLogs = Path.Combine(AppDataDirectoryPath, "logs");
        //internal static string myDocPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        //string CommonApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        //string LocalApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

        internal void MakeFoldersAndFiles()
        {
            // generation of folders. Since File.Create() doesn't work if the folder
            // doesn't exist, the following statements should be executed before
            // any code that creates or uses files is run 
            General.MakeFolderIfDontExist(PathLogs);
            General.MakeFolderIfDontExist(PathUser);

            if (!System.IO.File.Exists(PathAndFileDatabase))
            {
                // since the file doesn't exist yet, we create the database:
                // !!!! TODO !!!! remove bypass of bl 
                dl.CreateNewDatabase(PathAndFileDatabase);
            }

            General.Log = new Logger(PathLogs, true,
                @"DiabetesRecords_Log.txt",
                @"DiabetesRecords_Errors.txt",
                @"DiabetesRecords_Debug.txt",
                @"DiabetesRecords_Prompts.txt",
                @"DiabetesRecords_Data.txt");
        }
        public enum TypeOfMeal
        {
            NotSet = 0,
            Breakfast = 10,
            Lunch = 20,
            Dinner = 30,
            Snack = 40,
            Other = 90
        }
        public enum TypeOfInsulinSpeed
        {
            NotSet = 0,
            QuickAction = 10,
            SlowAction = 20
        }
        internal BusinessLayer()
        {
            dl = new DataLayer(PathAndFileDatabase);
        }
        internal List<DiabetesRecord> GetDiabetesRecords(DateTime DateFrom, DateTime DateTo)
        {
            return dl.GetDiabetesRecords(DateFrom, DateTo); 
        }
        internal bool IsLastRecordRecent()
        {
            DiabetesRecord lr = dl.GetLastDiabetesRecord();
            DateTime? before = (DateTime?)lr.Timestamp;
            if (before != null)
            {
                TimeSpan timepassed = DateTime.Now.Subtract((DateTime)before); 
                return timepassed < timeToBeRecent;
            }
            else
                return false;
        }
        internal bool ShouldUpdateLastRecord(DiabetesRecord FutureRecord)
        {
            DiabetesRecord lr = dl.GetLastDiabetesRecord();
            // the Id of the last record is copied into the new 
            FutureRecord.IdDiabetesRecord = lr.IdDiabetesRecord; 
            // if the types of insulin done are different, we don't make a new record 
            if (FutureRecord.IdTypeOfInsulinSpeed != lr.IdTypeOfInsulinSpeed)
                return false;
            // if the last record is recent we will update the previous, without  
            // creating another one 
            DateTime? before = (DateTime?)lr.Timestamp;
            if (before != null)
                return DateTime.Now.Subtract((DateTime)before) < timeToBeRecent;
            else
                return false;
        }
        internal void InsertOrUpdate(DiabetesRecord Record)
        {
            if (!ShouldUpdateLastRecord(Record))
                Record.IdDiabetesRecord= null;
            dl.SaveOneDiabetesRecord(Record);
        }
        internal int? SaveOneDiabetesRecord(DiabetesRecord currentRecord)
        {
            return dl.SaveOneDiabetesRecord(currentRecord);
        }
        internal bool ExportProgramsFiles()
        {
            try
            {
                // this is done here because before PathExternalPublic wasn't defined yet
                General.MakeFolderIfDontExist(PathExternalPublic);

                // copy database file
                string exportedDatabase = Path.Combine(PathExternalPublic, DatabaseFileName);
                File.Copy(PathAndFileDatabase, exportedDatabase, true);

                // copy log of errors 
                string exportedLogOfProgram = Path.Combine(PathExternalPublic,
                    Path.GetFileName(General.Log.ErrorsFile));
                File.Copy(General.Log.ErrorsFile, exportedLogOfProgram, true);

                return true;
            }
            catch (Exception ex)
            {
                General.Log.Error("ExportProgramsFiles. ", ex);
                return false;
            }
        }
        internal void DeleteOneDiabetesRecord(DiabetesRecord currentRecord)
        {
            dl.DeleteOneDiabetesRecord(currentRecord);
        }
        internal int SetTypeOfMealBasedOnTimeNow(DiabetesRecord Record)
        {
            Record.IdTypeOfMeal = (int)TypeOfMeal.NotSet;
            int hourNow = DateTime.Now.Hour;
            if (hourNow >= 6 && hourNow <= 9)
                Record.IdTypeOfMeal = (int)TypeOfMeal.Breakfast;
            else if (hourNow >= 12 && hourNow <= 14)
                Record.IdTypeOfMeal = (int)TypeOfMeal.Lunch;
            else if (hourNow >= 18 && hourNow <= 21)
                Record.IdTypeOfMeal = (int)TypeOfMeal.Dinner;
            else
                Record.IdTypeOfMeal = (int)TypeOfMeal.Snack;
            return (int)Record.IdTypeOfMeal;
        }
        internal void SetTypeOfInsulinSpeedBasedOnTimeNow(DiabetesRecord Record)
        {
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            // during the day: fast insulin,slow during the night
            // day is from 3 h to 21:59 min 
            if (timeOfDay > new TimeSpan(2, 0, 0) && timeOfDay < new TimeSpan(22, 0, 0))
            {
                Record.IdTypeOfInsulinSpeed = (int)TypeOfInsulinSpeed.QuickAction;
            }
            else
            {
                Record.IdTypeOfInsulinSpeed = (int)TypeOfInsulinSpeed.SlowAction;
            }
        }
        internal bool DeleteDatabase()
        {
            return dl.DeleteDatabase(); ;
        }
    }
}
