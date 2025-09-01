--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:40:43 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: FoodsInMeals
DROP TABLE IF EXISTS FoodsInMeals;
CREATE TABLE FoodsInMeals (IdFoodInMeal INT NOT NULL, IdMeal INT, IdFood INT, Name TEXT, CarbohydratesPercent INT, Quantity DOUBLE, UnitSymbol TEXT, GramsInOneUnit DOUBLE, QuantityInUnits DOUBLE, CarbohydratesGrams DOUBLE, AccuracyOfChoEstimate DOUBLE, PRIMARY KEY (IdFoodInMeal));

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
