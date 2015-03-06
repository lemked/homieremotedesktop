using System.Windows.Input;
using Homie.Common;
using Homie.Common.WebService;
using MVVMLib.ViewModel;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public class SettingsViewModel : DialogViewModelBase
    {

        #region Properties
        
        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        /// <value>
        /// The server port.
        /// </value>
        public int ServerPort
        {
            get
            {
                return Properties.Settings.Default.ServerPort;
            }
            set
            {
                Properties.Settings.Default.ServerPort = value;
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
                return Properties.Settings.Default.AuthenticationMode;
            }
            set
            {
                Properties.Settings.Default.AuthenticationMode = value;
                base.OnPropertyChanged();
            }
        }

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
                return Properties.Settings.Default.CertificateFilePath;
            }
            set
            {
                Properties.Settings.Default.CertificateFilePath = value;
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

        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        #endregion Methods
    }
}
