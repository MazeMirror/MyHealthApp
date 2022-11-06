using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace MyHealthApp.Helpers
{
    public class ValueToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fileName && parameter is String assemblyName)
            {
                try
                {
                    var imageSource = ImageSource.FromResource(assemblyName + "." + fileName, typeof(ValueToImageSourceConverter).GetTypeInfo().Assembly);
                    return imageSource;
                }
                catch (Exception)
                {
                    return value;
                }
            }
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}