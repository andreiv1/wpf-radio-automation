using RA.DAL.Models;
using RA.UI.StationManagement.Dialogs.TrackFilterDialog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RA.UI.StationManagement.Converters
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
