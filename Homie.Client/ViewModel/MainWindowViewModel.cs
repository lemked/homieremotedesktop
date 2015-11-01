using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Input;
using Homie.Client.ConnectionManagement;
using MVVMLib;
using MVVMLib.Dialog.Service;
using MVVMLib.ViewModel;

using Homie.Client.Properties;
using Homie.Common.Interfaces;
using Homie.Common.WebService;
using Homie.Model;

namespace Homie.Client.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDialogService dialogService;
        private IMachineControlService machineControlService;
        private IEnumerable<Machine> machines;
        private Machine currentMachine;
        private string statusMessage;

        private ICommand shutdownHostCommand;
        private ICommand openOptionsCommand;
        private ICommand connectToHostCommand;
        private ICommand showErrorCommand;
        private ICommand reconnectCommand;

        #endregion Fields

        #region Properties

        public Exception LastException
        {
            get
            {
                return lastException;
            }
            private set
            {
                if (value != lastException)
                {
                    lastException = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<Machine> Machines
        {
            get { return machines; }
            set
            {
                if (machines != value)
                {
                    machines = value;
                    OnPropertyChanged();
                }
            }
        }

        public Machine CurrentMachine 
        {
            get
            {
                return currentMachine;
            }
            set
            {
                currentMachine = value;
                base.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                statusMessage = value;
                base.OnPropertyChanged();
            }
        }

        private bool isMachinesComboBoxEnabled;
        private Exception lastException;

        public bool IsMachinesComboBoxEnabled
        {
            get
            {
                return isMachinesComboBoxEnabled;
            }
            set
            {
                isMachinesComboBoxEnabled = value;
                base.OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public MainWindowViewModel() : this(ServiceLocator.Resolve<IDialogService>())
        {
            
        }

        public MainWindowViewModel(IDialogService pdialogService)
        {
            dialogService = pdialogService;

            ConnectToServer();
        }
        
        #endregion Constructor

        #region Commands
        
        public ICommand OpenOpenOptionsCommand
        {
            get
            {
                if (openOptionsCommand == null)
                {
                    openOptionsCommand = new RelayCommand(param => this.OpenOptions());
                }
                return openOptionsCommand;
            }
        }

        public ICommand ConnectToHostCommand
        {
            get
            {
                if (connectToHostCommand == null)
                {
                    connectToHostCommand = new RelayCommand(pAction => this.ConnectToMachine(), pPredicate => IsMachineSelected());
                }
                return connectToHostCommand;
            }
        }

        public ICommand ShutdownHostCommand
        {
            get
            {
                if (shutdownHostCommand == null)
                {
                    shutdownHostCommand = new RelayCommand(pAction => this.ShutdownHost(), pPredicate => IsMachineSelected());
                }
                return shutdownHostCommand;
            }
        }

        public ICommand ShowErrorCommand
        {
            get
            {
                if (showErrorCommand == null)
                {
                    showErrorCommand = new RelayCommand(action => ShowLastError(), canExecute => LastException != null);
                }
                return showErrorCommand;
            }
        }

        public ICommand ReconnectCommand
        {
            get
            {
                if (reconnectCommand == null)
                {
                    reconnectCommand = new RelayCommand(action => ConnectToServer());
                }
                return reconnectCommand;
            }
        }

        #endregion Commands

        #region Methods

        private MessageBoxResult ShowLastError()
        {
            return dialogService.ShowMessageBox(this, LastException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private IMachineControlService GetService()
        {
            Binding binding;
            switch (Settings.Default.AuthenticationMode)
            {
                case AuthenticationMode.None:
                    binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                    break;
                case AuthenticationMode.Credentials:
                    binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
                    break;
                case AuthenticationMode.Certificate:
                    binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                    break;
                case AuthenticationMode.CertificateAndCredentials:
                    binding = new BasicHttpsBinding(BasicHttpsSecurityMode.TransportWithMessageCredential);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported authentication mode");
            }

            var factory = new WebServiceFactory(binding, Settings.Default.ServerAddress, Settings.Default.ServerPort, Settings.Default.ServiceEndPoint);
            var service = factory.Create<IMachineControlService>();

            return service;
        }

        private async void ConnectToServer()
        {
            machineControlService = GetService();
            var connectionHandler = new ServerConnectionHandler(machineControlService);

            connectionHandler.StatusChanged += ConnectionHandler_StatusChanged;
            connectionHandler.MachinesRetrieved += ConnectionHandler_MachinesRetrieved;
            connectionHandler.ConnectionFailed += ConnectionHandler_ConnectionFailed;

            await connectionHandler.ConnectAsync();
        }

        private void ConnectionHandler_ConnectionFailed(object sender, CommunicationException exception)
        {
            LastException = exception;
        }

        private void ConnectionHandler_MachinesRetrieved(object sender, IEnumerable<Machine> retrievedMachines)
        {
            var machineList = retrievedMachines as IList<Machine> ?? retrievedMachines.ToList();

            Machines = machineList;

            if (Machines.Any())
            {
                CurrentMachine = Machines.First();
                StatusMessage = Resources.Properties.Resources.ReadyToConnectChooseHost;
                IsMachinesComboBoxEnabled = true;
            }
            else
            {
                StatusMessage = Resources.Properties.Resources.NoHostToConnectWasFound;
            }
        }

        private void ConnectionHandler_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            StatusMessage = e.StatusMessage;
        }

        private void ConnectToMachine()
        {
            this.StatusMessage = Resources.Properties.Resources.ConnectingToServer;
            var connectionHandler = new MachineConnectionHandler(machineControlService);
            var connectionWindowViewModel = new ConnectWindowViewModel(connectionHandler, currentMachine);
            dialogService.ShowDialog(this, connectionWindowViewModel);
        }

        private bool IsMachineSelected()
        {
            return this.CurrentMachine != null;
        }


        private void ShutdownHost()
        {
            machineControlService.ShutdownAsync(currentMachine);
        }

        private void OpenOptions()
        {
            var optionsWindowViewModel = new OptionsWindowViewModel();
            dialogService.ShowDialog(this, optionsWindowViewModel);
        }

        #endregion Methods
    }
}