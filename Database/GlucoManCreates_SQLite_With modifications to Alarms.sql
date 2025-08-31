--
-- File generated with SQLiteStudio v3.4.17 on ven ago 29 19:13:00 2025
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

-- Table: CategoriesOfFood
DROP TABLE IF EXISTS CategoriesOfFood;
CREATE TABLE CategoriesOfFood (IdCategoryOfFood INTEGER PRIMARY KEY, Name TEXT, Description TEXT);

-- Table: Foods
DROP TABLE IF EXISTS Foods;
CREATE TABLE Foods (IdFood INT NOT NULL, Name VARCHAR (15), Description VARCHAR (256), Energy DOUBLE, TotalFatsPercent DOUBLE, SaturatedFatsPercent DOUBLE, MonounsaturatedFatsPercent DOUBLE, PolyunsaturatedFatsPercent DOUBLE, CarbohydratesPercent DOUBLE, SugarPercent DOUBLE, FibersPercent INT, ProteinsPercent INT, SaltPercent DOUBLE, PotassiumPercent DOUBLE, Cholesterol DOUBLE, GlycemicIndex DOUBLE, UnitSymbol TEXT, GramsInOneUnit DOUBLE, Manufacturer TEXT, Category REAL, PRIMARY KEY (IdFood));

-- Table: FoodsInMeals
DROP TABLE IF EXISTS FoodsInMeals;
CREATE TABLE FoodsInMeals (IdFoodInMeal INT NOT NULL, IdMeal INT, IdFood INT, Name TEXT, CarbohydratesPercent INT, Quantity DOUBLE, UnitSymbol TEXT, GramsInOneUnit DOUBLE, QuantityInUnits DOUBLE, CarbohydratesGrams DOUBLE, AccuracyOfChoEstimate DOUBLE, PRIMARY KEY (IdFoodInMeal));



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
CREATE TABLE Injections (IdInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX INT, InjectionPositionY INT, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinAction INT, IdInsulinDrug INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInjection));


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
CREATE TABLE Parameters (IdParameters INT NOT NULL, Timestamp DATETIME, Bolus_TargetGlucose INT, Bolus_GlucoseBeforeMeal INT, Bolus_ChoToEat INT, Bolus_ChoInsulinRatioBreakfast DOUBLE, Bolus_ChoInsulinRatioLunch DOUBLE, Bolus_ChoInsulinRatioDinner DOUBLE, Bolus_TotalDailyDoseOfInsulin DOUBLE, Bolus_InsulinCorrectionSensitivity DOUBLE, Correction_TypicalBolusMorning DOUBLE, Correction_TypicalBolusMidday DOUBLE, Correction_TypicalBolusEvening DOUBLE, Correction_TypicalBolusNight DOUBLE, Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, Hypo_GlucoseTarget DOUBLE, Hypo_GlucoseLast DOUBLE, Hypo_GlucosePrevious DOUBLE, Hypo_HourLast DOUBLE, Hypo_HourPrevious DOUBLE, Hypo_MinuteLast DOUBLE, Hypo_MinutePrevious DOUBLE, Hypo_AlarmAdvanceTime DOUBLE, Hypo_FutureSpanMinutes DOUBLE, Hit_ChoAlreadyTaken DOUBLE, Hit_ChoOfFood DOUBLE, Hit_TargetCho DOUBLE, Hit_NameOfFood TEXT, FoodInMeal_ChoGrams DOUBLE, FoodInMeal_QuantityGrams DOUBLE, FoodInMeal_CarbohydratesPercent DOUBLE, FoodInMeal_Name TEXT, FoodInMeal_AccuracyOfChoEstimate DOUBLE, Meal_ChoGrams DOUBLE, Meal_Breakfast_StartTime_Hours DOUBLE, Meal_Breakfast_EndTime_Hours DOUBLE, Meal_Lunch_StartTime_Hours DOUBLE, Meal_Lunch_EndTime_Hours DOUBLE, Meal_Dinner_StartTime_Hours DOUBLE, Meal_Dinner_EndTime_Hours DOUBLE, Insulin_Short_Id INTEGER, Insulin_Long_Id INTEGER, PRIMARY KEY (IdParameters));
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (1, NULL, 105, 200, 107.9, 6.0, 7.2, 6.2, 45.5, 45.0, 9.5, 11.5, 11.5, 13.0, 1800.0, 85.0, 109.0, 144.0, 1.0, 0.0, 37.0, 55.0, 20.0, 2033000.0, 42.0, '', 41.3, 'Pane bianco', 83.2, 177.0, '', 'Pane bianco', 70.0, 42.0, 6.75, 10.0, 11.75, 14.25, 18.25, 21.3, 1, 2);

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
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (199, '2025-06-11 22:43:17', 2, 0.26133233795331, 0.56987111280359, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (200, '2025-06-11 22:43:17', 2, 0.30820060929367, 0.59120958085638, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (201, '2025-06-11 22:43:17', 2, 0.35222354310682, 0.61249778399216, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (202, '2025-06-11 22:43:17', 2, 0.39345474015368, 0.63247738609822, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (203, '2025-06-11 22:43:17', 2, 0.43178688922463, 0.64978976107409, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (204, '2025-06-11 22:43:17', 2, 0.5780825245505, 0.64843099791861, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (205, '2025-06-11 22:43:17', 2, 0.57668667444865, 0.68708180668353, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (206, '2025-06-11 22:43:17', 2, 0.57953201197586, 0.72432348458471, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (207, '2025-06-11 22:43:17', 2, 0.59370528166395, 0.76161546166664, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (208, '2025-06-11 22:43:17', 2, 0.42754562847038, 0.72432348458471, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (209, '2025-06-11 22:43:17', 2, 0.41192287135693, 0.76030699769193, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (210, '2025-06-11 22:43:17', 2, 0.23234566070397, 0.69235074865945, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (211, '2025-06-11 22:43:17', 2, 0.27708503852125, 0.7147569778635, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (212, '2025-06-11 22:43:17', 2, 0.32345459609706, 0.74021864245252, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (213, '2025-06-11 22:43:17', 2, 0.37136380445185, 0.75668388054479, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (214, '2025-06-11 22:43:17', 2, 0.38576376221474, 0.71628466129224, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (215, '2025-06-11 22:43:17', 2, 0.39536363655642, 0.67732835695807, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (216, '2025-06-11 22:43:17', 2, 0.44001248539416, 0.68785253541107, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (217, '2025-06-11 22:43:17', 2, 0.35062425873914, 0.65942027247491, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (218, '2025-06-11 22:43:17', 2, 0.33939413151374, 0.69981949172746, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (219, '2025-06-11 22:43:17', 2, 0.28985481652562, 0.68038372381556, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (220, '2025-06-11 22:43:17', 2, 0.30588488092185, 0.63540138574928, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (221, '2025-06-11 22:43:17', 2, 0.25797574569225, 0.61596561783738, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (222, '2025-06-11 22:43:17', 2, 0.2483757251002, 0.65492205922653, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (223, '2025-06-11 22:43:17', 2, 0.62213971751302, 0.67588551056718, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (224, '2025-06-11 22:43:17', 2, 0.63010948522136, 0.71628466129224, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (225, '2025-06-11 22:43:17', 2, 0.64296971908009, 0.75965441036443, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (226, '2025-06-11 22:43:17', 2, 0.61733978034218, 0.62946032611001, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (227, '2025-06-11 22:43:17', 2, 0.66053950738049, 0.61299508801774, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (228, '2025-06-11 22:43:17', 2, 0.66524891557178, 0.65789258904617, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (229, '2025-06-11 22:43:17', 2, 0.67810929568088, 0.69981949172746, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (230, '2025-06-11 22:43:17', 2, 0.69087900056007, 0.73869095902378, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (231, '2025-06-11 22:43:17', 2, 0.74032771344346, 0.7133141314726, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (232, '2025-06-11 22:43:17', 2, 0.72438817802677, 0.6728300751822, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (233, '2025-06-11 22:43:17', 2, 0.71161847314759, 0.63692906917803, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (234, '2025-06-11 22:43:17', 2, 0.70201859880591, 0.59203163667709, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (235, '2025-06-11 22:43:17', 2, 0.74675790349801, 0.57106825386394, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (236, '2025-06-11 22:43:17', 2, 0.75472767120635, 0.61299508801774, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (237, '2025-06-11 22:43:17', 2, 0.76749752233591, 0.65186662384155, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (238, '2025-06-11 22:43:17', 2, 0.77872750331093, 0.69082306523071, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (239, '2025-06-11 22:43:40', 4, 0.16021229045704, 0.41419535393183, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (240, '2025-06-11 22:43:40', 4, 0.15705024775178, 0.59243697680543, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (241, '2025-06-11 22:43:40', 4, 0.85714285714286, 0.41047732620519, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (242, '2025-06-11 22:43:40', 4, 0.85633738322548, 0.50591597857639, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (243, '2025-06-11 22:43:40', 4, 0.85556181750893, 0.59319180715431, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (244, '2025-06-11 22:43:40', 4, 0.16052718578675, 0.51114836113414, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (245, '2025-08-29 18:15:06', 1, 0.47134432267915, 0.32415183028833, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (246, '2025-08-29 18:15:06', 1, 0.52185617109235, 0.3236602646353, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (247, '2025-08-29 18:15:06', 1, 0.47030099385093, 0.39919411013479, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (248, '2025-08-29 18:15:06', 1, 0.52083251807108, 0.3986835907393, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (249, '2025-08-29 18:15:06', 1, 0.38934786125804, 0.21962421571193, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (250, '2025-08-29 18:15:06', 1, 0.43317938991711, 0.21261107440487, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (251, '2025-08-29 18:15:06', 1, 0.47491988040614, 0.20658352770613, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (252, '2025-08-29 18:15:06', 1, 0.52190753261438, 0.20260307072524, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (253, '2025-08-29 18:15:06', 1, 0.60854471015018, 0.21462023953151, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (254, '2025-08-29 18:15:06', 1, 0.56573902476918, 0.20859269283278, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (255, '2025-08-29 18:15:06', 1, 0.21813911912544, 0.52593412527589, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (256, '2025-08-29 18:15:06', 1, 0.26701570355721, 0.54271089014986, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (257, '2025-08-29 18:15:06', 1, 0.32110318270597, 0.58120262248634, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (258, '2025-08-29 18:15:06', 1, 0.27367534363669, 0.19887575226514, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (259, '2025-08-29 18:15:06', 1, 0.31640259957199, 0.19544473143436, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (260, '2025-08-29 18:15:06', 1, 0.39869748348254, 0.17837269530703, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (261, '2025-08-29 18:15:06', 1, 0.35910023228403, 0.18764194779332, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (262, '2025-08-29 18:15:06', 1, 0.43826523922277, 0.16860537678672, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (263, '2025-08-29 18:15:06', 1, 0.4793979334489, 0.16127298124168, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (264, '2025-08-29 18:15:06', 1, 0.51899514814313, 0.1598064781839, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (265, '2025-08-29 18:15:06', 1, 0.55859236283736, 0.16470400207246, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (266, '2025-08-29 18:15:06', 1, 0.60025663238963, 0.17593778943802, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (267, '2025-08-29 18:15:06', 1, 0.64085782429818, 0.18813999672107, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (268, '2025-08-29 18:15:06', 1, 0.6825514797959, 0.19594276325585, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (269, '2025-08-29 18:15:06', 1, 0.72941268117804, 0.2008126092064, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (270, '2025-08-29 18:15:06', 1, 0.73254255814986, 0.23108298468483, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (271, '2025-08-29 18:15:06', 1, 0.68878188429837, 0.23401593948159, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (272, '2025-08-29 18:15:06', 1, 0.65024774724787, 0.23498437650535, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (273, '2025-08-29 18:15:06', 1, 0.61484348716918, 0.26232183139955, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (274, '2025-08-29 18:15:06', 1, 0.571614279131, 0.26425868834081, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (275, '2025-08-29 18:15:06', 1, 0.42627679560173, 0.26816006305506, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (276, '2025-08-29 18:15:06', 1, 0.38411059219871, 0.26768969206532, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (277, '2025-08-29 18:15:06', 1, 0.34660985481226, 0.24278717725266, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (278, '2025-08-29 18:15:06', 1, 0.3101425900984, 0.23888576832587, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (279, '2025-08-29 18:15:06', 1, 0.27004336854488, 0.23254945353008, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (280, '2025-08-29 18:15:06', 1, 0.2627499411551, 0.2701245637218, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (281, '2025-08-29 18:15:06', 1, 0.25752344085839, 0.30769970812605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (282, '2025-08-29 18:15:06', 1, 0.29452222614197, 0.32576785921516, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (283, '2025-08-29 18:15:06', 1, 0.30075264889658, 0.28329522513488, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (284, '2025-08-29 18:15:06', 1, 0.33775136117159, 0.29549746663047, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (285, '2025-08-29 18:15:06', 1, 0.33098945435154, 0.34380836657879, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (286, '2025-08-29 18:15:06', 1, 0.37265365089526, 0.36234687155138, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (287, '2025-08-29 18:15:06', 1, 0.41848134310052, 0.38381838263952, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (288, '2025-08-29 18:15:06', 1, 0.42161129308089, 0.31940384937509, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (289, '2025-08-29 18:15:06', 1, 0.37681711005252, 0.30769970812605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (290, '2025-08-29 18:15:06', 1, 0.57524627247496, 0.31500442573308, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (291, '2025-08-29 18:15:06', 1, 0.57940973163221, 0.38284996272203, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (292, '2025-08-29 18:15:06', 1, 0.62107389167165, 0.30379835051806, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (293, '2025-08-29 18:15:06', 1, 0.62680236232338, 0.36284490337286, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (294, '2025-08-29 18:15:06', 1, 0.66796466152063, 0.34477678649629, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (295, '2025-08-29 18:15:06', 1, 0.70283727326462, 0.32623821309864, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (296, '2025-08-29 18:15:06', 1, 0.69554386412698, 0.27939383331436, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (297, '2025-08-29 18:15:06', 1, 0.66013967705686, 0.29109800877592, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (298, '2025-08-29 18:15:06', 1, 0.73723766564182, 0.26572515718605, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (299, '2025-08-29 18:15:06', 1, 0.74346807014429, 0.30623322217454, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (300, '2025-08-29 18:15:06', 1, 0.21803630482067, 0.59203164139136, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (301, '2025-08-29 18:15:06', 1, 0.27074545536315, 0.62046382886946, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (302, '2025-08-29 18:15:06', 1, 0.32662434555127, 0.64592554750998, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (303, '2025-08-29 18:15:06', 1, 0.22437589362478, 0.66391846524226, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (304, '2025-08-29 18:15:06', 1, 0.27391528626948, 0.68632479954194, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (305, '2025-08-29 18:15:06', 1, 0.23397584394975, 0.73274990475231, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (306, '2025-08-29 18:15:06', 1, 0.2579757471404, 0.87194051015537, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (307, '2025-08-29 18:15:06', 1, 0.35062426699406, 0.91836575221588, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (308, '2025-08-29 18:15:06', 1, 0.29791511645157, 0.8914611542706, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (309, '2025-08-29 18:15:06', 1, 0.40487298554781, 0.93780148082784, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (310, '2025-08-29 18:15:06', 1, 0.40333341753654, 0.87643870537591, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (311, '2025-08-29 18:15:06', 1, 0.34419414757541, 0.84800651789781, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (312, '2025-08-29 18:15:06', 1, 0.28505480460573, 0.8195743304197, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (313, '2025-08-29 18:15:06', 1, 0.24194560438822, 0.79708314904183, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (314, '2025-08-29 18:15:06', 1, 0.28034540568813, 0.75668389700988, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (315, '2025-08-29 18:15:06', 1, 0.33785448576275, 0.78367327360829, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (316, '2025-08-29 18:15:06', 1, 0.40007303776353, 0.80760733429092, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (317, '2025-08-29 18:15:06', 1, 0.69884874261737, 0.57403879208415, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (318, '2025-08-29 18:15:06', 1, 0.7515580026727, 0.54407884401056, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (319, '2025-08-29 18:15:06', 1, 0.79946694990094, 0.52761358423618, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (320, '2025-08-29 18:15:06', 1, 0.79629728326387, 0.59050391500841, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (321, '2025-08-29 18:15:06', 1, 0.7451276277241, 0.61596563364893, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (322, '2025-08-29 18:15:06', 1, 0.68924892005738, 0.64295501024734, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (323, '2025-08-29 18:15:06', 1, 0.7467579088713, 0.68632479954194, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (324, '2025-08-29 18:15:06', 1, 0.79312747060967, 0.65942027002172, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (325, '2025-08-29 18:15:06', 1, 0.78189736690247, 0.72825170953178, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (326, '2025-08-29 18:15:06', 1, 0.61733979585638, 0.80905014517061, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (327, '2025-08-29 18:15:06', 1, 0.6796488784717, 0.78367327360829, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (328, '2025-08-29 18:15:06', 1, 0.73878814843283, 0.75371335974723, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (329, '2025-08-29 18:15:06', 1, 0.61253970205499, 0.87643870537591, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (330, '2025-08-29 18:15:06', 1, 0.60937003541791, 0.94832566607693, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (331, '2025-08-29 18:15:06', 1, 0.66524888910175, 0.92286387901135, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (332, '2025-08-29 18:15:06', 1, 0.67484878467031, 0.84800651789781, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (333, '2025-08-29 18:15:06', 1, 0.73235791950135, 0.82110205680266, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (334, '2025-08-29 18:15:06', 1, 0.7195882112786, 0.89443169153325, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (335, '2025-08-29 18:15:06', 1, 0.76595777301697, 0.87194051015537, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (336, '2025-08-29 18:15:06', 1, 0.77555766858553, 0.79861087542478, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (337, '2025-08-29 18:15:06', 1, 0.38192022588264, 0.74661124875193, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (338, '2025-08-29 18:15:06', 1, 0.33092272452761, 0.71822148481292, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (339, '2025-08-29 18:15:06', 1, 0.63708861136551, 0.73915360112896, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (340, '2025-08-29 18:15:06', 1, 0.68971361954246, 0.71669608488211, '');

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
INSERT INTO Recipes (IdRecipe, Name, Description, CarbohydratesPercent, AccuracyOfChoEstimate, IsCooked, RawToCookedRatio) VALUES (1, 'Torta al cioccolato', '', NULL, NULL, 1, NULL);

-- Table: UnitsOfFood
DROP TABLE IF EXISTS UnitsOfFood;
CREATE TABLE UnitsOfFood (
    IdUnitOfFood   INTEGER PRIMARY KEY,
    Symbol         TEXT,
    Name           TEXT,
    Description    TEXT,
    IdFood         INTEGER,
    GramsInOneUnit DOUBLE
);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (1, 'g', NULL, NULL, NULL, 1.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (2, 'g', NULL, NULL, NULL, 1.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (3, 'g', NULL, NULL, NULL, 1.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (4, 'g', NULL, NULL, NULL, 1.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (5, 'g', NULL, NULL, NULL, 1.0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
