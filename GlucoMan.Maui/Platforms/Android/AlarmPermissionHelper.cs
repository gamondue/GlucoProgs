#if ANDROID
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace GlucoMan.Maui.Platforms.Android
{
    public static class AlarmPermissionHelper
    {
        /// <summary>
        /// Check if the app has exact alarm permissions (Android 12+)
        /// </summary>
        public static bool HasExactAlarmPermission(Context context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                var alarmManager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.S && alarmManager != null)
                {
                    return alarmManager.CanScheduleExactAlarms();
                }
            }
            return true; // Pre-Android 12 doesn't need this permission
        }

        /// <summary>
        /// Request exact alarm permission by opening system settings (Android 12+)
        /// </summary>
        public static void RequestExactAlarmPermission(Activity activity)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                try
                {
                    var intent = new Intent(global::Android.Provider.Settings.ActionRequestScheduleExactAlarm);
                    intent.SetData(global::Android.Net.Uri.Parse($"package:{activity.PackageName}"));
                    activity.StartActivity(intent);
                }
                catch (Exception ex)
                {
                    global::System.Diagnostics.Debug.WriteLine($"Error requesting exact alarm permission: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Check if the app has notification permission (Android 13+)
        /// </summary>
        public static bool HasNotificationPermission(Context context)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var result = ContextCompat.CheckSelfPermission(context, Manifest.Permission.PostNotifications);
                return result == Permission.Granted;
            }
            return true; // Pre-Android 13 doesn't need this permission
        }

        /// <summary>
        /// Request notification permission (Android 13+)
        /// </summary>
        public static void RequestNotificationPermission(Activity activity, int requestCode = 1001)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (!HasNotificationPermission(activity))
                {
                    ActivityCompat.RequestPermissions(
                        activity,
                        new[] { Manifest.Permission.PostNotifications },
                        requestCode);
                }
            }
        }

        /// <summary>
        /// Check and request all alarm-related permissions
        /// </summary>
        public static async Task<bool> CheckAndRequestAllPermissionsAsync(Activity activity)
        {
            bool hasAll = true;

            // Check notification permission (Android 13+)
            if (!HasNotificationPermission(activity))
            {
                hasAll = false;
                RequestNotificationPermission(activity);
                await Task.Delay(1000); // Give user time to respond
            }

            // Check exact alarm permission (Android 12+)
            if (!HasExactAlarmPermission(activity))
            {
                hasAll = false;
                RequestExactAlarmPermission(activity);
                await Task.Delay(1000); // Give user time to respond
            }

            return hasAll;
        }
    }
}
#endif

