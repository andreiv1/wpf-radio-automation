using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RA.UI.Core.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (!targetType.IsEnum)
                throw new ArgumentException("TargetType must be an enum type.");
            try
            {
                return Enum.Parse(targetType, value.ToString());
            }
            catch (ArgumentException)
            {
                // Handle invalid input string
                return DependencyProperty.UnsetValue;
            }
        }
    }

}
