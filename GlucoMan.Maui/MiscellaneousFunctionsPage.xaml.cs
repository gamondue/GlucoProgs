using gamon;
using GlucoMan.BusinessLayer;
using System.Diagnostics;

namespace GlucoMan.Maui;

public partial class MiscellaneousFunctionsPage : ContentPage
{
    GlucoMan.BusinessLayer.BL_General blGeneral = new GlucoMan.BusinessLayer.BL_General();
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
        bool remove = await DisplayAlert("Should I delete the WHOLE database, LOSING ALL DATA" +
            "\nAfter creation of the new database the program will shut down."
            ,"", "Yes", "No");
        if (remove)
        {
            // deleting the database file
            // after deletion the software will automatically re-create the database
            if (!blGeneral.DeleteDatabase())
            {
                DisplayAlert("", "Error in deleting database file. File NOT deleted", "OK");
            }
            blGeneral.CreateNewDatabase(); // re-create the database
            // close program
            btnStopApplication_Click(null, null);
        }
    }
    private async void btnCopyProgramsFiles_Click(object sender, EventArgs e)
    {
        // write the SpecialFolders that are used
        // !!!! comment the next loop when development of this part has finished !!!!
        //foreach (var folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
        //{
        //    Debug.WriteLine("{0}={1}", folder, System.Environment.GetFolderPath((Environment.SpecialFolder)folder));
        //}
#if ANDROID
        if (!await AndroidExternalFilesHelper.ProgramHasPermissions())
        {
            DisplayAlert("", "Insufficient permissions to write file", "OK");
            return;
        }       
#endif
        if (!await blGeneral.ExportProgramsFilesAsync())
        {
            await DisplayAlert("", "Error in exporting program's files. NOT all files copied, check logs", "OK");
        }
        else
        {
            await DisplayAlert("", "Done", "OK");
        }
    }
    private async void btnImport_Click(object sender, EventArgs e)
    {
        bool import = await DisplayAlert("",
            "Please put a database named 'import.sqlite' in folder " + 
            Common.PathImportExport +
            "\n(the same folder where this program exports its data). " +
            "\nShould we continue with the import?", "Yes", "No");
#if ANDROID
        if (!await AndroidExternalFilesHelper.ProgramHasPermissions())
        {
            DisplayAlert("", "Insufficient permissions to write file", "OK");
            return;
        }       
#endif
        if (import)
        {
            if (!blGeneral.ImportDatabaseFromExternal(Common.PathAndFileDatabase,
                Path.Combine(Common.PathImportExport, "import.sqlite")))
            {
                DisplayAlert("", "Error in importing from import.sqlite to app's database", "OK");
            }
        }
    }
    private async void btnStopApplication_Click(object sender, EventArgs e)
    {
        // stops the application shutting all its processes
        ////Application.Current.MainPage = new AppPage();
        Application.Current.Quit();
        // Stops the application shutting all its processes
        Process.GetCurrentProcess().CloseMainWindow();
        Process.GetCurrentProcess().Close();
    }
    private async void btnShowErrorLog_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            string fileContent = File.ReadAllText(General.LogOfProgram.ErrorsFile);
            await Navigation.PushAsync(new ShowTextPage(fileContent));
        }
        catch
        {
            await DisplayAlert("Reading not possible", "File not existing or not accessible", "Ok");
        }
    }
    private async void btnDeleteErrorLog_ClickAsync(object sender, EventArgs e)
    {
        General.LogOfProgram.EraseContentOfAllLogs();
        await DisplayAlert("", "Done!", "Ok");
    }
    private async void btnReadDatabase_Click(object sender, EventArgs e)
    {
        bool read = await DisplayAlert("Read database from external folder",
            "Please put a database named 'readGlucomanData.sqlite' in the folder\n" +
            Common.PathImportExport +
            "\n(the same folder where this program exports its data). " +
            "\n" +
            "\nATTENTION, this file will replace the current database." +
            "\nYou should backup the current before continuing! " +
            "\nShould we continue in the process?", "Yes", "No");
        if (read)
        {
            bool success = await blGeneral.ReadDatabaseFromExternal(Common.PathAndFileDatabase,
                Path.Combine(Common.PathImportExport, "readGlucoManData.Sqlite"));
            if (!success)
            {
                await DisplayAlert("Error!", "Error in reading database from external folder", "OK");
                return;
            }
            else
            {
                await DisplayAlert("Done!", "The import of database from external folder is finished", "OK");
            }
//#if ANDROID
            await DisplayAlert("", "Program will now stop. Restart it manually.", "OK");
            btnStopApplication_Click(null, null);
//#endif
        }
    }
    private async void btnSettings_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
    private void btnMenu_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}
