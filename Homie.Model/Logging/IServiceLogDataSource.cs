using System.Collections.Generic;

namespace Homie.Model.Logging
{
    public interface IServiceLogDataSource
    {
        IList<LogMessage> LogMessages { get; }
    }
}