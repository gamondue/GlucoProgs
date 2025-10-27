--
-- File generated with SQLiteStudio v3.4.17 on sab ott 25 16:17:02 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: SensorsRecords
DROP TABLE IF EXISTS SensorsRecords;
CREATE TABLE SensorsRecords (IdGlucoseRecord INT NOT NULL, GlucoseValue DOUBLE, Timestamp DATETIME, GlucoseString VARCHAR (45), IdTypeOfGlucoseMeasurement INT, IdTypeOfGlucoseMeasurementDevice INT, IdModelOfMeasurementSystem INT, IdDevice VARCHAR (45), IdDocumentType INT, Notes VARCHAR (255), PRIMARY KEY (IdGlucoseRecord));

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
