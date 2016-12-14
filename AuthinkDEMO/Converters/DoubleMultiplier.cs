using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AuthinkDEMO.Converters
{
    public class DoubleMultiplier : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            if(!(value is double)) { throw new ArgumentException("Expected double", "value"); }

            var multiplier = double.Parse((string)parameter);

            return (double)value * multiplier;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            throw new System.NotSupportedException();
        }
    }
}
