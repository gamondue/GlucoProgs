--
-- File generated with SQLiteStudio v3.4.17 on sab lug 26 17:48:21 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Alarms
DROP TABLE IF EXISTS Alarms;
CREATE TABLE 'Alarms' (







	'IdAlarm'	INT NOT NULL,







	'TimeStart'	DATETIME,







	'TimeAlarm'	DATETIME,







	'Interval' DOUBLE,







	'Duration' DOUBLE,







	'IsRepeated' TINYINT,







	'IsEnabled'	TINYINT,







	PRIMARY KEY('IdAlarm')







);

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
CREATE TABLE CategoriesOfFood (IfCategoryOfFood INTEGER PRIMARY KEY, Name TEXT, Description TEXT);

-- Table: Foods
DROP TABLE IF EXISTS Foods;
CREATE TABLE Foods (IdFood INT NOT NULL, Name VARCHAR (15), Description VARCHAR (256), Energy DOUBLE, TotalFatsPercent DOUBLE, SaturatedFatsPercent DOUBLE, MonounsaturatedFatsPercent DOUBLE, PolyunsaturatedFatsPercent DOUBLE, CarbohydratesPercent DOUBLE, SugarPercent DOUBLE, FibersPercent INT, ProteinsPercent INT, SaltPercent DOUBLE, PotassiumPercent DOUBLE, Cholesterol DOUBLE, GlycemicIndex DOUBLE, UnitSymbol TEXT, GramsInOneUnit DOUBLE, Manufacturer TEXT, Category REAL, PRIMARY KEY (IdFood));
INSERT INTO Foods (IdFood, Name, Description, Energy, TotalFatsPercent, SaturatedFatsPercent, MonounsaturatedFatsPercent, PolyunsaturatedFatsPercent, CarbohydratesPercent, SugarPercent, FibersPercent, ProteinsPercent, SaltPercent, PotassiumPercent, Cholesterol, GlycemicIndex, UnitSymbol, GramsInOneUnit, Manufacturer, Category) VALUES (2, 'Uovo', '1 piccolo 40 g, 1 medio 50 g, 1 grande 60 g', NULL, NULL, NULL, NULL, NULL, 1.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', 60.0, '', '');
INSERT INTO Foods (IdFood, Name, Description, Energy, TotalFatsPercent, SaturatedFatsPercent, MonounsaturatedFatsPercent, PolyunsaturatedFatsPercent, CarbohydratesPercent, SugarPercent, FibersPercent, ProteinsPercent, SaltPercent, PotassiumPercent, Cholesterol, GlycemicIndex, UnitSymbol, GramsInOneUnit, Manufacturer, Category) VALUES (1, 'Zucchero', '', NULL, NULL, NULL, NULL, NULL, 100.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2', 1.034, '', '');
INSERT INTO Foods (IdFood, Name, Description, Energy, TotalFatsPercent, SaturatedFatsPercent, MonounsaturatedFatsPercent, PolyunsaturatedFatsPercent, CarbohydratesPercent, SugarPercent, FibersPercent, ProteinsPercent, SaltPercent, PotassiumPercent, Cholesterol, GlycemicIndex, UnitSymbol, GramsInOneUnit, Manufacturer, Category) VALUES (4, 'Parmigiano', 'Descrizione', 1.0, 2.0, 3.0, 4.0, 5.0, 0.0, 7.0, 8, 9, 10.0, 12.0, 11.0, 13.0, 'g', 1.0, 'Conad', 'Formaggi');
INSERT INTO Foods (IdFood, Name, Description, Energy, TotalFatsPercent, SaturatedFatsPercent, MonounsaturatedFatsPercent, PolyunsaturatedFatsPercent, CarbohydratesPercent, SugarPercent, FibersPercent, ProteinsPercent, SaltPercent, PotassiumPercent, Cholesterol, GlycemicIndex, UnitSymbol, GramsInOneUnit, Manufacturer, Category) VALUES (5, 'Spaghetti', 'crudi', 1.0, NULL, NULL, NULL, NULL, 70.0, NULL, NULL, NULL, NULL, NULL, NULL, 12.0, NULL, 1.0, '', '');
INSERT INTO Foods (IdFood, Name, Description, Energy, TotalFatsPercent, SaturatedFatsPercent, MonounsaturatedFatsPercent, PolyunsaturatedFatsPercent, CarbohydratesPercent, SugarPercent, FibersPercent, ProteinsPercent, SaltPercent, PotassiumPercent, Cholesterol, GlycemicIndex, UnitSymbol, GramsInOneUnit, Manufacturer, Category) VALUES (3, 'Latte', '', NULL, NULL, NULL, NULL, NULL, 4.8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'g', NULL, 'Conad', '');

-- Table: FoodsInMeals
DROP TABLE IF EXISTS FoodsInMeals;
CREATE TABLE FoodsInMeals (IdFoodInMeal INT NOT NULL, IdMeal INT, IdFood INT, Name TEXT, CarbohydratesPercent REAL, UnitSymbol TEXT, GramsInOneUnit DOUBLE, QuantityInUnits DOUBLE, CarbohydratesGrams DOUBLE, AccuracyOfChoEstimate DOUBLE, PRIMARY KEY (IdFoodInMeal));
INSERT INTO FoodsInMeals (IdFoodInMeal, IdMeal, IdFood, Name, CarbohydratesPercent, UnitSymbol, GramsInOneUnit, QuantityInUnits, CarbohydratesGrams, AccuracyOfChoEstimate) VALUES (1, 1, 1, 'Uovo', 1.0, 'piccolo', 40.0, 9.0, 3.6, 70.0);
INSERT INTO FoodsInMeals (IdFoodInMeal, IdMeal, IdFood, Name, CarbohydratesPercent, UnitSymbol, GramsInOneUnit, QuantityInUnits, CarbohydratesGrams, AccuracyOfChoEstimate) VALUES (2, 1, 2, 'Zucchero', 100.0, 'g', 1.034, 15.0, 15.5, 80.0);

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
INSERT INTO GlucoseRecords (IdGlucoseRecord, GlucoseValue, Timestamp, GlucoseString, IdTypeOfGlucoseMeasurement, IdTypeOfGlucoseMeasurementDevice, IdModelOfMeasurementSystem, IdDevice, IdDocumentType, Notes) VALUES (1, 99.0, '2025-04-09 07:45:00', NULL, 0, 0, NULL, NULL, NULL, NULL);
INSERT INTO GlucoseRecords (IdGlucoseRecord, GlucoseValue, Timestamp, GlucoseString, IdTypeOfGlucoseMeasurement, IdTypeOfGlucoseMeasurementDevice, IdModelOfMeasurementSystem, IdDevice, IdDocumentType, Notes) VALUES (2, 161.0, '2025-05-31 00:47:00', NULL, 0, 0, NULL, NULL, NULL, '');
INSERT INTO GlucoseRecords (IdGlucoseRecord, GlucoseValue, Timestamp, GlucoseString, IdTypeOfGlucoseMeasurement, IdTypeOfGlucoseMeasurementDevice, IdModelOfMeasurementSystem, IdDevice, IdDocumentType, Notes) VALUES (3, NULL, '0001-01-01 00:00:00', NULL, 0, 0, NULL, NULL, 0, NULL);
INSERT INTO GlucoseRecords (IdGlucoseRecord, GlucoseValue, Timestamp, GlucoseString, IdTypeOfGlucoseMeasurement, IdTypeOfGlucoseMeasurementDevice, IdModelOfMeasurementSystem, IdDevice, IdDocumentType, Notes) VALUES (4, 82.0, '2025-06-24 11:52:00', NULL, 0, 0, NULL, NULL, 0, NULL);

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
CREATE TABLE Ingredients (IdIngredient INTEGER NOT NULL, IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, QuantityGrams REAL, QuantityPercent REAL, CarbohydratesPercent REAL, AccuracyOfChoEstimate DOUBLE, IdFood INTEGER, PRIMARY KEY (IdIngredient));
INSERT INTO Ingredients (IdIngredient, IdRecipe, Name, Description, QuantityGrams, QuantityPercent, CarbohydratesPercent, AccuracyOfChoEstimate, IdFood) VALUES (1, 1, 'Uovo', '1 piccolo 40 g, 1 medio 50 g, 1 grande 60 g', NULL, NULL, NULL, NULL, NULL);
INSERT INTO Ingredients (IdIngredient, IdRecipe, Name, Description, QuantityGrams, QuantityPercent, CarbohydratesPercent, AccuracyOfChoEstimate, IdFood) VALUES (2, 3, 'Spaghetti', '', NULL, NULL, NULL, 80.0, NULL);
INSERT INTO Ingredients (IdIngredient, IdRecipe, Name, Description, QuantityGrams, QuantityPercent, CarbohydratesPercent, AccuracyOfChoEstimate, IdFood) VALUES (3, 1, 'Guanciale', '', NULL, NULL, NULL, NULL, NULL);

-- Table: Injections
DROP TABLE IF EXISTS Injections;
CREATE TABLE Injections (IdInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX INT, InjectionPositionY INT, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinAction INT, IdInsulinDrug INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInjection));
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (1, '2025-05-21 22:05:00', 10.0, NULL, 0.4703010103195, 0.39919410296242, '', 10, 10, NULL, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (3, '2025-05-30 16:30:00', 8.0, NULL, 0.60854472179121, 0.21462023124877, 'boh', 10, 10, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (4, '2025-06-02 16:49:00', 8.0, NULL, 0.26701570048919, 0.54271089147897, 'nota', 10, 10, 3, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (5, '2025-06-05 16:50:00', 3.0, NULL, 0.52190754270773, 0.20260308458939, '', 10, 10, 4, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (6, '2025-06-08 16:51:00', 9.0, NULL, 0.43178688922463, 0.64978976107409, '', 10, 10, 2, '', 2);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (8, '2025-06-11 20:18:00', 10.0, NULL, 0.68878191649265, 0.2340159448403, '', 30, 40, NULL, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (9, '2025-06-14 18:38:00', 8.0, NULL, 0.27004337738741, 0.23254945543077, '', 10, 10, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (10, '2025-06-16 01:07:00', 11.0, NULL, 0.33775136470795, 0.2954974865669, '', 10, 10, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (11, '2025-06-18 01:08:00', 13.0, NULL, 0.4262767791748, 0.26816005902269, 'boh', 10, 10, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (12, '2025-06-20 01:10:00', 13.0, NULL, 0.57161426544189, 0.26425867932433, 'boh', 10, 20, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (13, '2025-06-20 12:59:00', 9.0, NULL, 0.38934786219847, 0.21962421659439, '', 10, 20, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (14, '2025-06-20 13:01:00', 9.0, NULL, 0.59370525299557, 0.76161545537218, '', 10, 10, 2, '', 2);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (16, '2025-06-20 20:31:00', 13.0, NULL, 0.47939794203814, 0.16127298009677, '', 10, 10, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (17, '2025-06-20 20:35:00', 16.0, NULL, 0.38576374158182, 0.71628463058432, '', 10, 20, 2, '', 2);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (18, '2025-06-20 22:41:00', 11.0, NULL, 0.21813912730399, 0.52593410937037, '', 10, 10, 1, '', 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (20, '2025-06-20 21:44:00', NULL, NULL, 0.23577755438423, 0.11670375381857, NULL, 20, NULL, NULL, NULL, 3);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (21, '2025-06-20 21:57:00', NULL, NULL, 0.16052718784498, 0.51114837209053, '', 30, NULL, NULL, '', 4);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (22, '2025-06-20 21:57:00', NULL, NULL, 0.85556180215295, 0.59319179842865, '', 30, NULL, NULL, '', 4);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (23, '2025-06-20 22:01:00', 9.0, NULL, 0.5766866715228, 0.68708180729164, '', 10, 20, 1, '', 2);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (24, '2025-06-20 22:02:00', 15.0, NULL, 0.321103184601, 0.58120262956333, '', 10, 40, 2, '', 1);

-- Table: InsulinDrugs
DROP TABLE IF EXISTS InsulinDrugs;
CREATE TABLE InsulinDrugs (IdInsulinDrug INT NOT NULL, Name VARCHAR (32), Manufacturer VARCHAR (32), TypeOfInsulinAction INTEGER, DurationInHours DOUBLE, OnsetTimeInHours DOUBLE, PeakTimeInHours DOUBLE, PRIMARY KEY (IdInsulinDrug));
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (2, 'Toujeo', 'Sanofi', 40, 36.0, 3.5, 0.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (1, 'Humalog', 'Lilly', 20, 4.0, 0.25, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (4, 'Fiasp', 'Novo Nordisk', 20, 3.0, 0.03, 1.5);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (3, 'Lispro', '', 20, 4.5, 0.3333333, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (5, 'Lantus', '', 40, 24.0, 4.0, 0.0);

-- Table: Manufacturers
DROP TABLE IF EXISTS Manufacturers;
CREATE TABLE Manufacturers (IdManufacturer INTEGER PRIMARY KEY, Name TEXT, Description TEXT);

-- Table: Meals
DROP TABLE IF EXISTS Meals;
CREATE TABLE Meals (IdMeal INT NOT NULL, IdTypeOfMeal INT, Carbohydrates DOUBLE, TimeBegin DATETIME, Notes VARCHAR (255), AccuracyOfChoEstimate DOUBLE, IdBolusCalculation INT, IdGlucoseRecord INT, IdInjection INT, TimeEnd DATETIME, PRIMARY KEY (IdMeal));
INSERT INTO Meals (IdMeal, IdTypeOfMeal, Carbohydrates, TimeBegin, Notes, AccuracyOfChoEstimate, IdBolusCalculation, IdGlucoseRecord, IdInjection, TimeEnd) VALUES (1, 20, 19.1, '2025-06-23 12:13:17', 'Prova', 78.2, NULL, NULL, NULL, '2025-06-23 12:13:17');
INSERT INTO Meals (IdMeal, IdTypeOfMeal, Carbohydrates, TimeBegin, Notes, AccuracyOfChoEstimate, IdBolusCalculation, IdGlucoseRecord, IdInjection, TimeEnd) VALUES (2, 40, 15.0, '2025-06-23 11:14:01', 'Prova', 100.0, NULL, NULL, NULL, '2025-06-23 11:14:01');

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
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (1, NULL, 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 2.3, '', '', '', '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (2, '2025-06-11 16:28:43.2657299', 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, 2.3, NULL, '', NULL, NULL, 6.75, 10.0, 11.75, 14.25, 18.25, 21.3, 1, 2);
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (3, '2025-06-11 16:49:33.6212069', 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, 2.3, NULL, '', NULL, NULL, 6.75, 10.0, 11.75, 14.25, 18.25, 21.3, 4, 2);
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (4, '2025-06-11 16:50:21.1522557', 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, 2.3, NULL, '', NULL, NULL, 6.75, 10.0, 11.75, 14.25, 18.25, 21.3, 3, 2);
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (5, '2025-06-11 16:50:53.8949135', 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, 2.3, NULL, '', NULL, NULL, 6.75, 10.0, 11.75, 14.25, 18.25, 21.3, 1, 5);
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (6, '2025-06-11 20:21:30.0565891', 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 19.1, 100.0, 44.2, 'Zucchero', 15.5, '', 100.0, 'Zucchero', 80.0, '', 6.75, 10.0, 11.75, 14.25, 18.25, 21.3, 1, 2);

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
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (103, '2025-06-11 22:40:19', 1, 0.47134432390245, 0.32415183691717, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (104, '2025-06-11 22:40:19', 1, 0.52185616729404, 0.32366025493735, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (105, '2025-06-11 22:40:19', 1, 0.4703010103195, 0.39919410296242, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (106, '2025-06-11 22:40:19', 1, 0.52083252438597, 0.3986836073942, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (107, '2025-06-11 22:40:19', 1, 0.38934784692763, 0.21962421166827, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (108, '2025-06-11 22:40:19', 1, 0.43317937556795, 0.21261107364561, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (109, '2025-06-11 22:40:19', 1, 0.47491988953124, 0.20658353230862, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (110, '2025-06-11 22:40:19', 1, 0.5219075377368, 0.20260307854286, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (111, '2025-06-11 22:40:19', 1, 0.60854472179121, 0.21462023124877, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (112, '2025-06-11 22:40:19', 1, 0.56573902981453, 0.20859268991179, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (113, '2025-06-11 22:40:19', 1, 0.2181391190565, 0.52593412739796, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (114, '2025-06-11 22:40:19', 1, 0.26701570048919, 0.54271089147897, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (115, '2025-06-11 22:40:19', 1, 0.32110318263476, 0.58120264792979, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (116, '2025-06-11 22:40:19', 1, 0.27367533909428, 0.19887574820888, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (117, '2025-06-11 22:40:19', 1, 0.31640258602831, 0.19544473075993, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (118, '2025-06-11 22:40:19', 1, 0.39869748688482, 0.17837269883818, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (119, '2025-06-11 22:40:19', 1, 0.35910023554353, 0.18764195042132, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (120, '2025-06-11 22:40:19', 1, 0.43826523221378, 0.16860537229514, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (121, '2025-06-11 22:40:19', 1, 0.47939792963588, 0.16127298180876, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (122, '2025-06-11 22:40:19', 1, 0.51899514441456, 0.15980647630049, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (123, '2025-06-11 22:40:19', 1, 0.55859235919325, 0.16470399925771, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (124, '2025-06-11 22:40:19', 1, 0.60025664015074, 0.17593779704527, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (125, '2025-06-11 22:40:19', 1, 0.64085779060004, 0.18813999111747, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (126, '2025-06-11 22:40:19', 1, 0.68255150444466, 0.19594277145608, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (127, '2025-06-11 22:40:19', 1, 0.72941268264203, 0.20081260930564, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (128, '2025-06-11 22:40:19', 1, 0.7325425868283, 0.23108298521937, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (129, '2025-06-11 22:40:19', 1, 0.68878191649265, 0.2340159448403, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (130, '2025-06-11 22:40:19', 1, 0.65024772253438, 0.23498437538868, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (131, '2025-06-11 22:40:19', 1, 0.61484350588756, 0.26232182594524, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (132, '2025-06-11 22:40:19', 1, 0.57161427283694, 0.26425868704199, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (133, '2025-06-11 22:40:19', 1, 0.4262767968156, 0.26816006007942, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (134, '2025-06-11 22:40:19', 1, 0.3841105845854, 0.26768968735907, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (135, '2025-06-11 22:40:19', 1, 0.34660986887265, 0.24278717285917, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (136, '2025-06-11 22:40:19', 1, 0.31014259484282, 0.23888576555799, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (137, '2025-06-11 22:40:19', 1, 0.27004337566623, 0.2325494564639, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (138, '2025-06-11 22:40:19', 1, 0.26274994279782, 0.2701245720201, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (139, '2025-06-11 22:40:19', 1, 0.25752344813914, 0.30769972184005, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (140, '2025-06-11 22:40:19', 1, 0.29452223257919, 0.32576785228597, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (141, '2025-06-11 22:40:19', 1, 0.30075264462717, 0.28329523090442, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (142, '2025-06-11 22:40:19', 1, 0.33775137422333, 0.29549747637223, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (143, '2025-06-11 22:40:19', 1, 0.33098945176513, 0.34380836615172, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (144, '2025-06-11 22:40:19', 1, 0.37265365959744, 0.362346869318, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (145, '2025-06-11 22:40:19', 1, 0.4184813595493, 0.38381838350083, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (146, '2025-06-11 22:40:19', 1, 0.42161130029815, 0.31940384095235, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (147, '2025-06-11 22:40:19', 1, 0.3768171151544, 0.30769972184005, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (148, '2025-06-11 22:40:19', 1, 0.57524625454628, 0.31500441008689, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (149, '2025-06-11 22:40:19', 1, 0.57940974666584, 0.38284995295245, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (150, '2025-06-11 22:40:19', 1, 0.62107391793555, 0.30379834880262, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (151, '2025-06-11 22:40:19', 1, 0.62680239871083, 0.36284489288228, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (152, '2025-06-11 22:40:19', 1, 0.66796463870784, 0.3447767967001, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (153, '2025-06-11 22:40:19', 1, 0.70283730838185, 0.32623822500633, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (154, '2025-06-11 22:40:19', 1, 0.69554387551344, 0.27939384073511, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (155, '2025-06-11 22:40:19', 1, 0.66013965886662, 0.29109801124303, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (156, '2025-06-11 22:40:19', 1, 0.73723766248325, 0.26572515828652, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (157, '2025-06-11 22:40:19', 1, 0.74346807453124, 0.30623323346365, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (158, '2025-06-11 22:40:19', 1, 0.21803630504581, 0.59203163667709, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (159, '2025-06-11 22:40:19', 1, 0.27074545057143, 0.62046383108576, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (160, '2025-06-11 22:40:19', 1, 0.32662435350938, 0.64592556420228, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (161, '2025-06-11 22:40:19', 1, 0.22437589299562, 0.66391848572329, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (162, '2025-06-11 22:40:19', 1, 0.27391528110893, 0.68632478345483, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (163, '2025-06-11 22:40:19', 1, 0.32979418404688, 0.71178644804386, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (164, '2025-06-11 22:40:19', 1, 0.38893344650187, 0.7447169242284, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (165, '2025-06-11 22:40:19', 1, 0.23397584046249, 0.73274989938451, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (166, '2025-06-11 22:40:19', 1, 0.25797574569225, 0.87194047866318, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (167, '2025-06-11 22:40:19', 1, 0.35062425873914, 0.91836573164784, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (168, '2025-06-11 22:40:19', 1, 0.29791511321351, 0.89146115214043, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (169, '2025-06-11 22:40:19', 1, 0.40487298191855, 0.93780149955975, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (170, '2025-06-11 22:40:19', 1, 0.40333340426476, 0.87643869191156, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (171, '2025-06-11 22:40:19', 1, 0.34419414180977, 0.84800649750289, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (172, '2025-06-11 22:40:19', 1, 0.28505480622959, 0.81957430309423, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (173, '2025-06-11 22:40:19', 1, 0.24194560817083, 0.79708316832484, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (174, '2025-06-11 22:40:19', 1, 0.2803453980383, 0.75668388054479, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (175, '2025-06-11 22:40:19', 1, 0.33785448073477, 0.78367329709006, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (176, '2025-06-11 22:40:19', 1, 0.40007304474771, 0.80760734677783, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (177, '2025-06-11 22:40:19', 1, 0.69884876826841, 0.57403878368358, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (178, '2025-06-11 22:40:19', 1, 0.75155798691922, 0.54407883731868, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (179, '2025-06-11 22:40:19', 1, 0.79946697589845, 0.52761359922641, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (180, '2025-06-11 22:40:19', 1, 0.79629729161132, 0.59050388472086, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (181, '2025-06-11 22:40:19', 1, 0.7451276506143, 0.61596561783738, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (182, '2025-06-11 22:40:19', 1, 0.68924889392673, 0.64295503438264, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (183, '2025-06-11 22:40:19', 1, 0.63490956864257, 0.72977936956487, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (184, '2025-06-11 22:40:19', 1, 0.68924889392673, 0.70728816626799, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (185, '2025-06-11 22:40:19', 1, 0.74675790349801, 0.68632478345483, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (186, '2025-06-11 22:40:19', 1, 0.79312746107382, 0.65942027247491, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (187, '2025-06-11 22:40:19', 1, 0.78189733384843, 0.72825168613613, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (188, '2025-06-11 22:40:19', 1, 0.61733978034218, 0.80905012464123, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (189, '2025-06-11 22:40:19', 1, 0.67964887333467, 0.78367329709006, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (190, '2025-06-11 22:40:19', 1, 0.73878813578967, 0.75371335072515, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (191, '2025-06-11 22:40:19', 1, 0.61253973348356, 0.87643869191156, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (192, '2025-06-11 22:40:19', 1, 0.60937004919643, 0.94832567801274, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (193, '2025-06-11 22:40:19', 1, 0.66524891557178, 0.92286387636873, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (194, '2025-06-11 22:40:19', 1, 0.67484878991346, 0.84800649750289, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (195, '2025-06-11 22:40:19', 1, 0.73235794573512, 0.82110205505047, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (196, '2025-06-11 22:40:19', 1, 0.71958824085593, 0.89443168196006, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (197, '2025-06-11 22:40:19', 1, 0.76595779843174, 0.87194047866318, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (198, '2025-06-11 22:40:19', 1, 0.77555767277342, 0.79861085175358, '');
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

-- Table: Recipes
DROP TABLE IF EXISTS Recipes;
CREATE TABLE Recipes (IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, CarbohydratesPercent REAL, AccuracyOfChoEstimate REAL, IsCooked BOOL, RawToCookedRatio REAL, PRIMARY KEY (IdRecipe));
INSERT INTO Recipes (IdRecipe, Name, Description, CarbohydratesPercent, AccuracyOfChoEstimate, IsCooked, RawToCookedRatio) VALUES (1, 'Salsa carbonara', 'Solo il sugo', NULL, NULL, 1, NULL);
INSERT INTO Recipes (IdRecipe, Name, Description, CarbohydratesPercent, AccuracyOfChoEstimate, IsCooked, RawToCookedRatio) VALUES (2, '', '', NULL, NULL, 1, NULL);
INSERT INTO Recipes (IdRecipe, Name, Description, CarbohydratesPercent, AccuracyOfChoEstimate, IsCooked, RawToCookedRatio) VALUES (3, 'Spaghetti alla carbonara', '(una porzione abbondante)', NULL, NULL, 0, NULL);

-- Table: UnitsOfFood
DROP TABLE IF EXISTS UnitsOfFood;
CREATE TABLE UnitsOfFood (IdUnitOfFood INTEGER PRIMARY KEY, Symbol TEXT, Name TEXT, Description TEXT, IdFood INTEGER, GramsInOneUnit DOUBLE);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (1, 'g', 'grams', NULL, NULL, 1.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (2, 'bustina', 'bustina', NULL, 1, 5.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (3, 'piccolo', NULL, NULL, 2, 40.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (4, 'medio', NULL, NULL, 2, 50.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (5, 'grande', NULL, NULL, 2, 60.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (7, 'cucchiaio', 'cucchiaio', NULL, 1, 7.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (8, 'Bicchiere', NULL, NULL, 3, 200.0);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (9, 'tazza', NULL, NULL, 3, 80.0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
