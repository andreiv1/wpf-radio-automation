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
    public class HexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string hexColor && !string.IsNullOrWhiteSpace(hexColor))
            {
                try
                {
                    // Remove leading '#' if present
                    if (hexColor[0] == '#')
                        hexColor = hexColor.Substring(1);

                    // Parse the hex value and create the Color object
                    byte a = 255;
                    byte r = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
                    byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
                    byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);

                    return System.Windows.Media.Color.FromArgb(a, r, g, b);
                }
                catch (Exception)
                {
                    // Error occurred while converting hex to color, return a default color
                }
            }

            // Return a default color if conversion fails
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                // Convert Color object to hexadecimal string representation
                return "#" + color.ToString().Substring(3);
            }

            return null;
        }
    }
}
