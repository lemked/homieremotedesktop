using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Homie.Model
{
    public class DatabaseDatasource<T> where T : class, IEntityWithId
    {
        private readonly DatabaseContext databaseContext = new DatabaseContext();

        public bool EnableAutoCommit { get; set; }

        public void Add(T entity)
        {
            databaseContext.Set<T>().Add(entity);
            if (EnableAutoCommit)
            {
                databaseContext.SaveChanges();
            }
        }

        public void Remove(int ID)
        {
            var entity = databaseContext.Set<T>().Find(ID);
            databaseContext.Set<T>().Remove(entity);
            if (EnableAutoCommit)
            {
                databaseContext.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            var existingEntity = databaseContext.Set<T>().Find(entity.ID);
            databaseContext.Detach(existingEntity);

            databaseContext.Entry(entity).State = EntityState.Modified;
            if (EnableAutoCommit)
            {
                databaseContext.SaveChanges();
            }
        }

        public T Get(int ID)
        {
            var entity = databaseContext.Set<T>().Find(ID);
            return entity;
        }

        public bool Exists(int ID)
        {
            return (databaseContext.Set<T>().Find(ID) != null);
        }

        public IQueryable<T> GetAll()
        {
            return databaseContext.Set<T>();
        }

        public int SaveChanges()
        {
            return databaseContext.SaveChanges();
        }
    }
}
