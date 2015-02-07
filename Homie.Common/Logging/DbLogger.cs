using System;
using Homie.Model.Logging;

namespace Homie.Common.Logging
{
    public class DbLogger : LoggerBase
    {
        private DbServiceLogDataSource logDataSource;

        public DbLogger()
        {
            logDataSource = new DbServiceLogDataSource();
        }

        public override void Error(Exception pException)
        {
            var logMessage = new LogMessage()
            {
                Level = LogLevel.Error,
                LogTime = DateTime.Now,
                Text = pException.Message,
                Data = pException
            };
            logDataSource.Add(logMessage);
        }
        public override void Write(string pLogMessage, LogLevel pLogLevel)
        {
            var logMessage = new LogMessage()
            {
                Level = pLogLevel, 
                LogTime = DateTime.Now, 
                Text = pLogMessage
            };
            logDataSource.Add(logMessage);
        }
    }
}
