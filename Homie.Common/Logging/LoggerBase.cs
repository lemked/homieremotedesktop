using System;

using Homie.Model.Logging;

namespace Homie.Common.Logging
{
    /// <summary>
    /// Abstract base class for logger implementation.
    /// </summary>
    /// <author>Daniel Lemke - lemked@web.de</author>
    public abstract class LoggerBase : ILogger
    {
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Writes the specified log message with log level Debug.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Debug(string pLogMessage)
        {
            if (this.LogLevel <= LogLevel.Debug)
            {
                Write(pLogMessage, LogLevel.Debug);
            }
        }

        /// <summary>
        /// Writes the specified log message with log level Debug.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <param name="pArgument">An argument to substitute in the log message (as if using String.Format()).</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Debug(string pLogMessage, object pArgument)
        {
            Debug(String.Format(pLogMessage, pArgument));
        }

        /// <summary>
        /// Writes the specified log message with log level Info.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Info(string pLogMessage)
        {
            if (this.LogLevel <= LogLevel.Info)
            {
                Write(pLogMessage, LogLevel.Info);    
            }
        }

        /// <summary>
        /// Writes the specified log message with log level Warning.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Warning(string pLogMessage)
        {
            if (this.LogLevel <= LogLevel.Warning)
            {
                Write(pLogMessage, LogLevel.Warning);
            }
        }

        /// <summary>
        /// Writes the specified p exception with log level Error. The exception is formatted into its string representation.
        /// </summary>
        /// <param name="pException">The p exception.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Error(Exception pException)
        {
            if (this.LogLevel <= LogLevel.Error)
            {
                // Format the exception.
                string lLogMessage = pException.Message + Environment.NewLine;
                lLogMessage += "------------------------------------------------------------------" + Environment.NewLine;
                lLogMessage += pException.StackTrace;
                lLogMessage += Environment.NewLine;
                if (pException.InnerException != null)
                {
                    lLogMessage += pException.InnerException.Message + Environment.NewLine;
                    lLogMessage += pException.InnerException.StackTrace + Environment.NewLine;
                }
                lLogMessage += "------------------------------------------------------------------";

                // Write the exception.
                Write(lLogMessage, LogLevel.Error);
            }
        }

        public virtual void Error(string pLogMessage)
        {
            if (this.LogLevel <= LogLevel.Error)
            {
                Write(pLogMessage, LogLevel.Error);
            }
        }

        /// <summary>
        /// Writes the specified log message using the specified log level.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <param name="pLogLevel">The log level.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        public abstract void Write(string pLogMessage, LogLevel pLogLevel);
    }
}
