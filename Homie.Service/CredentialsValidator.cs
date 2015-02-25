using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using Homie.Common;
using Homie.Model;

namespace Homie.Service
{
    public class CredentialsValidator: UserNamePasswordValidator
    {
        private readonly IUserDataSource userDataSource;
        public CredentialsValidator(IUserDataSource userDataSource)
        {
            this.userDataSource = userDataSource;
        }
        /// <summary>
        /// Validates the specified username and password.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IdentityModel.Tokens.SecurityTokenValidationException">
        /// </exception>
        public override void Validate(string username, string password)
        {
            if (username == null || password == null)
            {
                throw new ArgumentNullException();
            }

            // Check if an user with the same name already exists.
            User user = this.userDataSource.GetAllUsers().SingleOrDefault(item => item.Username == username);
            if (user == null)
            {
                throw new SecurityTokenValidationException(String.Format(Resources.Properties.Resources.UsernameDoesNotExist, username));
            }

            var passwordHash = new PasswordHash(user.PasswordHash);
            if (!passwordHash.Verify(password))
            {
                throw new SecurityTokenValidationException(Resources.Properties.Resources.InvalidPassword);
            }
        }
    }
}
