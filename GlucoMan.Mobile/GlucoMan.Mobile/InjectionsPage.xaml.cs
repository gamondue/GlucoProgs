using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InjectionsPage : TabbedPage
    {
        private InsulinInjection CurrentInjection = new InsulinInjection();
        BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
        List<InsulinInjection> allInjections;
        public InjectionsPage()
        {
            InitializeComponent();
            
            RefreshGrid();
        }
        private void FromClassToUi()
        {
            if (CurrentInjection.IdInsulinInjection != null)
                txtIdInjection.Text = CurrentInjection.IdInsulinInjection.ToString();
            else
                txtIdInjection.Text = "";
            txtInsulinActual.Text = CurrentInjection.InsulinValue.Text;
            txtInsulinCalculated.Text = CurrentInjection.InsulinCalculated.Text;
            dtpInjectionDate.Date = ((DateTime)CurrentInjection.Timestamp.DateTime);
            dtpInjectionTime.Time = ((DateTime)CurrentInjection.Timestamp.DateTime).TimeOfDay;
        }
        private void FromUiToClass()
        {
            CurrentInjection.IdInsulinInjection = Safe.Int(txtIdInjection.Text);
            CurrentInjection.InsulinValue.Text = txtInsulinActual.Text;
            CurrentInjection.InsulinCalculated.Text = txtInsulinCalculated.Text;
            DateTime instant = new DateTime(
                dtpInjectionDate.Date.Year, dtpInjectionDate.Date.Month, dtpInjectionDate.Date.Day,
                dtpInjectionTime.Time.Hours, dtpInjectionTime.Time.Minutes, dtpInjectionTime.Time.Seconds);
            CurrentInjection.Timestamp.DateTime = instant;
        }
        private void RefreshGrid()
        {
            DateTime now = DateTime.Now;
            allInjections = bl.GetInjections(now.AddMonths(-2), now);
            gridInjections.ItemsSource = allInjections;
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtpInjectionDate.Date = now;
            dtpInjectionTime.Time = now.TimeOfDay;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (CurrentInjection.IdInsulinInjection == null)
            //{
            //    MessageBox.Show("Choose the injection to modify");
            //    return;
            //}
            FromUiToClass();
            bl.SaveOneInjection(CurrentInjection);
            RefreshGrid();
        }
        private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                //await DisplayAlert("XXXX", "YYYY", "Ok");
                return;
            }
            //make the tapped row the current injection 
            CurrentInjection = (InsulinInjection)e.SelectedItem;
            FromClassToUi();
        }
        private void btnAddInjection_Click(object sender, EventArgs e)
        {
            if (chkNowInAdd.IsChecked)
            {
                DateTime now = DateTime.Now;
                dtpInjectionDate.Date = now;
                dtpInjectionTime.Time = now.TimeOfDay;
            }
            FromUiToClass();
            // erase Id to save a new record
            CurrentInjection.IdInsulinInjection = null;
            bl.SaveOneInjection(CurrentInjection);
            RefreshGrid();
        }
        private async void btnRemoveInjection_Click(object sender, EventArgs e)
        {
            InsulinInjection inj = (InsulinInjection)gridInjections.SelectedItem; 
            if (inj != null)
            {
                bool remove = await DisplayAlert(String.Format(
                    "Should I delete the injection of {1}, insulin {0}, Id {2}?",
                    inj.InsulinValue.ToString(),
                    inj.Timestamp.ToString(), 
                    inj.IdInsulinInjection.ToString()), 
                    "", "Yes", "No");
                if (remove)
                {
                    bl.DeleteOneInjection(inj);
                    RefreshGrid();
                }
            }
            else
            {
                await DisplayAlert("Saving not possible", "Choose an injection to delete", "Ok");
                return;
            }
            RefreshGrid();
        }
    }
}