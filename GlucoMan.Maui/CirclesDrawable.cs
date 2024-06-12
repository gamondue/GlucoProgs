using System;
using System.Collections.Generic;
using System.IO;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui
{
    internal class CirclesDrawable : IDrawable
    {
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
                if (isCallerEditing != value)
                {
                    firstTime = true;
                }
                isCallerEditing = value;
            }
        }
        PointType type; 
        internal PointType Type 
        { get
            {  
                return type; 
            }
            set
            { 
                firstTime = true;
                type = value;
            }
        }
        // the coordinates of the reference points are the centers of the circles that will be added to the graphics
        private List<Point> PointsCoordinates = new();
        // when we aren't editing we also need a collection of reference points  
        private List<Point> ReferencePointsCoordinates = new();

        //private List<(float X, float Y)> InjectionPointsCoordinates = new List<(float, float)>();

        // the distance in pixels that is considered to be near enough to one reference point
        // to be considered the same point
        private double thresholdDistance = 8;
        // the radiuses of the circles for reference points and injection points
        private float referenceRadius = 4;
        private float injectionRadius = 8;

        private float XCenter { get; set; } = 0;
        private float YCenter { get; set; } = 0;

        private bool firstTime = true;

        //private string PointsCoordinatesFilePath = Path.Combine(Common.PathDatabase, "PointsCoordinates");
        private string PointsCoordinatesFilePath = Common.PathDatabase;
        private string ReferencePointsCoordinatesFilePath = Common.PathDatabase;

        public CirclesDrawable()
        {
            firstTime = true;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            string partOfFileName = "";
            if (firstTime)
            {
                PointsCoordinates = ReadCoordinatesFromFile();
                if (!isCallerEditing)
                {
                    ReferencePointsCoordinates = ReadCoordinatesFromReferenceFile(); 
                }
                firstTime = false;
            }
            foreach (Point point in PointsCoordinates)
            {
                if (isCallerEditing)
                {
                    canvas.StrokeColor = Colors.Blue;
                    canvas.StrokeSize = referenceRadius;
                    canvas.FillColor = Colors.Blue;
                    canvas.DrawEllipse((float)(point.X - referenceRadius), (float)(point.Y - referenceRadius),
                        referenceRadius, referenceRadius);
                }
                else
                {
                    canvas.StrokeColor = Colors.Red;
                    canvas.StrokeSize = injectionRadius;
                    canvas.FillColor = Colors.Red;
                    canvas.DrawEllipse((float)(point.X - injectionRadius), (float)(point.Y - injectionRadius),
                         injectionRadius, injectionRadius);
                }
            }
        }
        internal void AddPoint(Point LeftTopPosition, bool IsEditing)
        {
            // center position is stored in the right list depending on the IsEditing flag
            double XCenter;
            double YCenter;
            // calculate the center of the circle

            if (IsEditing)
            {
                XCenter = (float)LeftTopPosition.X + referenceRadius;
                YCenter = (float)LeftTopPosition.Y + referenceRadius;
                // check if the clicked point is near enough to a reference point
                Point nearestPoint = FindNearest(new Point(XCenter, YCenter), PointsCoordinates);
                // calculate the cartesion distance between the clicked point and the nearest reference point
                float distance = (float)Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X) 
                    + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
                // if the distance is less than thresholdDistance, the clicked point is considered
                // to be the same as the nearest reference point
                if (distance < thresholdDistance)
                {
                    // if the clicked point is near enough to a reference point, the reference point is removed
                    PointsCoordinates.Remove(nearestPoint);
                    // the clicked point is added to the list of reference points
                    PointsCoordinates.Add(new Point(XCenter, YCenter));
                }
                else
                {
                    // if the clicked point is not near enough to a reference point,
                    // it is just added to the list of reference points
                    PointsCoordinates.Add(new Point(XCenter, YCenter));
                }
            }
            else
            {
                XCenter = (float)LeftTopPosition.X + injectionRadius;
                YCenter = (float)LeftTopPosition.Y + injectionRadius;
                // if the click has been done while setting the point of the injection,
                // the center is brought to the nearest reference point and is added to the list
                Point nearest = FindNearest(new Point(XCenter, YCenter), ReferencePointsCoordinates);
                if (nearest.X != double.MaxValue && nearest.Y != double.MaxValue)
                {
                    XCenter = nearest.X;
                    YCenter = nearest.Y;
                    PointsCoordinates.Add(new Point(XCenter, YCenter));
                }
            }
        }
        internal void RemovePointIfNear(Point LeftTopPosition)
        {
            // center position is stored in the right list depending on the IsEditing flag
            double XCenter;
            double YCenter;
            // calculate the center of the circle
            XCenter = (float)LeftTopPosition.X + referenceRadius;
            YCenter = (float)LeftTopPosition.Y + referenceRadius;

            // check if the clicked point is near enough to a reference point
            Point nearestPoint = FindNearest(new Point(XCenter, YCenter), PointsCoordinates);
            // calculate the cartesion distance between the clicked point and the nearest reference point
            double distance = Math.Sqrt((XCenter - nearestPoint.X) * (XCenter - nearestPoint.X)
                + (YCenter - nearestPoint.Y) * (YCenter - nearestPoint.Y));
            // if the distance is less than thresholdDistance, the clicked point is deleted
            if (distance < thresholdDistance)
            {
                // if the clicked point is near enough to a point, the reference point is removed
                PointsCoordinates.Remove(nearestPoint);
            }
        }
        private Point FindNearest(Point Passed, List<Point> GivenPoints)
        {
            Point min =  new Point(double.MaxValue, double.MaxValue);
            // find the point in the list that is the nearest to the passed point
            {
                double SquaredDistanceMin = float.MaxValue;
                foreach (Point point in GivenPoints)
                {
                    double distanceX = (float) (point.X - Passed.X);
                    double distanceY = (float) (point.Y - Passed.Y);
                    double squaredDistance = distanceX * distanceX + distanceY * distanceY;
                    if (squaredDistance < SquaredDistanceMin)
                    {
                        SquaredDistanceMin = squaredDistance;
                        min.X = point.X;
                        min.Y = point.Y;
                    }
                }
                // when the loop ends, XMin and YMin are the coordinates of the nearest point
                // we give them back to the caller
                return min;
            }
        }
        private List<Point> ReadCoordinatesFromFile()
        {
            string partOfFileName;
            if (isCallerEditing)
                partOfFileName = Type.ToString() + "Reference";
            else
                partOfFileName = Type.ToString() + "Injection";
            string fileNameAndPath = Path.Combine(PointsCoordinatesFilePath, partOfFileName + "Coordinates.tsv");

            List<Point> coordinates = new();
            coordinates.Clear();
            try
            {
                if (File.Exists(fileNameAndPath))
                {
                    string[] lines = File.ReadAllLines(fileNameAndPath);
                    foreach (string line in lines)
                    {
                        string[] pointCoordinates = line.Split('\t');
                        if (pointCoordinates.Length == 2 && float.TryParse(pointCoordinates[0], out float x)
                            && float.TryParse(pointCoordinates[1], out float y))
                        {
                            coordinates.Add(new Point(x, y));
                        }
                    }
                }
                return coordinates;
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Si è verificato un errore durante la lettura delle coordinate dal file: {ex.Message}");
                Console.WriteLine($"An error occurred while reading the coordinates from file: {ex.Message}");
                return null;
            }
        }
        private List<Point> ReadCoordinatesFromReferenceFile()
        {
            string partOfFileName = Type.ToString() + "Reference";
            string fileNameAndPath = Path.Combine(PointsCoordinatesFilePath, partOfFileName + "Coordinates.tsv");

            List<Point> coordinates = new();
            coordinates.Clear();
            try
            {
                if (File.Exists(fileNameAndPath))
                {
                    string[] lines = File.ReadAllLines(fileNameAndPath);
                    foreach (string line in lines)
                    {
                        string[] pointCoordinates = line.Split('\t');
                        if (pointCoordinates.Length == 2 && float.TryParse(pointCoordinates[0], out float x)
                            && float.TryParse(pointCoordinates[1], out float y))
                        {
                            coordinates.Add(new Point(x, y));
                        }
                    }
                }
                return coordinates;
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Si è verificato un errore durante la lettura delle coordinate dal file: {ex.Message}");
                Console.WriteLine($"An error occurred while reading the coordinates from file: {ex.Message}");
                return null;
            }
        }
        internal void SaveCoordinatesToFile()
        {
            string partOfFileName;
            if (isCallerEditing)
                partOfFileName = Type.ToString() + "Reference";
            else
                partOfFileName = Type.ToString() + "Injection";
            string fileNameAndPath = Path.Combine(PointsCoordinatesFilePath, partOfFileName + "Coordinates.tsv");
            try
            {
                using (StreamWriter sw = new StreamWriter(fileNameAndPath))
                {
                    foreach (var (left, top) in PointsCoordinates)
                    {
                        sw.WriteLine($"{left}\t{top}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the coordinates into file: {ex.Message}");
            }
        }
        internal void ClearFile()
        {
            string partOfFileName;
            if (isCallerEditing)
                partOfFileName = Type.ToString() + "Reference";
            else
                partOfFileName = Type.ToString() + "Injection";
            string fileNameAndPath = Path.Combine(PointsCoordinatesFilePath, partOfFileName + "Coordinates.tsv");
            try
            {
                File.Delete(fileNameAndPath);
                PointsCoordinates.Clear();
            }
            catch (Exception ex)
            {
                // Console.WriteLine($"Si è verificato un errore durante la pulizia del file: {ex.Message}");
                Console.WriteLine($"An error occurred while clearing the coordinates from file: {ex.Message}");
            }
        }
    }
}
