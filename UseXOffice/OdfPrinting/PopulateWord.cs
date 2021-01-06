using System;
using System.Collections.Generic;
using System.Text;

namespace CalcoliConExcel
{
    class PopulateWord
    {
        Word.Application wrdApp;
        Word.Document wrdDoc;

        // per farlo funzionare: prima mettere un riferimento alla type library di Office
        // Riferimenti | click destro | aggiungi riferimento | COM | Libreria oggetti di Microsoft Word:
        // Microsoft Word XX Object Library

        public PopulateWord (string TemplateFile )
        {
        // Apre word e riempie il modello
        wrdApp = new Word.Application();

            try {
                wrdApp.Documents.Add(TemplateFile);
                // compilazione del bookmark
                wrdDoc = wrdApp.ActiveDocument;
            }
            catch (Exception ex){
                //LogErrore("PopulateWord|New(): " + ex.Message);
            }
        }
        
        public void InsertWord(string bookmarkName, string text)
        {
            wrdDoc.Bookmarks.Item(bookmarkName).Range.InsertAfter(text);
        }

        public void VisualizzaDocumento()
        {
            wrdApp.Visible = true;
            if (wrdApp.Application.Visible = false)
                wrdApp.Application.Visible = true; 
        
            wrdApp.Application.Activate(); 
        }

        public void SalvaDocumento()
        {
            try {
                wrdDoc.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossibile salvare il documento\n" + ex.Message, 
                "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public object InsertTableRowText(object oRow, string s1 , string s2, string s3, 
            string s4, bool bCreateNewRow) 
        {
            oRow.Cells(1).Range.InsertAfter(s1);
            oRow.Cells(2).Range.InsertAfter(s2);
            oRow.Cells(3).Range.InsertAfter(s3);
            if (s4 != "")
                oRow.Cells(4).Range.InsertAfter(s4);

            if (bCreateNewRow)
                InsertTableRowText = oRow.Range.Rows.Add;
            else
                InsertTableRowText = null; 
        }

        public object InsertTableRowText(object oRow, string s1, string s2, 
                    string s3, string s4, string s5, bool bCreateNewRow)
        {
            oRow.Cells(1).Range.InsertAfter(s1);
            oRow.Cells(2).Range.InsertAfter(s2);
            oRow.Cells(3).Range.InsertAfter(s3);
            oRow.Cells(4).Range.InsertAfter(s4);
            oRow.Cells(5).Range.InsertAfter(s5);
            if (bCreateNewRow)
                return oRow.Range.Rows.Add;
            else
                return null;
        }

        public object GetTableRowFromBookmark(string sBookmark )
        {
            object oRow;
            object  bk;
            bk = wrdDoc.Bookmarks.Item(sBookmark);
            oRow = bk.Range.Cells(1).Row;
            return oRow; 
        }

        public void ShowWordWindow()
        {
            wrdApp.Application.Visible = true; 
            AppActivate("Microsoft Word - Verbale"); 
            Err.Clear();
        }
    }
}
