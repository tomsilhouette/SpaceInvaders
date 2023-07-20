using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SpaceInvaders.Controls
{

    public class CapitalizeFirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string input)
            {
                if (string.IsNullOrEmpty(input))
                    return string.Empty;

                return char.ToUpper(input[0]) + input.Substring(1);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
