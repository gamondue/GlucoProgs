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

        // the distance in pixels that is considered to be near enough to one reference point
        // to be considered the same point
        private double nearEnoughZeroScaleDistance = 25;
        // the radiuses of the circles for reference points and injection points
        // !!!! TODO radiuses should be different based on the number of points in the image
        private float referencePointsZeroScaleRadius = 15; // !!! after debug restore value of 5 !!!!
        private float injectionPointZeroScaleRadius = 8;

        private bool isFirstClick = true;

        private static double DefaultImageWidth { get; set; }
        private static double DefaultImageHeight { get; set; }

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
        public static void SetDefaults(double imageWidth, double imageHeight)
        {
            DefaultImageWidth = imageWidth;
            DefaultImageHeight = imageHeight;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (isCallerEditing)
            {
                foreach (Microsoft.Maui.Graphics.Point point in ReferencePointsCoordinates)
                {
                    canvas.StrokeColor = Colors.Blue;
                    canvas.StrokeSize = referencePointsZeroScaleRadius;
                    canvas.FillColor = Colors.Blue;
                    canvas.DrawEllipse(
                        (float)(point.X - referencePointsZeroScaleRadius), 
                        (float)(point.Y - referencePointsZeroScaleRadius),
                        referencePointsZeroScaleRadius * 2,  // diameter
                        referencePointsZeroScaleRadius * 2   // diameter
                    );
                }
            }
            else
            {
                DateTime now = DateTime.Now;
                foreach (CircleData point in InjectionPointsCoordinates)
                {
                    canvas.StrokeColor = point.Color;
                    canvas.StrokeSize = injectionPointZeroScaleRadius;
                    canvas.FillColor = point.Color;
                    canvas.DrawEllipse((float)(point.Position.X - injectionPointZeroScaleRadius), 
                        (float)(point.Position.Y - injectionPointZeroScaleRadius),
                            injectionPointZeroScaleRadius, injectionPointZeroScaleRadius);
                }
            }
        }
        internal Microsoft.Maui.Graphics.Point AddPoint(Microsoft.Maui.Graphics.Point LeftTopPosition, bool IsEditing)
        {
            // center position is stored in the right list depending on the IsEditing flag
            double XCenter;
            double YCenter;
            Microsoft.Maui.Graphics.Point nearestPoint = new();
            if (IsEditing)
            {
                XCenter = (float)LeftTopPosition.X + referencePointsZeroScaleRadius;
                YCenter = (float)LeftTopPosition.Y + referencePointsZeroScaleRadius;
                // check if the clicked point is near enough to a reference point
                nearestPoint = FindNearest(new Microsoft.Maui.Graphics.Point(XCenter, YCenter), ReferencePointsCoordinates);
                // calculate the cartesion distance between the clicked point and the nearestPoint reference point
                float distance = (float)Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                    + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
                // if the distance is less than nearEnoughZeroScaleDistance, the clicked point is considered
                // to be the same as the nearestPoint reference point
                if (distance < nearEnoughZeroScaleDistance)
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
                // calculate the center of the circle
                XCenter = (float)LeftTopPosition.X + injectionPointZeroScaleRadius;
                YCenter = (float)LeftTopPosition.Y + injectionPointZeroScaleRadius;
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
            // center position is stored in the right list depending on the IsEditing flag
            double XCenter;
            double YCenter;
            // calculate the center of the circle
            XCenter = (float)LeftTopPosition.X + referencePointsZeroScaleRadius;
            YCenter = (float)LeftTopPosition.Y + referencePointsZeroScaleRadius;

            // check if the clicked point is near enough to a reference point
            Microsoft.Maui.Graphics.Point nearestPoint = FindNearest(new Microsoft.Maui.Graphics.Point(XCenter, YCenter), ReferencePointsCoordinates);
            // calculate the cartesion distance between the clicked point and the nearestPoint reference point
            double distance = Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
            // if the distance is less than nearEnoughZeroScaleDistance, the clicked point is deleted
            if (distance < nearEnoughZeroScaleDistance)
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
            double minimalSaturation = 255 * 0.7;
            foreach (Injection injection in allRecentInjections)
            {
                if (injection.PositionX != null && injection.PositionY != null)
                {
                    Color circleColor;
                    if (injection.Timestamp.DateTime != null)
                    {
                        if (injection.IdInjection != idCurrentInjection)
                        {
                            double diffInDays = now.Subtract((DateTime)injection.Timestamp.DateTime).TotalDays;
                            //double saturation = (1 - diffInDays / circlesVisibilityMaxTimeInDays);
                            double saturation = minimalSaturation + (255 - minimalSaturation) * diffInDays / circlesVisibilityMaxTimeInDays;
                            if (saturation < 0)
                                saturation = 0;
                            if (saturation > 255)
                                saturation = 255;
                            HSV circleColorHsv = new HSV(circleHue, saturation, 1);
                            RGB circleColorRgb = new RGB(255, 0, 0);
                            ColorConverter.HSV2RGB(circleColorHsv, circleColorRgb);
                            circleColor = new Color(
                                circleColorRgb.Red / 255f,
                                circleColorRgb.Green / 255f,
                                circleColorRgb.Blue / 255f,
                                1 // (float)saturation
                            );
                        }
                        else
                        {
                            // the current injection is drawn in green
                            circleColor = Colors.Green;
                        }
                    }
                    else
                    {
                        // the "to be" injection is drawn in red
                        circleColor = Colors.Red;
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
