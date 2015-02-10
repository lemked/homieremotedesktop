using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Homie.Model
{
    public class DbUserDataSource : IUserDataSource
    {
        private readonly DatabaseContext databaseContext = new DatabaseContext();

        public void Add(User user)
        {
            databaseContext.Users.Add(user);
            databaseContext.SaveChanges();
        }

        public void Remove(User user)
        {
            databaseContext.Users.Remove(user);
            databaseContext.SaveChanges();
        }

        public void Update(User user)
        {
            var existinguser = databaseContext.Users.Find(user.ID);
            databaseContext.Detach(existinguser);

            databaseContext.Entry(user).State = EntityState.Modified;
            databaseContext.SaveChanges();
        }

        public User GetUserByID(int userID)
        {
            return databaseContext.Users.SingleOrDefault(pItem => pItem.ID.Equals(userID));
        }

        public bool Exists(User user)
        {
            return databaseContext.Exists<User>(user.ID);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return databaseContext.Users.ToList();
        }
    }
}
