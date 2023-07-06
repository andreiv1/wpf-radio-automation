using RA.DAL.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RA.UI.Playout.Converters
{
    public class OperatorToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FilterOperator)
            {
                switch (value)
                {
                    case  FilterOperator.Equals:
                        return "=";
                    case  FilterOperator.GreaterThan:
                        return ">";
                    case FilterOperator.LessThan: 
                        return "<";
                    case FilterOperator.Like:
                        return "LIKE";
                    default:
                        return string.Empty;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
