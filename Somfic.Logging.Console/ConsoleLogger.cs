using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Somfic.Logging.Console.Builder;
using Somfic.Logging.Console.Builder.Ansi;
using Somfic.Logging.Console.Themes;

namespace Somfic.Logging.Console
{
    /// <summary>
    /// A Console logger
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        internal ConsoleLogger(string categoryName, bool useColor, IConsoleTheme theme)
        {
            CategoryName = categoryName;
            UseColor = useColor;
            Theme = theme;

            if (DoesNotWantToUseColor()) { UseColor = false; }

            FillConsoleBackgroundWithColor(theme.Background);
        }

        private string CategoryName { get; }
        private bool UseColor { get; }
        private IConsoleTheme Theme { get; }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception ex,
            Func<TState, Exception, string> formatter)
        {
            (string levelAsString, Color levelAsColor) = GetLocalisationFromLevel(Theme, logLevel);

            IAnsiStringBuilder stamp = CreateStamp(levelAsString, levelAsColor);
            IAnsiStringBuilder tag = CreateTag(CategoryName, eventId, levelAsColor);
            IAnsiStringBuilder sourceTag = CreateSourceTag(eventId, levelAsColor);
            IAnsiStringBuilder content = CreateContent(formatter, state, ex, levelAsColor);
            IAnsiStringBuilder exception = CreateException(ex, levelAsColor);

            int indentSize = stamp.BuildClean().Length + 1;
            int maxWidth = System.Console.WindowWidth - indentSize;

            string stampText;
            string tagText;
            string sourceTagText;
            string contentText;
            string exceptionText;

            if (UseColor)
            {
                stampText = stamp.Build();
                tagText = tag.BuildWithWrap(maxWidth);
                sourceTagText = sourceTag.BuildWithWrap(maxWidth);
                contentText = content.BuildWithWrap(maxWidth);
                exceptionText = exception.BuildWithWrap(maxWidth);
            }
            else
            {
                stampText = stamp.BuildClean();
                tagText = tag.BuildCleanWithWrap(maxWidth);
                sourceTagText = sourceTag.BuildCleanWithWrap(maxWidth);
                contentText = content.BuildCleanWithWrap(maxWidth);
                exceptionText = content.BuildCleanWithWrap(maxWidth);
            }

            StringBuilder logEntry = new StringBuilder();
            logEntry.Append(stampText);
            logEntry.Append(" ");
            logEntry.Append(tagText).AppendLine();
            logEntry.Append(WithIndent(contentText, indentSize));
            logEntry.AppendLine(WithIndent(exceptionText, indentSize));

            WriteToConsole(logEntry.ToString().TrimEnd());
        }

        private IAnsiStringBuilder CreateException(Exception ex, Color accentColor)
        {
            if (ex == null) { return new AnsiStringBuilder(); }

            IAnsiStringBuilder exception = new AnsiStringBuilder();

            string stack = ex.StackTrace;

            while (ex != null)
            {
                string exceptionName = GetPrettyExceptionName(ex);


                exception.AddPart($" {exceptionName} ").With(Theme.Background).WithBackground(accentColor);
                exception.AddPart(" - ").With(Theme.Punctuation).WithBackground(Theme.Background);
                exception.AddPart(ex.Message.Trim()).With(accentColor);
                exception.AddPart(Environment.NewLine);

                if (ex.Data.Count > 0)
                {
                    exception.AddPart("[").With(Theme.Punctuation);
                    bool isFirstEntry = true;

                    foreach (DictionaryEntry o in ex.Data)
                    {
                        if (!isFirstEntry)
                        {
                            exception.AddPart(", ").With(Theme.Punctuation);
                        }

                        exception.AddPart("{").With(Theme.Punctuation);
                        exception.AddPart(o.Key.ToString()).With(Theme.SecondaryText);
                        exception.AddPart(": ").With(Theme.Punctuation);
                        exception.AddPart(o.Value.ToString()).With(Theme.Text);
                        exception.AddPart("}").With(Theme.Punctuation);
                        isFirstEntry = false;
                    }
                    exception.AddPart("]").With(Theme.Punctuation);
                    exception.AddPart(Environment.NewLine);
                }

                ex = ex.InnerException;
            }

            if (!string.IsNullOrWhiteSpace(stack))
            {
                IEnumerable<string> stackLines = stack.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                stackLines = stackLines.Reverse();

                for (int index = 0; index < stackLines.Count(); index++)
                {
                    string stackLine = stackLines.ElementAt(index).Trim();
                    exception.AddPart((index + 1).ToString()).With(Theme.SecondaryText);
                    exception.AddPart(": ").With(Theme.Punctuation);
                    exception.AddPart(stackLine.Trim()).With(Theme.SecondaryText);
                    exception.AddPart(Environment.NewLine);
                }
            }

            exception.EndWith(AnsiCode.Reset);
            exception.EndWith(Theme.Text);
            exception.EndWithBackground(Theme.Background);

            return exception;
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
            }

            return output;
        }

        private IAnsiStringBuilder CreateContent<TState>(Func<TState, Exception, string> formatter, TState state, Exception exception, Color accentColor)
        {
            string message = formatter(state, exception);

            //(message)
            IAnsiStringBuilder content = new AnsiStringBuilder();
            content.AddPart(message).With(accentColor);

            content.EndWith(AnsiCode.Reset);
            content.EndWith(Theme.Text);
            content.EndWithBackground(Theme.Background);

            return content;
        }

        private IAnsiStringBuilder CreateStamp(string level, Color accentColor)
        {
            DateTime now = DateTime.Now;
            string levelAsString = $"{level,5}".ToUpper();

            //[12:04:12.233  INFO]
            IAnsiStringBuilder stamp = new AnsiStringBuilder();
            stamp.AddPart("[").With(Theme.Punctuation);
            stamp.AddPart($"{now.Hour:00}").With(Theme.SecondaryText);
            stamp.AddPart(":").With(Theme.Punctuation);
            stamp.AddPart($"{now.Minute:00}").With(Theme.SecondaryText);
            stamp.AddPart(":").With(Theme.Punctuation);
            stamp.AddPart($"{now.Second:00}").With(Theme.SecondaryText);
            stamp.AddPart(".").With(Theme.Punctuation);
            stamp.AddPart($"{now.Millisecond:000}").With(Theme.SecondaryText);
            stamp.AddPart(" ");
            stamp.AddPart(levelAsString).With(accentColor);
            stamp.AddPart("]").With(Theme.Punctuation);

            stamp.EndWith(AnsiCode.Reset);
            stamp.EndWith(Theme.Text);
            stamp.EndWithBackground(Theme.Background);

            return stamp;
        }

        private IAnsiStringBuilder CreateSourceTag(EventId eventId, Color accentColor)
        {
            AnsiStringBuilder tag = new AnsiStringBuilder();

            if (eventId.Id != 0)
            {
                tag.AddPart(" [").With(Theme.Punctuation);
                tag.AddPart($"{eventId.Id:000}").With(Theme.SecondaryText);
                tag.AddPart("]").With(Theme.Punctuation);
            }

            if (!string.IsNullOrWhiteSpace(eventId.Name))
            {
                tag.AddPart($" {eventId.Name}").With(Theme.Text);
            }

            tag.EndWith(AnsiCode.Reset);
            tag.EndWith(Theme.Text);
            tag.EndWithBackground(Theme.Background);

            return tag;
        }

        private IAnsiStringBuilder CreateTag(string category, EventId eventId, Color accentColor)
        {
            List<string> categoryParts = category.Split('.').ToList();

            //[Somfic.Logging.Console] EventName 1
            IAnsiStringBuilder tag = new AnsiStringBuilder();
            tag.AddPart("[").With(Theme.Punctuation);
            foreach (string categoryPart in categoryParts.Take(categoryParts.Count - 1))
            {
                tag.AddPart(categoryPart).With(Theme.SecondaryText);
                tag.AddPart(".").With(Theme.Punctuation);
            }
            tag.AddPart(categoryParts.Last()).With(Theme.Text);
            tag.AddPart("]").With(Theme.Punctuation);

            tag.EndWith(AnsiCode.Reset);
            tag.EndWith(Theme.Text);
            tag.EndWithBackground(Theme.Background);

            return tag;
        }

        private static (string, Color) GetLocalisationFromLevel(IConsoleTheme theme, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return ("Trace", theme.Trace);
                case LogLevel.Debug:
                    return ("Debug", theme.Debug);
                case LogLevel.Information:
                    return ("Info", theme.Information);
                case LogLevel.Warning:
                    return ("Warn", theme.Warning);
                case LogLevel.Error:
                    return ("Error", theme.Error);
                case LogLevel.Critical:
                    return ("Fail", theme.Critical);
                case LogLevel.None:
                    return ("None", theme.SecondaryText);
                default:
                    return ("?????", theme.SecondaryText);
            }
        }

        private string WithIndent(string content, int size)
        {
            string indent = string.Empty.PadLeft(size);

            StringBuilder sb = new StringBuilder();
            foreach (string line in content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                sb.Append(indent);
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        private void FillConsoleBackgroundWithColor(Color color)
        {
            IAnsiStringBuilder colorReset = new AnsiStringBuilder();
            colorReset.AddPart().WithBackground(color);

            WriteToConsole(colorReset.Build());
            ResetConsole();
        }

        private void WriteToConsole(string content)
        {
            System.Console.WriteLine(content);
        }

        private void ResetConsole()
        {
            System.Console.Clear();
        }

        private bool DoesNotWantToUseColor()
        {
            return Environment.GetEnvironmentVariable("NO_COLOR") != null;
        }
    }
}
