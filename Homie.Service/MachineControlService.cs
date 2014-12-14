using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model;

namespace Homie.Service
{
    public class MachineControlService : IMachineControlService
    {
        private readonly IMachineDataSource machineDataSource = new DbMachineDataSource();

        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string user, string pPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddMachineAsync(Machine machine)
        {
            return await Task.Factory.StartNew(() => AddMachine(machine));
        }

        private int AddMachine(Machine machine)
        {
            Log.Debug(String.Format("Request for adding machine \"{0}\" ...", machine.NameOrAddress));

            // TODO: Implement SingleOrDefault for IMachineDataSource, see http://stackoverflow.com/questions/14849202/the-objectstatemanager-cannot-track-multiple-objects-with-the-same-key
            
            // Check if a machine with the same name already exists.
            if (machineDataSource.GetAllMachines().SingleOrDefault(item => item.NameOrAddress == machine.NameOrAddress) != null)
            {
                throw new ArgumentException(String.Format("Name/address \"{0}\" already exists.", machine.NameOrAddress));
            }

            // Check if a machine with the same MAC address already exists.
            if (machineDataSource.GetAllMachines().SingleOrDefault(pItem => pItem.NameOrAddress == machine.NameOrAddress) != null)
            {
                throw new ArgumentException(String.Format("MAC address {0} already exists.", machine.MacAddress));
            }

            // Add machine.
            machineDataSource.Add(machine);

            Log.Info(String.Format("Machine \"{0}\" was successfully added.", machine.NameOrAddress));

            return machine.MachineID;
        }

        public async Task UpdateMachineAsync(Machine machine)
        {
            await Task.Factory.StartNew(() => UpdateMachine(machine));
        }

        private void UpdateMachine(Machine machine)
        {
            Log.Debug(String.Format("Request for update of machine \"{0}\" ...", machine.NameOrAddress));

            // Check if the machine exists in the list.
            if (!machineDataSource.Exists(machine))
            {
                throw new InvalidOperationException(String.Format("Machine \"{0}\" does not exist.", machine.NameOrAddress));
            }

            // Apply changes.            
            machineDataSource.Update(machine);

            Log.Info(String.Format("Machine \"{0}\" was successfully updated.", machine.NameOrAddress));
        }

        public async Task RemoveMachineAsync(int machineID)
        {
            await Task.Factory.StartNew(() => RemoveMachine(machineID));
        }

        private void RemoveMachine(int machineID)
        {
            var machine = machineDataSource.GetMachineByID(machineID);

            // Check if the machine exists in the list.
            if (machine == null)
            {
                throw new InvalidOperationException("Cannot remove machine: Not found.");
            }

            // Remove machine.
            machineDataSource.Remove(machine);

            Log.Info(String.Format("Machine \"{0}\" was removed.", machine.NameOrAddress));
        }

        private IEnumerable<Machine> GetMachines()
        {
            return machineDataSource.GetAllMachines();
        }

        public async Task<IEnumerable<Machine>> GetMachinesAsync()
        {
            return await Task.Factory.StartNew(() => GetMachines());
        }

        public async Task<Machine> GetMachineAsync(int pMachineID)
        {
            return await Task.Factory.StartNew(() => machineDataSource.GetMachineByID(pMachineID));
        }

        private void WakeUp(string pMacAddress)
        {
            Log.Info("Sending magic packet for MAC address: " + pMacAddress);
            try
            {
                string lSeparator = ":";
                // In case the mac address is splitted using "-", e.g. "00-00-00-00-00-00-00-E0"
                if (pMacAddress.Contains("-"))
                {
                    lSeparator = "-";
                }

                string[] lArrMac = pMacAddress.Split(Char.Parse(lSeparator));
                byte[] lMacadresse = new byte[6];

                for (int lIndex = 0; lIndex < lMacadresse.Length; lIndex++)
                {
                    lMacadresse[lIndex] = byte.Parse(lArrMac[lIndex], NumberStyles.HexNumber);
                }

                UdpClient lWoLclient = new UdpClient();
                lWoLclient.Connect(IPAddress.Broadcast, 0);

                byte[] lStartsignal = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                byte[] lWolSignal = new byte[102];

                lStartsignal.CopyTo(lWolSignal, 0);

                for (int lIndex = 1; lIndex <= 16; lIndex++)
                    lMacadresse.CopyTo(lWolSignal, lIndex * 6);

                lWoLclient.Send(lWolSignal, lWolSignal.Length);

                Log.Info("Broadcast successful, waiting for requested host to start up ...");
            }

            catch (Exception lException)
            {
                Log.Exception(lException);
                throw;
            }
        }

        public async Task StartMachineAsync(Machine pMachine)
        {
            await Task.Factory.StartNew(() => WakeUp(pMachine.MacAddress));
        }

        public async Task<IPStatus> PingAsync(Machine pMachine)
        {
            Log.Info("Sending ICMP echo request for " + pMachine.NameOrAddress);

            var lPing = new Ping();
            try
            {
                PingReply lReply = await lPing.SendPingAsync(pMachine.NameOrAddress);
                if (lReply != null)
                {
                    if (lReply.Status == IPStatus.Success)
                    {
                        Log.Info(String.Format("Host {0} is now online.", pMachine.NameOrAddress));
                    }
                    return lReply.Status;
                }
            }
            catch (PingException lException)
            {
                if (lException.InnerException != null && lException.InnerException.HResult.Equals(-2147467259))
                {
                    return IPStatus.DestinationHostUnreachable;
                }
            }
            return IPStatus.Unknown;
        }

        public async Task ShutdownAsync(Machine machine)
        {
            await Task.Factory.StartNew(() => Shutdown(machine));
        }
        private void Shutdown(Machine pMachine)
        {
            Log.Info(String.Format("Client has requested shutdown of host \"{0}\".", pMachine.NameOrAddress));
            Process lShutdownProc = new Process();
            lShutdownProc.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\shutdown.exe");
            lShutdownProc.StartInfo.Arguments = String.Format(@"/m \\{0} /s", pMachine.NameOrAddress);
            Log.Debug(lShutdownProc.StartInfo.FileName + " " + lShutdownProc.StartInfo.Arguments);
            if (lShutdownProc.Start())
            {
                Log.Info(String.Format("Shutdown for \"{0}\" successfully initiated.", pMachine.NameOrAddress));
            }
        }
    }
}
