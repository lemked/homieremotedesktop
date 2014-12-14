using System.Windows.Input;
using MVVMLib.ViewModel;

namespace Homie.Client.ViewModel
{
    class OptionsWindowViewModel : DialogViewModelBase
    {

        #region Properties

        private string serverAddress;
        public string ServerAddress
        {
            get
            {
                return serverAddress;
            }
            set
            {

                serverAddress = value;
                Properties.Settings.Default.ServerAddress = value;
                base.OnPropertyChanged();
            }
        }

        private int serverPort;

        public int ServerPort
        {
            get
            {
                return serverPort;
            }
            set
            {
                serverPort = value;
                Properties.Settings.Default.ServerPort = value;
                base.OnPropertyChanged();
            }
        }


        #endregion Properties


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsWindowViewModel"/> class.
        /// </summary>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public OptionsWindowViewModel()
        {
            serverAddress = Properties.Settings.Default.ServerAddress;
            serverPort = Properties.Settings.Default.ServerPort;
        }

        #endregion


        #region Commands

        private RelayCommand saveSettingsCommand;
        public ICommand SaveSettingsCommand
        {
            get
            {
                if (saveSettingsCommand == null)
                {
                    saveSettingsCommand = new RelayCommand(param =>
                    {
                        this.SaveSettings();
                        DialogResult = true; // Closes the dialog.
                    });
                }
                return saveSettingsCommand;
            }
        }


        #endregion Commands


        #region Methods
        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        #endregion Methods

    }
}