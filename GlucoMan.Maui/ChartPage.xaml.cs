using GlucoMan.BusinessLayer;
using GlucoMan.Maui.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace GlucoMan.Maui;

public partial class ChartPage : ContentPage
{
    BL_GlucoseMeasurements bl = new();
    private string _dataType;
    private DateTime _dateOfGraph;
    private List<(float Hour, float Value)> _dataPoints = new();
    private List<GlucoseRecord> list;

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
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Allow rotation for chart page (landscape is useful for charts)
      DisplayOrientationHelper.AllowAllOrientations();
    
  System.Diagnostics.Debug.WriteLine("ChartPage - Orientation unlocked");
    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    
        // Lock back to portrait when leaving chart page
 DisplayOrientationHelper.LockToPortrait();
   
        System.Diagnostics.Debug.WriteLine("ChartPage - Orientation locked to portrait");
    }

    private void UpdateDateDisplay()
    {
        lblDataType.Text = $"Graph of {_dataType}";
        lblDateRange.Text = $"{_dateOfGraph:f}";

        // Load records from database for the selected day
        ////////_dataPoints = bl.GetGraphData(_dateOfGraph);
        SetupChart();
    }
    private void OnPreviousDayClicked(object sender, EventArgs e)
    {
        // Subtract one day
        _dateOfGraph = _dateOfGraph.AddDays(-1);

        UpdateDateDisplay();

        // TODO: Reload data from database for the new date
        // SetupChart(); // When you implement real data loading

        System.Diagnostics.Debug.WriteLine($"Previous day clicked. New date: {_dateOfGraph:d}");
    }
    private void OnNextDayClicked(object sender, EventArgs e)
    {
        // Add one day
        _dateOfGraph = _dateOfGraph.AddDays(1);

        UpdateDateDisplay();

        // TODO: Reload data from database for the new date
        // SetupChart(); // When you implement real data loading

        System.Diagnostics.Debug.WriteLine($"Next day clicked. New date: {_dateOfGraph:d}");
    }
    private void SetupChart()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("SetupChart start");

            // Clear previous data
            _dataPoints.Clear();

            // Load glucose records from database for the selected day
            DateTime startOfDay = _dateOfGraph.Date; // Midnight of selected day
            DateTime endOfDay = startOfDay.AddDays(1).AddSeconds(-1); // 23:59:59 of selected day

            list = bl.GetGlucoseRecords(startOfDay, endOfDay);

            if (list != null && list.Count > 0)
            {
                // Convert GlucoseRecord to chart data points
                foreach (var record in list)
                {
                    if (record.Timestamp != null && record.Timestamp.DateTime.HasValue &&
                     record.GlucoseValue != null && record.GlucoseValue.Double.HasValue)
                    {
                        // Extract hour from timestamp (including fractional part for minutes)
                        float hour = (float)record.Timestamp.DateTime.Value.Hour +
                       (float)record.Timestamp.DateTime.Value.Minute / 60f;
                        float glucoseValue = (float)record.GlucoseValue.Double.Value;

                        _dataPoints.Add((hour, glucoseValue));

                        System.Diagnostics.Debug.WriteLine($"Added point: Hour={hour:F2}, Glucose={glucoseValue}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Loaded {_dataPoints.Count} glucose points from database");
            }
            else
            {
                //// If no data from database, use sample data for testing
                //System.Diagnostics.Debug.WriteLine("No data found in database, using sample data");

                //_dataPoints.Add((2, 100));
                //_dataPoints.Add((8, 130));
                //_dataPoints.Add((10, 120));
                //_dataPoints.Add((13, 250));
                //_dataPoints.Add((15, 180));
                //_dataPoints.Add((18, 130));
                //_dataPoints.Add((20, 218));
                //_dataPoints.Add((24, 170));
            }

            System.Diagnostics.Debug.WriteLine($"SetupChart completed with {_dataPoints.Count} points");

            // Force redraw
            chartView?.InvalidateSurface();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR in SetupChart: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

            //// In case of error, use sample data to at least show something
            //_dataPoints.Clear();
            //_dataPoints.Add((8, 120));
            //_dataPoints.Add((12, 180));
            //_dataPoints.Add((18, 150));
            //_dataPoints.Add((22, 130));

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
}

