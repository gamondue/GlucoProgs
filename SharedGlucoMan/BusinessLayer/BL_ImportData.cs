using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    public class BL_ImportData
    {
        DataLayer dl = Common.Database;

        public async Task<List<GlucoseRecord>> ImportDataFromFreeStyleLibre(string csvFilePath)
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

                // TODO: Parse the CSV data and import glucose measurements
                // The actual parsing logic depends on the CSV format from the sensor

                // For now, show a preview of the data
                int previewLines = Math.Min(10, lines.Length);
                string preview = string.Join("\n", lines.Take(previewLines));

                // read all the GlucoseRecords from the database,
                // by avoiding the passage of limit dates
                List<GlucoseRecord> existingRecords = dl.GetGlucoseRecords();
                
                // Create HashSet for O(1) lookup of existing timestamps
                // Filter out null timestamps and convert to non-nullable DateTime
                HashSet<DateTime> existingTimestamps = new HashSet<DateTime>(
                    existingRecords
                        .Where(r => r.Timestamp.DateTime.HasValue)
                        .Select(r => r.Timestamp.DateTime.Value)
                );

                General.LogOfProgram?.Event($"CSV file processed successfully. Lines read: {lines.Length}, Existing records: {existingTimestamps.Count}");

                // TODO: Implement actual data import logic
                // - Parse CSV lines according to FreeStyle Libre format
                // - Create GlucoseRecord objects
                // - Validate data
                // - Import into database
                // - Show import summary
                
                List<GlucoseRecord> importedRecords = new List<GlucoseRecord>();
                int lineNumber = 0;
                int successCount = 0;
                int errorCount = 0;
                int skippedCount = 0;
                
                foreach (string row in lines)
                {
                    lineNumber++;
                    
                    try
                    {
                        // Skip header lines (assuming first two lines are headers)
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
                        
                        // Create new GlucoseRecord
                        GlucoseRecord record = new GlucoseRecord();
                        
                        // Parse and validate timestamp first
                        var timestamp = Safe.DateTime(fields[2]);
                        if (!timestamp.HasValue || timestamp.Value == General.DateNull)
                        {
                            General.LogOfProgram?.Debug($"Line {lineNumber}: Invalid timestamp in field 2, skipping");
                            errorCount++;
                            continue;
                        }
                        record.Timestamp.DateTime = timestamp.Value;
                        
                        // Skip lines that have already been imported
                        if (record.Timestamp.DateTime.HasValue && existingTimestamps.Contains(record.Timestamp.DateTime.Value))
                        {
                            skippedCount++;
                            continue;
                        }
                        // Map CSV columns to GlucoseRecord properties
                        record.IdDevice = fields[1];
                        //////////record.IdModelOfMeasurementSystem = fields[0];
                        record.GlucoseString = fields[2];
                        record.Notes = fields[13];

                        // Parse record type and set appropriate fields
                        string recordType = fields[3].Trim();                    
                        switch (recordType)
                        {
                            case "0": // glucose automatically read from the sensor
                                record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.SensorIntermediateValue;
                                record.GlucoseValue.Double = Safe.Double(fields[4]);
                                // TODO SET THE NEXT or eliminate the property
                                record.IdDeviceType = "";
                                // FreeStyle Libre is an under-skin sensor
                                record.TypeOfGlucoseMeasurementDevice = Common.TypeOfGlucoseMeasurementDevice.UnderSkinSensor;
                                break;
                                
                            case "1": // instant glucose scanned with RFC by user
                                record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.SensorScanValue;
                                record.GlucoseValue.Double = Safe.Double(fields[5]);
                                // TODO SET THE NEXT or eliminate the property
                                record.IdDeviceType = "";
                                // FreeStyle Libre is an under-skin sensor
                                record.TypeOfGlucoseMeasurementDevice = Common.TypeOfGlucoseMeasurementDevice.UnderSkinSensor;
                                break;

                            // TODO: find out correct record type code for reactive strip
                            //case "X": // blood glucose from reactive strip
                            //   record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.GlucoseReactiveStripValue;
                            //   record.GlucoseValue.Double = 0;
                            //   // value of blood glucose from reactive strip will be in field index 14
                            //   record.GlucoseValue.Double = Safe.Double(fields[14]); 
                            //   record.IdModelOfMeasurementSystem = "";
                            //   record.IdDeviceType = "";
                            //   // accuracy of blood glucose from reactive strip is not provided
                            //   // assumed 100% accuracy for blood glucose from strip
                            //   record.BloodGlucoseAccuracy = 100.0; 
                            //   break;                                
                            case "4": // insulin bolus
                                record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.NotSet;
                                record.GlucoseValue.Double = 0;
                                record.IdModelOfMeasurementSystem = ((int)Common.ModelOfMeasurementSystem.NotApplicable).ToString();
                                record.IdDeviceType = fields[1];
                                
                                // insulin related fields
                                if (!string.IsNullOrEmpty(fields[7]))
                                {
                                    // bolus of short action insulin
                                    record.InsulinValue = Safe.Double(fields[7]);
                                    record.InsulinAction = Common.TypeOfInsulinAction.Short;
                                    record.InsulinString = fields[6];
                                }
                                else
                                {
                                    // bolus of long action insulin
                                    record.InsulinValue = Safe.Double(fields[12]);
                                    record.InsulinAction = Common.TypeOfInsulinAction.Long;
                                    record.InsulinString = fields[11];
                                }
                                
                                // TODO: Determine correct column index for InsulinDrugName
                                // record.InsulinDrugName = fields[XXXX];
                                record.InsulinInjectionType = (Common.InsulinDrug)Safe.Int(fields[13]);
                                // FreeStyle Libre does not provide accuracy for insulin boluses
                                // accuracy of the bolus insulin value is assumed to be 100% 
                                record.InsulinBolusAccuracy = 100.0;
                                break;
                                
                            case "5": // carbohydrates of food eaten at meal
                                record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.NotApplicable;
                                record.GlucoseValue.Double = 0;
                                record.IdModelOfMeasurementSystem = fields[0];
                                record.IdDeviceType = fields[1];
                                // food related fields
                                // FreeStyle Libre does not provide accuracy for carbs, set to default 50% (almost sufficient)
                                record.FoodCarbohydratesAccuracy = 50.0;
                                //
                                record.TypeOfMeal = Common.TypeOfMeal.NotSet;
                                record.CarbohydratesString = fields[8];
                                record.CarbohydratesValue_grams = Safe.Double(fields[9]);
                                // the field "carbohydrates (portions)" (index 10) not used in my data
                                // nor understood by myself, so ignoring it for now

                                // the following field maybe is useful, possibly will be eliminated later
                                record.MealFoodString = "";
                                
                                break;
                                
                            case "6": // TODO understand what '6' means in the file
                                      // for now treat as just notes
                                      // (some of 6 type records have notes only, others have nothing)
                                      // Notes property already set above
                                // if the note isn't present we null the record
                                // because the other data is never present in csv file
                                if (string.IsNullOrWhiteSpace(record.Notes))
                                    record = null;
                                else
                                {
                                    record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.NotApplicable;
                                    record.GlucoseValue.Double = 0;
                                    record.IdModelOfMeasurementSystem = "";
                                    record.IdDeviceType = "";
                                }
                                break;

                            default: // non recognized type
                                // we null the result because this record is wrong
                                record = null;
                                //record.TypeOfGlucoseMeasurement = Common.TypeOfGlucoseMeasurement.NotApplicable;
                                //record.GlucoseValue.Double = 0;
                                //record.IdModelOfMeasurementSystem = "";
                                //record.IdDeviceType = "";
                                break;
                        }
                        // we don't save IdDocumentType becasue I feel it is unuseful 
                        // record.IdDocumentType.Int = Safe.Int(fields[8]);

                        // the next properties do not exist in the FreeStyle file
                        //record.AccessoryIndex
                        //record.BodyWeight
                        //record.BloodPressure
                        //record.PhysicalActivity
                        //record.Photo

                        // Add to import list
                        if (record != null)
                            importedRecords.Add(record);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error($"Error parsing line {lineNumber}: {row}", ex);
                        errorCount++;
                    }
                }
                
                General.LogOfProgram?.Event($"CSV import completed. Success: {successCount}, Skipped (already imported): {skippedCount}, Errors: {errorCount}");

                // Save imported records to database
                if (importedRecords.Count > 0)
                {
                    dl.SaveGlucoseMeasurements(importedRecords);
                }
                return importedRecords;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("ProcessSensorDataCsv", ex);
                throw;
            }
        }
    }
}

