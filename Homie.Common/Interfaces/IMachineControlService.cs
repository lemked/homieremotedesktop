using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Threading.Tasks;
using Homie.Model;

namespace Homie.Common.Interfaces
{
    [ServiceContract]
    public interface IMachineControlService
    {
        [OperationContract]
        Task ConnectToServerAsync();

        /// <summary>
        /// Adds a new machine.
        /// </summary>
        /// <param name="machine">The machine.</param>
        /// <returns>The ID of the new machine.</returns>
        /// <author>Daniel Lemke - lemked@web.de</author>
        [OperationContract]
        Task<int> AddMachineAsync(Machine machine);

        /// <summary>
        /// Updates the given machine.
        /// </summary>
        /// <param name="machine">The machine to update.</param>
        /// <returns></returns>
        /// <author>Daniel Lemke - lemked@web.de</author>
        [OperationContract]
        Task UpdateMachineAsync(Machine machine);

        /// <summary>
        /// Removes the machine with the given ID.
        /// </summary>
        /// <param name="machineID">The ID of the machine to remove.</param>
        /// <returns></returns>
        /// <author>Daniel Lemke - lemked@web.de</author>
        [OperationContract]
        Task RemoveMachineAsync(int machineID);

        [OperationContract]
        Task<IEnumerable<Machine>> GetMachinesAsync();

        [OperationContract]
        Task<Machine> GetMachineAsync(int machine);

        [OperationContract]
        Task StartMachineAsync(Machine pMachine);

        [OperationContract]
        Task<IPStatus> PingAsync(Machine pMachine);

        [OperationContract]
        Task ShutdownAsync(Machine pMachine);
    }
}
