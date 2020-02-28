using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Somfic.Logging
{
    public class LogMessage
    {
        /// <summary>
        /// Creates a new log content with an exception.
        /// </summary>
        /// <param name="severity">The severity of the content.</param>
        /// <param name="content">The content of the content.</param>
        /// <param name="object">The object linked with this content.</param>
        /// <param name="stack">The stack of the message.</param>
        /// <param name="exception">The exception with which the content is linked.</param>
        internal LogMessage(Severity severity, string content, object @object, StackTrace stack, Exception exception)
        {
            Content = content == null ? string.Empty : content.ToString();
            Severity = severity;
            StackTrace = new LogStack(stack);
            Exception = new LogException(exception);
            Timestamp = DateTime.Now;
            Object = @object;
        }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// The severity of the content.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// The exception linked with this message. 
        /// </summary>
        public LogException Exception { get; }

        /// <summary>
        /// The object linked with this message.
        /// </summary>
        public object Object { get; }

        /// <summary>
        /// The stracktrace linked with this message.
        /// </summary>
        public LogStack StackTrace { get; }

        /// <summary>
        /// Timestamp of the content.
        /// </summary>
        public DateTime Timestamp { get; }

        //old
        [Obsolete("Use Content instead")]
        public string Message => Content;
    }

    public class LogException
    {
        internal LogException(Exception exception)
        {
            if (exception == null) { return; }

            string output = Regex.Replace(exception.GetType().Name, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());


            output = exception.GetType().Name == "Exception" ? "Exception" : output.Replace("exception", "");

            output = output.Trim();
            output = char.ToUpperInvariant(output[0]) + output.Substring(1);
            if (output == "I o") { output = "IO"; }
            Type = output;
            Exception = exception;
            Localised = null;

            switch (exception)
            {
                case ArgumentNullException a:
                    Localised = $"Argument '{a.ParamName}' cannot be null.";
                    break;
                case ArgumentOutOfRangeException a:
                    Localised = $"Argument '{a.ParamName}' is out of range.";
                    break;
                case FileNotFoundException a:
                    Localised = $"File '{a.FileName}' could not be found.";
                    break;
                case COMException a:
                    Localised = $"HRESULT {a.ErrorCode}.";
                    break;
                case SEHException a:
                    Localised = !a.CanResume()
                        ? $"HRESULT {a.ErrorCode}. This exception can not be recovered from."
                        : $"HRESULT {a.ErrorCode}. This exception can be recovered from.";
                    break;
            }
        }

        public string Localised { get; }

        public string Type { get; }

        public Exception Exception { get; }
    }

    public class LogStack
    {
        internal LogStack(StackTrace stack)
        {
            if (stack == null) { return; }
            Frames = stack.GetFrames().Skip(2).ToList();
        }

        public override string ToString()
        {
            return "";
        }

        public IList<StackFrame> Frames { get; }
    }
}