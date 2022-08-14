using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using DiabetesRecords;
using Android;
using System.Reflection;
using System.IO;

namespace DiabetesRecord.Droid
{
    [Activity(Label = "DiabetesRecord", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            RequestPermissions(new string[]
                {Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage}, 0);

            BusinessLayer.PathExternalPublic = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;
            BusinessLayer.PathExternalPublic = Path.Combine(BusinessLayer.PathExternalPublic, "diabetesrecords"); 
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}