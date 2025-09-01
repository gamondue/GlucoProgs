--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:37:07 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: CategoriesOfFood
DROP TABLE IF EXISTS CategoriesOfFood;
CREATE TABLE CategoriesOfFood (IdCategoryOfFood INTEGER PRIMARY KEY, Name TEXT, Description TEXT);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
