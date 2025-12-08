using gamon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    public class BL_ImportData
    {
        DataLayer dl = Common.Database;

        public async Task<string> ImportDataFromFreeStyleLibre(string csvFilePath)
        {
            try
            {
                General.LogOfProgram?.Debug($"Processing CSV file: {csvFilePath}");

                // Read all lines from the CSV file
                string[] lines = await File.ReadAllLinesAsync(csvFilePath);

                if (lines.Length == 0)
                {
                    throw new InvalidDataException("CSV file contains no data");
                }

                General.LogOfProgram?.Debug($"CSV file contains {lines.Length} lines");

                // make hash tables of times of existing records to rapidly know what to skip

                // read all sensor GlucoseRecords from the database (SensorsRecords table)
                List<GlucoseRecord> existingSensorRecords = dl.GetSensorsRecords();
                // Create HashSet for O(1) lookup of existing sensor timestamps
                // Filter out null timestamps and convert to non-nullable DateTime
                HashSet<DateTime> existingSensorTimes = new HashSet<DateTime>(
                    existingSensorRecords
                        .Where(r => r.EventTime.DateTime.HasValue)
                        .Select(r => r.EventTime.DateTime.Value)
                );

                // read all Meal records from the database
                List<Meal> existingMeals = dl.GetMeals(null, null);
                // Create HashSet for O(1) lookup of existing meal timestamps
                HashSet<DateTime> existingMealTimes = new HashSet<DateTime>(
                    existingMeals
                        .Where(m => m.EventTime.DateTime.HasValue)
                        .Select(m => m.EventTime.DateTime.Value)
                );

                // read all Injections records from the database
                // Get all injections (use wide date range) and build hashset of timestamps
                List<Injection> existingInjections = dl.GetInjections(DateTime.MinValue, DateTime.MaxValue);
                HashSet<DateTime> existingInjectionTimes = new HashSet<DateTime>(
                    existingInjections
                        .Where(i => i.EventTime.DateTime.HasValue)
                        .Select(i => i.EventTime.DateTime.Value)
                );

                // read all Events records from the database
                List<Event> existingEvents = dl.GetEvents(null, null);
                HashSet<DateTime> existingEventTimes = new HashSet<DateTime>(
                    existingEvents
                        .Where(e => e.EventTime.DateTime.HasValue)
                        .Select(e => e.EventTime.DateTime.Value)
                );

                General.LogOfProgram?.Event($"CSV file processed successfully. Lines read: {lines.Length}, " +
                    $"Existing sensor records: {existingSensorTimes.Count}, " +
                    $"Existing meals: {existingMealTimes.Count}, " +
                    $"Existing injections: {existingInjectionTimes.Count}, " +
                    $"Existing events: {existingEventTimes.Count}");

                // TODO: Implement actual data import logic
                // - Parse CSV lines according to FreeStyle Libre format
                // - Create GlucoseRecord objects
                // - Validate data
                // - Import into database
                // - Show import summary

                List<Event> importedEvents = new();
                List<GlucoseRecord> importedGlucose = new();
                List<Injection> importedInjections = new();
                List<Meal> importedMeals = new();
                int lineNumber = 0;
                int successCount = 0;
                int errorCount = 0;
                int skippedCount = 0;
                int noInformationNodes = 0;
                
                foreach (string row in lines)
                {
                    lineNumber++;                    
                    try
                    {
                        // Skip header lines (first two lines are headers in FreeStyle Libre file)
                        if (lineNumber <= 2)
                        {
                            continue;
                        }
                        
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(row))
                            continue;
                        
                        // Split CSV row by comma
                        string[] fields = row.Split(',');
                        
                        // Validate minimum column count (18 is the max index used: fields[18])
                        if (fields.Length < 19)
                        {
                            General.LogOfProgram?.Debug($"Line {lineNumber}: Insufficient columns ({fields.Length}), skipping");
                            errorCount++;
                            continue;
                        }

                        // Parse and validate timestamp first
                        DateTime? timestamp = Safe.DateTime(fields[2]);
                        if (!timestamp.HasValue || timestamp.Value == General.DateNull)
                        {
                            General.LogOfProgram?.Debug($"Line {lineNumber}: Invalid timestamp in field 2, skipping");
                            errorCount++;
                            continue;
                        }

                        // Parse event type and set appropriate fields
                        string recordType = fields[3].Trim();                    
                        switch (recordType)
                        {
                            // cases for glucose measurements
                            case "0": // glucose automatically read from the sensor
                            case "1": // instant glucose scanned with RFC by user
                                // Skip lines that have already been imported
                                if (existingSensorTimes.Contains(timestamp.Value))
                                {
                                    skippedCount++;
                                    continue;
                                }
                                // Create new GlucoseRecord
                                GlucoseRecord glucose = new GlucoseRecord();
                                glucose.EventTime.DateTime = timestamp.Value;
                                // Map CSV columns to GlucoseRecord properties
                                glucose.Notes = fields[13];

                                // Register device and get its ID (foreign key to Devices table)
                                int? deviceId = SaveDeviceFromFreeStyleFile(fields[1], timestamp.Value, timestamp.Value.AddSeconds(-1));
                                glucose.IdOfDevice = deviceId; // Store device ID (foreign key)
                                glucose.IdDeviceModel = (int)Common.DevicesModel.AbbotFreestyleLibre;
                                glucose.GlucoseString = fields[2];
                                if (recordType == "1")
                                {
                                    glucose.GlucoseValue.Double = Safe.Double(fields[5]);
                                    glucose.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.SensorScanValue;
                                }
                                else
                                {
                                    glucose.GlucoseValue.Double = Safe.Double(fields[4]);
                                    glucose.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.SensorIntermediateValue;
                                }
                                // FreeStyle Libre is an under-skin sensor
                                glucose.IdTypeOfDevice = ((int)Common.TypeOfDevice.UnderSkinSensor).ToString();
                                // Add to import list
                                importedGlucose.Add(glucose);
                                break;

                            // TODO: find out correct glucose type code for reactive strip
                            //case "X": // blood glucose from reactive strip
                            //   glucose.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.GlucoseReactiveStripValue;
                            //   glucose.GlucoseValue.Double = 0;
                            //   // value of blood glucose from reactive strip will be in field index 14
                            //   glucose.GlucoseValue.Double = Safe.Double(fields[14]); 
                            //   glucose.IdModelOfMeasurementSystem = "";
                            //   glucose.IdTypeOfDevice = "";
                            //   // accuracy of blood glucose from reactive strip is not provided
                            //   // assumed 100% accuracy for blood glucose from strip
                            //   glucose.BloodGlucoseAccuracy = 100.0; 
                            //   break;
                            
                            case "4": // insulin bolus
                                      // bolus is recorded in Injections
                                
                                // Skip lines that have already been imported
                                if (existingInjectionTimes.Contains(timestamp.Value))
                                {
                                    skippedCount++;
                                    continue;
                                }
                                Injection injection = new();
                                injection.EventTime.DateTime = timestamp.Value;
                                injection.Notes = fields[13];
                                // insulin related fields
                                if (!string.IsNullOrEmpty(fields[7]))
                                {
                                    // bolus of short action insulin
                                    injection.InsulinValue.Double = Safe.Double(fields[7]);
                                    injection.IdTypeOfInsulinAction= (int)Common.TypeOfInsulinAction.Short;
                                    injection.InsulinString = fields[6];
                                }
                                else
                                {
                                    // bolus of long action insulin
                                    injection.InsulinValue.Double = Safe.Double(fields[12]);
                                    injection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Long;
                                    injection.InsulinString = fields[11];
                                }
                                // TODO: Determine correct column index for InsulinDrugName
                                // injection.InsulinDrugName = fields[XXXX];
                                //injection.InsulinInjectionType = (Common.InsulinDrug)Safe.Int(fields[13]);
                                // FreeStyle Libre does not provide accuracy for insulin boluses
                                // accuracy of the bolus insulin value is assumed to be 100% 
                                //injection.InsulinBolusAccuracy = 100.0;
                                // Add to import list
                                importedInjections.Add(injection);
                                break;
                                
                            case "5": // carbohydrates of food eaten at meal
                                      // carbohydrates are stored in Meals
                                
                                // Skip lines that have already been imported
                                if (existingMealTimes.Contains(timestamp.Value))
                                {
                                    skippedCount++;
                                    continue;
                                }
                                Meal meal = new();
                                meal.EventTime.DateTime = timestamp.Value;
                                meal.Notes = fields[13];
                                // meal related fields
                                // FreeStyle Libre does not provide accuracy for carbs, set to default 50% (almost sufficient)
                                meal.AccuracyOfChoEstimate.Double = 50.0;
                                // type of meal is not provided in FreeStyle Libre file
                                meal.IdTypeOfMeal = Common.TypeOfMeal.NotSet;
                                // currently ignoring fields[8] "carbohydrates string"
                                // because it is not in the properties of Meal
                                // (if it will be used later, will be mapped here)
                                //meal.CarbohydratesString = fields[8];
                                meal.CarbohydratesGrams.Double = Safe.Double(fields[9]);
                                // the field "carbohydrates (portions)" (index 10) not used in my data
                                // nor understood by myself, so we are ignoring it for now

                                // Add to import list
                                importedMeals.Add(meal);
                                break;
                                
                            case "6": // TODO understand what '6' means in the file
                                      // for now treat as just notes
                                      // (some of 6 type records have notes only, others have nothing)
                                      // Notes property already set above
                                      // if the note isn't present we null the glucose
                                      // because the other data is never present in csv file

                                // Skip lines that have already been imported
                                if (existingEventTimes.Contains(timestamp.Value))
                                {
                                    skippedCount++;
                                    continue;
                                }
                                Event eventRecord = new();
                                eventRecord.EventTime.DateTime = timestamp.Value;
                                if (string.IsNullOrWhiteSpace(eventRecord.Notes))
                                {
                                    // no recod added, it doesn't have information
                                    noInformationNodes++;
                                }
                                else
                                {
                                    // Add to import list
                                    eventRecord.Notes = fields[13];
                                    importedEvents.Add(eventRecord);
                                    successCount++;
                                }
                                break;

                            default: // non recognized type
                                // do nothing
                                break;
                        }
                        // we don't save IdDocumentType because I feel it is unuseful for this program
                        // I also deleted it from class GlucoseRecord
                        // glucose.IdDocumentType.Int = Safe.Int(fields[8]);

                        // the next properties do not exist in the FreeStyle file
                        //glucose.AccessoryIndex
                        //glucose.BodyWeight
                        //glucose.BloodPressure
                        //glucose.PhysicalActivity
                        //glucose.Photo
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error($"Error parsing line {lineNumber}: {row}", ex);
                        errorCount++;
                    }
                }
                string summaryString = $"CSV import completed. Success: {successCount}" +
                    "\n" + $" Skipped (already imported): {skippedCount}" +
                    "\n" + $" No info nodes (skipped); {noInformationNodes}" +
                    "\n" + $" Errors: {errorCount}";
                General.LogOfProgram?.Event(summaryString);

                // Save imported records to database tables
                if (importedGlucose.Count > 0)
                {
                    dl.InsertSensorMeasurements(importedGlucose);
                }
                
                if (importedInjections.Count > 0)
                {
                    dl.InsertInjections(importedInjections);
                }

                if (importedMeals.Count > 0)
                {
                    // importedMeals were filtered against existingMealTimes, so use insert-only bulk
                    dl.InsertMeals(importedMeals);
                }
                
                if (importedEvents.Count > 0)
                {
                    // importedEvents were filtered against existingEventTimes, so use insert-only bulk
                    dl.InsertEvents(importedEvents);
                }
                return summaryString;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("ProcessSensorDataCsv", ex);
                throw;
            }
        }

        private int? SaveDeviceFromFreeStyleFile(string FreeStyleDeviceId, 
            DateTime ThisTime, DateTime PreviousTime)
        {
            // look if device already exists
            int DeviceModel = (int)Common.DevicesModel.AbbotFreestyle; // Code of FreeStyle device in DeviceModel table
            Device? device = dl.GetDeviceBySerialNumber(DeviceModel, FreeStyleDeviceId);
            int? keyOfDevice = device?.IdDevice;
            // if the device has not been registered till now, we do it
            if (device == null)
            {
                Device newDevice = new();
                newDevice.IdDeviceModel = (int)Common.DevicesModel.AbbotFreestyle;
                newDevice.PhysicalCode = FreeStyleDeviceId;
                newDevice.StartTime = ThisTime;
                keyOfDevice = dl.SaveDevice(newDevice);
                if (keyOfDevice.HasValue && keyOfDevice.Value > 0)
                {
                    // find the previous device (if exists)
                    Device oldDevice = dl.GetDeviceById(keyOfDevice.Value -1);
                    if (oldDevice != null)
                    {
                        oldDevice.FinishTime = PreviousTime;
                        dl.SaveDevice(oldDevice);
                    }
                }
            }
            return keyOfDevice;
        }
    }
}

 