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
-- File generated with SQLiteStudio v3.4.17 on lun giu 9 01:16:49 2025
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
INSERT INTO Foods (IdFood, Name, Description, Energy, TotalFats, SaturatedFats, MonounsaturatedFats, PolyunsaturatedFats, Carbohydrates, Sugar, Fibers, Proteins, Salt, Potassium, Cholesterol, GlycemicIndex, Unit, GramsInOneUnit, Manufacturer, Category) VALUES (1, 'Uovo', '1 piccolo 40 g, 1 medio 50 g, 1 grande 60 g', NULL, NULL, NULL, NULL, NULL, 1.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'g', 1.0, '', '');

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
INSERT INTO GlucoseRecords (IdGlucoseRecord, GlucoseValue, Timestamp, GlucoseString, IdTypeOfGlucoseMeasurement, IdTypeOfGlucoseMeasurementDevice, IdModelOfMeasurementSystem, IdDevice, IdDocumentType, Notes) VALUES (1, 99.0, '2025-04-09 07:45:00', NULL, 0, 0, NULL, NULL, NULL, NULL);
INSERT INTO GlucoseRecords (IdGlucoseRecord, GlucoseValue, Timestamp, GlucoseString, IdTypeOfGlucoseMeasurement, IdTypeOfGlucoseMeasurementDevice, IdModelOfMeasurementSystem, IdDevice, IdDocumentType, Notes) VALUES (2, 161.0, '2025-05-31 00:47:00', NULL, 0, 0, NULL, NULL, NULL, '');

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
INSERT INTO Ingredients (IdIngredient, IdRecipe, Name, Description, QuantityGrams, QuantityPercent, CarbohydratesPercent, AccuracyOfChoEstimate, IdFood) VALUES (1, 1, 'Uovo', '1 piccolo 40 g, 1 medio 50 g, 1 grande 60 g', NULL, NULL, NULL, NULL, NULL);

-- Table: Injections
CREATE TABLE Injections (IdInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX INT, InjectionPositionY INT, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinAction INT, IdInsulinDrug INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInjection));
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (1, '2025-05-20 22:05:57', NULL, NULL, 207.97955322265625, 241.99371337890625, NULL, NULL, 20, NULL, NULL, 1);
INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (2, '2025-05-22 13:02:42', NULL, NULL, 0, 0, NULL, NULL, 10, NULL, NULL, 1);

-- Table: InsulinDrugs
CREATE TABLE InsulinDrugs (IdInsulinDrug INT NOT NULL, Name VARCHAR (32), Manufacturer VARCHAR (32), TypeOfInsulinAction INTEGER, DurationInHours DOUBLE, PRIMARY KEY (IdInsulinDrug));
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours) VALUES (2, 'Toujeo', 'Solostar', 40, 24.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours) VALUES (1, 'Humalog', 'Lilly', 20, 4.0);

-- Table: Meals
CREATE TABLE Meals (IdMeal INT NOT NULL, IdTypeOfMeal INT, Carbohydrates DOUBLE, TimeBegin DATETIME, Notes VARCHAR (255), AccuracyOfChoEstimate DOUBLE, IdBolusCalculation INT, IdGlucoseRecord INT, IdInjection INT, TimeEnd DATETIME, PRIMARY KEY (IdMeal));

-- Table: ModelsOfMeasurementSystem
CREATE TABLE 'ModelsOfMeasurementSystem' (



	'IdModelOfMeasurementSystem'	INT NOT NULL,



	'Name'	VARCHAR(45),



	PRIMARY KEY('IdModelOfMeasurementSystem')



);

-- Table: Parameters
CREATE TABLE Parameters (IdParameters INT NOT NULL, Timestamp DATETIME, Bolus_TargetGlucose INT, Bolus_GlucoseBeforeMeal INT, Bolus_ChoToEat INT, Bolus_ChoInsulinRatioBreakfast DOUBLE, Bolus_ChoInsulinRatioLunch DOUBLE, Bolus_ChoInsulinRatioDinner DOUBLE, Bolus_TotalDailyDoseOfInsulin DOUBLE, Bolus_InsulinCorrectionSensitivity DOUBLE, Correction_TypicalBolusMorning DOUBLE, Correction_TypicalBolusMidday DOUBLE, Correction_TypicalBolusEvening DOUBLE, Correction_TypicalBolusNight DOUBLE, Correction_FactorOfInsulinCorrectionSensitivity DOUBLE, Hypo_GlucoseTarget DOUBLE, Hypo_GlucoseLast DOUBLE, Hypo_GlucosePrevious DOUBLE, Hypo_HourLast DOUBLE, Hypo_HourPrevious DOUBLE, Hypo_MinuteLast DOUBLE, Hypo_MinutePrevious DOUBLE, Hypo_AlarmAdvanceTime DOUBLE, Hypo_FutureSpanMinutes DOUBLE, Hit_ChoAlreadyTaken DOUBLE, Hit_ChoOfFood DOUBLE, Hit_TargetCho DOUBLE, Hit_NameOfFood TEXT, FoodInMeal_ChoGrams DOUBLE, FoodInMeal_QuantityGrams DOUBLE, FoodInMeal_CarbohydratesPercent DOUBLE, FoodInMeal_Name TEXT, FoodInMeal_AccuracyOfChoEstimate DOUBLE, Meal_ChoGrams DOUBLE, Meal_Breakfast_StartTime_Hours DOUBLE, Meal_Breakfast_EndTime_Hours DOUBLE, Meal_Lunch_StartTime_Hours DOUBLE, Meal_Lunch_EndTime_Hours DOUBLE, Meal_Dinner_StartTime_Hours DOUBLE, Meal_Dinner_EndTime_Hours DOUBLE, Insulin_Short_Id INTEGER, Insulin_Long_Id INTEGER, PRIMARY KEY (IdParameters));
INSERT INTO Parameters (IdParameters, Timestamp, Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, Insulin_Short_Id, Insulin_Long_Id) VALUES (1, NULL, 105, 99, 44.2, 6.0, 72.0, 6.2, 52.0, 44.0, 10.0, 12.0, 13.0, 17.0, 1800.0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', 2.3, '', '', '', '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- Table: PositionsOfReferences
CREATE TABLE PositionsOfReferences (IdPosition INTEGER PRIMARY KEY, Timestamp DATETIME, Zone INTEGER, PositionX REAL, PositionY REAL, Notes TEXT);
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (47, '2025-05-22 13:22:56', 1, 0.26978021059153, 0.22811406581246, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (48, '2025-05-22 13:22:56', 1, 0.27447424550747, 0.19108373236217, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (49, '2025-05-22 13:22:56', 1, 0.31717110678788, 0.18758591695048, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (50, '2025-05-22 13:22:56', 1, 0.35571440851501, 0.17958821563465, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (51, '2025-05-22 13:22:56', 1, 0.394789211413, 0.16858429006196, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (52, '2025-05-22 13:22:56', 1, 0.44217110678788, 0.15907402613654, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (53, '2025-05-22 13:22:56', 1, 0.52185617816386, 0.14856167893913, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (54, '2025-05-22 13:22:56', 1, 0.47134431369642, 0.3241518362483, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (55, '2025-05-22 13:22:56', 1, 0.52185617816386, 0.32366025787302, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (56, '2025-05-22 13:22:56', 1, 0.47030101729521, 0.39919407523457, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (57, '2025-05-22 13:22:56', 1, 0.52083253444385, 0.39868360867652, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (58, '2025-05-22 13:22:56', 1, 0.34756794608284, 0.23368845672863, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (59, '2025-05-22 13:22:56', 1, 0.31103512962035, 0.23467411827202, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (60, '2025-05-22 13:22:56', 1, 0.38934786864808, 0.21962422382093, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (61, '2025-05-22 13:22:56', 1, 0.43317936602687, 0.21261107222718, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (62, '2025-05-22 13:22:56', 1, 0.47491987671111, 0.20658353105861, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (63, '2025-05-22 13:22:56', 1, 0.48119275690581, 0.15044029913156, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (64, '2025-05-22 13:22:56', 1, 0.52190752012775, 0.20260308296037, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (65, '2025-05-22 13:22:56', 1, 0.38516594851828, 0.26378822007171, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (66, '2025-05-22 13:22:56', 1, 0.42899741926742, 0.26579739620138, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (67, '2025-05-22 13:22:56', 1, 0.57512863185809, 0.25874632766698, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (68, '2025-05-22 13:22:56', 1, 0.61481762098808, 0.25976984225326, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (69, '2025-05-22 13:22:56', 1, 0.60854474079338, 0.2146202355773, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (70, '2025-05-22 13:22:56', 1, 0.56573901750654, 0.20859269440873, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (71, '2025-05-22 13:22:56', 1, 0.5605312700255, 0.15343509846596, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (72, '2025-05-22 13:22:56', 1, 0.60124603324744, 0.1684849801375, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (73, '2025-05-22 13:22:56', 1, 0.21813911584451, 0.52593411392902, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (74, '2025-05-22 13:22:56', 1, 0.26701570680628, 0.54271090969169, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (75, '2025-05-22 13:22:56', 1, 0.32110318754772, 0.58120262363249, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (76, '2025-05-22 13:22:56', 1, 0.29663192456097, 0.90451995491782, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (77, '2025-05-22 13:22:56', 1, 0.34550851552274, 0.92629815745394, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (78, '2025-05-22 20:52:58', 4, 0.16021229925359, 0.41419535981127, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (79, '2025-05-22 20:52:58', 4, 0.15863128638023, 0.5081244474332, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (80, '2025-05-22 20:52:58', 4, 0.15705024942038, 0.59243697668674, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (81, '2025-05-22 20:52:58', 4, 0.85714285714286, 0.41047731659116, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (82, '2025-05-22 20:52:58', 4, 0.85633740504144, 0.50591598601031, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (83, '2025-05-22 20:52:58', 4, 0.85556182018301, 0.59319179015752, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (84, '2025-05-22 20:54:30', 2, 0.26133233850653, 0.56987111260507, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (85, '2025-05-22 20:54:30', 2, 0.30820061943748, 0.59120960464173, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (86, '2025-05-22 20:54:30', 2, 0.35222352634777, 0.61249778940579, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (87, '2025-05-22 20:54:30', 2, 0.39345472509211, 0.63247741617947, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (88, '2025-05-22 20:54:30', 2, 0.43178688396107, 0.64978978224347, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (89, '2025-05-22 20:54:30', 2, 0.43039105155251, 0.68708177508114, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (90, '2025-05-22 20:54:30', 2, 0.57808251814409, 0.64843099825233, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (91, '2025-05-22 20:54:30', 2, 0.57668668573553, 0.68708177508114, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (92, '2025-05-22 20:54:30', 2, 0.57953201640736, 0.72432346064622, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (93, '2025-05-22 20:54:30', 2, 0.59370526400479, 0.7616154534839, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (94, '2025-05-22 20:54:30', 2, 0.42754563418302, 0.72432346064622, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (95, '2025-05-22 20:54:30', 2, 0.41192288832231, 0.76030697676535, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (96, '2025-05-22 20:55:45', 3, 0.23148263584484, 0.14618778463078, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (97, '2025-05-22 20:55:45', 3, 0.26557354493575, 0.15106308020889, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (98, '2025-05-22 20:55:45', 3, 0.30111399563876, 0.14493412702031, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (99, '2025-05-22 20:55:45', 3, 0.29542324759743, 0.11545009753628, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (100, '2025-05-22 20:55:45', 3, 0.26702308654785, 0.12157905072486, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (101, '2025-05-22 20:55:45', 3, 0.23577755147761, 0.11670375514675, '');
INSERT INTO PositionsOfReferences (IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) VALUES (102, '2025-05-22 20:55:45', 3, 0.26841891895641, 0.09209502124083, '');

-- Table: Recipes
CREATE TABLE Recipes (IdRecipe INTEGER NOT NULL, Name TEXT, Description TEXT, CarbohydratesPercent REAL, AccuracyOfChoEstimate REAL, IsCooked BOOL, RawToCookedRatio REAL, PRIMARY KEY (IdRecipe));
INSERT INTO Recipes (IdRecipe, Name, Description, CarbohydratesPercent, AccuracyOfChoEstimate, IsCooked, RawToCookedRatio) VALUES (1, 'Salsa carbonara', 'Solo il sugo', NULL, NULL, 1, NULL);
INSERT INTO Recipes (IdRecipe, Name, Description, CarbohydratesPercent, AccuracyOfChoEstimate, IsCooked, RawToCookedRatio) VALUES (2, '', '', NULL, NULL, 1, NULL);

-- Table: UnitsOfFood
CREATE TABLE UnitsOfFood (IdUnitOfFood INTEGER PRIMARY KEY, IdFood INTEGER, Name TEXT, Description TEXT, GramsInOneUnit DOUBLE);
INSERT INTO UnitsOfFood (IdUnitOfFood, IdFood, Name, Description, GramsInOneUnit) VALUES (1, 1, 'g', NULL, 1.0);

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
