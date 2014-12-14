using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;

namespace ListViewSort
{
    public abstract class ListViewCustomComparer: IComparer
    {
        protected Dictionary<string, ListSortDirection> SortColumns = new Dictionary<string, ListSortDirection>();
        
        public void AddSort(string pSortColumn, ListSortDirection pDir)
        {
            if (SortColumns.ContainsKey(pSortColumn))
            {
                SortColumns.Remove(pSortColumn);           
            }
           SortColumns.Add(pSortColumn, pDir);
        }

        public void ClearSort()
        {
            SortColumns.Clear();
        }

        protected List<string> GetSortColumnList()
        {
            List<string> lResult = new List<string>();
            Stack<string> lTemp = new Stack<string>();

            foreach (string lColumn in SortColumns.Keys)
            {
                lTemp.Push(lColumn);
            }

            while (lTemp.Count > 0)
            {
                lResult.Add(lTemp.Pop());
            }

            return lResult;
        }
               
        public abstract int Compare(object pObject1, object pObject2);
    }
       
}
