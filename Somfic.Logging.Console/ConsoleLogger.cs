using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Somfic.Logging.Console
{
    /// <summary>
    /// A Console logger
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        internal ConsoleLogger(string categoryName, bool colorSupported)
        {
            CategoryName = categoryName;
            ColorSupported = colorSupported;

            if (Environment.GetEnvironmentVariable("NO_COLOR") != null)
            {
                ColorSupported = false;
            }
        }

        private string CategoryName { get; }
        private bool ColorSupported { get; }

        /// <summary>
        /// Whether a specific level is enabled
        /// </summary>
        /// <param name="logLevel">The level to check</param>
        /// <returns>Whether this level is enabled</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Begins the scope of this logger
        /// </summary>
        /// <typeparam name="TState">The class type</typeparam>
        /// <param name="state">The class</param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            bool isFirstLine = true;

            StringBuilder sb = new StringBuilder();

            Color utility = Color.FromArgb(105, 105, 105);
            Color utilityText = Color.FromArgb(176, 176, 176);
            Color background = Color.Black;
            Color secondary = Color.DarkGray;

            Color trace = Color.FromArgb(135, 240, 105);
            Color traceText = Color.FromArgb(190, 255, 171);

            Color debug = Color.FromArgb(90, 115, 237);
            Color debugText = Color.FromArgb(163, 179, 255);

            Color info = Color.FromArgb(138, 198, 255);
            Color infoText = Color.FromArgb(212, 234, 255);

            Color warning = Color.FromArgb(255, 207, 51);
            Color warningText = Color.FromArgb(252, 233, 169);

            Color error = Color.FromArgb(250, 105, 105);
            Color errorText = Color.FromArgb(255, 166, 166);

            Color critical = Color.FromArgb(255, 117, 250);
            Color criticalText = Color.FromArgb(255, 179, 253);

            int maxStackTrace = 4;


            // Tag
            StringBuilder tag = new StringBuilder();
            tag.Append("[".Fg(utility));

            // Timestamp
            tag.Append(DateTime.Now.ToString("HH").Fg(utilityText));
            tag.Append(":".Fg(utility));
            tag.Append(DateTime.Now.ToString("mm").Fg(utilityText));
            tag.Append(":".Fg(utility));
            tag.Append(DateTime.Now.ToString("ss").Fg(utilityText));

            tag.Append(" ");

            // Level
            Color accent;
            Color text;

            switch (logLevel)
            {
                case LogLevel.Trace:
                    accent = trace;
                    text = traceText;
                    tag.Append("TRACE".Fg(text));
                    break;
                case LogLevel.Debug:
                    accent = debug;
                    text = debugText;
                    tag.Append("DEBUG".Fg(text));
                    break;
                case LogLevel.Information:
                    accent = info;
                    text = infoText;
                    tag.Append(" INFO".Fg(text));
                    break;
                case LogLevel.Warning:
                    accent = warning;
                    text = warningText;
                    tag.Append(" WARN".Fg(text));
                    break;
                case LogLevel.Error:
                    accent = error;
                    text = errorText;
                    tag.Append("ERROR".Fg(text));
                    break;
                case LogLevel.Critical:
                    accent = critical;
                    text = criticalText;
                    tag.Append(" FAIL".Fg(text));
                    break;
                case LogLevel.None:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }

            tag.Append("]".Fg(utility));
            tag.Append(Ansi.Reset);

            sb.Append(tag);
            sb.Append(" ");

            // Remove color codes from counting
            int tagLength = Regex.Replace(sb.ToString(), "\\u001b(.*?)m", "").Length;

            StringBuilder source = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                source.Append("[".Fg(utility));
                source.Append(CategoryName.Fg(utilityText));
                source.Append("] ".Fg(utility));
                source.Append("".Fg(text));

                sb.Append(source);
            }
            string cleanSource = Regex.Replace(source.ToString(), "\\u001b(.*?)m", "");

            // Main message
            StringBuilder message = new StringBuilder();

            string msg = formatter(state, exception);
            foreach (string line in ToWrapped(cleanSource + msg, System.Console.WindowWidth - tagLength))
            {
                if (!isFirstLine)
                {
                    message.Append(string.Empty.PadRight(tagLength));
                    message.AppendLine(line.Fg(text));
                }
                else
                {
                    message.AppendLine(line.Substring(cleanSource.Length).Fg(text));
                }
                isFirstLine = false;
            }

            sb.Append(message);

            // Exception if not null
            Exception thisException = exception;
            bool isFirstException = true;
            while (thisException != null)
            {
                StringBuilder excep = new StringBuilder();

                bool isExceptionFirstLine = true;

                LogException ex = new LogException(thisException);

                // Exception type
                excep.Append(string.Empty.PadRight(tagLength));
                excep.Append(isFirstException ? "".Bg(accent).Fg(background) : "".Fg(accent));

                excep.Append($" {ex.Type} " + Ansi.Reset);
                excep.Append(" : ".Fg(utility));

                int exceptionTagLength = Regex.Replace(excep.ToString(), "\\u001b(.*?)m", "").Length;

                excep.Append("".Fg(accent));

                foreach (string line in ToWrapped(ex.Localised, System.Console.WindowWidth - exceptionTagLength))
                {
                    //excep.Append(string.Empty.PadRight(tagLength));
                    if (!isExceptionFirstLine) { excep.Append(string.Empty.PadRight(exceptionTagLength)); }

                    excep.AppendLine(line);
                    isExceptionFirstLine = false;
                }

                sb.Append(excep);
                isFirstException = false;
                thisException = thisException.InnerException;
            }

            // Stacktrace if not empty
            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
            {
                StringBuilder stack = new StringBuilder();

                List<string> stacks = exception.StackTrace.Split('\n').ToList();

                if (stacks.Count() > maxStackTrace)
                {
                    stacks.Reverse();
                    stacks = stacks.Take(maxStackTrace).ToList();
                    stacks.Reverse();
                }

                stacks = stacks.Select(x => x.Trim()).ToList();

                string stacktrace = string.Join(Environment.NewLine, stacks);
                stacks = ToWrapped(stacktrace, System.Console.WindowWidth - tagLength).ToList();

                for (int i = 0; i < stacks.Count(); i++)
                {
                    stack.Append(string.Empty.PadRight(tagLength));
                    stack.AppendLine(stacks.ElementAt(i).Fg(secondary));
                }

                sb.Append(stack);
            }

            string content = sb.ToString().TrimEnd('\n');
            System.Console.WriteLine(content);
        }

        private static IEnumerable<string> ToWrapped(string s, int maxLength)
        {
            string pattern = $@"(?<line>.{{1,{maxLength}}})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            return Regex.Matches(s, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);
        }
    }

    //todo: make this public
    internal static class Ansi
    {
        private static string ToAnsi(this string code)
        {
            string result = $"\u001b[{code}m";
            return result;
        }

        public static string Fg(this string s, int r, int g, int b)
        {
            return s.Fg(Color.FromArgb(r, g, b));
        }

        public static string Fg(this string s, string hex)
        {
            return s.Fg(Color.FromArgb(int.Parse(hex.Replace("#", ""), NumberStyles.HexNumber)));
        }

        public static string Fg(this string s, Color color)
        {
            return $"38;2;{color.R};{color.G};{color.B}".ToAnsi() + s;
        }

        public static string Bg(this string s, int r, int g, int b)
        {
            return s.Bg(Color.FromArgb(r, g, b));
        }

        public static string Bg(this string s, string hex)
        {
            return s.Bg(Color.FromArgb(int.Parse(hex.Replace("#", ""), NumberStyles.HexNumber)));
        }

        public static string Bg(this string s, Color color)
        {
            return $"48;2;{color.R};{color.G};{color.B}".ToAnsi() + s;
        }

        public static string Reset => "0".ToAnsi();

        public static string Invert => "7".ToAnsi();
        public static string ResetInvert => "27".ToAnsi();

    }
}
