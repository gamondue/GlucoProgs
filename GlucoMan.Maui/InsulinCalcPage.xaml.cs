using GlucoMan.BusinessLayer;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace GlucoMan.Maui;

public partial class InsulinCalcPage : ContentPage
{
    BL_BolusesAndInjections currentBolusCalculation;
    BL_GlucoseMeasurements currentGlucoseMeasurement;
    
    public InsulinCalcPage()
    {
        InitializeComponent();

        currentBolusCalculation = new BL_BolusesAndInjections();
        currentGlucoseMeasurement = new BL_GlucoseMeasurements();

        currentBolusCalculation.RestoreBolusParameters();
        currentBolusCalculation.MealOfBolus.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();

        currentBolusCalculation.TargetGlucose.Format = "0";
        currentBolusCalculation.GlucoseBeforeMeal.Format = "0";

        FromClassToUi();
        txtGlucoseBeforeMeal.Focus();
    }
    
    private void FromClassToUi()
    {
        txtChoToEat.Text = currentBolusCalculation.ChoToEat.Text;
        txtInsulinCorrectionSensitivity.Text = currentBolusCalculation.InsulinCorrectionSensitivity.Text;
        ////txtTdd.Text = currentBolusCalculation.TotalDailyDoseOfInsulin.Text;
        txtGlucoseBeforeMeal.Text = currentBolusCalculation.GlucoseBeforeMeal.Text;
        txtCorrectionInsulin.Text = currentBolusCalculation.BolusInsulinDueToCorrectionOfGlucose.Text;
        txtChoInsulinMeal.Text = currentBolusCalculation.BolusInsulinDueToChoOfMeal.Text;
        txtTargetGlucose.Text = currentBolusCalculation.TargetGlucose.Text;
        txtEmbarkedInsulin.Text = currentBolusCalculation.EmbarkedInsulin.Text;
        txtTotalInsulinExceptEmbarked.Text = currentBolusCalculation.TotalInsulinExceptEmbarked.Text;
        txtTotalInsulin.Text = currentBolusCalculation.TotalInsulinForMeal.Text;

        txtChoInsulinRatioBreakfast.Text = currentBolusCalculation.ChoInsulinRatioBreakfast.Text;
        txtChoInsulinRatioLunch.Text = currentBolusCalculation.ChoInsulinRatioLunch.Text;
        txtChoInsulinRatioDinner.Text = currentBolusCalculation.ChoInsulinRatioDinner.Text;

        txtStatusBar.Text = currentBolusCalculation.StatusMessage;
        switch (currentBolusCalculation.MealOfBolus.IdTypeOfMeal)
        {
            case (Common.TypeOfMeal.Breakfast):
                rdbIsBreakfast.IsChecked = true;
                break;
            case (Common.TypeOfMeal.Dinner):
                rdbIsDinner.IsChecked = true;
                break;
            case (Common.TypeOfMeal.Lunch):
                rdbIsLunch.IsChecked = true;
                break;
            case (Common.TypeOfMeal.Snack):
                rdbIsSnack.IsChecked = true;
                break;
        }
    }
    
    private void FromUiToClass()
    {
        // since it is easy to mistakenly insert blanks during editing, we tear blanks off 
        // from all the Entry controls that input a number 
        currentBolusCalculation.ChoInsulinRatioDinner.Text = NoBlank(txtChoInsulinRatioDinner.Text);
        currentBolusCalculation.ChoInsulinRatioBreakfast.Text = NoBlank(txtChoInsulinRatioBreakfast.Text);
        currentBolusCalculation.ChoInsulinRatioLunch.Text = NoBlank(txtChoInsulinRatioLunch.Text);

        currentBolusCalculation.ChoToEat.Text = NoBlank(txtChoToEat.Text);

        currentBolusCalculation.GlucoseBeforeMeal.Text = NoBlank(txtGlucoseBeforeMeal.Text);
        currentBolusCalculation.TargetGlucose.Text = NoBlank(txtTargetGlucose.Text);
        currentBolusCalculation.InsulinCorrectionSensitivity.Text = NoBlank(txtInsulinCorrectionSensitivity.Text);
        currentBolusCalculation.EmbarkedInsulin.Text = txtEmbarkedInsulin.Text;
        currentBolusCalculation.TotalInsulinExceptEmbarked.Text = txtTotalInsulinExceptEmbarked.Text;

        if (rdbIsBreakfast.IsChecked)
            currentBolusCalculation.MealOfBolus.IdTypeOfMeal = Common.TypeOfMeal.Breakfast;
        if (rdbIsLunch.IsChecked)
            currentBolusCalculation.MealOfBolus.IdTypeOfMeal = Common.TypeOfMeal.Lunch;
        if (rdbIsDinner.IsChecked)
            currentBolusCalculation.MealOfBolus.IdTypeOfMeal = Common.TypeOfMeal.Dinner;
        if (rdbIsSnack.IsChecked)
            currentBolusCalculation.MealOfBolus.IdTypeOfMeal = Common.TypeOfMeal.Snack;
    }
    
    private string NoBlank(string Text)
    {
        if (Text == null) return null;
        return Text.Replace(" ", "");
    }
    
    private async void btnSetParameters_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CorrectionParametersPage());
    }
    
    protected override void OnAppearing()
    {
        // emulates Windows modal behaviour in Android 
        base.OnAppearing();
        currentBolusCalculation.RestoreBolusParameters();
        FromClassToUi();
    }
    
    private void btnBolusCalculations_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        currentBolusCalculation.CalculateBolus();
        currentBolusCalculation.SaveBolusParameters();
        currentBolusCalculation.SaveBolusLog();
        FromClassToUi();
    }
    
    private void btnRoundInsulin_Click(object sender, EventArgs e)
    {
        if (rdbIsBreakfast.IsChecked || rdbIsDinner.IsChecked || rdbIsLunch.IsChecked)
        {
            FromUiToClass();
            currentBolusCalculation.RoundInsulinToZeroDecimal();
            currentBolusCalculation.SaveBolusParameters();
            FromClassToUi();
        }
    }
    
    private void btnReadGlucose_Click(object sender, EventArgs e)
    {
        List<GlucoseRecord> list = currentGlucoseMeasurement.GetLastTwoGlucoseMeasurements();
        if (list.Count > 0)
            txtGlucoseBeforeMeal.Text = list[0].GlucoseValue.ToString();
    }
    
    private void btnSaveBolus_Click(object sender, EventArgs e)
    {
        currentBolusCalculation.SaveBolusParameters();
    }
    
    private void btnReadCho_Click(object sender, EventArgs e)
    {
        txtChoToEat.Text = currentBolusCalculation.RestoreChoToEat();
    }
    
    private void btnInjection_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new InjectionsPage(null));
    }
    
    private void btnMeasureGlucose_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new GlucoseMeasurementsPage(null));
    }

    /// <summary>
    /// Opens calculator for the focused entry field
    /// </summary>
    private async void Calculator_Click(object sender, TappedEventArgs e)
    {
        // IMPORTANT: Get the focused entry BEFORE opening the modal
        // because the focus will be lost when the modal opens
        var focusedEntry = GetFocusedEntry();
        
        // If no entry of those interested has focus, do not open the calculator page
        if (focusedEntry == null)
        {
            return;
        }
        
        string sValue = focusedEntry?.Text ?? "0";
        double dValue = double.TryParse(sValue, out var val) ? val : 0;

        var calculator = new CalculatorPage(dValue);
        await Navigation.PushModalAsync(calculator);
        var result = await calculator.ResultSource.Task;

        // Update the entry that had focus BEFORE opening the calculator
        if (result.HasValue && focusedEntry != null)
        {
            focusedEntry.Text = result.Value.ToString();
            if (result.HasValue)
            {
                if (focusedEntry == txtChoToEat)
                {
                    txtChoToEat.Text = result.Value.ToString();
                }
                //    else if (focusedEntry == txtFoodCarbohydratesPerUnit)
                //    {
                //        txtFoodCarbohydratesPerUnit.Text = result.Value.ToString();
                //    }
                //    else if (focusedEntry == txtFoodQuantityInUnits)
                //    {
                //        txtFoodQuantityInUnits.Text = result.Value.ToString();
                //    }
                //    else if (focusedEntry == txtFoodCarbohydratesGrams)
                //    {
                //        txtFoodCarbohydratesGrams.Text = result.Value.ToString();
                //    }
                //    FromClassToBoxesFoodInMeal();
                //}
            }
            //FromUiToClass();
            // restore focus to the updated entry
            //focusedEntry.Focus();
        }
    }

    /// <summary>
    /// Returns the Entry control that currently has focus
    /// </summary>
    private Entry GetFocusedEntry()
    {
        if (txtChoInsulinRatioBreakfast.IsFocused) return txtChoInsulinRatioBreakfast;
        if (txtChoInsulinRatioLunch.IsFocused) return txtChoInsulinRatioLunch;
        if (txtChoInsulinRatioDinner.IsFocused) return txtChoInsulinRatioDinner;
        if (txtInsulinCorrectionSensitivity.IsFocused) return txtInsulinCorrectionSensitivity;
        if (txtGlucoseBeforeMeal.IsFocused) return txtGlucoseBeforeMeal;
        if (txtChoToEat.IsFocused) return txtChoToEat;
        return null;
    }
}
