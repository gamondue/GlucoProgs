using gamon;

namespace gamon
{
    /// <summary>
    /// Represents a container (pot, plate, bowl, etc.) with its tare weight
    /// </summary>
    public class Container
    {
        public int? IdContainer { get; set; }
        public string Name { get; set; }
        public DoubleAndText Weight { get; set; }
        public string Notes { get; set; }
        public string PhotoFileName { get; set; }
        
        public Container()
        {
            Weight = new DoubleAndText();
            Name = "";
            Notes = "";
            PhotoFileName = "";
        }
        
        public Container(string name, double weight)
        {
            Name = name;
            Weight = new DoubleAndText { Double = weight };
            Notes = "";
            PhotoFileName = "";
        }
        
        /// <summary>
        /// Gets the full path to the photo file if it exists
        /// </summary>
        public string GetPhotoFullPath()
        {
            if (string.IsNullOrWhiteSpace(PhotoFileName))
                return null;
                
            string containersFolder = Path.Combine(FileSystem.AppDataDirectory, "ContainerPhotos");
            return Path.Combine(containersFolder, PhotoFileName);
        }
        
        public override string ToString()
        {
            return $"{Name} ({Weight.Text} g)";
        }
    }
}
