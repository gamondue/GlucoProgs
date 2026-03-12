using gamon;

namespace GlucoMan.BusinessLayer
{
    public class BL_Containers
    {
        DataLayer dl = Common.Database;
        
        public Container Container { get; set; }
        public List<Container> Containers { get; set; }
        
        public BL_Containers()
        {
            Container = new Container();
            Containers = new List<Container>();
        }
        
        /// <summary>
        /// Gets the photo folder path
        /// </summary>
        private string GetPhotosFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ContainerPhotos");
        }
        
        /// <summary>
        /// Deletes photo file associated with container
        /// </summary>
        private void DeleteContainerPhoto(string photoFileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(photoFileName))
                    return;
                    
                string photoPath = Path.Combine(GetPhotosFolderPath(), photoFileName);
                
                if (File.Exists(photoPath))
                {
                    File.Delete(photoPath);
                    General.LogOfProgram?.Event($"BL_Containers - Deleted photo: {photoFileName}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error($"BL_Containers - DeleteContainerPhoto: {photoFileName}", ex);
            }
        }
        
        /// <summary>
        /// Get all containers from the database
        /// </summary>
        public List<Container> GetAllContainers()
        {
            try
            {
                return dl.GetAllContainers();
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_Containers - GetAllContainers", ex);
                return new List<Container>();
            }
        }
        
        /// <summary>
        /// Get one container by ID
        /// </summary>
        public Container GetOneContainer(int? idContainer)
        {
            try
            {
                if (!idContainer.HasValue)
                    return null;
                    
                return dl.GetOneContainer(idContainer.Value);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_Containers - GetOneContainer", ex);
                return null;
            }
        }
        
        /// <summary>
        /// Save or update a container
        /// </summary>
        public int? SaveContainer(Container container)
        {
            try
            {
                if (container == null)
                    return null;
                    
                return dl.SaveContainer(container);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_Containers - SaveContainer", ex);
                return null;
            }
        }
        
        /// <summary>
        /// Delete a container and its associated photo
        /// </summary>
        public bool DeleteContainer(Container container)
        {
            try
            {
                if (container?.IdContainer == null)
                    return false;
                
                // Delete photo file first
                if (!string.IsNullOrWhiteSpace(container.PhotoFileName))
                {
                    DeleteContainerPhoto(container.PhotoFileName);
                }
                
                // Then delete from database
                return dl.DeleteContainer(container.IdContainer.Value);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_Containers - DeleteContainer", ex);
                return false;
            }
        }
        
        /// <summary>
        /// Search containers by name
        /// </summary>
        public List<Container> SearchContainers(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return GetAllContainers();
                    
                return dl.SearchContainers(name);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_Containers - SearchContainers", ex);
                return new List<Container>();
            }
        }
    }
}
