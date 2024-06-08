using System;
using System.Collections.Generic;
using System.IO;

namespace GlucoMan.Maui
{
    internal class CirclesDrawable : IDrawable
    {
        private List<(float Left, float Top, bool IsChecked)> circleCoordinates = new List<(float, float, bool)>();
        private List<(float Left, float Top)> checkedCircleCoordinates = new List<(float, float)>();

        internal float LeftCerchio { get; set; } = 0;
        internal float TopCerchio { get; set; } = 0;
        internal bool firstTime = true;

        private string CirclesFilePath = "C:\\Users\\daniele.pieri\\Desktop\\Nuova cartella\\GlucoProgs\\CircleFile.txt";
        private string CheckedCirclesFilePath = "C:\\Users\\daniele.pieri\\Desktop\\Nuova cartella\\GlucoProgs\\CheckedCircleFile.txt";

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (firstTime)
            {
                ReadCoordinatesFromFile();
                ReadCheckedCoordinatesFromFile();
                firstTime = false;
            }

            foreach (var (left, top, isChecked) in circleCoordinates)
            {
                canvas.StrokeColor = isChecked ? Colors.Blue : Colors.Red;
                canvas.StrokeSize = 5;
                canvas.FillColor = Colors.Blue;
                canvas.DrawEllipse(left, top, 5, 5);
            }

            foreach (var (left, top) in checkedCircleCoordinates)
            {
                canvas.StrokeColor = Colors.Blue;
                canvas.StrokeSize = 5;
                canvas.FillColor = Colors.Blue;
                canvas.DrawEllipse(left, top, 5, 5);
            }
        }

        private void ReadCoordinatesFromFile()
        {
            circleCoordinates.Clear();
            try
            {
                if (File.Exists(CirclesFilePath))
                {
                    string[] lines = File.ReadAllLines(CirclesFilePath);
                    foreach (string line in lines)
                    {
                        string[] coordinates = line.Split('\t');
                        if (coordinates.Length == 2 && float.TryParse(coordinates[0], out float x) && float.TryParse(coordinates[1], out float y))
                        {
                            circleCoordinates.Add((x, y, false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore durante la lettura delle coordinate dal file: {ex.Message}");
            }
        }

        private void ReadCheckedCoordinatesFromFile()
        {
            checkedCircleCoordinates.Clear();
            try
            {
                if (File.Exists(CheckedCirclesFilePath))
                {
                    string[] lines = File.ReadAllLines(CheckedCirclesFilePath);
                    foreach (string line in lines)
                    {
                        string[] coordinates = line.Split('\t');
                        if (coordinates.Length == 2 && float.TryParse(coordinates[0], out float x) && float.TryParse(coordinates[1], out float y))
                        {
                            checkedCircleCoordinates.Add((x, y));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore durante la lettura delle coordinate verificate dal file: {ex.Message}");
            }
        }

        internal void AddCircle(float left, float top)
        {
            circleCoordinates.Add((left, top, false));
        }

        internal void CheckSquare(float left, float top)
        {
            float SumDiffMin = float.MaxValue;
            (float leftMin, float topMin) = (0, 0);

            foreach (var (leftList, topList, _) in circleCoordinates)
            {
                float diffLeft = Math.Abs(leftList - left);
                float diffTop = Math.Abs(topList - top);
                float sumOfDiff = diffLeft + diffTop;

                if (sumOfDiff < SumDiffMin)
                {
                    SumDiffMin = sumOfDiff;
                    leftMin = leftList;
                    topMin = topList;
                }
            }

            circleCoordinates.RemoveAll(c => c.Left == leftMin && c.Top == topMin);
            checkedCircleCoordinates.Add((leftMin, topMin));
        }

        internal void SaveCircleToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(CirclesFilePath))
                {
                    foreach (var (left, top, _) in circleCoordinates)
                    {
                        sw.WriteLine($"{left}\t{top}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore durante il salvataggio delle coordinate nel file: {ex.Message}");
            }
        }

        internal void SaveCircleToFileBlu()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(CheckedCirclesFilePath))
                {
                    foreach (var (left, top) in checkedCircleCoordinates)
                    {
                        sw.WriteLine($"{left}\t{top}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore durante il salvataggio delle coordinate verificate nel file: {ex.Message}");
            }
        }

        internal void ClearFile()
        {
            try
            {
                File.WriteAllText(CirclesFilePath, string.Empty);
                File.WriteAllText(CheckedCirclesFilePath, string.Empty);
                circleCoordinates.Clear();
                checkedCircleCoordinates.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Si è verificato un errore durante la pulizia del file: {ex.Message}");
            }
        }
    }
}
