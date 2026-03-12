namespace GlucoMan.Maui.Helpers;

/// <summary>
/// Helper to manage display orientation across platforms
/// </summary>
public static class DisplayOrientationHelper
{
    /// <summary>
    /// Sets the display orientation to Portrait only
    /// </summary>
    public static void LockToPortrait()
    {
#if ANDROID
   SetAndroidOrientation(Android.Content.PM.ScreenOrientation.Portrait);
#elif WINDOWS
      // Windows MAUI apps can rotate freely by default
        // No specific API needed for locking in Windows
#endif
    }

    /// <summary>
    /// Allows all orientations (portrait and landscape)
    /// </summary>
    public static void AllowAllOrientations()
    {
#if ANDROID
        SetAndroidOrientation(Android.Content.PM.ScreenOrientation.Unspecified);
#elif WINDOWS
        // Windows MAUI apps can rotate freely by default
#endif
    }

#if ANDROID
    private static void SetAndroidOrientation(Android.Content.PM.ScreenOrientation orientation)
 {
        try
        {
  var activity = Platform.CurrentActivity;
  if (activity != null)
{
         activity.RequestedOrientation = orientation;
      }
  }
        catch (Exception ex)
        {
   System.Diagnostics.Debug.WriteLine($"Error setting orientation: {ex.Message}");
        }
    }
#endif
}
