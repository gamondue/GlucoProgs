using gamon;

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

    // Crop overlay size tracking for pinch gesture
    private double currentCropSize = 300;
    private double startCropSize = 300;
    private const double MinCropSize = 100;
    private const double MaxCropSize = 600;

    // Original image dimensions
    private int originalImageWidth;
    private int originalImageHeight;

    public CropPhotoPage(string photoPath)
    {
        InitializeComponent();
        originalPhotoPath = photoPath;

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
                    startX = currentX;
                    startY = currentY;
                    break;

                case GestureStatus.Running:
                    // Update position
                    currentX = startX + e.TotalX;
                    currentY = startY + e.TotalY;

                    // Apply translation to image
                    imgPhoto.TranslationX = currentX;
                    imgPhoto.TranslationY = currentY;

                    General.LogOfProgram?.Debug($"CropPhotoPage - Pan: X={currentX:F1}, Y={currentY:F1}");
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    // Keep the current position
                    General.LogOfProgram?.Debug($"CropPhotoPage - Pan completed at: X={currentX:F1}, Y={currentY:F1}");
                    break;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - OnPanUpdated", ex);
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
                    startCropSize = currentCropSize;

                    // Show zoom hint
                    if (lblZoomHint != null)
                    {
                        lblZoomHint.IsVisible = true;
                    }

                    General.LogOfProgram?.Debug($"CropPhotoPage - Corner drag started, initial size: {startCropSize}");
                    break;

                case GestureStatus.Running:
                    // Simplified algorithm: use the maximum of X or Y drag distance
                    // Multiply by 2 because we're dragging from center to corner
                    double dragDistance;

                    // Determine which direction has more movement
                    if (Math.Abs(e.TotalX) > Math.Abs(e.TotalY))
                    {
                        dragDistance = e.TotalX * 2;
                    }
                    else
                    {
                        dragDistance = e.TotalY * 2;
                    }

                    // For top-left and top-right corners, invert Y direction
                    if (corner == cornerTopLeft || corner == cornerTopRight)
                    {
                        if (Math.Abs(e.TotalY) > Math.Abs(e.TotalX))
                        {
                            dragDistance = -e.TotalY * 2;
                        }
                    }

                    // Calculate new size
                    double newSize = startCropSize + dragDistance;

                    // Clamp to min/max values
                    newSize = Math.Max(MinCropSize, Math.Min(MaxCropSize, newSize));

                    // Apply new size to crop overlay
                    currentCropSize = newSize;
                    cropOverlay.WidthRequest = currentCropSize;
                    cropOverlay.HeightRequest = currentCropSize;

                    // Update size indicator
                    UpdateSizeIndicator();

                    General.LogOfProgram?.Debug($"CropPhotoPage - Corner drag: TotalX={e.TotalX:F1}, TotalY={e.TotalY:F1}, Distance={dragDistance:F1}, Size={currentCropSize:F0}");
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    General.LogOfProgram?.Debug($"CropPhotoPage - Corner drag completed at size: {currentCropSize:F0}");

                    // Hide zoom hint after a delay
                    HideZoomHintAfterDelay();
                    break;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CropPhotoPage - OnCornerDrag", ex);
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
            imgPhoto.TranslationX = 0;
            imgPhoto.TranslationY = 0;

            // Reset crop size
            currentCropSize = 300;
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
            await DisplayAlert("Errore", $"Errore durante il ritaglio: {ex.Message}", "OK");
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
}
