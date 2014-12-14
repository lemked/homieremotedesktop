using System;
using Homie.Model.Logging;

namespace Homie.Common.Logging
{
    public class ConsoleLogger : LoggerBase
    {
        public override void Write(string pLogMessage, LogLevel pLogLevel)
        {
            string logLevelString;
            switch (pLogLevel)
            {
                case LogLevel.Debug:
                    logLevelString = "DBG";
                    break;
                case LogLevel.Info:
                    logLevelString = "INF";
                    break;
                case LogLevel.Warning:
                    logLevelString = "WRN";
                    break;
                case LogLevel.Error:
                    logLevelString = "ERR";
                    break;
                default:
                    logLevelString = string.Empty;
                    break;
            }

            Console.WriteLine(DateTime.Now + " " + logLevelString + " " + pLogMessage);
        }
    }
}
