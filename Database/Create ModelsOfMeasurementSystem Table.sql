--
-- File generated with SQLiteStudio v3.4.17 on dom ott 26 17:30:40 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: ModelsOfMeasurementSystem
DROP TABLE IF EXISTS ModelsOfMeasurementSystem;
CREATE TABLE ModelsOfMeasurementSystem (IdModelOfMeasurementSystem INT NOT NULL, Name VARCHAR (45), Description TEXT, PRIMARY KEY (IdModelOfMeasurementSystem));
INSERT INTO ModelsOfMeasurementSystem (IdModelOfMeasurementSystem, Name, Notes) VALUES (1, 'Freestyle Libre', 'Abbot underskin sensor');

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
