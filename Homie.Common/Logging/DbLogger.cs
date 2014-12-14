using System;
using Homie.Model.Logging;

namespace Homie.Common.Logging
{
    public class DbLogger : LoggerBase
    {
        private DbServiceLogDataSource logDataSource;
        public override void Write(string pLogMessage, LogLevel pLogLevel)
        {
            if (logDataSource == null)
            {
                logDataSource = new DbServiceLogDataSource();
            }

            var logMessage = new LogMessage() {Level = pLogLevel, LogTime = DateTime.Now, Text = pLogMessage};
            logDataSource.Add(logMessage);
        }
    }
}
