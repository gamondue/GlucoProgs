# Containers Feature - Documentation

## Overview
The **Containers** feature allows users to manage and select containers (pots, plates, bowls, etc.) with predefined tare weights when weighing food portions. This eliminates the need to manually enter container weights repeatedly.

## Architecture

### 1. **Business Object**
- **File**: `SharedGlucoMan/BusinessObjects/Container.cs`
- **Properties**:
  - `IdContainer` (int?) - Primary key
  - `Name` (string) - Container name (e.g., "Small pot")
  - `Weight` (DoubleAndText) - Tare weight in grams
  - `Notes` (string) - Optional notes
  - `PhotoFileName` (string) - Filename of container photo
- **Methods**:
  - `GetPhotoFullPath()` - Returns full path to photo file

### 2. **Business Layer**
- **File**: `SharedGlucoMan/BusinessLayer/BL_Containers.cs`
- **Methods**:
  - `GetAllContainers()` - Retrieve all containers
  - `GetOneContainer(int? idContainer)` - Get specific container
  - `SaveContainer(Container container)` - Insert or update container
  - `DeleteContainer(Container container)` - Remove container
  - `SearchContainers(string name)` - Search by name

### 3. **Data Layer**
- **File**: `SharedGlucoMan/DataLayer/SqLite/SqLite_Containers.cs`
- **Database Table**: `Containers`
  ```sql
  CREATE TABLE Containers (
      IdContainer   INTEGER PRIMARY KEY NOT NULL,
      Name          TEXT NOT NULL,
      Weight        REAL DEFAULT 0,
      Notes         TEXT,
      PhotoFileName TEXT
  );
  ```

### 4. **Photo Management**
- **Storage Location**: `FileSystem.AppDataDirectory/ContainerPhotos/`
- **Filename Format**: `container_yyyyMMdd_HHmmss.jpg`
- **Supported Formats**: JPEG images
- **Photo Capture**: Uses .NET MAUI `MediaPicker` API

### 5. **User Interface**

#### ContainersPage
- **File**: `GlucoMan.Maui/ContainersPage.xaml` + `.xaml.cs`
- **Layout**:
  - Top section: Photo preview, input fields, and Picture button
  - Middle section: Title "Saved containers"
  - Bottom section: ListView with all saved containers
- **Photo Features**:
  - Photo preview area (120x100 pixels)
  - "Picture" button to capture new photo
  - Tap photo to view full size or take new photo

#### TakePicturePage
- **File**: `GlucoMan.Maui/TakePicturePage.xaml` + `.xaml.cs`
- **Layout**:
  - Full-screen camera preview
  - Circular capture button (center)
  - Cancel button (left)
  - Switch camera button (right)
- **Features**:
  - Live camera preview
  - Permission handling
  - Photo capture with timestamp
  - Modal dialog pattern

## Integration with WeighFoodPage

### UI Changes
Three "pan.png" icons have been added to open ContainersPage:

1. **Raw food section** (M0)
   - Button: `btnRawTareContainer_Click`
   - Updates: `TxtM0RawTare`

2. **Cooked food section** (S1)
   - Button: `btnCookedTareContainer_Click`
   - Updates: `TxtS1CookedTare`

3. **Cooked portion section**
   - Button: `btnPortionTareContainer_Click`
   - Updates: `TxtCookedPortionTare`

### Workflow
1. User clicks on pan icon in WeighFoodPage
2. ContainersPage opens modally
3. User selects existing container OR creates new one
4. User clicks "Choose" button
5. Selected container weight is returned to WeighFoodPage
6. Tare field is automatically filled with container weight
7. Calculations are triggered automatically

## Default Containers
The database is pre-populated with common containers:
- Small pot (250g)
- Medium pot (450g)
- Large pot (650g)
- Small plate (120g)
- Large plate (180g)
- Bowl (150g)
- Glass (85g)

## Features

### ContainersPage Features
- ? Add new containers with name and weight
- ? View all saved containers in a list
- ? Select container from list
- ? Delete unwanted containers
- ? Integration with Calculator for weight input
- ? Clear fields button
- ? Modal dialog pattern (async/await)
- ? Photo capture and preview
- ? Photo storage in dedicated folder
- ? Tap photo to view or retake

### TakePicturePage Features
- ? Camera permission handling
- ? Live camera preview
- ? Circular capture button (native style)
- ? Camera switching capability
- ? Photo saved with timestamp
- ? Clean black UI for photography

### UI Style
The page follows the same visual style as `MealPage`:
- Light green background for input fields (`LightGreen`)
- Yellow-green background for numeric fields (`GreenYellow`)
- Icon buttons (plus/minus) matching existing UI
- Grid layout with labels above input fields
- ToolTips for all controls

## Error Handling
All methods include try-catch blocks with logging to `General.LogOfProgram`:
- Error logging for exceptions
- Event logging for user actions
- Safe defaults initialization
- DisplayAlert for user-friendly error messages

## Testing
To test the feature:
1. Open WeighFoodPage
2. Click any pan icon (3 locations available)
3. In ContainersPage, click "Picture" button
4. Grant camera permissions if requested
5. Take a photo of a container
6. Verify photo appears in preview area
7. Add container name and weight
8. Click "Plus" to save
9. Verify container appears in list
10. Select container from list
11. Verify photo, name, and weight load correctly
12. Test deletion of containers
13. Test calculator integration
14. Test "Choose" button to return value to WeighFoodPage

## Permissions Required
The app requires the following permissions in `Platforms/Android/AndroidManifest.xml`:
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
```

For iOS, add to `Platforms/iOS/Info.plist`:
```xml
<key>NSCameraUsageDescription</key>
<string>This app needs camera access to take photos of containers</string>
<key>NSPhotoLibraryAddUsageDescription</key>
<string>This app needs photo library access to save container photos</string>
```

## Photo Storage
- Photos are stored in: `FileSystem.AppDataDirectory/ContainerPhotos/`
- Filename format: `container_yyyyMMdd_HHmmss.jpg`
- Only filename is stored in database, not full path
- Photos persist across app sessions
- Photos are automatically cleaned up when container is deleted

## Future Enhancements
Possible improvements:
- Container categories (pots, plates, bowls, etc.)
- Photo gallery view with thumbnails
- Frequently used containers list
- Import/Export container data including photos
- Share containers between users
- Photo editing (crop, rotate, brightness)
- Multiple photos per container
- Video recording for container identification
- QR code generation for quick container selection
