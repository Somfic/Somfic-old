using Microsoft.Extensions.Logging;

using Somfic.VoiceAttack.Log;
using Somfic.VoiceAttack.Proxy;

using System;
using System.Text.RegularExpressions;

namespace Somfic.Logging.VoiceAttack
{
    /// <summary>
    /// A pretty console logger
    /// </summary>
    public class VoiceAttackLogger : ILogger
    {
        private readonly VoiceAttackProxy proxy;

        internal VoiceAttackLogger(dynamic vaProxy)
        {
            proxy = new VoiceAttackProxy(vaProxy);
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            VoiceAttackColor color;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    color = VoiceAttackColor.Gray;
                    break;
                case LogLevel.Information:
                    color = VoiceAttackColor.Blue;
                    break;
                case LogLevel.Warning:
                    color = VoiceAttackColor.Yellow;
                    break;
                case LogLevel.Error:
                    color = VoiceAttackColor.Red;
                    break;
                case LogLevel.Critical:
                    color = VoiceAttackColor.Purple;
                    break;
                case LogLevel.None:
                    color = VoiceAttackColor.Blank;
                    break;
                default:
                    color = VoiceAttackColor.Black;
                    break;
            }

            string message = formatter(state, exception);

            proxy.Log.Write(message, color);

            if (logLevel >= LogLevel.Error)
            {
                proxy.Log.Write($"Error: [{GetPrettyExceptionName(exception)}] {exception.Message}", color);
            }
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Information;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        private string GetPrettyExceptionName(Exception ex)
        {
            string output = Regex.Replace(ex.GetType().Name, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());

            output = ex.GetType().Name == "Exception" ? "Exception" : output.Replace("exception", "");

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
                case "Sql":
                    output = "SQL";
                    break;
            }

            return output;
        }
    }
}
