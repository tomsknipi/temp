using System;
using System.Globalization;
using MobileLibrary.Extension;
using Xamarin.Forms;

namespace Event.Converters
{
    public class IsSectionReportsToColorConverter : IValueConverter
    {
        #region IValueConverter Реализаторы интерфейса

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.AsBool() ? App.ColorAccent : App.ColorPrimaryText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}