using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace RGR_TIMP_4_sem.ViewModels
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Brushes.Red : Brushes.Transparent; // или любой другой цвет по умолчанию
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
