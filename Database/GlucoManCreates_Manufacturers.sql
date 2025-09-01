--
-- File generated with SQLiteStudio v3.4.17 on lun set 1 11:08:56 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Manufacturers
DROP TABLE IF EXISTS Manufacturers;
CREATE TABLE Manufacturers (
    IdManufacturer INTEGER PRIMARY KEY,
    Name           TEXT,
    Description    TEXT
);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
