--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:46:24 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: ModelsOfMeasurementSystem
DROP TABLE IF EXISTS ModelsOfMeasurementSystem;
CREATE TABLE 'ModelsOfMeasurementSystem' (
	'IdModelOfMeasurementSystem'	INT NOT NULL,
	'Name'	VARCHAR(45),
	PRIMARY KEY('IdModelOfMeasurementSystem')
);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
