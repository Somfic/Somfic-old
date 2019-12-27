using System;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Somfic.Logging
{
    public class LogMessage
    {
        /// <summary>
        /// Creates a new log content.
        /// </summary>
        /// <param name="content">The content of the content.</param>
        /// <param name="severity">The severity of the content.</param>
        public LogMessage(string content, Severity severity)
        {
            Content = content;
            Severity = severity;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Creates a new log content with an exception.
        /// </summary>
        /// <param name="content">The content of the content.</param>
        /// <param name="severity">The severity of the content.</param>
        /// <param name="exception">The exception with which the content is linked.</param>
        public LogMessage(string content, Severity severity, Exception exception)
        {
            Content = content;
            Severity = severity;
            Exception = null;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// The content of the content.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// The severity of the content.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// The exception linked with this messsage. 
        /// </summary>
        public CustomException Exception { get; }

        /// <summary>
        /// Timestamp of the content.
        /// </summary>
        public DateTime Timestamp { get; }
    }

    public class CustomException
    {
        public CustomException(Exception exception)
        {
            if(exception == null) { return; }
            string output = Regex.Replace(exception.GetType().Name, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());
            output = char.ToUpperInvariant(output[0]) + output.Substring(1);
            Type = output;
            exception = Exception;
        }

        public string Type { get; }

        public Exception Exception { get; }
    }
}