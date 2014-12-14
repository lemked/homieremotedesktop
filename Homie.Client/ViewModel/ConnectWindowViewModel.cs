using System;
using System.Windows.Input;

using MVVMLib.ViewModel;

using Homie.Client.Interface;
using Homie.Model;

namespace Homie.Client.ViewModel
{
    class ConnectWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IConnectionHandler connectionHandler;
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

        public ConnectWindowViewModel(IConnectionHandler pConnectionHandler, Machine pMachine)
        {
            connectionHandler = pConnectionHandler;
            machine = pMachine;

            machineName = pMachine.NameOrAddress;
            pConnectionHandler.StatusChanged += ConnectionHandlerStatusChanged;
            pConnectionHandler.ConnectionInitiated += ConnectionHandlerConnectionInitiated;

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
            await connectionHandler.Connect(machine);
        }

        private void ConnectionHandlerStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Status = e.StatusMessage;
        }

        private void Abort(object param)
        {
            connectionHandler.Abort();
            isConnecting = false;
            DialogResult = true;
        }
    }
}
