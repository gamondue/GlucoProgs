using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.Maui.Models;

namespace GlucoMan.Maui;

public partial class ContainersPage : ContentPage
{
    private BL_Containers bl = new BL_Containers();
    private List<Container> allContainers = new();
    private List<ContainerViewModel> containerViewModels = new();
    private Container selectedContainer;

    // Properties to handle data exchange with calling page
    public Container SelectedContainer { get; private set; }
    public bool ContainerWasSelected { get; private set; } = false;
    // TaskCompletionSource for modal behavior
    private TaskCompletionSource<bool> pageClosedTaskSource = new TaskCompletionSource<bool>();
    public Task<bool> PageClosedTask => pageClosedTaskSource.Task;
    // Default constructor
    public ContainersPage()
    {
        try
        {
            InitializeComponent();
            LoadContainers();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - Constructor", ex);
        }
    }
    // Constructor with pre-selected container weight
    public ContainersPage(double? currentWeight) : this()
    {
        try
        {
            if (currentWeight.HasValue && currentWeight.Value > 0)
            {
                txtContainerWeight.Text = currentWeight.Value.ToString();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - Constructor with weight", ex);
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
                pageClosedTaskSource.SetResult(ContainerWasSelected);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - OnDisappearing", ex);
            // Ensure task is completed even if there's an error
            if (!pageClosedTaskSource.Task.IsCompleted)
            {
                pageClosedTaskSource.SetResult(false);
            }
        }
    }
    private void LoadContainers()
    {
        try
        {
            // Load containers from database
            allContainers = bl.GetAllContainers();

            // Create ViewModels for UI binding with thumbnails
            containerViewModels = allContainers
                .Select(c => new ContainerViewModel(c))
                .ToList();

            lvContainers.ItemsSource = null;
            lvContainers.ItemsSource = containerViewModels;

            General.LogOfProgram?.Event($"ContainersPage - Loaded {allContainers.Count} containers");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - LoadContainers", ex);
            DisplayAlert("Error", "Failed to load containers. Check logs for details.", "OK");
        }
    }
    private void LoadContainerPhoto(Container container)
    {
        try
        {
            if (container == null || string.IsNullOrWhiteSpace(container.PhotoFileName))
            {
                imgContainerPhoto.Source = null;
                return;
            }

            string photoPath = container.GetPhotoFullPath();

            if (!string.IsNullOrWhiteSpace(photoPath) && File.Exists(photoPath))
            {
                imgContainerPhoto.Source = ImageSource.FromFile(photoPath);
                General.LogOfProgram?.Event($"ContainersPage - Photo loaded: {container.PhotoFileName}");
            }
            else
            {
                imgContainerPhoto.Source = null;
                General.LogOfProgram?.Event($"ContainersPage - Photo file not found: {photoPath}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - LoadContainerPhoto", ex);
            imgContainerPhoto.Source = null;
        }
    }
    private void lvContainers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem != null)
            {
                var selectedViewModel = e.SelectedItem as ContainerViewModel;

                if (selectedViewModel != null)
                {
                    selectedContainer = selectedViewModel.Container;

                    // Update entry fields with selected container data
                    txtContainerName.Text = selectedContainer.Name;
                    txtContainerWeight.Text = selectedContainer.Weight?.Text ?? "";

                    // Load container photo
                    LoadContainerPhoto(selectedContainer);

                    General.LogOfProgram?.Event($"ContainersPage - Container selected: {selectedContainer.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - lvContainers_ItemSelected", ex);
        }
    }
    private async void btnNew_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtContainerName.Text))
            {
                await DisplayAlert("Validation Error", "Please enter a container name.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContainerWeight.Text))
            {
                await DisplayAlert("Validation Error", "Please enter a container weight.", "OK");
                return;
            }

            // ALWAYS create a new container (button "+" should add, not update)
            // If user wants to update, they should select from list and use "Save" button
            Container containerToSave = new Container
            {
                Name = txtContainerName.Text.Trim(),
                Weight = new DoubleAndText { Text = txtContainerWeight.Text.Trim() },
                PhotoFileName = selectedContainer?.PhotoFileName ?? ""
            };

            // Parse and set Double value
            if (double.TryParse(txtContainerWeight.Text.Trim(), out double weightValue))
            {
                containerToSave.Weight.Double = weightValue;
            }
            // Save to database (will create new record since IdContainer is null)
            int? result = bl.SaveContainer(containerToSave);

            if (result.HasValue && result.Value > 0)
            {
                await DisplayAlert("Success", "New container added successfully!", "OK");

                // Clear entry fields and selection
                txtContainerName.Text = "";
                txtContainerWeight.Text = "";
                imgContainerPhoto.Source = null;
                selectedContainer = null;
                lvContainers.SelectedItem = null;

                // Reload list
                LoadContainers();

                General.LogOfProgram?.Event($"ContainersPage - New container added: {containerToSave.Name}");
            }
            else
            {
                await DisplayAlert("Error", "Failed to add container. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnNew_Click", ex);
            await DisplayAlert("Error", "An error occurred while adding container. Please try again.", "OK");
        }
    }
    private async void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtContainerName.Text))
            {
                await DisplayAlert("Validation Error", "Please enter a container name.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContainerWeight.Text))
            {
                await DisplayAlert("Validation Error", "Please enter a container weight.", "OK");
                return;
            }

            // Check if a container is selected for update
            if (selectedContainer == null || !selectedContainer.IdContainer.HasValue)
            {
                await DisplayAlert("No Selection",
                   "Please select a container from the list to update it, or use the '+' button to create a new one.",
             "OK");
                return;
            }

            // Preserve the existing PhotoFileName before updating
            string existingPhotoFileName = selectedContainer.PhotoFileName;

            // Update the selected container with UI values
            selectedContainer.Name = txtContainerName.Text.Trim();
            
            // Preserve or create Weight object without losing PhotoFileName
            if (selectedContainer.Weight == null)
          {
             selectedContainer.Weight = new DoubleAndText();
               }
            
            selectedContainer.Weight.Text = txtContainerWeight.Text.Trim();

   // Parse and set Double value
    if (double.TryParse(txtContainerWeight.Text.Trim(), out double weightValue))
            {
       selectedContainer.Weight.Double = weightValue;
  }
            
            // Restore the PhotoFileName (it's stored at Container level, not Weight level)
selectedContainer.PhotoFileName = existingPhotoFileName;

            // Save to database (will update existing record since IdContainer has a value)
     int? result = bl.SaveContainer(selectedContainer);

   if (result.HasValue && result.Value > 0)
            {
         await DisplayAlert("Success", "Container updated successfully!", "OK");

        // Reload list to show updated data
                LoadContainers();

    General.LogOfProgram?.Event($"ContainersPage - Container updated: {selectedContainer.Name} (ID: {selectedContainer.IdContainer}), Photo: {selectedContainer.PhotoFileName ?? "none"}");
      }
            else
       {
       await DisplayAlert("Error", "Failed to update container. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnSave_Click", ex);
            await DisplayAlert("Error", "An error occurred while updating container. Please try again.", "OK");
        }
    }
    private async void btnChoose_Click(object sender, EventArgs e)
    {
        try
        {
            // Validate that we have weight data
            if (string.IsNullOrWhiteSpace(txtContainerWeight.Text))
            {
                await DisplayAlert("Validation Error", "Please enter or select a container weight.", "OK");
                return;
            }

            // Create container to return with current UI values
            Container containerToReturn;

            if (selectedContainer != null && selectedContainer.IdContainer.HasValue)
            {
                // Use the selected container from database
                containerToReturn = selectedContainer;
            }
            else
            {
                // Create a temporary container with UI values
                containerToReturn = new Container();
            }

            // Always update with current UI values
            containerToReturn.Name = txtContainerName.Text?.Trim() ?? "";

            // Ensure Weight object exists
            if (containerToReturn.Weight == null)
            {
                containerToReturn.Weight = new DoubleAndText();
            }

            // Set weight from UI
            containerToReturn.Weight.Text = txtContainerWeight.Text.Trim();

            // Parse and set Double value
            if (double.TryParse(txtContainerWeight.Text.Trim(), out double weightValue))
            {
                containerToReturn.Weight.Double = weightValue;
            }
            else
            {
                await DisplayAlert("Validation Error", "Please enter a valid numeric weight.", "OK");
                return;
            }

            // Set result
            SelectedContainer = containerToReturn;
            ContainerWasSelected = true;

            General.LogOfProgram?.Event($"ContainersPage - Container chosen: Name='{SelectedContainer.Name}', Weight={SelectedContainer.Weight?.Text ?? "N/A"}");

            // Close the page
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnChoose_Click", ex);
            await DisplayAlert("Error", $"An error occurred: {ex.Message}\nPlease try again.", "OK");
        }
    }
    private async void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            ContainerWasSelected = false;
            SelectedContainer = null;

            General.LogOfProgram?.Event("ContainersPage - User clicked Back button");

            // Close the page
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnBack_Click", ex);
            await DisplayAlert("Error", "An error occurred while going back. Please try again.", "OK");
        }
    }
    private void btnClearFields_Click(object sender, EventArgs e)
    {
        try
        {
            // Clear all entry fields
            txtContainerName.Text = "";
            txtContainerWeight.Text = "";
            imgContainerPhoto.Source = null;

            // Clear selection
            selectedContainer = null;
            lvContainers.SelectedItem = null;

            General.LogOfProgram?.Event("ContainersPage - Fields cleared");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnClearFields_Click", ex);
        }
    }
    private async void btnTakePicture_Click(object sender, EventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("ContainersPage - Take photo button clicked");

            // Check and request camera permission
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Required",
                        "Camera access is required to take photos. Please enable it in your device settings.",
                        "OK");
                General.LogOfProgram?.Event("ContainersPage - Camera permission denied");
                return;
            }

            General.LogOfProgram?.Event("ContainersPage - Launching camera");

            // Use MediaPicker to capture photo directly
            var photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Container Photo"
            });

            if (photo != null)
            {
                // Save to temporary location first
                string tempFolder = Path.Combine(FileSystem.CacheDirectory, "TempPhotos");
                Directory.CreateDirectory(tempFolder);

                string tempFileName = $"temp_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                string tempPath = Path.Combine(tempFolder, tempFileName);

                // Copy photo to temp location
                using (var sourceStream = await photo.OpenReadAsync())
                using (var targetStream = File.Create(tempPath))
                {
                    await sourceStream.CopyToAsync(targetStream);
                }

                General.LogOfProgram?.Event($"ContainersPage - Photo saved to temp: {tempPath}");

                // Open crop page
                var cropPage = new CropPhotoPage(tempPath);
                await Navigation.PushModalAsync(cropPage);

                // Wait for crop result
                string croppedPhotoPath = await cropPage.CropTask;

                if (!string.IsNullOrEmpty(croppedPhotoPath))
                {
                    // Move cropped photo to final location
                    string containersFolder = Path.Combine(FileSystem.AppDataDirectory, "ContainerPhotos");
                    Directory.CreateDirectory(containersFolder);

                    // Generate unique filename based on container name or default and timestamp
                    string baseName;
                    if (string.IsNullOrWhiteSpace(txtContainerName?.Text))
                    {
                        baseName = "container";
                    }
                    else
                    {
                        baseName = MakeValidFileName(txtContainerName.Text.Trim());
                    }

                    string fileName = $"{baseName}_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                    string targetPath = Path.Combine(containersFolder, fileName);

                    // Copy cropped photo to final location
                    File.Copy(croppedPhotoPath, targetPath, true);

                    // Clean up temp files
                    try
                    {
                        File.Delete(tempPath);
                        File.Delete(croppedPhotoPath);
                    }
                    catch { } // Ignore cleanup errors

                    // Create or update selected container with photo filename
                    if (selectedContainer == null)
                    {
                        selectedContainer = new Container();
                    }

                    selectedContainer.PhotoFileName = fileName;

                    // Save the container photo filename when container exists in DB
                    if (selectedContainer.IdContainer.HasValue)
                    {
                        int? result = bl.SaveContainer(selectedContainer);
                        if (result.HasValue && result.Value >0)
                        {
                            General.LogOfProgram?.Event($"ContainersPage - Photo filename saved to database for container ID: {selectedContainer.IdContainer}");
                        }
                        else
                        {
                            General.LogOfProgram?.Error("ContainersPage - Failed to save photo filename to database", null);
                        }
                    }

                    // Load and display the captured photo
                     LoadContainerPhoto(selectedContainer);

                    General.LogOfProgram?.Event($"ContainersPage - Photo captured, cropped and saved: {fileName}");
                    await DisplayAlert("Success", "Foto quadrata acquisita con successo!", "OK");
                }
                else
                {
                    General.LogOfProgram?.Event("ContainersPage - Photo crop cancelled by user");
                   // Clean up temp file
                    try { File.Delete(tempPath); } catch { }
                }
            }
            else
            {
                General.LogOfProgram?.Event("ContainersPage - Photo capture cancelled by user");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnTakePicture_Click", ex);
            await DisplayAlert("Error", "Failed to take picture. Please try again.", "OK");
        }
    }

    // Helper: sanitize filename by removing/replacing invalid filename characters
    private string MakeValidFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "container";

        // Replace invalid filename chars with underscore
        var invalid = Path.GetInvalidFileNameChars();
        var sb = new System.Text.StringBuilder();
        foreach (var c in name)
        {
            if (invalid.Contains(c))
                sb.Append('_');
            else
                sb.Append(c);
        }

        var result = sb.ToString();
        // Replace whitespace sequences with single underscore
        result = System.Text.RegularExpressions.Regex.Replace(result, "\\s+", "_");
        // Collapse multiple underscores
        result = System.Text.RegularExpressions.Regex.Replace(result, "_+", "_");
        // Trim underscores and dots from ends
        result = result.Trim('_', '.');

        if (string.IsNullOrWhiteSpace(result))
            return "container";

        return result;
    }
    private async void imgContainerPhoto_Tapped(object sender, EventArgs e)
    {
        try
        {
            // Allow user to view full-size photo or take new photo
            if (selectedContainer != null && !string.IsNullOrWhiteSpace(selectedContainer.PhotoFileName))
            {
                string photoPath = selectedContainer.GetPhotoFullPath();
                if (File.Exists(photoPath))
                {
                    // Open full screen page
                    await Navigation.PushModalAsync(new FullScreenImagePage(photoPath));
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - imgContainerPhoto_Tapped", ex);
        }
    }
    private async void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            if (button?.CommandParameter is Container containerToDelete)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    $"Are you sure you want to delete '{containerToDelete.Name}'?",
                    "Yes", "No");

                if (confirm)
                {
                    bl.DeleteContainer(containerToDelete);

                    await DisplayAlert("Success", "Container deleted successfully!", "OK");

                    // Reload list
                    LoadContainers();

                    // Clear selection if deleted item was selected
                    if (selectedContainer?.IdContainer == containerToDelete.IdContainer)
                    {
                        selectedContainer = null;
                        txtContainerName.Text = "";
                        txtContainerWeight.Text = "";
                        imgContainerPhoto.Source = null;
                    }

                    General.LogOfProgram?.Event($"ContainersPage - Container deleted: {containerToDelete.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnDelete_Click", ex);
            await DisplayAlert("Error", "An error occurred while deleting. Please try again.", "OK");
        }
    }
    private async void btnMinus_Click(object sender, EventArgs e)
    {
        try
        {
            // Check if a container is selected
            if (selectedContainer == null || !selectedContainer.IdContainer.HasValue)
            {
                await DisplayAlert("No Selection", "Please select a container from the list first.", "OK");
                return;
            }

            // Ask for confirmation before deleting
            bool confirm = await DisplayAlert(
        "Confirm Delete",
          $"Are you sure you want to delete '{selectedContainer.Name}'?",
         "Yes",
             "No");

            if (confirm)
            {
                // Delete the selected container
                bl.DeleteContainer(selectedContainer);

                await DisplayAlert("Success", "Container deleted successfully!", "OK");

                // Clear fields and photo
                txtContainerName.Text = "";
                txtContainerWeight.Text = "";
                imgContainerPhoto.Source = null;
                selectedContainer = null;
                lvContainers.SelectedItem = null;

                // Reload list
                LoadContainers();

                General.LogOfProgram?.Event($"ContainersPage - Minus: Container deleted");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnMinus_Click", ex);
            await DisplayAlert("Error", "An error occurred while deleting. Please try again.", "OK");
        }
    }

    private void imgContainerPhoto_Tapped(object sender, TappedEventArgs e)
    {

    }
}
