--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:49:09 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: UnitsOfFood
DROP TABLE IF EXISTS UnitsOfFood;
CREATE TABLE UnitsOfFood (
    IdUnitOfFood   INTEGER PRIMARY KEY,
    Symbol         TEXT,
    Name           TEXT,
    Description    TEXT,
    IdFood         INTEGER,
    GramsInOneUnit DOUBLE
);
INSERT INTO UnitsOfFood (IdUnitOfFood, Symbol, Name, Description, IdFood, GramsInOneUnit) VALUES (1, 'g', 'grams', 'Unit of mass in the SI', NULL, 1.0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
