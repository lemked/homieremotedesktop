using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

using Homie.Model.Logging;

namespace Homie.Common.Interfaces
{
    [ServiceContract]
    public interface IServiceLogProvider 
    {
        [OperationContract]
        Task<IEnumerable<LogMessage>> GetLogEntriesAsync();
    }
}
