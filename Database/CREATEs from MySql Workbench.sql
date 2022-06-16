-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Meals`
-- -----------------------------------------------------
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
  INDEX `fk_Meals_GlucoseRecords1_idx` (`IdGlucoseRecord` ASC),
  INDEX `fk_Meals_QualitativeAccuracies1_idx` (`IdQualitativeAccuracyCHO` ASC),
  INDEX `fk_Meals_TypesOfMeal1_idx` (`IdTypeOfMeal` ASC),
  INDEX `fk_Meals_InsulineInjections1_idx` (`IdInsulineInjection` ASC),
  INDEX `fk_Meals_BolusCalculations1_idx` (`IdBolusCalculation` ASC),
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
CREATE TABLE IF NOT EXISTS `mydb`.`Parameters` (
)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`InsulinInjection`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`InsulinInjection` (
  `idInsulinInjection` INT NOT NULL,
  `descInsulinInjection` VARCHAR(45) NULL,
  `amount` VARCHAR(45) NULL COMMENT '[Insulin Units]',
  PRIMARY KEY (`idInsulinInjection`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfGlucoseMeasurementDevice`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfGlucoseMeasurementDevice` (
  `IdTypeOfGlucoseMeasurementDevice` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfGlucoseMeasurementDevice`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfGlucoseMeasurement`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfGlucoseMeasurement` (
  `IdTypeOfGlucoseMeasurement` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfGlucoseMeasurement`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`ModelsOfMeasurementSystem`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`ModelsOfMeasurementSystem` (
  `IdModelOfMeasurementSystem` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdModelOfMeasurementSystem`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`GlucoseRecords`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`GlucoseRecords` (
  `IdGlucoseRecord` INT NOT NULL,
  `Timestamp` DATETIME NULL,
  `GlucoseValue` DOUBLE NULL,
  `InsulinDrugName` VARCHAR(45) NULL,
  `IdTypeOfGlucoseMeasurement` INT NULL,
  `IdTypeOfGlucoseMeasurementDevice` INT NULL,
  `IdModelOfMeasurementSystem` INT NULL,
  `IdDevice` VARCHAR(45) NULL,
  `IdDocumentType` INT NULL,
  `GlucoseString` VARCHAR(45) NULL,
  PRIMARY KEY (`IdGlucoseRecord`),
  INDEX `fk_GlucoseRecords_TypesOfGlucoseMeasurementDevice1_idx` (`IdTypeOfGlucoseMeasurementDevice` ASC),
  INDEX `fk_GlucoseRecords_TypesOfGlucoseMeasurement1_idx` (`IdTypeOfGlucoseMeasurement` ASC),
  INDEX `fk_GlucoseRecords_ModelsOfMeasurementSystem1_idx` (`IdModelOfMeasurementSystem` ASC),
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


-- -----------------------------------------------------
-- Table `mydb`.`QualitativeAccuracies`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`QualitativeAccuracies` (
  `IdQualitativeAccuracy` INT NOT NULL,
  `name` VARCHAR(255) NOT NULL,
  `QuantitativeAccuracy` DOUBLE NULL COMMENT 'Quantitative accuracy associated with this qualitative accuracy',
  PRIMARY KEY (`IdQualitativeAccuracy`));


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfMeal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfMeal` (
  `IdTypeOfMeal` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfMeal`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfInsulinSpeed`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfInsulinSpeed` (
  `IdTypeOfInsulinSpeed` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  PRIMARY KEY (`IdTypeOfInsulinSpeed`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TypesOfInsulinInjection`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TypesOfInsulinInjection` (
  `IdTypeOfInsulinInjection` INT NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`IdTypeOfInsulinInjection`));


-- -----------------------------------------------------
-- Table `mydb`.`InsulineInjections`
-- -----------------------------------------------------
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
  `TypesOfGlucoseMeasurement_IdTypeOfGlucoseMeasurement` INT NOT NULL,
  PRIMARY KEY (`IdInsulineInjection`, `TypesOfGlucoseMeasurement_IdTypeOfGlucoseMeasurement`),
  INDEX `fk_InsulineInjections_TypesOfInjection1_idx` (`IdTypeOfInjection` ASC),
  INDEX `fk_InsulineInjections_TypesOfInsulinSpeed1_idx` (`IdTypeOfInsulinSpeed` ASC),
  INDEX `fk_InsulineInjections_TypesOfInsulinInjection1_idx` (`IdTypeOfInsulinInjection` ASC),
  INDEX `fk_InsulineInjections_TypesOfGlucoseMeasurement1_idx` (`TypesOfGlucoseMeasurement_IdTypeOfGlucoseMeasurement` ASC),
  CONSTRAINT `fk_InsulineInjections_TypesOfInjection1`
    FOREIGN KEY (`IdTypeOfInjection`)
    REFERENCES `mydb`.`TypesOfInsulinInjection` (`IdTypeOfInsulinInjection`)
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
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_InsulineInjections_TypesOfGlucoseMeasurement1`
    FOREIGN KEY (`TypesOfGlucoseMeasurement_IdTypeOfGlucoseMeasurement`)
    REFERENCES `mydb`.`TypesOfGlucoseMeasurement` (`IdTypeOfGlucoseMeasurement`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`BolusCalculations`
-- -----------------------------------------------------
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
  INDEX `fk_Meals_GlucoseRecords1_idx` (`IdGlucoseRecord` ASC),
  INDEX `fk_Meals_QualitativeAccuracies1_idx` (`IdQualitativeAccuracyCHO` ASC),
  INDEX `fk_Meals_TypesOfMeal1_idx` (`IdTypeOfMeal` ASC),
  INDEX `fk_Meals_InsulineInjections1_idx` (`IdInsulineInjection` ASC),
  INDEX `fk_Meals_BolusCalculations1_idx` (`IdBolusCalculation` ASC),
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
-- Table `mydb`.`FoodsInMeals`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`FoodsInMeals` (
  `IdFoodInMeal` INT NOT NULL,
  `IdMeal` INT NOT NULL,
  `IdFood` INT NULL,
  `Quantity` DOUBLE NULL COMMENT 'grams of this food eaten in this meal',
  `Carbohydrates` DOUBLE NULL COMMENT '[g]: grams of this foood eaten in this meal',
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1.1 = perfect! ',
  `QualitativeAccuracy` INT NULL,
  PRIMARY KEY (`IdFoodInMeal`),
  INDEX `fk_Meals_has_Foods_Foods1_idx` (`IdFood` ASC),
  INDEX `fk_Meals_has_Foods_Meals_idx` (`IdFoodInMeal` ASC),
  CONSTRAINT `fk_Meals_has_Foods_Meals`
    FOREIGN KEY (`IdFoodInMeal`)
    REFERENCES `mydb`.`Meals` (`IdMeal`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Meals_has_Foods_Foods1`
    FOREIGN KEY (`IdFood`)
    REFERENCES `mydb`.`Foods` (`IdFood`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`HypoPredictions`
-- -----------------------------------------------------
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
CREATE TABLE IF NOT EXISTS `mydb`.`WeightsOfFoods` (
)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Recipes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Recipes` (
  `IdRecipe` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  `Description` VARCHAR(255) NULL,
  PRIMARY KEY (`IdRecipe`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`FoodsInRecipes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`FoodsInRecipes` (
  `IdRecipe` INT NOT NULL,
  `IdFood` INT NOT NULL,
  `Quantity` DOUBLE NULL COMMENT 'grams of this food eaten in this meal',
  `Carbohydrates` DOUBLE NULL COMMENT '[g]: grams of this foood eaten in this meal',
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1.1 = perfect! ',
  `QualitativeAccuracy` INT NULL,
  PRIMARY KEY (`IdRecipe`, `IdFood`),
  INDEX `fk_FoodsInRecipes_Foods1_idx` (`IdFood` ASC),
  CONSTRAINT `fk_FoodsInRecipes_Recipes1`
    FOREIGN KEY (`IdRecipe`)
    REFERENCES `mydb`.`Recipes` (`IdRecipe`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_FoodsInRecipes_Foods1`
    FOREIGN KEY (`IdFood`)
    REFERENCES `mydb`.`Foods` (`IdFood`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Alarms`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Alarms` (
  `idAlarm` INT NOT NULL,
  `TimeStart` DATETIME NULL,
  `TimeAlarm` DATETIME NULL,
  `Interval` DOUBLE,
  `Duration` DOUBLE,
  `IsRepeated` TINYINT NULL,
  `IsEnabled` TINYINT NULL,
  PRIMARY KEY (`idAlarm`))
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
