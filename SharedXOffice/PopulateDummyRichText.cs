using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SharedData; 

namespace Comuni.XOffice
{
    class PopulateDummyRichText : AbstractPopulateRichText
    {
        public PopulateDummyRichText (string TemplateFile, string ResultFile) : base (TemplateFile, ResultFile)
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|Costruttore 2:\n" + TemplateFile + "," + ResultFile);
        }

        public  override string Close()
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|Close():\n");
            return "Close()"
;       }

        public  override void InsertInBookmark(string BookmarkName, string text)
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|InsertInBookmark():\n" + BookmarkName + "," + text);
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
            CommonData.CommonObj.LogOfProgram.Event(s);
            return "ReplaceSquaredParenthesisTag()"; 
        }

        public override string ShowWindow()
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|ShowWindow():\n");
            return "ShowWindow()";
        }

        public override string SaveAsPdf()
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|SaveAsPdf():\n");
            return "Dummy SaveAsPdf";
        }

        public  override string Save()
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|Save():\n");
            return "Save()"; 
        }

        public override string InsertTextInTable(int TableNumber, int RowNumber, int ColumnNumber, 
            string CellText, bool CreateNewRowAfter)
        {
            string s = "PopulateRichText|InsertTextInTable():\n" + TableNumber + "," +
                    RowNumber + "," + ColumnNumber + "," + CellText + "," + CreateNewRowAfter;
            CommonData.CommonObj.LogOfProgram.Event (s);
            return "InsertTextInTable()"; 
        }

        public  override string DeleteRowInTable(int TableNumber, int Row)
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|DeleteRowInTable():\n" + TableNumber + "," + Row);
            return "DeleteRowInTable()"; 
        }

        public override void ReplaceSquaredParenthesisTagAndNearCell(string Tag, string ReplacingString, string NearbyNote)
        {
            CommonData.CommonObj.LogOfProgram.Event("PopulateRichText|ReplaceSquaredParenthesisTagAndNearCell():\n" +
                String.Format("Tag {0}, ReplacingString {1}, NearbyNote: {2}", Tag, ReplacingString, NearbyNote));
        }
    }
}
