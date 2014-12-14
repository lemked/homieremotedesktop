using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using Homie.Model.Logging;

namespace Homie.Model
{
    /// <summary>
    /// Provides repository of data model using a database.
    /// </summary>
    /// <author>Daniel Lemke - lemked@web.de</author>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets the machines.
        /// </summary>
        /// <value>
        /// The machines.
        /// </value>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public DbSet<Machine> Machines { get; set; }

        /// <summary>
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>
        /// The log messages.
        /// </value>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public DbSet<LogMessage> LogMessages { get; set; }

        /// <summary>
        /// Detaches the specified entity from the object context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
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
        /// <returns></returns>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public bool Exists<T>(params object[] keys) where T : class
        {
            return (this.Set<T>().Find(keys) != null);
        }
    }
}
