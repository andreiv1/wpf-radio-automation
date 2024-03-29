﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RA.UI.Core
{
    public class TimeSpanToIntSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not null)
            {
                return (int)((TimeSpan)value).TotalSeconds;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int seconds)
            {
                return TimeSpan.FromSeconds(seconds);
            }
            return null;
        }
    }
}
