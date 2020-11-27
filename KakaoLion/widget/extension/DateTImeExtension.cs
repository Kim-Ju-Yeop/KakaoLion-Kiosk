using System;

namespace KakaoLion.widget.extension
{
    class DateTImeExtension
    {
        public static string dateTimeFormat(DateTime dateTIme)
        {
            return String.Format("{0:tt HH시 mm분 ss초 dddd}", dateTIme);
        }

        public static string dateTimeFormat2(DateTime dateTime)
        {
            return String.Format("{0:HHmmss}", dateTime);
        }
    }
}
