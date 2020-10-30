using System;

namespace Somfic.Logging
{
    /// <summary>
    /// NoopDisposable helper class
    /// </summary>
    public class NoopDisposable : IDisposable
    {
        /// <summary>
        /// Disposes
        /// </summary>
        public void Dispose()
        {
        }
    }
}