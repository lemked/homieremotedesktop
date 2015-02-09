using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Homie.Model;

namespace Homie.Common.Interfaces
{
    [ServiceContract]
    public interface IUserControlService
    {
        /// <summary>
        /// Adds a new userID.
        /// </summary>
        /// <param name="user">The userID.</param>
        /// <returns>The ID of the new userID.</returns>
        [OperationContract]
        Task<int> AddUserAsync(User user);

        /// <summary>
        /// Updates the given userID.
        /// </summary>
        /// <param name="user">The userID to update.</param>
        /// <returns></returns>
        [OperationContract]
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Removes the userID with the given ID.
        /// </summary>
        /// <param name="userID">The ID of the userID to remove.</param>
        /// <returns></returns>
        [OperationContract]
        Task RemoveUserAsync(int userID);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>The users.</returns>
        [OperationContract]
        Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Gets the userID with the specified ID.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns>The user.</returns>
        [OperationContract]
        Task<User> GetUserAsync(int userID);
    }
}
