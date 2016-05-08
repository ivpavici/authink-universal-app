using Windows.UI.Xaml.Data;

namespace AuthinkDEMO.Converters
{
    public class EnableButtonConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            return value != null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            throw new System.NotImplementedException();
        }
    }
}
