using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using gamon;

namespace GlucoMan.BusinessLayer
{
    public static class AndroidExternalFilesHelper
    {
        private static TaskCompletionSource<bool> permissionTaskCompletionSource;
        public static async Task<bool> ProgramHasPermissions()
        {
            // Check if permissions are granted, eventually after asking the user
            if (!await RequestStoragePermissionsAsync())
                return false;
            return true;
        }
        public static async Task<bool> RequestStoragePermissionsAsync()
        {
            var activity = Platform.CurrentActivity;
            // Ensure activity is not null
            if (activity == null)
                throw new InvalidOperationException("Current activity is null.");
            // Check if permissions are already granted
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {   // Android 11+
                if (Android.OS.Environment.IsExternalStorageManager)
                    return true;
            }
            else
            {   // Older Android
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted &&
                    ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted)
                {
                    return true;
                }
            }
            // Request permissions
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R) // Android 11+
            {
                var intent = new Intent(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);
                intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
                activity.StartActivity(intent);
                return false; // User must manually grant permission
            }
            else
            {
                permissionTaskCompletionSource = new TaskCompletionSource<bool>();
                ActivityCompat.RequestPermissions(activity, new string[]
                {
                    Manifest.Permission.WriteExternalStorage,
                    Manifest.Permission.ReadExternalStorage
                }, 1);
                return await permissionTaskCompletionSource.Task;
            }
        }
        // Callback to catch user's response
        private static void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == 1)
            {
                // check if all the authorizations have been granted
                bool allGranted = grantResults.All(result => result == Permission.Granted);
                permissionTaskCompletionSource?.SetResult(allGranted);
            }
        }
        public static async Task SaveFileToExternalStoragePublicDirectoryAsync
            (string sourceInternalPathAndName, string fileName)
        {
            try
            {
                // source is from internal storage, destination is to external storage
                // => source can be read with file class
                byte[] fileContent = await File.ReadAllBytesAsync(sourceInternalPathAndName); // read the file content
                //int startOfExternalDocumentsPath = destinationExternalPathAndName.IndexOf(Android.OS.Environment.DirectoryDocuments);
                try
                {
                    // Documents folder
                    string externalDirectory = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "Glucoman");
                    // create the directory if it does not exist
                    if (!Directory.Exists(externalDirectory))
                    {
                        Directory.CreateDirectory(externalDirectory);
                    }
                    // file's full path
                    string filePath = Path.Combine(externalDirectory, fileName);
                    // Save file
                    await File.WriteAllBytesAsync(filePath, fileContent);

                    //// Log per confermare il salvataggio
                    //General.LogOfProgram.Info($"File salvato in: {filePath}");
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Errore durante il salvataggio del file", ex);
                 }
                //string relativePath = Path.GetDirectoryName(destinationExternalPathAndName);
                //relativePath = relativePath.Substring(startOfExternalDocumentsPath);
                //string fullPath = Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath, fileName);
                ////relativePath = relativePath.Replace("Documents/", "");

                //ContentValues contentValues = new ContentValues();
                //contentValues.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                //contentValues.Put(MediaStore.IMediaColumns.MimeType, "application/octet-stream");
                //contentValues.Put(MediaStore.IMediaColumns.RelativePath, Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath));

                //ContentResolver resolver = Android.App.Application.Context.ContentResolver;

                //Android.Net.Uri uri = resolver.Insert(MediaStore.Files.GetContentUri("external"), contentValues);

                //if (uri != null)
                //{
                //    using (Stream outputStream = resolver.OpenOutputStream(uri))
                //    {
                //        await outputStream.WriteAsync(fileContent, 0, fileContent.Length);
                //    }
                //}
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("AndroidExternalFilesHelper | SaveFileToExternalPublicDirectoryAsync", ex);
            }
        }
        public static async Task<bool> ReadFileFromExternalPublicDirectoryAsync
            (string sourceExternalFileName, string targetInternalPathAndFile)
        {
            try
            {
                // use Context.GetExternalFilesDir() to access the private directory of the app in the external storage
                // Documents folder
                string externalDirectory = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "Glucoman");
                string externalPathAndFile = Path.Combine(externalDirectory, sourceExternalFileName);
                // check if the external file exists
                if (!File.Exists(externalPathAndFile))
                {
                    General.LogOfProgram.Error($"File non trovato: {externalPathAndFile}", null);
                    return false;
                }
                // read the source file content from external storage
                byte[] fileContent = await File.ReadAllBytesAsync(externalPathAndFile);
                // wri the file content to the internal storage
                File.WriteAllBytesAsync(targetInternalPathAndFile, fileContent);
                //General.LogOfProgram.Info($"File copiato con successo da {externalPathAndFile} a {destinationInternalPath}");
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Errore durante la lettura del file dallo storage esterno", ex);
                return false;
            }
        }
        private static byte[] ReadBinaryFile(Context context, string filePath)
        {
            byte[] fileData = null;

            //if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            //{
            //    if (context.CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
            //    {
            //        Toast.MakeText(context, "Permission to read external storage is required.", ToastLength.Short).Show();
            //        return null;
            //    }
            //}

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, (int)fs.Length);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error reading file: {ex.Message}", ToastLength.Short).Show();
            }
            return fileData;
        }
    }
}
