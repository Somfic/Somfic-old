using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Somfic.Logging.Handlers
{
    public class LogFileHandler : ILoggerHandler
    {
        private const int SeverityColumn = 9;
        private const int TimeColomn = 13;
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

            try { File.WriteAllText(LogFile.FullName, ""); }
            catch {}
        }

        public void WriteLog(LogMessage message)
        {
            StringBuilder s = new StringBuilder();
            string time = message.Timestamp.ToString("hh:mm:ss.fff");


            s.Append(string.Format("{0, -" + TimeColomn + "}",time)); // Write the time
            s.Append(string.Format("{0, -" + SeverityColumn + "}", $" [{message.Severity}] ")); // Write the severity
            
            if (message.Content != null)
            {
                s.Append(message.Content);
                s.Append("\n");
            }

            if (message.Object != null)
            {
                s.Append(' ', SeverityColumn + TimeColomn);
                s.Append(Newtonsoft.Json.JsonConvert.SerializeObject(message.Object));
                s.Append("\n");
            }

            File.AppendAllText(LogFile.FullName, s.ToString());
        }
    }
}