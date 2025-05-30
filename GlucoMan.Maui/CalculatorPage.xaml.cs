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
        string decimalSeparator;
        public TaskCompletionSource<double?> ResultSource { get; private set; } = new();
        public CalculatorPage(double InitialValue)
        {
            InitializeComponent();
            DisplayLabel.Text = InitialValue.ToString();
            // localize the decimal separator
            // get the local decimal separator
            decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            // put the decimal sepatator in the button
            btnDecimal.Text = decimalSeparator;
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
            // C button clears the display and the current operation
            DisplayLabel.Text = "0";
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            isNewEntry = true;
        }
        private void OnClearEntryClicked(object sender, EventArgs e)
        {
            // CE button just clears the display
            DisplayLabel.Text = "0";
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
        }
        private void OnBackspaceClicked(object sender, EventArgs e)
        {
            // delete the last character of the display
            if (!string.IsNullOrEmpty(DisplayLabel.Text) && DisplayLabel.Text.Length > 1)
            {
                // delete last character
                DisplayLabel.Text = DisplayLabel.Text.Substring(0, DisplayLabel.Text.Length - 1);
            }
            else
            {
                // if there is just one character, set the display to "0"
                DisplayLabel.Text = "0";
                isNewEntry = true;
            }
        }
        private void OnDecimalClicked(object sender, EventArgs e)
        {
            // add to the display the local separator between integer and decimal

            // add the separator to the display omky if it isn't already present in the display
            if (!DisplayLabel.Text.Contains(decimalSeparator))
            {
                DisplayLabel.Text += decimalSeparator;
                isNewEntry = false; // allows writing other digits, after the separator
            }
        }
        private void OnOkClicked(object sender, EventArgs e)
        {
            // convert the result to a double
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
        private void OnEscapeClicked(object sender, EventArgs e)
        {
            ResultSource.TrySetResult(null);
            // close the page going back to the calling page
            Navigation.PopModalAsync();
        }
        protected override void OnDisappearing()
        {
            // when the Page is closing, set the Page's result
            if (!ResultSource.Task.IsCompleted)
                ResultSource.TrySetResult(null);
            base.OnDisappearing();
        }
    }
}
