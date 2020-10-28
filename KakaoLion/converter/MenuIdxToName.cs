using KakaoLion.model;
using KakaoLion.pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace KakaoLion.converter
{
    public class MenuIdxToName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            List<MenuModel> menuList = OrderPage.menuList.Where(menu => menu.idx.Equals(value)).ToList();
            return menuList[0].name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
