using System;
using System.ComponentModel;
using System.Collections;

namespace Homie.Common.WPF
{
    public abstract class ListViewCustomComparer : IComparer
    {
        public ListSortDirection SortDirection { get; set; }

        public string SortColumn { get; set; }

        public abstract int Compare(object pItemA, object pItemB);

        public int StringCompare(string pValueA, string pValueB)
        {
            if (SortDirection.Equals(ListSortDirection.Ascending))
            {
                return String.CompareOrdinal(pValueA, pValueB);
            }
            return (-1) * String.CompareOrdinal(pValueA, pValueB);
        }

        public int IntegerCompare(int pValueA, int pValueB)
        {
            if (SortDirection.Equals(ListSortDirection.Ascending))
            {
                return pValueA.CompareTo(pValueB);
            }
            return pValueB.CompareTo(pValueA);
        }

        public int DateCompare(DateTime pValueA, DateTime pValueB)
        {
            if (SortDirection.Equals(ListSortDirection.Ascending))
            {
                return DateTime.Compare(pValueA, pValueB);
            }
            return (-1) * DateTime.Compare(pValueA, pValueB);
        }
    }
}
