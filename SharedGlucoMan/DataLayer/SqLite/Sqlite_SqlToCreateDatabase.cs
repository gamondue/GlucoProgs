using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
		string creationScript = @"
CREATE TABLE 'BolusCalculations' (
	'IdBolusCalculation'	INTEGER NOT NULL,
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
	PRIMARY KEY('IdBolusCalculation' AUTOINCREMENT)
);

CREATE TABLE 'Foods' (
	'IdFood'	INTEGER NOT NULL,
	'Name'	VARCHAR(15),
	'Description'	VARCHAR(256),
	'Energy'	DOUBLE,
	'TotalFats'	DOUBLE,
	'SaturatedFats'	DOUBLE,
	'Carbohydrates'	DOUBLE NOT NULL,
	'Sugar'	DOUBLE,
	'Fibers'	DOUBLE,
	'Proteins'	DOUBLE,
	'Salt'	DOUBLE,
	'Potassium'	DOUBLE,
	'GlycemicIndex'	DOUBLE,
	'AccuracyOfChoEstimate'	DOUBLE,
	'QualitativeAccuracyOfChoEstimate'	INTEGER,
	PRIMARY KEY('IdFood' AUTOINCREMENT)
);

CREATE TABLE 'FoodsInMeals' (
	'IdFoodInMeal' INT NOT NULL,
	'IdMeal' INT NOT NULL,
	'IdFood' INT NULL,
	'Quantity' DOUBLE NULL,
	'CarbohydratesGrams' DOUBLE NULL,
	'CarbohydratesPercent' DOUBLE NULL,
	'Name'	VARCHAR(15),
	'Description'	VARCHAR(256),
	'SugarPercent' DOUBLE NULL,
	'FiberPercent' DOUBLE NULL,
	'AccuracyOfChoEstimate' DOUBLE NULL,
	'QualitativeAccuracy' INT NULL,
	PRIMARY KEY('IdFoodInMeal')
)

CREATE TABLE 'GlucoseRecords' (
	'IdGlucoseRecord'	INTEGER NOT NULL,
	'Timestamp'	DATETIME,
	'GlucoseValue'	DOUBLE,
	'InsulinDrugName'	VARCHAR(45),
	'IdTypeOfGlucoseMeasurement'	INTEGER,
	'IdTypeOfGlucoseMeasurementDevice'	INTEGER,
	'IdModelOfMeasurementSystem'	INTEGER,
	'IdDevice'	VARCHAR(45),
	'IdDeviceType'	VARCHAR(45),
	'IdDocumentType'	INTEGER,
	'GlucoseString'	VARCHAR(45),
	'TypeOfGlucoseMeasurement'	INTEGER,
	'Notes'	VARCHAR(255),
	PRIMARY KEY('IdGlucoseRecord' AUTOINCREMENT)
);

CREATE TABLE 'HypoPredictions' (
	'IdHypoPrediction'	INTEGER NOT NULL,
	'PredictedTime'	DATETIME,
	'AlarmTime'	DATETIME,
	'GlucoseSlope'	DOUBLE,
	'HypoGlucoseTarget'	INTEGER,
	'GlucoseLast'	DOUBLE,
	'GlucosePrevious'	DOUBLE,
	'Interval'	VARCHAR(10),
	'DatetimeLast'	DATETIME,
	'DatetimePrevious'	DATETIME,
	'HypoPredictionscol'	VARCHAR(45),
	PRIMARY KEY('IdHypoPrediction' AUTOINCREMENT)
);

CREATE TABLE 'InsulineInjections' (
	'IdInsulineInjection'	INTEGER NOT NULL,
	'Timestamp'	VARCHAR(45),
	'InsulinValue'	DOUBLE,
	'InjectionPositionX'	INTEGER,
	'InjectionPositionY'	INTEGER,
	'IdTypeOfInjection'	INTEGER,
	'IdTypeOfInsulinSpeed'	INTEGER,
	'IdTypeOfInsulinInjection'	INTEGER,
	'InsulinString'	VARCHAR(45),
	PRIMARY KEY('IdInsulineInjection' AUTOINCREMENT)
);

CREATE TABLE 'Meals' (
	'IdMeal'	INTEGER NOT NULL,
	'IdTypeOfMeal'	INTEGER,
	'TimeBegin'	DATETIME,
	'TimeEnd'	DATETIME,
	'Carbohydrates'	DOUBLE,
	'AccuracyOfChoEstimate'	DOUBLE,
	'IdBolusCalculation'	INTEGER,
	'IdGlucoseRecord'	INTEGER,
	'IdQualitativeAccuracyCHO'	INTEGER,
	'IdInsulineInjection'	INTEGER,
	PRIMARY KEY('IdMeal' AUTOINCREMENT)
);

CREATE TABLE 'ModelsOfMeasurementSystem' (
	'IdModelOfMeasurementSystem'	INTEGER NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdModelOfMeasurementSystem')
);

CREATE TABLE 'Parameters' (
	'IdParameters'	INTEGER NOT NULL,
	'TargetGlucose'	INTEGER,
	'GlucoseBeforeMeal'	INTEGER,
	'ChoToEat'	INTEGER,
	'ChoInsulinRatioBreakfast'	DOUBLE,
	'ChoInsulinRatioLunch'	DOUBLE,
	'ChoInsulinRatioDinner'	DOUBLE,
	'TypicalBolusMorning'	DOUBLE,
	'TypicalBolusMidday'	DOUBLE,
	'TypicalBolusEvening'	DOUBLE,
	'TypicalBolusNight'	DOUBLE,
	'TotalDailyDoseOfInsulin'	DOUBLE,
	'FactorOfInsulinCorrectionSensitivity'	DOUBLE,
	'InsulinCorrectionSensitivity'	DOUBLE,
	'Hypo_GlucoseTarget' DOUBLE,
	'Hypo_GlucoseLast' DOUBLE,
	'Hypo_GlucosePrevious' DOUBLE,
	'Hypo_HourLast' INTEGER,
	'Hypo_HourPrevious' INTEGER,
	'Hypo_MinuteLast' INTEGER,
	'Hypo_MinutePrevious' INTEGER,
	'Hypo_AlarmAdvanceTime' DATETIME,
	'Hit_ChoAlreadyTaken' DOUBLE,
	'Hit_ChoOfFood' DOUBLE,
	'Hit_TargetCho' INTEGER,
	PRIMARY KEY('IdParameters')
);

CREATE TABLE 'QualitativeAccuracies' (
	'IdQualitativeAccuracy'	INTEGER NOT NULL,
	'name'	VARCHAR(255) NOT NULL,
	PRIMARY KEY('IdQualitativeAccuracy' AUTOINCREMENT)
);

CREATE TABLE 'TypesOfGlucoseMeasurement' (
	'IdTypeOfGlucoseMeasurement'	INTEGER NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdTypeOfGlucoseMeasurement' AUTOINCREMENT)
);

CREATE TABLE 'TypesOfGlucoseMeasurementDevice' (
	'IdTypeOfGlucoseMeasurementDevice'	INTEGER NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdTypeOfGlucoseMeasurementDevice' AUTOINCREMENT)
);

CREATE TABLE 'TypesOfInsulinInjection' (
	'IdTypeOfInsulinInjection'	INTEGER NOT NULL,
	'Name'	VARCHAR(255) NOT NULL,
	PRIMARY KEY('IdTypeOfInsulinInjection')
);

CREATE TABLE 'TypesOfInsulinSpeed' (
	'IdTypeOfInsulinSpeed' INTEGER NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdTypeOfInsulinSpeed' AUTOINCREMENT)
);

CREATE TABLE 'TypesOfMeal' (
	'IdTypeOfMeal' INTEGER NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdTypeOfMeal' AUTOINCREMENT)
);

CREATE TABLE 'FoodsInRecipes' (
  'IdRecipe' INTEGER NOT NULL,
  'IdFood' INTEGER NOT NULL,
  'Quantity' DOUBLE NULL,
  'Carbohydrates' DOUBLE NULL,
  'AccuracyOfChoEstimate' DOUBLE NULL,
  'QualitativeAccuracy' INTEGER NULL,
  PRIMARY KEY ('IdRecipe', 'IdFood')
);

CREATE TABLE 'Recipes' (
  'IdRecipe' INTEGER NOT NULL,
  'Name' VARCHAR(45) NULL,
  'Description' VARCHAR(255) NULL,
  PRIMARY KEY ('IdRecipe')
);";
        private void CreateNewDatabase(string dbName)
		{
			// making new, means erasing existent! 
			if (File.Exists(dbName))
				File.Delete(dbName); 

			// when the file does not exist 
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
				// !!!! TODO fill the tables of enumerations
			}
			catch (Exception ex)
			{
				Common.LogOfProgram.Error("Sqlite_DataAndGeneral | CreateNewDatabase", ex);
			}
		}

	}
}
