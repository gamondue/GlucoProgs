using gamon;

namespace GlucoMan.Maui.Models;

/// <summary>
/// Wrapper for Container that includes UI-specific properties like thumbnail
/// </summary>
public class ContainerViewModel
{
    public Container Container { get; set; }
    
    // UI Properties
    public int? IdContainer => Container?.IdContainer;
    public string Name => Container?.Name ?? "";
    public DoubleAndText Weight => Container?.Weight;
    public string Notes => Container?.Notes ?? "";
    public string PhotoFileName => Container?.PhotoFileName ?? "";
    
    public ImageSource ThumbnailSource
    {
        get
        {
   // Temporaneamente disabilitato per debug
            return null;
      
         /*
      try
  {
        if (Container == null || string.IsNullOrWhiteSpace(Container.PhotoFileName))
   {
     // Use ImageSource for embedded resources, not FromFile
 return "container_placeholder.png";
  }
       
 string photoPath = Container.GetPhotoFullPath();
    
        if (!string.IsNullOrWhiteSpace(photoPath) && File.Exists(photoPath))
     {
        return ImageSource.FromFile(photoPath);
 }
       
      // Use ImageSource for embedded resources
         return "container_placeholder.png";
     }
        catch (Exception ex)
   {
      General.LogOfProgram?.Error("ContainerViewModel - ThumbnailSource", ex);
     // Use ImageSource for embedded resources
      return "container_placeholder.png";
      }
      */
     }
    }
    
    public ContainerViewModel(Container container)
    {
        Container = container;
    }
}
