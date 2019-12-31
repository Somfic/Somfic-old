﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Somfic.Logging
{
    public class Logger
    {
        public IList<ILoggerHandler> Handlers { get; private set; }
        public bool IsDisabled { get; private set; }

        public Logger()
        {
            Handlers = new List<ILoggerHandler>();
        }

        /// <summary>
        /// Adds a handler to the logger.
        /// </summary>
        /// <param name="handler">The handler</param>
        public void AddHandler(ILoggerHandler handler)
        {
            Handlers.Add(handler);
        }

        /// <summary>
        /// Removes a handler from the logger.
        /// </summary>
        /// <param name="handler">The handler</param>
        public void RemoveHandler(ILoggerHandler handler)
        {
            Handlers.Remove(handler);
        }

        /// <summary>
        /// Disables logging.
        /// </summary>
        public void Disable()
        {
            IsDisabled = true;
        }

        /// <summary>
        /// Enables logging, should only be used after logging has been disabled.
        /// </summary>
        public void Enable()
        {
            IsDisabled = false;
        }

        ///old
        [Obsolete("Use Log(Severity.Success, obj, exception) instead")]
        public void Success(object s, Exception ex = null) => Log(Severity.Success, s, ex);

        [Obsolete("Use Log(Severity.Warning, obj, exception) instead")]
        public void Warning(object s, Exception ex = null) => Log(Severity.Warning, s, ex);

        [Obsolete("Use Log(Severity.Error, obj, exception) instead")]
        public void Error(object s, Exception ex = null) => Log(Severity.Error, s, ex);

        [Obsolete("Use Log(Severity.Debug, obj, exception) instead")]
        public void Debug(object s, Exception ex = null) => Log(Severity.Debug, s, ex);

        [Obsolete("Use AddHandler(new ConsoleHandler()) instead", true)]
        public Logger UseConsole() { return this; }

        [Obsolete("Use AddHandler(new LogFileHandler(dir, path)) instead", true)]
        public Logger UseLogFile(string path) { return this; }

        [Obsolete("This method is no longer supported", true)]
        public Logger UseFormat() { return this; }

        [Obsolete("This method is no longer supported", true)]
        public Logger UseTCP() { return this; }

        //Nothing
        public void Log() => ProcessLog(Severity.Info, null, null, null);

        //Only content
        public void Log(string content) => ProcessLog(Severity.Info, content, null, null);
        public void Log(Severity severity, string content) => ProcessLog(severity, content, null, null);

        //Only object
        public void Log(object obj)
        {
            if (obj.GetType() == "".GetType()) { ProcessLog(Severity.Info, obj.ToString(), null, null); }
            else { ProcessLog(Severity.Info, null, obj, null); }
        }

        public void Log(Severity severity, object obj)
        {
            if (obj.GetType() == "".GetType()) { ProcessLog(severity, obj.ToString(), null, null); }
            else { ProcessLog(severity, null, obj, null); }
        }

        //Only exception
        public void Log(Exception exception) => ProcessLog(Severity.Info, null, null, exception);
        public void Log(Severity severity, Exception exception) => ProcessLog(severity, null, null, exception);

        //Content and exception
        public void Log(string content, Exception exception) => ProcessLog(Severity.Info, content, null, exception);
        public void Log(Severity severity, string content, Exception exception) => ProcessLog(severity, content, null, exception);

        //Object and exception
        public void Log(object obj, Exception exception)
        {
            if (obj.GetType() == "".GetType()) { ProcessLog(Severity.Info, obj.ToString(), null, exception); }
            else { ProcessLog(Severity.Info, null, obj, exception); }
        }

        public void Log(Severity severity, object obj, Exception exception)
        {
            if (obj.GetType() == "".GetType()) { ProcessLog(severity, obj.ToString(), null, exception); }
            else { ProcessLog(severity, null, obj, exception); }
        }

        //Content and object
        public void Log(string content, object obj) => ProcessLog(Severity.Info, content, obj, null);
        public void Log(Severity severity, string content, object obj) => ProcessLog(severity, content, obj, null);

        //Content, object and exception
        public void Log(string content, object obj, Exception exception) => ProcessLog(Severity.Info, content, obj, exception);
        public void Log(Severity severity, string content, object obj, Exception exception) => ProcessLog(severity, content, obj, exception);

        private void ProcessLog(Severity severity, string content, object @object, Exception exception)
        {
            if(IsDisabled) { return; }

            StackTrace stack = new StackTrace();
            LogMessage m = new LogMessage(content, severity, @object, stack, exception);
            Handlers.ToList().ForEach(x => x.WriteLog(m));
            LogEvent?.Invoke(this, m);
        }

        public event EventHandler<LogMessage> LogEvent;
    }
}