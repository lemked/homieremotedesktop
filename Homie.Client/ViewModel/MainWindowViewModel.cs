using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Input;

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
        private IEnumerable<Machine> machines = new List<Machine>();
        private readonly IMachineControlService machineControlService;
        private Machine currentMachine;
        private string statusMessage;

        #endregion Fields

        #region Properties

        public IList<Machine> Machines
        {
            get
            {
                return new List<Machine>(machines);
            }
            set
            {
                machines = value;
                OnPropertyChanged();
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
            machineControlService = factory.Create<IMachineControlService>();

            Initialize();
        }
        
        #endregion Constructor

        #region Commands

        RelayCommand openOptionsCommand;
        
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

        private ICommand connectToHostCommand;

        public ICommand ConnectToHostCommand
        {
            get
            {
                if (connectToHostCommand == null)
                {
                    connectToHostCommand = new RelayCommand(pAction => this.ConnectToHost(), pPredicate => IsMachineSelected());
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

        private ICommand shutdownHostCommand;

        #endregion Commands

        #region Methods

        private async void Initialize()
        {
            StatusMessage = Resources.Properties.Resources.ConnectingToServer;
            try
            {
                this.StatusMessage = Resources.Properties.Resources.DeterminingAvailableHosts;
                this.Machines.Clear();
                // Retrieve the machines from the service.
                this.Machines = await machineControlService.GetMachinesAsync() as IList<Machine>;
                if (this.Machines.Count > 0)
                {
                    CurrentMachine = this.Machines.First();
                    StatusMessage = Resources.Properties.Resources.ReadyToConnectChooseHost;
                    IsMachinesComboBoxEnabled = true;
                }
                else
                {
                    StatusMessage = Resources.Properties.Resources.NoHostToConnectWasFound;
                }
            }
            catch (EndpointNotFoundException) // e.g. server not found
            {
                StatusMessage = Resources.Properties.Resources.ConnectionFailed;
            }
            catch (CommunicationException) // e.g. connection aborted
            {
                StatusMessage = Resources.Properties.Resources.ConnectionFailed;
            }
        }


        private void ConnectToHost()
        {
            this.StatusMessage = Resources.Properties.Resources.ConnectingToServer;
            var connectionHandler = new ConnectionHandler(machineControlService);
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