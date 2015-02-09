using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using Homie.Model.Logging;

namespace Homie.Model
{
    /// <summary>
    /// Provides repository of data model using a database.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets the machines.
        /// </summary>
        /// <value>
        /// The machines.
        /// </value>
        public DbSet<Machine> Machines { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>
        /// The log messages.
        /// </value>
        public DbSet<LogMessage> LogMessages { get; set; }

        /// <summary>
        /// Detaches the specified entity from the object context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            objectContext.Detach(entity);
        }

        /// <summary>
        /// Checks if the entity with the given primary key exists in the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns>True if the entity already exists, false otherwise.</returns>
        public bool Exists<T>(params object[] keys) where T : class
        {
            return (this.Set<T>().Find(keys) != null);
        }
    }
}
