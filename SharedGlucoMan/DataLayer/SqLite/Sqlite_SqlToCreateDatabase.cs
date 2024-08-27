using gamon;
using System;
using System.Data.Common;
using System.IO;

namespace GlucoMan
{
  internal partial class DL_Sqlite : DataLayer
  {
		string creationScript = @"
--
-- File generated with SQLiteStudio v3.4.4 on mar ago 27 09:56:20 2024
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







	'Interval'	DOUBLE,







	'Duration'	DOUBLE,







	'IsRepeated'	TINYINT,







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

-- Table: Foods
DROP TABLE IF EXISTS Foods;
CREATE TABLE Foods (IdFood INT NOT NULL, Name VARCHAR (15), Description VARCHAR (256), Energy DOUBLE, TotalFats DOUBLE, SaturatedFats DOUBLE, MonounsaturatedFats DOUBLE, PolyunsaturatedFats DOUBLE, Carbohydrates DOUBLE, Sugar DOUBLE, Fibers INT, Proteins INT, Salt DOUBLE, Potassium DOUBLE, Cholesterol DOUBLE, GlycemicIndex DOUBLE, PRIMARY KEY (IdFood));

-- Table: FoodsInMeals
DROP TABLE IF EXISTS FoodsInMeals;
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

-- Table: InsulinDrugs
DROP TABLE IF EXISTS InsulinDrugs;
CREATE TABLE 'InsulinDrugs' (







	'IdInsulinDrug'	INT NOT NULL,







	'Name'	VARCHAR(30),







	'InsulinDuration'	DOUBLE,







	PRIMARY KEY('IdInsulinDrug')







);

-- Table: InsulinInjections
DROP TABLE IF EXISTS InsulinInjections;
CREATE TABLE InsulinInjections (IdInsulinInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX INT, InjectionPositionY INT, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinSpeed INT, IdTypeOfInsulinInjection INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInsulinInjection));

-- Table: Meals
DROP TABLE IF EXISTS Meals;
CREATE TABLE 'Meals' (







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

-- Table: RecipeIngredients
DROP TABLE IF EXISTS RecipeIngredients;
CREATE TABLE 'RecipeIngredients' (







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

-- Table: Recipes
DROP TABLE IF EXISTS Recipes;
CREATE TABLE 'Recipes' (







	'IdRecipe'	INTEGER NOT NULL,







	'Name'	TEXT,







	'Description'	TEXT,







	'ChoPercent'	REAL,







	PRIMARY KEY('IdRecipe')







);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
";
	internal override void CreateNewDatabase(string dbName)
	{
		// making new, means erasing existent! 
		if (File.Exists(dbName))
			File.Delete(dbName);

		//when the file does not exist
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
