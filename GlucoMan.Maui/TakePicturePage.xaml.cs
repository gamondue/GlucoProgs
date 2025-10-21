using gamon;

namespace GlucoMan.Maui;

public partial class TakePicturePage : ContentPage
{
    // Properties to handle photo result
    public string CapturedPhotoPath { get; private set; }
    public bool PhotoWasCaptured { get; private set; } = false;
    
    // TaskCompletionSource for modal behavior
    private TaskCompletionSource<bool> pageClosedTaskSource = new TaskCompletionSource<bool>();
    public Task<bool> PageClosedTask => pageClosedTaskSource.Task;
    
    private bool isCameraInitialized = false;
    
    public TakePicturePage()
    {
        try
        {
            InitializeComponent();
            InitializeCameraAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - Constructor", ex);
        }
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        try
        {
            if (!isCameraInitialized)
            {
                InitializeCameraAsync();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - OnAppearing", ex);
        }
    }
    
    protected override void OnDisappearing()
    {
        try
        {
            base.OnDisappearing();
            
            // Complete the task when the page is closed
            if (!pageClosedTaskSource.Task.IsCompleted)
            {
                pageClosedTaskSource.SetResult(PhotoWasCaptured);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - OnDisappearing", ex);
            // Ensure task is completed even if there's an error
            if (!pageClosedTaskSource.Task.IsCompleted)
            {
                pageClosedTaskSource.SetResult(false);
            }
        }
    }
    
    private async void InitializeCameraAsync()
    {
        try
        {
            lblCameraStatus.Text = "Requesting camera permission...";
            
            // Check and request camera permission
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }
            
            if (status != PermissionStatus.Granted)
            {
                lblCameraStatus.Text = "Camera permission denied.\nPlease enable camera access in settings.";
                await DisplayAlert("Permission Required", 
                    "Camera access is required to take photos. Please enable it in your device settings.", 
                    "OK");
                return;
            }
            
            lblCameraStatus.Text = "Camera ready!";
            isCameraInitialized = true;
            
            General.LogOfProgram?.Event("TakePicturePage - Camera initialized successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - InitializeCameraAsync", ex);
            lblCameraStatus.Text = "Error initializing camera.\nPlease try again.";
            await DisplayAlert("Error", "Failed to initialize camera. Please try again.", "OK");
        }
    }
    
    private async void btnCapture_Tapped(object sender, EventArgs e)
    {
        try
        {
            if (!isCameraInitialized)
            {
                await DisplayAlert("Camera Not Ready", "Please wait for camera initialization.", "OK");
                return;
            }
            
            General.LogOfProgram?.Event("TakePicturePage - Capture button tapped");
            
            // Use MediaPicker to capture photo
            var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Container Photo"
            });
            
            if (photo != null)
            {
                // Create ContainerPhotos folder if it doesn't exist
                string containersFolder = Path.Combine(FileSystem.AppDataDirectory, "ContainerPhotos");
                Directory.CreateDirectory(containersFolder);
                
                // Generate unique filename
                string fileName = $"container_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                string targetPath = Path.Combine(containersFolder, fileName);
                
                // Copy the photo to our app folder
                using (var sourceStream = await photo.OpenReadAsync())
                using (var targetStream = File.Create(targetPath))
                {
                    await sourceStream.CopyToAsync(targetStream);
                }
                
                // Set result properties
                CapturedPhotoPath = fileName; // Store only filename, not full path
                PhotoWasCaptured = true;
                
                General.LogOfProgram?.Event($"TakePicturePage - Photo captured and saved: {fileName}");
                
                await DisplayAlert("Success", "Photo captured successfully!", "OK");
                
                // Close the page
                await Navigation.PopModalAsync();
            }
            else
            {
                General.LogOfProgram?.Event("TakePicturePage - Photo capture cancelled by user");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - btnCapture_Tapped", ex);
            await DisplayAlert("Error", $"Failed to capture photo: {ex.Message}", "OK");
        }
    }
    
    private async void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            PhotoWasCaptured = false;
            CapturedPhotoPath = null;
            
            General.LogOfProgram?.Event("TakePicturePage - User cancelled photo capture");
            
            // Close the page
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - btnCancel_Click", ex);
        }
    }
    
    private async void btnSwitchCamera_Click(object sender, EventArgs e)
    {
        try
        {
            // Camera switching functionality
            // Note: MAUI's MediaPicker doesn't provide direct camera switching
            // This is a placeholder for future enhancement
            await DisplayAlert("Info", "Camera switching feature coming soon!", "OK");
            
            General.LogOfProgram?.Event("TakePicturePage - Switch camera requested");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TakePicturePage - btnSwitchCamera_Click", ex);
        }
    }
}
