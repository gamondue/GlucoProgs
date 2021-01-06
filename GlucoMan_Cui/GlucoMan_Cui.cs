using GlucoMan;
using System;
using System.Collections.Generic;

namespace GlucoMan_Cui
{
    class GlucoMan_Cui
    {
        private static string fileFreeStyleLibre = @"GabrieleMonti_glucose_4-12-2020.csv"; 
        private static string fileFatSecret = @"";
        private static string fileDiabetesM = @"";
        private static string fileOptiumNeo = @"";

        private static string pathFreeStyleLibre = @"C:\Users\gabri\OneDrive\Diabete\Dati\Freestyle Libre\";
        private static string pathFatSecret = @"C:\Users\gabri\OneDrive\Diabete\Dati\fatsecret\";
        private static string pathDiabetesM = @"C:\Users\gabri\OneDrive\Diabete\Dati\DiabetesM\";
        private static string pathOptiumNeo = @"C:\Users\gabri\OneDrive\Diabete\Dati\OptiumNeo\";

        private static string pathOutFile = @"C:\Users\gabri\OneDrive\Diabete\Dati\Elaborazioni\";

        static void Main(string[] args)
        {
            Console.WriteLine("Calcoli per controllo glicemia");

            //string[,] fatData = TextFile.FileToMatrix(fileFatSecret, '\t');
            //string[,] mData = TextFile.FileToMatrix(fileDiabetesM, '\t');
            
            GlycemiaAfterTwoHours(); 
        }
        static private void GlycemiaAfterTwoHours()
        {
            List<GlucoseRecord> grl = FileToListFreestyleLibre();

            // sort the list by date
            grl.Sort((e1, e2) => e1.Timestamp.HasValue ?
            (e2.Timestamp.HasValue ? e1.Timestamp.Value.CompareTo(e2.Timestamp.Value) : -1) : (e2.Timestamp.HasValue ? 1 : 0));

            ShowData(grl); 

            string outData = "Istante\tGlicemia\tTipo Misurazione glicemia\tCarboidrati\tTipo pasto\t" +
                "Insulina iniettata\tTipo di iniezione\tTipo di bolo\tTipo di insulina\n"; 

            //foreach(GlucoseRecord gr in grl)
            //{
            //    outData += gr.Timestamp + "\t" + gr.GlucoseValue_mg_l + "\t" + gr.GlucoseMeasurementType + "\t" +
            //        gr.CarbohydratesValue_grams + "\t" + gr.TypeOfMeal + "\t" + gr.InsulineValue + "\t" +
            //        gr.InsulineInjection + "\t" + gr.InsulineSpeed;
            //    outData += "\n"; 
            //}
            //Console.WriteLine(outData);
            //TextFile.StringToFile(pathOutFile + "Dati glicemia istantanea.tsv", outData, false);

            // find glucose at 2h from meals and calc TDD
            DateTime currentDate = (DateTime)grl[0].Timestamp;
            for (int i = 0; i<grl.Count; i++)
            {
                GlucoseRecord currentRecord = grl[i]; 
                DateTime loopDate = (DateTime)currentRecord.Timestamp; 
                if (loopDate.Day == currentDate.Day)
                {
                    double dayInsuline = 0; 
                    if (currentRecord.CarbohydratesValue_grams != null && currentRecord.CarbohydratesValue_grams != 0)
                    {
                        DateTime mealTime = (DateTime)currentRecord.Timestamp;
                        currentRecord.AccessoryIndex = i; 
                        double bolusInsuline = InsulineTakenInPeriod(grl, currentRecord, 1);
                        if (bolusInsuline != 0)
                        {
                            GlucoseRecord afterTwoHours = GetFirstRecordAfterNHours(grl, currentRecord, 2);
                            ////////GlucoseRecord glucoseNear = GetFirstRecordAfterNHours; 
                            double glucoseDifferenceAfterTwoHours = 
                                (double)currentRecord.GlucoseValue_mg_l - (double)afterTwoHours.GlucoseValue_mg_l;
                        }
                    }
                    if (currentRecord.InsulineValue != null && currentRecord.InsulineValue != 0)
                        dayInsuline += (double)currentRecord.InsulineValue; 
                    currentDate = loopDate; 
                }

            }
        }

        private static GlucoseRecord GetFirstRecordAfterNHours(List<GlucoseRecord> Records, 
            GlucoseRecord CurrentRecord, int NHours)
        {
            int i = CurrentRecord.AccessoryIndex;

            int IndexOfCentralRecord = i; 
            i = IndexOfCentralRecord;
            DateTime after = ((DateTime)CurrentRecord.Timestamp).AddHours(NHours);
            while (i < Records.Count)
            {
                if (Records[i].Timestamp >= after)
                    return Records[i];
                i++;
            }
            return null; 
        }

        private static double InsulineTakenInPeriod(List<GlucoseRecord> Records, 
            GlucoseRecord CentralRecord, int TimeInterval)
        {
            // look for an insuline bolus since Timespan hours before and after CentralTime

            int IndexOfCentralRecord = CentralRecord.AccessoryIndex;
            int i = IndexOfCentralRecord; 
            // before
            DateTime before = ((DateTime)CentralRecord.Timestamp).AddHours(-TimeInterval);
            double totalInsuline = 0; 
            while ((DateTime)Records[i].Timestamp > before)
            {
                if (Records[i].InsulineValue != null)
                    totalInsuline += (double)Records[i].InsulineValue;
                i--; 
            }
            // after
            DateTime after = ((DateTime)CentralRecord.Timestamp).AddHours(TimeInterval);
            i = IndexOfCentralRecord; 
            while ((DateTime)Records[i].Timestamp < after)
            {
                if (Records[i].InsulineValue != null)
                    totalInsuline += (double)Records[i].InsulineValue;
                i++;
            }
            return totalInsuline; 
        }

        private GlucoseRecord GetFirstNearbyGlucoseRecord(List<GlucoseRecord> Records,
            GlucoseRecord CentralRecord, int NHours)
        {
            GlucoseRecord near = new GlucoseRecord();

            int i = CentralRecord.AccessoryIndex; 
            for (; i < Records.Count; i++)
            {
                if (Records[i] == CentralRecord)
                    break;
            }
            
            return near; 
        }

        private TypeOfMeal MealInference (List<GlucoseRecord> Records, GlucoseRecord Record)
        {
            //DateTime Instant = (DateTime)Record.Timestamp;
            //if (Instant.Hour >= 6 && Instant.Hour <= 9.5 && lookForBolus(Records,Record, 2) != null)
            //    return TypeOfMeal.Breakfast;
            //else if (Instant.Hour >= 11.5 && Instant.Hour <= 14.5)
            //    return TypeOfMeal.Lunch;
            //else if (Instant.Hour >= 6.5 && Instant.Hour <= 9)
            //    return TypeOfMeal.Dinner;
            ////else if (Instant.Hour >= 11.5 && Instant.Hour <= 14.5)
            ////    return TypeOfMeal.Breakfast;
            //else
                return TypeOfMeal.NotSet;
        }

        private static void ShowData(List<GlucoseRecord> grl)
        {
            foreach (GlucoseRecord gr in grl)
            {
                Console.WriteLine("{0} Glu: {1} CHO: {2} Insul.: {3}", gr.Timestamp, PrintNullableDouble(gr.GlucoseValue_mg_l),
                    PrintNullableDouble(gr.CarbohydratesValue_grams), PrintNullableDouble(gr.InsulineValue));
            }
        }

        private static string PrintNullableDouble(double? Value)
        {
            if (Value is null)
                return "-----";
            else
                return ((double)Value).ToString("000.0"); 
        }

        private static List<GlucoseRecord> FileToListFreestyleLibre()
        {
            List<GlucoseRecord> lr = new List<GlucoseRecord>(); 
            List<List<string>> libreData = TextFile.FileToListOfLists(pathFreeStyleLibre + fileFreeStyleLibre, ',', '"');
            // reading into class GlucoseRecord
            GlucoseRecord e = null; 
            // the first two lines are prompts to user 
            for (int i = 2; i < libreData.Count; i++)
            {
                e = new GlucoseRecord();
                // keep the old object if the new record has identical date
                e.DeviceType = libreData[i][0];
                e.DeviceId = libreData[i][1];
                e.Timestamp = DateTime.Parse(libreData[i][2]);
                e.DeviceType = libreData[i][3];
                if (libreData[i][4] != "")
                {
                    e.GlucoseValue_mg_l = DoubleFromString(libreData[i][4]);
                    e.GlucoseMeasurementType = TypeOfGlucoseMeasurement.SensorIntermediateValue; 
                }
                if (libreData[i][5] != "")
                {
                    e.GlucoseValue_mg_l = DoubleFromString(libreData[i][5]);
                    e.GlucoseMeasurementType = TypeOfGlucoseMeasurement.SensorScanValue;
                }
                if (libreData[i][6] != "")
                {
                    e.InsulineString = libreData[i][6];
                    e.InsulineSpeed = TypeOfInsulineSpeed.QuickActionInsuline;
                }
                if (libreData[i][7] != "")
                {
                    e.InsulineValue = DoubleFromString(libreData[i][7]); 
                    e.InsulineSpeed= TypeOfInsulineSpeed.QuickActionInsuline;
                }
               
                e.MealFoodString = libreData[i][8];

                e.CarbohydratesValue_grams = DoubleFromString(libreData[i][9]);
                e.CarbohydratesString = libreData[i][10];
                
                if (libreData[i][11] != "")
                {
                    e.InsulineString= libreData[i][11];
                    e.InsulineSpeed = TypeOfInsulineSpeed.SlowActionInsuline; 
                }
                
                if (libreData[i][12] != "")
                {
                    e.InsulineValue = DoubleFromString(libreData[i][12]);
                    e.InsulineSpeed = TypeOfInsulineSpeed.SlowActionInsuline;
                }
                
                e.Notes = libreData[i][13];
                
                if (libreData[i][14] != "")
                {
                    e.GlucoseValue_mg_l = DoubleFromString(libreData[i][14]);
                    e.GlucoseMeasurementType = TypeOfGlucoseMeasurement.Strip; 
                }

                e.Chetons_mmol_l = DoubleFromString(libreData[i][15]);

                if (libreData[i][16] != "")
                {
                    e.InsulineValue = DoubleFromString( libreData[i][16]);
                    e.InsulineInjection = TypeOfInsulineBolus.CarbBolus; 
                }
                
                if (libreData[i][17] != "")
                {
                    e.InsulineValue = DoubleFromString(libreData[i][17]);
                    e.InsulineInjection = TypeOfInsulineBolus.CorrectionBolus;
                }

                if (libreData[i][18] != "")
                {
                    e.InsulineValue = DoubleFromString(libreData[i][18]);
                    e.InsulineInjection = TypeOfInsulineBolus.ExtendedBolus;
                }
                lr.Add(e);
               }
            return lr; 
        }

        private static double? DoubleFromString(string StringValue)
        {
            if (StringValue != null && StringValue != "" && StringValue != " ")
            {
                StringValue = StringValue.Replace(',', '.');
                return double.Parse(StringValue);
            }
            else
                return null; 
        }

        private void AggiungiAFileExcel()
        {
            //    if (DtpMeseODataIniziale.SelectedDate == null)
            //    {
            //        MessageBox.Show("Scegliere il mese");
            //        return;
            //    }
            //    if (!File.Exists(TxtPathDeiFile.Text + TxtFileDati.Text)
            //    {
            //        MessageBox.Show("Il file " + TxtPathDeiFile.Text + TxtFileDati.Text + " non esiste!");
            //        return;
            //    }
            //    if (!File.Exists(TxtPathDeiFile.Text + TxtFileModello.Text)
            //    {
            //        MessageBox.Show("Il file " + TxtPathDeiFile.Text + TxtFileModello.Text + " non esiste!");
            //        return;
            //    }
            //    int mese = DtpMeseODataIniziale.SelectedDate.Value.Month;
            //    int anno = DtpMeseODataIniziale.SelectedDate.Value.Year;

            //    DateTime dataInizio = new DateTime(anno, mese, 1);
            //    DateTime dataFine = dataInizio.AddMonths(1);

            //    string nomeFileDestinazione = dataInizio.ToString("yyyy-MM") + "_Glicemia ed insulina_" + paziente + ".xlsx";

            //    sheetInputDati = new PopulateExcel(TxtPathDeiFile.Text + TxtFileDati.Text, false);

            //    sheetOutputGlicemiaEInsulina = new PopulateExcel(TxtPathDeiFile.Text + TxtFileModello.Text,
            //        TxtPathDeiFile.Text + nomeFileDestinazione, false);
            //    ////sheetOutputCalcoloCarboidrati = new PopulateExcel(TxtPathDeiFile.Text + "Calcolo carboidrati_Test.xlsx",
            //    ////    DtpMeseODataIniziale.SelectedDate.Value.ToString() + "_Calcolo-carboidrati_Gabriele-Monti",
            //    ////    false);
            //    //ApriExcel(TxtPathDeiFile.Text + TxtFileModello.Text, sheetDestinazione);
            //    //ApriExcel(TxtPathDeiFile.Text + TxtFileDati, sheetDati);
            //    int rigaPrimoGiornoDelMeseInFileGlicemiaEInsulina = 4;
            //    int rigaInizioGiornoInFileCalcoloCarboidrati = 5;

            //    int rigaFileGlicemiaEInsulina = rigaPrimoGiornoDelMeseInFileGlicemiaEInsulina;
            //    int rigaDati = 5, colonnaDati = 1;
            //    int colonnaFileGlicemiaEInsulina;

            //    // scansiona tutte le righe del file dati
            //    string contenuto, scritta;
            //    do
            //    {   // legge la prima colonna
            //        contenuto = sheetInputDati.ReadCell(1, rigaDati, 1);
            //        try
            //        {
            //            DateTime istante = Convert.ToDateTime(contenuto);
            //            // guarda se la data non è nulla on non è nel mese che interessa
            //            if (istante != new DateTime(1, 1, 1) && (istante >= dataInizio && istante < dataFine)
            //            {
            //                // stabilisce la riga del foglio di destinazione a partire dalla data del foglio di origine 
            //                rigaFileGlicemiaEInsulina = istante.Day + rigaPrimoGiornoDelMeseInFileGlicemiaEInsulina - 1;

            //                // Meal Insulin (units) letto da colonna 3
            //                int? valore = SafeInt(sheetInputDati.ReadCell(1, rigaDati, 3);
            //                if (valore != null && valore != 0)
            //                {
            //                    colonnaFileGlicemiaEInsulina = 3 + InsulinaColonnaOrario(istante);
            //                    // foglio del file risultato 
            //                    sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
            //                            colonnaFileGlicemiaEInsulina, valore.ToString();
            //                }
            //                // Long Acting Insulin (units) letto da   colonna 4
            //                valore = SafeInt(sheetInputDati.ReadCell(1, rigaDati, 4);
            //                if (valore != null && valore != 0)
            //                {
            //                    colonnaFileGlicemiaEInsulina = 3 + InsulinaColonnaOrario(istante);
            //                    sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
            //                        colonnaFileGlicemiaEInsulina, valore.ToString();
            //                }
            //                // Glucose (mg/dL) colonna 5
            //                valore = SafeInt(sheetInputDati.ReadCell(1, rigaDati, 5);
            //                if (valore != null && valore != 0)
            //                {
            //                    colonnaFileGlicemiaEInsulina = 2 + InsulinaColonnaOrario(istante);
            //                    sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
            //                        colonnaFileGlicemiaEInsulina, valore.ToString();
            //                }
            //                // Note colonna 
            //                scritta = SafeString(sheetInputDati.ReadCell(1, rigaDati, 11);
            //                if (scritta != null && scritta != "")
            //                {
            //                    colonnaFileGlicemiaEInsulina = 13;
            //                    contenuto = sheetOutputGlicemiaEInsulina.ReadCell(1, rigaFileGlicemiaEInsulina, colonnaFileGlicemiaEInsulina);
            //                    if (contenuto == "")
            //                        contenuto = scritta;
            //                    else
            //                        contenuto = contenuto + " | " + scritta;
            //                    sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
            //                        colonnaFileGlicemiaEInsulina, contenuto);
            //                }
            //            }
            //        }
            //        catch
            //        {
            //            // se non era una data non fa niente 
            //        }
            //        rigaDati++;
            //    } while (contenuto != "" && contenuto != null);

            //    sheetOutputGlicemiaEInsulina.ReplaceSquaredParenthesisTag("Paziente", paziente);
            //    sheetOutputGlicemiaEInsulina.ReplaceSquaredParenthesisTag("MeseEAnno", dataInizio.ToString("MMMM yyyy");
            //    sheetOutputGlicemiaEInsulina.ReplaceSquaredParenthesisTag("TipoDiTerapia", "Lispro + Abasaglar");

            //    sheetOutputGlicemiaEInsulina.Save();
            //    sheetInputDati.Close();
            //    sheetOutputGlicemiaEInsulina.Close();
            //    MessageBox.Show("Fatto!");
            //    //sheetDati.Dispose(); 
        }
    }
}
