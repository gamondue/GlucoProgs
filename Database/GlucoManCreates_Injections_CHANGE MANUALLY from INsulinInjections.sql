--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:43:43 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Injections
DROP TABLE IF EXISTS Injections;
CREATE TABLE Injections (IdInjection INT NOT NULL, Timestamp DATETIME, InsulinValue DOUBLE, InsulinCalculated DOUBLE, InjectionPositionX INT, InjectionPositionY INT, Notes VARCHAR (255), IdTypeOfInjection INT, IdTypeOfInsulinAction INT, IdInsulinDrug INT, InsulinString VARCHAR (45), Zone INTEGER, PRIMARY KEY (IdInjection));

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
