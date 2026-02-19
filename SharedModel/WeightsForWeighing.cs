using gamon;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GlucoMan
{
    /// <summary>
    /// Weights used for weighing: Gross, Tare, Net
    /// </summary>
    public class WeightsForWeighing : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DoubleAndText Gross { get; set; } = new DoubleAndText();
        public DoubleAndText Tare { get; set; } = new DoubleAndText();
        public DoubleAndText Net { get; set; } = new DoubleAndText();
        public DoubleAndText CarbohydratesPercent { get; set; } = new DoubleAndText();

        public WeightsForWeighing() { 

        }
    }
}
