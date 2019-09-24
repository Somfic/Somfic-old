using Somfic.Logging.Theme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Somfic.Logging
{
    public static class Logger
    {
        private static string DirectoryPath = "";
        private static string LogFile = "";
        private static Severity MinType = Severity.Debug;
        private static ITheme Theme = Themes.Dark;
        private static bool SoftWrap = false;
        private static bool Format = false;

        private static int maxLenght => Console.WindowWidth - 11;

        /// <summary>
        /// Creates a new log entry with a severity of Info.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public static void Log(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Info));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Info.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the log.</param>
        public static void Log(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Info, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Success.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public static void Success(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Success));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Success.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the log.</param>
        public static void Success(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Success, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Warning.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public static void Warning(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Warning));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Warning.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the warning.</param>
        public static void Warning(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Warning, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Error.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public static void Error(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Error));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Error.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the error.</param>
        public static void Error(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Error, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Debug.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public static void Debug(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Debug));
        }


        /// <summary>
        /// Creates a new log entry with a severity of Debug.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the debug.</param>
        public static void Debug(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Debug, ex));
        }

        /// <summary>
        /// Whether to use soft wrap.
        /// </summary>
        public static void UseSoftWrap()
        {
            SoftWrap = true;
        }

        /// <summary>
        /// Whether to automatically format text.
        /// </summary>
        public static void UseFormat()
        {
            Format = true;
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public static void UseConsole(Severity type)
        {
            MinType = type;
            UseConsole();
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public static void UseConsole(ITheme theme)
        {
            Theme = theme;
            UseConsole();
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public static void UseConsole(ITheme theme, Severity type)
        {
            MinType = type;
            Theme = theme;
            UseConsole();
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public static void UseConsole()
        {
            Console.BackgroundColor = Theme.BackGround;
            Console.ForegroundColor = Theme.Main;
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;

            LogEvent += (sender, arg) =>
            {
                if (arg.Severity <= MinType)
                {

                    switch (arg.Severity)
                    {
                        case Severity.Info:
                            Write(arg.Severity, Theme.Main, arg.Message, arg.Exception);
                            break;

                        case Severity.Success:
                            Write(arg.Severity, Theme.Success, arg.Message, arg.Exception);
                            break;

                        case Severity.Warning:
                            Write(arg.Severity, Theme.Warning, arg.Message, arg.Exception);
                            break;

                        case Severity.Error:
                            Write(arg.Severity, Theme.Error, arg.Message, arg.Exception);
                            break;

                        case Severity.Debug:
                            Write(arg.Severity, Theme.Debug, arg.Message, arg.Exception);
                            break;

                        case Severity.Special:
                            Write(arg.Severity, Theme.Special, arg.Message, arg.Exception);
                            break;
                    }
                };
            };


        }

        /// <summary>
        /// Outputs the logs to an external log file.
        /// </summary>
        /// <param name="directory">The directory in which to save the log files.</param>
        public static void UseLogFile(string directory, string name = "log")
        {
            if (!Directory.Exists(directory))
            {
                Debug($"Folder '{directory}' does not exist, creating it.");
                Directory.CreateDirectory(directory);
            }

            DirectoryPath = directory;

            string date = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";

            int i = 1;
            while (true)
            {
                if (!File.Exists(DirectoryPath + $"\\{name}.{date}.{i}.log"))
                {
                    LogFile = DirectoryPath + $"\\{name}.{date}.{i}.log";
                    break;
                }
                else
                {
                    i++;
                }
            }

            try { File.WriteAllText(LogFile, ""); }
            catch (Exception ex) { Error($"Could not use {LogFile} as log file. Proceeding without logging.", ex); }

            LogEvent += (sender, arg) =>
            {
                WriteLog(arg.Severity, arg.Message, arg.Exception);
            };

            Debug($"Logging to folder '{new FileInfo(LogFile).DirectoryName}'.");
            Debug($"Log to file '{new FileInfo(LogFile).Name}'.");
        }

        private static void Write(Severity severity, ConsoleColor color, string content, Exception ex = null)
        {
            //If it's nothing, return.
            if (string.IsNullOrWhiteSpace(content)) { return; }

            //Write the severity to the screen.
            Console.BackgroundColor = Theme.BackGround;
            Console.ForegroundColor = color;
            Console.Write(severity);
            Console.ForegroundColor = Theme.Debug;
            Console.SetCursorPosition(8, Console.CursorTop);

            //Set the correct color scheme.
            if (severity == Severity.Error || severity == Severity.Success)
            {
                Console.ForegroundColor = Theme.BackGround;
                Console.BackgroundColor = color;
            }
            else { Console.ForegroundColor = color; }

            char c = '─';
            char c2 = '└';
            if (ex != null)
            {
                c2 = '├';
                if (!string.IsNullOrWhiteSpace(ex.Message)) { c = '┬'; }
            }

            WithSoftWrap(content, c, '┬', '│', c2);

            //Back to normal colors.
            Console.ForegroundColor = color;
            Console.BackgroundColor = Theme.BackGround;

            if (ex != null)
            {
                c = '└';
                if (ex.StackTrace != null) { c = '├'; }
                WithSoftWrap(ex.Message, '├', '├', '│', c);

                if (ex.StackTrace != null)
                {
                    Console.ForegroundColor = Theme.Debug;

                    string[] splitStackTrace = Regex.Split(ex.StackTrace.Trim(), " in ");
                    WithSoftWrap(splitStackTrace[0], '╞', '╞');

                    string stackTrace = splitStackTrace[1];
                    List<string> lines = stackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                    int i = 0;
                    foreach (string stackLine in lines)
                    {
                        string line = stackLine.Trim();
                        c = '╞';
                        if (lines.Count == i) { c = '└'; }

                        WithSoftWrap(line, c, c, '╞', '╘', " \\\\");

                        i++;
                    }
                }
            }
        }

        private static void WithSoftWrap(string text, char cBegin = '─', char cBeginMultiple = '┬', char c = '│', char cEnd = '└', string split = " ")
        {
            string s = text;

            if(string.IsNullOrWhiteSpace(text)) { return; }

            if (s[s.Length - 1] != ' ') { s += " "; }

            if (!SoftWrap) { Console.WriteLine(s); return; }

            if (s.Length <= maxLenght) { Write(s, cBegin); return; }

            var match = Regex.Match(s, @"(.*)(\{.+\})(.*)");
            if (match.Success)
            {
                WithSoftWrap(match.Groups[1].Value, cBegin, cBeginMultiple, c, cEnd, split);
                ShowJson(match.Groups[2].Value);
                WithSoftWrap(match.Groups[3].Value, cBegin, cBeginMultiple, c, cEnd, split);

                return;
            }

            string[] words = Regex.Split(s, $"([^{split}]+ )");

            string line = "";
            bool hasLeftOver = false;
            bool first = false;
            int i = 0;
            foreach (string word in words)
            {
                int length = line.Length + word.Length;

                if (length >= maxLenght)
                {
                    if (!first)
                    {
                        Write(line, cBeginMultiple);
                    }
                    else { Write(line, c); }
                    line = "";
                    first = true;
                    hasLeftOver = false;
                }
                else
                {
                    hasLeftOver = true;
                }

                i++;
                line += word;
            }

            if (hasLeftOver) { Write(line, cEnd); }
        }

        private static void Write(string s, char c)
        {
            if (string.IsNullOrWhiteSpace(s)) { return; }
            if (s[0] != ' ') { s = " " + s; }

            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;

            Console.ForegroundColor = Theme.Debug;
            Console.BackgroundColor = Theme.BackGround;
            Console.SetCursorPosition(8, Console.CursorTop);
            Console.Write(c);
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

            Console.WriteLine(s);
        }

        private static void WriteLog(Severity severity, string content, Exception ex = null)
        {
            StringBuilder s = new StringBuilder("                     ");
            s.Insert(0, DateTime.Now.ToLongTimeString() + " :");
            s.Insert(11, severity.ToString());
            s.Insert(19, $": " + content);

            WriteToLog(s);

            if (ex != null)
            {
                s = new StringBuilder("                     ");
                s.Insert(19, "* " + ex.Message);
                WriteToLog(s);

                if (ex.StackTrace != null)
                {
                    string[] trace = ex.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string x in trace)
                    {
                        string m = x.Trim();
                        s = new StringBuilder("                     ");
                        s.Insert(19, "| " + m);
                        WriteToLog(s);
                    }
                }
            }
        }

        private static void WriteLog(string content)
        {
            StringBuilder s = new StringBuilder("                     ");
            s.Insert(19, $"| " + content);

            WriteToLog(s);
        }

        private static void WriteToLog(object s)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    File.AppendAllText(LogFile, $"{s.ToString()}{Environment.NewLine}");
                    return;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); Thread.Sleep(1000); }
            }
        }

        private static void ShowJson(string json)
        {
            string formatJson = "";
            try { formatJson = Newtonsoft.Json.JsonConvert.SerializeObject(Newtonsoft.Json.JsonConvert.DeserializeObject(json), Newtonsoft.Json.Formatting.Indented); }
            catch(Exception ex) { Debug("Could not correctly output JSON from object.", ex); Debug("JSON:"); Debug(json); }

            var lines = Regex.Split(formatJson, Environment.NewLine);

            Write(lines[0], '┌');
            for (int i = 1; i < lines.Length - 1; i++)
            {
                Write(lines[i], '╞');
            }
            Write(lines[lines.Length - 1], '└');
        }

        private static object DoFormat(object o)
        {
            if (!Format) { return o.ToString(); }

            //Turn into json if it's a custom object.
            if (!o.GetType().Namespace.StartsWith("System"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.None);
            }
            else
            {
                string s = "";
                string[] lines = Regex.Split(o.ToString(), @"(\. |! |\? )");
                if (lines.Length == 0) { s = o.ToString(); }
                else
                {
                    foreach (string line in lines)
                    {
                        s += line[0].ToString().ToUpper() + line.Substring(1);
                    }
                }

                if (s.Length > 0)
                {
                    s = s[0].ToString().ToUpper() + s.Substring(1);
                    if (s[s.Length - 1] != '.') { s += "."; }
                }

                return s;
            }
        }

        public static event EventHandler<LogMessage> LogEvent;
    }

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

    /// <summary>
    /// The severity levle of the message.
    /// </summary>
    public enum Severity
    {
        Success, Error, Warning, Info, Debug, Special
    }
}