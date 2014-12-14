using System;
using System.IO;
using System.Reflection;

using Homie.Model.Logging;

namespace Homie.Common.Logging
{
    public class FileLogger : LoggerBase
    {
        private const string cLogFilename = Constants.AppName + ".log";

        private readonly string filePath;

        public FileLogger()
        {
            // create the path by using the location of the current executing assembly
            string lPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!String.IsNullOrEmpty(lPath) && Directory.Exists(lPath))
            {
                filePath = Path.Combine(lPath, cLogFilename);
            }
        }

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public override void Write(string pLogMessage, LogLevel pLogLevel)
        {
            string logLevelString = string.Empty;
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
            }

            string lText = DateTime.Now + " " + logLevelString + " " + pLogMessage;
            try
            {
                File.AppendAllText(filePath, lText + Environment.NewLine);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                // Do nothing else. Logging should not cause an application error.
            }
        }
    }
}
