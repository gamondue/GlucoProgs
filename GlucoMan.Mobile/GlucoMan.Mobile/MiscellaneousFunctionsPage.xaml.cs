using System;
using System.Collections.Generic;
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
                SharedGlucoMan.BusinessLayer.BL_General b = new SharedGlucoMan.BusinessLayer.BL_General();
                b.PurgeDatabase();
            }
        }
    }
}