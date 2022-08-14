using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;

namespace GlucoMan.Mobile.Droid
{
    [Activity(Label = "GlucoMan.Mobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            RequestPermissions(new string[] 
            {Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage}, 0);

            Common.SetAndroidPaths();
            // the next definition is in this Activity to have the Android.OS namespace
            // must be before SetGeneralPaths();
            Common.ExternalPublicPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;
            Common.SetGeneralPaths();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}