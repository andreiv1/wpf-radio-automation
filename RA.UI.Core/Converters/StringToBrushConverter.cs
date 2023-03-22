using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RA.UI.Core
{
    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.White;
            if (!(value is string str)) return Brushes.White;
            try
            {
                var converter = new BrushConverter();
                var brush = (Brush)converter.ConvertFromString(str);
                return brush;
            }
            catch
            {
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.White;
            if (!(value is SolidColorBrush brush)) return Brushes.White;
            return brush.Color.ToString();
        }
    }
}
