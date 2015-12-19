using System;
using System.Globalization;
using System.Windows.Data;

namespace Pyxie
{
    [ValueConversion(typeof(Int16), typeof(String))]
    public class FloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string floatStr = ((float)value).ToString("0.0");
            return floatStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            float resultFloat;
            if (float.TryParse(strValue, out resultFloat))
            {
                return resultFloat;
            }
            return float.NaN;
        }
    }
}
