using Comuni.XOffice;
using System.IO;
using System.Windows;
using NetOffice.ExcelApi;
using NetOffice.ExcelApi.Enums;
using System;
using GlucoMan;
using System.Diagnostics;
using SharedData;

namespace FoglioGlicemiaTerapia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //ExcelSpreadsheet sheetDestinazione;
        //AbstractPopulateSpreadsheet sheetDati;
        private PopulateExcel sheetInputDati;
        private PopulateExcel sheetOutputGlicemiaEInsulina;
        private PopulateExcel sheetOutputCalcoloCarboidrati; 

        private string paziente = "Gabriele Monti";

        public MainWindow()
        {
            InitializeComponent();

            CommonData.CommonObj = new CommonObjects();
            CommonData.CommonObj.LogOfProgram = new Logger(CommonData.PathProgramsData, true, "FoglioGlicemiaTerapia_log.txt",
                CommonData.PathProgramsData, CommonData.PathProgramsData, null, null);
        }

        private void BtnGeneraFile_Click(object sender, RoutedEventArgs e)
        {
            if (DtpMeseODataIniziale.SelectedDate == null)
            {
                MessageBox.Show("Scegliere il mese");
                return;
            }
            if (!File.Exists(TxtPathDeiFile.Text + TxtFileDati.Text))
            {
                MessageBox.Show("Il file " + TxtPathDeiFile.Text + TxtFileDati.Text + " non esiste!");
                return;
            }
            if (!File.Exists(TxtPathDeiFile.Text + TxtFileModello.Text))
            {
                MessageBox.Show("Il file " + TxtPathDeiFile.Text + TxtFileModello.Text + " non esiste!");
                return;
            }
            int mese = DtpMeseODataIniziale.SelectedDate.Value.Month;
            int anno = DtpMeseODataIniziale.SelectedDate.Value.Year;

            DateTime dataInizio = new DateTime(anno, mese, 1);
            DateTime dataFine = dataInizio.AddMonths(1);

            string nomeFileDestinazione = dataInizio.ToString("yyyy-MM") + "_Glicemia ed insulina_" + paziente + ".xlsx";

            sheetInputDati = new PopulateExcel(TxtPathDeiFile.Text + TxtFileDati.Text, false);
            
            sheetOutputGlicemiaEInsulina = new PopulateExcel(TxtPathDeiFile.Text + TxtFileModello.Text,
                TxtPathDeiFile.Text + nomeFileDestinazione, false);
            ////sheetOutputCalcoloCarboidrati = new PopulateExcel(TxtPathDeiFile.Text + "Calcolo carboidrati_Test.xlsx",
            ////    DtpMeseODataIniziale.SelectedDate.Value.ToString() + "_Calcolo-carboidrati_Gabriele-Monti",
            ////    false);
            //ApriExcel(TxtPathDeiFile.Text + TxtFileModello.Text, sheetDestinazione);
            //ApriExcel(TxtPathDeiFile.Text + TxtFileDati, sheetDati);
            int rigaPrimoGiornoDelMeseInFileGlicemiaEInsulina = 4;
            int rigaInizioGiornoInFileCalcoloCarboidrati = 5;

            int rigaFileGlicemiaEInsulina = rigaPrimoGiornoDelMeseInFileGlicemiaEInsulina;
            int rigaDati = 5, colonnaDati = 1;
            int colonnaFileGlicemiaEInsulina; 

            // scansiona tutte le righe del file dati
            string contenuto, scritta;
            do
            {   // legge la prima colonna
                contenuto = sheetInputDati.ReadCell(1, rigaDati, 1);
                try
                {
                    DateTime istante = Convert.ToDateTime(contenuto);
                    // guarda se la data non è nulla on non è nel mese che interessa
                    if (istante != new DateTime(1, 1, 1) && (istante >= dataInizio && istante < dataFine))
                    {
                        // stabilisce la riga del foglio di destinazione a partire dalla data del foglio di origine 
                        rigaFileGlicemiaEInsulina = istante.Day + rigaPrimoGiornoDelMeseInFileGlicemiaEInsulina - 1;

                        // Meal Insulin (units) letto da colonna 3
                        int? valore = SafeInt(sheetInputDati.ReadCell(1, rigaDati, 3));
                        if (valore != null && valore != 0)
                        {
                            colonnaFileGlicemiaEInsulina = 3 + InsulinaColonnaOrario(istante);
                            // foglio del file risultato 
                            sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
                                   colonnaFileGlicemiaEInsulina, valore.ToString());
                        }
                        // Long Acting Insulin (units) letto da   colonna 4
                        valore = SafeInt(sheetInputDati.ReadCell(1, rigaDati, 4));
                        if (valore != null && valore != 0)
                        {
                            colonnaFileGlicemiaEInsulina = 3 + InsulinaColonnaOrario(istante);
                            sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
                                colonnaFileGlicemiaEInsulina, valore.ToString());
                        }
                        // Glucose (mg/dL) colonna 5
                        valore = SafeInt(sheetInputDati.ReadCell(1, rigaDati, 5));
                        if (valore != null && valore != 0)
                        {
                            colonnaFileGlicemiaEInsulina = 2 + InsulinaColonnaOrario(istante);
                            sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
                                colonnaFileGlicemiaEInsulina, valore.ToString());
                        }
                        // Note colonna 
                        scritta = SafeString(sheetInputDati.ReadCell(1, rigaDati, 11));
                        if (scritta != null && scritta != "")
                        {
                            colonnaFileGlicemiaEInsulina = 13;
                            contenuto = sheetOutputGlicemiaEInsulina.ReadCell(1, rigaFileGlicemiaEInsulina, colonnaFileGlicemiaEInsulina);
                            if (contenuto == "")
                                contenuto = scritta;
                            else
                                contenuto = contenuto + " | " + scritta; 
                            sheetOutputGlicemiaEInsulina.InsertTextInCell(1, rigaFileGlicemiaEInsulina,
                                colonnaFileGlicemiaEInsulina, contenuto);
                        }
                    }
                }
                catch
                {
                    // se non era una data non fa niente 
                }
                rigaDati++;
            } while (contenuto != "" && contenuto != null);

            sheetOutputGlicemiaEInsulina.ReplaceSquaredParenthesisTag("Paziente", paziente);
            sheetOutputGlicemiaEInsulina.ReplaceSquaredParenthesisTag("MeseEAnno", dataInizio.ToString("MMMM yyyy"));
            sheetOutputGlicemiaEInsulina.ReplaceSquaredParenthesisTag("TipoDiTerapia", "Lispro + Abasaglar");

            sheetOutputGlicemiaEInsulina.Save();
            sheetInputDati.Close();
            sheetOutputGlicemiaEInsulina.Close();
            MessageBox.Show("Fatto!"); 
            //sheetDati.Dispose(); 
        }

        private int InsulinaColonnaOrario(DateTime istante)
        {
            if (istante.Hour >= 7 && istante.Hour < 10)
                return 0; // colazione
            if (istante.Hour >= 10 && istante.Hour < 12)
                return 2;  // due ore dopo colazione
            if (istante.Hour >= 12 && istante.Hour < 15)
                return 3;  // pranzo
            if (istante.Hour >= 15 && istante.Hour < 18)
                return 5;  // due ore dopo pranzo
            if (istante.Hour >= 18 && istante.Hour < 21)
                return 6;  // cena
            if (istante.Hour >= 21 && istante.Hour < 22)
                return 8;  // cena
            if (istante.Hour >= 22 && istante.Hour < 24 || istante.Hour >= 0 && istante.Hour < 2)
                return 9;  // notte
            return 11; // se non sta negli orari per cui ho un posto, scrive nelle note
        }

        internal Nullable<DateTime> SafeDateTime(object d)
        {
            try
            {
                return Convert.ToDateTime(d);
            }
            catch
            {
                return null;
            }
        }

        internal Nullable<int> SafeInt(object d)
        {
            try
            {
                return Convert.ToInt32 (d);
            }
            catch
            {
                return null;
            }
        }

        internal string SafeString(object d)
        {
            try
            {
                return Convert.ToString(d);
            }
            catch
            {
                return null;
            }
        }

        private bool ApriExcel(string PathEFile, AbstractPopulateSpreadsheet sheetDaAprire)
        {
            int i, j;
            int rigaExcel;

            if (File.Exists(PathEFile))
            {
                sheetDaAprire = new 
                    
                PopulateExcel(PathEFile, true);
            }
            else
            {
                MessageBox.Show("Scegliere un file Excel da popolare");
            }
            ////////NetOffice.ExcelApi.Application.UseWaitCursor = false; // !!!!
            return (sheetDaAprire == null);
        }
        private void PopolaRigaExcel()
        {
            //if (sheetDestinazione == null)
            //{
            //    ApriExcel();
            //}
            int riga, colonna, foglio;
            try
            {
                riga = sheetOutputGlicemiaEInsulina.CurrentRow();
                //colonna = tab.CurrentColumn();
                foglio = sheetOutputGlicemiaEInsulina.CurrentSheet();
            }
            catch
            {
                MessageBox.Show("Cliccare su una cella della riga da popolare");
                return;
            }
            //tab.InsertTextInCell(foglio, riga, colonna, "prova"); 

            if (riga == -1 || foglio == -1)
            {
                MessageBox.Show("Non trovo il file excel. Riaprilo.");
                return;
            }
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 3, DatiZ["Pmis"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 5, DatiZ["Tmis"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 7, DatiZ["Pbase"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 8, DatiZ["Tbase"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 9, DatiZ["Zbase"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 10, DatiZ["Z"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 12, DatiZ["CRead"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 13, DatiZ["PRead"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 14, DatiZ["TRead"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 15, DatiZ["ZRead"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 16, DatiZ["VRead"]);
            //sheetDestinazione.InsertDoubleInCell(foglio, riga, 17, DatiZ["VcRead"]);

            sheetOutputGlicemiaEInsulina.Save();

            sheetOutputGlicemiaEInsulina.SetCurrentSheet(foglio);

            ////////NetOffice.ExcelApi.Application.UseWaitCursor = false; // !!!!
        }

        private void TxtPathDeiFile_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void TxtPathDeiFile_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // !!!! TODO !!!! Far Partire il file explorer con la cartella che c'è nella textbox
            ////Process proc = new Process();
            ////proc.StartInfo.FileName = TxtPathDeiFile.Text;
            ////proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(TxtPathDeiFile.Text);
            ////proc.Start();
            //System.Diagnostics.Process.Start(TxtPathDeiFile.Text);
        }

        private void GeneraFileDieta_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
