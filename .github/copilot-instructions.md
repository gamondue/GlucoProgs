# GlucoProgs AI Coding Agent Instructions

## Project Overview
GlucoProgs is a diabetes management application suite with **GlucoMan** as the primary .NET MAUI app for glucose tracking, carbohydrate counting, and insulin bolus calculations. The project uses a **shared code architecture** spanning Windows and Android platforms with SQLite data persistence.

## Architecture & Code Organization

### Multi-Project Structure
- **GlucoMan.Maui**: Main .NET 9 MAUI application (Windows/Android) 
- **SharedGlucoMan**: Core business objects and data layer (shared project)
- **SharedGeneral**: Utility classes like `Logger`, `Safe`, SQL helpers
- **SharedMaui**: Platform-specific initialization and common MAUI code
- **TestGlucoMan**: NUnit test project
- **Database/**: SQLite schema definitions and data migration scripts

### Key Domain Models (SharedGlucoMan/BusinessObjects/)
- `Food`: Nutritional data with carbohydrate percentages, glycemic index
- `Meal`: Event-based meal tracking with CHO calculations and accuracy estimates  
- `GlucoseRecord`: Sensor/manual glucose measurements with device metadata
- `Injection`: Insulin injection tracking with timing and dosage
- `Alarm`: System notifications for hypo prediction and medication reminders

### Data Layer Pattern
- Abstract `DataLayer` class with SQLite implementation (`SharedGlucoMan/DataLayer/`)
- `SqlHelper` classes provide parameterized query building (`Sql.SqlString()`, `Sql.SqlDate()`)
- Database managed via `Common.Database` singleton initialized in `MauiProgram.cs`

## Platform-Specific Development

### Conditional Compilation & DI
- Use `#if WINDOWS` / `#if ANDROID` for platform-specific code
- Register platform services via DI in `MauiProgram.cs`:
  ```csharp
  #if ANDROID
      builder.Services.AddSingleton<ISystemAlarmScheduler, Android.SystemAlarmScheduler>();
  #elif WINDOWS  
      builder.Services.AddSingleton<ISystemAlarmScheduler, Windows.SystemAlarmScheduler>();
  ```

### Android Specific Considerations
- File system access via `AndroidExternalFilesHelper` for database/log export
- Portrait-only orientation enforced in `MainActivity` attributes
- Special handling for Xiaomi devices in file operations

### Windows Development
- Window sizing configured for mobile-like dimensions (400x950) in lifecycle events
- SQLite works seamlessly, no platform-specific database code needed

## Critical Developer Workflows

### Building & Running
- Primary target frameworks: `net9.0-windows10.0.19041.0` and `net9.0-android`
- Use Visual Studio 2022 with MAUI workload installed
- Deploy to Android: Build → Deploy, requires Android SDK 21+
- Deploy to Windows: F5 debug or Publish for distribution

### Database Schema Management
- **CRITICAL**: Use `Database/GlucoManCreates_CURRENT.sql` for latest schema
- Migration scripts in Database/ folder follow naming pattern `GlucoManCreates_YYYY-MM-DD.sql`
- Never modify database structure without corresponding schema script
- Test data deletion available via `Database/WARNING_Deletes of Tables for initial database.sql`

### Testing Strategy
- Unit tests in `TestGlucoMan/` using NUnit framework
- Android-specific tests in `TestGlucoMan.Android/` project
- Logger must be initialized in test setup: `General.LogOfProgram = new Logger(...)`

## Development Patterns & Conventions

### Business Layer Architecture
- Business logic classes in `SharedGlucoMan/BusinessLayer/` prefixed with `BL_`
- Example: `BL_MealAndFood` handles food calculations, `BL_GlucoseMeasurements` manages glucose data
- Access via static instances: `Common.MealAndFood_CommonBL` 

### Data Binding & UI
- Business objects implement property change notification for MAUI binding
- Use `DoubleAndText`, `DateTimeAndText` wrapper classes for dual string/numeric representation
- Navigation pattern: `await Navigation.PushAsync(new PageName())`

### Error Handling & Logging
- Comprehensive logging via `General.LogOfProgram` (initialized in `MauiProgram.cs`)
- Log files: `GlucoMan_Log.txt`, `GlucoMan_Errors.txt`, `GlucoMan_Debug.txt`
- Use `Logger.Error()` for exceptions, `Logger.Debug()` for development traces

### Configuration Management  
- App configuration stored via `DataLayer.SaveParameter()` / `RestoreParameter()`
- File paths determined by platform in `CommonDataMaui.SetGlobalParameters()`
- Windows: `%UserProfile%/GlucoMan`, Android: App local data + Downloads for export

## Essential Implementation Details

### Glucose & Meal Calculations
- Insulin bolus calculation involves carbohydrate ratios, correction factors, and timing
- Hypo prediction uses glucose trend analysis and active insulin calculations
- Food portions calculated as percentages (g/100g) stored in database, converted to absolute values in UI

### XAML Page Structure
- All pages inherit from `ContentPage`
- Common pattern: bind to business objects, handle navigation in code-behind
- Use `DisplayOrientationHelper` for orientation management across platforms

### SQLite Integration
- Connection string and file location managed in `Common.PathAndFileDatabase`
- All queries use parameterized SQL via `SqlHelper` static methods
- Database backup/restore functionality built into data layer

## When Modifying Code

1. **Database changes**: Always update schema scripts first, then modify business objects
2. **Platform features**: Implement interface in SharedGlucoMan, platform-specific implementations in Platforms/
3. **New business logic**: Follow BL_ naming pattern, add to Common class for global access
4. **UI changes**: Consider both Windows and Android layouts, test orientation changes
5. **Dependencies**: Update both main project and test projects when adding NuGet packages