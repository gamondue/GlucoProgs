using gamon;
using GlucoMan.BusinessLayer;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.IO;

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
        bool import = await DisplayAlert(
            "",
            "Select the SQLite database file to import.\n" +
            "ATTENTION: the selected file may REPLACE the current app database.\n" +
            "You should backup the current database before continuing!\n\nContinue?",
            "Yes", "No");

        if (!import)
            return;

        // Usa SAF per scegliere il file .sqlite/.db
        var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "application/x-sqlite3", "application/octet-stream", ".sqlite", ".db" } },
            { DevicePlatform.iOS, new[] { "public.data", ".sqlite", ".db" } },
            { DevicePlatform.WinUI, new[] { ".sqlite", ".db" } },
            { DevicePlatform.MacCatalyst, new[] { ".sqlite", ".db" } }
        });

        var picked = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select GlucoMan database (.sqlite/.db)",
            FileTypes = customFileType
        });

        if (picked is null)
            return;

        // Copia il file scelto nello storage interno dell’app
        string tempImportName = "import.sqlite";
        string tempImportPath = Path.Combine(FileSystem.AppDataDirectory, tempImportName);

        try
        {
            Directory.CreateDirectory(FileSystem.AppDataDirectory);
            using var src = await picked.OpenReadAsync(); // usare lo stream (non il path esterno)
            using var dst = File.Open(tempImportPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await src.CopyToAsync(dst);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error!", $"Error copying selected file: {ex.Message}", "OK");
            return;
        }

        // Importa dal file interno
        if (!blGeneral.ImportDatabaseFromExternal(Common.PathAndFileDatabase, tempImportPath))
        {
            await DisplayAlert("", "Error in importing from selected file to app's database", "OK");
        }
        else
        {
            await DisplayAlert("", "Import completed successfully.", "OK");
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
        bool read = await DisplayAlert("Read database from external storage",
            "Select the SQLite database file to import.\n" +
            "ATTENTION: the selected file will REPLACE the current app database.\n" +
            "You should backup the current database before continuing!\n\nContinue?",
            "Yes", "No");
        if (!read)
            return;

        // Usa SAF: l'utente sceglie il file e noi leggiamo lo stream
        var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "application/x-sqlite3", "application/octet-stream", ".sqlite", ".db" } },
            { DevicePlatform.iOS, new[] { "public.data", ".sqlite", ".db" } },
            { DevicePlatform.WinUI, new[] { ".sqlite", ".db" } },
            { DevicePlatform.MacCatalyst, new[] { ".sqlite", ".db" } }
        });

        var picked = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select GlucoMan database (.sqlite/.db)",
            FileTypes = customFileType
        });
        if (picked is null)
            return;

        // Copia il file scelto nello storage interno dell'app e passa quel path alla BL
        string tempImportName = "readGlucoManData.sqlite";
        string tempImportPath = Path.Combine(FileSystem.AppDataDirectory, tempImportName);

        try
        {
            Directory.CreateDirectory(FileSystem.AppDataDirectory);
            using var src = await picked.OpenReadAsync();
            using var dst = File.Open(tempImportPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await src.CopyToAsync(dst);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error!", $"Error copying selected file: {ex.Message}", "OK");
            return;
        }

        bool success = await blGeneral.ReadDatabaseFromExternal(Common.PathAndFileDatabase, tempImportPath);
        if (!success)
        {
            await DisplayAlert("Error!", "Error importing database from selected file", "OK");
            return;
        }
        else
        {
            await DisplayAlert("Done!", "Database import completed successfully.", "OK");
            await DisplayAlert("", "Program will now stop. Restart it manually.", "OK");
            btnStopApplication_Click(null, null);
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
