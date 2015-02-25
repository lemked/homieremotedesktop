using System.Windows.Input;
using Homie.Common;
using Homie.Common.WebService;
using MVVMLib.ViewModel;

namespace Homie.Admin.ViewModel
{
    public class SettingsViewModel : DialogViewModelBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the server address.
        /// </summary>
        /// <value>
        /// The server address.
        /// </value>
        public string ServerAddress
        {
            get
            {
                return Properties.Settings.Default.ServerAddress;
            }
            set
            {

                Properties.Settings.Default.ServerAddress = value;
                base.OnPropertyChanged();
            }
        }

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


        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get
            {
                return Properties.Settings.Default.Username;
            }
            set
            {
                Properties.Settings.Default.Username = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get
            {
                if (Properties.Settings.Default.PasswordHash != null)
                {
                    return Properties.Settings.Default.PasswordHash.ToString();
                }

                return string.Empty;
            }
            set
            {
                var passwordHash = new PasswordHash(value);
                byte[] hashBytes = passwordHash.ToArray();
                Properties.Settings.Default.PasswordHash = hashBytes;
            }
        }

        #endregion Properties

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
