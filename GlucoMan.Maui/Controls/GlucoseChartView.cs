using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace GlucoMan.Maui.Controls;

public class GlucoseChartView : SKCanvasView
{
    private List<(float Hour, float Value)> _dataPoints = new();
    
    // Proprietà configurabili
    public float MinY { get; set; } = 0;
    public float MaxY { get; set; } = 500;
    public float YStep { get; set; } = 50;
    public float MinX { get; set; } = 0;
    public float MaxX { get; set; } = 24;
    public float XStep { get; set; } = 2;
    
    public SKColor BackgroundColor { get; set; } = SKColors.White;
    public SKColor AxisColor { get; set; } = SKColors.Black;
    public SKColor CurveColor { get; set; } = SKColor.Parse("#8B0000");
    public SKColor SafeZoneColor { get; set; } = SKColor.Parse("#E8F5E9");
    public SKColor ThresholdColor { get; set; } = SKColors.Red;
    
    public float HighThreshold { get; set; } = 240;
    public float LowThreshold { get; set; } = 80;
    public float SafeZoneMin { get; set; } = 60;
    public float SafeZoneMax { get; set; } = 180;
    
    public GlucoseChartView()
    {
        PaintSurface += OnPaintSurface;
    }
    
    public void SetData(List<(float Hour, float Value)> dataPoints)
    {
        _dataPoints = dataPoints ?? new();
        InvalidateSurface();
    }
    
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var info = e.Info;
        
        canvas.Clear(BackgroundColor);
        
        // Margini del grafico
        float marginLeft = 60;
        float marginRight = 30;
        float marginTop = 30;
        float marginBottom = 60;
        
        float chartWidth = info.Width - marginLeft - marginRight;
        float chartHeight = info.Height - marginTop - marginBottom;
        
        // Rettangolo del grafico
        var chartRect = new SKRect(marginLeft, marginTop, 
            marginLeft + chartWidth, marginTop + chartHeight);
        
        // 1. Disegna fascia verde pastello (zona sicura 60-180)
        DrawSafeZone(canvas, chartRect);
        
        // 2. Disegna griglia e assi
        DrawGrid(canvas, chartRect);
        DrawAxes(canvas, chartRect);
        
        // 3. Disegna soglie rosse (80 e 240)
        DrawThresholds(canvas, chartRect);
        
        // 4. Disegna la curva dei dati
        if (_dataPoints.Count > 0)
        {
            DrawCurve(canvas, chartRect);
        }
        
        // 5. Disegna label e tick
        DrawLabels(canvas, chartRect);
    }
    
    private void DrawSafeZone(SKCanvas canvas, SKRect chartRect)
    {
        float yMin = MapYToCanvas(SafeZoneMax, chartRect);
        float yMax = MapYToCanvas(SafeZoneMin, chartRect);
        
        using var paint = new SKPaint
        {
            Color = SafeZoneColor,
            Style = SKPaintStyle.Fill
        };
        
        canvas.DrawRect(chartRect.Left, yMin, chartRect.Width, yMax - yMin, paint);
    }
    
    private void DrawGrid(SKCanvas canvas, SKRect chartRect)
    {
        using var paint = new SKPaint
        {
            Color = AxisColor,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            PathEffect = SKPathEffect.CreateDash(new float[] { 5, 5 }, 0)
        };
        
        // Linee orizzontali ogni 50 mg/dL
        for (float y = MinY; y <= MaxY; y += YStep)
        {
            float yPos = MapYToCanvas(y, chartRect);
            canvas.DrawLine(chartRect.Left, yPos, chartRect.Right, yPos, paint);
        }
    }
    
    private void DrawThresholds(SKCanvas canvas, SKRect chartRect)
    {
        using var paint = new SKPaint
        {
            Color = ThresholdColor,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2,
            PathEffect = SKPathEffect.CreateDash(new float[] { 3, 3 }, 0)
        };
        
        // Linea a 240
        float y240 = MapYToCanvas(HighThreshold, chartRect);
        canvas.DrawLine(chartRect.Left, y240, chartRect.Right, y240, paint);
        
        // Linea a 80
        float y80 = MapYToCanvas(LowThreshold, chartRect);
        canvas.DrawLine(chartRect.Left, y80, chartRect.Right, y80, paint);
    }
    
    private void DrawAxes(SKCanvas canvas, SKRect chartRect)
    {
        using var paint = new SKPaint
        {
            Color = AxisColor,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };
        
        // Asse Y (sinistra)
        canvas.DrawLine(chartRect.Left, chartRect.Top, chartRect.Left, chartRect.Bottom, paint);
        
        // Asse X (basso)
        canvas.DrawLine(chartRect.Left, chartRect.Bottom, chartRect.Right, chartRect.Bottom, paint);
    }
    
    private void DrawLabels(SKCanvas canvas, SKRect chartRect)
    {
        using var textPaint = new SKPaint
        {
            Color = AxisColor,
            TextSize = 24,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };
        
        using var tickPaint = new SKPaint
        {
            Color = AxisColor,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2
        };
        
        // Label asse Y con tick
        for (float y = MinY; y <= MaxY; y += YStep)
        {
            float yPos = MapYToCanvas(y, chartRect);
            
            // Tick lungo
            canvas.DrawLine(chartRect.Left - 10, yPos, chartRect.Left, yPos, tickPaint);
            
            // Label
            textPaint.TextAlign = SKTextAlign.Right;
            canvas.DrawText($"{y:F0}", chartRect.Left - 15, yPos + 8, textPaint);
        }
        
        // Tick corti intermedi asse Y (ogni 25)
        using var shortTickPaint = new SKPaint
        {
            Color = AxisColor,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1
        };
        
        for (float y = MinY; y <= MaxY; y += YStep / 2)
        {
            if (y % YStep != 0) // Solo tick intermedi
            {
                float yPos = MapYToCanvas(y, chartRect);
                canvas.DrawLine(chartRect.Left - 5, yPos, chartRect.Left, yPos, shortTickPaint);
            }
        }
        
        // Label asse X con tick
        for (float x = MinX; x <= MaxX; x += XStep)
        {
            float xPos = MapXToCanvas(x, chartRect);
            
            // Tick lungo
            canvas.DrawLine(xPos, chartRect.Bottom, xPos, chartRect.Bottom + 10, tickPaint);
            
            // Label
            textPaint.TextAlign = SKTextAlign.Center;
            canvas.DrawText($"{x:F0}h", xPos, chartRect.Bottom + 35, textPaint);
        }
        
        // Tick corti intermedi asse X (ogni ora)
        for (float x = MinX; x <= MaxX; x += 1)
        {
            if (x % XStep != 0) // Solo tick intermedi
            {
                float xPos = MapXToCanvas(x, chartRect);
                canvas.DrawLine(xPos, chartRect.Bottom, xPos, chartRect.Bottom + 5, shortTickPaint);
            }
        }
        
        // Label unità di misura
        textPaint.TextSize = 28;
        textPaint.TextAlign = SKTextAlign.Center;
        canvas.DrawText("mg/dL", chartRect.Left - 30, chartRect.Top - 10, textPaint);
        canvas.DrawText("Hours", chartRect.Left + chartRect.Width / 2, chartRect.Bottom + 55, textPaint);
    }
    
    private void DrawCurve(SKCanvas canvas, SKRect chartRect)
    {
        using var path = new SKPath();
        
        bool first = true;
        foreach (var point in _dataPoints.OrderBy(p => p.Hour))
        {
            float x = MapXToCanvas(point.Hour, chartRect);
            float y = MapYToCanvas(point.Value, chartRect);
            
            if (first)
            {
                path.MoveTo(x, y);
                first = false;
            }
            else
            {
                path.LineTo(x, y);
            }
        }
        
        using var paint = new SKPaint
        {
            Color = CurveColor,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 3,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };
        
        canvas.DrawPath(path, paint);
    }
    
    private float MapXToCanvas(float x, SKRect chartRect)
    {
        float ratio = (x - MinX) / (MaxX - MinX);
        return chartRect.Left + ratio * chartRect.Width;
    }
    
    private float MapYToCanvas(float y, SKRect chartRect)
    {
        float ratio = (y - MinY) / (MaxY - MinY);
        return chartRect.Bottom - ratio * chartRect.Height;
    }
}
