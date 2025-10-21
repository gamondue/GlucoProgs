-- ============================================
-- Create table Containers
-- ============================================
-- This table stores information about containers (pots, plates, bowls, etc.)
-- used for weighing food. Each container has a name, tare weight, and optional photo.
-- ============================================

CREATE TABLE IF NOT EXISTS Containers (
    IdContainer   INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name          TEXT NOT NULL,
    Weight        REAL DEFAULT 0,
    Notes         TEXT,
    PhotoFileName TEXT
);

-- ============================================
-- Create index on Name for faster searches
-- ============================================
CREATE INDEX IF NOT EXISTS idx_Containers_Name ON Containers(Name);

-- ============================================
-- Insert some default containers as examples
-- ============================================
INSERT INTO Containers (Name, Weight, Notes, PhotoFileName) VALUES 
('Small pot', 250.0, 'Standard small cooking pot', NULL),
('Medium pot', 450.0, 'Standard medium cooking pot', NULL),
('Large pot', 650.0, 'Standard large cooking pot', NULL),
('Small plate', 120.0, 'Standard small dinner plate', NULL),
('Large plate', 180.0, 'Standard large dinner plate', NULL),
('Bowl', 150.0, 'Standard serving bowl', NULL),
('Glass', 85.0, 'Standard drinking glass', NULL);
