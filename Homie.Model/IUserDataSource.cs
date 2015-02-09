using System.Collections.Generic;

namespace Homie.Model
{
    public interface IUserDataSource
    {
        void Add(User user);

        void Remove(User user);

        void Update(User user);

        User GetUserByID(int userID);

        bool Exists(User user);

        IEnumerable<User> GetAllUsers();
    }
}
