using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;
using System.Security.AccessControl;
using Microsoft.Maui.Graphics;

namespace GlucoMan.Maui
{
    public class CirclesDrawable : IDrawable
    {
        BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
        // this is an IDrawable object that draws the circles of injections on the containing GraphicsView
        // it includes the logic to re-draw all the circles anytime anything has changhed in the graphics
        // it also includes the logic to save the circles and read them from data file
        public enum PointType
        {
            Front,
            Back,
            Hands,
            Sensor,
        }
        bool isCallerEditing;
        internal bool IsCallerEditing
        {
            get
            {
                return isCallerEditing;
            }
            set
            {
                isCallerEditing = value;
            }
        }
        PointType type;
        internal PointType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        // the coordinates of the reference points are the centers of the circles that will be added to the graphics
        private List<CircleData> InjectionPointsCoordinates = new();
        // when we aren't editing we also need a collection of reference points  
        private List<Microsoft.Maui.Graphics.Point> ReferencePointsCoordinates = new();

        // Reference Image Height and Widht.
        // Dynamic radii of circles will be based on theese values
        // (values on my computer at max dimensions on Windows screen)
        private static double DefaultImageWidth { get; set; } = 1280;
        private static double DefaultImageHeight { get; set; } = 1366;
        // to be considered the same point (base value - will be scaled)
        // base radii (will be scaled according to image size)
        // (evaluated at dimensions DefaultImageWidth and DefaultImageHeight)
        private float baseReferenceRadius = 15f; 
        private float baseInjectionRadius = 22;
        // the distance in pixels that is considered to be near enough to one reference point
        private float nearEnoughScaleOneDistance = 30;

        // radii used for drawing/hit testing are computed properties
        private float CurrentReferenceRadius => (float)(baseReferenceRadius * GetScale());
        private float CurrentInjectionRadius => (float)(baseInjectionRadius * GetScale());
        private float CurrentNearEnoughDistance => (float)(nearEnoughScaleOneDistance * GetScale());

        private bool isFirstClick = true;

        private double imageWidth;
        private double imageHeight;
        private int? idCurrentInjection;
        private double circlesVisibilityMaxTimeInDays;

        public CirclesDrawable() // constructor with no parameters, required by XAMl
        {
            imageWidth = DefaultImageWidth;
            imageHeight = DefaultImageHeight;
        }
        public CirclesDrawable(double ImageWidth, double ImageHeight,
            int? IdInjection, double CirclesVisibilityMaxTimeInDays) 
        {
            imageWidth = ImageWidth;
            imageHeight = ImageHeight;
            idCurrentInjection = IdInjection;
            circlesVisibilityMaxTimeInDays = CirclesVisibilityMaxTimeInDays;
        }
        private double GetScale()
        {
            try
            {
                if (DefaultImageWidth <= 0 || DefaultImageHeight <= 0)
                    return 1.0;
                double sx = imageWidth / DefaultImageWidth;
                double sy = imageHeight / DefaultImageHeight;
                double s = Math.Min(sx, sy);
                if (s <= 0) s = 1.0;
                return s;
            }
            catch
            {
                return 1.0;
            }
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Debug per capire cosa viene disegnato
            System.Diagnostics.Debug.WriteLine($"Draw - isCallerEditing: {isCallerEditing}");
            System.Diagnostics.Debug.WriteLine($"Draw - ReferencePoints count: {ReferencePointsCoordinates.Count}");
            System.Diagnostics.Debug.WriteLine($"Draw - InjectionPoints count: {InjectionPointsCoordinates.Count}");
            
            if (isCallerEditing)
            {
                // PUNTINI BLU - sempre uguali (punti di riferimento)
                System.Diagnostics.Debug.WriteLine("Drawing BLUE reference points");
                foreach (Microsoft.Maui.Graphics.Point point in ReferencePointsCoordinates)
                {
                    // Forza colore blu fisso per evitare problemi di rendering su Windows
                    canvas.StrokeColor = Color.FromRgb(0, 0, 255);  // Blu fisso
                    canvas.StrokeSize = Math.Max(1f, CurrentReferenceRadius / 3f);
                    canvas.FillColor = Color.FromRgb(0, 0, 255);    // Blu fisso
                    // Disegna cerchio pieno usando il raggio scalato
                    canvas.FillEllipse(
                        (float)(point.X - CurrentReferenceRadius),
                        (float)(point.Y - CurrentReferenceRadius),
                        CurrentReferenceRadius * 2,
                        CurrentReferenceRadius * 2
                    );
                }
            }
            else
            {
                // CERCHI COLORATI - con saturazione variabile (iniezioni)
                System.Diagnostics.Debug.WriteLine("Drawing COLORED injection points");
                DateTime now = DateTime.Now;
                foreach (CircleData point in InjectionPointsCoordinates)
                {
                    canvas.StrokeColor = point.Color;
                    canvas.StrokeSize = Math.Max(1f, CurrentInjectionRadius / 3f);
                    canvas.FillColor = point.Color;
                    // Disegna cerchio pieno usando il raggio scalato
                    canvas.FillEllipse(
                        (float)(point.Position.X - CurrentInjectionRadius),
                        (float)(point.Position.Y - CurrentInjectionRadius),
                        CurrentInjectionRadius * 2,
                        CurrentInjectionRadius * 2
                    );
                }
            }
        }
        internal Microsoft.Maui.Graphics.Point AddPoint(Microsoft.Maui.Graphics.Point clickPosition, bool IsEditing)
        {
            // Usa direttamente la posizione del click come centro
            double XCenter = clickPosition.X;
            double YCenter = clickPosition.Y;
            Microsoft.Maui.Graphics.Point nearestPoint = new();
            if (IsEditing)
            {
                nearestPoint = FindNearest(new Microsoft.Maui.Graphics.Point(XCenter, YCenter), ReferencePointsCoordinates);
                // calculate the cartesion distance between the clicked point and the nearestPoint reference point
                float distance = (float)Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                    + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
                // if the distance is less than nearEnoughScaleOneDistance, the clicked point is considered
                // to be the same as the nearestPoint reference point
                if (distance < CurrentNearEnoughDistance)
                {
                    // if the clicked point is near enough to a reference point, the reference point is removed
                    ReferencePointsCoordinates.Remove(nearestPoint);
                    // the clicked point is added to the list of reference points
                    ReferencePointsCoordinates.Add(new Microsoft.Maui.Graphics.Point(XCenter, YCenter));
                }
                else
                {
                    // if the clicked point is not near enough to a reference point,
                    // it is just added to the list of reference points
                    ReferencePointsCoordinates.Add(new Microsoft.Maui.Graphics.Point(XCenter, YCenter));
                }
            }
            else
            {
                // we are not editing 
                // since the click has been done while setting the point of the injection
                // the center is brought to the nearestPoint reference point
                nearestPoint = FindNearest(new Microsoft.Maui.Graphics.Point(XCenter, YCenter), ReferencePointsCoordinates);
                if (nearestPoint.X != double.MaxValue && nearestPoint.Y != double.MaxValue)
                {
                    XCenter = nearestPoint.X;
                    YCenter = nearestPoint.Y;
                }
                // the center is added to the list
                if (!isFirstClick)
                {
                    // if it is not the first click, the last point is deleted from the list,
                    InjectionPointsCoordinates.RemoveAt(InjectionPointsCoordinates.Count - 1);
                }
                else
                {   // if it is the first click, the flag is set to false
                    isFirstClick = false;
                }
                CircleData circleData = new CircleData(
                    new Microsoft.Maui.Graphics.Point(XCenter, YCenter), Colors.Red);
                // the center is added to the list
                InjectionPointsCoordinates.Add(circleData);
            }
            return nearestPoint;
        }
        internal void RemovePointIfNear(Microsoft.Maui.Graphics.Point LeftTopPosition)
        {
            // Il click è già il centro - non aggiungere il raggio
            double XCenter = LeftTopPosition.X;
            double YCenter = LeftTopPosition.Y;
            
            // check if the clicked point is near enough to a reference point
            Microsoft.Maui.Graphics.Point nearestPoint = FindNearest(new Microsoft.Maui.Graphics.Point(XCenter, YCenter), ReferencePointsCoordinates);
            // calculate the cartesion distance between the clicked point and the nearestPoint reference point
            double distance = Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
            // if the distance is less than nearEnoughScaleOneDistance, the clicked point is deleted
            if (distance < CurrentNearEnoughDistance)
            {
                // if the clicked point is near enough to a point, the reference point is removed
                ReferencePointsCoordinates.Remove(nearestPoint);
            }
        }
        private Microsoft.Maui.Graphics.Point FindNearest(Microsoft.Maui.Graphics.Point Passed, List<Microsoft.Maui.Graphics.Point> GivenPoints)
        {
            Microsoft.Maui.Graphics.Point min = new Microsoft.Maui.Graphics.Point(double.MaxValue, double.MaxValue);
            // find the point in the list that is the nearestPoint to the passed point
            {
                double SquaredDistanceMin = float.MaxValue;
                foreach (Microsoft.Maui.Graphics.Point point in GivenPoints)
                {
                    double displacementX = (float)(point.X - Passed.X);
                    double displacementY = (float)(point.Y - Passed.Y);
                    double squaredDistance = displacementX * displacementX + displacementY * displacementY;
                    if (squaredDistance < SquaredDistanceMin)
                    {
                        SquaredDistanceMin = squaredDistance;
                        min.X = point.X;
                        min.Y = point.Y;
                    }
                }
                // when the loop ends, XMin and YMin are the coordinates of the nearestPoint point
                // we give them back to the caller
                return min;
            }
        }
        internal void SaveReferenceCoordinates(Common.ZoneOfPosition ZoneOfPositions,
            double imgWidth, double imgHeight)
        {
            if (isCallerEditing)
            {
                // Convert Microsoft.Maui.Graphics.Point to GlucoMan.BusinessObjects.Point
                var businessPoints = ReferencePointsCoordinates.Select(p => 
                    new GlucoMan.BusinessObjects.Point(p.X, p.Y)).ToList();
                bl.SaveNewReferenceCoordinates(businessPoints, ZoneOfPositions, imgWidth, imgHeight);
            }
        }
        internal void ClearAll(Common.ZoneOfPosition ZoneOfPositions)
        {
            try
            {
                bl.DeleteAllReferenceCoordinates(ZoneOfPositions);
                InjectionPointsCoordinates.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while clearing the coordinates from database");
            }
        }
        internal void LoadInjectionsCoordinates(List<Injection> allRecentInjections)
        {
            int circleHue = 240;
            DateTime now = DateTime.Now;
            double maximalSaturation = 100;
            double minimalSaturation = 20;

            // Parametri per la trasparenza
            double maximalAlpha = 1.0;      // Completamente opaco (iniezioni recenti)
            double minimalAlpha = 0.1;      // Quasi trasparente (iniezioni vecchie)

            System.Diagnostics.Debug.WriteLine($"LoadInjectionsCoordinates - Loading {allRecentInjections.Count} injections");

            // Trova le coordinate dell'iniezione corrente per il confronto
            Injection currentInjection = allRecentInjections.FirstOrDefault(inj => inj.IdInjection == idCurrentInjection);
            double? currentX = currentInjection?.PositionX;
            double? currentY = currentInjection?.PositionY;

            foreach (Injection injection in allRecentInjections)
            {
                if (injection.PositionX != null && injection.PositionY != null)
                {
                    Color circleColor;
                    if (injection.Timestamp.DateTime != null)
                    {
                        if (injection.IdInjection != idCurrentInjection)
                        {
                            // if this injection has the same coordinates as the current injection
                            // we avoid drawing its circle
                            if (currentX.HasValue && currentY.HasValue &&
                                Math.Abs(injection.PositionX.Value - currentX.Value) < 0.001 &&
                                Math.Abs(injection.PositionY.Value - currentY.Value) < 0.001)
                            {
                                System.Diagnostics.Debug.WriteLine($"Skipping injection {injection.IdInjection}: same coordinates as current injection");
                                continue; // Skip this injection - same coordinates as current
                            }

                            double diffInDays = now.Subtract((DateTime)injection.Timestamp.DateTime).TotalDays;

                            // Formula per saturazione: iniezioni recenti hanno saturazione alta, vecchie hanno saturazione bassa
                            double saturation = maximalSaturation - (maximalSaturation - minimalSaturation) * diffInDays / circlesVisibilityMaxTimeInDays;

                            if (saturation < minimalSaturation)
                                saturation = minimalSaturation;
                            if (saturation > maximalSaturation)
                                saturation = maximalSaturation;

                            // Formula per trasparenza: iniezioni recenti sono opache, vecchie sono trasparenti
                            double alpha = maximalAlpha - (maximalAlpha - minimalAlpha) * diffInDays / circlesVisibilityMaxTimeInDays;

                            if (alpha < minimalAlpha)
                                alpha = minimalAlpha;
                            if (alpha > maximalAlpha)
                                alpha = maximalAlpha;

                            // operation in double, not in byte
                            var s01 = saturation / 100.0;
                            circleColor = HsvToColor(circleHue, s01, 1.0, (float)alpha);
                        }
                        else
                        {
                            // the current injection is drawn in different color
                            circleColor = Colors.Brown;
                            System.Diagnostics.Debug.WriteLine($"Current injection {injection.IdInjection}: BROWN");
                        }
                    }
                    else
                    {
                        // the "to be" injection is drawn in red 
                        circleColor = Colors.Red;
                        System.Diagnostics.Debug.WriteLine($"To-be injection: RED");
                    }
                    // multiply by image height and width to bring the coordinates from 
                    // normalized (from 0 to 1) to real.
                    // N.B.: they are the normalized coordinates that are stored in the database
                    CircleData circlePoint = new CircleData(new Microsoft.Maui.Graphics.Point(
                        (float)(injection.PositionX * imageWidth),
                        (float)(injection.PositionY * imageHeight)),
                        circleColor);
                    InjectionPointsCoordinates.Add(circlePoint);
                }
            }
        }
        internal void LoadReferenceCoordinates(List<PositionOfInjection> allReferencePositions)
        {
            foreach (PositionOfInjection injection in allReferencePositions)
            {
                if (injection.PositionX != null & injection.PositionY != null)
                    ReferencePointsCoordinates.Add(new Microsoft.Maui.Graphics.Point(
                        (float)(injection.PositionX * imageWidth),
                        (float)(injection.PositionY * imageHeight)
                    ));
            }
        }
        internal double? NormalizeXPosition(double x)
        {
            return x / imageWidth;
        }
        internal double? NormalizeYPosition(double y)
        {
            return y / imageHeight;
        }
        // helper to convert with no quantization to byte
        private static Color HsvToColor(int hue, double saturation01, double value01, float alpha)
        {
            double h = ((hue % 360) + 360) % 360;
            double s = Math.Clamp(saturation01, 0.0, 1.0);
            double v = Math.Clamp(value01, 0.0, 1.0);

            double r, g, b;
            if (s == 0)
            {
                r = g = b = v;
            }
            else
            {
                double sector = h / 60.0;
                int i = (int)Math.Floor(sector);
                double f = sector - i;
                double p = v * (1 - s);
                double q = v * (1 - s * f);
                double t = v * (1 - s * (1 - f));

                switch (i)
                {
                    case 0: r = v; g = t; b = p; break;
                    case 1: r = q; g = v; b = p; break;
                    case 2: r = p; g = v; b = t; break;
                    case 3: r = p; g = q; b = v; break;
                    case 4: r = t; g = p; b = v; break;
                    default: r = v; g = p; b = q; break;
                }
            }
            return new Color((float)r, (float)g, (float)b, alpha);
        }
    }
    internal class CircleData
    {
        internal Microsoft.Maui.Graphics.Point Position { get; private set; }
        internal Color Color { get; private set; }
        internal CircleData(Microsoft.Maui.Graphics.Point Position, Color Color)
        {
            this.Position = Position;
            this.Color = Color;
        }
    }
}
