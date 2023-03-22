using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace RA.UI.Core
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInverted = parameter != null && parameter.ToString().Equals("Inverted", StringComparison.OrdinalIgnoreCase);

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return isInverted ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return isInverted ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
