--
-- File generated with SQLiteStudio v3.4.17 on dom ago 31 17:35:50 2025
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Alarms
DROP TABLE IF EXISTS Alarms;
CREATE TABLE Alarms (IdAlarm INT NOT NULL, ReminderText TEXT, TimeStart DATETIME, NextTriggerTime DATETIME, IsDisabled TINYINT, ValidTimeAfterStart INTEGER, RingingState TINYINT, Duration INTEGER, RepetitionTime INTEGER, Interval INTEGER, IsPlaying TINYINT, EnablePlaySoundFile TINYINT, SoundFilePath TEXT, RepeatCount INTEGER, MaxRepeatCount INTEGER, LastTriggerTime DATETIME, TriggeredCount INTEGER, DoVibrate TINYINT, PRIMARY KEY (IdAlarm));

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
