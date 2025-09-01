--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:38:06 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Foods
DROP TABLE IF EXISTS Foods;
CREATE TABLE Foods (IdFood INT NOT NULL, Name VARCHAR (15), Description VARCHAR (256), Energy DOUBLE, TotalFatsPercent DOUBLE, SaturatedFatsPercent DOUBLE, MonounsaturatedFatsPercent DOUBLE, PolyunsaturatedFatsPercent DOUBLE, CarbohydratesPercent DOUBLE, SugarPercent DOUBLE, FibersPercent INT, ProteinsPercent INT, SaltPercent DOUBLE, PotassiumPercent DOUBLE, Cholesterol DOUBLE, GlycemicIndex DOUBLE, UnitSymbol TEXT, GramsInOneUnit DOUBLE, Manufacturer TEXT, Category REAL, PRIMARY KEY (IdFood));

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
