using System;
using System.Globalization;
using System.Windows.Data;

namespace Homie.Common
{
    public class LogMessageTypeConverter : IValueConverter
    {
        private static readonly LogMessageTypeConverter instance = new LogMessageTypeConverter();
        public static LogMessageTypeConverter Instance
        {
            get
            {
                return instance;
            }
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
