--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:47:38 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Recipes
DROP TABLE IF EXISTS Recipes;
CREATE TABLE Recipes (
    IdRecipe              INTEGER NOT NULL,
    Name                  TEXT,
    Description           TEXT,
    CarbohydratesPercent  REAL,
    AccuracyOfChoEstimate REAL,
    IsCooked              BOOL,
    RawToCookedRatio      REAL,
    PRIMARY KEY (
        IdRecipe
    )
);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
