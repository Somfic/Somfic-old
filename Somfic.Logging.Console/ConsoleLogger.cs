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
        internal ConsoleLogger(string categoryName, bool colorSupported, IConsoleTheme theme)
        {
            CategoryName = categoryName;
            ColorSupported = colorSupported;
            Theme = theme;

            if (Environment.GetEnvironmentVariable("NO_COLOR") != null)
            {
                ColorSupported = false;
            }

            // Make entire console the background color
            System.Console.Write("".Bg(Theme.Background));
            System.Console.Clear();
        }

        private string CategoryName { get; }
        private bool ColorSupported { get; }
        private IConsoleTheme Theme { get; }

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
            StringBuilder sb = new StringBuilder();

            //Color utility = Color.FromArgb(105, 105, 105);
            //Color utilityText = Color.FromArgb(176, 176, 176);
            //Color background = Color.Black;
            //Color secondary = Color.DarkGray;

            //Color trace = Color.FromArgb(135, 240, 105);
            //Color traceText = Color.FromArgb(190, 255, 171);

            //Color debug = Color.FromArgb(90, 115, 237);
            //Color debugText = Color.FromArgb(163, 179, 255);

            //Color info = Color.FromArgb(138, 198, 255);
            //Color infoText = Color.FromArgb(212, 234, 255);

            //Color warning = Color.FromArgb(255, 207, 51);
            //Color warningText = Color.FromArgb(252, 233, 169);

            //Color error = Color.FromArgb(250, 105, 105);
            //Color errorText = Color.FromArgb(255, 166, 166);

            //Color critical = Color.FromArgb(255, 117, 250);
            //Color criticalText = Color.FromArgb(255, 179, 253);

            int maxStackTrace = 4;


            // Tag
            StringBuilder tag = new StringBuilder();
            tag.Append("[".Fg(Theme.PunctuationText));

            // Timestamp
            tag.Append(DateTime.Now.ToString("HH").Fg(Theme.SecondaryText));
            tag.Append(":".Fg(Theme.PunctuationText));
            tag.Append(DateTime.Now.ToString("mm").Fg(Theme.SecondaryText));
            tag.Append(":".Fg(Theme.PunctuationText));
            tag.Append(DateTime.Now.ToString("ss").Fg(Theme.SecondaryText));

            tag.Append(" ");

            // Level
            Color accent;

            switch (logLevel)
            {
                case LogLevel.Trace:
                    accent = Theme.Trace;
                    tag.Append("TRACE".Fg(accent));
                    break;
                case LogLevel.Debug:
                    accent = Theme.Debug;
                    tag.Append("DEBUG".Fg(accent));
                    break;
                case LogLevel.Information:
                    accent = Theme.Information;
                    tag.Append(" INFO".Fg(accent));
                    break;
                case LogLevel.Warning:
                    accent = Theme.Warning;
                    tag.Append(" WARN".Fg(accent));
                    break;
                case LogLevel.Error:
                    accent = Theme.Error;
                    tag.Append("ERROR".Fg(accent));
                    break;
                case LogLevel.Critical:
                    accent = Theme.Critical;
                    tag.Append(" FAIL".Fg(accent));
                    break;
                case LogLevel.None:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }

            tag.Append("]".Fg(Theme.PunctuationText));
            tag.Append(Ansi.Reset(Theme));

            sb.Append(tag);
            sb.Append(" ");

            // Remove color codes from counting
            int tagLength = Regex.Replace(sb.ToString(), "\\u001b(.*?)m", "").Length;


            // Main message
            StringBuilder message = new StringBuilder();

            // Add the category if not null
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                message.Append("[".Fg(Theme.PunctuationText));
                message.Append(CategoryName.Fg(Theme.SecondaryText));
                message.Append("] ".Fg(Theme.PunctuationText));
            }

            // Add the actual message
            message.Append(formatter(state, exception).FormatJson().Fg(accent));

            foreach (string line in ToWrapped(message.ToString(), System.Console.WindowWidth - tagLength, tagLength))
            {
                sb.AppendLine(line.Fg(accent));
            }

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
                excep.Append(isFirstException ? "".Bg(accent).Fg(Theme.Background) : "".Fg(accent));
                excep.Append(isFirstException ? $" {ex.Type} " + Ansi.Reset(Theme) : $" {ex.Type}" + Ansi.Reset(Theme));
                excep.Append(" : ".Fg(Theme.PunctuationText));

                int exceptionTagLength = Regex.Replace(excep.ToString(), "\\u001b(.*?)m", "").Length;

                excep.Append(isFirstException ? "".Fg(accent) : "".Fg(Theme.SecondaryText));

                foreach (string line in ToWrapped(ex.Localised, System.Console.WindowWidth - exceptionTagLength, tagLength))
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
                stacks = ToWrapped(stacktrace, System.Console.WindowWidth - tagLength, tagLength).ToList();

                for (int i = 0; i < stacks.Count(); i++)
                {
                    stack.Append(string.Empty.PadRight(tagLength));
                    stack.AppendLine(stacks.ElementAt(i).Fg(Theme.SecondaryText));
                }

                sb.Append(stack);
            }

            string content = sb.ToString().TrimEnd('\n');
            System.Console.WriteLine(content);
        }

        private static IEnumerable<string> ToWrapped(string s, int maxLength, int indent)
        {
            string[] lines = s.Replace("\r\n", "\n").Split('\n');

            StringBuilder sb = new StringBuilder();
            foreach (string line in lines)
            {
                //if (string.IsNullOrWhiteSpace(line)) { continue; }

                int spaceLeft = maxLength;
                bool isFirstLine = true;


                foreach (string word in Regex.Split(line, @"(?<=[.,\-\:= ])"))
                {
                    int wordLength = word.RemoveAnsi().Length;

                    if (wordLength <= spaceLeft)
                    {
                        sb.Append(word);
                        spaceLeft -= wordLength;
                    }
                    else
                    {
                        if (!isFirstLine)
                        {
                            yield return $"{string.Empty.PadRight(indent)}{sb.ToString().Trim()}";
                        }
                        else
                        {
                            yield return sb.ToString();
                        }
                       
                        isFirstLine = false;
                        sb = new StringBuilder(word);
                        spaceLeft = maxLength - sb.Length;
                    }
                }

                if (!isFirstLine)
                {
                    yield return $"{string.Empty.PadRight(indent)}{sb.ToString().Trim()}";
                }
                else
                {
                    yield return sb.ToString();
                }
            }

            //string pattern = $@"(?<line>.{{1,{maxLength}}})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            //return Regex.Matches(s, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);
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

        public static string FormatJson(this string s)
        {
            return s;

            //// Check if it's actually JSON
            //s = Regex.Unescape(s).Replace("\\\"", "\"");

            //Color strings = Color.OrangeRed;
            //Color bools = Color.Blue;
            //Color ints = Color.Aqua;
            //Color utility = Color.DarkGray;

            //Color positive = Color.Green;
            //Color negative = Color.Red;

            //// Make ints pretty
            //s = Regex.Replace(s, "-?(?=[1-9]|0(?!\\d))\\d+(\\.\\d+)?([eE][+-]?\\d+)?", "$&".Fg(ints) + Ansi.Reset);

            //// Make strings pretty
            //s = Regex.Replace(s, "\"([^\"\\\\]*|\\\\[\"\\\\bfnrt\\/] |\\\\u[0 - 9a - f]{ 4})*\"", "$&".Fg(strings) + Ansi.Reset );

            //// Make bool values pretty
            //s = Regex.Replace(s, "true|false|null", "$&".Fg(bools) + Ansi.Reset);

            //// Fail messages
            //s = Regex.Replace(s, "(Not ok|Fail|Failure|Negative)", "$&".Fg(negative) + Ansi.Reset, RegexOptions.IgnoreCase);

            //// Success messages
            //s = Regex.Replace(s, "(Ok|Success|Positive)", Ansi.Bold + "$&".Fg(positive) + Ansi.Reset, RegexOptions.IgnoreCase);

            //// Utility messages
            //s = Regex.Replace(s, "[,\\-:/{}]", "$&".Fg(utility) + Ansi.Reset);

            return s;
        }

        public static string RemoveAnsi(this string s)
        {
            return Regex.Replace(s, "\\u001b(.*?)m", "");
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

        public static string Reset(IConsoleTheme theme) => "0".ToAnsi() + "".Bg(theme.Background) + "".Fg(theme.Text);

        public static string Blink => "5".ToAnsi();

        public static string Underline => "4".ToAnsi();

        public static string Bold => "1".ToAnsi();

        public static string Invert => "7".ToAnsi();
        public static string ResetInvert => "27".ToAnsi();

    }
}
