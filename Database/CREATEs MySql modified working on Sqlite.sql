-- -----------------------------------------------------
-- Table Meals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS Meals;

CREATE TABLE Meals (
  `IdMeal` INT NOT NULL,
  `IdTypeOfMeal` INT NULL,
  `TimeBegin` DATETIME NULL,
  `TimeEnd` DATETIME NULL,
  `Carbohydrates` DOUBLE NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL,
  `IdGlucoseRecord` INT NULL,
  `IdQualitativeAccuracyCHO` INT NULL,
  `IdInsulineInjection` INT NULL,
  PRIMARY KEY (`IdMeal`));
  
-- -----------------------------------------------------
-- Table Foods
-- -----------------------------------------------------
DROP TABLE IF EXISTS Foods;

CREATE TABLE Foods (
  `IdFood` INT NOT NULL,
  `Name` VARCHAR(15) NULL,
  `Description` VARCHAR(256) NULL,
  `Energy` DOUBLE NULL,
  `TotalFats` DOUBLE NULL, 
  `SaturatedFats` DOUBLE NULL, 
  `Carbohydrates` DOUBLE NOT NULL, 
  `Sugar` DOUBLE NULL,
  `Fibers` DOUBLE NULL,
  `Proteins` DOUBLE NULL,
  `Salt` DOUBLE NULL,
  `Potassium` DOUBLE NULL,
  `GlycemicIndex` DOUBLE NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL,
  `QualitativeAccuracyOfChoEstimate` INT NULL,
  PRIMARY KEY (`IdFood`));
  
-- -----------------------------------------------------
-- Table InsulinInjection
-- -----------------------------------------------------
DROP TABLE IF EXISTS InsulinInjection;

CREATE TABLE InsulinInjection (
  `idInsulinInjection` INT NOT NULL,
  `descInsulinInjection` VARCHAR(45) NULL,
  `amount` VARCHAR(45) NULL,
  PRIMARY KEY (`idInsulinInjection`));

-- -----------------------------------------------------
-- Table TypesOfGlucoseMeasurementDevice
-- -----------------------------------------------------
DROP TABLE IF EXISTS TypesOfGlucoseMeasurementDevice;

CREATE TABLE TypesOfGlucoseMeasurementDevice(
  `IdTypeOfGlucoseMeasurementDevice` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfGlucoseMeasurementDevice`));

-- -----------------------------------------------------
-- Table TypesOfGlucoseMeasurement
-- -----------------------------------------------------
DROP TABLE IF EXISTS TypesOfGlucoseMeasurement;

CREATE TABLE TypesOfGlucoseMeasurement(
  `IdTypeOfGlucoseMeasurement` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfGlucoseMeasurement`));

-- -----------------------------------------------------
-- Table ModelsOfMeasurementSystem
-- -----------------------------------------------------
DROP TABLE IF EXISTS ModelsOfMeasurementSystem;

CREATE TABLE ModelsOfMeasurementSystem(
  `IdModelOfMeasurementSystem` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdModelOfMeasurementSystem`));
  
-- -----------------------------------------------------
-- Table GlucoseRecords
-- -----------------------------------------------------
DROP TABLE IF EXISTS GlucoseRecords;

CREATE TABLE GlucoseRecords(
  `IdGlucoseRecord` INT NOT NULL,
  `Timestamp` DATETIME NULL,
  `GlucoseValue` DOUBLE NULL,
  `InsulinDrugName` VARCHAR(45) NULL,
  `IdTypeOfGlucoseMeasurement` INT NULL,
  `IdTypeOfGlucoseMeasurementDevice` INT NULL,
  `IdModelOfMeasurementSystem` INT NULL,
  `DeviceId` VARCHAR(45) NULL,
  `IdDocumentType` INT NULL,
  `GlucoseString` VARCHAR(45) NULL,
  `TypeOfGlucoseMeasurement` INT NULL,
  PRIMARY KEY (`IdGlucoseRecord`));

-- -----------------------------------------------------
-- Table QualitativeAccuracies
-- -----------------------------------------------------
DROP TABLE IF EXISTS QualitativeAccuracies;

CREATE TABLE QualitativeAccuracies(
  `IdQualitativeAccuracy` INT NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`IdQualitativeAccuracy`));

-- -----------------------------------------------------
-- Table TypesOfMeal
-- -----------------------------------------------------
DROP TABLE IF EXISTS TypesOfMeal;

CREATE TABLE TypesOfMeal(
  `IdTypeOfMeal` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfMeal`));

-- -----------------------------------------------------
-- Table TypesOfInjection
-- -----------------------------------------------------
DROP TABLE IF EXISTS TypesOfInjection;

CREATE TABLE TypesOfInjection(
  `IdTypeOfInjection` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfInjection`));

-- -----------------------------------------------------
-- Table TypesOfInsulinSpeed
-- -----------------------------------------------------
DROP TABLE IF EXISTS TypesOfInsulinSpeed;

CREATE TABLE TypesOfInsulinSpeed(
  `IdTypeOfInsulinSpeed` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfInsulinSpeed`));

-- -----------------------------------------------------
-- Table TypesOfInsulinInjection
-- -----------------------------------------------------
DROP TABLE IF EXISTS TypesOfInsulinInjection;

CREATE TABLE TypesOfInsulinInjection(
  `IdTypeOfInsulinInjection` INT NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`IdTypeOfInsulinInjection`));


-- -----------------------------------------------------
-- Table InsulineInjections
-- -----------------------------------------------------
DROP TABLE IF EXISTS InsulineInjections;

CREATE TABLE InsulineInjections(
  `IdInsulineInjection` INT NOT NULL,
  `Timestamp` VARCHAR(45) NULL,
  `InsulinValue` DOUBLE NULL,
  `InjectionPositionX` INT NULL,
  `InjectionPositionY` INT NULL,
  `IdTypeOfInjection` INT NULL,
  `IdTypeOfInsulinSpeed` INT NULL,
  `IdTypeOfInsulinInjection` INT NULL,
  `InsulinString` VARCHAR(45) NULL,
  PRIMARY KEY (`IdInsulineInjection`));
  
-- -----------------------------------------------------
-- Table BolusCalculations`
-- -----------------------------------------------------
DROP TABLE IF EXISTS BolusCalculations;

CREATE TABLE BolusCalculations(
  `IdBolusCalculation` INT NOT NULL,
  `Timestamp` DATETIME NULL,
  `TotalInsulinForMeal` DOUBLE NULL,
  `CalculatedChoToEat` DOUBLE NULL,
  `BolusInsulinDueToChoOfMeal` DOUBLE NULL,
  `BolusInsulinDueToCorrectionOfGlucose` DOUBLE NULL,
  `TargetGlucose` DOUBLE NULL,
  `InsulinCorrectionSensitivity` DOUBLE NULL,
  `TypicalBolusMorning` DOUBLE NULL,
  `TypicalBolusMidday` DOUBLE NULL,
  `TypicalBolusEvening` DOUBLE NULL,
  `TypicalBolusNight` DOUBLE NULL,
  `TotalDailyDoseOfInsulin` DOUBLE NULL,
  `ChoInsulinRatioBreakfast` DOUBLE NULL,
  `ChoInsulinRatioLunch` DOUBLE NULL,
  `ChoInsulinRatioDinner` DOUBLE NULL,
  `GlucoseBeforeMeal` DOUBLE NULL,
  `GlucoseToBeCorrected` DOUBLE NULL,
  `FactorOfInsulinCorrectionSensitivity` DOUBLE NULL,
  PRIMARY KEY (`IdBolusCalculation`));

-- -----------------------------------------------------
-- Table Meals
-- -----------------------------------------------------
DROP TABLE IF EXISTS Meals;

CREATE TABLE Meals(
  `IdMeal` INT NOT NULL,
  `IdTypeOfMeal` INT NULL,
  `TimeBegin` DATETIME NULL,
  `TimeEnd` DATETIME NULL,
  `Carbohydrates` DOUBLE NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL, 
  `IdBolusCalculation` INT NULL,
  `IdGlucoseRecord` INT NULL,
  `IdQualitativeAccuracyCHO` INT NULL,
  `IdInsulineInjection` INT NULL,
  PRIMARY KEY (`IdMeal`)); 

-- -----------------------------------------------------
-- Table FoodsInMeals
-- -----------------------------------------------------
DROP TABLE IF EXISTS FoodsInMeals;

CREATE TABLE FoodsInMeals(
  `IdMeal` INT NOT NULL,
  `IdFood` INT NULL,
  `Quantity` DOUBLE NULL,
  `Carbohydrates` DOUBLE NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL,
  PRIMARY KEY (`IdMeal`));

-- -----------------------------------------------------
-- Table HypoPredictions
-- -----------------------------------------------------
DROP TABLE IF EXISTS HypoPredictions;

CREATE TABLE HypoPredictions(
  `IdHypoPrediction` INT NOT NULL,
  `PredictedTime` DATETIME NULL,
  `AlarmTime` DATETIME NULL,
  `GlucoseSlope` DOUBLE NULL,
  `HypoGlucoseTarget` INT NULL,
  `GlucoseLast` DOUBLE NULL,
  `GlucosePrevious` DOUBLE NULL,
  `Interval` VARCHAR(10) NULL,
  `DatetimeLast` DATETIME NULL,
  `DatetimePrevious` DATETIME NULL,
  `HypoPredictionscol` VARCHAR(45) NULL,
  PRIMARY KEY (`IdHypoPrediction`));

CREATE TABLE IF NOT EXISTS `Alarms` (
  `idAlarm` INT NOT NULL,
  `TimeStart` DATETIME NULL,
  `TimeAlarm` DATETIME NULL,
  `Interval` DOUBLE,
  `Duration` DOUBLE,
  `IsRepeated` TINYINT NULL,
  `IsEnabled` TINYINT NULL,
  PRIMARY KEY (`idAlarm`))
