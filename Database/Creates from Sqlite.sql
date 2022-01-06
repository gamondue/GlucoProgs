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
	'QualitativeAccuracyOfChoEstimate'	INT,
	PRIMARY KEY('IdFood' AUTOINCREMENT)
);

CREATE TABLE 'FoodsInMeals' (
	'IdMeal'	INTEGER NOT NULL,
	'IdFood'	INT,
	'Quantity'	DOUBLE,
	'Carbohydrates'	DOUBLE,
	'AccuracyOfChoEstimate'	DOUBLE,
	PRIMARY KEY('IdMeal' AUTOINCREMENT)
);

CREATE TABLE 'GlucoseRecords' (
	'IdGlucoseRecord'	INTEGER NOT NULL,
	'Timestamp'	DATETIME,
	'GlucoseValue'	DOUBLE,
	'InsulinDrugName'	VARCHAR(45),
	'IdTypeOfGlucoseMeasurement'	INT,
	'IdTypeOfGlucoseMeasurementDevice'	INT,
	'IdModelOfMeasurementSystem'	INT,
	'DeviceId'	VARCHAR(45),
	'IdDocumentType'	INT,
	'GlucoseString'	VARCHAR(45),
	'TypeOfGlucoseMeasurement'	INT,
	PRIMARY KEY('IdGlucoseRecord' AUTOINCREMENT)
);

CREATE TABLE 'HypoPredictions' (
	'IdHypoPrediction'	INTEGER NOT NULL,
	'PredictedTime'	DATETIME,
	'AlarmTime'	DATETIME,
	'GlucoseSlope'	DOUBLE,
	'HypoGlucoseTarget'	INT,
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
	'InjectionPositionX'	INT,
	'InjectionPositionY'	INT,
	'IdTypeOfInjection'	INT,
	'IdTypeOfInsulinSpeed'	INT,
	'IdTypeOfInsulinInjection'	INT,
	'InsulinString'	VARCHAR(45),
	PRIMARY KEY('IdInsulineInjection' AUTOINCREMENT)
);

CREATE TABLE 'Meals' (
	'IdMeal'	INTEGER NOT NULL,
	'IdTypeOfMeal'	INT,
	'TimeBegin'	DATETIME,
	'TimeEnd'	DATETIME,
	'Carbohydrates'	DOUBLE,
	'AccuracyOfChoEstimate'	DOUBLE,
	'IdBolusCalculation'	INT,
	'IdGlucoseRecord'	INT,
	'IdQualitativeAccuracyCHO'	INT,
	'IdInsulineInjection'	INT,
	PRIMARY KEY('IdMeal' AUTOINCREMENT)
);

CREATE TABLE 'ModelsOfMeasurementSystem' (
	'IdModelOfMeasurementSystem'	INT NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdModelOfMeasurementSystem' AUTOINCREMENT)
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
	PRIMARY KEY('IdParameters' AUTOINCREMENT)
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
	'IdTypeOfInsulinInjection'	INT NOT NULL,
	'Name'	VARCHAR(255) NOT NULL,
	PRIMARY KEY('IdTypeOfInsulinInjection')
);

CREATE TABLE 'TypesOfInsulinSpeed' (
	'IdTypeOfInsulinSpeed'	INTEGER NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdTypeOfInsulinSpeed' AUTOINCREMENT)
);

CREATE TABLE 'TypesOfMeal' (
	'IdTypeOfMeal'	INT NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdTypeOfMeal')
);

