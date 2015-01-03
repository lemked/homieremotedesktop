using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;

using MVVMLib;
using MVVMLib.ViewModel;
using MVVMLib.Dialog.Service;

using Homie.Common;
using Homie.Common.Interfaces;
using Homie.Model;
using Homie.Admin.Properties;

namespace Homie.Admin.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDialogService dialogService;

        private List<Machine> machines = new List<Machine>();
        private IMachineControlService machineControlService;
        private IServiceLogProvider serviceLogProvider;

        private IViewModel currentViewModel;
        private EventLogViewModel eventLogViewModel;
        private MachinesViewModel machinesViewModel;

        private RelayCommand reconnectCommand;
        private RelayCommand showMachinesCommand;
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

            machineControlService = WebServiceFactory.Create<IMachineControlService>(Settings.Default.ServerAddress, Settings.Default.ServerPort);
            IServiceLogProvider serviceLogProvider = WebServiceFactory.Create<IServiceLogProvider>(Settings.Default.ServerAddress, Settings.Default.ServerPort);

            eventLogViewModel = new EventLogViewModel(serviceLogProvider);
            machinesViewModel = new MachinesViewModel(dialogService, machineControlService);

            ConnectToServer();
        }

        #endregion Constructor

        #region Methods

        private async void ConnectToServer(bool faulted = false, int failCount = 0)
        {
            // If already faulted, wait a moment before trying again.
            if (faulted && failCount < 10)
            {
                await Task.Delay(1000);

                // Recreate the service
                machineControlService = WebServiceFactory.Create<IMachineControlService>(Settings.Default.ServerAddress, Settings.Default.ServerPort);

                serviceLogProvider = WebServiceFactory.Create<IServiceLogProvider>(Settings.Default.ServerAddress, Settings.Default.ServerPort);

                eventLogViewModel = new EventLogViewModel(serviceLogProvider);
                machinesViewModel = new MachinesViewModel(this.dialogService, machineControlService);
            }
            else if (faulted)
            {
                // If maximum retry count reached, do not try again.
                StatusMessage = Resources.Properties.Resources.ConnectionFailed;
                return;
            }

            try
            {
                // Connect to the server.
                IsConnected = false;
                StatusMessage = "Connecting to server ...";
                await machineControlService.ConnectAsync();

                // If connected, retrieve the data.
                StatusMessage = "Retrieving data ...";
                await eventLogViewModel.GetEventLogEntriesAsync();
                await machinesViewModel.GetMachinesAsync();

                // Show the event log as default view.
                CurrentViewModel = eventLogViewModel;

                StatusMessage = "Connected to server.";
                IsConnected = true;
            }
            catch (EndpointNotFoundException exception) // e.g. server not found / service not running
            {
                ConnectToServer(true, failCount+1);
                LastException = exception;
            }
            catch (CommunicationObjectFaultedException exception)
            {
                ConnectToServer(true, failCount+1);
                LastException = exception;
            }
            catch (CommunicationException exception) // e.g. connection aborted
            {
                ConnectToServer(true, failCount+1);
                LastException = exception;
            }
        }

        #endregion Methods
    }


}
