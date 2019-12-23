using System;
using System.Collections.Generic;
using System.Text;

namespace Somfic.Logging.Handlers
{
    public class ConsoleHandler : ILoggerHandler
    {
        public void WriteLog(LogMessage message)
        {
            Console.WriteLine(message.Content);
        }
    }
}
