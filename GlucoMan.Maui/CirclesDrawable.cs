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
        private List<Point> InjectionPointsCoordinates = new();
        // when we aren't editing we also need a collection of reference points  
        private List<Point> ReferencePointsCoordinates = new();

        // the distance in pixels that is considered to be near enough to one reference point
        // to be considered the same point
        private double nearEnoughDistance = 16;
        // the radiuses of the circles for reference points and injection points
        private float referencePointsRadius = 5;
        private float injectionPointsRadius = 8;

        private bool isFirstClick = true;

        private static double DefaultImageWidth { get; set; }
        private static double DefaultImageHeight { get; set; }

        private double imageWidth;
        private double imageHeight;

        public CirclesDrawable() // Costruttore senza parametri richiesto da XAML
        {
            imageWidth = DefaultImageWidth;
            imageHeight = DefaultImageHeight;
        }
        public CirclesDrawable(double ImageWidth, double ImageHeight) // Costruttore con parametri
        {
            imageWidth = ImageWidth;
            imageHeight = ImageHeight;
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
                foreach (Point point in ReferencePointsCoordinates)
                {
                    canvas.StrokeColor = Colors.Blue;
                    canvas.StrokeSize = referencePointsRadius;
                    canvas.FillColor = Colors.Blue;
                    canvas.DrawEllipse((float)(point.X - referencePointsRadius), (float)(point.Y - referencePointsRadius),
                        referencePointsRadius, referencePointsRadius);
                }
            }
            else
            {
                foreach (Point point in InjectionPointsCoordinates)
                {
                    canvas.StrokeColor = Colors.Red;
                    canvas.StrokeSize = injectionPointsRadius;
                    canvas.FillColor = Colors.Red;
                    canvas.DrawEllipse((float)(point.X - injectionPointsRadius), (float)(point.Y - injectionPointsRadius),
                            injectionPointsRadius, injectionPointsRadius);
                }
            }
        }
        internal Point AddPoint(Point LeftTopPosition, bool IsEditing)
        {
            // center position is stored in the right list depending on the IsEditing flag
            double XCenter;
            double YCenter;
            Point nearestPoint = new();
            if (IsEditing)
            {
                XCenter = (float)LeftTopPosition.X + referencePointsRadius;
                YCenter = (float)LeftTopPosition.Y + referencePointsRadius;
                // check if the clicked point is near enough to a reference point
                nearestPoint = FindNearest(new Point(XCenter, YCenter), ReferencePointsCoordinates);
                // calculate the cartesion distance between the clicked point and the nearestPoint reference point
                float distance = (float)Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                    + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
                // if the distance is less than nearEnoughDistance, the clicked point is considered
                // to be the same as the nearestPoint reference point
                if (distance < nearEnoughDistance)
                {
                    // if the clicked point is near enough to a reference point, the reference point is removed
                    ReferencePointsCoordinates.Remove(nearestPoint);
                    // the clicked point is added to the list of reference points
                    ReferencePointsCoordinates.Add(new Point(XCenter, YCenter));
                }
                else
                {
                    // if the clicked point is not near enough to a reference point,
                    // it is just added to the list of reference points
                    ReferencePointsCoordinates.Add(new Point(XCenter, YCenter));
                }
            }
            else
            {
                // we are not editing 
                // calculate the center of the circle
                XCenter = (float)LeftTopPosition.X + injectionPointsRadius;
                YCenter = (float)LeftTopPosition.Y + injectionPointsRadius;
                // since the click has been done while setting the point of the injection
                // the center is brought to the nearestPoint reference point
                nearestPoint = FindNearest(new Point(XCenter, YCenter), ReferencePointsCoordinates);
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
                // the center is added to the list
                InjectionPointsCoordinates.Add(new Point(XCenter, YCenter));
            }
            return nearestPoint;
        }
        internal void RemovePointIfNear(Point LeftTopPosition)
        {
            // center position is stored in the right list depending on the IsEditing flag
            double XCenter;
            double YCenter;
            // calculate the center of the circle
            XCenter = (float)LeftTopPosition.X + referencePointsRadius;
            YCenter = (float)LeftTopPosition.Y + referencePointsRadius;

            // check if the clicked point is near enough to a reference point
            Point nearestPoint = FindNearest(new Point(XCenter, YCenter), ReferencePointsCoordinates);
            // calculate the cartesion distance between the clicked point and the nearestPoint reference point
            double distance = Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
            // if the distance is less than nearEnoughDistance, the clicked point is deleted
            if (distance < nearEnoughDistance)
            {
                // if the clicked point is near enough to a point, the reference point is removed
                ReferencePointsCoordinates.Remove(nearestPoint);
            }
        }
        private Point FindNearest(Point Passed, List<Point> GivenPoints)
        {
            Point min = new Point(double.MaxValue, double.MaxValue);
            // find the point in the list that is the nearestPoint to the passed point
            {
                double SquaredDistanceMin = float.MaxValue;
                foreach (Point point in GivenPoints)
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
        internal void SaveReferenceCoordinates(Common.ZoneOfPosition ZoneOfPositions
            , double imgWidth, double imgHeight)
        {
            if (isCallerEditing)
                bl.SaveNewReferenceCoordinates(ReferencePointsCoordinates, ZoneOfPositions
                    , imgWidth, imgHeight);
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
            foreach (Injection injection in allRecentInjections)
            {
                if (injection.PositionX != null && injection.PositionY != null)
                {
                    // Moltiplica per la larghezza e altezza dell'immagine per riportare a coordinate reali
                    // a partire dalle coordinate normalizzate fra 0 e 1 che sono memorizzate nel database
                    InjectionPointsCoordinates.Add(new Point(
                        (float)(injection.PositionX * imageWidth),
                        (float)(injection.PositionY * imageHeight)
                    ));
                }
            }
        }
        internal void LoadReferenceCoordinates(List<PositionOfInjection> allReferencePositions)
        {
            foreach (PositionOfInjection injection in allReferencePositions)
            {
                if (injection.PositionX != null & injection.PositionY != null)
                    ReferencePointsCoordinates.Add(new Point(
                        (float)(injection.PositionX * imageWidth),
                        (float)(injection.PositionY * imageHeight)
                    ));
            }
        }
    }
}
