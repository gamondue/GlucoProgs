--
-- File generated with SQLiteStudio v3.4.17 on mer ago 13 16:39:47 2025
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
CREATE TABLE CategoriesOfFood (IdCategoryOfFood INTEGER PRIMARY KEY, Name TEXT, Description TEXT);
-- Table: Foods
DROP TABLE IF EXISTS Foods;
CREATE TABLE 'Foods' (
	'IdFood' INT NOT NULL,
	'Name' VARCHAR(15),
	'Description' VARCHAR(256),
	'Energy' DOUBLE,
	'TotalFatsPercent' DOUBLE,
	'SaturatedFatsPercent' DOUBLE,
	'MonounsaturatedFatsPercent' DOUBLE,
	'PolyunsaturatedFatsPercent' DOUBLE,
	'CarbohydratesPercent' DOUBLE,
	'SugarPercent' DOUBLE,
	'FibersPercent' INT,
	'ProteinsPercent' INT,
	'SaltPercent' DOUBLE,
	'PotassiumPercent' DOUBLE,
	'Cholesterol' DOUBLE,
	'GlycemicIndex' DOUBLE,
	'UnitSymbol' TEXT,
	'GramsInOneUnit' DOUBLE,
	'Manufacturer' TEXT,
	'Category' REAL,
	PRIMARY KEY ('IdFood'));
-- Table: FoodsInMeals
DROP TABLE IF EXISTS FoodsInMeals;
CREATE TABLE 'FoodsInMeals' (
	'IdFoodInMeal' INT NOT NULL,
	'IdMeal' INT,
	'IdFood' INT,
	'Name' TEXT,
	'CarbohydratesPercent' REAL,
	'UnitSymbol' TEXT,
	'GramsInOneUnit' DOUBLE,
	'QuantityInUnits' DOUBLE,
	'CarbohydratesGrams' DOUBLE,
	'AccuracyOfChoEstimate' DOUBLE,
	PRIMARY KEY('IdFoodInMeal')
);
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
CREATE TABLE Ingredients (IdIngredient INTEGER NOT NULL, IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, QuantityGrams REAL, QuantityPercent REAL, CarbohydratesPercent REAL, AccuracyOfChoEstimate DOUBLE, IdFood INTEGER, PRIMARY KEY (IdIngredient));
-- Table: Injections
DROP TABLE IF EXISTS Injections;
CREATE TABLE 'Injections? (
	'IdInjection' INT NOT NULL,
	'Timestamp' DATETIME,
	'InsulinValue' DOUBLE,
	'InsulinCalculated' DOUBLE,
	'InjectionPositionX' INT,
	'InjectionPositionY' INT,
	'Notes' VARCHAR(255),
	'IdTypeOfInjection' INT,
	'IdTypeOfInsulinAction' INT,
	'IdInsulinDrug' INT,
	'InsulinString' VARCHAR(45),
	'Zone' INTEGER,
	PRIMARY KEY (IdInjection));
-- Table: InsulinDrugs
DROP TABLE IF EXISTS InsulinDrugs;
CREATE TABLE 'InsulinDrugs' (
	'IdInsulinDrug' INT NOT NULL,
	'Name VARCHAR' (32),
	'Manufacturer' VARCHAR (32),
	'TypeOfInsulinAction' INTEGER,
	'DurationInHours' DOUBLE, 
	'OnsetTimeInHours' DOUBLE,
	'PeakTimeInHours' DOUBLE,
	PRIMARY KEY ('IdInsulinDrug'));
-- Table: Manufacturers
DROP TABLE IF EXISTS Manufacturers;
CREATE TABLE Manufacturers (IdManufacturer INTEGER PRIMARY KEY, Name TEXT, Description TEXT);
-- Table: Meals
DROP TABLE IF EXISTS Meals;
CREATE TABLE 'Meals' (
	'IdMeal' INT NOT NULL,
	'IdTypeOfMeal' INT,
	'Carbohydrates' DOUBLE,
	'TimeBegin' DATETIME,
	'Notes' VARCHAR(255),
	'AccuracyOfChoEstimate' DOUBLE,
	'IdBolusCalculation' INT,
	'IdGlucoseRecord' INT,
	'IdInjection' INT,
	'TimeEnd' DATETIME,
	PRIMARY KEY('IdMeal')
);
-- Table: ModelsOfMeasurementSystem
DROP TABLE IF EXISTS ModelsOfMeasurementSystem;
CREATE TABLE 'ModelsOfMeasurementSystem' (
	'IdModelOfMeasurementSystem'	INT NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdModelOfMeasurementSystem')
);
-- Table: Parameters
DROP TABLE IF EXISTS Parameters;
CREATE TABLE 'Parameters' (
	'IdParameters' INT NOT NULL,
	'Timestamp' DATETIME,
	'Bolus_TargetGlucose' INT,
	'Bolus_GlucoseBeforeMeal' INT,
	'Bolus_ChoToEat' INT,
	'Bolus_ChoInsulinRatioBreakfast' DOUBLE,
	'Bolus_ChoInsulinRatioLunch' DOUBLE,
	'Bolus_ChoInsulinRatioDinner' DOUBLE,
	'Bolus_TotalDailyDoseOfInsulin' DOUBLE,
	'Bolus_InsulinCorrectionSensitivity' DOUBLE,
	'Correction_TypicalBolusMorning' DOUBLE,
	'Correction_TypicalBolusMidday' DOUBLE,
	'Correction_TypicalBolusEvening' DOUBLE,
	'Correction_TypicalBolusNight' DOUBLE,
	'Correction_FactorOfInsulinCorrectionSensitivity' DOUBLE,
	'Hypo_GlucoseTarget' DOUBLE,
	'Hypo_GlucoseLast' DOUBLE,
	'Hypo_GlucosePrevious' DOUBLE,
	'Hypo_HourLast' DOUBLE,
	'Hypo_HourPrevious' DOUBLE,
	'Hypo_MinuteLast' DOUBLE,
	'Hypo_MinutePrevious' DOUBLE,
	'Hypo_AlarmAdvanceTime' DOUBLE,
	'Hypo_FutureSpanMinutes' DOUBLE,
	'Hit_ChoAlreadyTaken' DOUBLE,
	'Hit_ChoOfFood' DOUBLE,
	'Hit_TargetCho' DOUBLE,
	'Hit_NameOfFood' TEXT,
	'FoodInMeal_ChoGrams' DOUBLE,
	'FoodInMeal_QuantityGrams' DOUBLE,
	'FoodInMeal_CarbohydratesPercent' DOUBLE,
	'FoodInMeal_Name' TEXT,
	'FoodInMeal_AccuracyOfChoEstimate' DOUBLE,
	'Meal_ChoGrams' DOUBLE,
	'Meal_Breakfast_StartTime_Hours' DOUBLE,
	'Meal_Breakfast_EndTime_Hours' DOUBLE,
	'Meal_Lunch_StartTime_Hours' DOUBLE,
	'Meal_Lunch_EndTime_Hours' DOUBLE,
	'Meal_Dinner_StartTime_Hours' DOUBLE,
	'Meal_Dinner_EndTime_Hours' DOUBLE,
	'Insulin_Short_Id' INTEGER,
	'Insulin_Long_Id' INTEGER,
	PRIMARY KEY('IdParameters')
);
-- Table: PositionsOfReferences
DROP TABLE IF EXISTS PositionsOfReferences;
CREATE TABLE PositionsOfReferences (IdPosition INTEGER PRIMARY KEY, Timestamp DATETIME, Zone INTEGER, PositionX REAL, PositionY REAL, Notes TEXT);
-- Table: Recipes
DROP TABLE IF EXISTS Recipes;
CREATE TABLE Recipes (IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, CarbohydratesPercent REAL, AccuracyOfChoEstimate REAL, IsCooked BOOL, RawToCookedRatio REAL, PRIMARY KEY (IdRecipe));
-- Table: UnitsOfFood
DROP TABLE IF EXISTS UnitsOfFood;
CREATE TABLE UnitsOfFood (IdUnitOfFood INTEGER PRIMARY KEY, Symbol TEXT, Name TEXT, Description TEXT, IdFood INTEGER, GramsInOneUnit DOUBLE);
COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
