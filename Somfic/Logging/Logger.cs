using System;
using System.Collections.Generic;
using System.Linq;

namespace Somfic.Logging
{
    public class Logger
    {
        private readonly IList<ILoggerHandler> handlers;

        public Logger(ILoggerHandler handler)
        {
            var handlers = new List<ILoggerHandler> { handler };
        }

        public Logger(IEnumerable<ILoggerHandler> handlers)
        {
            this.handlers = new List<ILoggerHandler>();
            handlers.ToList().ForEach(x => this.handlers.Add(x));
        }

        public void AddHandler(ILoggerHandler handler)
        {
            handlers.Add(handler);
        }

        public void RemoveHandler(ILoggerHandler handler)
        {
            handlers.Remove(handler);
        }

        public void Log(Severity severity, string message, Exception exception = null)
        {
            LogMessage m = new LogMessage(message, severity, exception);
            handlers.ToList().ForEach(x => x.WriteLog(m));
        }
    }
}