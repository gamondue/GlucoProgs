# GlucoMan new number of version System

## Overview
GlucoMan uses a **deterministic versioning system** that automatically generates version numbers based on build date and time.

## Version Format

```
FileVersion:      Major.Minor.Patch.BuildDate.BuildTime
AssemblyVersion:  Major.Minor.Patch.DaysSince2025
```

### Example
```
FileVersion:      0.9.6.251231.1407
AssemblyVersion:  0.9.6.365
```

Where:
- **0** = Major version
- **9** = Minor version  
- **6** = Patch version
- **251231** = Build date (YY-MM-DD): December 31, 2025 *(FileVersion only)*
- **1407** = Build time (HH:mm): 2:07 PM *(FileVersion only)*
- **365** = Days since January 1, 2025 *(AssemblyVersion only)*

## Version Components

### 1. Semantic Version (Major.Minor.Patch)
- **Major (0)**: Breaking changes, major features
- **Minor (9)**: New features, backward compatible
- **Patch (6)**: Bug fixes, minor improvements

This follows [Semantic Versioning 2.0.0](https://semver.org/) principles.

### 2. AssemblyVersion (4-part numeric)
**Format:** `Major.Minor.Patch.DaysSince2025`

.NET `AssemblyVersion` attribute requires 4 numeric parts (each 0-65535).

- **DaysSince2025**: Calculated as number of days since January 1, 2025
  - Example: Day 1 (Jan 1, 2025) = 0
  - Example: Day 365 (Dec 31, 2025) = 364
  - Example: Day 730 (Dec 31, 2026) = 729
  
This ensures:
- Chronological ordering
- Automatic increment each day
- Fits in ushort range (0-65535, supports ~179 years)

### 3. FileVersion (5-part with date/time)
**Format:** `Major.Minor.Patch.YYMMDD.HHmm`

Stored as string in file metadata, not limited to 4 parts.

#### Build Date (YYMMDD)
Automatically generated at compile time in 6-digit format:
- **YY**: Year (last 2 digits)
- **MM**: Month (01-12)
- **DD**: Day (01-31)

Examples:
- `250129` = January 29, 2025
- `250515` = May 15, 2025
- `261231` = December 31, 2026

#### Build Time (HHmm)
Automatically generated at compile time in 24-hour format:
- **HH**: Hour (00-23)
- **mm**: Minute (00-59)

Examples:
- `0830` = 8:30 AM
- `1445` = 2:45 PM
- `2359` = 11:59 PM

### 4. InformationalVersion
Same as `FileVersion` - provides human-readable version with full timestamp.

## Implementation

### MSBuild Properties
The version is generated in `GlucoMan.Maui.csproj` using MSBuild properties:

```xml
<!-- Base semantic version -->
<VersionMajor>0</VersionMajor>
<VersionMinor>9</VersionMinor>
<VersionPatch>6</VersionPatch>

<!-- AssemblyVersion: days since 2025-01-01 -->
<VersionBuildNumber>$([MSBuild]::Subtract(
    $([System.DateTime]::Now.Subtract($([System.DateTime]::Parse('2025-01-01'))).Days), 
    0))</VersionBuildNumber>

<!-- Assembled versions -->
<AssemblyVersion>0.9.6.$(VersionBuildNumber)</AssemblyVersion>
<FileVersion>0.9.6.$([System.DateTime]::Now.ToString('yyMMdd')).$([System.DateTime]::Now.ToString('HHmm'))</FileVersion>
<InformationalVersion>0.9.6.$([System.DateTime]::Now.ToString('yyMMdd')).$([System.DateTime]::Now.ToString('HHmm'))</InformationalVersion>
```

### Reading Version at Runtime

#### Get Assembly Version (numeric)
```csharp
var version = System.Reflection.Assembly
    .GetExecutingAssembly()
    .GetName()
    .Version;

// Returns: Version object with Major=0, Minor=9, Build=6, Revision=365
string versionString = version.ToString();
// Returns: "0.9.6.365"
```

#### Get File Version (with date/time)
```csharp
var assembly = System.Reflection.Assembly.GetExecutingAssembly();
var fileVersionAttr = assembly
    .GetCustomAttribute<System.Reflection.AssemblyFileVersionAttribute>();

string fileVersion = fileVersionAttr?.Version;
// Returns: "0.9.6.251231.1407"
```

#### Get Informational Version
```csharp
var assembly = System.Reflection.Assembly.GetExecutingAssembly();
var infoVersionAttr = assembly
    .GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>();

string infoVersion = infoVersionAttr?.InformationalVersion;
// Returns: "0.9.6.251231.1407"
```

### Display in UI
The `AboutPage.xaml.cs` displays the **Assembly Version** (numeric):

```csharp
public AboutPage()
{
    InitializeComponent();
    
    // Gets AssemblyVersion (e.g., "0.9.6.365")
    string version = System.Reflection.Assembly
        .GetExecutingAssembly()
        .GetName()
        .Version
        .ToString();
        
    lblAppName.Text += " " + version;
}
```

To show the full build date/time, use:

```csharp
public AboutPage()
{
    InitializeComponent();
    
    var assembly = System.Reflection.Assembly.GetExecutingAssembly();
    
    // Get FileVersion with full date/time
    var fileVersionAttr = assembly
        .GetCustomAttribute<System.Reflection.AssemblyFileVersionAttribute>();
    string fullVersion = fileVersionAttr?.Version ?? "0.0.0.0.0";
    
    lblAppName.Text += " " + fullVersion;
    // Displays: "GlucoMan 0.9.6.251231.1407"
}
```


## Platform-Specific Versioning

### Android (`ApplicationVersion`)
For Android, `ApplicationVersion` is an **integer** used as the version code:
- Uses build date as integer: `250129` (6 digits)
- Automatically increments with each new build day
- Required for Google Play Store updates

### iOS/macOS (`CFBundleVersion`)
Similar to Android, uses build date integer for bundle version.

### Windows (`FileVersion`)
Uses the full version string including build time for detailed tracking.

## Version Comparison

### Sorting Builds
Versions sort chronologically:

```
0.9.6.250125.0900  (Jan 25, 2025 at 9:00 AM)
0.9.6.250125.1430  (Jan 25, 2025 at 2:30 PM)  <- Later same day
0.9.6.250129.0815  (Jan 29, 2025 at 8:15 AM)  <- Later date
0.9.6.250201.1200  (Feb 1, 2025 at 12:00 PM)  <- Next month
```

### Parsing Version Components

```csharp
public static class VersionHelper
{
    public static (int major, int minor, int patch, DateTime buildDateTime) 
        ParseVersion(string version)
    {
        var parts = version.Split('.');
        if (parts.Length != 5)
            throw new ArgumentException("Invalid version format");
        
        int major = int.Parse(parts[0]);
        int minor = int.Parse(parts[1]);
        int patch = int.Parse(parts[2]);
        
        // Parse build date: YYMMDD
        string dateStr = parts[3];
        int year = 2000 + int.Parse(dateStr.Substring(0, 2));
        int month = int.Parse(dateStr.Substring(2, 2));
        int day = int.Parse(dateStr.Substring(4, 2));
        
        // Parse build time: HHmm
        string timeStr = parts[4];
        int hour = int.Parse(timeStr.Substring(0, 2));
        int minute = int.Parse(timeStr.Substring(2, 2));
        
        DateTime buildDateTime = new DateTime(year, month, day, hour, minute, 0);
        
        return (major, minor, patch, buildDateTime);
    }
}
```

## Benefits of Deterministic Versioning

### 1. **Automatic Version Increment**
No manual version updates needed - each build gets a unique version.

### 2. **Build Traceability**
Exact build date/time embedded in version number for debugging and support.

### 3. **Chronological Sorting**
Versions naturally sort by build time.

### 4. **No Version Conflicts**
Impossible to have two builds with the same version (unless built in same minute).

### 5. **Reproducible Builds**
When combined with `<Deterministic>True</Deterministic>`, ensures:
- Same source code ? Same binary output
- Verifiable builds for security audits

## Updating Semantic Version

To update the semantic version (e.g., for a new release):

### Minor Version Bump (New Features)
Edit `GlucoMan.Maui.csproj`:
```xml
<VersionMinor>10</VersionMinor>  <!-- Was 9 -->
```

Next build becomes: `0.10.6.250130.1000`

### Patch Version Bump (Bug Fixes)
```xml
<VersionPatch>7</VersionPatch>  <!-- Was 6 -->
```

Next build becomes: `0.9.7.250130.1000`

### Major Version Bump (Breaking Changes)
```xml
<VersionMajor>1</VersionMajor>  <!-- Was 0 -->
```

Next build becomes: `1.0.0.250130.1000`

## Version History Tracking

### Recommended Git Workflow

#### Tag Releases
```bash
# Tag a release build with semantic version
git tag -a v0.9.6 -m "Release 0.9.6 - Enhanced versioning system"
git push origin v0.9.6
```

#### Commit Messages
Include version in commit messages for important releases:
```bash
git commit -m "Release 0.9.6.250129.1445 - Deterministic versioning implemented"
```

### Release Notes Template
```markdown
## Version 0.9.6 (Build 250129.1445)

**Release Date:** January 29, 2025 at 2:45 PM

### New Features
- Implemented deterministic versioning system
- Auto-generated build date/time in version number

### Bug Fixes
- Fixed database migration script issues

### Known Issues
- None

### Build Information
- Full Version: 0.9.6.250129.1445
- Build Date: January 29, 2025
- Build Time: 14:45 (2:45 PM)
```

## Migration Notes

### Database Schema Versioning
The deterministic versioning complements the database migration system:

| Schema Version | Release Version | Build Date |
|---------------|-----------------|------------|
| OLD           | 0.9.5.x.x       | Pre-Jan 2025 |
| CURRENT       | 0.9.6.250129+   | Jan 29, 2025+ |

Reference: See `Database/MIGRATION_GUIDE.md` for schema migration details.

## Troubleshooting

### Issue: Version shows as "0.0.0.0"
**Cause:** MSBuild properties not evaluated correctly.

**Solution:** 
1. Clean and rebuild the project
2. Verify `<Deterministic>True</Deterministic>` is set
3. Check MSBuild output for property evaluation errors

### Issue: Build date is wrong timezone
**Cause:** `System.DateTime.Now` uses local system time.

**Solution:** This is intentional - build time reflects the builder's local timezone.

For UTC time, modify to:
```xml
<VersionBuildDate>$([System.DateTime]::UtcNow.ToString('yyMMdd'))</VersionBuildDate>
```

### Issue: Two builds have same version
**Cause:** Built within the same minute.

**Solution:** This is extremely rare in practice. If critical, add seconds:
```xml
<VersionBuildTime>$([System.DateTime]::Now.ToString('HHmmss'))</VersionBuildTime>
```
Results in: `0.9.6.250129.144530` (includes seconds)

## Best Practices

### 1. **Update Semantic Version Before Major Releases**
Don't rely only on build date/time - increment Major/Minor/Patch for significant releases.

### 2. **Document Version in Release Notes**
Always include full version string in release documentation.

### 3. **Tag Git Commits for Releases**
Use Git tags to mark important version milestones:
```bash
git tag v0.9.6.250129.1445
```

### 4. **Log Version on App Startup**
```csharp
General.LogOfProgram.Event($"GlucoMan started - Version: {Common.Version}");
```

### 5. **Display Version in About Page**
Already implemented in `AboutPage.xaml.cs` - ensure it remains visible to users.

## Future Enhancements

### Possible Improvements
1. **Git Commit Hash in Version**
   - Add short Git SHA to version metadata
   - Example: `0.9.6.250129.1445+g1a2b3c4`

2. **Build Configuration Indicator**
   - Append `-debug` or `-release` suffix
   - Example: `0.9.6.250129.1445-debug`

3. **Continuous Integration Build Number**
   - Integrate CI/CD pipeline build numbers
   - Example: `0.9.6.250129.1445-ci.42`

## References

- [Semantic Versioning 2.0.0](https://semver.org/)
- [.NET Assembly Versioning](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/versioning)
- [MSBuild Properties](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-properties)
- [Deterministic Builds](https://github.com/dotnet/reproducible-builds)

---

**Last Updated:** January 2025  
**Maintained by:** Ing. Gabriele Monti
