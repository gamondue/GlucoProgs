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
    
    // Cached thumbnail to avoid reloading
    private ImageSource _cachedThumbnail;
    private bool _thumbnailLoaded = false;
    
    public ImageSource ThumbnailSource
    {
        get
        {
            if (_thumbnailLoaded)
            {
                return _cachedThumbnail;
            }
            
            try
            {
                if (Container == null || string.IsNullOrWhiteSpace(Container.PhotoFileName))
    {
        // No photo - return null (no placeholder needed, the Border will show empty)
      _cachedThumbnail = null;
 _thumbnailLoaded = true;
          return _cachedThumbnail;
   }
                
    string photoPath = Container.GetPhotoFullPath();
      
     if (!string.IsNullOrWhiteSpace(photoPath) && File.Exists(photoPath))
           {
          General.LogOfProgram?.Debug($"ContainerViewModel - Loading thumbnail from: {photoPath}");
    
   // Load image from file - MAUI handles this efficiently
_cachedThumbnail = ImageSource.FromFile(photoPath);
           _thumbnailLoaded = true;
    return _cachedThumbnail;
        }
    
     General.LogOfProgram?.Debug($"ContainerViewModel - Photo file not found: {photoPath}");
                _cachedThumbnail = null;
          _thumbnailLoaded = true;
        return _cachedThumbnail;
  }
            catch (Exception ex)
            {
         General.LogOfProgram?.Error("ContainerViewModel - ThumbnailSource", ex);
    _cachedThumbnail = null;
        _thumbnailLoaded = true;
              return _cachedThumbnail;
     }
        }
    }
    
    public ContainerViewModel(Container container)
    {
        Container = container;
    }
    
    /// <summary>
    /// Forces reload of the thumbnail (useful after photo changes)
    /// </summary>
    public void ReloadThumbnail()
    {
        _thumbnailLoaded = false;
        _cachedThumbnail = null;
    }
}
