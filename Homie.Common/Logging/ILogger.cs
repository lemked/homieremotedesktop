using System;

using Homie.Model.Logging;

namespace Homie.Common.Logging
{
    /// <summary>
    /// Defines methods for logging purposes.
    /// </summary>
    /// <author>Daniel Lemke - lemked@web.de</author>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        /// <author>Daniel Lemke - lemked@web.de</author>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// Writes the specified log message with log level Debug.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Debug(string pLogMessage);

        /// <summary>
        /// Writes the specified log message with log level Debug.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <param name="pArgument">An argument to substitute in the log message (as if using String.Format()).</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Debug(string pLogMessage, object pArgument);

        /// <summary>
        /// Writes the specified log message with log level Info.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Info(string pLogMessage);

        /// <summary>
        /// Writes the specified log message with log level Warning.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Warning(string pLogMessage);

        /// <summary>
        /// Writes the specified exception with log level Error. The exception is formatted into its string representation.
        /// </summary>
        /// <param name="pException">The p exception.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Error(Exception pException);

        /// <summary>
        /// Writes the specified log message with log level Error.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Error(string pLogMessage);

        /// <summary>
        /// Writes the specified log message using the specified log level.
        /// </summary>
        /// <param name="pLogMessage">The log message.</param>
        /// <param name="pLogLevel">The log level.</param>
        /// <author>Daniel Lemke - lemked@web.de</author>
        void Write(string pLogMessage, LogLevel pLogLevel);
    }
}
