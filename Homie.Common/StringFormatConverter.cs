using System;
using System.Globalization;
using System.Windows.Data;

namespace Homie.Common
{
    public sealed class StringFormatConverter : IValueConverter
    {
        private static readonly StringFormatConverter m_Instance = new StringFormatConverter();
        public static StringFormatConverter Instance
        {
            get
            {
                return m_Instance;
            }
        }

        private StringFormatConverter()
        {
        }

        public object Convert(object pValue, Type pTargetType, object pArameter, CultureInfo pCulture)
        {
            return string.Format(pCulture, (string)pArameter, pValue);
        }

        public object ConvertBack(object pValue, Type pTargetType, object pArameter, CultureInfo pCulture)
        {
            throw new NotSupportedException();
        }
    }
}
