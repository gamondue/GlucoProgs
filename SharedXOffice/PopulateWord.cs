using GlucoMan;
using NetOffice.WordApi.Enums;
using System;
using System.Threading;
using Word = NetOffice.WordApi;

namespace Comuni.XOffice
{
    public class PopulateWord : AbstractPopulateRichText
    {
        // per farlo funzionare: prima mettere un riferimento alla type library di Office
        // Riferimenti | click destro | aggiungi riferimento | COM | Libreria oggetti di Microsoft Word:
        // Microsoft Word XX object Library
        //
        // per usare i comandi COM di Interop assegnare ogni oggetto a variabile del programma C#
        // altrimenti la memoria non viene deallocata alla fine del programma C# ed 
        // il processo relativo al programma Office utilizzato rimane aperto

        Word.Application wrdApp;
        Word.Document wrdDoc;

        // C# doesn't have optional arguments so we'll need a dummy value
        object oMissing = System.Reflection.Missing.Value; 

        private int nUltimaTabellaUsata;
        private int nUltimaRigaUsata;
        private Word.Row UltimaRigaUsata;

        //public  string InsertTextInBookmarkedTable(string BookmarkName, int RowNumber, int ColumnNumber, 
        //    string CellText, bool CreateNewRowAfter)
        //{   //TODO Fare! 
        //    //Row riga = GetTableRowFromBookmark(string BookmarkName);
        //    //if (CreateNewRowAfter)
        //    //    UltimaRigaUsata.Range.Rows.Add();
        //}

        public PopulateWord (string TemplateFile, string ResultFile) : base (TemplateFile, ResultFile)
        {
            CostruttoreConDueParametri(TemplateFile, ResultFile); 
        }

        private string CostruttoreConDueParametri(string TemplateFile, string ResultFile)
        {
            wrdApp = new Word.Application();

            object openFilename = (object) TemplateFile;
            object saveAsFilename = (object) ResultFile;

            object readOnly = (object) false;
            object isVisible = (object) false; 
            wrdApp.Visible = false;            
            wrdApp.ScreenUpdating = false;   

            try 
            {
                // API doc
                // Document Open(
                //   ref object FileName, ref object ConfirmConversions, ref object ReadOnly, ref object AddToRecentFiles, ref object PasswordDocument,
                //   ref object PasswordTemplate, ref object Revert, ref object WritePasswordDocument, ref object WritePasswordTemplate, ref object Format,
                //   ref object Encoding, ref object Visible, ref object OpenAndRepair, ref object DocumentDirection, ref object NoEncodingDialog,
                //   ref object XMLTransform
                // )
                // Apre in sola lettura il modello di Word, Use the dummy value as a placeholder for optional arguments
                //wrdDoc = wrdApp.Documents.Open(ref openFilename, ref oMissing, ref readOnly, ref oMissing, ref oMissing, 
                //             ref oMissing,  ref oMissing, ref oMissing, ref oMissing, ref oMissing, 
                //             ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing,
                //             ref oMissing);
                wrdDoc = wrdApp.Documents.Open(openFilename, oMissing, readOnly, oMissing, oMissing,
                          oMissing,  oMissing,  oMissing,  oMissing,  oMissing,
                          oMissing,  isVisible,  oMissing,  oMissing,  oMissing,
                          oMissing);
                wrdApp.Documents.Add(TemplateFile);
                object fileFormat = WdSaveFormat.wdFormatDocumentDefault;
                // salva con il nome finale 
                // void SaveAs(
                //    ref object FileName, ref object FileFormat,	ref object LockComments, ref object Password, ref object AddToRecentFiles,
                //    ref object WritePassword, ref object ReadOnlyRecommended, ref object EmbedTrueTypeFonts, ref object SaveNativePictureFormat, ref object SaveFormsData,
                //    ref object SaveAsAOCELetter,ref object Encoding, ref object InsertLineBreaks, ref object AllowSubstitutions, ref object LineEnding,
                //    ref object AddBiDiMarks )
                //wrdDoc.SaveAs(ref saveAsFilename, ref fileFormat, ref oMissing, ref oMissing, ref oMissing,
                //              ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, 
                //              ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, 
                //              ref oMissing);
                wrdDoc.SaveAs(saveAsFilename,   fileFormat, oMissing, oMissing, oMissing,
                                oMissing, oMissing, oMissing, oMissing, oMissing,
                                oMissing, oMissing, oMissing, oMissing, oMissing,
                                oMissing);
                return "OK";
            }
            catch (Exception ex){
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }
        }

        private Word.Row InsertTableCellText(Word.Row TableRow, int Column, string Text, bool CreateNewRow)
        {
            try
            {
                TableRow.Cells[Column].Range.InsertAfter(Text);

                if (CreateNewRow)
                    return TableRow.Range.Rows.Add();
                else
                    return null;
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                General.Log.Error(err, ex);
                return null;
            }
        }

        private Word.Row GetTableRowFromTableNumber(int nTable, int Index)
        {
            try
            {
                return wrdDoc.Tables[nTable].Rows[Index];
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                General.Log.Error(err, ex);
                return null;
            }
        }

        private Word.Row GetTableRowFromBookmark(string BookmarkName)
        {
            // TODO fare! 
            //try
            //{
            //    Row Row = null;
            //    //bk = xclWorkbook.Bookmarks.Item(BookmarkName);
            //    Bookmark bk = xclWorkbook.Bookmarks[BookmarkName]; 
            //    Table t = bk.Tables[0];
            //    Column C = bk.Column; 
            //        bk.Rows
            //        //.Range .Cells(1).Row;
            //    return Row;
            //}
            //catch (Exception ex)
            //{
            //    Common.LogErrore("PopulateWord|GetTableRowFromBookmark():\n" + ex.Message);
            return null;
            //}
        }

        public override string Close()
        {
            try
            {
                //???? salvare per default alla chiusura ????
                //xclWorkbook.SaveAs(ref saveAsFilename, ref fileFormat, ref missing, ref missing, ref missing,
                //                ref missing, ref missing, ref missing, ref missing, ref missing,
                //                ref missing, ref missing, ref missing, ref missing, ref missing,
                //                ref missing);
                // API doc
                // void Close(
                //    ref object SaveChanges, ref object OriginalFormat, ref object RouteDocument )
                
                // chiude il documento

                object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
                // Close the wrdApp document, but leave the wrdApp application open.
                // xclWorkbook has to be cast to type _xclWorkbook so that it will find the
                // correct Close method.                
                //((_Document)wrdDoc).Close(ref saveChanges, ref oMissing, ref oMissing);
                ((Word._Document)wrdDoc).Close(saveChanges, oMissing, oMissing);
                wrdDoc = null;

                // Chiude Word
                // wrdApp has to be cast to type _Application so that it will find
                // the correct Quit method.
                //((_Application)wrdApp).Quit(ref oMissing, ref oMissing, ref oMissing);
                ((Word._Application)wrdApp).Quit(oMissing, oMissing, oMissing);
                wrdApp = null;

                wrdApp.Dispose();
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
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }
        }

        public  override void InsertInBookmark(string BookmarkName, string text)
        {
            try
            {
                wrdDoc.Bookmarks[BookmarkName].Select();
                wrdApp.Selection.TypeText(text);
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                General.Log.Error(err, ex);
            }
            // NOTA; dopo l'inserzione in un bookmark, il bookmark sparisce
        }
        
        /// <summary>
        /// Sostituisce una stringa con un'altra in tutto il file
        /// </summary>
        /// <param name="Tag">Stringa da sostituire</param>
        /// <param name="ReplacingString">Stringa che viene messa nel file</param>
        /// <returns>Prompt di errore, "OK" se tutto bene</returns>
        public  override string ReplaceSquaredParenthesisTag(string Tag, string ReplacingString)
        {
            try
            {
                // resetta la formattazione delle ricerche precedenti
                wrdDoc.Select(); // senza questo andava a modificare un altro documento identico
                // (Documento1.doc), che si apriva non si sa perchè 

                wrdApp.Selection.Find.ClearFormatting();

                Word.Find findobject = wrdApp.Selection.Find;

                findobject.ClearFormatting();
                findobject.Text = "[" + Tag + "]";
                findobject.Replacement.ClearFormatting();
                findobject.Replacement.Text = ReplacingString;

                object MatchCase = false;        // ricerca case sensitive 
                object MatchWholeWord = true;   // ricerca della parola intera 
                object MatchWildcards = false;
                object MatchSoundsLike = false;
                object Forward = true;
                object Wrap = WdFindWrap.wdFindContinue; // cerca su tutto il testo, ricominciando se arriva in fondo
                // API doc
                // bool Execute(
                //    ref object FindText, ref object MatchCase, ref object MatchWholeWord, ref object MatchWildcards, ref object MatchSoundsLike,
                //    ref object MatchAllWordForms, ref object Forward, ref object Wrap, ref object Format, object ReplaceWith,
                //    ref object Replace, ref object MatchKashida, ref object MatchDiacritics, ref object MatchAlefHamza, ref object MatchControl )
                object replaceAll = WdReplace.wdReplaceAll;
                //findobject.Execute(ref oMissing, ref MatchCase, ref MatchWholeWord, ref oMissing, ref oMissing,
                //                   ref oMissing, ref Forward, ref Wrap, ref oMissing, ref oMissing,
                //                   ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                findobject.Execute(  oMissing,   MatchCase,   MatchWholeWord, oMissing, oMissing,
                                     oMissing,   Forward,   Wrap, oMissing, oMissing,
                                     replaceAll, oMissing, oMissing, oMissing, oMissing);
                //findobject.Execute(ref missing, ref MatchCase, ref MatchWholeWord, ref MatchWildcards, ref MatchSoundsLike,
                //   ref missing, ref missing, ref Wrap, ref missing, ref missing,
                //   ref replaceAll, ref missing, ref missing, ref missing, ref missing);
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }  
        }

        public override void ReplaceSquaredParenthesisTagAndNearCell(string Tag, string ReplacingString, string NearbyNote)
        {
            try
            {
                // resetta la formattazione delle ricerche precedenti
                wrdDoc.Select(); // senza questo andava a modificare un altro documento identico
                // (Documento1.doc), che si apriva non si sa perchè 

                wrdApp.Selection.Find.ClearFormatting();

                Word.Find findobject = wrdApp.Selection.Find;

                findobject.ClearFormatting();
                findobject.Text = "[" + Tag + "]";
                findobject.Replacement.ClearFormatting();
                findobject.Replacement.Text = ReplacingString;

                object MatchCase = false;        // ricerca case sensitive 
                object MatchWholeWord = true;   // ricerca della parola intera 
                object MatchWildcards = false;
                object MatchSoundsLike = false;
                object Forward = true;
                object Wrap = WdFindWrap.wdFindContinue; // cerca su tutto il testo, ricominciando se arriva in fondo
                // API doc
                // bool Execute(
                //    ref object FindText, ref object MatchCase, ref object MatchWholeWord, ref object MatchWildcards, ref object MatchSoundsLike,
                //    ref object MatchAllWordForms, ref object Forward, ref object Wrap, ref object Format, object ReplaceWith,
                //    ref object Replace, ref object MatchKashida, ref object MatchDiacritics, ref object MatchAlefHamza, ref object MatchControl )
                object replaceAll = WdReplace.wdReplaceAll;
                //findobject.Execute(ref oMissing, ref MatchCase, ref MatchWholeWord, ref oMissing, ref oMissing,
                //                   ref oMissing, ref Forward, ref Wrap, ref oMissing, ref oMissing,
                //                   ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                findobject.Execute(oMissing, MatchCase, MatchWholeWord, oMissing, oMissing,
                                     oMissing, Forward, Wrap, oMissing, oMissing,
                                     replaceAll, oMissing, oMissing, oMissing, oMissing);
                //findobject.Execute(ref missing, ref MatchCase, ref MatchWholeWord, ref MatchWildcards, ref MatchSoundsLike,
                //   ref missing, ref missing, ref Wrap, ref missing, ref missing,
                //   ref replaceAll, ref missing, ref missing, ref missing, ref missing);
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                General.Log.Error(err, ex);
            }
        }


        public override string ShowWindow()
        {
            try{
                //wrdApp.Visible = true;
                if (wrdApp.Application.Visible == false)
                    wrdApp.Application.Visible = true; 
        
                wrdApp.Application.Activate();
                //Err.Clear();
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }
        }

        public  override string SaveAsPdf()
        {
            // void SaveAs(
            //    ref object FileName, ref object FileFormat,	ref object LockComments, ref object Password, ref object AddToRecentFiles,
            //    ref object WritePassword, ref object ReadOnlyRecommended, ref object EmbedTrueTypeFonts, ref object SaveNativePictureFormat, ref object SaveFormsData,
            //    ref object SaveAsAOCELetter,ref object Encoding, ref object InsertLineBreaks, ref object AllowSubstitutions, ref object LineEnding,
            //    ref object AddBiDiMarks )
            try
            {
                string outputFileName = wrdDoc.FullName.Replace(".docx", ".pdf");
                outputFileName = outputFileName.Replace(".doc", ".pdf");
                object fileFormat = WdSaveFormat.wdFormatPDF;
                //wrdDoc.SaveAs(ref outputFileName,
                //    ref fileFormat, ref oMissing, ref oMissing,
                //    ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                //    ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                //    ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                wrdDoc.SaveAs(  outputFileName,
                                  fileFormat, oMissing, oMissing,
                                  oMissing, oMissing, oMissing, oMissing,
                                  oMissing, oMissing, oMissing, oMissing,
                                  oMissing, oMissing, oMissing, oMissing);
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateWord";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }
        }

        public override string Save()
        {
            try
            {
              //  wrdDoc.SaveAs(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
              //ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
              //ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
              //ref oMissing);
                wrdDoc.SaveAs(  oMissing, oMissing, oMissing, oMissing, oMissing,
                                  oMissing, oMissing, oMissing, oMissing, oMissing,
                                  oMissing, oMissing, oMissing, oMissing, oMissing,
                                  oMissing);
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateWord. Impossibile salvare il documento";
                err += "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err,ex);
            }
        }

        public  override string InsertTextInTable(int TableNumber, int RowNumber, int ColumnNumber, 
            string CellText, bool CreateNewRowAfter)
        {
            try
            {
                // se non è cambiata, usa l'ultima riga che era stata usata in precedenza 
                if (nUltimaTabellaUsata != TableNumber || nUltimaRigaUsata != RowNumber)
                {
                    UltimaRigaUsata = GetTableRowFromTableNumber(TableNumber, RowNumber);
                }
                UltimaRigaUsata.Cells[ColumnNumber].Range.InsertAfter(CellText);
                nUltimaTabellaUsata = TableNumber;
                nUltimaRigaUsata = RowNumber;
                if (CreateNewRowAfter)
                    UltimaRigaUsata.Range.Rows.Add();
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }
        }

        public  override string DeleteRowInTable(int TableNumber, int Row)
        {
            try
            {
                GetTableRowFromTableNumber(TableNumber, Row).Delete();
                return "OK";
            }
            catch (Exception ex)
            {
                string err = "PopulateWord" +
                     "\nMetodo: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                return General.Log.Error(err, ex);
            }
        }

        public  string GetOfficeVersion()
        {
            string sVersion = string.Empty;
            Word.Application appVersion = new Word.Application();
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
                    sVersion = "Too Old (or Too New)!";
                    break;
            }
            General.Log.Error("MS office version: " + sVersion, null);
            return null;
        }
    }
}
