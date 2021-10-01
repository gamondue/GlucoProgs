using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal abstract partial class DataLayer
    {
        internal List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialInstant, 
            DateTime? FinalInstant)
        {
            // !!!! currently InitialInstant, DateTime FinalInstant are simply IGNORED 
            List<GlucoseRecord> list = new List<GlucoseRecord>(); 
            if (File.Exists(persistentGlucoseMeasurements))
                try
                {
                    string[,] f = TextFile.FileToMatrix(persistentGlucoseMeasurements, '\t');
                    for (int i = 0; i < f.GetLength(0); i++)
                    {
                        GlucoseRecord rec = new GlucoseRecord(); 
                        rec.GlucoseValue = double.Parse(f[i, 0]);
                        rec.Timestamp = DateTime.Parse(f[i, 1]);
                        rec.GlucoseString = f[i, 2];
                        //rec.GlucoseAccuracy = f[i, 3]; // convert the enum to string 
                        rec.DeviceType = f[i, 4];
                        rec.DeviceType = f[i, 5];
                        rec.DeviceId = f[i, 6];
                        rec.Notes = f[i, 7];

                        list.Add(rec); 
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
            return list; 
        }
        internal void SaveGlucoseMeasurements(List<GlucoseRecord> List)
        {
            try
            {
                string file = "";

                foreach (GlucoseRecord rec in List)
                {
                    file += rec.GlucoseValue + "\t";
                    file += rec.Timestamp + "\t";
                    file += rec.GlucoseString + "\t";
                    file += rec.DeviceType + "\t";
                    file += rec.DeviceType + "\t";
                    file += rec.DeviceId + "\t";
                    file += rec.Notes + "\t";
                    //file += rec.GlucoseAccuracy + "\t"; // convert to string the enum
                    file += "\n";
                }
                TextFile.StringToFile(persistentGlucoseMeasurements, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            if (File.Exists(persistentGlucoseMeasurements))
                try
                {
                    string[,] f = TextFile.FileToMatrix(persistentGlucoseMeasurements, '\t');
                    int indexLast = f.GetLength(0) - 1;
                    int indexLastM1 = indexLast - 1; 

                    GlucoseRecord recLast = new GlucoseRecord(); 
                    GlucoseRecord recBeforeLast = new GlucoseRecord(); 

                    recLast.GlucoseValue = double.Parse(f[indexLast, 0]);
                    recBeforeLast.GlucoseValue = double.Parse(f[indexLastM1, 0]);
                    recLast.Timestamp = DateTime.Parse(f[indexLast, 1]);
                    recBeforeLast.Timestamp = DateTime.Parse(f[indexLastM1, 1]);
                    recLast.GlucoseString = f[indexLast, 2];
                    recBeforeLast.GlucoseString = f[indexLastM1, 2];
                    //rec.GlucoseAccuracy = f[indexLast, 3]; // convert the enum to string 
                    //recBeforeLast.GlucoseAccuracy = f[indexLastM1, 3]; // convert the enum to string 
                    recLast.DeviceType = f[indexLast, 4];
                    recBeforeLast.DeviceType = f[indexLastM1, 4];
                    recLast.DeviceType = f[indexLast, 5];
                    recBeforeLast.DeviceType = f[indexLastM1, 5];
                    recLast.DeviceId = f[indexLast, 6];
                    recBeforeLast.DeviceId = f[indexLastM1, 6];
                    recLast.Notes = f[indexLast, 7];
                    recBeforeLast.Notes = f[indexLastM1, 7];

                    list.Add(recLast);
                    list.Add(recBeforeLast);
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
            return list;
        }
    }
}
