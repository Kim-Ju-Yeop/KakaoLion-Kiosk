using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KakaoLion.converter
{
    public class BooleanToVisibillity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool possible = bool.Parse(value.ToString());

            return possible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
