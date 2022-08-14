BEGIN TRANSACTION;
DROP TABLE IF EXISTS "DiabetesRecords";
CREATE TABLE IF NOT EXISTS "DiabetesRecords" (
	"IdDiabetesRecord"	INTEGER NOT NULL,
	"Timestamp"	DATETIME,
	"GlucoseValue"	DOUBLE,
	"InsulinValue"	DOUBLE,
	"IdTypeOfInsulinSpeed"	INTEGER,
	"IdTypeOfMeal"	INTEGER,
	"Notes"	TEXT,
	PRIMARY KEY('IdDiabetesRecord')
);
COMMIT;
