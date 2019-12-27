using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Somfic.Logging_old.Theme;

namespace Somfic.Logging_old
{
    public class Logger
    {
        private string DirectoryPath = "";
        private string LogFile = "";

        private Severity MinType = Severity.Debug;
        private ITheme Theme = Themes.Dark;

        private bool SoftWrap = true;
        private bool Format = false;
        private bool Tcp = false;
        private readonly bool PrettyJson = false;

        private TcpListener TcpServer;
        private readonly TcpClient TcpClient;
        private List<NetworkStream> TcpStreams;

        private int MaxLength => Console.WindowWidth - 11;

        /// <summary>
        /// Creates a new log entry with a severity of Info.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public void Log(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Info));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Info.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the log.</param>
        public void Log(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Info, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Success.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public void Success(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Success));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Success.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the log.</param>
        public void Success(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Success, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Warning.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public void Warning(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Warning));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Warning.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the warning.</param>
        public void Warning(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Warning, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Error.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public void Error(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Error));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Error.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the error.</param>
        public void Error(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Error, ex));
        }

        /// <summary>
        /// Creates a new log entry with a severity of Debug.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        public void Debug(object s)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Debug));
        }


        /// <summary>
        /// Creates a new log entry with a severity of Debug.
        /// </summary>
        /// <param name="s">The content of the log.</param>
        /// <param name="ex">The linked exception that caused the debug.</param>
        public void Debug(object s, Exception ex)
        {
            if (Format) { s = DoFormat(s); }
            LogEvent?.Invoke(null, new LogMessage(s.ToString(), Severity.Debug, ex));
        }

        /// <summary>
        /// Whether to use soft wrap.
        /// </summary>
        public Logger UseSoftWrap(bool state)
        {
            SoftWrap = state;
            return this;
        }

        /// <summary>
        /// Whether to automatically format text.
        /// </summary>
        public Logger UseFormat(bool state)
        {
            Format = state;
            return this;
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public Logger UseConsole(Severity type)
        {
            MinType = type;
            return UseConsole();
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public Logger UseConsole(ITheme theme)
        {
            Theme = theme;
            return UseConsole();
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public Logger UseConsole(ITheme theme, Severity type)
        {
            MinType = type;
            Theme = theme;
            return UseConsole();
        }

        /// <summary>
        /// Outputs the logs to console.
        /// </summary>
        public Logger UseConsole()
        {
            ChangeBackground(Theme.BackGround);
            ChangeForeground(Theme.Main);
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

            return this;
        }


        public Logger UseTCP(int port)
        {
            TcpStreams = new List<NetworkStream>();
            Tcp = true;

            string localhost = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress thisIp in host.AddressList)
            {
                if (thisIp.AddressFamily == AddressFamily.InterNetwork)
                {
                    localhost = thisIp.ToString();
                    break;
                }
            }

            //Host TCP server.
            IPAddress ip = IPAddress.Parse(localhost);
            TcpServer = new TcpListener(ip, port);

            TcpServer.Start();

            byte[] bytes = new byte[256];

            Debug($"Hosting TCP server on {ip}:{port}.");

            Task.Run(() =>
            {
                while (true)
                {
                    //Wait for the next client to connect.
                    TcpClient client = TcpServer.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    Debug($"TCP client connected.");

                    //Add the stream to the list.
                    TcpStreams.Add(stream);
                    SendMessage(new TcpMessage(MessageType.WindowSize, $"{Console.WindowWidth},{Console.WindowHeight}"));
                }
            });


            return this;
        }

        public Logger UseTCP(string ip, int port)
        {
            Task.Run(() =>
            {
                Debug($"Connecting to TPC server via {ip}:{port}.");
                TcpClient client;

                try
                {
                    client = new TcpClient(ip, port);
                    Success("Connected to TPC server.");
                }
                catch (Exception ex)
                {
                    Error("Could not connect to TPC server.", ex);
                    return this;
                }

                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                while (true)
                {
                    string command = reader.ReadLine();


                    try
                    {
                        TcpMessage message = JsonConvert.DeserializeObject<TcpMessage>(command);

                        switch (message.MessageType)
                        {
                            case MessageType.Background:
                                Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), message.Content);
                                break;

                            case MessageType.Foreground:
                                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), message.Content);
                                break;

                            case MessageType.Cursor:
                                Console.SetCursorPosition(int.Parse(message.Content.Split(',')[0]), int.Parse(message.Content.Split(',')[1]));
                                break;

                            case MessageType.Write:
                                Console.Write(message.Content);
                                break;

                            case MessageType.WriteLine:
                                Console.WriteLine(message.Content);
                                break;

                            case MessageType.WindowSize:
                                Console.WindowWidth = int.Parse(message.Content.Split(',')[0]);
                                Console.WindowHeight = int.Parse(message.Content.Split(',')[1]);
                                break;
                        }
                    }
                    catch (Exception ex) { Error($"Error while parsing message '{command}'.", ex); }
                }
            });

            return this;
        }

        /// <summary>
        /// Outputs the logs to an external log file.
        /// </summary>
        /// <param name="directory">The directory in which to save the log files.</param>
        public Logger UseLogFile(string directory, string name = "log")
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
            Debug($"Logging to file '{new FileInfo(LogFile).Name}'.");

            return this;
        }

        private void Write(Severity severity, ConsoleColor color, string content, Exception ex = null)
        {
            //If it's nothing, return.
            if (string.IsNullOrWhiteSpace(content)) { return; }

            //Write the severity to the screen.
            ChangeBackground(Theme.BackGround);
            ChangeForeground(color);
            Write(severity.ToString());
            ChangeForeground(Theme.Debug);
            ChangeCursor(8, Console.CursorTop);

            //Set the correct color scheme.
            if (severity == Severity.Error || severity == Severity.Success)
            {
                ChangeForeground(Theme.BackGround);
                ChangeBackground(color);
            }
            else { ChangeForeground(color); }

            char c = '─';
            char c2 = '└';
            if (ex != null)
            {
                c2 = '├';
                if (!string.IsNullOrWhiteSpace(ex.Message)) { c = '┬'; }
            }

            WithSoftWrap(content, c, '┬', '│', c2);

            //Back to normal colors.
            ChangeForeground(color);
            ChangeBackground(Theme.BackGround);

            if (ex == null)
            {
                return;
            }

            c = '└';
            if (ex.StackTrace != null) { c = '│'; }
            WithSoftWrap(ex.Message, '├', '├', '│', c);


            if (ex.StackTrace == null)
            {
                return;
            }

            ChangeForeground(Theme.Debug);

            string stackTrace = ex.StackTrace;
            List<string> lines = stackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            int i = 0;
            foreach (string line in lines.Select(stackLine => stackLine.Trim()))
            {
                c = '╞';
                if (lines.Count == i) { c = '└'; }

                WithSoftWrap(line, c, c, '│', '╘', " \\\\");

                i++;
            }
        }

        private void WithSoftWrap(string text, char cBegin = '─', char cBeginMultiple = '┬', char c = '│', char cEnd = '└', string split = " ")
        {
            string s = text;

            if (string.IsNullOrWhiteSpace(text)) { return; }

            if (s[s.Length - 1] != ' ') { s += " "; }

            if (!SoftWrap) { WriteLine(s); return; }

            if (s.Length <= MaxLength) { Write(s, cBegin); return; }

            try
            {
                Match match = Regex.Match(s, @"(.*)(\{.+\})(.*)");
                if (match.Success && Format)
                {
                    if (PrettyJson)
                    {
                        WithSoftWrap(match.Groups[1].Value, cBegin, cBeginMultiple, c, cEnd, split);
                        ShowJson(match.Groups[2].Value);
                        WithSoftWrap(match.Groups[3].Value, cBegin, cBeginMultiple, c, cEnd, split);
                    }
                    else
                    {
                        WithSoftWrap(match.Groups[0].Value, cBegin, cBeginMultiple, c, cEnd, split);
                    }
                    return;
                }
            }
            catch { }

            string[] words = Regex.Split(s, $"([^{split}]+ )");

            string line = "";
            bool hasLeftOver = false;
            bool first = false;
            int i = 0;
            foreach (string word in words)
            {
                int length = line.Length + word.Length;

                if (length >= MaxLength)
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

        public string GetInput(string message = "")
        {
            Write(message, '>', false);
            string s = Console.ReadLine();
            Console.WriteLine();
            return s;
        }

        private void Write(string s, char c, bool writeLine = true)
        {
            if (string.IsNullOrWhiteSpace(s)) { return; }
            if (s[0] != ' ') { s = " " + s; }

            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;

            ChangeForeground(Theme.Debug);
            ChangeBackground(Theme.BackGround);
            ChangeCursor(8, Console.CursorTop);
            Write(c.ToString());
            ChangeCursor(10, Console.CursorTop);
            ChangeBackground(background);
            ChangeForeground(foreground);

            if (writeLine)
            {
                WriteLine(s);
            }
            else { Write(s); }
        }

        private void WriteLog(Severity severity, string content, Exception ex = null)
        {
            StringBuilder s = new StringBuilder("                     ");
            s.Insert(0, DateTime.Now.ToLongTimeString() + " :");
            s.Insert(11, severity.ToString());
            s.Insert(19, $": " + content);

            WriteToLog(s);

            if (ex == null)
            {
                return;
            }

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

        private void WriteLog(string content)
        {
            StringBuilder s = new StringBuilder("                     ");
            s.Insert(19, $"| " + content);

            WriteToLog(s);
        }

        private void WriteToLog(object s)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    File.AppendAllText(LogFile, $"{s.ToString()}{Environment.NewLine}");
                    return;
                }
                catch (Exception ex) { WriteLine(ex.Message); Thread.Sleep(1000); }
            }
        }

        private void ShowJson(string json)
        {
            string formatJson = "";
            try { formatJson = Newtonsoft.Json.JsonConvert.SerializeObject(Newtonsoft.Json.JsonConvert.DeserializeObject(json), Newtonsoft.Json.Formatting.Indented); }
            catch (Exception ex) { Debug("Could not correctly output JSON from object.", ex); Debug("JSON:"); Debug(json); }

            string[] lines = Regex.Split(formatJson, Environment.NewLine);

            Write(lines[0], '┌');
            for (int i = 1; i < lines.Length - 1; i++)
            {
                Write(lines[i], '╞');
            }
            Write(lines[lines.Length - 1], '└');
        }

        private object DoFormat(object o)
        {
            //if (!Format) { return o.ToString(); }

            if (string.IsNullOrWhiteSpace(o.ToString())) { return o; }

            //Turn into json if it's a custom object.
            if (!o.GetType().Namespace.StartsWith("System"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.None);
            }
            else
            {
                string s = "";
                string[] lines = Regex.Split(o.ToString(), @"(\. |! |\? )");
                if (lines.Length == 0) { return o.ToString(); }
                else
                {
                    foreach (string line in lines)
                    {
                        s += line;
                    }
                }

                return s;
            }
        }

        private void WriteLine(string s)
        {
            if (Tcp) { SendMessage(new TcpMessage(MessageType.WriteLine, s)); }
            Console.WriteLine(s);
        }

        private void Write(string s)
        {
            if (Tcp) { SendMessage(new TcpMessage(MessageType.Write, s)); }
            Console.Write(s);
        }

        private void ChangeForeground(ConsoleColor c)
        {
            if (Tcp) { SendMessage(new TcpMessage(MessageType.Foreground, c.ToString())); }
            Console.ForegroundColor = c;
        }

        private void ChangeBackground(ConsoleColor c)
        {
            if (Tcp) { SendMessage(new TcpMessage(MessageType.Background, c.ToString())); }
            Console.BackgroundColor = c;
        }

        private void ChangeCursor(int x, int y)
        {
            if (Tcp) { SendMessage(new TcpMessage(MessageType.Cursor, $"{x},{y}")); }
            Console.SetCursorPosition(x, y);

        }

        private void SendMessage(TcpMessage TcpMessage)
        {
            foreach (NetworkStream stream in TcpStreams)
            {
                try
                {
                    if (TcpMessage.MessageType == MessageType.Write || TcpMessage.MessageType == MessageType.WriteLine) { SendMessage(new TcpMessage(MessageType.WindowSize, $"{Console.WindowWidth},{Console.WindowHeight}")); }
                    byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(TcpMessage) + Environment.NewLine);
                    stream.Write(message, 0, message.Length);
                }
                catch (Exception ex) { Debug("Could not send TCP message to client.", ex); }
            }
        }

        public event EventHandler<LogMessage> LogEvent;
    }
}