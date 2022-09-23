using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyHealthApp.Helpers
{
    public class ValueToDoubleConverter : IValueConverter
    {
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value;
        }
    }
}