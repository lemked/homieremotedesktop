using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

using Homie.Model.Logging;

namespace Homie.Common.Interfaces
{
    using System.ServiceProcess;

    [ServiceContract]
    public interface IServiceLogProvider : IWebServiceSession
    {
        [OperationContract]
        Task<IEnumerable<LogMessage>> GetLogEntriesAsync();
    }
}
