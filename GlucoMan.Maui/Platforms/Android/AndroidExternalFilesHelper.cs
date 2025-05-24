using Android;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using gamon;

namespace GlucoMan
{
    internal static class AndroidExternalFilesHelper
    {
        internal static TaskCompletionSource<bool> permissionTaskCompletionSource;
        internal static async Task<bool> RequestStoragePermissionsAsync()
        {
            var activity = Platform.CurrentActivity;
            // check if authorizations are already granted 
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted &&
                ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted &&
                ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ManageExternalStorage) == (int)Permission.Granted)
            {
                return true; // authorizations are already granted 
            }

            // create a new TaskCompletionSource to wait for the user's response
            permissionTaskCompletionSource = new TaskCompletionSource<bool>();
            // ask for authorizations
            ActivityCompat.RequestPermissions(activity, new string[]
            {
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.ManageExternalStorage
            }, 1);
            // wait for user's response
            return await permissionTaskCompletionSource.Task;
        }
        // Callback to catch user's response
        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == 1)
            {
                // check if all the authorizations have been granted
                bool allGranted = grantResults.All(result => result == Permission.Granted);
                permissionTaskCompletionSource?.SetResult(allGranted);
            }
        }
        internal static async Task SaveFileToExternalPublicDirectoryAsync
            (string sourceInternalPathAndName, string destinationExternalPathAndName)
        {
            try
            {
                // source is from internal storage, destination is to external storage
                // source can be read with file class
                byte[] fileContent = await File.ReadAllBytesAsync(sourceInternalPathAndName); // read the file content
                                                                                              // destination must be written with ContentResolver
                string fileName = Path.GetFileName(destinationExternalPathAndName);
                int startOfExternalDocumentsPath = destinationExternalPathAndName.IndexOf(Android.OS.Environment.DirectoryDocuments);
                string relativePath = Path.GetDirectoryName(destinationExternalPathAndName);
                relativePath = relativePath.Substring(startOfExternalDocumentsPath);
                relativePath = relativePath.Replace("Documents/", "");

                ContentValues contentValues = new ContentValues();
                contentValues.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                contentValues.Put(MediaStore.IMediaColumns.MimeType, "application/octet-stream");
                contentValues.Put(MediaStore.IMediaColumns.RelativePath, Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath));

                ContentResolver resolver = Android.App.Application.Context.ContentResolver;

                Android.Net.Uri uri = resolver.Insert(MediaStore.Files.GetContentUri("external"), contentValues);

                if (uri != null)
                {
                    using (Stream outputStream = resolver.OpenOutputStream(uri))
                    {
                        await outputStream.WriteAsync(fileContent, 0, fileContent.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("AndroidExternalFilesHelper | SaveFileToExternalPublicDirectoryAsync", ex);
            }
        }
        internal static async Task<bool> ReadFileFromExternalStorageAsync
            (string sourceExternalPathAndName, string destinationInternalPathAndName)
        {
            try
            {
                // Ottieni il nome del file e il percorso relativo
                string fileName = Path.GetFileName(sourceExternalPathAndName);
                string relativePath = Path.GetDirectoryName(sourceExternalPathAndName)?.Replace("\\", "/");
                relativePath = relativePath.Replace("Documents/", "");

                // Configura il ContentResolver per accedere al file
                ContentResolver resolver = Android.App.Application.Context.ContentResolver;

                // Proiezione dei campi richiesti
                string[] projection = new string[]
                {
                    MediaStore.MediaColumns.Id,
                    MediaStore.MediaColumns.DisplayName,
                    MediaStore.MediaColumns.RelativePath
                };

                // Costruisci la query per cercare il file nel MediaStore
                string selection = MediaStore.MediaColumns.DisplayName + "=? AND " +
                                   MediaStore.MediaColumns.RelativePath + "=?";
                string[] selectionArgs = new string[] { fileName, relativePath + "/" };

                // Ottieni l'URI del MediaStore
                Android.Net.Uri externalUri = MediaStore.Files.GetContentUri("external");

                // Esegui la query per trovare il file
                using (var cursor = resolver.Query(externalUri, projection, selection, selectionArgs, null))
                {
                    if (cursor != null && cursor.MoveToFirst())
                    {
                        // Ottieni l'ID del file
                        int idColumn = cursor.GetColumnIndexOrThrow(MediaStore.MediaColumns.Id);
                        long id = cursor.GetLong(idColumn);

                        // Costruisci l'URI del file
                        Android.Net.Uri fileUri = Android.Net.Uri.WithAppendedPath(externalUri, id.ToString());

                        // Leggi il contenuto del file
                        using (Stream inputStream = resolver.OpenInputStream(fileUri))
                        {
                            if (inputStream == null)
                                throw new Exception("Impossibile aprire il file dallo storage esterno.");

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                await inputStream.CopyToAsync(memoryStream);
                                byte[] fileContent = memoryStream.ToArray();

                                // Scrivi il contenuto nello storage interno
                                await File.WriteAllBytesAsync(destinationInternalPathAndName, fileContent);
                            }
                        }
                    }
                }
                return true;

                //// source must be read with ContentResolver
                //string sourceFileName = Path.GetFileName(sourceExternalPathAndName);
                //int startOfExternalDocumentsPath = sourceExternalPathAndName.IndexOf(Android.OS.Environment.DirectoryDocuments);
                //string relativePath = Path.GetDirectoryName(sourceExternalPathAndName)?.Replace("\\", "/");
                //relativePath = relativePath.Substring(startOfExternalDocumentsPath);
                //relativePath = relativePath.Replace("Documents/", "");

                //ContentValues contentValues = new ContentValues();
                //contentValues.Put(MediaStore.IMediaColumns.DisplayName, sourceFileName);
                //contentValues.Put(MediaStore.IMediaColumns.MimeType, "application/octet-stream");
                //contentValues.Put(MediaStore.IMediaColumns.RelativePath, Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath));

                //ContentResolver resolver = Android.App.Application.Context.ContentResolver;
                //Android.Net.Uri uri = resolver.Insert(MediaStore.Files.GetContentUri("external"), contentValues);

                //byte[] fileContent;

                //if (uri != null)
                //{
                //    using (Stream inputStream = resolver.OpenInputStream(uri))
                //    {
                //        using (MemoryStream memoryStream = new MemoryStream())
                //        {
                //            await inputStream.CopyToAsync(memoryStream);
                //            fileContent = memoryStream.ToArray();
                //        }
                //    }
                //}
                //else
                //{
                //    throw new Exception("Failed to retrieve file URI.");
                //}
                //// destination can be written with File class
                //await File.WriteAllBytesAsync(destinationInternalPathAndName, fileContent);

                //return true;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, $"Error reading file: {ex.Message}", ToastLength.Short).Show();
                return false;
            }
        }
        public static byte[] ReadBinaryFile(Context context, string filePath)
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
