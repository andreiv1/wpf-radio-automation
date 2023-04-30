using RA.Database.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RA.UI.StationManagement.Converters
{
    public class PlannedScheduleFrequencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlannedScheduleFrequency frequency)
            {
                switch (frequency)
                {
                    case PlannedScheduleFrequency.EveryWeek:
                        return "Every week";
                    case PlannedScheduleFrequency.EveryTwoWeeks:
                        return "Every two weeks";
                    default:
                        return frequency.ToString();
                }
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
