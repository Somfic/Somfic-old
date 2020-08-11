using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Somfic.Logging
{
    /// <summary>
    /// LogException helper class
    /// </summary>
    public class LogException
    {
        /// <summary>
        /// Creates a new LogException class, should not be manually instantiated
        /// </summary>
        /// <param name="exception">The exception</param>
        public LogException(Exception exception)
        {
            if (exception == null) { return; }

            string output = Regex.Replace(exception.GetType().Name, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());


            output = exception.GetType().Name == "Exception" ? "Exception" : output.Replace("exception", "");

            output = output.Trim();
            output = char.ToUpperInvariant(output[0]) + output.Substring(1);
            switch (output)
            {
                case "I o":
                    output = "IO";
                    break;
                case "My sql":
                    output = "MySQL";
                    break;
            }

            Type = output;
            Exception = exception;
            Localised = null;

            Localised = exception switch
            {
                ArgumentNullException a => $"Argument '{a.ParamName}' cannot be null.",
                ArgumentOutOfRangeException a => $"Argument '{a.ParamName}' is out of range.",
                FileNotFoundException a => $"File '{a.FileName}' could not be found.",
                COMException a => $"HRESULT {a.ErrorCode}.",
                SEHException a => !a.CanResume()
                    ? $"HRESULT {a.ErrorCode}. This exception can not be recovered from."
                    : $"HRESULT {a.ErrorCode}. This exception can be recovered from.",
                _ => exception.Message
            };
        }

        /// <summary>
        /// The localised description
        /// </summary>
        public string Localised { get; }

        /// <summary>
        /// The type of exception
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The original exception
        /// </summary>
        public Exception Exception { get; }
    }
}
