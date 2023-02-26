using GlucoMan.BusinessLayer;
using gamon;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodsPage : ContentPage
    {
        BL_MealAndFood bl = Common.MealAndFood_CommonBL;
        public Food CurrentFood { get; set; }
        bool foodIsChosen = false;
        public bool FoodIsChosen { get => foodIsChosen; }

        List<Food> allFoods;
        private FoodPage foodPage;
        private bool loading = false;

        public FoodsPage(Food Food)
        {
            InitializeComponent();
            CurrentFood = Food;
            PageLoad();
        }
        public FoodsPage(string FoodNameForSearch, string FoodDescriptionForSearch)
        {
            InitializeComponent();
            if (CurrentFood == null)
                CurrentFood = new Food();
            CurrentFood.Name = FoodNameForSearch;
            CurrentFood.Description = FoodDescriptionForSearch;
            RefreshGrid(); 
            PageLoad();
        }
        public FoodsPage(FoodInMeal FoodInMeal)
        {
            InitializeComponent();
            if (CurrentFood == null)
                CurrentFood = new Food();
            bl.FromFoodInMealToFood(FoodInMeal, CurrentFood);
            PageLoad();
        }
        private void PageLoad()
        {
            loading = true;

            foodIsChosen = false;
            txtName.Text = "";
            txtDescription.Text = "";
            CurrentFood.Name = "";
            CurrentFood.Description = ""; 
            allFoods = new List<Food>();
            // if a specific food is passed, load its persistent data from database 
            if (CurrentFood.IdFood != 0 && CurrentFood.IdFood != null)
            {
                CurrentFood = bl.GetOneFood(CurrentFood.IdFood);
            }
            // if what is passed has not and IdFood,
            // we use the data actually passed 

            // let's show the CurrentFood
            FromClassToUi();

            loading = false;
        }
        private void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                //await DisplayAlert("XXXX", "YYYY", "Ok");
                return;
            }
            loading = true;
            //make the tapped row the current food
            CurrentFood = (Food)gridFoods.SelectedItem;
            FromClassToUi();
            loading = false;
        }
        private void FromClassToUi()
        {
            txtIdFood.Text = CurrentFood.IdFood.ToString();
            txtName.Text = CurrentFood.Name;
            txtDescription.Text = CurrentFood.Description;
            txtFoodCarbohydrates.Text = CurrentFood.Cho.Text;

            //txtCalories.Text = CurrentFood.Energy.Text;
            //txtTotalFats.Text = CurrentFood.TotalFats.Text;
            //txtSaturatedFats.Text = CurrentFood.SaturatedFats.Text;
            //txtSugar.Text = CurrentFood.Sugar.Text;
            //txtFibers.Text = CurrentFood.Fibers.Text;
            //txtProteins.Text = CurrentFood.Proteins.Text;
            //txtSalt.Text = CurrentFood.Salt.Text;
            //txtPotassium.Text = CurrentFood.Potassium.Text;

            //txtCholesterol.Text = CurrentFood.Cholesterol.Text;
            //txtGlicemicIndex.Text = CurrentFood.GlycemicIndex.Text;
        }
        private void FromUiToClass()
        {
            CurrentFood.IdFood = Safe.Int(txtIdFood.Text);
            CurrentFood.Name = txtName.Text;
            CurrentFood.Description = txtDescription.Text;
            CurrentFood.Cho.Double = Safe.Double(txtFoodCarbohydrates.Text);

            //CurrentFood.Energy.Double = Safe.Double(txtCalories.Text);
            //CurrentFood.TotalFats.Double = Safe.Double(txtTotalFats.Text);
            //CurrentFood.SaturatedFats.Double = Safe.Double(txtSaturatedFats.Text);
            //CurrentFood.Sugar.Double = Safe.Double(txtSugar.Text);
            //CurrentFood.Fibers.Double = Safe.Double(txtFibers.Text);
            //CurrentFood.Proteins.Double = Safe.Double(txtProteins.Text);
            //CurrentFood.Salt.Double = Safe.Double(txtSalt.Text);
            //CurrentFood.Potassium.Double = Safe.Double(txtPotassium.Text);

            //CurrentFood.Cholesterol.Double = Safe.Double(txtCholesterol.Text);
            //CurrentFood.GlycemicIndex.Double = Safe.Double(txtGlicemicIndex.Text);
        }
        private void RefreshUi()
        {
            FromClassToUi();
            RefreshGrid();
        }
        private void RefreshGrid()
        {
            allFoods = bl.SearchFoods(CurrentFood.Name, CurrentFood.Description, 0);
            gridFoods.ItemsSource = allFoods;
        }
        private async void btnFoodDetails_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            foodPage = new FoodPage(CurrentFood);
            await Navigation.PushAsync(foodPage);
            if (foodPage.FoodIsChosen)
            {
                //bl.FromFoodToFoodInMeal(foodPage.CurrentFood, bl.FoodInMeal);
                FromClassToUi();
            }
        }
        private async void btnSaveFood_Click(object sender, EventArgs e)
        {
            if (txtIdFood.Text == "")
            {
                await DisplayAlert("Select one food from the list", "Choose a food to save", "Ok");
                return;
            }
            FromUiToClass();
            bl.SaveOneFood(CurrentFood);
            FromClassToUi();
            RefreshUi();
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            CurrentFood.IdFood = null;
            bl.SaveOneFood(CurrentFood);
            RefreshUi();
        }
        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            bl.DeleteOneFood(CurrentFood);
            RefreshUi();
        }
        //private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}
        //private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex > 0)
        //    {
        //        CurrentFood = allFoods[e.RowIndex];
        //        gridFoods.Rows[e.RowIndex].Selected = true;
        //        RefreshUi();
        //    }
        //}
        //private void gridFoods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (gridFoods.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("Choose a food to save");
        //        return;
        //    }
        //}
        //private void btnFatSecret_Click(object sender, EventArgs e)
        //{
        //    //MessageBox.Show("To be implemented yet!");
        //}
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            RefreshUi();
        }
        private void btnChoose_Click(object sender, EventArgs e)
        {
            foodIsChosen = true;
            FromUiToClass();
            bl.SaveOneFood(CurrentFood);
            this.Navigation.PopAsync();
        }
        private void btnCleanFields_Click(object sender, EventArgs e)
        {
            loading = true;
            txtIdFood.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtFoodCarbohydrates.Text = "";

            //txtCalories.Text = "";
            //txtTotalFats.Text = "";
            //txtSaturatedFats.Text = "";
            //txtSugar.Text = "";
            //txtFibers.Text = "";
            //txtProteins.Text = "";
            //txtSalt.Text = "";
            loading = false;
            FromUiToClass(); 
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CurrentFood.Name = txtName.Text;
                allFoods = bl.SearchFoods(txtName.Text, txtDescription.Text, 3);
                if (allFoods != null)
                {
                    gridFoods.ItemsSource = allFoods;
                }
            }
        }
        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                allFoods = bl.SearchFoods(txtName.Text, txtDescription.Text, 3);
                if (allFoods != null)
                {
                    gridFoods.ItemsSource = allFoods;
                }
            }
        }
        protected override async void OnAppearing()
        {
            foodIsChosen = false;
        }
    }
}