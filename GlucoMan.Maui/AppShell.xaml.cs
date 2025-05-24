namespace GlucoMan.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registra le rotte per le pagine
            Routing.RegisterRoute("GlucoseMeasurementsPage", typeof(GlucoseMeasurementsPage));
            Routing.RegisterRoute("MealsPage", typeof(MealsPage));
            Routing.RegisterRoute("MealPage", typeof(MealPage));
            Routing.RegisterRoute("RecipesPage", typeof(RecipesPage));
            Routing.RegisterRoute("InsulinCalcPage", typeof(InsulinCalcPage));
            Routing.RegisterRoute("FoodToHitTargetCarbsPage", typeof(FoodToHitTargetCarbsPage));
            Routing.RegisterRoute("HypoPredictionPage", typeof(HypoPredictionPage));
        }
    }
}
