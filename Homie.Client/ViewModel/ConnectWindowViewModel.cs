using System;
using System.Windows.Input;
using Homie.Client.ConnectionManagement;
using MVVMLib.ViewModel;
using Homie.Model;

namespace Homie.Client.ViewModel
{
    class ConnectWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IMachineConnectionHandler machineConnectionHandler;
        private readonly Machine machine;
        private bool isConnecting;

        #endregion

        #region Properties

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                dialogResult = value;
                OnPropertyChanged();
            }
        }

        private readonly string machineName;
        public string MachineName
        {
            get
            {
                return machineName; 
                
            }
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }


        private bool IsConnecting()
        {
            return isConnecting;
        }


        #endregion

        # region Commands

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(Abort, pPredicate => IsConnecting());
            }
        }


        #endregion

        public ConnectWindowViewModel(IMachineConnectionHandler machineConnectionHandler, Machine machine)
        {
            this.machineConnectionHandler = machineConnectionHandler;
            this.machine = machine;

            machineName = machine.NameOrAddress;
            machineConnectionHandler.StatusChanged += ConnectionHandlerStatusChanged;
            machineConnectionHandler.ConnectionInitiated += ConnectionHandlerConnectionInitiated;

            Connect();
        }

        void ConnectionHandlerConnectionInitiated(object sender, EventArgs e)
        {
            // Close this dialog when the connection has successully been established.
            DialogResult = true;
        }

        private async void Connect()
        {
            isConnecting = true;
            await machineConnectionHandler.Connect(machine);
        }

        private void ConnectionHandlerStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Status = e.StatusMessage;
        }

        private void Abort(object param)
        {
            machineConnectionHandler.Abort();
            isConnecting = false;
            DialogResult = true;
        }
    }
}
