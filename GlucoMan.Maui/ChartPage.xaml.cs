using GlucoMan.BusinessLayer;
using GlucoMan.Maui.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace GlucoMan.Maui;

public partial class ChartPage : ContentPage
{
    BL_GlucoseMeasurements bl = new();
    private BL_BolusesAndInjections blInj = new();
    private string _dataType;
    private DateTime _dateOfGraph;
    private List<(float Hour, float Value)> _dataPoints = new();
    private List<GlucoseRecord> list;
    private List<Injection> _injections = new();
    private SKBitmap? _injectionBitmap;
    private List<Meal> _meals = new();
    private SKBitmap? _mealBitmap;
    // store icon rectangles to detect taps
    private List<(SKRect Rect, Injection Injection)> _injectionIconRects = new();
    private List<(SKRect Rect, Meal Meal)> _mealIconRects = new();

    private static float greenBandLower = 80;
    private static float greenBandUpper = 180;

    private static float initialY = 0;
    private static float finalY = 500;
    private static float stepY = 50;
    private static float halfStepY = stepY / 2;
    private static float quarterStepY = stepY / 4;

    private static float initialX = 0;
    private static float finalX = 24;
    private static float stepX = 4;
    private static float halfStepX = stepX / 2;
    private static float quarterStepX = stepX / 4;

    private float alarmHigh = 240;
    private float alarmLow = 60;

    public ChartPage(string dataType, DateTime dateOfGraph)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("ChartPage constructor start");

            InitializeComponent();

            System.Diagnostics.Debug.WriteLine("InitializeComponent completed");

            _dataType = dataType;
            _dateOfGraph = dateOfGraph;

            // Display the parameters
            UpdateDateDisplay();

            System.Diagnostics.Debug.WriteLine("Labels set");

            // Setup chart data
            SetupChart();

            // Enable touch events on SKCanvasView to allow tap detection
            try
            {
                if (chartView != null)
                {
                    chartView.EnableTouchEvents = true;
                    chartView.Touch += OnChartTouched;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to enable touch on chartView: {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine("ChartPage constructor completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR in ChartPage constructor: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

            // Mostra messaggio di errore all'utente
            DisplayAlert("Error", $"Failed to initialize chart: {ex.Message}", "OK");
            throw;
        }
    }

    private async Task LoadInjectionBitmapAsync()
    {
        try
        {
            // load graphics as embedded resource (images must be added as EmbeddedResource)
            var asm = typeof(App).Assembly;
            var names = asm.GetManifestResourceNames();
            System.Diagnostics.Debug.WriteLine("Assembly embedded resources: " + string.Join(",", names));

            // Try common endingsSyringe for the syringe resource
            var endingsSyringe = new[] { "syringe_20x20.png", "Resources.Images.syringe_20x20.png", ".syringe_20x20.png" };
            foreach (var end in endingsSyringe)
            {
                var resName = names.FirstOrDefault(n => n.EndsWith(end, StringComparison.OrdinalIgnoreCase));
                if (resName != null)
                {
                    using var rs = asm.GetManifestResourceStream(resName);
                    if (rs != null)
                    {
                        _injectionBitmap = SKBitmap.Decode(rs);
                        System.Diagnostics.Debug.WriteLine($"Injection icon loaded from embedded resource: {resName}");
                            
                        //return; 

                    }
                }
            }
            // load meal_20x20.png as well
            var endingsMeal = new[] { "meal_20x20.png", "Resources.Images.meal_20x20.png", ".meal_20x20.png" };
            foreach (var end in endingsMeal)
            {
                var resName = names.FirstOrDefault(n => n.EndsWith(end, StringComparison.OrdinalIgnoreCase));
                if (resName != null)
                {
                    using var rs = asm.GetManifestResourceStream(resName);
                    if (rs != null)
                    {
                        _mealBitmap = SKBitmap.Decode(rs);
                        System.Diagnostics.Debug.WriteLine($"Meal icon loaded from embedded resource: {resName}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _injectionBitmap = null;
            _mealBitmap = null;
            System.Diagnostics.Debug.WriteLine($"Failed to load injection/meal icon: {ex.Message}");
        }
    }

    // Async OnAppearing: await image load when package is ready
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Allow rotation for chart page (landscape is useful for charts)
        DisplayOrientationHelper.AllowAllOrientations();

        System.Diagnostics.Debug.WriteLine("ChartPage - Orientation unlocked");

        try
        {
            await LoadInjectionBitmapAsync();
            // Redraw to show icons if loaded
            chartView?.InvalidateSurface();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load injection icon in OnAppearing: {ex.Message}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Lock back to portrait when leaving chart page
        DisplayOrientationHelper.LockToPortrait();

        System.Diagnostics.Debug.WriteLine("ChartPage - Orientation locked to portrait");
    }

    // UpdateDateDisplay: show date in the top label and use the date label as a status bar
    private void UpdateDateDisplay()
    {
        // move date where "Graph of" used to be
        lblDataType.Text = $"{_dateOfGraph:f}";
        // use the lower label as a status bar (clear for now)
        lblDateRange.Text = string.Empty;

        // Load records from database for the selected day
        ////////_dataPoints = bl.GetGraphData(_dateOfGraph);
        SetupChart();
    }
    private void OnPreviousDayClicked(object sender, EventArgs e)
    {
        // Subtract one day
        _dateOfGraph = _dateOfGraph.AddDays(-1);

        UpdateDateDisplay();

        System.Diagnostics.Debug.WriteLine($"Previous day clicked. New date: {_dateOfGraph:d}");
    }
    private void OnNextDayClicked(object sender, EventArgs e)
    {
        // Add one day
        _dateOfGraph = _dateOfGraph.AddDays(1);

        UpdateDateDisplay();

        System.Diagnostics.Debug.WriteLine($"Next day clicked. New date: {_dateOfGraph:d}");
    }
    private void SetupChart()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("SetupChart start");

            // Clear previous data
            _dataPoints.Clear();
            _injections.Clear();
            _meals.Clear();

            // Load glucose records from database for the selected day
            DateTime startOfDay = _dateOfGraph.Date; // Midnight of selected day
            DateTime endOfDay = startOfDay.AddDays(1).AddSeconds(-1); //23:59:59 of selected day

            // Try to load sensor (continuous) records first
            list = bl.GetSensorsRecords(startOfDay, endOfDay);

            // If continuous sensor data is completely missing, fallback to all glucose records
            if (list == null || list.Count ==0)
            {
                System.Diagnostics.Debug.WriteLine("No sensor (continuous) records found for the day, falling back to GetGlucoseRecords().");
                try
                {
                    list = bl.GetGlucoseRecords(startOfDay, endOfDay);
                    System.Diagnostics.Debug.WriteLine($"Loaded { (list?.Count ??0) } glucose records from fallback GetGlucoseRecords().");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load fallback glucose records: {ex.Message}");
                }
            }

            if (list != null && list.Count >0)
            {
                // Convert GlucoseRecord to chart data points
                foreach (var record in list)
                {
                    if (record.EventTime != null && record.EventTime.DateTime.HasValue &&
                     record.GlucoseValue != null && record.GlucoseValue.Double.HasValue)
                    {
                        // Extract hour from timestamp (including fractional part for minutes)
                        float hour = (float)record.EventTime.DateTime.Value.Hour +
                       (float)record.EventTime.DateTime.Value.Minute / 60f;
                        float glucoseValue = (float)record.GlucoseValue.Double.Value;

                        _dataPoints.Add((hour, glucoseValue));

                        System.Diagnostics.Debug.WriteLine($"Added point: Hour={hour:F2}, Glucose={glucoseValue}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {_dataPoints.Count} glucose points from database");
            }
            //else
            //{
            //    //// If no data from database, use sample data for testing
            //    //System.Diagnostics.Debug.WriteLine("No data found in database, using sample data");

            //    //_dataPoints.Add((2, 100));
            //    //_dataPoints.Add((8, 130)); 
            //    //_dataPoints.Add((10, 120));
            //    //_dataPoints.Add((13, 250));
            //    //_dataPoints.Add((15, 180));
            //    //_dataPoints.Add((18, 130));
            //    //_dataPoints.Add((20, 218));
            //    //_dataPoints.Add((24, 170));
            //}

            // Load injections for the same day
            try
            {
                var inj = blInj.GetInjections(startOfDay, endOfDay);
                if (inj != null)
                    _injections = inj;
                System.Diagnostics.Debug.WriteLine($"Loaded {_injections.Count} injections from database");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load injections: {ex.Message}");
            }

            // Load meals for the same day
            try
            {
                var meals = Common.Database.GetMeals(startOfDay, endOfDay);
                if (meals != null)
                    _meals = meals;
                System.Diagnostics.Debug.WriteLine($"Loaded {_meals.Count} meals from database");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load meals: {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine($"SetupChart completed with {_dataPoints.Count} points");

            // Force redraw
            chartView?.InvalidateSurface();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR in SetupChart: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

            chartView?.InvalidateSurface();
        }
    }
    private void OnChartPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("OnChartPaintSurface called");

            var canvas = e.Surface.Canvas;
            var info = e.Info;

            canvas.Clear(SKColors.White);

            // Chart margins
            float marginLeft = 60;
            float marginRight = 30;
            float marginTop = 30;
            float marginBottom = 60;

            float chartWidth = info.Width - marginLeft - marginRight;
            float chartHeight = info.Height - marginTop - marginBottom;

            var chartRect = new SKRect(marginLeft, marginTop,
                marginLeft + chartWidth, marginTop + chartHeight);

            // Draw green band (greenBandLower-greenBandUpper)
            float MinY = MapYToCanvas(greenBandUpper, chartRect, initialY, finalY);
            float MaxY = MapYToCanvas(greenBandLower, chartRect, initialY, finalY);

            using (var paint = new SKPaint { Color = SKColor.Parse("#E8F5E9"), Style = SKPaintStyle.Fill })
            {
                canvas.DrawRect(chartRect.Left, MinY, chartRect.Width, MaxY - MinY, paint);
            }

            // Draw horizontal grid
            using (var paint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                PathEffect = SKPathEffect.CreateDash(new float[] { 5, 5 }, 0)
            })
            {
                for (float y = initialY; y <= finalY; y += stepY)
                {
                    float yPos = MapYToCanvas(y, chartRect, initialY, finalY);
                    canvas.DrawLine(chartRect.Left, yPos, chartRect.Right, yPos, paint);
                }
            }

            // Draw red lines (alarmLoe and alarmHigh)
            using (var paint = new SKPaint
            {
                Color = SKColors.Red,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2,
                PathEffect = SKPathEffect.CreateDash(new float[] { 3, 3 }, 0)
            })
            {
                float yHighAlarm = MapYToCanvas(alarmHigh, chartRect, initialY, finalY);
                canvas.DrawLine(chartRect.Left, yHighAlarm, chartRect.Right, yHighAlarm, paint);

                float yLowAlarm = MapYToCanvas(alarmLow, chartRect, initialY, finalY);
                canvas.DrawLine(chartRect.Left, yLowAlarm, chartRect.Right, yLowAlarm, paint);
            }

            // Draw axes
            using (var paint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 2 })
            {
                canvas.DrawLine(chartRect.Left, chartRect.Top, chartRect.Left, chartRect.Bottom, paint);
                canvas.DrawLine(chartRect.Left, chartRect.Bottom, chartRect.Right, chartRect.Bottom, paint);
            }

            // Draw ticks on Y axis
            using (var tickPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 2 })
            {
                // Long ticks every stepY (where there are labels)
                for (float y = initialY; y <= finalY; y += stepY)
                {
                    float yPos = MapYToCanvas(y, chartRect, initialY, finalY);
                    canvas.DrawLine(chartRect.Left - 10, yPos, chartRect.Left, yPos, tickPaint);
                }
            }

            // Medium ticks every halfStepY (halfway between labels)
            using (var mediumTickPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1.5f })
            {
                for (float y = initialY; y <= finalY; y += halfStepY)
                {
                    if (y % stepY != 0) // Only intermediate ticks
                    {
                        float yPos = MapYToCanvas(y, chartRect, initialY, finalY);
                        canvas.DrawLine(chartRect.Left - 7, yPos, chartRect.Left, yPos, mediumTickPaint);
                    }
                }
            }

            // Short ticks every quarterStepY (intermediate between all others)
            using (var shortTickPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1 })
            {
                for (float y = initialY; y <= finalY; y += quarterStepY)
                {
                    if (y % halfStepY != 0) // Only smaller ticks
                    {
                        float yPos = MapYToCanvas(y, chartRect, initialY, finalY);
                        canvas.DrawLine(chartRect.Left - 5, yPos, chartRect.Left, yPos, shortTickPaint);
                    }
                }
            }

            // Draw ticks on X axis
            using (var tickPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 2 })
            {
                // Long ticks every 4 hours (where there are labels)
                for (float x = initialX; x <= finalX; x += stepX)
                {
                    float xPos = MapXToCanvas(x, chartRect, initialX, finalX);
                    canvas.DrawLine(xPos, chartRect.Bottom, xPos, chartRect.Bottom + 10, tickPaint);
                }
            }

            // Medium ticks every 2 hours (halfway between labels)
            using (var mediumTickPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1.5f })
            {
                for (float x = initialX; x <= finalX; x += halfStepX)
                {
                    if (x % stepX != 0) // Only intermediate ticks
                    {
                        float xPos = MapXToCanvas(x, chartRect, initialX, finalX);
                        canvas.DrawLine(xPos, chartRect.Bottom, xPos, chartRect.Bottom + 7, mediumTickPaint);
                    }
                }
            }

            // Short ticks every hour (intermediate between all others)
            using (var shortTickPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 1 })
            {
                for (float x = initialX; x <= finalX; x += quarterStepY)
                {
                    if (x % halfStepX != 0) // Only smaller ticks
                    {
                        float xPos = MapXToCanvas(x, chartRect, initialX, finalX);
                        canvas.DrawLine(xPos, chartRect.Bottom, xPos, chartRect.Bottom + 5, shortTickPaint);
                    }
                }
            }

            // Draw Y labels
            using (var paint = new SKPaint { Color = SKColors.Black, TextSize = 24, IsAntialias = true })
            {
                for (float y = initialY; y <= finalY; y += stepY)
                {
                    float yPos = MapYToCanvas(y, chartRect, 0, finalY);
                    paint.TextAlign = SKTextAlign.Right;
                    canvas.DrawText($"{y:F0}", chartRect.Left - 15, yPos + 8, paint);
                }
            }

            // Draw X labels (every 4 hours, without 'h')
            using (var paint = new SKPaint { Color = SKColors.Black, TextSize = 24, IsAntialias = true, TextAlign = SKTextAlign.Center })
            {
                for (float x = initialX; x <= finalX; x += stepX)
                {
                    float xPos = MapXToCanvas(x, chartRect, initialX, finalX);
                    canvas.DrawText($"{x:F0}", xPos, chartRect.Bottom + 35, paint);
                }
            }

            // Draw curve with spline (Bezier curves)
            if (_dataPoints.Count > 1)
            {
                using (var path = new SKPath())
                {
                    var orderedPoints = _dataPoints.OrderBy(p => p.Hour).ToList();

                    // First point
                    float x0 = MapXToCanvas(orderedPoints[0].Hour, chartRect, initialX, finalX);
                    float y0 = MapYToCanvas(orderedPoints[0].Value, chartRect, initialY, finalY);
                    path.MoveTo(x0, y0);

                    // Use cubic Bezier curves to create a smooth spline
                    for (int i = 0; i < orderedPoints.Count - 1; i++)
                    {
                        var p0 = orderedPoints[i];
                        var p1 = orderedPoints[i + 1];

                        float x1 = MapXToCanvas(p0.Hour, chartRect, initialX, finalX);
                        float y1 = MapYToCanvas(p0.Value, chartRect, initialY, finalY);
                        float x2 = MapXToCanvas(p1.Hour, chartRect, initialX, finalX);
                        float y2 = MapYToCanvas(p1.Value, chartRect, initialY, finalY);

                        // Calculate control points for Bezier curves
                        // Use 1/3 of horizontal distance as control point
                        float controlPointDistance = (x2 - x1) / 3;

                        float cx1 = x1 + controlPointDistance;
                        float cy1 = y1;
                        float cx2 = x2 - controlPointDistance;
                        float cy2 = y2;

                        path.CubicTo(cx1, cy1, cx2, cy2, x2, y2);
                    }

                    // Platform-specific stroke width: 6 for Android, 3 for Windows
#if ANDROID
                    float curveStrokeWidth = 6;
#else
                    float curveStrokeWidth = 3;
#endif

                    using (var paint = new SKPaint
                    {
                        Color = SKColor.Parse("#8B0000"),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = curveStrokeWidth,
                        IsAntialias = true,
                        StrokeCap = SKStrokeCap.Round,
                        StrokeJoin = SKStrokeJoin.Round
                    })
                    {
                        canvas.DrawPath(path, paint);
                    }
                }
            }

            // Clear icon rects
            _injectionIconRects.Clear();
            _mealIconRects.Clear();

            // Draw meal icons first so injection icons can be drawn on top
            if (_mealBitmap != null && _meals != null && _meals.Count >0)
            {
                // base icon size in device-independent pixels
                float iconBaseSize =20f;
                float density =1f;
                try { density = (float)DeviceDisplay.MainDisplayInfo.Density; } catch { density =1f; }
                // icon size in physical pixels for the canvas
                float iconSize = iconBaseSize * density;

                using var mealBorderPaint = new SKPaint { Color = SKColors.Blue, Style = SKPaintStyle.Stroke, StrokeWidth =1 };

                var orderedPoints = _dataPoints.OrderBy(p => p.Hour).ToList();

                foreach (var meal in _meals)
                {
                    if (meal.EventTime?.DateTime.HasValue != true)
                        continue;

                    var dt = meal.EventTime.DateTime.Value;
                    float hour = dt.Hour + dt.Minute /60f;
                    float x = MapXToCanvas(hour, chartRect, initialX, finalX);

                    // Determine Y on curve by interpolation or nearest
                    float glucoseY = chartRect.Top + iconSize; // default
                    if (orderedPoints.Count >0)
                    {
                        var before = orderedPoints.LastOrDefault(p => p.Hour <= hour);
                        var after = orderedPoints.FirstOrDefault(p => p.Hour >= hour);

                        bool hasBefore = !before.Equals(default);
                        bool hasAfter = !after.Equals(default);

                        if (hasBefore && hasAfter && before.Hour != after.Hour)
                        {
                            float t = (hour - before.Hour) / (after.Hour - before.Hour);
                            float interpolatedValue = before.Value + t * (after.Value - before.Value);
                            glucoseY = MapYToCanvas(interpolatedValue, chartRect, initialY, finalY);
                        }
                        else if (hasBefore)
                        {
                            glucoseY = MapYToCanvas(before.Value, chartRect, initialY, finalY);
                        }
                        else if (hasAfter)
                        {
                            glucoseY = MapYToCanvas(after.Value, chartRect, initialY, finalY);
                        }
                    }

                    // Place icon slightly above curve
                    float y = glucoseY - iconSize /2f -4f;
                    var destRect = new SKRect(x - iconSize /2f, y, x + iconSize /2f, y + iconSize);
                    canvas.DrawBitmap(_mealBitmap, destRect);
                    canvas.DrawRect(destRect, mealBorderPaint);
                    // remember rect for touch detection
                    _mealIconRects.Add((destRect, meal));
                }
            }

            // Draw injection icons on top of meals so syringe is visible above meal icons
            if (_injectionBitmap != null && _injections != null && _injections.Count >0)
            {
                // base icon size in device-independent pixels
                float iconBaseSize =20f;
                float density =1f;
                try { density = (float)DeviceDisplay.MainDisplayInfo.Density; } catch { density =1f; }
                // icon size in physical pixels for the canvas
                float iconSize = iconBaseSize * density;

                using var borderPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth =1 };

                foreach (var inj in _injections)
                {
                    if (inj.EventTime?.DateTime.HasValue != true)
                        continue;

                    var dt = inj.EventTime.DateTime.Value;
                    // compute hour with minutes fraction
                    float hour = dt.Hour + dt.Minute /60f;
                    float x = MapXToCanvas(hour, chartRect, initialX, finalX);

                    // Find the glucose value at this time by interpolating or using nearest point
                    float glucoseY = chartRect.Top + iconSize; // default if no data

                    // Find nearest data point or interpolate
                    var orderedPoints = _dataPoints.OrderBy(p => p.Hour).ToList();
                    if (orderedPoints.Count >0)
                    {
                        // Find closest point before and after injection time
                        var before = orderedPoints.LastOrDefault(p => p.Hour <= hour);
                        var after = orderedPoints.FirstOrDefault(p => p.Hour >= hour);

                        if (before.Hour >0 && after.Hour >0 && before.Hour != after.Hour)
                        {
                            // Linear interpolation between before and after
                            float t = (hour - before.Hour) / (after.Hour - before.Hour);
                            float interpolatedValue = before.Value + t * (after.Value - before.Value);
                            glucoseY = MapYToCanvas(interpolatedValue, chartRect, initialY, finalY);
                        }
                        else if (before.Hour >0)
                        {
                            // Use the before point
                            glucoseY = MapYToCanvas(before.Value, chartRect, initialY, finalY);
                        }
                        else if (after.Hour >0)
                        {
                            // Use the after point
                            glucoseY = MapYToCanvas(after.Value, chartRect, initialY, finalY);
                        }
                    }

                    // Place icon centered on (x, glucoseY) - shifted slightly above the curve
                    float y = glucoseY - iconSize /2f -4f; //4px above curve center

                    // center the icon on (x,y)
                    var destRect = new SKRect(x - iconSize /2f, y, x + iconSize /2f, y + iconSize);

                    // draw bitmap scaled to destRect
                    canvas.DrawBitmap(_injectionBitmap, destRect);

                    // draw1px black border around icon
                    canvas.DrawRect(destRect, borderPaint);
                    // remember rect for touch detection
                    _injectionIconRects.Add((destRect, inj));
                }
            }

            System.Diagnostics.Debug.WriteLine("OnChartPaintSurface completed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR in OnChartPaintSurface: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
    private float MapXToCanvas(float x, SKRect chartRect, float minX, float maxX)
    {
        float ratio = (x - minX) / (maxX - minX);
        return chartRect.Left + ratio * chartRect.Width;
    }
    private float MapYToCanvas(float y, SKRect chartRect, float minY, float maxY)
    {
        float ratio = (y - minY) / (maxY - minY);
        return chartRect.Bottom - ratio * chartRect.Height;
    }

    // Return interpolated glucose value (mg/dL) for a given hour (fractional), or null if no data
    private double? GetInterpolatedGlucoseValue(double hour)
    {
    if (_dataPoints == null || _dataPoints.Count ==0)
        return null;

    var ordered = _dataPoints.OrderBy(p => p.Hour).ToList();

    // exact match
    var exact = ordered.FirstOrDefault(p => Math.Abs(p.Hour - hour) <0.0001f);
    if (!exact.Equals(default((float, float))))
        return exact.Value;

    // find before and after
    var before = ordered.LastOrDefault(p => p.Hour <= hour);
    var after = ordered.FirstOrDefault(p => p.Hour >= hour);

    bool hasBefore = !before.Equals(default((float, float)));
    bool hasAfter = !after.Equals(default((float, float)));

    if (hasBefore && hasAfter && before.Hour != after.Hour)
    {
        double t = (hour - before.Hour) / (after.Hour - before.Hour);
        return before.Value + t * (after.Value - before.Value);
    }
    else if (hasBefore)
        return before.Value;
    else if (hasAfter)
        return after.Value;

    return null;
    }
    // Touch handler: detect taps near plotted points and show time/value
    private async void OnChartTouched(object sender, SKTouchEventArgs e)
    {
        try
        {
            // Only respond to release to avoid multiple events during move
            if (e.ActionType != SKTouchAction.Released)
            {
                e.Handled = false;
                return;
            }

            var info = chartView?.CanvasSize ?? new SKSize(0, 0);

            // Chart margins - must match drawing logic
            float marginLeft = 60;
            float marginRight = 30;
            float marginTop = 30;
            float marginBottom = 60;

            var chartRect = new SKRect(marginLeft, marginTop,
                marginLeft + (info.Width - marginLeft - marginRight), marginTop + (info.Height - marginTop - marginBottom));

            // First check if touch is on any injection icon
            var touchPoint = e.Location;
            foreach (var tuple in _injectionIconRects)
            {
                if (tuple.Rect.Contains(touchPoint))
                {
                    var injection = tuple.Injection;
                    if (injection?.EventTime?.DateTime.HasValue == true)
                    {
                        var dt = injection.EventTime.DateTime.Value;
                        double hour = dt.Hour + dt.Minute /60.0;
                        var glucose = GetInterpolatedGlucoseValue(hour);

                        // get IU value from Injection.InsulinValue or InsulinCalculated
                        double? iu = null;
                        try { iu = injection.InsulinValue?.Double; } catch { iu = null; }
                        if (iu == null)
                        {
                            try { iu = injection.InsulinCalculated?.Double; } catch { iu = null; }
                        }

                        string glucoseText = glucose.HasValue ? $"{glucose.Value:F0} mg/dL" : "No glucose";
                        string iuText = iu.HasValue ? $"{iu.Value:G}" : "No IU";

                        try { lblDateRange.Text = $"{dt:HH:mm} - {glucoseText} - {iuText} IU"; } catch { }
                        e.Handled = true;
                        return;
                    }
                }
            }

            // Then check meal icons
            foreach (var tuple in _mealIconRects)
            {
                if (tuple.Rect.Contains(touchPoint))
                {
                    var meal = tuple.Meal;
                    if (meal?.EventTime?.DateTime.HasValue == true)
                    {
                        var dt = meal.EventTime.DateTime.Value;
                        double hour = dt.Hour + dt.Minute /60.0;
                        var glucose = GetInterpolatedGlucoseValue(hour);

                        double? cho = null;
                        try { cho = meal.CarbohydratesGrams?.Double; } catch { cho = null; }

                        string glucoseText = glucose.HasValue ? $"{glucose.Value:F0} mg/dL" : "No glucose";
                        string choText = cho.HasValue ? $"{cho.Value:G}" : "No CHO";

                        try { lblDateRange.Text = $"{dt:HH:mm} - {glucoseText} - {choText} CHO"; } catch { }
                        e.Handled = true;
                        return;
                    }
                }
            }

            // If no data points, ignore
            if (_dataPoints == null || _dataPoints.Count ==0)
            {
                e.Handled = true;
                return;
            }

            // Find nearest plotted point to touch location
            SKPoint touch = e.Location;

            float bestDist = float.MaxValue;
            (float Hour, float Value) bestPoint = (0, 0);

            foreach (var p in _dataPoints)
            {
                float px = MapXToCanvas(p.Hour, chartRect, initialX, finalX);
                float py = MapYToCanvas(p.Value, chartRect, initialY, finalY);

                float dx = touch.X - px;
                float dy = touch.Y - py;
                float dist = (float)System.Math.Sqrt(dx * dx + dy * dy);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestPoint = p;
                }
            }

            // Threshold in pixels to consider the tap "on the curve"
            const float threshold = 24f;

            if (bestDist <= threshold)
            {
                // Convert hour (float) to time on selected date
                int hourPart = (int)bestPoint.Hour;
                int minutePart = (int)System.Math.Round((bestPoint.Hour - hourPart) *60);
                var timestamp = new DateTime(_dateOfGraph.Year, _dateOfGraph.Month, _dateOfGraph.Day, hourPart, minutePart,0);

                string timeText = timestamp.ToString("HH:mm");
                string valueText = bestPoint.Value.ToString("F0");

                // Update status label (previously lblDateRange) with time and value
                try
                {
                    lblDateRange.Text = $"{timeText} - {valueText} mg/dL";
                }
                catch
                {
                    // ignore if label not available
                }

                e.Handled = true;
                return;
            }

            e.Handled = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR in OnChartTouched: {ex.Message}");
            e.Handled = false;
        }
    }
}

