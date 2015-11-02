using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Windows.Input;

using Homie.Admin.Properties;
using Homie.Admin.Services;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model;
using Homie.Model.Logging;
using Homie.Service;
using Homie.Service.Settings;
using MVVMLib;
using MVVMLib.Dialog.Service;
using MVVMLib.ViewModel;

namespace Homie.Admin.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private List<Machine> machines = new List<Machine>();

        private IViewModel currentViewModel;
        private readonly EventLogViewModel eventLogViewModel;
        private readonly UsersViewModel usersViewModel;
        private readonly MachinesViewModel machinesViewModel;
        private readonly SettingsViewModel settingsViewModel;

        private ICommand startServiceCommand;

        private ServiceControllerStatus status = 0;
        private bool isConnected;
        private IServiceControl serviceControl;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the current view model.
        /// </summary>
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
        public ServiceControllerStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
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
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged();
                }
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

        public ICommand ShowSettingsCommand { get; }

        public ICommand ShowMachinesCommand { get; }

        public ICommand ShowUsersCommand { get; }

        public ICommand ShowEventLogCommand { get; }

        public ICommand StartServiceCommand { get; }

        public ICommand StopServiceCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Resolves all required interfaces via the ServiceLocator class.
        /// </remarks>
        public MainWindowViewModel() : this(ServiceLocator.Resolve<IDialogService>(), ServiceLocator.Resolve<IServiceControl>())
        {
            ShowEventLogCommand = new RelayCommand(action => CurrentViewModel = eventLogViewModel);
            ShowUsersCommand = new RelayCommand(action => CurrentViewModel = usersViewModel);
            ShowMachinesCommand = new RelayCommand(action => CurrentViewModel = machinesViewModel);
            ShowSettingsCommand = new RelayCommand(action => CurrentViewModel = settingsViewModel);

            var timeout = new TimeSpan(0, 0, 0, 5);
            StartServiceCommand = new RelayCommand(action => serviceControl.StartServiceAsync(timeout));
            StopServiceCommand = new RelayCommand(action => serviceControl.StopServiceAsync(timeout));

            UpdateServiceStatus();
        }

        private async void UpdateServiceStatus()
        {
            Status = await serviceControl.GetStatusAsync();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="dialogService">The pdialog service.</param>
        public MainWindowViewModel(IDialogService dialogService, IServiceControl serviceControl)
        {
            // Configure default logger
            ILogger textLogger = new FileLogger();
            textLogger.LogLevel = Settings.Default.LogLevel;
            Log.Register(textLogger);

            // Service log
            IServiceLogDataSource serviceLogDataSource = new DbServiceLogDataSource(); // TODO: Use DI container 
            IServiceLogProvider serviceLogProvider = new ServiceLogProvider(serviceLogDataSource);
            eventLogViewModel = new EventLogViewModel(serviceLogProvider);

            // Users
            IUserDataSource userDataSource = new DbUserDataSource(); // TODO: Use DI container 
            IUserControlService userControlService = new UserControlService(userDataSource);
            usersViewModel = new UsersViewModel(dialogService, userControlService);

            // Users
            IMachineDataSource machinedaDataSource = new DbMachineDataSource(); // TODO: Use DI container 
            IMachineControlService machineControlService = new MachineControlService(machinedaDataSource);
            machinesViewModel = new MachinesViewModel(dialogService, machineControlService);

            // Settings
            IServiceSettingsProvider serviceSettingsProvider = new DbServiceSettingsProvider();
            settingsViewModel = new SettingsViewModel(serviceSettingsProvider);

            this.serviceControl = serviceControl;
            this.serviceControl.StatusChanged += ServiceControl_StatusChanged;
        }

        private void ServiceControl_StatusChanged(object sender, ServiceControllerStatus serviceControllerStatus)
        {
            Status = serviceControllerStatus;
        }

        #endregion Constructor
    }


}
