INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (2, 'Toujeo', 'Sanofi', 40, 36.0, 3.5, 0.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (1, 'Humalog', 'Lilly', 20, 4.0, 0.25, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (4, 'Fiasp', 'Novo Nordisk', 20, 3.0, 0.03, 1.5);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (3, 'Lispro', '', 20, 4.5, 0.3333333, 2.0);
INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours) VALUES (5, 'Lantus', '', 40, 24.0, 4.0, 0.0);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
