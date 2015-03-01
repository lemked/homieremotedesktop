using System.Collections.Generic;
using System.Threading.Tasks;
using Homie.Common.Interfaces;
using Homie.Model.Logging;

namespace Homie.Admin
{
    public class ServiceLogProvider : IServiceLogProvider
    {
        private readonly IServiceLogDataSource serviceLogDataSource;
        public ServiceLogProvider(IServiceLogDataSource serviceLogDataSource)
        {
            this.serviceLogDataSource = serviceLogDataSource;
        }
        public async Task<IEnumerable<LogMessage>> GetLogEntriesAsync()
        {
            return await Task.Run( () => serviceLogDataSource.LogMessages);
        }
    }
}
