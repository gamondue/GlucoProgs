namespace GlucoMan
{
    /// <summary>
    /// Represents a food item retrieved from the FatSecret API
    /// </summary>
    public class FatSecretFood
    {
        public long FoodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string FoodType { get; set; }        // "Brand" or "Generic"
        public string Category { get; set; }        // food_sub_categories (e.g. "Dairy Products")
        public string FoodUrl { get; set; }
        
        // Nutritional values per 100g
        public double? Calories { get; set; }
        public double? CarbohydratesPercent { get; set; }
        public double? ProteinsPercent { get; set; }
        public double? TotalFatsPercent { get; set; }
        public double? SaturatedFatsPercent { get; set; }
        public double? FibersPercent { get; set; }
        public double? SugarPercent { get; set; }
        public double? SodiumPercent { get; set; }
        
        // Serving information
        public string ServingDescription { get; set; }
        public double? ServingSize { get; set; }
        public string ServingUnit { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Description}";
        }
    }
}
