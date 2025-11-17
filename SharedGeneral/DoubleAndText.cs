using System;
using System.ComponentModel;

namespace gamon
{
    [Browsable(false)]
    public class DoubleAndText : INotifyPropertyChanged
    {
        private double? doubleVal;
        private string? text;
        private string format;

        public DoubleAndText()
        {
            format = "0.0";
            doubleVal = null;
            text = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Format
        {
            get => format;
            set
            {
                if (format == value) return;
                format = value ?? "0.0";
                // Text representation may change when format changes
                if (doubleVal != null)
                {
                    text = ((double)doubleVal).ToString(format);
                }
                OnPropertyChanged(nameof(Format));
                OnPropertyChanged(nameof(Text));
            }
        }

        public double? Double
        {
            get => doubleVal;
            set
            {
                if (doubleVal == value) return;
                doubleVal = value;
                if (value == null)
                {
                    text = null;
                }
                else
                {
                    try
                    {
                        text = ((double)doubleVal).ToString(format);
                    }
                    catch
                    {
                        text = null;
                    }
                }
                OnPropertyChanged(nameof(Double));
                OnPropertyChanged(nameof(Text));
            }
        }

        public string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                if (string.IsNullOrEmpty(value))
                {
                    doubleVal = null;
                }
                else
                {
                    if (double.TryParse(value, out var parsed))
                    {
                        doubleVal = parsed;
                    }
                    else
                    {
                        doubleVal = null;
                    }
                }
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Double));
            }
        }

        public override string? ToString()
        {
            return Text;
        }
    }
}