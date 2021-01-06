using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Comuni.XOffice
{
    public abstract class AbstractPopulateRichText
    {
        bool isLogging = false;
        internal string errorLogFilePathAndFilename;

        public  AbstractPopulateRichText(ref string TemplateFile, ref string ResultFile)
        {
            isLogging = true;

            string pathEFileExe = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8);
            string pathExe = pathEFileExe.Substring(0, pathEFileExe.LastIndexOf("/") + 1).Replace("/", "\\");

            //// se la prima lettera non è uno slash, o la seconda non è un :, assumo che la cartella 
            //// sia relativa ed aggiungo la cartella dell'eseguibile come "inizio"
            //if (LogFile.Substring(0, 1) != "\\" && LogFile.Substring(1, 1) != ":")
            //    logTextPathAndFile = pathExe + LogFile; 
            //else
            //    logTextPathAndFile = LogFile;

            if (TemplateFile.Substring(0, 1) != "\\" && TemplateFile.Substring(1, 1) != ":")
                TemplateFile = pathExe + TemplateFile;

            if (ResultFile.Substring(0, 1) != "\\" && ResultFile.Substring(1, 1) != ":")
                ResultFile = pathExe + ResultFile;
        }

        public  AbstractPopulateRichText (string TemplateFile, string ResultFile)
        {
            string pathEFileExe = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string pathExe = pathEFileExe.Substring(0, pathEFileExe.LastIndexOf("/") + 1);

            if (TemplateFile.Substring(0, 1) != "\\" && TemplateFile.Substring(1, 1) != ":")
                TemplateFile = pathExe + TemplateFile;

            if (ResultFile.Substring(0, 1) != "\\" && ResultFile.Substring(1, 1) != ":")
                ResultFile = pathExe + ResultFile;
        }

        public  abstract string Close(); 

        public  abstract void InsertInBookmark(string BookmarkName, string text);         
        
        public  abstract string ReplaceSquaredParenthesisTag(string Tag, string ReplacingString); 

        public  abstract string ShowWindow(); 
		
        public  abstract string SaveAsPdf(); 

        public  abstract string Save();

        public  abstract string InsertTextInTable(int TableNumber, int RowNumber, int ColumnNumber, 
            string CellText, bool CreateNewRowAfter);

        public  abstract string DeleteRowInTable(int TableNumber, int Row);

        public abstract void ReplaceSquaredParenthesisTagAndNearCell(string tag, string siNoNa, string nota); 
    }
}
