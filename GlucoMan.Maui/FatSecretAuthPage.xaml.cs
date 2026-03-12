using gamon;
using GlucoMan;

namespace GlucoMan.Maui;

public partial class FatSecretAuthPage : ContentPage
{
    private readonly FatSecretService _fatSecretService;
    private readonly TaskCompletionSource<bool> _tcs = new();

    /// <summary>Awaitable task – true when the user has authorized successfully.</summary>
    public Task<bool> PageClosedTask => _tcs.Task;

    public FatSecretAuthPage(FatSecretService fatSecretService)
    {
        InitializeComponent();
        _fatSecretService = fatSecretService;
        _ = StartAuthFlowAsync();
    }

    private async Task StartAuthFlowAsync()
    {
        try
        {
            lblStatus.Text = "Requesting authorization token...";
            var requestToken = await _fatSecretService.OAuth1_GetRequestTokenAsync();
            var authUrl = _fatSecretService.OAuth1_GetAuthorizationUrl();

            lblStatus.Text = "Please log in with your FatSecret account";
            webViewAuth.Source = new UrlWebViewSource { Url = authUrl };
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            General.LogOfProgram?.Error("FatSecretAuthPage | StartAuthFlowAsync", ex);
        }
    }

    private async void webViewAuth_Navigating(object sender, WebNavigatingEventArgs e)
    {
        // Intercept the callback URL to extract the oauth_verifier
        if (!e.Url.StartsWith(FatSecretService.OAuth1CallbackUrl, StringComparison.OrdinalIgnoreCase))
            return;

        e.Cancel = true; // prevent the WebView from actually loading the callback URL

        try
        {
            lblStatus.Text = "Completing authorization...";
            var uri = new Uri(e.Url);
            var query = ParseQueryString(uri.Query);

            if (!query.TryGetValue("oauth_verifier", out var verifier) ||
                string.IsNullOrEmpty(verifier))
            {
                lblStatus.Text = "Authorization denied or verifier not received.";
                return;
            }

            await _fatSecretService.OAuth1_ExchangeRequestTokenAsync(verifier);

            lblStatus.Text = "Authorization completed!";
            _tcs.TrySetResult(true);
            await ClosePageAsync();
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
            General.LogOfProgram?.Error("FatSecretAuthPage | webViewAuth_Navigating", ex);
        }
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(false);
        await ClosePageAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        _tcs.TrySetResult(false);
        return base.OnBackButtonPressed();
    }

    private async Task ClosePageAsync()
    {
        try { await Navigation.PopModalAsync(); }
        catch { await Navigation.PopAsync(); }
    }

    private static Dictionary<string, string> ParseQueryString(string query)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(query))
            return result;

        // Remove leading '?'
        if (query.StartsWith("?"))
            query = query.Substring(1);

        foreach (var pair in query.Split('&'))
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2)
                result[Uri.UnescapeDataString(parts[0])] = Uri.UnescapeDataString(parts[1]);
        }
        return result;
    }
}
