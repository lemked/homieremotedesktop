using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using System.Windows.Input;

using MVVMLib;
using MVVMLib.ViewModel;
using MVVMLib.Dialog.Service;

using Homie.Common.Interfaces;
using Homie.Model;
using Homie.Admin.Properties;
using Homie.Admin.View;
using Homie.Common.Logging;
using Homie.Common.WebService;

namespace Homie.Admin.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDialogService dialogService;

        private List<Machine> machines = new List<Machine>();
        private IMachineControlService machineControlService;
        private IUserControlService userControlService;
        private IServiceLogProvider serviceLogProvider;

        private IViewModel currentViewModel;
        private EventLogViewModel eventLogViewModel;
        private UsersViewModel usersViewModel;
        private MachinesViewModel machinesViewModel;
        private SettingsViewModel settingsViewModel;

        private RelayCommand reconnectCommand;
        private RelayCommand showSettingsCommand;
        private RelayCommand showMachinesCommand;
        private RelayCommand showUsersCommand;
        private RelayCommand showEventLogCommand;

        private string statusMessage;
        private bool isConnected;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the current view model.
        /// </summary>
        /// <value>
        /// The current view model.
        /// </value>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public IViewModel CurrentViewModel
        {
            get
            {
                if (currentViewModel == null)
                {
                    currentViewModel = eventLogViewModel;
                }
                return currentViewModel;
            }
            set
            {
                if (value != currentViewModel)
                {
                    currentViewModel = value;
                    OnPropertyChanged();
                }
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

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
                base.OnPropertyChanged();
            }
        }

        public List<Machine> Machines
        {
            get
            {
                return new List<Machine>(machines);
            }
            set
            {
                machines = value;
                base.OnPropertyChanged();
            }
        }

        public Exception LastException { get; private set; }

        #endregion Properties

        #region Commands

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

        public ICommand ShowSettingsCommand
        {
            get
            {
                if (showSettingsCommand == null)
                {
                    showSettingsCommand = new RelayCommand(action => CurrentViewModel = settingsViewModel);
                }
                return showSettingsCommand;
            }
        }

        public ICommand ShowMachinesCommand
        {
            get
            {
                if (showMachinesCommand == null)
                {
                    showMachinesCommand = new RelayCommand(action => CurrentViewModel = machinesViewModel);
                }
                return showMachinesCommand;
            }
        }

        public ICommand ShowUsersCommand
        {
            get
            {
                if (showUsersCommand == null)
                {
                    showUsersCommand = new RelayCommand(action => CurrentViewModel = usersViewModel);
                }
                return showUsersCommand;
            }
        }

        public ICommand ShowEventLogCommand
        {
            get
            {
                if (showEventLogCommand == null)
                {
                    showEventLogCommand = new RelayCommand(action => CurrentViewModel = eventLogViewModel);
                }
                return showEventLogCommand;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Resolves all required interfaces via the ServiceLocator class.
        /// </remarks>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public MainWindowViewModel() : this(ServiceLocator.Resolve<IDialogService>())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="dialogService">The pdialog service.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            // Configure default logger
            ILogger textLogger = new FileLogger();
            textLogger.LogLevel = Settings.Default.LogLevel;
            Log.Register(textLogger);

            settingsViewModel = new SettingsViewModel();

            CreateChannelsAndAssignToViewModels();
            ConnectToServer();
        }

        #endregion Constructor

        #region Methods

        private void CreateChannelsAndAssignToViewModels()
        {
            HttpBindingBase binding;

            AuthenticationMode authenticationMode = Settings.Default.AuthenticationMode;
            switch (authenticationMode)
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

            binding.MaxReceivedMessageSize = 5242880;

            var factory = new WebServiceFactory(binding, Settings.Default.ServerAddress, Settings.Default.ServerPort, Settings.Default.ServiceEndPoint);
            
            // For these authentication modes, credentials have to be set for the factory.
            if (authenticationMode == AuthenticationMode.Credentials ||
                authenticationMode == AuthenticationMode.CertificateAndCredentials)
            {
                if (string.IsNullOrEmpty(Settings.Default.Username) || string.IsNullOrEmpty(Settings.Default.Password))
                {
                    throw new ArgumentException("Invalid username or password. Please review your settings.");
                }

                factory.Username = Settings.Default.Username;
                factory.Password = Settings.Default.Password;
            }

            machineControlService = factory.Create<IMachineControlService>();
            userControlService = factory.Create<IUserControlService>();
            serviceLogProvider = factory.Create<IServiceLogProvider>();

            eventLogViewModel = new EventLogViewModel(serviceLogProvider);
            machinesViewModel = new MachinesViewModel(dialogService, machineControlService);
            usersViewModel = new UsersViewModel(dialogService, userControlService);
        }

        private async void ConnectToServer(bool faulted = false, int failCount = 0)
        {
            // If already faulted, wait a moment before trying again.
            if (faulted && failCount < 10)
            {
                await Task.Delay(1000);

                // Recreate the channels.
                CreateChannelsAndAssignToViewModels();
            }
            else if (faulted)
            {
                // If maximum retry count reached, do not try again.
                StatusMessage = Resources.Properties.Resources.ConnectionFailed;
                return;
            }

            try
            {
                // Connect to the server and retrieve the data.
                IsConnected = false;
                StatusMessage = Resources.Properties.Resources.ConnectingToServer;

                // If connected, retrieve the data.
                await eventLogViewModel.GetEventLogEntriesAsync();
                await machinesViewModel.GetMachinesAsync();
                await usersViewModel.GetUsersAsync();

                // Show the event log as default view.
                CurrentViewModel = eventLogViewModel;

                StatusMessage = "Connected to server.";
                IsConnected = true;
            }
            catch (EndpointNotFoundException exception) // e.g. server not found / service not running
            {
                Log.Exception(exception);
                LastException = exception;
                ConnectToServer(true, failCount+1);
            }
            catch (CommunicationObjectFaultedException exception)
            {
                Log.Exception(exception);
                LastException = exception;
                ConnectToServer(true, failCount+1);
            }
            catch (CommunicationException exception) // e.g. connection aborted
            {
                Log.Exception(exception);
                LastException = exception;
                ConnectToServer(true, failCount+1);
            }
        }

        #endregion Methods
    }


}
