--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:42:32 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Ingredients
DROP TABLE IF EXISTS Ingredients;
CREATE TABLE Ingredients (
    IdIngredient          INTEGER NOT NULL,
    IdRecipe              INTEGER NOT NULL,
    Name                  TEXT,
    Description           TEXT,
    QuantityGrams         REAL,
    QuantityPercent       REAL,
    CarbohydratesPercent  REAL,
    AccuracyOfChoEstimate DOUBLE,
    IdFood                INTEGER,
    PRIMARY KEY (
        IdIngredient
    )
);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
