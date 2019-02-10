using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace CalcDmitriyElisov.Views
{
    public class IsValueMoreThanParameter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double actualWidth = (Double)value;

            if (actualWidth > Int32.Parse((string)parameter))
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

