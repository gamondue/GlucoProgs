using System.Diagnostics;

namespace GlucoMan.Maui;

public partial class MiscellaneousFunctionsPage : ContentPage
{
    SharedGlucoMan.BusinessLayer.BL_General blGeneral = new SharedGlucoMan.BusinessLayer.BL_General();
    bool canModify = true;
    public MiscellaneousFunctionsPage()
    {
        InitializeComponent();
    }
    private void txt_mgPerdL_TextChanged(object sender, EventArgs e)
    {
        double value;
        double.TryParse(txt_mgPerdL.Text, out value);
        if (canModify)
        {
            canModify = false;
            txt_mmolPerL.Text = Common.mgPerdL_To_mmolPerL(value).ToString("0.00");
        }
        else
        {
            canModify = true;
        }
    }
    private void txt_mmolPerL_TextChanged(object sender, EventArgs e)
    {
        double value;
        double.TryParse(txt_mmolPerL.Text, out value);
        if (canModify)
        {
            canModify = false;
            txt_mgPerdL.Text = Common.mmolPerL_To_mgPerdL(value).ToString("0");
        }
        else
        {
            canModify = true;
        }
    }
    private async void btnResetDatabase_Click(object sender, EventArgs e)
    {
        bool remove = await DisplayAlert("Should I delete the WHOLE database? All data will be lost!",
            "", "Yes", "No");
        if (remove)
        {
            // deleting the database file
            // after deletion the software will automatically re-create the database
            if (!blGeneral.DeleteDatabase())
            {
                DisplayAlert("", "Error in deleting file. File NOT deleted", "OK");
            }
        }
    }
    private void btnCopyProgramsFiles_Click(object sender, EventArgs e)
    {
        // write the SpecialFolders that are used in Android
        // !!!! comment the next loop when devepolment of this part has finished !!!!
        //foreach (var folder in Enum.GetValues(typeof(Environment.SpecialFolder))) 
        //{
        //    Console.WriteLine("{0}={1}", folder, System.Environment.GetFolderPath((Environment.SpecialFolder)folder));
        //}
        if (!blGeneral.ExportProgramsFiles())
        {
            DisplayAlert("", "Error in exporting program's files. NOT all files copied, check logs", "OK");
        }
    }
    private async void btnImport_Click(object sender, EventArgs e)
    {
        bool import = await DisplayAlert("",
            "Please put a database named 'import.sqlite' in the same folder where this program exports its data. " +
            "\nShould we continue with the import?", "Yes", "No");
        if (import)
        {
            if (!blGeneral.ImportDatabaseFromExternal(Common.PathAndFileDatabase,
                Path.Combine(Common.PathImportExport, "import.sqlite")))
            {
                DisplayAlert("", "Error in importing form import-sqlite to app's database", "OK");
            }
        }
    }
    private async void btnStopApplication_Click(object sender, EventArgs e)
    {
        Process.GetCurrentProcess().CloseMainWindow();
        Process.GetCurrentProcess().Close();
    }
    private async void btnShowErrorLog_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            string fileContent = File.ReadAllText(Common.LogOfProgram.ErrorsFile);
            await Navigation.PushAsync(new ShowTextPage(fileContent));
        }
        catch
        {
            await DisplayAlert("Reading not possible", "File not existing or not accessible", "Ok");
        }
    }
    private async void btnDeleteErrorLog_ClickAsync(object sender, EventArgs e)
    {
        File.Delete(Common.LogOfProgram.ErrorsFile);
        await DisplayAlert("", "Done!", "Ok");
    }
    private async void btnReadDatabase_Click(object sender, EventArgs e)
    {
        bool read = await DisplayAlert("Read database from external folder",
            "Please put a database named 'readGlucoman.sqlite' in the same " +
            "folder where this program exports its data." +
            "\nAttention, this file will replace the current database." +
            "\nYou should backup it before continuing! " +
            "\nShould we continue in the process?", "Yes", "No");
        if (read)
        {
            if (!blGeneral.ReadDatabaseFromExternal(Common.PathAndFileDatabase,
                Path.Combine(Common.PathImportExport, "readGlucoman.sqlite")))
            {
                DisplayAlert("Error!", "Error in reading database from external folder", "OK");
            }
        }
    }
    private async void btnSettings_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}
