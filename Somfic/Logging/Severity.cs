using System;

namespace Somfic.Logging
{
    /// <summary>
    /// The severity level of the message.
    /// </summary>
    [Flags]
    public enum Severity
    {
        Success,
        Info, 
        Warning, 
        Error, 
        Debug, 
        Special
    }
}