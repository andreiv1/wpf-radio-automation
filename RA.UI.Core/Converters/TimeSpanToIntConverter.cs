using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RA.UI.Core.Converters
{
    public class TimeSpanToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not null)
            {
                return ((TimeSpan)value).Hours;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not null) {
                return TimeSpan.FromHours((int)value);
            }
            return TimeSpan.Zero;
        }
    }
}
