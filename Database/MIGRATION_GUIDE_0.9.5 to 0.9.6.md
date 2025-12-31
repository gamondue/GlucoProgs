# Database Migration Guide: 0.9.5 to 0.9.6 Schema

## Overview
This guide documents the migration from the OLD GlucoMan database schema to the CURRENT version, preserving all existing user data.

## Schema Changes Summary

### 1. **Foods Table**
**Added Columns:**
- `IsRaw` (TINYINT): Indicates if the food is in raw/uncooked state
- `RawCookedRatio` (DOUBLE): Ratio between raw and cooked weight

**Impact:** Existing food records will have these fields as NULL until updated by the user.

---

### 2. **Parameters Table** - MAJOR CHANGES

The Parameters table has significant restructuring in the weighing-related fields.

#### Removed/Renamed Columns:
```
OLD NAME                        ? NEW NAME (or removed)
----------------------------------------------------
Weigh_CookedPortionGross       ? Weigh_PortionGross
Weigh_CookedPortionTare        ? Weigh_PortionTare  
Weigh_CookedPortionNet         ? Weigh_PortionNet
```

#### New Columns Added:
```sql
-- Food identification
Weigh_FoodId TEXT
Weigh_FoodName TEXT
Weigh_FoodCarbohydratesPercent DOUBLE
Weigh_TotalCarbohydratesPercent DOUBLE

-- Seasoning measurements (NEW feature)
Weigh_SeasoningGross DOUBLE
Weigh_SeasoningTare DOUBLE
Weigh_SeasoningNet DOUBLE
Weigh_SeasoningCarbohydratesPercent DOUBLE

-- Weighing options (NEW feature)
Weigh_DoWeighCookedPortion TEXT
Weigh_IsChoOfRawFood TEXT
Weigh_RawCookedRatio DOUBLE

-- Calculated results (NEW feature)
Weigh_WeightOfPortion DOUBLE
Weigh_CarbohydratesOfPortion DOUBLE
```

#### Retained Columns (redefined but data preserved):
```sql
Weigh_RawGross DOUBLE
Weigh_RawTare DOUBLE
Weigh_RawNet DOUBLE
Weigh_CookedGross DOUBLE
Weigh_CookedTare DOUBLE
Weigh_CookedNet DOUBLE
Weigh_NPortions DOUBLE
```

---

### 3. **PositionsOfReferences Table**
**No structural changes.** 

The CURRENT schema includes updated default INSERT statements with:
- Different timestamps (2025-09-01 instead of 2025-08-31)
- Some zone reassignments (e.g., Zone 1?4, Zone 4?3)

**Migration Strategy:** Keep user's existing data unchanged, as these are personalized injection positions.

---

## Migration Process

### Prerequisites
1. **CRITICAL: Create a backup of your database**
   ```bash
   cp GlucoManData.Sqlite GlucoManData_BACKUP_[date].Sqlite
   ```

2. Verify database integrity:
   ```sql
   PRAGMA integrity_check;
   ```

### Running the Migration

1. Locate the migration script:
   ```
   Database/MigrationScript_OLD_to_CURRENT.sql
   ```

2. Execute the script using SQLite command line:
   ```bash
   sqlite3 path/to/GlucoManData.Sqlite < Database/MigrationScript_OLD_to_CURRENT.sql
   ```

   Or using a GUI tool like DB Browser for SQLite:
   - Open your database
   - Go to "Execute SQL" tab
   - Load and run `MigrationScript_OLD_to_CURRENT.sql`

3. The script will:
   - Add new columns to Foods table
   - Create `Parameters_OLD` backup table
   - Drop and recreate Parameters table with new schema
   - Migrate all existing data
   - Map old column names to new column names

---

## Post-Migration Verification

Run these queries to verify successful migration:

```sql
-- Check Foods table structure
PRAGMA table_info(Foods);

-- Verify Parameters migration
SELECT COUNT(*) FROM Parameters;
SELECT COUNT(*) FROM Parameters_OLD;

-- Compare specific record
SELECT * FROM Parameters WHERE IdParameters = 1;
SELECT * FROM Parameters_OLD WHERE IdParameters = 1;

-- Check PositionsOfReferences
SELECT COUNT(*) FROM PositionsOfReferences;
```

Expected results:
- Parameters and Parameters_OLD should have the same row count
- Foods table should show the two new columns (IsRaw, RawCookedRatio)
- PositionsOfReferences data should be unchanged

---

## Data Mapping Details

### Parameters Table Column Mapping

| OLD Column Name            | NEW Column Name          | Notes                           |
|---------------------------|--------------------------|---------------------------------|
| Weigh_RawGross            | Weigh_RawGross           | Direct copy                     |
| Weigh_RawTare             | Weigh_RawTare            | Direct copy                     |
| Weigh_RawNet              | Weigh_RawNet             | Direct copy                     |
| Weigh_CookedGross         | Weigh_CookedGross        | Direct copy                     |
| Weigh_CookedTare          | Weigh_CookedTare         | Direct copy                     |
| Weigh_CookedNet           | Weigh_CookedNet          | Direct copy                     |
| Weigh_CookedPortionGross  | Weigh_PortionGross       | **Renamed**                     |
| Weigh_CookedPortionTare   | Weigh_PortionTare        | **Renamed**                     |
| Weigh_CookedPortionNet    | Weigh_PortionNet         | **Renamed**                     |
| Weigh_NPortions           | Weigh_NPortions          | Direct copy                     |
| (not in OLD)              | Weigh_FoodId             | **New field**, set to NULL      |
| (not in OLD)              | Weigh_FoodName           | **New field**, set to NULL      |
| (not in OLD)              | Weigh_FoodCarbohydratesPercent | **New**, set to NULL   |
| (not in OLD)              | Weigh_TotalCarbohydratesPercent | **New**, set to NULL  |
| (not in OLD)              | Weigh_SeasoningGross     | **New field**, set to NULL      |
| (not in OLD)              | Weigh_SeasoningTare      | **New field**, set to NULL      |
| (not in OLD)              | Weigh_SeasoningNet       | **New field**, set to NULL      |
| (not in OLD)              | Weigh_SeasoningCarbohydratesPercent | **New**, set to NULL |
| (not in OLD)              | Weigh_DoWeighCookedPortion | **New field**, set to NULL    |
| (not in OLD)              | Weigh_IsChoOfRawFood     | **New field**, set to NULL      |
| (not in OLD)              | Weigh_RawCookedRatio     | **New field**, set to NULL      |
| (not in OLD)              | Weigh_WeightOfPortion    | **New field**, set to NULL      |
| (not in OLD)              | Weigh_CarbohydratesOfPortion | **New field**, set to NULL  |

---

## Rollback Procedure

If migration fails or causes issues:

1. Restore from backup:
   ```bash
   cp GlucoManData_BACKUP_[date].Sqlite GlucoManData.Sqlite
   ```

2. Or use the Parameters_OLD table:
   ```sql
   DROP TABLE Parameters;
   ALTER TABLE Parameters_OLD RENAME TO Parameters;
   ```

---

## Cleanup After Successful Migration

Once you've verified the migration and the application works correctly:

```sql
-- Remove the backup table to save space
DROP TABLE IF EXISTS Parameters_OLD;

-- Vacuum the database to reclaim space
VACUUM;
```

---

## New Features Enabled

After migration, users can access these new features:

1. **Enhanced Food Weighing:**
   - Track food name and carbohydrate percentage during weighing
   - Add seasonings with separate CHO% tracking
   - Store calculated total CHO% including seasonings

2. **Raw/Cooked Food Management:**
   - Mark foods as raw or cooked
   - Define raw-to-cooked weight ratios
   - Choose whether to calculate CHO from raw or cooked weight

3. **Improved Portion Tracking:**
   - Generalized "Portion" instead of "CookedPortion" naming
   - Store final calculated weight and carbohydrates
   - Support for dividing into equal portions vs. weighing single portion

---

## Troubleshooting

### Issue: "table Parameters already exists"
**Solution:** The old table wasn't dropped. Run:
```sql
DROP TABLE IF EXISTS Parameters;
```
Then re-run the migration script.

### Issue: Data loss in Parameters table
**Solution:** The Parameters_OLD backup table contains your original data:
```sql
SELECT * FROM Parameters_OLD WHERE IdParameters = 1;
```

### Issue: Application crashes after migration
**Solution:** 
1. Check application logs for specific errors
2. Verify all expected columns exist:
   ```sql
   PRAGMA table_info(Parameters);
   ```
3. If needed, restore from backup and contact support

---

## Database Schema Versioning

| Schema Version | Date       | File                                    | App Version        |
|---------------|------------|-----------------------------------------|--------------------|
| OLD           | 2025-08-31 | Sqlite_SqlToCreateDatabase_OLD.cs       | 0.9.5.x.x          |
| CURRENT       | 2025-10-27 | Sqlite_SqlToCreateDatabase.cs           | 0.9.6.250129+      |
| Migration     | 2025-01-XX | MigrationScript_OLD_to_CURRENT.sql      | 0.9.6.250129+      |

**Note:** Starting with version 0.9.6, GlucoMan uses a deterministic versioning system: `Major.Minor.Patch.BuildDate.BuildTime`

For details, see: `Database/VERSIONING_SYSTEM.md`

---

## Support

For issues or questions about migration:
1. Check application logs: `GlucoMan_Errors.txt`, `GlucoMan_Log.txt`
2. Verify backup exists before attempting fixes
3. Review this migration guide thoroughly
4. Contact development team with specific error messages

---

**Remember: Always maintain your database backup until you're certain the migration was successful!**
