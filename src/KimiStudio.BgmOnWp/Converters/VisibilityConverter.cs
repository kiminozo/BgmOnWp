using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KimiStudio.BgmOnWp.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (!(bool)value) return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }

        #endregion
    }
}