--
-- File generated with SQLiteStudio v3.4.17 on sab ago 30 13:54:27 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Parameters
DROP TABLE IF EXISTS Parameters;
CREATE TABLE Parameters (IdParameters INT NOT NULL, Timestamp DATETIME, Bolus_TargetGlucose INT, Bolus_GlucoseBeforeMeal INT, Bolus_ChoToEat INT, Bolus_ChoInsulinRatioBreakfast DOUBLE, Bolus_ChoInsulinRatioLunch DOUBLE, Bolus_ChoInsulinRatioDinner DOUBLE, Bolus_TotalDailyDoseOfInsulin DOUBLE, Bolus_InsulinCorrectionSensitivity DOUBLE, Correction_TypicalBolusMorning DOUBLE, Correction_TypicalBolusMidday DOUBLE, Correction_TypicalBolusEvening DOUBLE, Correction_TypicalBolusNight DOUBLE, Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, Hypo_GlucoseTarget DOUBLE, Hypo_GlucoseLast DOUBLE, Hypo_GlucosePrevious DOUBLE, Hypo_HourLast DOUBLE, Hypo_HourPrevious DOUBLE, Hypo_MinuteLast DOUBLE, Hypo_MinutePrevious DOUBLE, Hypo_AlarmAdvanceTime DOUBLE, Hypo_FutureSpanMinutes DOUBLE, Hit_ChoAlreadyTaken DOUBLE, Hit_ChoOfFood DOUBLE, Hit_TargetCho DOUBLE, Hit_NameOfFood TEXT, FoodInMeal_ChoGrams DOUBLE, FoodInMeal_QuantityGrams DOUBLE, FoodInMeal_CarbohydratesPercent DOUBLE, FoodInMeal_Name TEXT, FoodInMeal_AccuracyOfChoEstimate DOUBLE, Meal_ChoGrams DOUBLE, Meal_Breakfast_StartTime_Hours DOUBLE, Meal_Breakfast_EndTime_Hours DOUBLE, Meal_Lunch_StartTime_Hours DOUBLE, Meal_Lunch_EndTime_Hours DOUBLE, Meal_Dinner_StartTime_Hours DOUBLE, Meal_Dinner_EndTime_Hours DOUBLE, Insulin_Short_Id INTEGER, Insulin_Long_Id INTEGER, MonthsOfDataShownInTheGrids DOUBLE, PRIMARY KEY (IdParameters));

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
