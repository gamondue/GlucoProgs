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
	'Timestamp'	DATETIME,
	'TotalInsulinForMeal'	DOUBLE,
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
CREATE TABLE Parameters (IdParameters INT NOT NULL, Timestamp DATETIME, Bolus_TargetGlucose INT, Bolus_GlucoseBeforeMeal INT, Bolus_ChoToEat INT, Bolus_ChoInsulinRatioBreakfast DOUBLE, Bolus_ChoInsulinRatioLunch DOUBLE, Bolus_ChoInsulinRatioDinner DOUBLE, Bolus_TotalDailyDoseOfInsulin DOUBLE, Bolus_InsulinCorrectionSensitivity DOUBLE, Correction_TypicalBolusMorning DOUBLE, Correction_TypicalBolusMidday DOUBLE, Correction_TypicalBolusEvening DOUBLE, Correction_TypicalBolusNight DOUBLE, Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, Hypo_GlucoseTarget DOUBLE, Hypo_GlucoseLast DOUBLE, Hypo_GlucosePrevious DOUBLE, Hypo_HourLast DOUBLE, Hypo_HourPrevious DOUBLE, Hypo_MinuteLast DOUBLE, Hypo_MinutePrevious DOUBLE, Hypo_AlarmAdvanceTime DOUBLE, Hypo_FutureSpanMinutes DOUBLE, Hit_ChoAlreadyTaken DOUBLE, Hit_ChoOfFood DOUBLE, Hit_TargetCho DOUBLE, Hit_NameOfFood TEXT, FoodInMeal_ChoGrams DOUBLE, FoodInMeal_QuantityGrams DOUBLE, FoodInMeal_CarbohydratesPercent DOUBLE, FoodInMeal_Name TEXT, FoodInMeal_AccuracyOfChoEstimate DOUBLE, Meal_ChoGrams DOUBLE, Meal_Breakfast_StartTime_Hours DOUBLE, Meal_Breakfast_EndTime_Hours DOUBLE, Meal_Lunch_StartTime_Hours DOUBLE, Meal_Lunch_EndTime_Hours DOUBLE, Meal_Dinner_StartTime_Hours DOUBLE, Meal_Dinner_EndTime_Hours DOUBLE, Insulin_Short_Id INTEGER, Insulin_Long_Id INTEGER, MonthsOfDataShownInTheGrids DOUBLE, PRIMARY KEY (IdParameters));
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id, MonthsOfDataShownInTheGrids) VALUES (1, '0001-01-01 00:00:00', 105, 200, 107.9, 6.0, 7.2, 6.2, 45.5, 45.0, 9.5, 11.5, 11.5, 13.0, 1800.0, 85.0, 109.0, 144.0, 1.0, 0.0, 37.0, 55.0, 20.0, 2033000.0, 42.0, 70.0, 107.9, 'Pane bianco', 83.2, 177.0, NULL, 'Pane bianco', 70.0, '', 6.75, 10.0, 11.75, 14.5, 18.5, 21.5, 1, 2, 3.0);

-- Table: PositionsOfReferences
DROP TABLE IF EXISTS PositionsOfReferences;
CREATE TABLE PositionsOfReferences (IdPosition INTEGER PRIMARY KEY, Timestamp DATETIME, Zone INTEGER, PositionX REAL, PositionY REAL, Notes TEXT);
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (96, '2025-05-22 20:55:45', 3, 0.23148263584484, 0.14618778463078, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (97, '2025-05-22 20:55:45', 3, 0.26557354493575, 0.15106308020889, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (98, '2025-05-22 20:55:45', 3, 0.30111399563876, 0.14493412702031, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (99, '2025-05-22 20:55:45', 3, 0.29542324759743, 0.11545009753628, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (100, '2025-05-22 20:55:45', 3, 0.26702308654785, 0.12157905072486, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (101, '2025-05-22 20:55:45', 3, 0.23577755147761, 0.11670375514675, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (102, '2025-05-22 20:55:45', 3, 0.26841891895641, 0.09209502124083, '');
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
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (388, '2025-08-31 11:42:29', 2, 0.25350921119799, 0.56500264668144, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (389, '2025-08-31 11:42:29', 2, 0.24717973407946, 0.60534154780777, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (390, '2025-08-31 11:42:29', 2, 0.22647330179169, 0.68601914478524, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (391, '2025-08-31 11:42:29', 2, 0.23759509729997, 0.64568038050904, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (392, '2025-08-31 11:42:29', 2, 0.2981772354345, 0.58593483142254, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (393, '2025-08-31 11:42:29', 2, 0.29501249687523, 0.63076515368817, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (394, '2025-08-31 11:42:29', 2, 0.28705543992622, 0.67110391796437, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (395, '2025-08-31 11:42:29', 2, 0.26788616636723, 0.71000212938796, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (396, '2025-08-31 11:42:29', 2, 0.34601001648241, 0.60686694773858, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (397, '2025-08-31 11:42:29', 2, 0.38914091849441, 0.62627366412381, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (398, '2025-08-31 11:42:29', 2, 0.42738900800641, 0.64271429087549, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (399, '2025-08-31 11:42:29', 2, 0.42738900800641, 0.68161236544896, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (400, '2025-08-31 11:42:29', 2, 0.38751330215965, 0.66813782833082, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (401, '2025-08-31 11:42:29', 2, 0.34284527792315, 0.65169733842927, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (402, '2025-08-31 11:42:29', 2, 0.33172348241487, 0.69203610270547, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (403, '2025-08-31 11:42:29', 2, 0.31418178868636, 0.730934245704, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (404, '2025-08-31 11:42:29', 2, 0.36355171021092, 0.7518664304451, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (405, '2025-08-31 11:42:29', 2, 0.37639150665137, 0.70847672945715, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (406, '2025-08-31 11:42:29', 2, 0.42422430595142, 0.72042579821942, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (407, '2025-08-31 11:42:29', 2, 0.57088733289801, 0.64271429087549, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (408, '2025-08-31 11:42:29', 2, 0.61239058207097, 0.62779906405462, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (409, '2025-08-31 11:42:29', 2, 0.69530673232375, 0.58890092105609, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (410, '2025-08-31 11:42:29', 2, 0.65063863507869, 0.60983303737213, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (411, '2025-08-31 11:42:29', 2, 0.74160233639074, 0.56644333638418, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (412, '2025-08-31 11:42:29', 2, 0.56935013766494, 0.6830530551517, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (413, '2025-08-31 11:42:29', 2, 0.61239058207097, 0.66957858645863, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (414, '2025-08-31 11:42:29', 2, 0.65868614963367, 0.65169733842927, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (415, '2025-08-31 11:42:29', 2, 0.70489129609468, 0.62923975375736, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (416, '2025-08-31 11:42:29', 2, 0.75118690016167, 0.60686694773858, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (417, '2025-08-31 11:42:29', 2, 0.57414249255897, 0.71890039828861, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (418, '2025-08-31 11:42:29', 2, 0.58689183139345, 0.7577986097122, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (419, '2025-08-31 11:42:29', 2, 0.62360276217666, 0.7114428190907, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (420, '2025-08-31 11:42:29', 2, 0.6363521740197, 0.74881556215842, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (421, '2025-08-31 11:42:29', 2, 0.66980790863767, 0.69356150263628, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (422, '2025-08-31 11:42:29', 2, 0.6857220225357, 0.73390033533755, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (423, '2025-08-31 11:42:29', 2, 0.73201762660268, 0.71000212938796, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (424, '2025-08-31 11:42:29', 2, 0.71764070793772, 0.67262938632024, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (425, '2025-08-31 11:42:29', 2, 0.76068115234375, 0.64864647014259, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (426, '2025-08-31 11:42:29', 2, 0.7735209487842, 0.68754454471605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (427, '2025-08-31 11:54:37', 1, 0.47134432267915, 0.32415183028833, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (428, '2025-08-31 11:54:37', 1, 0.52185617109235, 0.3236602646353, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (429, '2025-08-31 11:54:37', 1, 0.47030099385093, 0.39919411013479, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (430, '2025-08-31 11:54:37', 1, 0.52083255457536, 0.3986835907393, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (431, '2025-08-31 11:54:37', 1, 0.38934786125804, 0.21962421571193, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (432, '2025-08-31 11:54:37', 1, 0.43317938991711, 0.21261107440487, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (433, '2025-08-31 11:54:37', 1, 0.47491988040614, 0.20658351059986, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (434, '2025-08-31 11:54:37', 1, 0.52190753261438, 0.20260307072524, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (435, '2025-08-31 11:54:37', 1, 0.56573902476918, 0.20859269283278, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (436, '2025-08-31 11:54:37', 1, 0.27367534363669, 0.19887575226514, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (437, '2025-08-31 11:54:37', 1, 0.51899514814313, 0.1598064781839, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (438, '2025-08-31 11:54:37', 1, 0.55859236283736, 0.16470400207246, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (439, '2025-08-31 11:54:37', 1, 0.42627679560173, 0.26816006305506, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (440, '2025-08-31 11:54:37', 1, 0.25752344085839, 0.30769970812605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (441, '2025-08-31 11:54:37', 1, 0.29452224439411, 0.32576785921516, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (442, '2025-08-31 11:54:37', 1, 0.33098945435154, 0.34380836657879, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (443, '2025-08-31 11:54:37', 1, 0.41848134310052, 0.38381838263952, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (444, '2025-08-31 11:54:37', 1, 0.42161129308089, 0.31940384937509, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (445, '2025-08-31 11:54:37', 1, 0.57524627247496, 0.31500442573308, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (446, '2025-08-31 11:54:37', 1, 0.57940973163221, 0.38284992850949, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (447, '2025-08-31 11:54:37', 1, 0.62107389167165, 0.30379835051806, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (448, '2025-08-31 11:54:37', 1, 0.66013967705686, 0.29109800877592, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (449, '2025-08-31 11:54:37', 1, 0.74346807014429, 0.30623322217454, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (450, '2025-08-31 11:54:37', 1, 0.21803628656853, 0.59203164139136, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (451, '2025-08-31 11:54:37', 1, 0.32662434555127, 0.64592554750998, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (452, '2025-08-31 11:54:37', 1, 0.26770214829148, 0.23279289279818, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (453, '2025-08-31 11:54:37', 1, 0.3062363400984, 0.2356981867632, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (454, '2025-08-31 11:54:37', 1, 0.31299828342273, 0.19375129665495, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (455, '2025-08-31 11:54:37', 1, 0.35466247996645, 0.18641888400364, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (456, '2025-08-31 11:54:37', 1, 0.39632671301445, 0.17618121159985, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (457, '2025-08-31 11:54:37', 1, 0.43592396421296, 0.16641389307954, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (458, '2025-08-31 11:54:37', 1, 0.47602318576648, 0.15908148042824, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (459, '2025-08-31 11:54:37', 1, 0.59738381855796, 0.17421667672059, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (460, '2025-08-31 11:54:38', 1, 0.6791472366552, 0.19422168475095, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (461, '2025-08-31 11:54:38', 1, 0.34426857980244, 0.23912919048771, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (462, '2025-08-31 11:54:38', 1, 0.26353865262994, 0.26646659406311, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (463, '2025-08-31 11:54:38', 1, 0.29947441502621, 0.28060569249996, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (464, '2025-08-31 11:54:38', 1, 0.33541015917034, 0.29280795110181, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (465, '2025-08-31 11:54:38', 1, 0.37914133756355, 0.26646659406311, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (466, '2025-08-31 11:54:38', 1, 0.37604088304145, 0.30794309607536, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (467, '2025-08-31 11:54:38', 1, 0.37031241238973, 0.36358639156872, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (468, '2025-08-31 11:54:38', 1, 0.61353568483198, 0.26256523645512, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (469, '2025-08-31 11:54:38', 1, 0.60780725068453, 0.21912421666988, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (470, '2025-08-31 11:54:38', 1, 0.57080852015737, 0.26353367347888, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (471, '2025-08-31 11:54:38', 1, 0.64947144722824, 0.23619621858469, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (472, '2025-08-31 11:54:38', 1, 0.63851651278409, 0.18445436623064, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (473, '2025-08-31 11:54:38', 1, 0.68697222111898, 0.23326328089419, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (474, '2025-08-31 11:54:38', 1, 0.69373412793903, 0.2776727377032, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (475, '2025-08-31 11:54:38', 1, 0.69999410090834, 0.32648166947301, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (476, '2025-08-31 11:54:38', 1, 0.62392962150026, 0.36358639156872, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (477, '2025-08-31 11:54:38', 1, 0.66612528385728, 0.34161684865909, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (478, '2025-08-31 11:54:38', 1, 0.73592986330461, 0.2620671704211, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (479, '2025-08-31 11:54:38', 1, 0.73020146566145, 0.22789540312215, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (480, '2025-08-31 11:54:38', 1, 0.72707136966395, 0.19862112549923, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (481, '2025-08-31 11:54:38', 1, 0.21209634662245, 0.51864699291007, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (482, '2025-08-31 11:54:38', 1, 0.31255420885588, 0.57398569423522, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (483, '2025-08-31 11:54:38', 1, 0.25992910941822, 0.54110444073185, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (484, '2025-08-31 11:54:38', 1, 0.26625858653675, 0.6188160849259, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (485, '2025-08-31 11:54:38', 1, 0.26951374619771, 0.6845784550825, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (486, '2025-08-31 11:54:38', 1, 0.21842582374098, 0.65762951769636, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (487, '2025-08-31 11:54:38', 1, 0.32530358419464, 0.70847672945715, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (488, '2025-08-31 11:54:38', 1, 0.22801046052048, 0.72339188785296, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (489, '2025-08-31 11:54:38', 1, 0.2710509049265, 0.74737487245568, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (490, '2025-08-31 11:54:38', 1, 0.32530358419464, 0.77720532609743, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (491, '2025-08-31 11:54:38', 1, 0.38751330215965, 0.80262886355276, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (492, '2025-08-31 11:54:38', 1, 0.39393320037988, 0.86839137055949, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (493, '2025-08-31 11:54:38', 1, 0.40026267749841, 0.93415374071609, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (494, '2025-08-31 11:54:38', 1, 0.27909838297721, 0.81457800074009, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (495, '2025-08-31 11:54:38', 1, 0.37313634699041, 0.72788344584238, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (496, '2025-08-31 11:54:38', 1, 0.23596751746949, 0.78915439485969, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (497, '2025-08-31 11:54:38', 1, 0.33326064114365, 0.84152707497635, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (498, '2025-08-31 11:54:38', 1, 0.24717973407946, 0.85796770172803, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (499, '2025-08-31 11:54:38', 1, 0.28859259865501, 0.88339123918337, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (500, '2025-08-31 11:54:38', 1, 0.34121769809267, 0.90881498191389, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (501, '2025-08-31 11:54:38', 1, 0.69376953709068, 0.56644333638418, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (502, '2025-08-31 11:54:38', 1, 0.74160233639074, 0.54254513043459, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (503, '2025-08-31 11:54:38', 1, 0.79106264251271, 0.52161308254362, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (504, '2025-08-31 11:54:38', 1, 0.78780755586031, 0.58440936306667, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (505, '2025-08-31 11:54:38', 1, 0.73834717672978, 0.61432452693648, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (506, '2025-08-31 11:54:38', 1, 0.67776496558669, 0.63822280131113, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (507, '2025-08-31 11:54:38', 1, 0.62830465946471, 0.72788344584238, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (508, '2025-08-31 11:54:38', 1, 0.67776496558669, 0.70847672945715, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (509, '2025-08-31 11:54:38', 1, 0.73834717672978, 0.68008696551815, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (510, '2025-08-31 11:54:38', 1, 0.78627036062724, 0.65313795970694, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (511, '2025-08-31 11:54:38', 1, 0.61076300224049, 0.80118817385002, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (512, '2025-08-31 11:54:38', 1, 0.67622784336218, 0.77576463639469, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (513, '2025-08-31 11:54:38', 1, 0.73518240166623, 0.74288338289133, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (514, '2025-08-31 11:54:38', 1, 0.77822284607226, 0.71890039828861, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (515, '2025-08-31 11:54:38', 1, 0.60922588001598, 0.8714421704211, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (516, '2025-08-31 11:54:38', 1, 0.66818040181575, 0.84296769625403, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (517, '2025-08-31 11:54:38', 1, 0.76547343422922, 0.79067986321556, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (518, '2025-08-31 11:54:38', 1, 0.72406056965367, 0.81610340067089, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (519, '2025-08-31 11:54:38', 1, 0.60443352512195, 0.93864529870551, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (520, '2025-08-31 11:54:38', 1, 0.66338804692173, 0.91322169282511, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (521, '2025-08-31 11:54:38', 1, 0.71447600588274, 0.88788279717279, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (522, '2025-08-31 11:54:38', 1, 0.75272409539474, 0.865425349351, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (523, '2025-08-31 11:56:42', 4, 0.85714283628327, 0.41047733460841, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (524, '2025-08-31 11:56:42', 4, 0.1530513672167, 0.5904263209869, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (525, '2025-08-31 11:56:42', 4, 0.15621610577597, 0.50068089780252, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (526, '2025-08-31 11:56:42', 4, 0.15467894704718, 0.41102025327127, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (527, '2025-08-31 11:56:42', 4, 0.85480955571079, 0.5037317660892, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (528, '2025-08-31 11:56:42', 4, 0.85480955571079, 0.58890092105609, '');

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
