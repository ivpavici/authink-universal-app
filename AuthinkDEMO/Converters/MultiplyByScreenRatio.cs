using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AuthinkDEMO.Converters
{
    public class MultiplyByScreenRatio : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, string language)
        {
            if(!(value is double)) { throw new ArgumentException("Expected double", "value"); }

            var frame = Window.Current.Content as Frame;
            if  (frame != null)
            {
                var currentPage = frame.Content as Page;
                if (currentPage != null)
                {
                    return (double)value * currentPage.ActualHeight / currentPage.ActualWidth;
                }
            } 

            return (double)value * Window.Current.Bounds.Height / Window.Current.Bounds.Width;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            throw new System.NotSupportedException();
        }
    }
}
