 using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using System.Collections.ObjectModel;

namespace GlucoMan.Maui;

public partial class FatSecretSearchPage : ContentPage
{
    private readonly FatSecretService _fatSecretService;
    private ObservableCollection<FatSecretFood> _searchResults;
    private TaskCompletionSource<bool> _taskCompletionSource;
    private FatSecretFood _selectedFood;

    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);
    public FatSecretFood SelectedFood { get; private set; }

    public FatSecretSearchPage(string initialSearchQuery = "")
    {
        InitializeComponent();

        _fatSecretService = new FatSecretService();
        _searchResults = new ObservableCollection<FatSecretFood>();
        _taskCompletionSource = new TaskCompletionSource<bool>();

        txtSearchQuery.Text = initialSearchQuery;
        listResults.ItemsSource = _searchResults;

        CheckCredentials();

        if (!string.IsNullOrWhiteSpace(initialSearchQuery))
        {
            _ = PerformSearchAsync();
        }
    }

    private void CheckCredentials()
    {
        // Pre-fill all fields with current saved values from DB
        PreFillCredentialFields();
        UpdateAuthStatus();
    }

    private void PreFillCredentialFields()
    {
        txtLanguage.Text = _fatSecretService.Language;
        txtRegion.Text = _fatSecretService.Region;
        // Load current credential values (masked by IsPassword=true)
        var bl = new BL_General();
        txtClientId.Text = bl.RestoreParameter("FatSecret_ClientId") ?? "";
        txtClientSecret.Text = bl.RestoreParameter("FatSecret_ClientSecret") ?? "";
        txtConsumerSecret.Text = bl.RestoreParameter("FatSecret_ConsumerSecret") ?? "";
    }

    private void UpdateAuthStatus()
    {
        if (_fatSecretService.HasUserToken)
        {
            lblAuthStatus.Text = "Logged in with FatSecret (OAuth 1.0)";
            lblAuthStatus.TextColor = Colors.Green;
            btnLogin.Text = "Logout";
        }
        else
        {
            if (!_fatSecretService.HasCredentials)
                lblAuthStatus.Text = "Not configured";
            else if (!_fatSecretService.HasConsumerSecret)
                lblAuthStatus.Text = "Using OAuth 2.0 (add Consumer Secret for Login)";
            else
                lblAuthStatus.Text = "Ready (click Login)";
            lblAuthStatus.TextColor = Colors.Gray;
            btnLogin.Text = "Login with FatSecret";
        }
    }

    private void btnShowCredentials_Clicked(object sender, EventArgs e)
    {
        frameCredentials.IsVisible = !frameCredentials.IsVisible;
    }

    private void btnToggleSecret_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            Entry target = null;
            if (btn == btnToggleClientSecret)
                target = txtClientSecret;
            else if (btn == btnToggleConsumerSecret)
                target = txtConsumerSecret;

            if (target != null)
            {
                target.IsPassword = !target.IsPassword;
                btn.Text = target.IsPassword ? "Show" : "Hide";
            }
        }
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        if (_fatSecretService.HasUserToken)
        {
            // Logout
            bool confirm = await DisplayAlert("Logout",
                "Do you want to disconnect your FatSecret account?", "Yes", "No");
            if (confirm)
            {
                _fatSecretService.ClearUserToken();
                UpdateAuthStatus();
                lblStatus.Text = "Logged out from FatSecret";
            }
            return;
        }

        if (!_fatSecretService.HasCredentials || !_fatSecretService.HasConsumerSecret)
        {
            lblStatus.Text = "Please configure API credentials first (Client ID, Secret, and Shared Secret)";
            frameCredentials.IsVisible = true;
            return;
        }

        try
        {
            var authPage = new FatSecretAuthPage(_fatSecretService);
            await Navigation.PushModalAsync(new NavigationPage(authPage));

            bool authorized = await authPage.PageClosedTask;
            if (authorized)
            {
                lblStatus.Text = "Login successful!";
            }
            UpdateAuthStatus();
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Login error: {ex.Message}";
            General.LogOfProgram?.Error("FatSecretSearchPage | btnLogin_Clicked", ex);
        }
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
        await PerformSearchAsync();
    }

    private async void txtSearchQuery_Completed(object sender, EventArgs e)
    {
        await PerformSearchAsync();
    }

    private async Task PerformSearchAsync()
    {
        var query = txtSearchQuery.Text?.Trim();
        if (string.IsNullOrEmpty(query))
        {
            lblStatus.Text = "Please enter a search term";
            return;
        }

        if (!_fatSecretService.HasCredentials)
        {
            lblStatus.Text = "Please configure FatSecret API credentials first";
            frameCredentials.IsVisible = true;
            return;
        }

        try
        {
            lblStatus.Text = "Searching...";
            btnSearch.IsEnabled = false;
            _searchResults.Clear();

            var results = await _fatSecretService.SearchFoodsAsync(query);

            foreach (var food in results)
            {
                _searchResults.Add(food);
            }

            lblStatus.Text = $"Found {results.Count} results";
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            General.LogOfProgram.Error("FatSecretSearchPage | PerformSearchAsync", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
        finally
        {
            btnSearch.IsEnabled = true;
        }
    }

    private void listResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedFood = e.CurrentSelection.FirstOrDefault() as FatSecretFood;
        btnSelect.IsEnabled = _selectedFood != null;
    }

    private async void btnSelect_Clicked(object sender, EventArgs e)
    {
        var selected = _selectedFood ?? listResults.SelectedItem as FatSecretFood;
        if (selected == null)
        {
            await DisplayAlert("", "Please select a food from the list", AppStrings.OK);
            return;
        }

        try
        {
            lblStatus.Text = "Loading food details...";

            var detailedFood = await _fatSecretService.GetFoodDetailsAsync(selected.FoodId);
            SelectedFood = detailedFood ?? selected;

            _taskCompletionSource?.TrySetResult(true);
            await ClosePageAsync();
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            General.LogOfProgram.Error("FatSecretSearchPage | btnSelect_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        SelectedFood = null;
        _taskCompletionSource?.TrySetResult(false);
        await ClosePageAsync();
    }

    private void btnSaveCredentials_Clicked(object sender, EventArgs e)
    {
        var clientId = txtClientId.Text?.Trim();
        var clientSecret = txtClientSecret.Text?.Trim();

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            DisplayAlert("", "Please enter both Client ID and Client Secret", AppStrings.OK);
            return;
        }

        _fatSecretService.SetCredentials(clientId, clientSecret);

        // Save OAuth 1.0 Shared Secret if provided
        var consumerSecret = txtConsumerSecret?.Text?.Trim();
        if (!string.IsNullOrEmpty(consumerSecret))
            _fatSecretService.SetConsumerSecret(consumerSecret);

        // Save locale if the fields are present in the UI
        var language = txtLanguage?.Text?.Trim();
        var region = txtRegion?.Text?.Trim();
        if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(region))
            _fatSecretService.SetLocale(language, region);

        frameCredentials.IsVisible = false;
        UpdateAuthStatus();
        lblStatus.Text = $"Credentials saved. Locale: {_fatSecretService.Language}/{_fatSecretService.Region}";
    }

    private async void txtBarcode_Completed(object sender, EventArgs e)
    {
        await PerformBarcodeSearchAsync();
    }

    private async void btnSearchBarcode_Clicked(object sender, EventArgs e)
    {
        await PerformBarcodeSearchAsync();
    }

    private async void btnScanBarcode_Clicked(object sender, EventArgs e)
    {
        try
        {
            var scannerPage = new BarcodeScannerPage();
            await Navigation.PushModalAsync(scannerPage);

            bool barcodeScanned = await scannerPage.PageClosedTask;
            if (!barcodeScanned || string.IsNullOrWhiteSpace(scannerPage.ScannedBarcode))
                return;

            txtBarcode.Text = scannerPage.ScannedBarcode;
            await PerformBarcodeSearchAsync();
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            General.LogOfProgram.Error("FatSecretSearchPage | btnScanBarcode_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }

    private async Task PerformBarcodeSearchAsync()
    {
        var barcode = txtBarcode.Text?.Trim();
        if (string.IsNullOrEmpty(barcode))
        {
            lblStatus.Text = "Please enter a barcode";
            return;
        }

        if (!_fatSecretService.HasCredentials)
        {
            lblStatus.Text = "Please configure FatSecret API credentials first";
            frameCredentials.IsVisible = true;
            return;
        }

        try
        {
            lblStatus.Text = "Searching by barcode...";
            btnSearchBarcode.IsEnabled = false;
            btnScanBarcode.IsEnabled = false;
            _searchResults.Clear();

            var food = await _fatSecretService.FindFoodByBarcodeAsync(barcode);

            if (food != null)
            {
                _searchResults.Add(food);
                lblStatus.Text = "Found 1 result";
            }
            else
            {
                lblStatus.Text = "No food found for this barcode";
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            General.LogOfProgram.Error("FatSecretSearchPage | PerformBarcodeSearchAsync", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
        finally
        {
            btnSearchBarcode.IsEnabled = true;
            btnScanBarcode.IsEnabled = true;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        SelectedFood = null;
        _taskCompletionSource?.TrySetResult(false);
        return base.OnBackButtonPressed();
    }

    private async Task ClosePageAsync()
    {
        if (this.Parent is NavigationPage navPage)
            await navPage.Navigation.PopModalAsync();
        else
        {
            try { await Navigation.PopModalAsync(); }
            catch { await Navigation.PopAsync(); }
        }
    }
}
