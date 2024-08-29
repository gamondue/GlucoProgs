using gamon;
using static GlucoMan.Common;

namespace GlucoMan
{
    public class FoodInMeal
    {
        public int? IdFoodInMeal { get; set; }
        public int? IdFood { get; set; }
        public int? IdMeal { get; set; }
        public DoubleAndText QuantityGrams { get; set; }// [g/100]
        public DoubleAndText ChoGrams { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText CarbohydratesPercent { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText AccuracyOfChoEstimate { get; set; } // [0..1]
        public DoubleAndText SugarPercent { get; set; }        // [g/100]
        public DoubleAndText FibersPercent { get; set; }       // [g/100]
        public string Name { get; set; }

        public string Description;

        public FoodInMeal()
        {
            QuantityGrams = new DoubleAndText(); // [g]
            ChoGrams = new DoubleAndText();   // [g]
            CarbohydratesPercent = new DoubleAndText(); // [%]
            SugarPercent = new DoubleAndText();         // [%]
            FibersPercent = new DoubleAndText();        // [%]
            AccuracyOfChoEstimate = new DoubleAndText(); 
            ChoGrams.Format = "0.0";
            QuantityGrams.Format = "0.0";
            CarbohydratesPercent.Format = "0.0";
        }
        public FoodInMeal DeepCopy()
        {
            FoodInMeal c = (FoodInMeal)this.MemberwiseClone();
            if (Name != null)
                c.Name = string.Copy(Name);
            if (Description != null)
                c.Description = string.Copy(Description);
            return c;
        }
        public bool DeepEquals(FoodInMeal Other, out FoodInMeal Differences)
        {
            Differences = new FoodInMeal(); 
            bool areEqual = true;
            // Id is included in equality! 
            if (this.IdFoodInMeal != Other.IdFoodInMeal)
            {
                areEqual = false;
                Differences.IdFoodInMeal = Other.IdFoodInMeal;
            }
            else if (this.IdFood != Other.IdFood)
            {
                areEqual = false;
                Differences.IdFood = Other.IdFood;
            }
            else if (this.IdMeal != Other.IdMeal)
            {
                areEqual = false;
                Differences.IdMeal = Other.IdMeal;
            }
            else if (this.QuantityGrams.Double != Other.QuantityGrams.Double)
            {
                areEqual = false;
                Differences.QuantityGrams.Double = Other.QuantityGrams.Double;
            }
            else if (this.ChoGrams.Double != Other.ChoGrams.Double)
            {
                areEqual = false;
                Differences.ChoGrams.Double = Other.ChoGrams.Double;
            }
            else if (this.CarbohydratesPercent.Double != Other.CarbohydratesPercent.Double)
            {
                areEqual = false;
                Differences.CarbohydratesPercent.Double = Other.CarbohydratesPercent.Double;
            }
            else if (this.AccuracyOfChoEstimate.Double != Other.AccuracyOfChoEstimate.Double)
            {
                areEqual = false;
                Differences.AccuracyOfChoEstimate.Double = Other.AccuracyOfChoEstimate.Double;
            }
            if (this.SugarPercent.Double != Other.SugarPercent.Double)
            {
                areEqual = false;
                Differences.SugarPercent.Double = Other.SugarPercent.Double;
            }
            if (this.FibersPercent.Double != Other.FibersPercent.Double)
            {
                areEqual = false;
                Differences.FibersPercent.Double = Other.FibersPercent.Double;
            }
            if (this.Name != Other.Name)
            {
                areEqual = false;
                Differences.Name = Other.Name;
            }
            if (this.Description != Other.Description)
            {
                areEqual = false;
                Differences.Description = Other.Description;
            }
            return areEqual;
        }
    }
}
