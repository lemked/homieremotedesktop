using System.Collections.Generic;

namespace Homie.Model
{
    public interface IMachineDataSource
    {
        void Add(Machine pMachine);

        void Remove(Machine pMachine);

        void Update(Machine machine);
        
        Machine GetMachineByID(int pMachineID);

        bool Exists(Machine pMachine);

        IEnumerable<Machine> GetAllMachines();
    }
}
