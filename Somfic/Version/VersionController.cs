using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Somfic.Logging;

namespace Somfic.Version
{
    public abstract class VersionController
    {
        public VersionController()
        {
            Latest = GetVersion();
            HasNewer = GetHasNewer();
        }

        /// <summary>
        /// The latest available version.
        /// </summary>
        public System.Version Latest { get; private set; }

        /// <summary>
        /// Whether the latest available version is newer than this version.
        /// </summary>
        public bool HasNewer { get; private set; }

        public abstract System.Version GetVersion();

        private bool GetHasNewer()
        {
            System.Version latestVersion = GetVersion();
            System.Version thisVersion = Assembly.GetCallingAssembly().GetName().Version;
            return latestVersion > thisVersion;
        }
    }
}
