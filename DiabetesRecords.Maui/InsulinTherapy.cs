using gamon;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiabetesRecords
{
    internal class InsulinTherapy
    {
        private string TherapyFileName = "DiabetesRecords_Therapy.txt";

        internal Dictionary<int, InsulinBase> InsulinBases { get; set; }
        internal List<GlucoseRangeForInsulin> Ranges { get; set; }
        internal bool GetTherapy()
        {
            // get therapy fron the program itself 
            InsulinBases = new Dictionary<int, InsulinBase>();

            // the code of the meal is the key to recover the value of base insulin for tha meal  
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Breakfast, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Breakfast, 4, "Breakfast"));
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Lunch,
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Lunch, 5, "Lunch"));
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Dinner, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Dinner, 5, "Dinner"));
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Snack, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Snack, 10, "------"));

            Ranges = new List<GlucoseRangeForInsulin>();
            Ranges.Add(new GlucoseRangeForInsulin(0, -1, 80));
            Ranges.Add(new GlucoseRangeForInsulin(1, 0, 150));
            Ranges.Add(new GlucoseRangeForInsulin(2, 1, 200));
            Ranges.Add(new GlucoseRangeForInsulin(3, 2, 250));
            Ranges.Add(new GlucoseRangeForInsulin(4, 3, 300));
            Ranges.Add(new GlucoseRangeForInsulin(5, 4, 350));
            Ranges.Add(new GlucoseRangeForInsulin(6, 5, 400));
 
            // tries to get the terapy from an outside file 
            string PathAndFileTherapy = Path.Combine(BusinessLayer.PathExternalPublic, TherapyFileName); 
            if (File.Exists(PathAndFileTherapy))
            { 
                try
                {
                    string[] fileContent = File.ReadAllLines(PathAndFileTherapy);
                    string line= fileContent[0];
                    int separatingLine = 0;
                    InsulinBases = new Dictionary<int, InsulinBase>();
                    for (int i = 1; i < fileContent.Length; i++)
                    {
                        line = fileContent[i];
                        if (line.Substring(0, 5) != "Range")
                        {
                            separatingLine = i;
                            break;
                        }
                        string[] fields = line.Split(',');
                        InsulinBases.Add(int.Parse(fields[0]), 
                            new InsulinBase(int.Parse(fields[0]), int.Parse(fields[1]), fields[2]));
                    }
                    Ranges = new List<GlucoseRangeForInsulin>();
                    for (int i = separatingLine + 1; i < fileContent.Length; i++)
                    {
                        line = fileContent[i];
                        string[] fields = line.Split(',');
                        Ranges.Add(new GlucoseRangeForInsulin(
                            int.Parse(fields[0]), int.Parse(fields[1]), int.Parse(fields[1])));
                    }
                    return true; 
                }
                catch (Exception ex)
                {
                    General.Log.Error("Therapy file wrong", ex);
                    return false; 
                }
            }
            return true; // !!!! when regularly using a therapy file (it should be normal!)
            // in this point we should return false !!!!
        }
        internal double CalcInsulinHint(double Glucose, int TypeOfInsulin, int TypeOfMeal)
        {
            if (TypeOfInsulin == (int)BusinessLayer.TypeOfInsulinSpeed.NotSet)
                return 0;
            // if we have a slow acting insulin, we give the insulin of "night"
            // (that is stored in InsulinBase[] and retrieved with the TypeOfMeal code of a Snack 
            if (TypeOfInsulin == (int)BusinessLayer.TypeOfInsulinSpeed.SlowAction)
                return InsulinBases[(int)BusinessLayer.TypeOfMeal.Snack].BaseValue;
            // if we aren't going to eat, we do not make any insulin 
            if (TypeOfMeal == (int)BusinessLayer.TypeOfMeal.NotSet
                || TypeOfMeal == (int)BusinessLayer.TypeOfMeal.Snack)
                return 0;
            // if we are going to eat, we make the base insulin of the meal (InsulinBases[TypeOfMeal].BaseValue)
            // plus the correction for blood glucose (r.InsulinDelta)
            foreach (GlucoseRangeForInsulin r in Ranges)
            {
                if (Glucose <= r.GlucoseSup)
                    return InsulinBases[TypeOfMeal].BaseValue + r.InsulinDelta;
            }
            // value over the defined ranges
            return InsulinBases[TypeOfMeal].BaseValue +
                    Ranges[Ranges.Count - 1].InsulinDelta;
        }
    }
}