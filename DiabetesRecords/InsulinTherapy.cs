using System;
using System.Collections.Generic;

namespace DiabetesRecords
{
    internal class InsulinTherapy
    {
        internal Dictionary<int, InsulinBase> InsulinBases { get; set; }
        internal List<GlucoseRangeForInsulin> Ranges { get; set; }
        internal void GetTherapy()
        {
            InsulinBases = new Dictionary<int, InsulinBase>();

            // the code of the meal is the key to recover the value of base insulin for tha meal  
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Breakfast, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Breakfast, 4, "Breakfast"));
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Dinner, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Dinner, 5, "Dinner"));
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Lunch, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Lunch, 5, "Lunch"));
            InsulinBases.Add((int)BusinessLayer.TypeOfMeal.Snack, 
                new InsulinBase((int)BusinessLayer.TypeOfMeal.Snack, 10, "Night"));

            Ranges = new List<GlucoseRangeForInsulin>();
            Ranges.Add(new GlucoseRangeForInsulin(0, -1, 80));
            Ranges.Add(new GlucoseRangeForInsulin(1, 0, 150));
            Ranges.Add(new GlucoseRangeForInsulin(2, 1, 200));
            Ranges.Add(new GlucoseRangeForInsulin(3, 2, 250));
            Ranges.Add(new GlucoseRangeForInsulin(4, 3, 300));
            Ranges.Add(new GlucoseRangeForInsulin(5, 4, 350));
            Ranges.Add(new GlucoseRangeForInsulin(6, 5, 400));
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