using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessObjects
{
    public class Parameters
    {
        public int? IdParameters { get; set; }
        public int? Bolus_TargetGlucose { get; set; }
        public int? Bolus_GlucoseBeforeMeal { get; set; }
        public double? Bolus_ChoToEat { get; set; }
        public double? Bolus_ChoInsulinRatioBreakfast { get; set; }
        public double? Bolus_ChoInsulinRatioLunch { get; set; }
        public double? Bolus_ChoInsulinRatioDinner { get; set; }
        public double? Bolus_TotalDailyDoseOfInsulin { get; set; }
        public double? Bolus_InsulinCorrectionSensitivity { get; set; }
        public double? Correction_TypicalBolusMorning { get; set; }
        public double? Correction_TypicalBolusMidday { get; set; }
        public double? Correction_TypicalBolusEvening { get; set; }
        public double? Correction_TypicalBolusNight { get; set; }
        public double? Correction_FactorOfInsulinCorrectionSensitivity { get; set; }
        public double? Hypo_GlucoseTarget { get; set; }
        public double? Hypo_GlucoseLast { get; set; }
        public double? Hypo_GlucosePrevious { get; set; }
        public double? Hypo_HourLast { get; set; }
        public double? Hypo_HourPrevious { get; set; }
        public double? Hypo_MinuteLast { get; set; }
        public double? Hypo_MinutePrevious { get; set; }
        public double? Hypo_AlarmAdvanceTime { get; set; }
        public double? Hypo_FutureSpanMinutes { get; set; }
        public double? Hit_ChoAlreadyTaken { get; set; }
        public double? Hit_ChoOfFood { get; set; }
        public double? Hit_TargetCho { get; set; }
        public string Hit_NameOfFood { get; set; }
        public double? FoodInMeal_ChoGrams { get; set; }
        public double? FoodInMeal_QuantityGrams { get; set; }
        public double? FoodInMeal_CarbohydratesPercent { get; set; }
        public string FoodInMeal_Name { get; set; }
        public double? FoodInMeal_AccuracyOfChoEstimate { get; set; }
        public double? Meal_ChoGrams { get; set; }
        
        // fields managed by the SettingsPage
        public DateTime? Timestamp { get; set; }
        public int? IdInsulinDrug_Short { get; set; }
        public int? IdInsulinDrug_Long { get; set; }
        public double? Meal_Breakfast_StartTime_Hours { get; set; }
        public double? Meal_Breakfast_EndTime_Hours { get; set; }
        public double? Meal_Lunch_StartTime_Hours { get; set; }
        public double? Meal_Lunch_EndTime_Hours { get; set; }
        public double? Meal_Dinner_StartTime_Hours { get; set; }
        public double? Meal_Dinner_EndTime_Hours { get; set; }
    }
}
