using gamon;
using System.Data.Common;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        string creationScript = @"
--
-- File generated with SQLiteStudio v3.4.17 on mar mag 20 20:49:21 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Alarms
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
CREATE TABLE Foods (IdFood INT NOT NULL, Name VARCHAR (15), Description VARCHAR (256), Energy DOUBLE, TotalFats DOUBLE, SaturatedFats DOUBLE, MonounsaturatedFats DOUBLE, PolyunsaturatedFats DOUBLE, Carbohydrates DOUBLE, Sugar DOUBLE, Fibers INT, Proteins INT, Salt DOUBLE, Potassium DOUBLE, Cholesterol DOUBLE, GlycemicIndex DOUBLE, Unit TEXT, GramsInOneUnit DOUBLE, Manufacturer TEXT, Category REAL, PRIMARY KEY (IdFood));

-- Table: FoodsInMeals
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
CREATE TABLE Ingredients (IdIngredient INTEGER NOT NULL, IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, QuantityGrams REAL, QuantityPercent REAL, CarbohydratesPercent REAL, AccuracyOfChoEstimate DOUBLE, IdFood INTEGER, PRIMARY KEY (IdIngredient));

-- Table: Injections
CREATE TABLE Injections (IdInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX INT, InjectionPositionY INT, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinSpeed INT, IdTypeOfInsulinInjection INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInjection));

-- Table: InsulinDrugs
CREATE TABLE InsulinDrugs (IdInsulinDrug INT NOT NULL, Name VARCHAR (30), InsulinDuration DOUBLE, PRIMARY KEY (IdInsulinDrug));

-- Table: Meals
CREATE TABLE Meals (IdMeal INT NOT NULL, IdTypeOfMeal INT, Carbohydrates DOUBLE, TimeBegin DATETIME, Notes VARCHAR (255), AccuracyOfChoEstimate DOUBLE, IdBolusCalculation INT, IdGlucoseRecord INT, IdInjection INT, TimeEnd DATETIME, PRIMARY KEY (IdMeal));

-- Table: ModelsOfMeasurementSystem
CREATE TABLE 'ModelsOfMeasurementSystem' (



	'IdModelOfMeasurementSystem'	INT NOT NULL,



	'Name'	VARCHAR(45),



	PRIMARY KEY('IdModelOfMeasurementSystem')



);

-- Table: Parameters
CREATE TABLE Parameters (IdParameters INT NOT NULL, Bolus_TargetGlucose INT, Bolus_GlucoseBeforeMeal INT, Bolus_ChoToEat INT, Bolus_ChoInsulinRatioBreakfast DOUBLE, Bolus_ChoInsulinRatioLunch DOUBLE, Bolus_ChoInsulinRatioDinner DOUBLE, Bolus_TotalDailyDoseOfInsulin DOUBLE, Bolus_InsulinCorrectionSensitivity DOUBLE, Correction_TypicalBolusMorning DOUBLE, Correction_TypicalBolusMidday DOUBLE, Correction_TypicalBolusEvening DOUBLE, Correction_TypicalBolusNight DOUBLE, Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, Hypo_GlucoseTarget DOUBLE, Hypo_GlucoseLast DOUBLE, Hypo_GlucosePrevious DOUBLE, Hypo_HourLast DOUBLE, Hypo_HourPrevious DOUBLE, Hypo_MinuteLast DOUBLE, Hypo_MinutePrevious DOUBLE, Hypo_AlarmAdvanceTime DOUBLE, Hypo_FutureSpanMinutes DOUBLE, Hit_ChoAlreadyTaken DOUBLE, Hit_ChoOfFood DOUBLE, Hit_TargetCho DOUBLE, Hit_NameOfFood TEXT, FoodInMeal_ChoGrams DOUBLE, FoodInMeal_QuantityGrams DOUBLE, FoodInMeal_CarbohydratesPercent DOUBLE, FoodInMeal_Name TEXT, FoodInMeal_AccuracyOfChoEstimate DOUBLE, Meal_ChoGrams DOUBLE, PRIMARY KEY (IdParameters));

-- Table: PositionsOfReferences
CREATE TABLE PositionsOfReferences (IdPosition INTEGER PRIMARY KEY, Timestamp DATETIME, Zone INTEGER, PositionX REAL, PositionY REAL, Notes TEXT);

-- Table: Recipes
CREATE TABLE Recipes (IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, CarbohydratesPercent REAL, AccuracyOfChoEstimate REAL, IsCooked BOOL, RawToCookedRatio REAL, PRIMARY KEY (IdRecipe));

-- Table: UnitsOfFood
CREATE TABLE UnitsOfFood (IdUnitOfFood INTEGER PRIMARY KEY, IdFood INTEGER, Name TEXT, Description TEXT, GramsInOneUnit DOUBLE);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
";
        internal override void CreateNewDatabase(string dbFile)
        {
			// making new, means erasing existent! 
			if (File.Exists(dbFile))
				File.Delete(dbFile);

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
