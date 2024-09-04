--
-- File generato con SQLiteStudio v3.4.4 su mer set 4 00:07:36 2024
--
-- Codifica del testo utilizzata: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Tabella: Alarms
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

-- Tabella: BolusCalculations
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

-- Tabella: Foods
CREATE TABLE 'Foods' (
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

-- Tabella: FoodsInMeals
CREATE TABLE 'FoodsInMeals' (
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

-- Tabella: GlucoseRecords
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

-- Tabella: HypoPredictions
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

-- Tabella: InsulinDrugs
CREATE TABLE 'InsulinDrugs' (
	'IdInsulinDrugs'	INT NOT NULL,
	'Name'	VARCHAR(30),
	'InsulinSpeed'	DOUBLE,
	PRIMARY KEY('IdInsulinDrugs')
);

-- Tabella: InsulinInjections
CREATE TABLE 'InsulinInjections' (
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

-- Tabella: Meals
CREATE TABLE 'Meals' (
	'IdMeal'	INT NOT NULL,
	'IdTypeOfMeal'	INT,
	'Carbohydrates'	DOUBLE,
	'TimeBegin'	DATETIME,
	'Notes'	VARCHAR(255),
	'AccuracyOfChoEstimate'	DOUBLE,
	'IdBolusCalculation' INT,
	'IdGlucoseRecord' INT,
	'IdInsulinInjection' INT,
	'TimeEnd'	DATETIME,
	PRIMARY KEY('IdMeal')
);

-- Tabella: ModelsOfMeasurementSystem
CREATE TABLE 'ModelsOfMeasurementSystem' (
	'IdModelOfMeasurementSystem'	INT NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdModelOfMeasurementSystem')
);

-- Tabella: Parameters
CREATE TABLE 'Parameters' (
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

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
