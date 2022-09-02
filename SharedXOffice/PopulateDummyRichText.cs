using System;
using GlucoMan;

namespace Comuni.XOffice
{
    class PopulateDummyRichText : AbstractPopulateRichText
    {
        public PopulateDummyRichText (string TemplateFile, string ResultFile) : base (TemplateFile, ResultFile)
        {
            Common.LogOfProgram.Event("PopulateRichText|Costruttore 2:\n" + TemplateFile + "," + ResultFile);
        }

        public  override string Close()
        {
            Common.LogOfProgram.Event("PopulateRichText|Close():\n");
            return "Close()"
;       }

        public  override void InsertInBookmark(string BookmarkName, string text)
        {
            Common.LogOfProgram.Event("PopulateRichText|InsertInBookmark():\n" + BookmarkName + "," + text);
        }
        
        /// <summary>
        /// Sostituisce una stringa con un'altra in tutto il file
        /// </summary>
        /// <param name="Tag">Stringa da sostituire</param>
        /// <param name="ReplacingString">Stringa che viene messa nel file</param>
        /// <returns>Prompt di errore, "OK" se tutto bene</returns>
        public  override string ReplaceSquaredParenthesisTag(string Tag, string ReplacingString)
        {
            string s = "PopulateRichText|ReplaceSquaredParenthesisTag():\n" + Tag + "," + ReplacingString;
            Common.LogOfProgram.Event(s);
            return "ReplaceSquaredParenthesisTag()"; 
        }

        public override string ShowWindow()
        {
            Common.LogOfProgram.Event("PopulateRichText|ShowWindow():\n");
            return "ShowWindow()";
        }

        public override string SaveAsPdf()
        {
            Common.LogOfProgram.Event("PopulateRichText|SaveAsPdf():\n");
            return "Dummy SaveAsPdf";
        }

        public  override string Save()
        {
            Common.LogOfProgram.Event("PopulateRichText|Save():\n");
            return "Save()"; 
        }

        public override string InsertTextInTable(int TableNumber, int RowNumber, int ColumnNumber, 
            string CellText, bool CreateNewRowAfter)
        {
            string s = "PopulateRichText|InsertTextInTable():\n" + TableNumber + "," +
                    RowNumber + "," + ColumnNumber + "," + CellText + "," + CreateNewRowAfter;
            Common.LogOfProgram.Event (s);
            return "InsertTextInTable()"; 
        }

        public  override string DeleteRowInTable(int TableNumber, int Row)
        {
            Common.LogOfProgram.Event("PopulateRichText|DeleteRowInTable():\n" + TableNumber + "," + Row);
            return "DeleteRowInTable()"; 
        }

        public override void ReplaceSquaredParenthesisTagAndNearCell(string Tag, string ReplacingString, string NearbyNote)
        {
            Common.LogOfProgram.Event("PopulateRichText|ReplaceSquaredParenthesisTagAndNearCell():\n" +
                String.Format("Tag {0}, ReplacingString {1}, NearbyNote: {2}", Tag, ReplacingString, NearbyNote));
        }
    }
}
