//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using Microsoft.Extensions.Logging;
//using Somfic.Logging.Console.Builder.Ansi;
//using Somfic.Logging.Console.Themes;

//namespace Somfic.Logging.Console
//{
//    / <summary>
//    / A Console logger
//    / </summary>
//    public class ConsoleLogger : ILogger
//    {
//        internal ConsoleLogger(string categoryName, bool useColor, IConsoleTheme theme)
//        {
//            CategoryName = categoryName;
//            UseColor = useColor;
//            Theme = theme;

//            if (DoesNotWantToUseColor()) { UseColor = false; }

//            FillConsoleBackgroundColor(theme.Background);
//        }

//        private string CategoryName { get; }
//        private bool UseColor { get; }
//        private IConsoleTheme Theme { get; }

//        / <inheritdoc />
//        public bool IsEnabled(LogLevel logLevel)
//        {
//            return true;
//        }

//        / <inheritdoc />
//        public IDisposable BeginScope<TState>(TState state)
//        {
//            return new NoopDisposable();
//        }

//        / <inheritdoc />
//        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
//        {
//            StringBuilder sb = new StringBuilder();

//            int maxStackTrace = 4;

//            Tag
//           StringBuilder tag = new StringBuilder();
//            tag.Append("[".Fg(Theme.PunctuationText));

//            Timestamp
//            tag.Append(DateTime.Now.ToString("HH").Fg(Theme.SecondaryText));
//            tag.Append(":".Fg(Theme.PunctuationText));
//            tag.Append(DateTime.Now.ToString("mm").Fg(Theme.SecondaryText));
//            tag.Append(":".Fg(Theme.PunctuationText));
//            tag.Append(DateTime.Now.ToString("ss").Fg(Theme.SecondaryText));

//            tag.Append(" ");

//            Level
//           Color accent;

//            switch (logLevel)
//            {
//                case LogLevel.Trace:
//                    accent = Theme.Trace;
//                    tag.Append("TRACE".Fg(accent));
//                    break;
//                case LogLevel.Debug:
//                    accent = Theme.Debug;
//                    tag.Append("DEBUG".Fg(accent));
//                    break;
//                case LogLevel.Information:
//                    accent = Theme.Information;
//                    tag.Append(" INFO".Fg(accent));
//                    break;
//                case LogLevel.Warning:
//                    accent = Theme.Warning;
//                    tag.Append(" WARN".Fg(accent));
//                    break;
//                case LogLevel.Error:
//                    accent = Theme.Error;
//                    tag.Append("ERROR".Fg(accent));
//                    break;
//                case LogLevel.Critical:
//                    accent = Theme.Critical;
//                    tag.Append(" FAIL".Fg(accent));
//                    break;
//                case LogLevel.None:
//                    return;
//                default:
//                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
//            }

//            tag.Append("]".Fg(Theme.PunctuationText));
//            tag.Append(Ansi.Reset(Theme));

//            sb.Append(tag);
//            sb.Append(" ");

//            Remove color codes from counting
//            int tagLength = Regex.Replace(sb.ToString(), "\\u001b(.*?)m", "").Length;


//            Main message
//            StringBuilder message = new StringBuilder();

//            Add the category if not null
//            if (!string.IsNullOrWhiteSpace(CategoryName))
//            {
//                message.Append("[".Fg(Theme.PunctuationText));
//                message.Append(CategoryName.Fg(Theme.SecondaryText));
//                message.Append("] ".Fg(Theme.PunctuationText));
//            }

//            Add the actual message
//            message.Append(formatter(state, exception).FormatJson().Fg(accent));

//            foreach (string line in ToWrapped(message.ToString(), System.Console.WindowWidth - tagLength, tagLength))
//            {
//                sb.AppendLine(line.Fg(accent));
//            }

//            Exception if not null
//            Exception thisException = exception;
//            bool isFirstException = true;
//            while (thisException != null)
//            {
//                StringBuilder excep = new StringBuilder();

//                bool isExceptionFirstLine = true;

//                LogException ex = new LogException(thisException);

//                Exception type
//                excep.Append(string.Empty.PadRight(tagLength));
//                excep.Append(isFirstException ? "".Bg(accent).Fg(Theme.Background) : "".Fg(accent));
//                excep.Append(isFirstException ? $" {ex.Type} " + Ansi.Reset(Theme) : $" {ex.Type}" + Ansi.Reset(Theme));
//                excep.Append(" : ".Fg(Theme.PunctuationText));

//                int exceptionTagLength = Regex.Replace(excep.ToString(), "\\u001b(.*?)m", "").Length;

//                excep.Append(isFirstException ? "".Fg(accent) : "".Fg(Theme.SecondaryText));

//                foreach (string line in ToWrapped(ex.Localised, System.Console.WindowWidth - exceptionTagLength, tagLength))
//                {
//                    excep.Append(string.Empty.PadRight(tagLength));
//                    if (!isExceptionFirstLine) { excep.Append(string.Empty.PadRight(exceptionTagLength)); }

//                    excep.AppendLine(line);
//                    isExceptionFirstLine = false;
//                }

//                sb.Append(excep);
//                isFirstException = false;
//                thisException = thisException.InnerException;
//            }

//            Stacktrace if not empty
//            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
//            {
//                StringBuilder stack = new StringBuilder();

//                List<string> stacks = exception.StackTrace.Split('\n').ToList();

//                if (stacks.Count() > maxStackTrace)
//                {
//                    stacks.Reverse();
//                    stacks = stacks.Take(maxStackTrace).ToList();
//                    stacks.Reverse();
//                }

//                stacks = stacks.Select(x => x.Trim()).ToList();

//                string stacktrace = string.Join(Environment.NewLine, stacks);
//                stacks = ToWrapped(stacktrace, System.Console.WindowWidth - tagLength, tagLength).ToList();

//                for (int i = 0; i < stacks.Count(); i++)
//                {
//                    stack.Append(string.Empty.PadRight(tagLength));
//                    stack.AppendLine(stacks.ElementAt(i).Fg(Theme.SecondaryText));
//                }

//                sb.Append(stack);
//            }

//            string content = sb.ToString().TrimEnd('\n');
//            System.Console.WriteLine(content);
//        }

//        private static IEnumerable<string> ToWrapped(string s, int maxLength, int indent)
//        {
//            string[] lines = s.Replace("\r\n", "\n").Split('\n');

//            StringBuilder sb = new StringBuilder();
//            foreach (string line in lines)
//            {
//                if (string.IsNullOrWhiteSpace(line)) { continue; }

//                int spaceLeft = maxLength;
//                bool isFirstLine = true;


//                foreach (string word in Regex.Split(line, @"(?<=[.,\-\:= ])"))
//                {
//                    int wordLength = word.RemoveAnsi().Length;

//                    if (wordLength <= spaceLeft)
//                    {
//                        sb.Append(word);
//                        spaceLeft -= wordLength;
//                    }
//                    else
//                    {
//                        if (!isFirstLine)
//                        {
//                            yield return $"{string.Empty.PadRight(indent)}{sb.ToString().Trim()}";
//                        }
//                        else
//                        {
//                            yield return sb.ToString();
//                        }

//                        isFirstLine = false;
//                        sb = new StringBuilder(word);
//                        spaceLeft = maxLength - sb.Length;
//                    }
//                }

//                if (!isFirstLine)
//                {
//                    yield return $"{string.Empty.PadRight(indent)}{sb.ToString().Trim()}";
//                }
//                else
//                {
//                    yield return sb.ToString();
//                }
//            }

//            string pattern = $@"(?<line>.{{1,{maxLength}}})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
//            return Regex.Matches(s, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);
//        }

//        private void FillConsoleBackgroundColor(Color color)
//        {
//            IAnsiStringBuilder colorReset = new AnsiStringBuilder();
//            colorReset.AddPart("").WithBackground(color);


//            WriteToConsole(colorReset.Build());
//            ResetConsole();
//        }

//        private void WriteToConsole(string content)
//        {
//            System.Console.WriteLine(content);
//        }

//        private void ResetConsole()
//        {
//            System.Console.Clear();
//        }

//        private bool DoesNotWantToUseColor() => Environment.GetEnvironmentVariable("NO_COLOR") != null;
//    }
//}
