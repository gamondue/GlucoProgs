using System;
using System.Windows.Forms;
using gamon;

namespace gamon.XOffice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            //PopulateWord wd = new PopulateWord(@".\Modello_di_prova.dot", @".\provaOut.doc");
            AbstractPopulateRichText wd = new PopulateWord(@"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\Modello_di_prova.dot",
                @"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\prova.doc", ".\\errori.txt");
            wd.InsertInBookmark("PostoDoveScrivere", "stringa sostituita");
            wd.InsertInBookmark("DoveScrivereInTabella", "stringa in tabella");
            wd.ReplaceSquaredParenthesisTag("[Cambio qui]", "Ho cambiato i tag 'quadrati'!");

            // aggiunge 3 righe alla tabella

            // riempie alcune celle della tabella
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    wd.InsertTextInTable(1, i, j, i.ToString() + j.ToString(), false);
                }
            }
            wd.InsertTextInTable(1, 5, 3, "Aggiunta nell'ultima riga, ora tutte le righe hanno qualcosa", false);
            wd.InsertTextInTable(1, 4, 3, "Aggiunta in un punto qualsiasi, con creazione di nuova riga dopo", true);

            // posizione 3,3 della tabella 2
            wd.InsertTextInTable(2, 3, 3, "Posizione 3,3 della tabella 2", false);
            // posizione 1, 5 della tabella individuata da segnalibro "NellaTabella2"
            //wd.InsertTextInBookmarkedTable("NellaTabella2", 1, 5, "Tabella individuata con segnalibro", false);

            wd.Save();

            wd.Close();
            wd = null; 
        }

        private void btnWord_PDF_Click(object sender, EventArgs e)
        {
            //PopulateWord wd = new PopulateWord(@".\Modello_di_prova.dot", @".\provaOut.doc");
            AbstractPopulateRichText wd = new PopulateWord(@"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\Modello_di_prova.dot",
                @"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\prova.doc", ".\\errori.txt");
            wd.InsertInBookmark("PostoDoveScrivere", "stringa sostituita il giorno " + DateTime.Now.ToString("yyyy-MM-dd") + " ");
            wd.InsertInBookmark("DoveScrivereInTabella", "stringa in tabella");
            wd.ReplaceSquaredParenthesisTag("[Cambio qui]", "Ho cambiato i tag 'quadrati'!");

            // aggiunge 3 righe alla tabella

            // riempie alcune celle della tabella
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    wd.InsertTextInTable(1, i, j, i.ToString() + j.ToString(), false);
                }
            }
            wd.InsertTextInTable(1, 5, 3, "Aggiunta nell'ultima riga, ora tutte le righe hanno qualcosa", false);
            wd.InsertTextInTable(1, 4, 3, "Aggiunta in un punto qualsiasi, con creazione di nuova riga dopo", true);

            // posizione 3,3 della tabella 2
            wd.InsertTextInTable(2, 3, 3, "Posizione 3,3 della tabella 2", false);
            // posizione 1, 5 della tabella individuata da segnalibro "NellaTabella2"
            //wd.InsertTextInBookmarkedTable("NellaTabella2", 1, 5, "Tabella individuata con segnalibro", false);

            wd.SaveAsPdf();
            
            wd.Save();

            wd.Close();
            wd = null; 
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            //SpreadsheetControl xclCtrl = new PopulateExcel(false); 

            AbstractPopulateSpreadsheet ppe = new PopulateExcel(@"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\Modello_di_prova.xlt",
                @"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\file Excel generato.xls",
                @"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\errori.txt");

            ppe.InsertTextInCell(1, 2, 1, "punto (2,1)");
            ppe.InsertTextInCell(1, 10, 10, "punto (10, 10)");
            ppe.InsertTextInCell(1, 3, 6, "1000");
            ppe.InsertTextInCell(1, 3, 7, "1000.1");
            ppe.InsertTextInCell(1, 3, 2, "Questo dovrebbe cancellare l'altra frase");
            // foglio 2
            ppe.InsertTextInCell(2, 3, 2, "So scrivere anche nel foglio 2");
            ppe.InsertTextInCell(3, 3, 2, "Idem nel foglio 3");
            // foglio 4 (lo deve creare)
            ppe.InsertTextInCell(4, 4, 2, "So scrivere anche in un foglio che prima non c'era!");

            ppe.ReplaceSquaredParenthesisTag("[c'e' un tag]", "c'era un tag!"); // questa non rimpiazza il tag
            ppe.ReplaceSquaredParenthesisTag("[tag]", "tag sostituito!");       // questa rimpiazza il tag

            ppe.InsertTextInCell(2, 7, "A", "So scrivere anche con le coordinate da battaglia navale!"); // questo è da implementare

            ppe.Save();
            ppe.Close();
            ppe = null;
        }

        private void btnExcelPDF_Click(object sender, EventArgs e)
        {
            //SpreadsheetControl xclCtrl = new PopulateExcel(false); 

            AbstractPopulateSpreadsheet ppe = new PopulateExcel(@"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\Modello_di_prova.xlt",
                @"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\file Excel generato.xls",
                @"Z:\Dropbox\Develop\dotNet\V.S.2010\Classi_e_programmi_riusabili\UseXOffice\bin\Debug\errori.txt");

            ppe.InsertTextInCell(1, 2, 1, "punto (2,1)");
            ppe.InsertTextInCell(1, 10, 10, "punto (10, 10)");
            ppe.InsertTextInCell(1, 3, 6, "1000");
            ppe.InsertTextInCell(1, 3, 7, "1000.1");
            ppe.InsertTextInCell(1, 3, 2, "Questo dovrebbe cancellare l'altra frase");
            // foglio 2
            ppe.InsertTextInCell(2, 3, 2, "So scrivere anche nel foglio 2");
            ppe.InsertTextInCell(3, 3, 2, "Idem nel foglio 3");
            // foglio 4 (lo deve creare)
            ppe.InsertTextInCell(4, 4, 2, "So scrivere anche in un foglio che prima non c'era!");

            ppe.ReplaceSquaredParenthesisTag("[c'e' un tag]", "c'era un tag!"); // questa non rimpiazza il tag
            ppe.ReplaceSquaredParenthesisTag("[tag]", "tag sostituito!");       // questa rimpiazza il tag

            ppe.InsertTextInCell(2, 7, "A", "So scrivere anche con le coordinate da battaglia navale!"); // questo è da implementare

            ppe.SaveAsPdf();

            ppe.Save();

            ppe.Close();
            ppe = null;
        }
    }
}