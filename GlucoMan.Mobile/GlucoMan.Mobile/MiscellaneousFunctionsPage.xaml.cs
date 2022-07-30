using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MiscellaneousFunctionsPage : ContentPage
    {
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
                try
                {
                    SharedGlucoMan.BusinessLayer.BL_General b = new SharedGlucoMan.BusinessLayer.BL_General();
                    // deleting the database file
                    b.DeleteDatabase();
                    // after deletion the software will automatically re-create the database
                }
                catch (Exception ex)
                {
                    DisplayAlert("", "Error in deleting file. File NOT deleted", "OK"); 
                }
            }
        }
        private async void btnCopyDatabase_Click(object sender, EventArgs e)
        {
            // write the SpecialFolders that are used in Android
            // !!!! comment the next loop when devepolment of this part has finished !!!!
            foreach (var folder in Enum.GetValues(typeof(Environment.SpecialFolder))) 
            {
                Console.WriteLine("{0}={1}", folder, System.Environment.GetFolderPath((Environment.SpecialFolder)folder));
            }
            string externalFile = Path.Combine(Common.PathExport, Common.DatabaseFileName); 
            if (File.Exists(externalFile))
                File.Delete(externalFile);
            File.Copy(Common.PathAndFileDatabase, externalFile);
            // !!!! TODO !!!! also copy the other files 
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
    }
}