using System;
using System.Globalization;
using MobileLibrary.Extension;
using Xamarin.Forms;

namespace Event.Converters
{
    public class BadgeCountToTextConverter : IValueConverter
    {
        #region IValueConverter Реализаторы интерфейса

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.AsString().AsInt(-1000);

            return val <= 0 ? string.Empty : "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}