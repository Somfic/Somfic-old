using System;
using System.Collections.Generic;
using System.Text;

namespace Somfic.Logging
{
    public interface ILoggerHandler
    {
        void WriteLog(LogMessage message);
    }
}