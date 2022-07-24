using GlucoMan.BusinessLayer;
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
    public partial class FoodPage : ContentPage
    {
        BL_MealAndFood bl = new BL_MealAndFood();
        public FoodPage(Food currentFood)
        {
            InitializeComponent();
            FoodIsChosen = false; 
        }

        public bool FoodIsChosen { get; internal set; }
        public Food CurrentFood { get; private set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FoodIsChosen = true;
            //FromUiToClass();
            bl.SaveOneFood(CurrentFood);
            this.Navigation.PopAsync();
        }
    }
}