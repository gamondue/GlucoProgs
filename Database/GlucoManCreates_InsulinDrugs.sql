--
-- File generated with SQLiteStudio v3.4.17 on lun set 1 11:07:20 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: InsulinDrugs
DROP TABLE IF EXISTS InsulinDrugs;
CREATE TABLE InsulinDrugs (IdInsulinDrug INT NOT NULL, Name VARCHAR (32), Manufacturer VARCHAR (32), TypeOfInsulinAction INTEGER, DurationInHours DOUBLE, OnsetTimeInHours DOUBLE, PeakTimeInHours DOUBLE, PRIMARY KEY (IdInsulinDrug));
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (1, 'Humalog', 'Lilly', 20, 4.0, 0.25, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (2, 'Toujeo', 'Sanofi', 40, 36.0, 3.5, 0.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (4, 'Fiasp', 'Novo Nordisk', 20, 3.0, 0.03, 1.5);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (3, 'Lispro', 'Lilly', 20, 4.5, 0.3333333, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (5, 'Lantus', 'Sanofi', 40, 24.0, 4.0, 0.0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
