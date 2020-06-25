using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace CollectionViewExample
{
    [ValueConversion(typeof(string), typeof(string))]
    public class SomeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var text = ((string)value);

            return text.ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
