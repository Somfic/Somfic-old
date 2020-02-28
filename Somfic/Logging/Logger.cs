using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Somfic.Logging
{
    public class Logger
    {
        public static IList<LoggerHandler> Handlers { get; private set; } = new List<LoggerHandler>();
        public static bool IsDisabled { get; private set; }
        public static IList<Severity> AllowedLevels { get; private set; } = new List<Severity> { Severity.Debug, Severity.Info, Severity.Warning, Severity.Error, Severity.Success, Severity.Special, Severity.Verbose, Severity.Verbose, Severity.Verbose, Severity.Verbose };

        /// <summary>
        /// Adds a handler to the logger that only allows certain levels.
        /// </summary>
        /// <param name="handler">The handler</param>
        public static void AddHandler(LoggerHandler handler, params Severity[] allowedLevels)
        {
            handler.AllowedLevels = allowedLevels;
            Handlers.Add(handler);
        }

        /// <summary>
        /// Adds a handler to the logger.
        /// </summary>
        /// <param name="handler">The handler</param>
        public static void AddHandler(LoggerHandler handler)
        {
            Handlers.Add(handler);
        }

        /// <summary>
        /// Removes a handler from the logger.
        /// </summary>
        /// <param name="handler">The handler</param>
        public static void RemoveHandler(LoggerHandler handler)
        {
            Handlers.Remove(handler);
        }

        /// <summary>
        /// Disables logging.
        /// </summary>
        public static void Disable()
        {
            IsDisabled = true;
        }

        /// <summary>
        /// Enables logging, should only be used after logging has been disabled.
        /// </summary>
        public static void Enable()
        {
            IsDisabled = false;
        }



        public static void SetAllowedLevels(params Severity[] severity)
        {
            AllowedLevels = severity;
        }

        public static void AllowAllLevels()
        {
            AllowedLevels = new List<Severity> { Severity.Debug, Severity.Info, Severity.Warning, Severity.Error, Severity.Success, Severity.Special, Severity.Verbose };
        }

        private static void ProcessLog(Severity severity, string content, object @object, Exception exception)
        {
            if (IsDisabled) { return; }

            if (!AllowedLevels.Contains(severity)) { return; }

            if(string.IsNullOrWhiteSpace(content)) { content = null; }

            if(content == null && @object == null && exception == null) { return; }

            StackTrace stack = new StackTrace();
            LogMessage m = new LogMessage(severity, content, @object, stack, exception);
            Handlers.ToList().ForEach(x => x.ProcessLog(m));
            LogEvent?.Invoke(null, m);
        }

        public static event EventHandler<LogMessage> LogEvent;

        //Only content
        public static void Log(Severity severity, string content) => ProcessLog(severity, content, null, null);

        //Only object
        public static void Log(Severity severity, object obj)
        {
            if (obj.GetType() == "".GetType()) { ProcessLog(severity, obj.ToString(), null, null); }
            else { ProcessLog(severity, null, obj, null); }
        }

        //Only exception
        public static void Log(Severity severity, Exception exception) => ProcessLog(severity, null, null, exception);

        //Content and exception
        public static void Log(Severity severity, string content, Exception exception) => ProcessLog(severity, content, null, exception);

        //Object and exception
        public static void Log(Severity severity, object obj, Exception exception)
        {
            if (obj.GetType() == "".GetType()) { ProcessLog(severity, obj.ToString(), null, exception); }
            else { ProcessLog(severity, null, obj, exception); }
        }

        //Content and object
        public static void Log(Severity severity, string content, object obj) => ProcessLog(severity, content, obj, null);

        //Content, object and exception
        public static void Log(Severity severity, string content, object obj, Exception exception) => ProcessLog(severity, content, obj, exception);


        //Info
        public static void Log()
        {
            ProcessLog(Severity.Info, null, null, null);
        }

        public static void Log(string content)
        {
            ProcessLog(Severity.Info, content, null, null);
        }

        public static void Log(object obj) {
            if(obj == null) { Log(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Info, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Info, null, obj, null); 

            }

        }
        public static void Log(Exception exception)
        {
            ProcessLog(Severity.Info, null, null, exception);
        }

        public static void Log(string content, Exception exception)
        {
            ProcessLog(Severity.Info, content, null, exception);
        }

        public static void Log(object obj, Exception exception)
        { 
            if(obj == null) { Log(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Info, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Info, null, obj, exception);
            }
        }

        public static void Log(string content, object obj)
        {
            ProcessLog(Severity.Info, content, obj, null);
        }

        public static void Log(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Info, content, obj, exception);
        }

        //Debug
        public static void Debug()
        {
            ProcessLog(Severity.Debug, null, null, null);
        }

        public static void Debug(string content)
        {
            ProcessLog(Severity.Debug, content, null, null);
        }

        public static void Debug(object obj)
        {
            if (obj == null) { Debug(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Debug, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Debug, null, obj, null);
            }
        }

        public static void Debug(Exception exception)
        {
            ProcessLog(Severity.Debug, null, null, exception);
        }

        public static void Debug(string content, Exception exception)
        {
            ProcessLog(Severity.Debug, content, null, exception);
        }

        public static void Debug(object obj, Exception exception)
        {
            if (obj == null) { Debug(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Debug, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Debug, null, obj, exception);
            }
        }

        public static void Debug(string content, object obj)
        {
            ProcessLog(Severity.Debug, content, obj, null);
        }

        public static void Debug(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Debug, content, obj, exception);
        }

        //Warning
        public static void Warning()
        {
            ProcessLog(Severity.Warning, null, null, null);
        }

        public static void Warning(string content)
        {
            ProcessLog(Severity.Warning, content, null, null);
        }

        public static void Warning(object obj)
        {
            if (obj == null) { Warning(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Warning, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Warning, null, obj, null);
            }
        }

        public static void Warning(Exception exception)
        {
            ProcessLog(Severity.Warning, null, null, exception);
        }

        public static void Warning(string content, Exception exception)
        {
            ProcessLog(Severity.Warning, content, null, exception);
        }

        public static void Warning(object obj, Exception exception)
        {
            if (obj == null) { Warning(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Warning, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Warning, null, obj, exception);
            }
        }

        public static void Warning(string content, object obj)
        {
            ProcessLog(Severity.Warning, content, obj, null);
        }

        public static void Warning(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Warning, content, obj, exception);
        }

        //Error
        public static void Error()
        {
            ProcessLog(Severity.Error, null, null, null);
        }

        public static void Error(string content)
        {
            ProcessLog(Severity.Error, content, null, null);
        }

        public static void Error(object obj)
        {
            if (obj == null) { Error(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Error, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Error, null, obj, null);
            }
        }
        public static void Error(Exception exception)
        {
            ProcessLog(Severity.Error, null, null, exception);
        }

        public static void Error(string content, Exception exception)
        {
            ProcessLog(Severity.Error, content, null, exception);
        }

        public static void Error(object obj, Exception exception)
        {
            if (obj == null) { Error(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Error, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Error, null, obj, exception);
            }
        }

        public static void Error(string content, object obj)
        {
            ProcessLog(Severity.Error, content, obj, null);
        }

        public static void Error(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Error, content, obj, exception);
        }

        //Success
        public static void Success()
        {
            ProcessLog(Severity.Success, null, null, null);
        }

        public static void Success(string content)
        {
            ProcessLog(Severity.Success, content, null, null);
        }

        public static void Success(object obj)
        {
            if (obj == null) { Success(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Success, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Success, null, obj, null);
            }
        }

        public static void Success(Exception exception)
        {
            ProcessLog(Severity.Success, null, null, exception);
        }

        public static void Success(string content, Exception exception)
        {
            ProcessLog(Severity.Success, content, null, exception);
        }

        public static void Success(object obj, Exception exception)
        {
            if (obj == null) { Success(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Success, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Success, null, obj, exception);
            }
        }

        public static void Success(string content, object obj)
        {
            ProcessLog(Severity.Success, content, obj, null);
        }

        public static void Success(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Success, content, obj, exception);
        }

        //Special
        public static void Special()
        {
            ProcessLog(Severity.Special, null, null, null);
        }

        public static void Special(string content)
        {
            ProcessLog(Severity.Special, content, null, null);
        }

        public static void Special(object obj)
        {
            if (obj == null) { Special(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Special, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Special, null, obj, null);
            }
        }

        public static void Special(Exception exception)
        {
            ProcessLog(Severity.Special, null, null, exception);
        }

        public static void Special(string content, Exception exception)
        {
            ProcessLog(Severity.Special, content, null, exception);
        }

        public static void Special(object obj, Exception exception)
        {
            if (obj == null) { Special(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Special, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Special, null, obj, exception);
            }
        }

        public static void Special(string content, object obj)
        {
            ProcessLog(Severity.Special, content, obj, null);
        }

        public static void Special(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Special, content, obj, exception);
        }

        //Verbose
        public static void Verbose()
        {
            ProcessLog(Severity.Verbose, null, null, null);
        }

        public static void Verbose(string content)
        {
            ProcessLog(Severity.Verbose, content, null, null);
        }

        public static void Verbose(object obj)
        {
            if (obj == null) { Verbose(); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Verbose, obj.ToString(), null, null);
            }
            else
            {
                ProcessLog(Severity.Verbose, null, obj, null);
            }
        }

        public static void Verbose(Exception exception)
        {
            ProcessLog(Severity.Verbose, null, null, exception);
        }

        public static void Verbose(string content, Exception exception)
        {
            ProcessLog(Severity.Verbose, content, null, exception);
        }

        public static void Verbose(object obj, Exception exception)
        {
            if (obj == null) { Verbose(exception); }
            else if (obj.GetType() == "".GetType())
            {
                ProcessLog(Severity.Verbose, obj.ToString(), null, exception);
            }
            else
            {
                ProcessLog(Severity.Verbose, null, obj, exception);
            }
        }

        public static void Verbose(string content, object obj)
        {
            ProcessLog(Severity.Verbose, content, obj, null);
        }

        public static void Verbose(string content, object obj, Exception exception)
        {
            ProcessLog(Severity.Verbose, content, obj, exception);
        }
    }
}