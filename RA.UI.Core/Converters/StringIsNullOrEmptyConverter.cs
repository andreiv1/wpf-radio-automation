using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RA.UI.Core
{
    public class StringIsNullOrEmptyConverter : IValueConverter
    {
        public static StringIsNullOrEmptyConverter Instance = new StringIsNullOrEmptyConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? stringValue = value as string;
            if (stringValue == null) return false;
            return string.IsNullOrEmpty(stringValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
