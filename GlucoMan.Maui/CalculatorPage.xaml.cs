using Microsoft.Maui.Controls;
using System;

namespace GlucoMan.Maui
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculatorPage : ContentPage
    {
        double firstNumber = 0;
        double secondNumber = 0;
        string operation = "";
        bool isNewEntry = true;
        public TaskCompletionSource<double?> ResultSource { get; private set; } = new();
        public CalculatorPage(double InitialValue)
        {
            InitializeComponent();
            DisplayLabel.Text = InitialValue.ToString();
        }
        void OnNumberClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (isNewEntry)
            {
                DisplayLabel.Text = button.Text;
                isNewEntry = false;
            }
            else
            {
                DisplayLabel.Text += button.Text;
            }
        }
        void OnOperatorClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            firstNumber = double.Parse(DisplayLabel.Text);
            operation = button.Text;
            isNewEntry = true;
        }
        void OnClearClicked(object sender, EventArgs e)
        {
            DisplayLabel.Text = "0";
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            isNewEntry = true;
        }
        void OnEqualsClicked(object sender, EventArgs e)
        {
            secondNumber = double.Parse(DisplayLabel.Text);
            double result = 0;
            switch (operation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                case "/":
                    result = secondNumber != 0 ? firstNumber / secondNumber : 0;
                    break;
            }

            DisplayLabel.Text = result.ToString();
            isNewEntry = true;

            ////////double result = double.Parse(DisplayLabel.Text);
            ////////ResultSource.TrySetResult(result);
            ////////Navigation.PopModalAsync();
        }
        private void OnClearEntryClicked(object sender, EventArgs e)
        {

        }
        private void OnBackspaceClicked(object sender, EventArgs e)
        {

        }
        private void OnDecimalClicked(object sender, EventArgs e)
        {

        }
        private void OnOkClicked(object sender, EventArgs e)
        {
            if (double.TryParse(DisplayLabel.Text, out double result))
            {
                ResultSource.TrySetResult(result);
            }
            else
            {
                ResultSource.TrySetResult(null);
            }
            // close the page going back to the calling page
            Navigation.PopModalAsync();
        }
        private void OnAbortClicked(object sender, EventArgs e)
        {
            ResultSource.TrySetResult(null);
            // close the page going back to the calling page
            Navigation.PopModalAsync();
        }
        protected override void OnDisappearing()
        {
            if (!ResultSource.Task.IsCompleted)
                ResultSource.TrySetResult(null);
            base.OnDisappearing();
        }
    }
}
