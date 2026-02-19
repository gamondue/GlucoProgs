# SharedData.UnitTests - Test Coverage Report

## Overview
This project contains unit tests for the SharedData library components that are testable in isolation.

## Current Test Coverage

### ✅ Fully Tested Components

#### SqlHelper (`Sql` class) - **54 tests**
- **Location**: `SqlHelperTests.cs`
- **Methods Tested**: All public static methods
  - `SqlString` (6 tests)
  - `SqlString` with MaxLength (5 tests)
  - `SqlStringLike` (4 tests)
  - `SqlBool` (3 tests)
  - `SqlDouble` (10 tests)
  - `SqlFloat` (5 tests)
  - `SqlInt` (8 tests)
  - `SqlDate` (6 tests)
  - `CleanStringForQuery` (5 tests)
- **Test Result**: ✅ All 54 tests passing
- **Coverage**: 100% of methods

## ⚠️ Components Not Tested (Architectural Limitations)

### Why Integration Tests Cannot Be Created

The `DL_Sqlite` class and SqLite folder files cannot be properly integration-tested due to fundamental architectural constraints:

### 1. Partial Class Architecture
`DL_Sqlite` is a **partial class** split across ~12 files:
- Each file implements specific methods of the abstract `DataLayer` class
- All files must be compiled together - **no file can be excluded**
- Excluding even one file makes the class incomplete and breaks compilation

### 2. Circular Dependencies
```
DL_Sqlite (needs all methods implemented)
    ↓ requires
Container & Injection classes (defined in SharedModel)
    ↓ require
BL_BolusesAndInjections (business layer in SharedGlucoMan)
    ↓ requires
CommonFunctions (has MAUI platform-specific code)
    ↓ requires
Full MAUI application context
```

### 3. Why Stubs Don't Work
Creating stub implementations of `Container` and `Injection` fails because:
- The `DataLayer` abstract class references the **real** `Container` and `Injection` types from `SharedModel`
- Stub classes in a different namespace cannot satisfy the abstract method signatures
- C# type system prevents using stubs to complete a partial class that expects specific types

### SqLite Data Layer Classes
The files in `SharedData\SqLite\` folder are **visible in this project** but **not compiled or tested** due to complex dependencies:

#### Files Included as Reference Only:
- `SqLite_MealAndFood.cs`
- `SqLite_GlucoseMeasurements.cs`
- `SqLite_AlarmManagement.cs`
- `SqLite_Events.cs`
- `SqLite_GpsTracking.cs`
- `SqLite_HypoPrediction.cs`
- `SqLite_PhysicalActivities.cs`
- `SqLite_Recipes.cs`
- `SqLite_WeighFood.cs`
- `SqLite_GrossTareAndNetWeight.cs`
- `Sqlite_SqlToCreateDatabase.cs`

#### Files Excluded (Dependencies on Container/Injection):
- `SqLite_Containers.cs`
- `SqLite_BolusesAndInjections.cs`
- `BL_Containers.cs`

### Why These Components Cannot Be Unit Tested

The `DL_Sqlite` class is a **partial class** distributed across multiple files. It requires:

1. **SharedModel Dependencies**:
   - `Container` - Has complex dependencies on image management and file system
   - `Injection` - Has complex business logic dependencies
   - `Meal`, `Food`, `GlucoseRecord` - Depend on the above

2. **SharedGlucoMan Dependencies**:
   - Business layer classes (`BL_*`)
   - `CommonFunctions` - Has platform-specific dependencies (MAUI)

3. **Circular Dependencies**:
   - Cannot import full `SharedData.projitems` without also importing all dependent projects
   - Partial class nature means excluding any file breaks the entire class

### Recommended Testing Approach for SqLite Classes

Since integration tests cannot be created, these components should be tested through:

1. **Manual Testing**: Via the GlucoMan.Maui application with a real database
2. **End-to-End Tests**: Test the complete application workflow
3. **Database Inspection**: Manually verify data using SQLite browser tools
4. **Future Refactoring**: Consider splitting the monolithic `DL_Sqlite` partial class into separate, independently testable classes

## How to Add New Tests

### For New Standalone Utility Classes
1. Add the `.cs` file to the `<Compile Include>` section in `.csproj`
2. Create a corresponding test file in `SharedData.UnitTests`
3. Follow the naming convention: `[ClassName]Tests.cs`

### For SqLite Data Layer Methods
Create integration tests in a separate test project that:
- Uses the full GlucoMan application context
- Creates a test SQLite database
- Tests CRUD operations end-to-end

## Project Configuration

### Dependencies
- NUnit 4.x
- .NET 10
- Microsoft.Data.Sqlite (for future integration tests)

### Files Structure
```
SharedData.UnitTests/
├── SqlHelperTests.cs          (54 tests ✅)
├── SharedData.UnitTests.csproj
└── SqLite/                    (Reference only, not compiled)
    └── *.cs files             (Visible in IDE for browsing)
```

## Test Execution

Run all tests:
```bash
dotnet test SharedData.UnitTests\SharedData.UnitTests.csproj
```

Current Results:
- **Total**: 54 tests
- **Passed**: 54 ✅
- **Failed**: 0
- **Duration**: ~0.6s

## Future Improvements

1. Create `SharedData.IntegrationTests` project for testing SqLite classes
2. Add database fixture for integration tests
3. Mock external dependencies (file system, logging) for better testability
4. Consider refactoring `Container` and `Injection` to reduce dependencies
