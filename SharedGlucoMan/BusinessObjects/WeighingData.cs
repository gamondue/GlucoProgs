using gamon;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GlucoMan
{
    public class WeighingData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _foodId;
        public string FoodId 
        { 
            get => _foodId;
            set
            {
                if (_foodId != value)
                {
                    _foodId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _foodName;
        public string FoodName 
        { 
            get => _foodName;
            set
            {
                if (_foodName != value)
                {
                    _foodName = value;
                    OnPropertyChanged();
                }
            }
        }

        public DoubleAndText FoodCarbohydratesPercent { get; set; } = new DoubleAndText();
        public DoubleAndText TotalCarbohydratesPercent { get; set; } = new DoubleAndText();

        /// <summary>
        /// Weights for raw food (before cooking)
        /// </summary>
        public WeightsForWeighing Raw { get; set; } = new WeightsForWeighing();
        
        /// <summary>
        /// Weights for cooked food (after cooking, whole quantity)
        /// </summary>
        public WeightsForWeighing Cooked { get; set; } = new WeightsForWeighing();
        
        /// <summary>
        /// Weights for seasoning/condiment
        /// </summary>
        public WeightsForWeighing Seasoning { get; set; } = new WeightsForWeighing();
        
        /// <summary>
        /// Weights for the cooked portion
        /// </summary>
        public WeightsForWeighing Portion { get; set; } = new WeightsForWeighing();
        
        public IntAndText NPortions { get; set; } = new IntAndText();
        public DoubleAndText RawCookedRatio { get; set; } = new DoubleAndText();
        
        private bool _doWeighCookedPortion;
        public bool DoWeighCookedPortion 
        { 
            get => _doWeighCookedPortion;
            set
            {
                if (_doWeighCookedPortion != value)
                {
                    _doWeighCookedPortion = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isChoOfRawFood;
        public bool IsChoOfRawFood 
        { 
            get => _isChoOfRawFood;
            set
            {
                if (_isChoOfRawFood != value)
                {
                    _isChoOfRawFood = value;
                    OnPropertyChanged();
                }
            }
        }

        public DoubleAndText WeightOfPortion { get; set; } = new DoubleAndText();
        public DoubleAndText CarbohydratesOfPortion { get; set; } = new DoubleAndText();

        public WeighingData() {
            RawCookedRatio.Format = "0.000";
        }
    }
}
