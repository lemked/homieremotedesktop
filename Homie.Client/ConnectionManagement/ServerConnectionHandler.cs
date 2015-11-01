using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Homie.Common.Interfaces;
using Homie.Model;

using ResourceStrings = Homie.Resources.Properties.Resources;

namespace Homie.Client.ConnectionManagement
{
    public class ServerConnectionHandler : IServerConnectionHandler
    {
        private readonly IMachineControlService service;

        public event EventHandler ConnectionInitiated;

        public event EventHandler ConnectionClosed;

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public event EventHandler<IEnumerable<Machine>> MachinesRetrieved;

        public event EventHandler<CommunicationException> ConnectionFailed;

        public ServerConnectionHandler(IMachineControlService service)
        {
            this.service = service;
        }

        public async Task ConnectAsync()
        {
            RaiseStatusChangedEvent(ResourceStrings.ConnectingToServer);

            try
            {
                await service.ConnectToServerAsync();

                RaiseStatusChangedEvent(ResourceStrings.DeterminingAvailableHosts);

                var machines = await service.GetMachinesAsync();
                OnMachinesRetrieved(machines);
            }
            catch (CommunicationException exception)
            {
                RaiseStatusChangedEvent(ResourceStrings.ConnectionFailed);
                OnConnectionFailed(exception);
            }
        }

        private void RaiseStatusChangedEvent(string statusMessage)
        {
            OnRaiseStatusChangedEvent(new StatusChangedEventArgs(statusMessage));
        }

        protected virtual void OnRaiseStatusChangedEvent(StatusChangedEventArgs eventArgs)
        {
            StatusChanged?.Invoke(this, eventArgs);
        }

        protected virtual void OnMachinesRetrieved(IEnumerable<Machine> machines)
        {
            MachinesRetrieved?.Invoke(this, machines);
        }

        protected virtual void OnConnectionFailed(CommunicationException exception)
        {
            ConnectionFailed?.Invoke(this, exception);
        }
    }
}
