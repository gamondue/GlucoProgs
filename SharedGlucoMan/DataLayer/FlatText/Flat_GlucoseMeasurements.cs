using SharedData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public partial class DL_FlatText : DataLayer
    {
        public  override List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialInstant, 
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
                        rec.IdGlucoseRecord = int.Parse(f[i, 0]);
                        rec.GlucoseValue = double.Parse(f[i, 1]);
                        rec.Timestamp = DateTime.Parse(f[i, 2]);
                        rec.GlucoseString = f[i, 3];
                        rec.IdDeviceType = f[i, 4];
                        rec.IdDevice = f[i, 5];
                        rec.Notes = f[i, 6];
                        //rec.GlucoseAccuracy = f[i, 3]; // convert the enum to string 

                        list.Add(rec); 
                    }
                }
                catch (Exception ex)
                {
                    Common.LogOfProgram.Error("DL_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
                }
            return list; 
        }
        public  override void SaveGlucoseMeasurements(List<GlucoseRecord> List)
        {
            try
            {
                //string file = "";
                //int? nextIndex = FindNextIndex(List); 
                //foreach (GlucoseRecord rec in List)
                //{
                //    if (rec.IdGlucoseRecord == null || rec.IdGlucoseRecord == 0)
                //    {
                //        rec.IdGlucoseRecord = nextIndex++; 
                //    }
                //    file += rec.IdGlucoseRecord + "\t";
                //    file += rec.GlucoseValue + "\t";
                //    file += rec.Timestamp + "\t";
                //    file += rec.GlucoseString + "\t";
                //    file += rec.IdDeviceType + "\t";
                //    file += rec.IdDevice + "\t";
                //    file += rec.Notes + "\t";
                //    //file += rec.GlucoseAccuracy + "\t"; // convert to string the enum
                //    file += "\n";
                //}
                ////TextFile.StringToFile(persistentGlucoseMeasurements, file, false);
                //TextFile.StringToFileAsync(persistentGlucoseMeasurements, file);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("DL_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
            }
        }
        //public  override int FindNextIndex(List<GlucoseRecord> List)
        public  override int GetNextPrimaryKey()
        {
            // !! We were passing a List to this method. If we ever re-establish the flat text
            // !! DataLayer we wuold have to solve the problem of this List passing

            //// find next Id from list
            //int? maxId = 0;
            //foreach (GlucoseRecord rec in List)
            //{
            //    if (rec.IdGlucoseRecord != null && rec.IdGlucoseRecord > maxId)
            //    {
            //        maxId = rec.IdGlucoseRecord;
            //    }
            //}
            //return (int)(maxId + 1);
            return 0; //!!
        }
        public  override long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement)
        {
            // save one single record using sequential access..
            try
            {
                List<GlucoseRecord> List = ReadGlucoseMeasurements(null, null);
                if (GlucoseMeasurement.IdGlucoseRecord == null || GlucoseMeasurement.IdGlucoseRecord == 0)
                {
                    //GlucoseMeasurement.IdGlucoseRecord = FindNextIndex(List);
                    GlucoseMeasurement.IdGlucoseRecord = GetNextPrimaryKey();
                    // append new record to the file 
                    string addedRecord = GlucoseMeasurement.IdGlucoseRecord + "\t";
                    addedRecord += GlucoseMeasurement.GlucoseValue + "\t";
                    addedRecord += GlucoseMeasurement.Timestamp + "\t";
                    addedRecord += GlucoseMeasurement.GlucoseString + "\t";
                    addedRecord += GlucoseMeasurement.IdDeviceType + "\t";
                    addedRecord += GlucoseMeasurement.IdDevice + "\t";
                    addedRecord += GlucoseMeasurement.Notes + "\t";
                    //file += rec.GlucoseAccuracy + "\t"; // convert to string the enum
                    addedRecord += "\n";
                    TextFile.StringToFile(persistentGlucoseMeasurements, addedRecord, true);
                    return GlucoseMeasurement.IdGlucoseRecord;
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    string fileContent = "";
                    foreach (GlucoseRecord rec in List)
                    {
                        if (rec.IdGlucoseRecord != GlucoseMeasurement.IdGlucoseRecord)
                        {
                            fileContent += rec.IdGlucoseRecord + "\t";
                            fileContent += rec.GlucoseValue + "\t";
                            fileContent += rec.Timestamp + "\t";
                            fileContent += rec.GlucoseString + "\t";
                            fileContent += rec.IdDeviceType + "\t";
                            fileContent += rec.IdDevice + "\t";
                            fileContent += rec.Notes + "\t";
                            //file += rec.GlucoseAccuracy + "\t"; // convert to string the enum
                            fileContent += "\n";
                        }
                        else
                        {
                            fileContent += GlucoseMeasurement.IdGlucoseRecord + "\t";
                            fileContent += GlucoseMeasurement.GlucoseValue + "\t";
                            fileContent += GlucoseMeasurement.Timestamp + "\t";
                            fileContent += GlucoseMeasurement.GlucoseString + "\t";
                            fileContent += GlucoseMeasurement.IdDeviceType + "\t";
                            fileContent += GlucoseMeasurement.IdDevice + "\t";
                            fileContent += GlucoseMeasurement.Notes + "\t";
                            //file += rec.GlucoseAccuracy + "\t"; // convert to string the enum
                            fileContent += "\n";
                        }
                    }
                    TextFile.StringToFileAsync(persistentGlucoseMeasurements, fileContent);
                    return GlucoseMeasurement.IdGlucoseRecord;
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("DL_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
                return GlucoseMeasurement.IdGlucoseRecord;
            }
        }
        public  override List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            if (File.Exists(persistentGlucoseMeasurements))
                try
                {
                    GlucoseRecord recLast = new GlucoseRecord(); 
                    GlucoseRecord recBeforeLast = new GlucoseRecord();

                    // open file
                    using (FileStream fsIn = File.OpenRead(persistentGlucoseMeasurements))
                    {
                        using (StreamReader sr = new StreamReader(fsIn))
                        {
                            string line = sr.ReadLine();
                            string[] fields = line.Split('\t');
                            recLast.IdGlucoseRecord = int.Parse(fields[0]);
                            recLast.GlucoseValue = double.Parse(fields[1]);
                            recLast.Timestamp = Safe.DateTime(fields[2]);
                            recLast.GlucoseString = fields[3];
                            recLast.IdDeviceType = fields[4];
                            recLast.IdDevice = fields[5];
                            recLast.Notes = fields[6];
                            line = sr.ReadLine();
                            fields = line.Split('\t');
                            recBeforeLast.IdGlucoseRecord = int.Parse(fields[0]);
                            recBeforeLast.GlucoseValue = double.Parse(fields[1]);
                            recBeforeLast.Timestamp = Safe.DateTime(fields[2]);
                            recBeforeLast.GlucoseString = fields[3];
                            recBeforeLast.IdDeviceType = fields[4];
                            recBeforeLast.IdDevice = fields[5];
                            recBeforeLast.Notes = fields[6];
                        }
                    }
                    list.Add(recLast);
                    list.Add(recBeforeLast);
                }
                catch (Exception ex)
                {
                    Common.LogOfProgram.Error("DL_GlucoseMeasurement | GetLastTwoGlucoseMeasurements", ex);
                }
            return list;
        }
    }
}
