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
 // TEMPORANEAMENTE DISABILITATO PER DEBUG
      allContainers = new List<Container>(); // Lista vuota
containerViewModels = new List<ContainerViewModel>();
     
       lvContainers.ItemsSource = null;
       lvContainers.ItemsSource = containerViewModels;
    
      General.LogOfProgram?.Event($"ContainersPage - DEBUG: Containers loading disabled");
   return;
        
        // CODICE ORIGINALE COMMENTATO
       /*
      allContainers = bl.GetAllContainers();
       
   // Create ViewModels for UI binding with thumbnails
     containerViewModels = allContainers
   .Select(c => new ContainerViewModel(c))
 .ToList();
    
       lvContainers.ItemsSource = null;
       lvContainers.ItemsSource = containerViewModels;
    
 General.LogOfProgram?.Event($"ContainersPage - Loaded {allContainers.Count} containers");
     */
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
            General.LogOfProgram?.Event("ContainersPage - Opening TakePicturePage");
            
            // Open TakePicturePage
            var takePicturePage = new TakePicturePage();
            await Navigation.PushModalAsync(takePicturePage);
            
            // Wait for the page to be closed and get the result
            bool photoWasTaken = await takePicturePage.PageClosedTask;
            
            // Check if photo was captured
            if (photoWasTaken && !string.IsNullOrWhiteSpace(takePicturePage.CapturedPhotoPath))
            {
                // Create or update selected container with photo filename
                if (selectedContainer == null)
                {
                    selectedContainer = new Container();
                }
                
                selectedContainer.PhotoFileName = takePicturePage.CapturedPhotoPath;
                
                // Load and display the captured photo
                LoadContainerPhoto(selectedContainer);
                
                General.LogOfProgram?.Event($"ContainersPage - Photo set for container: {takePicturePage.CapturedPhotoPath}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ContainersPage - btnTakePicture_Click", ex);
            await DisplayAlert("Error", "Failed to take picture. Please try again.", "OK");
        }
    }
    
    private async void imgContainerPhoto_Tapped(object sender, EventArgs e)
    {
        try
        {
            // Allow user to view full-size photo or take new photo
            string action = await DisplayActionSheet(
                "Container Photo", 
                "Cancel", 
                null, 
                "Take New Photo", 
                "View Full Size");
            
            if (action == "Take New Photo")
            {
                btnTakePicture_Click(sender, e);
            }
            else if (action == "View Full Size")
            {
                if (selectedContainer != null && !string.IsNullOrWhiteSpace(selectedContainer.PhotoFileName))
                {
                    string photoPath = selectedContainer.GetPhotoFullPath();
                    if (File.Exists(photoPath))
                    {
                        // Could open a full-screen image viewer here
                        await DisplayAlert("Photo", $"Photo: {selectedContainer.PhotoFileName}", "OK");
                    }
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
}
