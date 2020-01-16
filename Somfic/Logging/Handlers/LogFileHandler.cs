using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Somfic.Logging.Handlers
{
    public class LogFileHandler : LoggerHandler
    {
        private const int SeverityColumn = 11;
        private const int TimeColumn = 12;
        private List<string> backupLogs = new List<string>();
        
        public FileInfo LogFile { get; private set; }

        public LogFileHandler(string path, string name)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string date = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";

            int i = 1;
            while (true)
            {
                string filePath = Path.Combine(path, $"{name}.{date}.{i}.log");
                if (!File.Exists(filePath))
                {
                    LogFile = new FileInfo(filePath);
                    break;
                }
                i++;
            }

            File.WriteAllText(LogFile.FullName, "");


            }

        public override void WriteLog(LogMessage message)
        {
            StringBuilder s = new StringBuilder();
            string time = message.Timestamp.ToString("hh:mm:ss.fff");

            s.Append(string.Format("{0, -" + TimeColumn + "}",time)); // Write the time
            s.Append(string.Format("{0, -" + SeverityColumn + "}", $" [{message.Severity}] ")); // Write the severity

            bool isIndented = true;

            if (!string.IsNullOrEmpty(message.Content))
            {
                s.Append(message.Content);
                s.Append("\n");
                isIndented = false;
            }

            if (message.Object != null)
            {
                if(!isIndented) { s.Append(' ', SeverityColumn + TimeColumn);}
                isIndented = false;
                s.Append(Newtonsoft.Json.JsonConvert.SerializeObject(message.Object));
                s.Append("\n");
            }

            if (message.Exception.Exception != null)
            {
                LogException ex = message.Exception;

                while (true)
                {
                    if (!isIndented)
                    {
                        s.Append(' ', SeverityColumn + TimeColumn);
                    }

                    isIndented = false;
                    s.Append($"{ex.Type} : {ex.Exception.Message}");
                    if (ex.Localised != null)
                    {
                        s.Append("\n");
                        s.Append(' ', SeverityColumn + TimeColumn);
                        isIndented = false;
                        s.Append(ex.Localised);
                    }

                    s.Append("\n");
                    if (ex.Exception.InnerException != null)
                    {
                        ex = new LogException(ex.Exception.InnerException);
                    }
                    else
                    {
                        break;
                    }
                }

                if (message.Exception.Exception.StackTrace != null)
                {
                    string[] stacks = message.Exception.Exception.StackTrace.Split('\n');
                    foreach (string stack in stacks)
                    {
                        s.Append(' ', SeverityColumn + TimeColumn);
                        s.Append(stack.Trim());
                        s.Append("\n");
                    }
                }
            }


            backupLogs.Add(s.ToString());
            string[] strings = backupLogs.ToArray();
            backupLogs.Clear();

           foreach (string str in strings)
           {
               try
               {
                   File.AppendAllText(LogFile.FullName, str);
               }
               catch
               {
                    backupLogs.Add(str);
               }
            }
        }

    }
}