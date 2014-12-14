using System;
using System.Collections.Generic;

namespace Homie.Model
{
    public class DummyMachineProvider : IMachineProvider
    {
        private List<Machine> m_Machines = new List<Machine>();
 
        public DummyMachineProvider()
        {
            m_Machines.Add(new Machine("Foo", "00-00-00-00-00-00-00-00"));
            m_Machines.Add(new Machine("Bar", "00-00-00-00-00-00-00-01"));
        }

        public void Add(Machine pMachine)
        {
            throw new NotImplementedException();
        }

        public void Remove(Machine pMachine)
        {
            throw new NotImplementedException();
        }

        public List<Machine> GetMachines()
        {
            return m_Machines;
        }

        public Machine GetMachine(int pMachineID)
        {
            return m_Machines.Find(m => m.MachineID.Equals(pMachineID));
        }
    }
}
