using gamon;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class CropPhotoPage : ContentPage
{
    private string originalPhotoPath;
    private string croppedPhotoPath;

    // TaskCompletionSource for modal behavior
    private TaskCompletionSource<string> cropTaskSource = new TaskCompletionSource<string>();
    public Task<string> CropTask => cropTaskSource.Task;

    // Image position tracking for pan gesture
    private double currentX = 0;
    private double currentY = 0;
    private double startX = 0;
    private double startY = 0;
    
    // Smoothing for pan gesture to reduce jitter
    private double targetX = 0;
    private double targetY = 0;
    private const double SmoothingFactor = 0.3; // Lower = smoother but more lag
    
    // Debouncing for pan gesture to avoid vibration
    private bool isPanProcessing = false;

    // Crop overlay size tracking for pinch gesture
    private double currentCropSize = 300;
    private double startCropSize = 300;
    private const double MinCropSize = 100;
    private const double MaxCropSize = 600;
    
    // Smoothing for crop resize to reduce jitter
    private double targetCropSize = 300;
    private const double CropSmoothingFactor = 0.4; // Slightly more responsive than pan
    private bool isResizeProcessing = false;
    private System.Timers.Timer cropSmoothingTimer;

    // Gesture conflict prevention
    private bool isCornerDragging = false;
    private bool isPanning = false;

    // Original image dimensions
    private int originalImageWidth;
    private int originalImageHeight;

    public CropPhotoPage(string photoPath)
    {
        InitializeComponent();
        originalPhotoPath = photoPath;

        // Initialize smoothing timer for crop resize
        cropSmoothingTimer = new System.Timers.Timer(16); // ~60 FPS
        cropSmoothingTimer.Elapsed += CropSmoothingTimer_Elapsed;
        cropSmoothingTimer.AutoReset = true;

        // Load the photo
        if (File.Exists(photoPath))
        {
            imgPhoto.Source = ImageSource.FromFile(photoPath);

            // Get original image dimensions
#if ANDROID
            LoadImageDimensionsAndroid(photoPath);
#elif WINDOWS
            LoadImageDimensionsWindows(photoPath);
#endif
        }

        // Update size indicator
        UpdateSizeIndicator();

        // Hide zoom hint after 3 seconds
        HideZoomHintAfterDelay();
    }

    private async void HideZoomHintAfterDelay()
    {
        await Task.Delay(3000);
        if (lblZoomHint != null)
        {
            lblZoomHint.IsVisible = false;
        }
    }

#if ANDROID
    private void LoadImageDimensionsAndroid(string photoPath)
    {
        try
        {
            var options = new Android.Graphics.BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };
            Android.Graphics.BitmapFactory.DecodeFile(photoPath, options);

            originalImageWidth = options.OutWidth;
            originalImageHeight = options.OutHeight;

            General.LogOfProgram?.Debug($"CropPhotoPage - Original image size: {originalImageWidth}x{originalImageHeight}");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - LoadImageDimensionsAndroid", ex);
        }
    }
#endif

#if WINDOWS
    private void LoadImageDimensionsWindows(string photoPath)
    {
        try
        {
            using (var stream = File.OpenRead(photoPath))
            using (var image = System.Drawing.Image.FromStream(stream, false, false))
            {
                originalImageWidth = image.Width;
                originalImageHeight = image.Height;
            }

            General.LogOfProgram?.Debug($"CropPhotoPage - Original image size (Windows): {originalImageWidth}x{originalImageHeight}");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - LoadImageDimensionsWindows", ex);
        }
    }
#endif

    /// <summary>
    /// Handles pan gesture for dragging the image
    /// </summary>
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        try
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    // Don't start pan if corner is being dragged
                    if (isCornerDragging)
                    {
                        return;
                    }
                    
                    isPanning = true;
                    isPanProcessing = false;
                    startX = currentX;
                    startY = currentY;
                    targetX = currentX;
                    targetY = currentY;
                    
                    General.LogOfProgram?.Debug($"CropPhotoPage - Pan started at: X={currentX:F1}, Y={currentY:F1}");
                    break;

                case GestureStatus.Running:
                    // Don't continue pan if corner dragging started or already processing
                    if (isCornerDragging || !isPanning || isPanProcessing)
                    {
                        return;
                    }
                    
                    isPanProcessing = true;
                    
                    try
                    {
                        // Calculate target position (where we want to go)
                        targetX = startX + e.TotalX;
                        targetY = startY + e.TotalY;
                        
                        // Apply exponential smoothing for fluid movement
                        // newPosition = currentPosition + smoothingFactor * (target - currentPosition)
                        double newX = currentX + SmoothingFactor * (targetX - currentX);
                        double newY = currentY + SmoothingFactor * (targetY - currentY);
                        
                        // Only update if movement is significant enough (0.5 pixel threshold)
                        double deltaX = Math.Abs(newX - currentX);
                        double deltaY = Math.Abs(newY - currentY);
                        
                        if (deltaX > 0.5 || deltaY > 0.5)
                        {
                            // Update position
                            currentX = newX;
                            currentY = newY;

                            // Apply translation to image
                            imgPhoto.TranslationX = currentX;
                            imgPhoto.TranslationY = currentY;

                            General.LogOfProgram?.Debug($"CropPhotoPage - Pan: X={currentX:F1}, Y={currentY:F1}, Target: X={targetX:F1}, Y={targetY:F1}");
                        }
                    }
                    finally
                    {
                        isPanProcessing = false;
                    }
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    isPanning = false;
                    isPanProcessing = false;
                    
                    // Snap to final target position
                    currentX = targetX;
                    currentY = targetY;
                    imgPhoto.TranslationX = currentX;
                    imgPhoto.TranslationY = currentY;
                    
                    General.LogOfProgram?.Debug($"CropPhotoPage - Pan completed at: X={currentX:F1}, Y={currentY:F1}");
                    break;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - OnPanUpdated", ex);
            isPanProcessing = false;
        }
    }

    /// <summary>
    /// Handles pinch gesture for zooming the crop overlay
    /// </summary>
    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        try
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    startCropSize = currentCropSize;

                    // Show zoom hint
                    if (lblZoomHint != null)
                    {
                        lblZoomHint.IsVisible = true;
                    }
                    break;

                case GestureStatus.Running:
                    // Calcula
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    // Hide zoom hint
                    if (lblZoomHint != null)
                    {
                        lblZoomHint.IsVisible = false;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - OnPinchUpdated", ex);
        }
    }

    /// <summary>
    /// Handles corner drag for resizing the crop overlay (mouse/touch)
    /// </summary>
    private void OnCornerDrag(object sender, PanUpdatedEventArgs e)
    {
        try
        {
            if (sender is not BoxView corner)
                return;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    isCornerDragging = true;
                    isResizeProcessing = false;
                    startCropSize = currentCropSize;
                    targetCropSize = currentCropSize;
                    
                    // Stop any ongoing smoothing
                    cropSmoothingTimer?.Stop();

                    // Show zoom hint
                    if (lblZoomHint != null)
                    {
                        lblZoomHint.IsVisible = true;
                    }

                    General.LogOfProgram?.Debug($"CropPhotoPage - Corner drag started, initial size: {startCropSize}");
                    break;

                case GestureStatus.Running:
                    if (!isCornerDragging)
                    {
                        return;
                    }
                    
                    try
                    {
                        // Improved algorithm for smoother Android experience
                        // Use a sensitivity factor to make movement more responsive
                        const double sensitivityFactor = 1.5; // Adjust this to control sensitivity (1.0 = original, higher = more sensitive)
                        
                        // Calculate drag distance based on dominant direction
                        double dragDistance;
                        
                        // Use the direction with maximum absolute value
                        double absX = Math.Abs(e.TotalX);
                        double absY = Math.Abs(e.TotalY);
                        
                        if (absX > absY)
                        {
                            // Horizontal drag dominant - multiply by sensitivity
                            dragDistance = e.TotalX * sensitivityFactor * 2;
                        }
                        else
                        {
                            // Vertical drag dominant - multiply by sensitivity
                            dragDistance = e.TotalY * sensitivityFactor * 2;
                            
    
                            // For top corners, invert Y direction
                            if (corner == cornerTopLeft || corner == cornerTopRight)
                            {
                                dragDistance = -dragDistance;
                            }
                        }

                        // Calculate target size (where we want to go)
                        targetCropSize = startCropSize + dragDistance;
                        
                        // Clamp target to min/max values
                        targetCropSize = Math.Max(MinCropSize, Math.Min(MaxCropSize, targetCropSize));
                        
                        // Start/restart the smoothing timer for continuous animation
                        if (!cropSmoothingTimer.Enabled)
                        {
                            cropSmoothingTimer.Start();
                        }

                        General.LogOfProgram?.Debug($"CropPhotoPage - Corner drag: TotalX={e.TotalX:F1}, TotalY={e.TotalY:F1}, Target={targetCropSize:F0}, Current={currentCropSize:F0}");
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("CropPhotoPage - OnCornerDrag Running", ex);
                    }
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    isCornerDragging = false;
                    isResizeProcessing = false;
                    
                    // Timer will handle smooth animation to final size
                    // It will auto-stop when close enough to target
                    
                    General.LogOfProgram?.Debug($"CropPhotoPage - Corner drag completed, animating to size: {targetCropSize:F0}");

                    // Hide zoom hint after a delay
                    HideZoomHintAfterDelay();
                    break;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - OnCornerDrag", ex);
            isResizeProcessing = false;
            cropSmoothingTimer?.Stop();
        }
    }

    /// <summary>
    /// Updates the size indicator label
    /// </summary>
    private void UpdateSizeIndicator()
    {
        if (lblSizeIndicator != null)
        {
            int displaySize = (int)currentCropSize;

            // Calculate approximate final image size based on scale
            double containerWidth = imageContainer.Width > 0 ? imageContainer.Width : 400;
            double scale = originalImageWidth > 0 ? originalImageWidth / containerWidth : 1;
            int finalSize = (int)(currentCropSize * scale);

            lblSizeIndicator.Text = $"?? Ritaglio: {displaySize}x{displaySize} px (?{finalSize}x{finalSize} px finale)";
        }
    }

    /// <summary>
    /// Resets position and zoom
    /// </summary>
    private void btnReset_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Reset image position
            currentX = 0;
            currentY = 0;
            targetX = 0;
            targetY = 0;
            imgPhoto.TranslationX = 0;
            imgPhoto.TranslationY = 0;

            // Reset crop size
            currentCropSize = 300;
            targetCropSize = 300;
            cropOverlay.WidthRequest = currentCropSize;
            cropOverlay.HeightRequest = currentCropSize;

            // Update size indicator
            UpdateSizeIndicator();

            General.LogOfProgram?.Event("CropPhotoPage - Reset to default position and size");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - btnReset_Clicked", ex);
        }
    }

    private async void btnConfirm_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Crop the image to square
            croppedPhotoPath = await CropImageToSquare(originalPhotoPath);

            if (!string.IsNullOrEmpty(croppedPhotoPath))
            {
                General.LogOfProgram?.Event($"CropPhotoPage - Photo cropped successfully: {croppedPhotoPath}");
                cropTaskSource.SetResult(croppedPhotoPath);
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Errore", "Impossibile ritagliare la foto", "OK");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - btnConfirm_Clicked", ex);
            await DisplayAlert(AppStrings.Error, string.Format(AppStrings.ErrorDuringCrop, ex.Message), AppStrings.OK);
        }
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        cropTaskSource.SetResult(null);
        await Navigation.PopModalAsync();
    }

    /// <summary>
    /// Crops the image to a square format based on the current pan position and crop size
    /// </summary>
    private async Task<string> CropImageToSquare(string sourcePath)
    {
        try
        {
            // Generate output path
            string directory = Path.GetDirectoryName(sourcePath);
            string fileName = Path.GetFileNameWithoutExtension(sourcePath);
            string extension = Path.GetExtension(sourcePath);
            string outputPath = Path.Combine(directory, $"{fileName}_square{extension}");

            // Use platform-specific image cropping
#if ANDROID
            outputPath = await CropImageAndroid(sourcePath, outputPath);
#elif WINDOWS
   outputPath = await CropImageWindows(sourcePath, outputPath);
#endif

            return outputPath;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - CropImageToSquare", ex);
            return null;
        }
    }

#if ANDROID
    private async Task<string> CropImageAndroid(string sourcePath, string outputPath)
    {
        try
        {
            // Load the original bitmap
            using var bitmap = await Android.Graphics.BitmapFactory.DecodeFileAsync(sourcePath);
            if (bitmap == null) return null;

            // Get the container size (image display area)
            double containerWidth = imageContainer.Width;
            double containerHeight = imageContainer.Height;

            // Calculate the scale factor between displayed image and original bitmap
            double scaleX = bitmap.Width / containerWidth;
            double scaleY = bitmap.Height / containerHeight;
            double scale = Math.Max(scaleX, scaleY); // Use larger scale to ensure coverage

            // Calculate crop size in bitmap coordinates based on current overlay size
            int cropSize = (int)(currentCropSize * scale);

            // Ensure crop size doesn't exceed bitmap dimensions
            cropSize = Math.Min(cropSize, Math.Min(bitmap.Width, bitmap.Height));

            // Calculate center offset based on pan translation
            // The pan translation is in display coordinates, convert to bitmap coordinates
            double offsetX = -currentX * scale;
            double offsetY = -currentY * scale;

            // Calculate crop position (center of displayed area + pan offset)
            int centerX = bitmap.Width / 2;
            int centerY = bitmap.Height / 2;

            int cropX = (int)(centerX - (cropSize / 2) + offsetX);
            int cropY = (int)(centerY - (cropSize / 2) + offsetY);

            // Clamp crop position to valid range
            cropX = Math.Max(0, Math.Min(cropX, bitmap.Width - cropSize));
            cropY = Math.Max(0, Math.Min(cropY, bitmap.Height - cropSize));

            General.LogOfProgram?.Debug($"CropImageAndroid - Crop params: size={cropSize}, pos=({cropX},{cropY}), offset=({offsetX:F1},{offsetY:F1}), scale={scale:F2}");

            // Create square bitmap
            using var croppedBitmap = Android.Graphics.Bitmap.CreateBitmap(bitmap, cropX, cropY, cropSize, cropSize);

            // Save to file
            using var stream = new FileStream(outputPath, FileMode.Create);
            await croppedBitmap.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, stream);

            General.LogOfProgram?.Debug($"CropImageAndroid - Cropped to {cropSize}x{cropSize} at ({cropX},{cropY})");
            return outputPath;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - CropImageAndroid", ex);
            return null;
        }
    }
#endif

#if WINDOWS
    private async Task<string> CropImageWindows(string sourcePath, string outputPath)
 {
        try
        {
      // Windows implementation using System.Drawing
            await Task.Run(() =>
  {
         using (var originalImage = System.Drawing.Image.FromFile(sourcePath))
 {
     // Get the container size (image display area)
double containerWidth = imageContainer.Width > 0 ? imageContainer.Width : 400;
       double containerHeight = imageContainer.Height > 0 ? imageContainer.Height : 600;

       // Calculate the scale factor between displayed image and original bitmap
double scaleX = originalImage.Width / containerWidth;
         double scaleY = originalImage.Height / containerHeight;
    double scale = Math.Max(scaleX, scaleY); // Use larger scale to ensure coverage

        // Calculate crop size in bitmap coordinates based on current overlay size
  int cropSize = (int)(currentCropSize * scale);

  // Ensure crop size doesn't exceed bitmap dimensions
     cropSize = Math.Min(cropSize, Math.Min(originalImage.Width, originalImage.Height));

 // Calculate center offset based on pan translation
   // The pan translation is in display coordinates, convert to bitmap coordinates
         double offsetX = -currentX * scale;
        double offsetY = -currentY * scale;

         // Calculate crop position (center of displayed area + pan offset)
       int centerX = originalImage.Width / 2;
       int centerY = originalImage.Height / 2;

       int cropX = (int)(centerX - (cropSize / 2) + offsetX);
          int cropY = (int)(centerY - (cropSize / 2) + offsetY);

       // Clamp crop position to valid range
  cropX = Math.Max(0, Math.Min(cropX, originalImage.Width - cropSize));
            cropY = Math.Max(0, Math.Min(cropY, originalImage.Height - cropSize));

           General.LogOfProgram?.Debug($"CropImageWindows - Crop params: size={cropSize}, pos=({cropX},{cropY}), offset=({offsetX:F1},{offsetY:F1}), scale={scale:F2}");

                 // Create cropped bitmap
 using (var croppedBitmap = new System.Drawing.Bitmap(cropSize, cropSize))
{
          using (var graphics = System.Drawing.Graphics.FromImage(croppedBitmap))
    {
          // Set high quality rendering
       graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
      graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
      graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // Draw the cropped portion
           graphics.DrawImage(originalImage,
  new System.Drawing.Rectangle(0, 0, cropSize, cropSize),
       new System.Drawing.Rectangle(cropX, cropY, cropSize, cropSize),
  System.Drawing.GraphicsUnit.Pixel);
 }

        // Save to file with JPEG quality 90%
         var jpegEncoder = GetJpegEncoder();
                   var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
        encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(
       System.Drawing.Imaging.Encoder.Quality, 90L);

              croppedBitmap.Save(outputPath, jpegEncoder, encoderParameters);
        }

        General.LogOfProgram?.Debug($"CropImageWindows - Cropped to {cropSize}x{cropSize} at ({cropX},{cropY})");
         }
       });

            return outputPath;
      }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - CropImageWindows", ex);
            return null;
        }
    }

    /// <summary>
    /// Gets the JPEG image encoder
    /// </summary>
    private System.Drawing.Imaging.ImageCodecInfo GetJpegEncoder()
    {
   var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
        foreach (var codec in codecs)
        {
if (codec.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
  {
     return codec;
            }
        }
        return null;
    }
#endif

    /// <summary>
    /// Timer callback for continuous crop size smoothing
    /// </summary>
    private void CropSmoothingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            // Check if we need to continue smoothing
            double delta = Math.Abs(targetCropSize - currentCropSize);
            
            if (delta < 0.5) // Close enough to target
            {
                // Snap to final value and stop timer
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    currentCropSize = targetCropSize;
                    cropOverlay.WidthRequest = currentCropSize;
                    cropOverlay.HeightRequest = currentCropSize;
                    UpdateSizeIndicator();
                });
                
                cropSmoothingTimer?.Stop();
                return;
            }
            
            // Apply exponential smoothing
            double newSize = currentCropSize + CropSmoothingFactor * (targetCropSize - currentCropSize);
            
            // Update UI on main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                currentCropSize = newSize;
                cropOverlay.WidthRequest = currentCropSize;
                cropOverlay.HeightRequest = currentCropSize;
                UpdateSizeIndicator();
            });
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - CropSmoothingTimer_Elapsed", ex);
            cropSmoothingTimer?.Stop();
        }
    }
}
