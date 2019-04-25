using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace OrderForm
{
    /// <summary>
    /// Converts an int for a modifier to a text.
    /// </summary>

    [ValueConversion(typeof(int), typeof(string))]
    class ModifierToTextConverter : IValueConverter
    {
        public static ModifierToTextConverter modifierConverter = new ModifierToTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int modifierAmount = (int)value;
            string returnString = "";
            switch (modifierAmount)
            {
                case 0:
                    returnString = "None";
                    break;
                case 1:
                    returnString = "Normal";
                    break;
                case 2:
                    returnString = "Extra";
                    break;
                case 3:
                    returnString = "Double Extra";
                    break;
            }
            return returnString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
