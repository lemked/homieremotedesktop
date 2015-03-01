using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homie.Common.Interfaces;
using Homie.Common.Logging;
using Homie.Model;

namespace Homie.Service
{
    public class UserControlService : IUserControlService
    {
        private readonly IUserDataSource dataSource;

        public UserControlService(IUserDataSource userDataSource)
        {
            this.dataSource = userDataSource;
        }

        public async Task<int> AddUserAsync(User user)
        {
            return await Task.Factory.StartNew(() => AddUser(user));
        }

        private int AddUser(User user)
        {
            Log.Debug("Request for adding user with username \"{0}\" ...", user.Username);

            // Check if an user with the same name already exists.
            if (dataSource.GetAllUsers().SingleOrDefault(item => item.Username == user.Username) != null)
            {
                throw new InvalidOperationException(String.Format(Resources.Properties.Resources.UsernameAlreadyExists, user.Username));
            }

            // Add user.
            dataSource.Add(user);

            Log.Info(Resources.Properties.Resources.UserSuccessfullyAdded, user.Username);

            return user.ID;
        }

        public async Task UpdateUserAsync(User user)
        {
            await Task.Factory.StartNew(() => UpdateUser(user));
        }

        private void UpdateUser(User user)
        {
            Log.Debug("Request for update of user with username \"{0}\" ...", user.Username);

            // Check if the machine exists in the list.
            if (!dataSource.Exists(user))
            {
                throw new InvalidOperationException(String.Format(Resources.Properties.Resources.UsernameDoesNotExist, user.Username));
            }

            // Apply changes.            
            dataSource.Update(user);

            Log.Info(String.Format(Resources.Properties.Resources.UserSuccessfullyUpdated, user.Username));
        }

        public async Task RemoveUserAsync(int userID)
        {
            await Task.Factory.StartNew(() => RemoveUser(userID));
        }

        private void RemoveUser(int userID)
        {
            var user = dataSource.GetUserByID(userID);

            // Check if the machine exists in the list.
            if (user == null)
            {
                throw new InvalidOperationException(Resources.Properties.Resources.UnableToRemoveUserAccountUserNotFound);
            }

            // Remove machine.
            dataSource.Remove(user);

            Log.Info(Resources.Properties.Resources.UserWasRemoved, user.Username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.Factory.StartNew(() => GetUsers());
        }

        private IEnumerable<User> GetUsers()
        {
            return dataSource.GetAllUsers();
        }

        public async Task<User> GetUserAsync(int userID)
        {
            return await Task.Factory.StartNew(() => GetUser(userID));
        }

        private User GetUser(int userID)
        {
            return dataSource.GetUserByID(userID);
        }
    }
}
