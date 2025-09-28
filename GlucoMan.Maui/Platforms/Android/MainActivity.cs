using Android.App;
using Android.Content.PM;
using Android.OS;
using GlucoMan.BusinessLayer;
using Android.Widget;
using System;
using System.Threading.Tasks;
using gamon;

namespace GlucoMan.Maui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set the static reference for the helper class
            ////////AndroidExternalFilesHelper.SetMainActivity(this);

            ////////// Start the permission flow (do not block UI thread)
            //////////_ = EnsureStoragePermissionsAsync();
        }

        private async Task EnsureStoragePermissionsAsync()
        {
            try
            {
                bool granted = await AndroidExternalFilesHelper.ProgramHasPermissions();
                if (!granted)
                {
                    // Notify user and log
                    try
                    {
                        Toast.MakeText(this, "Permessi storage non concessi. Alcune funzionalità potrebbero non funzionare.", ToastLength.Long).Show();
                    }
                    catch { }

                    General.LogOfProgram.Debug("Storage permissions not granted on startup");
                }
                else
                {
                    General.LogOfProgram.Debug("Storage permissions granted on startup");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error while requesting storage permissions in MainActivity", ex);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            // Forward the permission result to the helper class
            AndroidExternalFilesHelper.OnPermissionResult(requestCode, permissions, grantResults);
            ////////base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            // Handle the result from MANAGE_EXTERNAL_STORAGE permission request
            AndroidExternalFilesHelper.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
