using System;
using System.Threading.Tasks;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Client.Interface
{
    interface IConnectionHandler
    {
        Task Connect(Machine pMachine);
        Task Connect(Machine pMachine, int pTimeout);
        void Abort();

        event EventHandler ConnectionInitiated;

        event EventHandler ConnectionClosed;

        event EventHandler<StatusChangedEventArgs> StatusChanged;
    }

    abstract class ConnectionHandlerBase : IConnectionHandler
    {
        public IMachineControlService Service;

        protected ConnectionHandlerBase(IMachineControlService machineControlService)
        {
            Service = machineControlService;
        }
        public abstract Task Connect(Machine pMachine);

        public abstract Task Connect(Machine pMachine, int pTimeout);

        public abstract void Abort();

        public abstract event EventHandler ConnectionInitiated;

        public abstract event EventHandler ConnectionClosed;

        public abstract event EventHandler<StatusChangedEventArgs> StatusChanged;
    }

    public class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(string pStatusMessage)
        {
            msg = pStatusMessage;
        }
        private readonly string msg;
        public string StatusMessage
        {
            get { return msg; }
        }
    }
}
