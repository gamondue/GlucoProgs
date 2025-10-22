using gamon;
using System.Data.Common;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        // creationScript must have DROP TABLE statements
        string creationScript = @"
--
-- File generated with SQLiteStudio v3.4.17 on lun set 1 15:44:18 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Alarms
DROP TABLE IF EXISTS Alarms;
CREATE TABLE Alarms (IdAlarm INT NOT NULL, ReminderText TEXT, TimeStart DATETIME, NextTriggerTime DATETIME, IsDisabled TINYINT, ValidTimeAfterStart INTEGER, RingingState TINYINT, Duration INTEGER, RepetitionTime INTEGER, Interval INTEGER, IsPlaying TINYINT, EnablePlaySoundFile TINYINT, SoundFilePath TEXT, RepeatCount INTEGER, MaxRepeatCount INTEGER, LastTriggerTime DATETIME, TriggeredCount INTEGER, DoVibrate TINYINT, PRIMARY KEY (IdAlarm));

-- Table: BolusCalculations
DROP TABLE IF EXISTS BolusCalculations;
CREATE TABLE 'BolusCalculations' (

	'IdBolusCalculation'	INT NOT NULL,

	'Timestamp'	DATETIME,t
	'TotalInsulinForMeal'	DOUBLE,abk

	'CalculatedChoToEat'	DOUBLE,

	'BolusInsulinDueToChoOfMeal'	DOUBLE,

	'BolusInsulinDueToCorrectionOfGlucose'	DOUBLE,

	'TargetGlucose'	DOUBLE,

	'InsulinCorrectionSensitivity'	DOUBLE,

	'TypicalBolusMorning'	DOUBLE,

	'TypicalBolusMidday'	DOUBLE,

	'TypicalBolusEvening'	DOUBLE,

	'TypicalBolusNight'	DOUBLE,

	'TotalDailyDoseOfInsulin'	DOUBLE,

	'ChoInsulinRatioBreakfast'	DOUBLE,

	'ChoInsulinRatioLunch'	DOUBLE,

	'ChoInsulinRatioDinner'	DOUBLE,

	'GlucoseBeforeMeal'	DOUBLE,

	'GlucoseToBeCorrected'	DOUBLE,

	'FactorOfInsulinCorrectionSensitivity'	DOUBLE,

	PRIMARY KEY('IdBolusCalculation')

);

-- Table: Containers
DROP TABLE IF EXISTS Containers;
CREATE TABLE Containers (
    IdContainer   INTEGER PRIMARY KEY NOT NULL,
    Name          TEXT NOT NULL,
    Weight        REAL DEFAULT 0,
    Notes         TEXT,
    PhotoFileName TEXT
);

-- Insert some default containers as examples
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (1, 'Small pot', 250.0, 'Standard small cooking pot', NULL);
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (2, 'Medium pot', 450.0, 'Standard medium cooking pot', NULL);
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (3, 'Large pot', 650.0, 'Standard large cooking pot', NULL);
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (4, 'Small plate', 120.0, 'Standard small dinner plate', NULL);
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (5, 'Large plate', 180.0, 'Standard large dinner plate', NULL);
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (6, 'Bowl', 150.0, 'Standard serving bowl', NULL);
INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (7, 'Glass', 85.0, 'Standard drinking glass', NULL);

-- Table: Foods
DROP TABLE IF EXISTS Foods;
CREATE TABLE Foods (IdFood INT NOT NULL, Name VARCHAR (15), Description VARCHAR (256), CarbohydratesPercent DOUBLE, Energy DOUBLE, TotalFatsPercent DOUBLE, SaturatedFatsPercent DOUBLE, MonounsaturatedFatsPercent DOUBLE, PolyunsaturatedFatsPercent DOUBLE, SugarPercent DOUBLE, FibersPercent DOUBLE, ProteinsPercent DOUBLE, SaltPercent DOUBLE, PotassiumPercent DOUBLE, Cholesterol DOUBLE, GlycemicIndex DOUBLE, UnitSymbol TEXT, GramsInOneUnit DOUBLE, Manufacturer TEXT, Category INTEGER, PRIMARY KEY (IdFood));
INSERT INTO Foods (IdFood, Name, Description, CarbohydratesPercent, Energy, TotalFatsPercent, SaturatedFatsPercent, MonounsaturatedFatsPercent, PolyunsaturatedFatsPercent, SugarPercent, FibersPercent, ProteinsPercent, SaltPercent, PotassiumPercent, Cholesterol, GlycemicIndex, UnitSymbol, GramsInOneUnit, Manufacturer, Category) VALUES (1, 'Zucchero', NULL, 100.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'g', 1.0, NULL, NULL);

-- Table: FoodsInMeals
DROP TABLE IF EXISTS FoodsInMeals;
CREATE TABLE FoodsInMeals (IdFoodInMeal INT NOT NULL, IdMeal INT, IdFood INT, Name TEXT, CarbohydratesPercent INT, UnitSymbol TEXT, GramsInOneUnit DOUBLE, QuantityInUnits DOUBLE, CarbohydratesGrams DOUBLE, AccuracyOfChoEstimate DOUBLE, PRIMARY KEY (IdFoodInMeal));

-- Table: GlucoseRecords
DROP TABLE IF EXISTS GlucoseRecords;
CREATE TABLE 'GlucoseRecords' (

	'IdGlucoseRecord'	INT NOT NULL,

	'GlucoseValue'	DOUBLE,

	'Timestamp'	DATETIME,

	'GlucoseString'	VARCHAR(45),

	'IdTypeOfGlucoseMeasurement'	INT,

	'IdTypeOfGlucoseMeasurementDevice'	INT,

	'IdModelOfMeasurementSystem'	INT,

	'IdDevice'	VARCHAR(45),

	'IdDocumentType'	INT,

	'Notes'	VARCHAR(255),

	PRIMARY KEY('IdGlucoseRecord')

);

-- Table: HypoPredictions
DROP TABLE IF EXISTS HypoPredictions;
CREATE TABLE 'HypoPredictions' (

	'IdHypoPrediction'	INT NOT NULL,

	'PredictedTime'	DATETIME,

	'AlarmTime'	DATETIME,

	'GlucoseSlope'	DOUBLE,

	'HypoGlucoseTarget'	INT,

	'GlucoseLast'	DOUBLE,

	'GlucosePrevious'	DOUBLE,

	'Interval'	VARCHAR(10),

	'DatetimeLast'	DATETIME,

	'DatetimePrevious'	DATETIME,

	PRIMARY KEY('IdHypoPrediction')

);

-- Table: Ingredients
DROP TABLE IF EXISTS Ingredients;
CREATE TABLE Ingredients (
    IdIngredient          INTEGER NOT NULL,
    IdRecipe              INTEGER NOT NULL,
    Name                  TEXT,
    Description           TEXT,
    QuantityGrams         REAL,
    QuantityPercent       REAL,
    CarbohydratesPercent  REAL,
    AccuracyOfChoEstimate DOUBLE,
    IdFood                INTEGER,
    PRIMARY KEY (
        IdIngredient
    )
);

-- Table: Injections
DROP TABLE IF EXISTS Injections;
CREATE TABLE Injections (IdInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX DOUBLE, InjectionPositionY DOUBLE, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinAction INT, IdInsulinDrug INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInjection));

-- Table: InsulinDrugs
DROP TABLE IF EXISTS InsulinDrugs;
CREATE TABLE InsulinDrugs (IdInsulinDrug INT NOT NULL, Name VARCHAR (32), Manufacturer VARCHAR (32), TypeOfInsulinAction INTEGER, DurationInHours DOUBLE, OnsetTimeInHours DOUBLE, PeakTimeInHours DOUBLE, PRIMARY KEY (IdInsulinDrug));
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (1, 'Humalog', 'Lilly', 20, 4.0, 0.25, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (2, 'Toujeo', 'Sanofi', 40, 36.0, 3.5, 0.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (4, 'Fiasp', 'Novo Nordisk', 20, 3.0, 0.03, 1.5);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (3, 'Lispro', 'Lilly', 20, 4.5, 0.3333333, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (5, 'Lantus', 'Sanofi', 40, 24.0, 4.0, 0.0);

-- Table: Manufacturers
DROP TABLE IF EXISTS Manufacturers;
CREATE TABLE Manufacturers (
    IdManufacturer INTEGER PRIMARY KEY,
    Name           TEXT,
    Description    TEXT
);

-- Table: Meals
DROP TABLE IF EXISTS Meals;
CREATE TABLE Meals (IdMeal INT NOT NULL, IdTypeOfMeal INT, Carbohydrates DOUBLE, TimeBegin DATETIME, Notes VARCHAR (255), AccuracyOfChoEstimate DOUBLE, IdBolusCalculation INT, IdGlucoseRecord INT, IdInjection INT, TimeEnd DATETIME, PRIMARY KEY (IdMeal));

-- Table: ModelsOfMeasurementSystem
DROP TABLE IF EXISTS ModelsOfMeasurementSystem;
CREATE TABLE 'ModelsOfMeasurementSystem' (

	'IdModelOfMeasurementSystem'	INT NOT NULL,

	'Name'	VARCHAR(45),

	PRIMARY KEY('IdModelOfMeasurementSystem')

);

-- Table: Parameters
DROP TABLE IF EXISTS Parameters;
CREATE TABLE Parameters (IdParameters INT NOT NULL, Timestamp DATETIME, Bolus_TargetGlucose INT, Bolus_GlucoseBeforeMeal INT, Bolus_ChoToEat INT, Bolus_ChoInsulinRatioBreakfast DOUBLE, Bolus_ChoInsulinRatioLunch DOUBLE, Bolus_ChoInsulinRatioDinner DOUBLE, Bolus_TotalDailyDoseOfInsulin DOUBLE, Bolus_InsulinCorrectionSensitivity DOUBLE, Correction_TypicalBolusMorning DOUBLE, Correction_TypicalBolusMidday DOUBLE, Correction_TypicalBolusEvening DOUBLE, Correction_TypicalBolusNight DOUBLE, Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, Hypo_GlucoseTarget DOUBLE, Hypo_GlucoseLast DOUBLE, Hypo_GlucosePrevious DOUBLE, Hypo_HourLast DOUBLE, Hypo_HourPrevious DOUBLE, Hypo_MinuteLast DOUBLE, Hypo_MinutePrevious DOUBLE, Hypo_AlarmAdvanceTime DOUBLE, Hypo_FutureSpanMinutes DOUBLE, Hit_ChoAlreadyTaken DOUBLE, Hit_ChoOfFood DOUBLE, Hit_TargetCho DOUBLE, Hit_NameOfFood TEXT, FoodInMeal_ChoGrams DOUBLE, FoodInMeal_QuantityGrams DOUBLE, FoodInMeal_CarbohydratesPercent DOUBLE, FoodInMeal_Name TEXT, FoodInMeal_AccuracyOfChoEstimate DOUBLE, Meal_ChoGrams DOUBLE, Meal_Breakfast_StartTime_Hours DOUBLE, Meal_Breakfast_EndTime_Hours DOUBLE, Meal_Lunch_StartTime_Hours DOUBLE, Meal_Lunch_EndTime_Hours DOUBLE, Meal_Dinner_StartTime_Hours DOUBLE, Meal_Dinner_EndTime_Hours DOUBLE, Insulin_Short_Id INTEGER, Insulin_Long_Id INTEGER, MonthsOfDataShownInTheGrids DOUBLE, Weigh_RawGross DOUBLE, Weigh_RawTare DOUBLE, Weigh_RawNet DOUBLE, Weigh_CookedGross DOUBLE, Weigh_CookedTare DOUBLE, Weigh_CookedNet DOUBLE, Weigh_CookedPortionGross DOUBLE, Weigh_CookedPortionTare DOUBLE, Weigh_CookedPortionNet DOUBLE, Weigh_NPortions DOUBLE, PRIMARY KEY (IdParameters));
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id, MonthsOfDataShownInTheGrids, Weigh_RawGross, Weigh_RawTare, Weigh_RawNet, Weigh_CookedGross, Weigh_CookedTare, Weigh_CookedNet, Weigh_CookedPortionGross, Weigh_CookedPortionTare, Weigh_CookedPortionNet, Weigh_NPortions) VALUES (1, '0001-01-01 00:00:00', 105, 200, 107.9, 6.0, 7.2, 6.2, 45.5, 45.0, 9.5, 11.5, 11.5, 13.0, 1800.0, 85.0, 109.0, 144.0, 1.0, 0.0, 37.0, 55.0, 20.0, 2033000.0, 42.0, 70.0, 107.9, 'Pane bianco', 83.2, 177.0, NULL, 'Pane bianco', 70.0, '', 6.75, 10.0, 11.75, 14.5, 18.5, 21.5, 1, 2, 3.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- Table: PositionsOfReferences
DROP TABLE IF EXISTS PositionsOfReferences;
CREATE TABLE PositionsOfReferences (IdPosition INTEGER PRIMARY KEY, Timestamp DATETIME, Zone INTEGER, PositionX REAL, PositionY REAL, Notes TEXT);
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (341, '2025-08-31 11:42:29', 2, 0.4119228746332, 0.76030700409893, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (342, '2025-08-31 11:42:29', 2, 0.09791531174947, 0.04635614557651, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (343, '2025-08-31 11:42:29', 2, 0.13228607177734, 0.04879105999865, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (344, '2025-08-31 11:42:29', 2, 0.1697868091638, 0.05512737051788, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (345, '2025-08-31 11:42:29', 2, 0.09738380943189, 0.09466699848261, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (346, '2025-08-31 11:42:29', 2, 0.13488454681835, 0.09563543550637, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (347, '2025-08-31 11:42:29', 2, 0.17341876600348, 0.09613350154039, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (348, '2025-08-31 11:42:29', 2, 0.20885252154045, 0.09270247215647, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (349, '2025-08-31 11:42:29', 2, 0.09738380943189, 0.14687927314519, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (350, '2025-08-31 11:42:29', 2, 0.13488454681835, 0.14591083612143, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (351, '2025-08-31 11:42:29', 2, 0.17238529333087, 0.14591083612143, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (352, '2025-08-31 11:42:29', 2, 0.20935452839975, 0.14200943574777, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (353, '2025-08-31 11:42:29', 2, 0.20988603071733, 0.19665659061996, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (354, '2025-08-31 11:42:29', 2, 0.17238529333087, 0.19765270558173, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (355, '2025-08-31 11:42:29', 2, 0.13435304450076, 0.19812305946521, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (356, '2025-08-31 11:42:29', 2, 0.09841729123056, 0.19715463954772, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (357, '2025-08-31 11:42:29', 2, 0.09998228447289, 0.26256523645512, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (358, '2025-08-31 11:42:29', 2, 0.13591802861702, 0.26156913859962, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (359, '2025-08-31 11:42:29', 2, 0.17288726368589, 0.25816579570685, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (360, '2025-08-31 11:42:29', 2, 0.21248449663226, 0.25083338305555, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (361, '2025-08-31 11:42:29', 2, 0.10101576627156, 0.31574591392893, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (362, '2025-08-31 11:42:29', 2, 0.13644954918674, 0.31427939376489, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (363, '2025-08-31 11:42:29', 2, 0.1749837592458, 0.30891151599285, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (364, '2025-08-31 11:42:29', 2, 0.21248449663226, 0.30110876656434, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (365, '2025-08-31 11:42:29', 2, 0.82811667702415, 0.05512737051788, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (366, '2025-08-31 11:42:29', 2, 0.86248750093451, 0.04928910464984, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (367, '2025-08-31 11:42:29', 2, 0.89842319032222, 0.04439162780351, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (368, '2025-08-31 11:42:29', 2, 0.78645251698471, 0.09320052963736, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (369, '2025-08-31 11:42:29', 2, 0.82498672704377, 0.09369857001198, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (370, '2025-08-31 11:42:29', 2, 0.86195599861693, 0.09416895810799, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (371, '2025-08-31 11:42:29', 2, 0.8994566994991, 0.09466699848261, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (372, '2025-08-31 11:42:29', 2, 0.78435600317266, 0.13857842347013, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (373, '2025-08-31 11:42:29', 2, 0.82445529773475, 0.14250748467552, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (374, '2025-08-31 11:42:29', 2, 0.86248750093451, 0.14394630124216, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (375, '2025-08-31 11:42:29', 2, 0.89895476564836, 0.14347591314615, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (376, '2025-08-31 11:42:29', 2, 0.78435600317266, 0.1922848278097, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (377, '2025-08-31 11:42:29', 2, 0.82395321786689, 0.19471973367871, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (378, '2025-08-31 11:42:29', 2, 0.86195599861693, 0.19618620252395, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (379, '2025-08-31 11:42:29', 2, 0.89789168800464, 0.1956881535962, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (380, '2025-08-31 11:42:29', 2, 0.78385406932192, 0.24936689710403, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (381, '2025-08-31 11:42:29', 2, 0.82395321786689, 0.25573087273157, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (382, '2025-08-31 11:42:29', 2, 0.86039106013102, 0.25913423273061, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (383, '2025-08-31 11:42:29', 2, 0.89632674951873, 0.2596322645521, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (384, '2025-08-31 11:42:29', 2, 0.78538943934098, 0.29720737474382, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (385, '2025-08-31 11:42:29', 2, 0.82238827938098, 0.30647664433638, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (386, '2025-08-31 11:42:29', 2, 0.86145399175763, 0.31134645607439, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (387, '2025-08-31 11:42:29', 2, 0.89685817882775, 0.31184452210841, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (388, '2025-08-31 11:54:37', 1, 0.25350921119799, 0.56500264668144, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (389, '2025-08-31 11:54:37', 1, 0.24717973407946, 0.60534154780777, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (390, '2025-08-31 11:54:37', 1, 0.22647330179169, 0.68601914478524, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (391, '2025-08-31 11:54:37', 1, 0.23759509729997, 0.64568038050904, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (392, '2025-08-31 11:54:37', 1, 0.2981772354345, 0.58593483142254, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (393, '2025-08-31 11:54:37', 1, 0.29501249687523, 0.63076515368817, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (394, '2025-08-31 11:54:37', 1, 0.28705543992622, 0.67110391796437, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (395, '2025-08-31 11:54:37', 1, 0.26788616636723, 0.71000212938796, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (396, '2025-08-31 11:54:37', 1, 0.34601001648241, 0.60686694773858, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (397, '2025-08-31 11:54:37', 1, 0.38914091849441, 0.62627366412381, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (398, '2025-08-31 11:54:37', 1, 0.42738900800641, 0.64271429087549, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (399, '2025-08-31 11:54:37', 1, 0.42738900800641, 0.68161236544896, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (400, '2025-08-31 11:54:37', 1, 0.38751330215965, 0.66813782833082, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (401, '2025-08-31 11:54:37', 1, 0.34284527792315, 0.65169733842927, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (402, '2025-08-31 11:54:37', 1, 0.33172348241487, 0.69203610270547, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (403, '2025-08-31 11:54:37', 1, 0.31418178868636, 0.730934245704, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (404, '2025-08-31 11:54:37', 1, 0.36355171021092, 0.7518664304451, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (405, '2025-08-31 11:54:37', 1, 0.37639150665137, 0.70847672945715, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (406, '2025-08-31 11:54:37', 1, 0.42422430595142, 0.72042579821942, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (407, '2025-08-31 11:54:37', 1, 0.57088733289801, 0.64271429087549, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (408, '2025-09-28 22:26:46', 1, 0.61239058207097, 0.62779906405462, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (409, '2025-09-28 22:26:46', 1, 0.69530673232375, 0.58890092105609, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (410, '2025-09-28 22:26:46', 1, 0.65063863507869, 0.60983303737213, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (411, '2025-09-28 22:26:46', 1, 0.74160233639074, 0.56644333638418, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (412, '2025-09-28 22:26:47', 4, 0.56935013766494, 0.6830530551517, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (413, '2025-09-28 22:26:47', 4, 0.61239058207097, 0.66957858645863, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (414, '2025-09-28 22:26:47', 4, 0.65868614963367, 0.65169733842927, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (415, '2025-09-28 22:26:47', 4, 0.70489129609468, 0.62923975375736, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (416, '2025-09-28 22:26:47', 4, 0.75118690016167, 0.60686694773858, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (417, '2025-09-28 22:26:47', 4, 0.57414249255897, 0.71890039828861, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (418, '2025-09-28 22:26:47', 4, 0.58689183139345, 0.7577986097122, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (419, '2025-09-28 22:26:47', 4, 0.62360276217666, 0.7114428190907, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (420, '2025-09-28 22:26:47', 4, 0.6363521740197, 0.74881556215842, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (421, '2025-09-28 22:26:47', 4, 0.66980790863767, 0.69356150263628, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (422, '2025-09-28 22:26:47', 4, 0.6857220225357, 0.73390033533755, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (423, '2025-09-28 22:26:47', 4, 0.73201762660268, 0.71000212938796, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (424, '2025-09-28 22:26:47', 4, 0.71764070793772, 0.67262938632024, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (425, '2025-09-28 22:26:47', 4, 0.76068115234375, 0.64864647014259, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (426, '2025-09-28 22:26:47', 4, 0.7735209487842, 0.68754454471605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (427, '2025-09-28 22:26:47', 4, 0.47134432267915, 0.32415183028833, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (428, '2025-09-28 22:26:47', 4, 0.52185617109235, 0.3236602646353, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (429, '2025-09-28 22:26:47', 4, 0.47030099385093, 0.39919411013479, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (430, '2025-09-28 22:26:47', 4, 0.52083255457536, 0.3986835907393, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (431, '2025-09-28 22:26:47', 4, 0.38934786125804, 0.21962421571193, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (432, '2025-09-28 22:26:47', 4, 0.43317938991711, 0.21261107440487, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (433, '2025-09-28 22:26:47', 4, 0.47491988040614, 0.20658351059986, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (434, '2025-09-28 22:26:47', 4, 0.52190753261438, 0.20260307072524, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (435, '2025-09-28 22:26:47', 4, 0.56573902476918, 0.20859269283278, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (436, '2025-09-28 22:26:47', 4, 0.27367534363669, 0.19887575226514, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (437, '2025-09-28 22:26:47', 4, 0.51899514814313, 0.1598064781839, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (438, '2025-09-28 22:26:47', 4, 0.55859236283736, 0.16470400207246, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (439, '2025-09-28 22:26:47', 4, 0.42627679560173, 0.26816006305506, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (440, '2025-09-28 22:26:47', 4, 0.25752344085839, 0.30769970812605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (441, '2025-09-28 22:26:47', 4, 0.29452224439411, 0.32576785921516, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (442, '2025-09-28 22:26:47', 4, 0.33098945435154, 0.34380836657879, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (443, '2025-09-28 22:26:47', 4, 0.41848134310052, 0.38381838263952, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (444, '2025-09-28 22:26:47', 4, 0.42161129308089, 0.31940384937509, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (445, '2025-09-28 22:26:47', 4, 0.57524627247496, 0.31500442573308, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (446, '2025-09-28 22:26:47', 4, 0.57940973163221, 0.38284992850949, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (447, '2025-09-28 22:26:47', 4, 0.62107389167165, 0.30379835051806, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (448, '2025-09-28 22:26:47', 4, 0.66013967705686, 0.29109800877592, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (449, '2025-09-28 22:26:47', 4, 0.74346807014429, 0.30623322217454, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (450, '2025-09-28 22:26:47', 4, 0.21803628656853, 0.59203164139136, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (451, '2025-09-28 22:26:47', 4, 0.32662434555127, 0.64592554750998, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (452, '2025-09-28 22:26:47', 4, 0.26770214829148, 0.23279289279818, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (453, '2025-09-28 22:26:47', 4, 0.3062363400984, 0.2356981867632, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (454, '2025-09-28 22:26:47', 4, 0.31299828342273, 0.19375129665495, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (455, '2025-09-28 22:26:47', 4, 0.35466247996645, 0.18641888400364, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (456, '2025-09-28 22:26:47', 4, 0.39632671301445, 0.17618121159985, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (457, '2025-09-28 22:26:47', 4, 0.43592396421296, 0.16641389307954, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (458, '2025-09-28 22:26:47', 4, 0.47602318576648, 0.15908148042824, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (459, '2025-09-28 22:26:47', 4, 0.59738381855796, 0.17421667672059, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (460, '2025-09-28 22:26:48', 1, 0.6791472366552, 0.19422168475095, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (461, '2025-09-28 22:26:48', 1, 0.34426857980244, 0.23912919048771, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (462, '2025-09-28 22:26:48', 1, 0.26353865262994, 0.26646659406311, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (463, '2025-09-28 22:26:48', 1, 0.29947441502621, 0.28060569249996, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (464, '2025-09-28 22:26:48', 1, 0.33541015917034, 0.29280795110181, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (465, '2025-09-28 22:26:48', 1, 0.37914133756355, 0.26646659406311, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (466, '2025-09-28 22:26:48', 1, 0.37604088304145, 0.30794309607536, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (467, '2025-09-28 22:26:48', 1, 0.37031241238973, 0.36358639156872, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (468, '2025-09-28 22:26:48', 1, 0.61353568483198, 0.26256523645512, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (469, '2025-09-28 22:26:48', 1, 0.60780725068453, 0.21912421666988, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (470, '2025-09-28 22:26:48', 1, 0.57080852015737, 0.26353367347888, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (471, '2025-09-28 22:26:48', 1, 0.64947144722824, 0.23619621858469, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (472, '2025-09-28 22:26:48', 1, 0.63851651278409, 0.18445436623064, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (473, '2025-09-28 22:26:48', 1, 0.68697222111898, 0.23326328089419, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (474, '2025-09-28 22:26:48', 1, 0.69373412793903, 0.2776727377032, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (475, '2025-09-28 22:26:48', 1, 0.69999410090834, 0.32648166947301, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (476, '2025-09-28 22:26:48', 1, 0.62392962150026, 0.36358639156872, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (477, '2025-09-28 22:26:48', 1, 0.66612528385728, 0.34161684865909, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (478, '2025-09-28 22:26:48', 1, 0.73592986330461, 0.2620671704211, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (479, '2025-09-28 22:26:48', 1, 0.73020146566145, 0.22789540312215, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (480, '2025-09-28 22:26:48', 1, 0.72707136966395, 0.19862112549923, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (481, '2025-09-28 22:26:48', 1, 0.21209634662245, 0.51864699291007, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (482, '2025-09-28 22:26:48', 1, 0.31255420885588, 0.57398569423522, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (483, '2025-09-28 22:26:48', 1, 0.25992910941822, 0.54110444073185, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (484, '2025-09-28 22:26:48', 1, 0.26625858653675, 0.6188160849259, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (485, '2025-09-28 22:26:48', 1, 0.26951374619771, 0.6845784550825, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (486, '2025-09-28 22:26:48', 1, 0.21842582374098, 0.65762951769636, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (487, '2025-09-28 22:26:48', 1, 0.32530358419464, 0.70847672945715, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (488, '2025-09-28 22:26:48', 1, 0.22801046052048, 0.72339188785296, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (489, '2025-09-28 22:26:48', 1, 0.2710509049265, 0.74737487245568, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (490, '2025-09-28 22:26:48', 1, 0.32530358419464, 0.77720532609743, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (491, '2025-09-28 22:26:48', 1, 0.38751330215965, 0.80262886355276, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (492, '2025-09-28 22:26:48', 1, 0.39393320037988, 0.86839137055949, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (493, '2025-09-28 22:26:48', 1, 0.40026267749841, 0.93415374071609, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (494, '2025-09-28 22:26:48', 1, 0.27909838297721, 0.81457800074009, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (495, '2025-09-28 22:26:48', 1, 0.37313634699041, 0.72788344584238, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (496, '2025-09-28 22:26:48', 1, 0.23596751746949, 0.78915439485969, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (497, '2025-09-28 22:26:48', 1, 0.33326064114365, 0.84152707497635, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (498, '2025-09-28 22:26:48', 1, 0.24717973407946, 0.85796770172803, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (499, '2025-09-28 22:26:46', 1, 0.28859259865501, 0.88339123918337, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (500, '2025-09-28 22:26:46', 1, 0.34121769809267, 0.90881498191389, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (501, '2025-09-28 22:26:46', 1, 0.69376953709068, 0.56644333638418, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (502, '2025-09-28 22:26:46', 1, 0.74160233639074, 0.54254513043459, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (503, '2025-09-28 22:26:46', 1, 0.79106264251271, 0.52161308254362, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (504, '2025-09-28 22:26:46', 1, 0.78780755586031, 0.58440936306667, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (505, '2025-09-28 22:26:46', 1, 0.73834717672978, 0.61432452693648, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (506, '2025-09-28 22:26:46', 1, 0.67776496558669, 0.63822280131113, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (507, '2025-09-28 22:26:46', 1, 0.62830465946471, 0.72788344584238, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (508, '2025-09-28 22:26:46', 1, 0.67776496558669, 0.70847672945715, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (509, '2025-09-28 22:26:46', 1, 0.73834717672978, 0.68008696551815, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (510, '2025-09-28 22:26:46', 1, 0.78627036062724, 0.65313795970694, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (511, '2025-09-28 22:26:46', 1, 0.61076300224049, 0.80118817385002, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (512, '2025-09-28 22:26:46', 1, 0.67622784336218, 0.77576463639469, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (513, '2025-09-28 22:26:46', 1, 0.73518240166623, 0.74288338289133, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (514, '2025-09-28 22:26:46', 1, 0.77822284607226, 0.71890039828861, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (515, '2025-09-28 22:26:46', 1, 0.60922588001598, 0.8714421704211, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (516, '2025-09-28 22:26:46', 1, 0.66818040181575, 0.84296769625403, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (517, '2025-09-28 22:26:46', 1, 0.76547343422922, 0.79067986321556, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (518, '2025-09-28 22:26:46', 1, 0.72406056965367, 0.81610340067089, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (519, '2025-09-28 22:26:46', 1, 0.60443352512195, 0.93864529870551, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (520, '2025-09-28 22:26:46', 1, 0.66338804692173, 0.91322169282511, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (521, '2025-09-28 22:26:46', 1, 0.71447600588274, 0.88788279717279, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (522, '2025-09-28 22:26:46', 1, 0.75272409539474, 0.865425349351, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (523, '2025-09-28 22:26:47', 4, 0.85714283628327, 0.41047733460841, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (524, '2025-09-28 22:26:47', 4, 0.1530513672167, 0.5904263209869, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (525, '2025-09-28 22:26:47', 4, 0.15621610577597, 0.50068089780252, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (526, '2025-09-28 22:26:47', 4, 0.15467894704718, 0.41102025327127, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (527, '2025-09-28 22:26:47', 4, 0.85480955571079, 0.5037317660892, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (528, '2025-09-28 22:26:47', 4, 0.85480955571079, 0.58890092105609, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (529, '2025-09-28 22:26:46', 3, 0.20974692937327, 0.16120066445255, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (530, '2025-09-28 22:26:46', 3, 0.23812317472151, 0.17154404679699, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (531, '2025-09-28 22:26:46', 3, 0.3169924378019, 0.134587440265, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (532, '2025-09-28 22:26:46', 3, 0.31067337373078, 0.16271024624977, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (533, '2025-09-28 22:26:46', 3, 0.34065925285267, 0.11239109377889, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (534, '2025-09-28 22:26:46', 3, 0.34226879035637, 0.13900431796644, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (535, '2025-09-28 22:26:46', 3, 0.34226879035637, 0.16712716909555, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (536, '2025-09-28 22:26:46', 3, 0.37380461587515, 0.16271024624977, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (537, '2025-09-28 22:26:46', 3, 0.36903549820091, 0.13162421051567, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (538, '2025-09-28 22:26:46', 3, 0.23180411065038, 0.21151981692342, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (539, '2025-09-28 22:26:46', 3, 0.25547082943119, 0.19524997508032, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (540, '2025-09-28 22:26:46', 3, 0.20503740280585, 0.21889996951854, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (541, '2025-09-28 22:26:46', 3, 0.21922547734498, 0.18490659273588, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (542, '2025-09-28 22:26:46', 3, 0.19555875856418, 0.19228674533099, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (543, '2025-09-28 22:26:46', 3, 0.15925381537116, 0.25887573964497, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (544, '2025-09-28 22:26:46', 3, 0.16718251321594, 0.27805292253664, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (545, '2025-09-28 22:26:46', 3, 0.1482252247326, 0.28252573408319, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (546, '2025-09-28 22:26:46', 3, 0.18608011432251, 0.27067276994152, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (547, '2025-09-28 22:26:46', 3, 0.15299434240684, 0.3047220805693, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (548, '2025-09-28 22:26:46', 3, 0.17350157728707, 0.30024926902274, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (549, '2025-09-28 22:26:46', 3, 0.19555875856418, 0.29286911642763, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (550, '2025-09-28 22:26:46', 3, 0.43532622424586, 0.07688813519901, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (551, '2025-09-28 22:26:46', 3, 0.41165950546505, 0.10053812963723, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (552, '2025-09-28 22:26:46', 3, 0.41004996796136, 0.128660935622, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (553, '2025-09-28 22:26:46', 3, 0.44003575081329, 0.10350135938656, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (554, '2025-09-28 22:26:46', 3, 0.44319533098383, 0.13162421051567, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (555, '2025-09-28 22:26:46', 3, 0.46370256586406, 0.09466755883934, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (556, '2025-09-28 22:26:46', 3, 0.47318111383576, 0.12128082817123, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (557, '2025-09-28 22:26:46', 3, 0.56146952632098, 0.07839771699623, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (558, '2025-09-28 22:26:46', 3, 0.5315432383059, 0.09612125193579, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (559, '2025-09-28 22:26:46', 3, 0.52361463673107, 0.12424405792056, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (560, '2025-09-28 22:26:46', 3, 0.55675999975355, 0.10501094118378, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (561, '2025-09-28 22:26:46', 3, 0.553600419583, 0.13313374716855, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (562, '2025-09-28 22:26:46', 3, 0.58513614883182, 0.13017051741922, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (563, '2025-09-28 22:26:46', 3, 0.58674578260548, 0.10350135938656, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (564, '2025-09-28 22:26:47', 3, 0.65452696021047, 0.10942786402956, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (565, '2025-09-28 22:26:47', 3, 0.62931019876282, 0.13017051741922, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (566, '2025-09-28 22:26:47', 3, 0.65452696021047, 0.13755067001433, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (567, '2025-09-28 22:26:47', 3, 0.68135316289186, 0.13313374716855, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (568, '2025-09-28 22:26:47', 3, 0.62299103842173, 0.15974701650044, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (569, '2025-09-28 22:26:47', 3, 0.65452696021047, 0.16712716909555, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (570, '2025-09-28 22:26:47', 3, 0.68606268945929, 0.16120066445255, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (571, '2025-09-28 22:26:47', 3, 0.78543928368987, 0.16416389420188, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (572, '2025-09-28 22:26:47', 3, 0.75867257584533, 0.17450727654632, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (573, '2025-09-28 22:26:47', 3, 0.77917981072555, 0.18636024068799, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (574, '2025-09-28 22:26:47', 3, 0.799627358229, 0.1937403932831, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (575, '2025-09-28 22:26:47', 3, 0.74132492113565, 0.19670362303243, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (576, '2025-09-28 22:26:47', 3, 0.76493204880964, 0.21151981692342, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (577, '2025-09-28 22:26:47', 3, 0.79175844403095, 0.22186319926787, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (578, '2025-09-28 22:26:47', 3, 0.83748224781891, 0.25591250989564, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (579, '2025-09-28 22:26:47', 3, 0.82961333362086, 0.27659927458453, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (580, '2025-09-28 22:26:47', 3, 0.81071553997437, 0.26770954019219, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (581, '2025-09-28 22:26:47', 3, 0.85012037596116, 0.27956250433386, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (582, '2025-09-28 22:26:47', 3, 0.82013459310923, 0.29879562107064, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (583, '2025-09-28 22:26:47', 3, 0.799627358229, 0.29141546847552, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (584, '2025-09-28 22:26:47', 3, 0.84225146176311, 0.3047220805693, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (585, '2025-09-28 22:26:47', 3, 0.96052565710026, 0.69525462494799, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (586, '2025-09-28 22:26:47', 3, 0.92898973531151, 0.67898478310489, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (587, '2025-09-28 22:26:47', 3, 0.92898973531151, 0.70408847063956, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (588, '2025-09-28 22:26:47', 3, 0.94001842222003, 0.72628477198132, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (589, '2025-09-28 22:26:47', 3, 0.91480166077238, 0.73668404302653, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (590, '2025-09-28 22:26:47', 3, 0.89900395245958, 0.67747515616332, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (591, '2025-09-28 22:26:47', 3, 0.90061358623324, 0.71001493013822, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (592, '2025-09-28 22:26:47', 3, 0.03627018973654, 0.69525462494799, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (593, '2025-09-28 22:26:47', 3, 0.05516779084311, 0.72332149708765, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (594, '2025-09-28 22:26:47', 3, 0.06619638148166, 0.70118108444665, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (595, '2025-09-28 22:26:47', 3, 0.06780591898536, 0.67602150821121, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (596, '2025-09-28 22:26:47', 3, 0.09618226060356, 0.67747515616332, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (597, '2025-09-28 22:26:47', 3, 0.09618226060356, 0.70856128218611, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (598, '2025-09-28 22:26:47', 3, 0.08199408979446, 0.73517450637366, '');


-- Table: Recipes
DROP TABLE IF EXISTS Recipes;
CREATE TABLE Recipes (
    IdRecipe              INTEGER NOT NULL,
    Name                  TEXT,
    Description           TEXT,
    CarbohydratesPercent  REAL,
    AccuracyOfChoEstimate REAL,
    IsCooked              BOOL,
    RawToCookedRatio      REAL,
    PRIMARY KEY (
        IdRecipe
    )
);

-- Table: UnitsOfFood
DROP TABLE IF EXISTS UnitsOfFood;
CREATE TABLE UnitsOfFood (IdUnitOfFood INTEGER PRIMARY KEY, Symbol TEXT, Name TEXT, Description TEXT, GramsInOneUnit DOUBLE, IdFood INTEGER);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, GramsInOneUnit, IdFood) VALUES (1, 'g', 'grams', 'Unit of mass in the SI', 1.0, NULL);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
";
        internal override void CreateNewDatabase(string dbFile)
        {
			//// making new, means erasing existent! 
			//if (File.Exists(dbFile))
			//	File.Delete(dbFile);

			// create the database; when the file does not exist
			// Microsoft.Data.Sqlite creates the file at first connection
			DbConnection c = Connect();
			c.Close();
			c.Dispose();

			try
			{
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = creationScript;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_DataAndGeneral | CreateNewDatabase", ex);
            }
        }
    }
}
