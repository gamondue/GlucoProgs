-- -----------------------------------------------------
-- Table `Meals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Meals` ;

CREATE TABLE IF NOT EXISTS `Meals` (
  `IdMeal` INT NOT NULL,
  `Type` INT NULL,
  `TimeBegin` DATETIME NULL,
  `TimeEnd` DATETIME NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1',
  PRIMARY KEY (`IdMeal`))
-- -----------------------------------------------------
-- Table `Foods`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Foods` ;

CREATE TABLE IF NOT EXISTS `Foods` (
  `IdFood` INT NOT NULL,
  `Name` VARCHAR(15) NULL,
  `Description` VARCHAR(256) NULL,
  `Carbohydrates` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Sugar` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Fibers` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `kCalories` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food\n',
  `TotalFats` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food\n',
  `SaturatedFats` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food\n',
  `Proteins` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Salt` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `Potassium` DOUBLE NULL COMMENT '[g/100]: grams in 100 grams of this food',
  `GlycemicIndex` DOUBLE NULL COMMENT 'number',
  PRIMARY KEY (`IdFood`))


-- -----------------------------------------------------
-- Table `InsulinInjection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `InsulinInjection` ;

CREATE TABLE IF NOT EXISTS `InsulinInjection` (
  `idInsulinInjection` INT NOT NULL,
  `descInsulinInjection` VARCHAR(45) NULL,
  `amount` VARCHAR(45) NULL COMMENT '[Insulin Units]',
  PRIMARY KEY (`idInsulinInjection`))


-- -----------------------------------------------------
-- Table `Meals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Meals` ;

CREATE TABLE IF NOT EXISTS `Meals` (
  `IdMeal` INT NOT NULL,
  `Type` INT NULL,
  `TimeBegin` DATETIME NULL,
  `TimeEnd` DATETIME NULL,
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1',
  PRIMARY KEY (`IdMeal`))


-- -----------------------------------------------------
-- Table `FoodsInMeals`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `FoodsInMeals` ;

CREATE TABLE IF NOT EXISTS `FoodsInMeals` (
  `IdMeal` INT NOT NULL,
  `IdFood` INT NOT NULL,
  `Quantity` DOUBLE NULL COMMENT 'grams of this food eaten in this meal',
  `Carbohydrates` DOUBLE NULL COMMENT '[g]: grams of this foood eaten in this meal',
  `AccuracyOfChoEstimate` DOUBLE NULL COMMENT '0 .. 1.1 = perfect! ',
  PRIMARY KEY (`IdMeal`))

CREATE INDEX `fk_Meals_has_Foods_Foods1_idx` ON `FoodsInMeals` (`IdFood` ASC);

CREATE INDEX `fk_Meals_has_Foods_Meals_idx` ON `FoodsInMeals` (`IdMeal` ASC);


-- -----------------------------------------------------
-- Table `GlucoseRecords`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GlucoseRecords` ;

CREATE TABLE IF NOT EXISTS `GlucoseRecords` (
  `IdGlucoseRecord` INT NOT NULL,
  `Timestamp` DATETIME NULL,
  `DeviceType` INT NULL,
  `DeviceId` VARCHAR(45) NULL,
  `IdDocumentType` INT NULL,
  `GlucoseValue` DOUBLE NULL,
  `InsulinValue` DOUBLE NULL,
  `GlucoseValue_copy2` DOUBLE NULL,
  `GlucoseValue_copy3` DOUBLE NULL,
  `GlucoseString` VARCHAR(45) NULL,
  `InsulinString` VARCHAR(45) NULL,
  `TypeOfInsulinInjection` VARCHAR(45) NULL,
  `InsulinDrugName` VARCHAR(45) NULL,
  `TypeOfInsulinSpeed` INT NULL,
  `TypeOfGlucoseMeasurement` INT NULL,
  `GlucoseRecordscol` VARCHAR(45) NULL,
  PRIMARY KEY (`IdGlucoseRecord`))

0