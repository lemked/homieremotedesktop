using System.Collections.Generic;

namespace Homie.Model
{
    public interface IMachineProvider
    {
        void Add(Machine pMachine);

        void Remove(Machine pMachine);

        List<Machine> GetMachines();

        Machine GetMachine(int pMachineID);
    }
}
