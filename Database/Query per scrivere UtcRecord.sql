-- ATTENZIONE: SOVRASCRIVE EVENTUALI VALORI PRECEDENTI!
-- Aggiorna Injections (ignora righe con timestamp NULL)
UPDATE Injections
SET UtcOffset = CASE 
  WHEN Timestamp >= '2017-03-26' AND Timestamp < '2017-10-29' THEN 2.0
  WHEN Timestamp >= '2018-03-25' AND Timestamp < '2018-10-28' THEN 2.0
  WHEN Timestamp >= '2019-03-31' AND Timestamp < '2019-10-27' THEN 2.0
  WHEN Timestamp >= '2020-03-29' AND Timestamp < '2020-10-25' THEN 2.0
  WHEN Timestamp >= '2021-03-28' AND Timestamp < '2021-10-31' THEN 2.0
  WHEN Timestamp >= '2022-03-27' AND Timestamp < '2022-10-30' THEN 2.0
  WHEN Timestamp >= '2023-03-26' AND Timestamp < '2023-10-29' THEN 2.0
  WHEN Timestamp >= '2024-03-31' AND Timestamp < '2024-10-27' THEN 2.0
  WHEN Timestamp >= '2025-03-30' AND Timestamp < '2025-10-26' THEN 2.0
  WHEN Timestamp >= '2026-03-29' AND Timestamp < '2026-10-25' THEN 2.0
  ELSE 1.0
END
WHERE Timestamp IS NOT NULL;

-- Aggiorna Meals (ignora righe con timestamp NULL)
UPDATE Meals
SET UtcOffset = CASE 
  WHEN TimeBegin >= '2017-03-26' AND TimeBegin < '2017-10-29' THEN 2.0
  WHEN TimeBegin >= '2018-03-25' AND TimeBegin < '2018-10-28' THEN 2.0
  WHEN TimeBegin >= '2019-03-31' AND TimeBegin < '2019-10-27' THEN 2.0
  WHEN TimeBegin >= '2020-03-29' AND TimeBegin < '2020-10-25' THEN 2.0
  WHEN TimeBegin >= '2021-03-28' AND TimeBegin < '2021-10-31' THEN 2.0
  WHEN TimeBegin >= '2022-03-27' AND TimeBegin < '2022-10-30' THEN 2.0
  WHEN TimeBegin >= '2023-03-26' AND TimeBegin < '2023-10-29' THEN 2.0
  WHEN TimeBegin >= '2024-03-31' AND TimeBegin < '2024-10-27' THEN 2.0
  WHEN TimeBegin >= '2025-03-30' AND TimeBegin < '2025-10-26' THEN 2.0
  WHEN TimeBegin >= '2026-03-29' AND TimeBegin < '2026-10-25' THEN 2.0
  ELSE 1.0
END
WHERE TimeBegin IS NOT NULL;

-- Aggiorna tabella GlucoseRecords
UPDATE GlucoseRecords
SET UtcOffset = CASE 
  WHEN strftime('%Y', TimeOfMeasurement) = '2017' AND TimeOfMeasurement >= '2017-03-26' AND TimeOfMeasurement < '2017-10-29' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2018' AND TimeOfMeasurement >= '2018-03-25' AND TimeOfMeasurement < '2018-10-28' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2019' AND TimeOfMeasurement >= '2019-03-31' AND TimeOfMeasurement < '2019-10-27' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2020' AND TimeOfMeasurement >= '2020-03-29' AND TimeOfMeasurement < '2020-10-25' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2021' AND TimeOfMeasurement >= '2021-03-28' AND TimeOfMeasurement < '2021-10-31' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2022' AND TimeOfMeasurement >= '2022-03-27' AND TimeOfMeasurement < '2022-10-30' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2023' AND TimeOfMeasurement >= '2023-03-26' AND TimeOfMeasurement < '2023-10-29' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2024' AND TimeOfMeasurement >= '2024-03-31' AND TimeOfMeasurement < '2024-10-27' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2025' AND TimeOfMeasurement >= '2025-03-30' AND TimeOfMeasurement < '2025-10-26' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2026' AND TimeOfMeasurement >= '2026-03-29' AND TimeOfMeasurement < '2026-10-25' THEN 2.0
  ELSE 1.0
END
WHERE TimeOfMeasurement IS NOT NULL;

-- Aggiorna tabella SensorsRecords
UPDATE SensorsRecords
SET UtcOffset = CASE 
  WHEN strftime('%Y', TimeOfMeasurement) = '2017' AND TimeOfMeasurement >= '2017-03-26' AND TimeOfMeasurement < '2017-10-29' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2018' AND TimeOfMeasurement >= '2018-03-25' AND TimeOfMeasurement < '2018-10-28' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2019' AND TimeOfMeasurement >= '2019-03-31' AND TimeOfMeasurement < '2019-10-27' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2020' AND TimeOfMeasurement >= '2020-03-29' AND TimeOfMeasurement < '2020-10-25' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2021' AND TimeOfMeasurement >= '2021-03-28' AND TimeOfMeasurement < '2021-10-31' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2022' AND TimeOfMeasurement >= '2022-03-27' AND TimeOfMeasurement < '2022-10-30' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2023' AND TimeOfMeasurement >= '2023-03-26' AND TimeOfMeasurement < '2023-10-29' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2024' AND TimeOfMeasurement >= '2024-03-31' AND TimeOfMeasurement < '2024-10-27' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2025' AND TimeOfMeasurement >= '2025-03-30' AND TimeOfMeasurement < '2025-10-26' THEN 2.0
  WHEN strftime('%Y', TimeOfMeasurement) = '2026' AND TimeOfMeasurement >= '2026-03-29' AND TimeOfMeasurement < '2026-10-25' THEN 2.0
  ELSE 1.0
END
WHERE TimeOfMeasurement IS NOT NULL;