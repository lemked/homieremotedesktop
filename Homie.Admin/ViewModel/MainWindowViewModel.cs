using System;
using System.Collections.Generic;
using System.Windows.Input;

using Homie.Admin.Properties;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model;
using Homie.Model.Logging;
using Homie.Service;
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

            settingsViewModel = new SettingsViewModel();
        }

        #endregion Constructor
    }


}
