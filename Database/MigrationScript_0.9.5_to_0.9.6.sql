-- =====================================================================
-- Migration Script: OLD Database to CURRENT Database
-- =====================================================================
-- This script migrates an OLD GlucoMan database schema to the CURRENT version
-- preserving all existing data where possible.
--
-- CRITICAL: Backup your database before running this script!
-- =====================================================================

PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- =====================================================================
-- TABLE: Foods
-- Added columns: IsRaw, RawCookedRatio
-- =====================================================================
ALTER TABLE Foods ADD COLUMN IsRaw TINYINT;
ALTER TABLE Foods ADD COLUMN RawCookedRatio DOUBLE;

-- =====================================================================
-- TABLE: Parameters  
-- This table has significant changes in the Weigh_* parameters
-- OLD columns to be dropped:
--   Weigh_RawGross, Weigh_RawTare, Weigh_RawNet
--   Weigh_CookedGross, Weigh_CookedTare, Weigh_CookedNet
--   Weigh_CookedPortionGross, Weigh_CookedPortionTare, Weigh_CookedPortionNet
--   Weigh_NPortions
--
-- NEW columns to be added:
--   Weigh_FoodId, Weigh_FoodName, Weigh_FoodCarbohydratesPercent
--   Weigh_TotalCarbohydratesPercent
--   Weigh_RawGross, Weigh_RawTare, Weigh_RawNet (redefined)
--   Weigh_CookedGross, Weigh_CookedTare, Weigh_CookedNet (redefined)
--   Weigh_SeasoningGross, Weigh_SeasoningTare, Weigh_SeasoningNet
--   Weigh_SeasoningCarbohydratesPercent
--   Weigh_PortionGross, Weigh_PortionTare, Weigh_PortionNet (renamed from CookedPortion*)
--   Weigh_NPortions (redefined)
--   Weigh_DoWeighCookedPortion, Weigh_IsChoOfRawFood
--   Weigh_RawCookedRatio
--   Weigh_WeightOfPortion, Weigh_CarbohydratesOfPortion
-- =====================================================================

-- Strategy: Create new table with updated schema, copy data, rename tables
-- This is safer than multiple ALTER TABLE operations in SQLite

-- Create backup of current Parameters table
CREATE TABLE Parameters_OLD AS SELECT * FROM Parameters;

-- Drop the old Parameters table
DROP TABLE Parameters;

-- Create the new Parameters table with CURRENT schema
CREATE TABLE Parameters (
    IdParameters INT NOT NULL, 
    Timestamp DATETIME, 
    Bolus_TargetGlucose INT, 
    Bolus_GlucoseBeforeMeal INT, 
    Bolus_ChoToEat INT, 
    Bolus_ChoInsulinRatioBreakfast DOUBLE, 
    Bolus_ChoInsulinRatioLunch DOUBLE, 
    Bolus_ChoInsulinRatioDinner DOUBLE, 
    Bolus_TotalDailyDoseOfInsulin DOUBLE, 
    Bolus_InsulinCorrectionSensitivity DOUBLE, 
    Correction_TypicalBolusMorning DOUBLE, 
    Correction_TypicalBolusMidday DOUBLE, 
    Correction_TypicalBolusEvening DOUBLE, 
    Correction_TypicalBolusNight DOUBLE, 
    Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, 
    Hypo_GlucoseTarget DOUBLE, 
    Hypo_GlucoseLast DOUBLE, 
    Hypo_GlucosePrevious DOUBLE, 
    Hypo_HourLast DOUBLE, 
    Hypo_HourPrevious DOUBLE, 
    Hypo_MinuteLast DOUBLE, 
    Hypo_MinutePrevious DOUBLE, 
    Hypo_AlarmAdvanceTime DOUBLE, 
    Hypo_FutureSpanMinutes DOUBLE, 
    Hit_ChoAlreadyTaken DOUBLE, 
    Hit_ChoOfFood DOUBLE, 
    Hit_TargetCho DOUBLE, 
    Hit_NameOfFood TEXT, 
    FoodInMeal_ChoGrams DOUBLE, 
    FoodInMeal_QuantityGrams DOUBLE, 
    FoodInMeal_CarbohydratesPercent DOUBLE, 
    FoodInMeal_Name TEXT, 
    FoodInMeal_AccuracyOfChoEstimate DOUBLE, 
    Meal_ChoGrams DOUBLE, 
    Meal_Breakfast_StartTime_Hours DOUBLE, 
    Meal_Breakfast_EndTime_Hours DOUBLE, 
    Meal_Lunch_StartTime_Hours DOUBLE, 
    Meal_Lunch_EndTime_Hours DOUBLE, 
    Meal_Dinner_StartTime_Hours DOUBLE, 
    Meal_Dinner_EndTime_Hours DOUBLE, 
    Insulin_Short_Id INTEGER, 
    Insulin_Long_Id INTEGER, 
    MonthsOfDataShownInTheGrids DOUBLE, 
    -- NEW Weigh parameters
    Weigh_FoodId TEXT, 
    Weigh_FoodName TEXT, 
    Weigh_FoodCarbohydratesPercent DOUBLE, 
    Weigh_TotalCarbohydratesPercent DOUBLE, 
    Weigh_RawGross DOUBLE, 
    Weigh_RawTare DOUBLE, 
    Weigh_RawNet DOUBLE, 
    Weigh_CookedGross DOUBLE, 
    Weigh_CookedTare DOUBLE, 
    Weigh_CookedNet DOUBLE, 
    Weigh_SeasoningGross DOUBLE, 
    Weigh_SeasoningTare DOUBLE, 
    Weigh_SeasoningNet DOUBLE, 
    Weigh_SeasoningCarbohydratesPercent DOUBLE, 
    Weigh_PortionGross DOUBLE, 
    Weigh_PortionTare DOUBLE, 
    Weigh_PortionNet DOUBLE, 
    Weigh_NPortions DOUBLE, 
    Weigh_DoWeighCookedPortion TEXT, 
    Weigh_IsChoOfRawFood TEXT, 
    Weigh_RawCookedRatio DOUBLE, 
    Weigh_WeightOfPortion DOUBLE, 
    Weigh_CarbohydratesOfPortion DOUBLE, 
    PRIMARY KEY (IdParameters)
);

-- Migrate data from old table to new table
-- Map old Weigh_CookedPortion* fields to new Weigh_Portion* fields
INSERT INTO Parameters (
    IdParameters, 
    Timestamp, 
    Bolus_TargetGlucose, 
    Bolus_GlucoseBeforeMeal, 
    Bolus_ChoToEat, 
    Bolus_ChoInsulinRatioBreakfast, 
    Bolus_ChoInsulinRatioLunch, 
    Bolus_ChoInsulinRatioDinner, 
    Bolus_TotalDailyDoseOfInsulin, 
    Bolus_InsulinCorrectionSensitivity, 
    Correction_TypicalBolusMorning, 
    Correction_TypicalBolusMidday, 
    Correction_TypicalBolusEvening, 
    Correction_TypicalBolusNight, 
    Correction_FactorOfInsulinCorrectionSensitivity, 
    Hypo_GlucoseTarget, 
    Hypo_GlucoseLast, 
    Hypo_GlucosePrevious, 
    Hypo_HourLast, 
    Hypo_HourPrevious, 
    Hypo_MinuteLast, 
    Hypo_MinutePrevious, 
    Hypo_AlarmAdvanceTime, 
    Hypo_FutureSpanMinutes, 
    Hit_ChoAlreadyTaken, 
    Hit_ChoOfFood, 
    Hit_TargetCho, 
    Hit_NameOfFood, 
    FoodInMeal_ChoGrams, 
    FoodInMeal_QuantityGrams, 
    FoodInMeal_CarbohydratesPercent, 
    FoodInMeal_Name, 
    FoodInMeal_AccuracyOfChoEstimate, 
    Meal_ChoGrams, 
    Meal_Breakfast_StartTime_Hours, 
    Meal_Breakfast_EndTime_Hours, 
    Meal_Lunch_StartTime_Hours, 
    Meal_Lunch_EndTime_Hours, 
    Meal_Dinner_StartTime_Hours, 
    Meal_Dinner_EndTime_Hours, 
    Insulin_Short_Id, 
    Insulin_Long_Id, 
    MonthsOfDataShownInTheGrids,
    -- Map old Weigh columns to new schema
    Weigh_RawGross, 
    Weigh_RawTare, 
    Weigh_RawNet, 
    Weigh_CookedGross, 
    Weigh_CookedTare, 
    Weigh_CookedNet, 
    Weigh_PortionGross,      -- Mapped from Weigh_CookedPortionGross
    Weigh_PortionTare,       -- Mapped from Weigh_CookedPortionTare
    Weigh_PortionNet,        -- Mapped from Weigh_CookedPortionNet
    Weigh_NPortions
)
SELECT 
    IdParameters, 
    Timestamp, 
    Bolus_TargetGlucose, 
    Bolus_GlucoseBeforeMeal, 
    Bolus_ChoToEat, 
    Bolus_ChoInsulinRatioBreakfast, 
    Bolus_ChoInsulinRatioLunch, 
    Bolus_ChoInsulinRatioDinner, 
    Bolus_TotalDailyDoseOfInsulin, 
    Bolus_InsulinCorrectionSensitivity, 
    Correction_TypicalBolusMorning, 
    Correction_TypicalBolusMidday, 
    Correction_TypicalBolusEvening, 
    Correction_TypicalBolusNight, 
    Correction_FactorOfInsulinCorrectionSensitivity, 
    Hypo_GlucoseTarget, 
    Hypo_GlucoseLast, 
    Hypo_GlucosePrevious, 
    Hypo_HourLast, 
    Hypo_HourPrevious, 
    Hypo_MinuteLast, 
    Hypo_MinutePrevious, 
    Hypo_AlarmAdvanceTime, 
    Hypo_FutureSpanMinutes, 
    Hit_ChoAlreadyTaken, 
    Hit_ChoOfFood, 
    Hit_TargetCho, 
    Hit_NameOfFood, 
    FoodInMeal_ChoGrams, 
    FoodInMeal_QuantityGrams, 
    FoodInMeal_CarbohydratesPercent, 
    FoodInMeal_Name, 
    FoodInMeal_AccuracyOfChoEstimate, 
    Meal_ChoGrams, 
    Meal_Breakfast_StartTime_Hours, 
    Meal_Breakfast_EndTime_Hours, 
    Meal_Lunch_StartTime_Hours, 
    Meal_Lunch_EndTime_Hours, 
    Meal_Dinner_StartTime_Hours, 
    Meal_Dinner_EndTime_Hours, 
    Insulin_Short_Id, 
    Insulin_Long_Id, 
    MonthsOfDataShownInTheGrids,
    -- Map old Weigh columns
    Weigh_RawGross, 
    Weigh_RawTare, 
    Weigh_RawNet, 
    Weigh_CookedGross, 
    Weigh_CookedTare, 
    Weigh_CookedNet, 
    Weigh_CookedPortionGross,  -- Old column name
    Weigh_CookedPortionTare,   -- Old column name
    Weigh_CookedPortionNet,    -- Old column name
    Weigh_NPortions
FROM Parameters_OLD;

-- Optional: Keep the backup table for safety
-- To delete the backup after verifying migration: DROP TABLE Parameters_OLD;

-- =====================================================================
-- TABLE: PositionsOfReferences
-- Timestamps updated from '2025-08-31' to '2025-09-01' for some records
-- Zone values changed for some records (from 1 to 4, from 4 to 3)
-- 
-- NOTE: These are reference position data. Since the OLD database likely
--       contains the user's actual positions, we keep the OLD data.
--       The CURRENT script inserts are just default reference values.
-- =====================================================================
-- No changes needed - keep existing user data

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;

-- =====================================================================
-- VERIFICATION QUERIES
-- Run these after migration to verify data integrity:
-- =====================================================================
-- SELECT COUNT(*) FROM Foods WHERE IsRaw IS NOT NULL;
-- SELECT COUNT(*) FROM Parameters;
-- SELECT * FROM Parameters WHERE IdParameters = 1;
-- SELECT COUNT(*) FROM PositionsOfReferences;

-- =====================================================================
-- POST-MIGRATION NOTES:
-- =====================================================================
-- 1. The new Weigh_* parameters are initialized to NULL for existing records
--    Users will populate these fields when they use the WeighFood feature
--
-- 2. Foods.IsRaw and Foods.RawCookedRatio are NULL for existing foods
--    Users can update these values as needed through the Food management UI
--
-- 3. Parameters_OLD table is kept for safety - can be dropped after verification:
--    DROP TABLE IF EXISTS Parameters_OLD;
--
-- 4. Test the application thoroughly before deleting Parameters_OLD backup
-- =====================================================================
