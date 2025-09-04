using Android;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace GlucoMan
{
    internal static class AndroidExternalFilesHelper
    {
        internal static async Task<bool> RequestStoragePermissionsAsync()
        {
            var activity = Platform.CurrentActivity;
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted ||
                ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted ||
                ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ManageExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new string[] {
                    Manifest.Permission.WriteExternalStorage,
                    Manifest.Permission.ReadExternalStorage,
                    Manifest.Permission.ManageExternalStorage
                }, 1);

                // Attendi che l'utente conceda i permessi
                await Task.Delay(1000);
            }
            return ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted &&
                   ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted;
        }
        internal static async Task SaveFileToExternalPublicDirectoryAsync(string sourceInternalPathAndName, string destinationExternalPathAndName)
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
        internal static async Task<bool> ReadFileFromExternalStorageAsync(string sourceExternalPathAndName, string destinationExternalPathAndName)
        {

        }


            ////////// source must be read with ContentResolver
            ////////string fileName = Path.GetFileName(sourceExternalPathAndName);
            ////////int startOfExternalDocumentsPath = sourceExternalPathAndName.IndexOf(Android.OS.Environment.DirectoryDocuments);
            ////////string relativePath = Path.GetDirectoryName(sourceExternalPathAndName);
            ////////relativePath = relativePath.Substring(startOfExternalDocumentsPath);
            ////////relativePath = relativePath.Replace("Documents/", "");

            ////////ContentValues contentValues = new ContentValues();
            ////////contentValues.Put(MediaStore.IMediaColumns.DisplayName, fileName);
            ////////contentValues.Put(MediaStore.IMediaColumns.MimeType, "application/octet-stream");
            ////////contentValues.Put(MediaStore.IMediaColumns.RelativePath, Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath));

            ////////ContentResolver resolver = Android.App.Application.Context.ContentResolver;
            ////////string selection = MediaStore.Files.FileColumns.Data + "=?";
            ////////string[] selectionArgs = new string[] { sourceExternalPathAndName };
            ////////////string selection = MediaStore.Files.FileColumns.Data + "=? AND " + MediaStore.Files.FileColumns.RelativePath + "=?";
            ////////////string[] selectionArgs = new string[] { fileName, Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath) };
            //////////string selection = MediaStore.Files.FileColumns.Data + "=? AND " + MediaStore.Files.FileColumns.RelativePath + "=" + relativePath;
            //////////string[] selectionArgs = new string[] { fileName }; //, Path.Combine(Android.OS.Environment.DirectoryDocuments, relativePath) };


            //////////Android.Net.Uri uri = MediaStore.Files.GetContentUri("external", contentValues);

            ////////Android.Net.Uri uri = resolver.Insert(MediaStore.Files.GetContentUri("external"), contentValues);

            ////////byte[] fileContent = null;
            ////////// Query the file
            ////////var cursor = resolver.Query(uri, null, selection, selectionArgs, null);
            //////////if (uri != null && cursor != null && cursor.MoveToFirst())
            //////////{
            ////////try
            ////////{
            ////////    int idColumn = cursor.GetColumnIndexOrThrow(MediaStore.Files.FileColumns.Id);
            ////////    long id = cursor.GetLong(idColumn);
            ////////    uri = Android.Net.Uri.WithAppendedPath(uri, id.ToString());

            ////////    // read all bytes of the file through a stream
            ////////    using (Stream inputStream = resolver.OpenInputStream(uri))
            ////////    {
            ////////        using (MemoryStream memoryStream = new MemoryStream())
            ////////        {
            ////////            await inputStream.CopyToAsync(memoryStream);
            ////////            fileContent = memoryStream.ToArray();
            ////////        }
            ////////    }
            ////////    cursor.Close();
            ////////    await File.WriteAllBytesAsync(destinationExternalPathAndName, fileContent);
            ////////    return true;
            ////////}
            ////////catch (Exception e)
            ////////{
            ////////    return false;
            ////////}
            //}
            //else
            //{
            //    cursor?.Close();
            //    return false;
            //}
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
