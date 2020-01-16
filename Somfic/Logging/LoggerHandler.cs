using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Somfic.Logging
{
    public abstract class LoggerHandler
    {
        public LoggerHandler()
        {
            AllowedLevels = new Severity[0];
            AllowAllLevels();
        }

        public Severity[] AllowedLevels { get; internal set; }
        public bool IsDisabled { get; private set; }

        public void ProcessLog(LogMessage message)
        {
            if (!AllowedLevels.Contains(message.Severity)) { return; }
            if(!IsDisabled) { WriteLog(message);}
        }

        public abstract void WriteLog(LogMessage message);

        public void Enable()
        {
            IsDisabled = false;
        }

        public void Disable()
        {
            IsDisabled = true;
        }

        public void SetAllowedLevels(params Severity[] severity)
        {
            AllowedLevels = severity;
        }

        public void AllowAllLevels()
        {
            AllowedLevels = new []{Severity.Debug, Severity.Info, Severity.Warning, Severity.Error, Severity.Success};
        }
    }
}