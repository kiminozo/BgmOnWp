using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace KimiStudio.BgmOnWp.Converters
{
    public class StateBrushConverter : IValueConverter
    {
        private static readonly Brush defaultBrush = new SolidColorBrush(Colors.Gray);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value) return Application.Current.Resources["BangumiPink"];
            }

            return defaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
