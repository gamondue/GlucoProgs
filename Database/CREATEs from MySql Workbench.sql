-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `mydb` ;

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Meals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Meals` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Meals` (
  `IdMeal` INT NOT NULL,
  `IdTypeOfMeal` INT NULL,
  `TimeBegin` DATETIME NULL,
  `TimeEnd` DATETIME NULL,
  `Carbohydrates` DOUBLE NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1',
  `IdBolusCalculation` INT NULL,
  `IdGlucoseRecord` INT NULL,
  `IdQualitativeAccuracyCHO` INT NULL,
  `IdInsulineInjection` INT NULL,
  PRIMARY KEY (`IdMeal`),
  CONSTRAINT `fk_Meals_GlucoseRecords1`
    FOREIGN KEY (`IdGlucoseRecord`)
    REFERENCES `mydb`.`GlucoseRecords` (`IdGlucoseRecord`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_QualitativeAccuracies1`
    FOREIGN KEY (`IdQualitativeAccuracyCHO`)
    REFERENCES `mydb`.`QualitativeAccuracies` (`IdQualitativeAccuracy`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_TypesOfMeal1`
    FOREIGN KEY (`IdTypeOfMeal`)
    REFERENCES `mydb`.`TypesOfMeal` (`IdTypeOfMeal`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_InsulineInjections1`
    FOREIGN KEY (`IdInsulineInjection`)
    REFERENCES `mydb`.`InsulineInjections` (`IdInsulineInjection`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_BolusCalculations1`
    FOREIGN KEY (`IdBolusCalculation`)
    REFERENCES `mydb`.`BolusCalculations` (`IdBolusCalculation`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Foods`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Foods` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Foods` (
  `IdFood` INT NOT NULL,
  `Name` VARCHAR(15) NULL,
  `Description` VARCHAR(256) NULL,
  `Energy` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food\n',
  `TotalFats` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food\n',
  `SaturatedFats` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food\n',
  `Carbohydrates` DOUBLE NOT NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Sugar` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Fibers` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Proteins` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Salt` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Potassium` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `GlycemicIndex` DOUBLE NULL COMMENT 'number',
  `AccuracyOfChoEstimate` DOUBLE NULL,
  `QualitativeAccuracyOfChoEstimate` INT NULL,
  PRIMARY KEY (`IdFood`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Parameters`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Parameters` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Parameters` (
)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`InsulinInjection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`InsulinInjection` ;

CREATE TABLE IF NOT EXISTS `mydb`.`InsulinInjection` (
  `idInsulinInjection` INT NOT NULL,
  `descInsulinInjection` VARCHAR(45) NULL,
  `amount` VARCHAR(45) NULL COMMENT '[Insulin Units]',
  PRIMARY KEY (`idInsulinInjection`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfGlucoseMeasurementDevice`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`TypesOfGlucoseMeasurementDevice` ;

CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfGlucoseMeasurementDevice` (
  `IdTypeOfGlucoseMeasurementDevice` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfGlucoseMeasurementDevice`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfGlucoseMeasurement`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`TypesOfGlucoseMeasurement` ;

CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfGlucoseMeasurement` (
  `IdTypeOfGlucoseMeasurement` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfGlucoseMeasurement`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`ModelsOfMeasurementSystem`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`ModelsOfMeasurementSystem` ;

CREATE TABLE IF NOT EXISTS `mydb`.`ModelsOfMeasurementSystem` (
  `IdModelOfMeasurementSystem` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdModelOfMeasurementSystem`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`GlucoseRecords`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`GlucoseRecords` ;

CREATE TABLE IF NOT EXISTS `mydb`.`GlucoseRecords` (
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
  PRIMARY KEY (`IdGlucoseRecord`),
  CONSTRAINT `fk_GlucoseRecords_TypesOfGlucoseMeasurementDevice1`
    FOREIGN KEY (`IdTypeOfGlucoseMeasurementDevice`)
    REFERENCES `mydb`.`TypesOfGlucoseMeasurementDevice` (`IdTypeOfGlucoseMeasurementDevice`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_GlucoseRecords_TypesOfGlucoseMeasurement1`
    FOREIGN KEY (`IdTypeOfGlucoseMeasurement`)
    REFERENCES `mydb`.`TypesOfGlucoseMeasurement` (`IdTypeOfGlucoseMeasurement`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_GlucoseRecords_ModelsOfMeasurementSystem1`
    FOREIGN KEY (`IdModelOfMeasurementSystem`)
    REFERENCES `mydb`.`ModelsOfMeasurementSystem` (`IdModelOfMeasurementSystem`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_GlucoseRecords_TypesOfGlucoseMeasurementDevice1_idx` ON `mydb`.`GlucoseRecords` (`IdTypeOfGlucoseMeasurementDevice` ASC) VISIBLE;

CREATE INDEX `fk_GlucoseRecords_TypesOfGlucoseMeasurement1_idx` ON `mydb`.`GlucoseRecords` (`IdTypeOfGlucoseMeasurement` ASC) VISIBLE;

CREATE INDEX `fk_GlucoseRecords_ModelsOfMeasurementSystem1_idx` ON `mydb`.`GlucoseRecords` (`IdModelOfMeasurementSystem` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `mydb`.`QualitativeAccuracies`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`QualitativeAccuracies` ;

CREATE TABLE IF NOT EXISTS `mydb`.`QualitativeAccuracies` (
  `IdQualitativeAccuracy` INT NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`IdQualitativeAccuracy`));


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfMeal`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`TypesOfMeal` ;

CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfMeal` (
  `IdTypeOfMeal` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfMeal`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfInjection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`TypesOfInjection` ;

CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfInjection` (
  `IdTypeOfInjection` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfInjection`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfInsulinSpeed`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`TypesOfInsulinSpeed` ;

CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfInsulinSpeed` (
  `IdTypeOfInsulinSpeed` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfInsulinSpeed`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfInsulinInjection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`TypesOfInsulinInjection` ;

CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfInsulinInjection` (
  `IdTypeOfInsulinInjection` INT NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`IdTypeOfInsulinInjection`));


-- -----------------------------------------------------
-- Table `mydb`.`InsulineInjections`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`InsulineInjections` ;

CREATE TABLE IF NOT EXISTS `mydb`.`InsulineInjections` (
  `IdInsulineInjection` INT NOT NULL AUTO_INCREMENT,
  `Timestamp` VARCHAR(45) NULL,
  `InsulinValue` DOUBLE NULL,
  `InjectionPositionX` INT NULL,
  `InjectionPositionY` INT NULL,
  `IdTypeOfInjection` INT NULL,
  `IdTypeOfInsulinSpeed` INT NULL,
  `IdTypeOfInsulinInjection` INT NULL,
  `InsulinString` VARCHAR(45) NULL,
  PRIMARY KEY (`IdInsulineInjection`),
  CONSTRAINT `fk_InsulineInjections_TypesOfInjection1`
    FOREIGN KEY (`IdTypeOfInjection`)
    REFERENCES `mydb`.`TypesOfInjection` (`IdTypeOfInjection`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_InsulineInjections_TypesOfInsulinSpeed1`
    FOREIGN KEY (`IdTypeOfInsulinSpeed`)
    REFERENCES `mydb`.`TypesOfInsulinSpeed` (`IdTypeOfInsulinSpeed`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_InsulineInjections_TypesOfInsulinInjection1`
    FOREIGN KEY (`IdTypeOfInsulinInjection`)
    REFERENCES `mydb`.`TypesOfInsulinInjection` (`IdTypeOfInsulinInjection`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_InsulineInjections_TypesOfInjection1_idx` ON `mydb`.`InsulineInjections` (`IdTypeOfInjection` ASC) VISIBLE;

CREATE INDEX `fk_InsulineInjections_TypesOfInsulinSpeed1_idx` ON `mydb`.`InsulineInjections` (`IdTypeOfInsulinSpeed` ASC) VISIBLE;

CREATE INDEX `fk_InsulineInjections_TypesOfInsulinInjection1_idx` ON `mydb`.`InsulineInjections` (`IdTypeOfInsulinInjection` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `mydb`.`BolusCalculations`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`BolusCalculations` ;

CREATE TABLE IF NOT EXISTS `mydb`.`BolusCalculations` (
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
  PRIMARY KEY (`IdBolusCalculation`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Meals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`Meals` ;

CREATE TABLE IF NOT EXISTS `mydb`.`Meals` (
  `IdMeal` INT NOT NULL,
  `IdTypeOfMeal` INT NULL,
  `TimeBegin` DATETIME NULL,
  `TimeEnd` DATETIME NULL,
  `Carbohydrates` DOUBLE NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1',
  `IdBolusCalculation` INT NULL,
  `IdGlucoseRecord` INT NULL,
  `IdQualitativeAccuracyCHO` INT NULL,
  `IdInsulineInjection` INT NULL,
  PRIMARY KEY (`IdMeal`),
  CONSTRAINT `fk_Meals_GlucoseRecords1`
    FOREIGN KEY (`IdGlucoseRecord`)
    REFERENCES `mydb`.`GlucoseRecords` (`IdGlucoseRecord`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_QualitativeAccuracies1`
    FOREIGN KEY (`IdQualitativeAccuracyCHO`)
    REFERENCES `mydb`.`QualitativeAccuracies` (`IdQualitativeAccuracy`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_TypesOfMeal1`
    FOREIGN KEY (`IdTypeOfMeal`)
    REFERENCES `mydb`.`TypesOfMeal` (`IdTypeOfMeal`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_InsulineInjections1`
    FOREIGN KEY (`IdInsulineInjection`)
    REFERENCES `mydb`.`InsulineInjections` (`IdInsulineInjection`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_BolusCalculations1`
    FOREIGN KEY (`IdBolusCalculation`)
    REFERENCES `mydb`.`BolusCalculations` (`IdBolusCalculation`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Meals_GlucoseRecords1_idx` ON `mydb`.`Meals` (`IdGlucoseRecord` ASC) VISIBLE;

CREATE INDEX `fk_Meals_QualitativeAccuracies1_idx` ON `mydb`.`Meals` (`IdQualitativeAccuracyCHO` ASC) VISIBLE;

CREATE INDEX `fk_Meals_TypesOfMeal1_idx` ON `mydb`.`Meals` (`IdTypeOfMeal` ASC) VISIBLE;

CREATE INDEX `fk_Meals_InsulineInjections1_idx` ON `mydb`.`Meals` (`IdInsulineInjection` ASC) VISIBLE;

CREATE INDEX `fk_Meals_BolusCalculations1_idx` ON `mydb`.`Meals` (`IdBolusCalculation` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `mydb`.`FoodsInMeals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`FoodsInMeals` ;

CREATE TABLE IF NOT EXISTS `mydb`.`FoodsInMeals` (
  `IdMeal` INT NOT NULL,
  `IdFood` INT NULL,
  `Quantity` DOUBLE NULL COMMENT 'grams of this food eaten in this meal',
  `Carbohydrates` DOUBLE NULL COMMENT '[g]: grams of this foood eaten in this meal',
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1.1 = perfect! ',
  PRIMARY KEY (`IdMeal`),
  CONSTRAINT `fk_Meals_has_Foods_Meals`
    FOREIGN KEY (`IdMeal`)
    REFERENCES `mydb`.`Meals` (`IdMeal`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_has_Foods_Foods1`
    FOREIGN KEY (`IdFood`)
    REFERENCES `mydb`.`Foods` (`IdFood`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Meals_has_Foods_Foods1_idx` ON `mydb`.`FoodsInMeals` (`IdFood` ASC) VISIBLE;

CREATE INDEX `fk_Meals_has_Foods_Meals_idx` ON `mydb`.`FoodsInMeals` (`IdMeal` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `mydb`.`HypoPredictions`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`HypoPredictions` ;

CREATE TABLE IF NOT EXISTS `mydb`.`HypoPredictions` (
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
  PRIMARY KEY (`IdHypoPrediction`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`WeightsOfFoods`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `mydb`.`WeightsOfFoods` ;

CREATE TABLE IF NOT EXISTS `mydb`.`WeightsOfFoods` (
)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
