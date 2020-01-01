using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Somfic.Logging.Handlers
{
    public class ConsoleHandler : ILoggerHandler
    {
        private const int SeverityColumn = 10;
        private static readonly Dictionary<char, ConsoleColor> ConsoleColors = new Dictionary<char, ConsoleColor>
        {
            { '0',ConsoleColor.Black },
            {'1',ConsoleColor.DarkBlue },
            {'2',ConsoleColor.DarkGreen},
            {'3',ConsoleColor.DarkCyan },
            {'4',ConsoleColor.DarkRed },
            {'5',ConsoleColor.DarkMagenta },
            {'6',ConsoleColor.DarkYellow },
            {'7',ConsoleColor.Gray },
            {'8',ConsoleColor.DarkGray },
            {'9',ConsoleColor.Blue },
            {'A',ConsoleColor.Green },
            {'B',ConsoleColor.Cyan },
            {'C',ConsoleColor.Red },
            {'D',ConsoleColor.Magenta },
            {'E',ConsoleColor.Yellow },
            {'F',ConsoleColor.White }
        };

        private static readonly Dictionary<Severity, char> FrontColors = new Dictionary<Severity, char>
        {
            {Severity.Info, 'f' },
            {Severity.Debug, '8' },
            {Severity.Error, 'c' },
            {Severity.Warning, 'e' },
            {Severity.Success, 'a' },
            {Severity.Special, 'b' }
        };

        private static readonly Dictionary<Severity, char> BackColors = new Dictionary<Severity, char>
        {
            {Severity.Info, '7' },
            {Severity.Debug, '8' },
            {Severity.Error, '4' },
            {Severity.Warning, '6' },
            {Severity.Success, '2' },
            {Severity.Special, '3' }
        };

        private static readonly int StackTraceMax = 3;

        public void WriteLog(LogMessage message)
        {
            StringBuilder s = new StringBuilder();

            bool isIndented = false;

            // Write down the severity
            s.Append($"&{FrontColors[message.Severity]};"); // Correct color
            s.Append(string.Format("{0, -" + SeverityColumn + "}", message.Severity)); // Write the severity
            isIndented = true;
            s.Append("&r;"); // Reset colors

            // Show the content
            if (!string.IsNullOrEmpty(message.Content))
            {
                s.Append($"&{FrontColors[message.Severity]};");
                s.Append(Format(message.Content));
                s.Append("&r;"); // Reset colors
                s.Append("\n");
                isIndented = false;
            }

            if (message.Object != null)
            {
                if(!isIndented) {s.Append(' ', SeverityColumn);}
                s.Append(Format(PrettyJson(Newtonsoft.Json.JsonConvert.SerializeObject(message.Object))));
                s.Append("\n");
                isIndented = false;
            }

            if (message.Exception.Exception != null)
            {
                LogException ex = message.Exception;

                while (true)
                {
                    if(!isIndented) {s.Append(' ', SeverityColumn);}
                    isIndented = false;
                    s.Append(Format($"&{BackColors[message.Severity]}0; {ex.Type} &r; &8;: &{FrontColors[message.Severity]};{ex.Exception.Message}&r;"));
                    if (ex.Localised != null)
                    {
                        s.Append("\n");
                        s.Append(' ', SeverityColumn);
                        s.Append("&8;");
                        s.Append(Format(ex.Localised));
                        s.Append("&r;");
                    }
                    s.Append("\n");
                    if (ex.Exception.InnerException != null) { ex = new LogException(ex.Exception.InnerException); }
                    else { break; }
                }

                if (message.Exception.Exception.StackTrace != null)
                {
                    string[] stacks = message.Exception.Exception.StackTrace.Split('\n');
                    if (stacks.Length > StackTraceMax)
                    {
                        s.Append(' ', SeverityColumn);
                        s.Append(Format($"&8;({stacks.Length - StackTraceMax} more ...)&r;"));
                        s.Append("\n");
                    }
                    foreach (string stack in stacks.Reverse().Take(StackTraceMax).Reverse())
                    {
                        s.Append(' ', SeverityColumn);
                        s.Append(Format($"&8;{stack.Trim()}&r;"));
                        s.Append("\n");
                    }

                }
            }

            string result = s.ToString();
            if (result.EndsWith("\n")) { result = result.Substring(0, result.Length - 1); }

            Write(result);
        }

        private static void Write(string content)
        {
            string[] parts = Regex.Split(content, @"((&[0-9a-fr]{1,2};))", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                Match match = Regex.Match(part, "&([0-9a-fr]){1,2};", RegexOptions.IgnoreCase);
                if (!match.Success) { Console.Write(part); continue; }

                ConsoleColor foregroundColor = ConsoleColor.White;
                ConsoleColor backgroundColor = ConsoleColor.Black;

                switch (part.Length)
                {
                    case 4:
                        ConsoleColors.TryGetValue(part.ToUpper()[1], out backgroundColor);
                        ConsoleColors.TryGetValue(part.ToUpper()[2], out foregroundColor);
                        break;
                    case 3:
                        if (part.ToUpper()[1] == 'R')
                        {
                            foregroundColor = ConsoleColor.White;
                            backgroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            ConsoleColors.TryGetValue(part.ToUpper()[1], out foregroundColor);
                        }

                        break;
                }

                Console.ForegroundColor = foregroundColor;
                Console.BackgroundColor = backgroundColor;
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        private static string Format(string content)
        {
            StringBuilder s = new StringBuilder();
            string[] parts = Regex.Split(content, @"(?<=[ .,;:\\/'" + '"' + "])");

            int currentWidth = 0;
            foreach (string part in parts)
            {
                string cleanPart = Regex.Replace(part, "([0-9a-fr]){1,2};", "");
                
                if (currentWidth + cleanPart.Length > Console.WindowWidth - SeverityColumn - 1)
                {
                    s.Append("\n");
                    s.Append(' ', SeverityColumn + 1);
                    currentWidth = 1;
                }

                s.Append(currentWidth == 0 ? part.TrimStart() : part);
                currentWidth += cleanPart.Length;
            }

            return s.ToString();
        }

        private static string PrettyJson(string json)
        {
            return $"&8;{Regex.Replace(json, "(\"|\')([^\"]*)(\"|\')(?=:)(: *)(\"|\')?(true|false|[^\"]*)", "&8;$1&6;$2&8;$3&7;$4&8;$5&b;$6&8;")}";
        }
    }
}
