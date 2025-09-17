using Android.Content;
using Android.OS;
using Android.Provider;
using Microsoft.Maui.Storage;
using gamon;
using System.IO;
using AndroidX.Core.Content;
using Android;

namespace GlucoMan.BusinessLayer
{
    /// <summary>
    /// Enhanced file helper specifically designed for Huawei/Xiaomi devices
    /// with Community Toolkit integration and robust fallback mechanisms
    /// </summary>
    public static class EnhancedFileHelper
    {
        /// <summary>
        /// Attempts to save a file using multiple methods for maximum compatibility
        /// </summary>
        public static async Task<(bool Success, string Path)> SaveFileWithFallback(string sourceFile, string fileName)
        {
            if (!File.Exists(sourceFile))
            {
                General.LogOfProgram.Error($"Source file not found: {sourceFile}", null);
                return (false, string.Empty);
            }

            var activity = Platform.CurrentActivity;
            if (activity == null)
            {
                General.LogOfProgram.Error("No current activity available", null);
                return (false, string.Empty);
            }

            // Method 1: Try app external files directory (most compatible)
            var result = await TrySaveToAppExternalDirectory(sourceFile, fileName, activity);
            if (result.Success)
            {
                General.LogOfProgram.Debug($"File saved using app external directory: {result.Path}");
                return result;
            }

            // Method 2: Try MediaStore if Android 10+
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                result = await TrySaveUsingMediaStore(sourceFile, fileName, activity);
                if (result.Success)
                {
                    General.LogOfProgram.Debug($"File saved using MediaStore: {result.Path}");
                    return result;
                }
            }

            // Method 3: Try Documents directory
            result = await TrySaveToDocumentsDirectory(sourceFile, fileName, activity);
            if (result.Success)
            {
                General.LogOfProgram.Debug($"File saved to Documents directory: {result.Path}");
                return result;
            }

            // Method 4: Last resort - app cache directory (always accessible)
            result = await TrySaveToAppCacheDirectory(sourceFile, fileName, activity);
            if (result.Success)
            {
                General.LogOfProgram.Debug($"File saved to app cache directory: {result.Path}");
                return result;
            }

            General.LogOfProgram.Error("All file saving methods failed", null);
            return (false, string.Empty);
        }

        private static async Task<(bool Success, string Path)> TrySaveToAppExternalDirectory(string sourceFile, string fileName, Android.App.Activity activity)
        {
            try
            {
                // Use app's external files directory - doesn't require special permissions
                var externalFilesDir = activity.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
                if (externalFilesDir != null)
                {
                    string glucoManDir = Path.Combine(externalFilesDir.AbsolutePath, "GlucoMan");
                    Directory.CreateDirectory(glucoManDir);

                    string destinationPath = Path.Combine(glucoManDir, fileName);
                    
                    byte[] fileContent = await File.ReadAllBytesAsync(sourceFile);
                    await File.WriteAllBytesAsync(destinationPath, fileContent);

                    if (File.Exists(destinationPath))
                    {
                        // Create a user-friendly path description
                        string userPath = $"Android/data/{activity.PackageName}/files/Downloads/GlucoMan/{fileName}";
                        return (true, userPath);
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("TrySaveToAppExternalDirectory", ex);
            }
            return (false, string.Empty);
        }

        private static async Task<(bool Success, string Path)> TrySaveUsingMediaStore(string sourceFile, string fileName, Android.App.Activity activity)
        {
            try
            {
                if (activity.ContentResolver == null) return (false, string.Empty);

                string mimeType = GetMimeType(fileName);
                
                var contentValues = new ContentValues();
                contentValues.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                contentValues.Put(MediaStore.IMediaColumns.MimeType, mimeType);
                contentValues.Put(MediaStore.IMediaColumns.RelativePath, "Download/GlucoMan");

                var uri = activity.ContentResolver.Insert(MediaStore.Downloads.ExternalContentUri, contentValues);
                if (uri != null)
                {
                    using var sourceStream = File.OpenRead(sourceFile);
                    using var outputStream = activity.ContentResolver.OpenOutputStream(uri);
                    
                    if (outputStream != null)
                    {
                        await sourceStream.CopyToAsync(outputStream);
                        return (true, $"Download/GlucoMan/{fileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("TrySaveUsingMediaStore", ex);
            }
            return (false, string.Empty);
        }

        private static async Task<(bool Success, string Path)> TrySaveToDocumentsDirectory(string sourceFile, string fileName, Android.App.Activity activity)
        {
            try
            {
                var documentsDir = activity.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                if (documentsDir != null)
                {
                    string glucoManDir = Path.Combine(documentsDir.AbsolutePath, "GlucoMan");
                    Directory.CreateDirectory(glucoManDir);

                    string destinationPath = Path.Combine(glucoManDir, fileName);
                    
                    byte[] fileContent = await File.ReadAllBytesAsync(sourceFile);
                    await File.WriteAllBytesAsync(destinationPath, fileContent);

                    if (File.Exists(destinationPath))
                    {
                        string userPath = $"Android/data/{activity.PackageName}/files/Documents/GlucoMan/{fileName}";
                        return (true, userPath);
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("TrySaveToDocumentsDirectory", ex);
            }
            return (false, string.Empty);
        }

        private static async Task<(bool Success, string Path)> TrySaveToAppCacheDirectory(string sourceFile, string fileName, Android.App.Activity activity)
        {
            try
            {
                string cacheDir = Path.Combine(activity.CacheDir.AbsolutePath, "GlucoManExports");
                Directory.CreateDirectory(cacheDir);

                string destinationPath = Path.Combine(cacheDir, fileName);
                
                byte[] fileContent = await File.ReadAllBytesAsync(sourceFile);
                await File.WriteAllBytesAsync(destinationPath, fileContent);

                if (File.Exists(destinationPath))
                {
                    string userPath = $"App Cache/GlucoManExports/{fileName}";
                    return (true, userPath);
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("TrySaveToAppCacheDirectory", ex);
            }
            return (false, string.Empty);
        }

        private static string GetMimeType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".sqlite" or ".db" => "application/x-sqlite3",
                ".log" or ".txt" => "text/plain",
                ".json" => "application/json",
                _ => "application/octet-stream"
            };
        }

        /// <summary>
        /// Creates a share intent for files that couldn't be saved directly
        /// </summary>
        public static async Task<bool> ShareFile(string sourceFile, string fileName)
        {
            try
            {
                var activity = Platform.CurrentActivity;
                if (activity == null) return false;

                // Copy to cache first
                string cacheDir = Path.Combine(activity.CacheDir.AbsolutePath, "shared");
                Directory.CreateDirectory(cacheDir);
                
                string cachedFile = Path.Combine(cacheDir, fileName);
                File.Copy(sourceFile, cachedFile, true);

                // Create file provider URI
                var file = new Java.IO.File(cachedFile);
                var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(
                    activity, 
                    $"{activity.PackageName}.fileprovider", 
                    file);

                // Create share intent
                var shareIntent = new Intent(Intent.ActionSend);
                shareIntent.SetType(GetMimeType(fileName));
                shareIntent.PutExtra(Intent.ExtraStream, uri);
                shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                shareIntent.SetFlags(ActivityFlags.NewTask);

                var chooserIntent = Intent.CreateChooser(shareIntent, $"Condividi {fileName}");
                chooserIntent.SetFlags(ActivityFlags.NewTask);
                
                activity.StartActivity(chooserIntent);
                
                General.LogOfProgram.Debug($"File shared successfully: {fileName}");
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("ShareFile", ex);
                return false;
            }
        }

        /// <summary>
        /// Check if basic file permissions are available
        /// </summary>
        public static bool HasBasicFilePermissions()
        {
            var activity = Platform.CurrentActivity;
            if (activity == null) return false;

            // Check if we can write to app's external files directory
            try
            {
                var externalFilesDir = activity.GetExternalFilesDir(null);
                if (externalFilesDir == null) return false;

                string testFile = Path.Combine(externalFilesDir.AbsolutePath, "test.tmp");
                File.WriteAllText(testFile, "test");
                
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                    return true;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("HasBasicFilePermissions", ex);
            }
            
            return false;
        }
    }
}