BEGIN TRANSACTION;
DROP TABLE IF EXISTS 'ModelsOfMeasurementSystem';
CREATE TABLE IF NOT EXISTS 'ModelsOfMeasurementSystem' (
	'IdModelOfMeasurementSystem'	INT NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdModelOfMeasurementSystem')
);
DROP TABLE IF EXISTS 'BolusCalculations';
CREATE TABLE IF NOT EXISTS 'BolusCalculations' (
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
DROP TABLE IF EXISTS 'GlucoseRecords';
CREATE TABLE IF NOT EXISTS 'GlucoseRecords' (
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
DROP TABLE IF EXISTS 'HypoPredictions';
CREATE TABLE IF NOT EXISTS 'HypoPredictions' (
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
DROP TABLE IF EXISTS 'Alarms';
CREATE TABLE IF NOT EXISTS 'Alarms' (
	'IdAlarm'	INT NOT NULL,
	'TimeStart'	DATETIME,
	'TimeAlarm'	DATETIME,
	'Interval'	DOUBLE,
	'Duration'	DOUBLE,
	'IsRepeated'	TINYINT,
	'IsEnabled'	TINYINT,
	PRIMARY KEY('IdAlarm')
);
DROP TABLE IF EXISTS 'Parameters';
CREATE TABLE IF NOT EXISTS 'Parameters' (
	'IdParameters'	INT NOT NULL,
	'Bolus_TargetGlucose'	INT,
	'Bolus_GlucoseBeforeMeal'	INT,
	'Bolus_ChoToEat'	INT,
	'Bolus_ChoInsulinRatioBreakfast'	DOUBLE,
	'Bolus_ChoInsulinRatioLunch'	DOUBLE,
	'Bolus_ChoInsulinRatioDinner'	DOUBLE,
	'Bolus_TotalDailyDoseOfInsulin'	DOUBLE,
	'Bolus_InsulinCorrectionSensitivity'	DOUBLE,
	'Correction_TypicalBolusMorning'	DOUBLE,
	'Correction_TypicalBolusMidday'	DOUBLE,
	'Correction_TypicalBolusEvening'	DOUBLE,
	'Correction_TypicalBolusNight'	DOUBLE,
	'Correction_FactorOfInsulinCorrectionSensitivity'	DOUBLE,
	'Hypo_GlucoseTarget'	DOUBLE,
	'Hypo_GlucoseLast'	DOUBLE,
	'Hypo_GlucosePrevious'	DOUBLE,
	'Hypo_HourLast'	DOUBLE,
	'Hypo_HourPrevious'	DOUBLE,
	'Hypo_MinuteLast'	DOUBLE,
	'Hypo_MinutePrevious'	DOUBLE,
	'Hypo_AlarmAdvanceTime'	DOUBLE,
	'Hypo_FutureSpanMinutes'	DOUBLE,
	'Hit_ChoAlreadyTaken'	DOUBLE,
	'Hit_ChoOfFood'	DOUBLE,
	'Hit_TargetCho'	DOUBLE,
	'Hit_NameOfFood'	TEXT,
	'FoodInMeal_ChoGrams'	DOUBLE,
	'FoodInMeal_QuantityGrams'	DOUBLE,
	'FoodInMeal_ChoPercent'	DOUBLE,
	'FoodInMeal_Name'	TEXT,
	'FoodInMeal_AccuracyOfChoEstimate'	DOUBLE,
	'Meal_ChoGrams'	DOUBLE,
	PRIMARY KEY('IdParameters')
);
DROP TABLE IF EXISTS 'FoodsInMeals';
CREATE TABLE IF NOT EXISTS 'FoodsInMeals' (
	'IdFoodInMeal'	INT NOT NULL,
	'IdMeal'	INT,
	'IdFood'	INT,
	'CarbohydratesGrams'	DOUBLE,
	'CarbohydratesPercent'	INT,
	'Quantity'	DOUBLE,
	'AccuracyOfChoEstimate'	DOUBLE,
	'Name'	TEXT,
	PRIMARY KEY('IdFoodInMeal')
);
DROP TABLE IF EXISTS 'InsulinInjections';
CREATE TABLE IF NOT EXISTS 'InsulinInjections' (
	'IdInsulinInjection'	INT NOT NULL,
	'Timestamp'	DATETIME,
	'InsulinValue'	DOUBLE,
	'InsulinCalculated'	DOUBLE,
	'InjectionPositionX'	INT,
	'InjectionPositionY'	INT,
	'Notes'	VARCHAR(255),
	'IdTypeOfInjection'	INT,
	'IdTypeOfInsulinSpeed'	INT,
	'IdTypeOfInsulinInjection'	INT,
	'InsulinString'	VARCHAR(45),
	PRIMARY KEY('IdInsulinInjection')
);
DROP TABLE IF EXISTS 'Meals';
CREATE TABLE IF NOT EXISTS 'Meals' (
	'IdMeal'	INT NOT NULL,
	'IdTypeOfMeal'	INT,
	'Carbohydrates'	DOUBLE,
	'TimeBegin'	DATETIME,
	'Notes'	VARCHAR(255),
	'AccuracyOfChoEstimate'	DOUBLE,
	'IdBolusCalculation'	INT,
	'IdGlucoseRecord'	INT,
	'IdInsulinInjection'	INT,
	'TimeEnd'	DATETIME,
	PRIMARY KEY('IdMeal')
);
DROP TABLE IF EXISTS 'Foods';
CREATE TABLE IF NOT EXISTS 'Foods' (
	'IdFood'	INT NOT NULL,
	'Name'	VARCHAR(15),
	'Description'	VARCHAR(256),
	'Energy'	DOUBLE,
	'TotalFats'	DOUBLE,
	'SaturatedFats'	DOUBLE,
	'Carbohydrates'	DOUBLE,
	'Sugar'	DOUBLE,
	'Fibers'	INT,
	'Proteins'	INT,
	'Salt'	DOUBLE,
	'Potassium'	DOUBLE,
	'GlycemicIndex'	DOUBLE,
	PRIMARY KEY('IdFood')
);
DROP TABLE IF EXISTS 'InsulinDrugs';
CREATE TABLE IF NOT EXISTS 'InsulinDrugs' (
	'IdInsulinDrug'	INT NOT NULL,
	'Name'	VARCHAR(30),
	'InsulinDuration'	DOUBLE,
	PRIMARY KEY('IdInsulinDrug')
);
DROP TABLE IF EXISTS 'Recipies';
CREATE TABLE IF NOT EXISTS 'Recipies' (
	'IdRecipe'	INTEGER NOT NULL,
	'Name'	TEXT,
	'Description'	TEXT,
	'ChoPercent'	REAL,
	PRIMARY KEY('IdRecipe')
);
DROP TABLE IF EXISTS 'RecipeIngredients';
CREATE TABLE IF NOT EXISTS 'RecipeIngredients' (
	'IdIngredient'	INTEGER NOT NULL,
	'IdRecipe'	INTEGER NOT NULL,
	'Name'	TEXT,
	'Description'	TEXT,
	'QuantityGrams'	REAL,
	'QuantityPercent'	REAL,
	'ChoPercent'	REAL,
	'IdFood'	INTEGER,
	PRIMARY KEY('IdIngredient')
);
COMMIT;
