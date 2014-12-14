using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Model;
using Homie.Client.Interface;

namespace Homie.Client
{
    class ConnectionHandler : ConnectionHandlerBase
    {
        public override event EventHandler ConnectionInitiated;

        public override event EventHandler ConnectionClosed;

        public override event EventHandler<StatusChangedEventArgs> StatusChanged;

        private bool isAborted;

        private readonly Process rdpProcess = new Process();

        private readonly Task rdpProcessWatcher;

        public ConnectionHandler(IMachineControlService machineControlService) : base(machineControlService)
        {
            rdpProcess.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe");
            rdpProcessWatcher = new Task(Watch);
        }
        public override Task Connect(Machine pMachine)
        {
            return Connect(pMachine, 600);
        }

        public override async Task Connect(Machine pMachine, int pTimeout)
        {
            OnRaiseStatusChangedEvent(new StatusChangedEventArgs("Verbinde"));
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.Elapsed.TotalSeconds < pTimeout)
            {
                // Ping machine. If this was successful, go and and establish the connection.
                IPStatus lPingResult = await Service.PingAsync(pMachine);
                if (lPingResult == IPStatus.Success)
                {
                    OnRaiseStatusChangedEvent(new StatusChangedEventArgs("Rechner ist online, initiiere RDP-Verbindung ..."));
                    InitiateRemoteDesktop(pMachine);
                    rdpProcessWatcher.Start();
                    return;
                }
                    
                if (isAborted) return;
            }
        }

        private void InitiateRemoteDesktop(Machine pMachine)
        {
            string lRdpArguments = "/v " + pMachine.NameOrAddress;
            if (!pMachine.Port.Equals(0))
            {
                lRdpArguments = String.Format("{0}:{1}", lRdpArguments, pMachine.Port);
            }
            rdpProcess.StartInfo.Arguments = lRdpArguments;
            try
            {
                rdpProcess.Start();
                
            }
            catch (Win32Exception exception)
            {
                OnRaiseStatusChangedEvent(new StatusChangedEventArgs("Fehler: " + exception.Message));
            }
            OnRaiseConnectionInitiatedEvent(new EventArgs());
        }

        private async void Watch()
        {
            while (!rdpProcess.HasExited)
            {
                await Task.Delay(1000);
            }

            OnRaiseConnectionClosedEvent(new EventArgs());
        }

        protected virtual void OnRaiseStatusChangedEvent(StatusChangedEventArgs e)
        {
            EventHandler<StatusChangedEventArgs> handler = StatusChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRaiseConnectionInitiatedEvent(EventArgs e)
        {
            EventHandler handler = ConnectionInitiated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRaiseConnectionClosedEvent(EventArgs e)
        {
            EventHandler handler = ConnectionClosed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void Abort()
        {
            isAborted = true;
        }
    }
}
