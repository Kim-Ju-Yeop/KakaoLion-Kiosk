using KakaoLion.dto.model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace KakaoLion.converter
{
    class CategoryToName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Category.SMALL:
                    return "소형인형";

                case Category.MEDIUM:
                    return "중형인형";

                case Category.BIG:
                    return "대형인형";
                default:
                    return "";
            } 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
