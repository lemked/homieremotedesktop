using System;
using System.Collections.Generic;

namespace Homie.Common.Logging
{
    public static class Log
    {
        private static readonly IList<ILogger> loggers = new List<ILogger>();
        public static void Register(ILogger logger)
        {
            loggers.Add(logger);
        }

        public static void UnRegister(ILogger logger)
        {
            loggers.Remove(logger);
        }

        public static void Debug(string pLogMessage)
        {
            foreach (var logger in loggers)
            {
                logger.Debug(pLogMessage);
            }
        }

        public static void Debug(string pLogMessage, object pArgument)
        {
            Debug(String.Format(pLogMessage, pArgument));
        }

        public static void Info(string pLogMessage)
        {
            foreach (var logger in loggers)
            {
                logger.Info(pLogMessage);
            }
        }

        public static void Exception(Exception pException)
        {
            foreach (var logger in loggers)
            {
                logger.Error(pException);
            }
        }

        public static void Error(string pLogMessage)
        {
            foreach (var logger in loggers)
            {
                logger.Error(pLogMessage);
            }
        }
    }
}
