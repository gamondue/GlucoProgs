using SkiaSharp;
using ZXing.Net.Maui;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class BarcodeScannerPage : ContentPage
{
    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);
    public string ScannedBarcode { get; private set; }
    private volatile bool _barcodeDetected;

#if WINDOWS
    private CancellationTokenSource _focusCts;
#endif

    public BarcodeScannerPage()
    {
        InitializeComponent();
        _taskCompletionSource = new TaskCompletionSource<bool>();

        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _barcodeDetected = false;
        cameraBarcodeReaderView.IsDetecting = true;
        lblScannedCode.Text = string.Empty;
#if WINDOWS
        StartAutoFocusCycle();
#endif
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraBarcodeReaderView.IsDetecting = false;
#if WINDOWS
        StopAutoFocusCycle();
#endif
    }

    /// <summary>
    /// Tap on the camera frame: briefly cycles IsDetecting to trigger a camera refocus.
    /// </summary>
    private async void OnCameraFrameTapped(object sender, TappedEventArgs e)
    {
        await CycleFocusAsync();
    }

    /// <summary>
    /// Cycles IsDetecting off then on. This forces most camera drivers to re-run autofocus.
    /// </summary>
    private async Task CycleFocusAsync()
    {
        cameraBarcodeReaderView.IsDetecting = false;
        lblTapToFocus.Text = "Focusing...";
        await Task.Delay(200);
        cameraBarcodeReaderView.IsDetecting = true;
        lblTapToFocus.Text = "Tap to focus";
    }

#if WINDOWS
    /// <summary>
    /// On Windows the camera driver often does not run continuous autofocus.
    /// This background loop cycles the reader every 4 seconds to force a refocus.
    /// </summary>
    private void StartAutoFocusCycle()
    {
        _focusCts = new CancellationTokenSource();
        var token = _focusCts.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(4000, token);
                    if (!token.IsCancellationRequested && cameraBarcodeReaderView.IsDetecting)
                        await Dispatcher.DispatchAsync(CycleFocusAsync);
                }
            }
            catch (OperationCanceledException) { }
        }, token);
    }

    private void StopAutoFocusCycle()
    {
        _focusCts?.Cancel();
        _focusCts?.Dispose();
        _focusCts = null;
    }
#endif

    private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // Guard: the event may fire on multiple frames before IsDetecting = false takes effect
        if (_barcodeDetected)
            return;

        var first = e.Results?.FirstOrDefault();
        if (first == null)
            return;

        _barcodeDetected = true;
        PlayBeep();

        Dispatcher.Dispatch(async () =>
        {
            ScannedBarcode = first.Value;
            lblScannedCode.Text = first.Value;
            txtManualBarcode.Text = first.Value;
            cameraBarcodeReaderView.IsDetecting = false;
#if WINDOWS
            StopAutoFocusCycle();
#endif
            // Auto-close: return the scanned barcode to the caller without user interaction.
            _taskCompletionSource?.TrySetResult(true);
            if (this.Parent is NavigationPage navPage)
                await navPage.Navigation.PopModalAsync();
            else
            {
                try { await Navigation.PopModalAsync(); }
                catch { await Navigation.PopAsync(); }
            }
        });
    }

    private async void btnPickPhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = AppStrings.ChoosePhotoFile ?? "Select barcode image",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                return;

            lblPickPhotoResult.Text = AppStrings.Loading ?? "Processing...";
            btnPickPhoto.IsEnabled = false;

            var decoded = await Task.Run(async () =>
            {
                using var stream = await result.OpenReadAsync();
                return DecodeFromStream(stream);
            });

            if (decoded != null)
            {
                PlayBeep();
                ScannedBarcode = decoded;
                lblScannedCode.Text = decoded;
                lblPickPhotoResult.Text = decoded;
                txtManualBarcode.Text = decoded;
                cameraBarcodeReaderView.IsDetecting = false;
#if WINDOWS
                StopAutoFocusCycle();
#endif
            }
            else
            {
                lblPickPhotoResult.Text = AppStrings.NoResults ?? "No barcode found in image";
            }
        }
        catch (Exception ex)
        {
            lblPickPhotoResult.Text = AppStrings.Error ?? "Error";
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
        finally
        {
            btnPickPhoto.IsEnabled = true;
        }
    }

    /// <summary>
    /// Plays a short beep to signal successful barcode recognition.
    /// </summary>
    private static void PlayBeep()
    {
        try
        {
#if WINDOWS
            // 880 Hz for 150 ms — plays through the Windows audio system on Win 10+
            Console.Beep(880, 150);
#elif ANDROID
            var toneGen = new Android.Media.ToneGenerator(
                Android.Media.Stream.Notification, 80);
            toneGen.StartTone(Android.Media.Tone.PropBeep, 200);
            toneGen.Release();
#endif
        }
        catch { /* ignore on unsupported platforms */ }
    }

    private static string DecodeFromStream(Stream stream)
    {
        using var original = SKBitmap.Decode(stream);
        if (original == null)
            return null;

        const int MaxDim = 1200;
        bool needsResize = original.Width > MaxDim || original.Height > MaxDim;
        using var bitmap = needsResize ? ResizeBitmap(original, MaxDim) : original.Copy();

        var gray = ToGrayscaleBytes(bitmap);
        var luminance = new ZXing.RGBLuminanceSource(
            gray, bitmap.Width, bitmap.Height,
            ZXing.RGBLuminanceSource.BitmapFormat.Gray8);

        var reader = new ZXing.BarcodeReaderGeneric
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions { TryHarder = true }
        };

        return reader.Decode(luminance)?.Text;
    }

    private static SKBitmap ResizeBitmap(SKBitmap source, int maxDimension)
    {
        float scale = Math.Min((float)maxDimension / source.Width, (float)maxDimension / source.Height);
        var resized = new SKBitmap((int)(source.Width * scale), (int)(source.Height * scale));
        source.ScalePixels(resized, SKSamplingOptions.Default);
        return resized;
    }

    private static byte[] ToGrayscaleBytes(SKBitmap bitmap)
    {
        using var gray = bitmap.Copy(SKColorType.Gray8);
        return gray.Bytes;
    }

    private async void btnConfirm_Clicked(object sender, EventArgs e)
    {
        string barcode = !string.IsNullOrWhiteSpace(txtManualBarcode.Text)
            ? txtManualBarcode.Text.Trim()
            : ScannedBarcode;

        if (!string.IsNullOrWhiteSpace(barcode))
        {
            ScannedBarcode = barcode;
            _taskCompletionSource?.TrySetResult(true);
        }
        else
        {
            _taskCompletionSource?.TrySetResult(false);
        }

        if (this.Parent is NavigationPage navPage)
            await navPage.Navigation.PopModalAsync();
        else
        {
            try { await Navigation.PopModalAsync(); }
            catch { await Navigation.PopAsync(); }
        }
    }

    private async void btnEscape_Clicked(object sender, EventArgs e)
    {
        ScannedBarcode = null;
        _taskCompletionSource?.TrySetResult(false);

        if (this.Parent is NavigationPage navPage)
            await navPage.Navigation.PopModalAsync();
        else
        {
            try { await Navigation.PopModalAsync(); }
            catch { await Navigation.PopAsync(); }
        }
    }

    protected override bool OnBackButtonPressed()
    {
        ScannedBarcode = null;
        _taskCompletionSource?.TrySetResult(false);
        return base.OnBackButtonPressed();
    }
}
