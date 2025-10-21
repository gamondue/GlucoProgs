# Container Photo Feature - Implementation Guide

## Overview
The Container Photo feature allows users to take photos of their food containers (pots, plates, bowls, etc.) using their device's camera. This visual reference helps users quickly identify and select the correct container when weighing food portions.

## Implementation Details

### 1. **New Components Created**

#### TakePicturePage
- **Purpose**: Dedicated page for capturing container photos
- **Features**:
  - Full-screen camera preview
  - Circular capture button (native camera app style)
  - Cancel and switch camera buttons
  - Permission handling
  - Auto-save to dedicated folder

#### ContainerPhotoHelper
- **Purpose**: Centralized photo management
- **Features**:
  - Photo folder creation and management
  - File naming conventions
  - Photo cleanup utilities
  - Orphaned photo detection

### 2. **Database Changes**

**New Column**: `PhotoFileName` (TEXT) in `Containers` table
- Stores only the filename, not the full path
- Nullable (existing containers have NULL)
- Format: `container_yyyyMMdd_HHmmss.jpg`

**Migration Path**:
- Existing databases: Column is added with NULL default
- New databases: Column is created from start
- No data loss for existing containers

### 3. **Photo Storage**

**Location**: `FileSystem.AppDataDirectory/ContainerPhotos/`
- Platform-specific paths:
  - Android: `/data/data/[package]/files/ContainerPhotos/`
  - iOS: `[App]/Library/ContainerPhotos/`
  - Windows: `%LOCALAPPDATA%/[App]/ContainerPhotos/`

**File Management**:
- Photos are saved as JPEG with timestamp
- Filename stored in database (not full path)
- Photos deleted automatically when container is deleted
- Orphaned photos can be cleaned up via helper

### 4. **User Flow**

1. **Taking a Photo**:
   - User clicks "Picture" button in ContainersPage
   - TakePicturePage opens with camera preview
   - User taps circular capture button
   - Photo is saved with unique timestamp name
   - Page closes and returns to ContainersPage
   - Photo appears in preview area

2. **Viewing a Photo**:
   - Photo preview shown in ContainersPage (120x100px)
   - Tap photo to view full size or take new photo
   - Photo loads from filesystem when container is selected

3. **Managing Photos**:
   - Photos are automatically deleted when container is deleted
   - Clear fields button clears photo preview
   - Orphaned photos can be cleaned up programmatically

### 5. **Permissions**

#### Android (AndroidManifest.xml)
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-feature android:name="android.hardware.camera" android:required="false" />
```

#### iOS (Info.plist)
```xml
<key>NSCameraUsageDescription</key>
<string>This app needs camera access to take photos of containers</string>
```

### 6. **Technology Stack**

- **.NET MAUI MediaPicker**: For camera access
- **FileSystem API**: For app data directory
- **ImageSource.FromFile**: For displaying photos
- **Border with RoundRectangle**: For circular capture button

### 7. **Error Handling**

All photo operations include:
- Try-catch blocks with logging
- Permission denied handling
- File not found handling
- User-friendly error messages
- Graceful degradation (app works without photos)

### 8. **Performance Considerations**

- Photos are loaded on-demand (not cached in memory)
- File I/O is async where possible
- Image resizing handled by .NET MAUI automatically
- Photos are JPEG compressed

### 9. **Testing Checklist**

- [ ] Camera permissions granted/denied
- [ ] Photo capture works
- [ ] Photo displays in preview
- [ ] Photo saves to correct folder
- [ ] Photo loads when selecting container
- [ ] Photo deletes with container
- [ ] App works without camera permission
- [ ] Works on Android device
- [ ] Works on iOS device
- [ ] Photo persists across app restarts

### 10. **Known Limitations**

1. **MediaPicker Limitations**:
   - No live camera preview (shows native camera app)
   - Limited control over camera settings
   - Camera switching via native UI only

2. **Platform Differences**:
   - Android: Shows native camera app
   - iOS: Shows native camera picker
   - Different UI on each platform

3. **Future Improvements Needed**:
   - Custom camera preview with CommunityToolkit.Maui.Camera
   - Real-time camera switching
   - Photo editing (crop, rotate)
   - Multiple photos per container
   - Photo compression settings

### 11. **Dependencies**

No additional NuGet packages required! Uses:
- .NET MAUI built-in MediaPicker
- .NET MAUI FileSystem
- .NET MAUI Permissions

### 12. **Troubleshooting**

**Problem**: Photos not appearing
- **Solution**: Check file permissions and folder existence

**Problem**: Camera permission denied
- **Solution**: Guide user to app settings to enable camera

**Problem**: Photo path not found
- **Solution**: Ensure ContainerPhotos folder is created at app start

**Problem**: Large app size
- **Solution**: Implement photo compression or resizing

## Code Examples

### Taking a Photo
```csharp
var photo = await MediaPicker.Default.CapturePhotoAsync();
if (photo != null)
{
    string fileName = ContainerPhotoHelper.GeneratePhotoFileName();
    string targetPath = ContainerPhotoHelper.GetPhotoFullPath(fileName);
    
    using var stream = await photo.OpenReadAsync();
    using var fileStream = File.Create(targetPath);
    await stream.CopyToAsync(fileStream);
}
```

### Loading a Photo
```csharp
string photoPath = container.GetPhotoFullPath();
if (File.Exists(photoPath))
{
    imgContainerPhoto.Source = ImageSource.FromFile(photoPath);
}
```

### Deleting a Photo
```csharp
ContainerPhotoHelper.DeletePhoto(container.PhotoFileName);
```

## Maintenance Notes

- Photos folder should be created at app startup
- Consider periodic cleanup of orphaned photos
- Monitor folder size (implement size limit if needed)
- Add photo count to app diagnostics
- Consider backup/restore for user data migration
