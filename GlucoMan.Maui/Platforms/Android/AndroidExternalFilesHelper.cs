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
        public static async Task<bool> SaveFileToExternalStoragePublicDirectoryAsync
            (string sourceInternalPathAndName, string destinationPathAndName)
        {
            try
            {
                // Debug: verifica che il file sorgente esista
                General.LogOfProgram.Debug($"File sorgente: {sourceInternalPathAndName}");
                General.LogOfProgram.Debug($"File sorgente esiste: {File.Exists(sourceInternalPathAndName)}");
                General.LogOfProgram.Debug($"Destinazione: {destinationPathAndName}");
                
                if (!File.Exists(sourceInternalPathAndName))
                {
                    General.LogOfProgram.Error($"File sorgente non trovato: {sourceInternalPathAndName}", null);
                    return false;
                }

                string justfileName = Path.GetFileName(destinationPathAndName);
                byte[] fileContent = await File.ReadAllBytesAsync(sourceInternalPathAndName);
                
                try
                {
                    // Crea la directory di destinazione se non esiste
                    string destinationDirectory = Path.GetDirectoryName(destinationPathAndName);
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                        General.LogOfProgram.Debug($"Directory creata: {destinationDirectory}");
                    }
                    
                    // *** QUESTA È LA PARTE CHE MANCAVA: SCRIVERE IL FILE ***
                    await File.WriteAllBytesAsync(destinationPathAndName, fileContent);
                    General.LogOfProgram.Debug($"File scritto: {destinationPathAndName}");
                    
                    // Verifica che il file sia stato effettivamente creato
                    if (File.Exists(destinationPathAndName))
                    {
                        var fileInfo = new FileInfo(destinationPathAndName);
                        General.LogOfProgram.Debug($"File salvato con successo in: {destinationPathAndName}, dimensione: {fileInfo.Length} bytes");

                        // Notifica il sistema del nuovo file
                        var intent = new Intent(Intent.ActionMediaScannerScanFile);
                        intent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(destinationPathAndName)));
                        Platform.CurrentActivity?.SendBroadcast(intent);
                        
                        // Toast for the user
                        var activity = Platform.CurrentActivity;
                        if (activity != null)
                        {
                            activity.RunOnUiThread(() =>
                            {
                                Toast.MakeText(activity, $"File saved in: Download/GlucoMan/", ToastLength.Long).Show();
                            });
                        }              
                        return true;
                    }
                    else
                    {
                        General.LogOfProgram.Error("File not found after writing", null);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Error during file saving", ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("AndroidExternalFilesHelper | SaveFileToExternalStoragePublicDirectoryAsync", ex);
                return false;
            }
        }
        public static async Task<bool> ReadFileFromExternalPublicDirectoryAsync
            (string sourceExternalFileName, string targetInternalPathAndFile)
        {
            try
            {
                // use Context.GetExternalFilesDir() to access the private directory of the app in the external storage
                // Documents folder
                string externalDirectory = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "Glucoman");
                string externalPathAndFile = Path.Combine(externalDirectory, sourceExternalFileName);
                // check if the external file exists
                if (!File.Exists(externalPathAndFile))
                {
                    General.LogOfProgram.Error($"File not found: {externalPathAndFile}", null);
                    return false;
                }
                // read the source file content from external storage
                byte[] fileContent = await File.ReadAllBytesAsync(externalPathAndFile);
                // write the file content to the internal storage
                File.WriteAllBytesAsync(targetInternalPathAndFile, fileContent);
                //General.LogOfProgram.Info($"File copied successfully from {externalPathAndFile} to {destinationInternalPath}");
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error during file reading from external storage", ex);
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
