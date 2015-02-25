using Homie.Common;
using Homie.Model;
using MVVMLib.ViewModel;

namespace Homie.Admin.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {

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
                if (User != null && User.PasswordHash != null)
                {
                    var passwordHash = new PasswordHash(User.PasswordHash);
                    return GetString(passwordHash.Hash);
                }

                return string.Empty;
            }
            set
            {
                var passwordHash = new PasswordHash(value);
                var hashBytes = passwordHash.ToArray();
                User.PasswordHash = hashBytes;
            }
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public User User { get; set; }

        public UserDetailsViewModel()
        {

        }

        public UserDetailsViewModel(User user)
        {
            this.User = user;
        }
    }
}
