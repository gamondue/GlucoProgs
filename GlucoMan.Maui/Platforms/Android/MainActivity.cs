using Android.App;
using Android.Content.PM;
using Android.OS;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set the static reference for the helper class
            AndroidExternalFilesHelper.SetMainActivity(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            // Forward the permission result to the helper class
            AndroidExternalFilesHelper.OnPermissionResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            // Handle the result from MANAGE_EXTERNAL_STORAGE permission request
            AndroidExternalFilesHelper.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
