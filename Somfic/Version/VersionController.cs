using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Somfic.Logging;

namespace Somfic.Version
{
    public abstract class VersionController
    {
        /// <summary>
        /// The latest available version.
        /// </summary>
        public System.Version Latest => GetVersion();

        public System.Version This => Assembly.GetCallingAssembly().GetName().Version;

        /// <summary>
        /// Whether the latest available version is newer than this version.
        /// </summary>
        public bool NewerAvailable => Latest > This;

        public abstract System.Version GetVersion();
    }
}
