using System.Windows.Input;
using MVVMLib.ViewModel;
using Homie.Model;
using Homie.Model.Logging;
using Homie.Service.Settings;

namespace Homie.Admin.ViewModel
{
    public class SettingsViewModel : DialogViewModelBase
    {
        private IServiceSettingsProvider serviceSettingsProvider;
        private ServiceSettings settings;

        #region Properties

        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the port the service is lisenting on.
        /// </summary>
        /// <value>
        /// The server port.
        /// </value>
        public int ListenPort
        {
            get
            {
                return serviceSettingsProvider.GetSettings().ListenPort;
            }
            set
            {
                settings.ListenPort = value;
                base.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the authentication mode.
        /// </summary>
        /// <value>
        /// The authentication mode.
        /// </value>
        public AuthenticationMode AuthenticationMode
        {
            get
            {
                return settings.AuthenticationMode;
            }
            set
            {
                settings.AuthenticationMode = value;
                base.OnPropertyChanged();
            }
        }

        public string EndPoint { get; set; }
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the certificate file path.
        /// </summary>
        /// <value>
        /// The certificate file path.
        /// </value>
        public string CertificateFilePath
        {
            get
            {
                return settings.CertificateFilePath;
            }
            set
            {
                settings.CertificateFilePath = value;
            }
        }

        #endregion Properties

        #region Commands

        private RelayCommand saveSettingsCommand;

        public ICommand SaveSettingsCommand
        {
            get
            {
                return saveSettingsCommand ?? (saveSettingsCommand = new RelayCommand(param =>
                {
                    this.SaveSettings(); // TODO: Enable button only if a setting has changed
                }));
            }
        }


        #endregion Commands


        #region Methods

        public SettingsViewModel(IServiceSettingsProvider serviceSettingsProvider)
        {
            this.serviceSettingsProvider = serviceSettingsProvider;
            settings = serviceSettingsProvider.GetSettings();
        }

        private void SaveSettings()
        {
            serviceSettingsProvider.SaveSettings(settings);
        }

        #endregion Methods
    }
}
