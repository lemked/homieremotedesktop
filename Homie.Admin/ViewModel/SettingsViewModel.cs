using System.Windows.Input;
using MVVMLib.ViewModel;
using Homie.Model;
using Homie.Service.Settings;

namespace Homie.Admin.ViewModel
{
    public class SettingsViewModel : DialogViewModelBase
    {
        private readonly IServiceSettingsProvider serviceSettingsProvider;
        private readonly ServiceSettings settings;

        #region Properties

        /// <summary>
        /// Gets or sets the port the service is lisenting on.
        /// </summary>
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
                base.OnPropertyChanged("IsCertificateRequired");
            }
        }

        /// <summary>
        /// Gets or sets the certificate file path.
        /// </summary>
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

        public bool IsCertificateRequired
        {
            get
            {
                return (AuthenticationMode == AuthenticationMode.Certificate ||
                        AuthenticationMode == AuthenticationMode.CertificateAndCredentials);
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
