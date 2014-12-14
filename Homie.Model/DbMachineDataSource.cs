using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Homie.Model
{
    public class DbMachineDataSource : IMachineDataSource
    {
        private readonly DatabaseContext databaseContext = new DatabaseContext();

        public bool Exists(Machine pMachine)
        {
            return databaseContext.Exists<Machine>(pMachine.MachineID);
        }

        public IEnumerable<Machine> GetAllMachines()
        {
            return databaseContext.Machines.ToList();
        }

        public void Add(Machine pMachine)
        {
            databaseContext.Machines.Add(pMachine);
            databaseContext.SaveChanges();
        }

        public void Remove(Machine pMachine)
        {
            databaseContext.Machines.Remove(pMachine);
            databaseContext.SaveChanges();
        }

        public void Update(Machine machine)
        {
            var existingMachine = databaseContext.Machines.Find(machine.MachineID);
            databaseContext.Detach(existingMachine);

            databaseContext.Entry(machine).State = EntityState.Modified;
            databaseContext.SaveChanges();
        }

        public Machine GetMachineByID(int pMachineID)
        {
            return databaseContext.Machines.SingleOrDefault(pItem => pItem.MachineID.Equals(pMachineID));
        }
    }
}
