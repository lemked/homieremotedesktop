using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Homie.Common.WPF
{
    public class WidthConverter : IValueConverter
    {

        public object Convert(object pObject, Type pType, object pArameter, CultureInfo pCulture)
        {

            ListView lListView = pObject as ListView;
            if (lListView == null) return null;

            GridView lGridView = lListView.View as GridView;
            if (lGridView == null) return null;


            double lTotal = 0;

            for (int lIndex = 0; lIndex < lGridView.Columns.Count - 1; lIndex++)
            {
                lTotal += lGridView.Columns[lIndex].ActualWidth;
            }

            return (lListView.ActualWidth - lTotal);
        }

        public object ConvertBack(object pObject, Type pType, object pArameter, CultureInfo pCulture)
        {

            throw new NotSupportedException();
        }
    }
}
