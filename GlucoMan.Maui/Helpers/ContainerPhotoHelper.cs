using gamon;

namespace GlucoMan.Maui.Helpers
{
    /// <summary>
    /// Helper class for managing container photos
    /// </summary>
    public static class ContainerPhotoHelper
    {
        private const string PhotoFolderName = "ContainerPhotos";
        
        /// <summary>
        /// Gets the full path to the container photos folder
        /// </summary>
        public static string GetPhotosFolderPath()
        {
            return Path.Combine(FileSystem.AppDataDirectory, PhotoFolderName);
        }
        
        /// <summary>
        /// Ensures the photos folder exists
        /// </summary>
        public static void EnsurePhotosFolderExists()
        {
            try
            {
                string folderPath = GetPhotosFolderPath();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    General.LogOfProgram?.Event($"ContainerPhotoHelper - Created photos folder: {folderPath}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("ContainerPhotoHelper - EnsurePhotosFolderExists", ex);
            }
        }
        
        /// <summary>
        /// Gets the full path for a photo filename
        /// </summary>
        public static string GetPhotoFullPath(string photoFileName)
        {
            if (string.IsNullOrWhiteSpace(photoFileName))
                return null;
                
            return Path.Combine(GetPhotosFolderPath(), photoFileName);
        }
        
        /// <summary>
        /// Checks if a photo file exists
        /// </summary>
        public static bool PhotoExists(string photoFileName)
        {
            if (string.IsNullOrWhiteSpace(photoFileName))
                return false;
                
            string fullPath = GetPhotoFullPath(photoFileName);
            return File.Exists(fullPath);
        }
        
        /// <summary>
        /// Deletes a photo file
        /// </summary>
        public static bool DeletePhoto(string photoFileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(photoFileName))
                    return false;
                    
                string fullPath = GetPhotoFullPath(photoFileName);
                
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    General.LogOfProgram?.Event($"ContainerPhotoHelper - Deleted photo: {photoFileName}");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error($"ContainerPhotoHelper - DeletePhoto: {photoFileName}", ex);
                return false;
            }
        }
        
        /// <summary>
        /// Generates a unique filename for a new container photo
        /// </summary>
        public static string GeneratePhotoFileName()
        {
            return $"container_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
        }
        
        /// <summary>
        /// Gets all photo files in the folder
        /// </summary>
        public static List<string> GetAllPhotoFiles()
        {
            try
            {
                string folderPath = GetPhotosFolderPath();
                
                if (!Directory.Exists(folderPath))
                    return new List<string>();
                    
                return Directory.GetFiles(folderPath, "*.jpg")
                    .Select(Path.GetFileName)
                    .ToList();
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("ContainerPhotoHelper - GetAllPhotoFiles", ex);
                return new List<string>();
            }
        }
        
        /// <summary>
        /// Cleans up orphaned photos (photos not referenced in database)
        /// </summary>
        public static int CleanupOrphanedPhotos(List<string> referencedPhotoFileNames)
        {
            try
            {
                var allPhotos = GetAllPhotoFiles();
                int deletedCount = 0;
                
                foreach (var photoFile in allPhotos)
                {
                    if (!referencedPhotoFileNames.Contains(photoFile))
                    {
                        if (DeletePhoto(photoFile))
                        {
                            deletedCount++;
                        }
                    }
                }
                
                General.LogOfProgram?.Event($"ContainerPhotoHelper - Cleaned up {deletedCount} orphaned photos");
                return deletedCount;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("ContainerPhotoHelper - CleanupOrphanedPhotos", ex);
                return 0;
            }
        }
    }
}
