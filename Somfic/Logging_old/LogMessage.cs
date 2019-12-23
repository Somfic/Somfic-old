using System;

namespace Somfic.Logging
{
    public class LogMessage
    {
        /// <summary>
        /// Creates a new log message.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="severity">The severity of the message.</param>
        public LogMessage(string message, Severity severity)
        {
            Message = message;
            Severity = severity;
        }

        /// <summary>
        /// Creates a new log message with an exception.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="exception">The exception with which the message is linked.</param>
        public LogMessage(string message, Severity severity, Exception exception)
        {
            Message = message;
            Severity = severity;
            Exception = exception;
        }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The severity of the message.
        /// </summary>
        public Severity Severity { get; private set; }

        /// <summary>
        /// The exception linked with this messsage. 
        /// </summary>
        public Exception Exception { get; private set; }
    }
}