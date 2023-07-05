using RA.UI.StationManagement.Dialogs.TrackFilterDialog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RA.UI.StationManagement.Converters
{
    public class FilterControlTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FilterControlType controlType && parameter is string)
            {
                var enumString = parameter.ToString();
                if (enumString != null)
                {
                    FilterControlType enumparameter = (FilterControlType)Enum.Parse(typeof(FilterControlType), enumString);
                    if (controlType == enumparameter)
                    {
                        return Visibility.Visible;
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
