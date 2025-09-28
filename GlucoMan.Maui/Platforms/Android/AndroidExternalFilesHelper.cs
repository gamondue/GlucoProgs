using Android;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using gamon;
using Java.IO;
using Microsoft.Maui.Storage;

namespace GlucoMan.BusinessLayer
{
    public static class AndroidExternalFilesHelper
    {
        private static TaskCompletionSource<bool> permissionTaskCompletionSource;
        private static Android.App.Activity mainActivity;
        private static TaskCompletionSource<bool> activityResultTaskCompletionSource;
        
        private const int STORAGE_PERMISSION_REQUEST = 1001;
        private const int MANAGE_EXTERNAL_STORAGE_REQUEST = 1002;
        private const int HUAWEI_PERMISSION_REQUEST = 1003;

        public static void SetMainActivity(Android.App.Activity activity)
        {
            mainActivity = activity;
        }
        public static async Task<bool> ProgramHasPermissions()
        {
            // Check if permissions are granted, eventually after asking the user
            if (!await RequestStoragePermissionsAsync())
                return false;
            return true;
        }
        private static bool IsHuaweiDevice()
        {
            string manufacturer = Build.Manufacturer?.ToLower();
            string brand = Build.Brand?.ToLower();
            return manufacturer?.Contains("huawei") == true || 
                   brand?.Contains("huawei") == true ||
                   manufacturer?.Contains("honor") == true ||
                   brand?.Contains("honor") == true;
        }
        private static bool IsXiaomiDevice()
        {
            string manufacturer = Build.Manufacturer?.ToLower();
            string brand = Build.Brand?.ToLower();
            return manufacturer?.Contains("xiaomi") == true || 
                   brand?.Contains("xiaomi") == true ||
                   manufacturer?.Contains("redmi") == true ||
                   brand?.Contains("redmi") == true;
        }
        public static async Task<bool> RequestStoragePermissionsAsync()
        {
            var activity = mainActivity ?? Platform.CurrentActivity;
            // Ensure activity is not null
            if (activity == null)
            {
                General.LogOfProgram.Error("Current activity is null in RequestStoragePermissionsAsync", null);
                return false;
            }

            General.LogOfProgram.Debug($"Device: {Build.Manufacturer} {Build.Brand} {Build.Model}");
            
            // Special handling for Huawei devices
            if (IsHuaweiDevice())
            {
                General.LogOfProgram.Debug("Detected Huawei device - using enhanced permission flow");
                return await RequestHuaweiStoragePermissions(activity);
            }

            // Special handling for Xiaomi devices
            if (IsXiaomiDevice())
            {
                General.LogOfProgram.Debug("Detected Xiaomi device - using enhanced permission flow");
                return await RequestXiaomiStoragePermissions(activity);
            }

            // Standard Android permission flow
            return await RequestStandardStoragePermissions(activity);
        }
        private static async Task<bool> RequestHuaweiStoragePermissions(Android.App.Activity activity)
        {
            try
            {
                // Check if permissions are already granted
                if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                {   
                    if (Android.OS.Environment.IsExternalStorageManager)
                    {
                        General.LogOfProgram.Debug("MANAGE_EXTERNAL_STORAGE already granted on Huawei");
                        return true;
                    }
                    
                    // For Huawei devices on Android 11+, first show informational dialog
                    await ShowHuaweiPermissionDialog(activity);
                    
                    // Request MANAGE_EXTERNAL_STORAGE permission
                    var intent = new Intent(Settings.ActionManageAppAllFilesAccessPermission);
                    intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
                    
                    activityResultTaskCompletionSource = new TaskCompletionSource<bool>();
                    activity.StartActivityForResult(intent, MANAGE_EXTERNAL_STORAGE_REQUEST);
                    
                    // Wait for user response with extended timeout for Huawei
                    var timeoutTask = Task.Delay(TimeSpan.FromMinutes(3));
                    var completedTask = await Task.WhenAny(activityResultTaskCompletionSource.Task, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        General.LogOfProgram.Error("Timeout waiting for Huawei MANAGE_EXTERNAL_STORAGE permission", null);
                        // Try fallback method
                        return await TryHuaweiFallbackPermissions(activity);
                    }
                    
                    bool result = await activityResultTaskCompletionSource.Task;
                    General.LogOfProgram.Debug($"Huawei MANAGE_EXTERNAL_STORAGE permission result: {result}");
                    
                    if (!result)
                    {
                        // Try fallback method
                        return await TryHuaweiFallbackPermissions(activity);
                    }
                    
                    return result;
                }
                else
                {
                    // Android 10 and below for Huawei
                    return await RequestLegacyPermissionsWithDialog(activity, "Huawei");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in RequestHuaweiStoragePermissions", ex);
                return await TryHuaweiFallbackPermissions(activity);
            }
        }
        private static async Task<bool> RequestXiaomiStoragePermissions(Android.App.Activity activity)
        {
            try
            {
                // Check if permissions are already granted
                if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                {   
                    if (Android.OS.Environment.IsExternalStorageManager)
                    {
                        General.LogOfProgram.Debug("MANAGE_EXTERNAL_STORAGE already granted on Xiaomi");
                        return true;
                    }
                    
                    // For Xiaomi devices, show specific instructions
                    await ShowXiaomiPermissionDialog(activity);
                    
                    var intent = new Intent(Settings.ActionManageAppAllFilesAccessPermission);
                    intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
                    
                    activityResultTaskCompletionSource = new TaskCompletionSource<bool>();
                    activity.StartActivityForResult(intent, MANAGE_EXTERNAL_STORAGE_REQUEST);
                    
                    var timeoutTask = Task.Delay(TimeSpan.FromMinutes(2));
                    var completedTask = await Task.WhenAny(activityResultTaskCompletionSource.Task, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        General.LogOfProgram.Error("Timeout waiting for Xiaomi MANAGE_EXTERNAL_STORAGE permission", null);
                        return false;
                    }
                    
                    bool result = await activityResultTaskCompletionSource.Task;
                    General.LogOfProgram.Debug($"Xiaomi MANAGE_EXTERNAL_STORAGE permission result: {result}");
                    return result;
                }
                else
                {
                    // Android 10 and below for Xiaomi
                    return await RequestLegacyPermissionsWithDialog(activity, "Xiaomi");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in RequestXiaomiStoragePermissions", ex);
                return false;
            }
        }
        private static async Task<bool> RequestStandardStoragePermissions(Android.App.Activity activity)
        {
            // Check if permissions are already granted
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {   
                // Android 11+ - Check for MANAGE_EXTERNAL_STORAGE
                if (Android.OS.Environment.IsExternalStorageManager)
                {
                    General.LogOfProgram.Debug("MANAGE_EXTERNAL_STORAGE already granted");
                    return true;
                }
                
                General.LogOfProgram.Debug("Requesting MANAGE_EXTERNAL_STORAGE permission");
                // Request MANAGE_EXTERNAL_STORAGE permission
                try 
                {
                    var intent = new Intent(Settings.ActionManageAppAllFilesAccessPermission);
                    intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
                    
                    activityResultTaskCompletionSource = new TaskCompletionSource<bool>();
                    activity.StartActivityForResult(intent, MANAGE_EXTERNAL_STORAGE_REQUEST);
                    
                    // Wait for user response with timeout
                    var timeoutTask = Task.Delay(TimeSpan.FromMinutes(2));
                    var completedTask = await Task.WhenAny(activityResultTaskCompletionSource.Task, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        General.LogOfProgram.Error("Timeout waiting for MANAGE_EXTERNAL_STORAGE permission", null);
                        return false;
                    }
                    
                    bool result = await activityResultTaskCompletionSource.Task;
                    General.LogOfProgram.Debug($"MANAGE_EXTERNAL_STORAGE permission result: {result}");
                    return result;
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Error requesting MANAGE_EXTERNAL_STORAGE permission", ex);
                    return false;
                }
            }
            else
            {   
                // Android 10 and below - Use legacy permissions
                return await RequestLegacyPermissionsWithDialog(activity, "Standard");
            }
        }
        private static async Task<bool> RequestLegacyPermissionsWithDialog(Android.App.Activity activity, string deviceType)
        {
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted &&
                ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted)
            {
                General.LogOfProgram.Debug($"Legacy storage permissions already granted on {deviceType}");
                return true;
            }

            General.LogOfProgram.Debug($"Requesting legacy storage permissions on {deviceType}");
            permissionTaskCompletionSource = new TaskCompletionSource<bool>();
            ActivityCompat.RequestPermissions(activity, new string[]
            {
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.ReadExternalStorage
            }, STORAGE_PERMISSION_REQUEST);
            
            var timeoutTask = Task.Delay(TimeSpan.FromMinutes(1));
            var completedTask = await Task.WhenAny(permissionTaskCompletionSource.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                General.LogOfProgram.Error($"Timeout waiting for storage permissions on {deviceType}", null);
                return false;
            }
            
            bool result = await permissionTaskCompletionSource.Task;
            General.LogOfProgram.Debug($"Legacy storage permissions result on {deviceType}: {result}");
            return result;
        }
        private static async Task ShowHuaweiPermissionDialog(Android.App.Activity activity)
        {
            var tcs = new TaskCompletionSource<bool>();
            
            activity.RunOnUiThread(() =>
            {
                var builder = new Android.App.AlertDialog.Builder(activity);
                builder.SetTitle("Autorizzazione Richiesta - Huawei")
                       .SetMessage("Per salvare i file su dispositivi Huawei:\n\n" +
                                 "1. Nella prossima schermata, trova 'GlucoMan'\n" +
                                 "2. Attiva 'Consenti accesso a tutti i file'\n" +
                                 "3. Se non vedi l'opzione, vai in Impostazioni > App > GlucoMan > Autorizzazioni\n" +
                                 "4. Potrebbe essere necessario disabilitare temporaneamente l'ottimizzazione della batteria per questa app")
                       .SetPositiveButton("Continua", (s, e) => tcs.SetResult(true))
                       .SetNegativeButton("Annulla", (s, e) => tcs.SetResult(false))
                       .SetCancelable(false)
                       .Create()
                       .Show();
            });
            
            await tcs.Task;
        }
        private static async Task ShowXiaomiPermissionDialog(Android.App.Activity activity)
        {
            var tcs = new TaskCompletionSource<bool>();
            
            activity.RunOnUiThread(() =>
            {
                var builder = new Android.App.AlertDialog.Builder(activity);
                builder.SetTitle("Autorizzazione Richiesta - Xiaomi")
                       .SetMessage("Per salvare i file su dispositivi Xiaomi/Redmi:\n\n" +
                                 "1. Nella prossima schermata, trova 'GlucoMan'\n" +
                                 "2. Attiva il toggle per 'Consenti accesso a tutti i file'\n" +
                                 "3. Se richiesto, conferma l'autorizzazione speciale")
                       .SetPositiveButton("Continua", (s, e) => tcs.SetResult(true))
                       .SetNegativeButton("Annulla", (s, e) => tcs.SetResult(false))
                       .SetCancelable(false)
                       .Create()
                       .Show();
            });
            
            await tcs.Task;
        }
        private static async Task<bool> TryHuaweiFallbackPermissions(Android.App.Activity activity)
        {
            try
            {
                General.LogOfProgram.Debug("Trying Huawei fallback permissions");
                
                // Try to open app settings directly
                var intent = new Intent(Settings.ActionApplicationDetailsSettings);
                intent.SetData(Android.Net.Uri.Parse($"package:{activity.PackageName}"));
                
                var tcs = new TaskCompletionSource<bool>();
                
                activity.RunOnUiThread(() =>
                {
                    var builder = new Android.App.AlertDialog.Builder(activity);
                    builder.SetTitle("Autorizzazioni Manuali Richieste")
                           .SetMessage("Si aprirà la pagina delle impostazioni dell'app.\n\n" +
                                     "Per Huawei:\n" +
                                     "1. Vai su 'Autorizzazioni'\n" +
                                     "2. Abilita 'Archiviazione' o 'File e media'\n" +
                                     "3. Torna all'app e riprova\n\n" +
                                     "Procedere?")
                           .SetPositiveButton("Apri Impostazioni", (s, e) => 
                           {
                               try
                               {
                                   activity.StartActivity(intent);
                                   tcs.SetResult(true);
                               }
                               catch (Exception ex)
                               {
                                   General.LogOfProgram.Error("Error opening Huawei app settings", ex);
                                   tcs.SetResult(false);
                               }
                           })
                           .SetNegativeButton("Annulla", (s, e) => tcs.SetResult(false))
                           .SetCancelable(false)
                           .Create()
                           .Show();
                });
                
                return await tcs.Task;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in TryHuaweiFallbackPermissions", ex);
                return false;
            }
        }
        // Callback to handle permission results
        public static void OnPermissionResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            General.LogOfProgram.Debug($"OnPermissionResult: requestCode={requestCode}, permissions={string.Join(",", permissions)}");
            
            if (requestCode == STORAGE_PERMISSION_REQUEST)
            {
                // check if all the authorizations have been granted
                bool allGranted = grantResults.All(result => result == Permission.Granted);
                General.LogOfProgram.Debug($"Storage permission result: {allGranted}");
                permissionTaskCompletionSource?.SetResult(allGranted);
            }
        }
        // Callback to handle activity results (for MANAGE_EXTERNAL_STORAGE)
        public static void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            General.LogOfProgram.Debug($"OnActivityResult: requestCode={requestCode}, resultCode={resultCode}");
            
            if (requestCode == MANAGE_EXTERNAL_STORAGE_REQUEST)
            {
                bool hasPermission = Build.VERSION.SdkInt >= BuildVersionCodes.R && 
                                   Android.OS.Environment.IsExternalStorageManager;
                General.LogOfProgram.Debug($"MANAGE_EXTERNAL_STORAGE activity result: {hasPermission}");
                activityResultTaskCompletionSource?.SetResult(hasPermission);
            }
        }
        public static async Task<bool> SaveFileToExternalStoragePublicDirectoryAsync
            (string sourceInternalPathAndName, string destinationPathAndName)
        {
            try
            {
                General.LogOfProgram.Debug($"SaveFileToExternalStoragePublicDirectoryAsync: source={sourceInternalPathAndName}, dest={destinationPathAndName}");
                
                if (!System.IO.File.Exists(sourceInternalPathAndName))
                {
                    General.LogOfProgram.Error($"Source file not found: {sourceInternalPathAndName}", null);
                    return false;
                }

                // Try multiple approaches based on Android version and OEM
                bool success = false;
                
                // First try: MediaStore (Android 10+)
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    General.LogOfProgram.Debug("Trying MediaStore approach");
                    success = await SaveFileUsingMediaStore(sourceInternalPathAndName, destinationPathAndName);
                }
                
                // Second try: Direct file access (if MediaStore failed or Android < 10)
                if (!success)
                {
                    General.LogOfProgram.Debug("Trying direct file access approach");
                    success = await SaveFileDirect(sourceInternalPathAndName, destinationPathAndName);
                }
                
                // Third try: Use app's external files directory as fallback
                if (!success && (IsHuaweiDevice() || IsXiaomiDevice()))
                {
                    General.LogOfProgram.Debug("Trying app external files directory fallback");
                    success = await SaveFileToAppExternalDirectory(sourceInternalPathAndName, destinationPathAndName);
                }
                
                return success;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("AndroidExternalFilesHelper | SaveFileToExternalStoragePublicDirectoryAsync", ex);
                return false;
            }
        }
        private static async Task<bool> SaveFileToAppExternalDirectory(string sourceInternalPathAndName, string destinationPathAndName)
        {
            try
            {
                var activity = mainActivity ?? Platform.CurrentActivity;
                if (activity == null) return false;
                
                // Use app's external files directory - this doesn't require special permissions
                var externalFilesDir = activity.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
                if (externalFilesDir == null) return false;
                
                string glucoManDir = System.IO.Path.Combine(externalFilesDir.AbsolutePath, "GlucoMan");
                System.IO.Directory.CreateDirectory(glucoManDir);
                
                string fileName = System.IO.Path.GetFileName(destinationPathAndName);
                string finalDestination = System.IO.Path.Combine(glucoManDir, fileName);
                
                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(sourceInternalPathAndName);
                await System.IO.File.WriteAllBytesAsync(finalDestination, fileContent);
                
                if (System.IO.File.Exists(finalDestination))
                {
                    General.LogOfProgram.Debug($"File saved to app external directory: {finalDestination}");
                    
                    // Show user where the file was saved
                    var currentActivity = Platform.CurrentActivity;
                    if (currentActivity != null)
                    {
                        currentActivity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(currentActivity, $"File salvato in: Android/data/it.ingmonti.glucoman/files/Downloads/GlucoMan/{fileName}", ToastLength.Long).Show();
                        });
                    }
                    
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in SaveFileToAppExternalDirectory", ex);
                return false;
            }
        }
        private static async Task<bool> SaveFileUsingMediaStore(string sourceInternalPathAndName, string destinationPathAndName)
        {
            try
            {
                var activity = mainActivity ?? Platform.CurrentActivity;
                if (activity?.ContentResolver == null)
                {
                    General.LogOfProgram.Error("ContentResolver is null", null);
                    return false;
                }

                string fileName = System.IO.Path.GetFileName(destinationPathAndName);
                string mimeType = GetMimeType(fileName);
                
                // Use MediaStore for Downloads directory
                var contentValues = new ContentValues();
                contentValues.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                contentValues.Put(MediaStore.IMediaColumns.MimeType, mimeType);
                contentValues.Put(MediaStore.IMediaColumns.RelativePath, "Download/GlucoMan");

                var uri = activity.ContentResolver.Insert(MediaStore.Downloads.ExternalContentUri, contentValues);
                if (uri == null)
                {
                    General.LogOfProgram.Error("Failed to create MediaStore URI", null);
                    return false;
                }

                using var sourceStream = System.IO.File.OpenRead(sourceInternalPathAndName);
                using var outputStream = activity.ContentResolver.OpenOutputStream(uri);
                
                if (outputStream == null)
                {
                    General.LogOfProgram.Error("Failed to open output stream", null);
                    return false;
                }

                await sourceStream.CopyToAsync(outputStream);
                
                General.LogOfProgram.Debug($"File successfully saved using MediaStore: {fileName}");
                
                // Show toast to user
                var currentActivity = Platform.CurrentActivity;
                if (currentActivity != null)
                {
                    currentActivity.RunOnUiThread(() =>
                    {
                        Toast.MakeText(currentActivity, $"File saved in: Download/GlucoMan/{fileName}", ToastLength.Long).Show();
                    });
                }
                
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in SaveFileUsingMediaStore", ex);
                return false;
            }
        }
        private static async Task<bool> SaveFileDirect(string sourceInternalPathAndName, string destinationPathAndName)
        {
            try
            {
                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(sourceInternalPathAndName);
                
                // Create destination directory if it doesn't exist
                string destinationDirectory = System.IO.Path.GetDirectoryName(destinationPathAndName);
                if (!System.IO.Directory.Exists(destinationDirectory))
                {
                    System.IO.Directory.CreateDirectory(destinationDirectory);
                    General.LogOfProgram.Debug($"Created directory: {destinationDirectory}");
                }
                
                await System.IO.File.WriteAllBytesAsync(destinationPathAndName, fileContent);
                General.LogOfProgram.Debug($"File written directly: {destinationPathAndName}");
                
                // Verify file was created
                if (System.IO.File.Exists(destinationPathAndName))
                {
                    var fileInfo = new System.IO.FileInfo(destinationPathAndName);
                    General.LogOfProgram.Debug($"File saved successfully: {destinationPathAndName}, size: {fileInfo.Length} bytes");

                    // Notify media scanner
                    var intent = new Intent(Intent.ActionMediaScannerScanFile);
                    intent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(destinationPathAndName)));
                    Platform.CurrentActivity?.SendBroadcast(intent);
                    
                    // Show toast
                    var activity = Platform.CurrentActivity;
                    if (activity != null)
                    {
                        activity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(activity, $"File saved: {System.IO.Path.GetFileName(destinationPathAndName)}", ToastLength.Long).Show();
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
                General.LogOfProgram.Error("Error in SaveFileDirect", ex);
                return false;
            }
        }
        public static async Task<bool> ReadFileFromExternalPublicDirectoryAsync
            (string sourceExternalFileName, string targetInternalPathAndFile)
        {
            try
            {
                General.LogOfProgram.Debug($"ReadFileFromExternalPublicDirectoryAsync: {sourceExternalFileName} -> {targetInternalPathAndFile}");
                
                bool success = false;
                
                // First try: Direct access to public Downloads/GlucoMan directory
                General.LogOfProgram.Debug("Trying direct read approach from public Downloads/GlucoMan directory");
                success = await ReadFileDirect(sourceExternalFileName, targetInternalPathAndFile);
                
                // Second try: MediaStore (Android 10+) - fallback
                if (!success && Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    General.LogOfProgram.Debug("Trying MediaStore read approach as fallback");
                    success = await ReadFileUsingMediaStore(sourceExternalFileName, targetInternalPathAndFile);
                }
                
                // Third try: App external files directory - last resort
                if (!success)
                {
                    General.LogOfProgram.Debug("Trying app external files directory read as last resort");
                    success = await ReadFileFromAppExternalDirectory(sourceExternalFileName, targetInternalPathAndFile);
                }
                
                return success;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in ReadFileFromExternalPublicDirectoryAsync", ex);
                return false;
            }
        }
        private static async Task<bool> ReadFileFromAppExternalDirectory(string sourceExternalFileName, string targetInternalPathAndFile)
        {
            try
            {
                var activity = mainActivity ?? Platform.CurrentActivity;
                if (activity == null) return false;
                
                var externalFilesDir = activity.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
                if (externalFilesDir == null) return false;
                
                string glucoManDir = System.IO.Path.Combine(externalFilesDir.AbsolutePath, "GlucoMan");
                string sourceFile = System.IO.Path.Combine(glucoManDir, sourceExternalFileName);
                
                if (!System.IO.File.Exists(sourceFile))
                {
                    General.LogOfProgram.Error($"File not found in app external directory: {sourceFile}", null);
                    return false;
                }
                
                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(sourceFile);
                await System.IO.File.WriteAllBytesAsync(targetInternalPathAndFile, fileContent);
                
                General.LogOfProgram.Debug($"File read from app external directory: {sourceExternalFileName}");
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in ReadFileFromAppExternalDirectory", ex);
                return false;
            }
        }
        private static async Task<bool> ReadFileUsingMediaStore(string sourceExternalFileName, string targetInternalPathAndFile)
        {
            try
            {
                var activity = mainActivity ?? Platform.CurrentActivity;
                if (activity?.ContentResolver == null) return false;

                // Query MediaStore for the file in Downloads directory
                // Use raw column name "_id" for the primary key (some bindings omit the constant)
                string idColumnName = "_id";
                string[] projection = { idColumnName, MediaStore.IMediaColumns.DisplayName, MediaStore.IMediaColumns.RelativePath };
                
                // Look specifically for files in Download/GlucoMan path
                string selection = $"{MediaStore.IMediaColumns.DisplayName} = ? AND {MediaStore.IMediaColumns.RelativePath} LIKE ?";
                string[] selectionArgs = { sourceExternalFileName, "%GlucoMan%" };

                General.LogOfProgram.Debug($"Searching in MediaStore for file: {sourceExternalFileName} in GlucoMan directory");

                using var cursor = activity.ContentResolver.Query(
                    MediaStore.Downloads.ExternalContentUri,
                    projection,
                    selection,
                    selectionArgs,
                    null);

                if (cursor != null && cursor.MoveToFirst())
                {
                    long id = cursor.GetLong(cursor.GetColumnIndexOrThrow(idColumnName));
                    var contentUri = ContentUris.WithAppendedId(MediaStore.Downloads.ExternalContentUri, id);

                    // Log the found file path for debugging
                    string relativePath = cursor.GetString(cursor.GetColumnIndexOrThrow(MediaStore.IMediaColumns.RelativePath));
                    General.LogOfProgram.Debug($"Found file in MediaStore: {relativePath}/{sourceExternalFileName}");

                    using var inputStream = activity.ContentResolver.OpenInputStream(contentUri);
                    if (inputStream != null)
                    {
                        using var outputStream = System.IO.File.Create(targetInternalPathAndFile);
                        await inputStream.CopyToAsync(outputStream);
                        
                        General.LogOfProgram.Debug($"File read successfully using MediaStore from GlucoMan directory: {sourceExternalFileName}");
                        return true;
                    }
                }                
                General.LogOfProgram.Error($"File not found in MediaStore GlucoMan directory: {sourceExternalFileName}", null);
                return false;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in ReadFileUsingMediaStore", ex);
                return false;
            }
        }
        private static async Task<bool> ReadFileDirect(string sourceExternalFileName, string targetInternalPathAndFile)
        {
            try
            {
                // Construct path to public Downloads/GlucoMan directory
                string externalDirectory = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "GlucoMan");
                string externalPathAndFile = System.IO.Path.Combine(externalDirectory, sourceExternalFileName);
                
                General.LogOfProgram.Debug($"Trying to read file from: {externalPathAndFile}");
                General.LogOfProgram.Debug($"External directory: {externalDirectory}");
                General.LogOfProgram.Debug($"Source file name: {sourceExternalFileName}");
                
                if (!System.IO.File.Exists(externalPathAndFile))
                {
                    General.LogOfProgram.Error($"File not found in Downloads/GlucoMan: {externalPathAndFile}", null);
                    
                    // List files in the directory for debugging
                    if (System.IO.Directory.Exists(externalDirectory))
                    {
                        var files = System.IO.Directory.GetFiles(externalDirectory);
                        General.LogOfProgram.Debug($"Files found in {externalDirectory}: {string.Join(", ", files.Select(System.IO.Path.GetFileName))}");
                    }
                    else
                    {
                        General.LogOfProgram.Error($"Directory not found: {externalDirectory}", null);
                    }
                    
                    return false;
                }
                
                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(externalPathAndFile);
                await System.IO.File.WriteAllBytesAsync(targetInternalPathAndFile, fileContent);
                
                General.LogOfProgram.Debug($"File read successfully from Downloads/GlucoMan: {sourceExternalFileName}");
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error in ReadFileDirect", ex);
                return false;
            }
        }
        private static string GetMimeType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".sqlite" or ".db" => "application/x-sqlite3",
                ".log" or ".txt" => "text/plain",
                ".json" => "application/json",
                _ => "application/octet-stream"
            };
        }
    }
}
