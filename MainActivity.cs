const int REQUEST_WRITE = 1001;
if (AndroidX.Core.Content.ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage)
    != Android.Content.PM.Permission.Granted)
{
    AndroidX.Core.App.ActivityCompat.RequestPermissions(this,
        new[] { Android.Manifest.Permission.WriteExternalStorage }, REQUEST_WRITE);
}