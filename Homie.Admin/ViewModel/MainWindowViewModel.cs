﻿using System;
using System.Collections.Generic;
using System.Windows.Input;

using Homie.Admin.Properties;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model;

using MVVMLib;
using MVVMLib.Dialog.Service;
using MVVMLib.ViewModel;

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
        }

        #endregion Constructor
    }


}
