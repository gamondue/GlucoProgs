using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Comuni.XOffice
{
    public abstract class AbstractPopulateSpreadsheet
    {
        bool isLogging = false;
        internal string errorLogFilePathAndFilename;
        private string FileToOpen;
        private string resultFile;

        internal AbstractPopulateSpreadsheet(string TemplateFile, string ResultFile, string LogFile)
        {
            isLogging = true; 
            errorLogFilePathAndFilename = LogFile; 
        }

        internal AbstractPopulateSpreadsheet(string FileToOpen)
        {
            // TODO: Complete member initialization
            this.FileToOpen = FileToOpen;
        }

        public AbstractPopulateSpreadsheet(string FileToOpen, string resultFile) : this(FileToOpen)
        {
            this.resultFile = resultFile;
        }

        internal abstract string Close(); 
                
        /// <summary>
        /// Sostituisce una stringa con un'altra in ogni foglio del file
        ///  vc </summary>
        /// <param name="Tag">Stringa senza blank da sostituire</param>
        /// <param name="ReplacingString">Stringa che viene messa nel file</param>
        /// <returns>Prompt di errore, "OK" se tutto bene</returns>
        internal abstract string ReplaceSquaredParenthesisTag(string Tag, string ReplacingString);

        internal abstract string ReplaceSquaredParenthesisTagAndNearCell(string Tag, string ReplacingString, string Note); 

        internal abstract string ShowWindow();

        internal abstract string Save();

        internal abstract string InsertTextInCell(int SheetNumber, int RowNumber, string ColumnName, 
                string CellText);

        internal abstract string InsertTextInCell(int SheetNumber, int RowNumber, int ColumnNumber,
                string CellText);
                
        internal abstract string InsertIntegerInCell(int SheetNumber, int RowNumber, int ColumnNumber, int value);
        
        internal abstract string InsertDoubleInCell(int SheetNumber, int RowNumber, int ColumnNumber, double value); 

        internal abstract void DeleteRow(int SheetNumber, int Row);

        internal abstract string SaveAsPdf(int[] sheetsToBeSelected);

        internal abstract object GetCurrentCellValueAndCoordinates(ref int row, ref int column);

        internal abstract int CurrentRow();

        internal abstract int CurrentColumn();

        internal abstract int CurrentSheet();

        internal abstract void SetCurrentSheet(int foglio); 
    }
}
