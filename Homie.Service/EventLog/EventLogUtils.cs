using System;
using System.Diagnostics;
using Homie.Common.WPF;

namespace Homie.Admin
{
    public class EventLogSorter : ListViewCustomComparer
    {
        public const string cMessageColumnName = "Message";
        public const string cDateTimeColumnName = "TimeGenerated";
        public override int Compare(object pItemA, object pItemB)
        {
            try
            {
                var lItemA = (EventLogEntry)pItemA;
                var lItemB = (EventLogEntry)pItemB;

                switch (SortColumn)
                {
                    //case "CustomerID":
                    //    return IntegerCompare(lItemA.EntryType, lCustomerB.CustomerID);
                    case cMessageColumnName:
                        return StringCompare(lItemA.Message, lItemB.Message);
                    case cDateTimeColumnName:
                        return DateCompare(lItemA.TimeGenerated, lItemB.TimeGenerated);
                }

                return 0;

            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
