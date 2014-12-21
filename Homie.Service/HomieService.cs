using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Threading.Tasks;

using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Common.Logging;

using Homie.Model;
using Homie.Model.Logging;

namespace Homie.Service
{
    using System.ServiceProcess;

#if DEBUG
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)] 
#endif
    public class HomieService : IMachineControlService, IServiceLogProvider
    {
        private readonly IServiceLogProvider serviceLogProvider;
        private readonly IMachineControlService machineControlService;

        public HomieService() : this(DependencyInjector.Resolve<IMachineControlService>(), DependencyInjector.Resolve<IServiceLogProvider>())
        {
            
        }

        public HomieService(IMachineControlService machineControlService, IServiceLogProvider serviceLogProvider)
        {
            this.machineControlService = machineControlService;
            this.serviceLogProvider = serviceLogProvider;
        }

        # region IWebServiceSession implementation
        public async Task ConnectAsync()
        {
            await Task.Factory.StartNew(() => Log.Debug("Client connected.") );
        }

        public bool Authenticate(string user, string pPassword)
        {
            // authentication stuff
            return true; // for testing purposes let it always be true
        }

        #endregion

        # region IServiceLogProvider implementation
        public Task<IEnumerable<LogMessage>> GetLogEntriesAsync()
        {
            return serviceLogProvider.GetLogEntriesAsync();
        }

        #endregion

        #region IMachineControlService implementation

        public async Task StartMachineAsync(Machine pMachine)
        {
            await machineControlService.StartMachineAsync(pMachine);
        }

        public async Task<IPStatus> PingAsync(Machine pMachine)
        {
            return await machineControlService.PingAsync(pMachine);
        }

        public async Task<int> AddMachineAsync(Machine machine)
        {
            return await machineControlService.AddMachineAsync(machine);
        }

        public async Task UpdateMachineAsync(Machine machine)
        {
            await machineControlService.UpdateMachineAsync(machine);
        }

        public async Task RemoveMachineAsync(int machineID)
        {
            await machineControlService.RemoveMachineAsync(machineID);
        }
        public async Task<IEnumerable<Machine>> GetMachinesAsync()
        {
            return await machineControlService.GetMachinesAsync();
        }

        public async Task<Machine> GetMachineAsync(int pMachineID)
        {
            return await machineControlService.GetMachineAsync(pMachineID);
        }

        public async Task ShutdownAsync(Machine machine)
        {
            await machineControlService.ShutdownAsync(machine);
        }

        #endregion
    }
}