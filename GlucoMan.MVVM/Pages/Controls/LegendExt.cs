using Syncfusion.Maui.Toolkit.Charts;

namespace GlucoMan.MVVM.Pages.Controls
{
    public class LegendExt : ChartLegend
    {
        protected override double GetMaximumSizeCoefficient()
        {
            return 0.5;
        }
    }
}
