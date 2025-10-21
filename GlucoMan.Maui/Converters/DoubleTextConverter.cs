using System.Globalization;

namespace GlucoMan.Maui.Converters;

/// <summary>
/// Converter that handles null or "NaN" text values from DoubleAndText
/// </summary>
public class DoubleTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
      return "";
     
        string text = value.ToString();
     
        // Handle NaN, empty, or null strings
        if (string.IsNullOrWhiteSpace(text) || 
            text.Equals("NaN", StringComparison.OrdinalIgnoreCase) ||
            text.Equals("nan", StringComparison.OrdinalIgnoreCase))
        {
  return "";
        }
        
        return text;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
   return value;
    }
}
