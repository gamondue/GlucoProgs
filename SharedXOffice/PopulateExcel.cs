using System;
using Excel = NetOffice.ExcelApi;
using NetOffice.ExcelApi.Enums;
using NetOffice.ExcelApi;
using System.Threading;
using SharedData;

namespace Comuni.XOffice
{
    internal class PopulateExcel : AbstractPopulateSpreadsheet
    {
        // per farlo funzionare: prima mettere un riferimento alla type library di Office
        // Riferimenti | click destro | aggiungi riferimento | COM | Libreria oggetti di Microsoft Word:
        // Microsoft Excel XX object Library
        //
        // per usare i comandi COM di Interop assegnare ogni oggetto a variabile del programma C#
        // altrimenti la memoria non viene deallocata alla fine del programma C# ed 
        // il processo relativo al programma Office utilizzato rimane aperto

        Excel.Application xclApp;
        Excel.Workbook xclWorkbook;

        // C# doesn't have optional arguments so we'll need a dummy value
        object oMissing = (object)System.Reflection.Missing.Value;
        object oFalse = (object)false;
        object oTrue = (object)true;

        string templateFile;

        bool isVisible; 

        internal PopulateExcel(string TemplateFile, string ResultFile, bool IsVisible) : base(TemplateFile, ResultFile)
        {
            isVisible = IsVisible;
            ConstructorCode(TemplateFile, ResultFile);
        }

        internal PopulateExcel(string FileToOpenAndModify, bool IsVisible) : base(FileToOpenAndModify)
        {
            isVisible = IsVisible;

            // il costruttore con un solo parametro apre proprio quel file, 
            // su cui eventualmente scriverà

            xclApp = new Excel.Application();
            xclApp.DisplayAlerts = false;

            templateFile = FileToOpenAndModify;

            object readOnly = oFalse;

            xclApp.ScreenUpdating = true;

            try
            {
                xclApp.Visible = isVisible;

                // API doc
                // Workbook Open(
                //    string Filename, object UpdateLinks, object ReadOnly, object Format, object Password,
                //    object WriteResPassword, object IgnoreReadOnlyRecommended, object Origin, object Delimiter, object Editable,
                //    object Notify, object Converter, object AddToMru, object Local, object CorruptLoad
                // )
                // Apre in lettura e scrittura il foglio di Excel
                xclWorkbook = xclApp.Workbooks.Open(templateFile, oMissing, readOnly, oMissing, oMissing,
                              oMissing, oMissing, oMissing, oMissing, oMissing,
                              oMissing, isVisible, oMissing, oMissing, oMissing);

                object fileFormat = XlFileFormat.xlExcel8;

                //return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel|Constructor()";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }

        private string ConstructorCode(string TemplateFile, string ResultFile)
        {
            // il costruttore con due parametri legge da un file e scrive in un altro 
            templateFile = TemplateFile;

            xclApp = new Excel.Application();
            xclApp.DisplayAlerts = false;

            xclApp.Visible = isVisible;

            object saveAsFilename = (object) ResultFile;

            object readOnly = oFalse;

            xclApp.ScreenUpdating = false;

            try 
            {
                // API doc
                // Apre in sola lettura il foglio di Excel nel template
                xclWorkbook = xclApp.Workbooks.Open(templateFile, oMissing,  readOnly,  
                    oMissing,  oMissing, oMissing,   oMissing,  oMissing,  oMissing,  
                    oMissing, oMissing,  isVisible,  oMissing,  oMissing,  oMissing );

                //exlApp.Documents.Add(TemplateFile);
                object fileFormat = XlFileFormat.xlExcel9795;
                // salva con il nome finale 
                // void SaveAs(
                //    object Filename, object FileFormat, object Password, object WriteResPassword, object ReadOnlyRecommended,
                //    object CreateBackup, XlSaveAsAccessMode AccessMode, object ConflictResolution, object AddToMru, object TextCodepage,
                //    object TextVisualLayout, object Local
                // )
                //xclWorkbook.SaveAs(saveAsFilename, oMissing, oMissing, oMissing, oMissing,
                //               oMissing,  XlSaveAsAccessMode.xlExclusive,  oMissing,  oMissing,  oMissing, 
                //               oMissing,  oMissing);
                xclWorkbook.SaveCopyAs(saveAsFilename);
                xclWorkbook.Close();
                // riapre il file appena salvato
                readOnly = (object)oFalse;
                xclWorkbook = xclApp.Workbooks.Open(ResultFile, oMissing, readOnly, oMissing, oMissing,
                              oMissing, oMissing, oMissing, oMissing, oMissing,
                              oMissing, isVisible, oMissing, oMissing, oMissing);
                return "OK";
            }
            catch (Exception ex){
                string err = "PopulateExcel|Constructor():" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }

        internal override string Close()
        {
            try
            {
                xclApp.DisplayAlerts = false;
                if (xclWorkbook != null)
                {
                    // chiude il foglio
                    //xclWorkbook.Close(false, oMissing, oMissing);
                    ((Excel.Workbook)xclWorkbook).Close();
                    //xclWorkbook = null;
                    // libera la memoria in attesa del garbage collector
                    xclWorkbook.Dispose(); 
                    xclWorkbook = null;
                }
                if (xclApp != null)
                {
                    // Chiude Excel
                    // xclApp has to be cast to type _Application so that it will find
                    // the correct Quit method.
                    ((Excel._Application)xclApp).Quit();
					//xclApp = null;
                    // libera la memoria in attesa del garbage collector
                    xclApp.Dispose();
                    xclApp = null;
                }
                Thread.Sleep(200);

                // rilascia gli oggetti liberati lanciando il garbage collector 
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel " +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }

        /// <summary>
        /// Sostituisce il tag fra parentesi quadre passato con la stringa passata, in ogni foglio del file
        /// !! NON RIESCE A SOSTITUIRE I TAG CHE INCLUDONO BLANK !! 
        /// </summary>
        /// <param name="Tag">Stringa senza blank da sostituire</param>
        /// <param name="ReplacingString">Stringa che viene messa nel file</param>
        /// <returns>Prompt di errore, "OK" se tutto bene</returns>

        internal string? ReadCell(int SheetNumber, int RowNumber, int ColumnNumber)
        {
            try
            {
                Excel.Worksheet xclSheet = (Excel.Worksheet)xclWorkbook.Sheets[SheetNumber];
                Excel.Range r1 = (Excel.Range)xclSheet.Cells[RowNumber, ColumnNumber];
                if (r1.Value == null) return null;
                return r1.Value.ToString();
            }
            catch
            {
                return null;
            }
        }


        internal override string ReplaceSquaredParenthesisTag(string Tag, string ReplacingString)
        {
            if (ReplacingString != null)
                ReplacingString = ReplacingString.Trim();
            else
                ReplacingString = ""; 
            object oTag = (object)("[" + Tag + "]");
            object oReplacingString = (object) ReplacingString;
            try
            {
                int fogliPresenti = xclWorkbook.Sheets.Count;
                for (int i = 1; i <= fogliPresenti; i++)
                {   // rimpiazza il tag con la stringa passata
                    Excel.Worksheet xclSheet = (Excel.Worksheet) xclWorkbook.Sheets[i];
                    // API Doc
                    // bool Replace(object What, object Replacement, object LookAt, object SearchOrder, object MatchCase,
                    //    object MatchByte, object SearchFormat, object ReplaceFormat
                    // )
                    // Cerca il tag. 
                    //object cercaTutto = (object)XlLookAt.xlWhole; // Così trova solo le stringhe che matchano TUTTA la cella
                    object cercaTutto = (object)XlLookAt.xlPart; // Così trova solo le stringhe che matchano PARTE della cella
                    // per trovare anche le stringhe parziali, metterci xlWhole
                    object ordineRicerca = (object)XlSearchOrder.xlByRows; 
                    xclSheet.Cells.Replace(
                        oTag, oReplacingString, cercaTutto, ordineRicerca, oFalse,
                         oTrue, oFalse, oFalse); 
                }

                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel|ReplaceSquaredParenthesisTag():" +
                 "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }  
        }

        /// <summary>
        /// Sostituisce il tag fra parentesi quadre passato con la striga passata, e mette la nota passata
        /// nella casella alla destra di quella dove ha trovato il tag, lavora in ogni foglio del file
        /// !! NON RIESCE A SOSTITUIRE I TAG CHE INCLUDONO BLANK !! 
        /// </summary>
        /// <param name="Tag">Stringa senza blank da sostituire</param>
        /// <param name="ReplacingString">Stringa che viene messa nel file</param>
        /// <returns>Prompt di errore, "OK" se tutto bene</returns>
        internal override string ReplaceSquaredParenthesisTagAndNearCell
            (string Tag, string ReplacingString, string Note)
        {
            object oTag = (object)("[" + Tag + "]");
            object oReplacingString = (object)ReplacingString;
            object oNota = Note; 
            try
            {
                int fogliPresenti = xclWorkbook.Sheets.Count;
                for (int i = 1; i <= fogliPresenti; i++)
                {
                    Excel.Worksheet xclSheet = (Excel.Worksheet)xclWorkbook.Sheets[i];

                    object cercaTutti = (object)XlLookAt.xlWhole;
                    object ordineRicerca = (object)XlSearchOrder.xlByRows;

                    Excel.Range r, r1;
                    int col, row;
                    XlSearchDirection d = XlSearchDirection.xlNext;
                    if ((r = (Excel.Range)xclSheet.Cells.Find(oTag,
                            oMissing, oMissing, oMissing, oMissing, d, oMissing, oMissing, oMissing)) != null)
                    { 
                        col = r.Column + 1;
                        row = r.Row;
                        // la successiva rimpiazza tutti i tag nel foglio, per cui dopo non si trovano più, ed il ciclo 
                        // commentato successivo è inutile (oltre che non funzionante..)
                        r.Replace(oTag, oReplacingString, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing); 
                        r1 = (Excel.Range)xclSheet.Cells[row, col];
                        r1.Value = oNota;
                    }
                    // !! Questa implementazione scrive la nota vicino al Sì/No solo per LA PRIMA occorrenza del tag in ogni pagina !!
                    // TODO cercare come fare il rimpiazzo di una singola cella, nella replace precedente, poi aggiustare il loop
                    // che segue: 
                    //while ((r = (Range)xclSheet.Cells.FindNext(oMissing)) != null)
                    //{
                    //    col = r.Column + 1;
                    //    row = r.Row;
                    //    r = (Range)xclSheet.Cells[row, col];
                    //    r.Value = oNota;

                    //    //r.Replace(oTag, oReplacingString, oMissing, oMissing, oMissing, oMissing, oMissing);
                    //    //r = xclSheet.get_Range(row, col + 1);
                    //    //r.Value = oNota;
                    //} 
                }
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel|ReplaceSquaredParenthesisTag():" +
                 "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }

        internal override string ShowWindow()
        {
            try{
                if (xclApp.Application.Visible == false)
                    xclApp.Application.Visible = true;
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel";
                err += "\nMetodo:|ShowWindow() " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }

        internal override string Save()
        {
            try
            {
                // rende attivo il primo foglio (per bellezza)
                ((Excel.Worksheet)xclApp.ActiveWorkbook.Sheets[1]).Select(oMissing); 

                xclWorkbook.Save();
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel|Save():" +
                 "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name; 
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
            return "OK";
        }

        internal override string InsertTextInCell(int SheetNumber, int RowNumber, string ColumnName, 
                string CellText)
        {
            return null; 
        }

        internal override string InsertTextInCell(int SheetNumber, int RowNumber, int ColumnNumber,
                string CellText)
        {   
            //TODO aggiustare
            object foglio = (object)SheetNumber;
            object riga = (object)RowNumber;
            object colonna = (object)ColumnNumber;
            Excel.Worksheet xclSheet;

            try
            {
                //int fogliPresenti = xclWorkbook.Sheets.Count;
                //for (int i = fogliPresenti + 1; i <= SheetNumber; i++)
                //{
                //    // aggiunge un nuovo foglio in fondo a quelli che ci sono 
                //    // API
                //    // object Add(object Before, object After, object Count, object Type
                //    //)
                //    //xclWorkbook.Sheets.Add(oMissing, (object)(i-1), (object)1, oMissing);
                //    //xclSheet = xclWorkbook.Worksheets[xclWorkbook.Worksheets.Add()];
                //    ((Worksheet)xclApp.ActiveWorkbook.Sheets[i-1]).Select(oMissing); // attiva l'ultimo foglio NON FUNZIONA BENE!!
                //    xclWorkbook.Worksheets.Add(); // aggiunge dopo l'ultimo foglio
                //}
                xclSheet = (Excel.Worksheet)xclWorkbook.Sheets[foglio];
                Excel.Range r1 = (Excel.Range)xclSheet.Cells[riga, colonna];
                r1.Value = CellText;

                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel" +
                 "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }
        internal override string InsertIntegerInCell(int SheetNumber, int RowNumber, int ColumnNumber, int value)
        {
            Object foglio = (Object)SheetNumber;
            Object riga = (Object)RowNumber;
            Object colonna = (Object)ColumnNumber;
            Worksheet xclSheet;
            try
            {
                int fogliPresenti = xclWorkbook.Sheets.Count;
                xclSheet = (Worksheet)xclWorkbook.Sheets[foglio];
                Excel.Range r1 = (Excel.Range)xclSheet.Cells[riga, colonna];
                r1.Value = value;

                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateExcel|InsertIntegerInCell():" +
                    "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
            /* EXAMPLE: some kind of numerics */

            //Worksheet workSheet = (Worksheet)xclApp.ActiveSheet;

            //// the given thread culture in all latebinding calls are stored 
            //// in NetOffice.Settings.
            //// you can change the culture. default is en-us.
            //System.Globalization.CultureInfo cultureInfo = NetOffice.Settings.ThreadCulture;
            //string Pattern1 = string.Format("0{0}00",
            //                         cultureInfo.NumberFormat.CurrencyDecimalSeparator);
            //string Pattern2 = string.Format("#{1}##0{0}00",
            //                         cultureInfo.NumberFormat.CurrencyDecimalSeparator,
            //                         cultureInfo.NumberFormat.CurrencyGroupSeparator);

            //workSheet.Range("A1").Value = "Type";
            //workSheet.Range("B1").Value = "Value";
            //workSheet.Range("C1").Value = "Formatted " + Pattern1;
            //workSheet.Range("D1").Value = "Formatted " + Pattern2;

            //int integerValue = 532234;
            //workSheet.Range("A3").Value = "Integer";
            //workSheet.Range("B3").Value = integerValue;
            //workSheet.Range("C3").Value = integerValue;
            //workSheet.Range("C3").NumberFormat = Pattern1;
            //workSheet.Range("D3").Value = integerValue;
            //workSheet.Range("D3").NumberFormat = Pattern2;

            //double doubleValue = 23172.64;
            //workSheet.Range("A4").Value = "double";
            //workSheet.Range("B4").Value = doubleValue;
            //workSheet.Range("C4").Value = doubleValue;
            //workSheet.Range("C4").NumberFormat = Pattern1;
            //workSheet.Range("D4").Value = doubleValue;
            //workSheet.Range("D4").NumberFormat = Pattern2;

            //float floatValue = 84345.9132f;
            //workSheet.Range("A5").Value = "float";
            //workSheet.Range("B5").Value = floatValue;
            //workSheet.Range("C5").Value = floatValue;
            //workSheet.Range("C5").NumberFormat = Pattern1;
            //workSheet.Range("D5").Value = floatValue;
            //workSheet.Range("D5").NumberFormat = Pattern2;

            //Decimal decimalValue = 7251231.313367m;
            //workSheet.Range("A6").Value = "Decimal";
            //workSheet.Range("B6").Value = decimalValue;
            //workSheet.Range("C6").Value = decimalValue;
            //workSheet.Range("C6").NumberFormat = Pattern1;
            //workSheet.Range("D6").Value = decimalValue;
            //workSheet.Range("D6").NumberFormat = Pattern2;

            //workSheet.Range("A9").Value = "DateTime";
            //workSheet.Range("B10").Value = cultureInfo.DateTimeFormat.FullDateTimePattern;
            //workSheet.Range("C10").Value = cultureInfo.DateTimeFormat.LongDatePattern;
            //workSheet.Range("D10").Value = cultureInfo.DateTimeFormat.ShortDatePattern;
            //workSheet.Range("E10").Value = cultureInfo.DateTimeFormat.LongTimePattern;
            //workSheet.Range("F10").Value = cultureInfo.DateTimeFormat.ShortTimePattern;

            //// DateTime
            //DateTime dateTimeValue = DateTime.Now;
            //workSheet.Range("B11").Value = dateTimeValue;
            //workSheet.Range("B11").NumberFormat = cultureInfo.DateTimeFormat.FullDateTimePattern;

            //workSheet.Range("C11").Value = dateTimeValue;
            //workSheet.Range("C11").NumberFormat = cultureInfo.DateTimeFormat.LongDatePattern;

            //workSheet.Range("D11").Value = dateTimeValue;
            //workSheet.Range("D11").NumberFormat = cultureInfo.DateTimeFormat.ShortDatePattern;

            //workSheet.Range("E11").Value = dateTimeValue;
            //workSheet.Range("E11").NumberFormat = cultureInfo.DateTimeFormat.LongTimePattern;

            //workSheet.Range("F11").Value = dateTimeValue;
            //workSheet.Range("F11").NumberFormat = cultureInfo.DateTimeFormat.ShortTimePattern;

            //// string
            //workSheet.Range("A14").Value = "String";
            //workSheet.Range("B14").Value = "This is a sample String";
            //workSheet.Range("B14").NumberFormat = "@";

            //// number as string
            //workSheet.Range("B15").Value = "513";
            //workSheet.Range("B15").NumberFormat = "@";

            //// set colums
            //workSheet.Columns[1].AutoFit();
            //workSheet.Columns[2].AutoFit();
            //workSheet.Columns[3].AutoFit();
            //workSheet.Columns[4].AutoFit();
        }

        internal override string InsertDoubleInCell(int SheetNumber, int RowNumber, int ColumnNumber, double value)
        {
            Object foglio = (Object)SheetNumber;
            Object riga = (Object)RowNumber;
            Object colonna = (Object)ColumnNumber;
            Worksheet xclSheet;
            try
            {
                int fogliPresenti = xclWorkbook.Sheets.Count;
                xclSheet = (Worksheet)xclWorkbook.Sheets[foglio];
                Excel.Range r1 = (Excel.Range)xclSheet.Cells[riga, colonna];
                r1.Value = value;

                return "OK";
            }
            catch (Exception ex)
            {
            	string err = "PopulateExcel|InsertDoubleInCell():";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }
        
        internal override void DeleteRow(int SheetNumber, int Row)
        {
            //TODO 
            try
            {
                //GetTableRowFromTableNumber(TableNumber, Row).Delete();
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                CommonObjectsAndData.LogDelProgramma.Error(err, ex);
            }
        }

        private string CreateGraph()
        {
            //TODO se serve, aggiustare
            string errore = "";

            //Chartobjects chartObjs = (Chartobjects); 
            //wcurrentWs.Chartobjects(Type.Missing);
            //Chartobject chartObj = chartObjs.Add(100, 20, 300, 300);
            //Chart xlChart = chartObj.Chart;

            return errore;
        }

        //private void Plot() 
        //{
        //    //TODO se serve, aggiustare
        //    xclApp.Visible = true;
        //    object tipoFoglio = (object)XlSheetType.xlWorksheet;
        //    Workbook wb = xclApp.Workbooks.Add(tipoFoglio);
        //    Worksheet ws = (Worksheet) xclApp.ActiveSheet;

        //   // Now create the chart.
        //    Chartobjects chartObjs = (Chartobjects)ws.Chartobjects(Type.Missing);
        //    Chartobject chartObj = chartObjs.Add(100, 20, 300, 300);
        //    Chart xlChart = chartObj.Chart;

        //    int nRows = 25;
        //    int nColumns = 25;
        //    string upperLeftCell = "B3";
        //    int endRowNumber = System.Int32.Parse(upperLeftCell.Substring(1)) 
        //        + nRows - 1;
        //    char endColumnLetter = System.Convert.ToChar(
        //        Convert.ToInt32(upperLeftCell[0]) + nColumns - 1);
        //    string upperRightCell = System.String.Format("{0}{1}", 
        //        endColumnLetter, System.Int32.Parse(upperLeftCell.Substring(1)));
        //    string lowerRightCell = System.String.Format("{0}{1}", 
        //        endColumnLetter, endRowNumber);

        //    // Send single dimensional array to Excel:
        //    Range rg1 = ws.get_Range("B2", "Z2");
        //    double[] xarray = new double[nColumns];
        //    ws.Cells[1, 1] = "Data for surface chart";
        //    for (int i = 0; i < xarray.Length; i++)
        //    {
        //        xarray[i] = -3.0f + i * 0.25f;
        //        ws.Cells[i + 3, 1] = xarray[i];
        //        ws.Cells[2, 2 + i] = xarray[i];
        //    }

        //    Range rg = ws.get_Range(upperLeftCell, lowerRightCell);
        //    rg.Value2 = AddData(nRows,nColumns);

        //    Range chartRange = ws.get_Range("A2", lowerRightCell);
        //    xlChart.SetSourceData(chartRange, Type.Missing);
        //    xlChart.ChartType = XlChartType.xlSurface;

        //    // Customize axes:
        //    Axis xAxis = (Axis)xlChart.Axes(XlAxisType.xlCategory,
        //        XlAxisGroup.xlPrimary);
        //    xAxis.HasTitle = true;
        //    xAxis.AxisTitle.Text = "X Axis";

        //    Axis yAxis = (Axis)xlChart.Axes(XlAxisType.xlSeriesAxis,
        //        XlAxisGroup.xlPrimary);
        //    yAxis.HasTitle = true;
        //    yAxis.AxisTitle.Text = "Y Axis";

        //    Axis zAxis = (Axis)xlChart.Axes(XlAxisType.xlValue,
        //        XlAxisGroup.xlPrimary);
        //    zAxis.HasTitle = true;
        //    zAxis.AxisTitle.Text = "Z Axis";

        //    // Add title:
        //    xlChart.HasTitle = true;
        //    xlChart.ChartTitle.Text = "Peak Function";

        //    // Remove legend:
        //    xlChart.HasLegend = false;
            
        //    /* This following code is used to create Excel default color indices:
        //    for (int i = 0; i < 14; i++)
        //    {
        //        string cellString = "A" + (i + 1).ToString();
        //        ws.get_Range(cellString, cellString).Interior.ColorIndex = i + 1;
        //        ws.get_Range(cellString, cellString).Value2 = i + 1;
        //        cellString = "B" + (i + 1).ToString();
        //        ws.get_Range(cellString, cellString).Interior.ColorIndex = 14 + i + 1;
        //        ws.get_Range(cellString, cellString).Value2 = 14 + i + 1;
        //        cellString = "C" + (i + 1).ToString();
        //        ws.get_Range(cellString, cellString).Interior.ColorIndex = 2 * 14 + i + 1;
        //        ws.get_Range(cellString, cellString).Value2 = 2 * 14 + i + 1;
        //        cellString = "D" + (i + 1).ToString();
        //        ws.get_Range(cellString, cellString).Interior.ColorIndex = 3 * 14 + i + 1;
        //        ws.get_Range(cellString, cellString).Value2 = 3 * 14 + i + 1;
        //    }*/
        //}

        private double[,] AddData(int nRows, int nColumns)
        {
            double[,] dataArray = new double[nRows, nColumns];
            double[] xarray = new double[nColumns];
            for (int i = 0; i < xarray.Length; i++)
            {
                xarray[i] = -3.0f + i * 0.25f;
            }
            double[] yarray = xarray;

            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                for (int j = 0; j < dataArray.GetLength(1); j++)
                {
                    dataArray[i, j] = 3 * Math.Pow((1 - xarray[i]), 2)
                        * Math.Exp(-xarray[i] * xarray[i] -
                        (yarray[j] + 1) * (yarray[j] + 1)) -
                        10 * (0.2 * xarray[i] - Math.Pow(xarray[i], 3) -
                        Math.Pow(yarray[j], 5)) *
                        Math.Exp(-xarray[i] * xarray[i] - yarray[j] * yarray[j])
                        - 1 / 3 * Math.Exp(-(xarray[i] + 1) * (xarray[i] + 1) -
                        yarray[j] * yarray[j]);
                }
            }
            return dataArray;
        }

        internal override string SaveAsPdf(int[] SheetsToBeSelected)
        {
            string nomeFile = xclWorkbook.Name.Replace(".xlsx", "");
            nomeFile = nomeFile.Replace(".xls", "");
            XlFixedFormatType exportFormat = XlFixedFormatType.xlTypePDF;
            XlFixedFormatQuality exportQuality =
                XlFixedFormatQuality.xlQualityStandard;
            bool paramOpenAfterPublish = false;
            bool includeDocProps = true;
            bool paramIgnorePrintAreas = true;
            object paramFromPage = Type.Missing;
            object paramToPage = Type.Missing;

            for (int i = 0; i < SheetsToBeSelected.Length; i++)
            {
                Excel.Worksheet xclSheet; 
                try { 
                    ((Excel.Worksheet)xclApp.ActiveWorkbook.Sheets[SheetsToBeSelected[i]]).Select(oMissing);
                    xclSheet = (Excel.Worksheet)xclWorkbook.Sheets[SheetsToBeSelected[i]];
                } catch (Exception e)
                {
                    break; // prossimo figlio
                }
                 
                string pathOut = xclWorkbook.Path + "\\" + nomeFile + "_" + i.ToString("00");
                try
                {
                    if (xclWorkbook != null)
                        xclSheet.ExportAsFixedFormat(exportFormat,
                            pathOut, exportQuality,
                            includeDocProps, paramIgnorePrintAreas, paramFromPage,
                            paramToPage, paramOpenAfterPublish,
                            oMissing);
                }
                catch (Exception ex)
                {
                    string err = "PopulateExcel" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                    return CommonObjectsAndData.LogDelProgramma.Error(err, ex);
                }
            }
            return "OK";
        }

        internal override int CurrentRow()
        {
            Excel.Range rng = (Excel.Range) xclApp.ActiveCell;
            if (rng != null)
            {
                //get the cell value
                object cellValue = rng.Value;

                //get the row and column details
                int row = rng.Row;
                int column = rng.Column;
                return row; 
            }
            else
            {
                return -1;
            }
        }

        internal override int CurrentColumn()
        {
            Excel.Range rng = (Excel.Range) xclApp.ActiveCell;
            if (rng != null) {
                //get the cell value
                object cellValue = rng.Value;

                //get the row and column details
                int row = rng.Row;
                int column = rng.Column;
                return column; 
            } 
            else
            {
                return -1; 
            }
        }

        internal override int CurrentSheet()
        {
            Excel.Worksheet sheet = (Excel.Worksheet)xclApp.ActiveSheet;
            if (sheet != null)
            {
                return sheet.Index;
            } else
            {
                return -1;
            }
        }

        internal string GetOfficeVersion()
        {
            string sVersion = string.Empty;
            Excel.Application appVersion = new Excel.Application();
            appVersion.Visible = false;
            switch (appVersion.Version.ToString())
            {
                case "7.0":
                    sVersion = "95";
                    break;
                case "8.0":
                    sVersion = "97";
                    break;
                case "9.0":
                    sVersion = "2000";
                    break;
                case "10.0":
                    sVersion = "2002";
                    break;
                case "11.0":
                    sVersion = "2003";
                    break;
                case "12.0":
                    sVersion = "2007";
                    break;
                case "14.0":
                    sVersion = "2010";
                    break;
                case "15.0":
                    sVersion = "2013";
                    break;
                default:
                    sVersion = "Too Old!";
                    break;
            }
            CommonObjectsAndData.LogDelProgramma.Error("MS office version: " + sVersion, null);
            return null;
        }

        internal override object GetCurrentCellValueAndCoordinates(ref int row, ref int column)
        {
            Excel.Range rng = (Excel.Range)xclApp.ActiveCell;
            //get the row and column details
            row = rng.Row;
            column = rng.Column;
            //get the cell value
            return rng.Value;
        }

        internal override void SetCurrentSheet(int foglio)
        {
            Worksheet sheet = (Worksheet)xclWorkbook.Worksheets[foglio]; 
            sheet.Select(); 
        }
    }
}
