using System.Collections;
using System.Collections.Generic;

namespace Homie.Model
{
    public class Machines : IEnumerable<Machine>
    {
        private List<Machine> m_List = new List<Machine>();

        public List<Machine> List
        {
            get { return m_List; }
            set { m_List = value; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public IEnumerator<Machine> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Add(Machine pMachine)
        {
            // Temporarily save list count so we can 
            // check if item was successfully added.
            int lListCount = this.List.Count;
            List.Add(pMachine);
            if (this.List.Count == lListCount + 1)
            {
                return true;
            }
            return false;
        }

        public bool Remove(Machine pMachine)
        {
            foreach (var lMachine in m_List)
            {
                if (lMachine.Equals(pMachine))
                {
                    m_List.Remove(lMachine);
                    return true;
                }
            }
            return false;
        }
    }
}
