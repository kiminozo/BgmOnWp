using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace KimiStudio.BgmOnWp.Converters
{
    public class StateBrushConverter :DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty OnBrushProperty =
            DependencyProperty.Register("OnBrush", typeof (Brush), typeof (StateBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public Brush OnBrush
        {
            get { return (Brush)GetValue(OnBrushProperty); }
            set { SetValue(OnBrushProperty, value); }
        }

        public static readonly DependencyProperty OffBrushProperty =
            DependencyProperty.Register("OffBrush", typeof (Brush), typeof (StateBrushConverter),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush OffBrush
        {
            get { return (Brush) GetValue(OffBrushProperty); }
            set { SetValue(OffBrushProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value) return OnBrush;// Application.Current.Resources["BangumiPink"];
            }

            return OffBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
