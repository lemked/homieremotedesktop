using System.Collections.Generic;
using System.Linq;

namespace Homie.Model.Logging
{
    public class DbServiceLogDataSource : IServiceLogDataSource
    {
        private readonly DatabaseContext databaseContext = new DatabaseContext();
        public IList<LogMessage> LogMessages 
        {
            get
            {
                return databaseContext.LogMessages.ToList();
            } 
        }

        public void Add(LogMessage log)
        {
            databaseContext.LogMessages.Add(log);
            databaseContext.SaveChanges();
        }
    }
}
