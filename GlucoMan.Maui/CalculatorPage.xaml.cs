using Microsoft.Maui.Controls;
using System;
using System.Globalization;

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
            try
            {
                InitializeComponent();
                
                // Safely format the initial value
                DisplayLabel.Text = InitialValue.ToString(CultureInfo.CurrentCulture);
                
                // localize the decimal separator
                // get the local decimal separator
                decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                
                // put the decimal separator in the button
                if (btnDecimal != null)
                    btnDecimal.Text = decimalSeparator;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - Constructor", ex);
                
                // Initialize with safe defaults
                if (DisplayLabel != null) DisplayLabel.Text = "0";
                decimalSeparator = ".";
                if (btnDecimal != null) btnDecimal.Text = decimalSeparator;
            }
        }
        
        void OnNumberClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is not Button button || DisplayLabel == null) return;
                
                if (isNewEntry)
                {
                    DisplayLabel.Text = button.Text ?? "0";
                    isNewEntry = false;
                }
                else
                {
                    DisplayLabel.Text += button.Text ?? "";
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnNumberClicked", ex);
            }
        }
        
        void OnOperatorClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is not Button button || DisplayLabel == null) return;
                
                if (double.TryParse(DisplayLabel.Text, out double parsedValue))
                {
                    firstNumber = parsedValue;
                }
                else
                {
                    firstNumber = 0;
                }
                
                operation = button.Text ?? "";
                isNewEntry = true;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnOperatorClicked", ex);
            }
        }
        
        void OnClearClicked(object sender, EventArgs e)
        {
            try
            {
                // C button clears the display and the current operation
                if (DisplayLabel != null) DisplayLabel.Text = "0";
                firstNumber = 0;
                secondNumber = 0;
                operation = "";
                isNewEntry = true;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnClearClicked", ex);
            }
        }
        
        private void OnClearEntryClicked(object sender, EventArgs e)
        {
            try
            {
                // CE button just clears the display
                if (DisplayLabel != null) DisplayLabel.Text = "0";
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnClearEntryClicked", ex);
            }
        }
        
        void OnEqualsClicked(object sender, EventArgs e)
        {
            try
            {
                if (DisplayLabel == null) return;
                
                if (!double.TryParse(DisplayLabel.Text, out secondNumber))
                {
                    secondNumber = 0;
                }
                
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
                    default:
                        result = secondNumber; // If no operation, just use the current number
                        break;
                }

                DisplayLabel.Text = result.ToString(CultureInfo.CurrentCulture);
                isNewEntry = true;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnEqualsClicked", ex);
                if (DisplayLabel != null) DisplayLabel.Text = "Error";
            }
        }
        
        private void OnBackspaceClicked(object sender, EventArgs e)
        {
            try
            {
                if (DisplayLabel == null) return;
                
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
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnBackspaceClicked", ex);
            }
        }
        
        private void OnDecimalClicked(object sender, EventArgs e)
        {
            try
            {
                if (DisplayLabel == null || string.IsNullOrEmpty(decimalSeparator)) return;
                
                // add to the display the local separator between integer and decimal
                // add the separator to the display only if it isn't already present in the display
                if (!DisplayLabel.Text.Contains(decimalSeparator))
                {
                    DisplayLabel.Text += decimalSeparator;
                    isNewEntry = false; // allows writing other digits, after the separator
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnDecimalClicked", ex);
            }
        }
        
        private async void OnOkClicked(object sender, EventArgs e)
        {
            try
            {
                // convert the result to a double
                if (DisplayLabel != null && double.TryParse(DisplayLabel.Text, out double result))
                {
                    ResultSource.TrySetResult(result);
                }
                else
                {
                    ResultSource.TrySetResult(null);
                }
                
                // close the page going back to the calling page
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnOkClicked", ex);
                ResultSource.TrySetResult(null);
                try
                {
                    await Navigation.PopModalAsync();
                }
                catch (Exception navEx)
                {
                    gamon.General.LogOfProgram?.Error("CalculatorPage - OnOkClicked Navigation", navEx);
                }
            }
        }
        
        private async void OnEscapeClicked(object sender, EventArgs e)
        {
            try
            {
                ResultSource.TrySetResult(null);
                // close the page going back to the calling page
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnEscapeClicked", ex);
                try
                {
                    await Navigation.PopModalAsync();
                }
                catch (Exception navEx)
                {
                    gamon.General.LogOfProgram?.Error("CalculatorPage - OnEscapeClicked Navigation", navEx);
                }
            }
        }
        
        protected override void OnDisappearing()
        {
            try
            {
                // when the Page is closing, set the Page's result
                if (!ResultSource.Task.IsCompleted)
                    ResultSource.TrySetResult(null);
                    
                base.OnDisappearing();
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("CalculatorPage - OnDisappearing", ex);
            }
        }
    }
}
