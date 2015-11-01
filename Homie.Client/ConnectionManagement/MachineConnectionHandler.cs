using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Client.ConnectionManagement
{
    public class MachineConnectionHandler : IMachineConnectionHandler
    {
        public event EventHandler ConnectionInitiated;

        public event EventHandler ConnectionClosed;

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        private bool isAborted;

        private readonly Process rdpProcess = new Process();

        private readonly Task rdpProcessWatcher;

        private IMachineControlService service;

        public MachineConnectionHandler(IMachineControlService machineControlService)
        {
            service = machineControlService;
            rdpProcess.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe");
            rdpProcessWatcher = new Task(Watch);
        }

        public Task Connect(Machine pMachine)
        {
            return Connect(pMachine, 600);
        }

        public async Task Connect(Machine pMachine, int pTimeout)
        {
            OnRaiseStatusChangedEvent(new StatusChangedEventArgs("Verbinde"));
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.Elapsed.TotalSeconds < pTimeout)
            {
                // Ping machine. If this was successful, go and and establish the connection.
                IPStatus lPingResult = await service.PingAsync(pMachine);
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

        public void Abort()
        {
            isAborted = true;
        }
    }
}
